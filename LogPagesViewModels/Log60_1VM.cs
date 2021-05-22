using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;
using Common;
using DevExpress.Mvvm;
using Model;
using Model.DBStructure;
using ReportPages;

namespace LogPagesViewModels
{
    public class Log60_1VM : BindableBase
    {
        public ObservableCollection<Band> Bands { get; private set; }
        private IEnumerable<string> accounts;
        private DateTime date;
        private string account;
        private ObservableCollection<ExpandoObject> outputList;
        public ObservableCollection<ExpandoObject> OutputList
        {
            get => outputList;
            set => outputList = value;
        }

        #region RefreshCommand

        private ICommand refreshCommand;

        public ICommand RefreshCommand
        {
            get => refreshCommand;
            set => refreshCommand = value;
        }

        #endregion

        public Log60_1VM(DateTime date, string account)
        {
            logPrintCommand = new DelegateCommand(PrintLog);
            refreshCommand = new DelegateCommand(RefillGrid);
            this.date = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            this.account = account;
            IEnumerable<string> accounts = new List<string>();
            Bands = new ObservableCollection<Band>()
            {

                new Band()
                {
                    Header = "Наименование поставщика",
                    Fixed = "Left",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "Наименование поставщика",
                            FieldName = "Partner",
                            
                            
                        }
                    }
                },
                new Band()
                {
                    Header = "Сальдо начальное",
                    Fixed = "Left",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "по дебету",
                            FieldName = "StartDebitBalance"
                        },
                        new Column()
                        {
                            Header = "по кредиту",
                            FieldName = "StartCreditBalance"
                        }
                    }
                },


                new Band()
                {
                    Header = "Итого по кредиту",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "Итого по кредиту",
                            FieldName = "CreditSum"
                        }
                    }
                },
                new Band()
                {
                    Header = "Отметка об оплате",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "Дата оплаты",
                            FieldName = "Date"
                        },
                        new Column()
                        {
                            Header = "Счет 51",
                            FieldName = "account51"
                        },
                        new Column()
                        {
                            Header = "Счет 90.3/7",
                            FieldName = ""
                        }
                    }
                },
                new Band()
                {
                    Header = "Итого по дебету",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "Итого по дебету",
                            FieldName = "DebitSum"
                        }
                    }
                },
                new Band()
                {
                    Header = "Сальдо конечное ",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "по дебету",
                            FieldName = "EndDebitBalance"
                        },
                        new Column()
                        {
                            Header = "по кредиту",
                            FieldName = "EndCreditBalance"
                        }
                    }
                },
            };
            InsertAccountColumns();
            RaisePropertyChanged(nameof(Bands));
        }

        private void InsertAccountColumns()
        {
           
            if (account == "60/1")
            {
                accounts = Lists.Get60_1CreditCorrespondents();
            }

            var accountsBand = new Band();
            accountsBand.ChildColumns = new ObservableCollection<Column>();
            foreach (string account in accounts)
            {
                accountsBand.ChildColumns.Add(new Column()
                {
                    Header = account,
                    FieldName = $"account{account}"
                });

            }

            Bands.Insert(2, accountsBand);
            RefillGrid();
        }

        #region PrintLog

        private ICommand logPrintCommand;

        public ICommand LogPrintCommand
        {
            set => logPrintCommand = value;
            get => logPrintCommand;
        }

        private void PrintLog()
        {
            new CalculationExcelReport(date).CreateLog(accounts.ToList());
            // new PreviewWindow($"{Directory.GetCurrentDirectory()}\\Калькуляция {Helpers.Monthes[date.Month]}.xls").ShowDialog();
            var previewWin = new PreviewWindow();
            previewWin.LoadDocument($"{Directory.GetCurrentDirectory()}\\Журнал-ордер 60.1 {Helpers.Monthes[date.Month]}.xlsx");
            previewWin.Show();
        }


        #endregion
        private void RefillGrid()
        {
            var model = new PartnersBalancesDB();
            outputList = new ObservableCollection<ExpandoObject>();
            List<Partner> partners = new PartnerDB().GetList();
            List<PartnersBalances> balances = model.GetList(date.AddMonths(-1));
            date.AddMonths(1);
            int count = 7 + accounts.Count();
            for (int i = 0; i < partners.Count; i++)
            {
                var partner = partners[i];
                IDictionary<string, object> row = new ExpandoObject();
                PartnersBalances? currentBalance = balances.FirstOrDefault(x => x.PartnerId == partner.Id);
                row["Partner"] = partner.Name;
                if (currentBalance == null)
                {

                    currentBalance = new PartnersBalances() {Sum = 0};
                }
                
                   
                    row["StartDebitBalance"] = currentBalance.Sum > 0 ? currentBalance.Sum.ToString() : 0.ToString();
                    row["StartCreditBalance"] = currentBalance.Sum < 0 ? -1 * currentBalance.Sum : 0;

                    foreach (string account in accounts)
                    {
                        row[$"account{account}"] = model.GetCostForAccount(account, partner.Id, date);
                    }

                    float creditSum = model.GetCostSum(partner.Id, date);
                    row[$"account18"] = model.GetVatSum(partner.Id, date);
                    row["CreditSum"] = creditSum;
                    (float sum, string description) = model.GetPaymentsInfo(partner.Id, date);
                    row["Date"] = description;
                    row["account51"] = sum;
                    row["DebitSum"] = sum;

                    float endBalance = currentBalance.Sum + sum - creditSum;

                    row["EndDebitBalance"] = endBalance > 0 ? endBalance : 0;
                    row["EndCreditBalance"] = endBalance < 0 ? -1 * endBalance : 0;

                    outputList.Add((ExpandoObject) row);
            }
        }
    }
   


}
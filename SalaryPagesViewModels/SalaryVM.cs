using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Common;
using DevExpress.Mvvm;
using DevExpress.Xpf.Grid;
using Model;
using Model.DBStructure;
using ReportPages;

namespace SalaryPagesViewModels
{
    public class SalaryVM : BindableBase
    {
       private ObservableCollection<ExpandoObject> outputList;
       private ReportCard model;
        public ObservableCollection<ExpandoObject> OutputList
        {
            get => outputList;
            set => outputList = value;
        }
        private DateTime date;
        public ObservableCollection<Band> Bands { get; private set; }

        public SalaryVM(DateTime date)
        {
            model = new ReportCard();
            this.date = date;
            cellEditedCommand = new DelegateCommand<CellValueChangedEventArgs>(CellEdited);
    
            Bands = new ObservableCollection<Band>()
            {
                new Band()
                {
                    Header ="ФИО",
                    Fixed = "Left",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "ФИО",
                            FieldName = "Name"
                        }
                        
                    }
                },
                new Band()
                {
                    Header ="Должность",
                    Fixed = "Left",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "Должность",
                            FieldName = "Post"
                        }
                        
                    }
                },
                new Band()
                {
                    Header ="Ставка",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "Ставка",
                            FieldName = "Rate"
                        }
                        
                    }
                },
                new Band()
                {
                    Header ="Дивиденды",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "Дивиденды",
                            FieldName = "Dividents"
                        }
                        
                    }
                },
                new Band()
                {
                    Header ="Повременно",
                    Fixed = "Left",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "Повременно",
                            FieldName = "TimeBased"
                        }
                        
                    }
                },
                
                
                new Band()
                {
                    Header = $"Отпускные {Helpers.Monthes[date.Month].ToLower()}",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = $"Отпускные {Helpers.Monthes[date.Month].ToLower()}",
                            FieldName = "VacationThis"
                        }
                        
                    }
                },
                new Band()
                {
                    Header = $"Отпускные {Helpers.Monthes[(date.AddMonths(1).Month)].ToLower()}",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = $"Отпускные {Helpers.Monthes[(date.AddMonths(1).Month)].ToLower()}",
                            FieldName = "VacationNext"
                        }
                        
                    }
                },
                new Band()
                {
                    Header ="Премия",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "Премия",
                            FieldName = "Prize"
                        }
                        
                    }
                },
                new Band()
                {
                    Header ="Начислено",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "за месяц",
                            FieldName = "Month"
                        },
                        new Column()
                        {
                            Header = "с начала года",
                            FieldName = "Year"
                        }
                        
                    }
                },
                new Band()
                {
                    Header ="1 % от ФОТ",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "1 % от ФОТ",
                            FieldName = "1%"
                        }
                        
                    }
                },
                new Band()
                {
                    Header ="Льготы",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "Количество",
                            FieldName = "BenefitsCount"
                        },
                        new Column()
                        {
                            Header = "за месяц",
                            FieldName = "BenefitsSum"
                        },
                        new Column()
                        {
                            Header = "Повременно",
                            FieldName = "Bonus"
                        }
                        
                    }
                },
                
                new Band()
                {
                    Header ="Подоходный налог",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "Подоходный налог",
                            FieldName = "IncomeTax"
                        }
                        
                    }
                },
                new Band()
                {
                    Header ="Сумма налогов",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "Сумма налогов",
                            FieldName = "TaxSum"
                        }

                    }
                },
                new Band()
                {
                    Header ="К получению",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "К получению",
                            FieldName = "Sum"
                        }
                        
                    }
                },
                new Band()
                {
                    Header ="Выдано в промежутке",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "Выдано в промежутке",
                            FieldName = "Gated"
                        }
                        
                    }
                },
                new Band()
                {
                    Header ="Выдать",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "Выдать",
                            FieldName = "ToGrant"
                        }
                        
                    }
                }

            };

            
            RefillGrid();
            
        }

        #region Lists

        #region Partners

        private List<Partner> partners = new PartnerDB().GetList();

        public List<string> Partners => partners.Select(partner => partner.Name).ToList();

        #endregion

        #endregion

        #region WorkingDays

        private int workingDays = 22;

        public int WorkingDays
        {
            get => workingDays;
            set => workingDays = value;
        }

        #endregion


        

       
        #region Edit

        private ICommand cellEditedCommand;

        public ICommand CellEditedCommand
        {
            get => cellEditedCommand;
            set => cellEditedCommand = value;
        }

       
        private void CellEdited(CellValueChangedEventArgs cell)
        {
            var salaryModel = new SalaryDB();
            int cellRow = cell.RowHandle;
            int column = cell.Column.VisibleIndex;
            IDictionary<string, object> row = outputList[cellRow];
            switch (column)
            {
                case 12:
                    salaryModel.UpdateGated(int.Parse(row["Id"].ToString()),float.Parse(cell.Value.ToString()));
                    break;
                case 3:
                    salaryModel.UpdateDividents(int.Parse(row["Id"].ToString()),float.Parse(cell.Value.ToString()));
                    break;
                case 5:
                    salaryModel.UpdateVacation(int.Parse(row["Id"].ToString()),float.Parse(cell.Value.ToString()));
                    break;
            }

            RefillGrid();
        }

        #endregion

        #region FSZN

        private string fszn;

        public string FSZN
        {
            get => fszn;
            set
            {
                SetProperties.SetProperty(ref fszn, value);
                RaisePropertiesChanged();
            }
        }

        #endregion
        
        
        #region Insurance

        private string insurance;

        public string Insurance
        {
            get => insurance;
            set
            {
                SetProperties.SetProperty(ref insurance, value);
            }
        }

        #endregion
        
        #region TaxSum

        private string taxSum;

        public string TaxSum
        {
            get => taxSum;
            set
            {
                SetProperties.SetProperty(ref taxSum, value);
            }
        }

        #endregion
        #region Social

        private string social;

        public string Social
        {
            get => social;
            set
            {
                SetProperties.SetProperty(ref social, value);
            }
        }

        #endregion
        
        #region Pension

        private string pension;

        public string Pension
        {
            get => pension;
            set
            {
                SetProperties.SetProperty(ref pension, value);
                RaisePropertiesChanged();
            }
        }

        
        #endregion
        
        #region IncomeTax

        private string incomeTax;

        public string IncomeTax
        {
            get => incomeTax;
            set
            {
                SetProperties.SetProperty(ref incomeTax, value);
                RaisePropertiesChanged();
            }
        }

        
        #endregion


       


        private void RefillGrid()
        {
            outputList = new ObservableCollection<ExpandoObject>();
            List<AdaptedEmployee> employees = model.GetAdaptedEmployeesPosts(date);
            double taxBase = 0f;
            double sumIncomeTax = 0;
            for (var i = 0; i < employees.Count; i++)
            {

                IDictionary<string, object> row = new ExpandoObject();
                AdaptedEmployee? employee = employees[i];
                ResultMonth? month = model.GetResultMont(date, employee.Id) ?? new ResultMonth();
                SalaryDB salaryDb = new SalaryDB();
                Salary? salary = salaryDb.GetEmployeesSalary(month.Id) ?? salaryDb.Add(new Salary() {ResultMonthId = month.Id});
                List<string?> values = model.GetValues(month.Id);
                int countDays = values.Count(x => (x ?? string.Empty).Contains(':'));
                double dividents = Math.Round(salary.Dividents == null ? 0 : salary.Dividents, 2);
                double vacations = Math.Round(salary.Vacations == null ? 0 : new VacationCalculations(employee, date).GetVacationSum(), 2);
                double gated = Math.Round(salary.Gated == null ? 0 : salary.Gated, 2);
                double premium = Math.Round(salary.Premium == null ? 0 : salary.Premium, 2);
                double medicals = Math.Round(salary.Medicals == null ? 0 : salary.Medicals, 2);
                double benefits = Math.Round(salary.Benefits == null ? 0 : salary.Benefits, 2);
                double bonus = Math.Round(salary.Bonus == null ? 0 : salary.Bonus, 2);
                row["Id"] = salary.Id;
                row["Name"] = $"{employee.LastName} {employee.Name} {employee.MiddleName}";
                row["Post"] = employee.PostName;
                row["Rate"] = employee.Rate.ToString("f2");
                row["Dividents"] = dividents.ToString("f2");
                double timeBase = Math.Round(employee.Rate / workingDays * countDays, 2);
                row["TimeBased"] = timeBase;
                row["VacationThis"] = vacations.ToString("f2");
                row["VacationNext"] = 0;
                row["Prize"] = premium.ToString("f2");
                double monthSum = Math.Round(dividents+ timeBase + vacations + premium, 2);
                row["Month"] = monthSum;
                row["Year"] = salaryDb.GetYearSalary(employee.Id, date, employee.Rate, workingDays, countDays);
                double percent =  Math.Round((monthSum - medicals) * 0.01f, 2);
                row["1%"] = percent.ToString("f2");
                row["BenefitsCount"] = 0;
                row["BenefitsSum"] = benefits.ToString("f2");
                row["Bonus"] = bonus.ToString("f2");
                double incomeTax = Math.Round((monthSum - bonus - benefits) * 0.13, 2);
                row["IncomeTax"] = incomeTax.ToString("f2");
                row["TaxSum"] = (incomeTax + percent).ToString("f2");
                double sum = Math.Round(monthSum - percent - incomeTax, 2);
                row["Sum"] = sum.ToString("f2");
                row["Gated"] = gated.ToString("f2");
                row["ToGrant"] = (sum - gated).ToString("f2");
                taxBase += monthSum - dividents - medicals;
                sumIncomeTax += incomeTax;
                outputList.Add((ExpandoObject)row);
            }

            double pens = Math.Round(taxBase * 0.01, 2);
            double soc = Math.Round(taxBase * 0.34, 2);
            double fsznSum = Math.Round(pens + soc, 2);
            sumIncomeTax = Math.Round(sumIncomeTax, 2);
            double insuranceSum = Math.Round(taxBase * 0.005, 2);
            Pension = pens.ToString("f2");
            Social = soc.ToString("f2");
            FSZN = (fsznSum).ToString("f2");
            IncomeTax = sumIncomeTax.ToString("f2");
            Insurance = (insuranceSum).ToString("f2");
            TaxSum = (fsznSum + sumIncomeTax + insuranceSum).ToString("f2");
            


            RaisePropertiesChanged(nameof(outputList));
        }

    }
}
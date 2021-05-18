using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.Spreadsheet;
using System.Drawing.Printing;
using System.IO;
using Common;
using DevExpress.Xpf.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Model;
using Model.DBStructure;
using ReportPages;
using ReportPagesViewModels.Annotations;
using Column = ReportPages.Column;

namespace ReportPagesViewModels
{
    public class Report10_1VM: BindableBase, IPageActions
    {
        // Returns a list of employees so that they can be bound to the grid control. 
        private CalculationExcelReport report;
        private ObservableCollection<ExpandoObject> outputList;
        public ObservableCollection<ExpandoObject> OutputList
        {
            get => outputList;
            set => outputList = value;
        }
        private Reports model;
        private DateTime date;
        public ObservableCollection<Band> Bands { get; private set; }
       private List<AvailableMaterials> list;
       
        public Report10_1VM(DateTime date, string account)
        {
            model = new Reports();
            report = new CalculationExcelReport(date);
            this.date = date;
            list = model.GetAvailableMaterials(date, account);
            cellEditedCommand = new DelegateCommand<CellValueChangedEventArgs>(CellEdited);
            mouseDoubleClickCommand = new DelegateCommand<ColumnHeaderClickEventArgs>(HeaderClick);
            calculationPrintCommand = new DelegateCommand(CalculationPrint);
            actPrintCommand = new DelegateCommand(ActPrint);
            consumptionRatePrintCommand = new DelegateCommand(ConsumptionRatePrint);

            
            addNewCommand = new DelegateCommand(AddNew);
            Bands = new ObservableCollection<Band>()
            {
                new Band()
                    {
                        Header = "Наименование документа, его номер и дата",
                        ChildColumns = new ObservableCollection<Column>()
                        {
                            new Column()
                            {
                                Header = "Наименование документа, его номер и дата",
                                FieldName = "Document",
                            }
                        }
                    },
                new Band()
                    {
                        Header = "Наименование сырья и материалов",
                        ChildColumns = new ObservableCollection<Column>()
                        {
                            new Column()
                            {
                                Header = "Наименование сырья и материалов",
                                FieldName = "Name",
                            }
                        }
                    },
                    new Band()
                    {
                        Header = "Остаток материалов на 01.......",
                        Fixed = "Left",
                        ChildColumns = new ObservableCollection<Column>()
                        {
                            new Column()
                            {
                                Header = "ед. изм.",
                                FieldName = "Unity",
                            },
                            new Column()
                            {
                                Header = "цена",
                                FieldName = "Price",
                            },
                            new Column()
                            {
                                Header = "кол-во",
                                FieldName = "Count",
                            },
                            new Column()
                            {
                                Header = "сумма",
                                FieldName = "Cost",
                            }
                        }
                    },
                    new Band()
                    {
                        Header = "Поступление материалов",
                        ChildColumns = new ObservableCollection<Column>()
                        {
                            new Column()
                            {
                                Header = "кол-во",
                                FieldName = "ICount",
                               
                            },
                            new Column()
                            {
                                Header = "сумма",
                                FieldName = "ICost",
                               
                            }
                        }
                    },
                    new Band()
                    {
                        Header = "Передано в доработку",
                        ChildColumns = new ObservableCollection<Column>()
                        {
                            new Column()
                            {
                                Header = "кол-во",
                                FieldName = "",
                               
                            },
                            new Column()
                            {
                                Header = "сумма",
                                FieldName = "",
                               
                            }
                        }
                    },
                   

                    new Band()
                    {
                        Header = "Остаток матералов на 01 ......+1",
                        Fixed = "Right",
                        ChildColumns = new ObservableCollection<Column>()
                        {
                            new Column()
                            {
                                Header = "кол-во",
                                FieldName = "",
                               
                            },
                            new Column()
                            {
                                Header = "сумма",
                                FieldName = "",
                               
                            }
                        }
                    }
            };
            RefillGrid();
            List<Production> productions = model.GetProductions(date);
            for (var i = 0; i < productions.Count; i++)
            {
                var production = productions[i];
                Bands.Add(new Band()
                {
                    Header = production.Name,
                    ChildColumns = new ObservableCollection<Column>
                    {
                        new Column()
                        {
                            Header = "кол-во",
                            FieldName = $"Production{i}Count",
                        },
                        new Column()
                        {
                            Header = "сумма",
                            FieldName = $"Production{i}Cost",
                        }
                    }
                });
            }
        }

        #region Lists

        #region Partners

        private List<Partner> partners = new PartnerDB().GetList();

        public List<string> Partners => partners.Select(partner => partner.Name).ToList();

        #endregion

        #endregion

        #region Add

        #region AddName

        private string addName;

        public string AddName
        {
            get => addName;
            set => addName = value;
        }

        #endregion

        #region AddPartner

        private int addPartner;

        public int AddPartner
        {
            get => addPartner;
            set => addPartner = value;
        }

        #endregion

        #region AddDate

        private DateTime addDate = DateTime.Today;

        public DateTime AddDate
        {
            get => addDate;
            set => addDate = value;
        }

        #endregion

        #region HeaderClick

        private ICommand mouseDoubleClickCommand;

        public ICommand MouseDoubleClickCommand
        {
            get => mouseDoubleClickCommand;
            set => mouseDoubleClickCommand = value;
        }

        private void HeaderClick(ColumnHeaderClickEventArgs  eventArgs)
        {
            int index = (eventArgs.Column.VisibleIndex - 10) / 2;
            List<Production>? productions = new Reports().GetProductions(date);
            new CalculationSettingsVM().Show(productions[index]);
        }

        #endregion
        #region AddNew

        private ICommand addNewCommand;

        public ICommand AddNewCommand
        {
            get => addNewCommand;
            set => addNewCommand = value;
        }

        private void AddNew()
        {
            var production = new CalculationSettingsVM().OpenForCreate();
            if (production == null)
                return;
            // var production = new Production() { Name = addName, Date = addDate, PartnerId = partners[addPartner].Id, Cost = 0f};
            int i = model.GetProductions(date).IndexOf(production);
             Bands.Add(new Band()
             {
                 Header = production.Name,
                 ChildColumns = new ObservableCollection<Column>
                 {
                     new Column()
                     {
                         Header = "кол-во",
                         FieldName = $"Production{i}Count",
                     },
                     new Column()
                     {
                         Header = "сумма",
                         FieldName = $"Production{i}Cost",
                     }
                 }
             });
            model.AddProduction(production);
             RaisePropertyChanged(nameof(Bands));
             RefillGrid();
             
        }

        #endregion

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
            List<Production> productions = model.GetProductions(date);
            int cellRow = cell.RowHandle;
            int column = cell.Column.VisibleIndex;
            var productionId = (int) Math.Truncate((decimal) ((column - 10) / 2));
            float value = cell.Value.ToString() == string.Empty ? 0f : float.Parse(cell.Value.ToString());
            int materialsId = list[cellRow].Id;
            var expence = new Expence() { MaterialId = list[cellRow].Id, ProductionId = productions[productionId].Id, CountForInstall = 0};
            Expence current = model.GetExpence(materialsId, productions[productionId].Id);
            if (current is null)
            {
                if (column % 2 == 0)
                {
                    expence.Count = value;
                    expence.Cost = list[cellRow].Price * expence.Count;
                }
                else
                {
                    expence.Cost = value;
                    expence.Count = expence.Cost / list[cellRow].Price;
                }
                model.InsertExpence(expence);
            }
            else
            {
                if (column % 2 == 0)
                {
                    if (value == 0)
                        model.Delete(current.Id);
                    else
                    {
                        expence.Count = value;
                        expence.Cost = list[cellRow].Price * expence.Count;
                        model.UpdateExpence(current.Id, expence);
                    }
                }
                else
                {
                    expence.Cost = value;
                    expence.Count = current.Count;
                    model.UpdateExpence(current.Id, expence);
                }
                
            }
            RefillGrid();
        }

        #endregion


        #region CalculationPrint

        private ICommand calculationPrintCommand;

        [NotNull]
        public ICommand CalculationPrintCommand
        {
            get => calculationPrintCommand;
            set => calculationPrintCommand = value;
        }

        private void CalculationPrint()
        {
            report.CreateReport();
           // new PreviewWindow($"{Directory.GetCurrentDirectory()}\\Калькуляция {Helpers.Monthes[date.Month]}.xls").ShowDialog();
           var previewWin = new PreviewWindow();
           previewWin.LoadDocument($"{Directory.GetCurrentDirectory()}\\Калькуляция {Helpers.Monthes[date.Month]}.xlsx");
           previewWin.Show();
        }

        #endregion
        #region ActPrint

        private ICommand actPrintCommand;

        [NotNull]
        public ICommand ActPrintCommand
        {
            get => actPrintCommand;
            set => actPrintCommand = value;
        }

        private void ActPrint()
        {
            report.CreateAct();
            new PreviewWindow($"{Directory.GetCurrentDirectory()}\\Акт {Helpers.Monthes[date.Month]}.xlsx").ShowDialog();
        }

        #endregion

        #region ConsumptionRatePrint

        private ICommand consumptionRatePrintCommand;

        [NotNull]
        public ICommand ConsumptionRatePrintCommand
        {
            get => consumptionRatePrintCommand;
            set => consumptionRatePrintCommand = value;
        }

        private void ConsumptionRatePrint()
        {
            report.CreateConsumptionRate();
            new PreviewWindow($"{Directory.GetCurrentDirectory()}\\Нормы расхода {Helpers.Monthes[date.Month]}.xlsx").ShowDialog();
        }

        #endregion


        private void RefillGrid()
        {
            outputList = new ObservableCollection<ExpandoObject>();
            int count = 10 + model.GetProductions(date).Count * 2;
            List<Production> productions = model.GetProductions(date);
            for (var i = 0; i < list.Count; i++)
            {
                
                AvailableMaterials elem = list[i];
                IDictionary<string, object> row = new ExpandoObject();
                row["Document"] = elem.Type + " № " + elem.Number + " от " + Model.Common.ConvertDate(elem.Date);
                row["Name"] = elem.Name;
                row["Unity"] = elem.Unity;
                row["Price"] = elem.Price.ToString(CultureInfo.CurrentCulture);
                row["Count"] = elem.Count.ToString();
                row["Cost"] = elem.Cost.ToString();
                row["ICount"] = elem.ICount.ToString();
                row["ICost"] = elem.ICost.ToString();
                for (var j = 0; j < productions.Count; j++)
                {
                    row[$"Production{j}Count"] = string.Empty;
                    row[$"Production{j}Cost"] = string.Empty;

                }

                outputList.Add((ExpandoObject) row);
            }

            if (list.Count > 0)
            {
                for (var index = 0; index < productions.Count; index++)
                {
                    Production production = productions[index];
                    List<Expence> expences = model.GetExpences(production.Id);
                    for (var j = 0; j < expences.Count; j++)
                    {
                        Expence expence = expences[j];
                        int i = list.IndexOf(list.First(x => x.Id == expence.MaterialId));
                        IDictionary<string, object> row = outputList[i];
                        row[$"Production{index}Count"] = expence.Count.ToString(CultureInfo.CurrentCulture);
                        row[$"Production{index}Cost"] = expence.Cost.ToString(CultureInfo.CurrentCulture);
                    }
                }
            }

            RaisePropertiesChanged(nameof(outputList));
        }

        public void OpenSettings(string name)
        {
            throw new NotImplementedException();
        }
    }

}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Globalization;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.Xpf.Grid;
using Model;
using Model.DBStructure;
using ReportPages;

namespace SalaryPagesViewModels
{
    public class ReportCardVM : BindableBase
    {
        public ObservableCollection<Band> Bands { get; private set; }
        public ReportCardVM(DateTime date, IFillGrid fillGrid)
        {
            model = new ReportCard();
            employees = model.GetAdaptedEmployeesPosts(date);
            this.date = date;
            this.fillGrid = fillGrid;
            RefillGrid();
            Bands = new ObservableCollection<Band>()
            {
                new Band()
                {
                    Header = "Фамилия Имя Отчество",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "Фамилия Имя Отчество",
                            FieldName = "Name",
                        }
                    }
                },
                new Band()
                {
                    Header = "Должность",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "Должность",
                            FieldName = "Post",
                        }
                    }
                },
                new Band()
                {
                    Header = "Категория",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "Категория",
                            FieldName = ""
                        }
                    }
                },
                new Band()
                {
                    Header = "Ставка",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "Ставка",
                            FieldName = "Rate",
                        }
                    }
                },
                new Band()
                {
                    Header = "осн/совм",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column(){Header = "осн/совм"}
                    }
                },
                new Band()
                {
                    Header = "Числа месяца",
                    ChildColumns = new ObservableCollection<Column>()
                }
            };
            Band daysOfMonth = Bands[Bands.Count - 1];
            for (var i = 0; i < DateTime.DaysInMonth(date.Year, date.Month); i++)
            {
                daysOfMonth.ChildColumns.Add(new Column()
                {
                    Header = (i+1).ToString(),
                    FieldName = $"Day{i}"
                });
            }
           
        }

        #region Lists

        private List<AdaptedEmployee> employees;
        private ObservableCollection<ExpandoObject> outputList;

        private ObservableCollection<ExpandoObject> OutputList
        {
            get => outputList;
            set => outputList = value;
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
            int row = (int) cell.Row;
            

        }
        #endregion

        private void RefillGrid()
        {
            int columnCount = DateTime.DaysInMonth(date.Year, date.Month) + 5;
            var output = new string[employees.Count, columnCount];
            outputList = new ObservableCollection<ExpandoObject>();
            for (var i = 0; i < employees.Count; i++)
            {
                IDictionary<string, object> row = new ExpandoObject();
                row["Name"] = $"{employees[i].LastName} {employees[i].Name} {employees[i].MiddleName}";
                row["Post"] = employees[i].PostName;
                row["Rate"] = employees[i].Rate.ToString(CultureInfo.CurrentCulture);
                output[i, 0] = employees[i].LastName + " " + employees[i].Name + " " +
                               employees[i].MiddleName;
                output[i, 1] = employees[i].PostName;
                output[i, 3] = employees[i].Rate.ToString(CultureInfo.CurrentCulture);
                for (var j = 0; j < DateTime.DaysInMonth(date.Year, date.Month); j++) 
                    row[$"Day{j}"] = string.Empty;
                outputList.Add((ExpandoObject)row);
            }

         //   List<AdaptedWorkOut> outs = model.GetAdaptedWorkOuts(date);
            /*for (var index = 0; index < outs.Count; index++)
            {
                AdaptedWorkOut workOut = outs[index];
                int i = employees.IndexOf(employees.First(x => x.Id == workOut.PostEmployeeId));
                int column = workOut.Date.Day;
                var row = (IDictionary<string, object>) outputList[i];
                row[$"Day{column}"] = workOut.Type ?? workOut.Hours.ToString();
                output[i, column + 5] = workOut.Type ?? workOut.Hours.ToString();
            }*/

            fillGrid.FillGrid(output);
        }
        private DateTime date;
        private IFillGrid fillGrid;
        private ReportCard model;
    }
}
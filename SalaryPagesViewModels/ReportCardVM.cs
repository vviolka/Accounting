using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
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
        private IDictionary<string, string> transcript = new Dictionary<string, string>()
        {
            {"Trips", "К"},
            {"WorkLeave", "О"},
            {"MaternityLeave", "Р"},
            {"NoSLeave", "А"},
            {"StateDuties", "Г"},
            {"Donor", "Д"},
            {"ChildLeave", "ОЖ"},
            {"Absenteeism", "П"},
            {"Disability", "Б"},
            {"StudyLeave", "У"},
            {"Suspension", "ОР"},
            {"Weekends", "В"}

        };
       // Returns a list of employees so that they can be bound to the grid control. 
       private ObservableCollection<ExpandoObject> outputList;
       private ObservableCollection<ExpandoObject> resultOutputList;
       private ReportCard model;
        public ObservableCollection<ExpandoObject> OutputList
        {
            get => outputList;
            set => outputList = value;
        }

        public ObservableCollection<ExpandoObject> ResultOutputList
        {
            get => resultOutputList;
            set => resultOutputList = value;
        }
        private DateTime date;
        public ObservableCollection<Band> Bands { get; private set; }
        public ObservableCollection<Band> ResultBands { get; private set; }

        public ReportCardVM(DateTime date)
        {
            model = new ReportCard();
            this.date = date;
            cellEditedCommand = new DelegateCommand<CellValueChangedEventArgs>(CellEdited);
            resultCellEditedCommand = new DelegateCommand<CellValueChangedEventArgs>(ResultCellEdited);
            ResultBands = new ObservableCollection<Band>()
           {
               new Band()
               {
                   Header = "Фамилия И.О.",
                   Fixed = "Left",
                   ChildColumns = new ObservableCollection<Column>()
                   {
                       new Column()
                       {
                           Header = "Фамилия И.О.",
                           FieldName = "Name",
                       }
                   }
               },

               new Band()
               {
                   Header = "Должность",
                   Fixed = "Left",
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
                   Header = "Ставка",
                   Fixed = "Left",
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
                   Header = "Дни явок",
                   ChildColumns = new ObservableCollection<Column>()
                   {
                       new Column()
                       {
                           Header = "Служ. команд., дней '\n' К",
                           FieldName = "Trips"
                       },
                       new Column()
                       {
                           Header = "всего",
                           FieldName = "SumDays"
                       }
                   }
               },
               new Band
               {
                   Header = "Дни неявок",
                   ChildColumns = new ObservableCollection<Column>()
                   {
                       new Column()
                       {
                           Header = "Трудовой отпуск, дней О",
                           FieldName = "WorkLeave"
                       },
                       new Column()
                       {
                           Header = "Отпуск по беременности и родам, дней Р",
                           FieldName = "MaternityLeave"
                       },
                       new Column()
                       {
                           Header = "Отпуск без сохр. з/пл, дней А",
                           FieldName = "NoSLeave"
                       },
                       new Column()
                       {
                           Header = "Вып. гос. обязанностей, дней Г",
                           FieldName = "StateDuties"
                       },
                       new Column()
                       {
                           Header = "День донора, Д",
                           FieldName = "Donor"
                       },
                       new Column()
                       {
                           Header = "Прогул, дней П",
                           FieldName = "Absenteeism"
                       },
                       new Column()
                       {
                           Header = "Соц отп. по уходу за реб., дней ОЖ",
                           FieldName = "ChildLeave"
                       },
                       new Column()
                       {
                           Header = "Нетрудоспособность, дней Б",
                           FieldName = "Disability"
                       },
                       new Column()
                       {
                           Header = "Учебный отпуск, дней У",
                           FieldName = "StudyLeave"
                       },
                       new Column()
                       {
                           Header = "Отстранение от работы ОР",
                           FieldName = "Suspension"
                       },
                       new Column()
                       {
                           Header = "Выходные и праздничные дни В",
                           FieldName = "Weekends"
                       }
                   },

               },
               new Band()
               {
                   Header = "Всего отработанно (в часах)",
                   ChildColumns = new ObservableCollection<Column>()
                   {
                       new Column()
                       {
                           Header = "Всего отработано (в часах)",
                           FieldName = "SumHours"
                       }
                   }
               },
               new Band()
               {
                   Header = "Из них (в часах)",
                   ChildColumns = new ObservableCollection<Column>()
                   {
                       new Column()
                       {
                           Header = "В праздничные дни",
                           FieldName = "HolidaysCount"
                       },
                       new Column()
                       {
                           Header = "Сверхурочно",
                           FieldName = "OvertimeCount"
                       },
                       new Column()
                       {
                           Header = "В ночное время",
                           FieldName = "NightCount"
                       }
                   }
               }
           };
            Bands = new ObservableCollection<Band>()
            {
                new Band()
                    {
                        Header = "Фамилия И.О.",
                        Fixed="Left",
                        ChildColumns = new ObservableCollection<Column>()
                        {
                            new Column()
                            {
                                Header = "Фамилия И.О.",
                                FieldName = "Name",
                            }
                        }
                    },

                new Band()
                {
                    Header = "Должность",
                    Fixed="Left",
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
                    Header = "Ставка",
                    Fixed="Left",
                    ChildColumns = new ObservableCollection<Column>()
                    {
                        new Column()
                        {
                            Header = "Ставка",
                            FieldName = "Rate",
                        }
                    }
                },

            };

            var band = new Band() {Header = "Числа месяца", ChildColumns = new ObservableCollection<Column>()};
            for (int i = 0; i < DateTime.DaysInMonth(date.Year, date.Month); i++)
            {
                band.ChildColumns.Add(new Column()
                {
                    Header = $"{i + 1}",
                    FieldName = $"Day{i + 1}"
                });
            }
            Bands.Add(band);
            RefillGrid();
            
        }

        #region Lists

        #region Partners

        private List<Partner> partners = new PartnerDB().GetList();

        public List<string> Partners => partners.Select(partner => partner.Name).ToList();

        #endregion

        #endregion


        

       
        #region Edit

        private ICommand cellEditedCommand;

        public ICommand CellEditedCommand
        {
            get => cellEditedCommand;
            set => cellEditedCommand = value;
        }

        private ICommand resultCellEditedCommand;

        public ICommand ResultCellEditedCommand
        {
            get => resultCellEditedCommand;
            set => resultCellEditedCommand = value;
        }

        private void ResultCellEdited(CellValueChangedEventArgs cell)
        {
            int cellRow = cell.RowHandle;
            int column = cell.Column.VisibleIndex;
            IDictionary<string, object> row = resultOutputList[cellRow];
            int employeeId = Convert.ToInt32(row["Id"]);
            
            if (column == row.Count - 2) model.UpdateNight(employeeId, date, Convert.ToInt32(cell.Value));
            else if (column == row.Count - 3) model.UpdateOvertime(employeeId, date, Convert.ToInt32(cell.Value));
            else if (column == row.Count - 4) model.UpdateHolidays(employeeId, date, Convert.ToInt32(cell.Value));
            RefillGrid();



        }

        private void CellEdited(CellValueChangedEventArgs cell)
        {
            int cellRow = cell.RowHandle;
            int column = cell.Column.VisibleIndex;
            IDictionary<string, object> row = outputList[cellRow];
            int employeeId = Convert.ToInt32(row["Id"]);
            int day = column - 2;
            if (cell.Value == null)
                return;
            if (cell.OldValue == string.Empty)
            {
                if (cell.Value != string.Empty)
                    model.InsertDay(employeeId, date, day.ToString(), cell.Value.ToString());
            }
            else if (cell.Value == string.Empty)
                model.DeleteDay(employeeId, date, day.ToString(), cell.Value.ToString());
            else
                model.UpdateDay(employeeId, date, day.ToString(), cell.Value.ToString());

            RefillGrid();
        }

        #endregion


       


        private void RefillGrid()
        {
            outputList = new ObservableCollection<ExpandoObject>();
            resultOutputList = new ObservableCollection<ExpandoObject>();
            int count = 4 + DateTime.DaysInMonth(date.Year, date.Month);
            List<AdaptedEmployee> employees = model.GetAdaptedEmployeesPosts(date);
            for (var i = 0; i < employees.Count; i++)
            {

                IDictionary<string, object> row = new ExpandoObject();
                IDictionary<string, object> resultRow = new ExpandoObject();
                AdaptedEmployee? employee = employees[i];
                row["Id"] = employee.Id;
                row["Name"] = $"{employee.LastName} {employee.Name} {employee.MiddleName}";
                row["Post"] = employee.PostName;
                row["Rate"] = employee.Rate;
                resultRow["Id"] = employee.Id;
                resultRow["Name"] = $"{employee.LastName} {employee.Name} {employee.MiddleName}";
                resultRow["Post"] = employee.PostName;
                resultRow["Rate"] = employee.Rate;
                
                ResultMonth? month = model.GetResultMont(date, employee.Id) ?? new ResultMonth();
                List<WorkedDay>? days = model.GetWorkedDay(month);
                for (int j = 0; j < DateTime.DaysInMonth(date.Year, date.Month); j++)
                    if (days.FindIndex(x => x.Day == (j + 1).ToString()) >= 0)
                        row[$"Day{j + 1}"] = days.First(x => x.Day == (j + 1).ToString()).Value;
                    else row[$"Day{j + 1}"] = "";
                foreach (var day in days) row[$"Day{day.Day}"] = day.Value;
                foreach (var transcr in transcript)
                    resultRow[transcr.Key] = model.GetDaysCount(month.Id, transcr.Value);

                List<string?> values = model.GetValues(month.Id);
                TimeSpan hours = new TimeSpan(0, 0, 0);
                int countDays = 0;
                foreach (string? value in values)
                {
                    if (value.Contains(':'))
                    {
                        var tmp = value.Split(':');
                        hours += new TimeSpan(Convert.ToInt32(tmp[0]), Convert.ToInt32(tmp[1]), 0);
                        countDays++;
                    }
                }

                resultRow["SumDays"] = countDays;
                resultRow["SumHours"] = hours.Hours;
                resultRow["HolidaysCount"] = month.HolidaysCount;
                resultRow["OvertimeCount"] = month.OvertimeCount;
                resultRow["NightCount"] = month.NightCount;
                outputList.Add((ExpandoObject)row);
                resultOutputList.Add((ExpandoObject)resultRow);
            }


            RaisePropertiesChanged(nameof(outputList));
            RaisePropertiesChanged(nameof(resultOutputList));
        }

    }
}




   
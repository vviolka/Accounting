using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Model.DBStructure;

namespace Model
{
    public class VacationCalculations
    {
        private readonly AdaptedEmployee employee;
        private DateTime date;
      
        public VacationCalculations(AdaptedEmployee inEmployee, DateTime inDate)
        {
            employee = inEmployee;
            date = inDate;
        }
        
        public double GetVacationSum()
        {
            using var ac = new ApplicationContext();
            const int workingDays = 22;
            double yearSalary = 0;
            int workedDays = 0;
            if (ac.Vacations.Any(x => x.EmployeeId == employee.EmployeeId && date.Month == x.StartDate.Month))
            {
                List<ResultMonth>? months = new SalaryDB().GetSalariesForYear(date, employee.Id);
                foreach (ResultMonth month in months)
                {


                    Salary? salary = ac.Salaries.First(x => x.ResultMonthId == month.Id);
                    ReportCard model = new ReportCard();
                    List<string?> values = model.GetValues(month.Id);
                    int countDays = values.Count(x => (x ?? string.Empty).Contains(':'));
                    workedDays += countDays;
                    yearSalary += salary.Benefits + salary.Bonus + salary.Premium +
                                  Math.Round(employee.Rate / workingDays * countDays, 2);
                }

                int daysCount = 0;

                double averageSalary = yearSalary / workedDays;
                if (ac.Vacations.Any(x => x.EmployeeId == x.EmployeeId && x.StartDate.Month == date.Month))
                {
                    daysCount = ac.Vacations
                        .FirstOrDefault(x => x.EmployeeId == x.EmployeeId && x.StartDate.Month == date.Month).DaysCount;
                }
                return averageSalary * daysCount;
            }
            else
            {
                return 0;
            }



        }
        
    }
}
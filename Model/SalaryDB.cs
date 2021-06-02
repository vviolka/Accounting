using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Data.ODataLinq.Helpers;
using Model.DBStructure;

namespace Model
{
    public class SalaryDB
    {
        private ApplicationContext ac;
        public SalaryDB()
        {
            ac = new ApplicationContext();
        }

        public Salary? GetEmployeesSalary(int resultMontId)
        {
            return ac.Salaries.FirstOrDefault(x => x.ResultMonthId == resultMontId);
        }

        public Salary Add(Salary salary)
        {
            ac.Add(salary);
            ac.SaveChanges();
            return salary;
        }
        public double GetYearSalary(int EmployeeId, DateTime date, float rate, int workingDays, int countDays)
        {
            IQueryable<ResultMonth>? monthes = ac.ResultMonthes.Where(x => x.EmployeeId == EmployeeId &&
                                                                           x.Date.Year == date.Year &&
                                                                           x.Date.Month >= 1 && x.Date.Month < date.Month);
           double yearSalary = ac.Salaries.ToList().Where( y => monthes.Select(x => x.Id).Contains(y.ResultMonthId)).Sum(
               
               y => y.Dividents + y.Vacations + y.Medicals + y.Bonus +  Math.Round(rate / workingDays * countDays, 2));
           return yearSalary;
        }

        public void UpdateDividents(int salaryId, float value)
        {
            Salary? salary = ac.Salaries.First(x => x.Id == salaryId);
            salary.Dividents = value;
            ac.SaveChanges();
        }
        public void UpdateVacation(int salaryId, float value)
        {
            Salary? salary = ac.Salaries.First(x => x.Id == salaryId);
            salary.Vacations = value;
            ac.SaveChanges();
        }
        public void UpdateGated(int salaryId, float value)
        {
            Salary? salary = ac.Salaries.First(x => x.Id == salaryId);
            salary.Gated = value;
            ac.SaveChanges();
        }

        public List<ResultMonth> GetSalariesForYear(DateTime date, int employeeId)
        {
            DateTime startDate;
            DateTime endDate;
            ResultMonth? currMonth = ac.ResultMonthes.FirstOrDefault(x => x.Date.Month == date.Month && x.EmployeeId == employeeId);
            endDate = currMonth == null ? date.AddMonths(-1) : date;
            startDate = endDate.AddMonths(-12);
            List<ResultMonth> resultMonthes = ac.ResultMonthes.Where(x => x.EmployeeId == employeeId &&
                                                                           x.Date >= startDate && x.Date <= endDate).ToList();
            return resultMonthes;
        }
    }
}
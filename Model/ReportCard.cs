using System;
using System.Collections.Generic;
using System.Linq;
using Model.DBStructure;

namespace Model
{
    public class ReportCard
    {
        public ReportCard()
        {
            
        }

        public List<AdaptedEmployee> GetAdaptedEmployeesPosts(DateTime date)
        {
            using var ac = new ApplicationContext();
            //var employees = ac.Employees.Where(x => (x.FiredDate == null || (((DateTime)x.FiredDate).Month > date.Month && ((DateTime)x.FiredDate).Year > date.Year))&&(x.AcceptableDate.Month <= date.Month && x.AcceptableDate.Year <= date.Year)).ToList();
            var employees = ac.Employees.ToList(); //todo 
            var posts = ac.Posts.ToList();
            var postsEmployees = ac.EmployeesPosts.ToList();
            var result = (from employee in employees
                join postEmployee in postsEmployees on employee.Id equals postEmployee.EmployeeId
                join post in posts on postEmployee.PostId equals post.Id
                select new AdaptedEmployee
                {
                    Id = postEmployee.Id,
                    EmployeeId = employee.Id,
                    Name = employee.Name,
                    MiddleName = employee.MiddleName,
                    LastName = employee.LastName,
                    PostName = post.Name,
                    Rate = post.Rate * postEmployee.Bid
                }).ToList();
            return result;
        }

        public ResultMonth? GetResultMont(DateTime date, int employeeId)
        {
            using var ac = new ApplicationContext();
            return ac.ResultMonthes.FirstOrDefault(x =>
                x.EmployeeId == employeeId && x.Date.Month == date.Month && x.Date.Year == date.Year);
        }

        public void UpdateDay(int employeeId, DateTime date, string day, string value)
        {
            using var ac = new ApplicationContext();
            ResultMonth month = ac.ResultMonthes.First(x =>
                x.EmployeeId == employeeId && x.Date.Month == date.Month && date.Year == x.Date.Year);
            WorkedDay workedDay = ac.WorkedDays.First(x => x.Day == day && x.MonthId == month.Id);
            ac.WorkedDays.Remove(workedDay);
            ac.SaveChanges();

        }

        public void DeleteDay(int employeeId, DateTime date, string day, string value)
        {
            using var ac = new ApplicationContext();
            ResultMonth month = ac.ResultMonthes.First(x =>
                x.EmployeeId == employeeId && x.Date.Month == date.Month && date.Year == x.Date.Year);
            WorkedDay workedDay = ac.WorkedDays.First(x => x.Day == day && x.MonthId == month.Id);
            workedDay.Value = value;
            ac.SaveChanges();

        }

        public void InsertDay(int employeeId, DateTime date, string day, string value)
        {
            using var ac = new ApplicationContext();
            bool isAny = ac.ResultMonthes.Any(x => x.EmployeeId == employeeId && x.Date.Month == date.Month && date.Year == x.Date.Year);
            if (!isAny)
            {
                ac.ResultMonthes.Add(new ResultMonth()
                {
                    Date = date,
                    EmployeeId = employeeId,
                    HolidaysCount = 0,
                    NightCount = 0,
                    OvertimeCount = 0
                });
                ac.SaveChanges();
            }

            ResultMonth month = ac.ResultMonthes.First(x =>
                x.EmployeeId == employeeId && x.Date.Month == date.Month && date.Year == x.Date.Year);

            ac.WorkedDays.Add(new WorkedDay()
            {
                Day = day,
                MonthId = month.Id,
                Value = value
            });
            ac.SaveChanges();
        }

        public void UpdateNight(int employeeId, DateTime date, int value)
        {
            using var ac = new ApplicationContext();
            ResultMonth month = ac.ResultMonthes.First(x =>
                x.EmployeeId == employeeId && x.Date.Month == date.Month && date.Year == x.Date.Year);
            month.NightCount = value;
            ac.SaveChanges();

        }

        public void UpdateOvertime(int employeeId, DateTime date, int value)
        {
            using var ac = new ApplicationContext();
            ResultMonth month = ac.ResultMonthes.First(x =>
                x.EmployeeId == employeeId && x.Date.Month == date.Month && date.Year == x.Date.Year);
            month.OvertimeCount = value;
            ac.SaveChanges();

        }

        public void UpdateHolidays(int employeeId, DateTime date, int value)
        {
            using var ac = new ApplicationContext();
            ResultMonth month = ac.ResultMonthes.First(x =>
                x.EmployeeId == employeeId && x.Date.Month == date.Month && date.Year == x.Date.Year);
            month.HolidaysCount = value;
            ac.SaveChanges();

        }

        public List<string?> GetValues(int monthId)
        {
            using var ac = new ApplicationContext();
            return ac.WorkedDays.Where(x => x.MonthId == monthId).Select(x => x.Value).ToList();
        }

        public int GetDaysCount(int monthId, string type)
        {
            using var ac = new ApplicationContext();
            return ac.WorkedDays.Count(x => x.MonthId == monthId && x.Value == type);
        }

        public List<WorkedDay>? GetWorkedDay(ResultMonth month)
        {
            using var ac = new ApplicationContext();
            return ac.WorkedDays.Where(x => x.MonthId == month.Id).ToList();
        }
        /*public List<AdaptedWorkOut> GetAdaptedWorkOuts(DateTime date)
        {
            using var ac = new ApplicationContext();
            var workouts =
                (from workout in ac.WorkOuts.Where(x => date.Month == x.Date.Month && x.Date.Year == date.Year)
                 
                    select new AdaptedWorkOut()
                    {
                        PostEmployeeId = workout.UserId,
                        Date = workout.Date,
                        Hours = workout.Hours,
                        Type = workout.Type,
                        ResultMonthId = workout.ResultMonthId
                    }).ToList();
            return workouts;
        }
        */

        /*public void AddWorkOut(WorkOut workOut)
        {
            /*using var ac = new ApplicationContext();
            ac.WorkOuts.Add(workOut);
            ac.SaveChanges();#1#
        }*/
    }
}
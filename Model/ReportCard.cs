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
            var employees = ac.Employees.Where(x => (x.FiredDate == null || (((DateTime)x.FiredDate).Month > date.Month && ((DateTime)x.FiredDate).Year > date.Year))&&(x.AcceptableDate.Month <= date.Month && x.AcceptableDate.Year <= date.Year)).ToList();
            var posts = ac.Posts.ToList();
            var postsEmployees = ac.EmployeesPosts.ToList();
            var result = (from employee in employees
                join postEmployee in postsEmployees on employee.Id equals postEmployee.EmployeeId
                join post in posts on postEmployee.PostId equals post.Id
                select new AdaptedEmployee
                {
                    Id = postEmployee.Id,
                    Name = employee.Name,
                    MiddleName = employee.MiddleName,
                    LastName = employee.LastName,
                    PostName = post.Name,
                    Rate = post.Rate * postEmployee.Bid
                }).ToList();
            return result;
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

        public void AddWorkOut(WorkOut workOut)
        {
            /*using var ac = new ApplicationContext();
            ac.WorkOuts.Add(workOut);
            ac.SaveChanges();*/
        }
    }
}
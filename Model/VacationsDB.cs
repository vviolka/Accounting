using System.Collections.Generic;
using System.Linq;
using Model.DBStructure;

namespace Model
{
    public class VacationsDB
    {
        private ApplicationContext ac;
        public VacationsDB()
        {
            ac = new ApplicationContext();
        }

        public void Add(Vacation vacation)
        {
            ac.Vacations.Add(vacation);
            ac.SaveChanges();
        }

        public void Update(int vacationId, Vacation newVacation)
        {
            Vacation? current = ac.Vacations.First(x => x.Id == vacationId);
            current.DaysCount = newVacation.DaysCount;
            current.EmployeeId = newVacation.EmployeeId;
            current.StartDate = newVacation.StartDate;
            current.EndDate = newVacation.EndDate;
            ac.SaveChanges();
        }

        public void Delete(int vacationId)
        {
            Vacation? current = ac.Vacations.First(x => x.Id == vacationId);
            ac.Vacations.Remove(current);
            ac.SaveChanges();
        }

        
        public List<AdaptedVacation> GetList()
        {
            return (from vacation in ac.Vacations
                join employee in ac.Employees on vacation.EmployeeId equals employee.Id
                select new AdaptedVacation()
                {
                    Employee = $"{employee.LastName} {employee.Name} {employee.MiddleName}",
                    DaysCount = vacation.DaysCount,
                    EndDate = vacation.EndDate,
                    StartDate = vacation.StartDate,
                    Id = vacation.Id
                }).ToList();

        }
    }
}
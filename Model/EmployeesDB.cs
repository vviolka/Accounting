using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model.DBStructure;

namespace Model
{
    public class EmployeesDB
    {
        public EmployeesDB()
        {
            
        }
        public void Add(Employee employee)
        {
            using var dc = new ApplicationContext();
            dc.Employees.Add(employee);
            dc.SaveChanges();
        }

        public List<Employee> GetActualList()
        {
            using var dc = new ApplicationContext();
            var employees = dc.Employees.Where(employee => employee.FiredDate == null || employee.FiredDate > DateTime.Today).ToList();

            return employees;
        }

        public void Edit(int id, Employee newEmployee)
        {
            using var dataContext = new ApplicationContext();
            var employee = dataContext.Employees.First(x => x.Id == id);
            employee.Name = newEmployee.Name;
            employee.LastName = newEmployee.LastName;
            employee.MiddleName = newEmployee.MiddleName;
            employee.DateBirth = newEmployee.DateBirth;
            employee.AcceptableDate = newEmployee.AcceptableDate;
            employee.Education = newEmployee.Education;
     
            dataContext.SaveChanges();
        }

        public void Delete(Employee employees)
        {
            using var dataContext = new ApplicationContext();
            var newEmployees = dataContext.Employees.First(x => x.Id == employees.Id);
            dataContext.Employees.Remove(newEmployees);
            dataContext.SaveChanges();
        }
        
        public List<Employee> Search(string name, string lastName, string middleName, DateTime dateBirth, DateTime acceptableDate)
        {
            List<Employee> resultList;
            using var dc = new ApplicationContext();
            {
                resultList = (from employee in dc.Employees
                              where
          name == "" || EF.Functions.Like(employee.Name, '%' + name + '%') &&
          (lastName == "" || EF.Functions.Like(employee.LastName, '%' + lastName + '%')) &&
          (middleName == "" || EF.Functions.Like(employee.MiddleName, '%' + middleName + '%')) &&
          (dateBirth == null || employee.DateBirth == dateBirth) &&
          (acceptableDate == null || employee.AcceptableDate == acceptableDate)
                              select employee).ToList();
            }

            return resultList;
      
        }
    }
}

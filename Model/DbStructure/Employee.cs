using System;
using System.Data.Linq.Mapping;

namespace Model.DbStructure
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateBirth { get; set; }
        public DateTime AcceptableDate { get; set; }
        public DateTime? FiredDate { get; set; }
        public string Education { get; set; }
    }
}
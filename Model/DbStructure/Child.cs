using System;

namespace Model.DbStructure
{
    public class Child
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public  int EmployeeId { get; set; }
        public DateTime DateBirth { get; set; }
    }
}
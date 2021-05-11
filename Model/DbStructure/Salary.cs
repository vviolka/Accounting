using System;

namespace Model.DbStructure
{
    public class Salary
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public float Sum { get; set; }
        public DateTime Date { get; set; }
        public float Taxes { get; set; }
        public float WithoutTaxes { get; set; }
    }
}
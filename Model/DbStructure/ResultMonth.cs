using System;

namespace Model.DBStructure
{
    public class ResultMonth
    {
        public int Id { get; set; }
        public DateTime Date  { get; set; }
        public int EmployeeId { get; set; }
        public int HolidaysCount { get; set; }
        public int OvertimeCount { get; set; }
        public int NightCount { get; set; }
    }
}
using System;

namespace Model.DBStructure
{
    public class AdaptedVacation
    {
        public int Id { get; set; }
        public string Employee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DaysCount { get; set; }
    }
}
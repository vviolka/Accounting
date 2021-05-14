using System;

namespace Model.DBStructure
{
    public class WorkOut
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public int? Hours { get; set; }
        public string? Type { get; set; }
        public int ResultMonthId { get; set; }
    }
}

using System;

namespace Model.DBStructure
{
    public class AdaptedWorkOut
    {
        public int PostEmployeeId { get; set; }
                public DateTime Date { get; set; }
                public int? Hours { get; set; }
                public string? Type { get; set; }
                public int ResultMonthId { get; set; }


    }
}

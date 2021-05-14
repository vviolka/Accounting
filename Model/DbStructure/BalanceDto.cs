using System;

namespace Model.DBStructure
{
    public class BalanceDto
    {
        public int Id { get; set; }
        public int MaterialsId { get; set; }
        public float Count { get; set; }
        public float Cost { get; set; }
        public DateTime Date { get; set; }
    }
}
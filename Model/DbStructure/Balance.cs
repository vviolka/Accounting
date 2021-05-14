using System;

namespace Model.DBStructure
{
    public class Balance
    {
        public int Id { get; set; }
        public int MaterialId  { get; set; }
        public float Count { get; set; }
        public float Cost { get; set; }
        public DateTime Date { get; set; }
    }
}
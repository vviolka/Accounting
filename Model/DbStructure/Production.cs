using System;

namespace Model.DbStructure
{
    public class Production
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Cost { get; set; }
        public DateTime Date { get; set; }
        public int PartnerId { get; set; }
    }
}
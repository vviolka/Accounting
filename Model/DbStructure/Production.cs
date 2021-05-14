using System;

namespace Model.DBStructure
{
    public class Production
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PartnerId { get; set; }
        public float Cost { get; set; }
        public DateTime Date { get; set; }
    }
}
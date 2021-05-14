using System;

namespace Model.DBStructure
{
    public class AvailableMaterials
    {
        public AvailableMaterials()
        {
            
        }

        public int Id { get; set; }
        public float? Count { get; set; }
        public float? Cost { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Unity { get; set; }
        public float? ICount { get; set; }
        public float? ICost { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Number { get; set; }
    }
}


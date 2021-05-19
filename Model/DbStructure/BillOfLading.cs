using System;

namespace Model.DBStructure
{
    public class BillOfLading
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Number { get; set; }
        public int PartnerId { get; set; }
        public string Type { get; set; }
        public float Cost { get; set; }
        public float Vat { get; set; }
        public float CostWithVat { get; set; }
        public string Purpose { get; set; }
    }
}

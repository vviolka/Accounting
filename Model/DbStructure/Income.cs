namespace Model.DBStructure
{
    public class Income
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public float Count { get; set; }
        public float Cost { get; set; }
        public string VatRate { get; set; }
        public float Vat { get; set; }
        public float CostWithVat { get; set; }
        public float? Weight { get; set; }
        public int BillId { get; set; }
        

    }
}
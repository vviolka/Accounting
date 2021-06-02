namespace DocumentsPagesViewModels
{
    public class MaterialsJoined
    {
        public string Name { get; set; }
        public string Unity { get; set; }
        public double Count { get; set; }
        public double Cost { get; set; } 
        public double Price { get; set; }
        public string VatRate { get; set; }
        public double Vat { get; set; }
        public double CostWithVat { get; set; }
        public double? Weight  { get; set; }
        public string Account { get; set; }
    }
}
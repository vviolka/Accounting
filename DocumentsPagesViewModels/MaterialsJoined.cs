namespace DocumentsPagesViewModels
{
    public class MaterialsJoined
    {
        public string Name { get; set; }
        public string Unity { get; set; }
        public float Count { get; set; }
        public float Cost { get; set; } 
        public float Price { get; set; }
        public string VatRate { get; set; }
        public float Vat { get; set; }
        public float CostWithVat { get; set; }
        public float? Weight  { get; set; }
        public string Account { get; set; }
    }
}
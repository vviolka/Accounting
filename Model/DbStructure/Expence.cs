namespace Model.DbStructure
{
    public class Expence
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public float Count { get; set; }
        public float Cost { get; set; }
        public int ProductionId { get; set; }
    }
}
namespace Model.DbStructure
{
    public class EmployeesPosts
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int PostId { get; set; }
        public float Bid { get; set; }
    }
}
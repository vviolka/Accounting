
namespace Model.DBStructure
{
    public class WorkedDay
    {
        public int Id { get; set; }
        public int MonthId { get; set; }
    
        public string? Value{ get; set; }
       
        public string? Day { get; set; }
    }
}
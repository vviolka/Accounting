using System;

namespace Model.DBStructure
{
    public class PartnersBalances
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public DateTime Date { get; set; }
        public float Sum { get; set; }
    }
}
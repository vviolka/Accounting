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
        public string? DocumentNumber { get; set; }
        public float Cullet { get; set; }
        public float Steel { get; set; }
        public float Aluminum { get; set; }
        public float Overhead { get; set; }
        public float Transportation { get; set; }
        public float SellingPrice  { get; set; }
        public float HoursForProd { get; set; }
    }
}
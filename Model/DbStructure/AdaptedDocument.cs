using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DBStructure
{
    public class AdaptedDocument
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Number { get; set; }
        public string Partner { get; set; }
    }
}

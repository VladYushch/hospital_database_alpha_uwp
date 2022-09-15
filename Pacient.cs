using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3
{
    public class Pacient
    {
        public int ID { get; set; } 
        public string FirstName { get; set; }   
        public string LastName { get; set; }
        public string MiddleName { get; set; }  

        public DateTime birtdate { get; set; }
        public string phone { get; set; }   
        public string city { get; set; }
    }
}

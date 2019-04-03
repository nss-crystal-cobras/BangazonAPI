using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Computer
    {
        public int Id { get; set; }

        
        public DateTime PurchaseDate { get; set; }

 
        public DateTime DecommisionDate { get; set; }
        
        public string Make { get; set; }

       
        public string Manufacturer { get; set; }

       


    }
}

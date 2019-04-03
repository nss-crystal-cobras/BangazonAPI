using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Department
    {
            public int Id { get; set; }

            [StringLength(50, MinimumLength = 2)]
            public String Name { get; set; }
            public int Budget { get; set; }
    }
}

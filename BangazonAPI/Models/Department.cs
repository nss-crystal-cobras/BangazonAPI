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

            [Required]
            [StringLength(50, MinimumLength = 2)]
            public String Name { get; set; }

            [Required]
            public int Budget { get; set; }

            public Employee Employee { get; set; }
            public int EmployeeId { get; set; }
           

    }
}

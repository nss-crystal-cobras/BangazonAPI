using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Employee
    {
            public int Id { get; set; }

            [Required]
            [StringLength(50, MinimumLength = 2)]
            public string FirstName { get; set; }

            [Required]
            [StringLength(50, MinimumLength = 2)]
            public string LastName { get; set; }

            [Required]
            public int DepartmentId { get; set; }

            [Required]
            public bool IsSupervisor { get; set; }

            
            public Department Department { get; set; }

            
            public Computer Computer { get; set; }


            public int? ComputerId { get; set; }





    }
}

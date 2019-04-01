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

            [Required]
            public Department Department { get; set; }

            [Required]
            public Computer Computer { get; set; }

           public List<Department> Departments { get; set; } = new List<Department>();
           public List<Computer> Computers { get; set; } = new List<Computer>();


    }
}

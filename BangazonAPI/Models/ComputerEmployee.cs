using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class ComputerEmployee
    {

        public int Id { get; set; }

        [Required]
        public DateTime AssignDate { get; set; }

        [Required]
        public DateTime UnassignDate { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Make { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Manufacturer { get; set; }

        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
    }
}

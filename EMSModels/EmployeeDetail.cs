using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMSModels
{
    public class EmployeeDetail
    {
        [Key]
        public Guid Id {get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Department { get; set; }

        public string Feedback { get; set; }
    }
}

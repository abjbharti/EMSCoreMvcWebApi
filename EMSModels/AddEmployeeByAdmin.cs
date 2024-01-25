using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMSModels
{
    public class AddEmployeeByAdmin
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Department { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMSModels
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string FeedBackDescription { get; set; } = string.Empty;
        public string FeedBackBy { get; set; } = string.Empty;
    }
}

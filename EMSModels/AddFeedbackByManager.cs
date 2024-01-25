using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMSModels
{
    public class AddFeedbackByManager
    {
        [Required]
        public string Feedback { get; set; }
    }
}

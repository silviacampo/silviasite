using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SilviaSite.Models
{
    public class ContactFormModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public bool Required { get; set; }
        [Required]
        public string Message { get; set; }
    }
}

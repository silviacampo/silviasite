using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SilviaSite.Models
{
    public class EmailMessage
    {
        public EmailMessage()
        {
        }
        [Required]
        public string From { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public bool Required { get; set; }
    }
}

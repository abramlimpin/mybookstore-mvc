using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBookstoreMVC.Models
{
    public class Publisher
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Publisher Name")]
        [MaxLength(40)]
        [Required]
        public string Name { get; set; }
    }
}
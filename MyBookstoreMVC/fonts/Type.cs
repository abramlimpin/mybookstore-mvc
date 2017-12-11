using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBookstoreMVC.fonts
{
    public class Type
    {
        [Key]
        public int ID { get; set; }

        [Display(Name ="Type Name")]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
    }
}
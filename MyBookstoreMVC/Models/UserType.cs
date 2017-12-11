using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBookstoreMVC.Models
{
    public class UserType
    {
        [Key]
        public int ID { get; set; }

        [Display(Name ="User Type")]
        [Required(ErrorMessage ="Select from the list.")]
        public string Name { get; set; }
    }
}
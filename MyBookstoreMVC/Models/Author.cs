using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBookstoreMVC.Models
{
    public class Author
    {
        [Key]
        public int ID { get; set; }

        [Display(Name ="Last Name")]
        [MaxLength(40)]
        [Required]
        public string LN { get; set; }

        [Display(Name ="First Name")]
        [MaxLength(20)]
        [Required]
        public string FN { get; set; }

        public string FullName { get; set; }

        [Display(Name ="Phone")]
        [MaxLength(12)]
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name ="Address")]
        [MaxLength(40)]
        [Required]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name ="City")]
        [MaxLength(20)]
        [Required]
        public string City { get; set; }

        [Display(Name = "State")]
        [MaxLength(2, ErrorMessage ="Invalid format.")]
        [Required]
        public string State { get; set; }

        [Display(Name = "Zip Code")]
        [MaxLength(5, ErrorMessage ="Invalid format.")]
        [MinLength(5, ErrorMessage = "Invalid format.")]
        [Required]
        public string Zip { get; set; }
    }
}
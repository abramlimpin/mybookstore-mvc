using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBookstoreMVC.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "User Type")]
        [Required]
        public int TypeID { get; set; }
                
        public List<UserType> Types { get; set; }
        public string UserType { get; set; }

        [Display(Name ="Email Address")]
        [MaxLength(80)]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name ="Password")]
        [MaxLength(50)]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="First Name")]
        [MaxLength(80)]
        [Required]
        public string FN { get; set; }

        [Display(Name ="Last Name")]
        [MaxLength(80)]
        [Required]
        public string LN { get; set; }

        [Display(Name = "Phone")]
        [MaxLength(12)]
        [MinLength(12)]
        [Required]
        public string Phone { get; set; }

        [Display(Name = "Address")]
        [MaxLength(200)]
        [Required]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name ="Status")]
        public string CurrentStatus { get; set; }

        [Display(Name = "Status")]
        public List<SelectListItem> Status { get; set; }
    }
}
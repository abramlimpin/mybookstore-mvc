using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBookstoreMVC.Models
{
    public class Title
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Publisher")]
        [Required(ErrorMessage = "Select from the list.")]
        public int PubID { get; set; }
        
        public List<Publisher> Publishers { get; set; }

        public string Publisher { get; set; }

        [Display(Name = "Author")]
        [Required(ErrorMessage = "Select from the list.")]
        public int AuthorID { get; set; }
        
        public List<Author> Authors { get; set; }
        public string Author { get; set; }

        [Display(Name ="Title Name")]
        [MaxLength(80)]
        [Required]
        public string Name { get; set; }

        [Display(Name="Publication Date")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}", ApplyFormatInEditMode =true)]
        public DateTime PubDate { get; set; }

        [Display(Name ="Price")]
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name ="Notes")]
        [MaxLength(200)]
        [Required]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using ArtStore.Models;

namespace ArtStore.ViewModels
{
    public class PainterViewModel
    {

         public int PainterID { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Date of Birth")]
        public DateTime DateofBirth { get; set; }
        [Display(Name = "Date of Death")]
        public DateTime? DateofDeath { get; set; }
        public string Biography { get; set; }
        
        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public ICollection<Painting> Painting {get; set;}        
        public IFormFile Picture { get; set; }
        public string Paintings {get; set;}
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using ArtStore.Models;

namespace ArtStore.ViewModels
{
    public class PaintingViewModel
    {

        public int PaintingID { get; set; }
        public int PainterID { get; set; }
        public string Title { get; set; }
        [Display(Name = "Original Title")]
        public string OriginalTitle { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }
        [ForeignKey("PainterID")]
        public Painter Painter {get; set;}
        public ICollection<Painter> Painters {get; set;}      
        public IFormFile Picture { get; set; }
    }
}

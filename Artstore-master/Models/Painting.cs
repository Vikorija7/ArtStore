using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;


namespace ArtStore.Models
{
    public class Painting
    {

        [Display(Name = "Слика")]
        public string Picture { get; set; }
        [Required]
        [Key]
        public int PaintingID { get; set; }
        public int PainterID { get; set; }
        [Display(Name = "Наслов")]
        public string Title { get; set; }
        [Display(Name = "Оригинален наслов")]
        public string OriginalTitle { get; set; }
        [Display(Name = "Категорија")]
        public string Category { get; set; }
        [Display(Name = "Краток опис")]
        public string Description { get; set; }
        [Display(Name = "Цена")]
        public int Price { get; set; }
        [Display(Name = "Дата на излегување")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        [ForeignKey("PainterID")]
        [Display(Name = "Автор")]
        public Painter Painter {get; set;}
        public ICollection<User> Users { get; set; }
       
    }
}
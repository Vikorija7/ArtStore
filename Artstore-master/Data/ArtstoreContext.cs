using ArtStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtStore.Data
{
    public class ArtstoreContext : IdentityDbContext<AppUser>
    {
        public ArtstoreContext(DbContextOptions<ArtstoreContext> options) : base(options)
        {
        }

        public DbSet<Painting> Painting { get; set; }
        public DbSet<Painter> Painter { get; set; }
        public DbSet<AppUser> AppUser{ get; set; }
        public DbSet<User> User{ get; set; }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Painting>().ToTable("Painting")
              .HasOne(b => b.Painter)
            .WithMany(i => i.Painting)
            .HasForeignKey(b => b.PainterID).OnDelete(DeleteBehavior.NoAction);
            
             modelBuilder.Entity<Painter>().ToTable("Painter");
             
            modelBuilder.Entity<User>().ToTable("User")
            .Property(e => e.UserID)
            .ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }
    }
}
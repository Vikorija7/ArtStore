using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ArtStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ArtStore.Data;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using ArtStore.ViewModels;

namespace ArtStore.Controllers
{
  
    public class PaintingController : Controller
    {
        private readonly ArtstoreContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PaintingController(ArtstoreContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index(string searchString)
        {         
            ViewData["CurrentFilter"] = searchString;
           
            var paintings = from b in _context.Painting
                   select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                 paintings = paintings.Where(s => s.Title.Contains(searchString));
            }
          
         return View(await paintings.AsNoTracking().ToListAsync());
        }
        

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
             return NotFound();
             }

            var painting = await _context.Painting
            .Include(s => s.Painter)
            .AsNoTracking()
             .FirstOrDefaultAsync(m => m.PaintingID == id);

            if (painting == null)
             {
                 return NotFound();
             }

         return View(painting);
        }
        // GET: Painting/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            PopulateDropDownList();
            return View();
        }
         [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(PaintingViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Painting painting = new Painting
                {
                    Picture = uniqueFileName,
                    PainterID = model.PainterID,
                    Title = model.Title,
                    OriginalTitle = model.OriginalTitle,
                    Category = model.Category,
                    Description = model.Description,
                    Price = model.Price,
                    ReleaseDate = model.ReleaseDate,
                    Painter = model.Painter
                };

                _context.Add(painting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } 
          PopulateDropDownList(model.PainterID);          
           return View();
        }


        // GET: Painting/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var painting = await _context.Painting.FindAsync(id);

            if (painting == null)
            {
                return NotFound();
            }

             PaintingViewModel vm = new PaintingViewModel
                {
                    PaintingID = painting.PaintingID,
                    Title = painting.Title,
                    OriginalTitle = painting.OriginalTitle,
                    Category = painting.Category,
                    Description = painting.Description,
                    Price=painting.Price,
                    ReleaseDate=painting.ReleaseDate,
                    Painter=painting.Painter,
                    PainterID=painting.PainterID
                };
            ViewData["PainterID"] = new SelectList(_context.Painter, "PainterID", "FullName", painting.PainterID);

            return View(vm);
        }

        // POST: Painting/Edit/5
      
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditPost(int id, PaintingViewModel vm)
        {
            //if (id != vm.PaintingID)
            //{
            //    return NotFound();
            // }

            var paintingDatabase = await _context.Painting.FindAsync(id);

            if (ModelState.IsValid)
       {
           try
                {
                    string uniqueFileName = UploadedFile(vm);

               Painting painting = new Painting
                {
                    PaintingID = id,
                    Title = vm.Title,
                    OriginalTitle = vm.OriginalTitle,
                    Category = vm.Category,
                    Description = vm.Description,
                    Picture = uniqueFileName,
                    Price=vm.Price,
                    ReleaseDate=vm.ReleaseDate,
                    Painter= paintingDatabase.Painter,
                    PainterID = paintingDatabase.PainterID
                };

                    _context.Update(painting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaintingExists(vm.PaintingID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
       }
             return View(vm);
          }

        

        private void PopulateDropDownList(object selectedpainter = null)
        {
            var painterQuery = from d in _context.Painter
                                   orderby d.FirstName
                                   select d;
            ViewBag.PainterID = new SelectList(painterQuery.AsNoTracking(), "PainterID", "FullName", selectedpainter);
        }


        // GET: Painting/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var painting = await _context.Painting
                .Include(c => c.Painter)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.PaintingID == id);
            if (painting == null)
            {
                return NotFound();
            }

            return View(painting);
        }

        // POST: Painting/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var painting = await _context.Painting.FindAsync(id);
            _context.Painting.Remove(painting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
          [HttpPost]
     
        private bool PaintingExists(int id)
        {
            return _context.Painting.Any(e => e.PaintingID == id);
        }
          private string UploadedFile(PaintingViewModel model)
        {
            string uniqueFileName = null;

            if (model.Picture != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "pictures");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.Picture.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Picture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        public IActionResult Text()
        {
            return View();
        }
        public IActionResult Order(int id)
        {
            TempData["ID"] = id;
            ViewData["PaintingName"] = _context.Painting.Where(t => t.PaintingID == id).Select(t => t.Title).FirstOrDefault();
            ViewData["PaintingPrice"] = _context.Painting.Where(t => t.PaintingID == id).Select(t => t.Price).FirstOrDefault();
            string imageURL = "~/pictures/" + _context.Painting.Where(t => t.PaintingID == id).Select(t => t.Picture).FirstOrDefault();
            ViewData["Image"] = imageURL;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order([Bind("FullName, PhoneNumber, Address, City, PaintingID, Painting")] User order)
        {
           if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Text));
            }
            
            return View();
        }
       
            public IActionResult OrderedPaintings()
            {
            int id = Convert.ToInt32(TempData["ID"]);
            IQueryable<User> user = _context.User;
                return View(user);
            }        

    }
}
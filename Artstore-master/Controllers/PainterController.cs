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
  
    public class PainterController : Controller
    {
        private readonly ArtstoreContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PainterController(ArtstoreContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index(string String)
        {
            ViewData["Filter"] = String;

            var painters = from a in _context.Painter
                          select a;
            if (!String.IsNullOrEmpty(String))
            {
                painters = painters.Where(s => s.FirstName.Contains(String)
                                          || s.LastName.Contains(String));
            }
            return View(painters);
        }
        public async Task<IActionResult> Details(int? id)
{
    if (id == null)
    {
        return NotFound();
    }
    
      var painter = await _context.Painter
        .FirstOrDefaultAsync(m => m.PainterID == id);

    if (painter == null)
    {
        return NotFound();
    }

    return View(painter);
}
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
        return View();
        }
         [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(PainterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Painter painter = new Painter
                {
                    Picture = uniqueFileName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateofBirth = model.DateofBirth,
                    DateofDeath = model.DateofBirth,
                    Biography = model.Biography,
                    Painting=model.Painting,
                    Paintings=model.Paintings
                };

                _context.Add(painter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        // GET: Painter/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var painter = await _context.Painter.FindAsync(id);

            if (painter == null)
            {
                return NotFound();
            }

              PainterViewModel vm = new PainterViewModel
                {
                    PainterID = painter.PainterID,
                    FirstName = painter.FirstName,
                    LastName = painter.LastName,
                    DateofBirth = painter.DateofBirth,
                    DateofDeath = painter.DateofBirth,
                    Biography = painter.Biography,
                    Painting=painter.Painting
                };
            return View(vm);
        }

        // POST: Painter/Edit/5
        [Authorize(Roles = "Admin")]

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        
            public async Task<IActionResult> EditPost(int id, PainterViewModel vm)
        {
           // if (id != vm.PainterID)
           // {
            //    return NotFound();
           // }

       if (ModelState.IsValid)
       {
           try
                {
                    string uniqueFileName = UploadedFile(vm);

               Painter painter = new Painter
               {
                    PainterID = id,
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    DateofBirth = vm.DateofBirth,
                    DateofDeath = vm.DateofBirth,
                    Biography = vm.Biography,
                    Picture = uniqueFileName
                    //Painting=vm.Painting
                };

                    _context.Update(painter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(vm.PainterID))
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

        private void PopulateDropDownList(object selectedPainting = null)
        {
            var paintingsQuery = from d in _context.Painting
                                   orderby d.Title
                                   select d;
            ViewBag.Paintings = new SelectList(paintingsQuery.AsNoTracking(), "PaintingsID", "Title", selectedPainting);
        }


        // GET: Painter/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var painter = await _context.Painter
                .FirstOrDefaultAsync(m => m.PainterID == id);
            if (painter == null)
            {
                return NotFound();
            }

            return View(painter);
        }

        // POST: Painter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var painter = await _context.Painter.FindAsync(id);
            _context.Painter.Remove(painter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
          [HttpPost]
     
        private bool AuthorExists(int id)
        {
            return _context.Painter.Any(e => e.PainterID == id);
        }

         private string UploadedFile(PainterViewModel model)
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
         public async Task<IActionResult> MyPaintings(int id)
        {
            IQueryable<Painting> painting = _context.Painting;

            painting = painting.Where(s=>s.PainterID==id);
            
            ViewData["PaintersName"] = _context.Painter.Where(t => t.PainterID == id).Select(t => t.FullName).FirstOrDefault();
            return View(painting);
        }
             
    }
}
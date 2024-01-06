using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryModel.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Niculae_AnaMaria_Lab2.Data;
using Niculae_AnaMaria_Lab2.Models;
using LibraryModel.Models; 

namespace Niculae_AnaMaria_Lab2.Controllers
{
    public class BooksDropdownController : Controller
    {
        private readonly LibraryContext _context;

        public BooksDropdownController(LibraryContext context)
        {
            _context = context;
        }

        // GET: BooksDropdown
        public async Task<IActionResult> Index()
        {
            var libraryContext = _context.Books.Include(b => b.Author);
            return View(await libraryContext.ToListAsync());
        }


        // GET: BooksDropdown/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: BooksDropdown/Create
        public IActionResult Create()
        {
            ViewData["AuthorID"] = new SelectList(_context.Authors, "ID", "FullName");
            return View();
        }

        // POST: BooksDropdown/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Price,AuthorID")] Book book)
        {
            try
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch { }

            ViewData["AuthorID"] = new SelectList(_context.Authors, "ID", "FullName", book.AuthorID);
            return View(book);
        }

        // GET: BooksDropdown/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorID"] = new SelectList(_context.Authors, "ID", "FullName", book.AuthorID);
            return View(book);
        }

        // POST: BooksDropdown/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Price,AuthorID")] Book book)
        {
            if (id != book.ID)
            {
                return NotFound();
            }

            
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            ViewData["AuthorID"] = new SelectList(_context.Authors, "ID", "FullName", book.AuthorID);
            return View(book);
        }

        // GET: BooksDropdown/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: BooksDropdown/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'LibraryContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return (_context.Books?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}

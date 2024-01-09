using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Niculae_AnaMaria_Lab2.Data;
using Niculae_AnaMaria_Lab2.Models;
using LibraryModel.Data;
using LibraryModel.Models;
using Microsoft.AspNetCore.Authorization;

namespace Niculae_AnaMaria_Lab2.Controllers
{
    [Authorize(Roles = "Employee")]

    public class BooksController : Controller
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Books
        //public async Task<IActionResult> Index()
        //{
        //      return _context.Books != null ? 
        //                  View(await _context.Books.ToListAsync()) :
        //                  Problem("Entity set 'LibraryContext.Books'  is null.");
        //}

        //public async Task<IActionResult> Index(string sortOrder, string searchString)
        //{
        //    ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
        //    ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
        //    ViewData["CurrentFilter"] = searchString;
        //    var books = from b in _context.Books
        //                select b;

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        books = books.Where(s => s.Title.Contains(searchString));
        //    }

        //    switch (sortOrder)
        //    {
        //        case "title_desc":
        //            books = books.OrderByDescending(b => b.Title);
        //            break;
        //        case "Price":
        //            books = books.OrderBy(b => b.Price);
        //            break;
        //        case "price_desc":
        //            books = books.OrderByDescending(b => b.Price);
        //            break;
        //        default:
        //            books = books.OrderBy(b => b.Title);
        //            break;
        //    }
        //    return View(await books.AsNoTracking().ToListAsync());
        //}
        [AllowAnonymous]
        public async Task<IActionResult> Index(
        string sortOrder,
        string currentFilter,
        string searchString,
        int? pageNumber)
        {

            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var books = from b in _context.Books
                        select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "title_desc":
                    books = books.OrderByDescending(b => b.Title);
                    break;
                case "Price":
                    books = books.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    books = books.OrderByDescending(b => b.Price);
                    break;
                default:
                    books = books.OrderBy(b => b.Title);
                    break;
            }
            int pageSize = 2;
            return View(await PaginatedList<Book>.CreateAsync(books.AsNoTracking(), pageNumber ??
           1, pageSize));
        }
        [AllowAnonymous]
        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            //var book = await _context.Books
            //    .FirstOrDefaultAsync(m => m.ID == id);

            var book = await _context.Books
             .Include(s => s.Orders)
             .ThenInclude(e => e.Customer)
             .AsNoTracking()
             .FirstOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,FullName,Price")] Book book)
        {
            try
            {

                try { 
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } catch { }

            }
            catch (DbUpdateException /* ex*/)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists ");
            }

            return View(book);
        }

        // GET: Books/Edit/5
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
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Author,Price")] Book book)
        //{
        //    if (id != book.ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(book);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BookExists(book.ID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(book);
        //}

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var bookToUpdate = await _context.Books.FirstOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Book>(
            bookToUpdate,
            "",
            s => s.Author, s => s.Title, s => s.Price))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists");
                }
            }
            return View(bookToUpdate);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                "Delete failed. Try again";
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'LibraryContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                if (book != null)
            {
                _context.Books.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {

                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool BookExists(int id)
        {
          return (_context.Books?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}

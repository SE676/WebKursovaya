using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebKursovaya.Data;
using WebKursovaya.Models;

namespace WebKursovaya.Controllers
{
    public class Внутренние_экзаменыController : Controller
    {
        private readonly MyDBContext _context;

        public Внутренние_экзаменыController(MyDBContext context)
        {
            _context = context;
        }

        // GET: Внутренние_экзамены
        public async Task<IActionResult> Index()
        {
            var myDBContext = _context.Внутренние_экзамены.Include(в => в.Автошкола).Include(в => в.Заявитель);
            return View(await myDBContext.ToListAsync());
        }

        // GET: Внутренние_экзамены/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Внутренние_экзамены == null)
            {
                return NotFound();
            }

            var внутренние_экзамены = await _context.Внутренние_экзамены
                .Include(в => в.Автошкола)
                .Include(в => в.Заявитель)
                .FirstOrDefaultAsync(m => m.Код_внутреннего_экзамена == id);
            if (внутренние_экзамены == null)
            {
                return NotFound();
            }

            return View(внутренние_экзамены);
        }

        // GET: Внутренние_экзамены/Create
        public IActionResult Create()
        {
            ViewData["Код_автошколы"] = new SelectList(_context.Автошколы, "Код_автошколы", "АШ");
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "ЗаявительФИО");
            return View();
        }

        // POST: Внутренние_экзамены/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Код_внутреннего_экзамена,Код_автошколы,Код_заявителя,Дата_внутреннего_экзамена,Результат_внутреннего_экзамена")] Внутренние_экзамены внутренние_экзамены)
        {
            if (ModelState.IsValid)
            {
                _context.Add(внутренние_экзамены);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Код_автошколы"] = new SelectList(_context.Автошколы, "Код_автошколы", "Email_автошколы", внутренние_экзамены.Код_автошколы);
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "Имя", внутренние_экзамены.Код_заявителя);
            return View(внутренние_экзамены);
        }

        // GET: Внутренние_экзамены/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Внутренние_экзамены == null)
            {
                return NotFound();
            }

            var внутренние_экзамены = await _context.Внутренние_экзамены.FindAsync(id);
            if (внутренние_экзамены == null)
            {
                return NotFound();
            }
            ViewData["Код_автошколы"] = new SelectList(_context.Автошколы, "Код_автошколы", "Email_автошколы", внутренние_экзамены.Код_автошколы);
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "Имя", внутренние_экзамены.Код_заявителя);
            return View(внутренние_экзамены);
        }

        // POST: Внутренние_экзамены/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Код_внутреннего_экзамена,Код_автошколы,Код_заявителя,Дата_внутреннего_экзамена,Результат_внутреннего_экзамена")] Внутренние_экзамены внутренние_экзамены)
        {
            if (id != внутренние_экзамены.Код_внутреннего_экзамена)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(внутренние_экзамены);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Внутренние_экзаменыExists(внутренние_экзамены.Код_внутреннего_экзамена))
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
            ViewData["Код_автошколы"] = new SelectList(_context.Автошколы, "Код_автошколы", "Email_автошколы", внутренние_экзамены.Код_автошколы);
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "Имя", внутренние_экзамены.Код_заявителя);
            return View(внутренние_экзамены);
        }

        // GET: Внутренние_экзамены/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Внутренние_экзамены == null)
            {
                return NotFound();
            }

            var внутренние_экзамены = await _context.Внутренние_экзамены
                .Include(в => в.Автошкола)
                .Include(в => в.Заявитель)
                .FirstOrDefaultAsync(m => m.Код_внутреннего_экзамена == id);
            if (внутренние_экзамены == null)
            {
                return NotFound();
            }

            return View(внутренние_экзамены);
        }

        // POST: Внутренние_экзамены/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Внутренние_экзамены == null)
            {
                return Problem("Entity set 'MyDBContext.Внутренние_экзамены'  is null.");
            }
            var внутренние_экзамены = await _context.Внутренние_экзамены.FindAsync(id);
            if (внутренние_экзамены != null)
            {
                _context.Внутренние_экзамены.Remove(внутренние_экзамены);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Внутренние_экзаменыExists(int id)
        {
          return (_context.Внутренние_экзамены?.Any(e => e.Код_внутреннего_экзамена == id)).GetValueOrDefault();
        }
    }
}

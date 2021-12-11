using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using WorkLogerCA.Models;

namespace WorkLogerCA.Controllers
{
    [Authorize]
    public class WorksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Works
        public async Task<IActionResult> Index(string ExcelExport)
        {

            var equipments = await _context.Work.Include(t =>t.Transport).ToListAsync();

            var recordsToDisplay = equipments.Select(x => new
            {
                Дата_и_время_создания = x.CreationDateTime.GetDateTimeFormats('G'),
                Исполнители_работ = x.PerformersOfWork,
                Дата_завершения_работ = x.CompletionDate.GetDateTimeFormats('G'),
                Описание_выполненных_работ = x.DescriptionOfPerformedWork,
                Примечение = x.Note,
                Место_выполнения_работ = x.PlaceOfWork,
                Выполнение_работы = x.WorkCompletiting,
                Транспортное_средство = x.Transport?.Note,          
            }).ToList();

            if (ExcelExport != null)
            {
                // above code loads the data using LINQ with EF (query of table), you can substitute this with any data source.
                var stream = new MemoryStream();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Работа");
                    workSheet.Cells.LoadFromCollection(recordsToDisplay, true);
                    package.Save();
                }
                stream.Position = 0;
                string excelName = $"Works-{DateTime.Now:ddMMyyyyHHmmssfff}.xlsx";
                // above I define the name of the file using the current datetime.
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                // this will be the actual export.
            }

            var applicationDbContext = _context.Work.Include(w => w.Transport);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Works/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var work = await _context.Work
                .Include(w => w.Transport)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (work == null)
            {
                return NotFound();
            }

            return View(work);
        }

        // GET: Works/Create
        public IActionResult Create()
        {
            ViewData["TransportId"] = new SelectList(_context.Transport, "Id", "Destination");
            return View();
        }

        // POST: Works/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CreationDateTime,PerformersOfWork,CompletionDate,DescriptionOfPerformedWork,Note,PlaceOfWork,WorkCompletiting,TransportId")] Work work)
        {
            if (ModelState.IsValid)
            {
                _context.Add(work);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TransportId"] = new SelectList(_context.Transport, "Id", "Destination", work.TransportId);
            return View(work);
        }

        // GET: Works/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var work = await _context.Work.FindAsync(id);
            if (work == null)
            {
                return NotFound();
            }
            ViewData["TransportId"] = new SelectList(_context.Transport, "Id", "Destination", work.TransportId);
            return View(work);
        }

        // POST: Works/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreationDateTime,PerformersOfWork,CompletionDate,DescriptionOfPerformedWork,Note,PlaceOfWork,WorkCompletiting,TransportId")] Work work)
        {
            if (id != work.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(work);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkExists(work.Id))
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
            ViewData["TransportId"] = new SelectList(_context.Transport, "Id", "Destination", work.TransportId);
            return View(work);
        }

        // GET: Works/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var work = await _context.Work
                .Include(w => w.Transport)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (work == null)
            {
                return NotFound();
            }

            return View(work);
        }

        // POST: Works/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var work = await _context.Work.FindAsync(id);
            _context.Work.Remove(work);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkExists(int id)
        {
            return _context.Work.Any(e => e.Id == id);
        }
    }
}

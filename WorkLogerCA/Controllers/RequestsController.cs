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
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Requests
        public async Task<IActionResult> Index(string ExcelExport, string search)
        {

            var request = await _context.Request.ToListAsync();

            var recordsToDisplay = request.Select(x => new
            {
                Дата = x.CreationDateTime.GetDateTimeFormats('G'),
                Номер_заявки = x.RequestNumber,
                Дата_и_время_поступления_заявки = x.RequestDateTime.GetDateTimeFormats('G'),
                Номер_буровой_бригады = x.NumberDrillingCrew,
                Описание_заявки = x.RequestDescription,
                Место_выполнения_работ = x.PlaceOfWork,
                Примечания = x.Note,
                Отправка_результатов_испытания = x.SendResult,
                Повторная_заявка = x.RepeatedRequest,
                Дата_и_время_подачи_заявки = x.DateTimeSendRequest.GetDateTimeFormats('G'),
                Выполнение_заявки_подрядной_организацией = x.CompletedRequest,
                Примечание_для_подрядчика = x.ContractorNote,
                Выполнение_заявки = x.RequestState
            }).ToList();

            if (ExcelExport != null)
            {
                // above code loads the data using LINQ with EF (query of table), you can substitute this with any data source.
                var stream = new MemoryStream();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Заявки");
                    workSheet.Cells.LoadFromCollection(recordsToDisplay, true);
                    package.Save();
                }
                stream.Position = 0;
                string excelName = $"Requests-{DateTime.Now:ddMMyyyyHHmmssfff}.xlsx";
                // above I define the name of the file using the current datetime.
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                // this will be the actual export.
            }


            if (!string.IsNullOrEmpty(search))
            {
                var requests = from r in _context.Request select r;
                return View(requests.Where(r => r.RequestNumber.Contains(search) 
                || r.Note.Contains(search) 
                || r.RequestDescription.Contains(search)
                || r.NumberDrillingCrew.ToString().Contains(search)
                || r.PlaceOfWork.Contains(search)
                || r.RepeatedRequest.Contains(search)
                || r.ContractorNote.Contains(search)
                ));
            }

            return View(await _context.Request.ToListAsync());
        }

        // GET: Requests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Request
                .FirstOrDefaultAsync(m => m.Id == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // GET: Requests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CreationDateTime,RequestNumber,RequestDateTime,NumberDrillingCrew,RequestDescription,PlaceOfWork,Note,SendResult,RepeatedRequest,DateTimeSendRequest,CompletedRequest,ContractorNote,RequestState")] Request request)
        {
            if (ModelState.IsValid)
            {
                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Request.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreationDateTime,RequestNumber,RequestDateTime,NumberDrillingCrew,RequestDescription,PlaceOfWork,Note,SendResult,RepeatedRequest,DateTimeSendRequest,CompletedRequest,ContractorNote,RequestState")] Request request)
        {
            if (id != request.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestExists(request.Id))
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
            return View(request);
        }

        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Request
                .FirstOrDefaultAsync(m => m.Id == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = await _context.Request.FindAsync(id);
            _context.Request.Remove(request);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestExists(int id)
        {
            return _context.Request.Any(e => e.Id == id);
        }
    }
}

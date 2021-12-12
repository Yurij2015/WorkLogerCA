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
    public class TransportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transports
        public async Task<IActionResult> Index(string ExcelExport, string search)
        {
            var transport = await _context.Transport.ToListAsync();

            var recordsToDisplay = transport.Select(x => new
            {
                Дата_и_время_создания = x.CreationDateTime.GetDateTimeFormats('G'),
                ФИО_водителя = x.DriverFullName,
                Номер_путевого_листа = x.WaybillNumber,
                Прямой_рейс = x.DirectRide,
                Обратный_рейс = x.ReturnRide,
                Место_назначения = x.Destination,
                Обратное_место_назначения = x.ReturnDestination,
                Дата_и_время_отправления = x.DetpartureDateTime.GetDateTimeFormats('G'),
                Дата_и_время_прибытия = x.ArrivalDateTime.GetDateTimeFormats('G'),
                Пассажиры = x.Passengers,
                Примечение = x.Note,
            }).ToList();

            if (ExcelExport != null)
            {
                // above code loads the data using LINQ with EF (query of table), you can substitute this with any data source.
                var stream = new MemoryStream();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Транспорт");
                    workSheet.Cells.LoadFromCollection(recordsToDisplay, true);
                    package.Save();
                }
                stream.Position = 0;
                string excelName = $"Transports-{DateTime.Now:ddMMyyyyHHmmssfff}.xlsx";
                // above I define the name of the file using the current datetime.
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                // this will be the actual export.
            }

            if (!string.IsNullOrEmpty(search))
            {
                var transports = from r in _context.Transport select r;
                return View(transports.Where(r => r.DriverFullName.Contains(search)
                || r.DirectRide.Contains(search)
                || r.ReturnRide.Contains(search)
                || r.WaybillNumber.ToString().Contains(search)
                || r.Destination.Contains(search)
                || r.ReturnDestination.Contains(search)
                || r.Passengers.Contains(search)
                || r.Note.Contains(search)
                ));
            }

            return View(await _context.Transport.ToListAsync());
        }

        // GET: Transports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transport = await _context.Transport
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transport == null)
            {
                return NotFound();
            }

            return View(transport);
        }

        // GET: Transports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Transports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CreationDateTime,DriverFullName,WaybillNumber,DirectRide,ReturnRide,Destination,ReturnDestination,DetpartureDateTime,ArrivalDateTime,Passengers,Note")] Transport transport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transport);
        }

        // GET: Transports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transport = await _context.Transport.FindAsync(id);
            if (transport == null)
            {
                return NotFound();
            }
            return View(transport);
        }

        // POST: Transports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreationDateTime,DriverFullName,WaybillNumber,DirectRide,ReturnRide,Destination,ReturnDestination,DetpartureDateTime,ArrivalDateTime,Passengers,Note")] Transport transport)
        {
            if (id != transport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransportExists(transport.Id))
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
            return View(transport);
        }

        // GET: Transports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transport = await _context.Transport
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transport == null)
            {
                return NotFound();
            }

            return View(transport);
        }

        // POST: Transports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transport = await _context.Transport.FindAsync(id);
            _context.Transport.Remove(transport);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransportExists(int id)
        {
            return _context.Transport.Any(e => e.Id == id);
        }
    }
}

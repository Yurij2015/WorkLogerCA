using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using WorkLogerCA.Models;

namespace WorkLogerCA.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class EquipmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EquipmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Equipments
        public async Task<IActionResult> Index(string ExcelExport, string search)
        {

            var equipments = await _context.Equipment.Include(t => t.Transport).Include(r => r.Request).ToListAsync();

            var recordsToDisplay = equipments.Select(x => new
            {
                Дата_и_время_создания = x.CreationDateTime.GetDateTimeFormats('G'),
                Наименование_оборудовнаия = x.EquipmentIdentification,
                Заводской_номер = x.FactoryNumber,
                Количество = x.Count,
                Состояние = x.State,
                Откуда_отправлено = x.SentFrom,
                Место_расположения = x.Location,
                Дата_и_время_прибытия = x.ArrivalDateTime?.GetDateTimeFormats('G'),
                Дата_время_отправления = x.DepartureDateTime?.GetDateTimeFormats('G'),
                Документы = x.Document,
                Действие_свидетельства_о_поверке = x.CertificateExpiryDate.GetDateTimeFormats('G'),
                Транспортное_средство = x.Transport?.Note,
                Заявка = x.Request?.Note
            }).ToList();

            if (ExcelExport != null)
            {
                // above code loads the data using LINQ with EF (query of table), you can substitute this with any data source.
                var stream = new MemoryStream();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Оборудование");
                    workSheet.Cells.LoadFromCollection(recordsToDisplay, true);
                    package.Save();
                }
                stream.Position = 0;
                string excelName = $"Equipments-{DateTime.Now:ddMMyyyyHHmmssfff}.xlsx";
                // above I define the name of the file using the current datetime.
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                // this will be the actual export.
            }

            if (!string.IsNullOrEmpty(search))
            {
                var equipment = from r in _context.Equipment select r;
                return View(equipment.Where(r => r.EquipmentIdentification.Contains(search)
                || r.FactoryNumber.Contains(search)
                || r.State.Contains(search)
                || r.Count.ToString().Contains(search)
                || r.SentFrom.Contains(search)
                || r.Location.Contains(search)
                || r.Document.Contains(search)
                ).Include(e => e.Request).Include(e => e.Transport));
            }


            var applicationDbContext = _context.Equipment.Include(e => e.Request).Include(e => e.Transport);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Equipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment
                .Include(e => e.Request)
                .Include(e => e.Transport)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // GET: Equipments/Create
        public IActionResult Create()
        {
            ViewData["RequestId"] = new SelectList(_context.Request, "Id", "PlaceOfWork");
            ViewData["TransportId"] = new SelectList(_context.Transport, "Id", "Destination");
            return View();
        }

        // POST: Equipments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CreationDateTime,EquipmentIdentification,FactoryNumber,Count,State,SentFrom,Location,ArrivalDateTime,DepartureDateTime,Document,CertificateExpiryDate,TransportId,RequestId")] Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(equipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RequestId"] = new SelectList(_context.Request, "Id", "PlaceOfWork", equipment.RequestId);
            ViewData["TransportId"] = new SelectList(_context.Transport, "Id", "Destination", equipment.TransportId);
            return View(equipment);
        }

        // GET: Equipments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }
            ViewData["RequestId"] = new SelectList(_context.Request, "Id", "PlaceOfWork", equipment.RequestId);
            ViewData["TransportId"] = new SelectList(_context.Transport, "Id", "Destination", equipment.TransportId);
            return View(equipment);
        }

        // POST: Equipments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreationDateTime,EquipmentIdentification,FactoryNumber,Count,State,SentFrom,Location,ArrivalDateTime,DepartureDateTime,Document,CertificateExpiryDate,TransportId,RequestId")] Equipment equipment)
        {
            if (id != equipment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipmentExists(equipment.Id))
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
            ViewData["RequestId"] = new SelectList(_context.Request, "Id", "PlaceOfWork", equipment.RequestId);
            ViewData["TransportId"] = new SelectList(_context.Transport, "Id", "Destination", equipment.TransportId);
            return View(equipment);
        }

        // GET: Equipments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment
                .Include(e => e.Request)
                .Include(e => e.Transport)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // POST: Equipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            _context.Equipment.Remove(equipment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipmentExists(int id)
        {
            return _context.Equipment.Any(e => e.Id == id);
        }
    }
}

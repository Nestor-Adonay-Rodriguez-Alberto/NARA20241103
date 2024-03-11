using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NARA20241103.Models;

namespace NARA20241103.Controllers
{
    public class FiadoresController : Controller
    {
        private readonly NARA20241103BDContext _context;

        public FiadoresController(NARA20241103BDContext context)
        {
            _context = context;
        }

        // GET: Fiadores
        public async Task<IActionResult> Index()
        {
              return _context.Fiadores != null ? 
                          View(await _context.Fiadores.ToListAsync()) :
                          Problem("Entity set 'NARA20241103BDContext.Fiadores'  is null.");
        }

        // GET: Fiadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Fiadores == null)
            {
                return NotFound();
            }

            var fiador = await _context.Fiadores
                .Include(s => s.DetalleFamiliares)
                .FirstOrDefaultAsync(m => m.IdFiador == id);
            if (fiador == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Details";
            return View(fiador);
        }

        // GET: Fiadores/Create
        public IActionResult Create()
        {
            var facturaVenta = new Fiador();
            facturaVenta.Fecha = DateTime.Now;
            facturaVenta.DineroFiado = 0;
            facturaVenta.DetalleFamiliares = new List<DetalleFamiliares>();
            facturaVenta.DetalleFamiliares.Add(new DetalleFamiliares
            {
                Parentesco = ""
            }) ;
            ViewBag.Accion = "Create";
            return View(facturaVenta);
        }

        // POST: Fiadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFiador,Nombre,Fecha,Correlativo,DineroFiado,DetalleFamiliares")] Fiador fiador)
        {
            
                _context.Add(fiador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
        }

        // --- --- --- --- --- --- AGREGA UNA FILA DE NUEVOS PRODUCTOS --- --- --- --- --- --- ---
        [HttpPost]
        public ActionResult AgregarDetalles([Bind("IdFiador,Nombre,Fecha,Correlativo,DineroFiado,DetalleFamiliares")] Fiador facturaVenta, string accion)
        {
            facturaVenta.DetalleFamiliares.Add(new DetalleFamiliares { Parentesco = "" });
            ViewBag.Accion = accion;
            return View(accion, facturaVenta);
        }

        // --- --- --- --- --- --- ELIMINA UNA FILA DE NUEVOS PRODUCTOS --- --- --- --- --- --- ---
        public ActionResult EliminarDetalles([Bind("IdFiador,Nombre,Fecha,Correlativo,DineroFiado,DetalleFamiliares")] Fiador facturaVenta,
           int index, string accion)
        {
            var det = facturaVenta.DetalleFamiliares[index];
            if (accion == "Edit" && det.IdDetalleFamilia > 0)
            {
                det.IdDetalleFamilia = det.IdDetalleFamilia * -1;
            }
            else
            {
                facturaVenta.DetalleFamiliares.RemoveAt(index);
            }

            ViewBag.Accion = accion;
            return View(accion, facturaVenta);
        }


        // GET: Fiadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Fiadores == null)
            {
                return NotFound();
            }

            var fiador = await _context.Fiadores
                .Include(s => s.DetalleFamiliares)
                .FirstOrDefaultAsync(m => m.IdFiador == id);
            if (fiador == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Edit";
            return View(fiador);
        }

        // POST: Fiadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdFiador,Nombre,Fecha,Correlativo,DineroFiado")] Fiador fiador)
        {
            if (id != fiador.IdFiador)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fiador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FiadorExists(fiador.IdFiador))
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
            return View(fiador);
        }

        // GET: Fiadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Fiadores == null)
            {
                return NotFound();
            }

            var fiador = await _context.Fiadores
                .Include(s => s.DetalleFamiliares)
                .FirstOrDefaultAsync(m => m.IdFiador == id);
            if (fiador == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Delete";
            return View(fiador);
        }

        // POST: Fiadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Fiadores == null)
            {
                return Problem("Entity set 'NARA20241103BDContext.Fiadores'  is null.");
            }
            var fiador = await _context.Fiadores.FindAsync(id);
            if (fiador != null)
            {
                _context.Fiadores.Remove(fiador);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FiadorExists(int id)
        {
          return (_context.Fiadores?.Any(e => e.IdFiador == id)).GetValueOrDefault();
        }
    }
}

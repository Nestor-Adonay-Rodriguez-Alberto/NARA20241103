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
        public async Task<IActionResult> Edit([Bind("IdFiador,Nombre,Fecha,Correlativo,DineroFiado,DetalleFamiliares")] Fiador fiador)
        {

            try
            {
                // Obtener Objeto BD Y Agregarle Los Atributos Que Tengo En La Vista
                // Obtener los datos de la base de datos que van a ser modificados
                var Objeto_BD = await _context.Fiadores
                        .Include(s => s.DetalleFamiliares)
                        .FirstAsync(s => s.IdFiador == fiador.IdFiador);

                Objeto_BD.Nombre = fiador.Nombre;
                Objeto_BD.Fecha = fiador.Fecha;
                Objeto_BD.Correlativo = fiador.Correlativo;
                Objeto_BD.DineroFiado = fiador.DineroFiado;


                // MODIFICAR DATOS DEL DETALLE 
                var detUpdate = fiador.DetalleFamiliares.Where(s => s.IdDetalleFamilia > 0);
                foreach (var d in detUpdate)
                {
                    var det = Objeto_BD.DetalleFamiliares.FirstOrDefault(s => s.IdDetalleFamilia == d.IdDetalleFamilia);
                    det.Nombre = d.Nombre;
                    det.Parentesco = d.Parentesco;
                    det.Telefono = d.Telefono;
                    det.Dui = d.Dui;
                }


                // AGREGAR FILAS DE DETALLES
                var detNew = fiador.DetalleFamiliares.Where(s => s.IdDetalleFamilia == 0);
                foreach (var d in detNew)
                {

                    Objeto_BD.DetalleFamiliares.Add(d);
                }


                // ELIMINA FILAS DEL DETALLE
                var delDet = fiador.DetalleFamiliares.Where(s => s.IdDetalleFamilia < 0).ToList();
                if (delDet != null && delDet.Count > 0)
                {
                    foreach (var d in delDet)
                    {
                        d.IdDetalleFamilia = d.IdDetalleFamilia * -1;
                        var det = Objeto_BD.DetalleFamiliares.FirstOrDefault(s => s.IdDetalleFamilia == d.IdDetalleFamilia);
                        _context.Remove(det);
                        // facturaUpdate.DetFacturaVenta.Remove(det);
                    }
                }
                // Aplicar esos cambios a la base de datos
                _context.Update(Objeto_BD);
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

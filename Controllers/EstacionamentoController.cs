using Estacionamento.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Estacionamento.Controllers
{
    [Authorize]
    public class EstacionamentoController : Controller
    {
        private readonly Contexto _context;

        public EstacionamentoController(Contexto context)
        {
            _context = context;
        }

        // GET: Estacionamento
        public async Task<IActionResult> Index()
        {
            return View(await _context.Estacionamentos.ToListAsync());
        }

        // GET: Estacionamento/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Estacionamentos == null)
            {
                return NotFound();
            }

            var estacionamentoModel = await _context.Estacionamentos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estacionamentoModel == null)
            {
                return NotFound();
            }

            return View(estacionamentoModel);
        }

        // GET: Estacionamento/Create
        public IActionResult Create()
        {
            var estado = Enum.GetValues(typeof(EstadoEnum))
                .Cast<EstadoEnum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                });

            ViewBag.bagEstado = estado;

            return View();
        }

        // POST: Estacionamento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Endereco,Cidade,Estado,QtdeVagasP,QtdeVagasM,QtdeVagasG,Tarifa,ValorHora")] EstacionamentoModel estacionamentoModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estacionamentoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estacionamentoModel);
        }

        // GET: Estacionamento/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Estacionamentos == null)
            {
                return NotFound();
            }

            var estacionamentoModel = await _context.Estacionamentos.FindAsync(id);
            if (estacionamentoModel == null)
            {
                return NotFound();
            }
            var estado = Enum.GetValues(typeof(EstadoEnum))
                .Cast<EstadoEnum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                });

            ViewBag.bagEstado = estado;

            return View(estacionamentoModel);
        }

        // POST: Estacionamento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Endereco,Cidade,Estado,QtdeVagasP,QtdeVagasM,QtdeVagasG,Tarifa,ValorHora")] EstacionamentoModel estacionamentoModel)
        {
            if (id != estacionamentoModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estacionamentoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstacionamentoModelExists(estacionamentoModel.Id))
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
            return View(estacionamentoModel);
        }

        // GET: Estacionamento/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Estacionamentos == null)
            {
                return NotFound();
            }

            var estacionamentoModel = await _context.Estacionamentos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estacionamentoModel == null)
            {
                return NotFound();
            }

            return View(estacionamentoModel);
        }

        // POST: Estacionamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Estacionamentos == null)
            {
                return Problem("Entity set 'Contexto.Estacionamentos'  is null.");
            }
            var estacionamentoModel = await _context.Estacionamentos.FindAsync(id);
            if (estacionamentoModel != null)
            {
                _context.Estacionamentos.Remove(estacionamentoModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstacionamentoModelExists(int id)
        {
            return _context.Estacionamentos.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Estacionamento.Models;
using Microsoft.AspNetCore.Authorization;

namespace Estacionamento.Controllers
{
    [Authorize]
    public class VeiculosController : Controller
    {
        private readonly Contexto _context;

        public VeiculosController(Contexto context)
        {
            _context = context;
        }

        // GET: Veiculos
        public async Task<IActionResult> Index()
        {
            var contexto = _context.Veiculos.Include(v => v.Modelo).Include(v => v.cliente);
            return View(await contexto.ToListAsync());
        }

        // GET: Veiculos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Veiculos == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculos
                .Include(v => v.Modelo)
                .Include(v => v.cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }

            return View(veiculo);
        }

        // GET: Veiculos/Create
        public IActionResult Create()
        {
            ViewData["ModeloId"] = new SelectList(_context.ModelosVeiculo, "Id", "Descricao");
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome");
            return View();
        }

        // POST: Veiculos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Placa,cor,ModeloId,ClienteId")] Veiculo veiculo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(veiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModeloId"] = new SelectList(_context.ModelosVeiculo, "Id", "Id", veiculo.ModeloId);
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", veiculo.ClienteId);
            return View(veiculo);
        }

        // GET: Veiculos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Veiculos == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
            {
                return NotFound();
            }
            ViewData["ModeloId"] = new SelectList(_context.ModelosVeiculo, "Id", "Id", veiculo.ModeloId);
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", veiculo.ClienteId);
            return View(veiculo);
        }

        // POST: Veiculos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Placa,cor,ModeloId,ClienteId")] Veiculo veiculo)
        {
            if (id != veiculo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(veiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeiculoExists(veiculo.Id))
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
            ViewData["ModeloId"] = new SelectList(_context.ModelosVeiculo, "Id", "Id", veiculo.ModeloId);
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", veiculo.ClienteId);
            return View(veiculo);
        }

        // GET: Veiculos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Veiculos == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculos
                .Include(v => v.Modelo)
                .Include(v => v.cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }

            return View(veiculo);
        }

        // POST: Veiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Veiculos == null)
            {
                return Problem("Entity set 'Contexto.Veiculos'  is null.");
            }
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo != null)
            {
                _context.Veiculos.Remove(veiculo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VeiculoExists(int id)
        {
          return _context.Veiculos.Any(e => e.Id == id);
        }
    }
}

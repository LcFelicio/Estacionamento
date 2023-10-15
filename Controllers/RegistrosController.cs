using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Estacionamento.Models;

namespace Estacionamento.Controllers
{
    public class RegistrosController : Controller
    {
        private readonly Contexto _context;

        public RegistrosController(Contexto context)
        {
            _context = context;
        }

        // GET: Registros
        public async Task<IActionResult> Index()
        {
            var contexto = _context.Registros.Include(r => r.Estacionamento).Include(r => r.Funcionario).Include(r => r.Veiculo);
            return View(await contexto.ToListAsync());
        }

        // GET: Registros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Registros == null)
            {
                return NotFound();
            }

            var registro = await _context.Registros
                .Include(r => r.Estacionamento)
                .Include(r => r.Funcionario)
                .Include(r => r.Veiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registro == null)
            {
                return NotFound();
            }

            return View(registro);
        }

        // GET: Registros/Create
        public IActionResult Create()
        {
            ViewData["EstacionamentoId"] = new SelectList(_context.Estacionamentos, "Id", "Nome");
            ViewData["FuncionarioId"] = new SelectList(_context.Funcionarios, "Id", "Cpf");
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Id");
            return View();
        }

        // POST: Registros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Entrada,Saida,ValorTotal,Pago,VeiculoId,FuncionarioId,EstacionamentoId")] Registro registro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstacionamentoId"] = new SelectList(_context.Estacionamentos, "Id", "Nome", registro.EstacionamentoId);
            ViewData["FuncionarioId"] = new SelectList(_context.Funcionarios, "Id", "Cpf", registro.FuncionarioId);
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Id", registro.VeiculoId);
            return View(registro);
        }

        // GET: Registros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Registros == null)
            {
                return NotFound();
            }

            var registro = await _context.Registros.FindAsync(id);
            if (registro == null)
            {
                return NotFound();
            }
            ViewData["EstacionamentoId"] = new SelectList(_context.Estacionamentos, "Id", "Nome", registro.EstacionamentoId);
            ViewData["FuncionarioId"] = new SelectList(_context.Funcionarios, "Id", "Cpf", registro.FuncionarioId);
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Id", registro.VeiculoId);
            return View(registro);
        }

        // POST: Registros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Entrada,Saida,ValorTotal,Pago,VeiculoId,FuncionarioId,EstacionamentoId")] Registro registro)
        {
            if (id != registro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistroExists(registro.Id))
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
            ViewData["EstacionamentoId"] = new SelectList(_context.Estacionamentos, "Id", "Nome", registro.EstacionamentoId);
            ViewData["FuncionarioId"] = new SelectList(_context.Funcionarios, "Id", "Cpf", registro.FuncionarioId);
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Id", registro.VeiculoId);
            return View(registro);
        }

        // GET: Registros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Registros == null)
            {
                return NotFound();
            }

            var registro = await _context.Registros
                .Include(r => r.Estacionamento)
                .Include(r => r.Funcionario)
                .Include(r => r.Veiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registro == null)
            {
                return NotFound();
            }

            return View(registro);
        }

        // POST: Registros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Registros == null)
            {
                return Problem("Entity set 'Contexto.Registros'  is null.");
            }
            var registro = await _context.Registros.FindAsync(id);
            if (registro != null)
            {
                _context.Registros.Remove(registro);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistroExists(int id)
        {
          return _context.Registros.Any(e => e.Id == id);
        }
    }
}

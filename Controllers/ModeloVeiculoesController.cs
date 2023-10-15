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
    public class ModeloVeiculoesController : Controller
    {
        private readonly Contexto _context;

        public ModeloVeiculoesController(Contexto context)
        {
            _context = context;
        }

        // GET: ModeloVeiculoes
        public async Task<IActionResult> Index()
        {
              return View(await _context.ModelosVeiculo.ToListAsync());
        }

        // GET: ModeloVeiculoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ModelosVeiculo == null)
            {
                return NotFound();
            }

            var modeloVeiculo = await _context.ModelosVeiculo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modeloVeiculo == null)
            {
                return NotFound();
            }

            return View(modeloVeiculo);
        }

        // GET: ModeloVeiculoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ModeloVeiculoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao,Marca,Tipo")] ModeloVeiculo modeloVeiculo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modeloVeiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(modeloVeiculo);
        }

        // GET: ModeloVeiculoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ModelosVeiculo == null)
            {
                return NotFound();
            }

            var modeloVeiculo = await _context.ModelosVeiculo.FindAsync(id);
            if (modeloVeiculo == null)
            {
                return NotFound();
            }
            return View(modeloVeiculo);
        }

        // POST: ModeloVeiculoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,Marca,Tipo")] ModeloVeiculo modeloVeiculo)
        {
            if (id != modeloVeiculo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modeloVeiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModeloVeiculoExists(modeloVeiculo.Id))
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
            return View(modeloVeiculo);
        }

        // GET: ModeloVeiculoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ModelosVeiculo == null)
            {
                return NotFound();
            }

            var modeloVeiculo = await _context.ModelosVeiculo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modeloVeiculo == null)
            {
                return NotFound();
            }

            return View(modeloVeiculo);
        }

        // POST: ModeloVeiculoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ModelosVeiculo == null)
            {
                return Problem("Entity set 'Contexto.ModelosVeiculo'  is null.");
            }
            var modeloVeiculo = await _context.ModelosVeiculo.FindAsync(id);
            if (modeloVeiculo != null)
            {
                _context.ModelosVeiculo.Remove(modeloVeiculo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModeloVeiculoExists(int id)
        {
          return _context.ModelosVeiculo.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Estacionamento.Models;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol;

namespace Estacionamento.Controllers
{
    [Authorize]
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
                .OrderBy(r => r.Entrada)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registro == null)
            {
                return NotFound();
            }

            return View(registro);
        }

        // GET: Registros/Entrada
        public IActionResult Entrada()
        {
            ViewData["EstacionamentoId"] = new SelectList(_context.Estacionamentos, "Id", "Nome");
            ViewData["FuncionarioId"] = new SelectList(_context.Funcionarios, "Id", "Nome");
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Placa");
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult VagasInsuficientes()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: Registros/Entrada
        public async Task<IActionResult> Entrada([Bind("Id,Entrada,VeiculoId,FuncionarioId,EstacionamentoId")] Registro registro)
        {
            Veiculo veiculo = new Veiculo();

            var contexto = _context.Veiculos.Include(r => r.Modelo);
            var innerContexto = _context.Estacionamentos;

            var conReg = _context.Registros.Include(r => r.Estacionamento).Include(r => r.Funcionario).Include(r => r.Veiculo).Include(r => r.Veiculo.Modelo);

            foreach (Veiculo v in contexto)
            {
                if (v.Id == registro.VeiculoId)
                {
                    veiculo = v;
                    break;
                }
            }

            int vagas = 0;

            EstacionamentoModel estacionamento = registro.Estacionamento;

            switch (veiculo.Modelo.Tipo)
            {
                case TipoVeiculo.P:
                    vagas = innerContexto.Find(registro.EstacionamentoId).QtdeVagasP - 1;
                    if (vagas <= 0)
                    {
                        return VagasInsuficientes();
                    }
                    estacionamento.QtdeVagasP = vagas;
                    break;
                case TipoVeiculo.M:
                    vagas = innerContexto.Find(registro.EstacionamentoId).QtdeVagasM - 1;
                    if (vagas <= 0)
                    {
                        return VagasInsuficientes();
                    }
                    estacionamento.QtdeVagasM = vagas;
                    break;
                case TipoVeiculo.G:
                    vagas = innerContexto.Find(registro.EstacionamentoId).QtdeVagasG - 1;
                    if (vagas <= 0)
                    {
                        return VagasInsuficientes();
                    }
                    estacionamento.QtdeVagasG = vagas;
                    break;
            }

            _context.Update(estacionamento);

            if (ModelState.IsValid)
            {
                _context.Add(registro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstacionamentoId"] = new SelectList(_context.Estacionamentos, "Id", "Nome", registro.EstacionamentoId);
            ViewData["FuncionarioId"] = new SelectList(_context.Funcionarios, "Id", "Cpf", registro.FuncionarioId);
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Placa", registro.VeiculoId);
            return View(registro);
        }

        // GET: Registros/Saida
        public IActionResult Saida()
        {
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Placa");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: Registros/Saida
        public async Task<IActionResult> Saida([Bind("Saida,VeiculoId")] Registro registro)
        {
            Registro saida = new Registro();

            var contexto = _context.Registros.Include(r => r.Estacionamento).Include(r => r.Funcionario).Include(r => r.Veiculo).Include(r => r.Veiculo.Modelo);

            foreach (Registro r in contexto)
            {
                if (r.VeiculoId == registro.VeiculoId && !r.Pago)
                {
                    saida = r;
                    break;
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(saida.Veiculo == null)
                    {
                        return NotFound();
                    }
                    
                    double valor = 0;

                    switch (saida.Veiculo.Modelo.Tipo)
                    {
                        case TipoVeiculo.P:
                            valor = saida.Estacionamento.Tarifa + saida.Estacionamento.ValorHora * (registro.Saida - saida.Entrada).TotalHours;
                            break;
                        case TipoVeiculo.M:
                            valor = saida.Estacionamento.Tarifa + (saida.Estacionamento.ValorHora + saida.Estacionamento.ValorHora * 0.25) * (registro.Saida - saida.Entrada).TotalHours;
                            break;
                        case TipoVeiculo.G:
                            valor = saida.Estacionamento.Tarifa + (saida.Estacionamento.ValorHora + saida.Estacionamento.ValorHora * 0.75) * (registro.Saida - saida.Entrada).TotalHours;
                            break;
                    }

                    int vagas = 0;

                    EstacionamentoModel estacionamento = registro.Estacionamento;
                    Veiculo veiculo = new Veiculo();
                    var innerContexto = _context.Estacionamentos;
                    var veiculos = _context.Veiculos;

                    foreach (Veiculo v in veiculos)
                    {
                        if (v.Id == registro.VeiculoId)
                        {
                            veiculo = v;
                            break;
                        }
                    }

                    switch (veiculo.Modelo.Tipo)
                    {
                        case TipoVeiculo.P:
                            vagas = innerContexto.Find(registro.EstacionamentoId).QtdeVagasP + 1;
                            estacionamento.QtdeVagasP = vagas;
                            break;
                        case TipoVeiculo.M:
                            vagas = innerContexto.Find(registro.EstacionamentoId).QtdeVagasM + 1;
                            estacionamento.QtdeVagasM = vagas;
                            break;
                        case TipoVeiculo.G:
                            vagas = innerContexto.Find(registro.EstacionamentoId).QtdeVagasG + 1;
                            estacionamento.QtdeVagasG = vagas;
                            break;
                    }

                    _context.Update(estacionamento);

                    saida.Saida = registro.Saida;
                    saida.ValorTotal = valor;
                    saida.Pago = false;
                    _context.Update(saida);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistroExists(saida.Id))
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
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Placa", saida.VeiculoId);
            return View(saida);
        }

        // GET: Registros/Pagamento
        public IActionResult Pagamento()
        {
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Placa");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: Registros/Pagamento
        public async Task<IActionResult> Pagamento([Bind("VeiculoId")] Registro registro)
        {
            Registro saida = new Registro();

            var contexto = _context.Registros.Include(r => r.Estacionamento).Include(r => r.Funcionario).Include(r => r.Veiculo);

            foreach (Registro r in contexto)
            {
                if (r.VeiculoId == registro.VeiculoId && !r.Pago)
                {
                    saida = r;
                    break;
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (saida.Veiculo == null)
                    {
                        return NotFound();
                    }

                    saida.Pago = true;
                    _context.Update(saida);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistroExists(saida.Id))
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
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Placa", saida.VeiculoId);
            return View(saida);
        }

        // GET: Registros/Create
        public IActionResult Create()
        {
            ViewData["EstacionamentoId"] = new SelectList(_context.Estacionamentos, "Id", "Nome");
            ViewData["FuncionarioId"] = new SelectList(_context.Funcionarios, "Id", "Cpf");
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Placa");
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
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Placa", registro.VeiculoId);
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
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Placa", registro.VeiculoId);
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
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Placa", registro.VeiculoId);
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

        public async Task<IActionResult> Buscar(string filtro)
        {
            List<Registro> lista = new List<Registro>();
            lista = _context.Registros.Where(r => r.Veiculo.Placa == filtro).ToList();
            return View(lista);
        }
        private bool RegistroExists(int id)
        {
          return _context.Registros.Any(e => e.Id == id);
        }
    }
}

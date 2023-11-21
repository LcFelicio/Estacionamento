using Estacionamento.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Estacionamento.Controllers
{
    [Authorize]
    public class GerarDados : Controller
    {

        private readonly Contexto _context;

        public GerarDados(Contexto context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            _context.Database.ExecuteSqlRaw("DELETE * from Clientes");
            _context.Database.ExecuteSqlRaw("Estacionamento CHECKIDENT('Clientes', RESEED, 0)");

            _context.Database.ExecuteSqlRaw("DELETE * from Veiculos");
            _context.Database.ExecuteSqlRaw("Estacionamento CHECKIDENT('Veiculos', RESEED, 0)");

            _context.Database.ExecuteSqlRaw("DELETE * from ModelosVeiculo");
            _context.Database.ExecuteSqlRaw("Estacionamento CHECKIDENT('ModelosVeiculo', RESEED, 0)");



            return View();
        }
    }
}

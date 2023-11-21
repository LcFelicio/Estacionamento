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

            _context.Database.ExecuteSqlRaw("DELETE from Clientes");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Clientes', RESEED, 0)");

            _context.Database.ExecuteSqlRaw("DELETE from Veiculos");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Veiculos', RESEED, 0)");

            _context.Database.ExecuteSqlRaw("DELETE from ModelosVeiculo");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('ModelosVeiculo', RESEED, 0)");

            _context.Database.ExecuteSqlRaw("DELETE from Funcionarios");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Funcionarios', RESEED, 0)");

            _context.Database.ExecuteSqlRaw("DELETE from Estacionamentos");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Estacionamentos', RESEED, 0)");

            string[] nomes = {
                "Ana", "Carlos", "Beatriz", "Daniel", "Eva",
                "Fernando", "Giovana", "Hugo", "Isabel", "José",
                "Laura", "Miguel", "Catarina", "Pedro", "Rafael",
                "Clara", "Lucas", "Juliana", "Antônio", "Larissa",
                "Rodrigo", "Manuela", "Gustavo", "Camila", "Gabriel",
                "Amanda", "Diego", "Valentina", "Vinícius", "Carolina",
                "Felipe", "Luana", "Ricardo", "Mariana", "Paulo",
                "Isabela", "Rafaela", "Júlio", "Thaís", "Fábio",
                "Alice", "Vitor", "Natalia", "Anderson", "Lais",
                "Marcos", "Vanessa", "Roberto", "Eduarda"
            };

            string[] carros = {
                "Toyota Corolla", "Honda Civic", "Ford Mustang", "Chevrolet Camaro", "Volkswagen Golf",
                "Tesla Model 3", "Nissan Altima", "BMW M3", "Mercedes-Benz C-Class", "Audi A4",
                "Jeep Wrangler", "Subaru Outback", "Mazda CX-5", "Hyundai Tucson", "Kia Sportage",
                "Lexus RX", "Acura MDX", "Ford Explorer", "Chevrolet Tahoe", "Dodge Durango",
                "Porsche 911", "Jaguar F-Type", "Ferrari 488", "Lamborghini Huracan", "McLaren 720S",
                "Toyota RAV4", "Honda CR-V", "Ford Escape", "Chevrolet Equinox", "Volkswagen Tiguan",
                "Tesla Model X", "Nissan Rogue", "BMW X5", "Mercedes-Benz GLE", "Audi Q7",
                "Jeep Grand Cherokee", "Subaru Forester", "Mazda CX-9", "Hyundai Santa Fe", "Kia Sorento",
                "Lexus NX", "Acura RDX", "Ford Edge", "Chevrolet Traverse", "Dodge Journey",
                "Porsche Cayenne", "Jaguar I-PACE", "Ferrari Portofino", "Lamborghini Aventador", "McLaren GT"
            };

            string[] placas = {
                "ABC1D23", "XYZ4E56", "LMN7F89", "OPQ0G12", "RST3H45",
                "UVW6I78", "JKL9M01", "DEF2N34", "GHI5O67", "PQR8S90",
                "ABC1D23", "XYZ4E56", "LMN7F89", "OPQ0G12", "RST3H45",
                "UVW6I78", "JKL9M01", "DEF2N34", "GHI5O67", "PQR8S90",
                "ABC1D23", "XYZ4E56", "LMN7F89", "OPQ0G12", "RST3H45",
                "UVW6I78", "JKL9M01", "DEF2N34", "GHI5O67", "PQR8S90",
                "ABC1D23", "XYZ4E56", "LMN7F89", "OPQ0G12", "RST3H45",
                "UVW6I78", "JKL9M01", "DEF2N34", "GHI5O67", "PQR8S90",
                "ABC1D23", "XYZ4E56", "LMN7F89", "OPQ0G12", "RST3H45",
                "UVW6I78", "JKL9M01", "DEF2N34", "GHI5O67", "PQR8S90"
            };

            string[] cidades = {
                "São Paulo", "Guarulhos", "Campinas", "São Bernardo do Campo", "Santo André", "São José dos Campos", "Osasco", "Ribeirão Preto", "Sorocaba", "Mauá", "São José do Rio Preto", "Mogi das Cruzes", "Diadema", "Jundiaí", "Piracicaba"
            };

            string[] enderecos = {
                "Rua A, 123", "Av. B, 456", "Pç. C, 789", "Al. D, 101", "R. E, 202",
                "Av. F, 303", "Pç. G, 404", "Al. H, 505", "R. I, 606", "Av. J, 707",
                "Pç. K, 808", "Al. L, 909", "R. M, 1010", "Av. N, 1111", "Pç. O, 1212"
            };

            string[] cpfs = {
                "123.456.789-09", "987.654.321-98", "456.789.012-34", "876.543.210-87", "234.567.890-12",
                "567.890.123-45", "890.123.456-78", "012.345.678-90", "345.678.901-23", "789.012.345-67",
                "901.234.567-89", "234.567.890-12", "678.901.234-56", "012.345.678-90", "345.678.901-23"
            };

            for (int i = 0; i < 2; i++)
            {
                Random rnd = new Random();

                long r = rnd.NextInt64() % nomes.Length;

                Funcionario funcionario = new Funcionario()
                {
                    Nome = nomes[r],
                    Cpf = cpfs[rnd.NextInt64() % cpfs.Length],
                    Cidade = cidades[rnd.NextInt64() % cidades.Length],
                    Estado = EstadoEnum.SP,
                    Endereco = enderecos[rnd.NextInt64() % enderecos.Length],
                    Funcao = Funcao.MANOBRISTA
                };

                _context.Add(funcionario);
                _context.SaveChanges();

            }

            for (int i = 0; i < 10; i++)
            {
                Random rnd = new Random();

                long r = rnd.NextInt64() % nomes.Length;

                Cliente cliente = new Cliente()
                {
                    Nome = nomes[r],
                    Cpf = cpfs[rnd.NextInt64() % cpfs.Length],
                    Cidade = cidades[rnd.NextInt64() % cidades.Length],
                    Estado = EstadoEnum.SP,
                    Endereco = enderecos[rnd.NextInt64() % enderecos.Length],
                    Email = nomes[r] + "@email.com"
                };

                _context.Add(cliente);
                _context.SaveChanges();

            }

            for (int i = 0; i < 10; i++)
            {
                Random rnd = new Random();

                long r = rnd.NextInt64() % nomes.Length;

                ModeloVeiculo modelo = new ModeloVeiculo()
                {
                    Descricao = carros[r].Split(" ")[1],
                    Marca = carros[r].Split(" ")[0],
                    Tipo = TipoVeiculo.P
                };

                _context.Add(modelo);
                _context.SaveChanges();

            }

            for (int i = 1; i < 6; i++)
            {
                Random rnd = new Random();

                Veiculo veiculo = new Veiculo()
                {
                    Placa = placas[rnd.NextInt64() % placas.Length],
                    cor = "Prata",
                    ModeloId = i,
                    ClienteId = i,
                };

                _context.Add(veiculo);
                _context.SaveChanges();

            }

            return View();
        }
    }
}

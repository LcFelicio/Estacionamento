using Microsoft.EntityFrameworkCore;


namespace Estacionamento.Models
{
    public class Contexto: DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) { }

        public DbSet<ModeloVeiculo> ModelosVeiculo { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<EstacionamentoModel> Estacionamentos { get; set; }
        public DbSet<Registro> Registros { get; set; }
    }
}

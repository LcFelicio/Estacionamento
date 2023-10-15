using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Estacionamento.Models
{
    public class Registro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id: ")]
        public int Id { get; set; }

        [Display(Name = "Entrada: ")]
        public DateTime Entrada { get; set; }

        [Display(Name = "Saida: ")]
        public DateTime Saida { get; set; }

        [Display(Name = "Valor Total: ")]
        public Double ValorTotal { get; set; }

        [Display(Name = "Pago")]
        public Boolean Pago { get; set; }

        [ForeignKey("VeiculoId")]
        [Display(Name = "Veiculo: ")]
        public int VeiculoId { get; set; }
        public Veiculo Veiculo { get; set; }

        [ForeignKey("FuncionarioId")]
        [Display(Name = "Funcionario: ")]
        public int FuncionarioId { get; set; }
        public Funcionario Funcionario { get; set; }

        [ForeignKey("EstacionamentoId")]
        [Display(Name = "Estacionamento: ")]
        public int EstacionamentoId { get; set; }
        public EstacionamentoModel Estacionamento { get; set; }

    }
}

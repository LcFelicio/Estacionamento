using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Estacionamento.Models
{

    public enum EstadoEnum { AC, AL, AP, AM, BA, CE, DF, ES, GO, MA, MT, MS, MG, PA, PB, PR, PE, PI, RJ, RN, RS, RO, RR, SC, SP, SE, TO }

    public class Estacionamento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID: ")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Nome é Obrigatório")]
        [Display(Name = "Nome: ")]
        public string Nome { get; set; }

        [Display(Name = "Endereço: ")]
        public string Endereco { get; set; }

        [Display(Name = "Cidade: ")]
        public String Cidade { get; set; }

        [Display(Name = "Estado: ")]
        public EstadoEnum Estado { get; set; }

        [Display(Name = "Quantidade Vagas P: ")]
        public int QtdeVagasP { get; set; }

        [Display(Name = "Quantidade Vagas M: ")]
        public int QtdeVagasM { get; set; }

        [Display(Name = "Quantidade Vagas G: ")]
        public int QtdeVagasG { get; set; }

        [Required(ErrorMessage = "Campo Tarifa é Obrigatório")]
        [Display(Name = "Tarifa: ")]
        public double Tarifa { get; set; }

        [Required(ErrorMessage = "Campo Valor p/Hora é Obrigatório")]
        [Display(Name = "Valor p/Hora: ")]
        public double ValorHora { get; set; }

    }
}

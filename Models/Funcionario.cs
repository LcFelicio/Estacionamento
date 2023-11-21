using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Estacionamento.Models
{

    public enum Funcao { MANOBRISTA, SEGURANCA, PORTEIRO, ADMINISTRADOR }

    public class Funcionario
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

        [Required(ErrorMessage = "Campo CPF é Obrigatório")]
        [Display(Name = "CPF: ")]
        public String Cpf { get; set; }

        [Required(ErrorMessage = "Campo Função é Obrigatório")]
        [Display(Name = "Função: ")]
        public Funcao Funcao { get; set; }
    }
}

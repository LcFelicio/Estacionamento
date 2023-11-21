using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Estacionamento.Models
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id: ")]
        public int Id { get; set; }

        [Display(Name = "Nome: ")]
        public string Nome { get; set; }

        [Display(Name = "Endereço: ")]
        public string Endereco { get; set; }

        [Display(Name = "Cidade: ")]
        public String Cidade { get; set; }

        [Display(Name = "Estado: ")]
        public EstadoEnum Estado { get; set; }

        [Display(Name = "Email: ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo CPF é obrigatório!")]
        [Display(Name = "CPF: ")]
        public string Cpf { get; set; }
    }
}

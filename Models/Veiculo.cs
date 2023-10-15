using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Estacionamento.Models
{
    public class Veiculo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id: ")]
        public int Id { get; set; }

        [Display(Name = "Placa: ")]
        public string Placa { get; set; }

        [Display(Name = "Cor: ")]
        public string cor { get; set; }
        
        [ForeignKey]
        [Display(Name = "Modelo: ")]
        public string Modelo { get; set; }
        
        [ForeignKey]
        [Display(Name = "Cliente: ")]
        public int Cliente { get; set; }
    }
}

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
        
        [ForeignKey("ModeloId")]
        [Display(Name = "Modelo: ")]
        public int ModeloId { get; set; }
        public ModeloVeiculo Modelo { get; set; }
        
        [ForeignKey("ClienteId")]
        [Display(Name = "Cliente: ")]
        public int ClienteId { get; set; }
        public Cliente cliente { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Estacionamento.Models
{
    public enum TipoVeiculo { P, M, G }
    public class ModeloVeiculo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id: ")]
        public int Id { get; set; }
        
        [Display(Name = "Descricao: ")]
        public string Descricao { get; set; }

        [Display(Name = "Marca: ")]
        public string Marca { get; set; }

        [Display(Name = "Tipo: ")]
        public TipoVeiculo Tipo {  get; set; }
    }
}

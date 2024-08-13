
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TalentConnect.Models
{
    public class Experiencia
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string Cargo { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string Empresa { get; set; }

        //[Required(ErrorMessage = "Campo obrigatório")]
        //public string Area { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public DateTime? DataFim { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string Descricao { get; set; }

        public int CandidatoID { get; set; }
        [ForeignKey("CandidatoID")]
        public Candidato Candidato { get; set; }
    }
}

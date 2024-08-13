
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TalentConnect.Models
{
    public class Formacao
    {
        [Key]
        public int Id { get; set; }
        public string Instituicao { get; set; }
        public string Grau { get; set; }
        public string CampoEstudo { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public bool? Cursando { get; set; }
        public int CandidatoID { get; set; }

        [ForeignKey("CandidatoID")]
        public Candidato Candidato { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TalentConnect.Models
{
    public class Portfolio
    {
        [Key]
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Link { get; set; }
        public DateTime Data { get; set; }

        // Chave estrangeira
        public int CandidatoID { get; set; }
        [ForeignKey("CandidatoID")]
        public Candidato Candidato { get; set; }
    }
}

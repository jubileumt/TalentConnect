using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TalentConnect.Models
{
    [Table("Candidato")]
    public class Candidato : Usuario
    {
        [Required(ErrorMessage = "O campo CPF é obrigatório.")]
        [Display(Name = "CPF")]
        [Column("CPF")]
        public string CPF { get; set; }
        public string ResumoProfissional { get; set; }
        public List<string> Habilidades { get; set; }
        public bool Disponibilidade { get; set; }
        public decimal? PretensaoSalarial { get; set; }
        public virtual ICollection<Experiencia>? Experiencias { get; set; }
        public virtual ICollection<Formacao>? Formacoes { get; set; }
        public virtual ICollection<Portfolio>? Portfolios { get; set; }
    }

}

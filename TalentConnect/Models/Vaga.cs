using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TalentConnect.Models
{
    public class Vaga
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(100)]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(500)]
        public string Descricao { get; set; } = string.Empty;

        [StringLength(500)]
        public string Requisitos { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data de publicação é obrigatória.")]
        public DateTime DataPublicacao { get; set; }

        public DateTime? DataEncerramento { get; set; }

        [StringLength(100)]
        public string Localizacao { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Salario { get; set; }
    }
}


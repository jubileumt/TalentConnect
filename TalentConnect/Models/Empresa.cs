using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TalentConnect.Models
{
    public class Empresa : Usuario
    {
        
        [Required(ErrorMessage = "O campo CNPJ é obrigatório.")]
        public string CNPJ { get; set; }

        [Display(Name = "Descrição da Empresa")]
        [Column("Descricao")]
        public string DescricaoEmpresa { get; set; }

        //public List<Vaga> CargosDisponiveis { get; set; }
        public string Localizacao { get; set; }
        public string Site { get; set; }
        public string TamanhoEmpresa { get; set; }

    }
}

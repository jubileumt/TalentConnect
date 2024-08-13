using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TalentConnect.Models
{
    public abstract class Usuario
    {

        [Column("ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome Precisa ser informado")]
        [MaxLength(70, ErrorMessage = "Nome pode ter no maximo 70 caracteres")]
        [Display(Name = "Nome")]
        [Column("Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Nome Precisa ser informado")]
        [MaxLength(70, ErrorMessage = "Nome pode ter no maximo 70 caracteres")]
        [Display(Name = "Email")]
        [Column("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha precisa ser informado")]
        [MinLength(8, ErrorMessage = "Senha precisa ter no minimo 8 carateres")]
        [Display(Name = "Senha")]
        [Column("Senha")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
        public DateTime DataCadastro { get; set; }

        [Required(ErrorMessage = "Telefone precisa ser informado")]
        [MaxLength(11, ErrorMessage = "Telefone precisa ter o DDD e o numero")]
        [Display(Name = "Telefone")]
        [Column("Telefone")]
        public string Telefone { get; set; }

        //[Required(ErrorMessage = "O campo CEP é obrigatório.")]
        [Display(Name = "CEP")]
        [Column("CEP")]
        public string? CEP { get; set; }

        [Required(ErrorMessage = "O campo Bairro é obrigatório.")]
        [Display(Name = "Bairro")]
        [Column("Bairro")]
        public string? Bairro { get; set; }

        //[Required(ErrorMessage = "O campo Cidade é obrigatório.")]
        [Display(Name = "Cidade")]
        [Column("Cidade")]
        public string? Cidade { get; set; }

        //[Required(ErrorMessage = "O campo Estado é obrigatório.")]
        [Display(Name = "Estado")]
        [Column("Estado")]
        public string? Estado { get; set; }

        [Required(ErrorMessage = "O campo Endereço é obrigatório.")]
        [Display(Name = "Endereço")]
        [Column("Endereco")]
        public string Endereco { get; set; }
    }
}

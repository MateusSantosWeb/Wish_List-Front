using System.ComponentModel.DataAnnotations;

namespace WishListWeb.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [MaxLength(100)]
        public string Nome { get; set; }
        
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [MaxLength(100)]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Senha é obrigatória")]
        [MinLength(6, ErrorMessage = "Senha deve ter no mínimo 6 caracteres")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
        
        [Required(ErrorMessage = "Confirme a senha")]
        [Compare("Senha", ErrorMessage = "Senhas não conferem")]
        [DataType(DataType.Password)]
        public string ConfirmarSenha { get; set; }
        
        [Required(ErrorMessage = "Selecione o tipo de usuário")]
        public string Role { get; set; }

        [Display(Name = "Código da Namorada")]
        public string? CodigoNamorada { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace prj_chamadosBRA.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha atual")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0} tem que ter no minimo {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme a nova senha")]
        [Compare("NewPassword", ErrorMessage = "A nova senha e a confirmação da nova senha não batem.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }
        
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Lembrar do acesso?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0} tem que ter no minimo {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmação de Senha")]
        [Compare("Password", ErrorMessage = "A senha e a confirmação da senha não batem.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "PerfilUsuario")]
        public int perfil { get; set; }

        [Required]
        [Display(Name = "Nome do Usuario")]
        public string Nome { get; set; }

        [Required]
        [Display(Name = "Contato do Usuario")]
        public string Contato { get; set; }
        
        [Display(Name = "Chapa do Usuario")]
        public string Chapa { get; set; }
    }

    public class RecoveryInitalViewModel
    {

        [Required]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

    }

    public class RecoveryViewModel
    {
        [Required]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0} tem que ter no minimo {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmação de Senha")]
        [Compare("Password", ErrorMessage = "A senha e a confirmação da senha não batem.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Nome do Usuario")]
        public string Nome { get; set; }

    }
}

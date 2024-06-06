using System.ComponentModel.DataAnnotations;

namespace Lw.FchStore.Iam.Api.ViewModels;

public class LoginViewModel
{
    [Required]
    [EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}

public class RegisterViewModel
{
    [Required]
    public string Fullname { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}

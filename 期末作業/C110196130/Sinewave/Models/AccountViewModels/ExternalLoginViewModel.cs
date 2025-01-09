using System.ComponentModel.DataAnnotations;

namespace Sinewave.Models.AccountViewModels;

public class ExternalLoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
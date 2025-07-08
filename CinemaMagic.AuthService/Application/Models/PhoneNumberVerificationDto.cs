using System.ComponentModel.DataAnnotations;

namespace CinemaMagic.AuthService.Models;

public class PhoneNumberVerificationDto
{
    public string PhoneNumber { get; set; } = string.Empty;
    [Required]
    [StringLength(5, MinimumLength = 5)]
    public string Code { get; set; } = string.Empty;
}
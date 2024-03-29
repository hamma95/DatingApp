using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    public string Mail { get; set; }

    [Required]
    public string Password { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace HouseRules.Models;

public class Registration
{
    [Required]
    [EmailAddress]
    [MaxLength(50, ErrorMessage = "Email addresses must be 50 characters or less")]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Address { get; set; }

}
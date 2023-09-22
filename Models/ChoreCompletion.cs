using System.ComponentModel.DataAnnotations;

namespace HouseRules.Models;

public class ChoreCompletion
{
    public int Id { get; set; }
    public int UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; }
    public int ChoreId { get; set; }
    public Chore Chore { get; set; }
    [Required]
    public DateTime CompletedOn { get; set; }
}
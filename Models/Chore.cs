using System.ComponentModel.DataAnnotations;

namespace HouseRules.Models;

public class Chore
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100, ErrorMessage = "Chore names must be 100 characters or less")]
    public string Name { get; set; }
    [Range(1, 5, ErrorMessage = "Chore difficulty must be a number between 1 and 5")]
    public int Difficulty { get; set; }
    [Range(1, 14, ErrorMessage = "Chore frequency must be a number between 1 and 14")]
    public int ChoreFrequencyDays { get; set; }
    public List<ChoreAssignment>? ChoreAssignments { get; set; }
    public List<ChoreCompletion>? ChoreCompletions { get; set; }
    public bool OverdueChore
    {
        get
        {
            // check to see if there are no ChoreCompletions
            if (ChoreCompletions == null || ChoreCompletions.Count == 0)
            {
                return true;
            }
            // get the most recent chore completion date
            DateTime MostRecentCompletionDate = ChoreCompletions.Max(cc => cc.CompletedOn);

            // if most recent completion date + chore frequency date < today, return true
            if (MostRecentCompletionDate.AddDays(ChoreFrequencyDays) < DateTime.Now)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
using HouseRules.Data;
using HouseRules.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseRules.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChoreController : ControllerBase
{
    private HouseRulesDbContext _dbContext;

    public ChoreController(HouseRulesDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet] // route /api/chore
    [Authorize] // get all chores
    public IActionResult Get()
    {
        return Ok(_dbContext.Chores
            .Include(c => c.ChoreAssignments)
            .ThenInclude(ca => ca.UserProfile)
            .Include(c => c.ChoreCompletions)
            .ThenInclude(cc => cc.UserProfile)
            .ToList());
    }

    [HttpGet("{id}")] // route /api/chore/{id}
    [Authorize] // return chore by id with current assignees and all completions
    public IActionResult GetChoreById(int id)
    {
        Chore chore = _dbContext.Chores
        .Include(c => c.ChoreAssignments)
        .ThenInclude(ca => ca.UserProfile)
        .Include(c => c.ChoreCompletions)
        .ThenInclude(cc => cc.UserProfile)
        .SingleOrDefault(c => c.Id == id);

        if (chore == null)
        {
            return NotFound();
        }

        return Ok(chore);
    }

    [HttpPost("{id}/complete")] // route /api/chore/{id}
    [Authorize] // complete a chore
    public IActionResult CompleteChore(int id, int userId)
    {
        Chore choreToComplete = _dbContext.Chores
        .Include(c => c.ChoreAssignments)
        .ThenInclude(ca => ca.UserProfile)
        .SingleOrDefault(c => c.Id == id);

        if (choreToComplete == null)
        {
            return NotFound();
        }
        else if (id != choreToComplete.Id)
        {
            return BadRequest();
        }

        _dbContext.ChoreCompletions.Add(new ChoreCompletion
        {
            UserProfileId = userId,
            ChoreId = id,
            CompletedOn = DateTime.Now
        });

        _dbContext.SaveChanges();

        return NoContent();
    }

    [HttpPost] // /api/chore/
    [Authorize(Roles = "Admin")] // admins only, post a new chore 
    public IActionResult CreateChore(Chore chore)
    {
        _dbContext.Chores.Add(chore);
        _dbContext.SaveChanges();
        return Created($"/api/chore/{chore.Id}", chore);
    }

    [HttpPut("{id}")] // /api/chore/{id}
    [Authorize(Roles = "Admin")] // admins only, update a chore
    public IActionResult UpdateChore(int id, Chore updatedChore)
    {
        Chore chore = _dbContext.Chores.SingleOrDefault(c => c.Id == id);
        if (chore == null)
        {
            return NotFound();
        }
        else if (id != chore.Id)
        {
            return BadRequest();
        }
        chore.Name = updatedChore.Name;
        chore.Difficulty = updatedChore.Difficulty;
        chore.ChoreFrequencyDays = updatedChore.ChoreFrequencyDays;
        // chore.ChoreAssignments = updatedChore.ChoreAssignments;
        // chore.ChoreCompletions = updatedChore.ChoreCompletions;

        _dbContext.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // admins only, delete a chore
    public IActionResult DeleteChore(int id)
    {
        Chore choreToDelete = _dbContext.Chores.SingleOrDefault(c => c.Id == id);

        if (choreToDelete == null)
        {
            return NotFound();
        }

        _dbContext.Chores.Remove(choreToDelete);
        _dbContext.SaveChanges();
        return NoContent();
    }

    [HttpPost("{id}/assign")]
    [Authorize(Roles = "Admin")] // admins only, assign a chore to a user
    public IActionResult AssignChore(int id, int userId)
    {
        Chore choreToAssign = _dbContext.Chores.SingleOrDefault(c => c.Id == id);
        UserProfile userProfile = _dbContext.UserProfiles.SingleOrDefault(up => up.Id == userId);

        if (choreToAssign == null || userProfile == null)
        {
            return NotFound();
        }

        _dbContext.ChoreAssignments.Add(new ChoreAssignment
        {
            UserProfileId = userId,
            ChoreId = id
        });

        _dbContext.SaveChanges();

        return NoContent();
    }

    [HttpPost("{id}/unassign")]
    [Authorize(Roles = "Admin")] // admins only, assign a chore to a user
    public IActionResult UnssignChore(int id, int userId)
    {
        Chore choreToUnassign = _dbContext.Chores.SingleOrDefault(c => c.Id == id);
        UserProfile userProfile = _dbContext.UserProfiles.SingleOrDefault(up => up.Id == userId);

        ChoreAssignment choreAssignment = _dbContext.ChoreAssignments.SingleOrDefault(ca => ca.ChoreId == id && ca.UserProfileId == userId);

        if (choreToUnassign == null || userProfile == null || choreAssignment == null)
        {
            return NotFound();
        }

        _dbContext.ChoreAssignments.Remove(choreAssignment);
        _dbContext.SaveChanges();
        return NoContent();
    }

}
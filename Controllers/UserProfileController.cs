using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HouseRules.Data;
using HouseRules.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace HouseRules.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
    private HouseRulesDbContext _dbContext;

    public UserProfileController(HouseRulesDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet] // This method from the UserProfileController gets the data for all users.
    [Authorize] // This data should only be available to Admin users (it will be used to terminate employees, hire new ones, as well as upgrading an employee to be an Admin). [Authorize(Roles = "Admin")] ensures that this resource will only be accessible to authenticated users that also have the Admin role associated with their user id. A logged in user that is not an Admin will receive a 403 (Forbidden) response when trying to access this resource.
    public IActionResult Get()
    {
        return Ok(_dbContext.UserProfiles.ToList());
    }

    [HttpGet("withroles")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetWithRoles()
    {
        return Ok(_dbContext.UserProfiles
        .Include(up => up.IdentityUser)
        .Select(up => new UserProfile
        {
            Id = up.Id,
            FirstName = up.FirstName,
            LastName = up.LastName,
            Address = up.Address,
            Email = up.IdentityUser.Email,
            UserName = up.IdentityUser.UserName,
            IdentityUserId = up.IdentityUserId,
            Roles = _dbContext.UserRoles
            .Where(ur => ur.UserId == up.IdentityUserId)
            .Select(ur => _dbContext.Roles.SingleOrDefault(r => r.Id == ur.RoleId).Name)
            .ToList()
        }));
    }

    [HttpGet("{id}")]
    [Authorize]
    public IActionResult GetByIdWithChores(int id)
    {
        UserProfile userProfile = _dbContext.UserProfiles
        .Include(up => up.IdentityUser)
        .Include(up => up.ChoreAssignments)
        .ThenInclude(upca => upca.Chore)
        .Include(up => up.ChoreCompletions)
        .ThenInclude(upcc => upcc.Chore)
        .SingleOrDefault(up => up.Id == id);

        if (userProfile == null)
        {
            return NotFound();
        }

        return Ok(userProfile);
    }

    [HttpPost("promote/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Promote(string id)
    {
        IdentityRole role = _dbContext.Roles.SingleOrDefault(r => r.Name == "Admin");
        // This will create a new row in the many-to-many UserRoles table.
        _dbContext.UserRoles.Add(new IdentityUserRole<string>
        {
            RoleId = role.Id,
            UserId = id
        });
        _dbContext.SaveChanges();
        return NoContent();
    }

    [HttpPost("demote/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Demote(string id)
    {
        IdentityRole role = _dbContext.Roles
            .SingleOrDefault(r => r.Name == "Admin");
        IdentityUserRole<string> userRole = _dbContext
            .UserRoles
            .SingleOrDefault(ur =>
                ur.RoleId == role.Id &&
                ur.UserId == id);

        _dbContext.UserRoles.Remove(userRole);
        _dbContext.SaveChanges();
        return NoContent();
    }
}
using System.ComponentModel.DataAnnotations;

namespace TimesheetAPI.Models
{
    public record UserLogin([property: Required(ErrorMessage = "Username is required")] string Username,
        [property: Required(ErrorMessage = "Password is required")] string Password);
}

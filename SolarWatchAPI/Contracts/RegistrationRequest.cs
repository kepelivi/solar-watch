using System.ComponentModel.DataAnnotations;

namespace SolarWatchAPI.Contracts;

public record RegistrationRequest(
    [Required]string Email, 
    [Required]string Username, 
    [Required]string Password
);
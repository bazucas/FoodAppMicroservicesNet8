﻿using System.ComponentModel.DataAnnotations;

namespace Tasko.Web.Models;

public class RegistrationRequestDto
{
    [Required]
    public required string Email { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string PhoneNumber { get; set; }
    [Required]
    public required string Password { get; set; }
    public string? Role { get; set; }
}

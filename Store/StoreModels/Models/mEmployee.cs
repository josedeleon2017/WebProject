using System;
using System.Collections.Generic;

namespace StoreModels.Models;

public partial class mEmployee
{
    public int EmployeeId { get; set; }

    public int RoleId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string LoginId { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string NationalNumberId { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public string Gender { get; set; } = null!;

    public string? PhotoFileName { get; set; }

    public string? PhotoExtension { get; set; }

    public string? PhotoPath { get; set; }
}

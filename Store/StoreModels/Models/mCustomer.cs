using System;
using System.Collections.Generic;

namespace StoreModels.Models;

public partial class mCustomer
{
    public int CustomerId { get; set; }

    public int AddressId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public bool EmailPromotion { get; set; }
}

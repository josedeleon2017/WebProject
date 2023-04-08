using System;
using System.Collections.Generic;

namespace StoreModels.Models;

public partial class mAddress
{
    public int AddressId { get; set; }

    public int StateId { get; set; }

    public string AddressLine1 { get; set; } = null!;

    public string? AddressLine2 { get; set; }

    public string? PostalCode { get; set; }
}

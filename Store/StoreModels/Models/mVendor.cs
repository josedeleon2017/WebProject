using System;
using System.Collections.Generic;

namespace StoreModels.Models;

public partial class mVendor
{
    public int VendorId { get; set; }

    public string Name { get; set; } = null!;

    public string? AccountNumber { get; set; }

    public string? AccountType { get; set; }

    public bool PreferredVendorStatus { get; set; }

    public bool ActiveFlag { get; set; }

    public string? PurchasingWebServicesUrl { get; set; }
}

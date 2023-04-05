using System;
using System.Collections.Generic;

namespace StoreAPI.Models;

public partial class Vendor
{
    public int VendorId { get; set; }

    public string Name { get; set; } = null!;

    public string? AccountNumber { get; set; }

    public string? AccountType { get; set; }

    public bool PreferredVendorStatus { get; set; }

    public bool ActiveFlag { get; set; }

    public string? PurchasingWebServicesUrl { get; set; }

    public virtual ICollection<ProductVendor> ProductVendors { get; } = new List<ProductVendor>();
}

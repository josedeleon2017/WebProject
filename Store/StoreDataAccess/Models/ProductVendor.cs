using System;
using System.Collections.Generic;

namespace StoreAPI.Models;

public partial class ProductVendor
{
    public int VendorId { get; set; }

    public int ProductId { get; set; }

    public int AverageLeadTime { get; set; }

    public decimal StandarPrice { get; set; }

    public decimal? LastReceiptCost { get; set; }

    public DateTime? LastReceiptDate { get; set; }

    public int MinOrderQty { get; set; }

    public int MaxOrderQty { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Vendor Vendor { get; set; } = null!;
}

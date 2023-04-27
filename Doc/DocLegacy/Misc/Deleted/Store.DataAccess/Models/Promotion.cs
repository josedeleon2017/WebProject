using System;
using System.Collections.Generic;

namespace StoreDataAccess.Models;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public int ProductId { get; set; }

    public int EmployeeId { get; set; }

    public string Description { get; set; } = null!;

    public decimal DiscountPct { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}

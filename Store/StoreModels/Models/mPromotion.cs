using System;
using System.Collections.Generic;

namespace StoreModels.Models;

public partial class mPromotion
{
    public int PromotionId { get; set; }

    public int ProductId { get; set; }

    public int EmployeeId { get; set; }

    public string Description { get; set; } = null!;

    public decimal DiscountPct { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}

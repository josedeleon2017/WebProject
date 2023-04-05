using System;
using System.Collections.Generic;

namespace StoreDataAccess.Models;

public partial class ProductInventory
{
    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public int SafetyStockLevel { get; set; }

    public int ReordePoint { get; set; }

    public virtual Product Product { get; set; } = null!;
}

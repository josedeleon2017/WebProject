using System;
using System.Collections.Generic;

namespace StoreModels.Models;

public partial class mProductInventory
{
    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public int SafetyStockLevel { get; set; }

    public int ReordePoint { get; set; }
}

using System;
using System.Collections.Generic;

namespace StoreModels.Models;

public partial class mShipMethod
{
    public int ShipMethodId { get; set; }

    public string Name { get; set; } = null!;

    public decimal ShipBase { get; set; }

    public decimal ShipRate { get; set; }
}

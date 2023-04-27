using System;
using System.Collections.Generic;

namespace StoreDataAccess.Models;

public partial class ShipMethod
{
    public int ShipMethodId { get; set; }

    public string Name { get; set; } = null!;

    public decimal ShipBase { get; set; }

    public decimal ShipRate { get; set; }

    public virtual ICollection<SalesOrderHeader> SalesOrderHeaders { get; } = new List<SalesOrderHeader>();
}

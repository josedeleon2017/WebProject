﻿using System;
using System.Collections.Generic;

namespace StoreAPI.Models;

public partial class SalesOrderDetail
{
    public int SalesOrderId { get; set; }

    public int SalesOrderDetailId { get; set; }

    public int ProductId { get; set; }

    public int OrderQty { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal UnitPriceDiscount { get; set; }

    public decimal LineTotal { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual SalesOrderHeader SalesOrder { get; set; } = null!;
}

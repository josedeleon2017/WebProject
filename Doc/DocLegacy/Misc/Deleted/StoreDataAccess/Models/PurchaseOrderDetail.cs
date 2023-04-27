using System;
using System.Collections.Generic;

namespace StoreAPI.Models;

public partial class PurchaseOrderDetail
{
    public int PurchaseOrderId { get; set; }

    public int PurchaseOrderDetailId { get; set; }

    public int ProductId { get; set; }

    public DateTime DueDate { get; set; }

    public int OrderQty { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal LineTotal { get; set; }

    public int ReceivedQty { get; set; }

    public int RejectedQty { get; set; }

    public int SotckedQty { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual PurchaseOrderHeader PurchaseOrder { get; set; } = null!;
}

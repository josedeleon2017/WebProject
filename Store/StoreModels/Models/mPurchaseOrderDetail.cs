using System;
using System.Collections.Generic;

namespace StoreModels.Models;

public partial class mPurchaseOrderDetail
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
}

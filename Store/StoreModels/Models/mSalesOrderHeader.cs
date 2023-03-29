using System;
using System.Collections.Generic;

namespace StoreAPI.Models;

public partial class mSalesOrderHeader
{
    public int SalesOrderId { get; set; }

    public int CustomerId { get; set; }

    public int ShipToAddressId { get; set; }

    public int ShipToMethodId { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? ShipDate { get; set; }

    public byte Status { get; set; }

    public string? CreditCardType { get; set; }

    public string? CardNumber { get; set; }

    public byte? ExpMonth { get; set; }

    public short? ExpYear { get; set; }

    public string? CreditCardApprovalCode { get; set; }

    public decimal SubTotal { get; set; }

    public decimal TaxAmt { get; set; }

    public decimal Freight { get; set; }

    public decimal TotalDue { get; set; }

    public string? Comment { get; set; }

    public virtual mCustomer Customer { get; set; } = null!;

    public virtual ICollection<mSalesOrderDetail> SalesOrderDetails { get; } = new List<mSalesOrderDetail>();

    public virtual mAddress ShipToAddress { get; set; } = null!;

    public virtual mShipMethod ShipToMethod { get; set; } = null!;
}

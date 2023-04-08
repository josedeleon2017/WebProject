using System;
using System.Collections.Generic;

namespace StoreModels.Models;

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
}

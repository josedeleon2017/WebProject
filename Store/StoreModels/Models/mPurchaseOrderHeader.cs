using System;
using System.Collections.Generic;

namespace StoreModels.Models;

public partial class mPurchaseOrderHeader
{
    public int PurchaseOrderId { get; set; }

    public int EmployeeIdcreator { get; set; }

    public int EmployeeIdapprover { get; set; }

    public int VendorId { get; set; }

    public int Status { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime ShipDate { get; set; }

    public decimal SubTotal { get; set; }

    public decimal TaxAmt { get; set; }

    public decimal Freight { get; set; }

    public decimal TotalDue { get; set; }
}

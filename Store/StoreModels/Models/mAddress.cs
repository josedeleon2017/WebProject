using System;
using System.Collections.Generic;

namespace StoreAPI.Models;

public partial class mAddress
{
    public int AddressId { get; set; }

    public int StateId { get; set; }

    public string AddressLine1 { get; set; } = null!;

    public string? AddressLine2 { get; set; }

    public string? PostalCode { get; set; }

    public virtual ICollection<mCustomer> Customers { get; } = new List<mCustomer>();

    public virtual ICollection<mSalesOrderHeader> SalesOrderHeaders { get; } = new List<mSalesOrderHeader>();

    public virtual mState State { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace StoreDataAccess.Models;

public partial class Address
{
    public int AddressId { get; set; }

    public int StateId { get; set; }

    public string AddressLine1 { get; set; } = null!;

    public string? AddressLine2 { get; set; }

    public string? PostalCode { get; set; }

    public virtual ICollection<Customer> Customers { get; } = new List<Customer>();

    public virtual ICollection<SalesOrderHeader> SalesOrderHeaders { get; } = new List<SalesOrderHeader>();

    public virtual State State { get; set; } = null!;
}

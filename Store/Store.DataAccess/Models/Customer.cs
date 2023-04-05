using System;
using System.Collections.Generic;

namespace StoreDataAccess.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public int AddressId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public bool EmailPromotion { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<SalesOrderHeader> SalesOrderHeaders { get; } = new List<SalesOrderHeader>();

    public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; } = new List<ShoppingCartItem>();
}

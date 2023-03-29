using System;
using System.Collections.Generic;

namespace StoreAPI.Models;

public partial class mShoppingCartItem
{
    public int ShoppingCartItemId { get; set; }

    public int CustomerId { get; set; }

    public int ProductId { get; set; }

    public int OrderQty { get; set; }

    public DateTime DateCreated { get; set; }

    public virtual mCustomer Customer { get; set; } = null!;

    public virtual mProduct Product { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace StoreDataAccess.Models;

public partial class ShoppingCartItem
{
    public int ShoppingCartItemId { get; set; }

    public int CustomerId { get; set; }

    public int ProductId { get; set; }

    public int OrderQty { get; set; }

    public DateTime DateCreated { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}

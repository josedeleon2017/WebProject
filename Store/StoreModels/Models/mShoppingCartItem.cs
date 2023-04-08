using System;
using System.Collections.Generic;

namespace StoreModels.Models;

public partial class mShoppingCartItem
{
    public int ShoppingCartItemId { get; set; }

    public int CustomerId { get; set; }

    public int ProductId { get; set; }

    public int OrderQty { get; set; }

    public DateTime DateCreated { get; set; }
}

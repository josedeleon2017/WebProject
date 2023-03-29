using System;
using System.Collections.Generic;

namespace StoreAPI.Models;

public partial class mProductCategory
{
    public int ProductCategoryId { get; set; }

    public string Name { get; set; } = null!;
}

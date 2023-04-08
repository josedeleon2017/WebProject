using System;
using System.Collections.Generic;

namespace StoreModels.Models;

public partial class mProductSubCategory
{
    public int ProductSubCategoryId { get; set; }

    public int ProductCategoryId { get; set; }

    public string Name { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace StoreAPI.Models;

public partial class ProductCategory
{
    public int ProductCategoryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ProductSubCategory> ProductSubCategories { get; } = new List<ProductSubCategory>();
}

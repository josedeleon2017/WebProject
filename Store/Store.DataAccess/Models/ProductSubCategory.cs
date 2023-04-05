using System;
using System.Collections.Generic;

namespace StoreDataAccess.Models;

public partial class ProductSubCategory
{
    public int ProductSubCategoryId { get; set; }

    public int ProductCategoryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ProductCategory ProductCategory { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}

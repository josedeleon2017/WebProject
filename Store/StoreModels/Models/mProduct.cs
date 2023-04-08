using System;
using System.Collections.Generic;

namespace StoreModels.Models;

public partial class mProduct
{
    public int ProductId { get; set; }

    public int ProductSubCategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Specifications { get; set; } = null!;

    public decimal StandarCost { get; set; }

    public decimal ListPrice { get; set; }

    public DateTime SellStartDate { get; set; }

    public DateTime SellEndDate { get; set; }

    public bool? LowStock { get; set; }

    public bool? ActiveFlag { get; set; }

    public string ImagePath { get; set; } = null!;
}

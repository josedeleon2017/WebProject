using System;
using System.Collections.Generic;

namespace StoreAPI.Models;

public partial class Product
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

    public virtual ProductInventory? ProductInventory { get; set; }

    public virtual ICollection<ProductReview> ProductReviews { get; } = new List<ProductReview>();

    public virtual ProductSubCategory ProductSubCategory { get; set; } = null!;

    public virtual ICollection<ProductVendor> ProductVendors { get; } = new List<ProductVendor>();

    public virtual ICollection<Promotion> Promotions { get; } = new List<Promotion>();

    public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; } = new List<PurchaseOrderDetail>();

    public virtual ICollection<SalesOrderDetail> SalesOrderDetails { get; } = new List<SalesOrderDetail>();

    public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; } = new List<ShoppingCartItem>();
}

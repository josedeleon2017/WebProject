using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace StoreDataAccess.Models;

public partial class StoreContext : DbContext
{
    public StoreContext()
    {
    }

    public StoreContext(DbContextOptions<StoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductInventory> ProductInventories { get; set; }

    public virtual DbSet<ProductPhoto> ProductPhotos { get; set; }

    public virtual DbSet<ProductReview> ProductReviews { get; set; }

    public virtual DbSet<ProductSubCategory> ProductSubCategories { get; set; }

    public virtual DbSet<ProductVendor> ProductVendors { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

    public virtual DbSet<PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SalesOrderDetail> SalesOrderDetails { get; set; }

    public virtual DbSet<SalesOrderHeader> SalesOrderHeaders { get; set; }

    public virtual DbSet<ShipMethod> ShipMethods { get; set; }

    public virtual DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("Address");

            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.AddressLine1).HasMaxLength(60);
            entity.Property(e => e.AddressLine2).HasMaxLength(60);
            entity.Property(e => e.PostalCode).HasMaxLength(15);
            entity.Property(e => e.StateId).HasColumnName("StateID");

            entity.HasOne(d => d.State).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Address_State");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.EmailAddress).HasMaxLength(150);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);

            entity.HasOne(d => d.Address).WithMany(p => p.Customers)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customer_Address");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.BirthDate).HasColumnType("date");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.LoginId)
                .HasMaxLength(50)
                .HasColumnName("LoginID");
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.NationalNumberId)
                .HasMaxLength(13)
                .HasColumnName("NationalNumberID");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.PhotoExtension).HasMaxLength(10);
            entity.Property(e => e.PhotoFileName).HasMaxLength(50);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Role");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("Permission");

            entity.Property(e => e.PermissionId).HasColumnName("PermissionID");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.Permissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Permission_Role");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ListPrice).HasColumnType("money");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ProductSubCategoryId).HasColumnName("ProductSubCategoryID");
            entity.Property(e => e.SellEndDate).HasColumnType("date");
            entity.Property(e => e.SellStartDate).HasColumnType("date");
            entity.Property(e => e.StandarCost).HasColumnType("money");

            entity.HasOne(d => d.ProductSubCategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductSubCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ProductSubCategory");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.ToTable("ProductCategory");

            entity.Property(e => e.ProductCategoryId).HasColumnName("ProductCategoryID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<ProductInventory>(entity =>
        {
            entity.HasKey(e => e.ProductId);

            entity.ToTable("ProductInventory");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithOne(p => p.ProductInventory)
                .HasForeignKey<ProductInventory>(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductInventory_Product");
        });

        modelBuilder.Entity<ProductPhoto>(entity =>
        {
            entity.ToTable("ProductPhoto");

            entity.Property(e => e.ProductPhotoId).HasColumnName("ProductPhotoID");
            entity.Property(e => e.PhotoExtension).HasMaxLength(10);
            entity.Property(e => e.PhotoFileName).HasMaxLength(50);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductPhotos)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductPhoto_Product");
        });

        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.ToTable("ProductReview");

            entity.Property(e => e.ProductReviewId).HasColumnName("ProductReviewID");
            entity.Property(e => e.Comment).HasMaxLength(3850);
            entity.Property(e => e.EmailAddress).HasMaxLength(150);
            entity.Property(e => e.ModifiedDate).HasColumnType("date");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ReviewDate).HasColumnType("date");
            entity.Property(e => e.ReviewerName).HasMaxLength(50);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductReviews)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductReview_Product");
        });

        modelBuilder.Entity<ProductSubCategory>(entity =>
        {
            entity.ToTable("ProductSubCategory");

            entity.Property(e => e.ProductSubCategoryId).HasColumnName("ProductSubCategoryID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ProductCategoryId).HasColumnName("ProductCategoryID");

            entity.HasOne(d => d.ProductCategory).WithMany(p => p.ProductSubCategories)
                .HasForeignKey(d => d.ProductCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductSubCategory_ProductCategory");
        });

        modelBuilder.Entity<ProductVendor>(entity =>
        {
            entity.HasKey(e => new { e.VendorId, e.ProductId });

            entity.ToTable("ProductVendor");

            entity.Property(e => e.VendorId).HasColumnName("VendorID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.LastReceiptCost).HasColumnType("money");
            entity.Property(e => e.LastReceiptDate).HasColumnType("date");
            entity.Property(e => e.StandarPrice).HasColumnType("money");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductVendors)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductVendor_Product");

            entity.HasOne(d => d.Vendor).WithMany(p => p.ProductVendors)
                .HasForeignKey(d => d.VendorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductVendor_Vendor");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.ToTable("Promotion");

            entity.Property(e => e.PromotionId).HasColumnName("PromotionID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.DiscountPct).HasColumnType("smallmoney");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.StartDate).HasColumnType("date");

            entity.HasOne(d => d.Employee).WithMany(p => p.Promotions)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Promotion_Employee");

            entity.HasOne(d => d.Product).WithMany(p => p.Promotions)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Promotion_Product");
        });

        modelBuilder.Entity<PurchaseOrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.PurchaseOrderId, e.PurchaseOrderDetailId });

            entity.ToTable("PurchaseOrderDetail");

            entity.Property(e => e.PurchaseOrderId).HasColumnName("PurchaseOrderID");
            entity.Property(e => e.PurchaseOrderDetailId)
                .ValueGeneratedOnAdd()
                .HasColumnName("PurchaseOrderDetailID");
            entity.Property(e => e.DueDate).HasColumnType("date");
            entity.Property(e => e.LineTotal).HasColumnType("money");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.UnitPrice).HasColumnType("money");

            entity.HasOne(d => d.Product).WithMany(p => p.PurchaseOrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseOrderDetail_Product");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.PurchaseOrderDetails)
                .HasForeignKey(d => d.PurchaseOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseOrderDetail_PurchaseOrderHeader");
        });

        modelBuilder.Entity<PurchaseOrderHeader>(entity =>
        {
            entity.HasKey(e => e.PurchaseOrderId);

            entity.ToTable("PurchaseOrderHeader");

            entity.Property(e => e.PurchaseOrderId).HasColumnName("PurchaseOrderID");
            entity.Property(e => e.EmployeeIdapprover).HasColumnName("EmployeeIDApprover");
            entity.Property(e => e.EmployeeIdcreator).HasColumnName("EmployeeIDCreator");
            entity.Property(e => e.Freight).HasColumnType("money");
            entity.Property(e => e.OrderDate).HasColumnType("date");
            entity.Property(e => e.ShipDate).HasColumnType("date");
            entity.Property(e => e.SubTotal).HasColumnType("money");
            entity.Property(e => e.TaxAmt).HasColumnType("money");
            entity.Property(e => e.TotalDue).HasColumnType("money");
            entity.Property(e => e.VendorId).HasColumnName("VendorID");

            entity.HasOne(d => d.EmployeeIdapproverNavigation).WithMany(p => p.PurchaseOrderHeaderEmployeeIdapproverNavigations)
                .HasForeignKey(d => d.EmployeeIdapprover)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseOrderHeader_Employee1");

            entity.HasOne(d => d.EmployeeIdcreatorNavigation).WithMany(p => p.PurchaseOrderHeaderEmployeeIdcreatorNavigations)
                .HasForeignKey(d => d.EmployeeIdcreator)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseOrderHeader_Employee");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.ToTable("Region");

            entity.Property(e => e.RegionId).HasColumnName("RegionID");
            entity.Property(e => e.Code).HasMaxLength(5);
            entity.Property(e => e.Name).HasMaxLength(15);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<SalesOrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.SalesOrderId, e.SalesOrderDetailId });

            entity.ToTable("SalesOrderDetail");

            entity.Property(e => e.SalesOrderId).HasColumnName("SalesOrderID");
            entity.Property(e => e.SalesOrderDetailId)
                .ValueGeneratedOnAdd()
                .HasColumnName("SalesOrderDetailID");
            entity.Property(e => e.LineTotal).HasColumnType("money");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.UnitPrice).HasColumnType("money");
            entity.Property(e => e.UnitPriceDiscount).HasColumnType("money");

            entity.HasOne(d => d.Product).WithMany(p => p.SalesOrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOrderDetail_Product");

            entity.HasOne(d => d.SalesOrder).WithMany(p => p.SalesOrderDetails)
                .HasForeignKey(d => d.SalesOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOrderDetail_SalesOrderHeader");
        });

        modelBuilder.Entity<SalesOrderHeader>(entity =>
        {
            entity.HasKey(e => e.SalesOrderId);

            entity.ToTable("SalesOrderHeader");

            entity.Property(e => e.SalesOrderId).HasColumnName("SalesOrderID");
            entity.Property(e => e.CardNumber).HasMaxLength(25);
            entity.Property(e => e.Comment).HasMaxLength(256);
            entity.Property(e => e.CreditCardApprovalCode).HasMaxLength(50);
            entity.Property(e => e.CreditCardType).HasMaxLength(50);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DueDate).HasColumnType("date");
            entity.Property(e => e.Freight).HasColumnType("money");
            entity.Property(e => e.OrderDate).HasColumnType("date");
            entity.Property(e => e.ShipDate).HasColumnType("date");
            entity.Property(e => e.ShipToAddressId).HasColumnName("ShipToAddressID");
            entity.Property(e => e.ShipToMethodId).HasColumnName("ShipToMethodID");
            entity.Property(e => e.SubTotal).HasColumnType("money");
            entity.Property(e => e.TaxAmt).HasColumnType("money");
            entity.Property(e => e.TotalDue).HasColumnType("money");

            entity.HasOne(d => d.Customer).WithMany(p => p.SalesOrderHeaders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOrderHeader_Customer");

            entity.HasOne(d => d.ShipToAddress).WithMany(p => p.SalesOrderHeaders)
                .HasForeignKey(d => d.ShipToAddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOrderHeader_Address");

            entity.HasOne(d => d.ShipToMethod).WithMany(p => p.SalesOrderHeaders)
                .HasForeignKey(d => d.ShipToMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOrderHeader_ShipMethod");
        });

        modelBuilder.Entity<ShipMethod>(entity =>
        {
            entity.ToTable("ShipMethod");

            entity.Property(e => e.ShipMethodId).HasColumnName("ShipMethodID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ShipBase).HasColumnType("money");
            entity.Property(e => e.ShipRate).HasColumnType("money");
        });

        modelBuilder.Entity<ShoppingCartItem>(entity =>
        {
            entity.ToTable("ShoppingCartItem");

            entity.Property(e => e.ShoppingCartItemId).HasColumnName("ShoppingCartItemID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DateCreated).HasColumnType("date");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Customer).WithMany(p => p.ShoppingCartItems)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShoppingCartItem_Customer");

            entity.HasOne(d => d.Product).WithMany(p => p.ShoppingCartItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShoppingCartItem_Product");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.ToTable("State");

            entity.Property(e => e.StateId).HasColumnName("StateID");
            entity.Property(e => e.Name).HasMaxLength(15);
            entity.Property(e => e.RegionId).HasColumnName("RegionID");

            entity.HasOne(d => d.Region).WithMany(p => p.States)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_State_Region");
        });

        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.ToTable("Vendor");

            entity.Property(e => e.VendorId).HasColumnName("VendorID");
            entity.Property(e => e.AccountNumber).HasMaxLength(50);
            entity.Property(e => e.AccountType).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PurchasingWebServicesUrl)
                .HasMaxLength(1024)
                .HasColumnName("PurchasingWebServicesURL");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

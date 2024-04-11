using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SecurityLab.Models;

public partial class PobanzTestDbContext : DbContext
{
    public PobanzTestDbContext()
    {
    }

    public PobanzTestDbContext(DbContextOptions<PobanzTestDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<LineItem> LineItems { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductPipeline> ProductPipelines { get; set; }

    public virtual DbSet<UserPipeline> UserPipelines { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:myfreesqldbserverpobanz.database.windows.net,1433;Initial Catalog=PobanzTestDB;Persist Security Info=False;User ID=spobanz;Password=Bigfatpig@1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("customer_ID");

            entity.Property(e => e.CustomerId).HasColumnName("customer_ID");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.CountryOfResidence)
                .HasMaxLength(50)
                .HasColumnName("country_of_residence");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .HasColumnName("gender");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("user_ID");
        });

        modelBuilder.Entity<LineItem>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.ProductId).HasColumnName("product_ID");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_ID");

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_LineItems");

            entity.HasOne(d => d.Transaction).WithMany()
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_LineItems");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("transaction_ID");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_ID");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Bank)
                .HasMaxLength(50)
                .HasColumnName("bank");
            entity.Property(e => e.CountryOfTransaction)
                .HasMaxLength(50)
                .HasColumnName("country_of_transaction");
            entity.Property(e => e.CustomerId).HasColumnName("customer_ID");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DayOfWeek)
                .HasMaxLength(50)
                .HasColumnName("day_of_week");
            entity.Property(e => e.EntryMode)
                .HasMaxLength(50)
                .HasColumnName("entry_mode");
            entity.Property(e => e.Fraud).HasColumnName("fraud");
            entity.Property(e => e.ShippingAddress)
                .HasMaxLength(50)
                .HasColumnName("shipping_address");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.TypeOfCard)
                .HasMaxLength(50)
                .HasColumnName("type_of_card");
            entity.Property(e => e.TypeOfTransaction)
                .HasMaxLength(50)
                .HasColumnName("type_of_transaction");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Customers");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("product_ID");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("product_ID");
            entity.Property(e => e.Category1)
                .HasMaxLength(50)
                .HasColumnName("category1");
            entity.Property(e => e.Category2)
                .HasMaxLength(50)
                .HasColumnName("category2");
            entity.Property(e => e.Category3)
                .HasMaxLength(50)
                .HasColumnName("category3");
            entity.Property(e => e.Description)
                .HasMaxLength(2800)
                .HasColumnName("description");
            entity.Property(e => e.ImgLink)
                .HasMaxLength(150)
                .HasColumnName("img_link");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.NumParts).HasColumnName("num_parts");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.PrimaryColor)
                .HasMaxLength(50)
                .HasColumnName("primary_color");
            entity.Property(e => e.SecondaryColor)
                .HasMaxLength(50)
                .HasColumnName("secondary_color");
            entity.Property(e => e.Year).HasColumnName("year");
        });

        modelBuilder.Entity<ProductPipeline>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Product_Pipeline");

            entity.Property(e => e.ProductId).HasColumnName("product_ID");
            entity.Property(e => e.Rec1).HasColumnName("rec1");
            entity.Property(e => e.Rec2).HasColumnName("rec2");
            entity.Property(e => e.Rec3).HasColumnName("rec3");
            entity.Property(e => e.Rec4).HasColumnName("rec4");
            entity.Property(e => e.Rec5).HasColumnName("rec5");
            entity.Property(e => e.Rec6).HasColumnName("rec6");
            entity.Property(e => e.Rec7).HasColumnName("rec7");
            entity.Property(e => e.Rec8).HasColumnName("rec8");
        });

        modelBuilder.Entity<UserPipeline>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("User_Pipeline");

            entity.Property(e => e.AvgRating).HasColumnName("avg_rating");
            entity.Property(e => e.ProductId).HasColumnName("product_ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

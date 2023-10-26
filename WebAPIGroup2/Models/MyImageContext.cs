using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Models;

public partial class MyImageContext : DbContext
{
    public MyImageContext()
    {
    }

    public MyImageContext(DbContextOptions<MyImageContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Collection> Collections { get; set; }

    public virtual DbSet<CollectionTemplate> CollectionTemplates { get; set; }

    public virtual DbSet<ContentEmail> ContentEmails { get; set; }

    public virtual DbSet<DeliveryInfo> DeliveryInfos { get; set; }

    public virtual DbSet<DescriptionTemplate> DescriptionTemplates { get; set; }

    public virtual DbSet<FeedBack> FeedBacks { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<MaterialPage> MaterialPages { get; set; }

    public virtual DbSet<MonthlySpending> MonthlySpendings { get; set; }

    public virtual DbSet<MyImage> MyImages { get; set; }

    public virtual DbSet<PrintSize> PrintSizes { get; set; }

    public virtual DbSet<ProductDetail> ProductDetails { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<RefeshToken> RefeshTokens { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Template> Templates { get; set; }

    public virtual DbSet<TemplateImage> TemplateImages { get; set; }

    public virtual DbSet<TemplateSize> TemplateSizes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=tcp:myimages.database.windows.net,1433;Initial Catalog=MyImages;Persist Security Info=False;User ID=finsr8280-admin;Password=Tiennam123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");   
           //=> optionsBuilder.UseSqlServer("server=MSI;Database=MyImages;User ID=sa;Password=123;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Collection>(entity =>
        {
            entity.HasOne(d => d.Category).WithMany(p => p.Collections).HasConstraintName("FK_Collection");
        });

        modelBuilder.Entity<CollectionTemplate>(entity =>
        {
            entity.HasOne(d => d.Collection).WithMany(p => p.CollectionTemplates).HasConstraintName("FK_CollectionTemplate_Collection");

            entity.HasOne(d => d.Template).WithMany(p => p.CollectionTemplates).HasConstraintName("FK_CollectionTemplate_Template");
        });

        modelBuilder.Entity<ContentEmail>(entity =>
        {
            entity.HasOne(d => d.DeliveryInfo).WithMany(p => p.ContentEmails).HasConstraintName("FK_ContentEmail_DeliveryInfo");
        });

        modelBuilder.Entity<DeliveryInfo>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.DeliveryInfos).HasConstraintName("FK_DeliveryInfo_User");
        });

        modelBuilder.Entity<DescriptionTemplate>(entity =>
        {
            entity.HasOne(d => d.Template).WithMany(p => p.DescriptionTemplates).HasConstraintName("Fk_Description_Template");
        });

        modelBuilder.Entity<FeedBack>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.FeedBacks).HasConstraintName("FK_FeedBack_User");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.MyImages).WithMany(p => p.Images).HasConstraintName("Fk_Image_MyImages");
        });

        modelBuilder.Entity<MaterialPage>(entity =>
        {
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<MonthlySpending>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.MonthlySpendings).HasConstraintName("FK_MonthlySpending_User");
        });

        modelBuilder.Entity<MyImage>(entity =>
        {
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.MyImages).HasConstraintName("FK_MyImages_PerchaseOrder");

            entity.HasOne(d => d.Template).WithMany(p => p.MyImages).HasConstraintName("FK_MyImages_Template");
        });

        modelBuilder.Entity<ProductDetail>(entity =>
        {
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.MaterialPage).WithMany(p => p.ProductDetails).HasConstraintName("FK_ProductDetail_MaterialPage");

            entity.HasOne(d => d.MyImage).WithMany(p => p.ProductDetails).HasConstraintName("FK_ProductDetail_MyImages");

            entity.HasOne(d => d.TemplateSize).WithMany(p => p.ProductDetails).HasConstraintName("FK_ProductDetail_TemplateSize");
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.Property(e => e.Status).HasDefaultValueSql("('Pending')");

            entity.HasOne(d => d.DeliveryInfo).WithMany(p => p.PurchaseOrders).HasConstraintName("FK_PurchaseOrder_Delivery");

            entity.HasOne(d => d.User).WithMany(p => p.PurchaseOrders).HasConstraintName("FK_PurchaseOrder_User");
        });

        modelBuilder.Entity<RefeshToken>(entity =>
        {
            entity.Property(e => e.IsRevoked).HasDefaultValueSql("((1))");
            entity.Property(e => e.IsUsed).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.User).WithMany(p => p.RefeshTokens).HasConstraintName("Fk_RefeshToken_User");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasOne(d => d.Template).WithMany(p => p.Reviews).HasConstraintName("FK_Review_Detail");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews).HasConstraintName("FK_Review_User");
        });

        modelBuilder.Entity<Template>(entity =>
        {
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TemplateImage>(entity =>
        {
            entity.HasOne(d => d.Template).WithMany(p => p.TemplateImages).HasConstraintName("Fk_Template_Image");
        });

        modelBuilder.Entity<TemplateSize>(entity =>
        {
            entity.HasOne(d => d.PrintSize).WithMany(p => p.TemplateSizes).HasConstraintName("FK_TemplateSize_PrintSize");

            entity.HasOne(d => d.Template).WithMany(p => p.TemplateSizes).HasConstraintName("FK_TemplateSize_Template");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.EmailConfirmed).HasDefaultValueSql("((0))");
            entity.Property(e => e.Role).HasDefaultValueSql("('user')");
            entity.Property(e => e.Status).HasDefaultValueSql("('Pending')");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

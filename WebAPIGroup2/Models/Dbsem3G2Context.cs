using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Models;

public partial class Dbsem3G2Context : DbContext
{
    public Dbsem3G2Context()
    {
    }

    public Dbsem3G2Context(DbContextOptions<Dbsem3G2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("server=MSI;Database=DBSem3G2;User ID=sa;Password=123;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07F345F980");

            entity.ToTable("User");

            entity.Property(e => e.Address).HasMaxLength(256);
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.EmailConfirmed).HasDefaultValueSql("((0))");
            entity.Property(e => e.FullName).HasMaxLength(256);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UserName).HasMaxLength(256);
            entity.Property(e => e.Password).HasMaxLength(256);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserRole__3214EC072B95A71F");

            entity.ToTable("UserRole");

            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserRole__UserID__3E52440B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("server=MSI;Database=DBSem3G2;User ID=sa;Password=123456;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC078B0280C0");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__A9D105341CB33581").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__User__C9F284562CC960E7").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(256);
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.EmailConfirmed).HasDefaultValueSql("((0))");
            entity.Property(e => e.FullName).HasMaxLength(256);
            entity.Property(e => e.Password).HasMaxLength(256);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('user')");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("('Pending')");
            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

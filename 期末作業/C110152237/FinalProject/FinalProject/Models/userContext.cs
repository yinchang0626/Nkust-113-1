using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Models;

public partial class userContext : DbContext
{
    public userContext(DbContextOptions<userContext> options)
        : base(options)
    {
    }

    public virtual DbSet<hotels> hotels { get; set; }

    public virtual DbSet<managers> managers { get; set; }

    public virtual DbSet<members> members { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<hotels>(entity =>
        {
            entity.Property(e => e.id).ValueGeneratedOnAdd();
            entity.Property(e => e._class)
                .HasMaxLength(50)
                .HasColumnName("class");
            entity.Property(e => e.address).HasMaxLength(100);
            entity.Property(e => e.district).HasMaxLength(50);
            entity.Property(e => e.email).HasMaxLength(50);
            entity.Property(e => e.name).HasMaxLength(50);
            entity.Property(e => e.star).HasMaxLength(50);
            entity.Property(e => e.tel).HasMaxLength(50);
            entity.Property(e => e.website).HasMaxLength(150);
        });

        modelBuilder.Entity<managers>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.id).ValueGeneratedOnAdd();
            entity.Property(e => e.mail)
                .HasMaxLength(30)
                .IsFixedLength();
            entity.Property(e => e.name)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.password)
                .HasMaxLength(30)
                .IsFixedLength();
        });

        modelBuilder.Entity<members>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK_member");

            entity.Property(e => e.mail)
                .HasMaxLength(30)
                .IsFixedLength();
            entity.Property(e => e.name)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.password)
                .HasMaxLength(30)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

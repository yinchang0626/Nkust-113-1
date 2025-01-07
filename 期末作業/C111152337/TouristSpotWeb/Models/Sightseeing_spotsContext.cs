using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TouristSpotWeb.Models;

public partial class Sightseeing_spotsContext : DbContext
{
    public Sightseeing_spotsContext(DbContextOptions<Sightseeing_spotsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TouristSpots> TouristSpots { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TouristSpots>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Scenic_Spot_C_f");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.Address).HasColumnType("text");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Gov).HasMaxLength(50);
            entity.Property(e => e.Keyword).HasMaxLength(50);
            entity.Property(e => e.Map).HasMaxLength(250);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Opentime).HasMaxLength(250);
            entity.Property(e => e.Orgclass).HasMaxLength(50);
            entity.Property(e => e.Parkinginfo).HasMaxLength(100);
            entity.Property(e => e.Picdescribe1).HasMaxLength(250);
            entity.Property(e => e.Picdescribe2).HasMaxLength(250);
            entity.Property(e => e.Picdescribe3).HasMaxLength(250);
            entity.Property(e => e.Picture1).HasMaxLength(250);
            entity.Property(e => e.Picture2).HasMaxLength(250);
            entity.Property(e => e.Picture3).HasMaxLength(250);
            entity.Property(e => e.Region).HasMaxLength(50);
            entity.Property(e => e.Remarks).HasMaxLength(100);
            entity.Property(e => e.Tel).HasMaxLength(50);
            entity.Property(e => e.Ticketinfo).HasMaxLength(100);
            entity.Property(e => e.Toldescribe).HasColumnType("text");
            entity.Property(e => e.Town).HasMaxLength(50);
            entity.Property(e => e.Travellinginfo).HasMaxLength(200);
            entity.Property(e => e.Website).HasMaxLength(250);
            entity.Property(e => e.Zone).HasMaxLength(250);
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC070D1B9D16");

            entity.Property(e => e.Email).HasColumnType("text");
            entity.Property(e => e.PasswordHash).HasColumnType("text");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

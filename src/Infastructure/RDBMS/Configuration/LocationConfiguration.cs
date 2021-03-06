﻿using Domain.RDBMS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.RDBMS.Configuration
{
    class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Location");
            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.City)
                .IsRequired()
                .HasColumnName("city")
                .HasMaxLength(30);

            builder.Property(e => e.OfficeName)
                .HasColumnName("office_name")
                .HasMaxLength(10);

            builder.Property(e => e.Street)
                .IsRequired()
                .HasColumnName("street")
                .HasMaxLength(50);

            builder.Property(e => e.IsActive)
                .HasColumnName("is_active");

            builder.Property(e => e.Latitude)
                .IsRequired()
                .HasColumnName("latitude");

            builder.Property(e => e.Longitude)
                .IsRequired()
                .HasColumnName("longitude");

            builder.HasMany(d => d.UserRoom)
                .WithOne(p => p.Location)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasQueryFilter(location => location.IsActive == true);
        }
    }
}

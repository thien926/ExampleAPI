using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Persistence
{
    public class RentContext : DbContext
    {
        public RentContext(DbContextOptions<RentContext> options) : base(options) {} 

        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<RolePermission> RolePermissions { get; set; } = null!;

        public DbSet<Tenant> Tenants { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<FormType> FormTypes { get; set; } = null!;
        // public DbSet<CustomerFormType> CustomerFormTypes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
            });

            modelBuilder.Entity<Permission>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
            });

            modelBuilder.Entity<RolePermission>(entity => {
                entity.HasKey(e => new { e.RoleId, e.PermissionId });
            });

            modelBuilder.Entity<Tenant>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
            });

            modelBuilder.Entity<User>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.PasswordSalt).IsRequired();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(256);

                entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<Customer>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(512);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(512);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(256);
                entity.Property(e => e.MobilePhone).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Gender).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Properties).IsRequired();
            });

            modelBuilder.Entity<FormType>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.CustomerId).IsRequired();
            });

            // modelBuilder.Entity<CustomerFormType>(entity => {
            //     entity.HasKey(e => new { e.CustomerId, e.FormTypeId });
            // });

            base.OnModelCreating(modelBuilder);
        }
    }
}
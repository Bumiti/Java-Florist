using System;
using System.Collections.Generic;
using JavaFlorist.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JavaFlorist.Data

{
    public partial class JavaFloristDbContext : DbContext
    {
        public JavaFloristDbContext()
        {
        }

        public JavaFloristDbContext(DbContextOptions<JavaFloristDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Blog> Blogs { get; set; } = null!;
        public virtual DbSet<Bouquet> Bouquets { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Florist> Florists { get; set; } = null!;
        public virtual DbSet<Occasion> Occasions { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Receiver> Receivers { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Voucher> Vouchers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=.,1500; database=JavaFloristDb; user=sa;password=1");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable("Blog");

                entity.Property(e => e.BlogBrief).HasMaxLength(255);

                entity.Property(e => e.PublishDate).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_Blog_Status");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Blog_User");
            });

            modelBuilder.Entity<Bouquet>(entity =>
            {
                entity.ToTable("Bouquet");

                entity.Property(e => e.BouquetDate).HasColumnType("datetime");

                entity.Property(e => e.Image).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Tag).HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Bouquets)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Bouquet_Category");

                entity.HasOne(d => d.Florist)
                    .WithMany(p => p.Bouquets)
                    .HasForeignKey(d => d.FloristId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Bouquet_Florist");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Type).HasMaxLength(50);
            });

            modelBuilder.Entity<Florist>(entity =>
            {
                entity.ToTable("Florist");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.Logo).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Florists)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_Florist_Status");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Florists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Florist_User");
            });

            modelBuilder.Entity<Occasion>(entity =>
            {
                entity.ToTable("Occasion");

                entity.Property(e => e.Message).HasMaxLength(255);

                entity.Property(e => e.Type).HasMaxLength(50);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.ReceiveDate).HasColumnType("datetime");

                entity.HasOne(d => d.Occasion)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OccasionId)
                    .HasConstraintName("FK_Order_Occasion");

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ReceiverId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Order_Receiver");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_Order_Status");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.HasOne(d => d.Bouquet)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.BouquetId)
                    .HasConstraintName("FK_OrderDetail_Bouquet");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_OrderDetail_Order");
            });

            modelBuilder.Entity<Receiver>(entity =>
            {
                entity.ToTable("Receiver");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.ReceiverDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.Note).HasMaxLength(50);

                entity.Property(e => e.Type).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("DOB");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.Firstname).HasMaxLength(50);

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.Gender).HasMaxLength(10);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.Role).HasMaxLength(30);

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_User_Status");
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.ToTable("Voucher");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Vouchers)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_Voucher_Status");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Vouchers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Voucher_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

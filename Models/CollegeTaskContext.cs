using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CompClubAPI.Models;

namespace CompClubAPI.Models;

public partial class CollegeTaskContext : DbContext
{
    public CollegeTaskContext()
    {
    }

    public CollegeTaskContext(DbContextOptions<CollegeTaskContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Payment> Payments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=CollegeTaskV2;Encrypt=True;TrustServerCertificate=True;User Id=sa;Password=Milk2468!");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3213E83F81D1628A");

            entity.ToTable("Payment");

            entity.HasIndex(e => e.ClientId, "IX_Payment_client_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.EncryptedCardNumber)
                .HasMaxLength(32)
                .HasColumnName("encrypted_card_number");
            entity.Property(e => e.EncryptedCvv)
                .HasMaxLength(32)
                .HasColumnName("encrypted_CVV");
            entity.Property(e => e.LinkDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("link_date");
            entity.Property(e => e.PaymentMethodId).HasColumnName("payment_method_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

public DbSet<CompClubAPI.Models.Client> Client { get; set; } = default!;
}

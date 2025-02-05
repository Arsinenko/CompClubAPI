using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

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

    [JsonIgnore] public virtual DbSet<Account> Accounts { get; set; }

    [JsonIgnore] public virtual DbSet<BalanceHistory> BalanceHistories { get; set; }

    [JsonIgnore] public virtual DbSet<Booking> Bookings { get; set; }

    [JsonIgnore] public virtual DbSet<BookingStatus> BookingStatuses { get; set; }

    [JsonIgnore] public virtual DbSet<Client> Clients { get; set; }

    [JsonIgnore] public virtual DbSet<Club> Clubs { get; set; }

    [JsonIgnore] public virtual DbSet<Employee> Employees { get; set; }

    [JsonIgnore] public virtual DbSet<EmployeeActionLog> EmployeeActionLogs { get; set; }

    [JsonIgnore] public virtual DbSet<Equipment> Equipment { get; set; }

    [JsonIgnore] public virtual DbSet<EquipmentMaintenance> EquipmentMaintenances { get; set; }

    [JsonIgnore] public virtual DbSet<EquipmentStatus> EquipmentStatuses { get; set; }

    [JsonIgnore] public virtual DbSet<Feedback> Feedbacks { get; set; }

    [JsonIgnore] public virtual DbSet<Payment> Payments { get; set; }

    [JsonIgnore] public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    [JsonIgnore] public virtual DbSet<Role> Roles { get; set; }

    [JsonIgnore] public virtual DbSet<Shift> Shifts { get; set; }

    [JsonIgnore] public virtual DbSet<Tariff> Tariffs { get; set; }

    [JsonIgnore] public virtual DbSet<WorkingSpace> WorkingSpaces { get; set; }

    [JsonIgnore] public virtual DbSet<WorkingSpaceEquipment> WorkingSpaceEquipments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=PC11-36\\COOL;Initial Catalog=CollegeTaskV2;TrustServerCertificate=True;Integrated Security=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3213E83FAAA4239E");

            entity.ToTable("Account");

            entity.HasIndex(e => e.IdClient, "IX_Account_id_client");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Balance)
                .HasColumnType("money")
                .HasColumnName("balance");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IdClient).HasColumnName("id_client");
            entity.Property(e => e.LastLogin)
                .HasColumnType("datetime")
                .HasColumnName("last_login");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(64)
                .IsFixedLength()
                .HasColumnName("password");
            entity.Property(e => e.PasswordChangedAt)
                .HasColumnType("datetime")
                .HasColumnName("password_changed_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Account__id_clie__3C69FB99");
        });

        modelBuilder.Entity<BalanceHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BalanceH__3213E83FB261A664");

            entity.ToTable("BalanceHistory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Action)
                .HasMaxLength(50)
                .HasColumnName("action");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.PreviousBalance)
                .HasColumnType("money")
                .HasColumnName("previous_balance");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");

            entity.HasOne(d => d.Account).WithMany(p => p.BalanceHistories)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BalanceHi__accou__0F624AF8");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Booking__3213E83F0889DDB9");

            entity.ToTable("Booking");

            entity.HasIndex(e => e.IdWorkingSpace, "IX_Booking_id_working_space");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.IdPaymentMethod).HasColumnName("id_payment_method");
            entity.Property(e => e.IdStatus).HasColumnName("id_status");
            entity.Property(e => e.IdWorkingSpace).HasColumnName("id_working_space");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");
            entity.Property(e => e.TotalCost)
                .HasColumnType("money")
                .HasColumnName("total_cost");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Account).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__account__160F4887");

            entity.HasOne(d => d.IdPaymentMethodNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.IdPaymentMethod)
                .HasConstraintName("FK__Booking__id_paym__73BA3083");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.IdStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__id_stat__72C60C4A");

            entity.HasOne(d => d.IdWorkingSpaceNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.IdWorkingSpace)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__id_work__71D1E811");
        });

        modelBuilder.Entity<BookingStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookingS__3213E83FDAD5A503");

            entity.ToTable("BookingStatus");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Client__3213E83FAFB5A7C0");

            entity.ToTable("Client");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .HasColumnName("middle_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Club__3213E83FBE0BCDC2");

            entity.ToTable("Club");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.WorkingHours)
                .HasMaxLength(50)
                .HasDefaultValue("10:00-21:00")
                .HasColumnName("working_hours");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F27CF2024");

            entity.ToTable("Employee");

            entity.HasIndex(e => e.IdClub, "IX_Employee_id_club");

            entity.HasIndex(e => e.IdRole, "IX_Employee_id_role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.HireDate).HasColumnName("hire_date");
            entity.Property(e => e.IdClub).HasColumnName("id_club");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.LastLogin)
                .HasColumnType("datetime")
                .HasColumnName("last_login");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasColumnName("login");
            entity.Property(e => e.PassportData)
                .HasMaxLength(50)
                .HasColumnName("passport_data");
            entity.Property(e => e.Password)
                .HasMaxLength(64)
                .IsFixedLength()
                .HasColumnName("password");
            entity.Property(e => e.PasswordChangedAt)
                .HasColumnType("datetime")
                .HasColumnName("password_changed_at");
            entity.Property(e => e.Salary)
                .HasColumnType("money")
                .HasColumnName("salary");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdClubNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IdClub)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employee__id_clu__7D439ABD");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employee__id_rol__7C4F7684");
        });

        modelBuilder.Entity<EmployeeActionLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F8B98D81C");

            entity.ToTable("EmployeeActionLog");

            entity.HasIndex(e => e.IdEmployee, "IX_EmployeeActionLog_id_employee");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActionType)
                .HasMaxLength(50)
                .HasColumnName("action_type");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdEmployee).HasColumnName("id_employee");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.EmployeeActionLogs)
                .HasForeignKey(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EmployeeA__id_em__02FC7413");
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Equipmen__3213E83F7552EB31");

            entity.HasIndex(e => e.IdClub, "IX_Equipment_id_club");

            entity.HasIndex(e => e.Status, "IX_Equipment_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IdClub).HasColumnName("id_club");
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .HasColumnName("name");
            entity.Property(e => e.PurchaseDate).HasColumnName("purchase_date");
            entity.Property(e => e.PurchasePrice)
                .HasColumnType("money")
                .HasColumnName("purchase_price");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(1)
                .HasColumnName("quantity");
            entity.Property(e => e.Specification)
                .HasMaxLength(120)
                .HasColumnName("specification");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdClubNavigation).WithMany(p => p.Equipment)
                .HasForeignKey(d => d.IdClub)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Equipment__id_cl__5CD6CB2B");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Equipment)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Equipment__statu__5DCAEF64");
        });

        modelBuilder.Entity<EquipmentMaintenance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Equipmen__3213E83FBFB6EB88");

            entity.ToTable("EquipmentMaintenance");

            entity.HasIndex(e => e.EquipmentId, "IX_EquipmentMaintenance_equipment_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cost)
                .HasColumnType("money")
                .HasColumnName("cost");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
            entity.Property(e => e.MaintenanceDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("maintenance_date");

            entity.HasOne(d => d.Equipment).WithMany(p => p.EquipmentMaintenances)
                .HasForeignKey(d => d.EquipmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Equipment__equip__6A30C649");
        });

        modelBuilder.Entity<EquipmentStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Equipmen__3213E83F47EA80CB");

            entity.ToTable("EquipmentStatus");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3213E83FE11FE75C");

            entity.ToTable("Feedback");

            entity.HasIndex(e => e.IdClub, "IX_Feedback_id_club");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IdClub).HasColumnName("id_club");
            entity.Property(e => e.Rating).HasColumnName("rating");

            entity.HasOne(d => d.Account).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__accoun__17036CC0");

            entity.HasOne(d => d.IdClubNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.IdClub)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__id_clu__0B91BA14");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3213E83FCB0EC01C");

            entity.ToTable("Payment");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
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

            entity.HasOne(d => d.Account).WithMany(p => p.Payments)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__account__1332DBDC");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentM__3213E83F3C8802FA");

            entity.ToTable("PaymentMethod");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3213E83FAD08622B");

            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shift__3213E83F37EEADBA");

            entity.ToTable("Shift");

            entity.HasIndex(e => e.IdEmployee, "IX_Shift_id_employee");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.IdEmployee).HasColumnName("id_employee");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Shifts)
                .HasForeignKey(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Shift__id_employ__06CD04F7");
        });

        modelBuilder.Entity<Tariff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tariff__3213E83FFCBC004C");

            entity.ToTable("Tariff");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.PricePerMinute)
                .HasColumnType("money")
                .HasColumnName("pricePerMinute");
        });

        modelBuilder.Entity<WorkingSpace>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkingS__3213E83F500C02FE");

            entity.ToTable("WorkingSpace");

            entity.HasIndex(e => e.IdClub, "IX_WorkingSpace_id_club");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IdClub).HasColumnName("id_club");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TariffId).HasColumnName("tariff_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdClubNavigation).WithMany(p => p.WorkingSpaces)
                .HasForeignKey(d => d.IdClub)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WorkingSp__id_cl__5535A963");

            entity.HasOne(d => d.Tariff).WithMany(p => p.WorkingSpaces)
                .HasForeignKey(d => d.TariffId)
                .HasConstraintName("FK__WorkingSp__tarif__3A4CA8FD");
        });

        modelBuilder.Entity<WorkingSpaceEquipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkingS__3213E83F2597700A");

            entity.ToTable("WorkingSpaceEquipment");

            entity.HasIndex(e => e.IdEquipment, "IX_WorkingSpaceEquipment_id_equipment");

            entity.HasIndex(e => e.IdWorkingSpace, "IX_WorkingSpaceEquipment_id_working_space");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IdEquipment).HasColumnName("id_equipment");
            entity.Property(e => e.IdWorkingSpace).HasColumnName("id_working_space");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdEquipmentNavigation).WithMany(p => p.WorkingSpaceEquipments)
                .HasForeignKey(d => d.IdEquipment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WorkingSp__id_eq__656C112C");

            entity.HasOne(d => d.IdWorkingSpaceNavigation).WithMany(p => p.WorkingSpaceEquipments)
                .HasForeignKey(d => d.IdWorkingSpace)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WorkingSp__id_wo__6477ECF3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

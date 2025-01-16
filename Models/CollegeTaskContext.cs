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

    [JsonIgnore] public virtual DbSet<Booking> Bookings { get; set; }

    [JsonIgnore] public virtual DbSet<Client> Clients { get; set; }

    [JsonIgnore] public virtual DbSet<Club> Clubs { get; set; }

    [JsonIgnore] public virtual DbSet<Employee> Employees { get; set; }

    [JsonIgnore] public virtual DbSet<Equipment> Equipment { get; set; }

    [JsonIgnore] public virtual DbSet<EquipmentMaintenance> EquipmentMaintenances { get; set; }

    [JsonIgnore] public virtual DbSet<EquipmentStatus> EquipmentStatuses { get; set; }

    [JsonIgnore] public virtual DbSet<Feedback> Feedbacks { get; set; }

    [JsonIgnore] public virtual DbSet<Game> Games { get; set; }

    [JsonIgnore] public virtual DbSet<GameLauncher> GameLaunchers { get; set; }

    [JsonIgnore] public virtual DbSet<Payment> Payments { get; set; }

    [JsonIgnore] public virtual DbSet<Role> Roles { get; set; }

    [JsonIgnore] public virtual DbSet<Shift> Shifts { get; set; }

    [JsonIgnore] public virtual DbSet<UserActionLog> UserActionLogs { get; set; }

    [JsonIgnore] public virtual DbSet<WorkingSpace> WorkingSpaces { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=CollegeTask;Encrypt=True;TrustServerCertificate=True;User Id=sa;Password=Milk2468!");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3213E83F2E68D220");

            entity.ToTable("Account");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdClient).HasColumnName("id_client");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Account__id_clie__74AE54BC");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Booking__3213E83FFA8A0F6B");

            entity.ToTable("Booking");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.IdClient).HasColumnName("id_client");
            entity.Property(e => e.IdWorkingSpace).HasColumnName("id_working_space");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasColumnName("payment_method");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TotalCost)
                .HasColumnType("money")
                .HasColumnName("total_cost");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__id_clie__60A75C0F");

            entity.HasOne(d => d.IdWorkingSpaceNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.IdWorkingSpace)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__id_work__619B8048");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Client__3213E83FF84D6167");

            entity.ToTable("Client");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasColumnName("login");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .HasColumnName("middle_name");
            entity.Property(e => e.Password)
                .HasMaxLength(64)
                .IsFixedLength()
                .HasColumnName("password");
        });

        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Club__3213E83F42733CBE");

            entity.ToTable("Club");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .HasColumnName("address");
            entity.Property(e => e.Employees).HasColumnName("employees");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.WorkingHours)
                .HasMaxLength(50)
                .HasDefaultValue("10:00-21:00")
                .HasColumnName("working_hours");

            entity.HasOne(d => d.EmployeesNavigation).WithMany(p => p.Clubs)
                .HasForeignKey(d => d.Employees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Club__employees__44FF419A");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F56C19B8B");

            entity.ToTable("Employee");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.HireDate).HasColumnName("hire_date");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
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
            entity.Property(e => e.Salary)
                .HasColumnType("money")
                .HasColumnName("salary");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employee__id_rol__412EB0B6");
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Equipmen__3213E83F99846A9B");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdClub).HasColumnName("id_club");
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .HasColumnName("_name");
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

            entity.HasOne(d => d.IdClubNavigation).WithMany(p => p.Equipment)
                .HasForeignKey(d => d.IdClub)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Equipment__id_cl__47DBAE45");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Equipment)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Equipment__statu__48CFD27E");
        });

        modelBuilder.Entity<EquipmentMaintenance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Equipmen__3213E83F7C13CFB8");

            entity.ToTable("EquipmentMaintenance");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cost)
                .HasColumnType("money")
                .HasColumnName("cost");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
            entity.Property(e => e.MaintenanceDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("maintenance_date");

            entity.HasOne(d => d.Equipment).WithMany(p => p.EquipmentMaintenances)
                .HasForeignKey(d => d.EquipmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Equipment__equip__4CA06362");
        });

        modelBuilder.Entity<EquipmentStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Equipmen__3213E83FE64FDD47");

            entity.ToTable("EquipmentStatus");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("_name");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3213E83F732AD023");

            entity.ToTable("Feedback");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.FeedbackDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("feedback_date");
            entity.Property(e => e.IdClient).HasColumnName("id_client");
            entity.Property(e => e.IdClub).HasColumnName("id_club");
            entity.Property(e => e.Rating).HasColumnName("rating");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.IdClient)
                .HasConstraintName("FK__Feedback__id_cli__5165187F");

            entity.HasOne(d => d.IdClubNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.IdClub)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__id_clu__5070F446");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Game__3213E83FD9B26F91");

            entity.ToTable("Game");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdLauncher).HasColumnName("id_launcher");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("_name");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");

            entity.HasOne(d => d.IdLauncherNavigation).WithMany(p => p.Games)
                .HasForeignKey(d => d.IdLauncher)
                .HasConstraintName("FK__Game__id_launche__71D1E811");
        });

        modelBuilder.Entity<GameLauncher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GameLaun__3213E83F712672C9");

            entity.ToTable("GameLauncher");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("_name");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Payment_pk");

            entity.ToTable("Payment");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.EncryptedCardNumber)
                .HasMaxLength(256)
                .HasColumnName("encrypted_card_number");
            entity.Property(e => e.EncryptedCvv)
                .HasMaxLength(265)
                .HasColumnName("encrypted_CVV");
            entity.Property(e => e.LinkDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("link_date");

            entity.HasOne(d => d.Client).WithMany(p => p.Payments)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__client___3C69FB99");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3213E83FB31A9729");

            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("_name");
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shift__3213E83FA5C59450");

            entity.ToTable("Shift");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.IdEmployee).HasColumnName("id_employee");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Shifts)
                .HasForeignKey(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Shift__id_employ__5629CD9C");
        });

        modelBuilder.Entity<UserActionLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserActi__3213E83F482A7EA8");

            entity.ToTable("UserActionLog");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Action)
                .HasMaxLength(50)
                .HasColumnName("action");
            entity.Property(e => e.ActionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("action_date");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");

            entity.HasOne(d => d.Client).WithMany(p => p.UserActionLogs)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__UserActio__clien__59063A47");
        });

        modelBuilder.Entity<WorkingSpace>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Working___3213E83F715737F2");

            entity.ToTable("WorkingSpace");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdEquipment).HasColumnName("id_equipment");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.IdEquipmentNavigation).WithMany(p => p.WorkingSpaces)
                .HasForeignKey(d => d.IdEquipment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Working_s__id_eq__5CD6CB2B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

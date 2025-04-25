using System;
using System.Collections.Generic;
using CompClubAPI.Models; // Убедитесь, что путь к моделям правильный
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // Если используется IConfiguration

namespace CompClubAPI.Context;

public partial class CollegeTaskV2Context : DbContext
{
    // Оставляем конструкторы как есть
    private readonly IConfiguration _configuration;

    public CollegeTaskV2Context()
    {
    }

    public CollegeTaskV2Context(DbContextOptions<CollegeTaskV2Context> options)
        : base(options)
    {
    }

     public CollegeTaskV2Context(DbContextOptions<CollegeTaskV2Context> options, IConfiguration configuration)
         : base(options)
     {
         _configuration = configuration; // Если используете IConfiguration
     }

    // DbSet остаются без изменений
    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<BalanceHistory> BalanceHistories { get; set; }
    public virtual DbSet<Booking> Bookings { get; set; }
    public virtual DbSet<BookingStatus> BookingStatuses { get; set; }
    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<Club> Clubs { get; set; }
    public virtual DbSet<CostRevenue> CostRevenues { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<EmployeeActionLog> EmployeeActionLogs { get; set; }
    public virtual DbSet<Equipment> Equipment { get; set; }
    public virtual DbSet<EquipmentMaintenance> EquipmentMaintenances { get; set; }
    public virtual DbSet<EquipmentStatus> EquipmentStatuses { get; set; }
    public virtual DbSet<Feedback> Feedbacks { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Shift> Shifts { get; set; }
    public virtual DbSet<Statistic> Statistics { get; set; }
    public virtual DbSet<Tariff> Tariffs { get; set; }
    public virtual DbSet<WorkingSpace> WorkingSpaces { get; set; }
    public virtual DbSet<WorkingSpaceEquipment> WorkingSpaceEquipments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Логика конфигурации остается прежней
        if (!optionsBuilder.IsConfigured)
        {
            // Попробуем получить строку подключения из переменных окружения или конфигурации
            string connectionString = _configuration?.GetConnectionString("DefaultConnection") ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                // Фолбэк на жестко заданную строку (не рекомендуется для production)
                connectionString = "Server=localhost;Database=CollegeTaskV2;Encrypt=True;TrustServerCertificate=True;User Id=sa;Password=Milk2468!"; // Используйте безопасные способы хранения паролей
            }
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3213E83FAAA4239E");
            entity.ToTable("Account");

            // Имя индекса теперь может быть другим, если вы выполнили команду EXEC sp_rename для индекса
            // entity.HasIndex(e => e.IdClient, "IX_Account_id_client"); // Старое имя
            entity.HasIndex(e => e.IdClient, "IX_Account_idClient"); // Новое имя индекса (если переименовали)

            entity.Property(e => e.Id).HasColumnName("id"); // PK не меняем
            entity.Property(e => e.Balance).HasColumnType("money").HasColumnName("balance"); // Уже camelCase
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("createdAt"); // Изменено
            entity.Property(e => e.Email).HasMaxLength(50).HasColumnName("email"); // Уже camelCase
            entity.Property(e => e.IdClient).HasColumnName("idClient"); // Изменено
            entity.Property(e => e.IsAlive).HasDefaultValue(true).HasColumnName("isAlive"); // Изменено
            entity.Property(e => e.LastLogin).HasColumnType("datetime").HasColumnName("lastLogin"); // Изменено
            entity.Property(e => e.Login).HasMaxLength(50).HasColumnName("login"); // Уже camelCase
            entity.Property(e => e.Password).HasMaxLength(64).IsFixedLength().HasColumnName("password"); // Уже camelCase
            entity.Property(e => e.PasswordChangedAt).HasColumnType("datetime").HasColumnName("passwordChangedAt"); // Изменено
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("updatedAt"); // Изменено

            // Связи остаются как есть, они ссылаются на свойства C# (IdClient), а не на колонки БД напрямую здесь
            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.IdClient) // Ссылка на свойство C#
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Account__id_clie__3C69FB99"); // Имя констрейнта не меняем
        });

        modelBuilder.Entity<BalanceHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BalanceH__3213E83FB261A664");
            entity.ToTable("BalanceHistory");

            // entity.HasIndex(e => e.AccountId, "IX_BalanceHistory_account_id"); // Старое имя
            entity.HasIndex(e => e.AccountId, "IX_BalanceHistory_accountId"); // Новое имя индекса (если переименовали)

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("accountId"); // Изменено
            entity.Property(e => e.Action).HasMaxLength(50).HasColumnName("action"); // Уже camelCase
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("createdAt"); // Изменено
            entity.Property(e => e.PreviousBalance).HasColumnType("money").HasColumnName("previousBalance"); // Изменено
            entity.Property(e => e.Price).HasColumnType("money").HasColumnName("price"); // Уже camelCase

            // Связь ссылается на свойство C# AccountId
            // entity.HasOne(d => d.Account).WithMany(p => p.BalanceHistories)
            //     .HasForeignKey(d => d.AccountId)
            //     .OnDelete(DeleteBehavior.ClientSetNull)
            //     .HasConstraintName("FK__BalanceHi__accou__0F624AF8");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Booking__3213E83F0889DDB9");
            entity.ToTable("Booking");

            // entity.HasIndex(e => e.AccountId, "IX_Booking_account_id"); // Старое имя
            entity.HasIndex(e => e.AccountId, "IX_Booking_accountId"); // Новое имя индекса (если переименовали)
            // entity.HasIndex(e => e.IdPaymentMethod, "IX_Booking_id_payment_method"); // Старое имя
            entity.HasIndex(e => e.IdPaymentMethod, "IX_Booking_idPaymentMethod"); // Новое имя индекса (если переименовали)
            // entity.HasIndex(e => e.IdStatus, "IX_Booking_id_status"); // Старое имя
            entity.HasIndex(e => e.IdStatus, "IX_Booking_idStatus"); // Новое имя индекса (если переименовали)
            // entity.HasIndex(e => e.IdWorkingSpace, "IX_Booking_id_working_space"); // Старое имя
            entity.HasIndex(e => e.IdWorkingSpace, "IX_Booking_idWorkingSpace"); // Новое имя индекса (если переименовали)

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("accountId"); // Изменено
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("createdAt"); // Изменено
            entity.Property(e => e.EndTime).HasColumnType("datetime").HasColumnName("endTime"); // Изменено
            entity.Property(e => e.IdPaymentMethod).HasColumnName("idPaymentMethod"); // Изменено
            entity.Property(e => e.IdStatus).HasColumnName("idStatus"); // Изменено
            entity.Property(e => e.IdWorkingSpace).HasColumnName("idWorkingSpace"); // Изменено
            entity.Property(e => e.StartTime).HasColumnType("datetime").HasColumnName("startTime"); // Изменено
            entity.Property(e => e.TotalCost).HasColumnType("money").HasColumnName("totalCost"); // Изменено
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("updatedAt"); // Изменено

            // Связи ссылаются на свойства C#
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
            entity.Property(e => e.Name).HasMaxLength(50).HasColumnName("name"); // Уже camelCase
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Client__3213E83FAFB5A7C0");
            entity.ToTable("Client");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("createdAt"); // Изменено
            entity.Property(e => e.FirstName).HasMaxLength(50).HasColumnName("firstName"); // Изменено
            entity.Property(e => e.LastName).HasMaxLength(50).HasColumnName("lastName"); // Изменено
            entity.Property(e => e.MiddleName).HasMaxLength(50).HasColumnName("middleName"); // Изменено
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("updatedAt"); // Изменено
        });

        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Club__3213E83FBE0BCDC2");
            entity.ToTable("Club");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address"); // Уже camelCase
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("createdAt"); // Изменено
            entity.Property(e => e.IsAlive).HasDefaultValue(true).HasColumnName("isAlive"); // Изменено
            entity.Property(e => e.Name).HasMaxLength(50).HasColumnName("name"); // Уже camelCase
            entity.Property(e => e.Phone).HasMaxLength(15).IsUnicode(false).HasColumnName("phone"); // Уже camelCase
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("updatedAt"); // Изменено
            entity.Property(e => e.WorkingHours).HasMaxLength(50).HasDefaultValue("10:00-21:00").HasColumnName("workingHours"); // Изменено
        });

        modelBuilder.Entity<CostRevenue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CostReve__3213E83F693444FD");
            entity.ToTable("CostRevenue");

            // entity.HasIndex(e => e.IdClub, "IX_CostRevenue_id_club"); // Старое имя
             entity.HasIndex(e => e.IdClub, "IX_CostRevenue_idClub"); // Новое имя индекса (если переименовали)

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnType("money").HasColumnName("amount"); // Уже camelCase
            entity.Property(e => e.CreatedAt).HasColumnType("datetime").HasColumnName("createdAt"); // Изменено
            entity.Property(e => e.Description).HasColumnName("description"); // Уже camelCase
            entity.Property(e => e.IdClub).HasColumnName("idClub"); // Изменено
            entity.Property(e => e.Revenue).HasColumnName("revenue"); // Уже camelCase

            entity.HasOne(d => d.IdClubNavigation).WithMany(p => p.CostRevenues)
                .HasForeignKey(d => d.IdClub)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CostReven__id_cl__55009F39");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F27CF2024");
            entity.ToTable("Employee");

            // entity.HasIndex(e => e.IdClub, "IX_Employee_id_club"); // Старое имя
            entity.HasIndex(e => e.IdClub, "IX_Employee_idClub"); // Новое имя индекса (если переименовали)
            // entity.HasIndex(e => e.IdRole, "IX_Employee_id_role"); // Старое имя
            entity.HasIndex(e => e.IdRole, "IX_Employee_idRole"); // Новое имя индекса (если переименовали)

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("createdAt"); // Изменено
            entity.Property(e => e.HireDate).HasColumnName("hireDate"); // Изменено
            entity.Property(e => e.IdClub).HasColumnName("idClub"); // Изменено
            entity.Property(e => e.IdRole).HasColumnName("idRole"); // Изменено
            entity.Property(e => e.LastLogin).HasColumnType("datetime").HasColumnName("lastLogin"); // Изменено
            entity.Property(e => e.Login).HasMaxLength(50).HasColumnName("login"); // Уже camelCase
            entity.Property(e => e.PassportData).HasMaxLength(50).HasColumnName("passportData"); // Изменено
            entity.Property(e => e.Password).HasMaxLength(64).IsFixedLength().HasColumnName("password"); // Уже camelCase
            entity.Property(e => e.PasswordChangedAt).HasColumnType("datetime").HasColumnName("passwordChangedAt"); // Изменено
            entity.Property(e => e.Salary).HasColumnType("money").HasColumnName("salary"); // Уже camelCase
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("updatedAt"); // Изменено

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

            // entity.HasIndex(e => e.IdEmployee, "IX_EmployeeActionLog_id_employee"); // Старое имя
            entity.HasIndex(e => e.IdEmployee, "IX_EmployeeActionLog_idEmployee"); // Новое имя индекса (если переименовали)

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActionType).HasMaxLength(50).HasColumnName("actionType"); // Изменено
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("createdAt"); // Изменено
            entity.Property(e => e.Description).HasColumnName("description"); // Уже camelCase
            entity.Property(e => e.IdEmployee).HasColumnName("idEmployee"); // Изменено

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.EmployeeActionLogs)
                .HasForeignKey(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EmployeeA__id_em__02FC7413");
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Equipmen__3213E83F7552EB31");
            entity.ToTable("Equipment"); // Имя таблицы не меняем

            // entity.HasIndex(e => e.IdClub, "IX_Equipment_id_club"); // Старое имя
            entity.HasIndex(e => e.IdClub, "IX_Equipment_idClub"); // Новое имя индекса (если переименовали)
            entity.HasIndex(e => e.Status, "IX_Equipment_status"); // Индекс по status не меняем

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("createdAt"); // Изменено
            entity.Property(e => e.IdClub).HasColumnName("idClub"); // Изменено
            entity.Property(e => e.Name).HasMaxLength(80).HasColumnName("name"); // Уже camelCase
            entity.Property(e => e.PurchaseDate).HasColumnName("purchaseDate"); // Изменено
            entity.Property(e => e.PurchasePrice).HasColumnType("money").HasColumnName("purchasePrice"); // Изменено
            entity.Property(e => e.Quantity).HasDefaultValue(1).HasColumnName("quantity"); // Уже camelCase
            entity.Property(e => e.Specification).HasMaxLength(120).HasColumnName("specification"); // Уже camelCase
            entity.Property(e => e.Status).HasColumnName("status"); // Уже camelCase
            entity.Property(e => e.Type).HasMaxLength(50).HasColumnName("type"); // Уже camelCase
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("updatedAt"); // Изменено

            entity.HasOne(d => d.IdClubNavigation).WithMany(p => p.Equipment)
                .HasForeignKey(d => d.IdClub)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Equipment__id_cl__5CD6CB2B");

            // Свойство Status мапится на колонку status, не id_status
            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Equipment)
                .HasForeignKey(d => d.Status) // Ссылка на свойство C# Status
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Equipment__statu__5DCAEF64");
        });

        modelBuilder.Entity<EquipmentMaintenance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Equipmen__3213E83FBFB6EB88");
            entity.ToTable("EquipmentMaintenance");

            // entity.HasIndex(e => e.EquipmentId, "IX_EquipmentMaintenance_equipment_id"); // Старое имя
            entity.HasIndex(e => e.EquipmentId, "IX_EquipmentMaintenance_equipmentId"); // Новое имя индекса (если переименовали)

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cost).HasColumnType("money").HasColumnName("cost"); // Уже camelCase
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("createdAt"); // Изменено
            entity.Property(e => e.Description).HasColumnName("description"); // Уже camelCase
            entity.Property(e => e.EquipmentId).HasColumnName("equipmentId"); // Изменено
            entity.Property(e => e.MaintenanceDate).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("maintenanceDate"); // Изменено

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
            entity.Property(e => e.Name).HasMaxLength(50).HasColumnName("name"); // Уже camelCase
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3213E83FE11FE75C");
            entity.ToTable("Feedback");

            // entity.HasIndex(e => e.AccountId, "IX_Feedback_account_id"); // Старое имя
            entity.HasIndex(e => e.AccountId, "IX_Feedback_accountId"); // Новое имя индекса (если переименовали)
            // entity.HasIndex(e => e.IdClub, "IX_Feedback_id_club"); // Старое имя
            entity.HasIndex(e => e.IdClub, "IX_Feedback_idClub"); // Новое имя индекса (если переименовали)

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("accountId"); // Изменено
            entity.Property(e => e.Comment).HasColumnName("comment"); // Уже camelCase
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("createdAt"); // Изменено
            entity.Property(e => e.IdClub).HasColumnName("idClub"); // Изменено
            entity.Property(e => e.Rating).HasColumnName("rating"); // Уже camelCase

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

            // entity.HasIndex(e => e.AccountId, "IX_Payment_account_id"); // Старое имя
            entity.HasIndex(e => e.AccountId, "IX_Payment_accountId"); // Новое имя индекса (если переименовали)

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("accountId"); // Изменено
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("createdAt"); // Изменено
            entity.Property(e => e.EncryptedCardNumber).HasMaxLength(32).HasColumnName("encryptedCardNumber"); // Изменено
            entity.Property(e => e.EncryptedCvv).HasMaxLength(32).HasColumnName("encryptedCvv"); // Изменено (было encrypted_CVV)

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
            entity.Property(e => e.Name).HasMaxLength(50).HasColumnName("name"); // Уже camelCase
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3213E83FAD08622B");
            entity.ToTable("Role");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("createdAt"); // Изменено
            entity.Property(e => e.Name).HasMaxLength(50).HasColumnName("name"); // Уже camelCase
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shift__3213E83F37EEADBA");
            entity.ToTable("Shift");

            // entity.HasIndex(e => e.IdEmployee, "IX_Shift_id_employee"); // Старое имя
            entity.HasIndex(e => e.IdEmployee, "IX_Shift_idEmployee"); // Новое имя индекса (если переименовали)

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("createdAt"); // Изменено
            entity.Property(e => e.EndTime).HasColumnType("datetime").HasColumnName("endTime"); // Изменено
            entity.Property(e => e.IdEmployee).HasColumnName("idEmployee"); // Изменено
            entity.Property(e => e.StartTime).HasColumnType("datetime").HasColumnName("startTime"); // Изменено
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("updatedAt"); // Изменено

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Shifts)
                .HasForeignKey(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Shift__id_employ__06CD04F7");
        });

        modelBuilder.Entity<Statistic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Statisti__3213E83F70667EBB");
            entity.ToTable("Statistic");

            // entity.HasIndex(e => e.IdClub, "IX_Statistic_id_club"); // Старое имя
            entity.HasIndex(e => e.IdClub, "IX_Statistic_idClub"); // Новое имя индекса (если переименовали)

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientNumber).HasColumnName("clientNumber"); // Изменено
            entity.Property(e => e.Finances).HasColumnType("money").HasColumnName("finances"); // Уже camelCase
            entity.Property(e => e.IdClub).HasColumnName("idClub"); // Изменено

            // Имя FK констрейнта было FK_Statistic_Club_id_club, оставляем его или переименовываем вручную в БД
             entity.HasOne(d => d.IdClubNavigation).WithMany(p => p.Statistics)
                 .HasForeignKey(d => d.IdClub)
                 .OnDelete(DeleteBehavior.Cascade) // Добавлено Cascade по схеме
                 .HasConstraintName("FK_Statistic_Club_id_club"); // Оставляем старое имя констрейнта
        });

        modelBuilder.Entity<Tariff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tariff__3213E83FFCBC004C");
            entity.ToTable("Tariff");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasMaxLength(50).HasColumnName("name"); // Уже camelCase
            entity.Property(e => e.PricePerMinute).HasColumnType("money").HasColumnName("pricePerMinute"); // Уже camelCase
        });

        modelBuilder.Entity<WorkingSpace>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkingS__3213E83F500C02FE");
            entity.ToTable("WorkingSpace");

            // entity.HasIndex(e => e.IdClub, "IX_WorkingSpace_id_club"); // Старое имя
            entity.HasIndex(e => e.IdClub, "IX_WorkingSpace_idClub"); // Новое имя индекса (если переименовали)
            // entity.HasIndex(e => e.TariffId, "IX_WorkingSpace_tariff_id"); // Старое имя
            entity.HasIndex(e => e.TariffId, "IX_WorkingSpace_tariffId"); // Новое имя индекса (если переименовали)

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("createdAt"); // Изменено
            entity.Property(e => e.IdClub).HasColumnName("idClub"); // Изменено
            entity.Property(e => e.Name).HasMaxLength(50).HasColumnName("name"); // Уже camelCase
            entity.Property(e => e.Status).HasMaxLength(50).HasColumnName("status"); // Уже camelCase
            entity.Property(e => e.TariffId).HasColumnName("tariffId"); // Изменено
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("updatedAt"); // Изменено

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

            // entity.HasIndex(e => e.IdEquipment, "IX_WorkingSpaceEquipment_id_equipment"); // Старое имя
            entity.HasIndex(e => e.IdEquipment, "IX_WorkingSpaceEquipment_idEquipment"); // Новое имя индекса (если переименовали)
            // entity.HasIndex(e => e.IdWorkingSpace, "IX_WorkingSpaceEquipment_id_working_space"); // Старое имя
            entity.HasIndex(e => e.IdWorkingSpace, "IX_WorkingSpaceEquipment_idWorkingSpace"); // Новое имя индекса (если переименовали)

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("createdAt"); // Изменено
            entity.Property(e => e.IdEquipment).HasColumnName("idEquipment"); // Изменено
            entity.Property(e => e.IdWorkingSpace).HasColumnName("idWorkingSpace"); // Изменено
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("updatedAt"); // Изменено

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
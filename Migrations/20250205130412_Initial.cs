using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompClubAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookingStatus",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BookingS__3213E83FDAD5A503", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    middle_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Client__3213E83FAFB5A7C0", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Club",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    phone = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    working_hours = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "10:00-21:00"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Club__3213E83FBE0BCDC2", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentStatus",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Equipmen__3213E83F47EA80CB", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PaymentM__3213E83F3C8802FA", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role__3213E83FAD08622B", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Tariff",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    pricePerMinute = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tariff__3213E83FFCBC004C", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_client = table.Column<int>(type: "int", nullable: false),
                    balance = table.Column<decimal>(type: "money", nullable: true),
                    login = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password = table.Column<byte[]>(type: "binary(64)", fixedLength: true, maxLength: 64, nullable: false),
                    last_login = table.Column<DateTime>(type: "datetime", nullable: true),
                    password_changed_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Account__3213E83FAAA4239E", x => x.id);
                    table.ForeignKey(
                        name: "FK__Account__id_clie__3C69FB99",
                        column: x => x.id_client,
                        principalTable: "Client",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    specification = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    purchase_date = table.Column<DateOnly>(type: "date", nullable: true),
                    purchase_price = table.Column<decimal>(type: "money", nullable: true),
                    id_club = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Equipmen__3213E83F7552EB31", x => x.id);
                    table.ForeignKey(
                        name: "FK__Equipment__id_cl__5CD6CB2B",
                        column: x => x.id_club,
                        principalTable: "Club",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Equipment__statu__5DCAEF64",
                        column: x => x.status,
                        principalTable: "EquipmentStatus",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    login = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password = table.Column<byte[]>(type: "binary(64)", fixedLength: true, maxLength: 64, nullable: false),
                    passport_data = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    hire_date = table.Column<DateOnly>(type: "date", nullable: false),
                    id_role = table.Column<int>(type: "int", nullable: false),
                    salary = table.Column<decimal>(type: "money", nullable: false),
                    id_club = table.Column<int>(type: "int", nullable: false),
                    last_login = table.Column<DateTime>(type: "datetime", nullable: true),
                    password_changed_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Employee__3213E83F27CF2024", x => x.id);
                    table.ForeignKey(
                        name: "FK__Employee__id_clu__7D439ABD",
                        column: x => x.id_club,
                        principalTable: "Club",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Employee__id_rol__7C4F7684",
                        column: x => x.id_role,
                        principalTable: "Role",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "WorkingSpace",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_club = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    tariff_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkingS__3213E83F500C02FE", x => x.id);
                    table.ForeignKey(
                        name: "FK__WorkingSp__id_cl__5535A963",
                        column: x => x.id_club,
                        principalTable: "Club",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__WorkingSp__tarif__3A4CA8FD",
                        column: x => x.tariff_id,
                        principalTable: "Tariff",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "BalanceHistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    price = table.Column<decimal>(type: "money", nullable: true),
                    previous_balance = table.Column<decimal>(type: "money", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    account_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BalanceH__3213E83FB261A664", x => x.id);
                    table.ForeignKey(
                        name: "FK__BalanceHi__accou__0F624AF8",
                        column: x => x.account_id,
                        principalTable: "Account",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_club = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    account_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__3213E83FE11FE75C", x => x.id);
                    table.ForeignKey(
                        name: "FK__Feedback__accoun__17036CC0",
                        column: x => x.account_id,
                        principalTable: "Account",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Feedback__id_clu__0B91BA14",
                        column: x => x.id_club,
                        principalTable: "Club",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    encrypted_card_number = table.Column<byte[]>(type: "varbinary(32)", maxLength: 32, nullable: true),
                    encrypted_CVV = table.Column<byte[]>(type: "varbinary(32)", maxLength: 32, nullable: true),
                    account_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payment__3213E83FCB0EC01C", x => x.id);
                    table.ForeignKey(
                        name: "FK__Payment__account__1332DBDC",
                        column: x => x.account_id,
                        principalTable: "Account",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMaintenance",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    equipment_id = table.Column<int>(type: "int", nullable: false),
                    maintenance_date = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cost = table.Column<decimal>(type: "money", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Equipmen__3213E83FBFB6EB88", x => x.id);
                    table.ForeignKey(
                        name: "FK__Equipment__equip__6A30C649",
                        column: x => x.equipment_id,
                        principalTable: "Equipment",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeActionLog",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_employee = table.Column<int>(type: "int", nullable: false),
                    action_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Employee__3213E83F8B98D81C", x => x.id);
                    table.ForeignKey(
                        name: "FK__EmployeeA__id_em__02FC7413",
                        column: x => x.id_employee,
                        principalTable: "Employee",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Shift",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_employee = table.Column<int>(type: "int", nullable: false),
                    start_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    end_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Shift__3213E83F37EEADBA", x => x.id);
                    table.ForeignKey(
                        name: "FK__Shift__id_employ__06CD04F7",
                        column: x => x.id_employee,
                        principalTable: "Employee",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_working_space = table.Column<int>(type: "int", nullable: false),
                    start_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    end_time = table.Column<DateTime>(type: "datetime", nullable: true),
                    id_status = table.Column<int>(type: "int", nullable: false),
                    total_cost = table.Column<decimal>(type: "money", nullable: true),
                    id_payment_method = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    account_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Booking__3213E83F0889DDB9", x => x.id);
                    table.ForeignKey(
                        name: "FK__Booking__account__160F4887",
                        column: x => x.account_id,
                        principalTable: "Account",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Booking__id_paym__73BA3083",
                        column: x => x.id_payment_method,
                        principalTable: "PaymentMethod",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Booking__id_stat__72C60C4A",
                        column: x => x.id_status,
                        principalTable: "BookingStatus",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Booking__id_work__71D1E811",
                        column: x => x.id_working_space,
                        principalTable: "WorkingSpace",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "WorkingSpaceEquipment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_working_space = table.Column<int>(type: "int", nullable: false),
                    id_equipment = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkingS__3213E83F2597700A", x => x.id);
                    table.ForeignKey(
                        name: "FK__WorkingSp__id_eq__656C112C",
                        column: x => x.id_equipment,
                        principalTable: "Equipment",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__WorkingSp__id_wo__6477ECF3",
                        column: x => x.id_working_space,
                        principalTable: "WorkingSpace",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_id_client",
                table: "Account",
                column: "id_client");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceHistory_account_id",
                table: "BalanceHistory",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_account_id",
                table: "Booking",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_id_payment_method",
                table: "Booking",
                column: "id_payment_method");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_id_status",
                table: "Booking",
                column: "id_status");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_id_working_space",
                table: "Booking",
                column: "id_working_space");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_id_club",
                table: "Employee",
                column: "id_club");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_id_role",
                table: "Employee",
                column: "id_role");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeActionLog_id_employee",
                table: "EmployeeActionLog",
                column: "id_employee");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_id_club",
                table: "Equipment",
                column: "id_club");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_status",
                table: "Equipment",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMaintenance_equipment_id",
                table: "EquipmentMaintenance",
                column: "equipment_id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_account_id",
                table: "Feedback",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_id_club",
                table: "Feedback",
                column: "id_club");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_account_id",
                table: "Payment",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_Shift_id_employee",
                table: "Shift",
                column: "id_employee");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingSpace_id_club",
                table: "WorkingSpace",
                column: "id_club");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingSpace_tariff_id",
                table: "WorkingSpace",
                column: "tariff_id");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingSpaceEquipment_id_equipment",
                table: "WorkingSpaceEquipment",
                column: "id_equipment");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingSpaceEquipment_id_working_space",
                table: "WorkingSpaceEquipment",
                column: "id_working_space");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BalanceHistory");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "EmployeeActionLog");

            migrationBuilder.DropTable(
                name: "EquipmentMaintenance");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Shift");

            migrationBuilder.DropTable(
                name: "WorkingSpaceEquipment");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropTable(
                name: "BookingStatus");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "WorkingSpace");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "EquipmentStatus");

            migrationBuilder.DropTable(
                name: "Club");

            migrationBuilder.DropTable(
                name: "Tariff");
        }
    }
}

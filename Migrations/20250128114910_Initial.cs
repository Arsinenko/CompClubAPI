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
                name: "Client",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    login = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password = table.Column<byte[]>(type: "binary(32)", fixedLength: true, maxLength: 32, nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    middle_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Client__3213E83F30D0479D", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Club",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    phone = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    working_hours = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "10:00-21:00")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Club__3213E83F32C10B1A", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentStatus",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Equipmen__3213E83F2E2D25B6", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GameLauncher",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GameLaun__3213E83F9241E2D4", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role__3213E83FB39E85B7", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "WorkingSpace",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkingS__3213E83FD78B3B9E", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_client = table.Column<int>(type: "int", nullable: false),
                    balance = table.Column<decimal>(type: "money", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Account__3213E83F277E432D", x => x.id);
                    table.ForeignKey(
                        name: "FK__Account__id_clie__2B0A656D",
                        column: x => x.id_client,
                        principalTable: "Client",
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
                    link_date = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "(getdate())"),
                    client_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Payment_pk", x => x.id);
                    table.ForeignKey(
                        name: "FK__Payment__client___3587F3E0",
                        column: x => x.client_id,
                        principalTable: "Client",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserActionLog",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    client_id = table.Column<int>(type: "int", nullable: true),
                    action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    action_date = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    price = table.Column<decimal>(type: "money", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserActi__3213E83FB68F2708", x => x.id);
                    table.ForeignKey(
                        name: "FK__UserActio__clien__5224328E",
                        column: x => x.client_id,
                        principalTable: "Client",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_club = table.Column<int>(type: "int", nullable: false),
                    id_client = table.Column<int>(type: "int", nullable: true),
                    rating = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    feedback_date = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__3213E83FB7E3D431", x => x.id);
                    table.ForeignKey(
                        name: "FK__Feedback__id_cli__4A8310C6",
                        column: x => x.id_client,
                        principalTable: "Client",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Feedback__id_clu__498EEC8D",
                        column: x => x.id_club,
                        principalTable: "Club",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    price = table.Column<decimal>(type: "money", nullable: true),
                    id_launcher = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Game__3213E83F3A6D7B6C", x => x.id);
                    table.ForeignKey(
                        name: "FK__Game__id_launche__31B762FC",
                        column: x => x.id_launcher,
                        principalTable: "GameLauncher",
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
                    id_club = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Employee__3213E83F5C09894B", x => x.id);
                    table.ForeignKey(
                        name: "FK__Employee__id_clu__5F7E2DAC",
                        column: x => x.id_club,
                        principalTable: "Club",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Employee__id_rol__3A4CA8FD",
                        column: x => x.id_role,
                        principalTable: "Role",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_client = table.Column<int>(type: "int", nullable: false),
                    id_working_space = table.Column<int>(type: "int", nullable: false),
                    start_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    end_time = table.Column<DateTime>(type: "datetime", nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    total_cost = table.Column<decimal>(type: "money", nullable: true),
                    payment_method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Booking__3213E83FC1B1285F", x => x.id);
                    table.ForeignKey(
                        name: "FK__Booking__id_clie__59C55456",
                        column: x => x.id_client,
                        principalTable: "Client",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Booking__id_work__5AB9788F",
                        column: x => x.id_working_space,
                        principalTable: "WorkingSpace",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    _name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    specification = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    purchase_date = table.Column<DateOnly>(type: "date", nullable: true),
                    purchase_price = table.Column<decimal>(type: "money", nullable: true),
                    id_club = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    idWorkingSpace = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Equipmen__3213E83F38C4C1B9", x => x.id);
                    table.ForeignKey(
                        name: "FK__Equipment__idWor__73852659",
                        column: x => x.idWorkingSpace,
                        principalTable: "WorkingSpace",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Equipment__id_cl__40F9A68C",
                        column: x => x.id_club,
                        principalTable: "Club",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Equipment__statu__41EDCAC5",
                        column: x => x.status,
                        principalTable: "EquipmentStatus",
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
                    end_time = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Shift__3213E83FD85ED888", x => x.id);
                    table.ForeignKey(
                        name: "FK__Shift__id_employ__4F47C5E3",
                        column: x => x.id_employee,
                        principalTable: "Employee",
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
                    cost = table.Column<decimal>(type: "money", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Equipmen__3213E83FC092CE87", x => x.id);
                    table.ForeignKey(
                        name: "FK__Equipment__equip__45BE5BA9",
                        column: x => x.equipment_id,
                        principalTable: "Equipment",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_id_client",
                table: "Account",
                column: "id_client");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_id_client",
                table: "Booking",
                column: "id_client");

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
                name: "IX_Equipment_id_club",
                table: "Equipment",
                column: "id_club");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_idWorkingSpace",
                table: "Equipment",
                column: "idWorkingSpace");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_status",
                table: "Equipment",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMaintenance_equipment_id",
                table: "EquipmentMaintenance",
                column: "equipment_id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_id_client",
                table: "Feedback",
                column: "id_client");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_id_club",
                table: "Feedback",
                column: "id_club");

            migrationBuilder.CreateIndex(
                name: "IX_Game_id_launcher",
                table: "Game",
                column: "id_launcher");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_client_id",
                table: "Payment",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_Shift_id_employee",
                table: "Shift",
                column: "id_employee");

            migrationBuilder.CreateIndex(
                name: "IX_UserActionLog_client_id",
                table: "UserActionLog",
                column: "client_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "EquipmentMaintenance");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Shift");

            migrationBuilder.DropTable(
                name: "UserActionLog");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "GameLauncher");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "WorkingSpace");

            migrationBuilder.DropTable(
                name: "EquipmentStatus");

            migrationBuilder.DropTable(
                name: "Club");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MilitaryProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ammunitions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ammunitions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Brigades",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CommanderName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EstablishmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brigades", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrigadeID = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Users_Brigades_BrigadeID",
                        column: x => x.BrigadeID,
                        principalTable: "Brigades",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrigadeStorages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrigadeID = table.Column<int>(type: "int", nullable: false),
                    WeaponID = table.Column<int>(type: "int", nullable: false),
                    AmmunitionID = table.Column<int>(type: "int", nullable: false),
                    WeaponQuantity = table.Column<int>(type: "int", nullable: false),
                    AmmunitionQuantity = table.Column<int>(type: "int", nullable: false),
                    WeaponRemainder = table.Column<int>(type: "int", nullable: false),
                    AmmunitionRemainder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrigadeStorages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BrigadeStorages_Ammunitions_AmmunitionID",
                        column: x => x.AmmunitionID,
                        principalTable: "Ammunitions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrigadeStorages_Brigades_BrigadeID",
                        column: x => x.BrigadeID,
                        principalTable: "Brigades",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrigadeStorages_Weapons_WeaponID",
                        column: x => x.WeaponID,
                        principalTable: "Weapons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrigadeID = table.Column<int>(type: "int", nullable: false),
                    WeaponID = table.Column<int>(type: "int", nullable: false),
                    AmmunitionID = table.Column<int>(type: "int", nullable: false),
                    WeaponQuantity = table.Column<int>(type: "int", nullable: false),
                    AmmunitionQuantity = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Requests_Ammunitions_AmmunitionID",
                        column: x => x.AmmunitionID,
                        principalTable: "Ammunitions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_Brigades_BrigadeID",
                        column: x => x.BrigadeID,
                        principalTable: "Brigades",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_Weapons_WeaponID",
                        column: x => x.WeaponID,
                        principalTable: "Weapons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MilitaryRoutes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VolunteerID = table.Column<int>(type: "int", nullable: false),
                    WeaponID = table.Column<int>(type: "int", nullable: false),
                    AmmunitionID = table.Column<int>(type: "int", nullable: false),
                    StartPoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeaponQuantity = table.Column<int>(type: "int", nullable: false),
                    AmmunitionQuantity = table.Column<int>(type: "int", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilitaryRoutes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MilitaryRoutes_Ammunitions_AmmunitionID",
                        column: x => x.AmmunitionID,
                        principalTable: "Ammunitions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MilitaryRoutes_Users_VolunteerID",
                        column: x => x.VolunteerID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MilitaryRoutes_Weapons_WeaponID",
                        column: x => x.WeaponID,
                        principalTable: "Weapons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserItems",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    WeaponID = table.Column<int>(type: "int", nullable: false),
                    AmmunitionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserItems_Ammunitions_AmmunitionID",
                        column: x => x.AmmunitionID,
                        principalTable: "Ammunitions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserItems_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserItems_Weapons_WeaponID",
                        column: x => x.WeaponID,
                        principalTable: "Weapons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrigadeStorages_AmmunitionID",
                table: "BrigadeStorages",
                column: "AmmunitionID");

            migrationBuilder.CreateIndex(
                name: "IX_BrigadeStorages_BrigadeID",
                table: "BrigadeStorages",
                column: "BrigadeID");

            migrationBuilder.CreateIndex(
                name: "IX_BrigadeStorages_WeaponID",
                table: "BrigadeStorages",
                column: "WeaponID");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryRoutes_AmmunitionID",
                table: "MilitaryRoutes",
                column: "AmmunitionID");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryRoutes_VolunteerID",
                table: "MilitaryRoutes",
                column: "VolunteerID");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryRoutes_WeaponID",
                table: "MilitaryRoutes",
                column: "WeaponID");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_AmmunitionID",
                table: "Requests",
                column: "AmmunitionID");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_BrigadeID",
                table: "Requests",
                column: "BrigadeID");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_WeaponID",
                table: "Requests",
                column: "WeaponID");

            migrationBuilder.CreateIndex(
                name: "IX_UserItems_AmmunitionID",
                table: "UserItems",
                column: "AmmunitionID");

            migrationBuilder.CreateIndex(
                name: "IX_UserItems_UserID",
                table: "UserItems",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserItems_WeaponID",
                table: "UserItems",
                column: "WeaponID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_BrigadeID",
                table: "Users",
                column: "BrigadeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrigadeStorages");

            migrationBuilder.DropTable(
                name: "MilitaryRoutes");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "UserItems");

            migrationBuilder.DropTable(
                name: "Ammunitions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Weapons");

            migrationBuilder.DropTable(
                name: "Brigades");
        }
    }
}

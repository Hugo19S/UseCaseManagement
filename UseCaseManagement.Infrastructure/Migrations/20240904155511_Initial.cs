using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UseCaseManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogSource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogSource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UseCase",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Tag = table.Column<string>(type: "text", nullable: true),
                    Priority = table.Column<string>(type: "text", nullable: false),
                    MitreAttacks = table.Column<List<string>>(type: "text[]", nullable: false),
                    Tenants = table.Column<List<Guid>>(type: "uuid[]", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UseCase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogSourceFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LogSourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FileSize = table.Column<int>(type: "integer", nullable: false),
                    Uri = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogSourceFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogSourceFile_LogSource_LogSourceId",
                        column: x => x.LogSourceId,
                        principalTable: "LogSource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UseCaseFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UseCaseId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FileSize = table.Column<int>(type: "integer", nullable: false),
                    Uri = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UseCaseFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UseCaseFile_UseCase_UseCaseId",
                        column: x => x.UseCaseId,
                        principalTable: "UseCase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UseCaseLogSources",
                columns: table => new
                {
                    LogSourcesId = table.Column<Guid>(type: "uuid", nullable: false),
                    UseCasesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UseCaseLogSources", x => new { x.LogSourcesId, x.UseCasesId });
                    table.ForeignKey(
                        name: "FK_UseCaseLogSources_LogSource_LogSourcesId",
                        column: x => x.LogSourcesId,
                        principalTable: "LogSource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UseCaseLogSources_UseCase_UseCasesId",
                        column: x => x.UseCasesId,
                        principalTable: "UseCase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendorUseCase",
                columns: table => new
                {
                    UseCasesId = table.Column<Guid>(type: "uuid", nullable: false),
                    VendorsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorUseCase", x => new { x.UseCasesId, x.VendorsId });
                    table.ForeignKey(
                        name: "FK_VendorUseCase_UseCase_UseCasesId",
                        column: x => x.UseCasesId,
                        principalTable: "UseCase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VendorUseCase_Vendor_VendorsId",
                        column: x => x.VendorsId,
                        principalTable: "Vendor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogSource_Name",
                table: "LogSource",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LogSourceFile_LogSourceId",
                table: "LogSourceFile",
                column: "LogSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_UseCase_Title",
                table: "UseCase",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UseCaseFile_UseCaseId",
                table: "UseCaseFile",
                column: "UseCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_UseCaseLogSources_UseCasesId",
                table: "UseCaseLogSources",
                column: "UseCasesId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_Name",
                table: "Vendor",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VendorUseCase_VendorsId",
                table: "VendorUseCase",
                column: "VendorsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogSourceFile");

            migrationBuilder.DropTable(
                name: "UseCaseFile");

            migrationBuilder.DropTable(
                name: "UseCaseLogSources");

            migrationBuilder.DropTable(
                name: "VendorUseCase");

            migrationBuilder.DropTable(
                name: "LogSource");

            migrationBuilder.DropTable(
                name: "UseCase");

            migrationBuilder.DropTable(
                name: "Vendor");
        }
    }
}

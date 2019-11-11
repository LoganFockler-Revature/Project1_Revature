using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankModels.Migrations
{
    public partial class business : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Accounts",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Overdraft",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "OverdraftCost",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "OverdraftDueDate",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MonthlyDue",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Maturity",
                table: "Accounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Overdraft",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "OverdraftCost",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "OverdraftDueDate",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "MonthlyDue",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Maturity",
                table: "Accounts");
        }
    }
}

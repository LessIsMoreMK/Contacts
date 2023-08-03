using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contacts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubCattegoryIdToContacts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "sub_category_id",
                table: "contacts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_contacts_sub_category_id",
                table: "contacts",
                column: "sub_category_id");

            migrationBuilder.AddForeignKey(
                name: "fk_contacts_subcategories_sub_category_id",
                table: "contacts",
                column: "sub_category_id",
                principalTable: "subcategories",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_contacts_subcategories_sub_category_id",
                table: "contacts");

            migrationBuilder.DropIndex(
                name: "ix_contacts_sub_category_id",
                table: "contacts");

            migrationBuilder.DropColumn(
                name: "sub_category_id",
                table: "contacts");
        }
    }
}

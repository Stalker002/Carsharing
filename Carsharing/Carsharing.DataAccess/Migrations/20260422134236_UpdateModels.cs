using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Carsharing.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "bill_statuses",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "bill_statuses",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "bill_statuses",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "bill_statuses",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "booking_statuses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "booking_statuses",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "booking_statuses",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "fine_statuses",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "fine_statuses",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "fine_statuses",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "insurance_statuses",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "insurance_statuses",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "insurance_statuses",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "promocode_statuses",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "promocode_statuses",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "promocode_statuses",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "trip_statuses",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "trip_statuses",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "trip_statuses",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "trip_statuses",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "trip_statuses",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.InsertData(
                table: "bill_statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Не оплачен" },
                    { 2, "Частично оплачен" },
                    { 3, "Оплачен" },
                    { 4, "Отменён" }
                });

            migrationBuilder.InsertData(
                table: "booking_statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Активно" },
                    { 2, "Завершено" },
                    { 3, "Отменено" }
                });

            migrationBuilder.InsertData(
                table: "fine_statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Начислен" },
                    { 2, "Ожидает оплаты" },
                    { 3, "Оплачен" }
                });

            migrationBuilder.InsertData(
                table: "insurance_statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Активна" },
                    { 2, "Истекла" },
                    { 3, "Аннулирована" }
                });

            migrationBuilder.InsertData(
                table: "promocode_statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Активен" },
                    { 2, "Истёк" },
                    { 3, "Использован" }
                });

            migrationBuilder.InsertData(
                table: "trip_statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Ожидание начала" },
                    { 2, "В пути" },
                    { 3, "Завершена" },
                    { 4, "Отменена системой" },
                    { 5, "Требуется оплата" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "bill_statuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "bill_statuses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "bill_statuses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "bill_statuses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "booking_statuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "booking_statuses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "booking_statuses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "fine_statuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "fine_statuses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "fine_statuses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "insurance_statuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "insurance_statuses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "insurance_statuses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "promocode_statuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "promocode_statuses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "promocode_statuses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "trip_statuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "trip_statuses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "trip_statuses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "trip_statuses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "trip_statuses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.InsertData(
                table: "bill_statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 13, "Не оплачен" },
                    { 14, "Частично оплачен" },
                    { 15, "Оплачен" },
                    { 16, "Отменён" }
                });

            migrationBuilder.InsertData(
                table: "booking_statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 5, "Активно" },
                    { 6, "Завершено" },
                    { 7, "Отменено" }
                });

            migrationBuilder.InsertData(
                table: "fine_statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 17, "Начислен" },
                    { 18, "Ожидает оплаты" },
                    { 19, "Оплачен" }
                });

            migrationBuilder.InsertData(
                table: "insurance_statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 23, "Активна" },
                    { 24, "Истекла" },
                    { 25, "Аннулирована" }
                });

            migrationBuilder.InsertData(
                table: "promocode_statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 20, "Активен" },
                    { 21, "Истёк" },
                    { 22, "Использован" }
                });

            migrationBuilder.InsertData(
                table: "trip_statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 8, "Ожидание начала" },
                    { 9, "В пути" },
                    { 10, "Завершена" },
                    { 11, "Отменена системой" },
                    { 12, "Требуется оплата" }
                });
        }
    }
}

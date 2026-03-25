using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Carsharing.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RebuildSchemaFromDump : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bill_statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill_statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "booking_statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "car_statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_car_statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    category_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "fine_statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fine_statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "insurance_statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insurance_statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "promocode_statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promocode_statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "specifications_car",
                columns: table => new
                {
                    specification_car_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    specification_car_fuel_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    specification_car_brand = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    specification_car_model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    specification_car_transmission = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    specification_car_year = table.Column<int>(type: "integer", nullable: false),
                    specification_car_vin_number = table.Column<string>(type: "character varying(17)", maxLength: 17, nullable: false),
                    specification_car_state_number = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    specification_car_mileage = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    specification_car_max_fuel = table.Column<decimal>(type: "numeric", nullable: false),
                    specification_fuel_per_km = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0.08m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specifications_car", x => x.specification_car_id);
                    table.CheckConstraint("chk_specification_car_max_fuel_positive", "specification_car_max_fuel >= 0");
                    table.CheckConstraint("chk_specification_car_mileage_non_negative", "specification_car_mileage >= 0");
                    table.CheckConstraint("chk_specification_car_year_range", "specification_car_year >= 1900 AND specification_car_year <= EXTRACT(year FROM CURRENT_DATE) + 1");
                    table.CheckConstraint("specifications_car_specification_fuel_per_km_check", "specification_fuel_per_km >= 0");
                });

            migrationBuilder.CreateTable(
                name: "tariffs",
                columns: table => new
                {
                    tariff_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tariff_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tariff_price_per_minute = table.Column<decimal>(type: "numeric", nullable: false),
                    tariff_price_per_km = table.Column<decimal>(type: "numeric", nullable: false),
                    tariff_price_per_day = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tariffs", x => x.tariff_id);
                    table.CheckConstraint("chk_tariff_prices_nonneg", "COALESCE(tariff_price_per_minute, 0) >= 0 AND COALESCE(tariff_price_per_km, 0) >= 0 AND COALESCE(tariff_price_per_day, 0) >= 0");
                });

            migrationBuilder.CreateTable(
                name: "trip_statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trip_statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "promocodes",
                columns: table => new
                {
                    promocode_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    promocode_status_id = table.Column<int>(type: "integer", nullable: false),
                    promocode_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    promocode_discount = table.Column<decimal>(type: "numeric", nullable: false),
                    promocode_start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    promocode_end_date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promocodes", x => x.promocode_id);
                    table.CheckConstraint("chk_promocode_dates", "promocode_start_date IS NULL OR promocode_end_date IS NULL OR promocode_start_date <= promocode_end_date");
                    table.ForeignKey(
                        name: "FK_promocodes_promocode_statuses_promocode_status_id",
                        column: x => x.promocode_status_id,
                        principalTable: "promocode_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_role_id = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    user_password_hash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_users_roles_user_role_id",
                        column: x => x.user_role_id,
                        principalTable: "roles",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cars",
                columns: table => new
                {
                    car_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    car_status_id = table.Column<int>(type: "integer", nullable: false),
                    car_tariff_id = table.Column<int>(type: "integer", nullable: false),
                    car_category_id = table.Column<int>(type: "integer", nullable: false),
                    car_specification_id = table.Column<int>(type: "integer", nullable: false),
                    car_location = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    car_fuel_level = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    car_image_path = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cars", x => x.car_id);
                    table.CheckConstraint("chk_fuel_level", "car_fuel_level >= 0");
                    table.ForeignKey(
                        name: "FK_cars_car_statuses_car_status_id",
                        column: x => x.car_status_id,
                        principalTable: "car_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cars_categories_car_category_id",
                        column: x => x.car_category_id,
                        principalTable: "categories",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cars_specifications_car_car_specification_id",
                        column: x => x.car_specification_id,
                        principalTable: "specifications_car",
                        principalColumn: "specification_car_id");
                    table.ForeignKey(
                        name: "FK_cars_tariffs_car_tariff_id",
                        column: x => x.car_tariff_id,
                        principalTable: "tariffs",
                        principalColumn: "tariff_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    client_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    client_user_id = table.Column<int>(type: "integer", nullable: false),
                    client_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    client_surname = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    client_phone_number = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    client_email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.client_id);
                    table.CheckConstraint("chk_client_email_format", "client_email ~* '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$'");
                    table.ForeignKey(
                        name: "FK_clients_users_client_user_id",
                        column: x => x.client_user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "insurance",
                columns: table => new
                {
                    insurance_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    insurance_car_id = table.Column<int>(type: "integer", nullable: false),
                    insurance_status_id = table.Column<int>(type: "integer", nullable: false),
                    insurance_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    insurance_company = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    insurance_policy_number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    insurance_start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    insurance_end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    insurance_cost = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insurance", x => x.insurance_id);
                    table.CheckConstraint("chk_insurance_dates", "insurance_start_date IS NULL OR insurance_end_date IS NULL OR insurance_start_date <= insurance_end_date");
                    table.ForeignKey(
                        name: "FK_insurance_cars_insurance_car_id",
                        column: x => x.insurance_car_id,
                        principalTable: "cars",
                        principalColumn: "car_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_insurance_insurance_statuses_insurance_status_id",
                        column: x => x.insurance_status_id,
                        principalTable: "insurance_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "maintenance",
                columns: table => new
                {
                    maintenance_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    maintenance_car_id = table.Column<int>(type: "integer", nullable: false),
                    maintenance_work_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    maintenance_description = table.Column<string>(type: "text", nullable: false),
                    maintenance_cost = table.Column<decimal>(type: "numeric", nullable: false),
                    maintenance_date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_maintenance", x => x.maintenance_id);
                    table.CheckConstraint("chk_maintenance_cost_nonneg", "maintenance_cost IS NULL OR maintenance_cost >= 0");
                    table.ForeignKey(
                        name: "FK_maintenance_cars_maintenance_car_id",
                        column: x => x.maintenance_car_id,
                        principalTable: "cars",
                        principalColumn: "car_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    booking_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    booking_status_id = table.Column<int>(type: "integer", nullable: false),
                    booking_car_id = table.Column<int>(type: "integer", nullable: false),
                    booking_client_id = table.Column<int>(type: "integer", nullable: false),
                    booking_start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    booking_end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookings", x => x.booking_id);
                    table.CheckConstraint("chk_booking_times", "booking_start_time IS NULL OR booking_end_time IS NULL OR booking_start_time < booking_end_time");
                    table.ForeignKey(
                        name: "FK_bookings_booking_statuses_booking_status_id",
                        column: x => x.booking_status_id,
                        principalTable: "booking_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bookings_cars_booking_car_id",
                        column: x => x.booking_car_id,
                        principalTable: "cars",
                        principalColumn: "car_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bookings_clients_booking_client_id",
                        column: x => x.booking_client_id,
                        principalTable: "clients",
                        principalColumn: "client_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client_documents",
                columns: table => new
                {
                    document_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    document_client_id = table.Column<int>(type: "integer", nullable: false),
                    document_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    document_license_category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    document_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    document_issue_date = table.Column<DateOnly>(type: "date", nullable: false),
                    document_expiry_date = table.Column<DateOnly>(type: "date", nullable: false),
                    document_file_path = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client_documents", x => x.document_id);
                    table.ForeignKey(
                        name: "FK_client_documents_clients_document_client_id",
                        column: x => x.document_client_id,
                        principalTable: "clients",
                        principalColumn: "client_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "favorite_cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    CarId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favorite_cars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_favorite_cars_cars_CarId",
                        column: x => x.CarId,
                        principalTable: "cars",
                        principalColumn: "car_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_favorite_cars_clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "clients",
                        principalColumn: "client_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    review_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    review_client_id = table.Column<int>(type: "integer", nullable: false),
                    review_car_id = table.Column<int>(type: "integer", nullable: false),
                    review_rating = table.Column<short>(type: "smallint", nullable: false),
                    review_comment = table.Column<string>(type: "text", nullable: false),
                    review_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviews", x => x.review_id);
                    table.CheckConstraint("reviews_review_rating_check", "review_rating >= 1 AND review_rating <= 5");
                    table.ForeignKey(
                        name: "FK_reviews_cars_review_car_id",
                        column: x => x.review_car_id,
                        principalTable: "cars",
                        principalColumn: "car_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reviews_clients_review_client_id",
                        column: x => x.review_client_id,
                        principalTable: "clients",
                        principalColumn: "client_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trips",
                columns: table => new
                {
                    trip_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    trip_booking_id = table.Column<int>(type: "integer", nullable: false),
                    trip_status_id = table.Column<int>(type: "integer", nullable: false),
                    trip_tariff_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    trip_start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    trip_end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    trip_duration = table.Column<decimal>(type: "numeric", nullable: true, defaultValue: 0m),
                    trip_distance_km = table.Column<decimal>(type: "numeric", nullable: true, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trips", x => x.trip_id);
                    table.CheckConstraint("chk_trip_distance_nonneg", "trip_distance_km IS NULL OR trip_distance_km >= 0");
                    table.CheckConstraint("chk_trip_duration_nonneg", "trip_duration IS NULL OR trip_duration >= 0");
                    table.CheckConstraint("chk_trip_times", "trip_start_time IS NULL OR trip_end_time IS NULL OR trip_start_time <= trip_end_time");
                    table.ForeignKey(
                        name: "FK_trips_bookings_trip_booking_id",
                        column: x => x.trip_booking_id,
                        principalTable: "bookings",
                        principalColumn: "booking_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_trips_trip_statuses_trip_status_id",
                        column: x => x.trip_status_id,
                        principalTable: "trip_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bills",
                columns: table => new
                {
                    bill_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    bill_trip_id = table.Column<int>(type: "integer", nullable: false),
                    bill_promocode_id = table.Column<int>(type: "integer", nullable: true),
                    bill_status_id = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    bill_issue_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    bill_amount = table.Column<decimal>(type: "numeric", nullable: true),
                    bill_remaining_amount = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bills", x => x.bill_id);
                    table.CheckConstraint("bills_bill_remaining_amount_check", "bill_remaining_amount >= 0");
                    table.CheckConstraint("chk_bill_amount_nonneg", "bill_amount IS NULL OR bill_amount >= 0");
                    table.ForeignKey(
                        name: "FK_bills_bill_statuses_bill_status_id",
                        column: x => x.bill_status_id,
                        principalTable: "bill_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bills_promocodes_bill_promocode_id",
                        column: x => x.bill_promocode_id,
                        principalTable: "promocodes",
                        principalColumn: "promocode_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_bills_trips_bill_trip_id",
                        column: x => x.bill_trip_id,
                        principalTable: "trips",
                        principalColumn: "trip_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "fines",
                columns: table => new
                {
                    fine_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fine_trip_id = table.Column<int>(type: "integer", nullable: false),
                    fine_status_id = table.Column<int>(type: "integer", nullable: false),
                    fine_type = table.Column<string>(type: "text", nullable: false),
                    fine_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    fine_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_DATE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fines", x => x.fine_id);
                    table.CheckConstraint("chk_fine_amount_nonneg", "fine_amount >= 0");
                    table.ForeignKey(
                        name: "FK_fines_fine_statuses_fine_status_id",
                        column: x => x.fine_status_id,
                        principalTable: "fine_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_fines_trips_fine_trip_id",
                        column: x => x.fine_trip_id,
                        principalTable: "trips",
                        principalColumn: "trip_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trip_details",
                columns: table => new
                {
                    trip_detail_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    trip_detail_trip_id = table.Column<int>(type: "integer", nullable: false),
                    trip_detail_start_location = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    trip_detail_end_location = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    trip_detail_insurance_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    trip_detail_fuel_used = table.Column<decimal>(type: "numeric", nullable: true, defaultValue: 0m),
                    trip_detail_refueled = table.Column<decimal>(type: "numeric", nullable: true, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trip_details", x => x.trip_detail_id);
                    table.CheckConstraint("chk_trip_detail_fuel_used_nonneg", "trip_detail_fuel_used IS NULL OR trip_detail_fuel_used >= 0");
                    table.CheckConstraint("chk_trip_detail_refueled_nonneg", "trip_detail_refueled IS NULL OR trip_detail_refueled >= 0");
                    table.ForeignKey(
                        name: "FK_trip_details_trips_trip_detail_trip_id",
                        column: x => x.trip_detail_trip_id,
                        principalTable: "trips",
                        principalColumn: "trip_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    payment_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    payment_bill_id = table.Column<int>(type: "integer", nullable: false),
                    payment_sum = table.Column<decimal>(type: "numeric", nullable: false),
                    payment_method = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Картой"),
                    payment_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.payment_id);
                    table.ForeignKey(
                        name: "FK_payments_bills_payment_bill_id",
                        column: x => x.payment_bill_id,
                        principalTable: "bills",
                        principalColumn: "bill_id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                table: "car_statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Доступна" },
                    { 2, "Недоступен" },
                    { 3, "На обслуживании" },
                    { 4, "В ремонте" }
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
                table: "roles",
                columns: new[] { "role_id", "role_name" },
                values: new object[,]
                {
                    { 1, "Администратор" },
                    { 2, "Клиент" }
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

            migrationBuilder.CreateIndex(
                name: "IX_bills_bill_promocode_id",
                table: "bills",
                column: "bill_promocode_id");

            migrationBuilder.CreateIndex(
                name: "IX_bills_bill_status_id",
                table: "bills",
                column: "bill_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_bills_bill_trip_id",
                table: "bills",
                column: "bill_trip_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookings_booking_car_id",
                table: "bookings",
                column: "booking_car_id");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_booking_client_id",
                table: "bookings",
                column: "booking_client_id");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_booking_status_id",
                table: "bookings",
                column: "booking_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_cars_car_category_id",
                table: "cars",
                column: "car_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_cars_car_specification_id",
                table: "cars",
                column: "car_specification_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cars_car_status_id",
                table: "cars",
                column: "car_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_cars_car_tariff_id",
                table: "cars",
                column: "car_tariff_id");

            migrationBuilder.CreateIndex(
                name: "IX_client_documents_document_client_id",
                table: "client_documents",
                column: "document_client_id");

            migrationBuilder.CreateIndex(
                name: "IX_clients_client_email",
                table: "clients",
                column: "client_email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clients_client_phone_number",
                table: "clients",
                column: "client_phone_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clients_client_user_id",
                table: "clients",
                column: "client_user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_favorite_cars_CarId",
                table: "favorite_cars",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_favorite_cars_ClientId",
                table: "favorite_cars",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_fines_fine_status_id",
                table: "fines",
                column: "fine_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_fines_fine_trip_id",
                table: "fines",
                column: "fine_trip_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_insurance_car_id",
                table: "insurance",
                column: "insurance_car_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_insurance_policy_number",
                table: "insurance",
                column: "insurance_policy_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_insurance_insurance_status_id",
                table: "insurance",
                column: "insurance_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_maintenance_maintenance_car_id",
                table: "maintenance",
                column: "maintenance_car_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_payment_bill_id",
                table: "payments",
                column: "payment_bill_id");

            migrationBuilder.CreateIndex(
                name: "IX_promocodes_promocode_code",
                table: "promocodes",
                column: "promocode_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_promocodes_promocode_status_id",
                table: "promocodes",
                column: "promocode_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_review_car_id",
                table: "reviews",
                column: "review_car_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_review_client_id",
                table: "reviews",
                column: "review_client_id");

            migrationBuilder.CreateIndex(
                name: "IX_specifications_car_specification_car_state_number",
                table: "specifications_car",
                column: "specification_car_state_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_specifications_car_specification_car_vin_number",
                table: "specifications_car",
                column: "specification_car_vin_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trip_details_trip_detail_trip_id",
                table: "trip_details",
                column: "trip_detail_trip_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trips_trip_booking_id",
                table: "trips",
                column: "trip_booking_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trips_trip_status_id",
                table: "trips",
                column: "trip_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_user_login",
                table: "users",
                column: "user_login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_user_role_id",
                table: "users",
                column: "user_role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "client_documents");

            migrationBuilder.DropTable(
                name: "favorite_cars");

            migrationBuilder.DropTable(
                name: "fines");

            migrationBuilder.DropTable(
                name: "insurance");

            migrationBuilder.DropTable(
                name: "maintenance");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "trip_details");

            migrationBuilder.DropTable(
                name: "fine_statuses");

            migrationBuilder.DropTable(
                name: "insurance_statuses");

            migrationBuilder.DropTable(
                name: "bills");

            migrationBuilder.DropTable(
                name: "bill_statuses");

            migrationBuilder.DropTable(
                name: "promocodes");

            migrationBuilder.DropTable(
                name: "trips");

            migrationBuilder.DropTable(
                name: "promocode_statuses");

            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "trip_statuses");

            migrationBuilder.DropTable(
                name: "booking_statuses");

            migrationBuilder.DropTable(
                name: "cars");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropTable(
                name: "car_statuses");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "specifications_car");

            migrationBuilder.DropTable(
                name: "tariffs");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carsharing.DataAccess.Migrations;

/// <summary>
/// Removes legacy PostgreSQL triggers/functions whose business logic now lives in the application layer.
/// </summary>
public partial class RemoveDatabaseBusinessTriggers : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            DROP TRIGGER IF EXISTS trg_apply_payment_to_bill ON public.payments;
            DROP TRIGGER IF EXISTS trg_calculate_bill_total ON public.bills;
            DROP TRIGGER IF EXISTS trg_create_bill_after_trip ON public.trips;
            DROP TRIGGER IF EXISTS trg_prevent_overpayment ON public.payments;
            DROP TRIGGER IF EXISTS trg_set_initial_remaining_amount ON public.bills;
            DROP TRIGGER IF EXISTS trg_set_trip_fuel_used ON public.trip_details;
            DROP TRIGGER IF EXISTS trg_trip_duration ON public.trips;
            DROP TRIGGER IF EXISTS trg_update_car_fuel_after_trip ON public.trip_details;

            DROP FUNCTION IF EXISTS public.apply_payment_to_bill();
            DROP FUNCTION IF EXISTS public.calculate_bill_total();
            DROP FUNCTION IF EXISTS public.create_bill_after_trip();
            DROP FUNCTION IF EXISTS public.prevent_overpayment();
            DROP FUNCTION IF EXISTS public.set_initial_remaining_amount();
            DROP FUNCTION IF EXISTS public.set_trip_fuel_used();
            DROP FUNCTION IF EXISTS public.calculate_trip_duration();
            DROP FUNCTION IF EXISTS public.update_car_fuel_after_trip();
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // The legacy trigger implementation intentionally stays removed.
        // Business rules are now executed in the backend application layer.
    }
}

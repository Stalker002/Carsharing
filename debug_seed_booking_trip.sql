BEGIN;

-- Debug seed for local PostgreSQL / PostGIS environment.
-- Creates:
-- 1. client user
-- 2. client profile
-- 3. tariff + category
-- 4. BMW car with image BMW%203.png
-- 5. active booking
-- 6. active trip + trip details
--
-- Safe to run multiple times: old debug records are deleted first.

DO $$
DECLARE
    v_user_id integer;
    v_client_id integer;
    v_tariff_id integer;
    v_category_id integer;
    v_spec_id integer;
    v_car_id integer;
    v_booking_id integer;
    v_trip_id integer;
BEGIN
    -- Cleanup old debug chain.
    DELETE FROM trip_details
    WHERE trip_detail_trip_id IN (
        SELECT t.trip_id
        FROM trips t
        JOIN bookings b ON b.booking_id = t.trip_booking_id
        JOIN clients c ON c.client_id = b.booking_client_id
        JOIN users u ON u.user_id = c.client_user_id
        WHERE u.user_login = 'debug_bmw_trip_user'
    );

    DELETE FROM trips
    WHERE trip_booking_id IN (
        SELECT b.booking_id
        FROM bookings b
        JOIN clients c ON c.client_id = b.booking_client_id
        JOIN users u ON u.user_id = c.client_user_id
        WHERE u.user_login = 'debug_bmw_trip_user'
    );

    DELETE FROM bookings
    WHERE booking_client_id IN (
        SELECT c.client_id
        FROM clients c
        JOIN users u ON u.user_id = c.client_user_id
        WHERE u.user_login = 'debug_bmw_trip_user'
    );

    DELETE FROM clients
    WHERE client_user_id IN (
        SELECT user_id
        FROM users
        WHERE user_login = 'debug_bmw_trip_user'
    );

    DELETE FROM users
    WHERE user_login = 'debug_bmw_trip_user';

    DELETE FROM cars
    WHERE car_specification_id IN (
        SELECT specification_car_id
        FROM specifications_car
        WHERE specification_car_vin_number = 'WBAEV33444KL98765'
           OR specification_car_state_number = '1234 AB-7'
    );

    DELETE FROM specifications_car
    WHERE specification_car_vin_number = 'WBAEV33444KL98765'
       OR specification_car_state_number = '1234 AB-7';

    DELETE FROM tariffs
    WHERE tariff_name = 'debug_bmw_trip_tariff';

    DELETE FROM categories
    WHERE category_name = 'debug_bmw_trip_category';

    INSERT INTO users (user_role_id, user_login, user_password_hash)
    VALUES (
        2,
        'debug_bmw_trip_user',
        '$2a$11$9tcYC0YbZ0ztz.YsqkUr1e9pbIpXOgIf3w8p/dVjXoyf/ZBOEmTy.'
    )
    RETURNING user_id INTO v_user_id;

    INSERT INTO clients (
        client_user_id,
        client_name,
        client_surname,
        client_phone_number,
        client_email
    )
    VALUES (
        v_user_id,
        'Debug',
        'BMWTrip',
        '+375291112233',
        'debug.bmw.trip@example.com'
    )
    RETURNING client_id INTO v_client_id;

    INSERT INTO tariffs (
        tariff_name,
        tariff_price_per_minute,
        tariff_price_per_km,
        tariff_price_per_day
    )
    VALUES (
        'debug_bmw_trip_tariff',
        0.45,
        1.10,
        95.00
    )
    RETURNING tariff_id INTO v_tariff_id;

    INSERT INTO categories (category_name)
    VALUES ('debug_bmw_trip_category')
    RETURNING category_id INTO v_category_id;

    INSERT INTO specifications_car (
        specification_car_fuel_type,
        specification_car_brand,
        specification_car_model,
        specification_car_transmission,
        specification_car_year,
        specification_car_vin_number,
        specification_car_state_number,
        specification_car_mileage,
        specification_car_max_fuel,
        specification_fuel_per_km
    )
    VALUES (
        'Бензин',
        'BMW',
        '3 Series',
        'Автомат',
        2024,
        'WBAEV33444KL98765',
        '1234 AB-7',
        12500,
        59.00,
        0.08
    )
    RETURNING specification_car_id INTO v_spec_id;

    INSERT INTO cars (
        car_status_id,
        car_tariff_id,
        car_category_id,
        car_specification_id,
        car_location,
        car_coordinates,
        car_fuel_level,
        car_image_path
    )
    VALUES (
        2,
        v_tariff_id,
        v_category_id,
        v_spec_id,
        'Минск, проспект Победителей 9',
        ST_SetSRID(ST_MakePoint(27.5415, 53.9080), 4326),
        41.50,
        'BMW%203.png'
    )
    RETURNING car_id INTO v_car_id;

    INSERT INTO bookings (
        booking_status_id,
        booking_car_id,
        booking_client_id,
        booking_start_time,
        booking_end_time
    )
    VALUES (
        1,
        v_car_id,
        v_client_id,
        NOW() - INTERVAL '35 minutes',
        NOW() + INTERVAL '2 hours'
    )
    RETURNING booking_id INTO v_booking_id;

    INSERT INTO trips (
        trip_booking_id,
        trip_status_id,
        trip_tariff_type,
        trip_start_time,
        trip_end_time,
        trip_duration,
        trip_distance_km
    )
    VALUES (
        v_booking_id,
        1,
        'per_minute',
        NOW() - INTERVAL '20 minutes',
        NULL,
        20,
        8.4
    )
    RETURNING trip_id INTO v_trip_id;

    INSERT INTO trip_details (
        trip_detail_trip_id,
        trip_detail_start_location,
        trip_detail_end_location,
        trip_detail_insurance_active,
        trip_detail_fuel_used,
        trip_detail_refueled
    )
    VALUES (
        v_trip_id,
        'Минск, улица Немига 5',
        'Минск, проспект Победителей 9',
        TRUE,
        0.67,
        0
    );

    RAISE NOTICE 'Debug user_id=%', v_user_id;
    RAISE NOTICE 'Debug client_id=%', v_client_id;
    RAISE NOTICE 'Debug car_id=%', v_car_id;
    RAISE NOTICE 'Debug booking_id=%', v_booking_id;
    RAISE NOTICE 'Debug trip_id=%', v_trip_id;
    RAISE NOTICE 'Login: debug_bmw_trip_user';
    RAISE NOTICE 'Password: DebugPassword123!';
END $$;

COMMIT;

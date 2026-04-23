BEGIN;

-- Frontend debug seed for local PostgreSQL / PostGIS environment.
--
-- Login:    front_test_user
-- Password: DebugPassword123!
--
-- The data set is intentionally broad enough to check the mobile UI:
-- - map/main page: several available cars with different categories, tariffs, fuel levels and coordinates;
-- - booking card: one active booking that is visible on the main page;
-- - current trip: one active trip with live dashboard data;
-- - trip history: finished, paid, payment-required and cancelled-looking cases;
-- - bills: unpaid, partially paid, paid and cancelled bills;
-- - bill details: payment history and an active promocode FRONT20.
--
-- Safe to run multiple times: old frontend debug records are deleted first.

DO $$
DECLARE
    v_user_id integer;
    v_client_id integer;
    v_tariff_city_id integer;
    v_tariff_comfort_id integer;
    v_tariff_premium_id integer;
    v_category_city_id integer;
    v_category_business_id integer;
    v_category_electric_id integer;
    v_category_suv_id integer;
    v_promo_id integer;
    v_car_bmw_id integer;
    v_car_renault_id integer;
    v_car_vw_id integer;
    v_car_tesla_id integer;
    v_car_kia_id integer;
    v_car_audi_id integer;
    v_booking_id integer;
    v_trip_id integer;
    v_bill_id integer;
BEGIN
    -- Cleanup records created by previous versions of this debug seed.
    DELETE FROM payments
    WHERE payment_bill_id IN (
        SELECT bill_id
        FROM bills
        JOIN trips ON trips.trip_id = bills.bill_trip_id
        JOIN bookings ON bookings.booking_id = trips.trip_booking_id
        JOIN clients ON clients.client_id = bookings.booking_client_id
        JOIN users ON users.user_id = clients.client_user_id
        WHERE users.user_login IN ('front_test_user', 'debug_bmw_trip_user')
    );

    DELETE FROM bills
    WHERE bill_trip_id IN (
        SELECT trip_id
        FROM trips
        JOIN bookings ON bookings.booking_id = trips.trip_booking_id
        JOIN clients ON clients.client_id = bookings.booking_client_id
        JOIN users ON users.user_id = clients.client_user_id
        WHERE users.user_login IN ('front_test_user', 'debug_bmw_trip_user')
    );

    DELETE FROM fines
    WHERE fine_trip_id IN (
        SELECT trip_id
        FROM trips
        JOIN bookings ON bookings.booking_id = trips.trip_booking_id
        JOIN clients ON clients.client_id = bookings.booking_client_id
        JOIN users ON users.user_id = clients.client_user_id
        WHERE users.user_login IN ('front_test_user', 'debug_bmw_trip_user')
    );

    DELETE FROM trip_details
    WHERE trip_detail_trip_id IN (
        SELECT trip_id
        FROM trips
        JOIN bookings ON bookings.booking_id = trips.trip_booking_id
        JOIN clients ON clients.client_id = bookings.booking_client_id
        JOIN users ON users.user_id = clients.client_user_id
        WHERE users.user_login IN ('front_test_user', 'debug_bmw_trip_user')
    );

    DELETE FROM trips
    WHERE trip_booking_id IN (
        SELECT booking_id
        FROM bookings
        JOIN clients ON clients.client_id = bookings.booking_client_id
        JOIN users ON users.user_id = clients.client_user_id
        WHERE users.user_login IN ('front_test_user', 'debug_bmw_trip_user')
    );

    DELETE FROM bookings
    WHERE booking_client_id IN (
        SELECT client_id
        FROM clients
        JOIN users ON users.user_id = clients.client_user_id
        WHERE users.user_login IN ('front_test_user', 'debug_bmw_trip_user')
    );

    DELETE FROM client_documents
    WHERE document_client_id IN (
        SELECT client_id
        FROM clients
        JOIN users ON users.user_id = clients.client_user_id
        WHERE users.user_login IN ('front_test_user', 'debug_bmw_trip_user')
    );

    DELETE FROM favorite_cars
    WHERE "ClientId" IN (
        SELECT client_id
        FROM clients
        JOIN users ON users.user_id = clients.client_user_id
        WHERE users.user_login IN ('front_test_user', 'debug_bmw_trip_user')
    );

    DELETE FROM reviews
    WHERE review_client_id IN (
        SELECT client_id
        FROM clients
        JOIN users ON users.user_id = clients.client_user_id
        WHERE users.user_login IN ('front_test_user', 'debug_bmw_trip_user')
    );

    DELETE FROM clients
    WHERE client_user_id IN (
        SELECT user_id
        FROM users
        WHERE user_login IN ('front_test_user', 'debug_bmw_trip_user')
    );

    DELETE FROM users
    WHERE user_login IN ('front_test_user', 'debug_bmw_trip_user');

    -- Cleanup data attached to debug cars even if it was created by a different test user.
    DELETE FROM payments
    WHERE payment_bill_id IN (
        SELECT bill_id
        FROM bills
        JOIN trips ON trips.trip_id = bills.bill_trip_id
        JOIN bookings ON bookings.booking_id = trips.trip_booking_id
        JOIN cars ON cars.car_id = bookings.booking_car_id
        JOIN specifications_car ON specifications_car.specification_car_id = cars.car_specification_id
        WHERE specifications_car.specification_car_vin_number LIKE 'DBGFRONT%'
           OR specifications_car.specification_car_vin_number = 'WBAEV33444KL98765'
    );

    DELETE FROM bills
    WHERE bill_trip_id IN (
        SELECT trip_id
        FROM trips
        JOIN bookings ON bookings.booking_id = trips.trip_booking_id
        JOIN cars ON cars.car_id = bookings.booking_car_id
        JOIN specifications_car ON specifications_car.specification_car_id = cars.car_specification_id
        WHERE specifications_car.specification_car_vin_number LIKE 'DBGFRONT%'
           OR specifications_car.specification_car_vin_number = 'WBAEV33444KL98765'
    );

    DELETE FROM fines
    WHERE fine_trip_id IN (
        SELECT trip_id
        FROM trips
        JOIN bookings ON bookings.booking_id = trips.trip_booking_id
        JOIN cars ON cars.car_id = bookings.booking_car_id
        JOIN specifications_car ON specifications_car.specification_car_id = cars.car_specification_id
        WHERE specifications_car.specification_car_vin_number LIKE 'DBGFRONT%'
           OR specifications_car.specification_car_vin_number = 'WBAEV33444KL98765'
    );

    DELETE FROM trip_details
    WHERE trip_detail_trip_id IN (
        SELECT trip_id
        FROM trips
        JOIN bookings ON bookings.booking_id = trips.trip_booking_id
        JOIN cars ON cars.car_id = bookings.booking_car_id
        JOIN specifications_car ON specifications_car.specification_car_id = cars.car_specification_id
        WHERE specifications_car.specification_car_vin_number LIKE 'DBGFRONT%'
           OR specifications_car.specification_car_vin_number = 'WBAEV33444KL98765'
    );

    DELETE FROM trips
    WHERE trip_booking_id IN (
        SELECT booking_id
        FROM bookings
        JOIN cars ON cars.car_id = bookings.booking_car_id
        JOIN specifications_car ON specifications_car.specification_car_id = cars.car_specification_id
        WHERE specifications_car.specification_car_vin_number LIKE 'DBGFRONT%'
           OR specifications_car.specification_car_vin_number = 'WBAEV33444KL98765'
    );

    DELETE FROM bookings
    WHERE booking_car_id IN (
        SELECT car_id
        FROM cars
        JOIN specifications_car ON specifications_car.specification_car_id = cars.car_specification_id
        WHERE specifications_car.specification_car_vin_number LIKE 'DBGFRONT%'
           OR specifications_car.specification_car_vin_number = 'WBAEV33444KL98765'
    );

    DELETE FROM maintenance
    WHERE maintenance_car_id IN (
        SELECT car_id
        FROM cars
        JOIN specifications_car ON specifications_car.specification_car_id = cars.car_specification_id
        WHERE specifications_car.specification_car_vin_number LIKE 'DBGFRONT%'
           OR specifications_car.specification_car_vin_number = 'WBAEV33444KL98765'
    );

    DELETE FROM insurance
    WHERE insurance_car_id IN (
        SELECT car_id
        FROM cars
        JOIN specifications_car ON specifications_car.specification_car_id = cars.car_specification_id
        WHERE specifications_car.specification_car_vin_number LIKE 'DBGFRONT%'
           OR specifications_car.specification_car_vin_number = 'WBAEV33444KL98765'
    );

    DELETE FROM favorite_cars
    WHERE "CarId" IN (
        SELECT car_id
        FROM cars
        JOIN specifications_car ON specifications_car.specification_car_id = cars.car_specification_id
        WHERE specifications_car.specification_car_vin_number LIKE 'DBGFRONT%'
           OR specifications_car.specification_car_vin_number = 'WBAEV33444KL98765'
    );

    DELETE FROM reviews
    WHERE review_car_id IN (
        SELECT car_id
        FROM cars
        JOIN specifications_car ON specifications_car.specification_car_id = cars.car_specification_id
        WHERE specifications_car.specification_car_vin_number LIKE 'DBGFRONT%'
           OR specifications_car.specification_car_vin_number = 'WBAEV33444KL98765'
    );

    DELETE FROM cars
    WHERE car_specification_id IN (
        SELECT specification_car_id
        FROM specifications_car
        WHERE specification_car_vin_number LIKE 'DBGFRONT%'
           OR specification_car_vin_number = 'WBAEV33444KL98765'
    );

    DELETE FROM specifications_car
    WHERE specification_car_vin_number LIKE 'DBGFRONT%'
       OR specification_car_vin_number = 'WBAEV33444KL98765';

    DELETE FROM promocodes
    WHERE promocode_code IN ('FRONT20', 'FRONTEXPIRED');

    DELETE FROM tariffs
    WHERE tariff_name LIKE 'front_test_%'
       OR tariff_name = 'debug_bmw_trip_tariff';

    DELETE FROM categories
    WHERE category_name LIKE 'front_test_%'
       OR category_name = 'debug_bmw_trip_category';

    -- Client account. Password hash is for DebugPassword123!.
    INSERT INTO users (user_role_id, user_login, user_password_hash)
    VALUES (
        2,
        'front_test_user',
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
        'Front',
        'Tester',
        '+375291112233',
        'front.tester@example.com'
    )
    RETURNING client_id INTO v_client_id;

    INSERT INTO client_documents (
        document_client_id,
        document_type,
        document_license_category,
        document_number,
        document_issue_date,
        document_expiry_date,
        document_file_path
    )
    VALUES (
        v_client_id,
        'Водительское удостоверение',
        'B',
        'FT1234567',
        CURRENT_DATE - INTERVAL '2 years',
        CURRENT_DATE + INTERVAL '8 years',
        'debug/documents/front-test-license.png'
    );

    -- Reference data used only by the debug cars.
    INSERT INTO tariffs (tariff_name, tariff_price_per_minute, tariff_price_per_km, tariff_price_per_day)
    VALUES ('front_test_city', 0.35, 0.90, 65.00)
    RETURNING tariff_id INTO v_tariff_city_id;

    INSERT INTO tariffs (tariff_name, tariff_price_per_minute, tariff_price_per_km, tariff_price_per_day)
    VALUES ('front_test_comfort', 0.48, 1.20, 88.00)
    RETURNING tariff_id INTO v_tariff_comfort_id;

    INSERT INTO tariffs (tariff_name, tariff_price_per_minute, tariff_price_per_km, tariff_price_per_day)
    VALUES ('front_test_premium', 0.72, 1.75, 140.00)
    RETURNING tariff_id INTO v_tariff_premium_id;

    INSERT INTO categories (category_name)
    VALUES ('front_test_city')
    RETURNING category_id INTO v_category_city_id;

    INSERT INTO categories (category_name)
    VALUES ('front_test_business')
    RETURNING category_id INTO v_category_business_id;

    INSERT INTO categories (category_name)
    VALUES ('front_test_electric')
    RETURNING category_id INTO v_category_electric_id;

    INSERT INTO categories (category_name)
    VALUES ('front_test_suv')
    RETURNING category_id INTO v_category_suv_id;

    INSERT INTO promocodes (
        promocode_status_id,
        promocode_code,
        promocode_discount,
        promocode_start_date,
        promocode_end_date
    )
    VALUES (
        1,
        'FRONT20',
        20.00,
        CURRENT_DATE - INTERVAL '30 days',
        CURRENT_DATE + INTERVAL '30 days'
    )
    RETURNING promocode_id INTO v_promo_id;

    INSERT INTO promocodes (
        promocode_status_id,
        promocode_code,
        promocode_discount,
        promocode_start_date,
        promocode_end_date
    )
    VALUES (
        2,
        'FRONTEXPIRED',
        50.00,
        CURRENT_DATE - INTERVAL '90 days',
        CURRENT_DATE - INTERVAL '1 day'
    );

    -- Available cars for the map and car details screen.
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
    VALUES ('Бензин', 'Renault', 'Logan', 'Механика', 2021, 'DBGFRONTREN001', '1001 FT-7', 42100, 50.00, 0.065);

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
        1,
        v_tariff_city_id,
        v_category_city_id,
        currval(pg_get_serial_sequence('specifications_car', 'specification_car_id')),
        'Минск, улица Немига 5',
        ST_SetSRID(ST_MakePoint(27.5530, 53.9045), 4326),
        33.50,
        'http://localhost:9000/carsharing-images/images/cars/front-renault-logan.png'
    )
    RETURNING car_id INTO v_car_renault_id;

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
    VALUES ('Дизель', 'Volkswagen', 'Polo', 'Автомат', 2022, 'DBGFRONTVW002', '1002 FT-7', 28700, 45.00, 0.055);

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
        1,
        v_tariff_city_id,
        v_category_city_id,
        currval(pg_get_serial_sequence('specifications_car', 'specification_car_id')),
        'Минск, площадь Победы',
        ST_SetSRID(ST_MakePoint(27.5756, 53.9097), 4326),
        11.20,
        'http://localhost:9000/carsharing-images/images/cars/front-vw-polo.png'
    )
    RETURNING car_id INTO v_car_vw_id;

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
    VALUES ('Электро', 'Tesla', 'Model 3', 'Автомат', 2023, 'DBGFRONTTES003', '1003 FT-7', 15400, 75.00, 0.000);

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
        1,
        v_tariff_premium_id,
        v_category_electric_id,
        currval(pg_get_serial_sequence('specifications_car', 'specification_car_id')),
        'Минск, улица Октябрьская 16',
        ST_SetSRID(ST_MakePoint(27.5712, 53.8918), 4326),
        62.80,
        'http://localhost:9000/carsharing-images/images/cars/front-tesla-model-3.png'
    )
    RETURNING car_id INTO v_car_tesla_id;

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
    VALUES ('Бензин', 'Kia', 'Sportage', 'Автомат', 2024, 'DBGFRONTKIA004', '1004 FT-7', 9800, 62.00, 0.088);

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
        1,
        v_tariff_comfort_id,
        v_category_suv_id,
        currval(pg_get_serial_sequence('specifications_car', 'specification_car_id')),
        'Минск, проспект Дзержинского 3',
        ST_SetSRID(ST_MakePoint(27.5362, 53.8849), 4326),
        54.70,
        'http://localhost:9000/carsharing-images/images/cars/front-kia-sportage.png'
    )
    RETURNING car_id INTO v_car_kia_id;

    -- Reserved BMW with an active trip for the current-trip screen.
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
    VALUES ('Бензин', 'BMW', '3 Series', 'Автомат', 2024, 'DBGFRONTBMW005', '1005 FT-7', 12500, 59.00, 0.080);

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
        v_tariff_comfort_id,
        v_category_business_id,
        currval(pg_get_serial_sequence('specifications_car', 'specification_car_id')),
        'Минск, проспект Победителей 9',
        ST_SetSRID(ST_MakePoint(27.5415, 53.9080), 4326),
        41.50,
        'http://localhost:9000/carsharing-images/images/cars/front-bmw-3.png'
    )
    RETURNING car_id INTO v_car_bmw_id;

    -- Maintenance car is not shown in the available-car list, but is useful for admin/API checks.
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
    VALUES ('Бензин', 'Audi', 'A4', 'Автомат', 2020, 'DBGFRONTAUD006', '1006 FT-7', 66400, 58.00, 0.082);

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
        3,
        v_tariff_premium_id,
        v_category_business_id,
        currval(pg_get_serial_sequence('specifications_car', 'specification_car_id')),
        'Минск, сервисный центр',
        ST_SetSRID(ST_MakePoint(27.5091, 53.9140), 4326),
        25.00,
        'http://localhost:9000/carsharing-images/images/cars/front-audi-a4.png'
    )
    RETURNING car_id INTO v_car_audi_id;

    INSERT INTO reviews (review_client_id, review_car_id, review_rating, review_comment, review_date)
    VALUES
        (v_client_id, v_car_renault_id, 5, 'Чистый салон и удобная посадка.', NOW() - INTERVAL '10 days'),
        (v_client_id, v_car_tesla_id, 4, 'Хорошая динамика, но мало заряда на старте.', NOW() - INTERVAL '6 days');

    INSERT INTO favorite_cars ("ClientId", "CarId")
    VALUES (v_client_id, v_car_tesla_id);

    -- Active booking + active trip.
    INSERT INTO bookings (
        booking_status_id,
        booking_car_id,
        booking_client_id,
        booking_start_time,
        booking_end_time
    )
    VALUES (
        1,
        v_car_bmw_id,
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
        2,
        'per_minute',
        NOW() - INTERVAL '24 minutes',
        NULL,
        24.00,
        8.40
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
        'В пути',
        TRUE,
        0.67,
        0.00
    );

    -- Finished trip with an unpaid bill.
    INSERT INTO bookings (booking_status_id, booking_car_id, booking_client_id, booking_start_time, booking_end_time)
    VALUES (2, v_car_renault_id, v_client_id, NOW() - INTERVAL '1 day 2 hours', NOW() - INTERVAL '1 day 1 hour')
    RETURNING booking_id INTO v_booking_id;

    INSERT INTO trips (trip_booking_id, trip_status_id, trip_tariff_type, trip_start_time, trip_end_time, trip_duration, trip_distance_km)
    VALUES (v_booking_id, 5, 'per_minute', NOW() - INTERVAL '1 day 2 hours', NOW() - INTERVAL '1 day 1 hour 17 minutes', 43.00, 13.60)
    RETURNING trip_id INTO v_trip_id;

    INSERT INTO trip_details (
        trip_detail_trip_id,
        trip_detail_start_location,
        trip_detail_end_location,
        trip_detail_insurance_active,
        trip_detail_fuel_used,
        trip_detail_refueled
    )
    VALUES (v_trip_id, 'Минск, ул. Немига 5', 'Минск, ул. Кальварийская 21', TRUE, 0.88, 0.00);

    INSERT INTO bills (bill_trip_id, bill_promocode_id, bill_status_id, bill_issue_date, bill_amount, bill_remaining_amount)
    VALUES (v_trip_id, NULL, 1, NOW() - INTERVAL '1 day 1 hour', 15.80, 15.80);

    -- Finished trip with a partially paid bill and payment history.
    INSERT INTO bookings (booking_status_id, booking_car_id, booking_client_id, booking_start_time, booking_end_time)
    VALUES (2, v_car_vw_id, v_client_id, NOW() - INTERVAL '3 days 3 hours', NOW() - INTERVAL '3 days 2 hours')
    RETURNING booking_id INTO v_booking_id;

    INSERT INTO trips (trip_booking_id, trip_status_id, trip_tariff_type, trip_start_time, trip_end_time, trip_duration, trip_distance_km)
    VALUES (v_booking_id, 5, 'per_km', NOW() - INTERVAL '3 days 3 hours', NOW() - INTERVAL '3 days 2 hours 4 minutes', 56.00, 27.30)
    RETURNING trip_id INTO v_trip_id;

    INSERT INTO trip_details (
        trip_detail_trip_id,
        trip_detail_start_location,
        trip_detail_end_location,
        trip_detail_insurance_active,
        trip_detail_fuel_used,
        trip_detail_refueled
    )
    VALUES (v_trip_id, 'Минск, площадь Победы', 'Минск, ул. Притыцкого 29', FALSE, 1.50, 0.00);

    INSERT INTO bills (bill_trip_id, bill_promocode_id, bill_status_id, bill_issue_date, bill_amount, bill_remaining_amount)
    VALUES (v_trip_id, NULL, 2, NOW() - INTERVAL '3 days 2 hours', 24.57, 9.57)
    RETURNING bill_id INTO v_bill_id;

    INSERT INTO payments (payment_bill_id, payment_sum, payment_method, payment_date)
    VALUES (v_bill_id, 15.00, 'Картой', NOW() - INTERVAL '3 days 1 hour');

    -- Finished trip with an applied promocode and fully paid bill.
    INSERT INTO bookings (booking_status_id, booking_car_id, booking_client_id, booking_start_time, booking_end_time)
    VALUES (2, v_car_tesla_id, v_client_id, NOW() - INTERVAL '8 days 5 hours', NOW() - INTERVAL '8 days 4 hours')
    RETURNING booking_id INTO v_booking_id;

    INSERT INTO trips (trip_booking_id, trip_status_id, trip_tariff_type, trip_start_time, trip_end_time, trip_duration, trip_distance_km)
    VALUES (v_booking_id, 3, 'per_day', NOW() - INTERVAL '8 days 5 hours', NOW() - INTERVAL '7 days 2 hours', 1620.00, 104.20)
    RETURNING trip_id INTO v_trip_id;

    INSERT INTO trip_details (
        trip_detail_trip_id,
        trip_detail_start_location,
        trip_detail_end_location,
        trip_detail_insurance_active,
        trip_detail_fuel_used,
        trip_detail_refueled
    )
    VALUES (v_trip_id, 'Минск, ул. Октябрьская 16', 'Минск, Национальный аэропорт', TRUE, 0.00, 0.00);

    INSERT INTO bills (bill_trip_id, bill_promocode_id, bill_status_id, bill_issue_date, bill_amount, bill_remaining_amount)
    VALUES (v_trip_id, v_promo_id, 3, NOW() - INTERVAL '7 days 1 hour', 117.60, 0.00)
    RETURNING bill_id INTO v_bill_id;

    INSERT INTO payments (payment_bill_id, payment_sum, payment_method, payment_date)
    VALUES
        (v_bill_id, 80.00, 'ЕРИП', NOW() - INTERVAL '7 days'),
        (v_bill_id, 37.60, 'Картой', NOW() - INTERVAL '6 days 23 hours');

    -- Finished trip with a cancelled bill and refuel discount data.
    INSERT INTO bookings (booking_status_id, booking_car_id, booking_client_id, booking_start_time, booking_end_time)
    VALUES (2, v_car_kia_id, v_client_id, NOW() - INTERVAL '15 days 4 hours', NOW() - INTERVAL '15 days 3 hours')
    RETURNING booking_id INTO v_booking_id;

    INSERT INTO trips (trip_booking_id, trip_status_id, trip_tariff_type, trip_start_time, trip_end_time, trip_duration, trip_distance_km)
    VALUES (v_booking_id, 3, 'per_minute', NOW() - INTERVAL '15 days 4 hours', NOW() - INTERVAL '15 days 3 hours 12 minutes', 48.00, 18.90)
    RETURNING trip_id INTO v_trip_id;

    INSERT INTO trip_details (
        trip_detail_trip_id,
        trip_detail_start_location,
        trip_detail_end_location,
        trip_detail_insurance_active,
        trip_detail_fuel_used,
        trip_detail_refueled
    )
    VALUES (v_trip_id, 'Минск, пр-т Дзержинского 3', 'Минск, ул. Сурганова 57', FALSE, 1.66, 6.00);

    INSERT INTO bills (bill_trip_id, bill_promocode_id, bill_status_id, bill_issue_date, bill_amount, bill_remaining_amount)
    VALUES (v_trip_id, NULL, 4, NOW() - INTERVAL '15 days 3 hours', 0.00, 0.00);

    -- Extra old trip to exercise history pagination/count summaries.
    INSERT INTO bookings (booking_status_id, booking_car_id, booking_client_id, booking_start_time, booking_end_time)
    VALUES (2, v_car_renault_id, v_client_id, NOW() - INTERVAL '25 days 2 hours', NOW() - INTERVAL '25 days 1 hour')
    RETURNING booking_id INTO v_booking_id;

    INSERT INTO trips (trip_booking_id, trip_status_id, trip_tariff_type, trip_start_time, trip_end_time, trip_duration, trip_distance_km)
    VALUES (v_booking_id, 3, 'per_km', NOW() - INTERVAL '25 days 2 hours', NOW() - INTERVAL '25 days 1 hour 30 minutes', 30.00, 7.80)
    RETURNING trip_id INTO v_trip_id;

    INSERT INTO trip_details (
        trip_detail_trip_id,
        trip_detail_start_location,
        trip_detail_end_location,
        trip_detail_insurance_active,
        trip_detail_fuel_used,
        trip_detail_refueled
    )
    VALUES (v_trip_id, 'Минск, Комаровский рынок', 'Минск, парк Челюскинцев', TRUE, 0.51, 0.00);

    INSERT INTO bills (bill_trip_id, bill_promocode_id, bill_status_id, bill_issue_date, bill_amount, bill_remaining_amount)
    VALUES (v_trip_id, NULL, 3, NOW() - INTERVAL '25 days 1 hour', 7.02, 0.00)
    RETURNING bill_id INTO v_bill_id;

    INSERT INTO payments (payment_bill_id, payment_sum, payment_method, payment_date)
    VALUES (v_bill_id, 7.02, 'Наличными', NOW() - INTERVAL '25 days');

    RAISE NOTICE 'Frontend debug seed created';
    RAISE NOTICE 'Login: front_test_user';
    RAISE NOTICE 'Password: DebugPassword123!';
    RAISE NOTICE 'Active promocode: FRONT20';
END $$;

COMMIT;

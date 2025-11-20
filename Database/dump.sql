--
-- PostgreSQL database dump
--

\restrict lAE0UUYfBmecnXe4if1vfrxuvMwMZ16eQ0s0C5pwTUJMp8dIz8LyAOT1oXK4tZF

-- Dumped from database version 18.0
-- Dumped by pg_dump version 18.0

-- Started on 2025-11-16 19:11:13

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 921 (class 1247 OID 16817)
-- Name: fine_type_enum; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.fine_type_enum AS ENUM (
    'Превышение скорости',
    'Нарушение правил парковки',
    'Несчастный случай',
    'Позднее возвращение',
    'Курение в машине',
    'Другое'
);


--
-- TOC entry 936 (class 1247 OID 17527)
-- Name: fuel_type; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.fuel_type AS ENUM (
    'Бензин',
    'Дизель',
    'Электро',
    'Гибрид',
    'Газ'
);


--
-- TOC entry 933 (class 1247 OID 16993)
-- Name: insurance_type_enum; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.insurance_type_enum AS ENUM (
    'ОСАГО',
    'КАСКО'
);


--
-- TOC entry 924 (class 1247 OID 16904)
-- Name: maintenance_type_enum; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.maintenance_type_enum AS ENUM (
    'Замена масла',
    'Замена шин',
    'Обслуживание тормозов',
    'Осмотр',
    'Ремонт',
    'Чистка'
);


--
-- TOC entry 951 (class 1247 OID 18241)
-- Name: payment_method; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.payment_method AS ENUM (
    'Картой',
    'Наличными',
    'ЕРИП',
    'Другое'
);


--
-- TOC entry 927 (class 1247 OID 16924)
-- Name: role_name; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.role_name AS ENUM (
    'Администратор',
    'Клиент'
);


--
-- TOC entry 942 (class 1247 OID 18017)
-- Name: tariff_type; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.tariff_type AS ENUM (
    'per_minute',
    'per_km',
    'per_day'
);


--
-- TOC entry 939 (class 1247 OID 17637)
-- Name: transmission_type; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.transmission_type AS ENUM (
    'Автомат',
    'Механика',
    'Робот'
);


--
-- TOC entry 264 (class 1255 OID 17754)
-- Name: apply_payment_to_bill(); Type: FUNCTION; Schema: public; Owner: -
--

CREATE FUNCTION public.apply_payment_to_bill() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_bill_amount NUMERIC(10,2);
    v_remaining NUMERIC(10,2);
    v_status_paid INT;
    v_status_partial INT;
    v_status_pending INT;
BEGIN
    -- Проверяем, связан ли платёж со счётом
    IF NEW.payment_bill_id IS NULL THEN
        RAISE NOTICE 'Платёж % не привязан к счёту', NEW.payment_id;
        RETURN NEW;
    END IF;

    -- Получаем текущие значения
    SELECT bill_amount, bill_remaining_amount INTO v_bill_amount, v_remaining
    FROM bills
    WHERE bill_id = NEW.payment_bill_id
    FOR UPDATE;

    IF NOT FOUND THEN
        RAISE NOTICE 'Счёт % не найден', NEW.payment_bill_id;
        RETURN NEW;
    END IF;

    -- Вычитаем оплату
    v_remaining := GREATEST(v_remaining - NEW.payment_sum, 0);

    -- Обновляем остаток
    UPDATE bills
    SET bill_remaining_amount = v_remaining
    WHERE bill_id = NEW.payment_bill_id;

    -- Получаем ID статусов
    SELECT status_id INTO v_status_paid FROM status WHERE lower(status_name)='оплачен' LIMIT 1;
    SELECT status_id INTO v_status_partial FROM status WHERE lower(status_name)='частично оплачен' LIMIT 1;
    SELECT status_id INTO v_status_pending FROM status WHERE lower(status_name) IN ('ожидает оплаты','не оплачен') LIMIT 1;

    -- Определяем новый статус
    IF v_remaining = 0 THEN
        UPDATE bills SET bill_status_id = v_status_paid WHERE bill_id = NEW.payment_bill_id;
        RAISE NOTICE 'Счёт % полностью оплачен', NEW.payment_bill_id;
    ELSIF v_remaining < v_bill_amount THEN
        UPDATE bills SET bill_status_id = v_status_partial WHERE bill_id = NEW.payment_bill_id;
        RAISE NOTICE 'Счёт % частично оплачен. Остаток: %', NEW.payment_bill_id, v_remaining;
    ELSE
        UPDATE bills SET bill_status_id = v_status_pending WHERE bill_id = NEW.payment_bill_id;
        RAISE NOTICE 'Счёт % ожидает оплаты. Остаток: %', NEW.payment_bill_id, v_remaining;
    END IF;

    RETURN NEW;
END;
$$;


--
-- TOC entry 272 (class 1255 OID 17056)
-- Name: calculate_bill_total(); Type: FUNCTION; Schema: public; Owner: -
--

CREATE FUNCTION public.calculate_bill_total() RETURNS trigger
    LANGUAGE plpgsql
    AS $$DECLARE
    v_tariff_type TEXT;
    v_price_per_minute NUMERIC(10,2);
    v_price_per_km NUMERIC(10,2);
    v_price_per_day NUMERIC(10,2);
    v_trip_cost NUMERIC(10,2) := 0;
    v_fine_total NUMERIC(10,2) := 0;
    v_discount NUMERIC(5,2) := 0;
    v_trip_distance NUMERIC(10,2);
    v_trip_duration NUMERIC(10,2);
    v_trip_days NUMERIC(10,2);
	v_refueled NUMERIC(10,2);
	v_refuel_discount NUMERIC(10,2);
	v_insurance_cost NUMERIC(10,2) := 0;

	v_refuel_discount_rate CONSTANT NUMERIC(10,2) := 60;  -- 60 руб/литр скидка
    v_insurance_percent CONSTANT NUMERIC(5,2) := 5;       -- 5% при включённой страховке
BEGIN
    -- Получаем параметры поездки, машины и тарифа
    SELECT 
        tr.trip_tariff_type,
        t.tariff_price_per_minute,
        t.tariff_price_per_km,
        t.tariff_price_per_day,
        tr.trip_distance_km,
        tr.trip_duration,
		tr.trip_refueled
    INTO 
        v_tariff_type,
        v_price_per_minute,
        v_price_per_km,
        v_price_per_day,
        v_trip_distance,
        v_trip_duration,
		v_refueled
    FROM trips tr
    JOIN bookings b ON tr.trip_booking_id = b.booking_id
    JOIN cars c ON b.booking_car_id = c.car_id
    JOIN tariffs t ON c.car_tariff_id = t.tariff_id
    WHERE tr.trip_id = NEW.bill_trip_id;

    -- Расчёт стоимости по выбранному типу тарифа
    IF v_tariff_type = 'per_minute' THEN
        v_trip_cost := COALESCE(v_trip_duration, 0) * COALESCE(v_price_per_minute, 0);
    ELSIF v_tariff_type = 'per_km' THEN
        v_trip_cost := COALESCE(v_trip_distance, 0) * COALESCE(v_price_per_km, 0);
    ELSIF v_tariff_type = 'per_day' THEN
        v_trip_days := CEIL(v_trip_duration / (60 * 24));
        v_trip_cost := v_trip_days * COALESCE(v_price_per_day, 0);
    END IF;

    -- Добавляем штрафы
	SELECT COALESCE(SUM(f.fine_amount), 0)
	INTO v_fine_total
	FROM fines f
	JOIN trips tr ON f.fine_trip_id = tr.trip_id
	WHERE f.fine_trip_id = NEW.bill_trip_id
 	 AND (f.fine_status IS NULL OR lower(f.fine_status) NOT IN ('отменён', 'cancelled'))  -- не отменён
 	 AND (f.fine_date IS NULL OR (f.fine_date BETWEEN tr.trip_start_time AND tr.trip_end_time))  -- штраф в пределах поездки
 	 AND (f.fine_amount > 0);                                              -- сумма положительная

	IF v_fine_total IS NULL THEN
	    v_fine_total := 0;
	END IF;

    -- Проверяем промокод
    IF NEW.bill_promocode_id IS NULL THEN
        v_discount := 0;
    ELSE
        SELECT promocode_discount INTO v_discount
        FROM promocodes
        WHERE promocode_id = NEW.bill_promocode_id
          AND (promocode_start_date IS NULL OR promocode_start_date <= now())
          AND (promocode_end_date IS NULL OR promocode_end_date >= now());

        IF NOT FOUND THEN
            v_discount := 0;
        END IF;
    END IF;

	IF v_refueled > 0 THEN
        v_refuel_discount := v_refueled * v_refuel_discount_rate;
    ELSE
        v_refuel_discount := 0;
    END IF;

	IF (SELECT trip_insurance_active FROM trips WHERE trip_id = NEW.bill_trip_id) THEN
        v_insurance_cost := v_trip_cost * v_insurance_percent / 100;
    ELSE
        v_insurance_cost := 0;
    END IF;

    -- Итоговая сумма
    NEW.bill_amount := GREATEST(0, ROUND(
            (v_trip_cost + v_fine_total + v_insurance_cost)
            - (v_trip_cost * v_discount / 100)
            - v_refuel_discount,
            2)
			);

     RAISE NOTICE 'Поездка %: базовая = %, штрафы = %, промо = %%%, заправка = %, страховка = %, итог = %',
        NEW.bill_trip_id, v_trip_cost, v_fine_total, v_discount, v_refuel_discount, v_insurance_cost, NEW.bill_amount;

    RETURN NEW;
END;$$;


--
-- TOC entry 258 (class 1255 OID 17484)
-- Name: calculate_trip_duration(); Type: FUNCTION; Schema: public; Owner: -
--

CREATE FUNCTION public.calculate_trip_duration() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF NEW.trip_start_time IS NOT NULL AND NEW.trip_end_time IS NOT NULL THEN
        NEW.trip_duration := EXTRACT(EPOCH FROM (NEW.trip_end_time - NEW.trip_start_time)) / 60;
    END IF;
    RETURN NEW;
END;
$$;


--
-- TOC entry 259 (class 1255 OID 17486)
-- Name: create_bill_after_trip(); Type: FUNCTION; Schema: public; Owner: -
--

CREATE FUNCTION public.create_bill_after_trip() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF NEW.trip_end_time IS NOT NULL THEN
        INSERT INTO bills (bill_payment_id, bill_trip_id, bill_status_id)
        VALUES (NULL, NEW.trip_id, (SELECT status_id FROM status WHERE lower(status_name)='ожидает оплаты'));
    END IF;
    RETURN NEW;
END;
$$;


--
-- TOC entry 273 (class 1255 OID 17773)
-- Name: prevent_overpayment(); Type: FUNCTION; Schema: public; Owner: -
--

CREATE FUNCTION public.prevent_overpayment() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_remaining NUMERIC(10,2);
BEGIN
    -- если платёж не связан со счётом — пропускаем
    IF NEW.payment_bill_id IS NULL THEN
        RETURN NEW;
    END IF;

    -- получаем текущий остаток
    SELECT bill_remaining_amount
    INTO v_remaining
    FROM bills
    WHERE bill_id = NEW.payment_bill_id;

    -- если счёт не найден
    IF NOT FOUND THEN
        RAISE EXCEPTION 'Счёт % не найден', NEW.payment_bill_id;
    END IF;

    -- если платёж больше остатка
    IF NEW.payment_sum > v_remaining THEN
        RAISE EXCEPTION 
            'Ошибка: сумма платежа (%, руб.) превышает остаток по счёту (%, руб.)', 
            NEW.payment_sum, v_remaining;
    END IF;

    RETURN NEW;
END;
$$;


--
-- TOC entry 257 (class 1255 OID 17752)
-- Name: set_initial_remaining_amount(); Type: FUNCTION; Schema: public; Owner: -
--

CREATE FUNCTION public.set_initial_remaining_amount() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF TG_OP = 'INSERT' THEN
        NEW.bill_remaining_amount := NEW.bill_amount;
    ELSIF TG_OP = 'UPDATE' AND NEW.bill_amount <> OLD.bill_amount THEN
        -- если сумма счёта изменилась, пересчитать остаток
        NEW.bill_remaining_amount := GREATEST(NEW.bill_amount - (OLD.bill_amount - OLD.bill_remaining_amount), 0);
    END IF;
    RETURN NEW;
END;
$$;


--
-- TOC entry 274 (class 1255 OID 18427)
-- Name: set_trip_fuel_used(); Type: FUNCTION; Schema: public; Owner: -
--

CREATE FUNCTION public.set_trip_fuel_used() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_fuel_per_km NUMERIC(10,3);
    v_car_id INT;
    v_booking_id INT;
    v_distance NUMERIC(10,2);
BEGIN
    -- Получаем booking_id и пройденное расстояние из таблицы trips
    SELECT 
        trip_booking_id,
        trip_distance_km
    INTO 
        v_booking_id,
        v_distance
    FROM trips
    WHERE trip_id = NEW.trip_id;

    -- Получаем ID машины и расход топлива из спецификации
    SELECT 
        b.booking_car_id,
        s.specification_fuel_per_km
    INTO 
        v_car_id,
        v_fuel_per_km
    FROM bookings b
    JOIN cars c ON c.car_id = b.booking_car_id
    JOIN specifications_car s ON s.specification_car_id = c.car_specification_id
    WHERE b.booking_id = v_booking_id;

    -- Если расход не указан вручную — рассчитываем автоматически
    IF NEW.trip_detail_fuel_used IS NULL OR NEW.trip_detail_fuel_used <= 0 THEN
        NEW.trip_detail_fuel_used := COALESCE(v_fuel_per_km * v_distance, 0);
    END IF;

    RETURN NEW;
END;
$$;


--
-- TOC entry 275 (class 1255 OID 18428)
-- Name: update_car_fuel_after_trip(); Type: FUNCTION; Schema: public; Owner: -
--

CREATE FUNCTION public.update_car_fuel_after_trip() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_car_id INT;
    v_booking_id INT;
    v_fuel_per_km NUMERIC(10,3);
    v_fuel_max NUMERIC(10,2);
    v_fuel_used NUMERIC(10,2);
    v_new_level NUMERIC(10,2);
BEGIN
    -- Получаем booking_id по trip_id
    SELECT trip_booking_id INTO v_booking_id
    FROM trips
    WHERE trip_id = NEW.trip_id;

    -- Получаем car_id, расход и объём бака
    SELECT 
        b.booking_car_id,
        s.specification_fuel_per_km,
        s.specification_car_max_fuel
    INTO 
        v_car_id,
        v_fuel_per_km,
        v_fuel_max
    FROM bookings b
    JOIN cars c ON c.car_id = b.booking_car_id
    JOIN specifications_car s ON s.specification_car_id = c.car_specification_id
    WHERE b.booking_id = v_booking_id;

    -- Берём расход и заправку из trip_details
    v_fuel_used := COALESCE(NEW.trip_detail_fuel_used, 0);
    IF v_fuel_used <= 0 THEN
        v_fuel_used := COALESCE(v_fuel_per_km * (SELECT trip_distance_km FROM trips WHERE trip_id = NEW.trip_id), 0);
    END IF;

    -- Получаем текущий уровень топлива
    SELECT car_fuel_level INTO v_new_level
    FROM cars
    WHERE car_id = v_car_id
    FOR UPDATE;

    -- Новый уровень топлива
    v_new_level := v_new_level - v_fuel_used + COALESCE(NEW.trip_detail_refueled, 0);
    v_new_level := LEAST(GREATEST(v_new_level, 0), v_fuel_max);

    -- Обновляем значение в cars
    UPDATE cars
    SET car_fuel_level = v_new_level
    WHERE car_id = v_car_id;

    RAISE NOTICE 
        'Авто %, расход = %, заправлено = %, остаток = % (из макс %)',
        v_car_id, v_fuel_used, NEW.trip_detail_refueled, v_new_level, v_fuel_max;

    RETURN NEW;
END;
$$;


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 232 (class 1259 OID 16629)
-- Name: bills; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.bills (
    bill_id integer NOT NULL,
    bill_trip_id integer NOT NULL,
    bill_promocode_id integer,
    bill_status_id integer DEFAULT 1 NOT NULL,
    bill_issue_date timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    bill_amount numeric(10,2),
    bill_remaining_amount numeric(10,2) DEFAULT 0,
    CONSTRAINT bills_bill_remaining_amount_check CHECK ((bill_remaining_amount >= (0)::numeric)),
    CONSTRAINT chk_bill_amount_nonneg CHECK (((bill_amount IS NULL) OR (bill_amount >= (0)::numeric)))
);


--
-- TOC entry 231 (class 1259 OID 16628)
-- Name: bills_bill_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.bills ALTER COLUMN bill_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.bills_bill_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 230 (class 1259 OID 16559)
-- Name: bookings; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.bookings (
    booking_id integer NOT NULL,
    booking_status_id integer NOT NULL,
    booking_car_id integer NOT NULL,
    booking_client_id integer CONSTRAINT bookings_booking_user_id_not_null NOT NULL,
    booking_start_time timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    booking_end_time timestamp without time zone,
    CONSTRAINT chk_booking_times CHECK (((booking_start_time IS NULL) OR (booking_end_time IS NULL) OR (booking_start_time < booking_end_time)))
);


--
-- TOC entry 229 (class 1259 OID 16558)
-- Name: bookings_booking_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.bookings ALTER COLUMN booking_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.bookings_booking_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 240 (class 1259 OID 18142)
-- Name: cars; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.cars (
    car_id integer NOT NULL,
    car_status_id integer NOT NULL,
    car_tariff_id integer NOT NULL,
    car_category_id integer NOT NULL,
    car_specification_id integer NOT NULL,
    car_location character varying(50) NOT NULL,
    car_fuel_level numeric(5,2) DEFAULT 0 NOT NULL,
    CONSTRAINT chk_fuel_level CHECK ((car_fuel_level >= (0)::numeric))
);


--
-- TOC entry 239 (class 1259 OID 18141)
-- Name: cars_car_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.cars ALTER COLUMN car_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.cars_car_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 226 (class 1259 OID 16415)
-- Name: categories; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.categories (
    category_id integer NOT NULL,
    category_name character varying(100)
);


--
-- TOC entry 225 (class 1259 OID 16414)
-- Name: categories_category_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.categories ALTER COLUMN category_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.categories_category_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 244 (class 1259 OID 18476)
-- Name: client_documents; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.client_documents (
    document_id integer NOT NULL,
    document_client_id integer NOT NULL,
    document_type character varying(50) NOT NULL,
    document_number character varying(50) NOT NULL,
    document_issue_date date NOT NULL,
    document_expiry_date date NOT NULL,
    document_file_path character varying(255)
);


--
-- TOC entry 243 (class 1259 OID 18475)
-- Name: client_documents_document_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.client_documents ALTER COLUMN document_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.client_documents_document_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 235 (class 1259 OID 16935)
-- Name: clients; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.clients (
    client_id integer NOT NULL,
    client_user_id integer NOT NULL,
    client_name character varying(128) NOT NULL,
    client_surname character varying(128),
    client_phone_number character varying(32) NOT NULL,
    client_email character varying(128) NOT NULL,
    CONSTRAINT chk_client_email_format CHECK (((client_email)::text ~* '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$'::text))
);


--
-- TOC entry 236 (class 1259 OID 16943)
-- Name: clients_client_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.clients ALTER COLUMN client_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.clients_client_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 246 (class 1259 OID 18493)
-- Name: fines; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.fines (
    fine_id integer NOT NULL,
    fine_trip_id integer NOT NULL,
    fine_status_id integer NOT NULL,
    fine_type public.fine_type_enum NOT NULL,
    fine_amount numeric(10,2) NOT NULL,
    fine_date date NOT NULL,
    CONSTRAINT chk_fine_amount_nonneg CHECK (((fine_amount IS NULL) OR (fine_amount >= (0)::numeric)))
);


--
-- TOC entry 245 (class 1259 OID 18492)
-- Name: fines_fine_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.fines ALTER COLUMN fine_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.fines_fine_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 248 (class 1259 OID 18516)
-- Name: insurance; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.insurance (
    insurance_id integer NOT NULL,
    insurance_car_id integer NOT NULL,
    insurance_status_id integer NOT NULL,
    insurance_type public.insurance_type_enum NOT NULL,
    insurance_company character varying(100) NOT NULL,
    insurance_policy_number character varying(100) NOT NULL,
    insurance_start_date date NOT NULL,
    insurance_end_date date NOT NULL,
    insurance_cost numeric(10,2) NOT NULL,
    CONSTRAINT chk_insurance_dates CHECK (((insurance_start_date IS NULL) OR (insurance_end_date IS NULL) OR (insurance_start_date <= insurance_end_date)))
);


--
-- TOC entry 247 (class 1259 OID 18515)
-- Name: insurance_insurance_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.insurance ALTER COLUMN insurance_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.insurance_insurance_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 234 (class 1259 OID 16680)
-- Name: maintenance; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.maintenance (
    maintenance_id integer NOT NULL,
    maintenance_car_id integer NOT NULL,
    maintenance_work_type public.maintenance_type_enum,
    maintenance_description text,
    maintenance_cost numeric(10,2),
    maintenance_date date,
    CONSTRAINT chk_maintenance_cost_nonneg CHECK (((maintenance_cost IS NULL) OR (maintenance_cost >= (0)::numeric)))
);


--
-- TOC entry 233 (class 1259 OID 16679)
-- Name: maintenance_maintenance_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.maintenance ALTER COLUMN maintenance_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.maintenance_maintenance_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 250 (class 1259 OID 18544)
-- Name: payments; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.payments (
    payment_id integer NOT NULL,
    payment_bill_id integer NOT NULL,
    payment_sum numeric NOT NULL,
    payment_method public.payment_method DEFAULT 'Картой'::public.payment_method,
    payment_date timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


--
-- TOC entry 249 (class 1259 OID 18543)
-- Name: payments_payment_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.payments ALTER COLUMN payment_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.payments_payment_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 252 (class 1259 OID 18566)
-- Name: promocodes; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.promocodes (
    promocode_id integer NOT NULL,
    promocode_status_id integer NOT NULL,
    promocode_code character varying(50) NOT NULL,
    promocode_discount numeric(5,2) NOT NULL,
    promocode_start_date date NOT NULL,
    promocode_end_date date NOT NULL,
    CONSTRAINT chk_promocode_dates CHECK (((promocode_start_date IS NULL) OR (promocode_end_date IS NULL) OR (promocode_start_date <= promocode_end_date)))
);


--
-- TOC entry 251 (class 1259 OID 18565)
-- Name: promocodes_promocode_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.promocodes ALTER COLUMN promocode_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.promocodes_promocode_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 254 (class 1259 OID 18591)
-- Name: reviews; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.reviews (
    review_id integer NOT NULL,
    review_client_id integer NOT NULL,
    review_car_id integer NOT NULL,
    review_rating smallint NOT NULL,
    review_comment text,
    review_date timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT reviews_review_rating_check CHECK (((review_rating >= 1) AND (review_rating <= 5)))
);


--
-- TOC entry 253 (class 1259 OID 18590)
-- Name: reviews_review_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.reviews ALTER COLUMN review_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.reviews_review_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 220 (class 1259 OID 16390)
-- Name: roles; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.roles (
    role_id integer NOT NULL,
    role_name public.role_name NOT NULL
);


--
-- TOC entry 219 (class 1259 OID 16389)
-- Name: roles_role_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.roles ALTER COLUMN role_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.roles_role_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 238 (class 1259 OID 18111)
-- Name: specifications_car; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.specifications_car (
    specification_car_id integer NOT NULL,
    specification_car_fuel_type public.fuel_type NOT NULL,
    specification_car_brand character varying(50) NOT NULL,
    specification_car_model character varying(100) NOT NULL,
    specification_car_transmission public.transmission_type NOT NULL,
    specification_car_year integer NOT NULL,
    specification_car_vin_number character varying(17) NOT NULL,
    specification_car_state_number character varying(15) NOT NULL,
    specification_car_mileage integer DEFAULT 0 NOT NULL,
    specification_car_max_fuel numeric(5,2) NOT NULL,
    specification_fuel_per_km numeric(5,3) DEFAULT 0.08,
    CONSTRAINT chk_specification_car_max_fuel_positive CHECK ((specification_car_max_fuel >= (0)::numeric)),
    CONSTRAINT chk_specification_car_mileage_non_negative CHECK ((specification_car_mileage >= 0)),
    CONSTRAINT chk_specification_car_year_range CHECK (((specification_car_year >= 1900) AND ((specification_car_year)::numeric <= (EXTRACT(year FROM CURRENT_DATE) + (1)::numeric)))),
    CONSTRAINT specifications_car_specification_fuel_per_km_check CHECK ((specification_fuel_per_km >= (0)::numeric))
);


--
-- TOC entry 237 (class 1259 OID 18110)
-- Name: specifications_car_specification_car_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.specifications_car ALTER COLUMN specification_car_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.specifications_car_specification_car_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 222 (class 1259 OID 16398)
-- Name: status; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.status (
    status_id integer NOT NULL,
    status_name character varying(50) NOT NULL,
    status_description text
);


--
-- TOC entry 221 (class 1259 OID 16397)
-- Name: status_status_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.status ALTER COLUMN status_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.status_status_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 224 (class 1259 OID 16408)
-- Name: tariffs; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tariffs (
    tariff_id integer NOT NULL,
    tariff_name character varying(100),
    tariff_price_per_minute numeric(10,2),
    tariff_price_per_km numeric(10,2),
    tariff_price_per_day numeric(10,2),
    CONSTRAINT chk_tariff_prices_nonneg CHECK (((COALESCE(tariff_price_per_minute, (0)::numeric) >= (0)::numeric) AND (COALESCE(tariff_price_per_km, (0)::numeric) >= (0)::numeric) AND (COALESCE(tariff_price_per_day, (0)::numeric) >= (0)::numeric)))
);


--
-- TOC entry 223 (class 1259 OID 16407)
-- Name: tariffs_tariff_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.tariffs ALTER COLUMN tariff_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.tariffs_tariff_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 256 (class 1259 OID 18634)
-- Name: trip_details; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.trip_details (
    trip_detail_id integer NOT NULL,
    trip_detail_trip_id integer NOT NULL,
    trip_detail_start_location character varying(50) NOT NULL,
    trip_detail_end_location character varying(50) NOT NULL,
    trip_detail_insurance_active boolean DEFAULT false,
    trip_detail_fuel_used numeric(6,2) DEFAULT 0,
    trip_detail_refueled numeric(10,2) DEFAULT 0,
    CONSTRAINT chk_trip_detail_fuel_used_nonneg CHECK (((trip_detail_fuel_used IS NULL) OR (trip_detail_fuel_used >= (0)::numeric))),
    CONSTRAINT chk_trip_detail_refueled_nonneg CHECK (((trip_detail_refueled IS NULL) OR (trip_detail_refueled >= (0)::numeric))),
    CONSTRAINT trip_details_trip_detail_fuel_used_check CHECK ((trip_detail_fuel_used >= (0)::numeric)),
    CONSTRAINT trip_details_trip_detail_refueled_check CHECK ((trip_detail_refueled >= (0)::numeric))
);


--
-- TOC entry 255 (class 1259 OID 18633)
-- Name: trip_details_trip_detail_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.trip_details ALTER COLUMN trip_detail_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.trip_details_trip_detail_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 242 (class 1259 OID 18363)
-- Name: trips; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.trips (
    trip_id integer NOT NULL,
    trip_booking_id integer NOT NULL,
    trip_status_id integer NOT NULL,
    trip_tariff_type public.tariff_type NOT NULL,
    trip_start_time timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    trip_end_time timestamp without time zone,
    trip_duration numeric DEFAULT 0,
    trip_distance_km numeric(10,2) DEFAULT 0,
    CONSTRAINT chk_trip_distance_nonneg CHECK (((trip_distance_km IS NULL) OR (trip_distance_km >= (0)::numeric))),
    CONSTRAINT chk_trip_duration_nonneg CHECK (((trip_duration IS NULL) OR (trip_duration >= (0)::numeric))),
    CONSTRAINT chk_trip_times CHECK (((trip_start_time IS NULL) OR (trip_end_time IS NULL) OR (trip_start_time <= trip_end_time)))
);


--
-- TOC entry 241 (class 1259 OID 18362)
-- Name: trips_trip_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.trips ALTER COLUMN trip_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.trips_trip_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 228 (class 1259 OID 16436)
-- Name: users; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.users (
    user_id integer NOT NULL,
    user_role_id integer NOT NULL,
    user_login character varying(100) NOT NULL,
    user_password_hash character varying(256) NOT NULL
);


--
-- TOC entry 227 (class 1259 OID 16435)
-- Name: users_user_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.users ALTER COLUMN user_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.users_user_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 5037 (class 2606 OID 16967)
-- Name: clients clients_client_email_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.clients
    ADD CONSTRAINT clients_client_email_key UNIQUE (client_email);


--
-- TOC entry 5039 (class 2606 OID 16969)
-- Name: clients clients_client_phone_number_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.clients
    ADD CONSTRAINT clients_client_phone_number_key UNIQUE (client_phone_number);


--
-- TOC entry 5063 (class 2606 OID 18532)
-- Name: insurance insurance_insurance_policy_number_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.insurance
    ADD CONSTRAINT insurance_insurance_policy_number_key UNIQUE (insurance_policy_number);


--
-- TOC entry 5029 (class 2606 OID 16567)
-- Name: bookings pk_bookings_booking_id; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.bookings
    ADD CONSTRAINT pk_bookings_booking_id PRIMARY KEY (booking_id);


--
-- TOC entry 5051 (class 2606 OID 18155)
-- Name: cars pk_cars_car_id; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.cars
    ADD CONSTRAINT pk_cars_car_id PRIMARY KEY (car_id);


--
-- TOC entry 5023 (class 2606 OID 16420)
-- Name: categories pk_categories_category_id; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.categories
    ADD CONSTRAINT pk_categories_category_id PRIMARY KEY (category_id);


--
-- TOC entry 5041 (class 2606 OID 16945)
-- Name: clients pk_clients_client_id; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.clients
    ADD CONSTRAINT pk_clients_client_id PRIMARY KEY (client_id);


--
-- TOC entry 5061 (class 2606 OID 18504)
-- Name: fines pk_fines_fine_id; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.fines
    ADD CONSTRAINT pk_fines_fine_id PRIMARY KEY (fine_id);


--
-- TOC entry 5065 (class 2606 OID 18530)
-- Name: insurance pk_insurance_insurance_id; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.insurance
    ADD CONSTRAINT pk_insurance_insurance_id PRIMARY KEY (insurance_id);


--
-- TOC entry 5031 (class 2606 OID 16638)
-- Name: bills pk_invoices_bill_id; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.bills
    ADD CONSTRAINT pk_invoices_bill_id PRIMARY KEY (bill_id);


--
-- TOC entry 5035 (class 2606 OID 16688)
-- Name: maintenance pk_maintenance_id; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.maintenance
    ADD CONSTRAINT pk_maintenance_id PRIMARY KEY (maintenance_id);


--
-- TOC entry 5067 (class 2606 OID 18556)
-- Name: payments pk_payments_payment_id; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.payments
    ADD CONSTRAINT pk_payments_payment_id PRIMARY KEY (payment_id);


--
-- TOC entry 5069 (class 2606 OID 18577)
-- Name: promocodes pk_promocodes_promocode_id; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.promocodes
    ADD CONSTRAINT pk_promocodes_promocode_id PRIMARY KEY (promocode_id);


--
-- TOC entry 5073 (class 2606 OID 18603)
-- Name: reviews pk_reviews_review_id; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.reviews
    ADD CONSTRAINT pk_reviews_review_id PRIMARY KEY (review_id);


--
-- TOC entry 5017 (class 2606 OID 16396)
-- Name: roles pk_roles_role_id; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.roles
    ADD CONSTRAINT pk_roles_role_id PRIMARY KEY (role_id);


--
-- TOC entry 5045 (class 2606 OID 18131)
-- Name: specifications_car pk_specification_car_id; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.specifications_car
    ADD CONSTRAINT pk_specification_car_id PRIMARY KEY (specification_car_id);


--
-- TOC entry 5019 (class 2606 OID 16406)
-- Name: status pk_status_status_id; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.status
    ADD CONSTRAINT pk_status_status_id PRIMARY KEY (status_id);


--
-- TOC entry 5021 (class 2606 OID 16413)
-- Name: tariffs pk_tariffs_tariff_id; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tariffs
    ADD CONSTRAINT pk_tariffs_tariff_id PRIMARY KEY (tariff_id);


--
-- TOC entry 5055 (class 2606 OID 18380)
-- Name: trips pk_trips_trip_id; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.trips
    ADD CONSTRAINT pk_trips_trip_id PRIMARY KEY (trip_id);


--
-- TOC entry 5059 (class 2606 OID 18486)
-- Name: client_documents pk_user_documents_document_id; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.client_documents
    ADD CONSTRAINT pk_user_documents_document_id PRIMARY KEY (document_id);


--
-- TOC entry 5025 (class 2606 OID 16449)
-- Name: users pk_users_user_id; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT pk_users_user_id PRIMARY KEY (user_id);


--
-- TOC entry 5071 (class 2606 OID 18579)
-- Name: promocodes promocodes_promocode_code_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.promocodes
    ADD CONSTRAINT promocodes_promocode_code_key UNIQUE (promocode_code);


--
-- TOC entry 5075 (class 2606 OID 18649)
-- Name: trip_details trip_details_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.trip_details
    ADD CONSTRAINT trip_details_pkey PRIMARY KEY (trip_detail_id);


--
-- TOC entry 5077 (class 2606 OID 18651)
-- Name: trip_details trip_details_trip_id_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.trip_details
    ADD CONSTRAINT trip_details_trip_id_key UNIQUE (trip_detail_trip_id);


--
-- TOC entry 5047 (class 2606 OID 18133)
-- Name: specifications_car uk_specification_car_state_number; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.specifications_car
    ADD CONSTRAINT uk_specification_car_state_number UNIQUE (specification_car_state_number);


--
-- TOC entry 5049 (class 2606 OID 18135)
-- Name: specifications_car uk_specification_car_vin_number; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.specifications_car
    ADD CONSTRAINT uk_specification_car_vin_number UNIQUE (specification_car_vin_number);


--
-- TOC entry 5033 (class 2606 OID 18472)
-- Name: bills uq_bill_trip; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.bills
    ADD CONSTRAINT uq_bill_trip UNIQUE (bill_trip_id);


--
-- TOC entry 5053 (class 2606 OID 18474)
-- Name: cars uq_car_specification; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.cars
    ADD CONSTRAINT uq_car_specification UNIQUE (car_specification_id);


--
-- TOC entry 5043 (class 2606 OID 18468)
-- Name: clients uq_client_user; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.clients
    ADD CONSTRAINT uq_client_user UNIQUE (client_user_id);


--
-- TOC entry 5057 (class 2606 OID 18470)
-- Name: trips uq_trip_booking; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.trips
    ADD CONSTRAINT uq_trip_booking UNIQUE (trip_booking_id);


--
-- TOC entry 5027 (class 2606 OID 16956)
-- Name: users users_user_login_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_user_login_key UNIQUE (user_login);


--
-- TOC entry 5107 (class 2620 OID 18562)
-- Name: payments trg_apply_payment_to_bill; Type: TRIGGER; Schema: public; Owner: -
--

CREATE TRIGGER trg_apply_payment_to_bill AFTER INSERT ON public.payments FOR EACH ROW EXECUTE FUNCTION public.apply_payment_to_bill();


--
-- TOC entry 5103 (class 2620 OID 17057)
-- Name: bills trg_calculate_bill_total; Type: TRIGGER; Schema: public; Owner: -
--

CREATE TRIGGER trg_calculate_bill_total BEFORE INSERT OR UPDATE ON public.bills FOR EACH ROW EXECUTE FUNCTION public.calculate_bill_total();


--
-- TOC entry 5105 (class 2620 OID 18391)
-- Name: trips trg_create_bill_after_trip; Type: TRIGGER; Schema: public; Owner: -
--

CREATE TRIGGER trg_create_bill_after_trip AFTER UPDATE OF trip_end_time ON public.trips FOR EACH ROW WHEN (((new.trip_end_time IS NOT NULL) AND (old.trip_end_time IS DISTINCT FROM new.trip_end_time))) EXECUTE FUNCTION public.create_bill_after_trip();


--
-- TOC entry 5108 (class 2620 OID 18563)
-- Name: payments trg_prevent_overpayment; Type: TRIGGER; Schema: public; Owner: -
--

CREATE TRIGGER trg_prevent_overpayment BEFORE INSERT OR UPDATE ON public.payments FOR EACH ROW EXECUTE FUNCTION public.prevent_overpayment();


--
-- TOC entry 5104 (class 2620 OID 17753)
-- Name: bills trg_set_initial_remaining_amount; Type: TRIGGER; Schema: public; Owner: -
--

CREATE TRIGGER trg_set_initial_remaining_amount BEFORE INSERT OR UPDATE ON public.bills FOR EACH ROW EXECUTE FUNCTION public.set_initial_remaining_amount();


--
-- TOC entry 5109 (class 2620 OID 18657)
-- Name: trip_details trg_set_trip_fuel_used; Type: TRIGGER; Schema: public; Owner: -
--

CREATE TRIGGER trg_set_trip_fuel_used BEFORE INSERT OR UPDATE ON public.trip_details FOR EACH ROW EXECUTE FUNCTION public.set_trip_fuel_used();


--
-- TOC entry 5106 (class 2620 OID 18392)
-- Name: trips trg_trip_duration; Type: TRIGGER; Schema: public; Owner: -
--

CREATE TRIGGER trg_trip_duration BEFORE INSERT OR UPDATE ON public.trips FOR EACH ROW EXECUTE FUNCTION public.calculate_trip_duration();


--
-- TOC entry 5110 (class 2620 OID 18658)
-- Name: trip_details trg_update_car_fuel_after_trip; Type: TRIGGER; Schema: public; Owner: -
--

CREATE TRIGGER trg_update_car_fuel_after_trip AFTER INSERT OR UPDATE ON public.trip_details FOR EACH ROW WHEN (((new.trip_detail_fuel_used IS NOT NULL) OR (new.trip_detail_refueled IS NOT NULL))) EXECUTE FUNCTION public.update_car_fuel_after_trip();


--
-- TOC entry 5082 (class 2606 OID 18429)
-- Name: bills fk_bill_for_trip; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.bills
    ADD CONSTRAINT fk_bill_for_trip FOREIGN KEY (bill_trip_id) REFERENCES public.trips(trip_id) ON DELETE CASCADE;


--
-- TOC entry 5083 (class 2606 OID 18585)
-- Name: bills fk_bill_has_promocode; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.bills
    ADD CONSTRAINT fk_bill_has_promocode FOREIGN KEY (bill_promocode_id) REFERENCES public.promocodes(promocode_id) ON DELETE SET NULL;


--
-- TOC entry 5084 (class 2606 OID 16654)
-- Name: bills fk_bill_status; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.bills
    ADD CONSTRAINT fk_bill_status FOREIGN KEY (bill_status_id) REFERENCES public.status(status_id) ON DELETE SET NULL;


--
-- TOC entry 5079 (class 2606 OID 18181)
-- Name: bookings fk_booking_car; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.bookings
    ADD CONSTRAINT fk_booking_car FOREIGN KEY (booking_car_id) REFERENCES public.cars(car_id) ON DELETE SET NULL;


--
-- TOC entry 5080 (class 2606 OID 16985)
-- Name: bookings fk_booking_has_client; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.bookings
    ADD CONSTRAINT fk_booking_has_client FOREIGN KEY (booking_client_id) REFERENCES public.clients(client_id) ON DELETE CASCADE;


--
-- TOC entry 5081 (class 2606 OID 16568)
-- Name: bookings fk_booking_status; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.bookings
    ADD CONSTRAINT fk_booking_status FOREIGN KEY (booking_status_id) REFERENCES public.status(status_id) ON DELETE SET NULL;


--
-- TOC entry 5087 (class 2606 OID 18156)
-- Name: cars fk_car_has_category; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.cars
    ADD CONSTRAINT fk_car_has_category FOREIGN KEY (car_category_id) REFERENCES public.categories(category_id) ON DELETE SET NULL;


--
-- TOC entry 5088 (class 2606 OID 18161)
-- Name: cars fk_car_has_tariff; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.cars
    ADD CONSTRAINT fk_car_has_tariff FOREIGN KEY (car_tariff_id) REFERENCES public.tariffs(tariff_id) ON DELETE SET NULL;


--
-- TOC entry 5089 (class 2606 OID 18166)
-- Name: cars fk_car_specification; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.cars
    ADD CONSTRAINT fk_car_specification FOREIGN KEY (car_specification_id) REFERENCES public.specifications_car(specification_car_id);


--
-- TOC entry 5090 (class 2606 OID 18171)
-- Name: cars fk_car_status; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.cars
    ADD CONSTRAINT fk_car_status FOREIGN KEY (car_status_id) REFERENCES public.status(status_id) ON DELETE SET NULL;


--
-- TOC entry 5086 (class 2606 OID 16970)
-- Name: clients fk_client_has_user_account; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.clients
    ADD CONSTRAINT fk_client_has_user_account FOREIGN KEY (client_user_id) REFERENCES public.users(user_id) ON DELETE CASCADE;


--
-- TOC entry 5093 (class 2606 OID 18487)
-- Name: client_documents fk_documents_client; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.client_documents
    ADD CONSTRAINT fk_documents_client FOREIGN KEY (document_client_id) REFERENCES public.clients(client_id) ON DELETE CASCADE;


--
-- TOC entry 5094 (class 2606 OID 18505)
-- Name: fines fk_fine_for_trip; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.fines
    ADD CONSTRAINT fk_fine_for_trip FOREIGN KEY (fine_trip_id) REFERENCES public.trips(trip_id) ON DELETE CASCADE;


--
-- TOC entry 5095 (class 2606 OID 18510)
-- Name: fines fk_fine_status; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.fines
    ADD CONSTRAINT fk_fine_status FOREIGN KEY (fine_status_id) REFERENCES public.status(status_id) ON DELETE SET NULL;


--
-- TOC entry 5096 (class 2606 OID 18533)
-- Name: insurance fk_insurance_for_car; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.insurance
    ADD CONSTRAINT fk_insurance_for_car FOREIGN KEY (insurance_car_id) REFERENCES public.cars(car_id) ON DELETE CASCADE;


--
-- TOC entry 5097 (class 2606 OID 18538)
-- Name: insurance fk_insurance_status; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.insurance
    ADD CONSTRAINT fk_insurance_status FOREIGN KEY (insurance_status_id) REFERENCES public.status(status_id) ON DELETE SET NULL;


--
-- TOC entry 5085 (class 2606 OID 18186)
-- Name: maintenance fk_maintenance_for_car; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.maintenance
    ADD CONSTRAINT fk_maintenance_for_car FOREIGN KEY (maintenance_car_id) REFERENCES public.cars(car_id) ON DELETE CASCADE;


--
-- TOC entry 5098 (class 2606 OID 18557)
-- Name: payments fk_payments_bill_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.payments
    ADD CONSTRAINT fk_payments_bill_id FOREIGN KEY (payment_bill_id) REFERENCES public.bills(bill_id) ON DELETE CASCADE;


--
-- TOC entry 5099 (class 2606 OID 18580)
-- Name: promocodes fk_promocode_status; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.promocodes
    ADD CONSTRAINT fk_promocode_status FOREIGN KEY (promocode_status_id) REFERENCES public.status(status_id) ON DELETE SET NULL;


--
-- TOC entry 5100 (class 2606 OID 18604)
-- Name: reviews fk_review_for_car; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.reviews
    ADD CONSTRAINT fk_review_for_car FOREIGN KEY (review_car_id) REFERENCES public.cars(car_id) ON DELETE CASCADE;


--
-- TOC entry 5101 (class 2606 OID 18609)
-- Name: reviews fk_review_has_client; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.reviews
    ADD CONSTRAINT fk_review_has_client FOREIGN KEY (review_client_id) REFERENCES public.clients(client_id) ON DELETE CASCADE;


--
-- TOC entry 5091 (class 2606 OID 18381)
-- Name: trips fk_trip_has_booking; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.trips
    ADD CONSTRAINT fk_trip_has_booking FOREIGN KEY (trip_booking_id) REFERENCES public.bookings(booking_id) ON DELETE CASCADE;


--
-- TOC entry 5092 (class 2606 OID 18386)
-- Name: trips fk_trip_status; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.trips
    ADD CONSTRAINT fk_trip_status FOREIGN KEY (trip_status_id) REFERENCES public.status(status_id) ON DELETE SET NULL;


--
-- TOC entry 5078 (class 2606 OID 16452)
-- Name: users fk_user_has_role; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT fk_user_has_role FOREIGN KEY (user_role_id) REFERENCES public.roles(role_id) ON DELETE SET NULL;


--
-- TOC entry 5102 (class 2606 OID 18652)
-- Name: trip_details trip_details_trip_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.trip_details
    ADD CONSTRAINT trip_details_trip_id_fkey FOREIGN KEY (trip_detail_trip_id) REFERENCES public.trips(trip_id) ON DELETE CASCADE;


-- Completed on 2025-11-16 19:11:23

--
-- PostgreSQL database dump complete
--
INSERT INTO public.roles (role_name)
SELECT 'Администратор'
WHERE NOT EXISTS (SELECT 1 FROM public.roles WHERE role_name = 'Администратор');

INSERT INTO public.roles (role_name)
SELECT 'Клиент'
WHERE NOT EXISTS (SELECT 1 FROM public.roles WHERE role_name = 'Клиент');

INSERT INTO public.status (status_name) VALUES
('Создано'),
('Подтверждено'),
('Завершено'),
('Оплачен'),
('Частично оплачен'),
('Выставлен'),
('В пути'),
('Отменено');

\unrestrict lAE0UUYfBmecnXe4if1vfrxuvMwMZ16eQ0s0C5pwTUJMp8dIz8LyAOT1oXK4tZF

DO $$
DECLARE
    v_id_role INT;
    v_id_user UUID;
    v_id_room INT;
    v_id_room2 INT;
    v_id_booking UUID;
    v_id_method INT;
    v_id_type INT;
    v_total_price DECIMAL;
BEGIN
    -- 1. Insert Roles
    INSERT INTO roles (role_name) VALUES ('Admin'), ('Customer') ON CONFLICT (role_name) DO NOTHING;
    SELECT id_role INTO v_id_role FROM roles WHERE role_name = 'Customer' LIMIT 1;

    -- 2. Setup User
    INSERT INTO users (id_role, first_name, last_name, email, document_id, password_hash)
    VALUES (v_id_role, 'Carlos', 'Test', 'carlos@test.com', '123456', 'hashTest')
    ON CONFLICT (email) DO NOTHING;
    
    SELECT id_user INTO v_id_user FROM users WHERE email = 'carlos@test.com' LIMIT 1;

    -- 3. Setup Rooms
    INSERT INTO rooms (room_number, room_type, base_price, guest_capacity, description)
    VALUES ('101', 'Single', 50.00, 1, 'Habitacion sencilla para probar'),
           ('102', 'Double', 80.00, 2, 'Habitacion doble de prueba')
    ON CONFLICT (room_number) DO NOTHING;

    SELECT id_room INTO v_id_room FROM rooms WHERE room_number = '101' LIMIT 1;
    SELECT id_room INTO v_id_room2 FROM rooms WHERE room_number = '102' LIMIT 1;

    -- 4. Setup Payment dictionaries
    INSERT INTO payment_methods (name) VALUES ('Credit Card'), ('Cash') ON CONFLICT (name) DO NOTHING;
    SELECT id_method INTO v_id_method FROM payment_methods WHERE name = 'Credit Card' LIMIT 1;

    INSERT INTO payment_statuses (name) VALUES ('Completed'), ('Pending') ON CONFLICT (name) DO NOTHING;
    
    INSERT INTO invoice_types (name) VALUES ('Standard') ON CONFLICT (name) DO NOTHING;
    SELECT id_type INTO v_id_type FROM invoice_types WHERE name = 'Standard' LIMIT 1;

    -- 5. Using Stored Procedures (Calling the functions)
    -- We assume the functions exist as they were mentioned previously.
    -- Calling fn_calculate_total_price (room_id, check_in, check_out, guests)
    BEGIN
        SELECT fn_calculate_total_price(v_id_room, CURRENT_DATE, CURRENT_DATE + 3, 1) INTO v_total_price;
        RAISE NOTICE 'Calculated total price for Room 101: %', v_total_price;
    EXCEPTION WHEN OTHERS THEN
        RAISE NOTICE 'Error in fn_calculate_total_price: %', SQLERRM;
    END;

    -- Calling fn_create_booking (user_id, room_id, check_in, check_out, guests)
    -- Assuming it returns UUID or just performs the action.
    BEGIN
        -- Trying to execute as a function that returns the booking ID
        SELECT fn_create_booking(v_id_user, v_id_room2, CURRENT_DATE + 5, CURRENT_DATE + 10, 2) INTO v_id_booking;
        RAISE NOTICE 'Created booking: %', v_id_booking;
    EXCEPTION WHEN OTHERS THEN
        BEGIN
            -- Maybe it doesn't return anything (void)
            PERFORM fn_create_booking(v_id_user, v_id_room2, CURRENT_DATE + 5, CURRENT_DATE + 10, 2);
            RAISE NOTICE 'Created booking successfully (no return value)';
            -- Manually fetch the booking we just created
            SELECT id_booking INTO v_id_booking FROM bookings WHERE id_user = v_id_user ORDER BY created_at DESC LIMIT 1;
        EXCEPTION WHEN OTHERS THEN
            RAISE NOTICE 'Error in fn_create_booking: %', SQLERRM;
        END;
    END;

    -- If a booking was created, we test fn_register_payment
    IF v_id_booking IS NOT NULL THEN
        BEGIN
            PERFORM fn_register_payment(v_id_booking, 160.00, v_id_method, v_id_type, 'REF-12345');
            RAISE NOTICE 'Registered payment for booking : %', v_id_booking;
        EXCEPTION WHEN OTHERS THEN
            RAISE NOTICE 'Error in fn_register_payment: %', SQLERRM;
        END;
    END IF;

END $$;

INSERT INTO roles (role_name)
SELECT 'Администратор'
WHERE NOT EXISTS (SELECT 1 FROM roles WHERE role_name = 'Администратор');

INSERT INTO roles (role_name)
SELECT 'Клиент'
WHERE NOT EXISTS (SELECT 1 FROM roles WHERE role_name = 'Клиент');
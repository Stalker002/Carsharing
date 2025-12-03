import { useEffect, useState } from "react";
import "./Register.css";
import { useMask } from '@react-input/mask';
import { useDispatch } from "react-redux";
import { createClient } from "../../redux/actions/clients";
import { loginUser } from "../../redux/actions/users";

export default function Register({ isOpen, onClose, onLoginClick }) {
    const inputRef = useMask({
        mask: "+___ (__) ___-__-__",
        replacement: { _: /\d/ },
    });

    const phoneRegex = /^(\+375|80)(29|44|33|25)\d{7}$/;

    const emailRegex = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$/;

    const dispatch = useDispatch();
    const [error, setError] = useState("");

    const [regForm, setRegForm] = useState({
        name: "",
        surname: "",
        phoneNumber: "",
        email: "",
        login: "",
        password: "",
        confirmPassword: ""
    });

    useEffect(() => {
        if (isOpen) {
            setRegForm({
                name: "",
                surname: "",
                phoneNumber: "",
                email: "",
                login: "",
                password: "",
                confirmPassword: ""
            });
            setError("");
        }
    }, [isOpen]);

    useEffect(() => {
        const handleKeyPress = (event) => {
            if (event.key === 'Escape') onClose();
        };
        if (isOpen) {
            document.addEventListener('keydown', handleKeyPress);
        }
        return () => document.removeEventListener("keydown", handleKeyPress);
    }, [isOpen, onClose]);

    const regInputConfig = [
        { label: "Фамилия", name: "surname", input: "Введите фамилию", type: "text" },
        { label: "Имя", name: "name", input: "Введите имя", type: "text" },
        { label: "Почта", name: "email", input: "Введите почту", type: "email" },
        { label: "Номер телефона", name: "phoneNumber", input: "+375 (__) ___-__-__" },
        { label: "Логин", name: "login", input: "Введите логин", type: "text" },
        { label: "Пароль", name: "password", input: "Введите пароль", type: "password" },
        { label: "Подтверждение пароля", name: "confirmPassword", input: "Подтверждение пароля", type: "password" },
    ];

    const handleChange = (e) => {
        const { name, value } = e.target;
        setRegForm(prev => ({ ...prev, [name]: value }));
        if (error) setError("");
    };

    const submitReg = async (e) => {
        e.preventDefault();

        if (!regForm.name || !regForm.surname || !regForm.login) {
            setError("Пожалуйста, заполните все поля.");
            return;
        }

        if (!emailRegex.test(regForm.email)) {
            setError("Введите корректный email!");
            return;
        }

        if (!regForm.phoneNumber || regForm.phoneNumber.length < 10) {
            setError("Введите корректный номер телефона!");
            return;
        }

        if (regForm.password.length < 6) {
            setError("Пароль должен быть минимум 6 символов!");
            return;
        }

        if (regForm.password !== regForm.confirmPassword) {
            setError("Пароли не совпадают!");
            return;
        }

        const cleanedPhone = regForm.phoneNumber.replace(/[^\d+]/g, '');

        if (!phoneRegex.test(cleanedPhone)) {
            setError("Phone number should be in format +375XXXXXXXXX or 80XXXXXXXXX");
            return;
        }

        const { confirmPassword, ...dataToSend } = {
            ...regForm,
            phoneNumber: cleanedPhone
        };

        try {
            // ✅ 1. ЖДЕМ ЗАВЕРШЕНИЯ РЕГИСТРАЦИИ (Thunk возвращает Promise)
            const result = await dispatch(createClient(dataToSend));

            // 2. Проверяем успех, если Thunk вернул объект { success: true }
            if (result && result.success) {
                // Если регистрация успешна:
                await dispatch(loginUser(dataToSend.login, dataToSend.password));
                onClose(); // ✅ Закрываем модалку ТОЛЬКО при успехе
            } else {
                // Если Thunk вернул { success: false, message: '...' }
                setError(result.message || "Неизвестная ошибка регистрации");
            }

        } catch (error) {
            // 💡 Этот блок сработает, если Thunk бросил исключение (например, 500 ошибка)
            // В вашем случае, Thunk сам обрабатывает ошибки через return { success: false, ... },
            // но лучше оставить этот блок на случай непредвиденных исключений.
            setError(error.message || "Сетевая ошибка или сбой сервера.");
        }
    };

    if (!isOpen) return null;

    return (
        <div className="modal-overlay" onClick={onClose}>
            <div className="modal-container" onClick={(e) => e.stopPropagation()}>
                <button className="modal-close" onClick={onClose}>×</button>

                <h2>Регистрация</h2>
                <p className="login-option">Заполните данные, чтобы создать аккаунт</p>

                <form onSubmit={submitReg}>
                    <div>
                        {regInputConfig.map((item) => (
                            <div className='reg-element' key={item.name}>
                                <label className='element-name'>{item.label}</label>
                                {item.name === "phoneNumber" ? (
                                    <input
                                        ref={inputRef}
                                        value={regForm.phoneNumber}
                                        className="element-input"
                                        placeholder="+375 (__) ___-__-__"
                                        onInput={(e) =>
                                            setRegForm(prev => ({ ...prev, phoneNumber: e.target.value }))
                                        }
                                    />
                                ) : (
                                    <input
                                        className="element-input"
                                        placeholder={item.input}
                                        type={item.type}
                                        name={item.name}
                                        onChange={handleChange}
                                        value={regForm[item.name]}
                                        required
                                    />
                                )}
                            </div>
                        ))}
                    </div>

                    {error && <div style={{ color: 'red', marginTop: '10px' }}>{error}</div>}

                    <button type="submit" className="submit-btn">
                        Зарегистрироваться
                    </button>

                    <p className="agreement">
                        Уже есть аккаунт? <br />
                        <button type="button" className="link-btn" onClick={onLoginClick}>
                            Войти
                        </button>
                    </p>
                </form>
            </div>
        </div>
    );
}
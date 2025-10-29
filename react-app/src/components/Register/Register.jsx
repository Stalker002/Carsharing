import { useState } from "react";
import "./Register.css";
import InputMask from "react-input-mask";

import Hide from "./../../svg/Login/eye_hide.svg"
import Visible from "./../../svg/Login/visible_hide.svg"

export default function Register({ isOpen, onClose, onLoginClick }) {
    const [login, setLogin] = useState("");
    const [email, setEmail] = useState("");
    const [phone, setPhone] = useState("");
    const [pass, setPass] = useState("");
    const [confirmPass, setConfirmPass] = useState("");
    const [showPass, setShowPass] = useState(false);
    const [showConfirmPass, setShowConfirmPass] = useState(false);

    const formatPhone = (value) => {
        // Убираем всё кроме цифр
        const digits = value.replace(/\D/g, "");

        // Формируем по шаблону +375 (XX) XXX-XX-XX
        let result = "+375 ";

        if (digits.length > 3) result += "(";
        if (digits.length >= 5) result += digits.slice(3, 5) + ") ";
        else if (digits.length > 3) result += digits.slice(3);

        if (digits.length >= 8) result += digits.slice(5, 8) + "-";
        else if (digits.length > 5) result += digits.slice(5);

        if (digits.length >= 10) result += digits.slice(8, 10) + "-";
        else if (digits.length > 8) result += digits.slice(8);

        if (digits.length >= 12) result += digits.slice(10, 12);
        else if (digits.length > 10) result += digits.slice(10);

        return result.trim();
    };

    const handleChange = (e) => {
        const formatted = formatPhone(e.target.value);
        setPhone(formatted);
    };

    if (!isOpen) return null; // не рендерим, если окно закрыто

    return (
        <div className="modal-overlay" onClick={onClose}>
            <div className="modal-container" onClick={(e) => e.stopPropagation()}>
                <button className="modal-close" onClick={onClose}>×</button>

                <h2>Регистрация</h2>
                <p className="login-option">Заполните данные, чтобы создать аккаунт</p>

                <form className="login-form">
                    <div className="login-input">
                        <input
                            type="login"
                            placeholder="Ваш логин"
                            value={login}
                            onChange={(e) => setLogin(e.target.value)}
                        />
                    </div>
                    <div className="login-input">
                        <input
                            type="email"
                            placeholder="Ваша почта"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                        />
                    </div>
                    <div className="login-input">
                        <InputMask
                            mask="+375 (99) 999-99-99"
                            value={phone}
                            onChange={(e) => setPhone(e.target.value)}
                        >
                            {(inputProps) => (
                                <input
                                    {...inputProps}
                                    type="tel"
                                    placeholder="+375 (__) ___-__-__"
                                    className="phone-input"
                                />
                            )}
                        </InputMask>
                    </div>
                    <div className="pass-input">
                        <input
                            type={showPass ? "text" : "password"} // 🔹 Меняем тип input
                            placeholder="Пароль"
                            value={pass}
                            onChange={(e) => setPass(e.target.value)}
                            required
                        />
                        <button
                            type="button"
                            className="reg-toggle-pass"
                            onClick={() => setShowPass(!showPass)} // 🔹 Меняем состояние
                        >
                            <img
                                src={showPass ? Hide : Visible}
                                alt={showPass ? "Скрыть" : "Показать"}
                                width={35}
                                height={35}
                            />
                        </button>
                    </div>
                    <div className="pass-input">
                        <input
                            type={showConfirmPass ? "text" : "password"} // 🔹 Меняем тип input
                            placeholder="Повторите пароль"
                            value={confirmPass}
                            onChange={(e) => setConfirmPass(e.target.value)}
                            required
                        />
                        <button
                            type="button"
                            className="reg-toggle-confpass"
                            onClick={() => setShowConfirmPass(!showConfirmPass)} // 🔹 Меняем состояние
                        >
                            <img
                                src={showConfirmPass ? Hide : Visible}
                                alt={showConfirmPass ? "Скрыть" : "Показать"}
                                width={35}
                                height={35}
                            />
                        </button>
                    </div>
                    <button type="submit" className="submit-btn">
                        Зарегистрироваться
                    </button>
                    <p className="agreement">
                        Уже есть аккаунт?
                        <br />
                        <button type="button" className="link-btn" onClick={onLoginClick}>
                            Войти
                        </button>
                    </p>
                </form>
            </div>
        </div>
    )
}
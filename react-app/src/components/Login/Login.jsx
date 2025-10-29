import { useState } from "react";
import "./Login.css";
import Hide from "./../../svg/Login/eye_hide.svg"
import Visible from "./../../svg/Login/visible_hide.svg"

export default function Login({ isOpen, onClose, onRegisterClick }) {
    const [login, setLogin] = useState("");
    const [pass, setPass] = useState("");
    const [showPass, setShowPass] = useState(false);

    if (!isOpen) return null; // не рендерим, если окно закрыто

    return (
        <div className="modal-overlay" onClick={onClose}>
            <div className="modal-container" onClick={(e) => e.stopPropagation()}>
                <button className="modal-close" onClick={onClose}>×</button>

                <h2>Авторизация</h2>
                <p className="login-option">Введите данные чтобы войти</p>

                <form className="login-form">
                    <div className="login-input">
                        <input
                            type="login"
                            placeholder="Ваш логин"
                            value={login}
                            onChange={(e) => setLogin(e.target.value)}
                        />
                    </div>
                    <div className="pass-input">
                        <input
                            type= {showPass ? "text" : "password"} // 🔹 Меняем тип input
                            placeholder="Пароль"
                            value={pass}
                            onChange={(e) => setPass(e.target.value)}
                            required
                        />
                        <button
                            type="button"
                            className="toggle-pass"
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
                    <button type="submit" className="submit-btn">
                        Войти
                    </button>
                    <p className="agreement">
                        Нет аккаунта?
                        <br />
                        <button type="button" className="link-btn" onClick={onRegisterClick}>
                            Зарегистрироваться
                        </button>
                    </p>
                </form>
            </div>
        </div>
    )
}
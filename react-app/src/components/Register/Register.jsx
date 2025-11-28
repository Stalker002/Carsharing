import { useState } from "react";
import "./Register.css";
import { useMask, format } from '@react-input/mask';

import Hide from "./../../svg/Login/eye_hide.svg"
import Visible from "./../../svg/Login/visible_hide.svg"

export default function Register({ isOpen, onClose, onLoginClick }) {
    const [login, setLogin] = useState("");
    const [name, setName] = useState("");
    const [surname, setSurname] = useState("");
    const [email, setEmail] = useState("");
    const [phone, setPhone] = useState("");
    const [pass, setPass] = useState("");
    const [confirmPass, setConfirmPass] = useState("");
    const [showPass, setShowPass] = useState(false);
    const [showConfirmPass, setShowConfirmPass] = useState(false);


    const options = {
        mask: '+375 (__) ___-__-__',
        replacement: { _: /\d/ },
    };
    const inputRef = useMask(options);
    const defaultValue = format('0123456789', options);

    if (!isOpen) return null;

    return (
        <div className="modal-overlay" onClick={onClose}>
            <div className="modal-container" onClick={(e) => e.stopPropagation()}>
                <button className="modal-close" onClick={onClose}>×</button>

                <h2>Регистрация</h2>
                <p className="login-option">Заполните данные, чтобы создать аккаунт</p>

                <form className="login-form">
                    <div className="login-input">
                        <input
                            type="name"
                            placeholder="Ваше имя"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                        />
                    </div>
                    <div className="login-input">
                        <input
                            type="surname"
                            placeholder="Ваша фамилия"
                            value={surname}
                            onChange={(e) => setSurname(e.target.value)}
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
                        <input
                            defaultValue={defaultValue}
                            type="tel"
                            placeholder="+375 (__) ___-__-__"
                            value={phone}
                            ref={inputRef}
                            onChange={e => setPhone(e.target.value)}

                        />
                    </div>
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
                            type={showPass ? "text" : "password"}
                            placeholder="Пароль"
                            value={pass}
                            onChange={(e) => setPass(e.target.value)}
                            required
                        />
                        <button
                            type="button"
                            className="reg-toggle-pass"
                            onClick={() => setShowPass(!showPass)}
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
                            type={showConfirmPass ? "text" : "password"}
                            placeholder="Повторите пароль"
                            value={confirmPass}
                            onChange={(e) => setConfirmPass(e.target.value)}
                            required
                        />
                        <button
                            type="button"
                            className="reg-toggle-confpass"
                            onClick={() => setShowConfirmPass(!showConfirmPass)}
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
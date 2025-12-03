import { useDispatch } from "react-redux";
import "./Login.css";

import { useEffect, useState } from "react";
import { loginUser } from "../../redux/actions/users";

export default function Login({ isOpen, onClose, onRegisterClick }) {
    const dispatch = useDispatch();
    const [error, setError] = useState("");

    const [authForm, setAuthForm] = useState({
        login: "",
        password: ""
    });

    useEffect(() => {
        if (isOpen) {
            setAuthForm({
                login: "",
                password: ""
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

    const autInputConfig = [
        { label: "Логин", name: "login", input: "Введите логин", type: "text" },
        { label: "Пароль", name: "password", input: "Введите пароль", type: "password" },
    ];

    const changeAuth = (e) => {
        setAuthForm(prev => ({ ...prev, [e.target.name]: e.target.value }));
    };

    const submitAuth = async (e) => {
        e.preventDefault();

        if (!authForm.password || !authForm.login) {
            setError("Пожалуйста, заполните все поля.");
            return;
        }

        if (authForm.password.length < 6) {
            setError("Пароль должен быть минимум 6 символов!");
            return;
        }

        const result = dispatch(loginUser(authForm.login, authForm.password));
        
        if (result && result.success) {
            onClose();
        } else {
            setError(result.message || "Ошибка авторизации");
        }
    };

    if (!isOpen) return null;

    return (
        <div className="modal-overlay" onClick={onClose}>
            <div className="modal-container" onClick={(e) => e.stopPropagation()}>
                <button className="modal-close" onClick={onClose}>×</button>

                <h2>Авторизация</h2>
                <p className="login-option">Введите данные чтобы войти</p>

                <form className="login-form" onSubmit={submitAuth}>
                    {autInputConfig.map((item) => (
                        <div
                            className='login-element'
                            key={item.input}>
                            <label
                                className='element-name'
                                htmlFor="">{item.label}</label>
                            <input
                                className='element-input'
                                type={item.type}
                                name={item.name}
                                placeholder={item.input}
                                value={authForm[item.name]}
                                onChange={changeAuth}
                            />
                        </div>
                    ))}

                    {error && <div style={{ color: 'red', marginTop: '10px' }}>{error}</div>}

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
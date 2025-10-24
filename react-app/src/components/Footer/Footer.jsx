import './Footer.css';

function Footer() {
    return (
        <footer className="footer">
            <div className="footer-container">
                <div className="footer-main">
                    <div className="footer-brand">
                        <h1>CarShare</h1>
                        <p className="footer-description">
                            Наша компания всегда рада новым клиентам.
                        </p>
                    </div>
                    <div className="footer-links">
                        <div className="footer-column">
                            <h3 className="footer-title">Про нас</h3>
                            <ul className="footer-list">
                                <li><a href="#">Как это работает</a></li>
                                <li><a href="#">Рекомендации</a></li>
                                <li><a href="#">Сотрудничество</a></li>
                                <li><a href="#">Бизнес-сотрудничество</a></li>
                            </ul>
                        </div>
                        <div className="footer-column">
                            <h3 className="footer-title">Сообщество</h3>
                            <ul className="footer-list">
                                <li><a href="#">Мероприятия</a></li>
                                <li><a href="#">Блог</a></li>
                                <li><a href="#">Подкаст</a></li>
                            </ul>
                        </div>
                        <div className="footer-column">
                            <h3 className="footer-title">Социальные сети</h3>
                            <ul className="footer-list">
                                <li><a href="#">Discord</a></li>
                                <li><a href="#">Instagram</a></li>
                                <li><a href="#">Twitter</a></li>
                                <li><a href="#">Facebook</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div className="footer-down">
                    <div className="footer-copyright">
                        ©2025 Ozon. Все права защищены
                    </div>
                    <div className="footer-legal">
                        <a href="#">Политика конфиденциальности</a>
                        <a href="#">Правила и условия</a>
                    </div>
                </div>
            </div>
        </footer>
    );
}
export default Footer;
import './Footer.css';

function Footer() {
  const footerLinks = [
    {
      title: "Про нас",
      links: [
        { label: "Как это работает", href: "#" },
        { label: "Рекомендации", href: "#" },
        { label: "Сотрудничество", href: "#" },
        { label: "Бизнес-сотрудничество", href: "#" },
      ],
    },
    {
      title: "Сообщество",
      links: [
        { label: "Мероприятия", href: "#" },
        { label: "Блог", href: "#" },
        { label: "Подкаст", href: "#" },
      ],
    },
    {
      title: "Социальные сети",
      links: [
        { label: "Discord", href: "#" },
        { label: "Instagram", href: "#" },
        { label: "Twitter", href: "#" },
        { label: "Facebook", href: "#" },
      ],
    },
  ];

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
            {footerLinks.map((section) => (
              <div className="footer-column" key={section.title}>
                <h3 className="footer-title">{section.title}</h3>
                <ul className="footer-list">
                  {section.links.map((link) => (
                    <li key={link.label}>
                      <a href={link.href}>{link.label}</a>
                    </li>
                  ))}
                </ul>
              </div>
            ))}
          </div>
        </div>
        <div className="footer-down">
          <div className="footer-copyright">
            ©2025 CarShare. Все права защищены
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
import Exit from "./../../svg/Profile/exit.svg";

import "./SidebarAdmin.css"

function SidebarAdmin({ activeTab, setActiveTab }) {
  return (
    <div className="admin-sidebar">
      <nav className="sidebar-menu-admin">
        <button
          className={activeTab === "Dashboard" ? "sidebar-btn active" : "sidebar-btn"}
          onClick={() => setActiveTab("Dashboard")}>
          Статистика
        </button>
        <button
          className={activeTab === "cars" ? "sidebar-btn active" : "sidebar-btn"}
          onClick={() => setActiveTab("cars")}>
          Машины
        </button>
        <button
          className={activeTab === "users" ? "sidebar-btn active" : "sidebar-btn"}
          onClick={() => setActiveTab("users")}>
          Пользователи
        </button>
        <button
          className={activeTab === "clients" ? "sidebar-btn active" : "sidebar-btn"}
          onClick={() => setActiveTab("clients")}>
          Клиенты
        </button>
        <button
          className={activeTab === "bookings" ? "sidebar-btn active" : "sidebar-btn"}
          onClick={() => setActiveTab("bookings")}>
          Бронирования
        </button>
        <button
          className={activeTab === "trips" ? "sidebar-btn active" : "sidebar-btn"}
          onClick={() => setActiveTab("trips")}>
          Поездки
        </button>
        <button
          className={activeTab === "bills" ? "sidebar-btn active" : "sidebar-btn"}
          onClick={() => setActiveTab("bills")}>
          Счета
        </button>
        <button
          className={activeTab === "payments" ? "sidebar-btn active" : "sidebar-btn"}
          onClick={() => setActiveTab("payments")}>
          Платежи
        </button>
        <button
          className={activeTab === "promocodes" ? "sidebar-btn active" : "sidebar-btn"}
          onClick={() => setActiveTab("promocodes")}>
          Промокоды
        </button>
        <button
          className={activeTab === "reviews" ? "sidebar-btn active" : "sidebar-btn"}
          onClick={() => setActiveTab("reviews")}>
          Отзывы
        </button>
        <button
          className={activeTab === "insurance" ? "sidebar-btn active" : "sidebar-btn"}
          onClick={() => setActiveTab("insurance")}>
          Страховки
        </button>
        <button
          className={activeTab === "fines" ? "sidebar-btn active" : "sidebar-btn"}
          onClick={() => setActiveTab("fines")}>
          Штрафы
        </button>
        <div className="admin-action">
          <a>Помощь</a>
          <button className="logout"> <img src={Exit} />Выйти</button>
        </div>
      </nav>
    </div>
  );
}

export default SidebarAdmin
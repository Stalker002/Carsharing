import { useNavigate } from "react-router-dom";
import Exit from "./../../svg/Profile/exit.svg";
import "./SidebarAdmin.css";

function SidebarAdmin({ activeTab, setActiveTab }) {
  const navigate = useNavigate();

  const menuItems = [
    { id: "dashboard", label: "Статистика" },
    { id: "cars", label: "Машины" },
    { id: "users", label: "Пользователи" },
    { id: "bookings", label: "Бронирования" },
    { id: "trips", label: "Поездки" },
    { id: "bills", label: "Счета" },
    { id: "promocodes", label: "Промокоды" },
  ];

  return (
    <div className="admin-sidebar">
      <nav className="sidebar-menu-admin">
        {menuItems.map((item) => (
          <button
            key={item.id}
            className={`sidebar-btn ${activeTab === item.id ? "active" : ""}`}
            onClick={() => setActiveTab(item.id)}
          >
            {item.label}
          </button>
        ))}

        <div className="admin-action">
          <a>Помощь</a>
          <button className="logout" onClick={() => navigate("/")}>
            <img src={Exit} alt="Выход" />
            Выйти
          </button>
        </div>
      </nav>
    </div>
  );
}

export default SidebarAdmin;
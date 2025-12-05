import "./SideBarPersonal.css";
import Exit from "./../../svg/Profile/exit.svg";
import { useDispatch } from "react-redux";
import { logoutUser } from "../../redux/actions/users";
import { useNavigate } from "react-router-dom";

function SideBarPersonal({ active, onChange }) {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const menu = [
    { key: "profile", label: "Профиль пользователя", icon: "👤" },
    { key: "trip", label: "Текущая поездка", icon: "🚗" },
    { key: "history", label: "История поездок", icon: "📘" },
    { key: "help", label: "Помощь", icon: "❓" },
  ];

  return (
    <div className="sidebar">
      <div className="sidebar-menu">
        {menu.map((item) => (
          <button
            key={item.key}
            className={
              active === item.key ? "sidebar-item active" : "sidebar-item"
            }
            onClick={() => onChange(item.key)}
          >
            <span className="sidebar-icon">{item.icon}</span>
            {item.label}
          </button>
        ))}
      </div>
      <button
        className="sidebar-logout"
        onClick={() => {
          dispatch(logoutUser());
          navigate("/");
        }}
      >
        <img src={Exit} /> Выйти из аккаунта
      </button>
    </div>
  );
}

export default SideBarPersonal;

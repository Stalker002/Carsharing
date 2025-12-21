import { NavLink, useNavigate } from "react-router-dom";
import { useDispatch } from "react-redux";
import { logoutUser } from "../../redux/actions/users";
import Exit from "./../../svg/Profile/exit.svg";
import "./SideBarPersonal.css";

function SideBarPersonal() {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const menu = [
    { 
      path: "/personal-page", 
      label: "Профиль пользователя", 
      icon: "👤", 
      end: true
    }, 
    { 
      path: "/personal-page/current-trip", 
      label: "Текущая поездка", 
      icon: "🚗" 
    },
    { 
      path: "/personal-page/history", 
      label: "История поездок", 
      icon: "📘" 
    },
  ];

  return (
    <div className="sidebar">
      <div className="sidebar-menu">
        {menu.map((item) => (
          <NavLink
            key={item.path}
            to={item.path}
            end={item.end}
            className={({ isActive }) => 
              isActive ? "sidebar-item active" : "sidebar-item"
            }
          >
            <span className="sidebar-icon">{item.icon}</span>
            {item.label}
          </NavLink>
        ))}
      </div>
      <button
        className="sidebar-logout"
        onClick={() => {
          dispatch(logoutUser());
          navigate("/");
        }}
      >
        <img src={Exit} alt="Exit" /> Выйти из аккаунта
      </button>
    </div>
  );
}

export default SideBarPersonal;
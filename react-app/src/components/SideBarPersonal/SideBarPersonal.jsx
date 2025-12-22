import { NavLink, useNavigate } from "react-router-dom";
import { useDispatch } from "react-redux";
import { logoutUser } from "../../redux/actions/users";
import Profile from "./../../svg/Profile/Profile.svg";
import WProfile from "./../../svg/Profile/WhiteProfile.svg";
import Current from "./../../svg/Profile/Current.svg";
import WCurrent from "./../../svg/Profile/WhiteCurrent.svg";
import History from "./../../svg/Profile/History.svg";
import WHistory from "./../../svg/Profile/WhiteHistory.svg";
import Exit from "./../../svg/Profile/exit.svg";
import "./SideBarPersonal.css";

function SideBarPersonal() {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const menu = [
    { 
      path: "/personal-page", 
      label: "Профиль пользователя", 
      icon: Profile,
      whiteIcon: WProfile,
      end: true
    }, 
    { 
      path: "/personal-page/current-trip", 
      label: "Текущая поездка", 
      icon: Current,
      whiteIcon: WCurrent
    },
    { 
      path: "/personal-page/history", 
      label: "История поездок", 
      icon: History,
      whiteIcon: WHistory
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
            {({ isActive }) => (
              <>
                <img 
                  src={isActive ? item.whiteIcon : item.icon} 
                  className="sidebar-icon" 
                  alt={item.label}
                />
                {item.label}
              </>
            )}
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
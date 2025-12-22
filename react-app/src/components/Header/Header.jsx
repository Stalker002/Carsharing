import "./Header.css";
import Like from "../../svg/Header/like.svg";
import Search from "../../svg/Header/search.svg";
import Notification from "../../svg/Header/notification.svg";

import { Link, useLocation } from "react-router-dom";
import Account from "../Account/Account";

function Header() {
  const { pathname } = useLocation();

  const hasNotifications = true;
  const isHomePage = pathname === "/";
  const isCatalog = pathname === "/car-catalog";
  const isAdmin = pathname === "/admin";
  const isPayment = pathname === "/payment/:id";

  return (
    <header className="header">
      <div className="header-container">
        <div className="logo-header">
          <Link to="/" className="logo-header">
            {" "}
            <h1>CarShare</h1>
          </Link>
        </div>
        {isHomePage && (
          <nav className="navigation">
            {/* <a href="#home" className="nav-link active">
              Главная
            </a>
            <Link to="/car-catalog" className="nav-link">
              Машины
            </Link>
            <a href="#about" className="nav-link">
              Про нас
            </a> */}
          </nav>
        )}
        {isCatalog && (
          <nav className="navigation">
            {/* <a href="#home" className="nav-link">
              Главная
            </a>
            <Link to="/car-catalog" className="nav-link active">
              Машины
            </Link>
            <a href="#about" className="nav-link">
              Про нас
            </a> */}
          </nav>
        )}
        {isPayment && (
          <nav className="navigation">
            {/* <a href="#home" className="nav-link">
              Главная
            </a>
            <Link to="/car-catalog" className="nav-link">
              Машины
            </Link>
            <a href="#about" className="nav-link">
              Про нас
            </a> */}
          </nav>
        )}
        {/* {isCatalog && (
          <div className="search-container">
            <div className="search-box">
              <input
                type="text"
                placeholder="Найти машину на любой вкус"
                className="search-input"
              />
              <button className="search-button">
                <img src={Search} alt="Искать" width="20" height="20" />
              </button>
            </div>
          </div>
        )} */}
        {isAdmin && <div className=""></div>}
        <div className="options">
          {/* <button className="like-button">
            <img src={Like} alt="Избранное" width="35" height="35" />
          </button>
          <button className="notific-button">
            <img src={Notification} alt="Уведомления" width="35" height="35" />
            {hasNotifications && <span className="notification-dot"></span>}
          </button> */}
          <Account />
        </div>
      </div>
    </header>
  );
}

export default Header;

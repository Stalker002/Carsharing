import './Header.css';
import Like from '../../svg/Header/like.svg'
import Search from '../../svg/Header/search.svg'
import Notification from '../../svg/Header/notification.svg'

import { Routes, Route, Link, useLocation } from 'react-router-dom';
import { useState } from 'react';

function Header({onLoginClick}) {
    const hasNotifications = true;
    const isHomePage = location.pathname === "/";
    const isCatalog = location.pathname === "/car-catalog";
    const isBooking = location.pathname === "/booking";

    return (
        <header className="header">
            <div className="header-container">
                <div className="logo-header">
                    <Link to='/' className="logo-header"> <h1>CarShare</h1></Link>
                </div>
                {isHomePage && (
                    <><nav className="navigation">
                        <a href="#home" className="nav-link active">Главная</a>
                        <a href="#vehicles" className="nav-link">Машины</a>
                        <a href="#details" className="nav-link">Детали</a>
                        <a href="#about" className="nav-link">Про нас</a>
                    </nav>
                    </>
                )}

                {isCatalog && (
                    <>
                        <div className="search-container">
                            <div className="search-box">
                                <input type="text" placeholder="Найти машину на любой вкус" className="search-input" />
                                <button className="search-button">
                                    <img src={Search} alt="Искать" width="20" height="20" />
                                </button>
                            </div>
                        </div>
                    </>
                )}

                {/* {isBooking && (
                        <>
                            <Link to="/">Главная</Link>
                            <Link to="/booking">Бронирование</Link>
                        </>
                    )} */}

                <div className='options'>
                    <button className="like-button">
                        <img src={Like} alt="Избранное" width="35" height="35" />
                    </button>
                    <button className="notific-button">
                        <img src={Notification} alt="Уведомления" width="35" height="35" />
                        {hasNotifications && <span className="notification-dot"></span>}
                    </button>
                    <button className="user-button" onClick={onLoginClick}>
                        <div className="user-avatar">
                            <span>U</span>
                        </div>
                    </button>
                </div>
            </div>
        </header>
    );
}

export default Header;
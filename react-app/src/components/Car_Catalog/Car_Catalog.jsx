import { useEffect, useCallback, useMemo, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { Link, useNavigate } from "react-router-dom";
import Exit from "./../../svg/Profile/whiteExit.png";
import Register from "../Register/Register";
import Login from "../Login/Login";

import { getCarsByCategory } from "../../redux/actions/cars";

import Like from "../../svg/Popular_Car/like.svg";
import Liked from "../../svg/Popular_Car/liked.svg";
import Fuel from "../../svg/Popular_Car/fuel.svg";
import Transmission from "../../svg/Popular_Car/transmission.svg";
import People from "../../svg/Popular_Car/people.svg";
import "./Car_Catalog.css";

function CarCatalog({ filters }) {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const isLoggedIn = useSelector((state) => state.users.isLoggedIn);

  const cars = useSelector((state) => state.cars?.carsByCategory || []);
  const isLoading = useSelector((state) => state.cars?.isCarsLoading);

  useEffect(() => {
    if (cars.length === 0) {
      dispatch(getCarsByCategory());
    }
  }, [dispatch, cars.length]);

  const [isLoginOpen, setIsLoginOpen] = useState(false);
  const [isRegisterOpen, setIsRegisterOpen] = useState(false);

  const openLogin = () => {
    setIsRegisterOpen(false);
    setIsLoginOpen(true);
  };
  const openRegister = () => {
    setIsLoginOpen(false);
    setIsRegisterOpen(true);
  };
  const closeAll = () => {
    setIsLoginOpen(false);
    setIsRegisterOpen(false);
  };

  const getCapacity = (car) => {
    if (car.capacity) return String(car.capacity);
    if (["Спорткар", "Грузовой"].includes(car.categoryName)) return "2";
    if (["Минивэн", "Внедорожник"].includes(car.categoryName)) return "6";
    return "4";
  };

  const filteredCars = useMemo(() => {
    return cars.filter((car) => {
      if (
        filters.types.length > 0 &&
        !filters.types.includes(car.categoryName)
      ) {
        return false;
      }

      const seats = getCapacity(car);
      const hasLargeCapacity =
        filters.capacities.includes("8") && Number(seats) >= 8;
      const hasExactCapacity = filters.capacities.includes(seats);
      if (
        filters.capacities.length > 0 &&
        !hasExactCapacity &&
        !hasLargeCapacity
      ) {
        return false;
      }

      if (car.pricePerDay > filters.maxPrice) {
        return false;
      }

      if (car.statusName == "Недоступна" || car.statusName == "На обслуживании" || car.statusName == "В ремонте") {
        return false;
      }

      return true;
    });
  }, [cars, filters]);

  const toggleFavorite = useCallback((id) => {
    console.log("Toggle favorite:", id);
    // dispatch(addToFavorites(id))
  }, []);

  const renderCard = (filteredCars) => {
    const imageUrl = filteredCars.imagePath
      ? `http://localhost:5078${filteredCars.imagePath}`
      : "https://via.placeholder.com/300x200?text=No+Image";

    const seatsCount = getCapacity(filteredCars);

    return (
      <div key={filteredCars.id} className="card-card" onClick={() => navigate(`/car-catalog/${filteredCars.id}`)}>
        <div className="card-header">
          <div>
            <h3>
              {filteredCars.brand || ""}{" "}
              {filteredCars.model || `Авто #${filteredCars.id}`}
            </h3>
            <p>{filteredCars.categoryName || "Стандарт"}</p>
          </div>
          {/* <img
            src={filteredCars.isFavorite ? Liked : Like}
            alt="like"
            className={`heart ${filteredCars.isFavorite ? "active" : ""}`}
            onClick={() => toggleFavorite(filteredCars.id)}
          /> */}
        </div>

        <div className="card-image">
          <img src={imageUrl} alt={filteredCars.model} />
        </div>

        <div className="card-info">
          <div className="card-icon">
            <img src={Fuel} alt="Топливо" />
            {filteredCars.maxFuel ? `${filteredCars.maxFuel}Л` : "60Л"}
          </div>
          <div className="card-icon">
            <img src={Transmission} alt="Коробка" />
            {filteredCars.transmission || "Автомат"}
          </div>
          <div className="card-icon">
            <img src={People} alt="Мест" />
            {seatsCount} места
          </div>
        </div>

        <div className="card-footer">
          <div className="price">
            <h4>
              {filteredCars.pricePerDay
                ? filteredCars.pricePerDay.toFixed(2)
                : "0.00"}{" "}
              BYN
              <span>/день</span>
            </h4>
            {filteredCars.oldPrice && (
              <p className="old-price">{filteredCars.oldPrice} BYN</p>
            )}
          </div>
          <Link to={`/car-catalog/${filteredCars.id}`}>
            <button className="rent-btn">Арендовать</button>
          </Link>
        </div>
      </div>
    );
  };

  if (!isLoggedIn) {
    return (
      <div className="catalog-error-container">
        <div className="error-content">
          <h2>🔒 Доступ ограничен</h2>
          <p>
            Вы не авторизованы. Пожалуйста, войдите в систему, чтобы
            просматривать каталог автомобилей.
          </p>
          <button className="avtoriz-error-btn" onClick={openLogin}>
            Авторизоваться или зарегистрироваться
          </button>
          <Login
            isOpen={isLoginOpen}
            onClose={closeAll}
            onRegisterClick={openRegister}
          />
          <Register
            isOpen={isRegisterOpen}
            onClose={closeAll}
            onLoginClick={openLogin}
          />
        </div>
      </div>
    );
  }

  if (isLoading) {
    return (
      <div style={{ padding: "50px", textAlign: "center" }}>
        Загрузка автомобилей...
      </div>
    );
  }
  const isFiltering =
    filters.types.length > 0 ||
    filters.capacities.length > 0 ||
    filters.maxPrice < 500;
  return (
    <section className="catalog-page">
      {isFiltering ? (
        <div>
          <div className="popular-header">
            <h2>Результаты поиска ({filteredCars.length})</h2>
          </div>

          {filteredCars.length > 0 ? (
            <div className="catalog-grid">{filteredCars.map(renderCard)}</div>
          ) : (
            <div
              style={{ padding: "40px", textAlign: "center", color: "#888" }}
            >
              По вашему запросу ничего не найдено.
            </div>
          )}
        </div>
      ) : (
        <>
          <div>
            <div className="popular-header">
              <h2>Популярные автомобили</h2>
            </div>
            <div className="catalog-grid">
              {cars.slice(0, 4).map(renderCard)}
            </div>
          </div>
          <div>
            <div className="popular-header">
              <h2>Рекомендуемые автомобили</h2>
            </div>
            <div className="catalog-grid">{cars.slice(4).map(renderCard)}</div>
          </div>
        </>
      )}

      {cars.length === 0 && !isLoading && (
        <p style={{ textAlign: "center", fontSize: "18px" }}>
          Нет доступных автомобилей.
        </p>
      )}
    </section>
  );
}

export default CarCatalog;

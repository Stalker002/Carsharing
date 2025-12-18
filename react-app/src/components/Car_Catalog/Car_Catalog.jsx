import { useEffect, useMemo, useState, memo } from "react";
import { useDispatch, useSelector } from "react-redux";
import { Link, useNavigate } from "react-router-dom";
import Register from "../Register/Register";
import Login from "../Login/Login";

import { getCarsByCategory } from "../../redux/actions/cars";

import Fuel from "../../svg/Popular_Car/fuel.svg";
import Transmission from "../../svg/Popular_Car/transmission.svg";
import People from "../../svg/Popular_Car/people.svg";
import "./Car_Catalog.css";

const getCapacity = (car) => {
  if (car.capacity) return String(car.capacity);
  if (["Спорткар", "Грузовой"].includes(car.categoryName)) return "2";
  if (["Минивэн", "Внедорожник"].includes(car.categoryName)) return "6";
  return "4";
};

const CarCard = memo(({ car }) => {
  const navigate = useNavigate();
  
  const imageUrl = car.imagePath
    ? `http://localhost:5078${car.imagePath}`
    : "https://via.placeholder.com/300x200?text=No+Image";

  const seatsCount = getCapacity(car);
  
  const isAvailable = car.statusName === "Свободна" || car.statusName === "Доступна";
  const cardClass = isAvailable ? "card-card" : "card-card card-disabled";

  const handleClick = () => {
    navigate(`/car-catalog/${car.id}`);
  };

  return (
    <div className={cardClass} onClick={handleClick}>
      <div className="card-header">
        <div>
          <h3>
            {car.brand || ""} {car.model || `Авто #${car.id}`}
          </h3>
          <p>{car.categoryName || "Стандарт"}</p>
        </div>
      </div>

      <div className="card-image">
        {!isAvailable && (
          <div className="status-badge">
            {car.statusName || "Недоступна"}
          </div>
        )}
        <img src={imageUrl} alt={car.model} loading="lazy" />
      </div>

      <div className="card-info">
        <div className="card-icon">
          <img src={Fuel} alt="Топливо" />
          {car.maxFuel ? `${car.maxFuel}Л` : "60Л"}
        </div>
        <div className="card-icon">
          <img src={Transmission} alt="Коробка" />
          {car.transmission || "Автомат"}
        </div>
        <div className="card-icon">
          <img src={People} alt="Мест" />
          {seatsCount} места
        </div>
      </div>

      <div className="card-footer">
        <div className="price">
          <h4>
            {car.pricePerDay ? car.pricePerDay.toFixed(2) : "0.00"} BYN
            <span>/день</span>
          </h4>
          {car.oldPrice && (
            <p className="old-price">{car.oldPrice} BYN</p>
          )}
        </div>
        
        {isAvailable ? (
          <Link to={`/car-catalog/${car.id}`} onClick={(e) => e.stopPropagation()}>
            <button className="rent-btn">Арендовать</button>
          </Link>
        ) : (
          <button className="rent-btn disabled-btn" disabled>
            Занято
          </button>
        )}
      </div>
    </div>
  );
});

function CarCatalog({ filters }) {
  const dispatch = useDispatch();
  
  const isLoggedIn = useSelector((state) => state.users.isLoggedIn);
  const cars = useSelector((state) => state.cars?.carsByCategory || []);
  const isLoading = useSelector((state) => state.cars?.isCarsLoading);

  const [isLoginOpen, setIsLoginOpen] = useState(false);
  const [isRegisterOpen, setIsRegisterOpen] = useState(false);

  useEffect(() => {
    if (cars.length === 0) {
      dispatch(getCarsByCategory());
    }
  }, [dispatch, cars.length]);

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

  const filteredCars = useMemo(() => {
    return cars.filter((car) => {
      if (filters.types.length > 0 && !filters.types.includes(car.categoryName)) {
        return false;
      }

      const seats = getCapacity(car);
      const hasLargeCapacity = filters.capacities.includes("8") && Number(seats) >= 8;
      const hasExactCapacity = filters.capacities.includes(seats);
      
      if (filters.capacities.length > 0 && !hasExactCapacity && !hasLargeCapacity) {
        return false;
      }

      if (car.pricePerDay > filters.maxPrice) {
        return false;
      }

      return true;
    });
  }, [cars, filters]);

  if (!isLoggedIn) {
    return (
      <div className="catalog-error-container">
        <div className="error-content">
          <h2>Доступ ограничен</h2>
          <p>
            Вы не авторизованы. Пожалуйста, войдите в систему, чтобы
            просматривать каталог автомобилей.
          </p>
          <button className="avtoriz-error-btn" onClick={openLogin}>
            Авторизоваться или зарегистрироваться
          </button>
          <Login isOpen={isLoginOpen} onClose={closeAll} onRegisterClick={openRegister} />
          <Register isOpen={isRegisterOpen} onClose={closeAll} onLoginClick={openLogin} />
        </div>
      </div>
    );
  }

  if (isLoading) {
    return <div className="loading-state">Загрузка автомобилей...</div>;
  }

  const isFiltering =
    filters.types.length > 0 ||
    filters.capacities.length > 0 ||
    filters.maxPrice < 300;

  return (
    <section className="catalog-page">
      {isFiltering ? (
        <div>
          <div className="popular-header">
            <h2>Результаты поиска ({filteredCars.length})</h2>
          </div>

          {filteredCars.length > 0 ? (
            <div className="catalog-grid">
              {filteredCars.map((car) => (
                <CarCard key={car.id} car={car} />
              ))}
            </div>
          ) : (
            <div className="no-results">
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
              {cars.slice(0, 4).map((car) => (
                <CarCard key={car.id} car={car} />
              ))}
            </div>
          </div>
          <div>
            <div className="popular-header">
              <h2>Рекомендуемые автомобили</h2>
            </div>
            <div className="catalog-grid">
              {cars.slice(4).map((car) => (
                <CarCard key={car.id} car={car} />
              ))}
            </div>
          </div>
        </>
      )}

      {cars.length === 0 && !isLoading && (
        <p className="empty-catalog">
          Нет доступных автомобилей.
        </p>
      )}
    </section>
  );
}

export default CarCatalog;
import "./PopularCar.css";
import Like from "../../svg/Popular_Car/like.svg";
import Liked from "../../svg/Popular_Car/liked.svg";
import Fuel from "../../svg/Popular_Car/fuel.svg";
import Transmission from "../../svg/Popular_Car/transmission.svg";
import People from "../../svg/Popular_Car/people.svg";
import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { getCarsByCategory } from "../../redux/actions/cars";
import { useDispatch, useSelector } from "react-redux";

function PopularCar() {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const cars = useSelector((state) => state.cars?.carsByCategory || []);
  const isLoading = useSelector((state) => state.cars?.isCarsLoading);

  const getCapacity = (car) => {
    if (car.capacity) return String(car.capacity);
    if (["Спорткар", "Грузовой"].includes(car.categoryName)) return "2";
    if (["Минивэн", "Внедорожник"].includes(car.categoryName)) return "6";
    return "4";
  };

  useEffect(() => {
    if (cars.length === 0) {
      dispatch(getCarsByCategory());
    }
  }, [dispatch, cars.length]);

  // const toggleFavorite = (id) => {
  //   setCars((prevCars) =>
  //     prevCars.map((car) =>
  //       car.id === id ? { ...car, favorite: !car.favorite } : car
  //     )
  //   );
  // };
  const renderCard = (cars) => {
    const imageUrl = cars.imagePath
      ? `http://localhost:5078${cars.imagePath}`
      : "https://via.placeholder.com/300x200?text=No+Image";

    const seatsCount = getCapacity(cars);

    return (
      <div key={cars.id} className="car-card" onClick={() => navigate(`/car-catalog/${cars.id}`)}>
        <div className="car-header">
          <div>
            <h3>
              {cars.brand || ""} {cars.model || `Авто #${cars.id}`}
            </h3>
            <p>{cars.categoryName || "Стандарт"}</p>
          </div>
          {/* <img
              src={cars.isFavorite ? Liked : Like}
              alt="like"
              className={`heart ${cars.isFavorite ? "active" : ""}`}
              onClick={() => toggleFavorite(cars.id)}
            /> */}
        </div>

        <div className="car-image">
          <img src={imageUrl} alt={cars.model} />
        </div>

        <div className="car-info">
          <div className="car-icon">
            <img src={Fuel} alt="Топливо" />
            {cars.maxFuel ? `${cars.maxFuel}Л` : "60Л"}
          </div>
          <div className="car-icon">
            <img src={Transmission} alt="Коробка" />
            {cars.transmission || "Автомат"}
          </div>
          <div className="car-icon">
            <img src={People} alt="Мест" />
            {seatsCount} места
          </div>
        </div>

        <div className="car-footer">
          <div className="price">
            <h4>
              {cars.pricePerDay ? cars.pricePerDay.toFixed(2) : "0.00"} BYN
              <span>/день</span>
            </h4>
            {cars.oldPrice && <p className="old-price">{cars.oldPrice} BYN</p>}
          </div>
          <Link to={`/car-catalog/${cars.id}`}>
            <button className="rent-btn">Арендовать</button>
          </Link>
        </div>
      </div>
    );
  };

  return (
    <section className="popular-cars">
      <div className="popular-header-main">
        <h2>Популярные автомобили</h2>
        <p>Избранное водителями</p>
      </div>

      <div className="popular-cars-grid">{cars.slice(0, 4).map(renderCard)}</div>
    </section>
  );
}

export default PopularCar;

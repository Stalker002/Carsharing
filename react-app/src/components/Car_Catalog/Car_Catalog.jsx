import "./Car_Catalog.css";
import { useCallback, useState } from 'react'
import Like from "../../svg/Popular_Car/like.svg"
import Liked from "../../svg/Popular_Car/liked.svg"
import Fuel from "../../svg/Popular_Car/fuel.svg"
import Transmission from "../../svg/Popular_Car/transmission.svg"
import People from "../../svg/Popular_Car/people.svg"
import car1 from "../../svg/Popular_Car/BMW_i8.png";
import car2 from "../../svg/Popular_Car/Voyah_Free.png";
import car3 from "../../svg/Popular_Car/BMW_X7.png";
import car4 from "../../svg/Popular_Car/Tesla_S.png";
import { Link } from "react-router-dom";

function CarCatalog() {

  const toggleFavorite = useCallback((id) => {
    setCars(prev => prev.map(car =>
      car.id === id ? { ...car, favorite: !car.favorite } : car
    ));
  }, []);

  const [cars, setCars] = useState([
    {
      id: 1,
      name: "BMW i8",
      type: "Спорт",
      fuel: "90Л",
      transmission: "Автомат",
      capacity: "2 места",
      price: 99,
      oldPrice: null,
      image: car1,
      favorite: false,
    },
    {
      id: 2,
      name: "Voyah Free",
      type: "Премиум",
      fuel: "40Л",
      transmission: "Автомат",
      capacity: "4 места",
      price: 80,
      oldPrice: 100,
      image: car2,
      favorite: true,
    },
    {
      id: 3,
      name: "BMW X7 6 мест",
      type: "Внедорожник",
      fuel: "70Л",
      transmission: "Автомат",
      capacity: "6 мест",
      price: 96,
      oldPrice: null,
      image: car3,
      favorite: true,
    },
    {
      id: 4,
      name: "Tesla Model S Performance Ludicrous",
      type: "Электро",
      fuel: null,
      transmission: "Автомат",
      capacity: "4 места",
      price: 80,
      oldPrice: 100,
      image: car4,
      favorite: false,
    },
  ]);

  const sections = [
    { title: "Популярные автомобили", data: cars.slice(0) },
    { title: "Рекомендуемые автомобили", data: cars.slice(0) },
  ];

  const renderCard = (car) => (
    <div key={car.id} className="card-card">
      <div className="card-header">
        <div>
          <h3>{car.name}</h3>
          <p>{car.type}</p>
        </div>
        <img
          src={car.favorite ? Liked : Like}
          alt="like"
          className={`heart ${car.favorite ? "active" : ""}`}
          onClick={() => toggleFavorite(car.id)}
        />
      </div>

      <div className="card-image">
        <img src={car.image} alt={car.name} />
      </div>

      <div className="card-info">
        {car.fuel && (
          <div className="card-icon">
            <img src={Fuel} alt="Топливо" /> {car.fuel}
          </div>
        )}
        <div className="card-icon">
          <img src={Transmission} alt="Коробка передач" /> {car.transmission}
        </div>
        <div className="card-icon">
          <img src={People} alt="Мест" /> {car.capacity}
        </div>
      </div>

      <div className="card-footer">
        <div className="price">
          <h4>
            {car.price.toFixed(1)} BYN/<span>день</span>
          </h4>
          {car.oldPrice && (
            <p className="old-price">{car.oldPrice.toFixed(1)} BYN</p>
          )}
        </div>
        <Link to={`/car-catalog/${car.id}`}>
          <button className="rent-btn">Арендовать</button>
        </Link>
      </div>
    </div>
  );

  return (
    <section className="catalog-page">
      {sections.map(({ title, data }) => (
        <div key={title}>
          <div className="popular-header">
            <h2>{title}</h2>
          </div>
          <div className="catalog-grid">{data.map(renderCard)}</div>
        </div>
      ))}
    </section>
  );
}
export default CarCatalog;
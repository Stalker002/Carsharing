import "./PopularCar.css";
import Like from "../../svg/Popular_Car/like.svg"
import Liked from "../../svg/Popular_Car/liked.svg"
import Fuel from "../../svg/Popular_Car/fuel.svg"
import Transmission from "../../svg/Popular_Car/transmission.svg"
import People from "../../svg/Popular_Car/people.svg"
import car1 from "../../svg/Popular_Car/BMW_i8.png";
import car2 from "../../svg/Popular_Car/Voyah_Free.png";
import car3 from "../../svg/Popular_Car/BMW_X7.png";
import car4 from "../../svg/Popular_Car/Tesla_S.png";
import { useState } from "react";
import { Link } from "react-router-dom";

function PopularCar() {
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

  const toggleFavorite = (id) => {
    setCars((prevCars) =>
      prevCars.map((car) =>
        car.id === id ? { ...car, favorite: !car.favorite } : car
      )
    );
  };

  return (
    <section className="popular-cars">
      <div className="popular-header-main">
        <h2>Популярные автомобили</h2>
        <p>Избранное водителями</p>
      </div>

      <div className="cars-grid">
        {cars.map((car) => (

          <div key={car.id} className="car-card">
            <div className="car-header">
              <div>
                <h3>{car.name}</h3>
                <p>{car.type}</p>
              </div>
              <img
                src={car.favorite ? Liked : Like}
                alt="like"
                className={"heart"}
                onClick={() => toggleFavorite(car.id)}
              />
            </div>

            <div className="car-image">
              <img src={car.image} alt={car.name} />
            </div>

            <div className="car-info">
              {car.fuel && (
                <div className="car-icon"><img src={Fuel} /> {car.fuel} </div>
              )}
              <div className="car-icon"><img src={Transmission} />{car.transmission}</div>
              <div className="car-icon"><img src={People} />{car.capacity}</div>
            </div>

            <div className="car-footer">
              <div className="price">
                <h4>{car.price.toFixed(1)}BYN/<span>день</span></h4>
                {car.oldPrice && (
                  <p className="old-price">{car.oldPrice.toFixed(1)}BYN</p>
                )}
              </div>
              <Link to={`/car-catalog/${car.id}`}>
                <button className="rent-btn">Арендовать</button>
              </Link>
            </div>
          </div>
        ))}
      </div>
    </section>
  );
}

export default PopularCar;
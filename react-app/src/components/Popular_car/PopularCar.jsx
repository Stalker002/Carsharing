import "./PopularCar.css";
import Like from "../../svg/Popular_Car/like.svg"
import Liked from "../../svg/Popular_Car/liked.svg"
import Fuel from "../../svg/Popular_Car/fuel.svg"
import Transmission from "../../svg/Popular_Car/transmission.svg"
import People from "../../svg/Popular_Car/people.svg"
import car1 from "../../svg/Popular_Car/prevyu_BMW_i8.jpg";
import car2 from "../../svg/Popular_Car/prevyu_Voyah_Free.jpg";
import car3 from "../../svg/Popular_Car/prevyu_BMW_X7.jpg";
import car4 from "../../svg/Popular_Car/prevyu_Tesla_S.jpg";

function PopularCar() {
  const cars = [
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
  ];
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
              {car.favorite ? (
                <img src={Liked} alt="liked" className="heart" />
              ) : (
                <img src={Like} alt="like" className="heart" />
              )}
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
                  // .toFixed(2)
                )}
              </div>
              <button className="rent-btn">Арендовать</button>
            </div>
          </div>
        ))}
      </div>
    </section>
  );
}

export default PopularCar;
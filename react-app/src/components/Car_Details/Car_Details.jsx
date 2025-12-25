import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import { getInfoCars, getCarsByCategory } from "../../redux/actions/cars";
import Like from "../../svg/Popular_Car/like.svg";

import "./Car_Details.css";
import Fuel from "../../svg/Popular_Car/fuel.svg";
import Transmission from "../../svg/Popular_Car/transmission.svg";
import People from "../../svg/Popular_Car/people.svg";
import { getReviewsByCar } from "../../redux/actions/reviews";
import { openModal } from "../../redux/actions/modal";
import InfiniteScroll from "react-infinite-scroll-component";

const CarDetails = () => {
  const { id } = useParams();
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const car = useSelector((state) => state.cars.infoCar?.[0]);
  const isLoading = useSelector((state) => state.cars.isCarsInfoLoading);
  const isLoggedIn = useSelector((state) => state.users.isLoggedIn);

  const cars = useSelector((state) => state.cars?.carsByCategory || []);
  const isCarLoading = useSelector((state) => state.cars?.isCarsLoading);

  const { reviewsByCar: reviews, reviewsByCarTotal, isReviewsLoading } = useSelector((state) => state.reviews);
  
  const [page, setPage] = useState(1);

  useEffect(() => {
    setPage(1);
    dispatch(getReviewsByCar(id, 1));
  }, [dispatch, id]);

  const fetchMoreReviews = () => {
    const nextPage = page + 1;
    setPage(nextPage);
    dispatch(getReviewsByCar(id, nextPage));
  };

  const hasMore = reviews.length < reviewsByCarTotal;

  const getCapacity = (car) => {
    if (car.capacity) return String(car.capacity);
    if (["Спорткар", "Грузовой"].includes(car.categoryName)) return "2";
    if (["Минивэн", "Внедорожник"].includes(car.categoryName)) return "6";
    return "4";
  };

  const handleBookingClick = () => {
    if (!isLoggedIn) {
      dispatch(
        openModal({
          type: "error",
          title: "Внимание",
          message: "Пожалуйста, войдите в систему, чтобы арендовать авто.",
        })
      );
      return;
    }

    if (car.statusId !== 1 && car.statusName !== "Доступна") {
      return dispatch(
        openModal({
          type: "error",
          title: "Внимание",
          message: "Эта машина сейчас недоступна",
        })
      );
    }

    navigate(`/booking/${id}`);
  };

  useEffect(() => {
    window.scrollTo(0, 0);
    dispatch(getInfoCars(id));
    dispatch(getReviewsByCar(id));

    if (cars.length === 0) {
      dispatch(getCarsByCategory());
    }
  }, [dispatch, cars.length, id]);

  if (isLoading || !car) {
    return <div className="details-loading">Загрузка автомобиля...</div>;
  }

  const formatDate = (dateString) => {
    if (!dateString) return "";
    return new Date(dateString).toLocaleDateString("ru-RU", {
      day: "numeric",
      month: "long",
      year: "numeric",
    });
  };

  const renderCard = (cars) => {
    const imageUrl = cars.imagePath
      ? `http://localhost:5078${cars.imagePath}`
      : "https://via.placeholder.com/300x200?text=No+Image";

    const seatsCount = getCapacity(cars);

    return (
      <div
        key={cars.id}
        className="card-card"
        onClick={() => navigate(`/car-catalog/${cars.id}`)}
      >
        <div className="card-header">
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

        <div className="card-image">
          <img src={imageUrl} alt={cars.model} />
        </div>

        <div className="card-info">
          <div className="card-icon">
            <img src={Fuel} alt="Топливо" />
            {cars.maxFuel ? `${cars.maxFuel}Л` : "60Л"}
          </div>
          <div className="card-icon">
            <img src={Transmission} alt="Коробка" />
            {cars.transmission || "Автомат"}
          </div>
          <div className="card-icon">
            <img src={People} alt="Мест" />
            {seatsCount} места
          </div>
        </div>

        <div className="card-footer">
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

  const imageUrl = car.imagePath
    ? `http://localhost:5078${car.imagePath}`
    : "https://via.placeholder.com/600x400";

  return (
    <div className="details-page">
      <div className="details-content">
        <div className="navigation-header">
          <button className="back-btn" onClick={() => navigate(-1)}>
            <svg 
              width="24" 
              height="24" 
              viewBox="0 0 24 24" 
              fill="none" 
              xmlns="http://www.w3.org/2000/svg"
            >
              <path 
                d="M15 19.9201L8.47997 13.4001C7.70997 12.6301 7.70997 11.3701 8.47997 10.6001L15 4.08008" 
                stroke="currentColor" 
                strokeWidth="2" 
                strokeMiterlimit="10" 
                strokeLinecap="round" 
                strokeLinejoin="round"
              />
            </svg>
            Назад
          </button>
        </div>
        <div className="top-section">
          <div className="gallery-container">
            <div className="hero-banner">
              <div className="hero-text-desc">
                <h1>
                  {car.brand} {car.model}
                </h1>
                <p>
                  Автомобиль класса <strong>{car.categoryName}</strong>,
                  расположенный в городе <strong>{car.location}</strong>
                </p>
              </div>
              <img src={imageUrl} alt={car.model} className="hero-car-img" />
            </div>
          </div>

          <div className="car-info-card">
            <div className="info-header">
              <div>
                <h2>
                  {car.brand} {car.model}
                </h2>
                <span
                  className={`status ${
                    car.statusName === "Доступна" ? "active" : "inactive"
                  }`}
                >
                  {car.statusName}
                </span>
              </div>
              {/* <img src={Like} className="main-heart" alt="like" /> */}
            </div>

            <div className="specs-grid">
              <div className="spec-row">
                <span className="spec-label">Тип автомобиля</span>
                <span className="spec-value">{car.categoryName}</span>
              </div>

              <div className="spec-row">
                <span className="spec-label">Топливо</span>
                <span className="spec-value">{car.fuelType}</span>
              </div>

              <div className="spec-row">
                <span className="spec-label">Коробка передач</span>
                <span className="spec-value">{car.transmission}</span>
              </div>

              <div className="spec-row">
                <span className="spec-label">Год выпуска</span>
                <span className="spec-value">{car.year}</span>
              </div>

              <div className="spec-row">
                <span className="spec-label">Уровень топлива</span>
                <span className="spec-value">{car.fuelLevel}%</span>
              </div>

              <div className="spec-row">
                <span className="spec-label">Номер</span>
                <span className="spec-value">{car.stateNumber}</span>
              </div>
            </div>

            <div className="info-footer">
              <div>
                <span className="price-val">
                  {car.pricePerDay.toFixed(2)} BYN
                </span>
                <span className="price-unit"> / день</span>
                <div className="old-price">
                  {car.pricePerMinute} BYN / мин · {car.pricePerKm} BYN / км
                </div>
              </div>

              <button
                className="rent-now-btn"
                disabled={car.statusName !== "Доступна"}
                onClick={handleBookingClick}
              >
                Арендовать
              </button>
            </div>
          </div>
        </div>
        <div className="reviews-container">
          <div className="reviews-header">
            <h3>
              Отзывов <span className="badge">{reviewsByCarTotal}</span>
            </h3>
          </div>

          <InfiniteScroll
            dataLength={reviews.length}
            next={fetchMoreReviews}
            hasMore={hasMore}
            loader={<p style={{textAlign: 'center', color: '#90A3BF', marginTop: '20px'}}>Загрузка еще...</p>}
          >
            {reviews.length === 0 && !isReviewsLoading ? (
               <p style={{ color: "#90A3BF" }}>Нет отзывов на эту машину</p>
            ) : (
               reviews.map((review) => (
                <div key={review.id} className="review-item">
                  <div className="user-avatar">
                    <span>
                      {review.name?.[0]}
                      {review.surname?.[0]}
                    </span>
                  </div>
                  <div className="review-content">
                    <div className="review-top">
                      <div>
                        <h4>
                          {review.name} {review.surname}
                        </h4>
                        <span className="role">{review.role || "Client"}</span>
                      </div>
                      <div className="review-meta">
                        <span className="date">{formatDate(review.date)}</span>
                        <div className="stars">
                          {[...Array(5)].map((_, i) => (
                            <span
                              key={i}
                              style={{
                                color: i < review.rating ? "#FBAD39" : "#e0e0e0",
                                fontSize: "18px",
                              }}
                            >
                              ★
                            </span>
                          ))}
                        </div>
                      </div>
                    </div>
                    <p className="review-text">{review.comment}</p>
                  </div>
                </div>
              ))
            )}
          </InfiniteScroll>         
        </div>
        <div className="popular-header-detail">
          <Link to={`/car-catalog/`}>
            <p className="popular-header-detail">Популярные автомобили</p>
          </Link>
        </div>
        <div className="cars-grid-detail">
          {cars.slice(0, 4).map(renderCard)}
        </div>
      </div>
    </div>
  );
};

export default CarDetails;

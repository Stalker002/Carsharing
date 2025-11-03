import "./Car_Details.css";
import { useParams, useNavigate } from "react-router-dom";
import Like from "../../svg/Popular_Car/like.svg";
import Liked from "../../svg/Popular_Car/liked.svg";
import car1 from "../../svg/Popular_Car/BMW_i8.png";
import car2 from "../../svg/Popular_Car/Voyah_Free.png";
import car3 from "../../svg/Popular_Car/BMW_X7.png";
import car4 from "../../svg/Popular_Car/Tesla_S.png";

import { useState } from "react";
import Transmission from "../../svg/Popular_Car/transmission.svg";
import People from "../../svg/Popular_Car/people.svg";
import Fuel from "../../svg/Popular_Car/fuel.svg";

export default function Car_Details() {
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
            description: "BMW i8 — гибридный спорткар с потрясающим дизайном и динамикой.",
            rating: 4.7,
            reviews: [
                {
                    id: 1,
                    name: "Алексей Смирнов",
                    position: "CEO at BMW Club",
                    date: "21 июля 2024",
                    text: "Отличная машина! Ездить — одно удовольствие.",
                    rating: 5,
                    avatar: "/avatars/alex.png"
                },
                {
                    id: 2,
                    name: "Ирина Ковалёва",
                    position: "Маркетолог",
                    date: "15 июня 2024",
                    text: "Брала на выходные — комфорт, стиль и внимание на дороге гарантированы.",
                    rating: 4,
                    avatar: "/avatars/skylar.png"
                }
            ]
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
            description: "Voyah Free — премиум SUV с электрической силовой установкой и комфортом бизнес-класса.",
            rating: 4.5,
            reviews: []
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
            description: "Флагманский внедорожник BMW с мощным двигателем и роскошным интерьером.",
            rating: 4.8,
            reviews: []
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
            description: "Мощный электроседан Tesla с ускорением, достойным суперкара.",
            rating: 4.9,
            reviews: []
        }
    ];
    const { id } = useParams();
    const navigate = useNavigate();
    const [favorite, setFavorite] = useState(false);

    const car = cars.find((c) => c.id === Number(id));

    if (!car) {
        return (
            <div className="car-not-found">
                <h2>Машина не найдена 😢</h2>
                <button onClick={() => navigate("/")} className="rent-btn">Назад</button>
            </div>
        );
    }

    return (
        <section className="car-details">
            <div className="car-top">
                {/* Левая часть */}
                <div className="car-gallery">
                    <div className="car-main-image">
                        <div className="car-banner">
                            <h2>{car.name}</h2>
                            <p>{car.description}</p>
                            <img src={car.image} alt={car.name} />
                        </div>
                    </div>
                </div>

                {/* Правая часть */}
                <div className="car-info-panel">
                    <div className="car-info-header">
                        <div>
                            <h2>{car.name}</h2>
                            <p className="rating">
                                <span>★ {car.rating}</span> ({car.reviews.length} отзывов)
                            </p>
                        </div>
                        <img
                            src={favorite ? Liked : Like}
                            alt="like"
                            className="heart"
                            onClick={() => setFavorite(!favorite)}
                        />
                    </div>

                    <p className="car-description">{car.description}</p>

                    <div className="car-specs">
                        <div><span>Тип</span><strong>{car.type}</strong></div>
                        <div><img src={Transmission} /><strong>{car.transmission}</strong></div>
                        <div><img src={People} /><strong>{car.capacity}</strong></div>
                        <div><img src={Fuel} /><strong>{car.fuel || "Электро"}</strong></div>
                    </div>

                    <div className="car-price">
                        <h3>{car.price} BYN/<span>день</span></h3>
                        {car.oldPrice && (
                            <p className="old-price">{car.oldPrice} BYN</p>
                        )}
                    </div>

                    <button className="rent-btn">Арендовать</button>
                </div>
            </div>

            {/* Отзывы */}
            {car.reviews.length > 0 && (
                <div className="car-reviews">
                    <h3>Отзывы <span className="badge">{car.reviews.length}</span></h3>

                    {car.reviews.map((review) => (
                        <div key={review.id} className="review">
                            <img className="avatar" src={review.avatar} alt={review.name} />
                            <div className="review-content">
                                <div className="review-header">
                                    <h4>{review.name}</h4>
                                    <p>{review.position}</p>
                                    <span className="review-date">{review.date}</span>
                                </div>
                                <p className="review-text">{review.text}</p>
                                <div className="review-rating">{"★".repeat(review.rating)}{"☆".repeat(5 - review.rating)}</div>
                            </div>
                        </div>
                    ))}

                    <button className="show-all">Показать все ⌄</button>
                </div>
            )}
        </section>
    );
}

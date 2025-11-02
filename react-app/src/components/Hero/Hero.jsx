import './Hero.css';
import CarImg from "./../../svg/Hero/200.png"
import Avatar1 from './../../svg/Hero/avatar1.jpg'
import Avatar2 from './../../svg/Hero/avatar2.jpeg'
import Avatar3 from './../../svg/Hero/avatar3.jpeg'

import { Link } from 'react-router-dom';

function Hero() {
    return (
        <div className='hero'>
            <div className='hero-text'>
                <h1>Твой опыт,<br />Твоя машина,<br />Твой путь</h1>
                <p>Ощутите абсолютную свободу выбора с Morent - разработайте свое приключение, выбрав один из наших автомобилей премиум-класса.</p>
                <Link to='/car-catalog'><button className='btn-go-now'>Поехали!</button></Link>
            </div>
            <h1 className='hero-logo'>CARSHARE</h1>
            <img className='hero-car' src={CarImg} />
            <div className='car-box'>
                <h2>18+</h2>
                <p>Доступных <br /> типов машин</p>
            </div>
            <div className="people-box">
                <div className="avatars">
                    <img src={Avatar1} alt="User" />
                    <img src={Avatar2} alt="User" />
                    <img src={Avatar3} alt="User" />
                </div>
                <div>
                    <h3>12.5k+ ЛЮДЕЙ</h3>
                    <p>воспользовались нашими услугами и были довольны</p>
                </div>
            </div>
        </div>
    );
}

export default Hero;
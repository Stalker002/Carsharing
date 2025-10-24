import './Features.css';
import Location from '../../svg/Features/Location.svg'
import Comfort from '../../svg/Features/Comfort.svg'
import Savings from '../../svg/Features/Savings.svg'

function Features() {
    return (
        <section className="features">
            <div className='features-header'>
                <h1 className='header-title'>Почему нас</h1>
            </div>
            <div className="features-grid">
                <div className="feature-card">
                    <div className="feature-icon">
                        <img src={Location} width="48" height="48" />
                    </div>
                    <h3 className="feature-title">Доступность</h3>
                    <p className="feature-description">Широкий выбор автомобилей всегда готов к вашим поездкам. Бронируйте в любое время и в любом месте.</p>
                </div>
                <div className="feature-card">
                    <div className="feature-icon">
                        <img src={Comfort} width="48" height="48" />
                    </div>
                    <h3 className="feature-title">Комфорт</h3>
                    <p className="feature-description">Премиальные автомобили с современными удобствами для максимально комфортного путешествия.</p>
                </div>
                <div className="feature-card">
                    <div className="feature-icon">
                        <img src={Savings} width="48" height="48" />
                    </div>
                    <h3 className="feature-title">Экономия</h3>
                    <p className="feature-description">Конкурентные цены и специальные предложения помогут вам сэкономить на аренде без потери качества.</p>
                </div>
            </div>
        </section>
    );
}

export default Features;
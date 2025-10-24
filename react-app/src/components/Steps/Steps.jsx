import './Steps.css';
import Select from './../../svg/Steps/Select.svg'
import Book from './../../svg/Steps/Book.svg'
import Drive from './../../svg/Steps/Drive.svg'
import Return from './../../svg/Steps/Return.svg'

function Steps() {
    return (
        <section className="steps">
            <h1 className='step-logo'>CARSHARE</h1>
            <h2 className="steps-title">Простые шаги для начала аренды</h2>
            <p className="steps-subtitle">Как это делать</p>
            <div className="steps-container">
                <div className="step-item">
                    <div className="step-icon">
                        <img src={Select}/>
                    </div>
                    <div className="step-info left">
                        <h3>Выберите</h3>
                        <p className="step-description">Выберите автомобиль по своему вкусу из нашего автопарка</p>
                    </div>
                    <div className="line-point top" />
                </div>
                <div className="step-item">
                    <div className="step-icon">
                        <img src={Book}/>
                    </div>
                    <div className="step-info right">
                        <h3 className="step-title">Бронируйте</h3>
                        <p className="step-description">Забронируйте свой автомобиль онлайн</p>
                    </div>
                    <div className="line-point middle" />
                </div>
                <div className="step-item">
                    <div className="step-icon">
                        <img src={Drive} />
                    </div>
                    <div className="step-info left">
                        <h3 className="step-title">Езжайте</h3>
                        <p className="step-description">Забирайте свою машину и отправляйтесь в путь</p>
                    </div>
                    <div className="line-point middle" />
                </div>
                <div className="step-item">
                    <div className="step-icon">
                        <img src={Return}/>
                    </div>
                    <div className="step-info right">
                        <h3 className="step-title">Верните</h3>
                        <p className="step-description">Верните автомобиль обратно по истечению срока аренды</p>
                    </div>
                    <div className="line-point bottom" />
                </div>
            </div>
        </section>
    );
}

export default Steps;
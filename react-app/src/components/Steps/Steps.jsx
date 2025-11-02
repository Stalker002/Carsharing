import './Steps.css';
import Select from './../../svg/Steps/Select.svg'
import Book from './../../svg/Steps/Book.svg'
import Drive from './../../svg/Steps/Drive.svg'
import Return from './../../svg/Steps/Return.svg'

function Steps() {
    const steps = [
        {
            icon: Select,
            title: "Выберите",
            description: "Выберите автомобиль по своему вкусу из нашего автопарка",
            align: "left",
            line: "top",
        },
        {
            icon: Book,
            title: "Бронируйте",
            description: "Забронируйте свой автомобиль онлайн",
            align: "right",
            line: "middle",
        },
        {
            icon: Drive,
            title: "Езжайте",
            description: "Забирайте свою машину и отправляйтесь в путь",
            align: "left",
            line: "middle",
        },
        {
            icon: Return,
            title: "Верните",
            description: "Верните автомобиль обратно по истечению срока аренды",
            align: "right",
            line: "bottom",
        },
    ];

    return (
        <section className="steps">
            <h1 className="step-logo">CARSHARE</h1>
            <h2 className="steps-title">Простые шаги для начала аренды</h2>
            <p className="steps-subtitle">Как это делать</p>

            <div className="steps-container">
                {steps.map(({ icon, title, description, align, line }) => (
                    <div className="step-item" key={title}>
                        <div className="step-icon">
                            <img src={icon} alt={title} loading="lazy" />
                        </div>
                        <div className={`step-info ${align}`}>
                            <h3 className="step-title">{title}</h3>
                            <p className="step-description">{description}</p>
                        </div>
                        <div className={`line-point ${line}`} />
                    </div>
                ))}
            </div>
        </section>
    );
}

export default Steps;
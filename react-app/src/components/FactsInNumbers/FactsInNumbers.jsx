import './FactsInNumbers.css';
import Calendar from '../../svg/Facts/Calendar.svg'
import Customers from '../../svg/Facts/Customers.svg'
import Miles from '../../svg/Facts/Miles.svg'
import Cars from '../../svg/Facts/Cars.svg'

const stats = [
    { icon: Cars, value: "540+", label: "Машин" },
    { icon: Customers, value: "20k+", label: "Клиетов" },
    { icon: Calendar, value: "25+", label: "Лет" },
    // { icon: Miles, value: "20m+", label: "Километров" },
];

function FactsInNumbers() {
    return (

        <section className="facts" >
            < h2 className="facts_title" > Факты в цифрах</h2>
            <div className="facts_grid">
                {stats.map((item, index) => (
                    <div className="facts_card" key={index}>
                        <img className="facts_icon" src={item.icon} />
                        <div className='facts_text'>
                            <h3 className="facts_value">{item.value}</h3>
                            <p className="facts_label">{item.label}</p>
                        </div>
                        <div> </div>
                    </div>
                ))}
            </div>
        </section >
    );
}

export default FactsInNumbers;
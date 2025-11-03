import { useState } from "react";
import "./Select_Rent.css"

function Select_Rent() {
    const [pickup, setPickup] = useState("");
    const [dropoff, setDropoff] = useState("");
    const [pickupDate, setPickupDate] = useState("");
    const [pickupTime, setPickupTime] = useState("");
    const [dropoffDate, setDropoffDate] = useState("");
    const [dropoffTime, setDropoffTime] = useState("");
    return (
        <div className="rental-form">
            {[
                {
                    label: "Место аренды",
                    value: pickup,
                    setValue: setPickup,
                    date: pickupDate,
                    setDate: setPickupDate,
                    time: pickupTime,
                    setTime: setPickupTime,
                },
                {
                    label: "Место возврата",
                    value: dropoff,
                    setValue: setDropoff,
                    date: dropoffDate,
                    setDate: setDropoffDate,
                    time: dropoffTime,
                    setTime: setDropoffTime,
                },
            ].map((loc, i) => (
                <div key={i} className="location-section">
                    <h3>{loc.label}</h3>
                    <select
                        className="location-select"
                        value={loc.value}
                        onChange={(e) => loc.setValue(e.target.value)}
                    >
                        <option value="">Выберите место</option>
                        <option value="Минск">Минск</option>
                        <option value="Гродно">Гродно</option>
                        <option value="Брест">Брест</option>
                    </select>

                    <div className="datetime-group">
                        <div className="datetime-field">
                            <label>Дата</label>
                            <input
                                type="date"
                                value={loc.date}
                                onChange={(e) => loc.setDate(e.target.value)}
                            />
                        </div>
                        <div className="datetime-field">
                            <label>Время</label>
                            <input
                                type="time"
                                value={loc.time}
                                onChange={(e) => loc.setTime(e.target.value)}
                            />
                        </div>
                    </div>
                </div>
            ))}
        </div>
    )
}
export default Select_Rent;
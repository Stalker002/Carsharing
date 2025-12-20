import { useEffect } from "react";
import "./TripDetailModal.css";
import { formatCurrency, formatDateTime, formatDuration } from "./utils";

const TripDetailModal = ({ trip, onClose }) => {
  // Блокируем скролл основного окна, когда открыта модалка
  useEffect(() => {
    document.body.style.overflow = "hidden";
    return () => {
      document.body.style.overflow = "unset";
    };
  }, []);

  if (!trip) return null;

  // Формируем URL картинки
  const imageUrl = trip.carImage
    ? `http://localhost:5078${trip.carImage}`
    : "https://via.placeholder.com/300x200?text=Auto";

  // Логика цвета статуса
  const getStatusClass = (status) => {
    if (status === "Оплачено") return "modal-status-success";
    if (status === "Завершено") return "modal-status-warning";
    return "modal-status-default";
  };

  return (
    <div className="trip-modal-overlay" onClick={onClose}>
      <div 
        className="trip-modal-content" 
        onClick={(e) => e.stopPropagation()} // Чтобы клик внутри не закрывал
      >
        
        {/* --- Header --- */}
        <div className="trip-modal-header">
          <div>
            <h3>Поездка #{trip.id}</h3>
            <span className="trip-date-label">
              {new Date(trip.startTime).toLocaleDateString()}
            </span>
          </div>
          <button className="trip-modal-close" onClick={onClose}>
            ✕
          </button>
        </div>

        {/* --- Body --- */}
        <div className="trip-modal-body">
          
          {/* Блок Автомобиля */}
          <div className="trip-car-section">
            <div className="trip-car-img">
              <img src={imageUrl} alt={trip.carModel} />
            </div>
            <div className="trip-car-info">
              <h2>{trip.carBrand} {trip.carModel}</h2>
              <span className={`modal-status-badge ${getStatusClass(trip.statusName)}`}>
                {trip.statusName}
              </span>
            </div>
          </div>

          {/* Сетка деталей */}
          <div className="trip-details-grid">
            
            {/* Ряд 1: Локации */}
            <div className="grid-full-row">
              <div className="location-row">
                <div className="loc-dot start"></div>
                <div className="loc-info">
                  <label>Начало аренды</label>
                  <p>{trip.startLocation || "Неизвестно"}</p>
                  <small>{formatDateTime(trip.startTime)}</small>
                </div>
              </div>
              <div className="loc-line"></div>
              <div className="location-row">
                <div className="loc-dot end"></div>
                <div className="loc-info">
                  <label>Завершение</label>
                  <p>{trip.endLocation || "В пути"}</p>
                  <small>{trip.endTime ? formatDateTime(trip.endTime) : "—"}</small>
                </div>
              </div>
            </div>

            <hr className="modal-divider" />

            {/* Ряд 2: Параметры */}
            <div className="detail-box">
              <label>Длительность</label>
              <p>{formatDuration(trip.duration)}</p>
            </div>
            
            <div className="detail-box">
              <label>Пробег</label>
              <p>{trip.distance ? trip.distance.toFixed(1) : 0} км</p>
            </div>

            <div className="detail-box">
              <label>Тариф</label>
              <p>
                {trip.tariffType === 'per_minute' && 'Поминутный'}
                {trip.tariffType === 'per_day' && 'Суточный'}
                {trip.tariffType === 'per_km' && 'За км'}
              </p>
            </div>

            <div className="detail-box">
              <label>Топливо</label>
              {/* Если есть данные о топливе в DTO, можно вывести */}
              <p>—</p> 
            </div>

          </div>
        </div>

        {/* --- Footer (Цена) --- */}
        <div className="trip-modal-footer">
          <div className="footer-total">
            <span>Итоговая стоимость:</span>
            <span className="total-amount">{formatCurrency(trip.totalAmount)}</span>
          </div>
          
          {/* Если статус "Ожидает оплаты", можно добавить кнопку */}
          {/* {trip.statusName === 'Ожидает оплаты' && (
             <button className="btn-modal-pay">Оплатить</button>
          )} */}
        </div>

      </div>
    </div>
  );
};

export default TripDetailModal;
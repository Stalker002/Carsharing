import { useEffect } from "react";
import "./TripDetailModal.css";
import { formatCurrency, formatDateTime, formatDuration } from "./utils";

const TripDetailModal = ({ trip, onClose }) => {
  useEffect(() => {
    document.body.style.overflow = "hidden";
    return () => {
      document.body.style.overflow = "unset";
    };
  }, []);

  if (!trip) return null;

  const imageUrl = trip.carImage
    ? `http://localhost:5078${trip.carImage}`
    : null;
  const getStatusClass = (status) => {
    if (!status) return "modal-status-default";
    const s = status.toLowerCase();

    if (s.includes("оплачен")) return "modal-status-success";
    if (s.includes("заверш")) return "modal-status-warning"; 
    if (s.includes("пути") || s.includes("аренд")) return "modal-status-active";
    return "modal-status-default";
  };
  const getFuelInfo = () => {
    if (trip.refueled > 0) return `+${trip.refueled} л (Заправлено)`;
    if (trip.fuelUsed > 0) return `-${trip.fuelUsed} л (Потрачено)`;
    return "Без изменений";
  };

  return (
    <div className="trip-modal-overlay" onClick={onClose}>
      <div className="trip-modal-content" onClick={(e) => e.stopPropagation()}>
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
        <div className="trip-modal-body">
          <div className="trip-car-section">
            <div className="trip-car-img">
              {imageUrl ? <img src={imageUrl} alt={trip.carModel} /> : null}
            </div>
            <div className="trip-car-info">
              <h2>
                {trip.carBrand} {trip.carModel}
              </h2>
              <span
                className={`modal-status-badge ${getStatusClass(
                  trip.statusName
                )}`}
              >
                {trip.statusName}
              </span>
            </div>
          </div>
          <div className="trip-details-grid">
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
                  <small>
                    {trip.endTime ? formatDateTime(trip.endTime) : "—"}
                  </small>
                </div>
              </div>
            </div>
            <hr className="modal-divider" />
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
                {trip.tariffType === "per_minute" && "Поминутный"}
                {trip.tariffType === "per_day" && "Суточный"}
                {trip.tariffType === "per_km" && "За км"}
              </p>
            </div>
            <div className="detail-box">
              <label>Топливо</label>
              <p>{getFuelInfo()}</p>
            </div>
            <div className="detail-box">
              <label>Страховка</label>
              <p
                style={{
                  color: trip.insuranceActive ? "#2ed573" : "inherit",
                  fontWeight: trip.insuranceActive ? "bold" : "normal",
                }}
              >
                {trip.insuranceActive ? "Включена" : "Нет"}
              </p>
            </div>
          </div>
        </div>
        <div className="trip-modal-footer">
          <div className="footer-total">
            <span>Итоговая стоимость:</span>
            <span className="total-amount">
              {formatCurrency(trip.totalAmount)}
            </span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default TripDetailModal;

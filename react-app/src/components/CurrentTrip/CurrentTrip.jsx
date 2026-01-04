import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";

import "./CurrentTrip.css";
import {
  cancelTrip,
  finishTrip,
  getActiveTrip,
} from "../../redux/actions/trips";
import emptyTrip from "../../svg/Profile/emptyTrip.svg";
import { openModal } from "../../redux/actions/modal";

const CurrentTrip = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const { activeTrip, isLoading } = useSelector((state) => state.trips);

  const [formData, setFormData] = useState({
    endLocation: "",
    fuelLevel: "",
    distance: "",
  });

  useEffect(() => {
    dispatch(getActiveTrip());
  }, [dispatch]);

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleCancel = () => {
    dispatch(
      openModal({
        title: "Отмена поездки",
        message:
          "Вы уверены, что хотите отменить поездку? Деньги не будут списаны, а машина станет доступна для других.",
        type: "confirm",
        confirmText: "Да, отменить",
        cancelText: "Нет",
        onConfirm: async () => {
          const result = await dispatch(cancelTrip(activeTrip.id));

          if (result.success) {
            dispatch(
              openModal({
                type: "success",
                title: "Отменено",
                message: "Поездка успешно отменена.",
              })
            );
            dispatch(getActiveTrip());
          } else {
            dispatch(
              openModal({
                type: "error",
                title: "Ошибка",
                message: result.message,
              })
            );
          }
        },
      })
    );
  };

  const handleFinish = async () => {
    if (!formData.endLocation || !formData.fuelLevel) {
      return dispatch(
        openModal({
          type: "error",
          title: "Внимание",
          message: "Пожалуйста, укажите место парковки и уровень топлива.",
        })
      );
    }
    if (!formData.distance || Number(formData.distance) < 0) {
      return dispatch(
        openModal({
          type: "error",
          title: "Внимание",
          message: "Пожалуйста, укажите корректный пробег за поездку.",
        })
      );
    }
    if (formData.fuelLevel < 0 || formData.fuelLevel > 100) {
      return dispatch(
        openModal({
          type: "error",
          title: "Внимание",
          message: "Уровень топлива должен быть от 0 до 100%",
        })
      );
    }

    const payload = {
      tripId: activeTrip.id,
      endLocation: formData.endLocation,
      fuelLevel: Number(formData.fuelLevel),
      distance: Number(formData.distance),
    };
    try {
      const result = await dispatch(finishTrip(payload));

      if (result && result.success) {
        const billId = result.data.billId;
        dispatch(
          openModal({
            type: "success",
            title: "Поездка завершена!",
            message: `Сумма к оплате: ${result.data.totalAmount} BYN. Переходим к оплате...`,
          })
        );
        dispatch(getActiveTrip());
        setTimeout(() => {
          navigate(`/payment/${billId}`);
        }, 3000);
      } else {
        dispatch(
          openModal({
            type: "error",
            title: "Ошибка завершения",
            message: result?.message,
          })
        );
      }
    } catch (error) {
      dispatch(
        openModal({
          type: "error",
          title: "Сетевая ошибка",
          message: "Не удалось соединиться с сервером",
        })
      );
    }
  };

  if (isLoading) {
    return <div className="trip-loading">Загрузка информации о поездке...</div>;
  }

  if (!activeTrip) {
    return (
      <div className="empty-trip-container">
        <div className="empty-icon-circle">
          <img src={emptyTrip} />
        </div>
        <h2>Активных поездок нет</h2>
        <p>
          Вы еще не арендовали автомобиль. Перейдите в каталог, чтобы начать.
        </p>
        <button
          className="btn-go-catalog"
          onClick={() => navigate("/car-catalog")}
        >
          Выбрать автомобиль
        </button>
      </div>
    );
  }

  const imageUrl = activeTrip.carImage
    ? `http://localhost:5078${activeTrip.carImage}`
    : null;

  return (
    <div className="current-trip-wrapper">
      <div className="trip-card">
        <div className="trip-header">
          <div>
            <span className="badge-live">LIVE</span>
            <h2 className="car-title">
              {activeTrip.carBrand} {activeTrip.carModel}
            </h2>
          </div>
          <div className="trip-timer">
            Начало:{" "}
            {activeTrip.startTime ? (
              new Date(
                activeTrip.startTime.endsWith("Z")
                  ? activeTrip.startTime
                  : activeTrip.startTime + "Z"
              ).toLocaleTimeString("ru-RU", {
                hour: "2-digit",
                minute: "2-digit",
                timeZone: "Europe/Moscow",
              })
            ) : (
              "..."
            )}
          </div>
        </div>
        <div className="trip-body">
          <div className="car-image-box">
            <img src={imageUrl} alt="Car" />
          </div>
          <div className="trip-details-grid">
            <div className="detail-item">
              <label>Тариф</label>
              <strong>
                {activeTrip.tariffType === "per_minute" && "Поминутный"}
                {activeTrip.tariffType === "per_day" && "Суточный"}
                {activeTrip.tariffType === "per_km" && "За км"}
              </strong>
            </div>
            <div className="detail-item">
              <label>Текущая цена</label>
              <strong>
                {activeTrip.tariffType === "per_minute" &&
                  `${activeTrip.pricePerMinute}BYN/мин`}
                {activeTrip.tariffType === "per_day" &&
                  `${activeTrip.pricePerDay}BYN/день`}
                {activeTrip.tariffType === "per_km" &&
                  `${activeTrip.pricePerKm}BYN/км`}
              </strong>
            </div>
            <div className="detail-item">
              <label>Старт</label>
              <strong>{activeTrip.carLocation}</strong>
            </div>
          </div>
        </div>
        <hr className="divider" />
        <div className="trip-finish-section">
          <h3>Завершение аренды</h3>
          <div className="finish-form-grid">
            <div className="form-group">
              <label>Где вы оставили машину?</label>
              <input
                type="text"
                name="endLocation"
                placeholder="Адрес или точка на карте"
                value={formData.endLocation}
                onChange={handleChange}
              />
            </div>
            <div className="form-group">
              <label>Топливо (%)</label>
              <input
                type="number"
                name="fuelLevel"
                placeholder="Например: 45"
                min="0"
                max="100"
                value={formData.fuelLevel}
                onChange={handleChange}
              />
            </div>
            <div className="form-group">
              <label>Пробег за поездку (км)</label>
              <input
                type="number"
                name="distance"
                placeholder="Например: 12"
                min="0"
                value={formData.distance}
                onChange={handleChange}
              />
            </div>
          </div>
          <div className="trip-actions-row">
            <button className="btn-finish-trip" onClick={handleFinish}>
              Завершить поездку
            </button>
            <button className="btn-cancel-trip" onClick={handleCancel}>
              Отменить поездку
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CurrentTrip;

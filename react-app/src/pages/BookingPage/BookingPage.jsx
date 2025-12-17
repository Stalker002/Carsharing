import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";

import { getInfoCars } from "../../redux/actions/cars";
import { createBooking, deleteBooking } from "../../redux/actions/bookings";
import { createTrip } from "../../redux/actions/trips";

import Security from "../../svg/Payment/security.svg";
import "../PaymentPage/PaymentPage.css";
import Header from "../../components/Header/Header";

const BookingPage = () => {
  const { id } = useParams();
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const car = useSelector(
    (state) => state.cars.infoCar?.[0] || state.cars.currentCar
  );
  const isLoggedIn = useSelector((state) => state.users.isLoggedIn);
  const myClient = useSelector((state) => state.clients.myClient);

  const tomorrow = new Date();
  tomorrow.setDate(tomorrow.getDate() + 1);

  const [formData, setFormData] = useState({
    pickupLocation: "Minsk",
    pickupTime: new Date().toLocaleTimeString("ru-RU", {
      hour: "2-digit",
      minute: "2-digit",
    }),
    pickupDate: new Date().toISOString().split("T")[0],

    dropoffLocation: "Minsk",
    dropoffDate: tomorrow.toISOString().split("T")[0],
    dropoffTime: "12:00",

    tariffType: "per_minute",
    insuranceActive: false,
    agreeMarketing: false,
    agreeTerms: false,
  });

  useEffect(() => {
    window.scrollTo(0, 0);
    if (id) dispatch(getInfoCars(id));
  }, [dispatch, id]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value,
    }));
  };

  const handleRentNow = async () => {
    if (!formData.agreeTerms)
      return alert("Пожалуйста, примите условия соглашения");
    if (!isLoggedIn) return alert("Пожалуйста, авторизуйтесь");
    if (!car) return alert("Данные машины не загружены");

    if (!formData.dropoffDate || !formData.dropoffTime) {
      return alert("Укажите дату и время возврата");
    }

    const startTimeISO = new Date().toISOString();
    const plannedEndTimeString = `${formData.dropoffDate}T${formData.dropoffTime}`;
    const plannedEndTime = new Date(plannedEndTimeString);

    if (isNaN(plannedEndTime.getTime())) {
      return alert("Некорректная дата возврата");
    }

    const now = new Date();
    if (plannedEndTime <= now) {
      return alert("Время возврата должно быть в будущем!");
    }

    const endTimeISO = plannedEndTime.toISOString();

    try {
      const bookingPayload = {
        statusId: 5,
        carId: Number(id),
        clientId: myClient.id,
        startTime: startTimeISO,
        endTime: endTimeISO,
      };

      const bookingResult = await dispatch(createBooking(bookingPayload));

      if (!bookingResult || !bookingResult.success) {
        throw new Error(
          bookingResult?.message || "Не удалось создать бронирование"
        );
      }

      if (!bookingResult.success) {
         return alert(bookingResult.message);
      }

      const newBookingId = bookingResult.data;
      const tripPayload = {
        bookingId: newBookingId,
        statusId: 8,
        tariffType: formData.tariffType,

        startTime: startTimeISO,
        endTime: null,
        duration: 0,
        distance: 0,

        startLocation: car.location || formData.pickupLocation,
        endLocation: formData.dropoffLocation,
        insuranceActive: formData.insuranceActive,
        fuelUsed: 0,
        refueled: 0,
        carId: Number(id),
      };

      const tripResult = await dispatch(createTrip(tripPayload));

      if (tripResult && tripResult.success) {
        alert("Поездка началась!");
        navigate("/profile/current-trip");
      } else {
        console.warn("Trip creation failed. Rolling back booking:", newBookingId);
        await dispatch(deleteBooking(newBookingId));
        alert("Ошибка старта поездки: " + tripResult?.message);
      }
    } catch (error) {
      console.error("Rental Error:", error);
      alert("Ошибка: " + error.message);
    }
  };

  if (!car) return <div className="payment-loading">Загрузка данных...</div>;

  const imageUrl = car.imagePath ? `http://localhost:5078${car.imagePath}` : "";

  return (
    <>
      <Header />
      <div className="payment-page">
        <div className="payment-container">
          <div className="payment-forms-col">
            <div className="payment-card">
              <div className="payment-card-header">
                <div>
                  <h3 className="payment-card-title">Информация об аренде</h3>
                  <p className="payment-step-desc">Планирование времени</p>
                </div>
                <span className="payment-step-number">Шаг 1 из 3</span>
              </div>

              <div className="payment-rental-section">
                <div className="payment-radio-header">
                  <input
                    type="radio"
                    checked
                    readOnly
                    className="payment-radio-input"
                  />{" "}
                  Старт (Сейчас)
                </div>
                <div className="payment-grid-3">
                  <label className="payment-label">
                    Локация
                    <input
                      type="text"
                      className="payment-input"
                      value={car.location || "Minsk"}
                      readOnly
                    />
                  </label>
                  <label className="payment-label">
                    Дата
                    <input
                      type="date"
                      className="payment-input"
                      value={formData.pickupDate}
                      readOnly
                    />
                  </label>
                  <label className="payment-label">
                    Время
                    <input
                      type="time"
                      className="payment-input"
                      value={formData.pickupTime}
                      readOnly
                    />
                  </label>
                </div>
              </div>

              <div className="payment-rental-section">
                <div className="payment-radio-header">
                  <input
                    type="radio"
                    checked
                    readOnly
                    className="payment-radio-input"
                  />{" "}
                  Планируемый возврат
                </div>
                <div className="payment-grid-3">
                  <label className="payment-label">
                    Локация
                    <select
                      name="dropoffLocation"
                      className="payment-input"
                      value={formData.dropoffLocation}
                      onChange={handleChange}
                    >
                      <option value="Minsk">Минск</option>
                      <option value="Airport">Аэропорт</option>
                    </select>
                  </label>
                  <label className="payment-label">
                    Дата
                    <input
                      type="date"
                      name="dropoffDate"
                      className="payment-input"
                      value={formData.dropoffDate}
                      onChange={handleChange}
                    />
                  </label>
                  <label className="payment-label">
                    Время
                    <input
                      type="time"
                      name="dropoffTime"
                      className="payment-input"
                      value={formData.dropoffTime}
                      onChange={handleChange}
                    />
                  </label>
                </div>
              </div>
            </div>
            <div className="payment-card">
              <div className="payment-card-header">
                <div>
                  <h3 className="payment-card-title">Выбор тарифа</h3>
                  <p className="payment-step-desc">Как вы хотите платить?</p>
                </div>
                <span className="payment-step-number">Шаг 2 из 3</span>
              </div>

              <div className="payment-methods-list">
                <label
                  className={`payment-method-item ${
                    formData.tariffType === "per_minute" ? "active" : ""
                  }`}
                >
                  <div className="payment-method-header">
                    <div
                      style={{
                        display: "flex",
                        alignItems: "center",
                        gap: "10px",
                      }}
                    >
                      <input
                        type="radio"
                        name="tariffType"
                        value="per_minute"
                        checked={formData.tariffType === "per_minute"}
                        onChange={handleChange}
                        className="payment-radio-input"
                      />
                      <span>Поминутный</span>
                    </div>
                    <span style={{ fontWeight: "bold" }}>
                      ${car.pricePerMinute}/мин
                    </span>
                  </div>
                </label>
                <label
                  className={`payment-method-item ${
                    formData.tariffType === "per_km" ? "active" : ""
                  }`}
                >
                  <div className="payment-method-header">
                    <div
                      style={{
                        display: "flex",
                        alignItems: "center",
                        gap: "10px",
                      }}
                    >
                      <input
                        type="radio"
                        name="tariffType"
                        value="per_km"
                        checked={formData.tariffType === "per_km"}
                        onChange={handleChange}
                        className="payment-radio-input"
                      />
                      <span>За километраж</span>
                    </div>
                    <span style={{ fontWeight: "bold" }}>
                      ${car.pricePerKm}/км
                    </span>
                  </div>
                </label>
                <label
                  className={`payment-method-item ${
                    formData.tariffType === "per_day" ? "active" : ""
                  }`}
                >
                  <div className="payment-method-header">
                    <div
                      style={{
                        display: "flex",
                        alignItems: "center",
                        gap: "10px",
                      }}
                    >
                      <input
                        type="radio"
                        name="tariffType"
                        value="per_day"
                        checked={formData.tariffType === "per_day"}
                        onChange={handleChange}
                        className="payment-radio-input"
                      />
                      <span>Суточный</span>
                    </div>
                    <span style={{ fontWeight: "bold" }}>
                      ${car.pricePerDay}/день
                    </span>
                  </div>
                </label>
              </div>
            </div>

            <div className="payment-card">
              <div className="payment-card-header">
                <div>
                  <h3 className="payment-card-title">Подтверждение</h3>
                  <p className="payment-step-desc">
                    Начните свою поездку после клика!
                  </p>
                </div>
                <span className="payment-step-number">Шаг 3 из 3</span>
              </div>

              <div className="payment-checkbox-group">
                <label className="payment-checkbox-row">
                  <input
                    type="checkbox"
                    name="insuranceActive"
                    checked={formData.insuranceActive}
                    onChange={handleChange}
                    className="payment-checkbox-input"
                  />
                  <span>
                    <b>Добавить страховку (+10%)</b>. Полное покрытие ущерба.
                  </span>
                </label>
                <label className="payment-checkbox-row">
                  <input
                    type="checkbox"
                    name="agreeMarketing"
                    onChange={handleChange}
                    className="payment-checkbox-input"
                  />
                  <span>Я соглашаюсь с email рассылкой</span>
                </label>
                <label className="payment-checkbox-row">
                  <input
                    type="checkbox"
                    name="agreeTerms"
                    onChange={handleChange}
                    className="payment-checkbox-input"
                  />
                  <span>Я принимаю правила и условия пользования</span>
                </label>
              </div>

              <button className="payment-submit-btn" onClick={handleRentNow}>
                Начать аренду сейчас
              </button>

              <div className="payment-security">
                <img src={Security} alt="Security" />
                <div>
                  <h4>Все данные защищены</h4>
                  <p>Надежное хранение данных</p>
                </div>
              </div>
            </div>
          </div>
          <div className="payment-summary-col">
            <div className="payment-summary-card">
              <h3 className="payment-card-title">Сводка аренды</h3>
              <p className="payment-summary-desc">
                Итоговая цена будет рассчитана после завершения поездки.
              </p>

              <div className="payment-summary-car">
                <div className="payment-car-img-box">
                  <img src={imageUrl} alt={car.model} />
                </div>
                <div className="payment-car-info">
                  <h2>
                    {car.brand} {car.model}
                  </h2>
                </div>
              </div>

              <div className="payment-prices-list">
                <div
                  className={`payment-price-row ${
                    formData.tariffType === "per_minute"
                      ? "highlight-price"
                      : ""
                  }`}
                >
                  <span>Цена за минуту</span>
                  <span className="payment-price-value">
                    ${car.pricePerMinute || 0.5}
                  </span>
                </div>
                <div
                  className={`payment-price-row ${
                    formData.tariffType === "per_km" ? "highlight-price" : ""
                  }`}
                >
                  <span>Цена за км</span>
                  <span className="payment-price-value">
                    ${car.pricePerKm || 0.5}
                  </span>
                </div>
                <div
                  className={`payment-price-row ${
                    formData.tariffType === "per_day" ? "highlight-price" : ""
                  }`}
                >
                  <span>Цена за день</span>
                  <span className="payment-price-value">
                    ${car.pricePerDay}
                  </span>
                </div>

                {formData.insuranceActive && (
                  <div className="payment-price-row highlight-option">
                    <span>Страховка</span>
                    <span className="payment-price-value">+10%</span>
                  </div>
                )}
              </div>

              <div className="payment-total-block">
                <div>
                  <h3>Выбран тариф</h3>
                  <p>
                    {formData.tariffType === "per_minute" && "Оплата за время"}
                    {formData.tariffType === "per_km" && "Оплата за расстояние"}
                    {formData.tariffType === "per_day" && "Фикс. цена за сутки"}
                  </p>
                </div>
                <div className="payment-big-price">
                  {formData.tariffType === "per_minute" &&
                    `$${car.pricePerMinute}/мин`}
                  {formData.tariffType === "per_km" && `$${car.pricePerKm}/км`}
                  {formData.tariffType === "per_day" && `$${car.pricePerDay}/д`}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default BookingPage;

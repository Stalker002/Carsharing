import { useEffect, useState, useMemo } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";

import { getInfoCars } from "../../redux/actions/cars";
import { createBooking, deleteBooking } from "../../redux/actions/bookings";
import { createTrip } from "../../redux/actions/trips";
import { openModal } from "../../redux/actions/modal";

import Security from "../../svg/Payment/security.svg";
import "./BookingPage.css";
import Header from "../../components/Header/Header";
import { getMyDocuments } from "../../redux/actions/clients";

const TariffOption = ({ id, label, price, unit, selected, onChange }) => (
  <label className={`booking-method-item ${selected === id ? "active" : ""}`}>
    <div className="booking-method-content">
      <div className="radio-label">
        <input
          type="radio"
          name="tariffType"
          value={id}
          checked={selected === id}
          onChange={onChange}
          className="booking-radio-input"
        />
        <span className="method-name">{label}</span>
      </div>
      <div className="method-price">${price}{unit}</div>
    </div>
  </label>
);

const CheckboxRow = ({ name, checked, onChange, title, desc, price, link }) => (
  <label className={`booking-checkbox-row ${checked ? "checked" : ""}`}>
    <div className="checkbox-content">
      <input
        type="checkbox"
        name={name}
        checked={checked}
        onChange={onChange}
        className="booking-checkbox-input"
      />
      <div>
        <span className="checkbox-title">{title}</span>
        {desc && <p className="checkbox-desc">{desc}</p>}
      </div>
    </div>
    {price && <span className="checkbox-price">{price}</span>}
  </label>
);

const BookingPage = () => {
  const { id } = useParams();
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const car = useSelector((state) => state.cars.infoCar?.[0] || state.cars.currentCar);
  const isLoggedIn = useSelector((state) => state.users.isLoggedIn);
  const myClient = useSelector((state) => state.clients.myClient);
  const documentsState = useSelector((state) => state.clients.myDocument);
  const documents = Array.isArray(documentsState) ? documentsState : (documentsState ? [documentsState] : []);

  const today = new Date();
  const tomorrow = new Date(today);
  tomorrow.setDate(tomorrow.getDate() + 1);

  const [formData, setFormData] = useState({
    pickupLocation: "Minsk",
    pickupTime: today.toLocaleTimeString("ru-RU", { hour: "2-digit", minute: "2-digit" }),
    pickupDate: today.toISOString().split("T")[0],
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

    if (myClient?.id) {
        dispatch(getMyDocuments(myClient.id));
    }
  }, [dispatch, id, myClient]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData((prev) => ({ ...prev, [name]: type === "checkbox" ? checked : value }));
  };

  const tariffs = useMemo(() => [
    { id: 'per_minute', label: 'Поминутный', price: car?.pricePerMinute, unit: '/мин' },
    { id: 'per_km', label: 'За километраж', price: car?.pricePerKm, unit: '/км' },
    { id: 'per_day', label: 'Суточный', price: car?.pricePerDay, unit: '/день' }
  ], [car]);

  const handleRentNow = async () => {
    if (!formData.agreeTerms) return dispatch(openModal({ type: "error", title: "Внимание", message: "Примите условия соглашения" }));
    if (!isLoggedIn) return dispatch(openModal({ type: "error", title: "Внимание", message: "Авторизуйтесь" }));
    if (!car) return dispatch(openModal({ type: "error", title: "Внимание", message: "Данные машины не загружены" }));
    if (!formData.dropoffDate || !formData.dropoffTime) return dispatch(openModal({ type: "error", title: "Внимание", message: "Укажите дату возврата" }));

    const startTimeISO = new Date().toISOString();
    const plannedEndTimeString = `${formData.dropoffDate}T${formData.dropoffTime}`;
    const plannedEndTime = new Date(plannedEndTimeString);

    if (isNaN(plannedEndTime.getTime())) return dispatch(openModal({ type: "error", title: "Внимание", message: "Некорректная дата" }));
    if (plannedEndTime <= new Date()) return dispatch(openModal({ type: "error", title: "Внимание", message: "Время возврата должно быть в будущем" }));

    const hasLicense = documents.find(doc => 
        doc.type === "Водительское удостоверение" || 
        doc.type === "Водительские права"
    );

    if (!hasLicense) {
       return dispatch(openModal({
            type: "confirm",
            title: "Документы не найдены",
            message: "Для аренды автомобиля необходимо загрузить водительское удостоверение. Хотите перейти в профиль для загрузки?",
            confirmText: "Перейти в профиль", 
            cancelText: "Отмена",
            onConfirm: () => {
                navigate("/personal-page"); 
            }
        }));
    }

    if (new Date(hasLicense.expiryDate) < new Date()) {
      return dispatch(openModal({
            type: "confirm",
            title: "Документ просрочен",
            message: "Срок действия вашего водительского удостоверения истек. Хотите перейти в профиль для загрузки?",
            confirmText: "Перейти в профиль", 
            cancelText: "Отмена",
            onConfirm: () => {
                navigate("/personal-page"); 
            }
        }));
    }

    try {
      const bookingPayload = {
        statusId: 5,
        carId: Number(id),
        clientId: myClient.id,
        startTime: startTimeISO,
        endTime: plannedEndTime.toISOString(),
      };

      const bookingResult = await dispatch(createBooking(bookingPayload));

      if (!bookingResult?.success) throw new Error(bookingResult?.message || "Ошибка создания брони");

      const newBookingId = bookingResult.data;

      const tripPayload = {
        bookingId: newBookingId,
        statusId: 9,
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

      if (tripResult?.success) {
        await dispatch(openModal({ type: "success", title: "Поездка началась!", message: "Счастливого пути." }));
        navigate("/personal-page/current-trip");
      } else {
        console.warn("Rolling back booking:", newBookingId);
        await dispatch(deleteBooking(newBookingId));
        dispatch(openModal({ type: "error", title: "Ошибка", message: "Ошибка старта: " + tripResult?.message }));
      }
    } catch (error) {
      dispatch(openModal({ type: "error", title: "Ошибка", message: error.message || "Сбой системы" }));
    }
  };

  if (!car) return <div className="booking-loading">Загрузка...</div>;
  const imageUrl = car.imagePath ? `http://localhost:5078${car.imagePath}` : "";

  const selectedTariff = tariffs.find(t => t.id === formData.tariffType) || tariffs[0];

  return (
    <>
      <Header />
      <div className="booking-page">
        <div className="booking-container">
          <div className="booking-forms-col">
            <div className="booking-card">
              <div className="booking-card-header">
                <div><h3 className="booking-card-title">Детали аренды</h3><p className="booking-step-desc">Проверьте время</p></div>
                <span className="booking-step-number">Шаг 1 из 3</span>
              </div>
              <div className="booking-section">
                <div className="booking-section-title"><span className="dot start"></span> Начало (Сейчас)</div>
                <div className="booking-grid-3">
                  <div className="form-group">
                    <label>Локация</label>
                    <input type="text" className="booking-input" value={car.location || "Minsk"} readOnly />
                  </div>
                  <div className="form-group">
                    <label>Дата</label>
                    <input type="date" className="booking-input" value={formData.pickupDate} readOnly />
                  </div>
                  <div className="form-group">
                    <label>Время</label>
                    <input type="time" className="booking-input" value={formData.pickupTime} readOnly />
                  </div>
                </div>
              </div>
              <div className="booking-section">
                <div className="booking-section-title"><span className="dot end"></span> Возврат (План)</div>
                <div className="booking-grid-3">
                  <div className="form-group">
                    <label>Локация</label>
                    <select name="dropoffLocation" className="booking-input" value={formData.dropoffLocation} onChange={handleChange}>
                      <option value="Minsk">Минск</option>
                      <option value="Airport">Аэропорт</option>
                    </select>
                  </div>
                  <div className="form-group">
                    <label>Дата</label>
                    <input type="date" name="dropoffDate" className="booking-input" value={formData.dropoffDate} onChange={handleChange} />
                  </div>
                  <div className="form-group">
                    <label>Время</label>
                    <input type="time" name="dropoffTime" className="booking-input" value={formData.dropoffTime} onChange={handleChange} />
                  </div>
                </div>
              </div>
            </div>
            <div className="booking-card">
              <div className="booking-card-header">
                <div><h3 className="booking-card-title">Тариф</h3><p className="booking-step-desc">Выберите план</p></div>
                <span className="booking-step-number">Шаг 2 из 3</span>
              </div>
              <div className="booking-methods-list">
                {tariffs.map(t => (
                  <TariffOption key={t.id} {...t} selected={formData.tariffType} onChange={handleChange} />
                ))}
              </div>
            </div>
            <div className="booking-card">
              <div className="booking-card-header">
                <div><h3 className="booking-card-title">Подтверждение</h3><p className="booking-step-desc">Финальный шаг</p></div>
                <span className="booking-step-number">Шаг 3 из 3</span>
              </div>

              <div className="booking-checkbox-group">
                <CheckboxRow 
                    name="insuranceActive" 
                    checked={formData.insuranceActive} 
                    onChange={handleChange}
                    title="Полная страховка" 
                    desc="Покрытие ущерба при ДТП" 
                    price="+$10%" 
                />
                <div className="agreements">
                    <label className="simple-checkbox">
                        <input type="checkbox" name="agreeMarketing" checked={formData.agreeMarketing} onChange={handleChange} />
                        <span>Соглашаюсь на рассылку</span>
                    </label>
                    <label className="simple-checkbox">
                        <input type="checkbox" name="agreeTerms" checked={formData.agreeTerms} onChange={handleChange} />
                        <span>Принимаю <a href="#">условия</a></span>
                    </label>
                </div>
              </div>

              <button className="booking-submit-btn" onClick={handleRentNow}>Начать аренду</button>

              <div className="booking-security">
                <img src={Security} alt="Security" />
                <div><h4>Безопасно</h4><p>SSL шифрование данных</p></div>
              </div>
            </div>
          </div>
          <div className="booking-summary-col">
            <div className="booking-summary-card">
              <h3 className="booking-card-title">Сводка</h3>
              <p className="booking-summary-desc">Оплата по факту</p>

              <div className="booking-summary-car">
                <div className="booking-car-img-box">
                  <img src={imageUrl} alt={car.model} />
                </div>
                <div className="booking-car-info">
                  <h2>{car.brand} {car.model}</h2>
                </div>
              </div>

              <div className="booking-prices-list">
                {tariffs.map(t => (
                    <div key={t.id} className={`booking-price-row ${formData.tariffType === t.id ? "highlight-price" : ""}`}>
                        <span>{t.label}</span>
                        <span className="booking-price-value">{t.price || 0.5}BYN{t.unit}</span>
                    </div>
                ))}

                {formData.insuranceActive && (
                  <div className="booking-price-row highlight-option">
                    <span>Страховка</span>
                    <span className="booking-price-value">+10%</span>
                  </div>
                )}
              </div>

              <div className="booking-total-block">
                <div><h3>Выбрано</h3><p>{selectedTariff.label}</p></div>
                <div className="booking-big-price">{selectedTariff.price}BYN{selectedTariff.unit}</div>
              </div>
            </div>
          </div>

        </div>
      </div>
    </>
  );
};

export default BookingPage;
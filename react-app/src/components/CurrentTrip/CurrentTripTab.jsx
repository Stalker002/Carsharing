import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";

import "./CurrentTripTab.css";
import { finishTrip, getActiveTrip } from "../../redux/actions/trips";

const CurrentTripTab = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  // Получаем данные из Redux
  const { activeTrip, isLoading } = useSelector((state) => state.trips);
  
  // Локальный стейт для формы завершения
  const [formData, setFormData] = useState({
    endLocation: "",
    fuelLevel: "",
  });

  // Загружаем поездку при открытии вкладки
  useEffect(() => {
    dispatch(getActiveTrip());
  }, [dispatch]);

  // Обработчик ввода
  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  // Завершение поездки
  const handleFinish = async () => {
    if (!formData.endLocation || !formData.fuelLevel) {
      return alert("Пожалуйста, укажите место парковки и уровень топлива.");
    }

    if (formData.fuelLevel < 0 || formData.fuelLevel > 100) {
        return alert("Уровень топлива должен быть от 0 до 100%");
    }

    // Формируем запрос для C# FinishTripRequest
    const payload = {
      tripId: activeTrip.id, // или activeTrip.tripId, проверьте DTO
      endLocation: formData.endLocation,
      fuelLevel: Number(formData.fuelLevel),
    };
    console.log("Завершение поездки с данными:", payload);
    try {
      const result = await dispatch(finishTrip(payload));

      if (result && result.success) {
        // result.data - это объект TripFinishResult с бэкенда (там есть billAmount)
        alert(`Поездка завершена!\nСумма к оплате: $${result.data.totalAmount}`);
        
        // Обновляем страницу (поездка пропадет, появится заглушка)
        dispatch(getCurrentTrip());
        // Или редирект на счета
        // navigate("/profile/bills"); 
      } else {
        alert("Ошибка: " + (result?.message || "Не удалось завершить поездку"));
      }
    } catch (error) {
      console.error(error);
      alert("Ошибка сети");
    }
  };

  // 1. Состояние загрузки
  if (isLoading) {
    return <div className="trip-loading">Загрузка информации о поездке...</div>;
  }

  // 2. Состояние: НЕТ АКТИВНОЙ ПОЕЗДКИ
  if (!activeTrip) {
    return (
      <div className="empty-trip-container">
        <div className="empty-icon-circle">
            {/* Если нет иконки, можно просто текст или emoji 🚗 */}
            <span style={{fontSize: '40px'}}>🔑</span> 
        </div>
        <h2>Активных поездок нет</h2>
        <p>Вы еще не арендовали автомобиль. Перейдите в каталог, чтобы начать.</p>
        <button className="btn-go-catalog" onClick={() => navigate("/dashboard")}>
          Выбрать автомобиль
        </button>
      </div>
    );
  }

  // 3. Состояние: ЕСТЬ ПОЕЗДКА
  const imageUrl = activeTrip.carImage 
    ? `http://localhost:5078${activeTrip.carImage}` 
    : "https://via.placeholder.com/300x200?text=No+Image";

  return (
    <div className="current-trip-wrapper">
      <div className="trip-card">
        {/* Шапка карточки */}
        <div className="trip-header">
          <div>
            <span className="badge-live">LIVE</span>
            <h2 className="car-title">{activeTrip.carBrand} {activeTrip.carModel}</h2>
          </div>
          <div className="trip-timer">
            {/* Можно добавить таймер, если есть время начала */}
            Начало: {new Date(activeTrip.startTime).toLocaleTimeString([], {hour: '2-digit', minute:'2-digit'})}
          </div>
        </div>

        {/* Инфо о машине и тарифе */}
        <div className="trip-body">
            <div className="car-image-box">
                <img src={imageUrl} alt="Car" />
            </div>
            
            <div className="trip-details-grid">
                <div className="detail-item">
                    <label>Тариф</label>
                    <strong>
                        {activeTrip.tariffType === 'per_minute' && 'Поминутный'}
                        {activeTrip.tariffType === 'per_day' && 'Суточный'}
                        {activeTrip.tariffType === 'per_km' && 'За км'}
                    </strong>
                </div>
                <div className="detail-item">
                    <label>Текущая цена</label>
                    <strong>
                        {activeTrip.tariffType === 'per_minute' && `$${activeTrip.pricePerMinute}/мин`}
                        {activeTrip.tariffType === 'per_day' && `$${activeTrip.pricePerDay}/день`}
                    </strong>
                </div>
                <div className="detail-item">
                    <label>Старт</label>
                    <strong>{activeTrip.carLocation}</strong>
                </div>
            </div>
        </div>

        <hr className="divider" />

        {/* Форма завершения */}
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
                        min="0" max="100"
                        value={formData.fuelLevel}
                        onChange={handleChange}
                    />
                </div>
            </div>

            <button className="btn-finish-trip" onClick={handleFinish}>
                Завершить поездку
            </button>
        </div>
      </div>
    </div>
  );
};

export default CurrentTripTab;
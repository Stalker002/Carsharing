import { useEffect, useState, useMemo } from "react";
import { useDispatch, useSelector } from "react-redux";
import "./BookingHistory.css";
import { formatCurrency, formatDate, formatDuration } from "./utils";
import { getMyTrips } from "../../redux/actions/trips";

const BookingHistoryTab = () => {
  const dispatch = useDispatch();
  const { myTrips, isTripLoading } = useSelector((state) => state.trips);
  
  // Состояние для выбранной поездки (для показа деталей)
  const [selectedTrip, setSelectedTrip] = useState(null);

  useEffect(() => {
    dispatch(getMyTrips());
  }, [dispatch]);

  // Подсчет статистики для верхнего блока
  const stats = useMemo(() => {
    if (!myTrips) return { count: 0, distance: 0, duration: 0 };
    return myTrips.reduce(
      (acc, trip) => ({
        count: acc.count + 1,
        distance: acc.distance + (trip.distance || 0),
        duration: acc.duration + (trip.duration || 0),
      }),
      { count: 0, distance: 0, duration: 0 }
    );
  }, [myTrips]);

  // --- Рендер Деталей (Второй скриншот) ---
  if (selectedTrip) {
    return (
      <div className="history-wrapper">
        <button className="back-btn" onClick={() => setSelectedTrip(null)}>
          ← Назад к списку
        </button>

        <div className="detail-page">
          <div className="detail-header">
            <div className="detail-car-name">
              {selectedTrip.carBrand} {selectedTrip.carModel}
            </div>
            <div className="status-badge" style={{position: 'static', background: '#e6f9ed', color: '#2ed573'}}>
                {selectedTrip.statusName}
            </div>
          </div>

          <div className="detail-row">
            <span className="detail-label">Начало аренды</span>
            <span className="detail-val">{formatDate(selectedTrip.startTime)}</span>
          </div>
          <div className="detail-row">
            <span className="detail-label">Завершение</span>
            <span className="detail-val">{formatDate(selectedTrip.endTime)}</span>
          </div>
          <div className="detail-row">
            <span className="detail-label">Пробег</span>
            <span className="detail-val">{selectedTrip.distance || 0} км</span>
          </div>
          
          <div className="detail-section-title">Детализация стоимости</div>
          
          {/* Имитация чека как на скрине */}
          <div className="bill-item">
            <div>
              <div className="detail-val">Тариф ({selectedTrip.tariffType === 'per_minute' ? 'Мин.' : 'Км/Сут'})</div>
              <div className="detail-label" style={{fontSize: '12px'}}>
                 {formatDuration(selectedTrip.duration)}
              </div>
            </div>
            <div className="detail-val">
               {/* Здесь можно вывести детальный расчет, если он есть, или просто общую сумму */}
               {formatCurrency(selectedTrip.totalAmount)}
            </div>
          </div>

          {/* Если были штрафы */}
          {/* <div className="bill-item"> ... </div> */}

          <div className="bill-total">
            <span>Итого</span>
            <span>{formatCurrency(selectedTrip.totalAmount)}</span>
          </div>
        </div>
      </div>
    );
  }

  // --- Рендер Списка (Первый скриншот) ---
  if (isTripLoading) return <div style={{textAlign:'center', padding: 40}}>Загрузка...</div>;

  return (
    <div className="history-wrapper">
      
      {/* 1. Блок статистики */}
      <div className="history-stats">
        <div className="stat-item">
          <h4>Поездки</h4>
          <p>{stats.count}</p>
        </div>
        <div className="stat-item">
          <h4>Пробег</h4>
          <p>{stats.distance.toFixed(1)} км</p>
        </div>
        <div className="stat-item">
          <h4>Длительность</h4>
          <p>{Math.floor(stats.duration / 60)} ч. {Math.round(stats.duration % 60)} мин.</p>
        </div>
      </div>

      {/* 2. Список поездок */}
      <div className="history-list">
        {myTrips && myTrips.length > 0 ? (
          myTrips.map((trip) => {
             const imageUrl = trip.carImage ? `http://localhost:5078${trip.carImage}` : null;
             
             return (
              <div key={trip.id} className="history-card" onClick={() => setSelectedTrip(trip)}>
                {/* Иконка */}
                <div className="card-icon-box">
                  {imageUrl ? <img src={imageUrl} alt="Car" /> : "🚗"}
                </div>

                {/* Инфо */}
                <div className="card-main-info">
                  <div className="card-title">{trip.carBrand} {trip.carModel}</div>
                  <div className="card-dates">
                    <span>{formatDate(trip.startTime)}</span>
                    <span>{formatDate(trip.endTime)}</span>
                  </div>
                </div>

                {/* Цена */}
                <div className="card-price-block">
                  <div className="card-price">{formatCurrency(trip.totalAmount)}</div>
                  <div className="card-meta">
                    <span>{formatDuration(trip.duration)}</span>
                    <span>{trip.distance ? `${trip.distance} км` : ""}</span>
                  </div>
                </div>
              </div>
            );
          })
        ) : (
          <div style={{textAlign: 'center', color: '#999', marginTop: 20}}>История пуста</div>
        )}
      </div>
    </div>
  );
};

export default BookingHistoryTab;
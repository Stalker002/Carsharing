import { useEffect, useState, useMemo } from "react";
import { useDispatch, useSelector } from "react-redux";
import InfiniteScroll from "react-infinite-scroll-component";

import { getMyTrips } from "../../redux/actions/trips";

import TripDetailModal from "./TripDetailModal";

import { 
  formatCurrency, 
  formatDateShort, 
  formatDuration 
} from "./utils";

import "./BookingHistory.css";

const BookingHistoryTab = () => {
  const dispatch = useDispatch();

  const myTrips = useSelector((state) => state.trips.myTrips);
  const myTripsTotal = useSelector((state) => state.trips.myTripsTotal);

  const [page, setPage] = useState(1);
  const [selectedTrip, setSelectedTrip] = useState(null);
  const LIMIT = 10;

  useEffect(() => {
    setPage(1);
    dispatch(getMyTrips(1, LIMIT, true));
  }, [dispatch]);

  const fetchMoreData = () => {
    const nextPage = page + 1;
    setPage(nextPage);
    dispatch(getMyTrips(nextPage, LIMIT, false));
  };

  const stats = useMemo(() => {
    if (!myTrips || myTrips.length === 0) {
      return { count: 0, distance: 0, duration: 0 };
    }
    return myTrips.reduce(
      (acc, trip) => ({
        count: myTripsTotal,
        distance: acc.distance + (trip.distance || 0),
        duration: acc.duration + (trip.duration || 0),
      }),
      { count: 0, distance: 0, duration: 0 }
    );
  }, [myTrips, myTripsTotal]);

  const hasMore = myTrips.length < myTripsTotal;

  return (
    <div className="history-wrapper" id="history-scroll-target">
      <div className="history-stats">
        <div className="stat-item">
          <h4>Всего поездок</h4>
          <p>{myTripsTotal}</p>
        </div>
        <div className="stat-item">
          <h4>Пробег</h4>
          <p>{stats.distance.toFixed(1)} км</p>
        </div>
        <div className="stat-item">
          <h4>Время в пути</h4>
          <p>{formatDuration(stats.duration)}</p>
        </div>
      </div>
      <InfiniteScroll
        dataLength={myTrips.length}
        next={fetchMoreData}
        hasMore={hasMore}
        loader={<div className="history-loader">Загрузка поездок...</div>}
        endMessage={
          myTrips.length > 0 ? (
            <p className="history-end">Вы просмотрели всю историю</p>
          ) : (
            <div className="empty-history">
                <div className="empty-icon">📜</div>
                <h3>История пуста</h3>
                <p>Вы еще не совершали поездок.</p>
            </div>
          )
        }
        style={{ overflow: 'visible' }}
      >
        <div className="history-list">
          {myTrips.map((trip) => {
            const imageUrl = trip.carImage
              ? `http://localhost:5078${trip.carImage}`
              : null;
            const statusClass = trip.statusName === "Оплачено" 
                ? "status-success" 
                : trip.statusName === "Завершено" 
                    ? "status-warning" 
                    : "status-default";
            return (
              <div
                key={trip.id}
                className="history-card"
                onClick={() => setSelectedTrip(trip)}
              >
                <div className="card-icon-box">
                  {imageUrl ? (
                    <img src={imageUrl} alt="Car" />
                  ) : (
                    <span style={{ fontSize: "24px" }}>🚗</span>
                  )}
                </div>
                <div className="card-main-info">
                  <div className="card-title">
                    {trip.carBrand} {trip.carModel}
                  </div>
                  <div className="card-subtitle">
                    <span>{formatDateShort(trip.startTime)}</span>
                    <span className="dot">•</span>
                    <span className={`status-text ${statusClass}`}>
                      {trip.statusName}
                    </span>
                  </div>
                </div>
                <div className="card-price-block">
                  <div className="card-price">
                    {formatCurrency(trip.totalAmount)}
                  </div>
                  <div className="card-arrow">›</div>
                </div>
              </div>
            );
          })}
        </div>
      </InfiniteScroll>
      {selectedTrip && (
        <TripDetailModal
          trip={selectedTrip}
          onClose={() => setSelectedTrip(null)}
        />
      )}
    </div>
  );
};

export default BookingHistoryTab;
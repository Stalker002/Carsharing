import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import InfiniteScroll from "react-infinite-scroll-component";

// Импортируй свой экшен для получения машин
// import { getCars } from "../../redux/Actions/cars"; 

function CarsTab() {
  const dispatch = useDispatch();
  
  // 1. Локальный стейт для страницы
  const [page, setPage] = useState(1);

  // 2. Селекторы (нужно заменить на твои реальные из Redux)
  // Предположим, что машины лежат в state.cars.cars
  const cars = useSelector((state) => state.cars?.cars || []); 
  // Нужно знать общее количество, чтобы hasMore работал правильно
  const totalCars = useSelector((state) => state.cars?.carsTotal || 0);
  const isLoading = useSelector((state) => state.cars?.isCarsLoading);

  // 3. Вычисляем, есть ли еще данные для загрузки
  const hasMore = cars.length < totalCars;

  // 4. Загрузка данных при смене страницы
  useEffect(() => {
    // dispatch(getCars(page)); // <-- Твой экшен получения машин с пагинацией
    // Пример заглушки, пока нет реального экшена:
    console.log(`Загружаю страницу ${page}`);
  }, [page, dispatch]);

  // 5. Обработчик для загрузки следующей порции
  const nextHandler = () => {
    if (!isLoading) {
      setPage((prev) => prev + 1);
    }
  };

  return (
    // 6. Оборачиваем в InfiniteScroll
    <InfiniteScroll
      dataLength={cars.length} // Текущее кол-во элементов
      next={nextHandler}       // Функция для подгрузки
      hasMore={hasMore}        // Флаг: есть ли еще данные
      loader={<h4>Загрузка...</h4>}
      scrollableTarget="scrollableDiv" // ID контейнера, внутри которого скроллим
    >
      {/* Важно: этот div должен иметь фиксированную высоту и overflow-y: auto в CSS */}
      <div id="scrollableDiv" className="table-container" style={{ height: "500px", overflow: "auto" }}>
        <table className="admin-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Название</th>
              <th>Цена / день</th>
            </tr>
          </thead>
          <tbody>
            {/* Проверка, что cars это массив, чтобы не упало */}
            {Array.isArray(cars) && cars.map((c) => (
              <tr key={c.id}>
                <td>{c.id}</td>
                <td>{c.name}</td>
                <td>{c.price}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </InfiniteScroll>
  );
}

export default CarsTab;
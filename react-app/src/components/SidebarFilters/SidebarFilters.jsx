import "./SidebarFilters.css";
import { useState } from 'react';

export default function SidebarFilters() {
  const [selectedTypes, setSelectedTypes] = useState([]); // массив
  const [selectedCapacities, setSelectedCapacities] = useState([]); // массив
  const [maxPrice, setMaxPrice] = useState(1000);

  const toggleSelection = (value, selectedList, setSelectedList) => {
    if (selectedList.includes(value)) {
      setSelectedList(selectedList.filter((item) => item !== value)); // удалить
    } else {
      setSelectedList([...selectedList, value]); // добавить
    }
  };

  const carTypes = [
    { name: "Стандарт", count: 10 },
    { name: "Кроссовер", count: 12 },
    { name: "Премиум", count: 16 },
    { name: "Спорткар", count: 20 },
    { name: "Внедорожник", count: 14 },
    { name: "Грузовой", count: 14 }
  ];

  const capacities = [
    { name: "2 человека", count: 10 },
    { name: "4 человека", count: 14 },
    { name: "6 человек", count: 12 },
    { name: "8 и более", count: 16 }
  ];

  const clearFilters = () => {
    setSelectedType([]);
    setSelectedCapacity([]);
    // setMaxPrice(1000);
  };
  return (
    <div className="sidebar">
      <div className="sidebar-header">
        <h2>Фильтры</h2>
        <button className="clear-filters" onClick={clearFilters}>
          Очистить все
        </button>
      </div>

      <div className="filter-group">
        <h3>Тип машины</h3>
        <div className="filter-options">
          {carTypes.map(type => (
            <label key={type.name} className="filter-option">
              <input
                type="checkbox"
                name="type"
                value={type.name}
                checked={selectedTypes.includes(type.name)}
                onChange={() => toggleSelection(type.name, selectedTypes, setSelectedTypes)}
              />
              <span className="filter-label">
                {type.name} <span className="filter-count">({type.count})</span>
              </span>
            </label>
          ))}
        </div>
      </div>

      <div className="filter-group">
        <h3>Кол-во человек</h3>
        <div className="filter-options">
          {capacities.map(capacity => (
            <label key={capacity.name} className="filter-option">
              <input
                type="checkbox"
                name="capacity"
                value={capacity.name.split(' ')[0]}
                checked={selectedCapacities.includes(capacity.name)}
                onChange={() => toggleSelection(capacity.name, selectedCapacities, setSelectedCapacities)}
              />
              <span className="filter-label">
                {capacity.name} <span className="filter-count">({capacity.count})</span>
              </span>
            </label>
          ))}
        </div>
      </div>

      {/* <div className="filter-group">
        <h3>Цена</h3>
        <div className="price-filter">
          <div className="price-header">
            <span>Максимум <strong>{maxPrice.toFixed(2)} BYN</strong></span>
          </div>
          <input
            type="range"
            min="0"
            max="1000"
            value={maxPrice}
            onChange={(e) => setMaxPrice(Number(e.target.value))}
            className="price-slider"
          />
          <div className="price-labels">
            <span>0 BYN</span>
            <span>1000 BYN</span>
          </div>
        </div>
      </div> */}
    </div>
  );
}

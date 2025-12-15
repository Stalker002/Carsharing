import "./SidebarFilters.css";

export default function SidebarFilters({ filters, setFilters }) {

  const toggleSelection = (value, key) => {
    const list = filters[key];
    const newList = list.includes(value)
      ? list.filter((item) => item !== value)
      : [...list, value];

    setFilters({ ...filters, [key]: newList });
  };

  const handlePriceChange = (e) => {
      setFilters({ ...filters, maxPrice: Number(e.target.value) });
  };

  const clearFilters = () => {
    setFilters({
        types: [],
        capacities: [],
        maxPrice: 1000
    });
  };

  const carTypes = [
    { name: "Стандарт", count: 10 },
    { name: "Кроссовер", count: 12 },
    { name: "Премиум", count: 16 },
    { name: "Спорткар", count: 20 },
    { name: "Внедорожник", count: 14 },
    { name: "Грузовой", count: 14 },
  ];

  const capacities = [
    { name: "2 человека",  value: "2", count: 10 },
    { name: "4 человека",  value: "4", count: 14 },
  ];

  return (
    <div className="sidebar-filters">
      <div className="sidebar-header">
        <h2>Фильтры</h2>
        <button className="clear-filters" onClick={clearFilters}>
          Очистить все
        </button>
      </div>
      <div className="filter-group">
        <h3>Тип машины</h3>
        <div className="filter-options">
          {carTypes.map((type) => (
            <label key={type.name} className="filter-option">
              <input
                type="checkbox"
                checked={filters.types.includes(type.name)}
                onChange={() => toggleSelection(type.name, "types")}
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
          {capacities.map((capacity) => (
            <label key={capacity.name} className="filter-option">
              <input
                type="checkbox"
                checked={filters.capacities.includes(capacity.value)}
                onChange={() => toggleSelection(capacity.value, "capacities")}
              />
              <span className="filter-label">
                {capacity.name}
              </span>
            </label>
          ))}
        </div>
      </div>
      <div className="filter-group">
        <h3>Цена</h3>
        <div className="price-filter">
          <div className="price-header">
            <span>Максимум <strong>{filters.maxPrice} BYN</strong></span>
          </div>
          <input
            type="range"
            min="0"
            max="500"
            step="10"
            value={filters.maxPrice}
            onChange={handlePriceChange}
            className="price-slider"
          />
          <div className="price-labels">
            <span>0</span>
            <span>500</span>
          </div>
        </div>
      </div>
    </div>
  );
}

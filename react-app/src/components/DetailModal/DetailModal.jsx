import { useState } from "react";
import "./DetailModal.css";

const DetailModal = ({ title, data, fields, onClose, additionalTabs = [] }) => {
  const [activeTab, setActiveTab] = useState("main");

  if (!data) return null;

  return (
    // Уникальный класс оверлея
    <div className="detail-modal__overlay" onMouseDown={onClose}>
      <div
        className="detail-modal__container"
        onMouseDown={(e) => e.stopPropagation()}
      >
        {/* Хедер */}
        <div className="detail-modal__header">
          <h3 className="detail-modal__title">{title}</h3>
          <button className="detail-modal__close" onClick={onClose}>
            ×
          </button>
        </div>

        {/* Табы */}
        <div className="detail-modal__tabs">
          <button
            className={`detail-modal__tab-btn ${
              activeTab === "main" ? "detail-modal__tab-btn--active" : ""
            }`}
            onClick={() => setActiveTab("main")}
          >
            Основное
          </button>

          {additionalTabs.map((tab, index) => (
            <button
              key={index}
              className={`detail-modal__tab-btn ${
                activeTab === index ? "detail-modal__tab-btn--active" : ""
              }`}
              onClick={() => setActiveTab(index)}
            >
              {tab.title}
            </button>
          ))}
        </div>

        {/* Тело */}
        <div className="detail-modal__body">
          {/* Вкладка: Основное */}
          {activeTab === "main" && (
            <div className="detail-modal__grid">
              {fields
                .filter((f) => !f.hideOnDetail)
                .map((field) => {
                  // Логика получения значения (учитываем viewName)
                  const fieldKey = field.viewName || field.name;
                  const value = data[fieldKey] !== undefined ? data[fieldKey] : data[field.name];

                  let displayValue = value;

                  // Форматирование
                  if (field.type === "select") {
                    const option = field.options?.find(
                      (opt) => String(opt.value) === String(value)
                    );
                    // Если нашли опцию в конфиге - показываем её label, иначе оставляем value (например "Эконом")
                    if (option) displayValue = option.label;
                  }
                  
                  if (field.type === "datetime-local" && value) {
                    displayValue = new Date(value).toLocaleString("ru-RU", {
                        day: 'numeric', month: 'long', year: 'numeric',
                        hour: '2-digit', minute: '2-digit'
                    });
                  }
                  
                  if (
                    field.type === "boolean" ||
                    (field.options && field.options[0]?.value === true)
                  ) {
                    displayValue = value ? "Да" : "Нет";
                  }

                  return (
                    <div key={field.name} className="detail-modal__row">
                      <span className="detail-modal__label">{field.label}</span>
                      <span className="detail-modal__value">
                        {displayValue !== undefined && displayValue !== null && displayValue !== "" 
                          ? displayValue 
                          : "—"}
                      </span>
                    </div>
                  );
                })}
            </div>
          )}

          {/* Вкладки: Дополнительные */}
          {typeof activeTab === "number" && additionalTabs[activeTab] && (
            <div className="detail-modal__tab-content">
              {additionalTabs[activeTab].content}
            </div>
          )}
        </div>

        {/* Футер */}
        <div className="detail-modal__footer">
          <button className="detail-modal__btn" onClick={onClose}>
            Закрыть
          </button>
        </div>
      </div>
    </div>
  );
};

export default DetailModal;
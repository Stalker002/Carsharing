import { useState } from "react";
import "./DetailModal.css";

const DetailModal = ({ title, data, fields, onClose, additionalTabs = [] }) => {
  const [activeTab, setActiveTab] = useState("main");

  if (!data) return null;

  return (
    <div className="detail-modal-overlay" onMouseDown={onClose}>
      <div
        className="detail-modal-container"
        onMouseDown={(e) => e.stopPropagation()}
      >
        <div className="detail-modal-header">
          <h3 className="detail-modal-title">{title}</h3>
          <button className="detail-modal-close" onClick={onClose}>
            ×
          </button>
        </div>
        <div className="detail-modal-tabs">
          <button
            className={`detail-modal-tab-btn ${
              activeTab === "main" ? "detail-modal-tab-btn-active" : ""
            }`}
            onClick={() => setActiveTab("main")}
          >
            Основное
          </button>

          {additionalTabs.map((tab, index) => (
            <button
              key={index}
              className={`detail-modal-tab-btn ${
                activeTab === index ? "detail-modal-tab-btn-active" : ""
              }`}
              onClick={() => setActiveTab(index)}
            >
              {tab.title}
            </button>
          ))}
        </div>

        <div className="detail-modal-body">
          {activeTab === "main" && (
            <div className="detail-modal-grid">
              {fields
                .filter((f) => !f.hideOnDetail)
                .map((field) => {
                  const fieldKey = field.viewName || field.name;
                  const value =
                    data[fieldKey] !== undefined
                      ? data[fieldKey]
                      : data[field.name];

                  let displayValue = value;

                  if (field.type === "select") {
                    const option = field.options?.find(
                      (opt) => String(opt.value) === String(value)
                    );
                    if (option) displayValue = option.label;
                  }

                  if (field.type === "datetime-local" && value) {
                    displayValue = new Date(value).toLocaleString("ru-RU", {
                      day: "numeric",
                      month: "long",
                      year: "numeric",
                      hour: "2-digit",
                      minute: "2-digit",
                    });
                  }

                  if (
                    field.type === "boolean" ||
                    (field.options && field.options[0]?.value === true)
                  ) {
                    displayValue = value ? "Да" : "Нет";
                  }

                  return (
                    <div key={field.name} className="detail-modal-row">
                      <span className="detail-modal-label">{field.label}</span>
                      <span className="detail-modal-value">
                        {displayValue !== undefined &&
                        displayValue !== null &&
                        displayValue !== ""
                          ? displayValue
                          : "—"}
                      </span>
                    </div>
                  );
                })}
            </div>
          )}

          {typeof activeTab === "number" && additionalTabs[activeTab] && (
            <div className="detail-modal-tab-content">
              {additionalTabs[activeTab].content}
            </div>
          )}
        </div>

        <div className="detail-modal-footer">
          <button className="detail-modal-btn" onClick={onClose}>
            Закрыть
          </button>
        </div>
      </div>
    </div>
  );
};

export default DetailModal;

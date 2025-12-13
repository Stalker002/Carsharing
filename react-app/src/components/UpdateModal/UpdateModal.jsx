import { useEffect, useState } from "react";
import "./UpdateModal.css";

const UpdateModal = ({
  title,
  onClose,
  children,
  onDelete,
  formId,
  additionalTabs = [],
}) => {
  const [activeTab, setActiveTab] = useState("main");

  useEffect(() => {
    const handleKeyDown = (e) => {
      if (e.key === "Escape") onClose();
    };
    window.addEventListener("keydown", handleKeyDown);
    return () => window.removeEventListener("keydown", handleKeyDown);
  }, [onClose]);

  return (
    <div className="update-modal-overlay" onMouseDown={onClose}>
      <div
        className="update-modal-container"
        onMouseDown={(e) => e.stopPropagation()}
      >
        <div className="update-modal-header">
          <h3 className="update-modal-title">{title}</h3>
          <button className="update-modal-close" onClick={onClose}>
            ×
          </button>
        </div>
        <div className="update-modal-tabs">
          <button
            className={`update-modal-tab-btn ${
              activeTab === "main" ? "update-modal-tab-btn-active" : ""
            }`}
            onClick={() => setActiveTab("main")}
          >
            Основное
          </button>
          {additionalTabs.map((tab, index) => (
            <button
              key={index}
              className={`update-modal-tab-btn ${
                activeTab === index ? "update-modal-tab-btn-active" : ""
              }`}
              onClick={() => setActiveTab(index)}
            >
              {tab.title}
            </button>
          ))}
        </div>
        <div className="update-modal-body">
          {activeTab === "main" && (
            <div className="update-modal-content-main">{children}</div>
          )}
          {typeof activeTab === "number" && additionalTabs[activeTab] && (
            <div className="update-modal-content-extra">
              {additionalTabs[activeTab].content}
            </div>
          )}
        </div>
        {activeTab === "main" && (
          <div className="update-modal-footer">
            {onDelete ? (
              <button type="button" className="btn-delete" onClick={onDelete}>
                Удалить
              </button>
            ) : (
              <div></div>
            )}
            <div className="update-modal-right-actions">
              <button type="button" className="btn-cancel" onClick={onClose}>
                Отмена
              </button>
              <button type="submit" form={formId} className="btn-save">
                Сохранить
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default UpdateModal;

import { useEffect } from "react";
import "./UpdateModal.css";

const UpdateModal = ({ title, onClose, children, onDelete, formId }) => {
  useEffect(() => {
    const handleKeyDown = (e) => {
      if (e.key === "Escape") onClose();
    };
    window.addEventListener("keydown", handleKeyDown);
    return () => window.removeEventListener("keydown", handleKeyDown);
  }, [onClose]);

  return (
    <div className="update-overlay" onMouseDown={onClose}>
      <div
        className="update-container"
        onMouseDown={(e) => e.stopPropagation()}
      >
        <div className="update-header">
          <span style={{ width: "24px" }}></span>
          <h3>{title}</h3>
          <button className="update-close-btn" onClick={onClose}>
            ×
          </button>
        </div>

        <div className="update-body">{children}</div>

        <div className="update-footer">
          {onDelete ? (
            <button
              type="button"
              className="update-btn-delete"
              onClick={onDelete}
            >
              Удалить
            </button>
          ) : (
            <div></div>
          )}

          <div className="update-right-actions">
            <button
              type="button"
              className="update-btn-cancel"
              onClick={onClose}
            >
              Отмена
            </button>
            <button type="submit" form={formId} className="update-btn-save">
              Сохранить
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default UpdateModal;

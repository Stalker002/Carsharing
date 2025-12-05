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
    <div className="modal-overlay" onMouseDown={onClose}>
      <div className="modal-container" onMouseDown={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h3>{title}</h3>
          <button className="modal-close" onClick={onClose}>×</button>
        </div>
        <div className="modal-body">{children}</div>

        <div className="modal-footer" style={{justifyContent: "space-between"}}>
          {onDelete ? (
            <button type="button" className="btn-delete" onClick={onDelete}>Удалить</button>
          ) : <div></div>}

          <div className="right-actions" style={{display: 'flex', gap: '10px'}}>
            <button type="button" className="btn-cancel" onClick={onClose}>Отмена</button>
            <button type="submit" form={formId} className="modal-add-btn">Сохранить</button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default UpdateModal;
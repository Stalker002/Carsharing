import { useDispatch, useSelector } from "react-redux";
import { closeModal } from "../../redux/actions/modal";
import "./GlobalModal.css";

const GlobalModal = () => {
  const dispatch = useDispatch();
  const { isOpen, type, title, message, onConfirm, confirmText, cancelText } = useSelector((state) => state.modal);

  if (!isOpen) return null;

  const getModalStyle = () => {
    switch (type) {
      case "success":
        return { icon: "✓", className: "modal-success" };
      case "error":
        return { icon: "✕", className: "modal-error" };
      case "confirm":
        return { icon: "?", className: "modal-info" }; 
      default:
        return { icon: "i", className: "modal-info" };
    }
  };

  const style = getModalStyle();

  const handleClose = () => {
    dispatch(closeModal());
  };

  const handleConfirm = () => {
    if (onConfirm) {
      onConfirm();
    }
    dispatch(closeModal());
  };

  return (
    <div className="global-modal-overlay" onClick={handleClose}>
      <div className={`global-modal-content ${style.className}`} onClick={(e) => e.stopPropagation()}>
        <div className="global-modal-icon">
          {style.icon}
        </div>
        
        <h3 className="global-modal-title">{title}</h3>
        <p className="global-modal-text">{message}</p>
        {type === "confirm" ? (
          <div className="modal-actions-row" style={{ display: 'flex', gap: '10px' }}>
            <button className="global-modal-btn btn-secondary" onClick={handleClose}>
              {cancelText}
            </button>
            <button className="global-modal-btn" onClick={handleConfirm}>
              {confirmText}
            </button>
          </div>
        ) : (
          <button className="global-modal-btn" onClick={handleClose}>
            {confirmText === "Да" ? "Понятно" : confirmText}
          </button>
        )}
      </div>
    </div>
  );
};

export default GlobalModal;
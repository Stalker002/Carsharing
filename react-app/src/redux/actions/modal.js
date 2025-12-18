export const openModal = ({ 
  title, 
  message, 
  type = "info", 
  onConfirm = null, 
  confirmText = "Ок", 
  cancelText = "Отмена" 
}) => ({
  type: "OPEN_MODAL",
  payload: { title, message, type, onConfirm, confirmText, cancelText },
});

export const closeModal = () => ({
  type: "CLOSE_MODAL",
});
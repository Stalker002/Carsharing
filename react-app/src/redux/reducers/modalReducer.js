const initialState = {
  isOpen: false,
  type: "info",
  title: "",
  message: "",
  onConfirm: null,
  confirmText: "Да",
  cancelText: "Отмена",
};

export const modalReducer = (state = initialState, action) => {
  switch (action.type) {
    case "OPEN_MODAL":
      return {
        ...state,
        isOpen: true,
        type: action.payload.type || "info",
        title: action.payload.title,
        message: action.payload.message,
        onConfirm: action.payload.onConfirm || null,
        confirmText: action.payload.confirmText || "Да",
        cancelText: action.payload.cancelText || "Отмена",
      };
    case "CLOSE_MODAL":
      return initialState;
    default:
      return state;
  }
};
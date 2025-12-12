import { api } from "../../api";
import {
  createClientDocumentFailed,
  createClientDocumentStarted,
  createClientDocumentSuccess,
  deleteClientDocumentFailed,
  deleteClientDocumentStarted,
  deleteClientDocumentSuccess,
  getClientDocumentsFailed,
  getClientDocumentsStarted,
  getClientDocumentsSuccess,
  updateClientDocumentFailed,
  updateClientDocumentStarted,
  updateClientDocumentSuccess,
} from "../actionCreators/clientDocuments";

export const getClientDocuments = () => {
  return async (dispatch) => {
    try {
      dispatch(getClientDocumentsStarted());

      const response = await api.clientDocuments.getClientDocuments();

      dispatch(getClientDocumentsSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка документов клиента";

      dispatch(getClientDocumentsFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const createClientDocument = (data) => {
  return async (dispatch) => {
    try {
      dispatch(createClientDocumentStarted());

      const response = await api.clientDocuments.createClientDocument(data);

      dispatch(createClientDocumentSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка документов клиента";

      dispatch(createClientDocumentFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const updateClientDocument = (id, data) => {
  return async (dispatch) => {
    try {
      dispatch(updateClientDocumentStarted());

      const response = await api.clientDocuments.updateClientDocument(id, data);

      dispatch(updateClientDocumentSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка документов клиента";

      dispatch(updateClientDocumentFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const deleteClientDocument = (id) => {
  return async (dispatch) => {
    try {
      dispatch(deleteClientDocumentStarted());

      const response = await api.clientDocuments.deleteClientDocument(id);

      dispatch(deleteClientDocumentSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка документов клиента";

      dispatch(deleteClientDocumentFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

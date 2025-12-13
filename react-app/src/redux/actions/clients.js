import { api } from "../../api";
import {
  createClientFailed,
  createClientStarted,
  createClientSuccess,
  deleteClientFailed,
  deleteClientStarted,
  deleteClientSuccess,
  getClientByUserFailed,
  getClientByUserStarted,
  getClientByUserSuccess,
  getClientDocumentFailed,
  getClientDocumentStarted,
  getClientDocumentSuccess,
  getClientsFailed,
  getClientsStarted,
  getClientsSuccess,
  getMyClientFailed,
  getMyClientStarted,
  getMyClientSuccess,
  getMyDocumentsFailed,
  getMyDocumentsStarted,
  getMyDocumentsSuccess,
  setClientsTotal,
  updateClientFailed,
  updateClientStarted,
  updateClientSuccess,
} from "../actionCreators/clients";
import { logoutUser } from "./users";

export const getClients = (page = 1) => {
  return async (dispatch) => {
    try {
      dispatch(getClientsStarted());

      const response = await api.clients.getClients({
        params: {
          _page: page,
          _limit: 25,
        },
      });

      const totalCount = parseInt(response.headers["x-total-count"], 10);
      if (!isNaN(totalCount)) {
        dispatch(setClientsTotal(totalCount));
      }

      dispatch(
        getClientsSuccess({
          data: response.data,
          page,
        })
      );
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка клиента";

      dispatch(getClientsFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const getMyClient = () => {
  return async (dispatch) => {
    try {
      dispatch(getMyClientStarted());

      const response = await api.clients.getMyClient();

      dispatch(getMyClientSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      if (error.response && error.response.status === 401) {
        dispatch(logoutUser());
      }
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка клиента";

      dispatch(getMyClientFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const getClientByUser = (userId) => {
  return async (dispatch) => {
    try {
      dispatch(getClientByUserStarted());

      const response = await api.clients.getClientByUserId(userId);

      dispatch(getClientByUserSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка клиента";

      dispatch(getClientByUserFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const getMyDocuments = () => {
  return async (dispatch) => {
    try {
      dispatch(getMyDocumentsStarted());

      const response = await api.clients.getMyDocuments();

      dispatch(getMyDocumentsSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка клиента";

      dispatch(getMyDocumentsFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const getClientDocument = (clientId) => {
  return async (dispatch) => {
    try {
      dispatch(getClientDocumentStarted());

      const response = await api.clients.getClientDocument(clientId);

      dispatch(getClientDocumentSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка клиента";

      dispatch(getClientDocumentFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const createClient = (data) => {
  return async (dispatch) => {
    try {
      dispatch(createClientStarted());

      const response = await api.clients.createClient(data);

      dispatch(createClientSuccess(response.data));

      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка регистрации";

      dispatch(createClientFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const updateClient = (id, data) => {
  return async (dispatch) => {
    try {
      dispatch(updateClientStarted());

      const response = await api.clients.updateClient(id, data);

      dispatch(updateClientSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка клиента";

      dispatch(updateClientFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const deleteClient = (id) => {
  return async (dispatch) => {
    try {
      dispatch(deleteClientStarted());

      const response = await api.clients.deleteClient(id);

      dispatch(deleteClientSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка клиента";

      dispatch(deleteClientFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

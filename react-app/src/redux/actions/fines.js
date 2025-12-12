import { api } from "../../api";
import {
  createFineFailed,
  createFineStarted,
  createFineSuccess,
  deleteFineFailed,
  deleteFineStarted,
  deleteFineSuccess,
  getFineByTripFailed,
  getFineByTripStarted,
  getFineByTripSuccess,
  getFinesFailed,
  getFinesStarted,
  getFinesSuccess,
  setFinesTotal,
  updateFineFailed,
  updateFineStarted,
  updateFineSuccess,
} from "../actionCreators/fines";

export const getFines = (page = 1) => {
  return async (dispatch) => {
    try {
      dispatch(getFinesStarted());

      const response = await api.fines.getFines({
        params: {
          _page: page,
          _limit: 25,
        },
      });

      const totalCount = parseInt(response.headers["x-total-count"], 10);
      if (!isNaN(totalCount)) {
        dispatch(setFinesTotal(totalCount));
      }

      dispatch(
        getFinesSuccess({
          data: response.data,
          page,
        })
      );
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка штрафов";

      dispatch(getFinesFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const getFineByTrips = (tripId) => {
  return async (dispatch) => {
    try {
      dispatch(getFineByTripStarted());

      const response = await api.fines.getFinesByTrip(tripId);

      dispatch(getFineByTripSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка штрафов";

      dispatch(getFineByTripFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const createFine = (data) => {
  return async (dispatch) => {
    try {
      dispatch(createFineStarted());

      const response = await api.fines.createFine(data);

      dispatch(createFineSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка штрафов";

      dispatch(createFineFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const updateFine = (id, data) => {
  return async (dispatch) => {
    try {
      dispatch(updateFineStarted());

      const response = await api.fines.updateFine(id, data);

      dispatch(updateFineSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка штрафов";

      dispatch(updateFineFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const deleteFine = (id) => {
  return async (dispatch) => {
    try {
      dispatch(deleteFineStarted());

      const response = await api.fines.deleteFine(id);

      dispatch(deleteFineSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка штрафов";

      dispatch(deleteFineFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

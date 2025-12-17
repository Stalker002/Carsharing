import { api } from "../../api";
import {
  createTripFailed,
  createTripStarted,
  createTripSuccess,
  deleteTripFailed,
  deleteTripStarted,
  deleteTripSuccess,
  finishTripFailed,
  finishTripStarted,
  finishTripSuccess,
  getActiveTripFailed,
  getActiveTripStarted,
  getActiveTripSuccess,
  getInfoTripFailed,
  getInfoTripStarted,
  getInfoTripSuccess,
  getMyTripsFailed,
  getMyTripsStarted,
  getMyTripsSuccess,
  getTripsFailed,
  getTripsStarted,
  getTripsSuccess,
  setMyTripsTotal,
  setTripsTotal,
  updateTripFailed,
  updateTripStarted,
  updateTripSuccess,
} from "../actionCreators/trips";

export const getTrips = (page = 1) => {
  return async (dispatch) => {
    try {
      dispatch(getTripsStarted());

      const response = await api.trips.getTrips({
        params: {
          _page: page,
          _limit: 25,
        },
      });

      const totalCount = parseInt(response.headers["x-total-count"], 10);
      if (!isNaN(totalCount)) {
        dispatch(setTripsTotal(totalCount));
      }

      dispatch(
        getTripsSuccess({
          data: response.data,
          page,
        })
      );
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка поездок";

      dispatch(getTripsFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const getMyTrips = (page = 1) => {
  return async (dispatch) => {
    try {
      dispatch(getMyTripsStarted());

      const response = await api.trips.getMyTrips({
        params: {
          _page: page,
          _limit: 25,
        },
      });

      const totalCount = parseInt(response.headers["x-total-count"], 10);
      if (!isNaN(totalCount)) {
        dispatch(setMyTripsTotal(totalCount));
      }

      dispatch(
        getMyTripsSuccess({
          data: response.data,
          page,
        })
      );
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка поездок";

      dispatch(getMyTripsFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const getTripWithInfo = (id) => {
  return async (dispatch) => {
    try {
      dispatch(getInfoTripStarted());

      const response = await api.trips.getTripWithInfo(id);

      const data = Array.isArray(response.data) && response.data.length > 0 
          ? response.data[0] 
          : response.data;

      dispatch(getInfoTripSuccess(data));
      return { success: true, data: data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка поездок";

      dispatch(getInfoTripFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const getActiveTrip = () => {
  return async (dispatch) => {
    try {
      dispatch(getActiveTripStarted());

      const response = await api.trips.getActiveTrip();

      const data = Array.isArray(response.data) && response.data.length > 0 
          ? response.data[0] 
          : response.data;

      dispatch(getActiveTripSuccess(data));
      return { success: true, data: data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка поездок";

      dispatch(getActiveTripFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const finishTrip = (data) => {
  return async (dispatch) => {
    try {
      dispatch(finishTripStarted());

      const response = await api.trips.finishTrip(data);


      dispatch(finishTripSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка поездок";

      dispatch(finishTripFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const createTrip = (data) => {
  return async (dispatch) => {
    try {
      dispatch(createTripStarted());

      const response = await api.trips.createTrip(data);

      dispatch(createTripSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка поездок";

      dispatch(createTripFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const updateTrip = (id, data) => {
  return async (dispatch) => {
    try {
      dispatch(updateTripStarted());

      const response = await api.trips.updateTrip(id, data);

      dispatch(updateTripSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка поездок";
      dispatch(updateTripFailed(error));
      return { success: false, message: errorMessage };
    }
  };
};

export const deleteTrip = (id) => {
  return async (dispatch) => {
    try {
      dispatch(deleteTripStarted());

      const response = await api.trips.deleteTrip(id);

      dispatch(deleteTripSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка поездок";

      dispatch(deleteTripFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

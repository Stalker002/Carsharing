import { api } from "../../api";
import {
  createTripFailed,
  createTripStarted,
  createTripSuccess,
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
    } catch (error) {
      dispatch(getTripsFailed(error));
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
    } catch (error) {
      dispatch(getMyTripsFailed(error));
    }
  };
};

export const getTripWithInfo = (id) => {
  return async (dispatch) => {
    try {
      dispatch(getInfoTripStarted());

      const response = await api.trips.getTripWithInfo(id);

      dispatch(getInfoTripSuccess(response.data));
    } catch (error) {
      dispatch(getInfoTripFailed(error));
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
    } catch (error) {
      dispatch(updateTripFailed(error));
    }
  };
};

export const deleteTrip = (id) => {
  return async (dispatch) => {
    try {
      dispatch(deleteTripStarted());

      const response = await api.trips.deleteTrip(id);

      dispatch(deleteTripSuccess(response.data));
    } catch (error) {
      dispatch(deleteTripFailed(error));
    }
  };
};

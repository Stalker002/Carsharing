import { api } from "../../api";
import { createFineFailed, createFineStarted, createFineSuccess, deleteFineFailed, deleteFineStarted, deleteFineSuccess, getFineByTripFailed, getFineByTripStarted, getFineByTripSuccess, getFinesFailed, getFinesStarted, getFinesSuccess, setFinesTotal, updateFineFailed, updateFineStarted, updateFineSuccess } from "../actionCreators/fines";

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

            dispatch(getFinesSuccess({
                data: response.data,
                page,
            }));
        } 
        catch (error) {
            dispatch(getFinesFailed(error));
        }
    };
};

export const getFineByTrips = (tripId) => {
    return async (dispatch) => {
        try {
            dispatch(getFineByTripStarted())

            const response = await api.fines.getFinesByTrip(tripId);

            dispatch(getFineByTripSuccess(response.data));
        } 
        catch (error) {
            dispatch(getFineByTripFailed(error));
        }
    };
};

export const createFine = (data) => {
    return async (dispatch) => {
        try {
            dispatch(createFineStarted());

            const response = await api.fines.createFine(data);

            dispatch(createFineSuccess(response.data));
        } 
        catch (error) {
            dispatch(createFineFailed(error));
        }
    };
};

export const updateFine = (id, data) => {
  return async (dispatch) => {
    try {
      dispatch(updateFineStarted());

      const response = await api.fines.updateFine(id, data);

      dispatch(updateFineSuccess(response.data));
    } 
    catch (error) {
      dispatch(updateFineFailed(error));
    }
  };
};

export const deleteFine = (id) => {
  return async (dispatch) => {
    try {
      dispatch(deleteFineStarted());

      const response = await api.fines.deleteFine(id);

      dispatch(deleteFineSuccess(response.data));
    } 
    catch (error) {
      dispatch(deleteFineFailed(error));
    }
  };
};
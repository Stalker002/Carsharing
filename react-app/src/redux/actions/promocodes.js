import { api } from "../../api";
import { createPromocodeFailed, createPromocodeStarted, createPromocodeSuccess, deletePromocodeFailed, deletePromocodeStarted, deletePromocodeSuccess, getActivePromocodesFailed, getActivePromocodesStarted, getActivePromocodesSuccess, getPromocodesFailed, getPromocodesStarted, getPromocodesSuccess, setActivePromocodesTotal, setPromocodesTotal, updatePromocodeFailed, updatePromocodeStarted, updatePromocodeSuccess } from "../actionCreators/promocodes";

export const getPromocodes = (page = 1) => {
    return async (dispatch) => {
        try {
            dispatch(getPromocodesStarted());

            const response = await api.promocodes.getPromocodes({
                params: {
                    _page: page,
                    _limit: 25,
                },
            });

            const totalCount = parseInt(response.headers["x-total-count"], 10);
            if (!isNaN(totalCount)) {
                dispatch(setPromocodesTotal(totalCount));
            }

            dispatch(getPromocodesSuccess({
                data: response.data,
                page,
            }));
        } 
        catch (error) {
            dispatch(getPromocodesFailed(error));
        }
    };
};

export const getActivePromocodes = (page = 1) => {
    return async (dispatch) => {
        try {
            dispatch(getActivePromocodesStarted())

            const response = await api.promocodes.getActivePromocodes({
                params: {
                    _page: page,
                    _limit: 25,
                },
            });

            const totalCount = parseInt(response.headers["x-total-count"], 10);
            if (!isNaN(totalCount)) {
                dispatch(setActivePromocodesTotal(totalCount));
            }

            dispatch(getActivePromocodesSuccess({
                data: response.data,
                page,
            }));
        } 
        catch (error) {
            dispatch(getActivePromocodesFailed(error));
        }
    };
};

export const createPromocode = (data) => {
    return async (dispatch) => {
        try {
            dispatch(createPromocodeStarted());

            const response = await api.promocodes.createPromocode(data);

            dispatch(createPromocodeSuccess(response.data));
        } 
        catch (error) {
            dispatch(createPromocodeFailed(error));
        }
    };
};

export const updatePromocode = (id, data) => {
  return async (dispatch) => {
    try {
      dispatch(updatePromocodeStarted());

      const response = await api.promocodes.updatePromocode(id, data);

      dispatch(updatePromocodeSuccess(response.data));
    } 
    catch (error) {
      dispatch(updatePromocodeFailed(error));
    }
  };
};

export const deletePromocode = (id) => {
  return async (dispatch) => {
    try {
      dispatch(deletePromocodeStarted());

      const response = await api.promocodes.deletePromocode(id);

      dispatch(deletePromocodeSuccess(response.data));
    } 
    catch (error) {
      dispatch(deletePromocodeFailed(error));
    }
  };
};
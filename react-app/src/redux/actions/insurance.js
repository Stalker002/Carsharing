import { api } from "../../api";
import {
  createInsuranceFailed,
  createInsuranceStarted,
  createInsuranceSuccess,
  deleteInsuranceFailed,
  deleteInsuranceStarted,
  deleteInsuranceSuccess,
  getActiveInsuranceByCarFailed,
  getActiveInsuranceByCarStarted,
  getActiveInsuranceByCarSuccess,
  getInsurancesByCarFailed,
  getInsurancesByCarStarted,
  getInsurancesByCarSuccess,
  getInsurancesFailed,
  getInsurancesStarted,
  getInsurancesSuccess,
  updateInsuranceFailed,
  updateInsuranceStarted,
  updateInsuranceSuccess,
} from "../actionCreators/insurances";

export const getInsurances = () => {
  return async (dispatch) => {
    try {
      dispatch(getInsurancesStarted());

      const response = await api.insurances.getInsurances();

      dispatch(getInsurancesSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка страховок";

      dispatch(getInsurancesFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const getInsuranceByCars = (carId) => {
  return async (dispatch) => {
    try {
      dispatch(getInsurancesByCarStarted());

      const response = await api.insurances.getInsuranceByCar(carId);

      dispatch(getInsurancesByCarSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка страховок";

      dispatch(getInsurancesByCarFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const getActiveInsuranceByCars = (carId) => {
  return async (dispatch) => {
    try {
      dispatch(getActiveInsuranceByCarStarted());

      const response = await api.insurances.getActiveInsuranceByCar(carId);

      dispatch(getActiveInsuranceByCarSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка страховок";

      dispatch(getActiveInsuranceByCarFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const createInsurance = (data) => {
  return async (dispatch) => {
    try {
      dispatch(createInsuranceStarted());

      const response = await api.insurances.createInsurance(data);

      dispatch(createInsuranceSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка страховок";

      dispatch(createInsuranceFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const updateInsurance = (id, data) => {
  return async (dispatch) => {
    try {
      dispatch(updateInsuranceStarted());

      const response = await api.insurances.updateInsurance(id, data);

      dispatch(updateInsuranceSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка страховок";

      dispatch(updateInsuranceFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const deleteInsurance = (id) => {
  return async (dispatch) => {
    try {
      dispatch(deleteInsuranceStarted());

      const response = await api.insurances.deleteInsurance(id);

      dispatch(deleteInsuranceSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка страховок";

      dispatch(deleteInsuranceFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

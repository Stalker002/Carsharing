import { api } from "../../api";
import { createInsuranceFailed, createInsuranceStarted, createInsuranceSuccess, deleteInsuranceFailed, deleteInsuranceStarted, deleteInsuranceSuccess, getActiveInsuranceByCarFailed, getActiveInsuranceByCarStarted, getActiveInsuranceByCarSuccess, getInsurancesByCarFailed, getInsurancesByCarStarted, getInsurancesByCarSuccess, getInsurancesFailed, getInsurancesStarted, getInsurancesSuccess, updateInsuranceFailed, updateInsuranceStarted, updateInsuranceSuccess } from "../actionCreators/insurances";

export const getInsurances = () => {
    return async (dispatch) => {
        try {
            dispatch(getInsurancesStarted());

            const response = await api.insurances.getInsurances();

            dispatch(getInsurancesSuccess(response.data));
        } 
        catch (error) {
            dispatch(getInsurancesFailed(error));
        }
    };
};

export const getInsuranceByCars = (carId) => {
    return async (dispatch) => {
        try {
            dispatch(getInsurancesByCarStarted())

            const response = await api.insurances.getInsuranceByCar(carId);

            dispatch(getInsurancesByCarSuccess(response.data));
        } 
        catch (error) {
            dispatch(getInsurancesByCarFailed(error));
        }
    };
};

export const getActiveInsuranceByCars = (carId) => {
    return async (dispatch) => {
        try {
            dispatch(getActiveInsuranceByCarStarted())

            const response = await api.insurances.getActiveInsuranceByCar(carId);

            dispatch(getActiveInsuranceByCarSuccess(response.data));
        } 
        catch (error) {
            dispatch(getActiveInsuranceByCarFailed(error));
        }
    };
};

export const createInsurance = (data) => {
    return async (dispatch) => {
        try {
            dispatch(createInsuranceStarted());

            const response = await api.insurances.createInsurance(data);

            dispatch(createInsuranceSuccess(response.data));
        } 
        catch (error) {
            dispatch(createInsuranceFailed(error));
        }
    };
};

export const updateInsurance = (id, data) => {
  return async (dispatch) => {
    try {
      dispatch(updateInsuranceStarted());

      const response = await api.insurances.updateInsurance(id, data);

      dispatch(updateInsuranceSuccess(response.data));
    } 
    catch (error) {
      dispatch(updateInsuranceFailed(error));
    }
  };
};

export const deleteInsurance = (id) => {
  return async (dispatch) => {
    try {
      dispatch(deleteInsuranceStarted());

      const response = await api.insurances.deleteInsurance(id);

      dispatch(deleteInsuranceSuccess(response.data));
    } 
    catch (error) {
      dispatch(deleteInsuranceFailed(error));
    }
  };
};
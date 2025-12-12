import { api } from "../../api";
import {
  createMaintenanceFailed,
  createMaintenanceStarted,
  createMaintenanceSuccess,
  deleteMaintenanceFailed,
  deleteMaintenanceStarted,
  deleteMaintenanceSuccess,
  getMaintenancesByCarFailed,
  getMaintenancesByCarStarted,
  getMaintenancesByCarSuccess,
  getMaintenancesFailed,
  getMaintenancesStarted,
  getMaintenancesSuccess,
  updateMaintenanceFailed,
  updateMaintenanceStarted,
  updateMaintenanceSuccess,
} from "../actionCreators/maintenance";

export const getMaintenances = () => {
  return async (dispatch) => {
    try {
      dispatch(getMaintenancesStarted());

      const response = await api.maintenance.getMaintenances();

      dispatch(getMaintenancesSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка обслуживания";

      dispatch(getMaintenancesFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const getMaintenanceByCars = (carId) => {
  return async (dispatch) => {
    try {
      dispatch(getMaintenancesByCarStarted());

      const response = await api.maintenance.getMaintenanceByCar(carId);

      dispatch(getMaintenancesByCarSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка обслуживания";

      dispatch(getMaintenancesByCarFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const createMaintenance = (data) => {
  return async (dispatch) => {
    try {
      dispatch(createMaintenanceStarted());

      const response = await api.maintenance.createMaintenance(data);

      dispatch(createMaintenanceSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка обслуживания";

      dispatch(createMaintenanceFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const updateMaintenance = (id, data) => {
  return async (dispatch) => {
    try {
      dispatch(updateMaintenanceStarted());

      const response = await api.maintenance.updateMaintenance(id, data);

      dispatch(updateMaintenanceSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка обслуживания";

      dispatch(updateMaintenanceFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const deleteMaintenance = (id) => {
  return async (dispatch) => {
    try {
      dispatch(deleteMaintenanceStarted());

      const response = await api.maintenance.deleteMaintenance(id);

      dispatch(deleteMaintenanceSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка обслуживания";

      dispatch(deleteMaintenanceFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

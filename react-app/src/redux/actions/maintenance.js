import { api } from "../../api";
import { createMaintenanceFailed, createMaintenanceStarted, createMaintenanceSuccess, deleteMaintenanceFailed, deleteMaintenanceStarted, deleteMaintenanceSuccess, getMaintenancesByCarFailed, getMaintenancesByCarStarted, getMaintenancesByCarSuccess, getMaintenancesFailed, getMaintenancesStarted, getMaintenancesSuccess, updateMaintenanceFailed, updateMaintenanceStarted, updateMaintenanceSuccess } from "../actionCreators/maintenance";

export const getMaintenances = () => {
    return async (dispatch) => {
        try {
            dispatch(getMaintenancesStarted());

            const response = await api.maintenance.getMaintenances();

            dispatch(getMaintenancesSuccess(response.data));
        } 
        catch (error) {
            dispatch(getMaintenancesFailed(error));
        }
    };
};

export const getMaintenanceByCars = (carId) => {
    return async (dispatch) => {
        try {
            dispatch(getMaintenancesByCarStarted())

            const response = await api.maintenance.getMaintenanceByCar(carId);

            dispatch(getMaintenancesByCarSuccess(response.data));
        } 
        catch (error) {
            dispatch(getMaintenancesByCarFailed(error));
        }
    };
};

export const createMaintenance = (data) => {
    return async (dispatch) => {
        try {
            dispatch(createMaintenanceStarted());

            const response = await api.maintenance.createMaintenance(data);

            dispatch(createMaintenanceSuccess(response.data));
        } 
        catch (error) {
            dispatch(createMaintenanceFailed(error));
        }
    };
};

export const updateMaintenance = (id, data) => {
  return async (dispatch) => {
    try {
      dispatch(updateMaintenanceStarted());

      const response = await api.maintenance.updateMaintenance(id, data);

      dispatch(updateMaintenanceSuccess(response.data));
    } 
    catch (error) {
      dispatch(updateMaintenanceFailed(error));
    }
  };
};

export const deleteMaintenance = (id) => {
  return async (dispatch) => {
    try {
      dispatch(deleteMaintenanceStarted());

      const response = await api.maintenance.deleteMaintenance(id);

      dispatch(deleteMaintenanceSuccess(response.data));
    } 
    catch (error) {
      dispatch(deleteMaintenanceFailed(error));
    }
  };
};
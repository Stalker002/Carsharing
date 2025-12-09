import { api } from "../../api";
import {
  createCarFailed,
  createCarStarted,
  createCarSuccess,
  deleteCarFailed,
  deleteCarStarted,
  deleteCarSuccess,
  getCarsByCategoryFailed,
  getCarsByCategoryStarted,
  getCarsByCategorySuccess,
  getCarsFailed,
  getCarsStarted,
  getCarsSuccess,
  getInfoCarFailed,
  getInfoCarStarted,
  getInfoCarSuccess,
  setCarsByCategoryTotal,
  setCarsTotal,
  updateCarFailed,
  updateCarStarted,
  updateCarSuccess,
} from "../actionCreators/cars";

export const getCars = (page = 1) => {
  return async (dispatch) => {
    try {
      dispatch(getCarsStarted());

      const response = await api.cars.getCars({
        params: {
          _page: page,
          _limit: 25,
        },
      });

      const totalCount = parseInt(response.headers["x-total-count"], 10);
      if (!isNaN(totalCount)) {
        dispatch(setCarsTotal(totalCount));
      }

      dispatch(
        getCarsSuccess({
          data: response.data,
          page,
        })
      );
    } catch (error) {
      dispatch(getCarsFailed(error));
    }
  };
};

export const getCarsByCategory = (ids, page = 1) => {
  return async (dispatch) => {
    try {
      dispatch(getCarsByCategoryStarted());

      const response = await api.cars.getCarByCategory({
        params: {
          ids: ids,
          _page: page,
          _limit: 25,
        },
      });

      const totalCount = parseInt(response.headers["x-total-count"], 10);
      if (!isNaN(totalCount)) {
        dispatch(setCarsByCategoryTotal(totalCount));
      }

      dispatch(
        getCarsByCategorySuccess({
          data: response.data,
          page,
        })
      );
    } catch (error) {
      dispatch(getCarsByCategoryFailed(error));
    }
  };
};

export const getInfoCars = (id) => {
  return async (dispatch) => {
    try {
      dispatch(getInfoCarStarted());
      const response = await api.cars.getCarInfo(id);
      dispatch(getInfoCarSuccess(response.data));
      return { success: true, data: response.data[0] };
    } catch (error) {
      dispatch(getInfoCarFailed(error));
      return { success: false, message: error.message };
    }
  };
};

export const createCar = (data) => {
  return async (dispatch) => {
    try {
      dispatch(createCarStarted());

      const formData = new FormData();

      Object.keys(data).forEach((key) => {
        if (key === "image" && data[key] instanceof File) {
          formData.append("Image", data[key]);
        } 
        else if (data[key] !== null && data[key] !== undefined) {
          formData.append(key, data[key]);
        }
      });

      const response = await api.cars.createCar(formData);

      dispatch(createCarSuccess(response.data));
      
      return { success: true };
    } catch (error) {
      const errorMessage = error.response?.data || error.message;
      dispatch(createCarFailed(error));
      
      return { success: false, message: errorMessage };
    }
  };
};

// === UPDATE CAR ===
export const updateCar = (id, data) => {
  return async (dispatch) => {
    try {
      dispatch(updateCarStarted());

      const response = await api.cars.updateCar(id, data);

      dispatch(updateCarSuccess(response.data));

      return { success: true };
    } catch (error) {
      const errorMessage = error.response?.data || error.message;
      dispatch(updateCarFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const deleteCar = (id) => {
  return async (dispatch) => {
    try {
      dispatch(deleteCarStarted());

      const response = await api.cars.deleteCar(id);

      dispatch(deleteCarSuccess(response.data));

      return { success: true };
    } catch (error) {
      const errorMessage = error.response?.data || error.message;
      dispatch(deleteCarFailed(error));
      
      return { success: false, message: errorMessage };
    }
  };
};
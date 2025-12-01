import { api } from "../../api";
import { createCarFailed, createCarStarted, createCarSuccess, deleteCarFailed, deleteCarStarted, deleteCarSuccess, getCarsByCategoryFailed, getCarsByCategoryStarted, getCarsByCategorySuccess, getCarsFailed, getCarsStarted, getCarsSuccess, getInfoCarFailed, getInfoCarStarted, getInfoCarSuccess, setCarsByCategoryTotal, setCarsTotal, updateCarFailed, updateCarStarted, updateCarSuccess } from "../actionCreators/cars";

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

            dispatch(getCarsSuccess({
                data: response.data,
                page,
            }));
        } 
        catch (error) {
            dispatch(getCarsFailed(error));
        }
    };
};

export const getCarsByCategory = (ids, page = 1) => {
    return async (dispatch) => {
        try {
            dispatch(getCarsByCategoryStarted())

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

            dispatch(getCarsByCategorySuccess({
                data: response.data,
                page,
            }));
        } 
        catch (error) {
            dispatch(getCarsByCategoryFailed(error));
        }
    };
};

export const getInfoCars = (id) => {
    return async (dispatch) => {
        try {
            dispatch(getInfoCarStarted())

            const response = await api.cars.getCarInfo(id);

            dispatch(getInfoCarSuccess(response.data));
        } 
        catch (error) {
            dispatch(getInfoCarFailed(error));
        }
    };
};

export const createCar = (data) => {
    return async (dispatch) => {
        try {
            dispatch(createCarStarted());

            const response = await api.cars.createCar(data);

            dispatch(createCarSuccess(response.data));
        } 
        catch (error) {
            dispatch(createCarFailed(error));
        }
    };
};

export const updateCar = (id, data) => {
  return async (dispatch) => {
    try {
      dispatch(updateCarStarted());

      const response = await api.cars.updateCar(id, data);

      dispatch(updateCarSuccess(response.data));
    } 
    catch (error) {
      dispatch(updateCarFailed(error));
    }
  };
};

export const deleteCar = (id) => {
  return async (dispatch) => {
    try {
      dispatch(deleteCarStarted());

      const response = await api.cars.deleteCar(id);

      dispatch(deleteCarSuccess(response.data));
    } 
    catch (error) {
      dispatch(deleteCarFailed(error));
    }
  };
};
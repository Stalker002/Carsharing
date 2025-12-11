export const GET_CARS_SUCCESS = "GET_CARS_SUCCESS";
export const GET_CARS_FAILED = "GET_CARS_FAILED";
export const GET_CARS_STARTED = "GET_CARS_STARTED";
export const SET_CARS_TOTAL = "SET_CARS_TOTAL";

export const GET_INFO_CAR_STARTED = "GET_INFO_CAR_STARTED";
export const GET_INFO_CAR_SUCCESS = "GET_INFO_CAR_SUCCESS";
export const GET_INFO_CAR_FAILED = "GET_INFO_CAR_FAILED";

export const GET_INFO_CAR_ADMIN_STARTED = "GET_INFO_CAR_ADMIN_STARTED";
export const GET_INFO_CAR_ADMIN_SUCCESS = "GET_INFO_CAR_ADMIN_SUCCESS";
export const GET_INFO_CAR_ADMIN_FAILED = "GET_INFO_CAR_ADMIN_FAILED";

export const GET_CARS_BY_CATEGORY_SUCCESS = "GET_CARS_BY_CATEGORY_SUCCESS";
export const GET_CARS_BY_CATEGORY_FAILED = "GET_CARS_BY_CATEGORY_FAILED";
export const GET_CARS_BY_CATEGORY_STARTED = "GET_CARS_BY_CATEGORY_STARTED";
export const SET_CARS_BY_CATEGORY_TOTAL = "SET_CARS_BY_CATEGORY_TOTAL";

export const POST_CAR_STARTED = "POST_CAR_STARTED";
export const POST_CAR_SUCCESS = "POST_CAR_SUCCESS";
export const POST_CAR_FAILED = "POST_CAR_FAILED";

export const PUT_CAR_STARTED = "PUT_CAR_STARTED";
export const PUT_CAR_SUCCESS = "PUT_CAR_SUCCESS";
export const PUT_CAR_FAILED = "PUT_CAR_FAILED";

export const DELETE_CAR_STARTED = "DELETE_CAR_STARTED";
export const DELETE_CAR_SUCCESS = "DELETE_CAR_SUCCESS";
export const DELETE_CAR_FAILED = "DELETE_CAR_FAILED";

export const getCarsStarted = () => ({
    type: GET_CARS_STARTED
});
export const getCarsSuccess = (cars) => ({
    type: GET_CARS_SUCCESS,
    payload: cars,
});
export const getCarsFailed = (error) => ({
    type: GET_CARS_FAILED,
    payload: error,
});
export const setCarsTotal = (total) => ({
    type: SET_CARS_TOTAL,
    payload: total,
});

export const getInfoCarStarted = () => ({
    type: GET_INFO_CAR_STARTED
});
export const getInfoCarSuccess = (cars) => ({
    type: GET_INFO_CAR_SUCCESS,
    payload: cars,
});
export const getInfoCarFailed = (error) => ({
    type: GET_INFO_CAR_FAILED,
    payload: error,
});

export const getInfoCarAdminStarted = () => ({
    type: GET_INFO_CAR_ADMIN_STARTED
});
export const getInfoCarAdminSuccess = (cars) => ({
    type: GET_INFO_CAR_ADMIN_SUCCESS,
    payload: cars,
});
export const getInfoCarAdminFailed = (error) => ({
    type: GET_INFO_CAR_ADMIN_FAILED,
    payload: error,
});

export const getCarsByCategoryStarted = () => ({
    type: GET_CARS_BY_CATEGORY_STARTED
});
export const getCarsByCategorySuccess = (cars) => ({
    type: GET_CARS_BY_CATEGORY_SUCCESS,
    payload: cars,
});
export const getCarsByCategoryFailed = (error) => ({
    type: GET_CARS_BY_CATEGORY_FAILED,
    payload: error,
});
export const setCarsByCategoryTotal = (total) => ({
    type: SET_CARS_BY_CATEGORY_TOTAL,
    payload: total,
});

export const createCarStarted = () => ({
    type: POST_CAR_STARTED,
});
export const createCarSuccess = (car) => ({
    type: POST_CAR_SUCCESS,
    payload: car,
});
export const createCarFailed = (error) => ({
    type: POST_CAR_FAILED,
    payload: error,
});

export const updateCarStarted = () => ({
    type: PUT_CAR_STARTED,
});
export const updateCarSuccess = (car) => ({
    type: PUT_CAR_SUCCESS,
    payload: car,
});
export const updateCarFailed = (error) => ({
    type: PUT_CAR_FAILED,
    payload: error,
});

export const deleteCarStarted = () => ({
    type: DELETE_CAR_STARTED,
});
export const deleteCarSuccess = (id) => ({
    type: DELETE_CAR_SUCCESS,
    payload: id,
});
export const deleteCarFailed = (error) => ({
    type: DELETE_CAR_FAILED,
    payload: error,
});
export const GET_PROMOCODES_SUCCESS = "GET_PROMOCODES_SUCCESS";
export const GET_PROMOCODES_FAILED = "GET_PROMOCODES_FAILED";
export const GET_PROMOCODES_STARTED = "GET_PROMOCODES_STARTED";
export const SET_PROMOCODES_TOTAL = "SET_PROMOCODES_TOTAL";

export const GET_ACTIVE_PROMOCODES_STARTED = "GET_ACTIVE_PROMOCODES_STARTED";
export const GET_ACTIVE_PROMOCODES_SUCCESS = "GET_ACTIVE_PROMOCODES_SUCCESS";
export const GET_ACTIVE_PROMOCODES_FAILED = "GET_ACTIVE_PROMOCODES_FAILED";
export const SET_ACTIVE_PROMOCODES_TOTAL = "SET_ACTIVE_PROMOCODES_TOTAL";

export const POST_PROMOCODE_STARTED = "POST_PROMOCODE_STARTED";
export const POST_PROMOCODE_SUCCESS = "POST_PROMOCODE_SUCCESS";
export const POST_PROMOCODE_FAILED = "POST_PROMOCODE_FAILED";

export const PUT_PROMOCODE_STARTED = "PUT_PROMOCODE_STARTED";
export const PUT_PROMOCODE_SUCCESS = "PUT_PROMOCODE_SUCCESS";
export const PUT_PROMOCODE_FAILED = "PUT_PROMOCODE_FAILED";

export const DELETE_PROMOCODE_STARTED = "DELETE_PROMOCODE_STARTED";
export const DELETE_PROMOCODE_SUCCESS = "DELETE_PROMOCODE_SUCCESS";
export const DELETE_PROMOCODE_FAILED = "DELETE_PROMOCODE_FAILED";

export const getPromocodesStarted = () => ({
    type: GET_PROMOCODES_STARTED
});
export const getPromocodesSuccess = (promocodes) => ({
    type: GET_PROMOCODES_SUCCESS,
    payload: promocodes,
});
export const getPromocodesFailed = (error) => ({
    type: GET_PROMOCODES_FAILED,
    payload: error,
});
export const setPromocodesTotal = (total) => ({
    type: SET_PROMOCODES_TOTAL,
    payload: total,
});

export const getActivePromocodesStarted = () => ({
    type: GET_ACTIVE_PROMOCODES_STARTED
});
export const getActivePromocodesSuccess = (promocodes) => ({
    type: GET_ACTIVE_PROMOCODES_SUCCESS,
    payload: promocodes,
});
export const getActivePromocodesFailed = (error) => ({
    type: GET_ACTIVE_PROMOCODES_FAILED,
    payload: error,
});
export const setActivePromocodesTotal = (total) => ({
    type: SET_ACTIVE_PROMOCODES_TOTAL,
    payload: total,
});

export const createPromocodeStarted = () => ({
    type: POST_PROMOCODE_STARTED,
});
export const createPromocodeSuccess = (promocode) => ({
    type: POST_PROMOCODE_SUCCESS,
    payload: promocode,
});
export const createPromocodeFailed = (error) => ({
    type: POST_PROMOCODE_FAILED,
    payload: error,
});

export const updatePromocodeStarted = () => ({
    type: PUT_PROMOCODE_STARTED,
});
export const updatePromocodeSuccess = (promocode) => ({
    type: PUT_PROMOCODE_SUCCESS,
    payload: promocode,
});
export const updatePromocodeFailed = (error) => ({
    type: PUT_PROMOCODE_FAILED,
    payload: error,
});

export const deletePromocodeStarted = () => ({
    type: DELETE_PROMOCODE_STARTED,
});
export const deletePromocodeSuccess = (id) => ({
    type: DELETE_PROMOCODE_SUCCESS,
    payload: id,
});
export const deletePromocodeFailed = (error) => ({
    type: DELETE_PROMOCODE_FAILED,
    payload: error,
});
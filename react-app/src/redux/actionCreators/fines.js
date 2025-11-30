export const GET_FINES_SUCCESS = "GET_FINES_SUCCESS";
export const GET_FINES_FAILED = "GET_FINES_FAILED";
export const GET_FINES_STARTED = "GET_FINES_STARTED";
export const SET_FINES_TOTAL = "SET_FINES_TOTAL";

export const GET_FINE_BY_TRIP_STARTED = "GET_FINE_BY_TRIP_STARTED";
export const GET_FINE_BY_TRIP_SUCCESS = "GET_FINE_BY_TRIP_SUCCESS";
export const GET_FINE_BY_TRIP_FAILED = "GET_FINE_BY_TRIP_FAILED";

export const POST_FINE_STARTED = "POST_FINE_STARTED";
export const POST_FINE_SUCCESS = "POST_FINE_SUCCESS";
export const POST_FINE_FAILED = "POST_FINE_FAILED";

export const PUT_FINE_STARTED = "PUT_FINE_STARTED";
export const PUT_FINE_SUCCESS = "PUT_FINE_SUCCESS";
export const PUT_FINE_FAILED = "PUT_FINE_FAILED";

export const DELETE_FINE_STARTED = "DELETE_FINE_STARTED";
export const DELETE_FINE_SUCCESS = "DELETE_FINE_SUCCESS";
export const DELETE_FINE_FAILED = "DELETE_FINE_FAILED";

export const getFinesStarted = () => ({
    type: GET_FINES_STARTED
});
export const getFinesSuccess = (fines) => ({
    type: GET_FINES_SUCCESS,
    payload: fines,
});
export const getFinesFailed = (error) => ({
    type: GET_FINES_FAILED,
    payload: error,
});
export const setFinesTotal = (total) => ({
    type: SET_FINES_TOTAL,
    payload: total,
});

export const getFineByTripStarted = () => ({
    type: GET_FINE_BY_TRIP_STARTED
});
export const getFineByTripSuccess = (fines) => ({
    type: GET_FINE_BY_TRIP_SUCCESS,
    payload: fines,
});
export const getFineByTripFailed = (error) => ({
    type: GET_FINE_BY_TRIP_FAILED,
    payload: error,
});

export const createFineStarted = () => ({
    type: POST_FINE_STARTED,
});
export const createFineSuccess = (fine) => ({
    type: POST_FINE_SUCCESS,
    payload: fine,
});
export const createFineFailed = (error) => ({
    type: POST_FINE_FAILED,
    payload: error,
});

export const updateFineStarted = () => ({
    type: PUT_FINE_STARTED,
});
export const updateFineSuccess = (fine) => ({
    type: PUT_FINE_SUCCESS,
    payload: fine,
});
export const updateFineFailed = (error) => ({
    type: PUT_FINE_FAILED,
    payload: error,
});

export const deleteFineStarted = () => ({
    type: DELETE_FINE_STARTED,
});
export const deleteFineSuccess = (id) => ({
    type: DELETE_FINE_SUCCESS,
    payload: id,
});
export const deleteFineFailed = (error) => ({
    type: DELETE_FINE_FAILED,
    payload: error,
});
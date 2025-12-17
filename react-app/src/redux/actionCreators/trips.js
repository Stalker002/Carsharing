export const GET_TRIPS_SUCCESS = "GET_TRIPS_SUCCESS";
export const GET_TRIPS_FAILED = "GET_TRIPS_FAILED";
export const GET_TRIPS_STARTED = "GET_TRIPS_STARTED";
export const SET_TRIPS_TOTAL = "SET_TRIPS_TOTAL";

export const GET_MY_TRIPS_STARTED = "GET_MY_TRIPS_STARTED";
export const GET_MY_TRIPS_SUCCESS = "GET_MY_TRIPS_SUCCESS";
export const GET_MY_TRIPS_FAILED = "GET_MY_TRIPS_FAILED";
export const SET_MY_TRIPS_TOTAL = "SET_MY_TRIPS_TOTAL";

export const GET_INFO_TRIP_STARTED = "GET_INFO_TRIP_STARTED";
export const GET_INFO_TRIP_SUCCESS = "GET_INFO_TRIP_SUCCESS";
export const GET_INFO_TRIP_FAILED = "GET_INFO_TRIP_FAILED";

export const GET_ACTIVE_TRIP_STARTED = "GET_ACTIVE_TRIP_STARTED";
export const GET_ACTIVE_TRIP_SUCCESS = "GET_ACTIVE_TRIP_SUCCESS";
export const GET_ACTIVE_TRIP_FAILED = "GET_ACTIVE_TRIP_FAILED";

export const FINISH_TRIP_STARTED = "FINISH_TRIP_STARTED";
export const FINISH_TRIP_SUCCESS = "FINISH_TRIP_SUCCESS";
export const FINISH_TRIP_FAILED = "FINISH_TRIP_FAILED";

export const POST_TRIP_STARTED = "POST_TRIP_STARTED";
export const POST_TRIP_SUCCESS = "POST_TRIP_SUCCESS";
export const POST_TRIP_FAILED = "POST_TRIP_FAILED";

export const PUT_TRIP_STARTED = "PUT_TRIP_STARTED";
export const PUT_TRIP_SUCCESS = "PUT_TRIP_SUCCESS";
export const PUT_TRIP_FAILED = "PUT_TRIP_FAILED";

export const DELETE_TRIP_STARTED = "DELETE_TRIP_STARTED";
export const DELETE_TRIP_SUCCESS = "DELETE_TRIP_SUCCESS";
export const DELETE_TRIP_FAILED = "DELETE_TRIP_FAILED";

export const getTripsStarted = () => ({
    type: GET_TRIPS_STARTED
});
export const getTripsSuccess = (trips) => ({
    type: GET_TRIPS_SUCCESS,
    payload: trips,
});
export const getTripsFailed = (error) => ({
    type: GET_TRIPS_FAILED,
    payload: error,
});
export const setTripsTotal = (total) => ({
    type: SET_TRIPS_TOTAL,
    payload: total,
});

export const getMyTripsStarted = () => ({
    type: GET_MY_TRIPS_STARTED
});
export const getMyTripsSuccess = (trips) => ({
    type: GET_MY_TRIPS_SUCCESS,
    payload: trips,
});
export const getMyTripsFailed = (error) => ({
    type: GET_MY_TRIPS_FAILED,
    payload: error,
});
export const setMyTripsTotal = (total) => ({
    type: SET_MY_TRIPS_TOTAL,
    payload: total,
});

export const getInfoTripStarted = () => ({
    type: GET_INFO_TRIP_STARTED
});
export const getInfoTripSuccess = (trips) => ({
    type: GET_INFO_TRIP_SUCCESS,
    payload: trips,
});
export const getInfoTripFailed = (error) => ({
    type: GET_INFO_TRIP_FAILED,
    payload: error,
});

export const getActiveTripStarted = () => ({
    type: GET_ACTIVE_TRIP_STARTED
});
export const getActiveTripSuccess = (trips) => ({
    type: GET_ACTIVE_TRIP_SUCCESS,
    payload: trips,
});
export const getActiveTripFailed = (error) => ({
    type: GET_ACTIVE_TRIP_FAILED,
    payload: error,
});

export const finishTripStarted = () => ({
    type: FINISH_TRIP_STARTED
});
export const finishTripSuccess = (trips) => ({
    type: FINISH_TRIP_SUCCESS,
    payload: trips,
});
export const finishTripFailed = (error) => ({
    type: FINISH_TRIP_FAILED,
    payload: error,
});

export const createTripStarted = () => ({
    type: POST_TRIP_STARTED,
});
export const createTripSuccess = (trip) => ({
    type: POST_TRIP_SUCCESS,
    payload: trip,
});
export const createTripFailed = (error) => ({
    type: POST_TRIP_FAILED,
    payload: error,
});

export const updateTripStarted = () => ({
    type: PUT_TRIP_STARTED,
});
export const updateTripSuccess = (trip) => ({
    type: PUT_TRIP_SUCCESS,
    payload: trip,
});
export const updateTripFailed = (error) => ({
    type: PUT_TRIP_FAILED,
    payload: error,
});

export const deleteTripStarted = () => ({
    type: DELETE_TRIP_STARTED,
});
export const deleteTripSuccess = (id) => ({
    type: DELETE_TRIP_SUCCESS,
    payload: id,
});
export const deleteTripFailed = (error) => ({
    type: DELETE_TRIP_FAILED,
    payload: error,
});
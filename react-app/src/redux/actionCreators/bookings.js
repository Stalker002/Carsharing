export const GET_BOOKINGS_SUCCESS = "GET_BOOKINGS_SUCCESS";
export const GET_BOOKINGS_FAILED = "GET_BOOKINGS_FAILED";
export const GET_BOOKINGS_STARTED = "GET_BOOKINGS_STARTED";
export const SET_BOOKINGS_TOTAL = "SET_BOOKINGS_TOTAL";

export const GET_MY_BOOKINGS_STARTED = "GET_MY_BOOKINGS_STARTED";
export const GET_MY_BOOKINGS_SUCCESS = "GET_MY_BOOKINGS_SUCCESS";
export const GET_MY_BOOKINGS_FAILED = "GET_MY_BOOKINGS_FAILED";
export const SET_MY_BOOKINGS_TOTAL = "SET_MY_BOOKINGS_TOTAL";

export const GET_INFO_BOOKING_STARTED = "GET_INFO_BOOKING_STARTED";
export const GET_INFO_BOOKING_SUCCESS = "GET_INFO_BOOKING_SUCCESS";
export const GET_INFO_BOOKING_FAILED = "GET_INFO_BOOKING_FAILED";

export const POST_BOOKING_STARTED = "POST_BOOKING_STARTED";
export const POST_BOOKING_SUCCESS = "POST_BOOKING_SUCCESS";
export const POST_BOOKING_FAILED = "POST_BOOKING_FAILED";

export const PUT_BOOKING_STARTED = "PUT_BOOKING_STARTED";
export const PUT_BOOKING_SUCCESS = "PUT_BOOKING_SUCCESS";
export const PUT_BOOKING_FAILED = "PUT_BOOKING_FAILED";

export const DELETE_BOOKING_STARTED = "DELETE_BOOKING_STARTED";
export const DELETE_BOOKING_SUCCESS = "DELETE_BOOKING_SUCCESS";
export const DELETE_BOOKING_FAILED = "DELETE_BOOKING_FAILED";

export const getBookingsStarted = () => ({
    type: GET_BOOKINGS_STARTED
});
export const getBookingsSuccess = (bookings) => ({
    type: GET_BOOKINGS_SUCCESS,
    payload: bookings,
});
export const getBookingsFailed = (error) => ({
    type: GET_BOOKINGS_FAILED,
    payload: error,
});
export const setBookingsTotal = (total) => ({
    type: SET_BOOKINGS_TOTAL,
    payload: total,
});

export const getMyBookingsStarted = () => ({
    type: GET_MY_BOOKINGS_STARTED
});
export const getMyBookingsSuccess = (bookings) => ({
    type: GET_MY_BOOKINGS_SUCCESS,
    payload: bookings,
});
export const getMyBookingsFailed = (error) => ({
    type: GET_MY_BOOKINGS_FAILED,
    payload: error,
});
export const setMyBookingsTotal = (total) => ({
    type: SET_MY_BOOKINGS_TOTAL,
    payload: total,
});

export const getInfoBookingStarted = () => ({
    type: GET_INFO_BOOKING_STARTED
});
export const getInfoBookingSuccess = (bookings) => ({
    type: GET_INFO_BOOKING_SUCCESS,
    payload: bookings,
});
export const getInfoBookingFailed = (error) => ({
    type: GET_INFO_BOOKING_FAILED,
    payload: error,
});

export const createBookingStarted = () => ({
    type: POST_BOOKING_STARTED,
});
export const createBookingSuccess = (booking) => ({
    type: POST_BOOKING_SUCCESS,
    payload: booking,
});
export const createBookingFailed = (error) => ({
    type: POST_BOOKING_FAILED,
    payload: error,
});

export const updateBookingStarted = () => ({
    type: PUT_BOOKING_STARTED,
});
export const updateBookingSuccess = (booking) => ({
    type: PUT_BOOKING_SUCCESS,
    payload: booking,
});
export const updateBookingFailed = (error) => ({
    type: PUT_BOOKING_FAILED,
    payload: error,
});

export const deleteBookingStarted = () => ({
    type: DELETE_BOOKING_STARTED,
});
export const deleteBookingSuccess = (id) => ({
    type: DELETE_BOOKING_SUCCESS,
    payload: id,
});
export const deleteBookingFailed = (error) => ({
    type: DELETE_BOOKING_FAILED,
    payload: error,
});
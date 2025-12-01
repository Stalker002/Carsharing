import { api } from "../../api";
import { createBookingFailed, createBookingStarted, createBookingSuccess, deleteBookingFailed, deleteBookingStarted, deleteBookingSuccess, getBookingsFailed, getBookingsStarted, getBookingsSuccess, getInfoBookingFailed, getInfoBookingStarted, getInfoBookingSuccess, getMyBookingsFailed, getMyBookingsStarted, getMyBookingsSuccess, setBookingsTotal, setMyBookingsTotal, updateBookingFailed, updateBookingStarted, updateBookingSuccess } from "../actionCreators/bookings";

export const getBookings = (page = 1) => {
    return async (dispatch) => {
        try {
            dispatch(getBookingsStarted());

            const response = await api.bookings.getBookings({
                params: {
                    _page: page,
                    _limit: 25,
                },
            });

            const totalCount = parseInt(response.headers["x-total-count"], 10);
            if (!isNaN(totalCount)) {
                dispatch(setBookingsTotal(totalCount));
            }

            dispatch(getBookingsSuccess({
                data: response.data,
                page,
            }));
        } 
        catch (error) {
            dispatch(getBookingsFailed(error));
        }
    };
};

export const getMyBookings = (page = 1) => {
    return async (dispatch) => {
        try {
            dispatch(getMyBookingsStarted())

            const response = await api.bookings.getMyBookings({
                params: {
                    _page: page,
                    _limit: 25,
                },
            });

            const totalCount = parseInt(response.headers["x-total-count"], 10);
            if (!isNaN(totalCount)) {
                dispatch(setMyBookingsTotal(totalCount));
            }

            dispatch(getMyBookingsSuccess({
                data: response.data,
                page,
            }));
        } 
        catch (error) {
            dispatch(getMyBookingsFailed(error));
        }
    };
};

export const getInfoBookings = (id) => {
    return async (dispatch) => {
        try {
            dispatch(getInfoBookingStarted())

            const response = await api.bookings.getBookingInfo(id);

            dispatch(getInfoBookingSuccess(response.data));
        } 
        catch (error) {
            dispatch(getInfoBookingFailed(error));
        }
    };
};

export const createBooking = (data) => {
    return async (dispatch) => {
        try {
            dispatch(createBookingStarted());

            const response = await api.bookings.createBooking(data);

            dispatch(createBookingSuccess(response.data));
        } 
        catch (error) {
            dispatch(createBookingFailed(error));
        }
    };
};

export const updateBooking = (id, data) => {
  return async (dispatch) => {
    try {
      dispatch(updateBookingStarted());

      const response = await api.bookings.updateBooking(id, data);

      dispatch(updateBookingSuccess(response.data));
    } 
    catch (error) {
      dispatch(updateBookingFailed(error));
    }
  };
};

export const deleteBooking = (id) => {
  return async (dispatch) => {
    try {
      dispatch(deleteBookingStarted());

      const response = await api.bookings.deleteBooking(id);

      dispatch(deleteBookingSuccess(response.data));
    } 
    catch (error) {
      dispatch(deleteBookingFailed(error));
    }
  };
};
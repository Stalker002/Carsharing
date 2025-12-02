import { GET_BOOKINGS_STARTED, GET_BOOKINGS_SUCCESS, GET_BOOKINGS_FAILED, POST_BOOKING_STARTED, POST_BOOKING_SUCCESS, POST_BOOKING_FAILED, PUT_BOOKING_STARTED, PUT_BOOKING_SUCCESS, PUT_BOOKING_FAILED, DELETE_BOOKING_STARTED, DELETE_BOOKING_SUCCESS, DELETE_BOOKING_FAILED, SET_BOOKINGS_TOTAL, GET_MY_BOOKINGS_STARTED, GET_MY_BOOKINGS_SUCCESS, GET_MY_BOOKINGS_FAILED, SET_MY_BOOKINGS_TOTAL, GET_INFO_BOOKING_STARTED, GET_INFO_BOOKING_SUCCESS, GET_INFO_BOOKING_FAILED } from "../actionCreators/bookings";

const initialState = {
    bookings: [],
    myBookings: [],
    infoBooking: {},
    isBookingsLoading: false,
    isCreateBookingLoading: false,
    isUpdateBookingLoading: false,
    isDeleteBookingLoading: false,
};

export const bookingsReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_BOOKINGS_STARTED:
            return {
                ...state,
                isBookingsLoading: true,
            };
        case GET_BOOKINGS_SUCCESS:
            return {
                ...state,
                bookings: action.payload.page === 1
                    ? action.payload.data
                    : [...state.bills, ...action.payload.data],
                isBookingsLoading: false,
            };
        case GET_BOOKINGS_FAILED:
            return {
                ...state,
                isBookingsLoading: false,
            };
        case SET_BOOKINGS_TOTAL:
            return {
                ...state,
                billsTotal: action.payload,
            };
            
        case GET_MY_BOOKINGS_STARTED:
            return {
                ...state,
                isBookingsLoading: true,
            };
        case GET_MY_BOOKINGS_SUCCESS:
            return {
                ...state,
                myBookings: action.payload.page === 1
                    ? action.payload.data
                    : [...state.myBookings, ...action.payload.data],
                isBookingsLoading: false,
            };
        case GET_MY_BOOKINGS_FAILED:
            return {
                ...state,
                isBookingsLoading: false,
            };
        case SET_MY_BOOKINGS_TOTAL:
            return {
                ...state,
                myBookingsTotal: action.payload,
            };

        case GET_INFO_BOOKING_STARTED:
            return {
                ...state,
                isBookingsLoading: true,
            };
        case GET_INFO_BOOKING_SUCCESS:
            return {
                ...state,
                isBookingsLoading: false,
                infoBookings: action.payload,
            };
        case GET_INFO_BOOKING_FAILED:
            return {
                ...state,
                isBookingsLoading: action.payload,
            };

        case POST_BOOKING_STARTED:
            return {
                ...state,
                isCreateBookingLoading: true
            };
        case POST_BOOKING_SUCCESS:
            return {
                ...state,
                bookings: [...state.bookings, action.payload],
                isCreateBookingLoading: false,
            };
        case POST_BOOKING_FAILED:
            return {
                ...state,
                isCreateBookingLoading: false
            };

        case PUT_BOOKING_STARTED:
            return {
                ...state,
                isUpdateBookingLoading: true
            };
        case PUT_BOOKING_SUCCESS:
            return {
                ...state,
                bookings: state.bookings.map(b =>
                    b.id === action.payload.id ? action.payload : b
                ),
                isUpdateBookingLoading: false,
            };
        case PUT_BOOKING_FAILED:
            return {
                ...state,
                isUpdateBookingLoading: false
            };

        case DELETE_BOOKING_STARTED:
            return {
                ...state,
                isDeleteBookingLoading: true
            };
        case DELETE_BOOKING_SUCCESS:
            return {
                ...state,
                bookings: state.bookings.filter(b => b.id !== action.payload),
                isDeleteBookingLoading: false,
            };
        case DELETE_BOOKING_FAILED:
            return {
                ...state,
                isDeleteBookingLoading: false
            };

        default:
            return state;
    }
};

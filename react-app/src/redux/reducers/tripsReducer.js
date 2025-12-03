import { GET_TRIPS_STARTED, GET_TRIPS_SUCCESS, GET_TRIPS_FAILED, POST_TRIP_STARTED, POST_TRIP_SUCCESS, POST_TRIP_FAILED, PUT_TRIP_STARTED, PUT_TRIP_SUCCESS, PUT_TRIP_FAILED, DELETE_TRIP_STARTED, DELETE_TRIP_SUCCESS, DELETE_TRIP_FAILED, GET_MY_TRIPS_STARTED, GET_MY_TRIPS_SUCCESS, GET_MY_TRIPS_FAILED, GET_INFO_TRIP_STARTED, GET_INFO_TRIP_SUCCESS, GET_INFO_TRIP_FAILED, SET_TRIPS_TOTAL, SET_MY_TRIPS_TOTAL } from "../actionCreators/trips";

const initialState = {
    trips: [],
    myTrips: [],
    infoTrip: {},
    isTripLoading: false,
    isTripCreateLoading: false,
    isTripUpdateLoading: false,
    isTripDeleteLoading: false,
    tripsTotal: 0,
    myTripsTotal: 0,
};

export const tripsReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_TRIPS_STARTED:
            return {
                ...state,
                isTripLoading: true,
            };
        case GET_TRIPS_SUCCESS:
            return {
                ...state,
                trips: action.payload.page === 1
                    ? action.payload.data
                    : [...state.trips, ...action.payload.data],
                isTripLoading: false,
            };
        case GET_TRIPS_FAILED:
            return {
                ...state,
                isTripLoading: false,
            };
        case SET_TRIPS_TOTAL:
            return {
                ...state,
                tripsTotal: action.payload,
            };

        case GET_MY_TRIPS_STARTED:
            return {
                ...state,
                isTripLoading: true,
            };
        case GET_MY_TRIPS_SUCCESS:
            return {
                ...state,
                my: action.payload.page === 1
                    ? action.payload.data
                    : [...state.myTrips, ...action.payload.data],
                isTripLoading: false,
            };
        case GET_MY_TRIPS_FAILED:
            return {
                ...state,
                isTripLoading: false,
            };
        case SET_MY_TRIPS_TOTAL:
            return {
                ...state,
                myTotal: action.payload,
            };

        case GET_INFO_TRIP_STARTED:
            return {
                ...state,
                isTripLoading: true,
            };
        case GET_INFO_TRIP_SUCCESS:
            return {
                ...state,
                isTripLoading: false,
                infoTrip: action.payload,
            };
        case GET_INFO_TRIP_FAILED:
            return {
                ...state,
                isTripLoading: action.payload,
            };

        case POST_TRIP_STARTED:
            return {
                ...state,
                isTripCreateLoading: true
            };
        case POST_TRIP_SUCCESS:
            return {
                ...state,
                trips: [...state.trips, action.payload], 
                isTripCreateLoading: false
            };
        case POST_TRIP_FAILED:
            return {
                ...state,
                isTripCreateLoading: false
            };

        case PUT_TRIP_STARTED:
            return {
                ...state,
                isTripUpdateLoading: true
            };
        case PUT_TRIP_SUCCESS:
            return {
                ...state,
                trips: state.trips.map(t => t.id === action.payload.id ? action.payload : t),
                isTripUpdateLoading: false
            };
        case PUT_TRIP_FAILED:
            return {
                ...state,
                isTripUpdateLoading: false
            };

        case DELETE_TRIP_STARTED:
            return {
                ...state,
                isTripDeleteLoading: true
            };
        case DELETE_TRIP_SUCCESS:
            return {
                ...state,
                trips: state.trips.filter(t => t.id !== action.payload),
                isTripDeleteLoading: false
            };
        case DELETE_TRIP_FAILED:
            return {
                ...state,
                isTripDeleteLoading: false
            };

        default:
            return state;
    }
};

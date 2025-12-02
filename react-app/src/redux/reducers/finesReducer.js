import { GET_FINES_STARTED, GET_FINES_SUCCESS, GET_FINES_FAILED, SET_FINES_TOTAL, GET_FINE_BY_TRIP_STARTED, GET_FINE_BY_TRIP_SUCCESS, GET_FINE_BY_TRIP_FAILED, POST_FINE_STARTED, POST_FINE_SUCCESS, POST_FINE_FAILED, PUT_FINE_STARTED, PUT_FINE_SUCCESS, PUT_FINE_FAILED, DELETE_FINE_STARTED, DELETE_FINE_SUCCESS, DELETE_FINE_FAILED } from "../actionCreators/fines";

const initialState = {
    fines: [],
    finesTotal: 0,
    fineByTrip: null,
    isFinesLoading: true,
    isCreateFineLoading: false,
    isUpdateFineLoading: false,
    isDeleteFineLoading: false,
};

export const finesReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_FINES_STARTED:
            return {
                ...state,
                isFinesLoading: true,
            };
        case GET_FINES_SUCCESS:
            return {
                ...state,
                fines: action.payload.page === 1
                    ? action.payload.data
                    : [...state.fines, ...action.payload.data],
                isFinesLoading: false,
            };
        case GET_FINES_FAILED:
            return {
                ...state,
                isFinesLoading: false,
            };
        case SET_FINES_TOTAL:
            return {
                ...state,
                finesTotal: action.payload,
            };

        case GET_FINE_BY_TRIP_STARTED:
            return {
                ...state,
                isFineLoading: true,
            };
        case GET_FINE_BY_TRIP_SUCCESS:
            return {
                ...state,
                fineByTrip: action.payload,
                isFineLoading: false,
            };
        case GET_FINE_BY_TRIP_FAILED:
            return {
                ...state,
                isFineLoading: false,
            };

        case POST_FINE_STARTED:
            return {
                ...state,
                isCreateFineLoading: true,
            };
        case POST_FINE_SUCCESS:
            return {
                ...state,
                fines: [...state.fines, action.payload],
                isCreateFineLoading: false,
            };
        case POST_FINE_FAILED:
            return {
                ...state,
                isCreateFineLoading: false,
            };

        case PUT_FINE_STARTED:
            return {
                ...state,
                isUpdateFineLoading: true,
            };
        case PUT_FINE_SUCCESS:
            return {
                ...state,
                fines: state.fines.map(f =>
                    f.id === action.payload.id ? action.payload : f),
                isUpdateFineLoading: false,
            };
        case PUT_FINE_FAILED:
            return {
                ...state,
                isUpdateFineLoading: false,
            };

        case DELETE_FINE_STARTED:
            return {
                ...state,
                isDeleteFineLoading: true,
            };
        case DELETE_FINE_SUCCESS:
            return {
                ...state,
                fines: state.fines.filter(f => f.id !== action.payload),
                isDeleteFineLoading: false,
            };
        case DELETE_FINE_FAILED:
            return {
                ...state,
                isDeleteFineLoading: false,
            };

        default:
            return state;
    }
};

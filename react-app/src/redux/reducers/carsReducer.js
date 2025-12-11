import { GET_CARS_SUCCESS, GET_CARS_FAILED, GET_CARS_STARTED, POST_CAR_STARTED, POST_CAR_SUCCESS, POST_CAR_FAILED, PUT_CAR_STARTED, PUT_CAR_SUCCESS, PUT_CAR_FAILED, DELETE_CAR_STARTED, DELETE_CAR_SUCCESS, DELETE_CAR_FAILED, SET_CARS_TOTAL, SET_CARS_BY_CATEGORY_TOTAL, GET_CARS_BY_CATEGORY_FAILED, GET_CARS_BY_CATEGORY_SUCCESS, GET_CARS_BY_CATEGORY_STARTED, GET_INFO_CAR_STARTED, GET_INFO_CAR_SUCCESS, GET_INFO_CAR_FAILED, GET_INFO_CAR_ADMIN_STARTED, GET_INFO_CAR_ADMIN_SUCCESS, GET_INFO_CAR_ADMIN_FAILED } from "../actionCreators/cars";

const initialState = {
    cars: [],
    carsByCategory: [],
    infoCar: {},
    infoCarAdmin: {},
    isCarsLoading: false,
    isCreateCarLoading: false,
    isUpdateCarLoading: false,
    isDeleteCarLoading: false,
    carsTotal: 0,
};

export const carsReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_CARS_STARTED:
            return {
                ...state,
                isCarsLoading: true,
            };
        case GET_CARS_SUCCESS:
            return {
                ...state,
                cars: action.payload.page === 1
                    ? action.payload.data
                    : [...state.cars, ...action.payload.data],
                isCarsLoading: false,
            };
        case GET_CARS_FAILED:
            return {
                ...state,
                isCarsLoading: false,
            };
        case SET_CARS_TOTAL:
            return {
                ...state,
                carsTotal: action.payload,
            };

        case GET_CARS_BY_CATEGORY_STARTED:
            return {
                ...state,
                isCarsLoading: true,
            };
        case GET_CARS_BY_CATEGORY_SUCCESS:
            return {
                ...state,
                carsByCategory: action.payload.page === 1
                    ? action.payload.data
                    : [...state.carsByCategory, ...action.payload.data],
                isCarsLoading: false,
            };
        case GET_CARS_BY_CATEGORY_FAILED:
            return {
                ...state,
                isCarsLoading: false,
            };
        case SET_CARS_BY_CATEGORY_TOTAL:
            return {
                ...state,
                carsByCategoryTotal: action.payload,
            };

        case GET_INFO_CAR_STARTED:
            return {
                ...state,
                isCarsLoading: true,
            };
        case GET_INFO_CAR_SUCCESS:
            return {
                ...state,
                isCarsLoading: false,
                infoCar: action.payload,
            };
        case GET_INFO_CAR_FAILED:
            return {
                ...state,
                iscarsLoading: false,
            };

        case GET_INFO_CAR_ADMIN_STARTED:
            return {
                ...state,
                isCarsLoading: true,
            };
        case GET_INFO_CAR_ADMIN_SUCCESS:
            return {
                ...state,
                isCarsLoading: false,
                infoCarAdmin: action.payload,
            };
        case GET_INFO_CAR_ADMIN_FAILED:
            return {
                ...state,
                isCarsLoading: false,
            };

        case POST_CAR_STARTED:
            return { 
                ...state, 
                isCreateCarLoading: true 
            };
        case POST_CAR_SUCCESS:
            return {
                ...state,
                cars: [...state.cars, action.payload],
                isCreateCarLoading: false,
            };
        case POST_CAR_FAILED:
            return { 
                ...state, 
                isCreateCarLoading: false
             };

        case PUT_CAR_STARTED:
            return { 
                ...state, 
                isUpdateCarLoading: true 
            };
        case PUT_CAR_SUCCESS:
            return {
                ...state,
                cars: state.cars.map(c =>
                    c.id === action.payload.id ? action.payload : c
                ),
                isUpdateCarLoading: false,
            };
        case PUT_CAR_FAILED:
            return { 
                ...state, 
                isUpdateCarLoading: false
             };

        case DELETE_CAR_STARTED:
            return { 
                ...state, 
                isDeleteCarLoading: true 
            };
        case DELETE_CAR_SUCCESS:
            return {
                ...state,
                cars: state.cars.filter(c => c.id !== action.payload),
                isDeleteCarLoading: false,
            };
        case DELETE_CAR_FAILED:
            return { 
                ...state, 
                isDeleteCarLoading: false
             };

        default:
            return state;
    }
};

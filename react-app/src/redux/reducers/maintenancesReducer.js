import { GET_MAINTENANCES_STARTED, GET_MAINTENANCES_SUCCESS, GET_MAINTENANCES_FAILED, POST_MAINTENANCE_STARTED, POST_MAINTENANCE_SUCCESS, POST_MAINTENANCE_FAILED, PUT_MAINTENANCE_STARTED, PUT_MAINTENANCE_SUCCESS, PUT_MAINTENANCE_FAILED, DELETE_MAINTENANCE_STARTED, DELETE_MAINTENANCE_SUCCESS, DELETE_MAINTENANCE_FAILED, GET_MAINTENANCES_BY_CAR_STARTED, GET_MAINTENANCES_BY_CAR_SUCCESS, GET_MAINTENANCES_BY_CAR_FAILED, GET_MAINTENANCE_BY_DATE_RANGE_STARTED, GET_MAINTENANCE_BY_DATE_RANGE_SUCCESS, GET_MAINTENANCE_BY_DATE_RANGE_FAILED } from "../actionCreators/maintenance";

const initialState = {
    maintenances: [],
    maintenanceByCarList: [],
    maintenanceByDateRangeList: [],
    isMaintenanceLoading: false,
    isMaintenanceCreateLoading: false,
    isMaintenanceUpdateLoading: false,
    isMaintenanceDeleteLoading: false
};

export const maintenancesReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_MAINTENANCES_STARTED:
            return {
                ...state,
                isMaintenanceLoading: true
            };
        case GET_MAINTENANCES_SUCCESS:
            return {
                ...state,
                maintenances: action.payload, 
                isMaintenanceLoading: false
            };
        case GET_MAINTENANCES_FAILED:
            return {
                ...state,
                isMaintenanceLoading: false
            };

        case GET_MAINTENANCES_BY_CAR_STARTED:
            return {
                ...state,
                isMaintenanceLoading: true
            };
        case GET_MAINTENANCES_BY_CAR_SUCCESS:
            return {
                ...state,
                maintenanceByCarList: action.payload, 
                isMaintenanceLoading: false
            };
        case GET_MAINTENANCES_BY_CAR_FAILED:
            return {
                ...state,
                isMaintenanceLoading: false
            };

        case GET_MAINTENANCE_BY_DATE_RANGE_STARTED:
            return {
                ...state,
                isMaintenanceLoading: true
            };
        case GET_MAINTENANCE_BY_DATE_RANGE_SUCCESS:
            return {
                ...state,
                maintenanceByDateRangeList: action.payload, 
                isMaintenanceLoading: false
            };
        case GET_MAINTENANCE_BY_DATE_RANGE_FAILED:
            return {
                ...state,
                isMaintenanceLoading: false
            };

        case POST_MAINTENANCE_STARTED:
            return {
                ...state,
                isMaintenanceCreateLoading: true
            };
        case POST_MAINTENANCE_SUCCESS:
            return {
                ...state,
                maintenanceList: [...state.maintenanceList, action.payload],
                isMaintenanceCreateLoading: false
            };
        case POST_MAINTENANCE_FAILED:
            return {
                ...state,
                isMaintenanceCreateLoading: false
            };

        case PUT_MAINTENANCE_STARTED:
            return {
                ...state,
                isMaintenanceUpdateLoading: true
            };
        case PUT_MAINTENANCE_SUCCESS:
            return {
                ...state,
                maintenanceList: state.maintenanceList.map(m =>
                    m.id === action.payload.id ? action.payload : m
                ),
                isMaintenanceUpdateLoading: false
            };
        case PUT_MAINTENANCE_FAILED:
            return {
                ...state,
                isMaintenanceUpdateLoading: false
            };

        case DELETE_MAINTENANCE_STARTED:
            return {
                ...state,
                isMaintenanceDeleteLoading: true
            };
        case DELETE_MAINTENANCE_SUCCESS:
            return {
                ...state,
                maintenanceList: state.maintenanceList.filter(m => m.id !== action.payload),
                isMaintenanceDeleteLoading: false
            };
        case DELETE_MAINTENANCE_FAILED:
            return {
                ...state,
                isMaintenanceDeleteLoading: false
            };

        default:
            return state;
    }
};

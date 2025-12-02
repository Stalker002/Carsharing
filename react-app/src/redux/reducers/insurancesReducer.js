import { GET_INSURANCES_STARTED, GET_INSURANCES_SUCCESS, GET_INSURANCES_FAILED, POST_INSURANCE_STARTED, POST_INSURANCE_SUCCESS, POST_INSURANCE_FAILED, DELETE_INSURANCE_STARTED, DELETE_INSURANCE_SUCCESS, DELETE_INSURANCE_FAILED, GET_INSURANCES_BY_CAR_STARTED, GET_INSURANCES_BY_CAR_SUCCESS, GET_INSURANCES_BY_CAR_FAILED } from "../actionCreators/insurances";

const initialState = {
    insurances: [],
    insurancesByCar: [],
    isInsurancesLoading: false,
    isInsurancesCreateLoading: false,
    isInsurancesUpdateBillLoading: false,
    isInsurancesDeleteLoading: false
};

export const insurancesReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_INSURANCES_STARTED:
            return {
                ...state,
                isInsurancesLoading: true
            };
        case GET_INSURANCES_SUCCESS:
            return {
                ...state,
                insurances: action.payload,
                isInsurancesLoading: false
            };
        case GET_INSURANCES_FAILED:
            return {
                ...state,
                isInsurancesLoading: false
            };

        case GET_INSURANCES_BY_CAR_STARTED:
            return {
                ...state,
                isInsurancesLoading: true
            };
        case GET_INSURANCES_BY_CAR_SUCCESS:
            return {
                ...state,
                insurancesByCar: action.payload,
                isInsurancesLoading: false
            };
        case GET_INSURANCES_BY_CAR_FAILED:
            return {
                ...state,
                isInsurancesLoading: false
            };

        case POST_INSURANCE_STARTED:
            return {
                ...state,
                isInsurancesCreateLoading: true
            };
        case POST_INSURANCE_SUCCESS:
            return {
                ...state,
                insurances: [...state.insurances, action.payload],
                isInsurancesCreateLoading: false
            };
        case POST_INSURANCE_FAILED:
            return {
                ...state,
                isInsurancesCreateLoading: false
            };

        case PUT_BILL_STARTED:
            return {
                ...state,
                isInsurancesUpdateBillLoading: true
            };
        case PUT_BILL_SUCCESS:
            return {
                ...state,
                bills: state.bills.map(b => b.id === action.payload.id ? action.payload : b),
                isInsurancesUpdateBillLoading: false
            };
        case PUT_BILL_FAILED:
            return {
                ...state,
                isInsurancesUpdateBillLoading: false
            };

        case DELETE_INSURANCE_STARTED:
            return {
                ...state,
                isInsurancesDeleteLoading: true
            };
        case DELETE_INSURANCE_SUCCESS:
            return {
                ...state,
                insurances: state.insurances.filter(i => i.id !== action.payload),
                isInsurancesDeleteLoading: false
            };
        case DELETE_INSURANCE_FAILED:
            return {
                ...state,
                isInsurancesDeleteLoading: false
            };

        default:
            return state;
    }
};

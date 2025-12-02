import { GET_PAYMENTS_STARTED, GET_PAYMENTS_SUCCESS, GET_PAYMENTS_FAILED, POST_PAYMENT_STARTED, POST_PAYMENT_SUCCESS, POST_PAYMENT_FAILED, SET_PAYMENTS_TOTAL, GET_PAYMENTS_BY_BILL_STARTED, GET_PAYMENTS_BY_BILL_SUCCESS, GET_PAYMENTS_BY_BILL_FAILED, PUT_PAYMENT_STARTED, PUT_PAYMENT_SUCCESS, PUT_PAYMENT_FAILED, DELETE_PAYMENT_STARTED, DELETE_PAYMENT_SUCCESS, DELETE_PAYMENT_FAILED } from "../actionCreators/payments";

const initialState = {
    payments: [],
    paymentsByBill: [],
    isPaymentsLoading: false,
    isPaymentsCreateLoading: false,
    isPaymentsUpdateBillLoading: false,
    isPaymentsDeleteBillLoading: false,
    paymentsTotal: 0
};

export const paymentsReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_PAYMENTS_STARTED:
            return {
                ...state,
                isPaymentsLoading: true,
            };
        case GET_PAYMENTS_SUCCESS:
            return {
                ...state,
                payments: action.payload.page === 1
                    ? action.payload.data
                    : [...state.payments, ...action.payload.data],
                isPaymentsLoading: false,
            };
        case GET_PAYMENTS_FAILED:
            return {
                ...state,
                isPaymentsLoading: false,
            };
        case SET_PAYMENTS_TOTAL:
            return {
                ...state,
                paymentsTotal: action.payload,
            };

        case GET_PAYMENTS_BY_BILL_STARTED:
            return { 
                ...state, 
                isPaymentsLoading: true 
            };
        case GET_PAYMENTS_BY_BILL_SUCCESS:
            return { 
                ...state, 
                paymentsByBill: action.payload, 
                isPaymentsLoading: false 
            };
        case GET_PAYMENTS_BY_BILL_FAILED:
            return { 
                ...state, 
                isPaymentsLoading: false 
            };

        case POST_PAYMENT_STARTED:
            return { 
                ...state, 
                isPaymentsCreateLoading: true 
            };
        case POST_PAYMENT_SUCCESS:
            return { 
                ...state, 
                payments: [...state.payments, action.payload], 
                isPaymentsCreateLoading: false 
            };
        case POST_PAYMENT_FAILED:
            return { 
                ...state, 
                isPaymentsCreateLoading: false 
            };

        case PUT_PAYMENT_STARTED:
            return {
                ...state,
                isPaymentsUpdateBillLoading: true
            };
        case PUT_PAYMENT_SUCCESS:
            return {
                ...state,
                payments: state.payments.map(b => b.id === action.payload.id ? action.payload : b),
                isPaymentsUpdateBillLoading: false
            };
        case PUT_PAYMENT_FAILED:
            return {
                ...state,
                isPaymentsUpdateBillLoading: false
            };

        case DELETE_PAYMENT_STARTED:
            return {
                ...state,
                isPaymentsDeleteBillLoading: true
            };
        case DELETE_PAYMENT_SUCCESS:
            return {
                ...state,
                payments: state.payments.filter(b => b.id !== action.payload),
                isPaymentsDeleteBillLoading: false
            };
        case DELETE_PAYMENT_FAILED:
            return {
                ...state,
                isPaymentsDeleteBillLoading: false
            };
        default:
            return state;
    }
};

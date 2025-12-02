import { DELETE_BILL_FAILED, DELETE_BILL_STARTED, DELETE_BILL_SUCCESS, GET_BILLS_FAILED, GET_BILLS_STARTED, GET_BILLS_SUCCESS, GET_INFO_BILL_FAILED, GET_INFO_BILL_STARTED, GET_INFO_BILL_SUCCESS, GET_MY_BILLS_FAILED, GET_MY_BILLS_STARTED, GET_MY_BILLS_SUCCESS, POST_BILL_FAILED, POST_BILL_STARTED, POST_BILL_SUCCESS, PUT_BILL_FAILED, PUT_BILL_STARTED, PUT_BILL_SUCCESS, SET_BILLS_TOTAL, SET_MY_BILLS_TOTAL } from "../actionCreators/bills";

const initialeState = {
    bills: [],
    myBills: [],
    infoBill: {},
    isBillsLoading: true,
    isCreateLoading: false,
    isUpdateBillLoading: false,
    isDeleteBillLoading: false,
    billsTotal: 0,
    myBillsTotal: 0,
}

export const billsReducer = (state = initialeState, action) => {
    switch (action.type) {
        case GET_BILLS_STARTED:
            return {
                ...state,
                isBillsLoading: true,
            };
        case GET_BILLS_SUCCESS:
            return {
                ...state,
                bills: action.payload.page === 1
                    ? action.payload.data
                    : [...state.bills, ...action.payload.data],
                isBillsLoading: false,
            };
        case GET_BILLS_FAILED:
            return {
                ...state,
                isBillsLoading: false,
            };
        case SET_BILLS_TOTAL:
            return {
                ...state,
                billsTotal: action.payload,
            };

        case GET_MY_BILLS_STARTED:
            return {
                ...state,
                isBillsLoading: true,
            };
        case GET_MY_BILLS_SUCCESS:
            return {
                ...state,
                myBills: action.payload.page === 1
                    ? action.payload.data
                    : [...state.myBills, ...action.payload.data],
                isBillsLoading: false,
            };
        case GET_MY_BILLS_FAILED:
            return {
                ...state,
                isBillsLoading: false,
            };
        case SET_MY_BILLS_TOTAL:
            return {
                ...state,
                myBillsTotal: action.payload,
            };
            
        case GET_INFO_BILL_STARTED:
            return {
                ...state,
                isBillsLoading: true,
            };
        case GET_INFO_BILL_SUCCESS:
            return {
                ...state,
                isBillsLoading: false,
                infoBill: action.payload,
            };
        case GET_INFO_BILL_FAILED:
            return {
                ...state,
                isBillsLoading: action.payload,
            };

        case POST_BILL_STARTED:
            return {
                ...state,
                isBillsLoading: true,
            };
        case POST_BILL_SUCCESS:
            return {
                ...state,
                bills: [action.payload, ...state.bills],
                isBillsLoading: false,
            };
        case POST_BILL_FAILED:
            return {
                ...state,
                isBillsLoading: false,
            };

        case PUT_BILL_STARTED:
            return {
                ...state,
                isUpdateBillLoading: true
            };
        case PUT_BILL_SUCCESS:
            return {
                ...state,
                bills: state.bills.map(b => b.id === action.payload.id ? action.payload : b),
                isUpdateBillLoading: false
            };
        case PUT_BILL_FAILED:
            return {
                ...state,
                isUpdateBillLoading: false
            };

        case DELETE_BILL_STARTED:
            return {
                ...state,
                isDeleteBillLoading: true
            };
        case DELETE_BILL_SUCCESS:
            return {
                ...state,
                bills: state.bills.filter(b => b.id !== action.payload),
                isDeleteBillLoading: false
            };
        case DELETE_BILL_FAILED:
            return {
                ...state,
                isDeleteBillLoading: false
            };
        default:
            return state;
    }
}
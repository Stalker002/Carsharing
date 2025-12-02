import { GET_PROMOCODES_STARTED, GET_PROMOCODES_SUCCESS, GET_PROMOCODES_FAILED, POST_PROMOCODE_STARTED, POST_PROMOCODE_SUCCESS, POST_PROMOCODE_FAILED, DELETE_PROMOCODE_STARTED, DELETE_PROMOCODE_SUCCESS, DELETE_PROMOCODE_FAILED, GET_ACTIVE_PROMOCODES_STARTED, GET_ACTIVE_PROMOCODES_SUCCESS, GET_ACTIVE_PROMOCODES_FAILED, SET_ACTIVE_PROMOCODES_TOTAL, PUT_PROMOCODE_STARTED, PUT_PROMOCODE_SUCCESS, PUT_PROMOCODE_FAILED } from "../actionCreators/promocodes";

const initialState = {
    promocodes: [],
    activePromocodes: [],
    isPromocodesLoading: false,
    isPromocodesCreateLoading: false,
    isPromocodesDeleteLoading: false,
    promocodesTotal: 0,
    activePromocodesTotal: 0
};

export const promocodesReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_PROMOCODES_STARTED:
            return {
                ...state,
                isPromocodesLoading: true,
            };
        case GET_PROMOCODES_SUCCESS:
            return {
                ...state,
                promocodes: action.payload.page === 1
                    ? action.payload.data
                    : [...state.promocodes, ...action.payload.data],
                isPromocodesLoading: false,
            };
        case GET_PROMOCODES_FAILED:
            return {
                ...state,
                isPromocodesLoading: false,
            };
        case SET_PROMOCODES_TOTAL:
            return {
                ...state,
                promocodesTotal: action.payload,
            };

        case GET_ACTIVE_PROMOCODES_STARTED:
            return {
                ...state,
                isPromocodesLoading: true,
            };
        case GET_ACTIVE_PROMOCODES_SUCCESS:
            return {
                ...state,
                activePromocodes: action.payload.page === 1
                    ? action.payload.data
                    : [...state.activePromocodes, ...action.payload.data],
                isPromocodesLoading: false,
            };
        case GET_ACTIVE_PROMOCODES_FAILED:
            return {
                ...state,
                isPromocodesLoading: false,
            };
        case SET_ACTIVE_PROMOCODES_TOTAL:
            return {
                ...state,
                activePromocodesTotal: action.payload,
            };

        case POST_PROMOCODE_STARTED:
            return {
                ...state,
                isPromocodesCreateLoading: true
            };
        case POST_PROMOCODE_SUCCESS:
            return {
                ...state,
                promocodes: [...state.promocodes, action.payload],
                isPromocodesCreateLoading: false
            };
        case POST_PROMOCODE_FAILED:
            return {
                ...state,
                isPromocodesCreateLoading: false
            };

        case PUT_PROMOCODE_STARTED:
            return {
                ...state,
                isUpdateLoading: true
            };
        case PUT_PROMOCODE_SUCCESS:
            return {
                ...state,
                promocodes: state.promocodes.map(b => b.id === action.payload.id ? action.payload : b),
                isUpdateLoading: false
            };
        case PUT_PROMOCODE_FAILED:
            return {
                ...state,
                isUpdateLoading: false
            };

        case DELETE_PROMOCODE_STARTED:
            return {
                ...state,
                isPromocodesDeleteLoading: true
            };
        case DELETE_PROMOCODE_SUCCESS:
            return {
                ...state,
                promocodes: state.promocodes.filter(p => p.id !== action.payload),
                isPromocodesDeleteLoading: false
            };
        case DELETE_PROMOCODE_FAILED:
            return {
                ...state,
                isPromocodesDeleteLoading: false
            };

        default:
            return state;
    }
};

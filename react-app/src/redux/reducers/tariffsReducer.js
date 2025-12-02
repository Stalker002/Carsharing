import { GET_TARIFFS_STARTED, GET_TARIFFS_SUCCESS, GET_TARIFFS_FAILED } from "../actionCreators/tariffs";

const initialState = {
    tariffs: [],
    isTariffLoading: false
};

export const tariffsReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_TARIFFS_STARTED:
            return {
                ...state,
                isTariffLoading: true
            };
        case GET_TARIFFS_SUCCESS:
            return {
                ...state,
                tariffs: action.payload,
                isTariffLoading: false
            };
        case GET_TARIFFS_FAILED:
            return {
                ...state,
                isTariffLoading: false
            };

        default:
            return state;
    }
};

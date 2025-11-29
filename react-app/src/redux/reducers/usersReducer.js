import { GET_USERS_FAILED, GET_USERS_STARTED, GET_USERS_SUCCESS } from "../actionCreators/users";

const initialState = {
    users: [],
    isUsersLoading: true
}

export const userReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_USERS_STARTED:
            return {
                ...state,
                isUsersLoading: true
            }
        case GET_USERS_FAILED:
            return {
                ...state,
                isUsersLoading: false
            }
        case GET_USERS_SUCCESS:
            return {
                ...state,
                users: action.payload,
                isUsersLoading: false
            }
        default: {
            return {
                ...state
            }
        }
    }

}
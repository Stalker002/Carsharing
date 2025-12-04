import {
  DELETE_USER_FAILED,
  DELETE_USER_STARTED,
  DELETE_USER_SUCCESS,
  GET_MY_USER_FAILED,
  GET_MY_USER_STARTED,
  GET_MY_USER_SUCCESS,
  GET_USERS_FAILED,
  GET_USERS_STARTED,
  GET_USERS_SUCCESS,
  LOGIN_USER_FAILED,
  LOGIN_USER_STARTED,
  LOGIN_USER_SUCCESS,
  LOGOUT_USER_FAILED,
  LOGOUT_USER_STARTED,
  LOGOUT_USER_SUCCESS,
  POST_USER_FAILED,
  POST_USER_STARTED,
  POST_USER_SUCCESS,
  PUT_USER_FAILED,
  PUT_USER_STARTED,
  PUT_USER_SUCCESS,
  SET_USERS_TOTAL,
} from "../actionCreators/users";
const token = localStorage.getItem("tasty");

const initialState = {
  users: [],
  myUser: {},
  isLoggedIn: !token,
  isMyUserLoading: false,
  isUsersLoading: false,
};

export const userReducer = (state = initialState, action) => {
  switch (action.type) {
    case LOGIN_USER_STARTED:
      return {
        ...state,
        isUsersLoading: true,
      };
    case LOGIN_USER_SUCCESS:
      return {
        ...state,
        isLoggedIn: true,
        isUsersLoading: false,
      };
    case LOGIN_USER_FAILED:
      return {
        ...state,
        isUsersLoading: false,
      };

    case LOGOUT_USER_STARTED:
      return {
        ...state,
        isUsersLoading: true,
      };
    case LOGOUT_USER_SUCCESS:
      return {
        ...state,
        isLoggedIn: false,
        isUsersLoading: false,
      };
    case LOGOUT_USER_FAILED:
      return {
        ...state,
        isUsersLoading: false,
      };

    case GET_USERS_STARTED:
      return {
        ...state,
        isUsersLoading: true,
      };
    case GET_USERS_SUCCESS:
      return {
        ...state,
        users:
          action.payload.page === 1
            ? action.payload.data
            : [...state.users, ...action.payload.data],
        isUsersLoading: false,
      };
    case GET_USERS_FAILED:
      return {
        ...state,
        isUsersLoading: false,
      };
    case SET_USERS_TOTAL:
      return {
        ...state,
        usersTotal: action.payload,
      };

    case GET_MY_USER_STARTED:
      return {
        ...state,
        isMyUserLoading: true,
      };
    case GET_MY_USER_SUCCESS:
      return {
        ...state,
        myUser: action.payload[0],
        isMyUserLoading: false,
      };
    case GET_MY_USER_FAILED:
      return {
        ...state,
        isMyUserLoading: false,
      };

    case POST_USER_STARTED:
      return {
        ...state,
        isUsersLoading: true,
      };
    case POST_USER_SUCCESS:
      return {
        ...state,
        isUsersLoading: false,
        isLoggedIn: true,
        users: [action.payload, ...state.users],
      };
    case POST_USER_FAILED:
      return {
        ...state,
        isUsersLoading: false,
      };

    case PUT_USER_STARTED:
      return {
        ...state,
        isUpdateUserLoading: true,
      };
    case PUT_USER_SUCCESS:
      return {
        ...state,
        users: state.users.map((b) =>
          b.id === action.payload.id ? action.payload : b
        ),
        isUpdateUserLoading: false,
      };
    case PUT_USER_FAILED:
      return {
        ...state,
        isUpdateUserLoading: false,
      };

    case DELETE_USER_STARTED:
      return {
        ...state,
        isUsersLoading: true,
      };
    case DELETE_USER_SUCCESS:
      return {
        ...state,
        isUsersLoading: false,
        users: state.users.filter((item) => item.id != action.payload),
      };
    case DELETE_USER_FAILED:
      return {
        ...state,
        isUsersLoading: false,
      };

    default: {
      return {
        ...state,
      };
    }
  }
};

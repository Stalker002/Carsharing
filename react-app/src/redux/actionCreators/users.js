export const LOGIN_USER_STARTED = "LOGIN_USER_STARTED";
export const LOGIN_USER_SUCCESS = "LOGIN_USER_SUCCESS";
export const LOGIN_USER_FAILED = "LOGIN_USER_FAILED";

export const LOGOUT_USER_STARTED = "LOGOUT_USER_STARTED";
export const LOGOUT_USER_SUCCESS = "LOGOUT_USER_SUCCESS";
export const LOGOUT_USER_FAILED = "LOGOUT_USER_FAILED";

export const GET_USERS_STARTED = "GET_USERS_STARTED";
export const GET_USERS_SUCCESS = "GET_USERS_SUCCESS";
export const GET_USERS_FAILED = "GET_USERS_FAILED";
export const SET_USERS_TOTAL = "SET_USERS_TOTAL";

export const GET_MY_USER_STARTED = "GET_MY_USER_STARTED";
export const GET_MY_USER_SUCCESS = "GET_MY_USER_SUCCESS";
export const GET_MY_USER_FAILED = "GET_MY_USER_FAILED";

export const POST_USER_STARTED = "POST_USER_STARTED";
export const POST_USER_SUCCESS = "POST_USER_SUCCESS";
export const POST_USER_FAILED = "POST_USER_FAILED";

export const PUT_USER_STARTED = "PUT_USER_STARTED";
export const PUT_USER_SUCCESS = "PUT_USER_SUCCESS";
export const PUT_USER_FAILED = "PUT_USER_FAILED";

export const DELETE_USER_STARTED = "DELETE_USER_STARTED";
export const DELETE_USER_SUCCESS = "DELETE_USER_SUCCESS";
export const DELETE_USER_FAILED = "DELETE_USER_FAILED";

export const loginUserStarted = () => ({
    type: LOGIN_USER_STARTED,
});
export const loginUserSuccess = (data) => ({
    type: LOGIN_USER_SUCCESS,
    payload: data,
});
export const loginUserFailed = (error) => ({
    type: LOGIN_USER_FAILED,
    payload: error,
});

export const logoutUserStarted = () => ({
    type: LOGOUT_USER_STARTED,
});
export const logoutUserSuccess = () => ({
    type: LOGOUT_USER_SUCCESS,
});
export const logoutUserFailed = (error) => ({
    type: LOGOUT_USER_FAILED,
    payload: error,
});

export const getUsersStarted = () => ({
    type: GET_USERS_STARTED
});
export const getUsersSuccess = (users) => ({
    type: GET_USERS_SUCCESS,
    payload: users,
});
export const getUsersFailed = (error) => ({
    type: GET_USERS_FAILED,
    payload: error,
});
export const setUsersTotal = (total) => ({
    type: SET_USERS_TOTAL,
    payload: total,
});

export const getMyUserStarted = () => ({
    type: GET_MY_USER_STARTED
});
export const getMyUserSuccess = (client) => ({
    type: GET_MY_USER_SUCCESS,
    payload: client,
});
export const getMyUserFailed = (error) => ({
    type: GET_MY_USER_FAILED,
    payload: error,
});

export const createUserStarted = () => ({
    type: POST_USER_STARTED,
});
export const createUserSuccess = (user) => ({
    type: POST_USER_SUCCESS,
    payload: user,
});
export const createUserFailed = (error) => ({
    type: POST_USER_FAILED,
    payload: error,
});

export const updateUserStarted = () => ({
    type: PUT_USER_STARTED,
});
export const updateUserSuccess = (user) => ({
    type: PUT_USER_SUCCESS,
    payload: user,
});
export const updateUserFailed = (error) => ({
    type: PUT_USER_FAILED,
    payload: error,
});

export const deleteUserStarted = () => ({
    type: DELETE_USER_STARTED,
});
export const deleteUserSuccess = (id) => ({
    type: DELETE_USER_SUCCESS,
    payload: id,
});
export const deleteUserFailed = (error) => ({
    type: DELETE_USER_FAILED,
    payload: error,
});
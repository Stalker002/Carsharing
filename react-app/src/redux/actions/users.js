import { api } from "../../api";
import { createUserFailed, createUserStarted, createUserSuccess, deleteUserFailed, deleteUserStarted, deleteUserSuccess, getMyUserFailed, getMyUserStarted, getMyUserSuccess, getUsersFailed, getUsersStarted, getUsersSuccess, loginUserFailed, loginUserStarted, loginUserSuccess, logoutUserFailed, logoutUserStarted, logoutUserSuccess, setUsersTotal, updateUserFailed, updateUserStarted, updateUserSuccess } from "../actionCreators/users";

export const loginUser = (login, password) => {
    return async (dispatch) => {
        try {
            dispatch(loginUserStarted());

            const response = await api.users.loginUser({ data: { login, password } });

            if (response.data.token) {
                localStorage.setItem("tasty", response.data.token);
            }

            dispatch(loginUserSuccess(response.data));

            return { success: true, data: response.data };

        } catch (error) {
            const errorMessage = error.response?.data?.message || error.message;

            dispatch(loginUserFailed(error));

            return { success: false, message: errorMessage };
        }
    };
};

export const logoutUser = () => {
    return async (dispatch) => {
        dispatch(logoutUserStarted());

        try {
            await api.users.logoutUser();
            localStorage.removeItem("tasty");

            dispatch(logoutUserSuccess());
        } catch (error) {
            dispatch(logoutUserFailed(error));
        }
    };
};

export const getUsers = (page = 1) => {
    return async (dispatch) => {
        try {
            dispatch(getUsersStarted());

            const response = await api.users.getUsers({
                params: {
                    _page: page,
                    _limit: 25,
                },
            });

            const totalCount = parseInt(response.headers["x-total-count"], 10);
            if (!isNaN(totalCount)) {
                dispatch(setUsersTotal(totalCount));
            }

            dispatch(getUsersSuccess({
                data: response.data,
                page,
            }));
        }
        catch (error) {
            dispatch(getUsersFailed(error));
        }
    };
};

export const getMyUser = () => {
    return async (dispatch) => {
        try {
            dispatch(getMyUserStarted())

            const response = await api.users.getMyUser();

            dispatch(getMyUserSuccess(response.data));
        }
        catch (error) {
            dispatch(getMyUserFailed(error));
        }
    };
};

export const createUser = (data) => {
    return async (dispatch) => {
        try {
            dispatch(createUserStarted());

            const response = await api.users.createUser(data);

            dispatch(createUserSuccess(response.data));

            return { success: true, data: response.data };
        }
        catch (error) {
            const errorMessage = error.response?.data?.message || error.message;

            dispatch(createUserFailed(error));

            return { success: false, message: errorMessage };
        }
    };
};

export const updateUser = (id, data) => {
    return async (dispatch) => {
        try {
            dispatch(updateUserStarted());

            const response = await api.users.updateUser(id, data);

            dispatch(updateUserSuccess(response.data));
        }
        catch (error) {
            dispatch(updateUserFailed(error));
        }
    };
};

export const deleteUser = (id) => {
    return async (dispatch) => {
        try {
            dispatch(deleteUserStarted());

            const response = await api.users.deleteUser(id);

            dispatch(deleteUserSuccess(response.data));
        }
        catch (error) {
            dispatch(deleteUserFailed(error));
        }
    };
};
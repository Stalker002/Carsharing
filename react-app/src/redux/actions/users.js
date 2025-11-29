import { api } from "../../api"
import { getUsersFailed, getUsersStarted, getUsersSuccess } from "../actionCreators/users"

export const getPhotos = () => {
    return async (dispatch) => {
        try {
            dispatch(getUsersStarted)
            const response = api.users.getUsers({
                params: {
                    _page: 0,
                    _limit: 5
                }
            })
            dispatch(getUsersSuccess(response.data));
        } catch (error) {
            dispatch(getUsersFailed(error));
        }
    }
}
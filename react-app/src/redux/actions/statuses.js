import { api } from "../../api";
import {
  getStatusesFailed,
  getStatusesStarted,
  getStatusesSuccess,
} from "../actionCreators/statuses";

export const getStatuses = () => {
  return async (dispatch) => {
    try {
      dispatch(getStatusesStarted());

      const response = await api.statuses.getStatuses();

      dispatch(getStatusesSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка статусов";

      dispatch(getStatusesFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

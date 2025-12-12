import { api } from "../../api";
import {
  getTariffsFailed,
  getTariffsStarted,
  getTariffsSuccess,
} from "../actionCreators/tariffs";

export const getTariffs = () => {
  return async (dispatch) => {
    try {
      dispatch(getTariffsStarted());

      const response = await api.tariff.getTariffs();

      dispatch(getTariffsSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка тарифа";

      dispatch(getTariffsFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

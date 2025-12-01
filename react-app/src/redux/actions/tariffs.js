import { api } from "../../api";
import { getTariffsFailed, getTariffsStarted, getTariffsSuccess } from "../actionCreators/tariffs";

export const getTariffs = () => {
    return async (dispatch) => {
        try {
            dispatch(getTariffsStarted());

            const response = await api.tariff.getTariffs();

            dispatch(getTariffsSuccess(response.data));
        } 
        catch (error) {
            dispatch(getTariffsFailed(error));
        }
    };
};
export const GET_TARIFFS_SUCCESS = "GET_TARIFFS_SUCCESS";
export const GET_TARIFFS_FAILED = "GET_TARIFFS_FAILED";
export const GET_TARIFFS_STARTED = "GET_TARIFFS_STARTED";

export const getTariffsStarted = () => ({
    type: GET_TARIFFS_STARTED
});
export const getTariffsSuccess = (tariffs) => ({
    type: GET_TARIFFS_SUCCESS,
    payload: tariffs,
});
export const getTariffsFailed = (error) => ({
    type: GET_TARIFFS_FAILED,
    payload: error,
});
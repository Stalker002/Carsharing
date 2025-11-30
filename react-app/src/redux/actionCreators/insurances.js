export const GET_INSURANCES_SUCCESS = "GET_INSURANCES_SUCCESS";
export const GET_INSURANCES_FAILED = "GET_INSURANCES_FAILED";
export const GET_INSURANCES_STARTED = "GET_INSURANCES_STARTED";

export const GET_INSURANCES_BY_CAR_STARTED = "GET_INSURANCES_BY_CAR_STARTED";
export const GET_INSURANCES_BY_CAR_SUCCESS = "GET_INSURANCES_BY_CAR_SUCCESS";
export const GET_INSURANCES_BY_CAR_FAILED = "GET_INSURANCES_BY_CAR_FAILED";

export const GET_ACTIVE_INSURANCE_BY_CAR_STARTED = "GET_ACTIVE_INSURANCE_BY_CAR_STARTED";
export const GET_ACTIVE_INSURANCE_BY_CAR_SUCCESS = "GET_ACTIVE_INSURANCE_BY_CAR_SUCCESS";
export const GET_ACTIVE_INSURANCE_BY_CAR_FAILED = "GET_ACTIVE_INSURANCE_BY_CAR_FAILED";

export const POST_INSURANCE_STARTED = "POST_INSURANCE_STARTED";
export const POST_INSURANCE_SUCCESS = "POST_INSURANCE_SUCCESS";
export const POST_INSURANCE_FAILED = "POST_INSURANCE_FAILED";

export const PUT_INSURANCE_STARTED = "PUT_INSURANCE_STARTED";
export const PUT_INSURANCE_SUCCESS = "PUT_INSURANCE_SUCCESS";
export const PUT_INSURANCE_FAILED = "PUT_INSURANCE_FAILED";

export const DELETE_INSURANCE_STARTED = "DELETE_INSURANCE_STARTED";
export const DELETE_INSURANCE_SUCCESS = "DELETE_INSURANCE_SUCCESS";
export const DELETE_INSURANCE_FAILED = "DELETE_INSURANCE_FAILED";

export const getInsurancesStarted = () => ({
    type: GET_INSURANCES_STARTED
});
export const getInsurancesSuccess = (insurances) => ({
    type: GET_INSURANCES_SUCCESS,
    payload: insurances,
});
export const getInsurancesFailed = (error) => ({
    type: GET_INSURANCES_FAILED,
    payload: error,
});

export const getInsurancesByCarStarted = () => ({
    type: GET_INSURANCES_BY_CAR_STARTED
});
export const getInsurancesByCarSuccess = (insurances) => ({
    type: GET_INSURANCES_BY_CAR_SUCCESS,
    payload: insurances,
});
export const getInsurancesByCarFailed = (error) => ({
    type: GET_INSURANCES_BY_CAR_FAILED,
    payload: error,
});

export const getActiveInsuranceByCarStarted = () => ({
    type: GET_ACTIVE_INSURANCE_BY_CAR_STARTED
});
export const getActiveInsuranceByCarSuccess = (insurances) => ({
    type: GET_ACTIVE_INSURANCE_BY_CAR_SUCCESS,
    payload: insurances,
});
export const getActiveInsuranceByCarFailed = (error) => ({
    type: GET_ACTIVE_INSURANCE_BY_CAR_FAILED,
    payload: error,
});

export const createInsuranceStarted = () => ({
    type: POST_INSURANCE_STARTED,
});
export const createInsuranceSuccess = (insurance) => ({
    type: POST_INSURANCE_SUCCESS,
    payload: insurance,
});
export const createInsuranceFailed = (error) => ({
    type: POST_INSURANCE_FAILED,
    payload: error,
});

export const updateInsuranceStarted = () => ({
    type: PUT_INSURANCE_STARTED,
});
export const updateInsuranceSuccess = (insurance) => ({
    type: PUT_INSURANCE_SUCCESS,
    payload: insurance,
});
export const updateInsuranceFailed = (error) => ({
    type: PUT_INSURANCE_FAILED,
    payload: error,
});

export const deleteInsuranceStarted = () => ({
    type: DELETE_INSURANCE_STARTED,
});
export const deleteInsuranceSuccess = (id) => ({
    type: DELETE_INSURANCE_SUCCESS,
    payload: id,
});
export const deleteInsuranceFailed = (error) => ({
    type: DELETE_INSURANCE_FAILED,
    payload: error,
});
export const GET_PAYMENTS_SUCCESS = "GET_PAYMENTS_SUCCESS";
export const GET_PAYMENTS_FAILED = "GET_PAYMENTS_FAILED";
export const GET_PAYMENTS_STARTED = "GET_PAYMENTS_STARTED";
export const SET_PAYMENTS_TOTAL = "SET_PAYMENTS_TOTAL";

export const GET_PAYMENTS_BY_BILL_STARTED = "GET_PAYMENTS_BY_BILL_STARTED";
export const GET_PAYMENTS_BY_BILL_SUCCESS = "GET_PAYMENTS_BY_BILL_SUCCESS";
export const GET_PAYMENTS_BY_BILL_FAILED = "GET_PAYMENTS_BY_BILL_FAILED";

export const POST_PAYMENT_STARTED = "POST_PAYMENT_STARTED";
export const POST_PAYMENT_SUCCESS = "POST_PAYMENT_SUCCESS";
export const POST_PAYMENT_FAILED = "POST_PAYMENT_FAILED";

export const PUT_PAYMENT_STARTED = "PUT_PAYMENT_STARTED";
export const PUT_PAYMENT_SUCCESS = "PUT_PAYMENT_SUCCESS";
export const PUT_PAYMENT_FAILED = "PUT_PAYMENT_FAILED";

export const DELETE_PAYMENT_STARTED = "DELETE_PAYMENT_STARTED";
export const DELETE_PAYMENT_SUCCESS = "DELETE_PAYMENT_SUCCESS";
export const DELETE_PAYMENT_FAILED = "DELETE_PAYMENT_FAILED";

export const getPaymentsStarted = () => ({
    type: GET_PAYMENTS_STARTED
});
export const getPaymentsSuccess = (payments) => ({
    type: GET_PAYMENTS_SUCCESS,
    payload: payments,
});
export const getPaymentsFailed = (error) => ({
    type: GET_PAYMENTS_FAILED,
    payload: error,
});
export const setPaymentsTotal = (total) => ({
    type: SET_PAYMENTS_TOTAL,
    payload: total,
});

export const getPaymentsByBillStarted = () => ({
    type: GET_PAYMENTS_BY_BILL_STARTED
});
export const getPaymentsByBillSuccess = (payments) => ({
    type: GET_PAYMENTS_BY_BILL_SUCCESS,
    payload: payments,
});
export const getPaymentsByBillFailed = (error) => ({
    type: GET_PAYMENTS_BY_BILL_FAILED,
    payload: error,
});

export const createPaymentStarted = () => ({
    type: POST_PAYMENT_STARTED,
});
export const createPaymentSuccess = (payment) => ({
    type: POST_PAYMENT_SUCCESS,
    payload: payment,
});
export const createPaymentFailed = (error) => ({
    type: POST_PAYMENT_FAILED,
    payload: error,
});

export const updatePaymentStarted = () => ({
    type: PUT_PAYMENT_STARTED,
});
export const updatePaymentSuccess = (payment) => ({
    type: PUT_PAYMENT_SUCCESS,
    payload: payment,
});
export const updatePaymentFailed = (error) => ({
    type: PUT_PAYMENT_FAILED,
    payload: error,
});

export const deletePaymentStarted = () => ({
    type: DELETE_PAYMENT_STARTED,
});
export const deletePaymentSuccess = (id) => ({
    type: DELETE_PAYMENT_SUCCESS,
    payload: id,
});
export const deletePaymentFailed = (error) => ({
    type: DELETE_PAYMENT_FAILED,
    payload: error,
});
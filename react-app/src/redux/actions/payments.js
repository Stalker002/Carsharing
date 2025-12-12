import { api } from "../../api";
import {
  createPaymentFailed,
  createPaymentStarted,
  createPaymentSuccess,
  deletePaymentFailed,
  deletePaymentStarted,
  deletePaymentSuccess,
  getPaymentsByBillFailed,
  getPaymentsByBillStarted,
  getPaymentsByBillSuccess,
  getPaymentsFailed,
  getPaymentsStarted,
  getPaymentsSuccess,
  setPaymentsTotal,
  updatePaymentFailed,
  updatePaymentStarted,
  updatePaymentSuccess,
} from "../actionCreators/payments";

export const getPayments = (page = 1) => {
  return async (dispatch) => {
    try {
      dispatch(getPaymentsStarted());

      const response = await api.payments.getPayments({
        params: {
          _page: page,
          _limit: 25,
        },
      });

      const totalCount = parseInt(response.headers["x-total-count"], 10);
      if (!isNaN(totalCount)) {
        dispatch(setPaymentsTotal(totalCount));
      }

      dispatch(
        getPaymentsSuccess({
          data: response.data,
          page,
        })
      );
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка платежа";

      dispatch(getPaymentsFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const getPaymentByBill = (billId) => {
  return async (dispatch) => {
    try {
      dispatch(getPaymentsByBillStarted());

      const response = await api.payments.getPaymentsByBillId(billId);

      dispatch(getPaymentsByBillSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка платежа";

      dispatch(getPaymentsByBillFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const createPayment = (data) => {
  return async (dispatch) => {
    try {
      dispatch(createPaymentStarted());

      const response = await api.payments.createPayment(data);

      dispatch(createPaymentSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка платежа";

      dispatch(createPaymentFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const updatePayment = (id, data) => {
  return async (dispatch) => {
    try {
      dispatch(updatePaymentStarted());

      const response = await api.payments.updatePayment(id, data);

      dispatch(updatePaymentSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка платежа";

      dispatch(updatePaymentFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const deletePayment = (id) => {
  return async (dispatch) => {
    try {
      dispatch(deletePaymentStarted());

      const response = await api.payments.deletePayment(id);

      dispatch(deletePaymentSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка платежа";

      dispatch(deletePaymentFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

import { api } from "../../api";
import {
  applyPromocodeFailed,
  applyPromocodeStarted,
  applyPromocodeSuccess,
  createBillFailed,
  createBillStarted,
  createBillSuccess,
  deleteBillFailed,
  deleteBillStarted,
  deleteBillSuccess,
  getBillsFailed,
  getBillsStarted,
  getBillsSuccess,
  getInfoBillFailed,
  getInfoBillStarted,
  getInfoBillSuccess,
  getMyBillsFailed,
  getMyBillsStarted,
  getMyBillsSuccess,
  setBillsTotal,
  setMyBillsTotal,
  updateBillFailed,
  updateBillStarted,
  updateBillSuccess,
} from "../actionCreators/bills";
import { openModal } from "./modal";

export const getBills = (page = 1) => {
  return async (dispatch) => {
    try {
      dispatch(getBillsStarted());

      const response = await api.bills.getBills({
        params: {
          _page: page,
          _limit: 25,
        },
      });

      const totalCount = parseInt(response.headers["x-total-count"], 10);
      if (!isNaN(totalCount)) {
        dispatch(setBillsTotal(totalCount));
      }

      dispatch(
        getBillsSuccess({
          data: response.data,
          page,
        })
      );
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка счета";

      dispatch(getBillsFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const getMyBills = (page = 1) => {
  return async (dispatch) => {
    try {
      dispatch(getMyBillsStarted());

      const response = await api.bills.getMyBills({
        params: {
          _page: page,
          _limit: 25,
        },
      });

      const totalCount = parseInt(response.headers["x-total-count"], 10);
      if (!isNaN(totalCount)) {
        dispatch(setMyBillsTotal(totalCount));
      }

      dispatch(
        getMyBillsSuccess({
          data: response.data,
          page,
        })
      );
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка счета";

      dispatch(getMyBillsFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const getInfoBill = (id) => {
  return async (dispatch) => {
    try {
      dispatch(getInfoBillStarted());

      const response = await api.bills.getInfoBill(id);
      const billData = Array.isArray(response.data) ? response.data[0] : response.data;

      dispatch(getInfoBillSuccess(billData));
      return { success: true, data: billData };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка счета";

      dispatch(getInfoBillFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const createBill = (data) => {
  return async (dispatch) => {
    try {
      dispatch(createBillStarted());

      const response = await api.bills.createBill(data);

      dispatch(createBillSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка счета";

      dispatch(createBillFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const applyPromocode = (billId, code) => async (dispatch) => {
  try {
    dispatch(applyPromocodeStarted());
     const res = await api.bills.applyPromocode(billId, { code }); 

    dispatch(
      openModal({
        type: "success",
        title: "Успешно",
        message: "Промокод применен! Сумма пересчитана.",
      })
    );

    dispatch(applyPromocodeSuccess(res.data));
    return { success: true };
  } catch (error) {
    const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка счета";

    dispatch(applyPromocodeFailed(error));
    return { success: false, message: errorMessage };
  }
};

export const updateBill = (id, data) => {
  return async (dispatch) => {
    try {
      dispatch(updateBillStarted());

      const response = await api.bills.updateBill(id, data);

      dispatch(updateBillSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка счета";

      dispatch(updateBillFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const deleteBill = (id) => {
  return async (dispatch) => {
    try {
      dispatch(deleteBillStarted());

      const response = await api.bills.deleteBill(id);

      dispatch(deleteBillSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка счета";

      dispatch(deleteBillFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

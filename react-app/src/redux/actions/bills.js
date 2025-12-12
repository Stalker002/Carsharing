import { api } from "../../api";
import {
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

export const getInfoBills = (id) => {
  return async (dispatch) => {
    try {
      dispatch(getInfoBillStarted());

      const response = await api.bills.getInfoBill(id);

      dispatch(getInfoBillSuccess(response.data));
      return { success: true, data: response.data };
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

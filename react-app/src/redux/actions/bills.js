import { api } from "../../api";
import { createBillFailed, createBillStarted, createBillSuccess, deleteBillFailed, deleteBillStarted, deleteBillSuccess, getBillsFailed, getBillsStarted, getBillsSuccess, getInfoBillFailed, getInfoBillStarted, getInfoBillSuccess, getMyBillsFailed, getMyBillsStarted, getMyBillsSuccess, setBillsTotal, setMyBillsTotal, updateBillFailed, updateBillStarted, updateBillSuccess } from "../actionCreators/bills";

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

            dispatch(getBillsSuccess({
                data: response.data,
                page,
            }));
        } 
        catch (error) {
            dispatch(getBillsFailed(error));
        }
    };
};

export const getMyBills = (page = 1) => {
    return async (dispatch) => {
        try {
            dispatch(getMyBillsStarted())

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

            dispatch(getMyBillsSuccess({
                data: response.data,
                page,
            }));
        } 
        catch (error) {
            dispatch(getMyBillsFailed(error));
        }
    };
};

export const getInfoBills = (id) => {
    return async (dispatch) => {
        try {
            dispatch(getInfoBillStarted())

            const response = await api.bills.getInfoBill(id);

            dispatch(getInfoBillSuccess(response.data));
        } 
        catch (error) {
            dispatch(getInfoBillFailed(error));
        }
    };
};

export const createBill = (data) => {
    return async (dispatch) => {
        try {
            dispatch(createBillStarted());

            const response = await api.bills.createBill(data);

            dispatch(createBillSuccess(response.data));
        } 
        catch (error) {
            dispatch(createBillFailed(error));
        }
    };
};

export const updateBill = (id, data) => {
  return async (dispatch) => {
    try {
      dispatch(updateBillStarted());

      const response = await api.bills.updateBill(id, data);

      dispatch(updateBillSuccess(response.data));
    } 
    catch (error) {
      dispatch(updateBillFailed(error));
    }
  };
};

export const deleteBill = (id) => {
  return async (dispatch) => {
    try {
      dispatch(deleteBillStarted());

      const response = await api.bills.deleteBill(id);

      dispatch(deleteBillSuccess(response.data));
    } 
    catch (error) {
      dispatch(deleteBillFailed(error));
    }
  };
};
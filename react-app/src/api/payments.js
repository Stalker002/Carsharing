import { makeRequest } from "./makeRequest";

const URL = "/Payments";

export const getPayments = (config) => makeRequest({
    method: "GET",
    url: URL,
    withCredentials: true,
    ...config,
});

export const getPaymentsByBillId = (billId, config) => makeRequest({
    method: "GET",
    url: `${URL}/byBill/${billId}`,
    withCredentials: true,
    ...config
});

export const createPayment = (data) => makeRequest({
    method: "POST",
    url: URL,
    withCredentials: true,
    data,
});

export const updatePayment = (id, data) => makeRequest({
    method: "PUT",
    url: `${URL}/${id}`,
    withCredentials: true,
    data,
});

export const deletePayment = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    withCredentials: true,
    ...config,
});
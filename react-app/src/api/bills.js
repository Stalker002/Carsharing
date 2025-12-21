import { makeRequest } from "./makeRequest";

const URL = "/Bills";

export const getBills = (config) => makeRequest({
    method: "GET",
    url: URL,
    withCredentials: true,
    ...config,
});

export const getMyBills = (config) => makeRequest({
    method: "GET",
    url: `${URL}/pagedByUser`,
    withCredentials: true,
    ...config
});

export const getInfoBill = (id, config) => makeRequest({
    method: "GET",
    url: `${URL}/info/${id}`,
    withCredentials: true,
    ...config
});

export const createBill = (data) => makeRequest({
    method: "POST",
    url: URL,
    withCredentials: true,
    data,
});

export const applyPromocode = (id, data) => makeRequest({
    method: "POST",
    url: `${URL}/${id}/promocode`,
    withCredentials: true,
    data,
});

export const updateBill = (id, data) => makeRequest({
    method: "PUT",
    url: `${URL}/${id}`,
    withCredentials: true,
    data,
});

export const deleteBill = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    withCredentials: true,
    ...config,
});
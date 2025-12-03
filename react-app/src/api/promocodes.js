import { makeRequest } from "./makeRequest";

const URL = "/Promocodes";

export const getPromocodes = (config) => makeRequest({
    method: "GET",
    url: URL,
    withCredentials: true,
    ...config,
});

export const getActivePromocodes = (config) => makeRequest({
    method: "GET",
    url: `${URL}/pagedActive`,
    withCredentials: true,
    ...config
});

export const createPromocode = (data) => makeRequest({
    method: "POST",
    url: URL,
    withCredentials: true,
    data,
});

export const updatePromocode = (id, data) => makeRequest({
    method: "PUT",
    url: `${URL}/${id}`,
    withCredentials: true,
    data,
});

export const deletePromocode = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    withCredentials: true,
    ...config,
});
import { makeRequest } from "./makeRequest";

const URL = "/Cars";

export const getCars = (config) => makeRequest({
    method: "GET",
    url: URL,
    withCredentials: true,
    ...config,
});

export const getCarInfo = (id, config) => makeRequest({
    method: "GET",
    url: `${URL}/with-info/${id}`,
    withCredentials: true,
    ...config,
});

export const getCarInfoAdmin = (id, config) => makeRequest({
    method: "GET",
    url: `${URL}/with-info-admin/${id}`,
    withCredentials: true,
    ...config,
});

export const getCarByCategory = (config) => makeRequest({
    method: "GET",
    url: `${URL}/pagedByCategory`,
    withCredentials: true,
    ...config,
});

export const createCar = (data) => makeRequest({
    method: "POST",
    url: URL,
    withCredentials: true,
    data,
});

export const updateCar = (id, data) => makeRequest({
    method: "PUT",
    url: `${URL}/${id}`,
    withCredentials: true,
    data,
});

export const deleteCar = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    withCredentials: true,
    ...config,
});
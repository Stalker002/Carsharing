import { makeRequest } from "./makeRequest";

const URL = "/Insurances";

export const getInsurances = (config) => makeRequest({
    method: "GET",
    url: URL,
    withCredentials: true,
    ...config,
});

export const getInsuranceByCar = (carId, config) => makeRequest({
    method: "GET",
    url: `${URL}/byCarId/${carId}`,
    withCredentials: true,
    ...config
});

export const getActiveInsuranceByCar = (carId, config) => makeRequest({
    method: "GET",
    url: `${URL}/ActiveByCarId/${carId}`,
    withCredentials: true,
    ...config
});

export const createInsurance = (data) => makeRequest({
    method: "POST",
    url: URL,
    withCredentials: true,
    data,
});

export const updateInsurance = (id, data) => makeRequest({
    method: "PUT",
    url: `${URL}/${id}`,
    withCredentials: true,
    data,
});

export const deleteInsurance = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    withCredentials: true,
    ...config,
});
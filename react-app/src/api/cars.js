const URL = "/Cars";

export const getCars = (config) => makeRequest({
    method: "GET",
    url: URL,
    ...config,
});

export const getCarInfo = (id, config) => makeRequest({
    method: "GET",
    url: `${URL}/${id}`,
    ...config,
});

export const getCarByCategory = (id, config) => makeRequest({
    method: "GET",
    url: `${URL}/byCategory`,
    ...config,
});

export const createCar = (data) => makeRequest({
    method: "POST",
    url: URL,
    data,
});

export const deleteCar = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    ...config,
});
const URL = "/Tariff";

export const getTariffs = (config) => makeRequest({
    method: "GET",
    url: URL,
    withCredentials: true,
    ...config,
});

export const getTariffById = (id, config) => makeRequest({
    method: "GET",
    url: `${URL}/${id}`,
    withCredentials: true,
    ...config,
});
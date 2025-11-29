const URL = "/Tariff";

export const getTariffs = (config) => makeRequest({
    method: "GET",
    url: URL,
    ...config,
});
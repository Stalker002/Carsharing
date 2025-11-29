const URL = "/Promocodes";

export const getPromocodes = (config) => makeRequest({
    method: "GET",
    url: URL,
    ...config,
});

export const getActivePromocodes = (config) => makeRequest({
    method: "GET",
    url: `${URL}/Active`,
    ...config
});

export const createPromocode = (data) => makeRequest({
    method: "POST",
    url: URL,
    data,
});

export const deletePromocode = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    ...config,
});
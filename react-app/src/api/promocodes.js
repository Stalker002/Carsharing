const URL = "/Promocodes";

export const getPromocodes = (config) => makeRequest({
    method: "GET",
    url: URL,
    ...config,
});

export const getActivePromocodes = (config) => makeRequest({
    method: "GET",
    url: `${URL}/pagedActive`,
    ...config
});

export const createPromocode = (data) => makeRequest({
    method: "POST",
    url: URL,
    data,
});

export const updatePromocode = (id, data) => makeRequest({
    method: "PUT",
    url: `${URL}/${id}`,
    data,
});

export const deletePromocode = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    ...config,
});
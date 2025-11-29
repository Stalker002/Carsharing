const URL = "/Category";

export const getCategory = (config) => makeRequest({
    method: "GET",
    url: URL,
    ...config,
});

export const createCategory = (data) => makeRequest({
    method: "POST",
    url: URL,
    data,
});

export const deleteCategory = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    ...config,
});
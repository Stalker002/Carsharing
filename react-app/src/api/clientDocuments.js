const URL = "/ClientDocuments";

export const getClientDocuments = (config) => makeRequest({
    method: "GET",
    url: URL,
    ...config,
});

export const createClientDocument = (data) => makeRequest({
    method: "POST",
    url: URL,
    data,
});

export const updateClientDocument = (id, data) => makeRequest({
    method: "PUT",
    url: `${URL}/${id}`,
    data,
});

export const deleteClientDocument = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    ...config,
});
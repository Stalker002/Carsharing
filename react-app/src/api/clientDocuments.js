const URL = "/ClientDocuments";

export const getClientDocuments = (config) => makeRequest({
    method: "GET",
    url: URL,
    withCredentials: true,
    ...config,
});

export const createClientDocument = (data) => makeRequest({
    method: "POST",
    url: URL,
    withCredentials: true,
    data,
});

export const updateClientDocument = (id, data) => makeRequest({
    method: "PUT",
    url: `${URL}/${id}`,
    withCredentials: true,
    data,
});

export const deleteClientDocument = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    withCredentials: true,
    ...config,
});
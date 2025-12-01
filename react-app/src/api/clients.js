const URL = "/Clients";

export const getClients = (config) => makeRequest({
    method: "GET",
    url: URL,
    withCredentials: true,
    ...config,
});

export const getMyDocuments = (config) => makeRequest({
    method: "GET",
    url: `${URL}/MyDocuments`,
    withCredentials: true,
    ...config
});

export const getClientDocument = (clientId, config) => makeRequest({
    method: "GET",
    url: `${URL}/Documents/${clientId}`,
    withCredentials: true,
    ...config
});

export const getMyClient = (config) => makeRequest({
    method: "GET",
    url: `${URL}/My`,
    withCredentials: true,
    ...config
});

export const createClient = (data) => makeRequest({
    method: "POST",
    url: `${URL}/with-user`,
    withCredentials: true,
    data,
});

export const updateClient = (id, data) => makeRequest({
    method: "PUT",
    url: `${URL}/${id}`,
    withCredentials: true,
    data,
});

export const deleteClient = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    withCredentials: true,
    ...config,
});
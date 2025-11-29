const URL = "/Clients";

export const getClients = (config) => makeRequest({
    method: "GET",
    url: URL,
    ...config,
});

export const getClientDocuments = (clientId, config) => makeRequest({
    method: "GET",
    url: `${URL}/Documents/${clientId}`,
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
    data,
});

export const deleteClient = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    ...config,
});
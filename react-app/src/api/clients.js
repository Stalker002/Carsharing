const URL = "/Clients";

export const getClients = (config) => makeRequest({
    method: "GET",
    url: URL,
    ...config,
});

export const getMyClientDocuments = (config) => makeRequest({
    method: "GET",
    url: `${URL}/Documents`,
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
    data,
});

export const updateClient = (id, data) => makeRequest({
    method: "PUT",
    url: `${URL}/${id}`,
    data,
});

export const deleteClient = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    ...config,
});
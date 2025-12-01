const URL = "/Statuses";

export const getStatuses = (config) => makeRequest({
    method: "GET",
    url: URL,
    withCredentials: true,
    ...config,
});

export const getStatuseById = (id, config) => makeRequest({
    method: "GET",
    url: `${URL}/${id}`,
    withCredentials: true,
    ...config,
});
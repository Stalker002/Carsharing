const URL = "/Statuses";

export const getStatuses = (config) => makeRequest({
    method: "GET",
    url: URL,
    ...config,
});
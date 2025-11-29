const URL = "/Trips";

export const getTrips = (config) => makeRequest({
    method: "GET",
    url: URL,
    ...config,
});

export const getTripWithInfo = (id, config) => makeRequest({
    method: "GET",
    url: `${URL}/${id}`,
    ...config
});

export const createTrip = (data) => makeRequest({
    method: "POST",
    url: URL,
    data,
});

export const deleteTrip = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    ...config,
});
const URL = "/Trips";

export const getTrips = (config) => makeRequest({
    method: "GET",
    url: URL,
    ...config,
});

export const getMyTrips = (config) => makeRequest({
    method: "GET",
    url: `${URL}/pagedByClient`,
    withCredentials: true,
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

export const updateTrip = (id, data) => makeRequest({
    method: "PUT",
    url: `${URL}/${id}`,
    data,
});

export const deleteTrip = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    ...config,
});
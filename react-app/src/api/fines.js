const URL = "/Fines";

export const getFines = (config) => makeRequest({
    method: "GET",
    url: URL,
    withCredentials: true,
    ...config,
});

export const getFinesByTrip = (tripId, config) => makeRequest({
    method: "GET",
    url: `${URL}/byTrip/${tripId}`,
    withCredentials: true,
    ...config
});

export const createFine = (data) => makeRequest({
    method: "POST",
    url: URL,
    withCredentials: true,
    data,
});

export const updateFine = (id, data) => makeRequest({
    method: "PUT",
    url: `${URL}/${id}`,
    withCredentials: true,
    data,
});

export const deleteFine = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    withCredentials: true,
    ...config,
});
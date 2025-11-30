const URL = "/Fines";

export const getFines = (config) => makeRequest({
    method: "GET",
    url: URL,
    ...config,
});

export const getFinesByTrip = (tripId, config) => makeRequest({
    method: "GET",
    url: `${URL}/byTrip/${tripId}`,
    ...config
});

export const createFine = (data) => makeRequest({
    method: "POST",
    url: URL,
    data,
});

export const updateFine = (id, data) => makeRequest({
    method: "PUT",
    url: `${URL}/${id}`,
    data,
});

export const deleteFine = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    ...config,
});
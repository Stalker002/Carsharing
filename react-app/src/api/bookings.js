const URL = "/Bookings";

export const getBookings = (config) => makeRequest({
    method: "GET",
    url: URL,
    ...config,
});

export const getMyBookings = (clientId, config) => makeRequest({
    method: "GET",
    url: `${URL}/byClient/${clientId}`,
    ...config,
});

export const getBookingInfo = (id, config) => makeRequest({
    method: "GET",
    url: `${URL}/withInfo/${id}`,
    ...config,
});

export const createBooking = (data) => makeRequest({
    method: "POST",
    url: URL,
    data,
});

export const deleteBooking = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    ...config,
});
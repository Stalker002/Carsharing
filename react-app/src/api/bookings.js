const URL = "/Bookings";

export const getBookings = (config) => makeRequest({
    method: "GET",
    url: URL,
    ...config,
});

export const getMyBookings = (config) => makeRequest({
    method: "GET",
    url: `${URL}/pagedByClient`,
    withCredentials: true,
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

export const updateBooking = (id, data) => makeRequest({
    method: "PUT",
    url: `${URL}/${id}`,
    data,
});

export const deleteBooking = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    ...config,
});
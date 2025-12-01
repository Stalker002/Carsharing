const URL = "/Bookings";

export const getBookings = (config) => makeRequest({
    method: "GET",
    url: URL,
    withCredentials: true,
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
    withCredentials: true,
    ...config,
});

export const createBooking = (data) => makeRequest({
    method: "POST",
    url: URL,
    withCredentials: true,
    data,
});

export const updateBooking = (id, data) => makeRequest({
    method: "PUT",
    url: `${URL}/${id}`,
    withCredentials: true,
    data,
});

export const deleteBooking = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    withCredentials: true,
    ...config,
});
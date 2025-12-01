const URL = "/Reviews";

export const getReviews = (config) => makeRequest({
    method: "GET",
    url: URL,
    withCredentials: true,
    ...config,
});

export const getReviewsByCar = (carId, config) => makeRequest({
    method: "GET",
    url: `${URL}/pagedByCar/${carId}`,
    withCredentials: true,
    ...config
});

export const createReview = (data) => makeRequest({
    method: "POST",
    url: URL,
    withCredentials: true,
    data,
});

export const updateReview = (id, data) => makeRequest({
    method: "PUT",
    url: `${URL}/${id}`,
    withCredentials: true,
    data,
});

export const deleteReview = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    withCredentials: true,
    ...config,
});
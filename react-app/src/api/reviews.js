const URL = "/Reviews";

export const getReviews = (config) => makeRequest({
    method: "GET",
    url: URL,
    ...config,
});

export const getReviewsByCar = (carId, config) => makeRequest({
    method: "GET",
    url: `${URL}/byCar/${carId}`,
    ...config
});

export const createReview = (data) => makeRequest({
    method: "POST",
    url: URL,
    data,
});

export const deleteReview = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    ...config,
});
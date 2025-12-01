export const GET_REVIEWS_SUCCESS = "GET_REVIEWS_SUCCESS";
export const GET_REVIEWS_FAILED = "GET_REVIEWS_FAILED";
export const GET_REVIEWS_STARTED = "GET_REVIEWS_STARTED";
export const SET_REVIEWS_TOTAL = "SET_REVIEWS_TOTAL";

export const GET_REVIEWS_BY_CAR_STARTED = "GET_REVIEWS_BY_CAR_STARTED";
export const GET_REVIEWS_BY_CAR_SUCCESS = "GET_REVIEWS_BY_CAR_SUCCESS";
export const GET_REVIEWS_BY_CAR_FAILED = "GET_REVIEWS_BY_CAR_FAILED";
export const SET_REVIEWS_BY_CAR_TOTAL = "SET_REVIEWS_BY_CAR_TOTAL";

export const POST_REVIEW_STARTED = "POST_REVIEW_STARTED";
export const POST_REVIEW_SUCCESS = "POST_REVIEW_SUCCESS";
export const POST_REVIEW_FAILED = "POST_REVIEW_FAILED";

export const PUT_REVIEW_STARTED = "PUT_REVIEW_STARTED";
export const PUT_REVIEW_SUCCESS = "PUT_REVIEW_SUCCESS";
export const PUT_REVIEW_FAILED = "PUT_REVIEW_FAILED";

export const DELETE_REVIEW_STARTED = "DELETE_REVIEW_STARTED";
export const DELETE_REVIEW_SUCCESS = "DELETE_REVIEW_SUCCESS";
export const DELETE_REVIEW_FAILED = "DELETE_REVIEW_FAILED";

export const getReviewsStarted = () => ({
    type: GET_REVIEWS_STARTED
});
export const getReviewsSuccess = (reviews) => ({
    type: GET_REVIEWS_SUCCESS,
    payload: reviews,
});
export const getReviewsFailed = (error) => ({
    type: GET_REVIEWS_FAILED,
    payload: error,
});
export const setReviewsTotal = (total) => ({
    type: SET_REVIEWS_TOTAL,
    payload: total,
});

export const getReviewsByCarStarted = () => ({
    type: GET_REVIEWS_BY_CAR_STARTED
});
export const getReviewsByCarSuccess = (reviews) => ({
    type: GET_REVIEWS_BY_CAR_SUCCESS,
    payload: reviews,
});
export const getReviewsByCarFailed = (error) => ({
    type: GET_REVIEWS_BY_CAR_FAILED,
    payload: error,
});
export const setReviewsByCarTotal = (total) => ({
    type: SET_REVIEWS_BY_CAR_TOTAL,
    payload: total,
});

export const createReviewStarted = () => ({
    type: POST_REVIEW_STARTED,
});
export const createReviewSuccess = (review) => ({
    type: POST_REVIEW_SUCCESS,
    payload: review,
});
export const createReviewFailed = (error) => ({
    type: POST_REVIEW_FAILED,
    payload: error,
});

export const updateReviewStarted = () => ({
    type: PUT_REVIEW_STARTED,
});
export const updateReviewSuccess = (review) => ({
    type: PUT_REVIEW_SUCCESS,
    payload: review,
});
export const updateReviewFailed = (error) => ({
    type: PUT_REVIEW_FAILED,
    payload: error,
});

export const deleteReviewStarted = () => ({
    type: DELETE_REVIEW_STARTED,
});
export const deleteReviewSuccess = (id) => ({
    type: DELETE_REVIEW_SUCCESS,
    payload: id,
});
export const deleteReviewFailed = (error) => ({
    type: DELETE_REVIEW_FAILED,
    payload: error,
});
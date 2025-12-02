import { GET_REVIEWS_STARTED, GET_REVIEWS_SUCCESS, GET_REVIEWS_FAILED, POST_REVIEW_STARTED, POST_REVIEW_SUCCESS, POST_REVIEW_FAILED, DELETE_REVIEW_STARTED, DELETE_REVIEW_SUCCESS, DELETE_REVIEW_FAILED, GET_REVIEWS_BY_CAR_STARTED, GET_REVIEWS_BY_CAR_SUCCESS, GET_REVIEWS_BY_CAR_FAILED, SET_REVIEWS_BY_CAR_TOTAL, PUT_REVIEW_STARTED, PUT_REVIEW_SUCCESS, PUT_REVIEW_FAILED } from "../actionCreators/reviews";

const initialState = {
    reviews: [],
    reviewsByCar: [],
    isReviewsLoading: false,
    isReviewsCreateLoading: false,
    isReviewsUpdateLoading: false,
    isReviewsDeleteLoading: false,
    reviewsTotal: 0,
    reviewsByCarTotal: 0,
};

export const reviewsReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_REVIEWS_STARTED:
            return {
                ...state,
                isReviewsLoading: true,
            };
        case GET_REVIEWS_SUCCESS:
            return {
                ...state,
                reviews: action.payload.page === 1
                    ? action.payload.data
                    : [...state.reviews, ...action.payload.data],
                isReviewsLoading: false,
            };
        case GET_REVIEWS_FAILED:
            return {
                ...state,
                isReviewsLoading: false,
            };
        case SET_REVIEWS_TOTAL:
            return {
                ...state,
                reviewsTotal: action.payload,
            };

        case GET_REVIEWS_BY_CAR_STARTED:
            return {
                ...state,
                isReviewsLoading: true,
            };
        case GET_REVIEWS_BY_CAR_SUCCESS:
            return {
                ...state,
                my: action.payload.page === 1
                    ? action.payload.data
                    : [...state.reviewsByCar, ...action.payload.data],
                isReviewsLoading: false,
            };
        case GET_REVIEWS_BY_CAR_FAILED:
            return {
                ...state,
                isReviewsLoading: false,
            };
        case SET_REVIEWS_BY_CAR_TOTAL:
            return {
                ...state,
                reviewsByCarTotal: action.payload,
            };

        case POST_REVIEW_STARTED:
            return {
                ...state,
                isReviewsCreateLoading: true
            };
        case POST_REVIEW_SUCCESS:
            return {
                ...state,
                reviews: [...state.reviews, action.payload],
                isReviewsCreateLoading: false
            };
        case POST_REVIEW_FAILED:
            return {
                ...state,
                isReviewsCreateLoading: false
            };

        case PUT_REVIEW_STARTED:
            return {
                ...state,
                isReviewsUpdateLoading: true
            };
        case PUT_REVIEW_SUCCESS:
            return {
                ...state,
                bills: state.reviews.map(b => b.id === action.payload.id ? action.payload : b),
                isReviewsUpdateLoading: false
            };
        case PUT_REVIEW_FAILED:
            return {
                ...state,
                isReviewsUpdateLoading: false
            };

        case DELETE_REVIEW_STARTED:
            return {
                ...state,
                isReviewsDeleteLoading: true
            };
        case DELETE_REVIEW_SUCCESS:
            return {
                ...state,
                reviews: state.reviews.filter(r => r.id !== action.payload), isReviewsDeleteLoading: false
            };
        case DELETE_REVIEW_FAILED:
            return {
                ...state,
                isReviewsDeleteLoading: false
            };

        default:
            return state;
    }
};

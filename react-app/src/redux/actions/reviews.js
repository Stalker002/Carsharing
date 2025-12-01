import { api } from "../../api";
import { createReviewFailed, createReviewStarted, createReviewSuccess, deleteReviewFailed, deleteReviewStarted, deleteReviewSuccess, getReviewsByCarFailed, getReviewsByCarStarted, getReviewsByCarSuccess, getReviewsFailed, getReviewsStarted, getReviewsSuccess, setReviewsByCarTotal, setReviewsTotal, updateReviewFailed, updateReviewStarted, updateReviewSuccess } from "../actionCreators/reviews";

export const getReviews = (page = 1) => {
    return async (dispatch) => {
        try {
            dispatch(getReviewsStarted());

            const response = await api.reviews.getReviews({
                params: {
                    _page: page,
                    _limit: 25,
                },
            });

            const totalCount = parseInt(response.headers["x-total-count"], 10);
            if (!isNaN(totalCount)) {
                dispatch(setReviewsTotal(totalCount));
            }

            dispatch(getReviewsSuccess({
                data: response.data,
                page,
            }));
        } 
        catch (error) {
            dispatch(getReviewsFailed(error));
        }
    };
};

export const getReviewsByCar = (carId, page = 1) => {
    return async (dispatch) => {
        try {
            dispatch(getReviewsByCarStarted())

            const response = await api.reviews.getReviewsByCar(
                carId,
                {
                params: {
                    _page: page,
                    _limit: 25,
                },
            });

            const totalCount = parseInt(response.headers["x-total-count"], 10);
            if (!isNaN(totalCount)) {
                dispatch(setReviewsByCarTotal(totalCount));
            }

            dispatch(getReviewsByCarSuccess({
                data: response.data,
                page,
            }));
        } 
        catch (error) {
            dispatch(getReviewsByCarFailed(error));
        }
    };
};

export const createReview = (data) => {
    return async (dispatch) => {
        try {
            dispatch(createReviewStarted());

            const response = await api.reviews.createReview(data);

            dispatch(createReviewSuccess(response.data));
        } 
        catch (error) {
            dispatch(createReviewFailed(error));
        }
    };
};

export const updateReview = (id, data) => {
  return async (dispatch) => {
    try {
      dispatch(updateReviewStarted());

      const response = await api.reviews.updateReview(id, data);

      dispatch(updateReviewSuccess(response.data));
    } 
    catch (error) {
      dispatch(updateReviewFailed(error));
    }
  };
};

export const deleteReview = (id) => {
  return async (dispatch) => {
    try {
      dispatch(deleteReviewStarted());

      const response = await api.reviews.deleteReview(id);

      dispatch(deleteReviewSuccess(response.data));
    } 
    catch (error) {
      dispatch(deleteReviewFailed(error));
    }
  };
};
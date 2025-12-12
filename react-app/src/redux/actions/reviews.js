import { api } from "../../api";
import {
  createReviewFailed,
  createReviewStarted,
  createReviewSuccess,
  deleteReviewFailed,
  deleteReviewStarted,
  deleteReviewSuccess,
  getReviewsByCarFailed,
  getReviewsByCarStarted,
  getReviewsByCarSuccess,
  getReviewsFailed,
  getReviewsStarted,
  getReviewsSuccess,
  setReviewsByCarTotal,
  setReviewsTotal,
  updateReviewFailed,
  updateReviewStarted,
  updateReviewSuccess,
} from "../actionCreators/reviews";

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

      dispatch(
        getReviewsSuccess({
          data: response.data,
          page,
        })
      );
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка отзыва";

      dispatch(getReviewsFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const getReviewsByCar = (carId, page = 1) => {
  return async (dispatch) => {
    try {
      dispatch(getReviewsByCarStarted());

      const response = await api.reviews.getReviewsByCar(carId, {
        params: {
          _page: page,
          _limit: 25,
        },
      });

      const totalCount = parseInt(response.headers["x-total-count"], 10);
      if (!isNaN(totalCount)) {
        dispatch(setReviewsByCarTotal(totalCount));
      }

      dispatch(
        getReviewsByCarSuccess({
          data: response.data,
          page,
        })
      );
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка отзыва";

      dispatch(getReviewsByCarFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const createReview = (data) => {
  return async (dispatch) => {
    try {
      dispatch(createReviewStarted());

      const response = await api.reviews.createReview(data);

      dispatch(createReviewSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка отзыва";

      dispatch(createReviewFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const updateReview = (id, data) => {
  return async (dispatch) => {
    try {
      dispatch(updateReviewStarted());

      const response = await api.reviews.updateReview(id, data);

      dispatch(updateReviewSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка отзыва";

      dispatch(updateReviewFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const deleteReview = (id) => {
  return async (dispatch) => {
    try {
      dispatch(deleteReviewStarted());

      const response = await api.reviews.deleteReview(id);

      dispatch(deleteReviewSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка отзыва";

      dispatch(deleteReviewFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

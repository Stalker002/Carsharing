import { api } from "../../api";
import {
  createCategoryFailed,
  createCategoryStarted,
  createCategorySuccess,
  deleteCategoryFailed,
  deleteCategoryStarted,
  deleteCategorySuccess,
  getCategoriesFailed,
  getCategoriesStarted,
  getCategoriesSuccess,
  updateCategoryFailed,
  updateCategoryStarted,
  updateCategorySuccess,
} from "../actionCreators/category";

export const getCategories = () => {
  return async (dispatch) => {
    try {
      dispatch(getCategoriesStarted());

      const response = await api.category.getCategory();

      dispatch(getCategoriesSuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка категории";

      dispatch(getCategoriesFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const createCategory = (data) => {
  return async (dispatch) => {
    try {
      dispatch(createCategoryStarted());

      const response = await api.category.createCategory(data);

      dispatch(createCategorySuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка категории";

      dispatch(createCategoryFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const updateCategory = (id, data) => {
  return async (dispatch) => {
    try {
      dispatch(updateCategoryStarted());

      const response = await api.category.updateCategory(id, data);

      dispatch(updateCategorySuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка категории";

      dispatch(updateCategoryFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

export const deleteCategory = (id) => {
  return async (dispatch) => {
    try {
      dispatch(deleteCategoryStarted());

      const response = await api.category.deleteCategory(id);

      dispatch(deleteCategorySuccess(response.data));
      return { success: true, data: response.data };
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Неизвестная ошибка категории";

      dispatch(deleteCategoryFailed(error));

      return { success: false, message: errorMessage };
    }
  };
};

import { api } from "../../api";
import { createCategoryFailed, createCategoryStarted, createCategorySuccess, deleteCategoryFailed, deleteCategoryStarted, deleteCategorySuccess, getCategoriesFailed, getCategoriesStarted, getCategoriesSuccess, updateCategoryFailed, updateCategoryStarted, updateCategorySuccess } from "../actionCreators/category";

export const getCategories = () => {
    return async (dispatch) => {
        try {
            dispatch(getCategoriesStarted());

            const response = await api.category.getCategory();

            dispatch(getCategoriesSuccess(response.data));
        } 
        catch (error) {
            dispatch(getCategoriesFailed(error));
        }
    };
};

export const createCategory = (data) => {
    return async (dispatch) => {
        try {
            dispatch(createCategoryStarted());

            const response = await api.category.createCategory(data);

            dispatch(createCategorySuccess(response.data));
        } 
        catch (error) {
            dispatch(createCategoryFailed(error));
        }
    };
};

export const updateCategory = (id, data) => {
  return async (dispatch) => {
    try {
      dispatch(updateCategoryStarted());

      const response = await api.category.updateCategory(id, data);

      dispatch(updateCategorySuccess(response.data));
    } 
    catch (error) {
      dispatch(updateCategoryFailed(error));
    }
  };
};

export const deleteCategory = (id) => {
  return async (dispatch) => {
    try {
      dispatch(deleteCategoryStarted());

      const response = await api.category.deleteCategory(id);

      dispatch(deleteCategorySuccess(response.data));
    } 
    catch (error) {
      dispatch(deleteCategoryFailed(error));
    }
  };
};
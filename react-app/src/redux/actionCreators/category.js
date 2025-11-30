export const GET_CATEGORIES_SUCCESS = "GET_CATEGORIES_SUCCESS";
export const GET_CATEGORIES_FAILED = "GET_CATEGORIES_FAILED";
export const GET_CATEGORIES_STARTED = "GET_CATEGORIES_STARTED";

export const POST_CATEGORY_STARTED = "POST_CATEGORY_STARTED";
export const POST_CATEGORY_SUCCESS = "POST_CATEGORY_SUCCESS";
export const POST_CATEGORY_FAILED = "POST_CATEGORY_FAILED";

export const PUT_CATEGORY_STARTED = "PUT_CATEGORY_STARTED";
export const PUT_CATEGORY_SUCCESS = "PUT_CATEGORY_SUCCESS";
export const PUT_CATEGORY_FAILED = "PUT_CATEGORY_FAILED";

export const DELETE_CATEGORY_STARTED = "DELETE_CATEGORY_STARTED";
export const DELETE_CATEGORY_SUCCESS = "DELETE_CATEGORY_SUCCESS";
export const DELETE_CATEGORY_FAILED = "DELETE_CATEGORY_FAILED";

export const getCategoriessStarted = () => ({
    type: GET_CATEGORIES_STARTED
});
export const getCategoriessSuccess = (categories) => ({
    type: GET_CATEGORIES_SUCCESS,
    payload: categories,
});
export const getCategoriessFailed = (error) => ({
    type: GET_CATEGORIES_FAILED,
    payload: error,
});

export const createCategoryStarted = () => ({
    type: POST_CATEGORY_STARTED,
});
export const createCategorySuccess = (category) => ({
    type: POST_CATEGORY_SUCCESS,
    payload: category,
});
export const createCategoryFailed = (error) => ({
    type: POST_CATEGORY_FAILED,
    payload: error,
});

export const updateCategoryStarted = () => ({
    type: PUT_CATEGORY_STARTED,
});
export const updateCategorySuccess = (category) => ({
    type: PUT_CATEGORY_SUCCESS,
    payload: category,
});
export const updateCategoryFailed = (error) => ({
    type: PUT_CATEGORY_FAILED,
    payload: error,
});

export const deleteCategoryStarted = () => ({
    type: DELETE_CATEGORY_STARTED,
});
export const deleteCategorySuccess = (id) => ({
    type: DELETE_CATEGORY_SUCCESS,
    payload: id,
});
export const deleteCategoryFailed = (error) => ({
    type: DELETE_CATEGORY_FAILED,
    payload: error,
});
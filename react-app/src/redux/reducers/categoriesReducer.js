import { GET_CATEGORIES_STARTED, GET_CATEGORIES_SUCCESS, GET_CATEGORIES_FAILED, POST_CATEGORY_STARTED, POST_CATEGORY_SUCCESS, POST_CATEGORY_FAILED, PUT_CATEGORY_STARTED, PUT_CATEGORY_SUCCESS, PUT_CATEGORY_FAILED, DELETE_CATEGORY_STARTED, DELETE_CATEGORY_SUCCESS, DELETE_CATEGORY_FAILED} from "../actionCreators/categories";

const initialState = {
  categories: [],
  isCategoriesLoading: false,
  isCategoriesCreateLoading: false,
  isCategoriesUpdateLoading: false,
  isCategoriesDeleteLoading: false
};

export const categoriesReducer = (state = initialState, action) => {
  switch (action.type) {
    case GET_CATEGORIES_STARTED:
      return { 
        ...state, 
        isCategoriesLoading: true 
      };
    case GET_CATEGORIES_SUCCESS:
      return { 
        ...state, 
        categories: action.payload, 
        isCategoriesLoading: false 
      };
    case GET_CATEGORIES_FAILED:
      return { 
        ...state, 
        isCategoriesLoading: false 
      };

    case POST_CATEGORY_STARTED:
      return { 
        ...state, 
        isCategoriesCreateLoading: true 
      };
    case POST_CATEGORY_SUCCESS:
      return { 
        ...state, 
        categories: [...state.categories, action.payload], 
        isCategoriesCreateLoading: false 
      };
    case POST_CATEGORY_FAILED:
      return { 
        ...state, 
        isCategoriesCreateLoading: false 
      };

    case PUT_CATEGORY_STARTED:
      return { 
        ...state, 
        isCategoriesUpdateLoading: true 
      };
    case PUT_CATEGORY_SUCCESS:
      return {
        ...state,
        categories: state.categories.map(c => c.id === action.payload.id 
            ? action.payload : c),
        isCategoriesUpdateLoading: false
      };
    case PUT_CATEGORY_FAILED:
      return { 
        ...state, 
        isCategoriesUpdateLoading: false 
      };

    case DELETE_CATEGORY_STARTED:
      return { 
        ...state, 
        isCategoriesDeleteLoading: true 
      };
    case DELETE_CATEGORY_SUCCESS:
      return {
        ...state,
        categories: state.categories.filter(c => c.id !== action.payload),
        isCategoriesDeleteLoading: false
      };
    case DELETE_CATEGORY_FAILED:
      return { 
        ...state, 
        isCategoriesDeleteLoading: false 
      };

    default:
      return state;
  }
};

import { GET_CLIENT_DOCUMENTS_STARTED, GET_CLIENT_DOCUMENTS_SUCCESS, GET_CLIENT_DOCUMENTS_FAILED, POST_CLIENT_DOCUMENT_STARTED, POST_CLIENT_DOCUMENT_SUCCESS, POST_CLIENT_DOCUMENT_FAILED, DELETE_CLIENT_DOCUMENT_STARTED, DELETE_CLIENT_DOCUMENT_SUCCESS, DELETE_CLIENT_DOCUMENT_FAILED} from "../actionCreators/clientDocuments";

const initialState = {
  documents: [],
  isDocumentsLoading: false,
  isDocumentsCreateLoading: false,
  isDocumentsDeleteLoading: false
};

export const clientDocumentsReducer = (state = initialState, action) => {
  switch (action.type) {
    case GET_CLIENT_DOCUMENTS_STARTED:
      return { 
        ...state, 
        isDocumentsLoading: true 
    };
    case GET_CLIENT_DOCUMENTS_SUCCESS:
      return { 
        ...state, 
        documents: action.payload, 
        isDocumentsLoading: false 
    };
    case GET_CLIENT_DOCUMENTS_FAILED:
      return { 
        ...state, 
        isDocumentsLoading: false 
    };

    case POST_CLIENT_DOCUMENT_STARTED:
      return { 
        ...state, 
        isDocumentsCreateLoading: true 
    };
    case POST_CLIENT_DOCUMENT_SUCCESS:
      return { 
        ...state, 
        documents: [...state.documents, action.payload], 
        isDocumentsCreateLoading: false 
    };
    case POST_CLIENT_DOCUMENT_FAILED:
      return { 
        ...state, 
        isDocumentsCreateLoading: false 
    };

    case DELETE_CLIENT_DOCUMENT_STARTED:
      return { 
        ...state, 
        isDocumentsDeleteLoading: true 
    };
    case DELETE_CLIENT_DOCUMENT_SUCCESS:
      return {
        ...state,
        documents: state.documents.filter(d => d.id !== action.payload),
        isDocumentsDeleteLoading: false
      };
    case DELETE_CLIENT_DOCUMENT_FAILED:
      return { 
        ...state, 
        isDocumentsDeleteLoading: false 
    };

    default:
      return state;
  }
};

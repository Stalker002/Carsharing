import { api } from "../../api";
import { createClientDocumentFailed, createClientDocumentStarted, createClientDocumentSuccess, deleteClientDocumentFailed, deleteClientDocumentStarted, deleteClientDocumentSuccess, getClientDocumentsFailed, getClientDocumentsStarted, getClientDocumentsSuccess, updateClientDocumentFailed, updateClientDocumentStarted, updateClientDocumentSuccess } from "../actionCreators/clientDocuments";

export const getClientDocuments = () => {
    return async (dispatch) => {
        try {
            dispatch(getClientDocumentsStarted());

            const response = await api.clientDocuments.getClientDocuments();

            dispatch(getClientDocumentsSuccess(response.data));
        } 
        catch (error) {
            dispatch(getClientDocumentsFailed(error));
        }
    };
};

export const createClientDocument = (data) => {
    return async (dispatch) => {
        try {
            dispatch(createClientDocumentStarted());

            const response = await api.clientDocuments.createClientDocument(data);

            dispatch(createClientDocumentSuccess(response.data));
        } 
        catch (error) {
            dispatch(createClientDocumentFailed(error));
        }
    };
};

export const updateClientDocument = (id, data) => {
  return async (dispatch) => {
    try {
      dispatch(updateClientDocumentStarted());

      const response = await api.clientDocuments.updateClientDocument(id, data);

      dispatch(updateClientDocumentSuccess(response.data));
    } 
    catch (error) {
      dispatch(updateClientDocumentFailed(error));
    }
  };
};

export const deleteClientDocument = (id) => {
  return async (dispatch) => {
    try {
      dispatch(deleteClientDocumentStarted());

      const response = await api.clientDocuments.deleteClientDocument(id);

      dispatch(deleteClientDocumentSuccess(response.data));
    } 
    catch (error) {
      dispatch(deleteClientDocumentFailed(error));
    }
  };
};
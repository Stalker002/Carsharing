export const GET_CLIENT_DOCUMENTS_SUCCESS = "GET_CLIENT_DOCUMENTS_SUCCESS";
export const GET_CLIENT_DOCUMENTS_FAILED = "GET_CLIENT_DOCUMENTS_FAILED";
export const GET_CLIENT_DOCUMENTS_STARTED = "GET_CLIENT_DOCUMENTS_STARTED";

export const POST_CLIENT_DOCUMENT_STARTED = "POST_CLIENT_DOCUMENT_STARTED";
export const POST_CLIENT_DOCUMENT_SUCCESS = "POST_CLIENT_DOCUMENT_SUCCESS";
export const POST_CLIENT_DOCUMENT_FAILED = "POST_CLIENT_DOCUMENT_FAILED";

export const PUT_CLIENT_DOCUMENT_STARTED = "PUT_CLIENT_DOCUMENT_STARTED";
export const PUT_CLIENT_DOCUMENT_SUCCESS = "PUT_CLIENT_DOCUMENT_SUCCESS";
export const PUT_CLIENT_DOCUMENT_FAILED = "PUT_CLIENT_DOCUMENT_FAILED";

export const DELETE_CLIENT_DOCUMENT_STARTED = "DELETE_CLIENT_DOCUMENT_STARTED";
export const DELETE_CLIENT_DOCUMENT_SUCCESS = "DELETE_CLIENT_DOCUMENT_SUCCESS";
export const DELETE_CLIENT_DOCUMENT_FAILED = "DELETE_CLIENT_DOCUMENT_FAILED";

export const getClientDocumentsStarted = () => ({
    type: GET_CLIENT_DOCUMENTS_STARTED
});
export const getClientDocumentsSuccess = (documents) => ({
    type: GET_CLIENT_DOCUMENTS_SUCCESS,
    payload: documents,
});
export const getClientDocumentsFailed = (error) => ({
    type: GET_CLIENT_DOCUMENTS_FAILED,
    payload: error,
});

export const createClientDocumentStarted = () => ({
    type: POST_CLIENT_DOCUMENT_STARTED,
});
export const createClientDocumentSuccess = (document) => ({
    type: POST_CLIENT_DOCUMENT_SUCCESS,
    payload: document,
});
export const createClientDocumentFailed = (error) => ({
    type: POST_CLIENT_DOCUMENT_FAILED,
    payload: error,
});

export const updateClientDocumentStarted = () => ({
    type: PUT_CLIENT_DOCUMENT_STARTED,
});
export const updateClientDocumentSuccess = (document) => ({
    type: PUT_CLIENT_DOCUMENT_SUCCESS,
    payload: document,
});
export const updateClientDocumentFailed = (error) => ({
    type: PUT_CLIENT_DOCUMENT_FAILED,
    payload: error,
});

export const deleteClientDocumentStarted = () => ({
    type: DELETE_CLIENT_DOCUMENT_STARTED,
});
export const deleteClientDocumentSuccess = (id) => ({
    type: DELETE_CLIENT_DOCUMENT_SUCCESS,
    payload: id,
});
export const deleteClientDocumentFailed = (error) => ({
    type: DELETE_CLIENT_DOCUMENT_FAILED,
    payload: error,
});
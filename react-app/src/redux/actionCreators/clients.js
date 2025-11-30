export const GET_CLIENTS_SUCCESS = "GET_CLIENTS_SUCCESS";
export const GET_CLIENTS_FAILED = "GET_CLIENTS_FAILED";
export const GET_CLIENTS_STARTED = "GET_CLIENTS_STARTED";
export const SET_CLIENTS_TOTAL = "SET_CLIENTS_TOTAL";

export const GET_MY_CLIENTS_STARTED = "GET_MY_CLIENTS_STARTED";
export const GET_MY_CLIENTS_SUCCESS = "GET_MY_CLIENTS_SUCCESS";
export const GET_MY_CLIENTS_FAILED = "GET_MY_CLIENTS_FAILED";

export const GET_MY_CLIENT_DOCUMENTS_STARTED = "GET_MY_CLIENT_DOCUMENTS_STARTED";
export const GET_MY_CLIENT_DOCUMENTS_SUCCESS = "GET_MY_CLIENT_DOCUMENTS_SUCCESS";
export const GET_MY_CLIENT_DOCUMENTS_FAILED = "GET_MY_CLIENT_DOCUMENTS_FAILED";

export const POST_CLIENT_STARTED = "POST_CLIENT_STARTED";
export const POST_CLIENT_SUCCESS = "POST_CLIENT_SUCCESS";
export const POST_CLIENT_FAILED = "POST_CLIENT_FAILED";

export const PUT_CLIENT_STARTED = "PUT_CLIENT_STARTED";
export const PUT_CLIENT_SUCCESS = "PUT_CLIENT_SUCCESS";
export const PUT_CLIENT_FAILED = "PUT_CLIENT_FAILED";

export const DELETE_CLIENT_STARTED = "DELETE_CLIENT_STARTED";
export const DELETE_CLIENT_SUCCESS = "DELETE_CLIENT_SUCCESS";
export const DELETE_CLIENT_FAILED = "DELETE_CLIENT_FAILED";

export const getClientsStarted = () => ({
    type: GET_CLIENTS_STARTED
});
export const getClientsSuccess = (clients) => ({
    type: GET_CLIENTS_SUCCESS,
    payload: clients,
});
export const getClientsFailed = (error) => ({
    type: GET_CLIENTS_FAILED,
    payload: error,
});
export const setClientsTotal = (total) => ({
    type: SET_CLIENTS_TOTAL,
    payload: total,
});

export const getMyClientsStarted = () => ({
    type: GET_MY_CLIENTS_STARTED
});
export const getMyClientsSuccess = (clients) => ({
    type: GET_MY_CLIENTS_SUCCESS,
    payload: clients,
});
export const getMyClientsFailed = (error) => ({
    type: GET_MY_CLIENTS_FAILED,
    payload: error,
});

export const getMyClientDocumentsStarted = () => ({
    type: GET_MY_CLIENT_DOCUMENTS_STARTED
});
export const getMyClientDocumentsSuccess = (clients) => ({
    type: GET_MY_CLIENT_DOCUMENTS_SUCCESS,
    payload: clients,
});
export const getMyClientDocumentsFailed = (error) => ({
    type: GET_MY_CLIENT_DOCUMENTS_FAILED,
    payload: error,
});

export const createClientStarted = () => ({
    type: POST_CLIENT_STARTED,
});
export const createClientSuccess = (client) => ({
    type: POST_CLIENT_SUCCESS,
    payload: client,
});
export const createClientFailed = (error) => ({
    type: POST_CLIENT_FAILED,
    payload: error,
});

export const updateClientStarted = () => ({
    type: PUT_CLIENT_STARTED,
});
export const updateClientSuccess = (client) => ({
    type: PUT_CLIENT_SUCCESS,
    payload: client,
});
export const updateClientFailed = (error) => ({
    type: PUT_CLIENT_FAILED,
    payload: error,
});

export const deleteClientStarted = () => ({
    type: DELETE_CLIENT_STARTED,
});
export const deleteClientSuccess = (id) => ({
    type: DELETE_CLIENT_SUCCESS,
    payload: id,
});
export const deleteClientFailed = (error) => ({
    type: DELETE_CLIENT_FAILED,
    payload: error,
});MYDOCUMENTS_
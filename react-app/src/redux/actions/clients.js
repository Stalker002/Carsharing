import { api } from "../../api";
import { deleteUser } from "../../api/users";
import { createClientFailed, createClientStarted, createClientSuccess, deleteClientFailed, deleteClientStarted, deleteClientSuccess, getClientDocumentFailed, getClientDocumentStarted, getClientDocumentSuccess, getClientsFailed, getClientsStarted, getClientsSuccess, getMyClientFailed, getMyClientStarted, getMyClientSuccess, getMyDocumentsFailed, getMyDocumentsStarted, getMyDocumentsSuccess, setClientsTotal, updateClientFailed, updateClientStarted, updateClientSuccess } from "../actionCreators/clients";

export const getClients = (page = 1) => {
    return async (dispatch) => {
        try {
            dispatch(getClientsStarted());

            const response = await api.clients.getClients({
                params: {
                    _page: page,
                    _limit: 25,
                },
            });

            const totalCount = parseInt(response.headers["x-total-count"], 10);
            if (!isNaN(totalCount)) {
                dispatch(setClientsTotal(totalCount));
            }

            dispatch(getClientsSuccess({
                data: response.data,
                page,
            }));
        }
        catch (error) {
            dispatch(getClientsFailed(error));
        }
    };
};

export const getMyClient = () => {
    return async (dispatch) => {
        try {
            dispatch(getMyClientStarted())

            const response = await api.clients.getMyClient();

            dispatch(getMyClientSuccess(response.data));
        }
        catch (error) {
            dispatch(getMyClientFailed(error));
        }
    };
};

export const getMyDocuments = () => {
    return async (dispatch) => {
        try {
            dispatch(getMyDocumentsStarted())

            const response = await api.clients.getMyDocuments();

            dispatch(getMyDocumentsSuccess(response.data));
        }
        catch (error) {
            dispatch(getMyDocumentsFailed(error));
        }
    };
};

export const getClientDocument = (clientId) => {
    return async (dispatch) => {
        try {
            dispatch(getClientDocumentStarted())

            const response = await api.clients.getClientDocument(clientId);

            dispatch(getClientDocumentSuccess(response.data));
        }
        catch (error) {
            dispatch(getClientDocumentFailed(error));
        }
    };
};

export const createClient = (data) => {
    return async (dispatch) => {
        try {
            dispatch(createClientStarted());

            const response = await api.clients.createClient(data);

            dispatch(createClientSuccess(response.data));
            return { success: true, data: response.data };
        }
        catch (error) {
            const errorMessage = 
                error.response?.data?.message || // Если бэк вернул объект с полем 'message'
                error.response?.data ||          // Если бэк вернул простую строку ошибки (как в твоем C# коде!)
                error.message ||                 // Сетевая ошибка (например, "Network Error")
                "Неизвестная ошибка регистрации";

            dispatch(createClientFailed(error));

            return { success: false, message: errorMessage };
        }
    };
};

export const updateClient = (id, data) => {
    return async (dispatch) => {
        try {
            dispatch(updateClientStarted());

            const response = await api.clients.updateClient(id, data);

            dispatch(updateClientSuccess(response.data));
        }
        catch (error) {
            dispatch(updateClientFailed(error));
        }
    };
};

export const deleteClient = (id) => {
    return async (dispatch) => {
        try {
            dispatch(deleteClientStarted());

            const response = await api.clients.deleteClient(id);

            dispatch(deleteClientSuccess(response.data));
        }
        catch (error) {
            dispatch(deleteClientFailed(error));
        }
    };
};
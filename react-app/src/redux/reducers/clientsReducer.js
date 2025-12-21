import { DELETE_CLIENT_FAILED, DELETE_CLIENT_STARTED, DELETE_CLIENT_SUCCESS, GET_CLIENT_BY_USER_FAILED, GET_CLIENT_BY_USER_STARTED, GET_CLIENT_BY_USER_SUCCESS, GET_CLIENT_DOCUMENTS_FAILED, GET_CLIENT_DOCUMENTS_STARTED, GET_CLIENT_DOCUMENTS_SUCCESS, GET_CLIENTS_FAILED, GET_CLIENTS_STARTED, GET_CLIENTS_SUCCESS, GET_MY_CLIENT_FAILED, GET_MY_CLIENT_STARTED, GET_MY_CLIENT_SUCCESS, GET_MY_DOCUMENTS_FAILED, GET_MY_DOCUMENTS_STARTED, GET_MY_DOCUMENTS_SUCCESS, POST_CLIENT_FAILED, POST_CLIENT_STARTED, POST_CLIENT_SUCCESS, PUT_CLIENT_FAILED, PUT_CLIENT_STARTED, PUT_CLIENT_SUCCESS, SET_CLIENTS_TOTAL } from "../actionCreators/clients";

const initialState = {
    client: {},
    clients: [],
    myClient: {},
    clientDocument: {},
    myDocument: [],
    isClientsLoading: false,
    isUpdateClientLoading: false,
    isDeleteClientLoading: false,
    clientsTotal: 0
};

export const clientsReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_CLIENTS_STARTED:
            return {
                ...state,
                isClientsLoading: true,
            };
        case GET_CLIENTS_SUCCESS:
            return {
                ...state,
                clients: action.payload.page === 1
                    ? action.payload.data
                    : [...state.clients, ...action.payload.data],
                isClientsLoading: false,
            };
        case GET_CLIENTS_FAILED:
            return {
                ...state,
                isClientsLoading: false,
            };
        case SET_CLIENTS_TOTAL:
            return {
                ...state,
                clientsTotal: action.payload,
            };

        case GET_MY_CLIENT_STARTED:
            return {
                ...state,
                isClientsLoading: true,
            };
        case GET_MY_CLIENT_SUCCESS:
            return {
                ...state,
                myClient: action.payload[0],
                isClientsLoading: false,
            };
        case GET_MY_CLIENT_FAILED:
            return {
                ...state,
                isClientsLoading: false,
            };

        case GET_CLIENT_BY_USER_STARTED:
            return {
                ...state,
                isClientsLoading: true,
            };
        case GET_CLIENT_BY_USER_SUCCESS:
            return {
                ...state,
                client: action.payload[0],
                isClientsLoading: false,
            };
        case GET_CLIENT_BY_USER_FAILED:
            return {
                ...state,
                isClientsLoading: false,
            };

        case GET_MY_DOCUMENTS_STARTED:
            return {
                ...state,
                isClientsLoading: true,
            };
        case GET_MY_DOCUMENTS_SUCCESS:
            return {
                ...state,
                myDocument: action.payload,
                isClientsLoading: false,
            };
        case GET_MY_DOCUMENTS_FAILED:
            return {
                ...state,
                isClientsLoading: false,
            };

        case GET_CLIENT_DOCUMENTS_STARTED:
            return {
                ...state,
                isClientsLoading: true,
            };
        case GET_CLIENT_DOCUMENTS_SUCCESS:
            return {
                ...state,
                clientDocument: action.payload,
                isClientsLoading: false,
            };
        case GET_CLIENT_DOCUMENTS_FAILED:
            return {
                ...state,
                isClientsLoading: false,
            };

        case POST_CLIENT_STARTED:
            return {
                ...state,
                isClientsLoading: true,
            };
        case POST_CLIENT_SUCCESS:
            return {
                ...state,
                clients: [action.payload, ...state.clients],
                isClientsLoading: false,
            };
        case POST_CLIENT_FAILED:
            return {
                ...state,
                isClientsLoading: false,
            };

        case PUT_CLIENT_STARTED:
            return {
                ...state,
                isUpdateClientLoading: true
            };
        case PUT_CLIENT_SUCCESS:
            return {
                ...state,
                clients: state.clients.map(b => b.id === action.payload.id ? action.payload : b),
                isUpdateClientLoading: false
            };
        case PUT_CLIENT_FAILED:
            return {
                ...state,
                isUpdateClientLoading: false
            };

        case DELETE_CLIENT_STARTED:
            return {
                ...state,
                isDeleteClientLoading: true
            };
        case DELETE_CLIENT_SUCCESS:
            return {
                ...state,
                clients: state.clients.filter(b => b.id !== action.payload),
                isDeleteClientLoading: false
            };
        case DELETE_CLIENT_FAILED:
            return {
                ...state,
                isDeleteClientLoading: false
            };

        default:
            return state;
    }
};

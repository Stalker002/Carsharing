export const GET_MAINTENANCES_SUCCESS = "GET_MAINTENANCES_SUCCESS";
export const GET_MAINTENANCES_FAILED = "GET_MAINTENANCES_FAILED";
export const GET_MAINTENANCES_STARTED = "GET_MAINTENANCES_STARTED";

export const GET_MAINTENANCES_BY_CAR_STARTED = "GET_MAINTENANCES_BY_CAR_STARTED";
export const GET_MAINTENANCES_BY_CAR_SUCCESS = "GET_MAINTENANCES_BY_CAR_SUCCESS";
export const GET_MAINTENANCES_BY_CAR_FAILED = "GET_MAINTENANCES_BY_CAR_FAILED";

export const GET_MAINTENANCE_BY_DATE_RANGE_STARTED = "GET_MAINTENANCE_BY_DATE_RANGE_STARTED";
export const GET_MAINTENANCE_BY_DATE_RANGE_SUCCESS = "GET_MAINTENANCE_BY_DATE_RANGE_SUCCESS";
export const GET_MAINTENANCE_BY_DATE_RANGE_FAILED = "GET_MAINTENANCE_BY_DATE_RANGE_FAILED";

export const POST_MAINTENANCE_STARTED = "POST_MAINTENANCE_STARTED";
export const POST_MAINTENANCE_SUCCESS = "POST_MAINTENANCE_SUCCESS";
export const POST_MAINTENANCE_FAILED = "POST_MAINTENANCE_FAILED";

export const PUT_MAINTENANCE_STARTED = "PUT_MAINTENANCE_STARTED";
export const PUT_MAINTENANCE_SUCCESS = "PUT_MAINTENANCE_SUCCESS";
export const PUT_MAINTENANCE_FAILED = "PUT_MAINTENANCE_FAILED";

export const DELETE_MAINTENANCE_STARTED = "DELETE_MAINTENANCE_STARTED";
export const DELETE_MAINTENANCE_SUCCESS = "DELETE_MAINTENANCE_SUCCESS";
export const DELETE_MAINTENANCE_FAILED = "DELETE_MAINTENANCE_FAILED";

export const getMaintenancesStarted = () => ({
    type: GET_MAINTENANCES_STARTED
});
export const getMaintenancesSuccess = (maintenances) => ({
    type: GET_MAINTENANCES_SUCCESS,
    payload: maintenances,
});
export const getMaintenancesFailed = (error) => ({
    type: GET_MAINTENANCES_FAILED,
    payload: error,
});

export const getMaintenancesByCarStarted = () => ({
    type: GET_MAINTENANCES_BY_CAR_STARTED
});
export const getMaintenancesByCarSuccess = (maintenances) => ({
    type: GET_MAINTENANCES_BY_CAR_SUCCESS,
    payload: maintenances,
});
export const getMaintenancesByCarFailed = (error) => ({
    type: GET_MAINTENANCES_BY_CAR_FAILED,
    payload: error,
});

export const getMaintenanceByDateRangeStarted = () => ({
    type: GET_MAINTENANCE_BY_DATE_RANGE_STARTED
});
export const getMaintenanceByDateRangeSuccess = (maintenances) => ({
    type: GET_MAINTENANCE_BY_DATE_RANGE_SUCCESS,
    payload: maintenances,
});
export const getMaintenanceByDateRangeFailed = (error) => ({
    type: GET_MAINTENANCE_BY_DATE_RANGE_FAILED,
    payload: error,
});

export const createMaintenanceStarted = () => ({
    type: POST_MAINTENANCE_STARTED,
});
export const createMaintenanceSuccess = (maintenance) => ({
    type: POST_MAINTENANCE_SUCCESS,
    payload: maintenance,
});
export const createMaintenanceFailed = (error) => ({
    type: POST_MAINTENANCE_FAILED,
    payload: error,
});

export const updateMaintenanceStarted = () => ({
    type: PUT_MAINTENANCE_STARTED,
});
export const updateMaintenanceSuccess = (maintenance) => ({
    type: PUT_MAINTENANCE_SUCCESS,
    payload: maintenance,
});
export const updateMaintenanceFailed = (error) => ({
    type: PUT_MAINTENANCE_FAILED,
    payload: error,
});

export const deleteMaintenanceStarted = () => ({
    type: DELETE_MAINTENANCE_STARTED,
});
export const deleteMaintenanceSuccess = (id) => ({
    type: DELETE_MAINTENANCE_SUCCESS,
    payload: id,
});
export const deleteMaintenanceFailed = (error) => ({
    type: DELETE_MAINTENANCE_FAILED,
    payload: error,
});
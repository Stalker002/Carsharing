const URL = "/Maintenance";

export const getMaintenances = (config) => makeRequest({
    method: "GET",
    url: URL,
    ...config,
});

export const getMaintenanceByCar = (carId, config) => makeRequest({
    method: "GET",
    url: `${URL}/byCar/${carId}`,
    ...config
});

export const getMaintenanceByDateRange = (from, to, config) => makeRequest({
    method: "GET",
    url: `${URL}/byDate`,
    ...config
});

export const createMaintenance = (data) => makeRequest({
    method: "POST",
    url: URL,
    data,
});

export const updateMaintenance = (data) => makeRequest({
    method: "PUT",
    url: `${URL}/${id}`,
    data,
});

export const deleteMaintenance = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    ...config,
});
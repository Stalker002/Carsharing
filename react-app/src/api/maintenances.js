const URL = "/Maintenance";

export const getMaintenances = (config) => makeRequest({
    method: "GET",
    url: URL,
    withCredentials: true,
    ...config,
});

export const getMaintenanceByCar = (carId, config) => makeRequest({
    method: "GET",
    url: `${URL}/byCar/${carId}`,
    withCredentials: true,
    ...config
});

export const getMaintenanceByDateRange = (from, to, config) => makeRequest({
    method: "GET",
    url: `${URL}/byDate`,
    withCredentials: true,
    ...config
});

export const createMaintenance = (data) => makeRequest({
    method: "POST",
    url: URL,
    withCredentials: true,
    data,
});

export const updateMaintenance = (id, data) => makeRequest({
    method: "PUT",
    url: `${URL}/${id}`,
    withCredentials: true,
    data,
});

export const deleteMaintenance = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    withCredentials: true,
    ...config,
});
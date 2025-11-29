const URL = "/Insurances";

export const getInsurances = (config) => makeRequest({
    method: "GET",
    url: URL,
    ...config,
});

export const getInsuranceByCar = (carId, config) => makeRequest({
    method: "GET",
    url: `${URL}/byCarId/${carId}`,
    ...config
});

export const getActiveInsuranceByCar = (carId, config) => makeRequest({
    method: "GET",
    url: `${URL}/ActiveByCarId/${carId}`,
    ...config
});

export const createInsurance = (data) => makeRequest({
    method: "POST",
    url: URL,
    data,
});

export const deleteInsurancel = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    ...config,
});
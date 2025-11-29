const URL = "/Bills";

export const getBills = (config) => makeRequest({
    method: "GET",
    url: URL,
    ...config,
});

export const getMyBills = (userId, config) => makeRequest({
    method: "GET",
    url: `${URL}/byUser/${userId}`,
    ...config
});

export const getInfoBill = (id, config) => makeRequest({
    method: "GET",
    url: `${URL}/info/${id}`,
    ...config
});

export const createBill = (data) => makeRequest({
    method: "POST",
    url: URL,
    data,
});

export const deleteBill = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    ...config,
});
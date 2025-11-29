const URL = "/Payments";

export const getPayments = (config) => makeRequest({
    method: "GET",
    url: URL,
    ...config,
});

export const getPaymentsByBillId = (billId, config) => makeRequest({
    method: "GET",
    url: `${URL}/byBill/${billId}`,
    ...config
});

export const createPayment = (data) => makeRequest({
    method: "POST",
    url: URL,
    data,
});

export const deletePayment = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    ...config,
});
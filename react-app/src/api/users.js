const URL = "/Users";

export const loginUser = (config) => makeRequest({
    method: "POST",
    url: `${URL}/login`,
    withCredentials: true,
    ...config,
});

export const logoutUser = (config) => makeRequest({
    method: "POST",
    url: `${URL}/logout`,
    withCredentials: true,
    ...config,
});

export const getUsers = (config) => makeRequest({
    method: "GET",
    url: URL,
    ...config,
});

export const createUser = (data) => makeRequest({
    method: "POST",
    url: URL,
    withCredentials: true,
    data,
});

export const updateUser = (id, data) => makeRequest({
    method: "PUT",
    url: `${URL}/${id}`,
    withCredentials: true,
    data,
});

export const deleteUser = (id, config) => makeRequest({
    method: "DELETE",
    url: `${URL}/${id}`,
    withCredentials: true,
    ...config,
});
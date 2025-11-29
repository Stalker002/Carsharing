import { createBill, getBills, getInfoBill, getMyBills } from "./bills";
import { createUser, getUserById, getUsers } from "./users";

export const api = {
    bills: {
        getBills,
        getMyBills,
        getInfoBill,
        createBill
    },
    users: {
        getUsers,
        getUserById,
        createUser
    }
}
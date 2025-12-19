import { combineReducers } from "redux";
import { userReducer } from "./usersReducer";
import { billsReducer } from "./billsReducer";
import { bookingsReducer } from "./bookingsReducer";
import { carsReducer } from "./carsReducer";
import { categoriesReducer } from "./categoriesReducer";
import { clientsReducer } from "./clientsReducer";
import { finesReducer } from "./finesReducer";
import { insurancesReducer } from "./insurancesReducer";
import { maintenancesReducer } from "./maintenancesReducer";
import { paymentsReducer } from "./paymentsReducer";
import { promocodesReducer } from "./promocodesReducer";
import { statusesReducer } from "./statusesReducer";
import { reviewsReducer } from "./reviewsReducer";
import { tariffsReducer } from "./tariffsReducer";
import { tripsReducer } from "./tripsReducer";
import { modalReducer } from "./modalReducer";
import { clientDocumentsReducer } from "./clientDocumentsReducer";

export const rootReducer = combineReducers({
    bills: billsReducer,
    bookings: bookingsReducer,
    cars: carsReducer,
    categories: categoriesReducer,
    clientDocuments: clientDocumentsReducer,
    clients: clientsReducer,
    fines: finesReducer,
    insurances: insurancesReducer,
    maintenances: maintenancesReducer,
    modal: modalReducer,
    payments: paymentsReducer,
    promocodes: promocodesReducer,
    reviews: reviewsReducer,
    statuses: statusesReducer,
    tariffs: tariffsReducer,
    trips: tripsReducer,
    users: userReducer
})
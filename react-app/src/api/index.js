import { applyPromocode, createBill, deleteBill, getBills, getInfoBill, getMyBills, updateBill } from "./bills";
import { createBooking, deleteBooking, getBookingInfo, getBookings, getMyBookings, updateBooking } from "./bookings";
import { createCar, deleteCar, getCarByCategory, getCarInfo, getCarInfoAdmin, getCars, updateCar } from "./cars";
import { createCategory, deleteCategory, getCategory, updateCategory } from "./category";
import { createClientDocument, deleteClientDocument, getClientDocuments, updateClientDocument } from "./clientDocuments";
import { createClient, deleteClient, getClientByUserId, getClientDocument, getClients, getMyClient, getMyDocuments, updateClient } from "./clients";
import { createFine, deleteFine, getFines, getFinesByTrip, updateFine } from "./fines";
import { createInsurance, deleteInsurance, getActiveInsuranceByCar, getInsuranceByCar, getInsurances, updateInsurance } from "./insurances";
import { createMaintenance, deleteMaintenance, getMaintenanceByCar, getMaintenanceByDateRange, getMaintenances, updateMaintenance } from "./maintenances";
import { createPayment, deletePayment, getPayments, getPaymentsByBillId, updatePayment } from "./payments";
import { createPromocode, deletePromocode, getActivePromocodes, getPromocodes, updatePromocode } from "./promocodes";
import { createReview, deleteReview, getReviews, getReviewsByCar, updateReview } from "./reviews";
import { getStatuses } from "./statuses";
import { getTariffs } from "./tariff";
import { cancelTrip, createTrip, deleteTrip, finishTrip, getActiveTrip, getMyTrips, getTrips, getTripWithInfo, updateTrip } from "./trips";
import { createUser, deleteUser, getMyUser, getUsers, loginUser, logoutUser, updateUser } from "./users";

export const api = {
    bills: {
        getBills,
        getMyBills,
        getInfoBill,
        applyPromocode,
        createBill,
        updateBill,
        deleteBill
    },
    bookings: {
        getBookings,
        getMyBookings,
        getBookingInfo,
        createBooking,
        updateBooking,
        deleteBooking
    },
    cars: {
        getCars,
        getCarInfo,
        getCarInfoAdmin,
        getCarByCategory,
        createCar,
        updateCar,
        deleteCar
    },
    category: {
        getCategory,
        createCategory,
        updateCategory,
        deleteCategory
    },
    clientDocuments: {
        getClientDocuments,
        createClientDocument,
        updateClientDocument,
        deleteClientDocument
    },
    clients: {
        getClients,
        getMyDocuments,
        getClientDocument,
        getMyClient,
        getClientByUserId,
        createClient,
        updateClient,
        deleteClient
    },
    fines: {
        getFines,
        getFinesByTrip,
        createFine,
        updateFine,
        deleteFine
    },
    insurances: {
        getInsurances,
        getInsuranceByCar,
        getActiveInsuranceByCar,
        createInsurance,
        updateInsurance,
        deleteInsurance
    },
    maintenance: {
        getMaintenances,
        getMaintenanceByCar,
        getMaintenanceByDateRange,
        createMaintenance,
        updateMaintenance,
        deleteMaintenance
    },
    payments: {
        getPayments,
        getPaymentsByBillId,
        createPayment,
        updatePayment,
        deletePayment
    },
    promocodes: {
        getPromocodes,
        getActivePromocodes,
        createPromocode,
        updatePromocode,
        deletePromocode
    },
    reviews: {
        getReviews,
        getReviewsByCar,
        createReview,
        updateReview,
        deleteReview
    },
    statuses: {
        getStatuses
    },
    tariff: {
        getTariffs
    },
    trips: {
        getTrips,
        getMyTrips,
        getTripWithInfo,
        getActiveTrip,
        finishTrip,
        cancelTrip,
        createTrip,
        updateTrip,
        deleteTrip
    },
    users: {
        loginUser,
        logoutUser,
        getUsers,
        getMyUser,
        createUser,
        updateUser,
        deleteUser
    }
}
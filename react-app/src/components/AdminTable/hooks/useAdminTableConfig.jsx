import { useSelector } from "react-redux";

import {
  getUsers,
  createUser,
  updateUser,
  deleteUser,
} from "../../../redux/actions/users";
import {
  getCars,
  createCar,
  updateCar,
  deleteCar,
  getInfoCarAdmin,
} from "../../../redux/actions/cars";
import {
  getBills,
  createBill,
  updateBill,
  deleteBill,
} from "../../../redux/actions/bills";
import {
  getBookings,
  createBooking,
  updateBooking,
  deleteBooking,
} from "../../../redux/actions/bookings";
import {
  getClients,
  createClient,
  updateClient,
  deleteClient,
  getClientByUser,
} from "../../../redux/actions/clients";
import {
  getFines,
  createFine,
  updateFine,
  deleteFine,
} from "../../../redux/actions/fines";
import {
  getInsurances,
  createInsurance,
  updateInsurance,
  deleteInsurance,
} from "../../../redux/actions/insurance";
import {
  getPayments,
  createPayment,
  updatePayment,
  deletePayment,
} from "../../../redux/actions/payments";
import {
  getPromocodes,
  createPromocode,
  updatePromocode,
  deletePromocode,
} from "../../../redux/actions/promocodes";
import {
  getTrips,
  createTrip,
  updateTrip,
  deleteTrip,
  getTripWithInfo,
} from "../../../redux/actions/trips";

import {
  columnsBills,
  headTextBills,
  fieldsBills,
  columnsBookings,
  headTextBookings,
  fieldsBookings,
  columnsCars,
  headTextCars,
  fieldsCars,
  columnsClients,
  headTextClients,
  fieldsClients,
  columnsFines,
  headTextFines,
  fieldsFines,
  columnsInsurances,
  headTextInsurances,
  fieldsInsurances,
  columnsPayments,
  headTextPayments,
  fieldsPayments,
  columnsPromocodes,
  headTextPromocodes,
  fieldsPromocodes,
  columnsTrips,
  headTextTrips,
  fieldsTrips,
  columnsUsers,
  headTextUsers,
  fieldsUsers,
  columnsMaintenances,
  headTextMaintenances,
  fieldsMaintenances,
} from "../configs";
import {
  createMaintenance,
  deleteMaintenance,
  getMaintenances,
  updateMaintenance,
} from "../../../redux/actions/maintenance";

export const STATUS_FILTERS = {
    cars: [
      "Доступна",
      "Недоступна",
      "На обслуживании",
      "В ремонте",
    ],
    promocodes: ["Активен", "Истёк"],
    bookings: ["Активно", "Завершено", "Отменено"],
    bills: ["Не оплачен", "Частично оплачен", "Оплачен", "Отменен"],
    insurances: ["Активно", "Завершено", "Отменено"],
    trips: ["Ожидание начала", "В пути", "Завершена", "Отменена системой", "Требуется оплата",],
    fines: ["Начислен", "Ожидает оплаты", "Оплачен"],
  };

export const useAdminTableConfig = (activeTab) => {
  const users = useSelector((state) => state.users?.users || []);
  const totalUsers = useSelector((state) => state.users?.usersTotal || 0);

  const cars = useSelector((state) => state.cars?.cars || []);
  const totalCars = useSelector((state) => state.cars?.totalCars || 0);

  const bills = useSelector((state) => state.bills?.bills || []);
  const totalBills = useSelector((state) => state.bills?.billsTotal || 0);

  const bookings = useSelector((state) => state.bookings?.bookings || []);
  const totalBookings = useSelector(
    (state) => state.bookings?.bookingsTotal || 0
  );

  const clients = useSelector((state) => state.clients?.clients || []);
  const totalClients = useSelector((state) => state.clients?.clientsTotal || 0);

  const fines = useSelector((state) => state.fines?.fines || []);
  const totalFines = useSelector((state) => state.fines?.finesTotal || 0);

  const maintenances = useSelector(
    (state) => state.maintenances?.maintenances || []
  );

  const insurances = useSelector((state) => state.insurances?.insurances || []);
  const totalInsurances = useSelector(
    (state) => state.insurances?.insurancesTotal || 0
  );

  const payments = useSelector((state) => state.payments?.payments || []);
  const totalPayments = useSelector(
    (state) => state.payments?.paymentsTotal || 0
  );

  const promocodes = useSelector((state) => state.promocodes?.promocodes || []);
  const totalPromocodes = useSelector(
    (state) => state.promocodes?.promocodesTotal || 0
  );

  const trips = useSelector((state) => state.trips?.trips || []);
  const totalTrips = useSelector((state) => state.trips?.tripsTotal || 0);

  const tableConfig = {
    bills: {
      data: bills,
      total: totalBills,
      columns: columnsBills,
      headText: headTextBills,
      fields: fieldsBills,
      action: getBills,
      addAction: (data) =>
        createBill({
          ...data,
          tripId: Number(data.tripId),
          promocodeId: data.promocodeId ? Number(data.promocodeId) : null,
          statusId: Number(data.statusId),
          amount: Number(data.amount),
          remainingAmount: Number(data.remainingAmount),
          issueDate: data.issueDate ? new Date(data.issueDate).toISOString() : null,
        }),
      addTitle: "Выставление счета",
      updateAction: (id, data) =>
        updateBill(id, {
          ...data,
          tripId: Number(data.tripId),
          promocodeId: data.promocodeId ? Number(data.promocodeId) : null,
          statusId: Number(data.statusId),
          amount: Number(data.amount),
          remainingAmount: Number(data.remainingAmount),
          issueDate: data.issueDate ? new Date(data.issueDate).toISOString(): null
        }),
      editTitle: "Редактирование счета",
      deleteAction: (id) => deleteBill(id),
    },
    bookings: {
      data: bookings,
      total: totalBookings,
      columns: columnsBookings,
      headText: headTextBookings,
      fields: fieldsBookings,
      action: getBookings,
      addAction: (data) =>
        createBooking({
          ...data,
          statusId: Number(data.statusId),
          carId: Number(data.carId),
          clientId: Number(data.clientId),
          startTime: new Date(data.startTime).toISOString(),
          endTime: new Date(data.endTime).toISOString(),
        }),
      addTitle: "Создание бронирования",
      updateAction: (id, data) =>
        updateBooking(id, {
          ...data,
          statusId: Number(data.statusId),
          carId: Number(data.carId),
          clientId: Number(data.clientId),
          startTime: new Date(data.startTime).toISOString(),
          endTime: new Date(data.endTime).toISOString(),
        }),
      editTitle: "Изменение бронирования",
      deleteAction: (id) => deleteBooking(id),
    },
    cars: {
      data: cars,
      total: totalCars,
      columns: columnsCars,
      headText: headTextCars,
      fields: fieldsCars,
      action: getCars,
      addAction: (data) => createCar(data),
      addTitle: "Добавление авто (Full)",
      updateAction: (id, data) =>
        updateCar(id, {
          ...data,
          statusId: Number(data.statusId),
          tariffId: Number(data.tariffId),
          categoryId: Number(data.categoryId),
          fuelLevel: Number(data.fuelLevel),
          tariffName: data.tariffName || data.name,
          image: data.image,
        }),
      editTitle: "Редактирование параметров",
      deleteAction: (id) => deleteCar(id),
      detailAction: (id) => getInfoCarAdmin(id),
    },
    clients: {
      data: clients,
      total: totalClients,
      columns: columnsClients,
      headText: headTextClients,
      fields: fieldsClients,
      action: getClients,
      addAction: (data) => createClient({ ...data }),
      addTitle: "Регистрация нового клиента",
      updateAction: (id, data) =>
        updateClient(id, {
          userId: Number(data.userId),
          name: data.name,
          surname: data.surname,
          phoneNumber: data.phoneNumber,
          email: data.email,
        }),
      editTitle: "Редактирование профиля клиента",
      deleteAction: (id) => deleteClient(id),
    },
    fines: {
      data: fines,
      total: totalFines,
      columns: columnsFines,
      headText: headTextFines,
      fields: fieldsFines,
      action: getFines,
      addAction: (data) =>
        createFine({
          ...data,
          tripId: Number(data.tripId),
          statusId: Number(data.statusId),
          amount: Number(data.amount),
          date: new Date(data.date).toISOString(),
        }),
      addTitle: "Регистрация штрафа",
      updateAction: (id, data) =>
        updateFine(id, {
          ...data,
          tripId: Number(data.tripId),
          statusId: Number(data.statusId),
          amount: Number(data.amount),
          date: new Date(data.date).toISOString(),
        }),
      editTitle: "Редактирование штрафа",
      deleteAction: (id) => deleteFine(id),
    },
    insurance: {
      data: insurances,
      total: totalInsurances,
      columns: columnsInsurances,
      headText: headTextInsurances,
      fields: fieldsInsurances,
      action: getInsurances,
      addAction: (data) =>
        createInsurance({
          ...data,
          carId: Number(data.carId),
          statusId: Number(data.statusId),
          cost: Number(data.cost),
        }),
      addTitle: "Оформление страховки",
      updateAction: (id, data) =>
        updateInsurance(id, {
          ...data,
          carId: Number(data.carId),
          statusId: Number(data.statusId),
          cost: Number(data.cost),
        }),
      editTitle: "Редактирование страховки",
      deleteAction: (id) => deleteInsurance(id),
    },
    maintenance: {
      data: maintenances,
      columns: columnsMaintenances,
      headText: headTextMaintenances,
      fields: fieldsMaintenances,
      action: getMaintenances,
      addAction: (data) =>
        createMaintenance({
          ...data,
          carId: Number(data.carId),
          cost: Number(data.cost),
        }),
      addTitle: "Запись на обслуживание",
      updateAction: (id, data) =>
        updateMaintenance(id, {
          ...data,
          carId: Number(data.carId),
          cost: Number(data.cost),
        }),
      editTitle: "Редактирование записи ТО",
      deleteAction: (id) => deleteMaintenance(id),
    },
    payments: {
      data: payments,
      total: totalPayments,
      columns: columnsPayments,
      headText: headTextPayments,
      fields: fieldsPayments,
      action: getPayments,
      addAction: (data) =>
        createPayment({
          ...data,
          billId: Number(data.billId),
          sum: Number(data.sum),
          date: new Date(data.date).toISOString(),
        }),
      addTitle: "Регистрация платежа",
      updateAction: (id, data) =>
        updatePayment(id, {
          ...data,
          billId: Number(data.billId),
          sum: Number(data.sum),
        }),
      editTitle: "Редактирование платежа",
      deleteAction: (id) => deletePayment(id),
    },
    promocodes: {
      data: promocodes,
      total: totalPromocodes,
      columns: columnsPromocodes,
      headText: headTextPromocodes,
      fields: fieldsPromocodes,
      action: getPromocodes,
      addAction: (data) =>
        createPromocode({
          ...data,
          statusId: Number(data.statusId),
          discount: Number(data.discount),
        }),
      addTitle: "Создание промокода",
      updateAction: (id, data) =>
        updatePromocode(id, {
          ...data,
          statusId: Number(data.statusId),
          discount: Number(data.discount),
        }),
      editTitle: "Редактирование промокода",
      deleteAction: (id) => deletePromocode(id),
    },
    trips: {
      data: trips,
      total: totalTrips,
      columns: columnsTrips,
      headText: headTextTrips,
      fields: fieldsTrips,
      action: getTrips,
      addAction: (data) =>
        createTrip({
          ...data,
          bookingId: Number(data.bookingId),
          statusId: Number(data.statusId),
          duration: data.duration ? Number(data.duration) : null,
          distance: data.distance ? Number(data.distance) : null,
          fuelUsed: data.fuelUsed ? Number(data.fuelUsed) : null,
          refueled: data.refueled ? Number(data.refueled) : 0,
          insuranceActive: data.insuranceActive === "true",
          startTime: new Date(data.startTime).toISOString(),
          endTime: data.endTime ? new Date(data.endTime).toISOString(): null
        }),
      addTitle: "Создание поездки (Full)",
      updateAction: (id, data) =>
        updateTrip(id, {
          ...data,
          bookingId: Number(data.bookingId),
          statusId: Number(data.statusId),
          duration: data.duration,
          distance: Number(data.distance),
          startTime: new Date(data.startTime).toISOString(),
          endTime: new Date(data.endTime).toISOString(),
        }),
      editTitle: "Редактирование поездки",
      deleteAction: (id) => deleteTrip(id),
      detailAction: (id) => getTripWithInfo(id),
    },
    users: {
      data: users,
      total: totalUsers,
      columns: columnsUsers,
      headText: headTextUsers,
      fields: fieldsUsers,
      action: getUsers,
      addAction: (data) => createUser({ ...data, roleId: Number(data.roleId) }),
      addTitle: "Создание пользователя",
      updateAction: (id, data) =>
        updateUser(id, { ...data, roleId: Number(data.roleId) }),
      editTitle: "Редактирование пользователя",
      deleteAction: (id) => deleteUser(id),
    },
    Dashboard: {
      isDashboard: true,
    },
  };

  return tableConfig[activeTab];
};

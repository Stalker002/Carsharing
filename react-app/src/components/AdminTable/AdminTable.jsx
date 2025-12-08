import { useEffect, useRef, useState } from "react";
import { useDispatch, useSelector } from "react-redux";

import { GenericTable } from "./GenericTable";

import {
  createUser,
  deleteUser,
  getUsers,
  updateUser,
} from "../../redux/actions/users";
import {
  createCar,
  deleteCar,
  getCars,
  updateCar,
} from "../../redux/actions/cars";

import UpdateModal from "../UpdateModal/UpdateModal";
import AddModel from "../AddModel/AddModel";

import "./AdminTable.css";
import {
  fieldsCars,
  columnsCars,
  headTextCars,
  fieldsUsers,
  columnsUsers,
  headTextUsers,
  columnsTrips,
  headTextTrips,
  columnsBills,
  headTextBills,
  fieldsBills,
  columnsBookings,
  headTextBookings,
  fieldsBookings,
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
  fieldsTrips,
} from "./configs";
import CategoryManager from "../CategoryManager/CategoryManager";
import {
  createPayment,
  deletePayment,
  getPayments,
  updatePayment,
} from "../../redux/actions/payments";
import {
  createBill,
  deleteBill,
  getBills,
  updateBill,
} from "../../redux/actions/bills";
import {
  createBooking,
  deleteBooking,
  getBookings,
  updateBooking,
} from "../../redux/actions/bookings";
import {
  createClient,
  deleteClient,
  getClients,
  updateClient,
} from "../../redux/actions/clients";
import {
  createFine,
  deleteFine,
  getFines,
  updateFine,
} from "../../redux/actions/fines";
import {
  createInsurance,
  deleteInsurance,
  getInsurances,
  updateInsurance,
} from "../../redux/actions/insurance";
import {
  createPromocode,
  deletePromocode,
  getPromocodes,
  updatePromocode,
} from "../../redux/actions/promocodes";
import {
  createTrip,
  deleteTrip,
  getTrips,
  updateTrip,
} from "../../redux/actions/trips";

function AdminTable({ activeTab }) {
  const dispatch = useDispatch();

  const [page, setPage] = useState(1);
  const [editingItem, setEditingItem] = useState(null);
  const [isAddOpen, setIsAddOpen] = useState(false);
  const [isCatOpen, setIsCatOpen] = useState(false);

  const isLoadingRef = useRef(false);
  const isSwitchingTable = useRef(false);

  const users = useSelector((state) => state.users?.users || []);
  const totalUsers = useSelector((state) => state.users?.usersTotal || 0);

  const cars = useSelector((state) => state.cars?.cars || []);
  const totalCars = useSelector((state) => state.cars?.totalCars || 0);

  const tableConfig = {
    bills: {
      data: useSelector((state) => state.bills?.bills || []), // Проверь свой редьюсер
      total: useSelector((state) => state.bills?.billsTotal || 0),
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
          startTime: new Date(data.startTime).toISOString(),
          endTime: new Date(data.endTime).toISOString(),
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
          startTime: new Date(data.startTime).toISOString(),
          endTime: new Date(data.endTime).toISOString(),
        }),
      editTitle: "Редактирование счета",

      deleteAction: (id) => deleteBill(id),
    },
    bookings: {
      data: useSelector((state) => state.bookings?.bookings || []), // Проверь редьюсер
      total: useSelector((state) => state.bookings?.bookingsTotal || 0),
      columns: columnsBookings,
      headText: headTextBookings,
      fields: fieldsBookings,

      action: getBookings,

      // POST
      addAction: (data) =>
        createBooking({
          ...data,
          statusId: Number(data.statusId),
          carId: Number(data.carId),
          clientId: Number(data.clientId),
          // startTime и endTime обычно отправляются как строки ISO,
          // datetime-local возвращает строку вида "YYYY-MM-DDTHH:mm", что подходит
        }),
      addTitle: "Создание бронирования",

      // PUT
      updateAction: (id, data) =>
        updateBooking(id, {
          ...data,
          statusId: Number(data.statusId),
          carId: Number(data.carId),
          clientId: Number(data.clientId),
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
          statusId: Number(data.statusId),
          tariffId: Number(data.tariffId),
          categoryId: Number(data.categoryId),
          specificationId: Number(data.specificationId),
          location: data.location,
          fuelLevel: Number(data.fuelLevel),
        }),
      editTitle: "Редактирование параметров",
      deleteAction: (id) => deleteCar(id),
    },
    clients: {
      data: useSelector((state) => state.clients?.clients || []),
      total: useSelector((state) => state.clients?.clientsTotal || 0),
      columns: columnsClients,
      headText: headTextClients,
      fields: fieldsClients,

      action: getClients,

      // POST: Создание Клиента + Юзера
      addAction: (data) =>
        createClient({
          login: data.login,
          password: data.password,
          name: data.name,
          surname: data.surname,
          phoneNumber: data.phoneNumber,
          email: data.email,
        }),
      addTitle: "Регистрация нового клиента",

      // PUT: Обновление данных клиента
      updateAction: (id, data) =>
        updateClient(id, {
          userId: Number(data.userId), // Контроллер требует userId в теле запроса
          name: data.name,
          surname: data.surname,
          phoneNumber: data.phoneNumber,
          email: data.email,
        }),
      editTitle: "Редактирование профиля клиента",

      deleteAction: (id) => deleteClient(id),
    },
    fines: {
      data: useSelector((state) => state.fines?.fines || []), // Проверь редьюсер
      total: useSelector((state) => state.fines?.finesTotal || 0),
      columns: columnsFines,
      headText: headTextFines,
      fields: fieldsFines,

      action: getFines,

      // POST
      addAction: (data) =>
        createFine({
          ...data,
          tripId: Number(data.tripId),
          statusId: Number(data.statusId),
          amount: Number(data.amount),
          // type и date остаются строками
        }),
      addTitle: "Регистрация штрафа",

      // PUT
      updateAction: (id, data) =>
        updateFine(id, {
          ...data,
          tripId: Number(data.tripId),
          statusId: Number(data.statusId),
          amount: Number(data.amount),
        }),
      editTitle: "Редактирование штрафа",

      deleteAction: (id) => deleteFine(id),
    },
    insurances: {
      data: useSelector((state) => state.insurances?.insurances || []),
      total: useSelector((state) => state.insurances?.insurancesTotal || 0), // Если есть пагинация на бэке, но в контроллере я вижу только GetInsurances (без страниц), значит total может быть просто length
      columns: columnsInsurances,
      headText: headTextInsurances,
      fields: fieldsInsurances,

      // Обрати внимание: в контроллере нет метода GetPagedInsurances,
      // поэтому action просто getInsurances() без параметров страницы.
      action: getInsurances,

      // POST
      addAction: (data) =>
        createInsurance({
          ...data,
          carId: Number(data.carId),
          statusId: Number(data.statusId),
          cost: Number(data.cost),
        }),
      addTitle: "Оформление страховки",

      // PUT
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
    payments: {
      data: useSelector((state) => state.payments?.payments || []), // Проверь редьюсер
      total: useSelector((state) => state.payments?.paymentsTotal || 0),
      columns: columnsPayments,
      headText: headTextPayments,
      fields: fieldsPayments,

      action: getPayments,

      // POST
      addAction: (data) =>
        createPayment({
          ...data,
          billId: Number(data.billId),
          sum: Number(data.sum),
          // method и date отправляем строками
        }),
      addTitle: "Регистрация платежа",

      // PUT
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
      data: useSelector((state) => state.promocodes?.promocodes || []),
      total: useSelector((state) => state.promocodes?.promocodesTotal || 0),
      columns: columnsPromocodes,
      headText: headTextPromocodes,
      fields: fieldsPromocodes,

      action: getPromocodes,

      // POST
      addAction: (data) =>
        createPromocode({
          ...data,
          statusId: Number(data.statusId),
          discount: Number(data.discount),
          // code, startDate, endDate отправляются как строки
        }),
      addTitle: "Создание промокода",

      // PUT
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
      data: useSelector((state) => state.trips?.trips || []),
      total: useSelector((state) => state.trips?.tripsTotal || 0),
      columns: columnsTrips,
      headText: headTextTrips,
      fields: fieldsTrips,

      action: getTrips,

      // POST: Отправляем и данные Trip, и данные Detail
      addAction: (data) =>
        createTrip({
          ...data,
          bookingId: Number(data.bookingId),
          statusId: Number(data.statusId),
          duration: Number(data.duration),
          distance: Number(data.distance),
          fuelUsed: Number(data.fuelUsed),
          // Преобразуем строковые "true"/"false" из select в boolean
          insuranceActive: data.insuranceActive === "true",
          refueled: data.refueled === "true",
        }),
      addTitle: "Создание поездки (Full)",

      // PUT: Обновляем только основные данные (как в контроллере)
      updateAction: (id, data) =>
        updateTrip(id, {
          ...data,
          bookingId: Number(data.bookingId),
          statusId: Number(data.statusId),
          duration: Number(data.duration),
          distance: Number(data.distance),
          // Поля Detail тут игнорируются, так как их нет в fields при редактировании
        }),
      editTitle: "Редактирование поездки",

      deleteAction: (id) => deleteTrip(id),
    },
    users: {
      data: useSelector((state) => state.users?.users || []),
      total: useSelector((state) => state.users?.usersTotal || 0),
      columns: columnsUsers,
      headText: headTextUsers,
      fields: fieldsUsers,
      action: getUsers,
      addAction: (data) =>
        createUser({
          ...data,
          roleId: Number(data.roleId),
        }),
      addTitle: "Создание пользователя",
      updateAction: (id, data) =>
        updateUser(id, {
          ...data,
          roleId: Number(data.roleId),
        }),
      editTitle: "Редактирование пользователя",
      deleteAction: (id) => deleteUser(id),
    },
  };

  useEffect(() => {
    isSwitchingTable.current = true;
    setPage(1);
    const container = document.getElementById("container");
    if (container) container.scrollTop = 0;
  }, [activeTab]);

  useEffect(() => {
    const cfg = tableConfig[activeTab];
    if (!cfg || !cfg.action || cfg.isDashboard) return;

    if (isSwitchingTable.current) {
      if (page === 1) isSwitchingTable.current = false;
      else return;
    }

    if (isLoadingRef.current) return;

    isLoadingRef.current = true;
    dispatch(cfg.action(page)).finally(() => {
      isLoadingRef.current = false;
    });
  }, [page, activeTab, dispatch]);

  const nextHandler = () => {
    if (isLoadingRef.current) return;
    const cfg = tableConfig[activeTab];
    if (!cfg) return;

    if ((cfg.data?.length || 0) >= (cfg.total || 0)) return;

    setPage((prev) => prev + 1);
  };

  const handleEditClick = (item) => {
    setEditingItem(item);
  };

  const refreshTable = () => {
    const container = document.getElementById("container");
    if (container) container.scrollTop = 0;

    const cfg = tableConfig[activeTab];

    if (page === 1) {
      if (cfg?.action) {
        isLoadingRef.current = true;
        dispatch(cfg.action(1)).finally(() => {
          isLoadingRef.current = false;
        });
      }
    } else {
      setPage(1);
    }
  };

  const handleSaveEdit = async (e) => {
    e.preventDefault();
    const cfg = tableConfig[activeTab];

    if (!cfg?.updateAction) return alert("Не настроено");

    const formData = new FormData(e.target);
    const rawData = Object.fromEntries(formData.entries());
    const finalData = { ...editingItem, ...rawData };

    const result = await dispatch(cfg.updateAction(finalData.id, finalData));
    if (!result.success) {
      alert(result.message);
      return;
    }

    setEditingItem(null);
    refreshTable();
  };

  const handleAdd = async (data) => {
    const cfg = tableConfig[activeTab];
    if (cfg?.addAction) {
      const result = await dispatch(cfg.addAction(data));
      if (!result.success) {
        alert(result.message);
        return false;
      }
      refreshTable();
      return true;
    } else {
      console.warn("Action добавления не настроен для", activeTab);
      return false;
    }
  };

  if (activeTab === "Dashboard") {
    return <div>Здесь будет компонент Dashboard</div>;
  }

  const cfg = tableConfig[activeTab];
  if (!cfg) return <div>Ошибка конфигурации для таблицы: {activeTab}</div>;

  const addFields = cfg.fields ? cfg.fields.filter((f) => !f.hideOnAdd) : [];
  const editFields = cfg.fields ? cfg.fields.filter((f) => !f.hideOnEdit) : [];

  return (
    <>
      <div className="admin-body">
        <div className="table-top">
          <input className="search" placeholder="Поиск по таблице" />
          <div>
            {activeTab === "cars" && (
              <button
                className="category-button"
                onClick={() => setIsCatOpen(true)}
              >
                Категории
              </button>
            )}
            <button className="add-button" onClick={() => setIsAddOpen(true)}>
              + Добавить запись
            </button>
          </div>
        </div>

        <GenericTable
          headText={cfg.headText}
          bodyText={cfg.data || []}
          columns={cfg.columns}
          onEditClick={handleEditClick}
          nextHandler={nextHandler}
          hasMore={(cfg.data?.length || 0) < (cfg.total || 0)}
        />
      </div>

      {editingItem && (
        <UpdateModal
          title={`${cfg.editTitle || "Редактирование"} #${editingItem.id}`}
          onClose={() => setEditingItem(null)}
          formId="edit-form"
          onDelete={
            cfg.deleteAction
              ? () => {
                  if (window.confirm("Удалить?")) {
                    dispatch(cfg.deleteAction(editingItem.id));
                    setEditingItem(null);
                    refreshTable();
                  }
                }
              : null
          }
        >
          <form id="edit-form" onSubmit={handleSaveEdit}>
            {editFields.map((field) => (
              <div className="update-group" key={field.name}>
                <label>{field.label}</label>

                {field.type === "select" ? (
                  <select
                    name={field.name}
                    className="modal-input"
                    defaultValue={editingItem[field.name]}
                    disabled={field.readOnly}
                  >
                    <option value="" disabled>
                      Выберите...
                    </option>
                    {field.options.map((opt) => (
                      <option key={opt.value} value={opt.value}>
                        {opt.label}
                      </option>
                    ))}
                  </select>
                ) : (
                  <input
                    type={field.type}
                    name={field.name}
                    defaultValue={editingItem[field.name]}
                    className="modal-input"
                    readOnly={field.readOnly}
                    disabled={field.readOnly}
                  />
                )}
              </div>
            ))}
          </form>
        </UpdateModal>
      )}

      <AddModel
        isOpen={isAddOpen}
        onClose={() => setIsAddOpen(false)}
        activeTable={activeTab}
        title={cfg.addTitle}
        fields={addFields}
        onAdd={handleAdd}
      />

      <CategoryManager isOpen={isCatOpen} onClose={() => setIsCatOpen(false)} />
    </>
  );
}

export default AdminTable;

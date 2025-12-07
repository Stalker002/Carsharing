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
import { getTrips, updateTrip } from "../../redux/actions/trips";

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
} from "./configs";
import CategoryManager from "../CategoryManager/CategoryManager";
import { getPayments } from "../../redux/actions/payments";

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

  const trips = [];
  const totalTrips = 0;
  const payments = [];
  const totalPayments = 0;

  const tableConfig = {
    users: {
      data: users,
      total: totalUsers,
      columns: columnsUsers,
      headText: headTextUsers,
      fields: fieldsUsers,
      action: getUsers,
      updateAction: (id, data) =>
        updateUser(id, { ...data, roleId: Number(data.roleId) }),
      addAction: (data) => createUser({ ...data, roleId: Number(data.roleId) }),
      deleteAction: (id) => deleteUser(id),
      addTitle: "Новый пользователь",
      editTitle: "Редактирование пользователя",
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
    trips: {
      data: trips,
      total: totalTrips,
      action: getTrips,
      updateAction: updateTrip,
      columns: columnsTrips,
      headText: headTextTrips,
      fields: [],
    },
    payments: {
      data: payments,
      total: totalPayments,
      action: getPayments,
      columns: null,
      headText: null,
    },
    Dashboard: {
      isDashboard: true,
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

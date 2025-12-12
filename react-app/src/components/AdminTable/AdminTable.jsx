import { useEffect, useRef, useState } from "react";
import { useDispatch } from "react-redux";

import { GenericTable } from "./GenericTable";
import UpdateModal from "../UpdateModal/UpdateModal";
import AddModel from "../AddModel/AddModel";
import DetailingModal from "../DetailModal/DetailModal";
import CategoryManager from "../CategoryManager/CategoryManager";
import SimpleTable from "../SimpleTable/SimpleTable";
import { useAdminTableConfig } from "./useAdminTableConfig";
import "./AdminTable.css";

import { fieldsInsurances, fieldsMaintenances } from "./configs";
import {
  createInsurance,
  deleteInsurance,
  getInsuranceByCars,
  updateInsurance,
} from "../../redux/actions/insurance";
import {
  createMaintenance,
  getMaintenanceByCars,
} from "../../redux/actions/maintenance";
import SubTable from "../SubTable/SubTable";

function AdminTable({ activeTab }) {
  const dispatch = useDispatch();

  const [page, setPage] = useState(1);

  const [editingItem, setEditingItem] = useState(null);
  const [detailingItem, setDetailingItem] = useState(null);
  const [isAddOpen, setIsAddOpen] = useState(false);
  const [isCatOpen, setIsCatOpen] = useState(false);

  const [subEditingItem, setSubEditingItem] = useState(null);
  const [isSubAddOpen, setIsSubAddOpen] = useState(false);
  const [subType, setSubType] = useState(null);

  const [carInsurances, setCarInsurances] = useState([]);
  const [carMaintenances, setCarMaintenances] = useState([]);

  const [isDetailLoading, setIsDetailLoading] = useState(false);
  const isLoadingRef = useRef(false);
  const isSwitchingTable = useRef(false);

  const cfg = useAdminTableConfig(activeTab);

  useEffect(() => {
    isSwitchingTable.current = true;
    setPage(1);
    document.getElementById("container")?.scrollTo(0, 0);
  }, [activeTab]);

  useEffect(() => {
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
  }, [page, activeTab, dispatch, cfg]);

  const refreshTable = () => {
    document.getElementById("container")?.scrollTo(0, 0);
    if (page === 1 && cfg?.action) {
      isLoadingRef.current = true;
      dispatch(cfg.action(1)).finally(() => (isLoadingRef.current = false));
    } else {
      setPage(1);
    }
  };

  const nextHandler = () => {
    if (isLoadingRef.current || !cfg) return;
    if ((cfg.data?.length || 0) >= (cfg.total || 0)) return;
    setPage((prev) => prev + 1);
  };

  const fetchSubData = async (itemId) => {
    if (activeTab === "cars") {
      const ins = await dispatch(getInsuranceByCars(itemId));
      setCarInsurances(ins.data);

      const maintenance = await dispatch(getMaintenanceByCars(itemId));
      setCarMaintenances(maintenance.data);
    }
  };

  const handleRowClick = async (row) => {
    if (cfg.detailAction) {
      setIsDetailLoading(true);
      const result = await dispatch(cfg.detailAction(row.id));
      setIsDetailLoading(false);

      if (result.success && result.data) {
        setEditingItem(null);
        setDetailingItem(result.data);
        fetchSubData(result.data.id);
      } else {
        alert("Не удалось загрузить детали");
      }
    } else {
      setDetailingItem(row);
    }
  };

  const handleEditClick = (item) => {
    setDetailingItem(null);
    setEditingItem(item);
    fetchSubData(item.id);
  };

  const handleAdd = async (data) => {
    if (cfg?.addAction) {
      const result = await dispatch(cfg.addAction(data));
      if (!result.success) {
        alert(result.message);
        return false;
      }
      refreshTable();
      return true;
    }
    return false;
  };

  const handleSaveEdit = async (e) => {
    e.preventDefault();
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

  const openSubAdd = (type) => {
    setSubType(type);
    setIsSubAddOpen(true);
  };

  const openSubEdit = (item, type) => {
    setSubType(type);
    setSubEditingItem(item);
  };

  const handleSubAddSave = async (data) => {
    const payload = { ...data, carId: Number(editingItem.id) };
    let result = { success: false };

    if (subType === "insurance") {
      result = await dispatch(
        createInsurance({
          ...payload,
          statusId: Number(payload.statusId),
          cost: Number(payload.cost),
        })
      );
    }

    if (subType === "maintenance") {
      result = await dispatch(
        createMaintenance({
          ...payload,
          statusId: Number(payload.statusId),
          cost: Number(payload.cost),
        })
      );
    }

    if (result.success) {
      fetchSubData(editingItem.id);
      return true;
    } else {
      alert(result.message);
      return false;
    }
  };

  const handleSubUpdateSave = async (e) => {
    e.preventDefault();
    const formData = new FormData(e.target);
    const rawData = Object.fromEntries(formData.entries());
    const finalData = { ...subEditingItem, ...rawData };

    let result = { success: false };

    if (subType === "insurance") {
      result = await dispatch(
        updateInsurance(finalData.id, {
          ...finalData,
          carId: Number(editingItem.id),
          statusId: Number(finalData.statusId),
          cost: Number(finalData.cost),
        })
      );
    }

    if (result.success) {
      setSubEditingItem(null);
      fetchSubData(editingItem.id);
    } else {
      alert(result.message);
    }
  };

  const handleSubDelete = async (id) => {
    if (!window.confirm("Удалить запись?")) return;
    let result = { success: false };

    if (subType === "insurance") result = await dispatch(deleteInsurance(id));

    if (result.success) {
      setSubEditingItem(null);
      fetchSubData(editingItem.id);
    }
  };

  const getTabs = (isEditMode) => {
    if (activeTab === "cars") {
      const TableComponent = isEditMode ? SubTable : SimpleTable;

      const editProps = (type) =>
        isEditMode
          ? {
              onAdd: () => openSubAdd(type),
              onEdit: (item) => openSubEdit(item, type),
              onDelete: (id) => {
                setSubType(type);
                handleSubDelete(id);
              },
              addButtonText: type === "insurance" ? "Страховку" : "ТО",
            }
          : {};

      return [
        {
          title: "Страховки",
          content: (
            <TableComponent
              data={carInsurances}
              columns={["id", "type", "company", "endDate"]}
              headers={["ID", "Тип", "Компания", "Окончание"]}
              {...editProps("insurance")}
            />
          ),
        },
        {
          title: "Обслуживание",
          content: (
            <TableComponent
              data={carMaintenances}
              columns={["id", "date", "workType", "cost"]}
              headers={["ID", "Дата", "Тип работ", "Стоимость"]}
              {...editProps("maintenance")}
            />
          ),
        },
      ];
    }
    return [];
  };

  const getSubFields = () => {
    if (subType === "maintenance")
      return fieldsMaintenances.filter((f) => f.name !== "carId");
    if (subType === "insurance")
      return fieldsInsurances.filter((f) => f.name !== "carId");
    return [];
  };

  if (activeTab === "Dashboard")
    return <div>Здесь будет компонент Dashboard</div>;
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
          onRowClick={handleRowClick}
          nextHandler={nextHandler}
          hasMore={(cfg.data?.length || 0) < (cfg.total || 0)}
          style={{ cursor: isDetailLoading ? "wait" : "default" }}
        />
      </div>

      {editingItem && (
        <UpdateModal
          title={`${cfg.editTitle || "Редактирование"} #${editingItem.id}`}
          onClose={() => setEditingItem(null)}
          formId="edit-form"
          additionalTabs={getTabs(true)}
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
          <form
            id="edit-form"
            onSubmit={handleSaveEdit}
            className="update-modal__form-grid"
          >
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
                    step={field.step}
                  />
                )}
              </div>
            ))}
          </form>
        </UpdateModal>
      )}

      <DetailingModal
        title={`Детали #${detailingItem?.id}`}
        data={detailingItem}
        fields={cfg.fields}
        onClose={() => setDetailingItem(null)}
        additionalTabs={getTabs(false)}
      />

      <AddModel
        isOpen={isAddOpen}
        onClose={() => setIsAddOpen(false)}
        title={cfg.addTitle}
        fields={addFields}
        onAdd={handleAdd}
      />

      <AddModel
        isOpen={isSubAddOpen}
        onClose={() => setIsSubAddOpen(false)}
        title={subType === "insurance" ? "Добавить страховку" : "Добавить ТО"}
        fields={getSubFields().filter((f) => !f.hideOnAdd)}
        onAdd={handleSubAddSave}
      />

      {subEditingItem && (
        <UpdateModal
          title={`Редактирование #${subEditingItem.id}`}
          onClose={() => setSubEditingItem(null)}
          formId="sub-edit-form"
          onDelete={() => handleSubDelete(subEditingItem.id)}
        >
          <form
            id="sub-edit-form"
            onSubmit={handleSubUpdateSave}
            className="update-modal__form-grid"
          >
            {getSubFields()
              .filter((f) => !f.hideOnEdit)
              .map((field) => (
                <div className="update-group" key={field.name}>
                  <label>{field.label}</label>
                  <input
                    name={field.name}
                    defaultValue={subEditingItem[field.name]}
                    className="modal-input"
                    type={field.type}
                    readOnly={field.readOnly}
                  />
                </div>
              ))}
          </form>
        </UpdateModal>
      )}

      <CategoryManager isOpen={isCatOpen} onClose={() => setIsCatOpen(false)} />
    </>
  );
}

export default AdminTable;

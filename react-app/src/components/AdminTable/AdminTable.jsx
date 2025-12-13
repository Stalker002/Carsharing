import { useEffect, useRef, useState, useMemo, useCallback } from "react";
import { useDispatch, useSelector } from "react-redux";

import { GenericTable } from "./GenericTable";
import UpdateModal from "../UpdateModal/UpdateModal";
import AddModel from "../AddModel/AddModel";
import DetailingModal from "../DetailModal/DetailModal";
import CategoryManager from "../CategoryManager/CategoryManager";
import SubTable from "../SubTable/SubTable";
import SimpleTable from "../SimpleTable/SimpleTable";

import { useAdminTableConfig, STATUS_FILTERS } from "./useAdminTableConfig";
import { useSubData } from "./useSubData";
import { fieldsClients, fieldsInsurances, fieldsMaintenances } from "./configs";
import "./AdminTable.css";

import {
  createInsurance,
  deleteInsurance,
  updateInsurance,
} from "../../redux/actions/insurance";
import {
  createMaintenance,
  updateMaintenance,
} from "../../redux/actions/maintenance";
import { getStatuses } from "../../redux/actions/statuses";
import { getCategories } from "../../redux/actions/category";
import { updateClient } from "../../redux/actions/clients";
import { TabForm } from "./TabForm";

function AdminTable({ activeTab }) {
  const dispatch = useDispatch();
  const cfg = useAdminTableConfig(activeTab);

  const [page, setPage] = useState(1);
  const isLoadingRef = useRef(false);
  const isSwitchingTable = useRef(false);

  const [editingItem, setEditingItem] = useState(null);
  const [detailingItem, setDetailingItem] = useState(null);
  const [isAddOpen, setIsAddOpen] = useState(false);
  const [isCatOpen, setIsCatOpen] = useState(false);
  const [isDetailLoading, setIsDetailLoading] = useState(false);

  const [subEditingItem, setSubEditingItem] = useState(null);
  const [isSubAddOpen, setIsSubAddOpen] = useState(false);
  const [subType, setSubType] = useState(null);

  const categoriesList = useSelector(
    (state) => state.categories?.categories || []
  );
  const statusesList = useSelector((state) => state.statuses?.statuses || []);

  const {
    insurances,
    maintenances,
    clientProfile,
    isClientLoading,
    fetchSubData,
    fetchClientProfile,
    clearSubData,
  } = useSubData(activeTab);

  useEffect(() => {
    isSwitchingTable.current = true;
    setPage(1);
    document.getElementById("container")?.scrollTo(0, 0);
  }, [activeTab]);

  useEffect(() => {
    const tabsWithStatuses = ["cars", "bookings", "bills", "promocodes", "trips", "insurances"];
    const tabsWithCategories = ["cars"];

    if (tabsWithStatuses.includes(activeTab)) {
      if (statusesList.length === 0) dispatch(getStatuses());
    }
    if (tabsWithCategories.includes(activeTab)) {
      if (categoriesList.length === 0) dispatch(getCategories());
    }
  }, [activeTab, dispatch, categoriesList.length, statusesList.length]);

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
  }, [page, activeTab, dispatch]);

  const dynamicFields = useMemo(() => {
    if (!cfg?.fields) return [];
    return cfg.fields.map((field) => {
      if (field.name === "categoryId" && activeTab === "cars") {
        return {
          ...field,
          options: categoriesList
            .filter((c) => c && c.id)
            .map((c) => ({ value: c.id, label: c.name })),
        };
      }
      if (field.name === "statusId") {
        const allowed = STATUS_FILTERS[activeTab] || [];
        return {
          ...field,
          options: statusesList
            .filter((s) => s && s.id && allowed.includes(s.name))
            .map((s) => ({ value: s.id, label: s.name })),
        };
      }
      return field;
    });
  }, [cfg, categoriesList, statusesList, activeTab]);

  const addFields = useMemo(
    () => dynamicFields.filter((f) => !f.hideOnAdd),
    [dynamicFields]
  );
  const editFields = useMemo(
    () => dynamicFields.filter((f) => !f.hideOnEdit),
    [dynamicFields]
  );

  const subFields = useMemo(() => {
    if (subType === "maintenance")
      return fieldsMaintenances.filter((f) => f.name !== "carId");
    if (subType === "insurance")
      return fieldsInsurances.filter((f) => f.name !== "carId");
    return [];
  }, [subType]);

  const refreshTable = useCallback(() => {
    document.getElementById("container")?.scrollTo(0, 0);
    if (page === 1 && cfg?.action) {
      isLoadingRef.current = true;
      dispatch(cfg.action(1)).finally(() => (isLoadingRef.current = false));
    } else {
      setPage(1);
    }
  }, [page, cfg, dispatch]);

  const nextHandler = () => {
    if (isLoadingRef.current || !cfg) return;
    if ((cfg.data?.length || 0) >= (cfg.total || 0)) return;
    setPage((p) => p + 1);
  };

  const handleRowClick = async (row) => {
    setDetailingItem(row);

    if (cfg.detailAction) {
      setIsDetailLoading(true);
      const result = await dispatch(cfg.detailAction(row.id));
      setIsDetailLoading(false);
      if (result.success && result.data) {
        setEditingItem(null);
        setDetailingItem(result.data);
        clearSubData();
      } else {
        alert("Не удалось загрузить детали");
      }
    } else {
      setDetailingItem(row);
      if (activeTab === "users") {
        clearSubData();
        if (row && row.id) {          
          fetchClientProfile(row.id);
        }
      }
    }
  };

  const handleEditClick = async (item) => {
    setDetailingItem(null);
    if (cfg.detailAction) {
      const result = await dispatch(cfg.detailAction(item.id));
      if (result.success && result.data) {
        setEditingItem(result.data);
        fetchSubData(result.data.id);
      } else {
        alert("Ошибка загрузки");
      }
    } else {
      setEditingItem(item);
      if (activeTab === 'cars') {
         fetchSubData(item.id);
      }
    }
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
    } else if (subType === "maintenance") {
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

    if (subType === "maintenance") {
      result = await dispatch(
        updateMaintenance(finalData.id, {
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
    if (!window.confirm("Удалить?")) return;
    let result = { success: false };
    if (subType === "insurance") result = await dispatch(deleteInsurance(id));

    if (result.success) {
      setSubEditingItem(null);
      fetchSubData(editingItem.id);
    }
  };

  const handleClientProfileSave = async (formData) => {
    if (!clientProfile?.id) return;

    const fullData = { ...clientProfile, ...formData };

    const result = await dispatch(
      updateClient(clientProfile.id, {
        userId: Number(editingItem.id),
        ...fullData,
      })
    );

    if (result.success) {
      alert("Данные клиента обновлены");
      fetchClientProfile(editingItem.id);
    } else {
      alert(result.message);
    }
  };

  const handleTabChange = (tabIndex) => {
    const currentItem = editingItem || detailingItem;
    if (!currentItem) return;

    if (activeTab === "cars") {
      if (tabIndex === 0) fetchSubData(currentItem.id, "insurance");
      if (tabIndex === 1) fetchSubData(currentItem.id, "maintenance");
    }

    if (activeTab === "users") {
      if (tabIndex === 0) fetchClientProfile(currentItem.id);
    }
  };

  const additionalTabs = useMemo(() => {
    const currentItem = editingItem || detailingItem;
    const isEditMode = !!editingItem;

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
              addButtonText: type === "insurance" ? "Страховку" : "Обслуживание",
            }
          : {};

      return [
        {
          title: "Страховки",
          content: (
            <TableComponent
              data={insurances}
              columns={["id", "type", "company", "policyNumber", "endDate"]}
              headers={["ID", "Тип", "Компания", "Номер полиса", "Окончание"]}
              {...editProps("insurance")}
            />
          ),
        },
        {
          title: "Обслуживание",
          content: (
            <TableComponent
              data={maintenances}
              columns={["id", "workType", "description", "cost", "date"]}
              headers={["ID", "Тип", "Описание", "Стоимость", "Дата"]}
              {...editProps("maintenance")}
            />
          ),
        },
      ];
    }

     if (activeTab === "users") {
      if (isClientLoading) {
          return [{ title: "Клиент", content: <div style={{padding:20}}>Загрузка...</div> }];
      }
      
      if (!clientProfile) return [];

      const content = isEditMode ? (
        <TabForm 
            initialData={clientProfile} 
            fields={fieldsClients} 
            onSave={handleClientProfileSave} 
        />
      ) : (
        <div className="detail-modal-grid">
          {fieldsClients.map((f) => (
            <div key={f.name} className="detail-modal-row">
              <span className="detail-modal-label">{f.label}</span>
              <span className="detail-modal-value">{clientProfile?.[f.name] || "—"}</span>
            </div>
          ))}
        </div>
      );
      return [
        {
          title: "Клиент",
          content: content,
        },
      ];
    }
    return [];
  }, [
    activeTab,
    editingItem,
    detailingItem,
    insurances,
    maintenances,
    clientProfile,
    isClientLoading,
  ]);

  if (activeTab === "Dashboard") return <div>Dashboard</div>;
  if (!cfg) return <div>Ошибка конфигурации</div>;

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
          additionalTabs={additionalTabs}
          onDelete={
            cfg.deleteAction
              ? () => {
                  /* ... */
                }
              : null
          }
          onTabChange={handleTabChange}
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
                    {field.options?.map((opt) => (
                      <option key={opt.value} value={opt.value}>
                        {opt.label}
                      </option>
                    ))}
                  </select>
                ) : field.type === "file" ? (
                  <div className="file-input-wrapper">
                    <input type="file" className="modal-input" />
                  </div>
                ) : (
                  <input
                    type={field.type}
                    name={field.name}
                    defaultValue={editingItem[field.name]}
                    className="modal-input"
                    readOnly={field.readOnly}
                    step={field.step}
                  />
                )}
              </div>
            ))}
          </form>
        </UpdateModal>
      )}

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
            {subFields
              .filter((f) => !f.hideOnEdit)
              .map((field) => (
                <div className="update-group" key={field.name}>
                  <label>{field.label}</label>
                  <input
                    name={field.name}
                    defaultValue={subEditingItem[field.name]}
                    className="modal-input"
                    type={field.type}
                  />
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
        additionalTabs={additionalTabs}
        onTabChange={handleTabChange}
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
        fields={subFields.filter((f) => !f.hideOnAdd)}
        onAdd={handleSubAddSave}
      />

      <CategoryManager isOpen={isCatOpen} onClose={() => setIsCatOpen(false)} />
    </>
  );
}

export default AdminTable;

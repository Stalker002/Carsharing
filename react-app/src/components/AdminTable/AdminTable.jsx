import { useState, useMemo, useCallback } from "react";
import { GenericTable } from "./GenericTable";
import UpdateModal from "../UpdateModal/UpdateModal";
import AddModel from "../AddModel/AddModel";
import DetailingModal from "../DetailModal/DetailModal";
import CategoryManager from "../CategoryManager/CategoryManager";
import {
  useAdminTableConfig,
  STATUS_FILTERS,
} from "./hooks/useAdminTableConfig";
import { useSubData } from "./hooks/useSubData";
import { useMainTableOperations } from "./hooks/useMainTableOperations";
import { useSubItemOperations } from "./hooks/useSubItemOperations";
import { useTabContent } from "./hooks/useTabContent";
import { toLocalISOString } from "./utils";
import "./AdminTable.css";

function AdminTable({ activeTab }) {
  const cfg = useAdminTableConfig(activeTab);

  const subData = useSubData(activeTab);

  const mainOps = useMainTableOperations(activeTab, cfg, subData);

  const subOps = useSubItemOperations(activeTab, mainOps.editingItem, subData);

  const [isAddOpen, setIsAddOpen] = useState(false);
  const [isCatOpen, setIsCatOpen] = useState(false);

  const dynamicFields = useMemo(() => {
    if (!cfg?.fields) return [];
    return cfg.fields.map((field) => {
      if (field.name === "categoryId" && activeTab === "cars") {
        return {
          ...field,
          options: mainOps.categoriesList.map((c) => ({
            value: c.id,
            label: c.name,
          })),
        };
      }
      if (field.name === "statusId") {
        const allowed = STATUS_FILTERS[activeTab] || [];
        return {
          ...field,
          options: mainOps.statusesList
            .filter((s) => allowed.includes(s.name))
            .map((s) => ({ value: s.id, label: s.name })),
        };
      }
      return field;
    });
  }, [cfg, mainOps.categoriesList, mainOps.statusesList, activeTab]);

  const addFields = useMemo(
    () => dynamicFields.filter((f) => !f.hideOnAdd),
    [dynamicFields]
  );
  const editFields = useMemo(
    () => dynamicFields.filter((f) => !f.hideOnEdit),
    [dynamicFields]
  );

  const additionalTabs = useTabContent({
    activeTab,
    editingItem: mainOps.editingItem,
    detailingItem: mainOps.detailingItem,
    subData,
    subOps,
  });

  const handleTabChange = useCallback(
    (tabIndex) => {
      const item = mainOps.editingItem || mainOps.detailingItem;
      if (!item) return;

      if (activeTab === "cars") {
        if (tabIndex === 0) subData.fetchSubData(item.id, "insurance");
        if (tabIndex === 1) subData.fetchSubData(item.id, "maintenance");
      }
      if (activeTab === "users") {
        if (tabIndex === 0) {
          if (!subData.clientProfile) subData.fetchClientProfile(item.id);
        }
        if (tabIndex === 1) {
          if (subData.clientProfile?.id) {
            subData.fetchClientDocuments(subData.clientProfile.id);
          } else {
            subData.fetchClientProfile(item.id);
          }
        }
      }
      if (activeTab === "trips" && tabIndex === 0)
        subData.fetchSubData(item.id, "fine");
      if (activeTab === "bills" && tabIndex === 0)
        subData.fetchSubData(item.id, "payment");
    },
    [activeTab, mainOps.editingItem, mainOps.detailingItem, subData]
  );

  if (activeTab === "dashboard") return <div>Dashboard</div>;
  if (!cfg) return <div>Ошибка конфигурации</div>;

  return (
    <>
      <div className="admin-body">
        <div className="table-top">
          {/*<input className="search" placeholder="Поиск по таблице" /> */}
          <div></div>
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
          onEditClick={mainOps.handleEditClick}
          onRowClick={mainOps.handleRowClick}
          nextHandler={mainOps.nextHandler}
          hasMore={(cfg.data?.length || 0) < (cfg.total || 0)}
          style={{ cursor: mainOps.isDetailLoading ? "wait" : "default" }}
          statuses={mainOps.statusesList}
        />
      </div>

      {mainOps.editingItem && (
        <UpdateModal
          title={`${cfg.editTitle || "Редактирование"} #${
            mainOps.editingItem.id
          }`}
          onClose={() => mainOps.setEditingItem(null)}
          formId="edit-form"
          additionalTabs={additionalTabs}
          onDelete={cfg.deleteAction ? mainOps.handleDelete : null}
          onTabChange={handleTabChange}
        >
          <form
            id="edit-form"
            onSubmit={mainOps.handleSaveEdit}
            className="update-modal-form-grid"
          >
            {editFields.map((field) => {
              let val = mainOps.editingItem[field.name];
              if (field.type === "datetime-local") val = toLocalISOString(val);
              return (
                <div className="update-group" key={field.name}>
                  <label>{field.label}</label>
                  {field.type === "select" ? (
                    <select
                      name={field.name}
                      className="modal-input"
                      defaultValue={val}
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
                      <img
                        src={`http://localhost:5078${val}`}
                        alt={field.label}
                        className="file-input-wrapper-img"
                      />
                      <input type="file" className="modal-input" />
                    </div>
                  ) : (
                    <input
                      type={field.type}
                      name={field.name}
                      defaultValue={val}
                      className="modal-input"
                      readOnly={field.readOnly}
                      step={field.step}
                    />
                  )}
                </div>
              );
            })}
          </form>
        </UpdateModal>
      )}

      {subOps.subEditingItem && (
        <UpdateModal
          title={`Редактирование #${subOps.subEditingItem.id}`}
          onClose={() => subOps.setSubEditingItem(null)}
          formId="sub-edit-form"
          onDelete={() => subOps.handleSubDelete(subOps.subEditingItem.id)}
        >
          <form
            id="sub-edit-form"
            onSubmit={subOps.handleSubUpdateSave}
            className="update-modal-form-grid"
          >
            {subOps.subFields
              .filter((f) => !f.hideOnEdit)
              .map((field) => {
                let val = subOps.subEditingItem[field.name];
                if (field.type === "datetime-local") {
                  val = toLocalISOString(val);
                }
                return (
                  <div className="update-group" key={field.name}>
                    <label>{field.label}</label>
                    {field.type === "select" ? (
                      <select
                        name={field.name}
                        className="modal-input"
                        defaultValue={val}
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
                        {(() => {
                          const filePath =
                            subOps.subEditingItem.filePath || val;

                          if (filePath && typeof filePath === "string") {
                            return (
                              <img
                                src={`http://localhost:5078${filePath}`}
                                alt={field.label}
                                className="file-input-wrapper-img"
                                onError={(e) =>
                                  (e.target.style.display = "none")
                                }
                              />
                            );
                          }
                          return null;
                        })()}

                        <input
                          type="file"
                          name={field.name}
                          className="modal-input"
                          accept="image/*"
                        />
                      </div>
                    ) : (
                      <input
                        type={field.type}
                        name={field.name}
                        defaultValue={val}
                        className="modal-input"
                        readOnly={field.readOnly}
                        step={field.step}
                      />
                    )}
                  </div>
                );
              })}
          </form>
        </UpdateModal>
      )}

      <DetailingModal
        title={`Детали #${mainOps.detailingItem?.id}`}
        data={mainOps.detailingItem}
        fields={cfg.fields}
        onClose={() => mainOps.setDetailingItem(null)}
        additionalTabs={additionalTabs}
        onTabChange={handleTabChange}
      />

      <AddModel
        isOpen={isAddOpen}
        onClose={() => setIsAddOpen(false)}
        title={cfg.addTitle}
        fields={addFields}
        onAdd={mainOps.handleAdd}
      />

      <AddModel
        isOpen={subOps.isSubAddOpen}
        onClose={() => subOps.setIsSubAddOpen(false)}
        title="Добавить запись"
        fields={subOps.subFields.filter((f) => !f.hideOnAdd)}
        onAdd={subOps.handleSubAddSave}
      />

      <CategoryManager isOpen={isCatOpen} onClose={() => setIsCatOpen(false)} />
    </>
  );
}

export default AdminTable;

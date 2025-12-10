import { useEffect, useRef, useState } from "react";
import { useDispatch } from "react-redux";

import { GenericTable } from "./GenericTable";

import UpdateModal from "../UpdateModal/UpdateModal";
import AddModel from "../AddModel/AddModel";

import "./AdminTable.css";
import CategoryManager from "../CategoryManager/CategoryManager";
import DetailingModal from "../DetailModal/DetailModal";
import { useAdminTableConfig } from "./useAdminTableConfig";
import SimpleTable from "./SimpleTable";

function AdminTable({ activeTab }) {
  const dispatch = useDispatch();

  const [page, setPage] = useState(1);

  const [editingItem, setEditingItem] = useState(null);
  const [detailingItem, setDetailingItem] = useState(null);
  const [isAddOpen, setIsAddOpen] = useState(false);
  const [isCatOpen, setIsCatOpen] = useState(false);

  const [isDetailLoading, setIsDetailLoading] = useState(false);
  const [carInsurances, setCarInsurances] = useState([]);
  const [carMaintenances, setCarMaintenances] = useState([]);

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
  }, [page, activeTab, dispatch]);

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

  const handleEditClick = (item) => {
    setEditingItem(item);
  };
// const insRes = await dispatch(getInsurancesByCar(row.id));
        // setCarInsurances(insRes.data || []);
  // === ОБНОВЛЕННЫЙ ХЕНДЛЕР КЛИКА ===
  const handleRowClick = async (row) => {
    if (cfg.detailAction) {
      setIsDetailLoading(true);
      const result = await dispatch(cfg.detailAction(row.id));
      
      // Логика загрузки доп. таблиц (можно тоже вынести в хук, но пока оставим)
      if (activeTab === 'cars') {
          setCarInsurances([/*...заглушка...*/]); 
          setCarMaintenances([]); 
      }

      setIsDetailLoading(false);

      if (result.success && result.data) {
        setEditingItem(null);
        setDetailingItem(result.data);
      } else {
        alert("Не удалось загрузить детали");
      }
    } else {
      setDetailingItem(row);
    }
  };

  // === ПОДГОТОВКА ТАБОВ ===
  // Мы создаем массив табов динамически
  const getAdditionalTabs = () => {
    if (activeTab === "cars") {
      return [
        {
          title: "Страховки",
          content: (
            <SimpleTable
              data={carInsurances}
              columns={["id", "type", "company", "endDate"]}
              headers={["ID", "Тип", "Компания", "Окончание"]}
            />
          ),
        },
        {
          title: "Обслуживание",
          content: (
            <SimpleTable
              data={carMaintenances}
              columns={["id", "date", "description", "cost"]}
              headers={["ID", "Дата", "Описание", "Стоимость"]}
            />
          ),
        },
      ];
    }
    // Для других таблиц (например, Клиенты -> История поездок) можно добавить свои условия
    return [];
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

  if (activeTab === "Dashboard") {
    return <div>Здесь будет компонент Dashboard</div>;
  }

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
          onEditClick={setEditingItem}
          onRowClick={handleRowClick}
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
                    <option value="" disabled>Выберите...</option>
                    {field.options.map((opt) => (
                      <option key={opt.value} value={opt.value}>{opt.label}</option>
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
        additionalTabs={getAdditionalTabs()}
      />

      <AddModel
        isOpen={isAddOpen}
        onClose={() => setIsAddOpen(false)}
        title={cfg.addTitle}
        fields={addFields}
        onAdd={handleAdd}
      />

      <CategoryManager isOpen={isCatOpen} onClose={() => setIsCatOpen(false)} />
    </>
  );
}

export default AdminTable;

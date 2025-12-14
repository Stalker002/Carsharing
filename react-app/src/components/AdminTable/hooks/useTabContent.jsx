import { useMemo } from "react";
import { TabForm } from "../TabForm";
import { columnsDocuments, headTextDocuments, fieldsClients } from "../configs";
import SubTable from "../../SubTable/SubTable";
import SimpleTable from "../../SimpleTable/SimpleTable";

export const useTabContent = ({
  activeTab,
  editingItem,
  detailingItem,
  subData,
  subOps,
}) => {
  return useMemo(() => {
    const currentItem = editingItem || detailingItem;
    if (!currentItem && !subData.isClientLoading) return [];
    const isEditMode = !!editingItem;

    const createActions = (type, btnText) => {
      if (!isEditMode) return {};
      return {
        onAdd: () => subOps.openSubAdd(type),
        onEdit: (item) => subOps.openSubEdit(item, type),
        onDelete: (id) => {
          subOps.handleSubDelete(id);
        },
        addButtonText: btnText,
      };
    };

    const TableComponent = isEditMode ? SubTable : SimpleTable;

    if (activeTab === "cars") {
      return [
        {
          title: "Страховки",
          content: (
            <TableComponent
              data={subData.insurances}
              columns={["id", "type", "company", "policyNumber", "endDate"]}
              headers={["ID", "Тип", "Компания", "Номер полиса", "Окончание"]}
              {...createActions("insurance", "Страховку")}
            />
          ),
        },
        {
          title: "Обслуживание",
          content: (
            <TableComponent
              data={subData.maintenances}
              columns={["id", "workType", "description", "cost", "date"]}
              headers={["ID", "Тип", "Описание", "Стоимость", "Дата"]}
              {...createActions("maintenance", "Обслуживание")}
            />
          ),
        },
      ];
    }

    if (activeTab === "users") {
      if (subData.isClientLoading)
        return [
          {
            title: "Клиент",
            content: <div style={{ padding: 20 }}>Загрузка...</div>,
          },
        ];
      if (!subData.clientProfile || !subData.clientProfile.id) {
        return [];
      }
      const clientContent = isEditMode ? (
        <TabForm
          initialData={subData.clientProfile || {}}
          fields={fieldsClients}
          onSave={subOps.handleClientProfileSave}
        />
      ) : (
        <div className="detail-modal-grid">
          {fieldsClients.map((f) => (
            <div key={f.name} className="detail-modal-row">
              <span className="detail-modal-label">{f.label}</span>
              <span className="detail-modal-value">
                {subData.clientProfile?.[f.name] || "—"}
              </span>
            </div>
          ))}
        </div>
      );

      return [
        { title: "Клиент", content: clientContent },
        {
          title: "Документы",
          content: (
            <TableComponent
              data={subData.clientDocuments}
              columns={columnsDocuments}
              headers={headTextDocuments}
              {...createActions("document", "Документ")}
            />
          ),
        },
      ];
    }

    if (activeTab === "trips") {
      return [
        {
          title: "Штрафы",
          content: (
            <TableComponent
              data={subData.tripFines}
              columns={["id", "type", "amount", "date", "statusId"]}
              headers={["ID", "Тип", "Сумма", "Дата", "Статус"]}
              {...createActions("fine", "Штраф")}
            />
          ),
        },
      ];
    }

    if (activeTab === "bills") {
      return [
        {
          title: "Платежи",
          content: (
            <TableComponent
              data={subData.billPayments}
              columns={["id", "sum", "method", "date"]}
              headers={["ID", "Сумма", "Метод", "Дата"]}
              {...createActions("payment", "Платеж")}
            />
          ),
        },
      ];
    }

    return [];
  }, [activeTab, editingItem, detailingItem, subData, subOps]);
};

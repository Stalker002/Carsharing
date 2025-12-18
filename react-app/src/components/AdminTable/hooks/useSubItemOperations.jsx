import { useState, useCallback, useMemo } from "react";
import { useDispatch } from "react-redux";
import { createInsurance, deleteInsurance, updateInsurance } from "../../../redux/actions/insurance";
import { createMaintenance, deleteMaintenance, updateMaintenance } from "../../../redux/actions/maintenance";
import { createFine, deleteFine, updateFine } from "../../../redux/actions/fines";
import { createPayment, deletePayment, updatePayment } from "../../../redux/actions/payments";
import { createClientDocument, deleteClientDocument, updateClientDocument } from "../../../redux/actions/clientDocuments";
import { updateClient } from "../../../redux/actions/clients";
import {
  fieldsFines, fieldsInsurances, fieldsMaintenances, fieldsPayments, fieldsClientDocuments
} from "../configs";
import { openModal } from "../../../redux/actions/modal";

export const useSubItemOperations = (activeTab, editingItem, subData) => {
  const dispatch = useDispatch();
  const [subEditingItem, setSubEditingItem] = useState(null);
  const [isSubAddOpen, setIsSubAddOpen] = useState(false);
  const [subType, setSubType] = useState(null);

  const openSubAdd = useCallback((type) => {
    setSubType(type);
    setIsSubAddOpen(true);
  }, []);

  const openSubEdit = useCallback((item, type) => {
    setSubType(type);
    setSubEditingItem(item);
  }, []);

  const handleSubDelete = useCallback(async (id) => {
    dispatch(openModal({
      title: "Удаление записи",
      message: "Вы уверены, что хотите удалить эту запись?",
      type: "confirm",
      confirmText: "Удалить",
      cancelText: "Отмена",
      onConfirm: async () => {
        let result = { success: false };
        switch (subType) {
          case "insurance": result = await dispatch(deleteInsurance(id)); break;
          case "maintenance": result = await dispatch(deleteMaintenance(id)); break;
          case "fine": result = await dispatch(deleteFine(id)); break;
          case "payment": result = await dispatch(deletePayment(id)); break;
          case "document": result = await dispatch(deleteClientDocument(id)); break;
          default: break;
        }
        if (result.success) {
          setSubEditingItem(null);
          if (subType === "document") {
             subData.fetchClientDocuments(subData.clientProfile?.id);
          } else if (editingItem) {
             subData.fetchSubData(editingItem.id, subType);
          }
        } else {
          dispatch(openModal({
            type: "error",
            title: "Ошибка",
            message: result.message || "Не удалось удалить запись"
          }));
        }
      }
    }));
  }, [subType, dispatch, editingItem, subData]);

  const handleSubAddSave = useCallback(async (data) => {
    const payload = { ...data, carId: Number(editingItem?.id) };
    let result = { success: false };

    switch (subType) {
      case "insurance":
        result = await dispatch(createInsurance({ ...payload, statusId: Number(payload.statusId), cost: Number(payload.cost) }));
        break;
      case "maintenance":
        result = await dispatch(createMaintenance({ ...payload, statusId: Number(payload.statusId), cost: Number(payload.cost) }));
        break;
      case "fine":
        result = await dispatch(createFine({ ...data, tripId: Number(editingItem.id), statusId: Number(data.statusId), amount: Number(data.amount), date: new Date(data.date).toISOString() }));
        break;
      case "payment":
        result = await dispatch(createPayment({ ...data, billId: Number(editingItem.id), sum: Number(data.sum), date: new Date(data.date).toISOString() }));
        break;
      case "document":
        if (!subData.clientProfile?.id) {
          dispatch(
            openModal({
              type: "error",
              title: "Внимание",
              message: "Профиль клиента не найден",
            })
          );
          return false;
        }
        result = await dispatch(createClientDocument({ ...data, clientId: subData.clientProfile.id, filePath: "Нету" }));
        break;
      default: break;
    }

    if (result.success) {
      subData.fetchSubData(editingItem.id);
      return true;
    } else {
      dispatch(
            openModal({
              type: "error",
              title: "Внимание",
              message: result.message,
            })
          );
      return false;
    }
  }, [subType, editingItem, subData, dispatch]);

  const handleSubUpdateSave = useCallback(async (e) => {
    e.preventDefault();
    const formData = new FormData(e.target);
    const finalData = { ...subEditingItem, ...Object.fromEntries(formData.entries()) };
    let result = { success: false };

    switch (subType) {
      case "insurance":
        result = await dispatch(updateInsurance(finalData.id, { ...finalData, carId: Number(editingItem.id), statusId: Number(finalData.statusId), cost: Number(finalData.cost) }));
        break;
      case "maintenance":
        result = await dispatch(updateMaintenance(finalData.id, { ...finalData, carId: Number(editingItem.id), statusId: Number(finalData.statusId), cost: Number(finalData.cost) }));
        break;
      case "fine":
        result = await dispatch(updateFine(finalData.id, { ...finalData, tripId: Number(editingItem.id), statusId: Number(finalData.statusId), amount: Number(finalData.amount), date: new Date(finalData.date).toISOString() }));
        break;
      case "payment":
        result = await dispatch(updatePayment(finalData.id, { ...finalData, billId: Number(editingItem.id), sum: Number(finalData.sum), date: new Date(finalData.date).toISOString() }));
        break;
      case "document":
        result = await dispatch(updateClientDocument(finalData.id, { ...finalData, clientId: subData.clientProfile.id, filePath: "Нету" }));
        break;
      default: break;
    }

    if (result.success) {
      setSubEditingItem(null);
      subData.fetchSubData(editingItem.id);
    } else {
      dispatch(
            openModal({
              type: "error",
              title: "Внимание",
              message: result.message,
            })
          );
    }
  }, [subType, subEditingItem, editingItem, subData, dispatch]);

  const handleClientProfileSave = useCallback(async (formData) => {
    if (!subData.clientProfile?.id) return;
    const result = await dispatch(updateClient(subData.clientProfile.id, { userId: Number(editingItem.id), ...subData.clientProfile, ...formData }));
    if (result.success) {
      dispatch(openModal({
                type: "success",
                title: "Клиент обновлен!",
                message: `Данные клиента обновлены`
            }));
      subData.fetchClientProfile(editingItem.id);
    } else {
      dispatch(
            openModal({
              type: "error",
              title: "Внимание",
              message: result.message,
            })
          );
    }
  }, [subData, editingItem, dispatch]);

  const subFields = useMemo(() => {
    switch (subType) {
      case "maintenance": return fieldsMaintenances.filter((f) => f.name !== "carId");
      case "insurance": return fieldsInsurances.filter((f) => f.name !== "carId");
      case "fine": return fieldsFines.filter((f) => f.name !== "tripId");
      case "payment": return fieldsPayments.filter((f) => f.name !== "billId");
      case "document": return fieldsClientDocuments.filter((f) => f.name !== "clientId");
      default: return [];
    }
  }, [subType]);

  return {
    subEditingItem, setSubEditingItem,
    isSubAddOpen, setIsSubAddOpen,
    subFields,
    openSubAdd, openSubEdit, handleSubDelete,
    handleSubAddSave, handleSubUpdateSave, handleClientProfileSave
  };
};
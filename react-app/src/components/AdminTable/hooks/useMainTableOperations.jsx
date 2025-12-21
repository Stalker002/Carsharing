import { useState, useRef, useEffect, useCallback } from "react";
import { useDispatch, useSelector } from "react-redux";
import { getStatuses } from "../../../redux/actions/statuses";
import { getCategories } from "../../../redux/actions/category";
import { openModal } from "../../../redux/actions/modal";

export const useMainTableOperations = (activeTab, cfg, subData) => {
  const dispatch = useDispatch();
  const [page, setPage] = useState(1);
  const isLoadingRef = useRef(false);
  const isSwitchingTable = useRef(false);

  const [editingItem, setEditingItem] = useState(null);
  const [detailingItem, setDetailingItem] = useState(null);
  const [isDetailLoading, setIsDetailLoading] = useState(false);

  const categoriesList = useSelector(
    (state) => state.categories?.categories || []
  );
  const statusesList = useSelector((state) => state.statuses?.statuses || []);

  //   useEffect(() => {
  //     isSwitchingTable.current = true;
  //     setPage(1);
  //     document.getElementById("container")?.scrollTo(0, 0);
  //   }, [activeTab]);

  useEffect(() => {
    const tabsWithStatuses = [
      "cars",
      "bookings",
      "bills",
      "promocodes",
      "trips",
      "fines",
      "insurances",
    ];
    const tabsWithCategories = ["cars"];

    if (tabsWithStatuses.includes(activeTab) && statusesList.length === 0)
      dispatch(getStatuses());
    if (tabsWithCategories.includes(activeTab) && categoriesList.length === 0)
      dispatch(getCategories());
  }, [activeTab, categoriesList.length, statusesList.length]);

  const refreshTable = useCallback(() => {
    document.getElementById("container")?.scrollTo(0, 0);
    if (page === 1 && cfg?.action) {
      isLoadingRef.current = true;
      dispatch(cfg.action(1)).finally(() => (isLoadingRef.current = false));
    } else {
      setPage(1);
    }
  }, [page, cfg, dispatch]);

  useEffect(() => {
    if (!cfg || !cfg.action || cfg.isDashboard) return;
    if (isSwitchingTable.current && page !== 1) return;
    if (page === 1) isSwitchingTable.current = false;
    if (isLoadingRef.current) return;

    isLoadingRef.current = true;
    dispatch(cfg.action(page)).finally(() => (isLoadingRef.current = false));
  }, [page, activeTab, dispatch]);

  const nextHandler = useCallback(() => {
    if (isLoadingRef.current || !cfg) return;
    if ((cfg.data?.length || 0) >= (cfg.total || 0)) return;
    setPage((p) => p + 1);
  }, [cfg]);

  const handleRowClick = useCallback(
    async (row) => {
      setDetailingItem(row);
      if (cfg.detailAction) {
        setIsDetailLoading(true);
        const result = await dispatch(cfg.detailAction(row.id));
        setIsDetailLoading(false);
        if (result.success && result.data) {
          setEditingItem(null);
          setDetailingItem(result.data);
          subData.clearSubData();
        } else {
          dispatch(
            openModal({
              type: "error",
              title: "Внимание",
              message: "Не удалось загрузить детали",
            })
          );
        }
      } else {
        setDetailingItem(row);
        if (activeTab === "users") {
          subData.clearSubData();
          if (row?.id) subData.fetchClientProfile(row.id);
        }
      }
    },
    [cfg, dispatch, activeTab, subData]
  );

  const handleEditClick = useCallback(
    async (item) => {
      setDetailingItem(null);
      if (cfg.detailAction) {
        const result = await dispatch(cfg.detailAction(item.id));
        if (result.success || result.data) {
          setEditingItem(result.data);
          subData.fetchSubData(result.data.id);
        } else {
          dispatch(
            openModal({
              type: "error",
              title: "Внимание",
              message: "Ошибка загрузки",
            })
          );
        }
      } else {
        setEditingItem(item);
        if (activeTab === "cars") subData.fetchSubData(item.id);
        if (activeTab === "users") {
          subData.fetchClientProfile(item.id);
      }
      }
    },
    [cfg, dispatch, activeTab, subData]
  );

  const handleSaveEdit = useCallback(
    async (e) => {
      e.preventDefault();
      if (!cfg?.updateAction) {
        return dispatch(
          openModal({
            type: "error",
            title: "Внимание",
            message: "Действие редактирования не настроено",
          })
        );
      }

      const formData = new FormData(e.target);
      const formJson = Object.fromEntries(formData.entries());

      const finalData = {
        ...editingItem,
        ...formJson,
        id: Number(editingItem.id),
        statusId: formJson.statusId ? Number(formJson.statusId) : editingItem.statusId,
        categoryId: formJson.categoryId ? Number(formJson.categoryId) : editingItem.categoryId,
        tariffId: formJson.tariffId ? Number(formJson.tariffId) : (editingItem.tariffId || null),
      };

      const result = await dispatch(cfg.updateAction(finalData.id, finalData));
      
      if (!result.success) {
        return dispatch(
          openModal({
            type: "error",
            title: "Внимание",
            message: result.message || "Ошибка сохранения",
          })
        );
      }

      setEditingItem(null);
      refreshTable();
    },
    [cfg, dispatch, editingItem, refreshTable]
  );

  const handleDelete = useCallback(async () => {
    if (!cfg?.deleteAction || !editingItem) return;
    dispatch(openModal({
      title: "Удаление записи",
      message: `Вы уверены, что хотите удалить запись #${editingItem.id}?`,
      type: "confirm",
      confirmText: "Удалить",
      cancelText: "Отмена",
      onConfirm: async () => {
        const result = await dispatch(cfg.deleteAction(editingItem.id));
        
        if (result && !result.success) {
          dispatch(openModal({
            type: "error",
            title: "Внимание",
            message: result.message || "Ошибка при удалении",
          }));
        } else {
          setEditingItem(null);
          refreshTable();
        }
      }
    }));
  }, [cfg, editingItem, dispatch, refreshTable]);

  const handleAdd = useCallback(
    async (data) => {
      if (cfg?.addAction) {
        const result = await dispatch(cfg.addAction(data));
        if (!result.success) {
          dispatch(
            openModal({
              type: "error",
              title: "Внимание",
              message: result.message,
            })
          );
          return false;
        }
        refreshTable();
        return true;
      }
      return false;
    },
    [cfg, dispatch, refreshTable]
  );

  return {
    editingItem,
    setEditingItem,
    detailingItem,
    setDetailingItem,
    isDetailLoading,
    categoriesList,
    statusesList,
    nextHandler,
    handleRowClick,
    handleEditClick,
    handleSaveEdit,
    handleDelete,
    handleAdd,
  };
};

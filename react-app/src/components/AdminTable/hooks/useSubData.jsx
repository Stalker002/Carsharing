import { useState, useCallback } from "react";
import { useDispatch } from "react-redux";
import { getInsuranceByCars } from "../../../redux/actions/insurance";
import { getMaintenanceByCars } from "../../../redux/actions/maintenance";
import {
  getClientByUser,
  getClientDocument,
} from "../../../redux/actions/clients";
import { getFinesByTrip } from "../../../redux/actions/fines";
import { getPaymentByBill } from "../../../redux/actions/payments";

export const useSubData = (activeTab) => {
  const dispatch = useDispatch();

  const [insurances, setInsurances] = useState([]);
  const [maintenances, setMaintenances] = useState([]);
  const [tripFines, setTripFines] = useState([]);
  const [billPayments, setBillPayments] = useState([]);

  const [clientProfile, setClientProfile] = useState(null);
  const [clientDocuments, setClientDocuments] = useState([]);
  const [isClientLoading, setIsClientLoading] = useState(false);

  const fetchSubData = useCallback(
    async (itemId, type) => {
      if (activeTab == "cars") {
        try {
          if (type === "insurance") {
            if (insurances.length > 0) return;
            const res = await dispatch(getInsuranceByCars(itemId));
            setInsurances(res.data || []);
          }

          if (type === "maintenance") {
            const res = await dispatch(getMaintenanceByCars(itemId));
            setMaintenances(res.data || []);
          }

          if (!type) {
            const [insRes, maintRes] = await Promise.all([
              dispatch(getInsuranceByCars(itemId)),
              dispatch(getMaintenanceByCars(itemId)),
            ]);
            setInsurances(insRes.data || []);
            setMaintenances(maintRes.data || []);
          }
        } catch (e) {
          console.error("Ошибка загрузки под-данных", e);
        }
      }
      if (activeTab === "users") {
        if (tabIndex === 0) subData.fetchClientProfile(item.id);
        if (tabIndex === 1 && subData.clientProfile?.id) {
          subData.fetchClientDocuments(subData.clientProfile.id);
        }
      }
      if (activeTab === "trips") {
        if (!type || type === "fine") {
          const res = await dispatch(getFinesByTrip(itemId));
          setTripFines(res.data || []);
        }
      }
      if (activeTab === "bills") {
        try {
          if (!type || type === "payment") {
            const res = await dispatch(getPaymentByBill(itemId));
            const list = Array.isArray(res.data)
              ? res.data
              : res.data?.data || [];
            setBillPayments(list);
          }
        } catch (e) {
          console.error("Ошибка загрузки платежей", e);
        }
      }
    },
    [activeTab, dispatch]
  );

  const fetchClientProfile = useCallback(
    async (userId) => {
      setIsClientLoading(true);
      try {
        const res = await dispatch(getClientByUser(userId));

        if (res.success && res.data) {
          const profile = Array.isArray(res.data) ? res.data[0] : res.data;
          setClientProfile(profile);
          if (profile?.id) {
            fetchClientDocuments(profile.id);
          }
        } else {
          setClientProfile(null);
          setClientDocuments([]);
        }
      } catch (e) {
        console.error(e);
        setClientProfile(null);
      } finally {
        setIsClientLoading(false);
      }
    },
    [dispatch]
  );

  const fetchClientDocuments = useCallback(
    async (clientId) => {
      if (!clientId) return;
      try {
        const res = await dispatch(getClientDocument(clientId));
        setClientDocuments(res.data || []);
      } catch (e) {
        console.error(e);
      }
    },
    [dispatch]
  );

  const clearSubData = useCallback(() => {
    setInsurances([]);
    setMaintenances([]);
    setClientProfile(null);
    setTripFines([]);
    setBillPayments([]);
    setClientDocuments([]);
  }, []);

  return {
    insurances,
    maintenances,
    clientProfile,
    tripFines,
    billPayments,
    clientDocuments,
    isClientLoading,
    fetchSubData,
    fetchClientProfile,
    fetchClientDocuments,
    clearSubData,
  };
};

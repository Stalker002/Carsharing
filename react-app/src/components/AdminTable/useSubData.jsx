import { useState, useCallback } from "react";
import { useDispatch } from "react-redux";
import { getInsuranceByCars } from "../../redux/actions/insurance";
import { getMaintenanceByCars } from "../../redux/actions/maintenance";
import { getClientByUser } from "../../redux/actions/clients";

export const useSubData = (activeTab) => {
  const dispatch = useDispatch();

  const [insurances, setInsurances] = useState([]);
  const [maintenances, setMaintenances] = useState([]);

  const [clientProfile, setClientProfile] = useState(null);
  const [isClientLoading, setIsClientLoading] = useState(false);

  const fetchSubData = useCallback(
    async (itemId, type) => {
      if (activeTab == "cars") {
        try {
          if (type === "insurance") {
            if (insurances.length > 0) return;
            const res = await dispatch(getInsuranceByCars(itemId));
            console.log("Insurance Response:", res);
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
    },
    [activeTab, dispatch]
  );

  const fetchClientProfile = useCallback(async (userId) => {
      setIsClientLoading(true);
      try {
          const res = await dispatch(getClientByUser(userId));
          
          if (res.success && res.data) {
             const profile = Array.isArray(res.data) ? res.data[0] : res.data;
             setClientProfile(profile);
          } else {
             setClientProfile(null);
          }
      } catch (e) {
          console.error(e);
          setClientProfile(null);
      } finally {
          setIsClientLoading(false);
      }
  }, [dispatch]);

  const clearSubData = useCallback(() => {
    setInsurances([]);
    setMaintenances([]);
    setClientProfile(null);
  }, []);

  return {
    insurances,
    maintenances,
    clientProfile,
    isClientLoading,
    fetchSubData,
    fetchClientProfile,
    clearSubData,
  };
};

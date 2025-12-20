// utils/formatters.js

// Форматирование цены (например: 150.00 BYN)
export const formatCurrency = (amount) => {
  if (amount === null || amount === undefined) return "0.00 BYN";
  return new Intl.NumberFormat("ru-BY", {
    style: "currency",
    currency: "BYN",
    minimumFractionDigits: 2,
  }).format(amount);
};

// Короткая дата (19 дек.)
export const formatDateShort = (dateString) => {
  if (!dateString) return "—";
  return new Date(dateString).toLocaleDateString("ru-RU", {
    day: "numeric",
    month: "short",
    year: "numeric",
  });
};

// Полная дата и время (19.12.2025, 14:30)
export const formatDateTime = (dateString) => {
  if (!dateString) return "—";
  return new Date(dateString).toLocaleString("ru-RU", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  });
};

// Длительность (125 мин -> 2 ч 5 мин)
export const formatDuration = (minutes) => {
  if (!minutes) return "0 мин";
  const h = Math.floor(minutes / 60);
  const m = Math.round(minutes % 60);
  if (h === 0) return `${m} мин`;
  return `${h} ч ${m} мин`;
};
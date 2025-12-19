// utils/formatters.js

export const formatDate = (isoString) => {
  if (!isoString) return "—";
  const date = new Date(isoString);
  // Формат: 02.08.2025 15:18
  return date.toLocaleString("ru-RU", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  });
};

export const formatDuration = (minutes) => {
  if (!minutes) return "0 мин.";
  const h = Math.floor(minutes / 60);
  const m = Math.floor(minutes % 60);
  if (h > 0) return `${h} ч. ${m} мин.`;
  return `${m} мин.`;
};

export const formatCurrency = (amount) => {
  return new Intl.NumberFormat("ru-RU", {
    style: "currency",
    currency: "BYN",
  }).format(amount);
};
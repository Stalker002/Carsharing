export const toLocalISOString = (val) => {
  if (!val) return "";
  const date = new Date(val);
  if (!isNaN(date.getTime()) && date.getFullYear() > 1970) {
    const offset = date.getTimezoneOffset() * 60000;
    return new Date(date.getTime() - offset).toISOString().slice(0, 16);
  }
  return "";
};
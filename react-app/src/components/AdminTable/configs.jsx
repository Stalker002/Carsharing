export const columnsUsers = ["id", "roleId", "login", "password"];
export const headTextUsers = ["ID", "ID Роли", "Логин", "Пароль"];

export const fieldsUsers = [
  { name: "login", label: "Логин", type: "text", placeholder: "user123", required: true },
  { name: "password", label: "Пароль", type: "text", placeholder: "secret", required: true },
  { 
    name: "roleId", 
    label: "Роль", 
    type: "select", 
    options: [
      { value: "1", label: "Администратор" },
      { value: "2", label: "Пользователь" }
    ],
    required: true
  }
];

export const columnsCars = ["id", "location", "fuelLevel", "statusId", "categoryId", "tariffId"];
export const headTextCars = ["ID", "Локация", "Топливо (%)", "ID Статуса", "ID Категории", "ID Тарифа"];

// === ПОЛЯ ФОРМЫ (Добавление / Редактирование) ===
export const fieldsCars = [
  // --- Блок 1: Основное (Машина) ---
  { 
    name: "id", 
    label: "ID", 
    type: "text", 
    readOnly: true, 
    hideOnAdd: true 
  },
  { 
    name: "location", 
    label: "Локация (Координаты/Адрес)", 
    type: "text", 
    placeholder: "53.9, 27.5", 
    required: true 
  },
  { 
    name: "fuelLevel", 
    label: "Уровень топлива (%)", 
    type: "number", 
    placeholder: "100", 
    required: true 
  },
  {
    name: "statusId",
    label: "Статус",
    type: "select",
    options: [
      { value: "1", label: "Доступна" },
      { value: "2", label: "В аренде" },
      { value: "3", label: "На обслуживании" },
    ],
    required: true
  },
  {
    name: "categoryId",
    label: "Категория",
    type: "select",
    options: [
      { value: "1", label: "Эконом" },
      { value: "2", label: "Комфорт" },
      { value: "3", label: "Бизнес" },
    ],
    required: true
  },

  // --- Блок 2: Спецификация (Specification) ---
  // В твоем контроллере UpdateCar НЕ обновляет эти данные.
  // Поэтому ставим hideOnEdit: true (или readOnly: true).
  
  { name: "brand", label: "Марка", type: "text", placeholder: "BMW", required: true, hideOnEdit: true },
  { name: "model", label: "Модель", type: "text", placeholder: "X5", required: true, hideOnEdit: true },
  { name: "year", label: "Год выпуска", type: "number", placeholder: "2023", required: true, hideOnEdit: true },
  { name: "vinNumber", label: "VIN номер", type: "text", required: true, hideOnEdit: true },
  { name: "stateNumber", label: "Гос. номер", type: "text", placeholder: "1234 AB-7", required: true, hideOnEdit: true },
  { name: "transmission", label: "Коробка", type: "text", placeholder: "Automatic", required: true, hideOnEdit: true },
  { name: "fuelType", label: "Тип топлива", type: "text", placeholder: "Petrol", required: true, hideOnEdit: true },
  { name: "mileage", label: "Пробег (км)", type: "number", required: true, hideOnEdit: true },
  { name: "maxFuel", label: "Бак (литров)", type: "number", required: true, hideOnEdit: true },
  { name: "fuelPerKm", label: "Расход (л/км)", type: "number", required: true, hideOnEdit: true },

  // --- Блок 3: Тариф (Tariff) ---
  // Тоже создается 1 раз при создании машины
  { name: "name", label: "Название тарифа", type: "text", placeholder: "Базовый", required: true, hideOnEdit: true },
  { name: "pricePerMinute", label: "Цена/мин", type: "number", required: true, hideOnEdit: true },
  { name: "pricePerKm", label: "Цена/км", type: "number", required: true, hideOnEdit: true },
  { name: "pricePerDay", label: "Цена/сутки", type: "number", required: true, hideOnEdit: true },

  // --- Блок 4: Картинка ---
  { 
    name: "image", 
    label: "Фото автомобиля", 
    type: "file", // ВАЖНО: нужно поддержать этот тип в AddModel
    required: true, 
    hideOnEdit: true // Обновление фото у тебя идет через отдельный метод /image, пока скроем здесь
  }
];


export const columnsTrips = ["id", "startDate", "endDate", "userId", "carId"];
export const headTextTrips = ["ID", "Начало", "Конец", "ID Клиента", "ID Авто"];

export const columnsPayments = ["id", "amount", "date", "status"];
export const headTextPayments = ["ID", "Сумма", "Дата", "Статус"];
export const columnsBills = ["id", "tripId", "promocodeId", "statusId", "issueDate", "amount", "remainingAmount"];
export const headTextBills = ["ID", "ID Поездки", "ID Промокода", "Статус", "Дата выставления", "Сумма", "Остаток"];

export const fieldsBills = [
  { 
    name: "id", 
    label: "ID", 
    type: "text", 
    readOnly: true, 
    hideOnAdd: true 
  },
  { 
    name: "tripId", 
    label: "ID Поездки", 
    type: "number", 
    placeholder: "123", 
    required: true 
  },
  { 
    name: "promocodeId", 
    label: "ID Промокода", 
    type: "number", 
    placeholder: "0 если нет", 
    required: false 
  },
  {
    name: "statusId",
    label: "Статус",
    type: "select",
    options: [
      { value: "1", label: "Не оплачен" },
      { value: "2", label: "Оплачен" },
      { value: "3", label: "Отменен" },
      { value: "4", label: "Просрочен" }
    ],
    required: true
  },
  { 
    name: "issueDate", 
    label: "Дата выставления", 
    type: "datetime-local", // Используем datetime, так как счета точные
    required: true 
  },
  { 
    name: "amount", 
    label: "Сумма счета (BYN)", 
    type: "number", 
    placeholder: "100.00", 
    required: true 
  },
  { 
    name: "remainingAmount", 
    label: "Остаток к оплате", 
    type: "number", 
    placeholder: "100.00", 
    required: true 
  }
];

export const columnsBookings = ["id", "statusId", "carId", "clientId", "startTime", "endTime"];
export const headTextBookings = ["ID", "ID Статуса", "ID машины", "ID клиента", "Начало бронирования", "Конец бронирования"];

export const fieldsBookings = [
  { 
    name: "id", 
    label: "ID", 
    type: "text", 
    readOnly: true, 
    hideOnAdd: true 
  },
  {
    name: "statusId",
    label: "Статус бронирования",
    type: "select",
    options: [
      { value: "5", label: "Активно (В поездке)" },
      { value: "6", label: "Завершено" },
      { value: "7", label: "Отменено" }
    ],
    required: true
  },
  { 
    name: "carId", 
    label: "ID Автомобиля", 
    type: "number", 
    placeholder: "Введите ID авто", 
    required: true 
  },
  { 
    name: "clientId", 
    label: "ID Клиента (User)", 
    type: "number", 
    placeholder: "Введите ID пользователя", 
    required: true 
  },
  { 
    name: "startTime", 
    label: "Время начала", 
    type: "datetime-local", 
    required: true 
  },
  { 
    name: "endTime", 
    label: "Время окончания", 
    type: "datetime-local", 
    required: true 
  }
];

export const columnsCars = ["id",  "statusId", "categoryId", "tariffId", "location", "fuelLevel"];
export const headTextCars = ["ID",  "ID Статуса", "ID Категории", "ID Тарифа", "Локация", "Топливо (%)"];

export const fieldsCars = [
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
    viewName: "statusName", // 🔥 Берем значение из statusName для просмотра
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
    viewName: "categoryName", // 🔥 Берем значение из categoryName для просмотра
    label: "Категория",
    type: "select",
    options: [
      { value: "1", label: "Эконом" },
      { value: "2", label: "Комфорт" },
      { value: "3", label: "Бизнес" },
    ],
    required: true
  },
  
  { name: "brand", label: "Марка", type: "text", placeholder: "BMW", required: true, hideOnEdit: true },
  { name: "model", label: "Модель", type: "text", placeholder: "X5", required: true, hideOnEdit: true },
  { name: "year", label: "Год выпуска", type: "number", placeholder: "2023", required: true, hideOnEdit: true },
  
  { 
    name: "vinNumber", 
    label: "VIN номер", 
    type: "text", 
    required: true, 
    hideOnEdit: true,
    // hideOnDetail: true // 💡 Раскомментируй, если хочешь скрыть VIN в просмотре
  },
  
  { name: "stateNumber", label: "Гос. номер", type: "text", placeholder: "1234 AB-7", required: true, hideOnEdit: true },
  { name: "transmission", label: "Коробка", type: "text", placeholder: "Automatic", required: true, hideOnEdit: true },
  { name: "fuelType", label: "Тип топлива", type: "text", placeholder: "Petrol", required: true, hideOnEdit: true },
  { name: "mileage", label: "Пробег (км)", type: "number", required: true, hideOnEdit: true },
  { name: "maxFuel", label: "Бак (литров)", type: "number", required: true, hideOnEdit: true },
  { name: "fuelPerKm", label: "Расход (л/км)", type: "number", required: true, hideOnEdit: true },

  { name: "name", label: "Название тарифа", type: "text", placeholder: "Базовый", required: true, hideOnEdit: true },
  { name: "pricePerMinute", label: "Цена/мин", type: "number", required: true, hideOnEdit: true },
  { name: "pricePerKm", label: "Цена/км", type: "number", required: true, hideOnEdit: true },
  { name: "pricePerDay", label: "Цена/сутки", type: "number", required: true, hideOnEdit: true },

  { 
    name: "image", 
    label: "Фото автомобиля", 
    type: "file",
    required: true, 
    hideOnEdit: true,
    hideOnDetail: true // 🔥 Скрываем поле загрузки файла в деталях
  }
];

export const columnsClients = ["id", "userId", "name", "surname", "phoneNumber", "email"];
export const headTextClients = ["ID", "ID Пользователя", "Имя", "Фамилия", "Номер телефона", "Почта"];

export const fieldsClients = [
  { 
    name: "id", 
    label: "ID", 
    type: "text", 
    readOnly: true, 
    hideOnAdd: true 
  },
  // ID Пользователя нужен для Связи при обновлении, но при создании он генерируется сам
  { 
    name: "userId", 
    label: "ID Аккаунта (User)", 
    type: "number", 
    readOnly: true, // Не даем менять привязку к юзеру вручную
    hideOnAdd: true 
  },
  // --- Поля для регистрации (ТОЛЬКО Create) ---
  {
    name: "login",
    label: "Логин (для входа)",
    type: "text",
    placeholder: "client_login",
    required: true,
    hideOnEdit: true // Скрываем при редактировании
  },
  {
    name: "password",
    label: "Пароль",
    type: "password", // Можно password, но админу иногда удобно видеть, что создает
    placeholder: "secret",
    required: true,
    hideOnEdit: true
  },
  // --- Основные поля ---
  { 
    name: "name", 
    label: "Имя", 
    type: "text", 
    required: true 
  },
  { 
    name: "surname", 
    label: "Фамилия", 
    type: "text", 
    required: true 
  },
  { 
    name: "phoneNumber", 
    label: "Телефон", 
    type: "text", 
    placeholder: "+375...",
    required: true 
  },
  { 
    name: "email", 
    label: "Email", 
    type: "email", 
    required: true 
  }
];

export const columnsFines = ["id", "tripId", "statusId", "type", "amount", "date"];
export const headTextFines = ["ID", "ID Поездки", "ID Статуса", "Тип штрафа", "Сумма", "Дата"];

export const fieldsFines = [
  { 
    name: "id", 
    label: "ID", 
    type: "text", 
    readOnly: true, 
    hideOnAdd: true 
  },
  { 
    name: "tripId", 
    label: "ID Поездки", 
    type: "number", 
    placeholder: "123", 
    required: true 
  },
  {
    name: "statusId",
    label: "Статус оплаты",
    type: "select",
    options: [
      { value: "1", label: "Не оплачен" },
      { value: "2", label: "Оплачен" },
      { value: "3", label: "Оспаривается" }
    ],
    required: true
  },
  { 
    name: "type", 
    label: "Тип нарушения", 
    type: "text", // Или select, если типы фиксированы
    placeholder: "Превышение скорости", 
    required: true 
  },
  { 
    name: "amount", 
    label: "Сумма штрафа (BYN)", 
    type: "number", 
    placeholder: "50.00", 
    required: true 
  },
  { 
    name: "date", 
    label: "Дата нарушения", 
    type: "datetime-local", 
    required: true 
  }
];

export const columnsInsurances = ["id", "carId", "statusId", "type", "company", "policeNumber", "startDate", "endDate", "cost"];
export const headTextInsurances = ["ID", "ID Машины", "ID Статуса", "Тип страховки", "Компания", "Номер полиса", "Дата начала", "Дата окончания", "Цена"];

export const fieldsInsurances = [
  { 
    name: "id", 
    label: "ID", 
    type: "text", 
    readOnly: true, 
    hideOnAdd: true 
  },
  { 
    name: "carId", 
    label: "ID Автомобиля", 
    type: "number", 
    placeholder: "123", 
    required: true 
  },
  {
    name: "statusId",
    label: "Статус полиса",
    type: "select",
    options: [
      { value: "1", label: "Действующий" },
      { value: "2", label: "Истек" },
      { value: "3", label: "Аннулирован" }
    ],
    required: true
  },
  { 
    name: "type", 
    label: "Тип страховки", 
    type: "text", 
    placeholder: "ОСАГО / КАСКО", 
    required: true 
  },
  { 
    name: "company", 
    label: "Страховая компания", 
    type: "text", 
    placeholder: "Белгосстрах", 
    required: true 
  },
  { 
    name: "policyNumber", 
    label: "Номер полиса", 
    type: "text", 
    placeholder: "АА 1234567", 
    required: true 
  },
  { 
    name: "startDate", 
    label: "Дата начала", 
    type: "date", 
    required: true 
  },
  { 
    name: "endDate", 
    label: "Дата окончания", 
    type: "date", 
    required: true 
  },
  { 
    name: "cost", 
    label: "Стоимость (BYN)", 
    type: "number", 
    placeholder: "150.00", 
    required: true 
  }
];

export const columnsPayments = ["id", "billId", "sum", "method", "date"];
export const headTextPayments = ["ID", "ID Счета", "Сумма", "Метод оплаты", "Дата"];

export const fieldsPayments = [
  { 
    name: "id", 
    label: "ID", 
    type: "text", 
    readOnly: true, 
    hideOnAdd: true 
  },
  { 
    name: "billId", 
    label: "ID Счета", 
    type: "number", 
    placeholder: "123", 
    required: true 
  },
  { 
    name: "sum", 
    label: "Сумма (BYN)", 
    type: "number", 
    placeholder: "50.00", 
    required: true 
  },
  {
    name: "method",
    label: "Метод оплаты",
    type: "select",
    options: [
      { value: "Card", label: "Карта" },
      { value: "Cash", label: "Наличные" },
      { value: "Bonus", label: "Бонусы" }
    ],
    required: true
  },
  { 
    name: "date", 
    label: "Дата платежа", 
    type: "datetime-local", 
    required: true 
  }
];

export const columnsPromocodes = ["id", "statusId", "code", "discount", "startDate", "endDate"];
export const headTextPromocodes = ["ID", "ID Статуса", "Промокод", "Скидка", "Дата начала", "Дата окончания"];

export const fieldsPromocodes = [
  { 
    name: "id", 
    label: "ID", 
    type: "text", 
    readOnly: true, 
    hideOnAdd: true 
  },
  {
    name: "statusId",
    label: "Статус",
    type: "select",
    options: [
      { value: "19", label: "Активен" },
      { value: "20", label: "Неактивен" },
      { value: "3", label: "Истек" }
    ],
    required: true
  },
  { 
    name: "code", 
    label: "Код (Название)", 
    type: "text", 
    placeholder: "SUMMER2025", 
    required: true 
  },
  { 
    name: "discount", 
    label: "Скидка", 
    type: "number", 
    placeholder: "10", 
    required: true 
  },
  { 
    name: "startDate", 
    label: "Дата начала", 
    type: "date", // Можно datetime-local, если важно время
    required: true 
  },
  { 
    name: "endDate", 
    label: "Дата окончания", 
    type: "date", 
    required: true 
  }
];

export const columnsTrips = ["id", "bookingId", "statusId", "tariffType", "startTime", "endTime","duration","distance" ];
export const headTextTrips = ["ID", "ID Брони", "Статус", "Тариф", "Начало", "Конец","Длительность (мин)","Дистанция (км)"];

// Поля Модалок
export const fieldsTrips = [
  // --- Основные поля (Trip) ---
  { 
    name: "id", 
    label: "ID", 
    type: "text", 
    readOnly: true, 
    hideOnAdd: true 
  },
  { 
    name: "bookingId", 
    label: "ID Бронирования", 
    type: "number", 
    placeholder: "123", 
    required: true 
  },
  {
    name: "statusId",
    label: "Статус поездки",
    type: "select",
    options: [
      { value: "1", label: "Начата" },
      { value: "2", label: "Завершена" },
      { value: "3", label: "Отменена" }
    ],
    required: true
  },
  { 
    name: "tariffType", 
    label: "Тип тарифа", 
    type: "text", 
    placeholder: "PerMinute / Daily", 
    required: true 
  },
  { 
    name: "startTime", 
    label: "Время начала", 
    type: "datetime-local", 
    required: true 
  },
  { 
    name: "endTime", 
    label: "Время окончания", 
    type: "datetime-local", 
    required: true 
  },
  { 
    name: "duration", 
    label: "Длительность (мин)", 
    type: "number", 
    placeholder: "45", 
    required: true 
  },
  { 
    name: "distance", 
    label: "Дистанция (км)", 
    type: "number", 
    placeholder: "12.5", 
    required: true 
  },

  // --- Детали поездки (TripDetail) - Только при создании ---
  { 
    name: "startLocation", 
    label: "Начальная локация", 
    type: "text", 
    placeholder: "Minsk, Lenina 1", 
    required: true,
    hideOnEdit: true 
  },
  { 
    name: "endLocation", 
    label: "Конечная локация", 
    type: "text", 
    placeholder: "Minsk, Mira 10", 
    required: true,
    hideOnEdit: true 
  },
  { 
    name: "fuelUsed", 
    label: "Потрачено топлива (л)", 
    type: "number", 
    placeholder: "5", 
    required: true,
    hideOnEdit: true 
  },
  {
    name: "insuranceActive",
    label: "Страховка была активна?",
    type: "select",
    options: [
      { value: "true", label: "Да" },
      { value: "false", label: "Нет" }
    ],
    required: true,
    hideOnEdit: true
  },
  {
    name: "refueled",
    label: "Была заправка?",
    type: "select",
    options: [
      { value: "true", label: "Да" },
      { value: "false", label: "Нет" }
    ],
    required: true,
    hideOnEdit: true
  }
];
export const columnsUsers = ["id", "roleId", "login", "password"];
export const headTextUsers = ["ID", "ID Роли", "Логин", "Пароль"];

export const fieldsUsers = [
  { 
    name: "id", 
    label: "ID", 
    type: "text", 
    readOnly: true, 
    hideOnAdd: true 
  },
  {
    name: "roleId",
    label: "Роль",
    type: "select",
    options: [
      { value: "1", label: "Администратор" },
      { value: "2", label: "Клиент" },
      { value: "3", label: "Менеджер" }
    ],
    required: true
  },
  { 
    name: "login", 
    label: "Логин", 
    type: "text", 
    placeholder: "user_login", 
    required: true 
  },
  { 
    name: "password", 
    label: "Пароль", 
    type: "password",
    placeholder: "secret", 
    required: true 
  }
];
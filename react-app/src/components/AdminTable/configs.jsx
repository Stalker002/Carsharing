export const columnsBills = [
  "id",
  "tripId",
  "promocodeId",
  "statusId",
  "issueDate",
  "amount",
  "remainingAmount",
];
export const headTextBills = [
  "ID",
  "ID Поездки",
  "ID Промокода",
  "Статус",
  "Дата выставления",
  "Сумма",
  "Остаток",
];

export const fieldsBills = [
  {
    name: "id",
    label: "ID",
    type: "text",
    readOnly: true,
    hideOnAdd: true,
  },
  {
    name: "tripId",
    label: "ID Поездки",
    type: "number",
    placeholder: "123",
    required: true,
  },
  {
    name: "promocodeId",
    label: "ID Промокода",
    type: "number",
    placeholder: "0 если нет",
    required: false,
  },
  {
    name: "statusId",
    label: "Статус",
    type: "select",
    options: [
      { value: "13", label: "Не оплачен" },
      { value: "14", label: "Частично оплачен" },
      { value: "15", label: "Оплачен" },
      { value: "16", label: "Отменён" },
    ],
    required: true,
  },
  {
    name: "issueDate",
    label: "Дата выставления",
    type: "datetime-local",
  },
  {
    name: "amount",
    label: "Сумма счета (BYN)",
    type: "number",
    placeholder: "100.00",
    required: true,
  },
  {
    name: "remainingAmount",
    label: "Остаток к оплате",
    type: "number",
    placeholder: "100.00",
    required: true,
  },
];

export const columnsBookings = [
  "id",
  "statusId",
  "carId",
  "clientId",
  "startTime",
  "endTime",
];
export const headTextBookings = [
  "ID",
  "ID Статуса",
  "ID машины",
  "ID клиента",
  "Начало бронирования",
  "Конец бронирования",
];

export const fieldsBookings = [
  {
    name: "id",
    label: "ID",
    type: "text",
    readOnly: true,
    hideOnAdd: true,
  },
  {
    name: "statusId",
    label: "Статус бронирования",
    type: "select",
    options: [
      { value: "5", label: "Активно" },
      { value: "6", label: "Завершено" },
      { value: "7", label: "Отменено" },
    ],
    required: true,
  },
  {
    name: "carId",
    label: "ID Автомобиля",
    type: "number",
    placeholder: "Введите ID авто",
    required: true,
  },
  {
    name: "clientId",
    label: "ID Клиента (User)",
    type: "number",
    placeholder: "Введите ID пользователя",
    required: true,
  },
  {
    name: "startTime",
    label: "Время начала",
    type: "datetime-local",
  },
  {
    name: "endTime",
    label: "Время окончания",
    type: "datetime-local",
    required: true,
  },
];

export const columnsCars = [
  "id",
  "statusId",
  "tariffId",
  "categoryId",
  "specificationId",
  "location",
  "fuelLevel",
];
export const headTextCars = [
  "ID",
  "ID Статуса",
  "ID Тарифа",
  "ID Категории",
  "ID Характеристики",
  "Локация",
  "Топливо (%)",
];

export const fieldsCars = [
  {
    name: "id",
    label: "ID",
    type: "text",
    readOnly: true,
    hideOnAdd: true,
  },
  {
    name: "statusId",
    viewName: "statusName",
    label: "Статус",
    type: "select",
    options: [],
    required: true,
  },
  {
    name: "categoryId",
    viewName: "categoryName",
    label: "Категория",
    type: "select",
    options: [],
    required: true,
  },
  {
    name: "transmission",
    label: "Коробка",
    type: "select",
    options: [
      { value: "Автомат", label: "Автомат" },
      { value: "Механика", label: "Механика" },
      { value: "Робот", label: "Робот" },
    ],
    placeholder: "Коробка передач",
    required: true,
  },
  {
    name: "brand",
    label: "Марка",
    type: "text",
    placeholder: "BMW",
    required: true,
  },
  {
    name: "model",
    label: "Модель",
    type: "text",
    placeholder: "X5",
    required: true,
  },
  {
    name: "year",
    label: "Год выпуска",
    type: "number",
    placeholder: "2023",
    required: true,
  },
  {
    name: "location",
    label: "Локация (Координаты/Адрес)",
    type: "text",
    placeholder: "53.9, 27.5",
    required: true,
  },

  {
    name: "fuelType",
    label: "Тип топлива",
    type: "select",
    options: [
      { value: "Бензин", label: "Бензин" },
      { value: "Дизель", label: "Дизель" },
      { value: "Электро", label: "Электро" },
      { value: "Гибрид", label: "Гибрид" },
      { value: "Газ", label: "Газ" },
    ],
    placeholder: "Бензин",
    required: true,
  },
  {
    name: "fuelLevel",
    label: "Уровень топлива (%)",
    type: "number",
    placeholder: "100",
    required: true,
  },
  {
    name: "maxFuel",
    label: "Бак (литров)",
    type: "number",
    required: true,
  },
  {
    name: "fuelPerKm",
    label: "Расход (л/км)",
    type: "number",
    required: true,
  },
  {
    name: "vinNumber",
    label: "VIN номер",
    type: "text",
    required: true,
  },
  {
    name: "stateNumber",
    label: "Гос. номер",
    type: "text",
    placeholder: "1234 AB-7",
    required: true,
  },
  {
    name: "mileage",
    label: "Пробег (км)",
    type: "number",
    required: true,
  },
  {
    name: "tariffName",
    label: "Название тарифа",
    type: "text",
    placeholder: "Базовый",
    required: true,
    hideOnDetail: true,
  },
  {
    name: "pricePerMinute",
    label: "Цена/мин",
    type: "number",
    required: true,
  },
  {
    name: "pricePerKm",
    label: "Цена/км",
    type: "number",
    required: true,
  },
  {
    name: "pricePerDay",
    label: "Цена/сутки",
    type: "number",
    required: true,
  },

  {
    name: "image",
    label: "Фото автомобиля",
    type: "file",
    required: false,
  },
];

export const columnsDocuments = [
  "id",
  "type",
  "number",
  "issueDate",
  "expiryDate",
];
export const headTextDocuments = ["ID", "Тип", "Номер", "Выдан", "Истекает"];

export const fieldsClientDocuments = [
  { name: "id", label: "ID", type: "text", readOnly: true, hideOnAdd: true },
  {
    name: "type",
    label: "Тип документа",
    type: "select",
    options: [
      { value: "Водительские права", label: "Водительские права" },
      { value: "Паспорт", label: "Паспорт" },
    ],
    placeholder: "Паспорт / ВУ",
    required: true,
  },
  {
    name: "number",
    label: "Номер",
    type: "text",
    required: true,
  },
  {
    name: "issueDate",
    label: "Дата выдачи",
    type: "date",
    required: true,
  },
  {
    name: "expiryDate",
    label: "Срок действия",
    type: "date",
    required: true,
  },
  // { name: "filePath", label: "Файл", type: "file", hideOnEdit: true }
];

export const columnsClients = [
  "id",
  "userId",
  "name",
  "surname",
  "phoneNumber",
  "email",
];
export const headTextClients = [
  "ID",
  "ID Пользователя",
  "Имя",
  "Фамилия",
  "Номер телефона",
  "Почта",
];

export const fieldsClients = [
  {
    name: "id",
    label: "ID",
    type: "text",
    readOnly: true,
    hideOnAdd: true,
  },
  {
    name: "userId",
    label: "ID Аккаунта (User)",
    type: "number",
    readOnly: true,
    hideOnAdd: true,
  },
  {
    name: "name",
    label: "Имя",
    type: "text",
    required: true,
  },
  {
    name: "surname",
    label: "Фамилия",
    type: "text",
    required: true,
  },
  {
    name: "phoneNumber",
    label: "Телефон",
    type: "text",
    placeholder: "+375(xx) xxx-xx-xx",
    required: true,
  },
  {
    name: "email",
    label: "Email",
    type: "email",
    required: true,
  },
];

export const columnsFines = [
  "id",
  "tripId",
  "statusId",
  "type",
  "amount",
  "date",
];
export const headTextFines = [
  "ID",
  "ID Поездки",
  "ID Статуса",
  "Тип штрафа",
  "Сумма",
  "Дата",
];

export const fieldsFines = [
  {
    name: "id",
    label: "ID",
    type: "text",
    readOnly: true,
    hideOnAdd: true,
  },
  {
    name: "tripId",
    label: "ID Поездки",
    type: "number",
    placeholder: "123",
    required: true,
  },
  {
    name: "statusId",
    label: "Статус оплаты",
    type: "select",
    options: [
      { value: "17", label: "Начислен" },
      { value: "18", label: "Ожидает оплаты" },
      { value: "19", label: "Оплачен" },
    ],
    required: true,
  },
  {
    name: "type",
    label: "Тип нарушения",
    type: "select",
    options: [
      { value: "Превышение скорости", label: "Превышение скорости" },
      {
        value: "Нарушение правил парковки",
        label: "Нарушение правил парковки",
      },
      { value: "Несчастный случай", label: "Несчастный случай" },
      { value: "Позднее возвращение", label: "Позднее возвращение" },
      { value: "Курение в машине", label: "Курение в машине" },
      { value: "Другое", label: "Другое" },
    ],
    placeholder: "Превышение скорости",
    required: true,
  },
  {
    name: "amount",
    label: "Сумма штрафа (BYN)",
    type: "number",
    placeholder: "50.00",
    required: true,
  },
  {
    name: "date",
    label: "Дата нарушения",
    type: "datetime-local",
    required: true,
  },
];

export const columnsInsurances = [
  "id",
  "carId",
  "statusId",
  "type",
  "company",
  "policeNumber",
  "startDate",
  "endDate",
  "cost",
];
export const headTextInsurances = [
  "ID",
  "ID Машины",
  "ID Статуса",
  "Тип страховки",
  "Компания",
  "Номер полиса",
  "Дата начала",
  "Дата окончания",
  "Цена",
];

export const fieldsInsurances = [
  {
    name: "id",
    label: "ID",
    type: "text",
    readOnly: true,
    hideOnAdd: true,
  },
  {
    name: "carId",
    label: "ID Автомобиля",
    type: "number",
    placeholder: "123",
    required: true,
  },
  {
    name: "statusId",
    label: "Статус полиса",
    type: "select",
    options: [
      { value: "23", label: "Активна" },
      { value: "24", label: "Истекла" },
      { value: "25", label: "Аннулирована" },
    ],
    required: true,
  },
  {
    name: "type",
    label: "Тип страховки",
    type: "select",
    options: [
      { value: "ОСАГО", label: "ОСАГО" },
      { value: "КАСКО", label: "КАСКО" },
    ],
    placeholder: "ОСАГО / КАСКО",
    required: true,
  },
  {
    name: "company",
    label: "Страховая компания",
    type: "text",
    placeholder: "Белгосстрах",
    required: true,
  },
  {
    name: "policyNumber",
    label: "Номер полиса",
    type: "text",
    placeholder: "АА 1234567",
    required: true,
  },
  {
    name: "startDate",
    label: "Дата начала",
    type: "date",
    required: true,
  },
  {
    name: "endDate",
    label: "Дата окончания",
    type: "date",
    required: true,
  },
  {
    name: "cost",
    label: "Стоимость (BYN)",
    type: "number",
    placeholder: "150.00",
    required: true,
  },
];

export const columnsMaintenances = [
  "id",
  "carId",
  "workType",
  "description",
  "cost",
  "date",
];

export const headTextMaintenances = [
  "ID",
  "ID Авто",
  "Тип работ",
  "Описание",
  "Стоимость",
  "Дата",
];

export const fieldsMaintenances = [
  {
    name: "id",
    label: "ID",
    type: "text",
    readOnly: true,
    hideOnAdd: true,
  },
  {
    name: "carId",
    label: "ID Автомобиля",
    type: "number",
    placeholder: "123",
    required: true,
  },
  {
    name: "workType",
    label: "Тип работ",
    type: "select",
    options: [
      { value: "Замена масла", label: "Замена масла" },
      { value: "Замена шин", label: "Замена шин" },
      { value: "Обслуживание тормозов", label: "Обслуживание тормозов" },
      { value: "Осмотр", label: "Осмотр" },
      { value: "Ремонт", label: "Ремонт" },
      { value: "Чистка", label: "Чистка" },
    ],
    placeholder: "Тип работ",
    required: true,
  },
  {
    name: "description",
    label: "Описание",
    type: "textarea",
    placeholder: "Детали выполненных работ",
    required: true,
  },
  {
    name: "cost",
    label: "Стоимость (BYN)",
    type: "number",
    step: "0.01",
    placeholder: "150.00",
    required: true,
  },
  {
    name: "date",
    label: "Дата проведения",
    type: "date",
    required: true,
  },
];

export const columnsPayments = ["id", "billId", "sum", "method", "date"];
export const headTextPayments = [
  "ID",
  "ID Счета",
  "Сумма",
  "Метод оплаты",
  "Дата",
];

export const fieldsPayments = [
  {
    name: "id",
    label: "ID",
    type: "text",
    readOnly: true,
    hideOnAdd: true,
  },
  {
    name: "billId",
    label: "ID Счета",
    type: "number",
    placeholder: "123",
    required: true,
  },
  {
    name: "sum",
    label: "Сумма (BYN)",
    type: "number",
    placeholder: "50.00",
    required: true,
  },
  {
    name: "method",
    label: "Метод оплаты",
    type: "select",
    options: [
      { value: "Картой", label: "Картой" },
      { value: "Наличными", label: "Наличными" },
      { value: "ЕРИП", label: "ЕРИП" },
      { value: "Другое", label: "Другое" },
    ],
    required: true,
  },
  {
    name: "date",
    label: "Дата платежа",
    type: "datetime-local",
    required: true,
  },
];

export const columnsPromocodes = [
  "id",
  "statusId",
  "code",
  "discount",
  "startDate",
  "endDate",
];
export const headTextPromocodes = [
  "ID",
  "ID Статуса",
  "Промокод",
  "Скидка",
  "Дата начала",
  "Дата окончания",
];

export const fieldsPromocodes = [
  {
    name: "id",
    label: "ID",
    type: "text",
    readOnly: true,
    hideOnAdd: true,
  },
  {
    name: "statusId",
    label: "Статус",
    type: "select",
    options: [
      { value: "20", label: "Активен" },
      { value: "21", label: "Истёк" },
      { value: "22", label: "Использован" },
    ],
    required: true,
  },
  {
    name: "code",
    label: "Код (Название)",
    type: "text",
    placeholder: "SUMMER2025",
    required: true,
  },
  {
    name: "discount",
    label: "Скидка",
    type: "number",
    placeholder: "10",
    required: true,
  },
  {
    name: "startDate",
    label: "Дата начала",
    type: "date",
    required: true,
  },
  {
    name: "endDate",
    label: "Дата окончания",
    type: "date",
    required: true,
  },
];

export const columnsTrips = [
  "id",
  "bookingId",
  "statusId",
  "tariffType",
  "startTime",
  "endTime",
  "duration",
  "distance",
];
export const headTextTrips = [
  "ID",
  "ID Брони",
  "Статус",
  "Тариф",
  "Начало",
  "Конец",
  "Длительность (мин)",
  "Дистанция (км)",
];

export const fieldsTrips = [
  {
    name: "id",
    label: "ID",
    type: "text",
    readOnly: true,
    hideOnAdd: true,
  },
  {
    name: "bookingId",
    label: "ID Бронирования",
    type: "number",
    placeholder: "123",
    required: true,
  },
  {
    name: "statusId",
    viewName: "statusName",
    label: "Статус поездки",
    type: "select",
    options: [
      { value: "8", label: "Ожидание начала" },
      { value: "9", label: "В пути" },
      { value: "10", label: "Завершена" },
      { value: "11", label: "Отменена системой" },
      { value: "12", label: "Требуется оплата" },
    ],
    required: true,
  },
  {
    name: "tariffType",
    label: "Тип тарифа",
    type: "select",
    options: [
      { value: "per_minute", label: "За минуту" },
      { value: "per_km", label: "За км" },
      { value: "per_day", label: "За день" },
    ],
    placeholder: "Тип тарифа",
    required: true,
  },
  {
    name: "startTime",
    label: "Время начала",
    type: "datetime-local",
    required: true,
  },
  {
    name: "endTime",
    label: "Время окончания",
    type: "datetime-local",
  },
  {
    name: "duration",
    label: "Длительность (мин)",
    type: "number",
    placeholder: "45",
    required: true,
  },
  {
    name: "distance",
    label: "Дистанция (км)",
    type: "number",
    placeholder: "12.5",
    required: true,
  },
  {
    name: "startLocation",
    label: "Начальная локация",
    type: "text",
    placeholder: "Minsk, Lenina 1",
    required: true,
    hideOnEdit: true,
  },
  {
    name: "endLocation",
    label: "Конечная локация",
    type: "text",
    placeholder: "Minsk, Mira 10",
    required: true,
    hideOnEdit: true,
  },
  {
    name: "fuelUsed",
    label: "Потрачено топлива (л)",
    type: "number",
    placeholder: "5",
    step: "0.01",
    hideOnEdit: true,
  },
  {
    name: "insuranceActive",
    label: "Страховка была активна?",
    type: "select",
    options: [
      { value: "true", label: "Да" },
      { value: "false", label: "Нет" },
    ],
    required: true,
    hideOnEdit: true,
  },
  {
    name: "refueled",
    label: "Заправлено (литров)",
    type: "number",
    placeholder: "0",
    step: "0.01",
    required: false, 
  },
];
export const columnsUsers = ["id", "roleId", "login"];
export const headTextUsers = ["ID", "ID Роли", "Логин"];

export const fieldsUsers = [
  {
    name: "id",
    label: "ID",
    type: "text",
    readOnly: true,
    hideOnAdd: true,
  },
  {
    name: "roleId",
    label: "Роль",
    type: "select",
    options: [
      { value: "1", label: "Администратор" },
      { value: "2", label: "Клиент" },
    ],
    required: true,
  },
  {
    name: "login",
    label: "Логин",
    type: "text",
    placeholder: "user_login",
    required: true,
  },
  {
    name: "password",
    label: "Пароль",
    type: "password",
    placeholder: "secret",
    required: true,
  },
];

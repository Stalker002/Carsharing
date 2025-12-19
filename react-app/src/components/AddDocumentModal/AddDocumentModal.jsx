import { useState } from "react";
import { useDispatch } from "react-redux";
import { openModal } from "../../redux/actions/modal";
import { createClientDocument } from "../../redux/actions/clientDocuments";
import { getMyDocuments } from "../../redux/actions/clients";

const AddDocumentModal = ({ clientId, onClose }) => {
  const dispatch = useDispatch();

  // Начальное состояние формы
  const [formData, setFormData] = useState({
    documentNumber: "",
    licenseCategory: "B",
    issueDate: "",
    expiryDate: "",
  });

  const [file, setFile] = useState(null);

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleFileChange = (e) => {
    setFile(e.target.files[0]);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    // Простая валидация
    if (
      !formData.documentNumber ||
      !formData.issueDate ||
      !formData.expiryDate
    ) {
      dispatch(
        openModal({
          type: "error",
          title: "Ошибка",
          message: "Заполните все поля",
        })
      );
      return;
    }
    const cleanNumber = formData.documentNumber.replace(/\s/g, "").toUpperCase();
    const licenseRegex = /^[A-ZА-Я]{2}\d{7}$/;

    if (!licenseRegex.test(cleanNumber)) {
        dispatch(openModal({ 
            type: "error", 
            title: "Ошибка формата", 
            message: "Номер прав должен содержать 2 буквы и 7 цифр (Например: AA0000000)" 
        }));
        return;
    }
    if (!file) {
      dispatch(
        openModal({
          type: "error",
          title: "Ошибка",
          message: "Прикрепите фото прав",
        })
      );
      return;
    }

    const dataToSend = new FormData();
    dataToSend.append("ClientId", clientId);
    dataToSend.append("Type", "Водительские права");
    dataToSend.append("LicenseCategory", formData.licenseCategory);
    dataToSend.append("Number", cleanNumber.toUpperCase());
    dataToSend.append("IssueDate", formData.issueDate);
    dataToSend.append("ExpiryDate", formData.expiryDate);
    dataToSend.append("File", file);

    // Отправляем
    const res = await dispatch(createClientDocument(dataToSend));

    if (res.success) {
      dispatch(getMyDocuments(clientId)); // Обновляем список
      onClose();
    }
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <h3>Добавить водительские права</h3>

        <form onSubmit={handleSubmit} className="modal-form">
          <div className="form-group" style={{ flex: 1 }}>
            <label>Категория</label>
            <input
              type="text"
              name="licenseCategory"
              placeholder="B"
              value={formData.licenseCategory}
              onChange={handleChange}
              required
            />
          </div>
          <div className="form-group">
            <label>Номер удостоверения</label>
            <input
              type="text"
              name="documentNumber"
              placeholder="Например: 123456789"
              value={formData.documentNumber}
              onChange={handleChange}
              required
            />
          </div>
          <div style={{ display: "flex", gap: "15px" }}>
            <div className="form-group" style={{ flex: 1 }}>
              <label>Дата выдачи</label>
              <input
                type="date"
                name="issueDate"
                value={formData.issueDate}
                onChange={handleChange}
                required
              />
            </div>
            <div className="form-group" style={{ flex: 1 }}>
              <label>Срок действия</label>
              <input
                type="date"
                name="expiryDate"
                value={formData.expiryDate}
                onChange={handleChange}
                required
              />
            </div>
          </div>
          <div className="form-group">
            <label>Фотография (Скан)</label>
            <input
              type="file"
              accept="image/*"
              onChange={handleFileChange}
              required
            />
          </div>
          <div className="modal-actions">
            <button type="button" className="btn-secondary" onClick={onClose}>
              Отмена
            </button>
            <button type="submit" className="btn-primary">
              Загрузить
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default AddDocumentModal;

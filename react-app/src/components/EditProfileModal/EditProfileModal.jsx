import { useState } from "react";
import { useDispatch } from "react-redux";
import { getMyClient, updateClient } from "../../redux/actions/clients";
import "./../GlobalModal/GlobalModal.css";
import { openModal } from "../../redux/actions/modal";

const EditProfileModal = ({ client, onClose }) => {
  const dispatch = useDispatch();

  const [formData, setFormData] = useState({    
    userId: client.userId || client.user_id,
    name: client.name || "",
    surname: client.surname || "",
    phoneNumber: client.phoneNumber || "",
    email: client.email || "",
  });

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (
      !formData.name ||
      !formData.surname ||
      !formData.phoneNumber ||
      !formData.email
    ) {
      dispatch(
        openModal({
          type: "error",
          title: "Ошибка",
          message: "Пожалуйста, заполните все поля.",
        })
      );
      return;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(formData.email)) {
      dispatch(
        openModal({
          type: "error",
          title: "Ошибка",
          message: "Введите корректный email!",
        })
      );
      return;
    }

    const cleanedPhone = formData.phoneNumber.replace(/[^\d+]/g, "");
    const phoneRegex = /^(\+375|80)(29|44|33|25)\d{7}$/;

    if (!phoneRegex.test(cleanedPhone)) {
      dispatch(
        openModal({
          type: "error",
          title: "Ошибка",
          message: "Номер должен быть в формате +375XXXXXXXXX или 80XXXXXXXXX (коды 29, 44, 33, 25, 14)",
        })
      );
      return;
    }

    const result = await dispatch(updateClient(client.id, formData));

    if (!result.success) {
      dispatch(
        openModal({
          type: "error",
          title: "Внимание",
          message: result.message || "Произошла ошибка при обновлении",
        })
      );
      return;
    }
    onClose();
    dispatch(getMyClient());
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <h3>Редактирование профиля</h3>
        <form className="modal-form" onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Имя</label>
            <input
              type="text"
              name="name"
              value={formData.name}
              onChange={handleChange}
              required
            />
          </div>
          <div className="form-group">
            <label>Фамилия</label>
            <input
              type="text"
              name="surname"
              value={formData.surname}
              onChange={handleChange}
              required
            />
          </div>
          <div className="form-group">
            <label>Телефон</label>
            <input
              type="text"
              name="phoneNumber"
              value={formData.phoneNumber}
              onChange={handleChange}
              placeholder="+375 (XX) XXX-XX-XX"
              required
            />
          </div>
          <div className="form-group">
            <label>Email</label>
            <input
              type="email"
              name="email"
              value={formData.email}
              onChange={handleChange}
              required
            />
          </div>

          <div className="modal-actions">
            <button type="button" className="btn-secondary" onClick={onClose}>
              Отмена
            </button>
            <button type="submit" className="btn-primary">
              Сохранить
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default EditProfileModal;

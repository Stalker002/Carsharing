import { useState } from "react";
import { useDispatch } from "react-redux";
import { updateClientDocument } from "../../redux/actions/clientDocuments";

const AddDocumentModal = ({ onClose }) => {
  const dispatch = useDispatch();
  const [type, setType] = useState("Водительское удостоверение");
  const [number, setNumber] = useState("");
  const [expiryDate, setExpiryDate] = useState("");
  const [file, setFile] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();
    // if (!file) return alert("Выберите файл");

    const formData = new FormData();
    formData.append("DocumentType", type);
    formData.append("DocumentNumber", number);
    formData.append("DocumentExpiryDate", expiryDate);
    formData.append("File", null);

    const result = await dispatch(updateClientDocument(formData));
    if (result.success) onClose();
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <h3>Добавить документ</h3>
        <form onSubmit={handleSubmit} className="edit-form">
          <div className="form-group">
            <label>Тип документа</label>
            <select value={type} onChange={(e) => setType(e.target.value)}>
              <option value="Водительское удостоверение">Водительское удостоверение</option>
              <option value="Паспорт">Паспорт</option>
            </select>
          </div>
          <div className="form-group">
            <label>Номер документа</label>
            <input type="text" value={number} onChange={(e) => setNumber(e.target.value)} required />
          </div>
          <div className="form-group">
            <label>Срок действия</label>
            <input type="date" value={expiryDate} onChange={(e) => setExpiryDate(e.target.value)} required />
          </div>
          {/* <div className="form-group">
            <label>Фотография / Скан</label>
            <input type="file" onChange={(e) => setFile(e.target.files[0])} required />
          </div> */}
          
          <div className="modal-actions">
            <button type="submit" className="btn-primary">Загрузить</button>
            <button type="button" className="btn-secondary" onClick={onClose}>Отмена</button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default AddDocumentModal;
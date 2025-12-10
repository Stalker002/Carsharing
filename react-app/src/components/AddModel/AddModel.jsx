import { useEffect } from "react";
import "./AddModel.css";

function AddModel({ isOpen, onClose, title, fields, onAdd }) {
  if (!isOpen) return null;

  if (!fields) return null;

  useEffect(() => {
    const handleKeyDown = (e) => {
      if (e.key === "Escape") onClose();
    };
    window.addEventListener("keydown", handleKeyDown);
    return () => window.removeEventListener("keydown", handleKeyDown);
  }, [onClose]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    const formData = new FormData(e.target);
    const data = Object.fromEntries(formData.entries());
    const isSuccess = await onAdd(data);

    if (isSuccess) {
      onClose();
    }
  };

  return (
    <div className="modal-overlay" onMouseDown={onClose}>
      <div className="admin-modal-container" onMouseDown={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h3>{title}</h3>
          <button className="modal-close" onClick={onClose}>
            ×
          </button>
        </div>

        <form onSubmit={handleSubmit}>
          <div className="modal-body">
            {fields.map((field) => (
              <div key={field.name} className="form-group">
                <label>{field.label}</label>

                {field.type === "select" ? (
                  <select
                    name={field.name}
                    className="modal-input"
                    required={field.required}
                    defaultValue=""
                  >
                    <option value="" disabled>
                      Выберите...
                    </option>
                    {field.options.map((opt) => (
                      <option key={opt.value} value={opt.value}>
                        {opt.label}
                      </option>
                    ))}
                  </select>
                ) : field.type === "textarea" ? (
                  <textarea
                    name={field.name}
                    className="modal-input"
                    placeholder={field.placeholder}
                    rows={3}
                    required={field.required}
                  />
                ) : field.type === "file" ? (
                  <div key={field.name} className="form-group">
                    <label>{field.label}</label>
                    <input
                      type="file"
                      name={field.name}
                      className="modal-input"
                      accept="image/*"
                      required={field.required}
                    />
                  </div>
                ) : (
                  <input
                    type={field.type}
                    name={field.name}
                    className="modal-input"
                    placeholder={field.placeholder}
                    required={field.required}
                    step={field.step}
                  />
                )}
              </div>
            ))}
          </div>

          <div className="modal-footer">
            <button type="submit" className="modal-add-btn">
              Добавить
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default AddModel;

 export const TabForm = ({ initialData, fields, onSave, btnText = "Сохранить" }) => {
  const handleSubmit = (e) => {
    e.preventDefault();
    const formData = new FormData(e.target);
    const data = Object.fromEntries(formData.entries());
    onSave({ ...data });
  };

  return (
    <form onSubmit={handleSubmit} className="update-modal-form-grid">
      {fields.map((field) => (
        <div className="update-group" key={field.name}>
          <label>{field.label}</label>
          <input
            type={field.type}
            name={field.name}
            defaultValue={initialData[field.name]}
            className="modal-input"
            readOnly={field.readOnly}
            required={field.required}
          />
        </div>
      ))}
      <div style={{ gridColumn: "span 2", marginTop: "20px", textAlign: "right" }}>
        <button type="submit" className="btn-save">{btnText}</button>
      </div>
    </form>
  );
};
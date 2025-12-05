import "./AddModel.css"

function AddModel({ isOpen, onClose, activeTable, onAdd }) {
    if (!isOpen) return null;

    return (
        <div className="modal-overlay" onClick={onClose}>
            <div className="modal-container" onClick={(e) => e.stopPropagation()}>
                <div className="modal-header">
                    <h3>Добавление работы</h3>
                    <button className="modal-close" onClick={onClose}>×</button>
                </div>
                <div className="modal-body">
                    <label>Название работы</label>
                    <input type="text" placeholder="Введите название" id="work-title" />

                    <label>Категория</label>
                    <select id="work-category">
                        <option value="">Выберите категорию</option>
                        <option value="Подвеска">Подвеска</option>
                        <option value="Визуализация">Визуализация</option>
                        <option value="Внутрянка">Внутрянка</option>
                    </select>

                    <label>Описание</label>
                    <textarea
                        id="work-desc"
                        placeholder="Введите описание"
                        rows={3}
                    />

                    <label>Нормативное время</label>
                    <input type="number" id="work-time" placeholder="Введите время" />
                </div>

                <div className="modal-footer">
                    <button
                        className="modal-add-btn"
                        onClick={() => {
                            const data = {
                                title: document.getElementById("work-title").value,
                                category: document.getElementById("work-category").value,
                                description: document.getElementById("work-desc").value,
                                time: document.getElementById("work-time").value,
                            };
                            onAdd(data);
                            onClose();
                        }}
                    >
                        Добавить
                    </button>
                </div>
            </div>
        </div>
    )
}

export default AddModel
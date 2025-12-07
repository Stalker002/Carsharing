import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
// Импортируй свои экшены для категорий
 // <-- Убедись, что создал этот файл actions

import "./CategoryManager.css";
import { createCategory, deleteCategory, getCategories, updateCategory } from "../../redux/actions/category";

const CategoryManager = ({ isOpen, onClose }) => {
  const dispatch = useDispatch();
  
  // Берем категории из Redux (или можно грузить локально)
  // Предположим, что в redux они лежат в state.categories.items
  const categories = useSelector((state) => state.categories?.categories || []); 
  
  const [newCategoryName, setNewCategoryName] = useState("");
  const [editingId, setEditingId] = useState(null);
  const [editingName, setEditingName] = useState("");

  // При открытии модалки загружаем актуальный список
  useEffect(() => {
    if (isOpen) {
      dispatch(getCategories());
    }
  }, [isOpen, dispatch]);

  if (!isOpen) return null;

  // --- Хендлеры ---

  // 1. Создание
  const handleAdd = async (e) => {
    e.preventDefault();
    if (!newCategoryName.trim()) return;
    
    await dispatch(createCategory({ name: newCategoryName }));
    setNewCategoryName("");
    dispatch(getCategories()); // Если createCategory не обновляет стейт сам
  };

  // 2. Удаление
  const handleDelete = async (id) => {
    if (window.confirm("Удалить категорию?")) {
      await dispatch(deleteCategory(id));
    }
  };

  // 3. Начало редактирования
  const startEdit = (cat) => {
    setEditingId(cat.id);
    setEditingName(cat.name);
  };

  // 4. Сохранение редактирования
  const saveEdit = async () => {
    if (!editingName.trim()) return;
    await dispatch(updateCategory(editingId, { id: editingId, name: editingName }));
    setEditingId(null);
  };

  return (
    <div className="modal-overlay" onMouseDown={onClose}>
      <div className="admin-modal-container category-modal" onMouseDown={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h3>Управление категориями</h3>
          <button className="modal-close" onClick={onClose}>×</button>
        </div>

        <div className="modal-body category-body">
          
          {/* Форма добавления */}
          <div className="add-category-row">
            <input 
              type="text" 
              className="modal-input" 
              placeholder="Название новой категории..."
              value={newCategoryName}
              onChange={(e) => setNewCategoryName(e.target.value)}
            />
            <button className="modal-add-btn small-btn" onClick={handleAdd}>
              +
            </button>
          </div>

          {/* Список категорий */}
          <div className="categories-list">
            {categories.length === 0 && <p className="empty-text">Категорий нет</p>}
            
            {categories.map((cat) => (
              <div key={cat.id} className="category-item">
                {editingId === cat.id ? (
                  // Режим редактирования строки
                  <>
                    <input 
                      type="text" 
                      className="modal-input edit-input"
                      value={editingName}
                      onChange={(e) => setEditingName(e.target.value)}
                    />
                    <div className="item-actions">
                      <button className="icon-btn save-btn" onClick={saveEdit}>✔</button>
                      <button className="icon-btn cancel-btn" onClick={() => setEditingId(null)}>✖</button>
                    </div>
                  </>
                ) : (
                  // Режим просмотра строки
                  <>
                    <span className="cat-name">{cat.name}</span>
                    <div className="item-actions">
                      <button className="icon-btn edit-btn" onClick={() => startEdit(cat)}>✎</button>
                      <button className="icon-btn del-btn" onClick={() => handleDelete(cat.id)}>🗑</button>
                    </div>
                  </>
                )}
              </div>
            ))}
          </div>
        </div>

        <div className="modal-footer">
          <button className="modal-add-btn grey-btn" onClick={onClose}>Закрыть</button>
        </div>
      </div>
    </div>
  );
};

export default CategoryManager;
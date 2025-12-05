import React, { useEffect } from "react";
import "./UpdateModal.css";

const UpdateModal = ({ 
  title, 
  onClose, 
  children, 
  onDelete, // Новая функция для удаления
  formId,   // ID формы, чтобы кнопка "Сохранить" работала снаружи
  saveLabel = "Сохранить" // Текст кнопки (опционально)
}) => {
  
  useEffect(() => {
    const handleKeyDown = (e) => {
      if (e.key === "Escape") onClose();
    };
    window.addEventListener("keydown", handleKeyDown);
    return () => window.removeEventListener("keydown", handleKeyDown);
  }, [onClose]);

  return (
    <div className="modal-overlay" onMouseDown={onClose}>
      <div className="modal-content" onMouseDown={(e) => e.stopPropagation()}>
        
        <div className="modal-header">
          <h3>{title}</h3>
          <button className="close-icon-btn" onClick={onClose}>×</button>
        </div>

        {/* Тело модалки (здесь будут инпуты) */}
        <div className="modal-body">
          {children}
        </div>

        {/* Футер с кнопками теперь тут */}
        <div className="modal-actions">
          {/* Кнопка УДАЛИТЬ (рендерим, только если передали функцию onDelete) */}
          {onDelete ? (
            <button 
              type="button" 
              className="btn-delete" 
              onClick={onDelete}
            >
              Удалить
            </button>
          ) : (
            <div></div> /* Пустой блок, чтобы flexbox не ломался */
          )}

          <div className="right-actions">
            <button 
              type="button" 
              className="btn-cancel" 
              onClick={onClose}
            >
              Отмена
            </button>
            
            {/* 
                ВАЖНО: form={formId} 
                Это связывает кнопку с тегом <form id="...">, который лежит внутри children 
            */}
            <button 
              type="submit" 
              form={formId} 
              className="btn-save"
            >
              {saveLabel}
            </button>
          </div>
        </div>
        
      </div>
    </div>
  );
};

export default UpdateModal;

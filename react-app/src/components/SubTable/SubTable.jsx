import React from "react";
// Убедись, что подключил файл стилей, если создал отдельный
import "./SubTable.css"; 

const SubTable = ({ 
  data, 
  columns, 
  headers, 
  onAdd, 
  onEdit, 
  onDelete, 
  addButtonText = "Добавить" 
}) => {
  return (
    <div className="sub-table-wrapper">
      <div className="sub-table-actions">
        <button className="sub-add-btn" onClick={onAdd}>
          + {addButtonText}
        </button>
      </div>

      {/* Проверка на пустоту */}
      {(!data || data.length === 0) ? (
        <div className="sub-empty-text">Нет записей</div>
      ) : (
        <table className="sub-table">
          <thead>
            <tr>
              {headers.map((h, i) => <th key={i}>{h}</th>)}
              <th style={{ width: "100px", textAlign: "right" }}>Действия</th>
            </tr>
          </thead>
          <tbody>
            {data.map((row, i) => (
              <tr key={row.id || i}>
                {/* Данные колонок */}
                {columns.map((col, j) => (
                  <td key={j}>
                    {/* Форматирование дат */}
                    {col.includes("Date") || col === "date" || col.includes("time")
                      ? new Date(row[col]).toLocaleString("ru-RU", {
                          day: 'numeric', month: 'numeric', year: 'numeric',
                          hour: '2-digit', minute: '2-digit'
                        })
                      : row[col]}
                  </td>
                ))}
                
                {/* Колонка действий */}
                <td>
                  <div className="sub-action-buttons">
                    <button 
                        className="sub-icon-btn edit" 
                        title="Редактировать"
                        onClick={() => onEdit(row)}
                    >
                        ✎
                    </button>
                    <button 
                        className="sub-icon-btn delete" 
                        title="Удалить"
                        onClick={() => onDelete(row.id)}
                    >
                        ✕
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default SubTable;
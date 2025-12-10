import InfiniteScroll from "react-infinite-scroll-component";

export const GenericTable = ({
  headText,
  bodyText,
  columns,
  onEditClick,
  nextHandler,
  hasMore,
  onRowClick
}) => {
  return (
    <InfiniteScroll
      dataLength={bodyText.length}
      next={nextHandler}
      hasMore={hasMore}
      scrollableTarget="container"
    >
      <div id="container" className="table-container">
        <table className="admin-table">
          <thead className="thead">
            <tr>
              <th className="start-th-button">Действия</th>
              {headText.map((item, index) => (
                <th key={index} className="column-names">
                  <div className="column-elements">
                    <span className="names">{item}</span>
                    {/* Кнопка сортировки 
                    <button className="button-sort">
                       ⇅
                    </button>
                    */}
                  </div>
                </th>
              ))}
            </tr>
          </thead>
          <tbody>
            {bodyText.map((row, rowIndex) => (
              <tr
                key={row.id || rowIndex}
                className={`table-row ${rowIndex % 2 === 0 ? "even" : "odd"}`}
                onClick={() => onRowClick && onRowClick(row)}
                style={{ cursor: "pointer" }}
              >
                <td>
                  <button
                    className="edit-btn"
                    onClick={(e) => {
                        e.stopPropagation(); 
                        onEditClick(row);
                    }}
                  >
                    ✎
                  </button>
                </td>
                {columns.map((columnKey, colIndex) => {
                  let cellValue = row[columnKey];

                  if (cellValue === null || cellValue === undefined) {
                    cellValue = "-";
                  } 
                  else if (
                    columnKey.toLowerCase().includes("time") || 
                    columnKey.toLowerCase().includes("date")
                  ) {
                    let dateString = cellValue.toString();
                    
                    if (!dateString.endsWith("Z") && !dateString.includes("+")) {
                        dateString += "Z";
                    }

                    cellValue = new Date(dateString).toLocaleString("ru-RU", {
                      year: 'numeric',
                      month: '2-digit',
                      day: '2-digit',
                      hour: '2-digit',
                      minute: '2-digit'
                    });
                  } 
                  else {
                    cellValue = cellValue.toString();
                  }

                  return (
                    <td key={colIndex} className="td-sum">
                      {cellValue}
                    </td>
                  );
                })}
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </InfiniteScroll>
  );
};

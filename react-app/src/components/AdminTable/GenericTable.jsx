import InfiniteScroll from "react-infinite-scroll-component";

export const GenericTable = ({
  headText,
  bodyText,
  columns,
  onEditClick,
  nextHandler,
  hasMore,
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
              >
                <td>
                  <button className="edit-btn" onClick={() => onEditClick(row)}>
                    ✎
                  </button>
                </td>
                {columns.map((columnKey, colIndex) => (
                  <td key={colIndex} className="td-sum">
                    {row[columnKey] !== undefined && row[columnKey] !== null
                      ? row[columnKey].toString()
                      : "-"}
                  </td>
                ))}
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </InfiniteScroll>
  );
};

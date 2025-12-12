import "./SimpleTable.css";

const SimpleTable = ({ data, columns, headers }) => {
  if (!data || data.length === 0)
    return <p className="simple-table-p">Нет данных</p>;
  return (
    <table className="inner-table">
      <thead>
        <tr>
          {headers.map((h, i) => ( <th key={i}>{h}</th> ))}
        </tr>
      </thead>
      <tbody>
        {data.map((row, i) => (
          <tr key={i}>
            {columns.map((col, j) => (
              <td key={j}>
                {col.includes("Date") || col === "date"
                  ? new Date(row[col]).toLocaleDateString()
                  : row[col]}
              </td>
            ))}
          </tr>
        ))}
      </tbody>
    </table>
  );
};

export default SimpleTable;
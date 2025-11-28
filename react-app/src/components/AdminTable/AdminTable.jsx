import "./AdminTable.css"

function AdminTable() {
    const rows = Array.from({ length: 50 }, () => ({
    col1: "Текст в строке",
    col2: "Текст в строке",
    col3: "Текст в строке",
    col4: "Текст в строке",
    col5: "Текст в строке"
  }));
    return (
        <table className="admin-table">
            <thead>
              <tr>
                <th></th>
                <th>Название столбца</th>
                <th>Название столбца</th>
                <th>Название столбца</th>
                <th>Название столбца</th>
                <th>Название столбца</th>
              </tr>
            </thead>

            <tbody>
              {rows.map((r, i) => (
                <tr key={i}>
                  <td className="edit" />
                  <td>{r.col1}</td>
                  <td>{r.col2}</td>
                  <td>{r.col3}</td>
                  <td>{r.col4}</td>
                  <td>{r.col5}</td>
                </tr>
              ))}
            </tbody>
          </table>
    )
}

export default AdminTable;
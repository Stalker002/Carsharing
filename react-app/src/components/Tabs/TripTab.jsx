function TripTab() {
    const trips = [
    { id: 1, user: "Михаил", car: "BMW i8", date: "12.12.2024", status: "Завершена" },
    { id: 2, user: "Анна", car: "Tesla Model 3", date: "18.12.2024", status: "Активна" }
  ];
    return (
        <table className="admin-table">
      <thead>
        <tr>
          <th>ID</th>
          <th>Пользователь</th>
          <th>Машина</th>
          <th>Дата</th>
          <th>Статус</th>
        </tr>
      </thead>
      <tbody>
        {trips.map(t => (
          <tr key={t.id}>
            <td>{t.id}</td>
            <td>{t.user}</td>
            <td>{t.car}</td>
            <td>{t.date}</td>
            <td>{t.status}</td>
          </tr>
        ))}
      </tbody>
    </table>
    )
}

export default TripTab;
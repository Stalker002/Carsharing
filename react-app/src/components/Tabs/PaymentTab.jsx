function PaymentsTab(params) {
    const payments = [
    { id: 1, user: "Михаил", amount: "120 BYN", date: "01.12.2024" },
    { id: 2, user: "Анна", amount: "200 BYN", date: "02.12.2024" }
  ];
    return (
        <table className="admin-table">
      <thead>
        <tr>
          <th>ID</th>
          <th>Пользователь</th>
          <th>Сумма</th>
          <th>Дата</th>
        </tr>
      </thead>
      <tbody>
        {payments.map(p => (
          <tr key={p.id}>
            <td>{p.id}</td>
            <td>{p.user}</td>
            <td>{p.amount}</td>
            <td>{p.date}</td>
          </tr>
        ))}
      </tbody>
    </table>
    )
}

export default PaymentsTab;
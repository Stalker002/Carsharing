function CarsTab(params) {
    const cars = [
    { id: 1, name: "BMW i8", price: "200 BYN" },
    { id: 2, name: "Audi RS6", price: "300 BYN" }
  ];
    return (
        <table className="admin-table">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Название</th>
                    <th>Цена / день</th>
                </tr>
            </thead>
            <tbody>
                {cars.map(c => (
                    <tr key={c.id}>
                        <td>{c.id}</td>
                        <td>{c.name}</td>
                        <td>{c.price}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    )
}

export default CarsTab
function UsersTab(params) {
    const users = [
        { id: 1, name: "Михаил", email: "mail@example.com" },
        { id: 2, name: "Анна", email: "anna@test.com" }
    ];
    return (
        <table className="admin-table">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Имя</th>
                    <th>Email</th>
                </tr>
            </thead>
            <tbody>
                {users.map(u => (
                    <tr key={u.id}>
                        <td>{u.id}</td>
                        <td>{u.name}</td>
                        <td>{u.email}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    )
}

export default UsersTab
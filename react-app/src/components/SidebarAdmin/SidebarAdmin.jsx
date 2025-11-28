import "./SidebarAdmin.css"

function SidebarAdmin() {
  return (
    <div className="admin-sidebar">
      <nav className="sidebar-menu">
        <a>Dashboard</a>
        <a className="active">Машины</a>
        <a>Пользователи</a>
        <a>Клиенты</a>
        <a>Бронирования</a>
        <a>Поездки</a>
        <a>Счета</a>
        <a>Промокоды</a>
        <a>Отзывы</a>
        <a>Страховки</a>
        <a>Штрафы</a>
        <a>Помощь</a>
        <a className="logout">Выйти</a>
      </nav>
    </div>
  );
}

export default SidebarAdmin
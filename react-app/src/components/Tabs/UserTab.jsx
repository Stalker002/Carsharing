import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import InfiniteScroll from "react-infinite-scroll-component";
import { deleteUser, getUsers, updateUser } from "../../redux/actions/users";
import UpdateModal from "../UpdateModal/UpdateModal";

function UsersTab() {
  const dispatch = useDispatch();
  const [page, setPage] = useState(1);
  const [editingUser, setEditingUser] = useState(null);

  const users = useSelector((state) => state.users?.users || []);

  const totalUsers = useSelector((state) => state.users?.usersTotal || 0);

  const isLoading = useSelector((state) => state.users?.isUsersLoading);

  const hasMore = users.length < totalUsers;

  useEffect(() => {
    if (page === 1) {
      dispatch(getUsers(page));
      console.log(`Загрузка пользователей: страница ${page}`);
    }
  }, [page, dispatch]);

  const nextHandler = () => {
    if (!isLoading) {
      setPage((prev) => prev + 1);
      dispatch(getUsers(page + 1));
    }
  };

  const handleEditClick = (user) => {
    setEditingUser(user);
  };

  const handleCloseModal = () => {
    setEditingUser(null);
  };

  const handleSave = async (e) => {
    e.preventDefault();
    const formData = new FormData(e.target);
    const rawData = Object.fromEntries(formData.entries());

    const finalData = {
      ...editingUser,
      ...rawData,
      id: Number(editingUser.id),
      roleId: Number(rawData.roleId),
    };

    console.log("Сохраняем данные:", finalData);

    const result = await dispatch(updateUser(finalData.id, finalData));
    if (!result.success) {
      alert(result.message);
      return;
    }

    setPage(1);
    dispatch(getUsers(1));

    handleCloseModal();
  };

  const handleDelete = async () => {
    if (!editingUser) return;
    
    const isConfirmed = window.confirm(`Вы уверены, что хотите удалить пользователя ${editingUser.login}?`);
    
    if (isConfirmed) {
      await dispatch(deleteUser(editingUser.id));
      setEditingUser(null); 
    }
  };

  return (
    <>
      <InfiniteScroll
        dataLength={users.length}
        next={nextHandler}
        hasMore={hasMore}
        scrollableTarget="users-container"
      >
        <div id="users-container" className="table-container">
          <table className="admin-table">
            <thead>
              <tr>
                <th>Действия</th>
                <th>ID</th>
                <th>ID роли</th>
                <th>Логин</th>
                <th>Пароль</th>
              </tr>
            </thead>
            <tbody>
              {users.map((u) => (
                <tr key={`${u.id}-${u.login}`}>
                  <td>
                    <button
                      className="edit-btn"
                      onClick={() => handleEditClick(u)}
                    >
                      ✎
                    </button>
                  </td>
                  <td>{u.id}</td>
                  <td>{u.roleId}</td>
                  <td>{u.login}</td>
                  <td>{u.password}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </InfiniteScroll>
      {editingUser && (
        <UpdateModal
          title={`Редактирование пользователя #${editingUser.id}`}
          onClose={() => setEditingUser(null)}
          onDelete={handleDelete}
          formId="user-edit-form"
        >
          <form id="user-edit-form" onSubmit={handleSave}>
            <div className="modal-form-group">
              <label>Логин:</label>
              <input type="text" name="login" defaultValue={editingUser.login} required />
            </div>
            <div className="modal-form-group">
              <label>Пароль:</label>
              <input type="text" name="password" defaultValue={editingUser.password} required />
            </div>
            <div className="modal-form-group">
              <label>ID Роли:</label>
              <input type="number" name="roleId" defaultValue={editingUser.roleId} required />
            </div>
          </form>
        </UpdateModal>
      )}
    </>
  );
}

export default UsersTab;

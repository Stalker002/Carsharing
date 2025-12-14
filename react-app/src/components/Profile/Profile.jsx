import { useEffect, useState } from "react";
import "./Profile.css";
import { useDispatch, useSelector } from "react-redux";
import { getMyUser } from "../../redux/actions/users";
import { useNavigate } from "react-router-dom";

function Profile() {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const [profile, setProfile] = useState({
    category: "Категория прав ПОМЕНЯТЬ",
  });

  const formatPhoneNumber = (rawNumber) => {
    if (!rawNumber) return "";

    const cleaned = rawNumber.replace(/[^\d+]/g, "");

    const normalized = cleaned.startsWith("+")
      ? "+" + cleaned.slice(1).replace(/\+/g, "")
      : cleaned.replace(/\+/g, "");

    if (!normalized.startsWith("+375") || normalized.length < 13) {
      return rawNumber;
    }

    const countryCode = normalized.substring(0, 4);
    const areaCode = normalized.substring(4, 6);
    const part1 = normalized.substring(6, 9);
    const part2 = normalized.substring(9, 11);
    const part3 = normalized.substring(11, 13);

    return `${countryCode} (${areaCode}) ${part1}-${part2}-${part3}`;
  };

  const myClient = useSelector((state) => state.clients.myClient);
  const myUser = useSelector((state) => state.users.myUser);
  const isMyUserLoading = useSelector(
    (state) => state.users.isMyUserLoading
  );

  useEffect(() => {
    if (Object.keys(myUser).length === 0 && !isMyUserLoading) {
      dispatch(getMyUser());
    }
  }, [isMyUserLoading, dispatch]);

  const userRoleId = myUser.roleId;

  const isSpecialUser = userRoleId === 1;

  return (
    <div className="profile-wrapper">
      <div className="profile-card">
        <h1 className="profile-name">
          {myClient.name} {myClient.surname}
        </h1>
        <div className="profile-category">{profile.category}</div>

        <div className="profile-grid">
          <div className="profile-item">
            <span className="item-label">Номер телефона</span>
            <span className="item-value">
              {formatPhoneNumber(myClient.phoneNumber)}
            </span>
          </div>

          <div className="profile-item">
            <span className="item-label">Логин</span>
            <span className="item-value">{myUser.login}</span>
          </div>

          <div className="profile-item">
            <span className="item-label">Почта</span>
            <span className="item-value">{myClient.email}</span>
          </div>
        </div>

        <div className="profile-actions">
          <button className="profile-edit-btn">Редактировать</button>
          {isSpecialUser && (
            <button
              className="profile-edit-btn"
              onClick={() => {
                navigate("/admin");
              }}
            >
              Админ панель
            </button>
          )}
        </div>
      </div>
    </div>
  );
}

export default Profile;

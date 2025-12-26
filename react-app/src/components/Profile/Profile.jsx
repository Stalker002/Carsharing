import { useEffect, useState } from "react";
import "./Profile.css";
import { useDispatch, useSelector } from "react-redux";
import { getMyUser } from "../../redux/actions/users";
import { getMyDocuments } from "../../redux/actions/clients";
import { openModal } from "../../redux/actions/modal";
import { useNavigate } from "react-router-dom";
import EditProfileModal from "../EditProfileModal/EditProfileModal";
import AddDocumentModal from "../AddDocumentModal/AddDocumentModal";
import { deleteClientDocument } from "../../redux/actions/clientDocuments";

function Profile() {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const [isEditOpen, setIsEditOpen] = useState(false);
  const [isDocOpen, setIsDocOpen] = useState(false);

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
  const isLoggedIn = useSelector((state) => state.users.isLoggedIn);

  const documents = useSelector((state) => state.clients.myDocument);

  useEffect(() => {
    if (isLoggedIn) {
      dispatch(getMyUser());
    }
  }, [isLoggedIn, dispatch, myClient]);

  useEffect(() => {
    if (myClient?.id) {
      dispatch(getMyDocuments(myClient.id));
    }
  }, [dispatch, myClient]);

  const userRoleId = myUser.roleId;
  const isSpecialUser = userRoleId === 1;

  const licenseDoc = documents.find(
    (doc) =>
      doc.type?.toLowerCase().includes("права") ||
      doc.type?.toLowerCase().includes("удостоверение")
  );

  const category = licenseDoc?.licenseCategory;

  const handleDeleteDoc = (id) => {
    dispatch(
      openModal({
        title: "Удаление документа",
        message: "Вы уверены? Без действующих документов аренда невозможна.",
        type: "confirm",
        confirmText: "Удалить",
        onConfirm: async () => {
          const res = await dispatch(deleteClientDocument(id));
          if (res.success) {
            dispatch(getMyDocuments(myClient.id));
          } else {
            dispatch(
              openModal({
                type: "error",
                title: "Ошибка",
                message: res.message,
              })
            );
          }
        },
      })
    );
  };

  return (
    <div className="profile-wrapper">
      <div className="profile-column">
        <div className="profile-card">
          <div className="user-avatar-profile">
            <span>
              {myClient.surname?.[0]}
              {myClient.name?.[0]}
            </span>
          </div>
          <h1 className="profile-name">
            {myClient.surname} {myClient.name}
          </h1>
          <div className="profile-category">
            {category
              ? `Водительские права: Категория ${category}`
              : "Водительские права не загружены"}
          </div>

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
            <button
              className="profile-edit-btn"
              onClick={() => setIsEditOpen(true)}
            >
              Редактировать
            </button>
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
        <div className="docs-section">
          <div className="docs-header">
            <h2>Мои документы</h2>
            <button className="add-doc-btn" onClick={() => setIsDocOpen(true)}>
              + Добавить
            </button>
          </div>

          {documents.length > 0 ? (
            <div className="docs-list">
              {documents.map((doc) => {
                const docImageUrl = doc.filePath
                  ? `http://localhost:5078${doc.filePath}`
                  : null;
                const isPdf = doc.docImageUrl?.toLowerCase().endsWith(".pdf");
                return (
                  <div key={doc.id} className="doc-item">
                    <div className="doc-icon">
                      {docImageUrl ? (
                        isPdf ? (
                          <a
                            href={docImageUrl}
                            target="_blank"
                            rel="noopener noreferrer"
                            style={{
                              textDecoration: "none",
                              display: "flex",
                              flexDirection: "column",
                              alignItems: "center",
                              color: "#e74c3c",
                            }}
                          >
                            <span
                              style={{ fontSize: "24px", fontWeight: "bold" }}
                            >
                              PDF
                            </span>
                            <span style={{ fontSize: "10px" }}>Открыть</span>
                          </a>
                        ) : (
                          <img
                            src={docImageUrl}
                            alt="doc"
                            style={{
                              width: "100%",
                              height: "100%",
                              objectFit: "cover",
                              borderRadius: "8px",
                            }}
                          />
                        )
                      ) : (
                        "📄"
                      )}
                    </div>
                    <div className="doc-info">
                      <h4>{doc.type}</h4>
                      <p>№ {doc.number}</p>

                      <span
                        className={
                          new Date(doc.expiryDate) < new Date()
                            ? "doc-expired"
                            : "doc-valid"
                        }
                      >
                        До: {new Date(doc.expiryDate).toLocaleDateString()}
                      </span>
                    </div>
                    <button
                      className="doc-delete-btn"
                      onClick={() => handleDeleteDoc(doc.id)}
                    >
                      ✕
                    </button>
                  </div>
                );
              })}
            </div>
          ) : (
            <div className="no-docs-placeholder">
              <p>
                Загрузите водительское удостоверение и паспорт для начала
                аренды.
              </p>
            </div>
          )}
        </div>
      </div>
      {isEditOpen && (
        <EditProfileModal
          client={myClient}
          onClose={() => setIsEditOpen(false)}
        />
      )}
      {isDocOpen && (
        <AddDocumentModal
          clientId={myClient.id}
          onClose={() => setIsDocOpen(false)}
        />
      )}
    </div>
  );
}

export default Profile;

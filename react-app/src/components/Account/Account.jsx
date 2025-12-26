import { useDispatch, useSelector } from "react-redux";
import { useEffect, useState } from "react";
import Register from "../Register/Register";
import Login from "../Login/Login";
import Exit from "./../../svg/Profile/whiteExit.png";

import "./Account.css";
import { useNavigate } from "react-router-dom";
import { getMyClient } from "../../redux/actions/clients";

function Account() {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const myClient = useSelector((state) => state.clients.myClient);
  const isLoggedIn = useSelector((state) => state.users.isLoggedIn);
  const isClientLoading = useSelector(
    (state) => state.clients.isClientsLoading
  );
  const clientError = useSelector((state) => state.clients.error);

  useEffect(() => {
    if (isLoggedIn && (!myClient || Object.keys(myClient).length === 0)) {
      if (!isClientLoading && !clientError) {
        dispatch(getMyClient());
      }
    }
  }, [isLoggedIn, myClient, isClientLoading, clientError, dispatch]);

  const [isLoginOpen, setIsLoginOpen] = useState(false);
  const [isRegisterOpen, setIsRegisterOpen] = useState(false);

  const openLogin = () => {
    setIsRegisterOpen(false);
    setIsLoginOpen(true);
  };
  const openRegister = () => {
    setIsLoginOpen(false);
    setIsRegisterOpen(true);
  };
  const closeAll = () => {
    setIsLoginOpen(false);
    setIsRegisterOpen(false);
  };

  if (!isLoggedIn) {
    return (
      <>
        <p className="client-name" onClick={openLogin}>
          Войдите или зарегестрируйтесь
        </p>
        <button className="user-button" onClick={openLogin}>
          <div className="user-avatar">
            <img src={Exit} alt="Войти" />
          </div>
        </button>
        <Login
          isOpen={isLoginOpen}
          onClose={closeAll}
          onRegisterClick={openRegister}
        />
        <Register
          isOpen={isRegisterOpen}
          onClose={closeAll}
          onLoginClick={openLogin}
        />
      </>
    );
  }

  if (!myClient || Object.keys(myClient).length === 0) {
    return (
      <button className="user-button">
        <div className="user-avatar">...</div>
      </button>
    );
  }

  return (
    <>
      <p className="client-name" onClick={() => navigate("/personal-page")}>
        {myClient.surname} {myClient.name}
      </p>
      <button
        className="user-button"
        onClick={() => navigate("/personal-page")}
      >
        <div className="user-avatar">
          <span>
            {myClient.surname?.[0]}
            {myClient.name?.[0]}
          </span>
        </div>
      </button>
    </>
  );
}

export default Account;

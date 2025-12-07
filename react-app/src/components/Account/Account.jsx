import { useDispatch, useSelector } from "react-redux";
import { getMyClient } from "../../redux/actions/clients";
import { useEffect, useState } from "react";
import Register from "../Register/Register";
import Login from "../Login/Login";
import Exit from "./../../svg/Profile/whiteExit.png";

import "./Account.css";
import { useNavigate } from "react-router-dom";

function Account() {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const myClient = useSelector((state) => state.clients.myClient);
  const isLoggedIn = useSelector((state) => state.users.isLoggedIn);
  const isMyClientLoading = useSelector(
    (state) => state.clients.isClientsLoading
  );

  useEffect(() => {
    if (!isLoggedIn) {
        dispatch(getMyClient());
    }
  }, [isLoggedIn, dispatch]);
  console.log("myClient =", myClient);

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

  if (isLoggedIn) {
    return (
      <>
        <button className="user-button" onClick={openLogin}>
          <div className="user-avatar">
            <img src={Exit} />
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

  return (
    <>
      <p
        className="client-name"
        onClick={() => {
          navigate("/personal-page");
        }}
      >
        {myClient.name} {myClient.surname}
      </p>
      <button
        className="user-button"
        onClick={() => {
          navigate("/personal-page");
        }}
      >
        <div className="user-avatar">
          <span>
            {myClient.name?.[0]}
            {myClient.surname?.[0]}
          </span>
        </div>
      </button>
    </>
  );
}

export default Account;

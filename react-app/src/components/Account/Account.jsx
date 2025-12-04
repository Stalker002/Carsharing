import { useDispatch, useSelector } from "react-redux";
import { getMyClient } from "../../redux/actions/clients";
import { useEffect, useState } from "react";
import Register from "../Register/Register";
import Login from "../Login/Login";

import "./Account.css";

function Account() {
  const dispatch = useDispatch();

  const myClient = useSelector((state) => state.clients.myClient);
  const isLoggedIn = useSelector((state) => state.users.isLoggedIn);

  useEffect(() => {
    if (isLoggedIn && Object.keys(myClient).length === 0) {
      dispatch(getMyClient());
    }
  }, [isLoggedIn, myClient, dispatch]);
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

  if (!isLoggedIn) {
    return (
      <>
        <button className="user-button" onClick={openLogin}>
          <div className="user-avatar">
            <span>U</span>
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
      <p className="client-name">
        {myClient[0]?.name} {myClient[0]?.surname}
      </p>
      <button className="user-button">
        <div className="user-avatar">
          <span>
            {myClient[0]?.name?.[0]}
            {myClient[0]?.surname?.[0]}
          </span>
        </div>
      </button>
    </>
  );
}

export default Account;

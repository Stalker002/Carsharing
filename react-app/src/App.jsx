import "./App.css";
import { useEffect, useState } from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import { useSelector, useDispatch } from "react-redux";

import Home from "./pages/Home/Home";
import Car_Rent from "./pages/Car_Rent/Car_Rent";
import NotFoundPage from "./pages/NotFoundPage/NotFoundPage";
import Admin from "./pages/Admin/Admin";
import CarDetails from "./pages/CarDetails/CarDetails";
import PersonalPage from "./pages/PersonalPage/PersonalPage";
import PaymentPage from "./pages/PaymentPage/PaymentPage";
import BookingPage from "./pages/BookingPage/BookingPage";
import GlobalModal from "./components/GlobalModal/GlobalModal";

import { getMyUser } from "./redux/actions/users";
import Profile from "./components/Profile/Profile";
import CurrentTrip from "./components/CurrentTrip/CurrentTrip";
import BookingHistory from "./components/TripHistory/BookingHistory";

function AdminRoute({ children }) {
  const { myUser } = useSelector((state) => state.users);
  if (!myUser || myUser.roleId !== 1) {
    return <Navigate to="/" replace />;
  }

  return children;
}
function PrivateRoute({ children }) {
  const { myUser } = useSelector((state) => state.users);
  if (!myUser) {
    return <Navigate to="/" replace />;
  }

  return children;
}

function App() {
  const dispatch = useDispatch();
  const [isAppReady, setIsAppReady] = useState(false);

  useEffect(() => {
    dispatch(getMyUser()).finally(() => {
      setIsAppReady(true);
    });
  }, [dispatch]);

  if (!isAppReady) {
    return (
      <div
        style={{
          height: "100vh",
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          fontSize: "20px",
          color: "#555",
        }}
      >
        <div className="spinner"></div> Загрузка приложения...
      </div>
    );
  }

  return (
    <>
      <GlobalModal />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/car-catalog" element={<Car_Rent />} />
        <Route path="/car-catalog/:id" element={<CarDetails />} />
        <Route
          path="/personal-page/*"
          element={
            <PrivateRoute>
              <PersonalPage />
            </PrivateRoute>
          }
        >
          <Route index element={<Profile />} />
          <Route path="current-trip" element={<CurrentTrip />} />
          <Route path="history" element={<BookingHistory />} />
        </Route>
        <Route
          path="/booking/:id"
          element={
            <PrivateRoute>
              <BookingPage />
            </PrivateRoute>
          }
        />
        <Route path="/payment/:id" element={<PaymentPage />} />
        <Route
          path="/admin"
          element={
            <AdminRoute>
              <Admin />
            </AdminRoute>
          }
        />
        <Route path="*" element={<NotFoundPage />} />
      </Routes>
    </>
  );
}

export default App;

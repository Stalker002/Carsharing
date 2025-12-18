import "./App.css";
import { Routes, Route, Link } from "react-router-dom";

import Home from "./pages/Home/Home";
import Car_Rent from "./pages/Car_Rent/Car_Rent";
import NotFoundPage from "./pages/NotFoundPage/NotFoundPage";
import Admin from "./pages/Admin/Admin";
import CarDetails from "./pages/CarDetails/CarDetails";
import { Provider, useSelector } from "react-redux";
import { store } from "./redux/store";
import PersonalPage from "./pages/PersonalPage/PersonalPage";

import { Navigate } from "react-router-dom";
import PaymentPage from "./pages/PaymentPage/PaymentPage";
import BookingPage from "./pages/BookingPage/BookingPage";
import GlobalModal from "./components/GlobalModal/GlobalModal";

function AdminRoute({ children }) {
  const myUser = useSelector((state) => state.users.myUser);
  if (!myUser || myUser.roleId !== 1) {
    return <Navigate to="/" replace />;
  }
  return children;
}

function App() {
  return (
    <Provider store={store}>
      <GlobalModal />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/car-catalog" element={<Car_Rent />} />
        <Route path="/car-catalog/:id" element={<CarDetails />} />
        <Route path="/personal-page" element={<PersonalPage />} />
        <Route path="/booking/:id" element={<BookingPage />} />
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
    </Provider>
  );
}

export default App;

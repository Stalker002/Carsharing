import { useState } from 'react'
import './App.css';
import { Routes, Route, Link } from 'react-router-dom';

import Home from "./pages/Home"
import Car_Rent from './pages/Car_Rent'
import NotFoundPage from './pages/NotFoundPage'
import Header from './components/Header/Header';
import Footer from './components/Footer/Footer';
import Login from './components/Login/Login';
import Register from './components/Register/Register';

function App() {
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
  return (
    <>
    <Header onLoginClick={openLogin}/>
      <Routes>
        <Route path='/' element={<Home />} />
        <Route path='/car-catalog' element={<Car_Rent />} />
        <Route path='*' element={<NotFoundPage />} />
      </Routes>
    <Footer />
    <Login isOpen={isLoginOpen} onClose={closeAll} onRegisterClick={openRegister} />
    <Register isOpen={isRegisterOpen} onClose={closeAll} onLoginClick={openLogin} />
    </>
  )
}

export default App

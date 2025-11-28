import './App.css';
import { Routes, Route, Link } from 'react-router-dom';

import Home from "./pages/Home/Home"
import Car_Rent from './pages/Car_Rent/Car_Rent'
import NotFoundPage from './pages/NotFoundPage/NotFoundPage'
import Car_Details from './components/Car_Details/Car_Details';
import Admin from './pages/Admin/Admin';

function App() {
  return (
    <>
      <Routes>
        <Route path='/' element={<Home />} />
        <Route path='/car-catalog' element={<Car_Rent />} />
        <Route path="/car-catalog/:id" element={<Car_Details />} />
        <Route path="/admin" element={<Admin />} />
        <Route path='*' element={<NotFoundPage />} />
      </Routes>
    </>
  )
}

export default App

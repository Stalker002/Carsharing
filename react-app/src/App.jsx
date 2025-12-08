import './App.css';
import { Routes, Route, Link } from 'react-router-dom';

import Home from "./pages/Home/Home"
import Car_Rent from './pages/Car_Rent/Car_Rent'
import NotFoundPage from './pages/NotFoundPage/NotFoundPage'
import Admin from './pages/Admin/Admin';
import CarDetails from './pages/CarDetails/CarDetails';
import { Provider } from 'react-redux';
import { store } from './redux/store';
import PersonalPage from './pages/PersonalPage/PersonalPage';

function App() {
  return (
    <Provider store={store}>
      <Routes>
        <Route path='/' element={<Home />} />
        <Route path='/car-catalog' element={<Car_Rent />} />
        <Route path="/car-catalog/:id" element={<CarDetails />} />
        <Route path="/personal-page" element={<PersonalPage />} />
        <Route path="/admin" element={<Admin />} />
        <Route path='*' element={<NotFoundPage />} />
      </Routes>
    </Provider>
  )
}

export default App

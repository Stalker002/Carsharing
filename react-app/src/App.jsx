import { useState } from 'react'
import './App.css';
import { Routes, Route, Link } from 'react-router-dom';

import Home from "./pages/Home"
import Car_Rent from './pages/Car_Rent'

function App() {
  const [count, setCount] = useState(0)

  return (
    <>
      <Routes>
        <Route path='/' element={<Home />} />
        <Route path='/car-rent' element={<Car_Rent />} />
      </Routes>
    </>
  )
}

export default App

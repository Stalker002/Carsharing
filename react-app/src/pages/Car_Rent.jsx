import MainHeader from '../components/Header/MainHeader';
import Footer from './../components/Footer/Footer';
import Car_Catalog from './../components/Car_Catalog/Car_Catalog';
import SidebarFilters from './../components/SidebarFilters/SidebarFilters';
import './Car_Rent.css'

import { useState } from 'react'

function Home() {
  const [count, setCount] = useState(0);
  return (

    <div>
      <MainHeader />
      <div className="car-catalog">
        <div className="catalog-layout">
          <SidebarFilters />
          <div className="catalog-content">
            <Car_Catalog />
          </div>
        </div>
      </div>
      <Footer />
    </div>
  )
}

export default Home
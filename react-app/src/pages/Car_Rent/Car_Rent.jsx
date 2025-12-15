import Car_Catalog from '../../components/Car_Catalog/Car_Catalog';
import SidebarFilters from '../../components/SidebarFilters/SidebarFilters';
import Header from './../../components/Header/Header';
import Footer from './../../components/Footer/Footer';

import './Car_Rent.css'
import { useState } from 'react';

function Car_Rent() {
  const [filters, setFilters] = useState({
    types: [],
    capacities: [],
    maxPrice: 500
  });
  return (
    <>
      <Header />
      <div className="car-catalog">
        <SidebarFilters filters={filters} setFilters={setFilters} />
        <div className="catalog-content">
          <Car_Catalog filters={filters}/>
        </div>
      </div>
      <Footer />
    </>
  )
}

export default Car_Rent
import Car_Catalog from '../../components/Car_Catalog/Car_Catalog';
import Select_Rent from '../../components/Select_Rent/Select_Rent';
import SidebarFilters from '../../components/SidebarFilters/SidebarFilters';
import Header from './../../components/Header/Header';
import Footer from './../../components/Footer/Footer';

import './Car_Rent.css'

function Car_Rent() {
  return (
    <>
      <Header />
      <div className="car-catalog">
        <SidebarFilters />
        <div className="catalog-content">
          <Select_Rent />
          <Car_Catalog />
        </div>
      </div>
      <Footer />
    </>
  )
}

export default Car_Rent
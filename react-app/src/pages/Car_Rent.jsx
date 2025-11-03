import Car_Catalog from './../components/Car_Catalog/Car_Catalog';
import Select_Rent from './../components/Select_Rent/Select_Rent';
import SidebarFilters from './../components/SidebarFilters/SidebarFilters';
import './Car_Rent.css'

function Home() {
  return (
    <div className="car-catalog">
        <SidebarFilters />
        <div className="catalog-content">
          <Select_Rent />
          <Car_Catalog />
        </div>
    </div>
  )
}

export default Home
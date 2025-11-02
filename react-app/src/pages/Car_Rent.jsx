import Car_Catalog from './../components/Car_Catalog/Car_Catalog';
import SidebarFilters from './../components/SidebarFilters/SidebarFilters';
import './Car_Rent.css'

function Home() {
  return (
    <div className="car-catalog">
        <SidebarFilters />
        <div className="catalog-content">
          <Car_Catalog />
        </div>
    </div>
  )
}

export default Home
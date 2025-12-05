import { useState } from "react";
import CarsTab from "../Tabs/CarsTab";
import Dashboard from "../Tabs/Dashboard";
import PaymentsTab from "../Tabs/PaymentTab";
import TripTab from "../Tabs/TripTab";
import UsersTab from "../Tabs/UserTab";
import "./AdminTable.css"
import AddModel from "../AddModel/AddModel";

function AdminTable({ activeTab }) {
  const [isAddOpen, setIsAddOpen] = useState(false);
  const [page, setPage] = useState(1);
  
  const handleAdd = (data) => {
    console.log("Добавлено:", data);
    // тут добавляешь в таблицу
  };

  return (
    <>
      <div className="admin-body">
        <div className="table-top">
        <input className="search" placeholder="Поиск по таблице" />

        {/* <button
          className="filter-btn"
          onClick={() => setIsFilterOpen(!isFilterOpen)}
        >
          <img src={FilterIcon} alt="filter" />
        </button> */}
        <button className="add-button" onClick={() => setIsAddOpen(true)}>
          + Добавить работу
        </button>
      </div> 
        {activeTab === "Dashboard" && <Dashboard />}
        {activeTab === "trips" && <TripTab />}
        {activeTab === "cars" && <CarsTab />}
        {activeTab === "users" && <UsersTab />}
        {activeTab === "payments" && <PaymentsTab />}
      </div>
      <AddModel
        isOpen={isAddOpen}
        onClose={() => setIsAddOpen(false)}
        activeTable={activeTab}
        setPage={setPage}
        onAdd={handleAdd}
      />
    </>
  )
}

export default AdminTable;
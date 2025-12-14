import SidebarAdmin from "../../components/SidebarAdmin/SidebarAdmin"
import "./Admin.css"
import AdminTable from "../../components/AdminTable/AdminTable"
import Header from './../../components/Header/Header';
import { useState } from "react";

function Admin() {
    const [activeTab, setActiveTab] = useState("Dashboard");
    return (
        <>
            <Header />
            <div className="admin-container">
                <SidebarAdmin activeTab={activeTab} setActiveTab={setActiveTab}/>
                <AdminTable activeTab={activeTab}/>
            </div>
        </>
    )
}

export default Admin
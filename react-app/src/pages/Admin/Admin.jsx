import { Router } from "react-router-dom"
import SidebarAdmin from "../../components/SidebarAdmin/SidebarAdmin"
import "./Admin.css"
import AdminTable from "../../components/AdminTable/AdminTable"
import Header from './../../components/Header/Header';
import Footer from './../../components/Footer/Footer';

function Admin() {
    return (
        <>
            <Header />
            <div className="admin-container">
                <SidebarAdmin />
                <AdminTable />
            </div>
            <Footer />
        </>
    )
}

export default Admin
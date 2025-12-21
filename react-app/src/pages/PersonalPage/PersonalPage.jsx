import { Outlet } from "react-router-dom";
import Header from "../../components/Header/Header";
import SideBarPersonal from "../../components/SideBarPersonal/SideBarPersonal";

import "./PersonalPage.css";

function PersonalPage() {
  return (
    <div className="page-layout">
      <Header />
      <div className="page-content">
        <SideBarPersonal />
        <Outlet /> 
      </div>
    </div>
  );
}
export default PersonalPage;

import { useState } from "react";
import Header from "../../components/Header/Header";
import SideBarPersonal from "../../components/SideBarPersonal/SideBarPersonal";

import "./PersonalPage.css";
import Profile from "../../components/Profile/Profile";
import CurrentTripTab from "../../components/CurrentTrip/CurrentTripTab";

function PersonalPage() {
  const [activePage, setActivePage] = useState("profile");

  const renderContent = () => {
    switch (activePage) {
      case "profile":
        return <Profile />;
      case "trip":
        return <CurrentTripTab />;
      //   case "history":
      //     return <History />;
      //   case "help":
      //     return <Help />;
      default:
        return <Profile />;
    }
  };
  return (
    <div className="page-layout">
      <Header />
      <div className="page-content">
        <SideBarPersonal active={activePage} onChange={setActivePage} />
        {renderContent()}
      </div>
    </div>
  );
}

export default PersonalPage;

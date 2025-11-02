import MainHeader from '../components/Header/Header';
import Footer from './../components/Footer/Footer';
import './NotFoundPage.css'

function NotFoundPage() {

  return (
    <>
      <MainHeader />
      <div className="not-found-page">
        <h1>404 Not Found Page</h1>
      </div>
      <Footer />
    </>
  )
}

export default NotFoundPage
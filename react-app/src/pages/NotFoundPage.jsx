import MainHeader from '../components/Header/Header';
import Footer from './../components/Footer/Footer';
import './NotFoundPage.css'

import { useState } from 'react'

function NotFoundPage() {
  const [count, setCount] = useState(0);

  return (
    <div>
      <MainHeader />
      <div className="not-found-page">
        <h1>404 Not Found Page</h1>
      </div>
      <Footer />
    </div>
  )
}

export default NotFoundPage
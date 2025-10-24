import MainHeader from '../components/Header/MainHeader';
import Hero from './../components/Hero/Hero';
import Features from './../components/Features/Features';
import Steps from './../components/Steps/Steps';
import PopularCar from './../components/Popular_car/PopularCar';
import FactsInNumbers from './../components/FactsInNumbers/FactsInNumbers'
import Footer from './../components/Footer/Footer';

import { useState } from 'react'

function Home() {
const [count, setCount] = useState(0);
  return (
    
    <div>
            <MainHeader />
            <Hero />
            <Features />
            <Steps />
            <PopularCar />
            <FactsInNumbers />
            <Footer />
    </div>
  )
}

export default Home
import Hero from '../../components/Hero/Hero';
import Features from '../../components/Features/Features';
import Steps from '../../components/Steps/Steps';
import PopularCar from '../../components/Popular_car/PopularCar';
import FactsInNumbers from '../../components/FactsInNumbers/FactsInNumbers';
import Header from './../../components/Header/Header';
import Footer from './../../components/Footer/Footer';

function Home() {
  return (
    <>
      <Header />
      <Hero />
      <Features />
      <Steps />
      <PopularCar />
      <FactsInNumbers />
      <Footer />
    </>
  )
}

export default Home
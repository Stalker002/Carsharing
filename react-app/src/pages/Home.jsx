import Hero from './../components/Hero/Hero';
import Features from './../components/Features/Features';
import Steps from './../components/Steps/Steps';
import PopularCar from './../components/Popular_car/PopularCar';
import FactsInNumbers from './../components/FactsInNumbers/FactsInNumbers'

function Home() {
  return (
    <div>
      <Hero />
      <Features />
      <Steps />
      <PopularCar />
      <FactsInNumbers />
    </div>
  )
}

export default Home
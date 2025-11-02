import './Features.css';
import Location from '../../svg/Features/Location.svg'
import Comfort from '../../svg/Features/Comfort.svg'
import Savings from '../../svg/Features/Savings.svg'

function Features() {
  const features = [
    {
      icon: Location,
      title: "Доступность",
      description:
        "Широкий выбор автомобилей всегда готов к вашим поездкам. Бронируйте в любое время и в любом месте.",
    },
    {
      icon: Comfort,
      title: "Комфорт",
      description:
        "Премиальные автомобили с современными удобствами для максимально комфортного путешествия.",
    },
    {
      icon: Savings,
      title: "Экономия",
      description:
        "Конкурентные цены и специальные предложения помогут вам сэкономить на аренде без потери качества.",
    },
  ];

  return (
    <section className="features">
      <h2 className="features-header">Почему нас выбирают</h2>
      <div className="features-grid">
        {features.map(({ icon, title, description }) => (
          <div className="feature-card" key={title}>
            <img
              src={icon}
              alt={title}
              className="feature-icon"
              width={48}
              height={48}
              loading="lazy"
            />
            <h3 className="feature-title">{title}</h3>
            <p className="feature-description">{description}</p>
          </div>
        ))}
      </div>
    </section>
  );
}

export default Features;
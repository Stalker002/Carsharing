import { useState } from "react";
import "./FAQ.css";

const faqData = [
  {
    question: "Как это работает?",
    answer:
      "Выберите автомобиль в каталоге, укажите даты аренды и нажмите «Забронировать». После подтверждения бронирования вы сможете начать поездку через личный кабинет. По окончании аренды просто завершите поездку в приложении, указав место парковки.",
  },
  {
    question: "Могу ли я арендовать авто без кредитной карты?",
    answer:
      "Да, мы принимаем дебетовые карты большинства банков. Также возможна оплата через ЕРИП или наличными в офисе при подписании договора для долгосрочной аренды.",
  },
  {
    question: "Можно ли доверять техническому состоянию ваших автомобилей?",
    answer:
      "Безусловно. Мы уделяем первостепенное внимание безопасности. Весь наш автопарк проходит регулярный технический осмотр и обслуживание в сертифицированных центрах нашего официального партнера — «AutoService». Это гарантирует исправность и надежность каждого автомобиля перед передачей клиенту.",
  },
  {
    question: "Какие требования для аренды автомобиля?",
    answer:
      "Вам должно быть не менее 19 лет (для премиум-класса — 23 года). Стаж вождения должен составлять минимум 1 год. Также необходимо наличие действительного водительского удостоверения категории B и паспорта.",
  },
  {
    question: "Могу ли я выезжать за пределы города/страны?",
    answer:
      "Выезд за пределы города Минска разрешен без ограничений по территории Республики Беларусь. Выезд за границу требует дополнительного согласования с менеджером и оформления расширенной страховки.",
  },
  {
    question: "Включена ли страховка в стоимость аренды?",
    answer:
      "Базовая страховка (ОСАГО) включена в стоимость каждого тарифа. Вы также можете приобрести расширенную страховку (КАСКО) за дополнительные 10% от стоимости заказа при бронировании, чтобы полностью исключить ответственность при ДТП.",
  },
];

const FAQ = () => {
  const [activeIndex, setActiveIndex] = useState(0);

  const toggleIndex = (index) => {
    setActiveIndex(activeIndex === index ? -1 : index);
  };

  return (
    <section className="faq-section">
      <div className="faq-container">
        <h2 className="faq-title">Часто задаваемые вопросы</h2>

        <div className="faq-list">
          {faqData.map((item, index) => (
            <div
              key={index}
              className={`faq-item ${activeIndex === index ? "active" : ""}`}
              onClick={() => toggleIndex(index)}
            >
              <div className="faq-header">
                <h4 className="faq-question">{item.question}</h4>
                <div className="faq-icon">
                  <svg
                    width="24"
                    height="24"
                    viewBox="0 0 24 24"
                    fill="none"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      d="M6 9L12 15L18 9"
                      stroke="currentColor"
                      strokeWidth="2"
                      strokeLinecap="round"
                      strokeLinejoin="round"
                    />
                  </svg>
                </div>
              </div>

              <div
                className="faq-body"
                style={{
                  maxHeight: activeIndex === index ? "200px" : "0px",
                  opacity: activeIndex === index ? 1 : 0,
                }}
              >
                <p className="faq-answer">{item.answer}</p>
              </div>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
};

export default FAQ;

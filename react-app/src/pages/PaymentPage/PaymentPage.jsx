import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";

import { applyPromocode, getInfoBill } from "../../redux/actions/bills";
import { createPayment } from "../../redux/actions/payments";
import { openModal } from "../../redux/actions/modal";

import Header from "../../components/Header/Header";
import Visa from "../../svg/Payment/visa.svg";
import Security from "../../svg/Payment/security.svg";
import "./PaymentPage.css";

const PaymentPage = () => {
  const { id } = useParams();
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const billFromStore = useSelector((state) => {
    if (state.bills.infoBill && state.bills.infoBill.id === Number(id)) {
      return state.bills.infoBill;
    }
    return state.bills.myBills.find((b) => b.id === Number(id));
  });

  const [isFetching, setIsFetching] = useState(!billFromStore);

  const [promoCode, setPromoCode] = useState("");
  const [isApplyingPromo, setIsApplyingPromo] = useState(false);
  const [cardData, setCardData] = useState({
    number: "",
    holder: "",
    expiry: "",
    cvc: "",
  });

  useEffect(() => {
    const loadData = async () => {
      setIsFetching(true);
      const result = await dispatch(getInfoBill(id));

      if (!result.success) {
        dispatch(
          openModal({
            type: "error",
            title: "Ошибка",
            message: "Счет не найден",
          })
        );
        navigate("/personal-page/history");
      }
      setIsFetching(false);
    };

    loadData();
  }, [id, dispatch, navigate]);

  const bill = billFromStore;

  const handleCardChange = (e) => {
    setCardData({ ...cardData, [e.target.name]: e.target.value });
  };

  const handleApplyPromo = async () => {
    if (!promoCode.trim()) return;

    setIsApplyingPromo(true);

    const result = await dispatch(applyPromocode(id, promoCode));

    if (result.success) {
      await dispatch(getInfoBill(id));
      setPromoCode("");
    }
    setIsApplyingPromo(false);
  };

  const handlePay = async () => {
    if (
      !cardData.number ||
      !cardData.cvc ||
      !cardData.expiry ||
      !cardData.holder
    ) {
      return dispatch(
        openModal({
          type: "error",
          title: "Внимание",
          message: "Пожалуйста, заполните все поля карты.",
        })
      );
    }

    const result = await dispatch(
      createPayment({
        billId: Number(id),
        sum: bill.remainingAmount,
        method: "Картой",
      })
    );

    if (result.success) {
      setTimeout(() => {
        navigate("/personal-page/history");
      }, 1500);
    }
  };

  if (!bill) {
    return (
      <>
        <Header />
        <div className="payment-loading">Загрузка счета...</div>
      </>
    );
  }

  return (
    <>
      <Header />
      <div className="payment-page">
        <div className="payment-container">
          <div className="payment-forms-col">
            <div className="payment-card">
              <div className="payment-card-header">
                <div>
                  <h3 className="payment-card-title">Оплата поездки</h3>
                  <p className="payment-step-desc">
                    Введите данные банковской карты
                  </p>
                </div>
                <img src={Visa} alt="Visa" className="visa-logo" width="50" />
              </div>

              <div className="payment-grid-2">
                <div className="form-group full-width">
                  <label className="payment-label">Номер карты</label>
                  <input
                    type="text"
                    name="number"
                    placeholder="0000 0000 0000 0000"
                    className="payment-input"
                    value={cardData.number}
                    onChange={handleCardChange}
                    maxLength="19"
                  />
                </div>
                <div className="form-group full-width">
                  <label className="payment-label">Владелец карты</label>
                  <input
                    type="text"
                    name="holder"
                    placeholder="IVAN IVANOV"
                    className="payment-input"
                    value={cardData.holder}
                    onChange={handleCardChange}
                    style={{ textTransform: "uppercase" }}
                  />
                </div>
                <div className="form-group">
                  <label className="payment-label">Срок (MM/YY)</label>
                  <input
                    type="text"
                    name="expiry"
                    placeholder="12/26"
                    className="payment-input"
                    value={cardData.expiry}
                    onChange={handleCardChange}
                    maxLength="5"
                  />
                </div>
                <div className="form-group">
                  <label className="payment-label">CVC</label>
                  <input
                    type="password"
                    name="cvc"
                    placeholder="123"
                    className="payment-input"
                    value={cardData.cvc}
                    onChange={handleCardChange}
                    maxLength="3"
                  />
                </div>
              </div>

              <div className="payment-security">
                <img src={Security} alt="Security" />
                <div>
                  <h4>Безопасная оплата</h4>
                  <p>Ваши данные защищены протоколом SSL</p>
                </div>
              </div>

              <button className="payment-submit-btn" onClick={handlePay}>
                Оплатить {bill.remainingAmount} BYN
              </button>
            </div>
          </div>
          <div className="payment-summary-col">
            <div className="payment-summary-card">
              <h3 className="payment-card-title">Детали счета #{bill.id}</h3>
              <p className="payment-summary-desc">
                Дата формирования:{" "}
                {new Date(bill.issueDate).toLocaleDateString()}
              </p>

              <div className="payment-promo-block">
                <p className="promo-label">Есть промокод?</p>
                <div className="promo-input-group">
                  <input
                    type="text"
                    placeholder="Введите код"
                    value={promoCode}
                    onChange={(e) => setPromoCode(e.target.value)}
                    disabled={isApplyingPromo}
                  />
                  <button onClick={handleApplyPromo} disabled={isApplyingPromo}>
                    {isApplyingPromo ? "..." : "Применить"}
                  </button>
                </div>
                {bill.promocodeName && (
                  <div className="active-promo-badge">
                    <span>
                      🏷️ Скидка применена: <b>{bill.promocodeName}</b>
                    </span>
                  </div>
                )}
              </div>

              <div className="payment-prices-list">
                <div className="payment-price-row">
                  <span>Сумма поездки</span>
                  <span className="payment-price-value">{bill.amount} BYN</span>
                </div>
                {bill.remainingAmount < bill.amount && (
                  <div className="payment-price-row">
                    <span>Уже оплачено</span>
                    <span className="payment-price-value">
                      -{bill.amount - bill.remainingAmount} BYN
                    </span>
                  </div>
                )}
              </div>

              <div className="payment-total-block">
                <div>
                  <h3>К оплате</h3>
                  <p>Включая налоги</p>
                </div>
                <div className="payment-big-price">
                  {bill.remainingAmount} BYN
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default PaymentPage;

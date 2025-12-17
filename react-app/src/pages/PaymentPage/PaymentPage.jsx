import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import { getInfoCars } from "../../redux/actions/cars";
import { createBooking } from "../../redux/actions/bookings";

import Visa from "../../svg/Payment/visa.svg";
import PayPal from "../../svg/Payment/paypal.svg";
import Security from "../../svg/Payment/security.svg";

import "./PaymentPage.css";
import Header from "../../components/Header/Header";

const PaymentPage = () => {
  const { id } = useParams();
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const car = useSelector(
    (state) => state.cars.infoCar?.[0] || state.cars.currentCar
  );
  const user = useSelector((state) => state.users.myUser);

  const [formData, setFormData] = useState({
    name: "",
    phone: "",
    address: "",
    city: "",
    pickupLocation: "",
    pickupDate: "",
    pickupTime: "",
    dropoffLocation: "",
    dropoffDate: "",
    dropoffTime: "",
    cardNumber: "",
    cardHolder: "",
    cardDate: "",
    cardCvc: "",
    paymentMethod: "Credit Card",
    agreeMarketing: false,
    agreeTerms: false,
  });

  useEffect(() => {
    window.scrollTo(0, 0);
    if (id) dispatch(getInfoCars(id));
  }, [dispatch, id]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value,
    }));
  };

  const handleSubmit = async () => {
    if (!formData.agreeTerms)
      return alert("Пожалуйста, примите условия соглашения");
    if (!user?.id) return alert("Пожалуйста, авторизуйтесь");

    const startTime = new Date(
      formData.pickupDate + "T" + (formData.pickupTime || "12:00")
    ).toISOString();
    const endTime = new Date(
      formData.dropoffDate + "T" + (formData.dropoffTime || "12:00")
    ).toISOString();

    const bookingData = {
      carId: Number(id),
      clientId: user.id,
      statusId: 1,
      startTime: startTime,
      endTime: endTime,
    };

    const result = await dispatch(createBooking(bookingData));

    if (result && result.success) {
      alert("Бронирование успешно создано!");
      navigate("/dashboard");
    } else {
      alert("Ошибка бронирования");
    }
  };

  if (!car) return <div className="payment-loading">Загрузка...</div>;

  const imageUrl = car.imagePath ? `http://localhost:5078${car.imagePath}` : "";

  return (
    <>
      <Header />
      <div className="payment-page">
        <div className="payment-container">
          {/* ЛЕВАЯ КОЛОНКА */}
          <div className="payment-forms-col">
            {/* 1. Billing Info */}
            <div className="payment-card">
              <div className="payment-card-header">
                <div>
                  <h3 className="payment-card-title">Billing Info</h3>
                  <p className="payment-step-desc">
                    Please enter your billing info
                  </p>
                </div>
                <span className="payment-step-number">Step 1 of 4</span>
              </div>
              <div className="payment-grid-2">
                <label className="payment-label">
                  Name
                  <input
                    type="text"
                    name="name"
                    placeholder="Your name"
                    value={formData.name}
                    onChange={handleChange}
                    className="payment-input"
                  />
                </label>
                <label className="payment-label">
                  Phone Number
                  <input
                    type="text"
                    name="phone"
                    placeholder="Phone number"
                    value={formData.phone}
                    onChange={handleChange}
                    className="payment-input"
                  />
                </label>
                <label className="payment-label">
                  Address
                  <input
                    type="text"
                    name="address"
                    placeholder="Address"
                    value={formData.address}
                    onChange={handleChange}
                    className="payment-input"
                  />
                </label>
                <label className="payment-label">
                  Town / City
                  <input
                    type="text"
                    name="city"
                    placeholder="Town or city"
                    value={formData.city}
                    onChange={handleChange}
                    className="payment-input"
                  />
                </label>
              </div>
            </div>

            {/* 2. Rental Info */}
            <div className="payment-card">
              <div className="payment-card-header">
                <div>
                  <h3 className="payment-card-title">Rental Info</h3>
                  <p className="payment-step-desc">
                    Please select your rental date
                  </p>
                </div>
                <span className="payment-step-number">Step 2 of 4</span>
              </div>

              {/* Pick - Up */}
              <div className="payment-rental-section">
                <div className="payment-radio-header">
                  <input
                    type="radio"
                    checked
                    readOnly
                    className="payment-radio-input"
                  />{" "}
                  Pick - Up
                </div>
                <div className="payment-grid-3">
                  <label className="payment-label">
                    Locations
                    <select
                      name="pickupLocation"
                      className="payment-input"
                      onChange={handleChange}
                    >
                      <option>Select your city</option>
                      <option value="Minsk">Minsk</option>
                    </select>
                  </label>
                  <label className="payment-label">
                    Date
                    <input
                      type="date"
                      name="pickupDate"
                      className="payment-input"
                      onChange={handleChange}
                    />
                  </label>
                  <label className="payment-label">
                    Time
                    <input
                      type="time"
                      name="pickupTime"
                      className="payment-input"
                      onChange={handleChange}
                    />
                  </label>
                </div>
              </div>

              {/* Drop - Off */}
              <div className="payment-rental-section">
                <div className="payment-radio-header">
                  <input
                    type="radio"
                    checked
                    readOnly
                    className="payment-radio-input"
                  />{" "}
                  Drop - Off
                </div>
                <div className="payment-grid-3">
                  <label className="payment-label">
                    Locations
                    <select
                      name="dropoffLocation"
                      className="payment-input"
                      onChange={handleChange}
                    >
                      <option>Select your city</option>
                      <option value="Minsk">Minsk</option>
                    </select>
                  </label>
                  <label className="payment-label">
                    Date
                    <input
                      type="date"
                      name="dropoffDate"
                      className="payment-input"
                      onChange={handleChange}
                    />
                  </label>
                  <label className="payment-label">
                    Time
                    <input
                      type="time"
                      name="dropoffTime"
                      className="payment-input"
                      onChange={handleChange}
                    />
                  </label>
                </div>
              </div>
            </div>

            {/* 3. Payment Method */}
            <div className="payment-card">
              <div className="payment-card-header">
                <div>
                  <h3 className="payment-card-title">Payment Method</h3>
                  <p className="payment-step-desc">
                    Please enter your payment method
                  </p>
                </div>
                <span className="payment-step-number">Step 3 of 4</span>
              </div>

              <div className="payment-methods-list">
                {/* Credit Card */}
                <div className="payment-method-item active">
                  <div className="payment-method-header">
                    <label className="payment-method-label">
                      <input
                        type="radio"
                        name="paymentMethod"
                        value="Credit Card"
                        checked={formData.paymentMethod === "Credit Card"}
                        onChange={handleChange}
                        className="payment-radio-input"
                      />
                      <span>Credit Card</span>
                    </label>
                    <img src={Visa} alt="Visa" width="40" />
                  </div>
                  {formData.paymentMethod === "Credit Card" && (
                    <div className="payment-grid-2" style={{ marginTop: 20 }}>
                      <label className="payment-label">
                        Card Number{" "}
                        <input
                          type="text"
                          placeholder="Card number"
                          className="payment-input"
                        />
                      </label>
                      <label className="payment-label">
                        Expiration Date{" "}
                        <input
                          type="text"
                          placeholder="DD / MM / YY"
                          className="payment-input"
                        />
                      </label>
                      <label className="payment-label">
                        Card Holder{" "}
                        <input
                          type="text"
                          placeholder="Card holder"
                          className="payment-input"
                        />
                      </label>
                      <label className="payment-label">
                        CVC{" "}
                        <input
                          type="text"
                          placeholder="CVC"
                          className="payment-input"
                        />
                      </label>
                    </div>
                  )}
                </div>

                {/* PayPal */}
                <div className="payment-method-item">
                  <div className="payment-method-header">
                    <label className="payment-method-label">
                      <input
                        type="radio"
                        name="paymentMethod"
                        value="PayPal"
                        checked={formData.paymentMethod === "PayPal"}
                        onChange={handleChange}
                        className="payment-radio-input"
                      />
                      <span>PayPal</span>
                    </label>
                    <img src={PayPal} alt="PayPal" width="80" />
                  </div>
                </div>
              </div>
            </div>

            {/* 4. Confirmation */}
            <div className="payment-card">
              <div className="payment-card-header">
                <div>
                  <h3 className="payment-card-title">Confirmation</h3>
                  <p className="payment-step-desc">
                    We are getting to the end. Just few clicks and your rental
                    is ready!
                  </p>
                </div>
                <span className="payment-step-number">Step 4 of 4</span>
              </div>

              <div className="payment-checkbox-group">
                <label className="payment-checkbox-row">
                  <input
                    type="checkbox"
                    name="agreeMarketing"
                    onChange={handleChange}
                    className="payment-checkbox-input"
                  />
                  <span>
                    I agree with sending an Marketing and newsletter emails. No
                    spam, promised!
                  </span>
                </label>
                <label className="payment-checkbox-row">
                  <input
                    type="checkbox"
                    name="agreeTerms"
                    onChange={handleChange}
                    className="payment-checkbox-input"
                  />
                  <span>
                    I agree with our terms and conditions and privacy policy.
                  </span>
                </label>
              </div>

              <button className="payment-submit-btn" onClick={handleSubmit}>
                Rent Now
              </button>

              <div className="payment-security">
                <img src={Security} alt="Security" />
                <div>
                  <h4>All your data are safe</h4>
                  <p>
                    We are using the most advanced security to provide you the
                    best experience ever.
                  </p>
                </div>
              </div>
            </div>
          </div>

          {/* ПРАВАЯ КОЛОНКА - СВОДКА */}
          <div className="payment-summary-col">
            <div className="payment-summary-card">
              <h3 className="payment-card-title">Rental Summary</h3>
              <p className="payment-summary-desc">
                Prices may change depending on the length of the rental and the
                price of your rental car.
              </p>

              <div className="payment-summary-car">
                <div className="payment-car-img-box">
                  <img src={imageUrl} alt={car.model} />
                </div>
                <div className="payment-car-info">
                  <h2>
                    {car.brand} {car.model}
                  </h2>
                  <div className="payment-rating">⭐⭐⭐⭐⭐ 440+ Reviewer</div>
                </div>
              </div>

              <div className="payment-prices-list">
                <div className="payment-price-row">
                  <span>Subtotal</span>
                  <span className="payment-price-value">
                    ${car.pricePerDay}
                  </span>
                </div>
                <div className="payment-price-row">
                  <span>Tax</span>
                  <span className="payment-price-value">$0</span>
                </div>
              </div>

              <div className="payment-promo">
                <input type="text" placeholder="Apply promo code" />
                <button>Apply now</button>
              </div>

              <div className="payment-total-block">
                <div>
                  <h3>Total Rental Price</h3>
                  <p>Overall price and includes rental discount</p>
                </div>
                <div className="payment-big-price">${car.pricePerDay}</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default PaymentPage;

import { useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { createReview } from "../../redux/actions/reviews";
import "./ReviewModal.css";

const ReviewModal = ({ carId, onClose }) => {
  const dispatch = useDispatch();
  const [rating, setRating] = useState(0);
  const [hoverRating, setHoverRating] = useState(0);
  const [comment, setComment] = useState("");
  const myClient = useSelector((state) => state.clients.myClient);

  const handleSubmit = async () => {
    if (rating === 0) return;

    const reviewData = {
      carId: carId,
      clientId: myClient.id,
      rating: rating,
      comment: comment,
    };

    await dispatch(createReview(reviewData));
    onClose();
  };

  return (
    <div className="review-modal-overlay">
      <div className="review-modal-content">
        <h3>Как прошла поездка?</h3>
        <p className="review-subtitle">Оцените автомобиль и сервис</p>
        <div className="star-rating-large">
          {[1, 2, 3, 4, 5].map((star) => (
            <span
              key={star}
              className={`star-large ${
                star <= (hoverRating || rating) ? "filled" : ""
              }`}
              onMouseEnter={() => setHoverRating(star)}
              onMouseLeave={() => setHoverRating(0)}
              onClick={() => setRating(star)}
            >
              ★
            </span>
          ))}
        </div>
        <textarea
          className="review-textarea"
          placeholder="Напишите пару слов (необязательно)..."
          value={comment}
          onChange={(e) => setComment(e.target.value)}
          rows="3"
        />
        <div className="review-actions">
          <button className="btn-skip" onClick={onClose}>
            Позже
          </button>
          <button
            className="btn-submit-review"
            onClick={handleSubmit}
            disabled={rating === 0}
          >
            Отправить
          </button>
        </div>
      </div>
    </div>
  );
};

export default ReviewModal;

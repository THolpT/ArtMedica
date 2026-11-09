import "../style/stocks_style.css";
import Stock from "../image/stocks.svg";
import { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { apiGet, mediaUrl } from "../api";

interface ApiMediaFile {
  id: string;
  name: string;
  url: string;
}

interface ApiPromotion {
  id: string;
  title: string;
  description?: string;
  date: string;
  imgUrl?: ApiMediaFile | null;
}

const PAGE_SIZE = 4;

export function Stocks() {
  const [allCards, setAllCards] = useState<ApiPromotion[]>([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    apiGet<ApiPromotion[]>("/api/public/promotions")
      .then(setAllCards)
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false));
  }, []);

  const totalPages = Math.max(1, Math.ceil(allCards.length / PAGE_SIZE));
  const currentCards = allCards.slice(
    (currentPage - 1) * PAGE_SIZE,
    currentPage * PAGE_SIZE
  );

  const goToPage = (page: number) => {
    if (page >= 1 && page <= totalPages) setCurrentPage(page);
  };

  const getPaginationGroup = (): (number | "...")[] => {
    if (totalPages <= 5) {
      return Array.from({ length: totalPages }, (_, i) => i + 1);
    }
    if (currentPage <= 3) return [1, 2, 3, "...", totalPages];
    if (currentPage >= totalPages - 2) {
      return [1, "...", totalPages - 2, totalPages - 1, totalPages];
    }
    return [1, "...", currentPage, "...", totalPages];
  };

  return (
    <div className="Block_stocks">
      <nav className="breadcrumbs">
        <Link to="/">Арт-Медика</Link> &gt; <Link to="/stocks">Акции</Link>
      </nav>

      <div className="Block_link">
        <a href="#">Акции ↗</a>
        <p>
          Мы регулярно предлагаем выгодные акции и специальные предложения на популярные
          услуги, чтобы вы могли получать качественную медицинскую помощь по привлекательной
          стоимости
        </p>
      </div>

      {loading && <p>Загрузка…</p>}
      {error && <p>Не удалось загрузить акции: {error}</p>}

      <div className="stocks_boxs">
        {currentCards.map((card) => (
          <Link to={`/stocks/${card.id}`} className="stocks" key={card.id}>
              <img
    src={mediaUrl(card.imgUrl?.url) || Stock}
    alt={card.title}
    loading="lazy"
    style={{
      width: 400,
      height: 520,
      objectFit: "cover",
      objectPosition: "center",
      display: "block",
      borderRadius: "12px 12px 0 0", // если карточка со скруглением сверху
    }}
    onError={(e) => {
      e.currentTarget.onerror = null;
      e.currentTarget.src = Stock;
    }}
  />
            <div className="beet">
              <div className="stock_top">
                <p>Акции</p>
                <p className="stock_arrow">↗</p>
              </div>
              <div>
                <p className="stock_title">{card.title}</p>
                <p className="stock_desc">{card.description}</p>
              </div>
              <p className="stock_bottom">
                {new Date(card.date).toLocaleDateString("ru-RU")}
              </p>
            </div>
          </Link>
        ))}
      </div>

      {totalPages > 1 && (
        <div className="Block_pagination">
          <button
            className="nav-btn prev"
            onClick={() => goToPage(currentPage - 1)}
            disabled={currentPage === 1}
          >
            ←
          </button>

          <div className="count">
            {getPaginationGroup().map((item, index) => (
              <button
                key={index}
                onClick={() => typeof item === "number" && goToPage(item)}
                disabled={item === "..."}
                className={`page-btn ${item === currentPage ? "active" : ""} ${
                  item === "..." ? "dots" : ""
                }`}
              >
                {item}
              </button>
            ))}
          </div>

          <button
            className="nav-btn next"
            onClick={() => goToPage(currentPage + 1)}
            disabled={currentPage === totalPages}
          >
            →
          </button>
        </div>
      )}
    </div>
  );
}
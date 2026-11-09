import "../style/special_stocks_style.css";
import Stock from "../image/stocks.svg";
import { useState, useEffect } from "react";
import { Link, useParams } from "react-router-dom";
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

export function SpecialStocks() {
  const { id } = useParams<{ id: string }>();

  const [promo, setPromo] = useState<ApiPromotion | null>(null);
  const [others, setOthers] = useState<ApiPromotion[]>([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    setLoading(true);
    setError(null);
    apiGet<ApiPromotion[]>("/api/public/promotions")
      .then((list) => {
        const found = list.find((p) => p.id === id) ?? null;
        setPromo(found);
        setOthers(list.filter((p) => p.id !== found?.id));
      })
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false));
  }, [id]);

  const totalPages = Math.max(1, Math.ceil(others.length / PAGE_SIZE));
  const currentCards = others.slice(
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

  if (loading) {
    return (
      <div className="Block_stocks">
        <p>Загрузка…</p>
      </div>
    );
  }
  if (error) {
    return (
      <div className="Block_stocks">
        <p>Не удалось загрузить акцию: {error}</p>
      </div>
    );
  }
  if (!promo) {
    return (
      <div className="Block_stocks">
        <p>Акция не найдена.</p>
        <Link to="/stocks">← Вернуться к списку акций</Link>
      </div>
    );
  }

  return (
    <div className="Block_stocks">
      <nav className="breadcrumbs">
        <Link to="/">Арт-Медика</Link> &gt; <Link to="/stocks">Акции</Link> &gt;{" "}
        <span>{promo.title}</span>
      </nav>

      <div className="Block_link detail-header">
        <Link to="/stocks">Акции ↗</Link>
        <p className="detail-main-title">{promo.title}</p>
      </div>

      <div className="promo-detail-block">
        <div className="img_box">
          <img
            src={mediaUrl(promo.imgUrl?.url) || Stock}
            alt={promo.title}
            className="promo-detail-img"
            style={{
              width: 705,
              height: 607,
              objectFit: "contain",
              objectPosition: "center",
              display: "block",
              borderRadius: 16,
              background: "#eef2f7",
            }}
            onError={(e) => {
              e.currentTarget.onerror = null;
              e.currentTarget.src = Stock;
            }}
          />
        </div>

        <div className="promo-detail-content">
          <p className="promo-detail-text">{promo.description}</p>
          <p className="promo-detail-date">
            {new Date(promo.date).toLocaleDateString("ru-RU")}
          </p>
        </div>
      </div>

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
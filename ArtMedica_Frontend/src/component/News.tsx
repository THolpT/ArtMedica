import { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import "./News.css";
import { apiGet, mediaUrl } from "../api";

interface ApiMediaFile {
  id: string;
  name: string;
  type: string;
  url: string;
}

interface ApiNews {
  id: string;
  title: string;
  imgUrl: ApiMediaFile;
  description?: string;
}

const PAGE_SIZE = 9;

export default function NewsPage() {
  const [allNews, setAllNews] = useState<ApiNews[]>([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    apiGet<ApiNews[]>("/api/public/news")
      .then(setAllNews)
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false));
  }, []);

  const totalPages = Math.max(1, Math.ceil(allNews.length / PAGE_SIZE));
  const currentItems = allNews.slice(
    (currentPage - 1) * PAGE_SIZE,
    currentPage * PAGE_SIZE
  );

  const goToPage = (p: number) => {
    if (p >= 1 && p <= totalPages) setCurrentPage(p);
  };

  const getPaginationGroup = (): (number | "...")[] => {
    if (totalPages <= 5) return Array.from({ length: totalPages }, (_, i) => i + 1);
    if (currentPage <= 3) return [1, 2, 3, "...", totalPages];
    if (currentPage >= totalPages - 2)
      return [1, "...", totalPages - 2, totalPages - 1, totalPages];
    return [1, "...", currentPage, "...", totalPages];
  };

  return (
    <main className="news-container">
      <div className="news-header">
        <h1 className="news-title">Новости ↗</h1>
        <p className="news-intro-text">
          В этом разделе мы собираем всё, что формирует доверие пациентов:
          открытые итоги работы, значимые события, запуск новых услуг
          и кадровые изменения, влияющие на качество приёма.
        </p>
      </div>

      {loading && <p>Загрузка…</p>}
      {error && <p>Не удалось загрузить новости: {error}</p>}
      {!loading && !error && allNews.length === 0 && <p>Пока нет новостей.</p>}

      <div className="news-grid">
        {currentItems.map((item, idx) => {
          const isFeatured = currentPage === 1 && idx === 0;

          if (isFeatured) {
            return (
              <Link
                to={`/news/${item.id}`}
                key={item.id}
                className="news-card card-featured"
              >
                <div className="card-image">
                  <img src={mediaUrl(item.imgUrl?.url)} alt={item.title} />
                </div>
                <div className="card-featured-body">
                  <div className="card-featured-header">
                    <span className="card-badge">Новости</span>
                    <span className="card-arrow-icon">↗</span>
                  </div>
                  <h2 className="card-heading">{item.title}</h2>
                  {item.description && (
                    <p className="card-excerpt">{item.description}</p>
                  )}
                </div>
              </Link>
            );
          }

          return (
            <Link
              to={`/news/${item.id}`}
              key={item.id}
              className="news-card"
            >
              <div className="card-badge-row">
                <span className="card-badge">Новости</span>
                <span className="card-arrow-icon">↗</span>
              </div>
              <div className="card-image">
                <img src={mediaUrl(item.imgUrl?.url)} alt={item.title} />
              </div>
              <div className="card-body">
                <h2 className="card-heading">{item.title}</h2>
                {item.description && (
                  <p className="card-excerpt">{item.description}</p>
                )}
              </div>
            </Link>
          );
        })}
      </div>

      {totalPages > 1 && (
        <div className="pagination">
          <button
            className="page-arrow"
            onClick={() => goToPage(currentPage - 1)}
            disabled={currentPage === 1}
          >
            ←
          </button>

          <div className="page-numbers-wrap">
            {getPaginationGroup().map((item, i) =>
              typeof item === "number" ? (
                <button
                  key={i}
                  className={`page-num ${item === currentPage ? "active" : ""}`}
                  onClick={() => goToPage(item)}
                >
                  {item}
                </button>
              ) : (
                <span key={i} className="page-dots">
                  {item}
                </span>
              )
            )}
          </div>

          <button
            className="page-arrow"
            onClick={() => goToPage(currentPage + 1)}
            disabled={currentPage === totalPages}
          >
            →
          </button>
        </div>
      )}
    </main>
  );
}
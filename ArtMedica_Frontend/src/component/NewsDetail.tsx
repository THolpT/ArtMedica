import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import "./NewsDetail.css";
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

export default function NewsDetailPage() {
  const { id } = useParams<{ id: string }>();

  const [article, setArticle] = useState<ApiNews | null>(null);
  const [other, setOther] = useState<ApiNews[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!id) return;
    setLoading(true);
    setError(null);

    Promise.all([
      apiGet<ApiNews>(`/api/public/news/${id}`),
      apiGet<ApiNews[]>("/api/public/news"),
    ])
      .then(([detail, list]) => {
        setArticle(detail);
        setOther(list.filter((n) => n.id !== id).slice(0, 4));
      })
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false));
  }, [id]);

  if (loading) {
    return <main className="news-detail-container">Загрузка…</main>;
  }

  if (error || !article) {
    return (
      <main className="news-detail-container">
        <p>{error ? `Ошибка: ${error}` : "Новость не найдена"}</p>
        <Link to="/news">← Вернуться к новостям</Link>
      </main>
    );
  }

  // Абзацы приходят одной строкой — режем по переводу строки.
  // Если бэк будет отдавать HTML/Markdown — здесь можно поставить dangerouslySetInnerHTML
  // или библиотеку рендеринга.
  const paragraphs = (article.description ?? "").split("\n");

  return (
    <main className="news-detail-container">
      <nav className="breadcrumbs">
        <Link to="/">Арт-Медика</Link> <span>/</span>{" "}
        <Link to="/news">Новости</Link> <span>/</span>{" "}
        <span className="current">{article.title}</span>
      </nav>

      <div className="detail-header-row">
        <span className="back-arrow-detail">↗</span>
        <h1 className="detail-main-title">{article.title}</h1>
      </div>

      <div className="detail-content-wrapper">
        <div className="detail-image-side">
          <img src={mediaUrl(article.imgUrl?.url)} alt={article.title} />
        </div>

        <div className="detail-text-side">
          {paragraphs.map((p, i) =>
            p.trim() ? <p key={i}>{p}</p> : null
          )}
        </div>
      </div>

      {other.length > 0 && (
        <div className="news-grid block-separator">
          {other.map((item) => (
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
              </div>
            </Link>
          ))}
        </div>
      )}
    </main>
  );
}
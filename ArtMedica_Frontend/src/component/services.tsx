import { Link } from "react-router-dom";
import { useState, useEffect } from "react";
import "../style/services_style.css";
import { apiGet, mediaUrl } from "../api";

interface ApiMediaFile {
  id: string;
  name: string;
  type: string;
  url: string;
}

interface ApiServiceCategory {
  id: string;
  title: string;
  imgUrl: ApiMediaFile;
}

export const Services = () => {
  const [categories, setCategories] = useState<ApiServiceCategory[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    apiGet<ApiServiceCategory[]>("/public/service-categories")
      .then(setCategories)
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false));
  }, []);

  return (
    <main className="services-page">
      <div className="container">
        <nav className="breadcrumbs">
          <Link to="/">Арт-Медика</Link>
          <span className="separator">&gt;</span>
          <span className="current">Услуги</span>
        </nav>

        <header className="page-header">
          <div className="title-wrapper">
            <h1 className="page-title">Услуги ↗</h1>
          </div>
          <div className="header-description">
            <p>
              В клинике «Арт-медика» мы объединили многолетний опыт врачей и современные
              технологии, чтобы предложить вам максимально широкий спектр медицинских
              и эстетических услуг.
            </p>
          </div>
        </header>

        {loading && <p>Загрузка…</p>}
        {error && <p>Не удалось загрузить услуги: {error}</p>}

        <section className="services-grid">
          {categories.map((service) => (
            <article key={service.id} className="service-card">
              <div className="service-image-wrapper">
                <img
                  src={mediaUrl(service.imgUrl?.url)}
                  alt={service.title}
                  className="service-image"
                  loading="lazy"
                />
              </div>
              <h2 className="service-title">{service.title}</h2>
              <p className="service-description">
                Создаем молодость и поддерживаем вашу естественную красоту
              </p>
            </article>
          ))}
        </section>
      </div>
    </main>
  );
};
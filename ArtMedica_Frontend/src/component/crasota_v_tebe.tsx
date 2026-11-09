import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import "../style/crasota_v_tebe.css";
import Image_krasota_vector from "../image/image_crasota_vector.png";
import { apiGet, mediaUrl } from "../api";

interface ApiMediaFile {
  id: string;
  name: string;
  type: string;
  url: string;
}

interface ApiProjectFormat {
  id: string;
  title: string;
  description?: string;
  img?: ApiMediaFile | null;
  projectId: string;
}

interface ApiProjectPartner {
  id: string;
  title: string;
  description?: string;
  projectId: string;
}

interface ApiProjectDetail {
  id: string;
  title: string;
  mainImgUrl: ApiMediaFile;
  firstDescription?: string;
  secondDescription?: string;
  seasonCount: number;
  memberCount: number;
  expertCount: number;
  partnerCount: number;
  stages: string[];
  images: ApiMediaFile[];
  formats: ApiProjectFormat[];
  partners: ApiProjectPartner[];
  slideInfo?: {
    id: string;
    description?: string;
    imgUrl: ApiMediaFile;
    date: string;
    projectId: string;
  } | null;
}
const CARD_WIDTH = 380;
const GAP = 24;
const STEP = CARD_WIDTH + GAP;

const ProjectDetail = () => {
  const { id } = useParams<{ id: string }>();

  const [project, setProject] = useState<ApiProjectDetail | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [offset, setOffset] = useState(0);
  const [isAnimating, setIsAnimating] = useState(false);

  useEffect(() => {
    if (!id) return;
    setLoading(true);
    setError(null);
    apiGet<ApiProjectDetail>(`/api/public/projects/${id}`)
      .then((data) => {
        setProject(data);
        setOffset(0);
      })
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false));
  }, [id]);

  if (loading) {
    return (
      <main className="project-page">
        <div className="container">Загрузка…</div>
      </main>
    );
  }

  if (error || !project) {
    return (
      <main className="project-page">
        <div className="container">
          <p>{error ? `Ошибка: ${error}` : "Проект не найден"}</p>
          <Link to="/projects">← Вернуться к проектам</Link>
        </div>
      </main>
    );
  }

  // 🛡️ Безопасное извлечение массивов на случай null от бэкенда
  const formats = project.formats || [];
  const stages = project.stages || [];
  const images = project.images || [];
  const partners = project.partners || [];

  // Максимальный сдвиг для карусели
  const maxOffset = formats.length > 1 ? -(formats.length - 1) * STEP : 0;

  const handleNext = () => {
    if (isAnimating || formats.length <= 1) return;
    setIsAnimating(true);

    setOffset((prev) => {
      // Если дошли до конца — возвращаемся в начало
      if (prev <= maxOffset) return 0;
      return prev - STEP;
    });

    setTimeout(() => {
      setIsAnimating(false);
    }, 500);
  };

  const heroImg = project.slideInfo?.imgUrl?.url || project.mainImgUrl?.url || "";

  return (
    <main className="project-page">
      <div className="container">
        {/* Хлебные крошки и шапка (оставил без изменений) */}
        <nav className="breadcrumbs">
          <Link to="/">Арт-Медика</Link>
          <span className="separator">&gt;</span>
          <Link to="/projects">Проекты</Link>
          <span className="separator">&gt;</span>
          <span className="current">{project.title}</span>
        </nav>

        <header className="page-header">
          <div className="title-wrapper">
            <h1 className="page-title">{project.title}</h1>
          </div>
          <div className="header-description">
            <p>
              {project.slideInfo?.description ?? "Социальный проект-преображение"}
            </p>
          </div>
        </header>

        <section className="hero-image">
          <img src={mediaUrl(heroImg)} alt={project.title} />
        </section>

        <section className="project-description">
          <h2 className="section-title">Описание проекта</h2>
          <div className="description-grid">
            <p>{project.firstDescription}</p>
            <p>{project.secondDescription}</p>
          </div>
        </section>

        <section className="stats-grid">
          <div className="stat-card">
            <div className="stat-value">{project.seasonCount}</div>
            <div className="stat-label">сезонов проекта</div>
          </div>
          <div className="stat-card">
            <div className="stat-value">{project.memberCount}</div>
            <div className="stat-label">участников</div>
          </div>
          <div className="stat-card">
            <div className="stat-value">{project.expertCount}</div>
            <div className="stat-label">экспертов</div>
          </div>
          <div className="stat-card">
            <div className="stat-value">{project.partnerCount}</div>
            <div className="stat-label">партнеров</div>
          </div>
        </section>

        {/* 🎠 КАРУСЕЛЬ (Исправлена) */}
        {formats.length > 0 && (
          <section className="formats-section">
            <div className="formats-header">
              <h2 className="section-title">Форматы работы</h2>
              {formats.length > 1 && (
                <button
                  className="scroll-btn"
                  onClick={handleNext}
                  aria-label="Вперед"
                >
                  <img src={Image_krasota_vector} alt="" />
                </button>
              )}
            </div>

            <div className="carousel-viewport">
              <div
                className="carousel-track"
                style={{
                  transform: `translateX(${offset}px)`,
                  // Включаем анимацию только когда двигаем вручную
                  transition: isAnimating ? "transform 0.5s ease-in-out" : "none",
                }}
              >
                {/* Убрали infiniteFormats, рендерим оригинальный массив */}
                {formats.map((format) => {
                  const imgUrl = mediaUrl(format.img?.url);
                  return (
                    <div
                      key={format.id}
                      className={`format-card ${!imgUrl ? "format-card--dark" : ""}`}
                      style={imgUrl ? { backgroundImage: `url(${imgUrl})` } : {}}
                    >
                      {imgUrl && <div className="format-card__overlay" />}
                      <div className="format-card__content">
                        <h3 className="format-title">{format.title}</h3>
                        {format.description && (
                          <p className="format-description">
                            {format.description}
                          </p>
                        )}
                      </div>
                    </div>
                  );
                })}
              </div>
            </div>
          </section>
        )}

        {/* 🛡️ Безопасный рендеринг остальных блоков */}
        {stages.length > 0 && (
          <section className="stages-section">
            <h2 className="section-title">Этапы проекта</h2>
            <ol className="stages-list">
              {stages.map((stage, idx) => (
                <li key={idx} className="stage-item">
                  <span className="stage-number">{idx + 1}</span>
                  <span className="stage-name">{stage}</span>
                </li>
              ))}
            </ol>
          </section>
        )}

        {images.length > 0 && (
          <section className="gallery-section">
            <h2 className="section-title">Фотографии</h2>
            <div className="gallery-image">
              <img src={mediaUrl(images[0].url)} alt="Фото проекта" />
            </div>
          </section>
        )}

        {partners.length > 0 && (
          <section className="partners-section">
            <h2 className="section-title">Партнеры</h2>
            <ul className="partners-list">
              {partners.map((partner) => (
                <li key={partner.id} className="partner-item">
                  <span className="partner-name">{partner.title}</span>
                  {partner.description && (
                    <span className="partner-role">{partner.description}</span>
                  )}
                </li>
              ))}
            </ul>
          </section>
        )}
      </div>
    </main>
  );
};

export default ProjectDetail;
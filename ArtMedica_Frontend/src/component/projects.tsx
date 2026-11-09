import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import "../style/project._style.css";
import { apiGet, mediaUrl } from "../api";

interface ApiMediaFile {
  id: string;
  name: string;
  type: string;
  url: string;
}

interface ApiProject {
  id: string;
  title: string;
  mainImgUrl: ApiMediaFile;
  firstDescription?: string;
  secondDescription?: string;
  seasonCount: number;
  memberCount: number;
  expertCount: number;
  partnerCount: number;
}

export const Projects = () => {
  const [projectsData, setProjectsData] = useState<ApiProject[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    apiGet<ApiProject[]>("/api/public/projects")
      .then(setProjectsData)
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false));
  }, []);

  return (
    <main className="page-container">
      <nav className="breadcrumbs">
        <Link to="/">Арт-Медика</Link>
        <span className="separator">&gt;</span>
        <span className="current">Проекты</span>
      </nav>

      <header className="page-header">
        <div className="title-wrapper">
          <h1 className="page-title">Проекты ↗</h1>
        </div>
        <div className="header-description">
          <p>
            Подбираем для вас самые актуальные <br />
            и интересные проекты из нашей практики, медицины <br />
            и эстетической хирургии, с заботой о вас
          </p>
        </div>
      </header>

      {loading && <p>Загрузка…</p>}
      {error && <p>Не удалось загрузить проекты: {error}</p>}
      {!loading && !error && projectsData.length === 0 && (
        <p>Пока нет проектов.</p>
      )}

      <section className="projects-list">
        {projectsData.map((project) => {
          const description =
            project.firstDescription ?? project.secondDescription ?? "";

          return (
            <article key={project.id} className="card">
              <div className="image-wrapper">
                <img
                  src={mediaUrl(project.mainImgUrl?.url)}
                  alt={project.title}
                  className="image"
                  loading="lazy"
                />
              </div>

              <div className="content-wrapper">
                <div className="top-meta">
                  <span className="category">Проекты</span>
                  <Link
                    to={`/projects/${project.id}`}
                    className="arrow-link"
                    aria-label="Перейти к проекту"
                  >
                    ↗
                  </Link>
                </div>

                <div className="text-content">
                  <h2 className="card-title">{project.title}</h2>
                  <p className="card-description">{description}</p>
                </div>

                <div className="bottom-meta">
                  <div className="date-detail">
                    <Link
                      to={`/projects/${project.id}`}
                      className="detail-button"
                    >
                      Подробнее
                    </Link>
                    <time className="date">
                      {project.seasonCount} сезонов · {project.memberCount}{" "}
                      участников
                    </time>
                  </div>
                </div>
              </div>
            </article>
          );
        })}
      </section>
    </main>
  );
};
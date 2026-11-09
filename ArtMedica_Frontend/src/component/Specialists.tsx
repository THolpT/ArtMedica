import { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import "./Specialists.css";
import { apiGet, mediaUrl } from "../api";

interface ApiMediaFile {
  id: string;
  name: string;
  type: string;
  url: string;
}

interface ApiSpecialist {
  id: string;
  fullName: string;
  department?: string;
  imgUrl: ApiMediaFile;
  operationsCount: number;
  positiveReviewsPercentage: number;
  activityArea: string[];
}

export default function SpecialistsPage() {
  const [specialists, setSpecialists] = useState<ApiSpecialist[]>([]);
  const [departments, setDepartments] = useState<string[]>([]);
  const [activeCategory, setActiveCategory] = useState("Все");

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    Promise.all([
      apiGet<ApiSpecialist[]>("/api/public/specialists"),
      apiGet<string[]>("/api/public/specialists/departments"),
    ])
      .then(([list, deps]) => {
        setSpecialists(list);
        setDepartments(deps);
      })
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false));
  }, []);

  const categories = ["Все", ...departments];

  const filtered =
    activeCategory === "Все"
      ? specialists
      : specialists.filter((s) => s.department === activeCategory);

  return (
    <main className="specialists-container">
      <div className="breadcrumbs">
        <Link to="/">Арт-Медика</Link> <span>/</span>{" "}
        <span className="current">Специалисты</span>
      </div>

      <div className="specialists-header">
        <h1 className="specialists-title">Наши специалисты ↗</h1>
        <p className="specialists-intro-text">
          Наши специалисты — это команда профессионалов с многолетним опытом
          в пластической хирургии и косметологии. Они постоянно повышают своё
          мастерство, чтобы дарить вам гармонию, уверенность в себе и в своём теле.
        </p>
      </div>

      <div className="specialists-categories">
        {categories.map((cat) => (
          <button
            key={cat}
            className={`spec-tab-btn ${activeCategory === cat ? "active" : ""}`}
            onClick={() => setActiveCategory(cat)}
          >
            {cat}
          </button>
        ))}
      </div>

      {loading && <p>Загрузка…</p>}
      {error && <p>Не удалось загрузить специалистов: {error}</p>}

      {!loading && !error && filtered.length === 0 && (
        <p>В этой категории пока нет специалистов.</p>
      )}

      <div className="specialists-grid">
        {filtered.map((doctor) => (
          <Link
            to={`/specialists/${doctor.id}`}
            key={doctor.id}
            className="doctor-card"
          >
            <div className="doctor-photo">
              <img src={mediaUrl(doctor.imgUrl?.url)} alt={doctor.fullName} />
            </div>

            <div className="doctor-info-body">
              <div className="doctor-info-header">
                <span className="doctor-badge">{doctor.department ?? "Специалист"}</span>
                <span className="doctor-arrow">↗</span>
              </div>
              <h2 className="doctor-name">{doctor.fullName}</h2>
              <p className="doctor-description">
                Операций: {doctor.operationsCount} · Положительных отзывов:{" "}
                {doctor.positiveReviewsPercentage}%
              </p>

              {doctor.activityArea.length > 0 && (
                <p className="doctor-activity">
                  {doctor.activityArea.slice(0, 3).join(", ")}
                  {doctor.activityArea.length > 3 ? "…" : ""}
                </p>
              )}
            </div>
          </Link>
        ))}
      </div>
    </main>
  );
}
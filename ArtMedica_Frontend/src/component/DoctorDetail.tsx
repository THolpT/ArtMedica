import { useEffect, useRef, useState } from "react";
import { Link, useParams } from "react-router-dom";
import "./DoctorDetail.css";
import docPlaceholder from "../image/dock3.png";
import { apiGet, mediaUrl } from "../api";

interface ApiMediaFile {
  id: string;
  name: string;
  type: string;
  url: string;
}

interface ApiOperationResult {
  id: string;
  imgUrl: ApiMediaFile;
  operationId: string;
  operationTitle: string;
  specialistId: string;
  specialistName: string;
}

interface ApiQualification {
  id: string;
  title: string;
  level: string;
  organization: string;
  issuanceDate: string;
  specialistId: string;
}

interface ApiAccreditation {
  id: string;
  documentType: string;
  specialisation: string;
  jobTitle: string;
  issuanceDate: string;
  validityPeriod: string;
  description?: string;
  specialistId: string;
}

interface ApiSpecialistDetail {
  id: string;
  firstName: string;
  midName: string;
  lastName: string;
  fullName: string;
  department?: string;
  description?: string;
  imgUrl: ApiMediaFile;
  videoUrl: ApiMediaFile;
  startDate: string;
  operationsCount: number;
  positiveReviewsPercentage: number;
  activityArea: string[];
  operationResults: ApiOperationResult[];
  qualifications: ApiQualification[];
  accreditations: ApiAccreditation[];
}

const formatDate = (value?: string | null) =>
  value ? new Date(value).toLocaleDateString("ru-RU") : "—";

const formatYear = (value?: string | null) =>
  value ? new Date(value).getFullYear().toString() : "—";

export default function DoctorDetailPage() {
  const { id } = useParams<{ id: string }>();

  const [doctor, setDoctor] = useState<ApiSpecialistDetail | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [isEducationOpen, setIsEducationOpen] = useState(true);
  const [isAccreditationOpen, setIsAccreditationOpen] = useState(true);

  const [activeCategory, setActiveCategory] = useState<string>("0");
  const sliderRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (!id) return;
    setLoading(true);
    setError(null);
    apiGet<ApiSpecialistDetail>(`/api/public/specialists/${id}`)
      .then(setDoctor)
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false));
  }, [id]);

  if (loading) {
    return <main className="doctor-detail-container">Загрузка…</main>;
  }
  if (error || !doctor) {
    return (
      <main className="doctor-detail-container">
        <p>{error ? `Ошибка: ${error}` : "Специалист не найден"}</p>
        <Link to="/specialists">← Все специалисты</Link>
      </main>
    );
  }

  const operationTabs = [
    { id: "0", title: "Все" },
    ...Array.from(
      new Map(
        doctor.operationResults.map((r) => [r.operationId, r.operationTitle])
      )
    ).map(([opId, title]) => ({ id: opId, title })),
  ];

  const visibleResults =
    activeCategory === "0"
      ? doctor.operationResults
      : doctor.operationResults.filter((r) => r.operationId === activeCategory);

  const scrollSlider = (dir: "next" | "prev") => {
    if (!sliderRef.current) return;
    const el = sliderRef.current;
    const step = el.children[0]?.clientWidth ?? el.clientWidth / 3;
    el.scrollBy({ left: dir === "next" ? step : -step, behavior: "smooth" });
  };

  const startYear = new Date(doctor.startDate).getFullYear();
  const experienceYears = new Date().getFullYear() - startYear;

  return (
    <main className="doctor-detail-container">
      <div className="breadcrumbs">
        <Link to="/">Арт-Медика</Link>
        <span>/</span>
        <Link to="/specialists">Специалисты</Link>
        <span>/</span>
        <span className="current">{doctor.fullName}</span>
      </div>

      <div className="doctor-detail-header-row">
        <h1 className="doctor-page-title">
          Специалисты <span>↗</span>
        </h1>
        <h2 className="doctor-page-name">{doctor.fullName}</h2>
      </div>

      <div className="doctor-main-section">
        <div className="doctor-left-column">
          <div className="doctor-main-photo">
            <img
              src={mediaUrl(doctor.imgUrl?.url) || docPlaceholder}
              alt={doctor.fullName}
            />
          </div>
          <Link to="/contacts" className="btn-appointment">
            Записаться на приём
          </Link>
        </div>

        <div className="doctor-right-column">
          <p className="doctor-detail-bio">
            {doctor.description ?? doctor.department ?? ""}
          </p>

          <div className="doctor-metrics-grid">
            <div className="doc-metric-item">
              <h3>с {startYear} года</h3>
              <p>стаж ({experienceYears} лет)</p>
            </div>
            <div className="doc-metric-item">
              <h3>{doctor.operationsCount}</h3>
              <p>операций за время работы в нашей клинике</p>
            </div>
            <div className="doc-metric-item">
              <h3>{doctor.positiveReviewsPercentage}%</h3>
              <p>положительных отзывов</p>
            </div>
          </div>
        </div>
      </div>

      {doctor.activityArea.length > 0 && (
        <section className="directions-section">
          <h2>Направление деятельности:</h2>
          <div className="directions-grid">
            {doctor.activityArea.map((item, idx) => (
              <div key={idx} className="direction-list-col">
                <p>
                  <span>↗</span> {item}
                </p>
              </div>
            ))}
          </div>
        </section>
      )}

      {doctor.qualifications.length > 0 && (
        <section className="collapsible-table-section">
          <div
            className="table-toggle-header"
            onClick={() => setIsEducationOpen(!isEducationOpen)}
          >
            <h2>
              Данные об образовании из Федерального реестра медицинских работников
            </h2>
            <span
              className={`table-arrow-icon ${isEducationOpen ? "open" : ""}`}
            >
              ↑
            </span>
          </div>

          <div
            className={`table-scroll-wrapper ${
              isEducationOpen ? "visible" : "hidden"
            }`}
          >
            <table className="registry-table">
              <thead>
                <tr>
                  <th>Уровень образования</th>
                  <th>Образовательная организация</th>
                  <th>Год выдачи</th>
                  <th>Квалификация, специальность</th>
                </tr>
              </thead>
              <tbody>
                {doctor.qualifications.map((q) => (
                  <tr key={q.id}>
                    <td>{q.level}</td>
                    <td>{q.organization}</td>
                    <td>{formatYear(q.issuanceDate)}</td>
                    <td>{q.title}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </section>
      )}

      {doctor.accreditations.length > 0 && (
        <section className="collapsible-table-section">
          <div
            className="table-toggle-header"
            onClick={() => setIsAccreditationOpen(!isAccreditationOpen)}
          >
            <h2>Аккредитация (сертификация) специалиста</h2>
            <span
              className={`table-arrow-icon ${
                isAccreditationOpen ? "open" : ""
              }`}
            >
              ↑
            </span>
          </div>

          <div
            className={`table-scroll-wrapper ${
              isAccreditationOpen ? "visible" : "hidden"
            }`}
          >
            <table className="registry-table">
              <thead>
                <tr>
                  <th>Тип документа</th>
                  <th>Специальность</th>
                  <th>Должность</th>
                  <th>Дата выдачи</th>
                  <th>Срок действия</th>
                  <th>Примечание</th>
                </tr>
              </thead>
              <tbody>
                {doctor.accreditations.map((a) => (
                  <tr key={a.id}>
                    <td>{a.documentType}</td>
                    <td>{a.specialisation}</td>
                    <td>{a.jobTitle}</td>
                    <td>{formatDate(a.issuanceDate)}</td>
                    <td>{formatDate(a.validityPeriod)}</td>
                    <td>{a.description ?? "—"}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </section>
      )}

      <div className="results-section" style={{ marginTop: "80px" }}>
        <div className="results-header">
          <h2 className="results-title">Результат операций ↗</h2>
          <p className="results-intro-text">
            Мы гордимся результатами, которых достигаем вместе с нашими пациентами
            благодаря современным методикам и золотым рукам хирургов. Убедитесь
            сами, как тонко и естественно мы умеем работать с красотой.
          </p>
        </div>

        <div className="categories-tabs">
          {operationTabs.map((cat) => (
            <button
              key={cat.id}
              className={`tab-btn ${activeCategory === cat.id ? "active" : ""}`}
              onClick={() => {
                setActiveCategory(cat.id);
                sliderRef.current?.scrollTo({ left: 0, behavior: "smooth" });
              }}
            >
              {cat.title}
            </button>
          ))}
        </div>

        <div className="slider-viewport" ref={sliderRef}>
          <div className="slider-tape">
            {visibleResults.length === 0 && (
              <div className="slider-empty">
                У этого специалиста пока нет результатов по выбранной операции
              </div>
            )}
            {visibleResults.map((item) => (
              <div key={item.id} className="slider-slide-card">
                <img
                  src={mediaUrl(item.imgUrl?.url)}
                  alt={item.operationTitle}
                />
              </div>
            ))}
          </div>
        </div>

        {visibleResults.length > 3 && (
          <div className="slider-controls">
            <button
              className="slider-arrow"
              onClick={() => scrollSlider("prev")}
            >
              ←
            </button>
            <button
              className="slider-arrow"
              onClick={() => scrollSlider("next")}
            >
              →
            </button>
          </div>
        )}
      </div>
    </main>
  );
}
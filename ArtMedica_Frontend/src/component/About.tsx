import { useEffect, useRef, useState } from "react";
import { Link } from "react-router-dom";
import "./About.css";
import clinicHero from "../assets/1.1.png";
import { apiGet, mediaUrl } from "../api";

interface ApiMediaFile {
  id: string;
  name: string;
  type: string;
  url: string;
}

interface ApiService {
  id: string;
  displayTitle: string;
  categoryId: string;
}

interface ApiCategoryWithServices {
  id: string;
  title: string;
  imgUrl: ApiMediaFile;
  services: ApiService[];
}

interface ApiOperationResult {
  id: string;
  imgUrl: ApiMediaFile;
  operationId: string;
  operationTitle: string;
  specialistId: string;
  specialistName: string;
}

const DEP_CLINIC_ORDER = [
  "Оториноларингология",
  "Косметология",
  "Дерматология",
  "Венерология",
  "Эстетическая гинекология",
  "Неврология",
  "Диагностика",
  "Терапевтические услуги",
];

export default function AboutPage() {
  // данные для табов «Результаты операций»
  const [resultCategories, setResultCategories] = useState<
    { id: string; title: string }[]
  >([]);
  const [results, setResults] = useState<ApiOperationResult[]>([]);
  const [activeResultCategory, setActiveResultCategory] = useState<string>("0");
  const [resultsLoading, setResultsLoading] = useState(false);

  // данные для направлений поликлиники
  const [categories, setCategories] = useState<ApiCategoryWithServices[]>([]);

  const sliderRef = useRef<HTMLDivElement>(null);

  // загружаем категории с услугами один раз
  useEffect(() => {
    apiGet<ApiCategoryWithServices[]>("/api/public/services/by-categories")
      .then((list) => {
        setCategories(list);

        const allServices = list.flatMap((c) => c.services);
        setResultCategories([
          { id: "0", title: "Все" },
          ...allServices.map((s) => ({ id: s.id, title: s.displayTitle })),
        ]);
      })
      .catch(console.error);
  }, []);

  // загружаем результаты операций под активную услугу
  useEffect(() => {
    if (resultCategories.length === 0) return;
    setResultsLoading(true);

    const load = async () => {
      try {
        if (activeResultCategory === "0") {
          // bulk-endpoint отсутствует — берём по 6 первым услугам
          const services = categories.flatMap((c) => c.services).slice(0, 6);
          const arrays = await Promise.all(
            services.map((s) =>
              apiGet<ApiOperationResult[]>(
                `/api/public/services/${s.id}/operation-results`
              ).catch(() => [] as ApiOperationResult[])
            )
          );
          setResults(arrays.flat());
        } else {
          const data = await apiGet<ApiOperationResult[]>(
            `/api/public/services/${activeResultCategory}/operation-results`
          );
          setResults(data);
        }
      } catch (err) {
        console.error(err);
        setResults([]);
      } finally {
        setResultsLoading(false);
      }
    };

    load();
  }, [activeResultCategory, categories, resultCategories.length]);

  const scrollSlider = (dir: "next" | "prev") => {
    if (!sliderRef.current) return;
    const el = sliderRef.current;
    const step = el.children[0]?.clientWidth ?? el.clientWidth / 3;
    el.scrollBy({
      left: dir === "next" ? step : -step,
      behavior: "smooth",
    });
  };

  const sortedClinicDirections = [
    ...DEP_CLINIC_ORDER.filter((title) =>
      categories.some((c) => c.title.toLowerCase().includes(title.toLowerCase()))
    ),
    ...categories
      .map((c) => c.title)
      .filter(
        (title) =>
          !DEP_CLINIC_ORDER.some((t) =>
            title.toLowerCase().includes(t.toLowerCase())
          )
      ),
  ];

  return (
    <main className="about-container">
      <div className="breadcrumbs">
        <Link to="/">Арт-Медика</Link> <span>/</span>{" "}
        <span className="current">О клинике</span>
      </div>

      <div className="about-header">
        <h1 className="about-title">О клинике ↗</h1>
        <p className="about-intro-text">
          Превращаем заветные мечты пациентов в реальность, невзирая на сложность
          задачи
        </p>
      </div>

      <div className="about-hero-section">
        <div className="about-hero-image">
          <img src={clinicHero} alt="Интерьер Арт-Медика" />
        </div>
        <div className="about-hero-content">
          <p className="about-description-text">
            Центр «Арт-Медика» — партнер ЮУГМУ, клиническая база «Межрегионального
            центра непрерывного медицинского и фармацевтического образования»
            (г. Москва). Наша клиника является участником международных проектов
            в направлении "Пластическая хирургия и косметология".
          </p>
          <p className="about-description-text">
            Качество результата обеспечивает инновационное высокотехнологичное
            хирургическое, эндоскопическое и лазерное оборудование.
          </p>

          <div className="metrics-grid">
            <div className="metric-item">
              <h2>10+</h2>
              <p>обучений в год проходят специалисты центра</p>
            </div>
            <div className="metric-item">
              <h2>20</h2>
              <p>проектов уже было реализовано в нашей клинике</p>
            </div>
            <div className="metric-item">
              <h2>90%</h2>
              <p>положительных отзывов</p>
            </div>
          </div>
        </div>
      </div>

      <div className="departments-section">
        <div className="department-col">
          <h3>Поликлиническое отделение</h3>
          <p className="sub-caption">
            Вы можете получить помощь по направлениям:
          </p>
          <ul className="dep-links-list">
            {sortedClinicDirections.length === 0 && (
              <li className="dep-empty">Загрузка…</li>
            )}
            {sortedClinicDirections.map((title, i) => (
              <li key={`${title}-${i}`}>
                <span className="arrow">↗</span> {title}
              </li>
            ))}
          </ul>
        </div>

        <div className="department-col separator-left">
          <h3>Стационарное отделение</h3>
          <p className="dep-text">
            На территории отделения мы ежедневно оказываем помощь даже самым
            маленьким пациентам, выполняя полный спектр операций по направлению
            «Лор-хирургия», одним из самых востребованных остается аденотомия
            и септопластика (исправление носовой перегородки).
          </p>
          <p className="dep-text">
            В стационарном отделении к вашим услугам палата европейского уровня
            с 3-х разовым питанием. Медицинская сестра будет сопровождать Вас
            24 часа в сутки для создания самых комфортных условий.
          </p>
          <p className="dep-text">
            Также в стационарном отделении мы оказываем полный спектр операций
            по направлению «Пластическая хирургия», особое место занимает проект
            «Доступная пластическая хирургия из Милана».
          </p>
          <div className="dep-footer-row">
            <button className="btn-more">Подробнее</button>
            <div className="dep-legal-links">
              <a href="#" target="_blank" rel="noreferrer">
                Сайт Министерства здравоохранения Российской Федерации
              </a>
              <a href="#" target="_blank" rel="noreferrer">
                Официальный интернет-портал правовой информации
              </a>
            </div>
          </div>
        </div>
      </div>

      <div className="results-section">
        <div className="results-header">
          <h2 className="results-title">Результат операций ↗</h2>
          <p className="results-intro-text">
            Мы гордимся результатами, которых достигаем вместе с нашими пациентами
            благодаря современным методикам и золотым рукам хирургов. Убедитесь
            сами, как тонко и естественно мы умеем работать с красотой.
          </p>
        </div>

        <div className="categories-tabs">
          {resultCategories.map((cat) => (
            <button
              key={cat.id}
              className={`tab-btn ${activeResultCategory === cat.id ? "active" : ""}`}
              onClick={() => {
                setActiveResultCategory(cat.id);
                sliderRef.current?.scrollTo({ left: 0, behavior: "smooth" });
              }}
            >
              {cat.title}
            </button>
          ))}
        </div>

        <div className="slider-viewport" ref={sliderRef}>
          <div className="slider-tape">
            {resultsLoading && <div className="slider-empty">Загрузка…</div>}
            {!resultsLoading && results.length === 0 && (
              <div className="slider-empty">Нет результатов для этой услуги</div>
            )}
            {!resultsLoading &&
              results.map((item) => (
                <div key={item.id} className="slider-slide-card">
                  <img
                    src={mediaUrl(item.imgUrl?.url)}
                    alt={item.operationTitle}
                  />
                </div>
              ))}
          </div>
        </div>

        {results.length > 3 && (
          <div className="slider-controls">
            <button className="slider-arrow" onClick={() => scrollSlider("prev")}>
              ←
            </button>
            <button className="slider-arrow" onClick={() => scrollSlider("next")}>
              →
            </button>
          </div>
        )}
      </div>
    </main>
  );
}
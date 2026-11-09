import "../style/index_style.css";
import Phone from "../image/phone.svg";
import { useEffect, useState, useRef } from "react";
import { Link } from "react-router-dom";
import Health from "../image/your_health.svg";
import Block3 from "../image/Block3.svg";
import { apiGet, mediaUrl } from "../api";

interface ApiMediaFile {
  id: string;
  name: string;
  type: string;
  url: string;
}

interface ApiService {
  id: string;
  nomenclatureCode: string;
  misCode: string;
  duration: number;
  price: number;
  displayTitle: string;
  technicalTitle: string;
  description?: string;
  preparation?: string;
  types?: string;
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

interface ApiSpecialist {
  id: string;
  fullName: string;
  department?: string;
  imgUrl: ApiMediaFile;
  operationsCount: number;
  positiveReviewsPercentage: number;
  activityArea: string[];
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

interface ApiCase {
  id: string;
  clientName: string;
  problem: string;
  solution: string;
  date: string;
  review: number;
}

export function Index() {
  const [categories, setCategories] = useState<ApiCategoryWithServices[]>([]);
  const [openCategoryId, setOpenCategoryId] = useState<string | null>(null);

  const [resultCategories, setResultCategories] = useState<{ id: string; title: string }[]>([]);
  const [results, setResults] = useState<ApiOperationResult[]>([]);
  const [activeResultCategory, setActiveResultCategory] = useState<string>("0");

  const [specialistCategories, setSpecialistCategories] = useState<{ id: string; title: string }[]>([]);
  const [specialists, setSpecialists] = useState<ApiSpecialist[]>([]);
  const [activeSpecialistCategory, setActiveSpecialistCategory] = useState<string>("0");

  const [projects, setProjects] = useState<ApiProject[]>([]);
  const [cases, setCases] = useState<ApiCase[]>([]);

  const slider4Ref = useRef<HTMLDivElement>(null);
  const slider5Ref = useRef<HTMLDivElement>(null);
  const slider6Ref = useRef<HTMLDivElement>(null);
  const slider7Ref = useRef<HTMLDivElement>(null);

  const VISIBLE_RESULTS = 3;
  const VISIBLE_SLIDES = 4;

  // --- базовые данные главной страницы ---
  useEffect(() => {
    const fetchData = async () => {
      try {
        const [catsWithServices, specialistsList, departments, projectsList, casesList] =
          await Promise.all([
            apiGet<ApiCategoryWithServices[]>("/api/public/services/by-categories"),
            apiGet<ApiSpecialist[]>("/api/public/specialists"),
            apiGet<string[]>("/api/public/specialists/departments"),
            apiGet<ApiProject[]>("/api/public/projects"),
            apiGet<ApiCase[]>("/api/public/cases"),
          ]);

        setCategories(catsWithServices);

        const allServices = catsWithServices.flatMap((c) => c.services);
        setResultCategories([
          { id: "0", title: "Все" },
          ...allServices.map((s) => ({ id: s.id, title: s.displayTitle })),
        ]);

        setSpecialistCategories([
          { id: "0", title: "Все" },
          ...departments.map((d, i) => ({ id: `dept-${i}`, title: d })),
        ]);

        setSpecialists(specialistsList);
        setProjects(projectsList);
        setCases(casesList);
      } catch (err) {
        console.error(err);
      }
    };
    fetchData();
  }, []);

  // --- результаты операций по выбранной услуге ---
  useEffect(() => {
    if (categories.length === 0) return;

    const fetchResults = async () => {
      try {
        if (activeResultCategory === "0") {
          // bulk-эндпоинта нет — забираем результаты для 6 первых услуг
          const services = categories.flatMap((c) => c.services).slice(0, 6);
          const arrays = await Promise.all(
            services.map((s) =>
              apiGet<ApiOperationResult[]>(`/api/public/services/${s.id}/operation-results`)
                .catch(() => [] as ApiOperationResult[])
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
      }
    };
    fetchResults();
  }, [activeResultCategory, categories]);

  const toggleCategory = (id: string) =>
    setOpenCategoryId((prev) => (prev === id ? null : id));

  const filteredSpecialists =
    activeSpecialistCategory === "0"
      ? specialists
      : specialists.filter((s) => {
          const cat = specialistCategories.find((c) => c.id === activeSpecialistCategory);
          return cat && s.department === cat.title;
        });

  const scrollSlider = (
    ref: React.RefObject<HTMLDivElement | null>,
    direction: "next" | "prev",
    visibleCount: number
  ) => {
    if (ref.current) {
      const container = ref.current;
      const slideWidth =
        container.children[0]?.clientWidth || container.clientWidth / visibleCount;
      const scrollAmount = direction === "next" ? slideWidth : -slideWidth;
      container.scrollBy({ left: scrollAmount, behavior: "smooth" });
    }
  };

  const resetScroll = (ref: React.RefObject<HTMLDivElement | null>) => {
    ref.current?.scrollTo({ left: 0, behavior: "smooth" });
  };

  return (
    <>
      <div className="Block1">
        <div className="Block1_text">
          <p className="Block1_title">Ваше здоровье — наша забота</p>
          <p className="Block1_title2">
            Медико-косметологический центр с современным оборудованием и опытными специалистами
          </p>
        </div>
        <div className="Block1_bottom">
          <div className="Block1_boxs">
            <div className="adventeg">
              <p className="Block1_title">20</p>
              <p className="m0 t24">лет опыта работы<br />в сфере медицины</p>
            </div>
            <div className="adventeg">
              <p className="Block1_title">{categories.length}</p>
              <p className="m0 t24">направлений<br />медицинской деятельности</p>
            </div>
            <div className="adventeg">
              <p className="Block1_title">{specialists.length}</p>
              <p className="m0 t24">квалифицированных<br />специалистов</p>
            </div>
          </div>
          <img className="Block1_phone" src={Phone} alt="" />
        </div>
      </div>

      <div className="Block1_boxs_hide">
        <div className="adventeg">
          <p className="Block1_title">20</p>
          <p className="m0 t24">лет опыта работы в сфере медицины</p>
        </div>
        <div className="adventeg">
          <p className="Block1_title">{categories.length}</p>
          <p className="m0 t24">направлений медицинской деятельности</p>
        </div>
        <div className="adventeg">
          <p className="Block1_title">{specialists.length}</p>
          <p className="m0 t24">квалифицированных специалистов</p>
        </div>
      </div>

      <div className="Block2">
        <div className="Block_link">
          <Link to="/services">Услуги ↗</Link>
          <p>
            В клинике «Арт-медика» мы объединили многолетний опыт врачей и современные технологии, чтобы
            предложить вам максимально широкий спектр медицинских и эстетических услуг.
          </p>
        </div>
        <div className="Block2_lists">
          {categories.map((cat) => {
            const isOpen = openCategoryId === cat.id;

            return (
              <div key={cat.id} className="category-wrapper">
                <button className="list" onClick={() => toggleCategory(cat.id)}>
                  <p>{cat.title}</p>
                  <p>{isOpen ? "↑" : "↓"}</p>
                </button>

                <div className={`Block2_all_uslugi ${isOpen ? "active" : ""}`}>
                  <div>
                    {cat.services.length > 0 ? (
                      cat.services.map((service) => {
                        const typesItems =
                          service.types
                            ?.split(",")
                            .map((t) => t.trim())
                            .filter(Boolean) ?? [];

                        return (
                          <Link
                            to={`/services/${cat.id}`}
                            key={service.id}
                            className="uslug-card"
                            style={{
                              backgroundImage: `url(${mediaUrl(cat.imgUrl?.url) || Health})`,
                              backgroundRepeat: "no-repeat",
                              backgroundSize: "cover",
                              backgroundPosition: "center",
                            }}
                          >
                            <p className="name_usluga">{service.displayTitle}</p>
                            {typesItems.length > 0 && (
                              <ul className="uslug-list">
                                {typesItems.map((title, i) => (
                                  <li key={i} className="uslug-item">
                                    <span>{title}</span>
                                    <span className="arrow">→</span>
                                  </li>
                                ))}
                              </ul>
                            )}
                          </Link>
                        );
                      })
                    ) : (
                      <p>Нет доступных услуг в этой категории</p>
                    )}
                  </div>
                </div>
              </div>
            );
          })}
        </div>
      </div>

      <div className="Block3">
        {/* статичный блок — не меняем */}
      </div>

      <div className="Block4">
        <div className="Block_link">
          <Link to="/prices">Результаты операций ↗</Link>
          <p>
            Мы гордимся результатами, которых достигаем вместе с нашими пациентами благодаря современным
            методикам и золотым рукам хирургов. Убедитесь сами, как тонко и естественно мы умеем работать
            с красотой.
          </p>
        </div>

        <div className="Block4_tags">
          {resultCategories.map((cat) => (
            <button
              key={cat.id}
              className={`tag-btn ${activeResultCategory === cat.id ? "active" : ""}`}
              onClick={() => {
                setActiveResultCategory(cat.id);
                resetScroll(slider4Ref);
              }}
            >
              {cat.title}
            </button>
          ))}
        </div>

        <div className="Block_slider_scroll" ref={slider4Ref}>
          {results.map((item) => (
            <div key={item.id} className="Block4_slide">
              <img
                src={mediaUrl(item.imgUrl?.url) || Block3}
                alt={item.operationTitle}
                className="Block4_img"
              />
            </div>
          ))}
        </div>

        {results.length > VISIBLE_RESULTS && (
          <div className="Block4_controls">
            <button
              className="nav-btn prev"
              onClick={() => scrollSlider(slider4Ref, "prev", VISIBLE_RESULTS)}
            >
              ←
            </button>
            <button
              className="nav-btn next"
              onClick={() => scrollSlider(slider4Ref, "next", VISIBLE_RESULTS)}
            >
              →
            </button>
          </div>
        )}
      </div>

      <div className="Block5">
        <div className="Block_link">
          <Link to="/specialists">Наши специалисты ↗</Link>
          <p>
            Наши специалисты — это команда профессионалов с многолетним опытом в пластической хирургии
            и косметологии. Они постоянно повышают своё мастерство, чтобы дарить вам гармонию, уверенность
            в себе и в своём теле.
          </p>
        </div>

        <div className="Block4_tags">
          {specialistCategories.map((cat) => (
            <button
              key={cat.id}
              className={`tag-btn ${activeSpecialistCategory === cat.id ? "active" : ""}`}
              onClick={() => {
                setActiveSpecialistCategory(cat.id);
                resetScroll(slider5Ref);
              }}
            >
              {cat.title}
            </button>
          ))}
        </div>

        <div className="Block_slider_scroll" ref={slider5Ref}>
          {filteredSpecialists.map((item) => (
            <div key={item.id} className="Block5_slide">
              <Link to={`/specialists/${item.id}`} className="spec-card">
                <img
                  src={mediaUrl(item.imgUrl?.url) || Health}
                  alt={item.fullName}
                  className="spec-img"
                />
                <div className="spec-info">
                  <p className="spec-name">{item.fullName}</p>
                  <p className="spec-role">{item.department ?? ""}</p>
                  <p className="spec-desc">
                    Операций: {item.operationsCount} · Отзывов положительных:{" "}
                    {item.positiveReviewsPercentage}%
                  </p>
                </div>
              </Link>
            </div>
          ))}
        </div>

        {filteredSpecialists.length > VISIBLE_SLIDES && (
          <div className="Block4_controls">
            <button
              className="nav-btn prev"
              onClick={() => scrollSlider(slider5Ref, "prev", VISIBLE_SLIDES)}
            >
              ←
            </button>
            <button
              className="nav-btn next"
              onClick={() => scrollSlider(slider5Ref, "next", VISIBLE_SLIDES)}
            >
              →
            </button>
          </div>
        )}
      </div>

      <div className="Block6">
        <div className="Block_link">
          <Link to="/projects">Проекты ↗</Link>
          <p>
            Подбираем для вас самые актуальные и интересные проекты из нашей практики, медицины и
            эстетической хирургии, с заботой о вас.
          </p>
        </div>

        <div className="Block_slider_scroll" ref={slider6Ref}>
          {projects.map((item) => (
            <div key={item.id} className="Block6_slide">
              <Link to={`/projects/${item.id}`} className="project-card">
                <div className="project-header">
                  <span className="project-tag">Проекты</span>
                  <span className="project-arrow">↗</span>
                </div>
                <img
                  src={mediaUrl(item.mainImgUrl?.url) || Health}
                  alt={item.title}
                  className="project-img"
                />
                <div className="project-content">
                  <p className="project-title">{item.title}</p>
                  <p className="project-date">
                    {item.seasonCount} сезонов · {item.memberCount} участников
                  </p>
                </div>
              </Link>
            </div>
          ))}
        </div>

        {projects.length > VISIBLE_SLIDES && (
          <div className="Block4_controls">
            <button
              className="nav-btn prev"
              onClick={() => scrollSlider(slider6Ref, "prev", VISIBLE_SLIDES)}
            >
              ←
            </button>
            <button
              className="nav-btn next"
              onClick={() => scrollSlider(slider6Ref, "next", VISIBLE_SLIDES)}
            >
              →
            </button>
          </div>
        )}
      </div>

      <div className="Block7">
        {/* VK-видео — статика */}
      </div>

      <div className="Block8">
        <div className="Block_link">
          <a href="#">Кейсы клиентов ↗</a>
          <p>
            Высокий уровень доверия и качества медицинской помощи подтверждает рейтинг нашей
            клиники — 4.5 на основе 118 отзывов о врачах на сайте «ПроДокторов».
          </p>
        </div>

        <div className="Block_slider_scroll" ref={slider7Ref}>
          {cases.map((item) => (
            <div key={item.id} className="Block8_slide">
              <div className="case-card">
                <div className="case-header">
                  <div className="case-name-row">
                    <span className="case-name">{item.clientName}</span>
                    <span className="case-stars">{"★".repeat(item.review)}</span>
                  </div>
                  <span className="case-arrow">↗</span>
                </div>
                <div className="case-body">
                  <div className="case-column">
                    <p className="case-label">Проблема</p>
                    <p className="case-text">{item.problem}</p>
                  </div>
                  <div className="case-column">
                    <p className="case-label">Решение</p>
                    <p className="case-text">{item.solution}</p>
                  </div>
                </div>
                <p className="case-date">
                  {new Date(item.date).toLocaleDateString("ru-RU")}
                </p>
              </div>
            </div>
          ))}
        </div>

        {cases.length > 2 && (
          <div className="Block4_controls">
            <button className="nav-btn prev" onClick={() => scrollSlider(slider7Ref, "prev", 2)}>
              ←
            </button>
            <button className="nav-btn next" onClick={() => scrollSlider(slider7Ref, "next", 2)}>
              →
            </button>
          </div>
        )}
      </div>
    </>
  );
}
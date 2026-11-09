import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import "../style/prices_style.css";
import { apiGet } from "../api";

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

const formatPrice = (price: number) =>
  price > 0 ? `${price.toLocaleString("ru-RU")} ₽` : "—";

const Prices = () => {
  const [categories, setCategories] = useState<ApiCategoryWithServices[]>([]);
  const [openIds, setOpenIds] = useState<string[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    apiGet<ApiCategoryWithServices[]>("/api/public/services/by-categories")
      .then((data) => {
        setCategories(data);
        // Открываем первую категорию с услугами — чтобы страница не выглядела пустой
        const firstWithServices = data.find((c) => c.services.length > 0);
        if (firstWithServices) setOpenIds([firstWithServices.id]);
      })
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false));
  }, []);

  const toggle = (id: string) => {
    setOpenIds((prev) =>
      prev.includes(id) ? prev.filter((x) => x !== id) : [...prev, id]
    );
  };

  const renderTable = (services: ApiService[]) => (
    <div className="price-table-wrapper">
      <table className="price-table">
        <colgroup>
          <col className="col-code" />
          <col className="col-mis" />
          <col className="col-name" />
          <col className="col-unit" />
          <col className="col-time" />
          <col className="col-price" />
        </colgroup>
        <thead>
          <tr>
            <th>
              Код<br />Номенклатуры<br />мед услуг
            </th>
            <th>Код МИС</th>
            <th>Наименование медицинской услуги</th>
            <th>
              Ед.<br />изм-ия
            </th>
            <th>
              Время,<br />мин
            </th>
            <th>
              Цена,<br />руб.
            </th>
          </tr>
        </thead>
        <tbody>
          {services.map((s) => (
            <tr key={s.id}>
              <td data-label="Код">{s.nomenclatureCode || "—"}</td>
              <td data-label="Код МИС">{s.misCode || "—"}</td>
              <td data-label="Наименование">
                <div className="service-name">{s.displayTitle}</div>
                {s.technicalTitle && s.technicalTitle !== s.displayTitle && (
                  <div className="service-technical">{s.technicalTitle}</div>
                )}
                {s.types && (
                  <div className="service-types">
                    {s.types
                      .split(",")
                      .map((t) => t.trim())
                      .filter(Boolean)
                      .join(" · ")}
                  </div>
                )}
              </td>
              <td data-label="Ед. изм.">услуга</td>
              <td data-label="Время">{s.duration > 0 ? s.duration : "—"}</td>
              <td data-label="Цена" className="single-price">
                {formatPrice(s.price)}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );

  const renderCategory = (
    category: ApiCategoryWithServices,
    index: number
  ) => {
    const isOpen = openIds.includes(category.id);
    const hasServices = category.services.length > 0;

    const minPrice = hasServices
      ? Math.min(...category.services.map((s) => s.price).filter((p) => p > 0))
      : 0;

    return (
      <section
        key={category.id}
        className={`price-section ${
          index === 0 ? "price-section--white" : ""
        }`}
      >
        <button
          type="button"
          className="section-header"
          onClick={() => toggle(category.id)}
          aria-expanded={isOpen}
        >
          <h2 className="section-title">
            {category.title}
            {minPrice > 0 && (
              <span className="section-price">
                от {minPrice.toLocaleString("ru-RU")} ₽
              </span>
            )}
          </h2>
          <span className={`section-arrow ${isOpen ? "open" : ""}`}>
            {isOpen ? "↑" : "↓"}
          </span>
        </button>

        {isOpen && hasServices && (
          <div className="section-body">{renderTable(category.services)}</div>
        )}

        {isOpen && !hasServices && (
          <div className="section-body">
            <p className="section-empty">В этой категории пока нет услуг.</p>
          </div>
        )}
      </section>
    );
  };

  return (
    <main className="prices-page">
      <div className="container">
        <div className="breadcrumbs">
          <Link to="/">Главная</Link>
          <span className="separator">/</span>
          <span className="current">Цены на услуги</span>
        </div>

        <header className="page-header">
          <div className="title-wrapper">
            <h1 className="page-title">Цены на услуги ↗</h1>
          </div>
          <div className="header-description">
            <p>
              Актуальные цены на медицинские и эстетические услуги клиники
            </p>
          </div>
        </header>

        {loading && <p>Загрузка…</p>}
        {error && <p>Не удалось загрузить цены: {error}</p>}
        {!loading && !error && categories.length === 0 && (
          <p>Пока нет услуг.</p>
        )}

        {!loading &&
          !error &&
          categories.map((cat, idx) => renderCategory(cat, idx))}

        {!loading && !error && categories.length > 0 && (
          <p className="section-note">
            * Категорию сложности присваивает врач-хирург на консультации,
            в зависимости от состояния пациента.
          </p>
        )}
      </div>
    </main>
  );
};

export default Prices;
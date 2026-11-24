import { useEffect, useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import { useDesignScale } from '../hooks/useDesignScale';
import { apiGet, mediaUrl } from '../api';
import './PlasticSurgery.css';

interface MediaFileDto {
  id: string;
  name: string;
  url: string;
}

interface ServiceCategoryDto {
  id: string;
  title: string;
  imgUrl: MediaFileDto;
}

interface ServiceSubcategoryDto {
  id: string;
  title: string;
  imgUrl: MediaFileDto;
  description?: string | null;
}

export default function PlasticSurgeryPage() {
  const { categoryId } = useParams<{ categoryId: string }>();
  const scale = useDesignScale();

  const [categoryTitle, setCategoryTitle] = useState<string>('');
  const [subcategories, setSubcategories] = useState<ServiceSubcategoryDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!categoryId) return;

    let cancelled = false;
    setLoading(true);
    setError(null);

    (async () => {
      try {
        const categories = await apiGet<ServiceCategoryDto[]>('/public/service-categories');
        const category = categories.find((c) => c.id === categoryId);

        if (!category) {
          if (!cancelled) setError('Категория не найдена');
          return;
        }

        if (!cancelled) setCategoryTitle(category.title);

        const subs = await apiGet<ServiceSubcategoryDto[]>(
          `/public/service-subcategories?categoryId=${categoryId}`,
        );

        if (!cancelled) setSubcategories(subs);
      } catch (e) {
        if (!cancelled) setError(e instanceof Error ? e.message : 'Ошибка загрузки');
      } finally {
        if (!cancelled) setLoading(false);
      }
    })();

    return () => {
      cancelled = true;
    };
  }, [categoryId]);

  return (
    <div className="app-wrapper plastic-page" style={{ zoom: scale }}>

      <main className="plastic-container">
        <nav className="plastic-breadcrumbs" aria-label="Хлебные крошки">
          <Link to="/">Арт-Медика</Link>
          <span className="plastic-breadcrumbs-sep" aria-hidden="true">&gt;</span>
          <Link to="/services">Услуги</Link>
          <span className="plastic-breadcrumbs-sep" aria-hidden="true">&gt;</span>
          <span aria-current="page">{categoryTitle || '…'}</span>
        </nav>

        <div className="plastic-header">
          <h1 className="plastic-title">
            {categoryTitle || '…'}
            <svg className="plastic-title-arrow" width="22" height="21" viewBox="0 0 22 21" fill="none" stroke="currentColor" strokeWidth="2.6" aria-hidden="true">
              <path d="M0 1H20V20" />
              <path d="M2.2 18.8L19.6 1.4" />
            </svg>
          </h1>
          <p className="plastic-intro">
            Подбираем для вас самые актуальные{' '}
            <br className="plastic-intro-br" />
            направления и процедуры из нашей практики — с заботой о вас.
          </p>
        </div>

        {loading && <p className="plastic-status">Загрузка…</p>}
        {error && !loading && <p className="plastic-status plastic-status-error">{error}</p>}

        {!loading && !error && (
          <ul className="plastic-list">
            {subcategories.map((card) => (
              <li key={card.id} className="plastic-card">
                <div className="plastic-card-main">
                  <h2 className="plastic-card-title">{card.title}</h2>
                  <Link
                    className="plastic-card-btn"
                    to={`/services/${categoryId}/${card.id}`}
                    aria-label={`Подробнее: ${card.title}`}
                  >
                    Подробнее
                  </Link>
                </div>
                {card.description && <p className="plastic-card-text">{card.description}</p>}
                <div className="plastic-card-media">
                  <img src={mediaUrl(card.imgUrl?.url)} alt="" loading="lazy" />
                </div>
              </li>
            ))}
          </ul>
        )}
      </main>
    </div>
  );
}
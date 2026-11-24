import { useEffect, useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import { useDesignScale } from '../hooks/useDesignScale';
import { apiGet } from '../api';
import './SubcategoryServices.css';

interface MediaFileDto {
  id: string;
  name: string;
  url: string;
}

interface ServiceDto {
  id: string;
  displayTitle: string;
  technicalTitle: string;
  duration: number;
  price: number;
  description?: string | null;
  types?: string | null;
}

interface SubcategoryWithServicesDto {
  id: string;
  title: string;
  description?: string | null;
  imgUrl: MediaFileDto;
  services: ServiceDto[];
}

export default function SubcategoryServicesPage() {
  const { categoryId, subcategoryId } = useParams<{
    categoryId: string;
    subcategoryId: string;
  }>();
  const scale = useDesignScale();

  const [sub, setSub] = useState<SubcategoryWithServicesDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!categoryId || !subcategoryId) return;
    let cancelled = false;
    setLoading(true);
    setError(null);
    setSub(null);

    (async () => {
      try {
        const subs = await apiGet<SubcategoryWithServicesDto[]>(
          `/public/service-subcategories/with-services?categoryId=${categoryId}`,
        );
        const found = subs.find((s) => s.id === subcategoryId);
        if (!found) {
          if (!cancelled) setError('Подкатегория не найдена');
          return;
        }
        if (!cancelled) setSub(found);
      } catch (e) {
        if (!cancelled) setError(e instanceof Error ? e.message : 'Ошибка загрузки');
      } finally {
        if (!cancelled) setLoading(false);
      }
    })();

    return () => {
      cancelled = true;
    };
  }, [categoryId, subcategoryId]);

  return (
    <div className="app-wrapper subcat-page" style={{ zoom: scale }}>
      <main className="subcat-container">
        <nav className="subcat-breadcrumbs" aria-label="Хлебные крошки">
          <Link to="/">Арт-Медика</Link>
          <span className="subcat-sep" aria-hidden="true">&gt;</span>
          <Link to="/services">Услуги</Link>
          <span className="subcat-sep" aria-hidden="true">&gt;</span>
          <Link to={`/services/${categoryId}`}>{sub?.title ?? '…'}</Link>
        </nav>

        <h1 className="subcat-title">
          {sub?.title ?? '…'}
          <svg width="22" height="21" viewBox="0 0 22 21" fill="none" stroke="currentColor" strokeWidth="2.6" aria-hidden="true">
            <path d="M0 1H20V20" />
            <path d="M2.2 18.8L19.6 1.4" />
          </svg>
        </h1>

        {sub?.description && <p className="subcat-intro">{sub.description}</p>}

        {loading && <p className="subcat-status">Загрузка…</p>}
        {error && !loading && <p className="subcat-status subcat-status-error">{error}</p>}

        {!loading && !error && sub && (
          sub.services.length === 0 ? (
            <p className="subcat-status">В этой подкатегории пока нет услуг.</p>
          ) : (
            <ul className="subcat-grid">
              {sub.services.map((service) => (
                <li key={service.id} className="subcat-item">
                  <Link
                    to={`/services/${categoryId}/${subcategoryId}/${service.id}`}
                    className="subcat-card"
                  >
                    <span className="subcat-card-title">{service.displayTitle}</span>
                  </Link>
                </li>
              ))}
            </ul>
          )
        )}
      </main>
    </div>
  );
}
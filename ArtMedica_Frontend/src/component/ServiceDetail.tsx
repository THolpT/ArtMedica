import { useEffect, useMemo, useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import { useDesignScale } from '../hooks/useDesignScale';
import { apiGet, mediaUrl } from '../api';
import './ServiceDetail.css';

interface MediaFileDto {
  id: string;
  name: string;
  url: string;
}

interface ServiceDto {
  id: string;
  nomenclatureCode: string;
  misCode: string;
  duration: number;
  price: number;
  displayTitle: string;
  technicalTitle: string;
  description?: string | null;
  preparation?: string | null;
  types?: string | null;
  subcategoryId: string;
}

interface SubcategoryWithServicesDto {
  id: string;
  title: string;
  description?: string | null;
  imgUrl: MediaFileDto;
  services: ServiceDto[];
}

interface OperationResultDto {
  id: string;
  imgUrl: MediaFileDto;
  operationId: string;
  operationTitle: string;
  specialistId: string;
  specialistName: string;
}

interface SpecialistDetailDto {
  id: string;
  fullName: string;
  department?: string | null;
  description?: string | null;
  imgUrl: MediaFileDto;
}

export default function ServiceDetailPage() {
  const { categoryId, subcategoryId, serviceId } = useParams<{
    categoryId: string;
    subcategoryId: string;
    serviceId?: string;
  }>();
  const scale = useDesignScale();

  const [subcategory, setSubcategory] = useState<SubcategoryWithServicesDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [results, setResults] = useState<OperationResultDto[]>([]);
  const [doctors, setDoctors] = useState<SpecialistDetailDto[]>([]);

  // ---- 1. Загрузка подкатегории с услугами ----
  useEffect(() => {
    if (!categoryId || !subcategoryId) return;
    let cancelled = false;
    setLoading(true);
    setError(null);
    setSubcategory(null);

    (async () => {
      try {
        const subs = await apiGet<SubcategoryWithServicesDto[]>(
          `/public/service-subcategories/with-services?categoryId=${categoryId}`,
        );
        const sub = subs.find((s) => s.id === subcategoryId);
        if (!sub) {
          if (!cancelled) setError('Подкатегория не найдена');
          return;
        }
        if (!cancelled) setSubcategory(sub);
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

  // ---- 2. Активная услуга ----
  const activeService = useMemo<ServiceDto | null>(() => {
    if (!subcategory || subcategory.services.length === 0) return null;
    if (serviceId) {
      return subcategory.services.find((s) => s.id === serviceId) ?? subcategory.services[0];
    }
    return subcategory.services[0];
  }, [subcategory, serviceId]);

  // ---- 3. Результаты операции + врачи ----
  useEffect(() => {
    if (!activeService) {
      setResults([]);
      setDoctors([]);
      return;
    }
    let cancelled = false;

    (async () => {
      try {
        const res = await apiGet<OperationResultDto[]>(
          `/public/services/${activeService.id}/operation-results`,
        );
        if (cancelled) return;
        setResults(res);

        const uniqueIds = Array.from(new Set(res.map((r) => r.specialistId)));
        const docs = await Promise.all(
          uniqueIds.map((id) =>
            apiGet<SpecialistDetailDto>(`/public/specialists/${id}`).catch(() => null),
          ),
        );
        if (!cancelled) {
          setDoctors(docs.filter((d): d is SpecialistDetailDto => d !== null));
        }
      } catch {
        if (!cancelled) {
          setResults([]);
          setDoctors([]);
        }
      }
    })();

    return () => {
      cancelled = true;
    };
  }, [activeService]);

  // ---- Рендер ----
  if (loading) {
    return (
      <div className="app-wrapper svc-page" style={{ zoom: scale }}>
        <main><p className="svc-status">Загрузка…</p></main>
      </div>
    );
  }

  if (error || !subcategory) {
    return (
      <div className="app-wrapper svc-page" style={{ zoom: scale }}>
        <main><p className="svc-status svc-status-error">{error ?? 'Не найдено'}</p></main>
      </div>
    );
  }

  return (
    <div className="app-wrapper svc-page" style={{ zoom: scale }}>
      <main>
        <nav className="svc-breadcrumbs" aria-label="Хлебные крошки">
          <Link to="/">Арт-Медика</Link>
          <span className="svc-breadcrumbs-sep" aria-hidden="true">&gt;</span>
          <Link to="/services">Услуги</Link>
          <span className="svc-breadcrumbs-sep" aria-hidden="true">&gt;</span>
          <Link to={`/services/${categoryId}`}>{subcategory.title}</Link>
          {activeService && (
            <>
              <span className="svc-breadcrumbs-sep" aria-hidden="true">&gt;</span>
              <span aria-current="page">{activeService.displayTitle}</span>
            </>
          )}
        </nav>

        <h1 className="svc-title">
          {activeService?.displayTitle ?? subcategory.title}
          <svg className="svc-title-arrow" width="22" height="21" viewBox="0 0 22 21" fill="none" stroke="currentColor" strokeWidth="2.6" aria-hidden="true">
            <path d="M0 1H20V20" />
            <path d="M2.2 18.8L19.6 1.4" />
          </svg>
        </h1>

        {subcategory.services.length > 0 && (
          <nav className="svc-tabs" aria-label={`Процедуры раздела «${subcategory.title}»`}>
            {subcategory.services.map((item) => (
              <Link
                key={item.id}
                className="svc-tab"
                to={`/services/${categoryId}/${subcategoryId}/${item.id}`}
                aria-current={item.id === activeService?.id ? 'page' : undefined}
              >
                {item.displayTitle}
              </Link>
            ))}
          </nav>
        )}

        {!activeService && (
          <p className="svc-status">В этой подкатегории пока нет услуг.</p>
        )}

        {activeService && (
          <>
            {activeService.description && (
              <section className="svc-section svc-section-intro">
                <h2 className="svc-h2">{activeService.displayTitle}</h2>
                <p className="svc-text">{activeService.description}</p>
              </section>
            )}

            {activeService.types && (
              <section className="svc-section svc-section-types">
                <h2 className="svc-h2">Виды процедуры</h2>
                <p className="svc-text">{activeService.types}</p>
              </section>
            )}

            {activeService.preparation && (
              <section className="svc-section svc-section-preparation">
                <h2 className="svc-h2">Подготовка к операции</h2>
                <p className="svc-text">{activeService.preparation}</p>
              </section>
            )}

            {results.length > 0 && (
              <section className="svc-section svc-section-results">
                <h2 className="svc-h2">Результаты процедуры</h2>
                <div className="svc-slider">
                  {results.map((r) => (
                    <img
                      key={r.id}
                      className="svc-pair"
                      src={mediaUrl(r.imgUrl?.url)}
                      alt={r.operationTitle}
                      width={415}
                      height={622}
                      loading="lazy"
                    />
                  ))}
                </div>
              </section>
            )}

            {doctors.length > 0 && (
              <section className="svc-section svc-section-doctors">
                <h2 className="svc-h2">Врачи, проводящие процедуру</h2>
                {doctors.map((doctor) => (
                  <article key={doctor.id} className="svc-doctor">
                    <img
                      className="svc-doctor-photo"
                      src={mediaUrl(doctor.imgUrl?.url)}
                      alt={doctor.fullName}
                      width={156}
                      height={402}
                      loading="lazy"
                    />
                    <div className="svc-doctor-info">
                      <h3 className="svc-doctor-name">
                        <Link to={`/specialists/${doctor.id}`}>{doctor.fullName}</Link>
                      </h3>
                      {doctor.description && (
                        <p className="svc-doctor-text">{doctor.description}</p>
                      )}
                    </div>
                  </article>
                ))}
              </section>
            )}
          </>
        )}
      </main>
    </div>
  );
}
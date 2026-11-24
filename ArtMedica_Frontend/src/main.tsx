import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter, Routes, Route, Outlet } from 'react-router-dom';

import { Header } from './component/header.tsx';
import { Footer } from './component/footer.tsx';

import { Index } from './component/index.tsx';
import { Contacts } from './component/contacts.tsx';
import { Services } from './component/services.tsx';
import { Projects } from './component/projects.tsx';
import ProjectDetail from './component/crasota_v_tebe.tsx';
import { Stocks } from './component/stocks.tsx';
import { SpecialStocks } from './component/special_stocks.tsx';
import Prices from './component/prices.tsx';

import AboutPage from './component/About.tsx';
import FAQPage from './component/FAQ.tsx';
import SpecialistsPage from './component/Specialists.tsx';
import DoctorDetailPage from './component/DoctorDetail.tsx';
import NewsPage from './component/News.tsx';
import NewsDetailPage from './component/NewsDetail.tsx';

import PlasticSurgeryPage from './component/PlasticSurgery.tsx';
import ServiceDetailPage from './component/ServiceDetail.tsx';
import SubcategoryServicesPage from './component/SubcategoriesPage.tsx';

function DefaultLayout() {
  return (
    <>
      <Header />
      <Outlet />
      <Footer />
    </>
  );
}

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <BrowserRouter>
      <Routes>
        <Route element={<DefaultLayout />}>
          <Route path="/" element={<Index />} />
          <Route path="/contacts" element={<Contacts />} />
          <Route path="/prices" element={<Prices />} />

          <Route path="/projects" element={<Projects />} />
          <Route path="/projects/:id" element={<ProjectDetail />} />

          <Route path="/stocks" element={<Stocks />} />
          <Route path="/stocks/:id" element={<SpecialStocks />} />

          <Route path="/about" element={<AboutPage />} />
          <Route path="/faq" element={<FAQPage />} />

          <Route path="/specialists" element={<SpecialistsPage />} />
          <Route path="/specialists/:id" element={<DoctorDetailPage />} />

          <Route path="/news" element={<NewsPage />} />
          <Route path="/news/:id" element={<NewsDetailPage />} />
          <Route path="/services" element={<Services />} />
          <Route path="/services/:categoryId" element={<PlasticSurgeryPage />} />
          <Route path="/services/:categoryId/:subcategoryId" element={<SubcategoryServicesPage />} />
          <Route path="/services/:categoryId/:subcategoryId/:serviceId?" element={<ServiceDetailPage />} />
        </Route>
        <Route path="*" element={<div style={{ padding: 40 }}>Страница не найдена</div>} />
      </Routes>
    </BrowserRouter>
  </StrictMode>,
);
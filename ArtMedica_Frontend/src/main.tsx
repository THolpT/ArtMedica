import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route } from 'react-router-dom'

import { Header } from './component/header.tsx'
import { Footer } from './component/footer.tsx'

import { Index } from './component/index.tsx'
import { Contacts } from './component/contacts.tsx'
import { Services } from './component/services.tsx'
import { Projects } from './component/projects.tsx'
import ProjectDetail from './component/crasota_v_tebe.tsx'
import { Stocks } from './component/stocks.tsx'
import { SpecialStocks } from './component/special_stocks.tsx'
import Prices from './component/prices.tsx'

import AboutPage from './component/About.tsx'
import FAQPage from './component/FAQ.tsx'
import SpecialistsPage from './component/Specialists.tsx'
import DoctorDetailPage from './component/DoctorDetail.tsx'
import NewsPage from './component/News.tsx'
import NewsDetailPage from './component/NewsDetail.tsx'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <BrowserRouter>
      <Header />
      <Routes>
        {/* Главная и разделы */}
        <Route path="/" element={<Index />} />
        <Route path="/contacts" element={<Contacts />} />
        <Route path="/prices" element={<Prices />} />

        {/* Услуги */}
        <Route path="/services" element={<Services />} />
        <Route path="/services/:id" />

        {/* Проекты */}
        <Route path="/projects" element={<Projects />} />
        <Route path="/projects/:id" element={<ProjectDetail />} />

        {/* Акции */}
        <Route path="/stocks" element={<Stocks />} />
        <Route path="/stocks/:id" element={<SpecialStocks />} />

        {/* О клинике / FAQ */}
        <Route path="/about" element={<AboutPage />} />
        <Route path="/faq" element={<FAQPage />} />

        {/* Специалисты */}
        <Route path="/specialists" element={<SpecialistsPage />} />
        <Route path="/specialists/:id" element={<DoctorDetailPage />} />

        {/* Новости */}
        <Route path="/news" element={<NewsPage />} />
        <Route path="/news/:id" element={<NewsDetailPage />} />
      </Routes>
      <Footer />
    </BrowserRouter>
  </StrictMode>,
)
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import "./FAQ.css";
import { apiGet } from "../api";

interface ApiFaq {
  id: string;
  question: string;
  answer: string;
}

export default function FAQPage() {
  const [faqs, setFaqs] = useState<ApiFaq[]>([]);
  const [openId, setOpenId] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    apiGet<ApiFaq[]>("/api/public/faqs")
      .then((data) => {
        setFaqs(data);
        // По умолчанию раскрываем первый вопрос
        if (data.length > 0) setOpenId(data[0].id);
      })
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false));
  }, []);

  const toggleFAQ = (id: string) => {
    setOpenId((prev) => (prev === id ? null : id));
  };

  return (
    <main className="faq-container">
      <div className="breadcrumbs">
        <Link to="/">Арт-Медика</Link>
        <span>/</span>
        <span className="current">Вопросы и ответы</span>
      </div>

      <div className="faq-header">
        <h1 className="faq-title">Вопросы и ответы ↗</h1>
        <p className="faq-intro-text">
          Прозрачность медицинской помощи начинается с понятных ответов на сложные
          вопросы. Мы собрали самые частые обращения пациентов и детальные
          разъяснения наших врачей.
        </p>
      </div>

      {loading && <p>Загрузка…</p>}
      {error && <p>Не удалось загрузить вопросы: {error}</p>}
      {!loading && !error && faqs.length === 0 && (
        <p>Пока нет вопросов и ответов.</p>
      )}

      <div className="faq-list">
        {faqs.map((item) => {
          const isOpen = openId === item.id;
          return (
            <div
              key={item.id}
              className={`faq-item ${isOpen ? "faq-item--open" : ""}`}
            >
              <div
                className="faq-question-row"
                onClick={() => toggleFAQ(item.id)}
              >
                <h2 className="faq-question-text">{item.question}</h2>
                <span className="faq-arrow-btn">{isOpen ? "↑" : "↓"}</span>
              </div>

              <div
                className={`faq-answer-wrapper ${
                  isOpen ? "faq-answer-wrapper--open" : ""
                }`}
              >
                <div className="faq-answer-content">
                  <p>{item.answer}</p>
                </div>
              </div>
            </div>
          );
        })}
      </div>
    </main>
  );
}
import "../style/HF_style.css";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Logo from "../image/logo.svg";
import Max from "../image/max.svg";
import VK from "../image/vk.svg";
import Telegram from "../image/telegram.svg";
import Metka from "../image/metka.svg";
import Phone_mini from "../image/phone_mini.svg";
import Mail from "../image/mail.svg";
import Time from "../image/time.svg";
import { apiGet } from "../api";

interface ApiMediaFile {
  id: string;
  name: string;
  type: string;
  url: string;
}

interface ApiServiceCategory {
  id: string;
  title: string;
  imgUrl: ApiMediaFile;
}

export function Footer() {
  const [serviceCategories, setServiceCategories] = useState<ApiServiceCategory[]>([]);

  useEffect(() => {
    apiGet<ApiServiceCategory[]>("/api/public/service-categories")
      .then(setServiceCategories)
      .catch(console.error);
  }, []);

  return (
    <footer>
      <div className="footer_column">
        <div className="block_footer1">
          <Link to="/">
            <img src={Logo} alt="Арт-Медика" />
          </Link>

          <div className="box_mess">
            <a
              href="https://max.ru/"
              target="_blank"
              rel="noreferrer"
              aria-label="MAX"
            >
              <img src={Max} alt="" />
            </a>
            <a
              href="https://vk.com/"
              target="_blank"
              rel="noreferrer"
              aria-label="ВКонтакте"
            >
              <img src={VK} alt="" />
            </a>
            <a
              href="https://t.me/"
              target="_blank"
              rel="noreferrer"
              aria-label="Telegram"
            >
              <img src={Telegram} alt="" />
            </a>
          </div>

          <a href="#">👁 Версия для слабовидящих</a>

          <p>
            © Медико-косметологический центр Арт-медика 2026 <br />
            Категория 18+
          </p>
        </div>

        <div className="block_footer2">
          <p>Клиника</p>
          <ul>
            <li><Link to="/about">О клинике</Link></li>
            <li><Link to="/specialists">Специалисты</Link></li>
            <li><Link to="/services">Услуги</Link></li>
            <li><Link to="/projects">Проекты</Link></li>
            <li><Link to="/stocks">Акции</Link></li>
            <li><Link to="/news">Новости</Link></li>
            <li><Link to="/prices">Цены</Link></li>
            <li><Link to="/contacts">Контакты</Link></li>
            <li><Link to="/faq">Вопросы и ответы</Link></li>
          </ul>
        </div>

        <div className="block_footer3">
          <p>Услуги</p>
          <ul>
            {serviceCategories.length > 0 ? (
              serviceCategories.map((cat) => (
                <li key={cat.id}>
                  <Link to={`/services/${cat.id}`}>{cat.title}</Link>
                </li>
              ))
            ) : (
              <>
                <li><Link to="/services">Косметология</Link></li>
                <li><Link to="/services">Дерматология</Link></li>
                <li><Link to="/services">Оториноларингология</Link></li>
                <li><Link to="/services">Лор-хирургия</Link></li>
                <li><Link to="/services">Пластическая хирургия</Link></li>
                <li><Link to="/services">Эстетическая гинекология</Link></li>
              </>
            )}
          </ul>
        </div>

        <div className="block_footer4">
          <Link to="/contacts" className="button">
            Записаться
          </Link>

          <p className="footer_info">
            <img src={Metka} alt="" />
            Челябинск, пр. Ленина 12a
          </p>
          <p className="footer_info">
            <img src={Phone_mini} alt="" />
            <a href="tel:+73517751918">+7 (351) 775-19-18</a>
          </p>
          <p className="footer_info">
            <img src={Mail} alt="" />
            <a href="mailto:marketing.art-medica@mail.ru">
              marketing.art-medica@mail.ru
            </a>
          </p>
          <p className="footer_info">
            <img src={Time} alt="" />
            09:00 до 20:00
            <br />
            понедельник — суббота
          </p>
        </div>
      </div>

      <p className="bottom_text">
        ИМЕЮТСЯ ПРОТИВОПОКАЗАНИЯ. НЕОБХОДИМА КОНСУЛЬТАЦИЯ СПЕЦИАЛИСТА
      </p>
    </footer>
  );
}
import "../style/HF_style.css"
import Logo from "../image/logo.svg"
import Max from "../image/max.svg"
import VK from "../image/vk.svg"
import Telegram from "../image/telegram.svg"
import Phone from "../image/phone.svg"
import Metka from "../image/metka.svg"
import Phone_mini from "../image/phone_mini.svg"
import Mail from "../image/mail.svg"
import Time from "../image/time.svg"

export function Footer() {

  return (
    <>
      <footer>
        <div className="footer_column">
          <div className="block_footer1">
              <img src={Logo} alt="" />
              <div className="box_mess">
                  <img src={Max} alt="" />
                  <img src={VK} alt="" />
                  <img src={Telegram} alt="" />
              </div>
              <a href="#">👁 Версия для слабовидящих</a>
              {/* <button>👁 Версия для слабовидящих</button> */} 
              <p>© Медико-косметологический центр Арт-медика 2026 <br />Категория 18+</p>
          </div>
          <div className="block_footer2">
              <p>Клиника</p>
              <ul>
                <li><a href="#">О клинике</a></li>
                <li><a href="#">Специалисты</a></li>
                <li><a href="#">Услуги</a></li>
                <li><a href="#">Проекты</a></li>
                <li><a href="#">Акции</a></li>
                <li><a href="#">Новости</a></li>
                <li><a href="#">Цены</a></li>
                <li><a href="#">Контакты</a></li>
                <li><a href="#">Документы</a></li>
              </ul>
          </div>
          <div className="block_footer3">
            <p>Услуги</p>
            <ul>
              <li><a href="#">Пластическая хирургия</a></li>
              <li><a href="#">Косметология</a></li>
              <li><a href="#">Топ_продукты</a></li>
              <li><a href="#">Дерматология</a></li>
              <li><a href="#">Оториноларингология</a></li>
              <li><a href="#">Лор-хирургия</a></li>
              <li><a href="#">Неврология и рефлексотерапия</a></li>
              <li><a href="#">Эстетическая гинекология</a></li>
              <li><a href="#">Терапевтический приём</a></li>
              <li><a href="#">Массаж</a></li>
            </ul>
          </div>
          <div className="block_footer4">
            <div className="">
            <button className="button">Записаться</button>
            <button className="but_phone"><img src={Phone} alt="" /></button>
            </div>
            <p className="footer_info"><img src={Metka} alt="" />Челябинск, пр. Ленина 12a</p>
            <p className="footer_info"><img src={Phone_mini} alt="" />+7 (351) 775-19-18</p>
            <p className="footer_info"><img src={Mail} alt="" />marketing.art-medica@mail.ru</p>
            <p className="footer_info"><img src={Time} alt="" />09:00 до 20:00<br />понедельник - суббота</p>
          </div>
        </div>
        <p className="bottom_text">ИМЕЮТСЯ ПРОТИВОПОКАЗАНИЯ. НЕОБХОДИМА КОНСУЛЬТАЦИЯ СПЕЦИАЛИСТА</p>
      </footer>
    </>
  )
}
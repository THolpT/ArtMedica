import Max from "../image/max.svg"
import VK from "../image/vk.svg"
import Telegram from "../image/telegram.svg"

import '../style/contacts_style.css';

export const Contacts = () => {
  return (
    <main className="contacts-page">
      <div className="container">
        
        <nav className="breadcrumbs">
          <span>Арт-Медика</span>
          <span className="separator">&gt;</span>
          <span className="current">Контакты</span>
        </nav>

        <header className="page-header">
          <div className="title-wrapper">
            <h1 className="page-title">Контакты ↗</h1>
          </div>
          <div className="header-description">
            <p>
              Пишите нам, когда вам удобно — <br />
              даже в выходные или ночью. Ваш запрос будет
              обработан в ближайшее рабочее время
            </p>
          </div>
        </header>

        <section className="map-section">
          <iframe
            src="https://yandex.ru/map-widget/v1/?ll=61.448963%2C55.161074&mode=search&oid=1019606836&ol=biz&sctx=ZAAAAAgBEAAaKAoSCd9vtOOGs05AEUDdQIF3lEtAEhIJvR3htODF7T8RDeNuEK0V1T8iBgABAgMEBSgKOABA2VdIAWI6cmVhcnI9c2NoZW1lX0xvY2FsL0dlb3VwcGVyL0FkdmVydHMvQ3VzdG9tTWF4YWR2L0VuYWJsZWQ9MWI6cmVhcnI9c2NoZW1lX0xvY2FsL0dlb3VwcGVyL0FkdmVydHMvQ3VzdG9tTWF4YWR2L01heGFkdj0xNWJEcmVhcnI9c2NoZW1lX0xvY2FsL0dlb3VwcGVyL0FkdmVydHMvQ3VzdG9tTWF4YWR2L1JlZ2lvbklkcz1bMSwxMDE3NF1iQHJlYXJyPXNjaGVtZV9Mb2NhbC9HZW91cHBlci9BZHZlcnRzL01heGFkdlRvcE1peC9NYXhhZHZGb3JNaXg9MTBqAnJ1nQHNzMw9oAEAqAEAvQGSc%2FTZwgEFtO6X5gOCAhPQkNGA0YIt0LzQtdC00LjQutCwigIAkgIAmgIMZGVza3RvcC1tYXBzqgKiATI0ODczNDE1MDc0LDYwMDM3MTYsMjA3ODc5NjM4MzQzLDUyOTUzMTE2MjUzLDIyMzYyODU2OTA1OCw3Mzc3NTUxOTQ3MiwyNTkyNzU5ODI0OSw4MDU1NTA3OTgsODA1NTUwOTY1LDQ3MDQ2MjUxODExLDc1NTEzNzAwODAyLDM4NzQ3ODE2MDAsMjAyMDMwMjA0MTQxLDIzNjE1ODI5MTc0MLACAQ%3D%3D&sll=61.448963%2C55.161074&sspn=0.014538%2C0.005147&text=%D0%90%D1%80%D1%82-%D0%BC%D0%B5%D0%B4%D0%B8%D0%BA%D0%B0&z=17"
            width="100%"
            height="400"
            frameBorder="0"
            allowFullScreen={true}
            title="Карта Арт-Медика"
            style={{ display: 'block', borderRadius: '12px' }}
          />
        </section>

        <section className="info-grid">
          
          <div className="info-card">
            <h2 className="info-title">Адрес</h2>
            <p className="info-text">
              454007, Челябинская область,<br />
              г. Челябинск, проспект Ленина 12 "А"<br />
              (рядом Театр ЧТЗ)
            </p>
          </div>

          <div className="info-card">
            <h2 className="info-title">Телефоны</h2>
            <p className="info-text">
              +7 (351) 775-19-18 - запись на прием;<br />
              +7 (351) 775-19-68 - телефон горячей линии
            </p>
          </div>

          <div className="info-card">
            <h2 className="info-title">E-mail</h2>
            <p className="info-text">
              sender@artmdc.ru - отправка анализов<br />
              marketing.art-medica@mail.ru - прием обращений граждан
            </p>
          </div>

          <div className="info-card">
            <h2 className="info-title">Сообщения</h2>
            <p className="info-text">
              +7 (912) 326-34-60 - Telegram, MAX
            </p>
          </div>

          <div className="info-card">
            <h2 className="info-title">Режим работы</h2>
            <p className="info-text">
              ПН-СБ с 9:00 до 20:00
            </p>
          </div>
        </section>

        <div className="social-links">
          <a href="#" className="social-icon max" aria-label="Одноклассники">
            <img src={Max} alt="" />
        </a>
          <a href="#" className="social-icon vk" aria-label="ВКонтакте">
            <img src={VK} alt="" />
          </a>
          <a href="#" className="social-icon tg" aria-label="Telegram">
            <img src={Telegram} alt="" />
          </a>
        </div>

      </div>
    </main>
  );
};


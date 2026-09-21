import "../style/HF_style.css"
import Logo from "../image/logo.svg"
import Phone from "../image/phone.svg"

export function Header() {

  return (
    <>
    <header>
      <img src={Logo} alt="" />
      <ul className="menu">
        <li><a href="">О клинике</a></li>
        <li><a href="">Услуги</a></li>
        <li><a href="">Специалисты</a></li>
        <li><a href="">Акции</a></li>
        <li><a href="">Документы</a></li>
        <li><a href="">Контакты</a></li>
      </ul>
      <div className="right_header">
        <div className="address">
            <p>Челябинск, пр.Ленина 12а</p><br />
            <p>ПН-СБ с 9:00 до 20:00</p>
        </div>
        <button className="but_phone"><img src={Phone} alt="" /></button>
        <button className="button">Записаться</button>
      </div>
    </header>
    </>
  )
}
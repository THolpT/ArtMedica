import "../style/HF_style.css"
import Logo from "../image/logo.svg"
import { Link, NavLink } from "react-router-dom"

export function Header() {
  return (
    <header>
      <Link to="/">
        <img className="logo" src={Logo} alt="Арт-Медика" />
      </Link>
      <ul className="menu">
        <li><NavLink to="/about">О клинике</NavLink></li>
        <li><NavLink to="/services">Услуги</NavLink></li>
        <li><NavLink to="/specialists">Специалисты</NavLink></li>
        <li><NavLink to="/stocks">Акции</NavLink></li>
        <li><NavLink to="/documents">Документы</NavLink></li>
        <li><NavLink to="/contacts">Контакты</NavLink></li>
      </ul>
      <div className="right_header">
        <div className="address">
          <b>Челябинск, пр.Ленина 12а</b><br />
          <p>ПН-СБ с 9:00 до 20:00</p>
        </div>
        <Link to="/contacts" className="button">Записаться</Link>
      </div>
    </header>
  )
}
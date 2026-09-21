import "../style/index_style.css"
import Phone from "../image/phone.svg"
export function Index() {

  return (
    <>
    <div className="Block1">
        <div className="Block1_text">
            <p className="Block1_title">Ваше здоровье — наша забота</p>
            <p className="Block1_title2">Медико-косметологический центр с современным оборудованием и опытными специалистами</p>
        </div>
        <div className="Block1_bottom">
            <div className="Block1_boxs">
                <div className="adventeg">
                    <p className="Block1_title">20</p>
                    <p className="m0 t24">лет опыта работы<br />в сфере медицины</p>
                </div>
                <div className="adventeg">
                    <p className="Block1_title">20</p>
                    <p className="m0 t24">видов медицинских<br />направлений деятельности</p>
                </div>
                <div className="adventeg">
                    <p className="Block1_title">40+</p>
                    <p className="m0 t24">квалифицированных<br />специалистов</p>
                </div>
            </div>
                <img className="Block1_phone" src={Phone} alt="" />
        </div>
    </div>
    <div className="Block2">
        
    </div>
    </>
  )
}
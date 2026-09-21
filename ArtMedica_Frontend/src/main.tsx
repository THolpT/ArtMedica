import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { Header } from './component/header.tsx'
import { Footer } from './component/footer.tsx'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Header/>
    <Footer/>
  </StrictMode>,
)

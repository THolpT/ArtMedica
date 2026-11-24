import { useEffect, useState } from 'react';

const DESIGN_WIDTH = 1920; // width of the mockup
const MIN_WIDTH = 1200; // below this the page switches to its stacked (mobile) layout instead of scaling

function getScale(): number {
  const width = document.documentElement.clientWidth;
  return width >= MIN_WIDTH && width < DESIGN_WIDTH ? width / DESIGN_WIDTH : 1;
}

/**
 * Keeps the page a 1:1 copy of the 1920px mockup on smaller desktop screens:
 * between 1200px and 1920px the whole page is scaled proportionally instead of being re-flowed.
 * Use it as: <div style={{ zoom: useDesignScale() }}>
 */
export function useDesignScale(): number {
  const [scale, setScale] = useState(getScale);

  useEffect(() => {
    const onResize = () => setScale(getScale());
    window.addEventListener('resize', onResize);
    return () => window.removeEventListener('resize', onResize);
  }, []);

  return scale;
}

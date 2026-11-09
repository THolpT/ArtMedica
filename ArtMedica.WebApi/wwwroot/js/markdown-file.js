// wwwroot/js/admin/markdown-field.js
document.addEventListener('DOMContentLoaded', () => {
  document.querySelectorAll('[data-markdown-field]').forEach(root => {
    const input   = root.querySelector('[data-markdown-input]');
    const preview = root.querySelector('[data-markdown-preview]');
    const buttons = root.querySelectorAll('[data-markdown-action]');
    if (!input) return;

    const wrap = (before, after = before, placeholder = '') => {
      const s = input.selectionStart, e = input.selectionEnd;
      const sel = input.value.slice(s, e) || placeholder;
      input.setRangeText(before + sel + after, s, e, 'end');
      input.focus();
    };

    const actions = {
      bold:     () => wrap('**', '**', 'text'),
      italic:   () => wrap('*',  '*',  'text'),
      heading:  () => wrap('## ', '', 'Heading'),
      link:     () => wrap('[', '](https://)', 'link'),
      code:     () => wrap('`', '`', 'code'),
      list:     () => wrap('- ', '', 'item'),
      preview:  () => {
        // здесь можно подключить любую библиотеку — marked, markdown-it и т.п.
        const html = window.marked
          ? window.marked.parse(input.value)
          : `<pre>${input.value.replace(/[<>&]/g, c => ({'<':'&lt;','>':'&gt;','&':'&amp;'}[c]))}</pre>`;
        preview.innerHTML = html;
        preview.classList.toggle('d-none');
      }
    };

    buttons.forEach(btn => {
      btn.addEventListener('click', () => {
        const action = actions[btn.dataset.markdownAction];
        if (action) action();
      });
    });
  });
});
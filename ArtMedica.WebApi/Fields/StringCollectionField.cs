using ArtMedica.WebApi.Abstractions;
using Microsoft.AspNetCore.Html;
using System.Collections;
using System.Text;

public sealed class StringCollectionField : FieldBase
{
    public override Task<IHtmlContent> RenderAsync(AdminFieldRenderContext context)
    {
        var def = context.Definition;
        var name = Encode(def.Name);
        var label = Encode(def.Label);
        var placeholder = Encode(def.Placeholder);
        
        var requiredAttr = def.Required ? "required" : "";
        var readonlyAttr = def.ReadOnly ? "readonly" : "";
        var disabledAttr = def.ReadOnly ? "disabled" : "";

        // Извлекаем текущие значения (поддерживаем string[] и List<string>)
        var values = new List<string>();
        if (context.Value is IEnumerable<string> strList)
            values.AddRange(strList);
        else if (context.Value is IEnumerable list && context.Value is not string)
            foreach (var item in list) if (item != null) values.Add(item.ToString()!);

        if (values.Count == 0) values.Add("");

        var errorHtml = context.Error is null ? "" : $"<div class=\"text-danger\">{Encode(context.Error)}</div>";
        var sb = new StringBuilder();

        sb.Append($$"""
        <div class="mb-3">
            <label class="form-label">{{label}}</label>
            <div id="{{name}}_container" class="string-collection-container">
        """);

        foreach (var value in values)
        {
            sb.Append($$"""
                <div class="input-group mb-2 string-collection-item">
                    <input type="text" class="form-control" name="{{name}}" value="{{Encode(value)}}" placeholder="{{placeholder}}" {{requiredAttr}} {{readonlyAttr}} />
                    <button type="button" class="btn btn-outline-danger remove-btn" {{disabledAttr}}>✕</button>
                </div>
            """);
        }

        sb.Append($$"""
            </div>
            <button type="button" class="btn btn-outline-primary btn-sm" id="{{name}}_add_btn" {{disabledAttr}}>+ Добавить</button>
            {{errorHtml}}
        </div>
        <script>
            (function() {
                var container = document.getElementById('{{name}}_container');
                var addBtn = document.getElementById('{{name}}_add_btn');
                if (!container || !addBtn || container.dataset.init) return;

                container.addEventListener('click', function(e) {
                    var btn = e.target.closest('.remove-btn');
                    if (btn) {
                        var item = btn.closest('.string-collection-item');
                        if (item) {
                            if (container.querySelectorAll('.string-collection-item').length === 1) {
                                item.querySelector('input').value = '';
                            } else {
                                item.remove();
                            }
                        }
                    }
                });

                addBtn.addEventListener('click', function() {
                    var newItem = document.createElement('div');
                    newItem.className = 'input-group mb-2 string-collection-item';
                    // ВАЖНО: name="{{name}}" без индексов, чтобы форма отправила их массивом
                    newItem.innerHTML = `
                        <input type="text" class="form-control" name="{{name}}" placeholder="{{placeholder}}" {{requiredAttr}} />
                        <button type="button" class="btn btn-outline-danger remove-btn">✕</button>
                    `;
                    container.appendChild(newItem);
                    newItem.querySelector('input').focus();
                });
                container.dataset.init = 'true';
            })();
        </script>
        """);

        return Task.FromResult<IHtmlContent>(new HtmlString(sb.ToString()));
    }
}
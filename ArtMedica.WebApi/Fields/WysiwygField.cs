using ArtMedica.WebApi.Abstractions;
using Microsoft.AspNetCore.Html;

public sealed class WysiwygField : FieldBase
{
    public int Height { get; init; } = 300;

    public override Task<IHtmlContent> RenderAsync(AdminFieldRenderContext context)
    {
        var def  = context.Definition;
        var name = Encode(def.Name);

        var encodedValue = System.Net.WebUtility.HtmlEncode(
            context.Value?.ToString() ?? "");

        var errorHtml = context.Error is null
            ? ""
            : $"<div class=\"text-danger\">{Encode(context.Error)}</div>";

        // ВАЖНО: $$""" — теперь интерполяции это {{...}}, а одиночные { } в JS — литералы.
        var html = $$"""
<div class="mb-3">
    <label class="form-label" for="{{name}}">{{Encode(def.Label)}}</label>

    <textarea
        id="{{name}}"
        name="{{name}}"
        class="form-control wysiwyg-editor"
        style="display:none;"
        placeholder="{{Encode(def.Placeholder)}}"
        {{(def.Required ? "required" : "")}}
        {{(def.ReadOnly ? "readonly" : "")}}
    >{{encodedValue}}</textarea>

    <!-- ← Высота уходит сюда, а не в textarea -->
        <div id="{{name}}_editor"
         class="wysiwyg-container"
         style="height: {{Height}}px;"></div>

    {{errorHtml}}
</div>
<script>
    (function() {
        var textarea = document.getElementById('{{name}}');
        if (!textarea || textarea.dataset.quillInitialized) return;

        var quill = new Quill('#{{name}}_editor', {
            theme: 'snow',
            modules: { /* toolbar как был */ }
        });

        if (textarea.value) {
            quill.clipboard.dangerouslyPasteHTML(textarea.value);
        }

        var form = textarea.closest('form');
        if (form) {
            form.addEventListener('submit', function() {
                textarea.value = quill.root.innerHTML;
            });
        }

        textarea.dataset.quillInitialized = 'true';
    })();
</script>
""";
        return Task.FromResult<IHtmlContent>(new HtmlString(html));
    }
}
/* Formulário de produto, compartilhado por Cadastrar e Alterar
   (Views/Fornecedor/Cadastrar.cshtml e Alterar.cshtml têm os mesmos campos).
   O markup e as classes são os do Razor; só foram acrescentados os `id` que
   faltavam para os <label for> apontarem para algum campo de verdade. */

const CAMPOS_PRODUTO = [
  {
    nome: "txtNome", prop: "Nome", rotulo: "Nome do produto:", tipo: "text",
    placeholder: "Digite o nome do produto...", required: true,
  },
  {
    nome: "txtDescricao", prop: "Descricao", rotulo: "Descrição:", tipo: "textarea",
    placeholder: "Digite uma breve descrição do produto...",
  },
  {
    nome: "txtPreco", prop: "Preco", rotulo: "Preço:", tipo: "text",
    placeholder: "R$ 00,00...", required: true,
  },
  {
    nome: "txtGrupo", prop: "Grupo", rotulo: "Vincular à lista:", tipo: "text",
    placeholder: "Digite o nome da lista que esse produto pertence...", required: true,
  },
  { nome: "uplFoto", prop: "Foto", rotulo: "Foto:", tipo: "file" },
  {
    nome: "txtPeso", prop: "Peso", rotulo: "Peso <small>(Kg/mg)</small>:", tipo: "text",
    placeholder: "00,00g...", required: true,
  },
  {
    nome: "txtQuantidade", prop: "Quantidade",
    rotulo: "Serve <small>(Ex: até 1 pessoa)</small>:", tipo: "text",
    placeholder: "Digite a quantidade do produto...", required: true,
  },
  {
    nome: "txtObservacao", prop: "Observacao", rotulo: "Observação:", tipo: "textarea",
    placeholder: "Digite uma observação breve do produto...",
  },
];

// Sem servidor não há upload: a foto escolhida vira um data: URL no localStorage.
const LIMITE_FOTO = 800 * 1024;

function montarFormProduto(alvo, produto) {
  const alterando = produto !== null;

  let html = `<form id="${alterando ? "alterar" : "cadastrar"}">`;

  CAMPOS_PRODUTO.forEach(function (campo) {
    const valor = produto ? produto[campo.prop] || "" : "";

    html += `<div class="fornecedor-form__grupo">
      <label for="${campo.nome}">${campo.rotulo}</label>`;

    if (campo.tipo === "textarea") {
      // No Razor o textarea nasce vazio e é preenchido por script; aqui o valor
      // já entra no HTML, com escape.
      html += `<textarea id="${campo.nome}" name="${campo.nome}" placeholder="${campo.placeholder}">${esc(valor)}</textarea>`;
    } else if (campo.tipo === "file") {
      html += `<div class="fornecedor-form__file-upload">
        <img src="${alterando ? esc(urlFoto(valor)) : "/assets/img/icones/imagemnaodisponivel.svg"}"
             class="${alterando ? "" : "imagem-nao-disponivel"}"
             alt="${alterando ? esc(produto.Nome) : "Nenhuma imagem cadastrada ainda"}" />
        <input type="hidden" name="hdnFoto" value="${esc(valor)}" />
        <input type="file" id="${campo.nome}" name="${campo.nome}" accept="image/*" />
      </div>`;
    } else {
      html += `<input type="${campo.tipo}" id="${campo.nome}" name="${campo.nome}"
                      placeholder="${campo.placeholder}" value="${esc(valor)}" ${campo.required ? "required" : ""} />`;
    }

    html += `</div>`;
  });

  html += `<div class="fornecedor-form__grupo">
    <button class="fornecedor-form__submit" type="submit">${alterando ? "Alterar" : "Cadastrar"}</button>
    ${alterando ? `<div class="deletar"><a class="fornecedor-form__link" href="#" id="lnkDeletar">Deletar Produto</a></div>` : ""}
  </div>
  <p class="erro-demo" id="erro" hidden></p>
  </form>`;

  alvo.innerHTML = html;

  const form = alvo.querySelector("form");
  const inpFoto = form.querySelector(".fornecedor-form__file-upload [name='uplFoto']");
  const hdnFoto = form.querySelector(".fornecedor-form__file-upload [name='hdnFoto']");
  const imgFoto = form.querySelector(".fornecedor-form__file-upload img");

  inpFoto.addEventListener("change", function () {
    const [file] = inpFoto.files;
    if (!file) return;

    const leitor = new FileReader();
    leitor.onload = function () {
      if (leitor.result.length > LIMITE_FOTO) {
        inpFoto.value = "";
        mostrarErro("Imagem grande demais para a demo (limite ~600 KB). Escolha outra.");
        return;
      }
      mostrarErro("");
      hdnFoto.value = leitor.result;
      imgFoto.src = leitor.result;
      imgFoto.classList.remove("imagem-nao-disponivel");
    };
    leitor.readAsDataURL(file);
  });

  return form;
}

function lerFormProduto(form) {
  const dados = {};
  CAMPOS_PRODUTO.forEach(function (campo) {
    // A foto vem do hidden hdnFoto, como no Alterar do FornecedorController.
    const el = form.querySelector(`[name='${campo.tipo === "file" ? "hdnFoto" : campo.nome}']`);
    dados[campo.prop] = el.value;
  });
  return dados;
}

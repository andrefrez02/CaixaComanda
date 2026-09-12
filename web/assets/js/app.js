/* Layout compartilhado (espelha Views/Shared/_Layout.cshtml e _NavUsuario.cshtml)
   e helpers de render. */

function esc(valor) {
  if (valor === null || valor === undefined) return "";
  return String(valor)
    .replace(/&/g, "&amp;")
    .replace(/</g, "&lt;")
    .replace(/>/g, "&gt;")
    .replace(/"/g, "&quot;");
}

function query(nome) {
  return new URLSearchParams(location.search).get(nome);
}

/* Era @Html.Raw(File.ReadAllText("wwwroot/Icones/voltar.svg")) no _NavUsuario. */
const SVG_VOLTAR = `<svg width="8" height="12" viewBox="0 0 8 12" fill="none" xmlns="http://www.w3.org/2000/svg">
<path d="M6.85504 3.70205L4.06504 6.42005L6.85504 9.10205L5.07304 10.8481L1.11304 6.87005V5.91605L5.07304 1.95605L6.85504 3.70205Z" fill="#F3762B"/>
<path d="M7.20394 4.0602C7.30037 3.96626 7.35484 3.8374 7.35504 3.70277C7.35523 3.56815 7.30113 3.43913 7.20496 3.34491L5.42296 1.59891C5.22714 1.40705 4.91334 1.40865 4.71948 1.6025L0.759484 5.5625C0.665716 5.65627 0.613037 5.78345 0.613037 5.91605V6.87005C0.613037 7.00227 0.665405 7.1291 0.758683 7.22281L4.71868 11.2008C4.91241 11.3954 5.22683 11.3974 5.42296 11.2052L7.20496 9.4592C7.30154 9.36457 7.35568 9.23487 7.35503 9.09967C7.35439 8.96447 7.29902 8.83529 7.20155 8.74159L4.78403 6.41766L7.20394 4.0602Z" stroke="#902B26" stroke-opacity="0.3" stroke-linejoin="round"/>
</svg>`;

/* O _NavUsuario sobe um nível na URL para montar o "Voltar". Lá as rotas eram
   /fornecedor/alterar; aqui são arquivos .html dentro de pastas, então tiramos o
   index.html / a barra final antes de subir. */
function urlAnterior() {
  const partes = location.pathname.replace(/index\.html$/, "").replace(/\/$/, "").split("/");
  partes.pop();
  return partes.join("/") + "/";
}

function montarLayout(titulo) {
  document.title = titulo + " - CaixaComanda";

  // Mesmas condições do _NavUsuario.cshtml.
  const inicio = titulo === "Identifique-se" || titulo === "Login" || titulo === "Cadastre-se";
  const voltar = titulo !== "Administrar";
  const voltarListagemFornecedor = titulo === "Alterar Cadastro";

  const esquerda =
    inicio || !voltar
      ? `<img class="main-header__icone" src="/assets/img/favicon.svg" alt="CaixaComanda" />`
      : `<button class="main-header__voltar" title="Voltar" type="button">${SVG_VOLTAR}</button>`;

  const login = inicio
    ? "&nbsp;"
    : `<div class="logado">
         <a href="#" id="lnkSair">Sair</a>
         <a href="/login/alterar.html">Alterar</a>
       </div>
       <div class="deslogado">
         <a href="/login/login.html">Login</a>
         <a href="/login/cadastrar.html">Cadastrar</a>
       </div>`;

  document.body.insertAdjacentHTML(
    "afterbegin",
    `<header class="main-header">
      ${esquerda}
      <p class="main-header__titulo">${esc(titulo)}</p>
      <div class="main-header__login">${login}</div>
    </header>`
  );

  const btnVoltar = document.querySelector(".main-header__voltar");
  if (btnVoltar) {
    btnVoltar.addEventListener("click", function () {
      location.href = voltarListagemFornecedor ? "/fornecedor/" : urlAnterior();
    });
  }

  if (!inicio) {
    // Mesmo script do _NavUsuario: some com o bloco que não se aplica.
    document.querySelector(store.usuarioLogado() ? ".deslogado" : ".logado").remove();

    const lnkSair = document.getElementById("lnkSair");
    if (lnkSair) {
      lnkSair.addEventListener("click", function (e) {
        e.preventDefault();
        store.deslogar(); // LoginController.Deslogar expira o cookie e volta pro login
        location.href = "/login/login.html";
      });
    }
  }

  // Rodapé só da demo estática: restaura o seed e dá acesso à rota JSON,
  // que no original não tinha link em tela nenhuma.
  document.body.insertAdjacentHTML(
    "beforeend",
    `<footer class="rodape-demo">
      <a href="/login/json.html" target="_blank">JSON tb_usuarios</a>
      <button type="button" id="btnRestaurar">Restaurar dados de exemplo</button>
    </footer>`
  );

  document.getElementById("btnRestaurar").addEventListener("click", function () {
    if (confirm("Isso apaga os produtos e usuários que você criou nesta demo. Continuar?")) {
      store.restaurar();
      location.href = "/";
    }
  });
}

/* Views/Login/Alterar.cshtml lê o usuário do cookie LoginId; sem login o original
   estoura. Aqui manda para a tela de login. */
function exigirLogin() {
  const usuario = store.usuarioLogado();
  if (!usuario) {
    location.href = "/login/login.html";
    return null;
  }
  return usuario;
}

/* Foto do produto: o seed guarda o nome do arquivo (coluna Foto do MySQL) e o
   cadastro pelo site guarda um data: URL, já que não há servidor para receber upload. */
function urlFoto(foto) {
  if (!foto) return "/assets/img/alternativo.svg";
  return String(foto).startsWith("data:")
    ? foto
    : "/assets/img/produtos/" + encodeURIComponent(foto);
}

function mostrarErro(texto) {
  const alvo = document.getElementById("erro");
  if (!alvo) return;
  alvo.textContent = texto;
  alvo.hidden = !texto;
}

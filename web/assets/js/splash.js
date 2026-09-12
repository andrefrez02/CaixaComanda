/* Splash de abertura: espelha Views/Shared/_Splash.cshtml (cliente) e
   _SplashFornecedor.cshtml. Continua usando cookie, como no original — só os dados
   do "banco" foram para o localStorage. */

function montarSplash(fornecedor) {
  const cookie = fornecedor ? "primeiroAcessoFornecedor" : "primeiroAcesso";
  const espera = fornecedor ? 5500 : 7500;

  const conteudo = fornecedor
    ? `<div class="splash-inicio">
         <img class="texto" src="/assets/img/favicon.svg" alt="CaixaComanda" />
         <span>Nome do estabelecimento</span>
         <span>Administrar</span>
       </div>`
    : `<div class="splash-inicio">
         <img class="icone" src="/assets/img/favicon.svg" alt="CaixaComanda" />
         <img class="texto" src="/assets/img/title.svg" alt="CaixaComanda" />
       </div>`;

  const main = document.querySelector(".main-conteudo");
  main.insertAdjacentHTML(
    "afterbegin",
    `<div class="splash${fornecedor ? " fornecedor" : ""}">${conteudo}</div>`
  );

  // Só aparece no primeiro acesso do dia, como no original.
  if (getCookie(cookie) !== "S") {
    setCookie(cookie, "S", "1");
  } else {
    main.querySelector(".splash").remove();
    return;
  }

  setTimeout(function () {
    const splash = document.querySelector(".splash");
    if (!splash) return; // o original não testava e estourava se a splash já tivesse saído

    splash.firstElementChild.querySelectorAll("img, span").forEach((item) => {
      item.style.scale = "0";
    });

    const estilo = document.createElement("style");
    estilo.innerHTML = ".splash{ animation: none !important; background-color: #ff9c65 !important; }";
    splash.appendChild(estilo);

    setTimeout(function () {
      estilo.innerHTML += "\n.splash{ background-color: transparent !important; }";
    }, 500);

    setTimeout(function () {
      splash.remove();
    }, 1000);
  }, espera);
}

# CaixaComanda — front estático

Versão só-front do CaixaComanda, para publicar no Vercel. O projeto ASP.NET Core
original continua em [`../CaixaComanda`](../CaixaComanda) e não foi alterado.

## O que mudou em relação ao original

O MySQL (`tb_produtos` / `tb_usuarios`) foi substituído por `localStorage`, em
[`assets/js/store.js`](assets/js/store.js). Os campos são os mesmos das classes
`Usuario` e `Produto` de `CaixaComanda/Classes/Classes.cs` — descontadas as
propriedades que a classe `Usuario` declara mas que **não existem** na tabela
(`NomeUsuario`, `NomeCompleto`, `Nascimento`): os dumps em `DadosBanco/` mostram
que `tb_usuarios` só tem `Id`, `Email`, `Senha`, `DataCriacao` e `Tipo`. Os
métodos de `assets/js/store.js` espelham os de `CaixaComanda/Models/DataModel.cs`,
inclusive as frases de retorno ("Produto deletado com sucesso." etc.).

As Razor views viraram HTML com render no cliente, preservando o mesmo markup e
CSS: `assets/css/main.css` é cópia do `wwwroot/SASS/main.css` (os fontes `.sass` e
o `main.css.map` vieram junto, para o sourcemap continuar resolvendo). Este projeto
não usa Bootstrap nem tem `site.css` — o único CSS novo é
[`assets/css/demo.css`](assets/css/demo.css), com o rodapé da demo e as mensagens de
erro que o Razor calculava na variável `erro` mas nunca chegava a exibir.

As imagens saíram do `wwwroot` para `assets/img/`: `Imagens/` na raiz,
`Icones/` em `assets/img/icones/` e as fotos de produto em `assets/img/produtos/`
(no Razor eram `~/Arquivos/Imagens/`).

Aos `<label for="...">` do original foram acrescentados os `id` correspondentes nos
campos — no Razor eles apontavam para o `name`, ou seja, não apontavam para nada.

### Telas portadas

| Tela | Arquivo | Origem |
|---|---|---|
| Identifique-se (splash + Cliente/Fornecedor) | `index.html` | `Views/Home/Index.cshtml` |
| Cliente | `cliente/index.html` | `Views/Cliente/Index.cshtml` |
| Login | `login/login.html` | `Views/Login/Login.cshtml` |
| Cadastre-se | `login/cadastrar.html` | `Views/Login/Cadastrar.cshtml` |
| Alterar Cadastro (+ deletar conta) | `login/alterar.html` | `Views/Login/Alterar.cshtml` |
| JSON da tb_usuarios | `login/json.html` | `Views/Shared/JSON.cshtml` |
| Administrar (listagem de produtos) | `fornecedor/index.html` | `Views/Fornecedor/Index.cshtml` |
| Cadastrar produto | `fornecedor/cadastrar.html` | `Views/Fornecedor/Cadastrar.cshtml` |
| Alterar produto (+ deletar) | `fornecedor/alterar.html` | `Views/Fornecedor/Alterar.cshtml` |

O cabeçalho (`Views/Shared/_NavUsuario.cshtml`) e as duas splashes
(`_Splash.cshtml` e `_SplashFornecedor.cshtml`) viraram
[`assets/js/app.js`](assets/js/app.js) e [`assets/js/splash.js`](assets/js/splash.js).
As splashes continuam controladas por cookie, como no original — só os dados do
"banco" foram para o `localStorage`.

### Comportamentos do original que foram mantidos de propósito

- **Todo cadastro vira administrador.** `LoginController.Cadastrar` grava
  `Tipo = "A"` fixo, então quem se cadastra pelo site cai na área do fornecedor,
  nunca na do cliente. Está replicado (e comentado) em `store.cadastrarUsuario`.
- **A área do fornecedor não pede login.** Nenhum controller do original verifica
  o cookie `LoginId` antes de listar, cadastrar, alterar ou deletar produto. Quem
  souber a URL `/fornecedor/` entra. Só `Login/Alterar` exige sessão.
- **"Nenhum campo foi alterado"** quando o formulário de produto é enviado sem
  mudança, e as validações de senha ("As senhas estão diferentes.", "A senha atual
  está incorreta.", "As senhas devem ser diferentes.").
- **E-mail único**, como o `UNIQUE KEY` de `tb_usuarios`.

### Onde o front estático precisou divergir

- **Upload de foto.** Sem servidor não há `IFormFile`: a imagem escolhida vira um
  `data:` URL guardado no campo `Foto` (limite de ~600 KB, para não estourar a cota
  do `localStorage`). O seed continua guardando só o nome do arquivo, como a coluna
  do MySQL.
- **`Views/Cliente/Index.cshtml` está vazia** no original (3 bytes, só o BOM). Como
  ela não define `ViewData["Title"]`, o `_NavUsuario` estoura e `/cliente` devolve
  **HTTP 500** no app .NET. Aqui a tela existe, com o título "Cliente", e diz que o
  conteúdo nunca foi implementado.
- **`/login` (`LoginController.Index`) não tem view** no original — a pasta
  `Views/Login/` não tem `Index.cshtml`. Não há tela correspondente aqui.
- **Rota JSON sem link.** No original nada em tela levava a `/login/json`; o link
  no rodapé da demo existe só para a rota ficar alcançável.

## Rodar local

```bash
cd web
python -m http.server 8123
```

Abra <http://127.0.0.1:8123>. Precisa ser servido por HTTP — abrir o `index.html`
direto pelo `file://` quebra os caminhos absolutos.

## Contas de demonstração

| E-mail | Senha | Tipo |
|---|---|---|
| admin@caixacomanda.com | admin | `A` — cai em **Administrar** (fornecedor) |
| cliente@caixacomanda.com | cliente | `C` — cai na tela do **Cliente** |

Os produtos de exemplo são os do dump `DadosBanco/Dump20240115c`. O botão
**Restaurar dados de exemplo**, no rodapé, devolve o site ao estado inicial.

## Publicar no Vercel

O [`vercel.json`](../vercel.json) na raiz já aponta `outputDirectory` para `web/`,
então basta importar o repositório — sem build step, sem variável de ambiente.
Se preferir, dá para ignorar o `vercel.json` e definir **Root Directory = `web`**
nas configurações do projeto.

## Limitações

Os dados vivem no navegador de quem acessa: cada visitante começa com os mesmos
produtos de exemplo e o que ele cadastrar não aparece para mais ninguém. Limpar os
dados do site zera tudo. As senhas ficam em texto puro no `localStorage` — como a
demo não tem servidor nem dados reais, isso é aceitável aqui, mas não é um modelo
de autenticação para copiar. A área do fornecedor, como no original, não tem
nenhuma barreira de acesso.

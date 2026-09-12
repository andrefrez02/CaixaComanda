/* Mock do banco: o que era MySQL (tb_produtos / tb_usuarios) agora vive no localStorage.
   Os campos espelham as classes de CaixaComanda/Classes/Classes.cs e o schema real dos
   dumps em DadosBanco/. Os métodos espelham CaixaComanda/Models/DataModel.cs. */

const CHAVE_PRODUTOS = "caixacomanda:produtos";
const CHAVE_USUARIOS = "caixacomanda:usuarios";
const CHAVE_LOGIN = "caixacomanda:LoginId";

/* Produtos vêm de DadosBanco/Dump20240115c/caixacomanda_tb_produtos.sql.
   Foto guarda só o nome do arquivo, como na coluna `Foto` do MySQL; quem renderiza
   prefixa /assets/img/produtos/ (era ~/Arquivos/Imagens/ no Razor). */
const SEED_PRODUTOS = [
  {
    Id: "3",
    Nome: "Filé de frango a parmegiana",
    Descricao:
      "Filé de frango preparado ao molho parmegiana com queijo parmesão.<br />Acompanhamentos: Arroz, feijão, filé de frango ao molho parmegiana.<br /><br />300g de Arroz<br />150g de Feijão<br />1 Filé de frango com molho (aproximadamente 150g)<br /><br />Obs: Foto ilustrativa.<br /><br />Serve 1 pessoa (aprox. 600g)<br />",
    Preco: "30.00",
    Grupo: "Pratos Principais",
    Foto: "file-de-frango-a-parmegiana.png",
    Peso: "400g",
    Quantidade: "1",
    Observacao: "",
  },
  {
    Id: "15",
    Nome: "Sopa de Cebola",
    Descricao: "Sopa de cebola gratinada, servida bem quente.",
    Preco: "13.49",
    Grupo: "Sopas",
    Foto: "sopa-de-cebola.jpeg",
    Peso: "01.00",
    Quantidade: "1",
    Observacao: "Acompanha torradas.",
  },
  {
    Id: "22",
    Nome: "Batata",
    Descricao: "Batata frita crocante.",
    Preco: "30.50",
    Grupo: "Acompanhamentos",
    Foto: "batata frita.jpg",
    Peso: "500",
    Quantidade: "2",
    Observacao: "Picante",
  },
  {
    Id: "23",
    Nome: "Peixe Frito",
    Descricao: "Peixe frito ",
    Preco: "19.99",
    Grupo: "Peixes",
    Foto: "peixe-frito.webp",
    Peso: "250g",
    Quantidade: "1",
    Observacao: "1 Peixe Frito Grande",
  },
  {
    Id: "24",
    Nome: "Saladinha",
    Descricao: "Salada saladinha bem temperadinha...",
    Preco: "39.99",
    Grupo: "Saladas",
    Foto: "salada.webp",
    Peso: "1.5Kg",
    Quantidade: "5",
    Observacao: "... com sal, pimenta, fogo, foguinho e FOGÃO!",
  },
  {
    Id: "25",
    Nome: "Suco de laranja",
    Descricao: "Suco com poupa de laranja.",
    Preco: "4.00",
    Grupo: "Bebidas",
    Foto: "ft_6.png",
    Peso: "1",
    Quantidade: "1",
    Observacao: "delicioso.",
  },
];

/* tb_usuarios só tem Id, Email, Senha, DataCriacao e Tipo (ver dumps). As demais
   propriedades da classe Usuario não existem no banco e ficam de fora aqui também.
   Tipo "A" = fornecedor/administrador, qualquer outro = cliente. */
const SEED_USUARIOS = [
  {
    Id: "1",
    Email: "admin@caixacomanda.com",
    Senha: "admin",
    DataCriacao: "2024-01-02 18:40:00",
    Tipo: "A",
  },
  {
    Id: "2",
    Email: "cliente@caixacomanda.com",
    Senha: "cliente",
    DataCriacao: "2024-01-02 22:00:03",
    Tipo: "C",
  },
];

function ler(chave, seed) {
  const bruto = localStorage.getItem(chave);
  if (bruto === null) {
    localStorage.setItem(chave, JSON.stringify(seed));
    return JSON.parse(JSON.stringify(seed));
  }
  try {
    return JSON.parse(bruto);
  } catch {
    localStorage.setItem(chave, JSON.stringify(seed));
    return JSON.parse(JSON.stringify(seed));
  }
}

function gravar(chave, valor) {
  localStorage.setItem(chave, JSON.stringify(valor));
}

/* Faz o papel do AUTO_INCREMENT / LAST_INSERT_ID(). */
function proximoId(lista) {
  return String(lista.reduce((max, i) => Math.max(max, Number(i.Id) || 0), 0) + 1);
}

function agora() {
  return new Date().toISOString().slice(0, 19).replace("T", " ");
}

const store = {
  listarProduto: () => ler(CHAVE_PRODUTOS, SEED_PRODUTOS),

  listarUsuarios: () => ler(CHAVE_USUARIOS, SEED_USUARIOS),

  /* DataModel.ListarJson(tabela) */
  listarJson(tabela) {
    const dados = tabela === "tb_usuarios" ? store.listarUsuarios() : store.listarProduto();
    return JSON.stringify(dados, null, 2);
  },

  pesquisarProduto(id) {
    return ler(CHAVE_PRODUTOS, SEED_PRODUTOS).find((p) => p.Id === String(id)) || null;
  },

  pesquisarUsuario(id) {
    return ler(CHAVE_USUARIOS, SEED_USUARIOS).find((u) => u.Id === String(id)) || null;
  },

  cadastrarProduto(produto) {
    const produtos = ler(CHAVE_PRODUTOS, SEED_PRODUTOS);
    produto.Id = proximoId(produtos);
    produtos.push(produto);
    gravar(CHAVE_PRODUTOS, produtos);
    return produto.Id;
  },

  alterarProduto(id, campos) {
    const produtos = ler(CHAVE_PRODUTOS, SEED_PRODUTOS);
    const i = produtos.findIndex((p) => p.Id === String(id));
    if (i === -1) return false;
    produtos[i] = { ...produtos[i], ...campos };
    gravar(CHAVE_PRODUTOS, produtos);
    return true;
  },

  /* Devolve as mesmas frases de DataModel.DeletarProduto. */
  deletarProduto(id) {
    const produtos = ler(CHAVE_PRODUTOS, SEED_PRODUTOS);
    const restantes = produtos.filter((p) => p.Id !== String(id));
    if (restantes.length === produtos.length) return "Produto não encontrado.";
    gravar(CHAVE_PRODUTOS, restantes);
    return "Produto deletado com sucesso.";
  },

  cadastrarUsuario(usuario) {
    const usuarios = ler(CHAVE_USUARIOS, SEED_USUARIOS);
    // tb_usuarios tem UNIQUE KEY em Email: e-mail repetido não entra.
    if (usuarios.some((u) => u.Email === usuario.Email)) return null;
    usuario.Id = proximoId(usuarios);
    usuario.DataCriacao = agora();
    // Peculiaridade do original: LoginController.Cadastrar grava Tipo = "A" fixo,
    // ou seja, todo cadastro pelo site nasce como fornecedor/administrador.
    usuario.Tipo = "A";
    usuarios.push(usuario);
    gravar(CHAVE_USUARIOS, usuarios);
    return usuario.Id;
  },

  alterarUsuario(id, campos) {
    const usuarios = ler(CHAVE_USUARIOS, SEED_USUARIOS);
    const i = usuarios.findIndex((u) => u.Id === String(id));
    if (i === -1) return false;
    usuarios[i] = { ...usuarios[i], ...campos };
    gravar(CHAVE_USUARIOS, usuarios);
    return true;
  },

  /* Devolve as mesmas frases de DataModel.DeletarUsuario. */
  deletarUsuario(id) {
    const usuarios = ler(CHAVE_USUARIOS, SEED_USUARIOS);
    const restantes = usuarios.filter((u) => u.Id !== String(id));
    if (restantes.length === usuarios.length) return "Usuário não encontrado.";
    gravar(CHAVE_USUARIOS, restantes);
    if (localStorage.getItem(CHAVE_LOGIN) === String(id)) store.deslogar();
    return "Usuário deletado com sucesso.";
  },

  /* DataModel.LoginUsuario(email, senha). O original grava o Id num cookie "LoginId";
     aqui a sessão fica no localStorage, junto com o resto dos dados da demo. */
  loginUsuario(email, senha) {
    const achado = ler(CHAVE_USUARIOS, SEED_USUARIOS).find(
      (u) => u.Email === email && u.Senha === senha
    );
    if (!achado) return null;
    localStorage.setItem(CHAVE_LOGIN, achado.Id);
    return achado;
  },

  deslogar: () => localStorage.removeItem(CHAVE_LOGIN),

  usuarioLogado() {
    const id = localStorage.getItem(CHAVE_LOGIN);
    return id ? store.pesquisarUsuario(id) : null;
  },

  restaurar() {
    gravar(CHAVE_PRODUTOS, SEED_PRODUTOS);
    gravar(CHAVE_USUARIOS, SEED_USUARIOS);
    store.deslogar();
  },
};

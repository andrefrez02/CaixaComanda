using CaixaComanda.Classes;
using CaixaComanda.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace CaixaComanda.Controllers
{
    public class FornecedorController : Controller
    {
        private IWebHostEnvironment Environment;

        public FornecedorController(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Alterar()
        {
            DataModel dados = new DataModel();

            Produto produto = new Produto();

            string id = string.Empty;
            string erro = string.Empty;

            if (!string.IsNullOrEmpty(Request.Query.FirstOrDefault().Value.ToString()))
            {
                id = Request.Query.FirstOrDefault().Value.ToString();
            }
            else
            {
                erro = "Id não encontrado";
            }

            if (string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(erro))
            {
                if (!string.IsNullOrEmpty(Request.Form["Id"].ToString()))
                {
                    id = (Request.Form["Id"].ToString());
                    erro = string.Empty;
                }
                else
                {
                    erro = "Id não encontrado";
                }
            }

            produto = dados.PesquisarProduto(id);

            if (!string.IsNullOrEmpty(id) || !string.IsNullOrEmpty(erro))
            {
                return View("Alterar", produto);
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost]
        public IActionResult Cadastrar(IFormFile uplFoto)
        {
            DataModel dados = new DataModel();

            int id = 0;

            CampoGenerico nome = new CampoGenerico() { Campo = "Nome" };
            CampoGenerico descricao = new CampoGenerico() { Campo = "Descricao" };
            CampoGenerico preco = new CampoGenerico() { Campo = "Preco" };
            CampoGenerico grupo = new CampoGenerico() { Campo = "Grupo" };
            CampoGenerico foto = new CampoGenerico() { Campo = "Foto" };
            CampoGenerico peso = new CampoGenerico() { Campo = "Peso" };
            CampoGenerico quantidade = new CampoGenerico() { Campo = "Quantidade" };
            CampoGenerico observacao = new CampoGenerico() { Campo = "Observacao" };

            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = Path.Combine(this.Environment.WebRootPath, "Arquivos\\Imagens");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = Path.GetFileName(uplFoto.FileName);
            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                uplFoto.CopyTo(stream);
            }

            string erro = string.Empty;

            try
            {
                nome.Valor = Request.Form["txtNome"].ToString();
                descricao.Valor = Request.Form["txtDescricao"].ToString();
                preco.Valor = Request.Form["txtPreco"].ToString();
                grupo.Valor = Request.Form["txtGrupo"].ToString();
                foto.Valor = fileName.ToString();
                peso.Valor = Request.Form["txtPeso"].ToString();
                quantidade.Valor = Request.Form["txtQuantidade"].ToString();
                observacao.Valor = Request.Form["txtObservacao"].ToString();
            }
            catch (Exception ex)
            {
                erro = ex.Message.ToString();
            }

            if (string.IsNullOrEmpty(erro))
            {
                List<CampoGenerico> parametros = new List<CampoGenerico>();

                parametros.Add(nome);
                parametros.Add(descricao);
                parametros.Add(preco);
                parametros.Add(grupo);
                parametros.Add(foto);
                parametros.Add(peso);
                parametros.Add(quantidade);
                parametros.Add(observacao);

                try
                {
                    id = int.Parse(dados.CadastrarProduto(parametros));
                }
                catch (Exception ex)
                {
                    erro = ex.Message;
                }
            }

            if (id != 0)
            {
                return View("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Alterar(IFormFile uplFoto)
        {
            DataModel dados = new DataModel();

            Produto produto = new Produto();

            string id = string.Empty;

            string urlFoto = string.Empty;

            CampoGenerico nome = new CampoGenerico() { Campo = "Nome" };
            CampoGenerico descricao = new CampoGenerico() { Campo = "Descricao" };
            CampoGenerico preco = new CampoGenerico() { Campo = "Preco" };
            CampoGenerico grupo = new CampoGenerico() { Campo = "Grupo" };
            CampoGenerico foto = new CampoGenerico() { Campo = "Foto" };
            CampoGenerico peso = new CampoGenerico() { Campo = "Peso" };
            CampoGenerico quantidade = new CampoGenerico() { Campo = "Quantidade" };
            CampoGenerico observacao = new CampoGenerico() { Campo = "Observacao" };

            if (uplFoto != null)
            {
                string wwwPath = this.Environment.WebRootPath;
                string contentPath = this.Environment.ContentRootPath;

                string path = Path.Combine(this.Environment.WebRootPath, "Arquivos\\Imagens");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = Path.GetFileName(uplFoto.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    uplFoto.CopyTo(stream);
                }

                urlFoto = fileName.ToString();
            }
            else
            {
                urlFoto = Request.Form["hdnFoto"].ToString();
            }

            string erro = string.Empty;

            if (!string.IsNullOrEmpty(Request.Form["hdnId"].ToString()))
            {
                id = Request.Form["hdnId"].ToString();
            }
            else
            {
                erro = "Id não encontrado";
            }

            produto = dados.PesquisarProduto(id);

            try
            {
                nome.Valor = Request.Form["txtNome"].ToString();
                descricao.Valor = Request.Form["txtDescricao"].ToString();
                preco.Valor = Request.Form["txtPreco"].ToString();
                grupo.Valor = Request.Form["txtGrupo"].ToString();
                foto.Valor = urlFoto;
                peso.Valor = Request.Form["txtPeso"].ToString();
                quantidade.Valor = Request.Form["txtQuantidade"].ToString();
                observacao.Valor = Request.Form["txtObservacao"].ToString();
            }
            catch (Exception ex)
            {
                erro = ex.Message.ToString();
            }

            if (string.IsNullOrEmpty(erro))
            {
                if (nome.Valor != produto.Nome || descricao.Valor != produto.Descricao || preco.Valor != produto.Preco || grupo.Valor != produto.Grupo || foto.Valor != produto.Foto || peso.Valor != produto.Peso || quantidade.Valor != produto.Quantidade || observacao.Valor != produto.Observacao)
                {
                    List<CampoGenerico> parametros = new List<CampoGenerico>();

                    parametros.Add(nome);
                    parametros.Add(descricao);
                    parametros.Add(preco);
                    parametros.Add(grupo);
                    parametros.Add(foto);
                    parametros.Add(peso);
                    parametros.Add(quantidade);
                    parametros.Add(observacao);

                    dados.AlterarProduto(parametros, id);
                }
                else
                {
                    erro = "Nenhum campo foi alterado";
                }
            }

            produto = dados.PesquisarProduto(id);

            if (!string.IsNullOrEmpty(id) || !string.IsNullOrEmpty(erro))
            {
                return View("Alterar", produto);
            }
            else
            {
                return View("Index");
            }
        }

        public IActionResult Deletar()
        {
            DataModel dados = new DataModel();

            string id = string.Empty;

            string erro = string.Empty;

            if (!string.IsNullOrEmpty(Request.Query.FirstOrDefault().Value.ToString()))
            {
                id = Request.Query.FirstOrDefault().Value.ToString();
            }
            else
            {
                erro = "Id não encontrado";
            }

            dados.DeletarProduto(id);

            return View("Index");
        }
    }
}
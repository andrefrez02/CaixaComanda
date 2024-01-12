using Microsoft.AspNetCore.Mvc;
using CaixaComanda.Classes;
using CaixaComanda.Models;

namespace CaixaComanda.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            if (Request.Cookies["LoginId"] != null)
            {
                TempData["LoginId"] = Request.Cookies["LoginId"];
            }
            else
            {
                TempData["LoginId"] = "error";
            }

            return View();
        }

        public IActionResult Login()
        {
            DataModel dados = new DataModel();

            Usuario login = new Usuario();
            string email = string.Empty;
            string senha = string.Empty;
            string erro = string.Empty;

            try
            {
                email = Request.Form["txtEmail"];
                senha = Request.Form["txtSenha"];
            }
            catch (Exception ex)
            {
                erro = ex.Message.ToString();
            }

            login = dados.LoginUsuario(email, senha);

            if (login != null)
            {
                CookieOptions cookieOptions = new CookieOptions()
                {
                    Path = "/",
                    HttpOnly = false,
                    IsEssential = true,
                    Expires = DateTime.Now.AddDays(1)
                };

                Response.Cookies.Append("LoginId", login.Id, cookieOptions);

                TempData["LoginId"] = login.Id;

                if (login.Tipo == "A")
                {
                    return View("../Fornecedor/Index");
                }
                else
                {
                    return View("../Cliente/Index");
                }
            }
            else
            {
                return View();
            }
        }

        public IActionResult Cadastrar()
        {
            DataModel dados = new DataModel();

            string id = string.Empty;

            CampoGenerico email = new CampoGenerico() { Campo = "Email" };
            CampoGenerico senha = new CampoGenerico() { Campo = "Senha" };
            CampoGenerico confirmarSenha = new CampoGenerico() { Campo = "ConfirmarSenha" };
            CampoGenerico tipo = new CampoGenerico() { Campo = "Tipo", Valor = "A" };

            string erro = string.Empty;

            try
            {
                email.Valor = Request.Form["txtEmail"].ToString();
                senha.Valor = Request.Form["txtSenha"].ToString();
                confirmarSenha.Valor = Request.Form["txtConfirmarSenha"].ToString();
            }
            catch (Exception ex)
            {
                erro = ex.Message.ToString();
            }

            if (senha.Valor != confirmarSenha.Valor)
            {
                erro = "As senhas estão diferentes.";
            }

            if (string.IsNullOrEmpty(erro))
            {
                List<CampoGenerico> parametros = new List<CampoGenerico>();

                parametros.Add(email);
                parametros.Add(senha);
                parametros.Add(tipo);

                id = dados.CadastrarUsuario(parametros);
            }

            if (!string.IsNullOrEmpty(id))
            {
                return View("../Login/Login");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Alterar()
        {
            DataModel dados = new DataModel();

            Usuario usuario = new Usuario();

            string id = string.Empty;

            if (Request.Cookies["LoginId"] != null)
            {
                TempData["LoginId"] = Request.Cookies["LoginId"];
                id = TempData["LoginId"].ToString();
            }
            else
            {
                TempData["LoginId"] = "error";
            }

            usuario = dados.PesquisarUsuario(id);

            CampoGenerico senha = new CampoGenerico() { Campo = "Senha", Valor = usuario.Senha };
            CampoGenerico novaSenha = new CampoGenerico() { Campo = "Senha", Valor = usuario.Senha };
            CampoGenerico tipo = new CampoGenerico() { Campo = "Tipo", Valor = usuario.Tipo};

            string erro = string.Empty;

            try
            {
                senha.Valor = Request.Form["txtSenha"].ToString();
                novaSenha.Valor = Request.Form["txtNovaSenha"].ToString();
            }
            catch (Exception ex)
            {
                erro = ex.Message.ToString();
            }

            if (senha.Valor != usuario.Senha)
            {
                erro = "A senha atual está incorreta.";
            }

            if (novaSenha.Valor == senha.Valor)
            {
                erro = "As senhas devem ser diferentes.";
            }

            if (string.IsNullOrEmpty(erro))
            {
                if (novaSenha.Valor != usuario.Senha)
                {
                    List<CampoGenerico> parametros = new List<CampoGenerico>();

                    parametros.Add(novaSenha);
                    parametros.Add(tipo);

                    dados.AlterarUsuario(parametros, id);
                }
                else
                {
                    erro = "Nenhum campo foi alterado";
                }
            }

            if (!string.IsNullOrEmpty(id) || !string.IsNullOrEmpty(erro))
            {
                return View();
            }
            else
            {
                if (tipo.Valor == "A")
                {
                    return View("../Fornecedor/Index");
                }
                else
                {
                    return View("../Cliente/Index");
                }
            }
        }

        public IActionResult Deletar()
        {
            DataModel dados = new DataModel();

            string id = string.Empty;

            if (Request.Cookies["LoginId"] != null)
            {
                TempData["LoginId"] = Request.Cookies["LoginId"];
                id = Request.Cookies["LoginId"];

                CookieOptions cookieOptions = new CookieOptions()
                {
                    Path = "/",
                    HttpOnly = false,
                    IsEssential = true,
                    Expires = DateTime.Now.AddDays(-1)
                };

                dados.DeletarUsuario(id);

                Response.Cookies.Append("LoginId", Request.Cookies["LoginId"], cookieOptions);

                return View("login");
            }
            else
            {
                TempData["LoginId"] = "error";
            }

            return View("login");   
        }

        public IActionResult Deslogar()
        {
            if (Request.Cookies["LoginId"] != null)
            {
                CookieOptions cookieOptions = new CookieOptions()
                {
                    Path = "/",
                    HttpOnly = false,
                    IsEssential = true,
                    Expires = DateTime.Now.AddDays(-1)
                };

                Response.Cookies.Append("LoginId", Request.Cookies["LoginId"], cookieOptions);

                return View("login");
            }

            return View("login");
        }

        public IActionResult JSON()
        {
            DataModel dados = new DataModel();

            string json = dados.ListarJson("tb_usuarios");

            return View("JSON", json);
        }
    }
}
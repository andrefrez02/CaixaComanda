using Microsoft.AspNetCore.Mvc;
using CaixaComanda.Classes;
using CaixaComanda.Models;

namespace CaixaComanda.Controllers
{
    public class EventosController : Controller
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

        public IActionResult Cadastrar()
        {
            DataModel dados = new DataModel();

            string id = string.Empty;

            CampoGenerico titulo = new CampoGenerico() { Campo = "Titulo" };
            CampoGenerico descricao = new CampoGenerico() { Campo = "Descricao" };
            CampoGenerico html = new CampoGenerico() { Campo = "Html" };
            CampoGenerico categoria = new CampoGenerico() { Campo = "Categoria" };
            CampoGenerico local = new CampoGenerico() { Campo = "Local" };
            CampoGenerico dataHoraEvento = new CampoGenerico() { Campo = "DataHoraEvento" };
            CampoGenerico dataHoraEventoAte = new CampoGenerico() { Campo = "DataHoraEventoAte" };
            CampoGenerico preco = new CampoGenerico() { Campo = "Preco" };
            CampoGenerico fotoCapa = new CampoGenerico() { Campo = "FotoCapa" };
            CampoGenerico tipo = new CampoGenerico() { Campo = "Tipo" };
            CampoGenerico horario = new CampoGenerico() { Campo = "Horario" };
            CampoGenerico destaque = new CampoGenerico() { Campo = "Destaque" };
            CampoGenerico ativo = new CampoGenerico() { Campo = "Ativo", Valor = "S" };
            CampoGenerico estoque = new CampoGenerico() { Campo = "Estoque" };
            CampoGenerico relacionado = new CampoGenerico() { Campo = "Relacionado", Valor = "NULL" };
            CampoGenerico eventoPai = new CampoGenerico() { Campo = "EventoPai", Valor = "S" };
            CampoGenerico duracao = new CampoGenerico() { Campo = "Duracao" };

            string erro = string.Empty;

            try
            {
                titulo.Valor = Request.Form["txtTitulo"].ToString();
                descricao.Valor = Request.Form["txtDescricao"].ToString();
                html.Valor = Request.Form["txtHtml"].ToString();
                categoria.Valor = Request.Form["txtCategoria"].ToString();
                local.Valor = Request.Form["txtLocal"].ToString();
                dataHoraEvento.Valor = string.Format("{0}:00", Request.Form["txtDataHoraEvento"].ToString().Replace("T", " "));
                dataHoraEventoAte.Valor = string.Format("{0}:00", Request.Form["txtDataHoraEventoAte"].ToString().Replace("T", " "));
                preco.Valor = Request.Form["txtPreco"].ToString();
                fotoCapa.Valor = string.Format("https://raw.githubusercontent.com/andrefrez02/analise-e-projeto-de-sistemas-ii-grupo-08/main/imagens/{0}", Request.Form["txtFotoCapa"].ToString());
                tipo.Valor = Request.Form["txtTipo"].ToString();
                horario.Valor = Request.Form["txtHorario"].ToString();
                destaque.Valor = Request.Form["txtDestaque"].ToString();
                estoque.Valor = Request.Form["txtEstoque"].ToString();
                duracao.Valor = Request.Form["txtDuracao"].ToString();
            }
            catch (Exception ex)
            {
                erro = ex.Message.ToString();
            }

            if (string.IsNullOrEmpty(erro))
            {
                List<CampoGenerico> parametros = new List<CampoGenerico>();

                parametros.Add(titulo);
                parametros.Add(descricao);
                parametros.Add(html);
                parametros.Add(categoria);
                parametros.Add(local);
                parametros.Add(dataHoraEvento);
                parametros.Add(dataHoraEventoAte);
                parametros.Add(preco);
                parametros.Add(fotoCapa);
                parametros.Add(tipo);
                parametros.Add(horario);
                parametros.Add(destaque);
                parametros.Add(estoque);
                parametros.Add(duracao);
                parametros.Add(eventoPai);
                parametros.Add(relacionado);
                parametros.Add(ativo);

                dados.CadastrarEvento(parametros);
            }

            if (!string.IsNullOrEmpty(id))
            {
                return View("Index");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Alterar()
        {
            DataModel dados = new DataModel();

            Evento evento = new Evento();

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
                if (!string.IsNullOrEmpty(Request.Form["txtId"].ToString()))
                {
                    id = (Request.Form["txtId"].ToString());
                    erro = string.Empty;
                }
                else
                {
                    erro = "Id não encontrado";
                }
            }

            evento = dados.PesquisarEvento(id);

            CampoGenerico titulo = new CampoGenerico() { Campo = "Titulo", Valor = evento.Titulo };
            CampoGenerico descricao = new CampoGenerico() { Campo = "Descricao", Valor = evento.Descricao };
            CampoGenerico html = new CampoGenerico() { Campo = "Html", Valor = evento.Html };
            CampoGenerico categoria = new CampoGenerico() { Campo = "Categoria", Valor = evento.Categoria };
            CampoGenerico local = new CampoGenerico() { Campo = "Local", Valor = evento.Local };
            CampoGenerico dataHoraEvento = new CampoGenerico() { Campo = "DataHoraEvento", Valor = evento.DataHoraEvento.ToString() };
            CampoGenerico dataHoraEventoAte = new CampoGenerico() { Campo = "DataHoraEventoAte", Valor = evento.DataHoraEventoAte.ToString() };
            CampoGenerico preco = new CampoGenerico() { Campo = "Preco", Valor = evento.Preco };
            CampoGenerico fotoCapa = new CampoGenerico() { Campo = "FotoCapa", Valor = evento.FotoCapa };
            CampoGenerico tipo = new CampoGenerico() { Campo = "Tipo", Valor = evento.Tipo };
            CampoGenerico horario = new CampoGenerico() { Campo = "Horario", Valor = evento.Horario };
            CampoGenerico destaque = new CampoGenerico() { Campo = "Destaque", Valor = evento.Destaque };
            CampoGenerico ativo = new CampoGenerico() { Campo = "Ativo", Valor = evento.Ativo };
            CampoGenerico estoque = new CampoGenerico() { Campo = "Estoque", Valor = evento.Estoque };
            CampoGenerico relacionado = new CampoGenerico() { Campo = "Relacionado", Valor = evento.Relacionado };
            CampoGenerico eventoPai = new CampoGenerico() { Campo = "EventoPai", Valor = evento.EventoPai };
            CampoGenerico duracao = new CampoGenerico() { Campo = "Duracao", Valor = evento.Duracao };

            try
            {
                titulo.Valor = Request.Form["txtTitulo"].ToString();
                descricao.Valor = Request.Form["txtDescricao"].ToString();
                html.Valor = Request.Form["txtHtml"].ToString();
                categoria.Valor = Request.Form["txtCategoria"].ToString();
                local.Valor = Request.Form["txtLocal"].ToString();
                dataHoraEvento.Valor = string.Format("{0}:00", Request.Form["txtDataHoraEvento"].ToString().Replace("T", " "));
                dataHoraEventoAte.Valor = string.Format("{0}:00", Request.Form["txtDataHoraEventoAte"].ToString().Replace("T", " "));
                preco.Valor = Request.Form["txtPreco"].ToString();
                fotoCapa.Valor = string.Format("https://raw.githubusercontent.com/andrefrez02/analise-e-projeto-de-sistemas-ii-grupo-08/main/imagens/{0}", Request.Form["txtFotoCapa"].ToString());
                tipo.Valor = Request.Form["txtTipo"].ToString();
                horario.Valor = Request.Form["txtHorario"].ToString();
                destaque.Valor = Request.Form["txtDestaque"].ToString();
                ativo.Valor = Request.Form["txtAtivo"].ToString();
                estoque.Valor = Request.Form["txtEstoque"].ToString();
                duracao.Valor = Request.Form["txtDuracao"].ToString();
            }
            catch (Exception ex)
            {
                erro = ex.Message.ToString();
            }

            if (string.IsNullOrEmpty(erro))
            {
                if (titulo.Valor != evento.Titulo || descricao.Valor != evento.Descricao || html.Valor != evento.Html || categoria.Valor != evento.Categoria || local.Valor != evento.Local || preco.Valor != evento.Preco || fotoCapa.Valor != evento.FotoCapa || tipo.Valor != evento.Tipo || horario.Valor != evento.Horario || destaque.Valor != evento.Destaque || ativo.Valor != evento.Ativo || estoque.Valor != evento.Estoque || relacionado.Valor != evento.Relacionado || eventoPai.Valor != evento.EventoPai || duracao.Valor != evento.Duracao || dataHoraEvento.Valor != evento.DataHoraEvento.ToString(@"yyyy-MM-dd HH:mm:ss") || dataHoraEventoAte.Valor != evento.DataHoraEventoAte.ToString(@"yyyy-MM-dd HH:mm:ss"))
                {
                    List<CampoGenerico> parametros = new List<CampoGenerico>();

                    parametros.Add(titulo);
                    parametros.Add(descricao);
                    parametros.Add(html);
                    parametros.Add(categoria);
                    parametros.Add(local);
                    parametros.Add(dataHoraEvento);
                    parametros.Add(dataHoraEventoAte);
                    parametros.Add(preco);
                    parametros.Add(fotoCapa);
                    parametros.Add(tipo);
                    parametros.Add(horario);
                    parametros.Add(destaque);
                    parametros.Add(ativo);
                    parametros.Add(estoque);
                    parametros.Add(relacionado);
                    parametros.Add(eventoPai);
                    parametros.Add(duracao);

                    dados.AlterarEvento(parametros, id);
                }
                else
                {
                    erro = "Nenhum campo foi alterado";
                }
            }

            if (!string.IsNullOrEmpty(id) || !string.IsNullOrEmpty(erro))
            {
                return View("Alterar", evento);
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

            dados.DeletarEvento(id);

            return View("Index");
        }

        public IActionResult Detalhes()
        {
            DataModel dados = new DataModel();

            Evento evento = new Evento();

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
                if (!string.IsNullOrEmpty(Request.Form["txtId"].ToString()))
                {
                    id = (Request.Form["txtId"].ToString());
                    erro = string.Empty;
                }
                else
                {
                    erro = "Id não encontrado";
                }
            }

            evento = dados.PesquisarEvento(id);

            if (evento != null && evento.Id != null)
            {
                return View("Detalhes", evento);
            }
            else
            {
                return View("Eventos");
            }
        }

        public IActionResult JSON()
        {
            DataModel dados = new DataModel();

            string json = dados.ListarJson("tb_eventos");

            return View("JSON", json);
        }
    }
}
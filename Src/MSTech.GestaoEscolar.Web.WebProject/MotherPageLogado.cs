using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Reflection;

namespace MSTech.GestaoEscolar.Web.WebProject
{
    public class MotherPageLogado : MotherPage
    {
        #region Propriedades

        /// <summary>
        /// Retorna o ent_id do usuário logado.
        /// </summary>
        public Guid Ent_ID_UsuarioLogado
        {
            get
            {
                if ((__SessionWEB.__UsuarioWEB != null) &&
                    (__SessionWEB.__UsuarioWEB.Usuario != null))
                    return __SessionWEB.__UsuarioWEB.Usuario.ent_id;

                return Guid.Empty;
            }
        }

        #endregion Propriedades

        #region Eventos

        protected virtual void Page_PreInit(object sender, EventArgs e)
        {
            base.Page_PreInit(sender, e);

            // Verifica autenticação do usuário pelo Ticket da autenticação SAML
            if (!UserIsAuthenticated())
            {
                try
                {
                    HttpContext.Current.Response.Redirect("~/logout.ashx", true);
                }
                catch (ThreadAbortException)
                {
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {

            base.OnLoad(e);

            // Caso tenha grupo permissão, verifica as permissões no módulo atual para o usuário.
            if ((__SessionWEB.__UsuarioWEB.GrupoPermissao != null) && (__SessionWEB.__UsuarioWEB.GrupoPermissao.mod_id > 0))
            {
                // Verifica permissão do usuário, caso não tenha nehuma permissão na página redireciona para a Index.
                if ((!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar) &&
                    (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir) &&
                    (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar) &&
                    (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir))
                {
                    __SessionWEB.PostMessages = CoreSSO.BLL.UtilBO.GetErroMessage("Você não possui permissão para acessar a página solicitada.", CoreSSO.BLL.UtilBO.TipoMensagem.Alerta);
                    RedirecionarPagina("~/Index.aspx");
                }
            }

            // Registra o GATC para a página. Código implementado na MotherPageLogado
            // para que apenas as páginas da área restrita resgistrem o acompanhamento. (Com exceção das paginas abaixo)

            if (!Request.CurrentExecutionFilePath.ToLower().Contains("configuracao/conteudo/visualizaconteudo.aspx")
                && !Request.CurrentExecutionFilePath.ToLower().Contains("configuracao/conteudo/visualizaconteudonovo.aspx")
                && !Request.CurrentExecutionFilePath.ToLower().Contains("configuracao/conteudo/visualizaservico.aspx")
                && !Request.CurrentExecutionFilePath.ToLower().Contains("configuracao/conteudo/visualizacache.aspx")
                && !Request.CurrentExecutionFilePath.ToLower().Contains("configuracao/conteudo/visualizaservice.aspx")
                && !Request.CurrentExecutionFilePath.ToLower().Contains("configuracao/conteudo/correcaofrequenciaacumulada/")
                && !Request.CurrentExecutionFilePath.ToLower().Contains("configuracao/controletarefa/controle.aspx")
                && !Request.CurrentExecutionFilePath.ToLower().Contains("configuracao/conteudo/visualizamovimentacao.aspx")
                && !Request.CurrentExecutionFilePath.ToLower().Contains("configuracao/conteudo/VisualizaModulo.aspx"))
                CoreSSO.BLL.UtilBO.RegistraGATC(Page);
        }

        #endregion Eventos

        #region Métodos

        /// <summary>
        /// Retorna o texto do resource solicitado.
        /// </summary>
        /// <param name="resName">Nome do resource para buscar</param>
        /// <returns></returns>
        //protected string GetResource(string resName)
        //{
        //    return (string)GetGlobalResourceObject(ApplicationWEB.Nome_GlobalResourcesCliente, resName);
        //}

        /// <summary>
        /// Seta para as colunas que aceitam ordenação o Tooltip adequado e configura as
        /// classes de css para a coluna ordenada.
        /// </summary>
        /// <param name="grid">GridView que será ordenado</param>
        protected void ConfiguraColunasOrdenacao(GridView grid)
        {
            ApplicationWEB.ConfiguraColunasOrdenacao(grid);
        }

        /// <summary>
        /// Seta para as colunas que aceitam ordenação o Tooltip adequado e configura as
        /// classes de css para a coluna ordenada.
        /// </summary>
        /// <param name="grid">GridView que será ordenado</param>
        /// <param name="SorExp"></param>
        /// <param name="SortDirect"></param>
        protected void ConfiguraColunasOrdenacao(GridView grid, string SorExp, SortDirection SortDirect)
        {
            ApplicationWEB.ConfiguraColunasOrdenacao(grid, SorExp, SortDirect);
        }

        /// <summary>
        /// Retorna as UA's de acordo com a visão do usuário, se o usuário estiver em um grupo
        /// com visão gestão ou unidade administrativa.
        /// </summary>
        /// <returns>UA's da visão do usuário, ou "" quando for outra visão de usuário.</returns>
        public string UAsVisaoGrupo()
        {
            return ESC_EscolaBO.GetUad_Ids_PermissaoUsuario
                (
                    __SessionWEB.__UsuarioWEB.Grupo.gru_id
                    , __SessionWEB.__UsuarioWEB.Usuario.usu_id
                );
        }

        /// <summary>
        /// Retorna as UA's de acordo com a visão do usuário, se o usuário estiver em um grupo
        /// com visão gestão ou unidade administrativa.
        /// </summary>
        /// <returns>UA's da visão do usuário, ou "" quando for outra visão de usuário.</returns>
        public List<Guid> UAsVisaoGrupoList()
        {
            return ESC_EscolaBO.GetSelect_Uad_Ids_By_PermissaoUsuario
                (
                    __SessionWEB.__UsuarioWEB.Grupo.gru_id
                    , __SessionWEB.__UsuarioWEB.Usuario.usu_id
                );
        }

        /// <summary>
        /// Seta a propriedade Enabled passada para todos os WebControl do ControlCollection
        /// passado.
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="enabled"></param>
        protected void HabilitaControles(ControlCollection controls, bool enabled)
        {
            foreach (Control c in controls)
            {
                if (c.Controls.Count > 0)
                    HabilitaControles(c.Controls, enabled);

                WebControl wb = c as WebControl;

                if (wb != null)
                    wb.Enabled = enabled;
            }
        }

        /// <summary>
        /// Retorna todos os controles da coleção de forma hierárquica, em uma única lista.
        /// </summary>
        /// <param name="collection">Coleção de controles</param>
        /// <returns></returns>
        public static IEnumerable<Control> FindAllControls(ControlCollection collection)
        {
            foreach (Control item in collection)
            {
                yield return item;

                if (item.HasControls())
                {
                    foreach (Control subItem in FindAllControls(item.Controls))
                    {
                        yield return subItem;
                    }
                }
            }
        }

        /// <summary>
        /// Método que remove o asterisco de obrigatório do label informado
        /// </summary>
        /// <param name="lbl"></param>
        protected void RemoveAsteriscoObrigatorio(Label lbl)
        {
            if (lbl.Text.EndsWith(ApplicationWEB.TextoAsteriscoObrigatorio))
                lbl.Text = lbl.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, "");

            else if (lbl.Text.EndsWith("*"))
                lbl.Text = lbl.Text.Replace("*", "");
        }

        /// <summary>
        /// Método que adiciona o asterisco de obrigatório ao label informado
        /// </summary>
        /// <param name="lbl"></param>
        protected void AdicionaAsteriscoObrigatorio(Label lbl)
        {
            if ((!lbl.Text.EndsWith("*")) && (!lbl.Text.EndsWith(ApplicationWEB.TextoAsteriscoObrigatorio)))
                lbl.Text += " *";
        }

        /// <summary>
        /// Metodo que realiza a substituição das chaves de parametro de mensagem pelo valor.
        /// </summary>
        /// <param name="msg">Texto que contém as chaves para substituição</param>
        /// <returns></returns>
        public static string TrocaParametroMensagem(string msg)
        {
            return GestaoEscolarUtilBO.TrocaParametroMensagem(msg);
        }

        #endregion Métodos
    }
}
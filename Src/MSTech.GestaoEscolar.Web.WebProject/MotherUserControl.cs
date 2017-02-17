using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using MSTech.GestaoEscolar.Entities;
using System.Web.UI;
using MSTech.CoreSSO.Entities;
using System.Web;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using System.Threading;
using System.Globalization;

namespace MSTech.GestaoEscolar.Web.WebProject
{
    public class MotherUserControl : MSTech.Web.WebProject.MotherUserControl
    {
        #region PROPRIEDADES

        /// <summary>
        /// Retorna o módulo que o usuário está acessando (carregado no evento OnLoad da página),
        /// de acordo com a Url.
        /// </summary>
        public SYS_Modulo Modulo
        {
            get
            {
                if (HttpContext.Current.Session[SYS_Modulo.SessionName] != null)
                    return (SYS_Modulo)HttpContext.Current.Session[SYS_Modulo.SessionName];

                return new SYS_Modulo();
            }
        }

        public new SessionWEB __SessionWEB
        {
            get
            {
                return (SessionWEB)Session[MSTech.Web.WebProject.ApplicationWEB.SessSessionWEB];
            }
            set
            {
                Session[MSTech.Web.WebProject.ApplicationWEB.SessSessionWEB] = value;
            }
        }

        #endregion

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
        /// Grava o erro, e seta mensagem de erro com a operação no label informado.
        /// </summary>
        /// <param name="ex">Excessão disparada</param>
        /// <param name="label">Label para mostrar a mensagem</param>
        /// <param name="operacao">Operação que originou o erro</param>
        public void TrataErro(Exception ex, Label label, string operacao)
        {
            ApplicationWEB._GravaErro(ex);
            label.Text = UtilBO.GetErroMessage("Erro ao tentar " + operacao + ".",
                                               UtilBO.TipoMensagem.Erro);
        }

        /// <summary>
        /// Retorna as UA's de acordo com a visão do usuário, se o usuário estiver em um grupo 
        /// com visão gestão ou unidade administrativa.
        /// </summary>
        /// <returns>UA's da visão do usuário, ou "" quando for outra visão de usuário.</returns>
        protected string UAsVisaoGrupo()
        {
            if (__SessionWEB.__UsuarioWEB.Usuario.usu_id != Guid.Empty)
            {
                MotherPageLogado motherPage = new MotherPageLogado();
                return motherPage.UAsVisaoGrupo();
            }

            return "";
        }

        /// <summary>
        /// Método de validação de campos data para ser usado em Validators.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void ValidarData_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                SqlDateTime d = SqlDateTime.Parse(DateTime.Parse(args.Value).ToString("MM/dd/yyyy"));
                args.IsValid = true;
            }
            catch
            {
                args.IsValid = false;
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
        /// Método que remove o asterisco de obrigatório do texto informado
        /// </summary>
        /// <param name="text"></param>
        protected string RemoveAsteriscoObrigatorio(string text)
        {
            if (text.EndsWith(ApplicationWEB.TextoAsteriscoObrigatorio))
                text = text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, "");

            else if (text.EndsWith("*"))
                text = text.Replace("*", "");

            return text.Trim();
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
        /// Metodo que realiza a substituição das chaves de parametro de mensagem pelo valor.
        /// </summary>
        /// <param name="msg">Texto que contém as chaves para substituição</param>
        /// <returns></returns>
        protected static string TrocaParametroMensagem(string msg)
        {
            return MotherPageLogado.TrocaParametroMensagem(msg);
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
        /// Registra os parâmetros da mensagem de sair.
        /// </summary>
        public void RegistrarParametrosMensagemSair(bool MensagemSair, bool visaoDocente)
        {
            if (!Page.ClientScript.IsClientScriptBlockRegistered("MensagemSair"))
            {
                string exibeMensagemSair = (MensagemSair && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_MENSAGEM_SAIR_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id)).ToString();
                string exibeMensagemPadraoNavegadorSair = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_MENSAGEM_NAVEGADOR_PADRAO_SAIR_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToString();
                string exibeMensagemNavegadorComJQuery = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_MENSAGEM_NAVEGADOR_COM_MENSAGEM_JQUERY, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToString();
                string mensagemNavegadorSair = string.Format("'{0}'", visaoDocente ? (string)GetGlobalResourceObject(ApplicationWEB.Nome_GlobalResourcesCliente, "MSG_CONFIRMACAO_SAIR_TELA_DOCENTE") : (string)GetGlobalResourceObject(ApplicationWEB.Nome_GlobalResourcesCliente, "MSG_CONFIRMACAO_SAIR_TELA"));

                string script = "var exibeMensagemSair = " + exibeMensagemSair.ToLower() + ";" +
                                "var exibeMensagemSairParametro = " + exibeMensagemSair.ToLower() + ";" +
                                "var exibeMensagemPadraoNavegadorSair = " + exibeMensagemPadraoNavegadorSair.ToLower() + ";" +
                                "var exibeMensagemNavegadorComJQuery = " + exibeMensagemNavegadorComJQuery.ToLower() + ";" +
                                "var mensagemNavegadorSair = " + mensagemNavegadorSair + ";" +
                                "var msgSairTelaBotaoSim='" + GetGlobalResourceObject(ApplicationWEB.Nome_GlobalResourcesCliente, "MSG_SAIR_TELA_BOTAO_SIM").ToString() + "';" +
                                "var msgSairTelaBotaoNao='" + GetGlobalResourceObject(ApplicationWEB.Nome_GlobalResourcesCliente, "MSG_SAIR_TELA_BOTAO_NAO").ToString() + "';";
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "MensagemSair", string.Format("<script type='text/javascript'>{0}</script>", script));
            }
        }

        #endregion

        #region Eventos

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            string lang = "pt-BR";

            if (__SessionWEB != null
                && __SessionWEB.__UsuarioWEB != null
                && __SessionWEB.__UsuarioWEB.language != null)
                lang = __SessionWEB.__UsuarioWEB.language;

            this.Page.UICulture = lang;
            this.Page.Culture = lang;

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(lang);
        }

        #endregion Eventos
    }
}

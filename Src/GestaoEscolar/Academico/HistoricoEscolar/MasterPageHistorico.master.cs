using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.Academico.HistoricoEscolar
{
    public partial class MasterPageHistorico : MotherMasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager sm = ScriptManager.GetCurrent(this.Page);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsHistoricoEscolar.js"));
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsUCCadastroEndereco.js"));
                    sm.Services.Add(new ServiceReference("~/WSServicos.asmx"));
                }

                if (!IsPostBack)
                {
                    InfoComplementarAluno1.HistoricoEscolar = true;
                    InfoComplementarAluno1.InformacaoComplementarAluno(Convert.ToInt64(Session["alu_id"]), null, true);

                    // Controla a visibilidade das abas
                    SYS_Modulo modulo;
                    SYS_GrupoPermissao grupoPermissao;

                    // Dados Aluno
                    string url = VirtualPathUtility.ToAbsolute("~/Academico/HistoricoEscolar/DadosAluno.aspx");
                    grupoPermissao = GestaoEscolarUtilBO.GetGrupoPermissao_Grupo_By_Url(ApplicationWEB.SistemaID, __SessionWEB.__UsuarioWEB.Grupo.gru_id, url, out modulo, ApplicationWEB.AppMinutosCacheLongoGeral);
                    liDadosAluno.Visible = (modulo != null && (grupoPermissao.grp_inserir || grupoPermissao.grp_excluir || grupoPermissao.grp_consultar || grupoPermissao.grp_alterar));
                    if (!liDadosAluno.Visible && Request.Url.AbsolutePath.Equals("/Academico/HistoricoEscolar/DadosAluno.aspx"))
                    {
                        Response.Redirect("Busca.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }

                    // Ensino fundamental
                    url = VirtualPathUtility.ToAbsolute("~/Academico/HistoricoEscolar/EnsinoFundamental.aspx");
                    grupoPermissao = GestaoEscolarUtilBO.GetGrupoPermissao_Grupo_By_Url(ApplicationWEB.SistemaID, __SessionWEB.__UsuarioWEB.Grupo.gru_id, url, out modulo, ApplicationWEB.AppMinutosCacheLongoGeral);
                    liEnsinoFundamental.Visible = (modulo != null && (grupoPermissao.grp_inserir || grupoPermissao.grp_excluir || grupoPermissao.grp_consultar || grupoPermissao.grp_alterar));
                    if (!liEnsinoFundamental.Visible && Request.Url.AbsolutePath.Equals("/Academico/HistoricoEscolar/EnsinoFundamental.aspx"))
                        RedirecionaDadosAluno();

                    // Transferencias
                    url = VirtualPathUtility.ToAbsolute("~/Academico/HistoricoEscolar/Transferencia.aspx");
                    grupoPermissao = GestaoEscolarUtilBO.GetGrupoPermissao_Grupo_By_Url(ApplicationWEB.SistemaID, __SessionWEB.__UsuarioWEB.Grupo.gru_id, url, out modulo, ApplicationWEB.AppMinutosCacheLongoGeral);
                    liTransferencia.Visible = (modulo != null && (grupoPermissao.grp_inserir || grupoPermissao.grp_excluir || grupoPermissao.grp_consultar || grupoPermissao.grp_alterar));
                    if (!liTransferencia.Visible && Request.Url.AbsolutePath.Equals("/Academico/HistoricoEscolar/Transferencia.aspx"))
                        RedirecionaDadosAluno();

                    // Informações complementares
                    url = VirtualPathUtility.ToAbsolute("~/Academico/HistoricoEscolar/InformacoesComplementares.aspx");
                    grupoPermissao = GestaoEscolarUtilBO.GetGrupoPermissao_Grupo_By_Url(ApplicationWEB.SistemaID, __SessionWEB.__UsuarioWEB.Grupo.gru_id, url, out modulo, ApplicationWEB.AppMinutosCacheLongoGeral);
                    liInformacoesComplementares.Visible = (modulo != null && (grupoPermissao.grp_inserir || grupoPermissao.grp_excluir || grupoPermissao.grp_consultar || grupoPermissao.grp_alterar));
                    if (!liInformacoesComplementares.Visible && Request.Url.AbsolutePath.Equals("/Academico/HistoricoEscolar/InformacoesComplementares.aspx"))
                        RedirecionaDadosAluno();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("Documentos", "HistoricoEscolar.HistoricoEscolar.lblMessage.msgErro"), UtilBO.TipoMensagem.Erro);
            }
        }

        private void RedirecionaDadosAluno()
        {
            Response.Redirect("DadosAluno.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}
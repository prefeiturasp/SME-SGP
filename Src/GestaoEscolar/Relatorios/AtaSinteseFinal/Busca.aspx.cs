using DevExpress.XtraReports.UI;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using System;
using System.Web;
using System.Web.UI;
using MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo;
using ReportNameGestaoAcademica = MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica;
using System.Linq;

namespace GestaoEscolar.Relatorios.AtaSinteseFinal
{
    public partial class Busca : MotherPageLogado
    {
        #region Constantes

        #endregion Constantes

        #region Propriedades

        /// <summary>
        /// Array de níveis de ensino que serão carregados no combo de curso.
        /// </summary>
        public int[] VS_FiltroTipoNivelEnsino
        {
            get
            {
                if (ViewState["VS_FiltroTipoNivelEnsino"] == null)
                {
                    int tne_idInfantil = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    ViewState["VS_FiltroTipoNivelEnsino"] = ACA_TipoNivelEnsinoBO.SelecionaTipoNivelEnsino().Select().Select(p => Convert.ToInt32(p["tne_id"])).Where(p => p != tne_idInfantil).ToArray();
                }

                return (int[])(ViewState["VS_FiltroTipoNivelEnsino"]);
            }
        }

        #endregion Propriedades

        #region Métodos

        protected void InicializarPagina()
        {
            UCBuscaDocenteTurma._VS_MostarComboEscola = false;
            divPeriodoCalendario.Visible = true;
            UCBuscaDocenteTurma.ComboTurma.Obrigatorio &= UCBuscaDocenteTurma._VS_doc_id > 0;
            UCCPeriodoCalendario.MostrarMensagemSelecione = true;
            UCCPeriodoCalendario.Obrigatorio = true;
            UCBuscaDocenteTurma.ComboEscola.ObrigatorioUA = true;
            UCBuscaDocenteTurma.ComboEscola.ObrigatorioEscola = true;
            UCBuscaDocenteTurma.ComboCalendario.Obrigatorio = true;
            UCBuscaDocenteTurma.ComboCurriculoPeriodo.Obrigatorio = true;
            UCBuscaDocenteTurma.ComboCursoCurriculo.Obrigatorio = true;
            UCBuscaDocenteTurma.ComboTurma.Obrigatorio = true;
            UCBuscaDocenteTurma.ComboCalendario.PermiteEditar = true;
            UCBuscaDocenteTurma.ComboCalendario.Carregar();
            UCBuscaDocenteTurma.VS_FiltroTipoNivelEnsino = VS_FiltroTipoNivelEnsino;
            UCBuscaDocenteTurma._VS_alteraCalendario = false;
            divPeriodoCalendario.Visible = true;
            UCCPeriodoCalendario.SelecionaPeriodoAtualAoCarregar = true;

            btnGerar.Visible =
            UCBuscaDocenteTurma.Visible =
            UCCamposObrigatorios.Visible = true;

            UCCPeriodoCalendario.MostrarMensagemSelecioneAnual = !UCCPeriodoCalendario.MostrarMensagemSelecione;
            UCBuscaDocenteTurma.InicializaCamposBusca();
        }

        #endregion Métodos

        #region Eventos

        protected void btnGerar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string report, parametros;
                parametros = string.Empty;
                XtraReport DevReport = null;
                SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);

                //  Realizar a chamada do relatório aqui, conforme o exemplo abaixo
                //
                //report = ((int)MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademicaDocumentosDocente.DocDctRelTarjetaBimestral).ToString();
                //
                //parametros = "esc_id=" + UCBuscaDocenteTurma.ComboEscola.Esc_ID +
                //             "&uni_id=" + UCBuscaDocenteTurma.ComboEscola.Uni_ID +
                //             "&cal_id=" + UCBuscaDocenteTurma.ComboCalendario.Valor +
                //             "&cur_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0] +
                //             "&crr_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1] +
                //             "&crp_id=" + UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2] +
                //             "&tur_id=" + UCBuscaDocenteTurma.ComboTurma.Valor[0] +
                //             "&tpc_id=" + UCCPeriodoCalendario.Valor[0] +
                //             "&tud_id=-1" +
                //             "&nomePeriodoCalendario=" + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario() +
                //             "&nomeDisciplina=" + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                //             "&visaoDocente=" + ((UCBuscaDocenteTurma._VS_doc_id > 0) && (__SessionWEB.__UsuarioWEB.Grupo.vis_id == 4)) +
                //             "&tituloRelatorio=Ata Síntese dos Resultados de avaliação" +
                //             "&mostraCodigoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleano(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO) +
                //             "&corAlunoInativo=" + ApplicationWEB.AlunoInativo +
                //             "&situacaoAlunoInativo=" + (byte)ACA_AlunoSituacao.Inativo +
                //             "&corAlunoFrequenciaLimite=" + ApplicationWEB.AlunoFrequenciaLimite;

                //MSTech.GestaoEscolar.BLL.CFG_RelatorioBO.SendParametersToReport(report, parametros);
                //Response.Redirect(String.Format("~/Documentos/Relatorio.aspx?dummy='{0}'", HttpUtility.UrlEncode(sa.Encrypt(report))), false);
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager sm = ScriptManager.GetCurrent(this);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                }

                if (!IsPostBack)
                {
                    InicializarPagina();
                }

                UCBuscaDocenteTurma.IndexChanged_Calendario += UCBuscaDocenteTurma_IndexChanged_Calendario;
            }
            catch (Exception error)
            {
                ApplicationWEB._GravaErro(error);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMessage.Text = message;
                // Inserção do título do relatório
                lgdTitulo.InnerText = Modulo.mod_nome;
            }
        }

        #endregion Eventos

        #region Delegates

        private void UCBuscaDocenteTurma_IndexChanged_Calendario()
        {
            UCCPeriodoCalendario.CarregarPorCalendario(-1);
            try
            {
                if (UCBuscaDocenteTurma.ComboCalendario.Valor > 0)
                {
                    UCCPeriodoCalendario.CarregarPorCalendario(UCBuscaDocenteTurma.ComboCalendario.Valor);
                    UCCPeriodoCalendario.SetarFoco();
                    divPeriodoCalendario.Visible = UCCPeriodoCalendario.PermiteEditar = true;
                }
                else
                    divPeriodoCalendario.Visible = UCCPeriodoCalendario.PermiteEditar = false;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }




        #endregion Delegates

    }
}
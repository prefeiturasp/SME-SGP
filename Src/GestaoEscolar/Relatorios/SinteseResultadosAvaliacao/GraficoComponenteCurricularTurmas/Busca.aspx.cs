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
using System.Collections.Generic;

namespace GestaoEscolar.Relatorios.SinteseResultadosAvaliacao.GraficoComponenteCurricularTurmas
{
    public partial class Busca : MotherPageLogado
    {
        #region Métodos

        /// <summary>
        /// Inicializa os filtros da pagina.
        /// </summary>
        protected void InicializarPagina()
        {
            UCBuscaDocenteTurma._VS_AnosAnteriores = true;
            UCBuscaDocenteTurma._VS_MostarComboEscola = false;
            UCBuscaDocenteTurma.ComboTurma.Visible = false;
            UCBuscaDocenteTurma.ComboEscola.ObrigatorioUA = true;
            UCBuscaDocenteTurma.ComboEscola.ObrigatorioEscola = true;
            UCBuscaDocenteTurma.ComboCalendario.Obrigatorio = true;
            UCBuscaDocenteTurma.ComboCurriculoPeriodo.Obrigatorio = true;
            UCBuscaDocenteTurma.ComboCursoCurriculo.Obrigatorio = true;
            divDisciplina.Visible = false;
            UCCPeriodoCalendario.SelecionaPeriodoAtualAoCarregar = true;
            UCCPeriodoCalendario.MostrarMensagemSelecione = true;
            UCCPeriodoCalendario.Obrigatorio = false;
            divPeriodoCalendario.Visible = false;
            UCComboTipoDisciplina._MostrarMessageSelecione = true;
            UCComboTipoDisciplina.Obrigatorio = false;

            btnGerar.Visible =
            UCBuscaDocenteTurma.Visible =
            UCCamposObrigatorios.Visible = true;

            UCCPeriodoCalendario.MostrarMensagemSelecioneAnual = !UCCPeriodoCalendario.MostrarMensagemSelecione;
            UCBuscaDocenteTurma.InicializaCamposBusca();

            UCBuscaDocenteTurma.ComboEscola.FocusUA();
        }

        /// <summary>
        /// Verifica se há busca salva e carrega os combos da tela.
        /// </summary>
        protected void CarregarBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.GraficoComponenteCurricularTurmas)
            {
                // Recuperar busca realizada e pesquisar automaticamente
                string valor, valor2;

                long doc_id = -1;
                UCBuscaDocenteTurma.ComboEscola.Inicializar();

                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                {
                    doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                    UCBuscaDocenteTurma.ComboEscola.InicializarVisaoIndividual(doc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    string esc_id;
                    string uni_id;

                    if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                        (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
                    {
                        UCBuscaDocenteTurma.ComboEscola.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                        UCBuscaDocenteTurma.UCComboUAEscola_IndexChangedUnidadeEscola();
                    }
                }
                else
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor);
                    if (!string.IsNullOrEmpty(valor))
                    {
                        UCBuscaDocenteTurma.ComboEscola.Uad_ID = new Guid(valor);
                        UCBuscaDocenteTurma.ComboEscola.CarregaEscolaPorUASuperiorSelecionada();

                        if (UCBuscaDocenteTurma.ComboEscola.Uad_ID != Guid.Empty)
                        {
                            UCBuscaDocenteTurma.ComboEscola.FocoEscolas = true;
                            UCBuscaDocenteTurma.ComboEscola.PermiteAlterarCombos = true;
                        }
                        string esc_id;
                        string uni_id;

                        if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                            (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
                        {
                            UCBuscaDocenteTurma.ComboEscola.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                            UCBuscaDocenteTurma.UCComboUAEscola_IndexChangedUnidadeEscola();
                        }
                    }
                }

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor);
                UCBuscaDocenteTurma.ComboCalendario.Valor = Convert.ToInt32(valor);
                UCBuscaDocenteTurma.UCCCalendario_IndexChanged();
                UCBuscaDocenteTurma_IndexChanged_Calendario();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out valor2);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out valor);
                UCBuscaDocenteTurma.ComboCursoCurriculo.Valor = new[] { Convert.ToInt32(valor2), Convert.ToInt32(valor) };
                UCBuscaDocenteTurma.UCCCursoCurriculo_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_id", out valor);
                if (doc_id <= 0)
                    UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor = new[] { UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0], UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1], Convert.ToInt32(valor) };
                else
                    UCBuscaDocenteTurma._VS_doc_id = doc_id;
                UCBuscaDocenteTurma.UCComboCurriculoPeriodo__OnSelectedIndexChange();
                UCBuscaDocenteTurma_IndexChanged_CurriculoPeriodo();

                if (divPeriodoCalendario.Visible)
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tpc_id", out valor);
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cap_id", out valor2);
                    UCCPeriodoCalendario.Valor = new[] { Convert.ToInt32(valor), Convert.ToInt32(valor2) };
                    UCCPeriodoCalendario_IndexChanged();
                }

                if (divDisciplina.Visible)
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tds_id", out valor);
                    UCComboTipoDisciplina.Valor = Convert.ToInt32(valor);
                }

                updPesquisa.Update();
            }
        }

        /// <summary>
        /// Salva os dados da busca realizada para ser carregada posteriormente.
        /// </summary>
        protected void SalvarBusca()
        {
            Dictionary<string, string> filtros = new Dictionary<string, string>();
            filtros.Add("uad_idSuperior", UCBuscaDocenteTurma.ComboEscola.Uad_ID.ToString());
            filtros.Add("esc_id", UCBuscaDocenteTurma.ComboEscola.Esc_ID.ToString());
            filtros.Add("uni_id", UCBuscaDocenteTurma.ComboEscola.Uni_ID.ToString());
            filtros.Add("cur_id", UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0].ToString());
            filtros.Add("crr_id", UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1].ToString());
            filtros.Add("crp_id", UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2].ToString());
            filtros.Add("cal_id", UCBuscaDocenteTurma.ComboCalendario.Valor.ToString());

            if (divPeriodoCalendario.Visible)
            {
                filtros.Add("tpc_id", UCCPeriodoCalendario.Valor[0].ToString());
                filtros.Add("cap_id", UCCPeriodoCalendario.Valor[1].ToString());
            }

            if (divDisciplina.Visible)
                filtros.Add("tds_id", UCComboTipoDisciplina.Valor.ToString());

            __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.GraficoComponenteCurricularTurmas, Filtros = filtros };
        }

        #endregion Métodos

        #region Eventos

        protected void btnGerar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SalvarBusca();

                string report, parameter;
                parameter = string.Empty;
                report = ((int)ReportNameGestaoAcademica.GraficoSinteseResultadosAvaliacaoComponenteCurricularTurmas).ToString();
                XtraReport DevReport = null;
                SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);

                DevReport = new RelGrafComponenteCurricularTurmas
                            (UCBuscaDocenteTurma.ComboEscola.Esc_ID,
                            UCBuscaDocenteTurma.ComboEscola.Uni_ID,
                            UCBuscaDocenteTurma.ComboCalendario.Valor,
                            UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0],
                            UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1],
                            UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2],
                            UCComboTipoDisciplina.Valor,
                            UCCPeriodoCalendario.Valor[0],
                            UCCPeriodoCalendario.Valor[1],
                            __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                            __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                            (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
                            __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                            ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id),
                            GetGlobalResourceObject("Relatorios", "SinteseResultadosAvaliacao.GraficoComponenteCurricularTurmas.Busca.MensagemAviso").ToString(),
                            GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio").ToString(),
                            GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria").ToString(),
                            ApplicationWEB.LogoRelatorioDB);

                GestaoEscolarUtilBO.SendParametersToReport(DevReport);
                Response.Redirect(String.Format("~/Documentos/RelatorioDev.aspx?dummy='{0}'", HttpUtility.UrlEncode(sa.Encrypt(report))), false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                UCBuscaDocenteTurma.IndexChanged_Calendario += UCBuscaDocenteTurma_IndexChanged_Calendario;
                UCBuscaDocenteTurma.IndexChanged_CurriculoPeriodo += UCBuscaDocenteTurma_IndexChanged_CurriculoPeriodo;
                UCCPeriodoCalendario.IndexChanged += UCCPeriodoCalendario_IndexChanged;
            
                ScriptManager sm = ScriptManager.GetCurrent(this);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                }

                if (!IsPostBack)
                {
                    InicializarPagina();

                    CarregarBusca();
                }
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
            try
            {
                UCCPeriodoCalendario.CarregarPorCalendario(-1);
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

        private void UCBuscaDocenteTurma_IndexChanged_CurriculoPeriodo()
        {

            try
            {
                if (UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[0] > 0 && UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[1] > 0 && UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2] > 0 &&
                    UCBuscaDocenteTurma.ComboEscola.Esc_ID > 0 && UCBuscaDocenteTurma.ComboEscola.Uni_ID > 0 && UCBuscaDocenteTurma.ComboCalendario.Valor > 0)
                {
                    UCComboTipoDisciplina.CarregarTipoDisciplinaPorCursoCurriculoPeriodoEscola(UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[0],
                                                                                               UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[1],
                                                                                               UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2],
                                                                                               UCBuscaDocenteTurma.ComboEscola.Esc_ID,
                                                                                               UCBuscaDocenteTurma.ComboEscola.Uni_ID,
                                                                                               UCBuscaDocenteTurma.ComboCalendario.Valor,
                                                                                               UCCPeriodoCalendario.Valor[1]);

                    UCComboTipoDisciplina.SetarFoco();
                    divDisciplina.Visible = UCComboTipoDisciplina.PermiteEditar = true;
                }
                else
                {
                    if (UCComboTipoDisciplina._Combo.Items.Count > 0)
                        UCComboTipoDisciplina.SelectedIndex = 0;
                    divDisciplina.Visible = UCComboTipoDisciplina.PermiteEditar = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCCPeriodoCalendario_IndexChanged()
        {

            try
            {
                if (UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[0] > 0 && UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[1] > 0 && UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2] > 0 &&
                    UCBuscaDocenteTurma.ComboEscola.Esc_ID > 0 && UCBuscaDocenteTurma.ComboEscola.Uni_ID > 0 && UCBuscaDocenteTurma.ComboCalendario.Valor > 0)
                {
                    UCComboTipoDisciplina.CarregarTipoDisciplinaPorCursoCurriculoPeriodoEscola(UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[0],
                                                                                               UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[1],
                                                                                               UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2],
                                                                                               UCBuscaDocenteTurma.ComboEscola.Esc_ID,
                                                                                               UCBuscaDocenteTurma.ComboEscola.Uni_ID,
                                                                                               UCBuscaDocenteTurma.ComboCalendario.Valor,
                                                                                               UCCPeriodoCalendario.Valor[1]);

                    UCComboTipoDisciplina.SetarFoco();
                    divDisciplina.Visible = UCComboTipoDisciplina.PermiteEditar = true;
                }
                else
                {
                    if (UCComboTipoDisciplina._Combo.Items.Count > 0)
                        UCComboTipoDisciplina.SelectedIndex = 0;
                    divDisciplina.Visible = UCComboTipoDisciplina.PermiteEditar = false;
                }
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
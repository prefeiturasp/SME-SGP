using DevExpress.XtraReports.UI;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Relatorios.GraficoJustificativaFalta
{
    public partial class Busca : MotherPageLogado
    {
        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));

            UCComboCursoCurriculo1.IndexChanged += UCComboCursoCurriculo1_IndexChanged;
            UCComboCurriculoPeriodo1.IndexChanged += UCComboCurriculoPeriodo1__OnSelectedIndexChange;
            UCComboCalendario1.IndexChanged += UCComboCalendario1_IndexChanged;

            if (!IsPostBack)
            {
                try
                {
                    string message = __SessionWEB.PostMessages;
                    if (!String.IsNullOrEmpty(message))
                    {
                        lblMessage.Text = message;
                        __SessionWEB.PostMessages = String.Empty;
                    }

                    if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                    {
                        uppPesquisa.Visible = false;
                        lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Relatorios", "GraficoJustificativaFalta.Busca.lblMessage.ErroPermissao").ToString(), UtilBO.TipoMensagem.Alerta);
                    }

                    Page.Form.DefaultFocus = UCComboUAEscola.ComboEscola_ClientID;
                    Page.Form.DefaultButton = btnGerarRel.UniqueID;

                    string nomeMatriculaEstadual = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    bool mostraMatriculaEstadual = !string.IsNullOrEmpty(nomeMatriculaEstadual);

                    Inicializar();
                    VerificaBusca();

                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Relatorios", "GraficoJustificativaFalta.Busca.lblMessage.ErroCarregarSistema").ToString(), UtilBO.TipoMensagem.Erro);
                }

            }
        }

        #endregion Page Life Cycle

        #region Eventos

        protected void btnGerarRel_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                    GerarRel();
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Relatorios", "GraficoJustificativaFalta.Busca.lblMessage.ErroGerarRelatorio").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Eventos

        #region Delegates

        protected void UCComboUAEscola_IndexChangedUA()
        {
            if (UCComboUAEscola.Uad_ID == Guid.Empty)
                UCComboUAEscola.SelectedValueEscolas = new[] { -1, -1 };

            UCComboUAEscola_IndexChangedUnidadeEscola();
        }

        protected void UCComboUAEscola_IndexChangedUnidadeEscola()
        {
            try
            {
                if (UCComboUAEscola.Esc_ID > 0 && UCComboUAEscola.Uni_ID > 0)
                {
                    UCComboCalendario1.SetarFoco();
                }

                UCComboCalendario1_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Relatorios", "GraficoJustificativaFalta.Busca.lblMessage.ErroCarregarDados").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCComboCalendario1_IndexChanged()
        {
            try
            {
                UCComboCursoCurriculo1.Valor = new[] { -1, -1 };
                UCComboCursoCurriculo1.PermiteEditar = false;

                if (UCComboUAEscola.Esc_ID > 0 && UCComboUAEscola.Uni_ID > 0 && UCComboCalendario1.Valor > 0)
                {
                    UCComboCursoCurriculo1.CarregarPorEscolaCalendarioSituacaoCurso(UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID, UCComboCalendario1.Valor, 0);

                    UCComboCursoCurriculo1.SetarFoco();
                    UCComboCursoCurriculo1.PermiteEditar = true;
                }

                UCComboCursoCurriculo1_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Relatorios", "GraficoJustificativaFalta.Busca.lblMessage.ErroCarregarDados").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCComboCursoCurriculo1_IndexChanged()
        {
            try
            {
                UCComboCurriculoPeriodo1.Valor = new[] { -1, -1, -1 };
                UCComboCurriculoPeriodo1.PermiteEditar = false;

                if (UCComboUAEscola.Esc_ID > 0 && UCComboUAEscola.Uni_ID > 0 && UCComboCalendario1.Valor > 0 && UCComboCursoCurriculo1.Valor[0] > 0)
                {
                    UCComboCurriculoPeriodo1.CarregarPorCursoCurriculo(UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1]);
                    UCComboCurriculoPeriodo1.PermiteEditar = true;
                    UCComboCurriculoPeriodo1.Focus();
                }

                UCComboCurriculoPeriodo1__OnSelectedIndexChange();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Relatorios", "GraficoJustificativaFalta.Busca.lblMessage.ErroCarregarDados").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCComboCurriculoPeriodo1__OnSelectedIndexChange()
        {
            try
            {
                UCComboTurma1.Valor = new long[] { -1, -1, -1 };
                UCComboTurma1.PermiteEditar = false;

                if (UCComboUAEscola.Esc_ID > 0 && UCComboUAEscola.Uni_ID > 0 && UCComboCalendario1.Valor > 0 && UCComboCursoCurriculo1.Valor[0] > 0 && UCComboCurriculoPeriodo1.Valor[0] > 0)
                {
                    UCComboTurma1.CarregarPorEscolaCurriculoCalendario_TurmasNormais(UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID, UCComboCurriculoPeriodo1.Valor[0], UCComboCurriculoPeriodo1.Valor[1], UCComboCurriculoPeriodo1.Valor[2], UCComboCalendario1.Valor);
                    UCComboTurma1.PermiteEditar = true;
                    UCComboTurma1.Focus();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Relatorios", "GraficoJustificativaFalta.Busca.lblMessage.ErroCarregarDados").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Delegates

        #region Métodos

        /// <summary>
        /// Inicializa os filtros da pagina.
        /// </summary>
        protected void Inicializar()
        {
            UCComboCalendario1.Carregar();
            UCComboUAEscola.Inicializar();

            if (UCComboUAEscola.VisibleUA)
                UCComboUAEscola_IndexChangedUA();
            else
                UCComboUAEscola.ObrigatorioEscola = true;
        }

        /// <summary>
        /// Verifica se há busca salva e carrega os combos da tela.
        /// </summary>
        private void VerificaBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.GraficoJustificativaFalta)
            {
                // Recuperar busca realizada e pesquisar automaticamente
                string valor, valor1, valor2;

                // UA Escola
                if (UCComboUAEscola.FiltroEscola)
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor);

                    if (!string.IsNullOrEmpty(valor))
                    {
                        UCComboUAEscola.Uad_ID = new Guid(valor);
                    }

                    UCComboUAEscola.EnableEscolas = (UCComboUAEscola.Uad_ID != Guid.Empty);

                    if (UCComboUAEscola.Uad_ID != Guid.Empty)
                    {
                        UCComboUAEscola.CarregaEscolaPorUASuperiorSelecionada();
                        SelecionarEscola();
                    }
                }
                else
                {
                    SelecionarEscola();
                }
                UCComboUAEscola_IndexChangedUnidadeEscola();

                // Calendario
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor);
                UCComboCalendario1.Valor = Convert.ToInt32(valor);
                UCComboCalendario1_IndexChanged();

                // Curso
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out valor1);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_id", out valor2);
                UCComboCursoCurriculo1.Valor = new[] { Convert.ToInt32(valor), Convert.ToInt32(valor1) };
                UCComboCursoCurriculo1_IndexChanged();
                UCComboCurriculoPeriodo1.Valor = new[] { Convert.ToInt32(valor), Convert.ToInt32(valor1), Convert.ToInt32(valor2) };
                UCComboCurriculoPeriodo1__OnSelectedIndexChange();

                // Turma
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_id", out valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_id", out valor1);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ttn_id", out valor2);
                UCComboTurma1.Valor = new[] { Convert.ToInt64(valor), Convert.ToInt64(valor1), Convert.ToInt64(valor2) };
            }
        }

        /// <summary>
        /// Seleciona a escola no combo de acordo com o parâmetro salvo na sessão de busca
        /// realizada.
        /// </summary>
        private void SelecionarEscola()
        {
            string esc_id;
            string uni_id;

            if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
            {
                UCComboUAEscola.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
            }
        }

        /// <summary>
        /// Gera o relatorio com base nos filtros selecionados.
        /// </summary>
        private void GerarRel()
        {
            SalvarBusca();

            string report, parameter;
            parameter = string.Empty;
            report = ((int)MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica.GraficoConsolidadoAtividadeAvaliativa).ToString();
            XtraReport DevReport = null;
            SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);

            string DRE = "";
            string escola = "";
            string filtros = "";

            if (UCComboUAEscola.Uad_ID != Guid.Empty)
                DRE = UCComboUAEscola.TextoComboUA;
            else if (UCComboUAEscola.Esc_ID > 0)
            {
                ESC_Escola esc = new ESC_Escola { esc_id = UCComboUAEscola.Esc_ID };
                ESC_EscolaBO.GetEntity(esc);
                SYS_UnidadeAdministrativa uadSup = new SYS_UnidadeAdministrativa { uad_id = esc.uad_idSuperiorGestao, ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id };
                SYS_UnidadeAdministrativaBO.GetEntity(uadSup);

                DRE = uadSup.uad_nome;
            }
            if (UCComboUAEscola.Esc_ID > 0)
                escola = UCComboUAEscola.TextoComboEscola;

            if (UCComboCursoCurriculo1.Valor[0] > 0)
                filtros = UCComboCursoCurriculo1.Texto;
            if (UCComboCurriculoPeriodo1.Valor[2] > 0)
                filtros += (string.IsNullOrEmpty(filtros) ? "" : " - ") + UCComboCurriculoPeriodo1.Texto;
            if (UCComboTurma1.Valor[0] > 0)
                filtros += (string.IsNullOrEmpty(filtros) ? "" : " - ") + UCComboTurma1.Texto;

            DevReport = new MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo.GraficoJustificativaFalta
                        (UCComboUAEscola.Uad_ID, 
                        UCComboUAEscola.Esc_ID,
                        UCComboUAEscola.Uni_ID,
                        UCComboCalendario1.Valor,
                        UCComboCursoCurriculo1.Valor[0],
                        UCComboCursoCurriculo1.Valor[1],
                        UCComboCurriculoPeriodo1.Valor[2],
                        UCComboTurma1.Valor[0],
                        __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                        __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                        (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                        GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio").ToString(),
                        GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria").ToString(),
                        DRE,
                        escola,
                        UCComboCalendario1.Texto,
                        filtros,
                        ApplicationWEB.LogoRelatorioDB);

            GestaoEscolarUtilBO.SendParametersToReport(DevReport);
            Response.Redirect(String.Format("~/Documentos/RelatorioDev.aspx?dummy='{0}'", HttpUtility.UrlEncode(sa.Encrypt(report))), false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Salva os dados da busca realizada para ser carregada posteriormente.
        /// </summary>
        private void SalvarBusca()
        {
            Dictionary<string, string> filtros = new Dictionary<string, string>();
            filtros.Add("uad_idSuperior", UCComboUAEscola.Uad_ID.ToString());
            filtros.Add("esc_id", UCComboUAEscola.Esc_ID.ToString());
            filtros.Add("uni_id", UCComboUAEscola.Uni_ID.ToString());
            filtros.Add("cur_id", UCComboCursoCurriculo1.Valor[0].ToString());
            filtros.Add("crr_id", UCComboCursoCurriculo1.Valor[1].ToString());
            filtros.Add("crp_id", UCComboCurriculoPeriodo1.Valor[2].ToString());
            filtros.Add("cal_id", UCComboCalendario1.Valor.ToString());
            filtros.Add("tur_id", UCComboTurma1.Valor[0].ToString());
            filtros.Add("crp_idTurma", UCComboTurma1.Valor[1].ToString());
            filtros.Add("ttn_id", UCComboTurma1.Valor[2].ToString());

            __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.GraficoJustificativaFalta, Filtros = filtros };
        }

        #endregion Métodos
    }
}
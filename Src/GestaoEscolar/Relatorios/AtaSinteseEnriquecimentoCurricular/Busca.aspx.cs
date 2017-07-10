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
using System.Data;
using System.Linq;
using MSTech.CoreSSO.Entities;
using System.Collections.Generic;
using MSTech.GestaoEscolar.Entities;

namespace GestaoEscolar.Relatorios.AtaSinteseEnriquecimentoCurricular
{
    public partial class Busca : MotherPageLogado
    {
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

        /// <summary>
        /// Inicializa os filtros da pagina.
        /// </summary>
        protected void InicializarPagina()
        {
            UCBuscaDocenteTurma._VS_AnosAnteriores = true;
            UCBuscaDocenteTurma._VS_MostarComboEscola = false;
            UCBuscaDocenteTurma._VS_CarregarApenasTurmasNormais = true;
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
            UCBuscaDocenteTurma._VS_alteraCalendario = false;
            UCBuscaDocenteTurma.VS_FiltroTipoNivelEnsino = VS_FiltroTipoNivelEnsino;
            divPeriodoCalendario.Visible = true;
            UCCPeriodoCalendario.SelecionaPeriodoAtualAoCarregar = true;

            btnGerar.Visible =
            UCBuscaDocenteTurma.Visible =
            UCCamposObrigatorios.Visible = true;

            UCCPeriodoCalendario.MostrarMensagemSelecioneAnual = !UCCPeriodoCalendario.MostrarMensagemSelecione;
            UCBuscaDocenteTurma.InicializaCamposBusca();
        }

        /// <summary>
        /// Verifica se há busca salva e carrega os combos da tela.
        /// </summary>
        protected void CarregarBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.AtaSinteseEnriquecimentoCurricular)
            {
                // Recuperar busca realizada e pesquisar automaticamente
                string valor, valor2, valor3;

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

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_id", out valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ttn_id", out valor2);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_idTurma", out valor3);
                UCBuscaDocenteTurma.ComboTurma.Valor = new[] { Convert.ToInt64(valor), Convert.ToInt64(valor3), Convert.ToInt64(valor2) };

                if (divPeriodoCalendario.Visible)
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tpc_id", out valor);
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cap_id", out valor2);
                    UCCPeriodoCalendario.Valor = new[] { Convert.ToInt32(valor), Convert.ToInt32(valor2) };
                }

                uppPesquisa.Update();
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
            filtros.Add("tur_id", UCBuscaDocenteTurma.ComboTurma.Valor[0].ToString());
            filtros.Add("crp_idTurma", UCBuscaDocenteTurma.ComboTurma.Valor[1].ToString());
            filtros.Add("ttn_id", UCBuscaDocenteTurma.ComboTurma.Valor[2].ToString());

            if (divPeriodoCalendario.Visible)
            {
                filtros.Add("tpc_id", UCCPeriodoCalendario.Valor[0].ToString());
                filtros.Add("cap_id", UCCPeriodoCalendario.Valor[1].ToString());
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.AtaSinteseEnriquecimentoCurricular, Filtros = filtros };
        }

        #endregion Métodos

        #region Eventos

        protected void btnGerar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SalvarBusca();

                // mostra a quantidade de dias letivos no ano
                int numeroDiasUteis = GestaoEscolarUtilBO.NumeroDeDiasUteis(UCBuscaDocenteTurma.ComboCalendario.Valor, UCBuscaDocenteTurma.ComboEscola.Esc_ID, UCBuscaDocenteTurma.ComboEscola.Uni_ID, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                string report, parametros;
                parametros = string.Empty;
                SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);

                report = ((int)MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademicaDocumentosDocente.DocDctRelSinteseEnriquecimentoCurricular).ToString();

                string parametroAtivDiversificada = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_ATIVIDADE_DIVERSIFICADA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                parametros = "esc_id=" + UCBuscaDocenteTurma.ComboEscola.Esc_ID +
                             "&uni_id=" + UCBuscaDocenteTurma.ComboEscola.Uni_ID +
                             "&cal_id=" + UCBuscaDocenteTurma.ComboCalendario.Valor +
                             "&cur_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0] +
                             "&crr_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1] +
                             "&crp_id=" + UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2] +
                             "&tur_id=" + UCBuscaDocenteTurma.ComboTurma.Valor[0] +
                             "&tpc_id=" + UCCPeriodoCalendario.Valor[0] +
                             "&tud_id=-1" +
                             "&nomePeriodoCalendario=" + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                             "&nomeDisciplina=" + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                             "&visaoDocente=" + ((UCBuscaDocenteTurma._VS_doc_id > 0) && (__SessionWEB.__UsuarioWEB.Grupo.vis_id == 4)) +
                             "&tituloRelatorio=Ata Síntese de Enriquecimento curricular" +
                             "&mostraCodigoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                             "&corAlunoInativo=" + ApplicationWEB.AlunoInativo +
                             "&situacaoAlunoInativo=" + (byte)ACA_AlunoSituacao.Inativo +
                             "&corAlunoFrequenciaLimite=" + ApplicationWEB.AlunoFrequenciaLimite +
                             "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio") +
                             "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria") +
                             "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                                                , ApplicationWEB.LogoRelatorioSSRS) +
                             "&tev_id=" + (string.IsNullOrEmpty(parametroAtivDiversificada) ? "0" : parametroAtivDiversificada) +
                             "&numeroDiasUteis=" + numeroDiasUteis +
                             "&visaoGestor=" + (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa) +
                             "&cal_ano=" + UCBuscaDocenteTurma.ComboCalendario.Cal_ano.ToString() +
                             "&documentoOficial=true" +
                             "&mensagemFrequenciaExterna=" + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MENSAGEM_FREQUENCIA_EXTERNA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                MSTech.GestaoEscolar.BLL.CFG_RelatorioBO.CallReport("Documentos", report, parametros, HttpContext.Current);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                UCBuscaDocenteTurma.IndexChanged_Calendario += UCBuscaDocenteTurma_IndexChanged_Calendario;
                
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
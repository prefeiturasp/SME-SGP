using DevExpress.XtraReports.UI;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo;
using MSTech.Validation.Exceptions;
using System.Data;
using MSTech.GestaoEscolar.Entities;
using ReportNameGestaoAcademicaDocumentosDocente = MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademicaDocumentosDocente;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;
using CFG_ServidorRelatorioBO = MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO;
using MSTech.CoreSSO.Entities;
using System.Linq;

namespace GestaoEscolar.Documentos.DocumentoDocente
{
    public partial class Busca : MotherPageLogado
    {
        #region Constantes

        /// <summary>
        /// Constantes dos relatórios de documentos do docente
        /// </summary>
        private const int DocDctDiarioClasseFrequencia = (int)ReportNameGestaoAcademicaDocumentosDocente.DocDctDiarioClasseFrequencia
                            , DocDctDiarioClasseAvaliacao = (int)ReportNameGestaoAcademicaDocumentosDocente.DocDctDiarioClasseAvaliacao
                            , DocDctGraficoAtividadeAvaliativa = (int)ReportNameGestaoAcademicaDocumentosDocente.DocDctGraficoAtividadeAvaliativa
                            , DocDctRelAtividadeAvaliativa = (int)ReportNameGestaoAcademicaDocumentosDocente.DocDctRelAtividadeAvaliativa
                            , DocDctRelTarjetaBimestral = (int)ReportNameGestaoAcademicaDocumentosDocente.DocDctRelTarjetaBimestral
                            , DocDctRelSinteseEnriquecimentoCurricular = (int)ReportNameGestaoAcademicaDocumentosDocente.DocDctRelSinteseEnriquecimentoCurricular
                            , DocDctRelFrequenciaBimestral = (int)ReportNameGestaoAcademicaDocumentosDocente.DocDctRelFrequenciaBimestral
                            , DocDctRelSinteseAula = (int)ReportNameGestaoAcademicaDocumentosDocente.DocDctRelSinteseAula
                            , DocDctRelDadosPlanejamento = (int)ReportNameGestaoAcademicaDocumentosDocente.DocDctRelDadosPlanejamento
                            , DocDctRelOrientacaoAlcancada = (int)ReportNameGestaoAcademicaDocumentosDocente.DocDctRelOrientacaoAlcancada
                            , DocDctRelAnotacoesAula = (int)ReportNameGestaoAcademicaDocumentosDocente.DocDctRelAnotacoesAula
                            , DocDctPlanoAnual = (int)ReportNameGestaoAcademicaDocumentosDocente.DocDctPlanoAnual
                            , DocDctPlanoCicloSerie = (int)ReportNameGestaoAcademicaDocumentosDocente.DocDctPlanoCicloSerie
                            , DocDctAlunosPendenciaEfetivacao = (int)ReportNameGestaoAcademicaDocumentosDocente.DocDctAlunosPendenciaEfetivacao
                            ;

        /// <summary>
        /// Constante usada para informar qual é o índice coluna
        /// Curso do GridView _grvDocumentoAluno.
        /// </summary>
        protected const int cellCurso = 6;

        /// <summary>
        /// Constante usada para informar qual é o índice coluna
        /// Matrícula estadual do GridView _grvDocumentoAluno.
        /// </summary>
        protected const int columnMatricula = 2;

        /// <summary>
        /// Constante usada para informar qual é o índice coluna
        /// Matrícula do GridView _grvDocumentoAluno.
        /// </summary>
        protected const int columnMatriculaEstadual = 1;

        #endregion Constantes

        #region Atributos/Propriedades

        /// <summary>
        /// Retorna se o usuário logado é docente.
        /// </summary>
        private bool VS_visaoDocente
        {
            get
            {
                long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                int visao = __SessionWEB.__UsuarioWEB.Grupo.vis_id;
                return (visao == SysVisaoID.Individual && doc_id > 0);
            }
        }

        /// <summary>
        /// Alunos selecionados no grid para impressao de seus documentos
        /// </summary>
        private List<long> _VS_AlunosSelecionados
        {
            get
            {
                if (ViewState["_VS_AlunosSelecionados"] == null)
                    ViewState["_VS_AlunosSelecionados"] = new List<long>();
                return (List<long>)ViewState["_VS_AlunosSelecionados"];
            }
        }


        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private string VS_Ordenacao
        {
            get
            {
                return (ViewState["VS_Ordenacao"] ?? string.Empty).ToString();
            }

            set
            {
                ViewState["VS_Ordenacao"] = value;
            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private SortDirection VS_SortDirection
        {
            get
            {
                return (SortDirection)(ViewState["VS_SortDirection"] ?? SortDirection.Ascending);
            }

            set
            {
                ViewState["VS_SortDirection"] = value;
            }
        }

        #endregion Atributos/Propriedades

        #region Métodos

        /// <summary>
        /// Lista nomes dos relatorios no radioButtonList.
        /// </summary>
        private void loadTipoRelatorio()
        {
            rdbRelatorios.Items.Clear();

            List<CFG_RelatorioDocumentoDocente> dtVisao = CFG_RelatorioDocumentoDocenteBO.SelecionaVisaoCache(__SessionWEB.__UsuarioWEB.Grupo.vis_id,
                                                                                                              ApplicationWEB.AppMinutosCacheLongo);

            foreach (CFG_RelatorioDocumentoDocente rel in dtVisao)
                rdbRelatorios.Items.Add(new ListItem(rel.rdd_nomeDocumento, rel.rlt_id.ToString()));
        }

        /// <summary>
        /// Pesquisa os alunos de acordo com os filtros de busca definidos.
        /// </summary>
        protected void Pesquisar(int pageIndex)
        {

            ACA_AlunoBO.numeroCursosPeja = 0;

            _grvDocumentoAluno.DataSource = ACA_AlunoBO.BuscaAlunos_Anotacoes
                (
                    UCBuscaDocenteTurma.ComboCalendario.Valor,
                    UCBuscaDocenteTurma.ComboEscola.Esc_ID,
                    UCBuscaDocenteTurma.ComboEscola.Uni_ID,
                    UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0],
                    UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1],
                    UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2],
                    UCBuscaDocenteTurma.ComboTurma.Valor[0],
                    UCCPeriodoCalendario.Cap_ID,
                    UCComboTurmaDisciplina.Valor,
                    Convert.ToByte(UCCamposBuscaAluno.TipoBuscaNomeAluno),
                    UCCamposBuscaAluno.NomeAluno,
                    string.IsNullOrEmpty(UCCamposBuscaAluno.DataNascAluno) ? new DateTime() : Convert.ToDateTime(UCCamposBuscaAluno.DataNascAluno),
                    UCCamposBuscaAluno.NomeMaeAluno,
                    UCCamposBuscaAluno.MatriculaAluno,
                    UCCamposBuscaAluno.MatriculaEstadualAluno,
                    __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                    UCBuscaDocenteTurma.ComboEscola.Uad_ID,
                    __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao,
                    __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                    __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                    ChkEmitirAnterior.Checked,
                    Convert.ToInt32(_ddlQtPaginado.SelectedValue),
                    pageIndex,
                    (int)VS_SortDirection,
                    VS_Ordenacao,
                    false
                );

            // atribui essa quantidade para o grid
            _grvDocumentoAluno.PageSize = Convert.ToInt32(_ddlQtPaginado.SelectedValue);
            _grvDocumentoAluno.PageIndex = pageIndex;
            _grvDocumentoAluno.VirtualItemCount = ACA_AlunoBO.GetTotalRecords();

            _grvDocumentoAluno.DataBind();

            fdsResultados.Visible = true;

            _chkTodos.Visible = !_grvDocumentoAluno.Rows.Count.Equals(0);
            divQtdPaginacao.Visible = _grvDocumentoAluno.Rows.Count > 0;
        }

        /// <summary>
        /// Verifica se há busca salva e carrega os combos da tela.
        /// </summary>
        protected void CarregaBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.DocumentosDocente)
            {
                // Recuperar busca realizada e pesquisar automaticamente
                long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                if (doc_id > 0)
                    UCBuscaDocenteTurma._VS_doc_id = doc_id;

                string valor, valor2, valor3;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("relSelecionado", out valor);
                rdbRelatorios.SelectedValue = valor;
                SelecionaRelatorios();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("emitirDocAnoAnt", out valor);
                ChkEmitirAnterior.Checked = valor == "True";
                AnosAnteriores(false);

                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                {

                    UCBuscaDocenteTurma.CarregaTelaInicialVisaoDocente();

                    if (Convert.ToInt32(rdbRelatorios.SelectedValue) == DocDctAlunosPendenciaEfetivacao && __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual
                            && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                        DesobrigaCombosVisaoDocente();

                    string esc_id;
                    string uni_id;

                    if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                        (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
                    {
                        UCBuscaDocenteTurma.ComboEscola.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                        UCBuscaDocenteTurma.ComboEscola.PermiteAlterarCombos = true;
                        UCBuscaDocenteTurma.UCComboUAEscola_IndexChangedUnidadeEscola();
                    }
                }
                else
                {
                    UCBuscaDocenteTurma.ComboEscola.Inicializar();

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
                UCBuscaDocenteTurma.UCComboCurriculoPeriodo__OnSelectedIndexChange();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_id", out valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ttn_id", out valor2);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_idTurma", out valor3);
                UCBuscaDocenteTurma.ComboTurma.Valor = new[] { Convert.ToInt64(valor), Convert.ToInt64(valor3), Convert.ToInt64(valor2) };
                UCBuscaDocenteTurma_IndexChanged_Turma();

                if (divPeriodoCalendario.Visible)
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tpc_id", out valor);
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cap_id", out valor2);
                    UCCPeriodoCalendario.Valor = new[] { Convert.ToInt32(valor), Convert.ToInt32(valor2) };
                    UCCPeriodoCalendario_IndexChanged();
                }

                if (divTurmaDisciplina.Visible)
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tud_id", out valor);
                    UCComboTurmaDisciplina.Valor = Convert.ToInt64(valor);
                    UCComboTurmaDisciplina_IndexChanged();
                }

                if (divAtividadeAvaliativa.Visible)
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tnt_id", out valor);
                    UCComboAtividadeAvaliativa.Valor = Convert.ToInt32(valor);
                    UCComboTurmaDisciplina_IndexChanged();
                }

                if (divBuscaAluno.Visible)
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tipoBusca", out valor);
                    UCCamposBuscaAluno.TipoBuscaNomeAluno = valor;
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("pes_nome", out valor);
                    UCCamposBuscaAluno.NomeAluno = valor;
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("pes_dataNascimento", out valor);
                    UCCamposBuscaAluno.DataNascAluno = valor;
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("pes_nomeMae", out valor);
                    UCCamposBuscaAluno.NomeMaeAluno = valor;
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("alc_matricula", out valor);
                    UCCamposBuscaAluno.MatriculaAluno = valor;
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("alc_matriculaEstadual", out valor);
                    UCCamposBuscaAluno.MatriculaEstadualAluno = valor;
                }

                if (divData.Visible)
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("dataInicio", out valor);
                    txtDataInicio.Text = valor;
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("dataFim", out valor);
                    txtDataFim.Text = valor;
                }

                if (btnPesquisar.Visible)
                    Pesquisar(0);

                uppPesquisa.Update();
            }
        }

        /// <summary>
        /// Salva os dados da busca realizada para ser carregada posteriormente.
        /// </summary>
        protected void SalvaBusca()
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
            filtros.Add("emitirDocAnoAnt", ChkEmitirAnterior.Checked ? "True" : "False");
            filtros.Add("relSelecionado", rdbRelatorios.SelectedValue);

            if (divPeriodoCalendario.Visible)
            {
                filtros.Add("tpc_id", UCCPeriodoCalendario.Valor[0].ToString());
                filtros.Add("cap_id", UCCPeriodoCalendario.Valor[1].ToString());
            }

            if (divTurmaDisciplina.Visible)
                filtros.Add("tud_id", UCComboTurmaDisciplina.Valor.ToString());

            if (divAtividadeAvaliativa.Visible)
                filtros.Add("tnt_id", UCComboAtividadeAvaliativa.Valor.ToString());

            if (divBuscaAluno.Visible)
            {
                filtros.Add("tipoBusca", UCCamposBuscaAluno.TipoBuscaNomeAluno);
                filtros.Add("pes_nome", UCCamposBuscaAluno.NomeAluno);
                filtros.Add("pes_dataNascimento", UCCamposBuscaAluno.DataNascAluno);
                filtros.Add("pes_nomeMae", UCCamposBuscaAluno.NomeMaeAluno);
                filtros.Add("alc_matricula", UCCamposBuscaAluno.MatriculaAluno);
                filtros.Add("alc_matriculaEstadual", UCCamposBuscaAluno.MatriculaEstadualAluno);
            }

            if (divData.Visible)
            {
                filtros.Add("dataInicio", txtDataInicio.Text);
                filtros.Add("dataFim", txtDataFim.Text);
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.DocumentosDocente, Filtros = filtros };
        }

        /// <summary>
        /// Controla a exibicao dos filtros baseado no relatorio selecionado
        /// </summary>
        private void SelecionaRelatorios()
        {
            lblAvisoMensagem.Visible =
            btnPesquisar.Visible =
            fdsResultados.Visible =
            divTurmaDisciplina.Visible =
            divBuscaAluno.Visible =
            divPeriodoCalendario.Visible =
            divData.Visible =
            ChkEmitirAnterior.Visible =
            ChkEmitirAnterior.Checked =
            UCBuscaDocenteTurma._VS_AnosAnteriores =
            divAtividadeAvaliativa.Visible =
            UCBuscaDocenteTurma._VS_PermiteSemEscola = false;

            btnGerar.Visible =
            UCBuscaDocenteTurma.Visible =
            UCComboTurmaDisciplina.Visible =
            UCBuscaDocenteTurma.mostraDivComboCursoTurma =
            UCCamposObrigatorios.Visible = true;

            UCBuscaDocenteTurma.ComboCalendario.Visible = false;

            int[] tne_ids = ACA_TipoNivelEnsinoBO.SelecionaTipoNivelEnsino().Select().Select(p => Convert.ToInt32(p["tne_id"])).ToArray();
            UCBuscaDocenteTurma.VS_FiltroTipoNivelEnsino = tne_ids;
            int tne_idInfantil = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            switch (Convert.ToInt32(rdbRelatorios.SelectedValue))
            {
                case DocDctDiarioClasseFrequencia:
                    divPeriodoCalendario.Visible = true;
                    divTurmaDisciplina.Visible = true;
                    divAtividadeAvaliativa.Visible = false;
                    UCBuscaDocenteTurma._VS_MostraTurmasEletivas = false;
                    UCBuscaDocenteTurma.ComboTurma.Obrigatorio = true; //UCBuscaDocenteTurma._VS_doc_id > 0;
                    UCCPeriodoCalendario.Obrigatorio = true;
                    UCCPeriodoCalendario.SelecionaPeriodoAtualAoCarregar = false;
                    UCCPeriodoCalendario.MostrarMensagemSelecione = true;
                    break;

                case DocDctDiarioClasseAvaliacao:
                    divPeriodoCalendario.Visible = true;
                    divTurmaDisciplina.Visible = true;
                    divAtividadeAvaliativa.Visible = false;
                    UCBuscaDocenteTurma._VS_MostraTurmasEletivas = false;
                    UCBuscaDocenteTurma.ComboTurma.Obrigatorio = true;
                    UCCPeriodoCalendario.Obrigatorio = true;
                    UCCPeriodoCalendario.SelecionaPeriodoAtualAoCarregar = false;
                    UCCPeriodoCalendario.MostrarMensagemSelecione = true;
                    UCComboTurmaDisciplina.VS_MostraFilhosRegencia = true;
                    UCComboTurmaDisciplina.VS_MostraRegencia = true;
                    break;

                case DocDctRelSinteseAula:
                    divPeriodoCalendario.Visible = true;
                    divTurmaDisciplina.Visible = true;
                    UCBuscaDocenteTurma._VS_MostraTurmasEletivas = true;
                    UCBuscaDocenteTurma.ComboTurma.Obrigatorio = true;
                    UCCPeriodoCalendario.Obrigatorio = true;
                    UCCPeriodoCalendario.SelecionaPeriodoAtualAoCarregar = true;
                    UCCPeriodoCalendario.MostrarMensagemSelecione = true;
                    ChkEmitirAnterior.Visible = true;
                    UCBuscaDocenteTurma.ComboCalendario.Visible = true;
                    UCComboTurmaDisciplina.VS_MostraFilhosRegencia = true;
                    break;

                case DocDctGraficoAtividadeAvaliativa:
                case DocDctRelAtividadeAvaliativa:
                    divPeriodoCalendario.Visible = true;
                    divTurmaDisciplina.Visible = true;
                    divAtividadeAvaliativa.Visible = true;
                    UCBuscaDocenteTurma._VS_MostraTurmasEletivas = false;
                    UCBuscaDocenteTurma.ComboTurma.Obrigatorio = true;
                    UCCPeriodoCalendario.Obrigatorio = true;
                    UCComboAtividadeAvaliativa.Obrigatorio = false;
                    UCCPeriodoCalendario.SelecionaPeriodoAtualAoCarregar = true;
                    UCCPeriodoCalendario.MostrarMensagemSelecione = true;
                    UCComboTurmaDisciplina.VS_MostraFilhosRegencia = true;
                    UCComboTurmaDisciplina.VS_MostraRegencia = false;
                    ChkEmitirAnterior.Visible = true;
                    UCBuscaDocenteTurma.ComboCalendario.Visible = true;
                    break;

                case DocDctRelFrequenciaBimestral:
                    divPeriodoCalendario.Visible = true;
                    divTurmaDisciplina.Visible = true;
                    UCBuscaDocenteTurma._VS_MostraTurmasEletivas = true;
                    UCCPeriodoCalendario.Obrigatorio = true;
                    UCComboTurmaDisciplina.VS_MostraFilhosRegencia = false;
                    UCComboTurmaDisciplina.VS_MostraRegencia = true;
                    UCCPeriodoCalendario.SelecionaPeriodoAtualAoCarregar = true;
                    UCCPeriodoCalendario.MostrarMensagemSelecione = true;
                    ChkEmitirAnterior.Visible = true;
                    UCBuscaDocenteTurma.ComboCalendario.Visible = true;
                    break;

                case DocDctRelTarjetaBimestral:
                    divPeriodoCalendario.Visible = true;
                    divTurmaDisciplina.Visible = true;
                    UCBuscaDocenteTurma._VS_MostraTurmasEletivas = false;
                    UCCPeriodoCalendario.Obrigatorio = true;
                    UCBuscaDocenteTurma.ComboTurma.Obrigatorio &= UCBuscaDocenteTurma._VS_doc_id > 0;
                    UCBuscaDocenteTurma._VS_CarregarApenasTurmasNormais = UCBuscaDocenteTurma._VS_doc_id <= 0;
                    UCComboTurmaDisciplina.Visible = UCBuscaDocenteTurma._VS_doc_id > 0;
                    UCComboTurmaDisciplina.VS_MostraFilhosRegencia = true;
                    UCComboTurmaDisciplina.VS_MostraRegencia = true;
                    UCCPeriodoCalendario.SelecionaPeriodoAtualAoCarregar = true;
                    UCCPeriodoCalendario.MostrarMensagemSelecione = true;
                    ChkEmitirAnterior.Visible = true;
                    UCBuscaDocenteTurma.ComboCalendario.Visible = true;

                    tne_ids = tne_ids.Where(p => p != tne_idInfantil).ToArray();
                    UCBuscaDocenteTurma.VS_FiltroTipoNivelEnsino = tne_ids;
                    break;

                case DocDctRelSinteseEnriquecimentoCurricular:
                    divPeriodoCalendario.Visible = true;
                    divTurmaDisciplina.Visible = true;
                    UCBuscaDocenteTurma._VS_MostraTurmasEletivas = false;
                    UCCPeriodoCalendario.Obrigatorio = true;
                    UCBuscaDocenteTurma.ComboTurma.Obrigatorio &= UCBuscaDocenteTurma._VS_doc_id > 0;
                    UCComboTurmaDisciplina.Visible = UCBuscaDocenteTurma._VS_doc_id > 0;
                    UCComboTurmaDisciplina.VS_MostraFilhosRegencia = true;
                    UCComboTurmaDisciplina.VS_MostraRegencia = true;
                    UCCPeriodoCalendario.SelecionaPeriodoAtualAoCarregar = true;
                    UCCPeriodoCalendario.MostrarMensagemSelecione = true;

                    tne_ids = tne_ids.Where(p => p != tne_idInfantil).ToArray();
                    UCBuscaDocenteTurma.VS_FiltroTipoNivelEnsino = tne_ids;
                    break;

                case DocDctRelDadosPlanejamento:
                    divPeriodoCalendario.Visible = true;
                    divTurmaDisciplina.Visible = true;
                    UCBuscaDocenteTurma._VS_MostraTurmasEletivas = false;
                    UCBuscaDocenteTurma.ComboTurma.Obrigatorio = true;
                    UCCPeriodoCalendario.Obrigatorio = false;
                    UCComboTurmaDisciplina.VS_MostraFilhosRegencia = true;
                    UCComboTurmaDisciplina.VS_MostraRegencia = false;
                    UCCPeriodoCalendario.SelecionaPeriodoAtualAoCarregar = true;
                    UCCPeriodoCalendario.MostrarMensagemSelecione = false;
                    break;

                case DocDctRelOrientacaoAlcancada:
                    divPeriodoCalendario.Visible = true;
                    divTurmaDisciplina.Visible = true;
                    UCBuscaDocenteTurma._VS_MostraTurmasEletivas = false;
                    UCBuscaDocenteTurma.ComboTurma.Obrigatorio = true;
                    UCCPeriodoCalendario.Obrigatorio = false;
                    UCComboTurmaDisciplina.VS_MostraFilhosRegencia = true;
                    UCComboTurmaDisciplina.VS_MostraRegencia = false;
                    UCCPeriodoCalendario.SelecionaPeriodoAtualAoCarregar = true;
                    UCCPeriodoCalendario.MostrarMensagemSelecione = false;
                    break;

                case DocDctRelAnotacoesAula:
                    divTurmaDisciplina.Visible = true;
                    divBuscaAluno.Visible = true;
                    divPeriodoCalendario.Visible = true;
                    divData.Visible = true;
                    UCBuscaDocenteTurma._VS_MostraTurmasEletivas = true;
                    UCComboTurmaDisciplina.Obrigatorio = UCBuscaDocenteTurma._VS_doc_id > 0;
                    UCCPeriodoCalendario.SelecionaPeriodoAtualAoCarregar = false;
                    UCCPeriodoCalendario.MostrarMensagemSelecione = true;
                    UCComboTurmaDisciplina.VS_MostraRegencia = true;
                    UCComboTurmaDisciplina.VS_MostraFilhosRegencia = false;
                    btnGerar.Visible = false;
                    btnPesquisar.Visible = true;

                    ChkEmitirAnterior.Visible = true;
                    UCBuscaDocenteTurma.ComboCalendario.Visible = true;
                    break;

                case DocDctPlanoAnual:
                    divPeriodoCalendario.Visible = false;
                    divTurmaDisciplina.Visible = true;
                    divAtividadeAvaliativa.Visible = false;
                    UCBuscaDocenteTurma._VS_MostraTurmasEletivas = false;
                    UCBuscaDocenteTurma.ComboTurma.Obrigatorio = true; //UCBuscaDocenteTurma._VS_doc_id > 0;
                    UCBuscaDocenteTurma.ComboCalendario.Visible = true;
                    break;

                case DocDctPlanoCicloSerie:
                    {
                        divPeriodoCalendario.Visible = false;
                        divTurmaDisciplina.Visible = false;
                        divAtividadeAvaliativa.Visible = false;
                        UCBuscaDocenteTurma._VS_MostraTurmasEletivas = false;
                        UCBuscaDocenteTurma.BuscaEscolasPorVinculoColaboradorDocente = true;
                        UCBuscaDocenteTurma.ComboCalendario.Visible = true;
                        UCBuscaDocenteTurma.mostraDivComboCursoTurma = false;
                        break;
                    }
                case DocDctAlunosPendenciaEfetivacao:
                    {
                        //Se for usuário administrador ou gestor permite gerar o relatório sem filtrar por escola
                        if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao ||
                            __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao)
                        {
                            UCBuscaDocenteTurma._VS_PermiteSemEscola = true;
                            UCBuscaDocenteTurma.ComboEscola.ObrigatorioEscola = false;
                        }
                        divPeriodoCalendario.Visible = false;
                        divTurmaDisciplina.Visible = false;
                        divAtividadeAvaliativa.Visible = false;
                        UCBuscaDocenteTurma._VS_MostraTurmasEletivas = false;
                        UCBuscaDocenteTurma.ComboCalendario.Visible = true;
                        UCBuscaDocenteTurma.mostraDivComboCursoTurma = true;
                        UCBuscaDocenteTurma.ComboTurma.Obrigatorio = UCBuscaDocenteTurma.ComboCursoCurriculo.Obrigatorio =
                            UCBuscaDocenteTurma.ComboCurriculoPeriodo.Obrigatorio = false;
                        break;
                    }
            }
            UCCPeriodoCalendario.MostrarMensagemSelecioneAnual = !UCCPeriodoCalendario.MostrarMensagemSelecione;
            UCBuscaDocenteTurma.InicializaCamposBusca();
        }

        /// <summary>
        /// Seta combos que não serao obrigatórios na busca, quando o usuário logado tiver  a visão de docente
        /// </summary>
        public void DesobrigaCombosVisaoDocente()
        {
            UCBuscaDocenteTurma.ComboTurma.Obrigatorio = false;
        }

        /// <summary>
        /// Carrega informações de anos anteriores
        /// </summary>
        private void AnosAnteriores(bool limpaBusca)
        {
            if (limpaBusca)
                LimpaBuscaRealizada();

            UCBuscaDocenteTurma._VS_AnosAnteriores = ChkEmitirAnterior.Checked;

            UCBuscaDocenteTurma.InicializaCamposBusca();

            //if (ChkEmitirAnterior.Checked)
            //{
            //    if (UCBuscaDocenteTurma.ComboEscola.Esc_ID > 0 && UCBuscaDocenteTurma.ComboEscola.Uni_ID > 0)
            //    {
            //        if (UCBuscaDocenteTurma.ComboCalendario.Visible)
            //        {
            //            UCBuscaDocenteTurma.ComboCalendario.CarregarCalendarioAnual();
            //            UCBuscaDocenteTurma.ComboCalendario.SetarFoco();
            //            UCBuscaDocenteTurma.ComboCalendario.PermiteEditar = true;

            //            UCBuscaDocenteTurma.UCCCalendario_IndexChanged();
            //        }
            //        else
            //        {
            //            UCBuscaDocenteTurma.ComboCursoCurriculo.CarregarPorEscola(UCBuscaDocenteTurma.ComboEscola.Esc_ID, UCBuscaDocenteTurma.ComboEscola.Uni_ID);

            //            UCBuscaDocenteTurma.ComboCursoCurriculo.SetarFoco();
            //            UCBuscaDocenteTurma.ComboCursoCurriculo.PermiteEditar = true;

            //            UCBuscaDocenteTurma.UCCCursoCurriculo_IndexChanged();
            //        }
            //    }
            //}
            //else
            //{
            //    if (UCBuscaDocenteTurma.ComboEscola.Esc_ID > 0 && UCBuscaDocenteTurma.ComboEscola.Uni_ID > 0)
            //    {
            //        if (UCBuscaDocenteTurma.ComboCalendario.Visible)
            //        {
            //            UCBuscaDocenteTurma.ComboCalendario.CarregarCalendarioAnualAnoAtual();
            //            UCBuscaDocenteTurma.ComboCalendario.SetarFoco();
            //            UCBuscaDocenteTurma.ComboCalendario.PermiteEditar = true;

            //            UCBuscaDocenteTurma.UCCCalendario_IndexChanged();
            //        }
            //        else
            //        {
            //            UCBuscaDocenteTurma.ComboCursoCurriculo.CarregarPorEscolaCalendarioSituacaoCurso(UCBuscaDocenteTurma.ComboEscola.Esc_ID, UCBuscaDocenteTurma.ComboEscola.Uni_ID, UCBuscaDocenteTurma.ComboCalendario.Valor, 0);

            //            UCBuscaDocenteTurma.ComboCursoCurriculo.SetarFoco();
            //            UCBuscaDocenteTurma.ComboCursoCurriculo.PermiteEditar = true;

            //            UCBuscaDocenteTurma.UCCCursoCurriculo_IndexChanged();
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Limpa a busca salva na session e recarrega a página.
        /// </summary>
        private void LimpaBuscaRealizada()
        {
            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Individual)
            {
                __SessionWEB.BuscaRealizada = new BuscaGestao();
                UCBuscaDocenteTurma.ComboEscola.Uad_ID = new Guid();
                UCBuscaDocenteTurma.ComboEscola.CarregaEscolaPorUASuperiorSelecionada();

                UCBuscaDocenteTurma.ComboEscola.SelectedValueEscolas = new[] { -1, -1 };
                UCBuscaDocenteTurma.UCComboUAEscola_IndexChangedUnidadeEscola();
                uppPesquisa.Update();
            }
        }

        #endregion

        #region Eventos

        protected void ChkEmitirAnterior_CheckedChanged(object sender, EventArgs e)
        {
            AnosAnteriores(true);
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                if (UCCamposBuscaAluno.IsValid)
                {
                    SalvaBusca();
                    Pesquisar(0);
                }
                else
                    throw new ValidationException("Data de nascimento do aluno não está no formato dd/mm/aaaa ou é inexistente.");
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alunos.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rdbRelatorios_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LimpaBuscaRealizada();

                SelecionaRelatorios();

                if (Convert.ToInt32(rdbRelatorios.SelectedValue) == DocDctAlunosPendenciaEfetivacao
                    && __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                    DesobrigaCombosVisaoDocente();
            }
            catch (Exception error)
            {
                ApplicationWEB._GravaErro(error);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void ValidarDataDocumento_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!string.IsNullOrEmpty(txtDataInicio.Text) && !string.IsNullOrEmpty(txtDataFim.Text))
            {
                DateTime dtIni = Convert.ToDateTime(txtDataInicio.Text);
                DateTime dtFim = Convert.ToDateTime(txtDataFim.Text);

                args.IsValid = (dtIni <= dtFim);
            }
        }

        protected void btnGerar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    SalvaBusca();

                    int relatorio = Convert.ToInt32(rdbRelatorios.SelectedValue);
                    string sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, relatorio);
                    string sReportDev = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.REPORT_DEVEXPRESS, __SessionWEB.__UsuarioWEB.Usuario.ent_id, relatorio);
                    string report, parametros;
                    report = parametros = string.Empty;
                    XtraReport DevReport = null;
                    SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);

                    if (relatorio == DocDctRelTarjetaBimestral && UCComboTurmaDisciplina.Valor > 0)
                    {
                        // Verifico se eh disciplina de enriquecimento curricular,
                        // se for carrego com o outro relatorio especifico
                        ACA_TipoDisciplina tipoDisciplina = ACA_TipoDisciplinaBO.GetEntity(new ACA_TipoDisciplina { tds_id = TUR_TurmaDisciplinaRelDisciplinaBO.GetSelectTdsBy_tud_id(UCComboTurmaDisciplina.Valor) });
                        if (tipoDisciplina.tds_tipo == (byte)ACA_TipoDisciplinaBO.TipoDisciplina.EnriquecimentoCurricular ||
                            tipoDisciplina.tds_tipo == (byte)ACA_TipoDisciplinaBO.TipoDisciplina.TerritorioSaber)
                        {
                            relatorio = DocDctRelSinteseEnriquecimentoCurricular;
                        }
                    }

                    int numeroDiasUteis = 0;
                    string parametroAtivDiversificada = String.Empty;
                    if (relatorio == DocDctRelTarjetaBimestral || relatorio == DocDctRelSinteseEnriquecimentoCurricular)
                    {
                        numeroDiasUteis = GestaoEscolarUtilBO.NumeroDeDiasUteis(UCBuscaDocenteTurma.ComboCalendario.Valor, UCBuscaDocenteTurma.ComboEscola.Esc_ID, UCBuscaDocenteTurma.ComboEscola.Uni_ID, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                        parametroAtivDiversificada = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_ATIVIDADE_DIVERSIFICADA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    }

                    switch (relatorio)
                    {
                        case DocDctDiarioClasseFrequencia:
                            report = ((int)ReportNameGestaoAcademicaDocumentosDocente.DocDctDiarioClasseFrequencia).ToString();
                            parametros = "esc_id=" + UCBuscaDocenteTurma.ComboEscola.Esc_ID +
                                         "&uni_id=" + UCBuscaDocenteTurma.ComboEscola.Uni_ID +
                                         "&cal_id=" + UCBuscaDocenteTurma.ComboCalendario.Valor +
                                         "&cur_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0] +
                                         "&crr_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1] +
                                         "&crp_id=" + UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2] +
                                         "&tur_id=" + UCBuscaDocenteTurma.ComboTurma.Valor[0] +
                                         "&tpc_id=" + UCCPeriodoCalendario.Valor[0] +
                                         "&tud_id=" + UCComboTurmaDisciplina.Valor +
                                         "&nomePeriodoCalendario=" + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&nomeComponenteCurricular=" + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                                         "&mostraCodigoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&corAlunoInativo=" + ApplicationWEB.AlunoInativo +
                                         "&situacaoAlunoInativo=" + (byte)ACA_AlunoSituacao.Inativo +
                                         "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoPaisagem.Municipio") +
                                         "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoPaisagem.Secretaria") +
                                         "&logo=" + String.Concat(CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                                                    , ApplicationWEB.LogoRelatorioSSRS) +
                                         "&cal_ano=" + UCBuscaDocenteTurma.ComboCalendario.Cal_ano.ToString() +
                                         "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString() +
                                         "&atg_tipo=" + Convert.ToInt32(ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.CabecalhoRelatorio).ToString() +
                                         "&documentoOficial=false";

                            break;
                        case DocDctDiarioClasseAvaliacao:
                            report = ((int)ReportNameGestaoAcademicaDocumentosDocente.DocDctDiarioClasseAvaliacao).ToString();
                            parametros = "esc_id=" + UCBuscaDocenteTurma.ComboEscola.Esc_ID +
                                         "&uni_id=" + UCBuscaDocenteTurma.ComboEscola.Uni_ID +
                                         "&cal_id=" + UCBuscaDocenteTurma.ComboCalendario.Valor +
                                         "&cur_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0] +
                                         "&crr_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1] +
                                         "&crp_id=" + UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2] +
                                         "&tur_id=" + UCBuscaDocenteTurma.ComboTurma.Valor[0] +
                                         "&tpc_id=" + UCCPeriodoCalendario.Valor[0] +
                                         "&tud_id=" + UCComboTurmaDisciplina.Valor +
                                         "&nomePeriodoCalendario=" + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&nomeComponenteCurricular=" + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                                         "&mostraCodigoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&corAlunoInativo=" + ApplicationWEB.AlunoInativo +
                                         "&situacaoAlunoInativo=" + (byte)ACA_AlunoSituacao.Inativo +
                                         "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoPaisagem.Municipio") +
                                         "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoPaisagem.Secretaria") +
                                         "&logo=" + String.Concat(CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                                                    , ApplicationWEB.LogoRelatorioSSRS) +
                                         "&cal_ano=" + UCBuscaDocenteTurma.ComboCalendario.Cal_ano.ToString() +
                                         "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString() +
                                         "&atg_tipo=" + Convert.ToInt32(ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.CabecalhoRelatorio).ToString() +
                                         "&documentoOficial=false";
                                         
                            break;
                        case DocDctGraficoAtividadeAvaliativa:
                            //sDeclaracaoHMTL = CFG_Parametro.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, relatorio);
                            sReportDev = "true";

                            report = ((int)ReportNameGestaoAcademicaDocumentosDocente.DocDctGraficoAtividadeAvaliativa).ToString();

                            if (UCComboAtividadeAvaliativa.Valor == -1)
                            {
                                DevReport = new RelAvaliacaoEfetivacao
                                    (UCBuscaDocenteTurma.ComboEscola.Esc_ID,
                                    UCBuscaDocenteTurma.ComboTurma.Valor[0],
                                    UCComboTurmaDisciplina.Valor,
                                    UCCPeriodoCalendario.Valor[0],
                                    UCBuscaDocenteTurma.ComboEscola.Uni_ID,
                                    UCBuscaDocenteTurma.ComboCalendario.Valor,
                                    UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0],
                                    UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1],
                                    UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2],
                                    UCComboAtividadeAvaliativa.Valor,
                                    GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id),
                                    GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA").ToString(),
                                    GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio").ToString(),
                                    GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria").ToString(),
                                    ApplicationWEB.LogoRelatorioDB);
                            }
                            else
                            {
                                DevReport = new RelAvaliacaoAtividadeAvaliativa
                                    (UCBuscaDocenteTurma.ComboEscola.Esc_ID,
                                    UCBuscaDocenteTurma.ComboTurma.Valor[0],
                                    UCComboTurmaDisciplina.Valor,
                                    UCCPeriodoCalendario.Valor[0],
                                    UCBuscaDocenteTurma.ComboEscola.Uni_ID,
                                    UCBuscaDocenteTurma.ComboCalendario.Valor,
                                    UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0],
                                    UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1],
                                    UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2],
                                    UCComboAtividadeAvaliativa.Valor,
                                    GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id),
                                    GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA").ToString(),
                                    GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio").ToString(),
                                    GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria").ToString(),
                                    ApplicationWEB.LogoRelatorioDB);
                            }
                            break;
                        case DocDctRelAtividadeAvaliativa:
                            report = ((int)ReportNameGestaoAcademicaDocumentosDocente.DocDctRelAtividadeAvaliativa).ToString();
                            parametros = "esc_id=" + UCBuscaDocenteTurma.ComboEscola.Esc_ID +
                                         "&uni_id=" + UCBuscaDocenteTurma.ComboEscola.Uni_ID +
                                         "&cal_id=" + UCBuscaDocenteTurma.ComboCalendario.Valor +
                                         "&cur_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0] +
                                         "&crp_id=" + UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2] +
                                         "&tur_id=" + UCBuscaDocenteTurma.ComboTurma.Valor[0] +
                                         "&tpc_id=" + UCCPeriodoCalendario.Valor[0] +
                                         "&tnt_id=" + (UCComboAtividadeAvaliativa.Valor > 0 ? UCComboAtividadeAvaliativa.Valor.ToString() : string.Empty) +
                                         "&doc_id=" + UCBuscaDocenteTurma._VS_doc_id +
                                         "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                         "&nomePeriodoCalendario=" + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&nomeDisciplina=" + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                                         "&tud_id=" + UCComboTurmaDisciplina.Valor +
                                         "&mostraCodigoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&corAlunoInativo=" + ApplicationWEB.AlunoInativo +
                                         "&situacaoAlunoInativo=" + (byte)ACA_AlunoSituacao.Inativo +
                                         "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio") +
                                         "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria") +
                                         "&logo=" + String.Concat(CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                                                    , ApplicationWEB.LogoRelatorioSSRS) +
                                         "&cal_ano=" + UCBuscaDocenteTurma.ComboCalendario.Cal_ano.ToString() +
                                         "&documentoOficial=false";

                            break;
                        case DocDctRelTarjetaBimestral:
                            report = ((int)ReportNameGestaoAcademicaDocumentosDocente.DocDctRelTarjetaBimestral).ToString();
                            parametros = "esc_id=" + UCBuscaDocenteTurma.ComboEscola.Esc_ID +
                                         "&uni_id=" + UCBuscaDocenteTurma.ComboEscola.Uni_ID +
                                         "&cal_id=" + UCBuscaDocenteTurma.ComboCalendario.Valor +
                                         "&cur_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0] +
                                         "&crr_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1] +
                                         "&crp_id=" + UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2] +
                                         "&tur_id=" + UCBuscaDocenteTurma.ComboTurma.Valor[0] +
                                         "&tpc_id=" + UCCPeriodoCalendario.Valor[0] +
                                         "&tud_id=" + UCComboTurmaDisciplina.Valor +
                                         "&nomePeriodoCalendario=" + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&nomeDisciplina=" + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                                         "&visaoDocente=" + ((UCBuscaDocenteTurma._VS_doc_id > 0) && (__SessionWEB.__UsuarioWEB.Grupo.vis_id == 4)) +
                                         "&tituloRelatorio=" + rdbRelatorios.SelectedItem.Text.ToString() +
                                         "&mostraCodigoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&corAlunoInativo=" + ApplicationWEB.AlunoInativo +
                                         "&situacaoAlunoInativo=" + (byte)ACA_AlunoSituacao.Inativo +
                                         "&corAlunoFrequenciaLimite=" + ApplicationWEB.AlunoFrequenciaLimite +
                                         "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoPaisagem.Municipio") +
                                         "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoPaisagem.Secretaria") +
                                         "&logo=" + String.Concat(CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                                                    , ApplicationWEB.LogoRelatorioSSRS) +
                                         "&tev_id=" + (string.IsNullOrEmpty(parametroAtivDiversificada) ? "0" : parametroAtivDiversificada) +
                                         "&numeroDiasUteis=" + numeroDiasUteis +
                                         "&visaoGestor=" + (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa) +
                                         "&cal_ano=" + UCBuscaDocenteTurma.ComboCalendario.Cal_ano.ToString() +
                                         "&documentoOficial=true" +
                                         "&mensagemFrequenciaExterna=" + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MENSAGEM_FREQUENCIA_EXTERNA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                            break;
                        case DocDctRelSinteseEnriquecimentoCurricular:
                            report = ((int)ReportNameGestaoAcademicaDocumentosDocente.DocDctRelSinteseEnriquecimentoCurricular).ToString();
                            parametros = "esc_id=" + UCBuscaDocenteTurma.ComboEscola.Esc_ID +
                                         "&uni_id=" + UCBuscaDocenteTurma.ComboEscola.Uni_ID +
                                         "&cal_id=" + UCBuscaDocenteTurma.ComboCalendario.Valor +
                                         "&cur_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0] +
                                         "&crr_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1] +
                                         "&crp_id=" + UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2] +
                                         "&tur_id=" + UCBuscaDocenteTurma.ComboTurma.Valor[0] +
                                         "&tpc_id=" + UCCPeriodoCalendario.Valor[0] +
                                         "&tud_id=" + UCComboTurmaDisciplina.Valor +
                                         "&nomePeriodoCalendario=" + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&nomeDisciplina=" + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                                         "&visaoDocente=" + ((UCBuscaDocenteTurma._VS_doc_id > 0) && (__SessionWEB.__UsuarioWEB.Grupo.vis_id == 4)) +
                                         "&tituloRelatorio=" + rdbRelatorios.SelectedItem.Text.ToString() +
                                         "&mostraCodigoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&corAlunoInativo=" + ApplicationWEB.AlunoInativo +
                                         "&situacaoAlunoInativo=" + (byte)ACA_AlunoSituacao.Inativo +
                                         "&corAlunoFrequenciaLimite=" + ApplicationWEB.AlunoFrequenciaLimite +
                                         "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio") +
                                         "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria") +
                                         "&logo=" + String.Concat(CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                                                    , ApplicationWEB.LogoRelatorioSSRS) +
                                        "&tev_id=" + (string.IsNullOrEmpty(parametroAtivDiversificada) ? "0" : parametroAtivDiversificada) +
                                        "&numeroDiasUteis=" + numeroDiasUteis +
                                        "&visaoGestor=" + (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa) +
                                        "&cal_ano=" + UCBuscaDocenteTurma.ComboCalendario.Cal_ano.ToString() +
                                        "&documentoOficial=true";
                            break;
                        case DocDctRelFrequenciaBimestral:
                            report = ((int)ReportNameGestaoAcademicaDocumentosDocente.DocDctRelFrequenciaBimestral).ToString();
                            parametros = "esc_id=" + UCBuscaDocenteTurma.ComboEscola.Esc_ID +
                                         "&uni_id=" + UCBuscaDocenteTurma.ComboEscola.Uni_ID +
                                         "&cal_id=" + UCBuscaDocenteTurma.ComboCalendario.Valor +
                                         "&cur_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0] +
                                         "&crr_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1] +
                                         "&crp_id=" + UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2] +
                                         "&tur_id=" + UCBuscaDocenteTurma.ComboTurma.Valor[0] +
                                         "&tpc_id=" + UCCPeriodoCalendario.Valor[0] +
                                         "&cap_id=" + UCCPeriodoCalendario.Valor[1] +
                                         "&tud_id=" + UCComboTurmaDisciplina.Valor +
                                         "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                         "&nomePeriodoCalendario=" + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&nomeComponenteCurricular=" + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                                         "&mostraCodigoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&corAlunoInativo=" + ApplicationWEB.AlunoInativo +
                                         "&situacaoAlunoInativo=" + (byte)ACA_AlunoSituacao.Inativo +
                                         "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoPaisagem.Municipio") +
                                         "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoPaisagem.Secretaria") +
                                         "&logo=" + String.Concat(CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                                                    , ApplicationWEB.LogoRelatorioSSRS) +
                                         "&cal_ano=" + UCBuscaDocenteTurma.ComboCalendario.Cal_ano.ToString() +
                                         "&documentoOficial=false";
                            break;
                        case DocDctRelSinteseAula:
                            report = ((int)ReportNameGestaoAcademicaDocumentosDocente.DocDctRelSinteseAula).ToString();
                            parametros = "uad_id=" + UCBuscaDocenteTurma.ComboEscola.Uad_ID +
                                         "&esc_id=" + UCBuscaDocenteTurma.ComboEscola.Esc_ID +
                                         "&cal_id=" + UCBuscaDocenteTurma.ComboCalendario.Valor +
                                         "&cur_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0] +
                                         "&crp_id=" + UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2] +
                                         "&tur_id=" + UCBuscaDocenteTurma.ComboTurma.Valor[0] +
                                         "&tpc_id=" + UCCPeriodoCalendario.Valor[0] +
                                         "&tud_id=" + UCComboTurmaDisciplina.Valor +
                                         "&doc_id=" + UCBuscaDocenteTurma._VS_doc_id +
                                         "&MSG_SinteseDaAula=" + GetGlobalResourceObject("Mensagens", "MSG_SINTESEDAAULA") +
                                         "&nomePeriodoCalendario=" + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&nomeDisciplina=" + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                                         "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                         "&mostraCodigoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio") +
                                         "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria") +
                                         "&logo=" + String.Concat(CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                                                    , ApplicationWEB.LogoRelatorioSSRS) +
                                         "&cal_ano=" + UCBuscaDocenteTurma.ComboCalendario.Cal_ano.ToString();
                            break;
                        case DocDctRelDadosPlanejamento:
                            report = ((int)ReportNameGestaoAcademicaDocumentosDocente.DocDctRelDadosPlanejamento).ToString();
                            parametros = "esc_id=" + UCBuscaDocenteTurma.ComboEscola.Esc_ID +
                                         "&uni_id=" + UCBuscaDocenteTurma.ComboEscola.Uni_ID +
                                         "&cal_id=" + UCBuscaDocenteTurma.ComboCalendario.Valor +
                                         "&cur_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0] +
                                         "&crr_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1] +
                                         "&crp_id=" + UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2] +
                                         "&tur_id=" + UCBuscaDocenteTurma.ComboTurma.Valor[0] +
                                         "&tpc_id=" + UCCPeriodoCalendario.Valor[0] +
                                         "&cap_id=" + UCCPeriodoCalendario.Valor[1] +
                                         "&tud_id=" + UCComboTurmaDisciplina.Valor +
                                         "&tud_nome=" + UCComboTurmaDisciplina.Texto +
                                         "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                         "&nomePeriodoCalendario=" + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&nomeComponenteCurricular=" + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                                         "&mostraCodigoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoPaisagem.Municipio") +
                                         "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoPaisagem.Secretaria") +
                                         "&logo=" + String.Concat(CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                                                    , ApplicationWEB.LogoRelatorioSSRS) +
                                         "&cal_ano=" + UCBuscaDocenteTurma.ComboCalendario.Cal_ano.ToString();
                            break;
                        case DocDctRelOrientacaoAlcancada:
                            report = ((int)ReportNameGestaoAcademicaDocumentosDocente.DocDctRelOrientacaoAlcancada).ToString();
                            parametros = "esc_id=" + UCBuscaDocenteTurma.ComboEscola.Esc_ID +
                                         "&uni_id=" + UCBuscaDocenteTurma.ComboEscola.Uni_ID +
                                         "&cal_id=" + UCBuscaDocenteTurma.ComboCalendario.Valor +
                                         "&cur_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0] +
                                         "&crr_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1] +
                                         "&crp_id=" + UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2] +
                                         "&tur_id=" + UCBuscaDocenteTurma.ComboTurma.Valor[0] +
                                         "&tpc_id=" + UCCPeriodoCalendario.Valor[0] +
                                         "&tud_id=" + UCComboTurmaDisciplina.Valor +
                                         "&tud_nome=" + UCComboTurmaDisciplina.Texto +
                                         "&doc_id=" + UCBuscaDocenteTurma._VS_doc_id +
                                         "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                         "&nomePeriodoCalendario=" + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&nomeDisciplina=" + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                                         "&mostraCodigoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&corAlunoInativo=" + ApplicationWEB.AlunoInativo +
                                         "&situacaoAlunoInativo=" + (byte)ACA_AlunoSituacao.Inativo +
                                         "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio") +
                                         "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria") +
                                         "&logo=" + String.Concat(CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                                                    , ApplicationWEB.LogoRelatorioSSRS) +
                                         "&cal_ano=" + UCBuscaDocenteTurma.ComboCalendario.Cal_ano.ToString();
                            break;
                        case DocDctRelAnotacoesAula:
                            if (_VS_AlunosSelecionados.Count.Equals(0))
                            {
                                lblMessage.Text = UtilBO.GetErroMessage("É necessário selecionar ao menos um aluno", UtilBO.TipoMensagem.Alerta);
                                return;
                            }
                            string alu_ids = String.Empty;
                            foreach (long id in _VS_AlunosSelecionados)
                            {
                                alu_ids = alu_ids = !String.IsNullOrEmpty(alu_ids) ? String.Concat(alu_ids + ',', id) : id.ToString();
                            }

                            report = ((int)ReportNameGestaoAcademicaDocumentosDocente.DocDctRelAnotacoesAula).ToString();
                            parametros = "esc_id=" + UCBuscaDocenteTurma.ComboEscola.Esc_ID +
                                         "&cal_id=" + UCBuscaDocenteTurma.ComboCalendario.Valor +
                                         "&cur_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0] +
                                         "&tur_id=" + UCBuscaDocenteTurma.ComboTurma.Valor[0] +
                                         "&cap_id=" + UCCPeriodoCalendario.Valor[1] +
                                         "&tud_id=" + UCComboTurmaDisciplina.Valor +
                                         "&doc_id=" + (VS_visaoDocente ? __SessionWEB.__UsuarioWEB.Docente.doc_id : -1) +
                                         "&dataAula=" + txtDataInicio.Text +
                                         "&nomePeriodoCalendario=" + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&nomeDisciplina=" + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                                         "&alu_id=" + alu_ids +
                                         "&mostraCodigoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoPaisagem.Municipio") +
                                         "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoPaisagem.Secretaria") +
                                         "&lblCodigoEOL2=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctRelAnotacoesAula.lblCodigoEOL2.label") +
                                         "&logo=" + String.Concat(CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                                                    , ApplicationWEB.LogoRelatorioSSRS) +
                                         "&cal_ano=" + UCBuscaDocenteTurma.ComboCalendario.Cal_ano.ToString() +
                                         "&documentoOficial=false";
                            break;

                        case DocDctPlanoAnual:
                            sReportDev = "true";

                            DevReport = new DocDctPlanoAnual
                                    (UCBuscaDocenteTurma.ComboCalendario.Cal_ano.ToString(),
                                    UCBuscaDocenteTurma.ComboCalendario.Valor,
                                    UCBuscaDocenteTurma.ComboEscola.Esc_ID,
                                    UCBuscaDocenteTurma.ComboEscola.Uni_ID,
                                    __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                                    UCBuscaDocenteTurma.ComboTurma.Valor[0],
                                    UCComboTurmaDisciplina.Valor,
                                    ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id),
                                     GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio").ToString(),
                                    GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria").ToString(),
                                    ApplicationWEB.LogoRelatorioDB);

                            break;

                        case DocDctPlanoCicloSerie:
                            sReportDev = "true";

                            DevReport = new DocDctPlanoCicloSerie
                                    (UCBuscaDocenteTurma.ComboCalendario.Cal_ano.ToString(),
                                    UCBuscaDocenteTurma.ComboCalendario.Valor,
                                    UCBuscaDocenteTurma.ComboEscola.Esc_ID,
                                    UCBuscaDocenteTurma.ComboEscola.Uni_ID,
                                    __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                                    ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id),
                                     GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio").ToString(),
                                    GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria").ToString(),
                                    ApplicationWEB.LogoRelatorioDB);

                            break;
                        case DocDctAlunosPendenciaEfetivacao:
                            report = ((int)ReportNameGestaoAcademicaDocumentosDocente.DocDctAlunosPendenciaEfetivacao).ToString();
                            parametros = "uad_idSuperiorGestao=" + UCBuscaDocenteTurma.ComboEscola.Uad_ID +
                                         "&esc_id=" + UCBuscaDocenteTurma.ComboEscola.Esc_ID +
                                         "&uni_id=" + UCBuscaDocenteTurma.ComboEscola.Uni_ID +
                                         "&cal_id=" + UCBuscaDocenteTurma.ComboCalendario.Valor +
                                         "&cal_ano=" + UCBuscaDocenteTurma.ComboCalendario.Cal_ano.ToString() +
                                         "&cur_id=" + (UCBuscaDocenteTurma.ComboCursoCurriculo.Visible ? UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0] : -1) +
                                         "&crr_id=" + (UCBuscaDocenteTurma.ComboCursoCurriculo.Visible ? UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1] : -1) +
                                         "&crp_id=" + (UCBuscaDocenteTurma.ComboCurriculoPeriodo.Visible? UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2] : -1) +
                                         "&tur_id=" + UCBuscaDocenteTurma.ComboTurma.Valor[0] +
                                         "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                         "&doc_id=" + __SessionWEB.__UsuarioWEB.Docente.doc_id +
                                         "&tud_ids=" + "" +
                                         "&tev_EfetivacaoNotas=" + ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&tev_EfetivacaoFinal=" + ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&nomeParecerConclusivo=" + GetGlobalResourceObject("Mensagens", "MSG_RESULTADOEFETIVACAO") +
                                         "&nomePeriodoCalendario=" + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&nomeComponenteCurricular=" + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                                         "&mostraCodigoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                         "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio") +
                                         "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria") +
                                         "&logo=" + String.Concat(CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                                                    , ApplicationWEB.LogoRelatorioSSRS);
                            break;
                    }

                    if (sDeclaracaoHMTL == "false" || string.IsNullOrEmpty(sDeclaracaoHMTL))
                    {

                        if (sReportDev == "false" || string.IsNullOrEmpty(sReportDev))
                        {
                            CFG_RelatorioBO.CallReport("Documentos", report, parametros, HttpContext.Current);
                        }
                        else
                        {
                            GestaoEscolarUtilBO.SendParametersToReport(DevReport);
                            Response.Redirect(String.Format("~/Documentos/RelatorioDev.aspx?dummy='{0}'", HttpUtility.UrlEncode(sa.Encrypt(report))), false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        }

                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar emitir o relatório.", UtilBO.TipoMensagem.Erro);
                }
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
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                }

                if (!IsPostBack)
                {
                    string message = __SessionWEB.PostMessages;
                    if (!String.IsNullOrEmpty(message))
                        lblMessage.Text = message;

                    long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                    if (doc_id > 0)
                        UCBuscaDocenteTurma._VS_doc_id = doc_id;

                    // Carrega o nome referente ao parametro de matricula estadual.
                    string nomeMatriculaEstadual = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    bool mostraMatriculaEstadual = !string.IsNullOrEmpty(nomeMatriculaEstadual);

                    _grvDocumentoAluno.Columns[columnMatricula].Visible = !mostraMatriculaEstadual;
                    _grvDocumentoAluno.Columns[columnMatriculaEstadual].Visible = mostraMatriculaEstadual;
                    _grvDocumentoAluno.Columns[columnMatriculaEstadual].HeaderText = nomeMatriculaEstadual;

                    UCCamposBuscaAluno.MostrarMatriculaEstadual = mostraMatriculaEstadual;
                    UCCamposBuscaAluno.TituloMatriculaEstadual = nomeMatriculaEstadual;

                    _grvDocumentoAluno.Columns[cellCurso].HeaderText = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    lblAvisoMensagem.Text = UtilBO.GetMessage("É necessário selecionar um tipo de relatório.", UtilBO.TipoMensagem.Informacao);
                    UCCamposObrigatorios.Visible = false;
                    btnGerar.Visible = false;

                    UCBuscaDocenteTurma._VS_MostarComboEscola = __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual;
                    UCBuscaDocenteTurma.Visible = false;
                    divTurmaDisciplina.Visible = false;
                    divPeriodoCalendario.Visible = false;

                    // quantidade de itens por página
                    string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                    _ddlQtPaginado.SelectedValue = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao.ToString() : qtItensPagina;

                    loadTipoRelatorio();

                    CarregaBusca();
                }

                UCBuscaDocenteTurma.IndexChanged_Turma += UCBuscaDocenteTurma_IndexChanged_Turma;
                UCBuscaDocenteTurma.IndexChanged_Calendario += UCBuscaDocenteTurma_IndexChanged_Calendario;
                UCCPeriodoCalendario.IndexChanged += UCCPeriodoCalendario_IndexChanged;
                UCComboTurmaDisciplina.IndexChanged += UCComboTurmaDisciplina_IndexChanged;
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
            }
        }

        protected void _ddlQtPaginado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_grvDocumentoAluno.Rows.Count.Equals(0))
            {
                // Seta propriedades necessárias para ordenação nas colunas.
                ConfiguraColunasOrdenacao(_grvDocumentoAluno, VS_Ordenacao, VS_SortDirection);

                Pesquisar(0);
            }
        }

        protected void _grvDocumentoAluno_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox _chkSelecionar = (CheckBox)e.Row.FindControl("_chkSelecionar");
                long alu_id = (long)(_grvDocumentoAluno.DataKeys[e.Row.RowIndex]["alu_id"]);
                bool contain = _VS_AlunosSelecionados.Contains(alu_id);
                if (_chkSelecionar != null)
                {
                    _chkSelecionar.Checked = contain;
                    if (!contain)
                        _chkTodos.Checked = false;
                }
            }
        }

        protected void grvDocumentoAluno_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Pesquisar(e.NewPageIndex);
        }

        protected void _grvDocumentoAluno_DataBound(object sender, EventArgs e)
        {
            int total = ACA_AlunoBO.GetTotalRecords();
            UCTotalRegistros1.Total = total;
            //Caso a quantia total seja a mesma quantia de selecionados significa q todos estao selecionados
            _chkTodos.Checked = _VS_AlunosSelecionados.Count.Equals(total);
            // Seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(_grvDocumentoAluno, VS_Ordenacao, VS_SortDirection);
        }

        protected void _grvDocumentoAluno_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridView grid = ((GridView)(sender));

            if (!string.IsNullOrEmpty(e.SortExpression))
            {
                VS_SortDirection = VS_SortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                VS_Ordenacao = e.SortExpression;
            }
            Pesquisar(grid.PageIndex);
        }

        protected void _chkSelecionar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkSelecionar = (CheckBox)sender;
                int row = ((GridViewRow)chkSelecionar.NamingContainer).RowIndex;
                long alu_id = (long)(_grvDocumentoAluno.DataKeys[row]["alu_id"]);
                bool contain = _VS_AlunosSelecionados.Contains(alu_id);

                if (chkSelecionar.Checked && !contain)
                    _VS_AlunosSelecionados.Add(alu_id);

                if (!chkSelecionar.Checked && contain)
                    _VS_AlunosSelecionados.Remove(alu_id);
            }
            catch (Exception ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar selecionar o aluno.", UtilBO.TipoMensagem.Erro);
                ApplicationWEB._GravaErro(ex);
            }
        }

        protected void _chkTodos_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                _VS_AlunosSelecionados.Clear();

                if (_chkTodos.Checked)
                {
                    DataTable dtAlunos = ACA_AlunoBO.BuscaAlunos_Anotacoes
                    (
                        UCBuscaDocenteTurma.ComboCalendario.Valor,
                        UCBuscaDocenteTurma.ComboEscola.Esc_ID,
                        UCBuscaDocenteTurma.ComboEscola.Uni_ID,
                        UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0],
                        UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1],
                        UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2],
                        UCBuscaDocenteTurma.ComboTurma.Valor[0],
                        UCCPeriodoCalendario.Cap_ID,
                        UCComboTurmaDisciplina.Valor,
                        Convert.ToByte(UCCamposBuscaAluno.TipoBuscaNomeAluno),
                        UCCamposBuscaAluno.NomeAluno,
                        string.IsNullOrEmpty(UCCamposBuscaAluno.DataNascAluno) ? new DateTime() : Convert.ToDateTime(UCCamposBuscaAluno.DataNascAluno),
                        UCCamposBuscaAluno.NomeMaeAluno,
                        UCCamposBuscaAluno.MatriculaAluno,
                        UCCamposBuscaAluno.MatriculaEstadualAluno,
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                        UCBuscaDocenteTurma.ComboEscola.Uad_ID,
                        __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao,
                        __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                        __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                        ChkEmitirAnterior.Checked,
                        0,
                        0,
                        (int)VS_SortDirection,
                        VS_Ordenacao,
                        false
                    );

                    _VS_AlunosSelecionados.AddRange(dtAlunos.Rows.Cast<DataRow>().Select(p => Convert.ToInt64(p["alu_id"])));
                }

                for (int i = 0; i < _grvDocumentoAluno.DataKeys.Count; i++)
                {
                    GridViewRow row = _grvDocumentoAluno.Rows[i];
                    CheckBox _chkSelecionar = (CheckBox)row.FindControl("_chkSelecionar");
                    long alu_id = (long)(_grvDocumentoAluno.DataKeys[row.RowIndex]["alu_id"]);
                    bool contain = _VS_AlunosSelecionados.Contains(alu_id);
                    if (_chkSelecionar != null)
                    {
                        _chkSelecionar.Checked = contain;
                        if (!contain)
                            _chkTodos.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar selecionar os alunos.", UtilBO.TipoMensagem.Erro);
                ApplicationWEB._GravaErro(ex);
            }
        }

        #endregion Eventos

        #region Delegates

        private void UCBuscaDocenteTurma_IndexChanged_Turma()
        {
            if (!divBuscaAluno.Visible)
            {
                try
                {
                    UCCPeriodoCalendario.Valor = new int[] { -1, -1 };
                    UCCPeriodoCalendario.PermiteEditar = false;

                    if (UCBuscaDocenteTurma.ComboTurma.Valor[0] > 0 && UCBuscaDocenteTurma.ComboTurma.Valor[1] > 0 && UCBuscaDocenteTurma.ComboTurma.Valor[2] > 0)
                    {
                        UCCPeriodoCalendario.CarregarPorTurma(UCBuscaDocenteTurma.ComboTurma.Valor[0]);

                        UCCPeriodoCalendario.SetarFoco();
                        UCCPeriodoCalendario.PermiteEditar = UCBuscaDocenteTurma.ComboTurma.Combo.SelectedIndex > 0;
                    }
                    else if (UCCPeriodoCalendario.Obrigatorio && UCBuscaDocenteTurma.ComboCalendario.Valor > 0)
                    {
                        UCCPeriodoCalendario.CarregarPorCalendario(UCBuscaDocenteTurma.ComboCalendario.Valor);
                        UCCPeriodoCalendario.SetarFoco();

                        if (UCBuscaDocenteTurma.ComboTurma.Combo.SelectedIndex > 0)
                            UCCPeriodoCalendario.PermiteEditar = true;
                        else
                        {
                            UCCPeriodoCalendario.Valor = new int[] { -1, -1 };
                            UCCPeriodoCalendario.PermiteEditar = false;
                        }
                        //UCCPeriodoCalendario.PermiteEditar = UCBuscaDocenteTurma.ComboTurma.Combo.SelectedIndex > 0;
                    }

                    UCCPeriodoCalendario_IndexChanged();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                }
            }
            else
            {
                //// Somente para carregar o valor -1 no combo
                //UCComboTurmaDisciplina.MostrarMensagemSelecione
                try
                {
                    UCComboTurmaDisciplina.Valor = -1;
                    UCComboTurmaDisciplina.PermiteEditar = false;
                    UCComboTurmaDisciplina.VS_MostraTerritorio = false;

                    if (UCBuscaDocenteTurma.ComboTurma.Valor[0] > 0 && UCBuscaDocenteTurma.ComboTurma.Valor[1] > 0 && UCBuscaDocenteTurma.ComboTurma.Valor[2] > 0)
                    {
                        if (UCBuscaDocenteTurma._VS_doc_id <= 0)
                            UCComboTurmaDisciplina.CarregarTurmaDisciplina(UCBuscaDocenteTurma.ComboTurma.Valor[0]);
                        else
                            UCComboTurmaDisciplina.CarregarTurmaDisciplina(UCBuscaDocenteTurma.ComboTurma.Valor[0], UCBuscaDocenteTurma._VS_doc_id);

                        UCCPeriodoCalendario.CarregarPorTurma(UCBuscaDocenteTurma.ComboTurma.Valor[0]);
                        UCCPeriodoCalendario.SetarFoco();

                        if (UCBuscaDocenteTurma.ComboTurma.Combo.SelectedIndex > 0)
                            UCCPeriodoCalendario.PermiteEditar = true;
                        else
                        {
                            UCCPeriodoCalendario.Valor = new int[] { -1, -1 };
                            UCCPeriodoCalendario.PermiteEditar = false;
                        }
                        //UCCPeriodoCalendario.PermiteEditar = UCBuscaDocenteTurma.ComboTurma.Combo.SelectedIndex > 0;

                        UCComboTurmaDisciplina.SetarFoco();
                        UCComboTurmaDisciplina.PermiteEditar = true;
                    }

                    UCCPeriodoCalendario_IndexChanged();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        private void UCBuscaDocenteTurma_IndexChanged_Calendario()
        {
            // se for o Resumo do conteudo programatico...
            if (rdbRelatorios.SelectedValue == DocDctRelSinteseAula.ToString())
            {
                // se o ano letivo selecionado for menor que 2015,
                // mostrar somente os componentes da regencia
                if (UCBuscaDocenteTurma.ComboCalendario.Cal_ano < 2015)
                {
                    UCComboTurmaDisciplina.VS_MostraFilhosRegencia = true;
                    UCComboTurmaDisciplina.VS_MostraRegencia = false;
                }
                // senao, mostrar somente a regencia
                else
                {
                    UCComboTurmaDisciplina.VS_MostraFilhosRegencia = false;
                    UCComboTurmaDisciplina.VS_MostraRegencia = true;
                }
            }
        }

        private void UCCPeriodoCalendario_IndexChanged()
        {
            if (!divBuscaAluno.Visible)
            {
                // Somente para carregar o valor -1 no combo
                try
                {
                    UCComboTurmaDisciplina.Valor = -1;
                    UCComboTurmaDisciplina.PermiteEditar = false;
                    UCComboTurmaDisciplina.VS_MostraTerritorio = false;

                    if (UCCPeriodoCalendario.Valor[0] > 0 && UCCPeriodoCalendario.Valor[1] > 0)
                    {
                        if (UCBuscaDocenteTurma._VS_doc_id <= 0)
                            UCComboTurmaDisciplina.CarregarTurmaDisciplina(UCBuscaDocenteTurma.ComboTurma.Valor[0], true, UCCPeriodoCalendario.Valor[1]);
                        else
                            UCComboTurmaDisciplina.CarregarTurmaDisciplina(UCBuscaDocenteTurma.ComboTurma.Valor[0], UCBuscaDocenteTurma._VS_doc_id, UCCPeriodoCalendario.Valor[1]);

                        UCComboTurmaDisciplina.SetarFoco();
                        UCComboTurmaDisciplina.PermiteEditar = UCCPeriodoCalendario.PermiteEditar;
                    }
                    else if (!UCCPeriodoCalendario.Obrigatorio && UCBuscaDocenteTurma.ComboTurma.Valor[0] > 0)
                    {
                        // Se for do tipo do Planejamento ou Orientacoes Alcançadas, busca pelas disciplinas do docente
                        if (UCBuscaDocenteTurma._VS_doc_id > 1 &&
                            (rdbRelatorios.SelectedValue == DocDctRelDadosPlanejamento.ToString()
                            || rdbRelatorios.SelectedValue == DocDctRelOrientacaoAlcancada.ToString()))
                            UCComboTurmaDisciplina.CarregarTurmaDisciplina(UCBuscaDocenteTurma.ComboTurma.Valor[0], UCBuscaDocenteTurma._VS_doc_id);
                        // Se for documento plano anual então não mostra tud_tipo de docencia compartilhada
                        else if (Convert.ToInt32(rdbRelatorios.SelectedValue) == DocDctPlanoAnual)
                        {
                            UCComboTurmaDisciplina.CarregarTurmaDisciplina_SemCompartilhada(UCBuscaDocenteTurma.ComboTurma.Valor[0], __SessionWEB.__UsuarioWEB.Docente.doc_id, true);
                        }
                        else
                            UCComboTurmaDisciplina.CarregarTurmaDisciplina(UCBuscaDocenteTurma.ComboTurma.Valor[0]);
                        UCComboTurmaDisciplina.SetarFoco();
                        UCComboTurmaDisciplina.PermiteEditar = UCCPeriodoCalendario.PermiteEditar;
                    }

                    UCComboTurmaDisciplina_IndexChanged();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                }
            }
            else if (Convert.ToInt32(rdbRelatorios.SelectedValue) == DocDctRelAnotacoesAula)
            {
                if (UCCPeriodoCalendario.Valor[0] > 0 && UCCPeriodoCalendario.Valor[1] > 0)
                {
                    if (UCBuscaDocenteTurma._VS_doc_id <= 0)
                        UCComboTurmaDisciplina.CarregarTurmaDisciplina(UCBuscaDocenteTurma.ComboTurma.Valor[0], true, UCCPeriodoCalendario.Valor[1]);
                    else
                        UCComboTurmaDisciplina.CarregarTurmaDisciplina(UCBuscaDocenteTurma.ComboTurma.Valor[0], UCBuscaDocenteTurma._VS_doc_id, UCCPeriodoCalendario.Valor[1]);

                    UCComboTurmaDisciplina.SetarFoco();
                    UCComboTurmaDisciplina.PermiteEditar = UCCPeriodoCalendario.PermiteEditar;
                }
                else
                {
                    if (UCBuscaDocenteTurma.ComboTurma.Valor[0] > 0 && UCBuscaDocenteTurma.ComboTurma.Valor[1] > 0 && UCBuscaDocenteTurma.ComboTurma.Valor[2] > 0)
                    {
                        if (UCBuscaDocenteTurma._VS_doc_id <= 0)
                            UCComboTurmaDisciplina.CarregarTurmaDisciplina(UCBuscaDocenteTurma.ComboTurma.Valor[0]);
                        else
                            UCComboTurmaDisciplina.CarregarTurmaDisciplina(UCBuscaDocenteTurma.ComboTurma.Valor[0], UCBuscaDocenteTurma._VS_doc_id);

                        UCComboTurmaDisciplina.SetarFoco();
                        UCComboTurmaDisciplina.PermiteEditar = true;
                    }
                }
            }
        }

        private void UCComboTurmaDisciplina_IndexChanged()
        {
            if (divAtividadeAvaliativa.Visible)
            {
                try
                {
                    UCComboAtividadeAvaliativa.Valor = -1;
                    UCComboAtividadeAvaliativa.PermiteEditar = false;

                    if (UCComboTurmaDisciplina.Valor > 0)
                    {
                        if (UCCPeriodoCalendario.Valor[0] > 0)
                            UCComboAtividadeAvaliativa.CarregarPorTurmaDisciplinaPeriodoCalendario(UCComboTurmaDisciplina.Valor, UCCPeriodoCalendario.Valor[0]);
                        else
                            UCComboAtividadeAvaliativa.CarregarAtividadePorTurma(UCComboTurmaDisciplina.Valor);

                        if (UCComboAtividadeAvaliativa.Count > 0)
                        {
                            UCComboAtividadeAvaliativa.Focus();
                            UCComboAtividadeAvaliativa.PermiteEditar = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        #endregion Delegates
    }
}
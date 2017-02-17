using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo;
using DevExpress.XtraReports.UI;
using ReportNameDocumentos = MSTech.GestaoEscolar.BLL.ReportNameDocumentos;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;

public partial class Documentos_DocumentoAluno_Busca : MotherPageLogado
{
    #region Constantes

    // Indicam a posicao das colunas no grid.
    protected const int columnMatriculaEstadual = 1;
    protected const int columnMatricula = 2;
    protected const int columnNumeroChamada = 3;
    protected const int columnNome = 4;
    protected const int columnEscola = 5;
    protected const int columnDataNascimento = 6;
    protected const int columnTurma = 7;
    protected const int columnCurso = 8;
    protected const int columnCalendario = 9;
    protected const int columnSituacao = 10;

    #endregion Constantes

    #region Propriedades

    /// <summary>
    /// Alunos selecionados no grid para impressao de seus documentos
    /// </summary>
    private SortedDictionary<long, bool> _VS_AlunosSelecionados
    {
        get
        {
            if (ViewState["_VS_AlunosSelecionados"] == null)
                ViewState["_VS_AlunosSelecionados"] = new SortedDictionary<long, bool>();
            return (SortedDictionary<long, bool>)ViewState["_VS_AlunosSelecionados"];
        }
    }

    //private Boolean _VS_PesquisaSalva
    //{
    //    get
    //    {
    //        return (Boolean)ViewState["_VS_PesquisaSalva"];
    //    }
    //    set
    //    {
    //        ViewState["_VS_PesquisaSalva"] = value;
    //    }
    //}

    /// <summary>
    /// Guarda os parametros usados no relatorio.
    /// </summary>
    private string _VS_SalvaParametros
    {
        get
        {
            return ViewState["_VS_SalvaParametros"].ToString();
        }
        set
        {
            ViewState["_VS_SalvaParametros"] = value;
        }
    }

    /// <summary>
    /// Salva o relatorio selecionado.
    /// </summary>
    private string _VS_SalvaReport
    {
        get
        {
            return ViewState["_VS_SalvaReport"].ToString();
        }
        set
        {
            ViewState["_VS_SalvaReport"] = value;
        }
    }

    /// <summary>
    /// Retorna o valor do o relatório selecionado.
    /// </summary>
    private ReportNameDocumentos TipoRelatorioSelecionado
    {
        get
        {
            int tipo = 0;
            if (!string.IsNullOrEmpty(_rdbRelatorios.SelectedValue))
                tipo = Convert.ToInt32(_rdbRelatorios.SelectedValue);

            return (ReportNameDocumentos)tipo;
        }
    }

    /// <summary>
    /// Guarda o ID do aluno
    /// </summary>
    private long VS_alu_id
    {
        get { return Convert.ToInt64(ViewState["VS_alu_id"]); }
        set { ViewState["VS_alu_id"] = value; }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada
    /// </summary>
    private string VS_Ordenacao
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.DocumentosAluno)
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                string valor;
                if (filtros.TryGetValue("VS_Ordenacao", out valor))
                {
                    return valor;
                }
            }

            return "";
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada
    /// </summary>
    private SortDirection VS_SortDirection
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.DocumentosAluno)
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                string valor;
                if (filtros.TryGetValue("VS_SortDirection", out valor))
                {
                    return (SortDirection)Enum.Parse(typeof(SortDirection), valor);
                }
            }

            return SortDirection.Ascending;
        }
    }

    /// <summary>
    /// Guarda o ID da unidade administrativa superior
    /// </summary>
    private Guid VS_uad_idSuperior
    {
        get { return new Guid(ViewState["VS_uad_idSuperior"].ToString()); }
        set { ViewState["VS_uad_idSuperior"] = value; }
    }

    /// <summary>
    /// Retorna o alu_id do registro que esta sendo editado no grid.
    /// </summary>
    public long EditItem
    {
        get
        {
            return Convert.ToInt64(_grvDocumentoAluno.DataKeys[_grvDocumentoAluno.EditIndex].Values["alu_id"]);
        }
    }

    /// <summary>
    /// Retorno a mtu_id do registro que esta sendo editado no grid.
    /// </summary>
    public int EditItemMatriculaTurma
    {
        get
        {
            return Convert.ToInt32(_grvDocumentoAluno.DataKeys[_grvDocumentoAluno.EditIndex].Values["mtu_id"]);
        }
    }

    #endregion Propriedades

    #region Page Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {

        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.Json));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsBuscaDocumentosAluno.js"));
        }

        if ((_rdbRelatorios.SelectedValue != ((int)ReportNameDocumentos.DeclaracaoSolicitacaoVaga).ToString())
            && (_rdbRelatorios.SelectedValue != ((int)ReportNameDocumentos.DeclaracaoSolicitacaoComparecimento).ToString())
            && (_rdbRelatorios.SelectedValue != ((int)ReportNameDocumentos.ConviteReuniao).ToString())
            && (!String.IsNullOrEmpty(_rdbRelatorios.SelectedValue)))
        {
            Page.ClientScript.RegisterStartupScript(GetType(), fdsPesquisa.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsPesquisa.ClientID)), true);
        }

        UCComboCursoCurriculo1.IndexChanged += UCComboCursoCurriculo1_IndexChanged;
        UCComboCurriculoPeriodo1.IndexChanged += UCComboCurriculoPeriodo1__OnSelectedIndexChange;
        if (string.IsNullOrEmpty(_rdbRelatorios.SelectedValue))
            UCComboCalendario1.IndexChanged += UCComboCalendario1_IndexChanged;

        //    _btnGerarRelatorio.Attributes.Add("onclick", String.Format("javascript: return ExtensaoDosFiltros('{0}');", __SessionWEB.__UsuarioWEB.Usuario.ent_id));
        //    btnGerarRelatorioCima.Attributes.Add("onclick", String.Format("javascript: return ExtensaoDosFiltros('{0}');", __SessionWEB.__UsuarioWEB.Usuario.ent_id));

        if (!string.IsNullOrEmpty(_rdbRelatorios.SelectedValue))
        {
            if (!CFG_ParametroDocumentoAlunoBO.ParametroValorBooleano(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, Convert.ToInt32(_rdbRelatorios.SelectedValue)))
            {
                _btnGerarRelatorio.Attributes.Add("onclick", String.Format("javascript: return ExtensaoDosFiltros('{0}');", __SessionWEB.__UsuarioWEB.Usuario.ent_id));
                btnGerarRelatorioCima.Attributes.Add("onclick", String.Format("javascript: return ExtensaoDosFiltros('{0}');", __SessionWEB.__UsuarioWEB.Usuario.ent_id));
            }
        }

        // Impede buscas desnecessárias para pop-up de períodos do calendário.
        _ddlPeriodo.Visible = false;

        if (!IsPostBack)
        {
            //_VS_PesquisaSalva = false;
            //_VS_SelecionarTodos = false;

            try
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                {
                    _lblMessage.Text = message;
                    __SessionWEB.PostMessages = String.Empty;
                }

                if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                {
                    uppPesquisa.Visible = false;
                    fdsDocumentos.Visible = false;
                    _lblMessage.Text = UtilBO.GetErroMessage("Você não possui permissão para acessar a página solicitada.", UtilBO.TipoMensagem.Alerta);
                }

                HabilitarFiltrosPadrao(false);

                Inicializar();

                CarregaDadosValidade();

                Page.Form.DefaultFocus = _rdbRelatorios.ClientID;
                Page.Form.DefaultButton = _btnPesquisar.UniqueID;

                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                _ddlQtPaginado.SelectedValue = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao.ToString() : qtItensPagina;

                // ******************************

                // Carrega o nome referente ao parametro de matricula estadual.
                string nomeMatriculaEstadual = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                bool mostraMatriculaEstadual = !string.IsNullOrEmpty(nomeMatriculaEstadual);

                _grvDocumentoAluno.Columns[columnMatricula].Visible = !mostraMatriculaEstadual;
                _grvDocumentoAluno.Columns[columnMatriculaEstadual].Visible = mostraMatriculaEstadual;
                _grvDocumentoAluno.Columns[columnMatriculaEstadual].HeaderText = nomeMatriculaEstadual;

                UCCamposBuscaAluno1.MostrarMatriculaEstadual = mostraMatriculaEstadual;
                UCCamposBuscaAluno1.TituloMatriculaEstadual = nomeMatriculaEstadual;

                _grvDocumentoAluno.Columns[columnCurso].HeaderText = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id);

                lblAvisoMensagem.Text = UtilBO.GetMessage("É necessário selecionar um tipo de documento.", UtilBO.TipoMensagem.Informacao);

                //fdsResultados.Style.Add("display", "none");
                fdsResultados.Visible = false;

                // adiciona atributo que controla se estao todas as paginas selecionavas
                //divSeleciona.Style.Add("display", "inline");
                //hdnSelecionaGrid.Value = "false";

                CarregaBusca();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }

        }
    }

    #endregion Page Life Cycle

    #region Eventos

    protected void _btnGerarRelatorio_Click(object sender, EventArgs e)
    {
        try
        {
            SalvaBusca();

            __SessionWEB.PostMessages = String.Empty;
            string report = String.Empty;
            string parametros = String.Empty;
            string dem_ids; // ids dos registros na tabela de validacao de documentos
            string esc_ids = String.Empty;
            string alu_ids_boletim = String.Empty;
            string mtu_ids_boletim = String.Empty;
            string mtu_ids = String.Empty;
            bool bGerarRelatorio = true;
            XtraReport DevReport = null;

            // Alu_ids não ordenado pelo id, e sim pelo nome
            string alu_ids_boletim_nao_ordenado = String.Empty;

            _VS_AlunosSelecionados.Clear();

            //bool bTodasPaginas = false;
            //Boolean.TryParse(hdnSelecionaGrid.Value, out bTodasPaginas);

            if (_chkTodos.Checked)
            {
                DataTable dtAlunos = SelecionaDataSource(0, true);

                foreach (DataRow row in dtAlunos.Rows)
                {
                    alu_ids_boletim_nao_ordenado = (String.IsNullOrEmpty(alu_ids_boletim_nao_ordenado) ? "" : alu_ids_boletim_nao_ordenado + ",") + row["alu_id"];

                    if (!_VS_AlunosSelecionados.ContainsKey(Convert.ToInt64(row["alu_id"])))
                    {
                        esc_ids = !String.IsNullOrEmpty(esc_ids) ? String.Concat(esc_ids + ',', row["esc_id"].ToString()) : row["esc_id"].ToString();
                        _VS_AlunosSelecionados.Add(Convert.ToInt64(row["alu_id"]), true);
                        mtu_ids = (String.IsNullOrEmpty(mtu_ids) ? "" : mtu_ids + ",") + row["mtu_id"];
                    }

                    if (Convert.ToInt32(_rdbRelatorios.SelectedValue).Equals((int)ReportNameDocumentos.BoletimEscolar) && ChkEmitirAnterior.Checked)
                    {
                        alu_ids_boletim = (String.IsNullOrEmpty(alu_ids_boletim) ? "" : alu_ids_boletim + ",") + row["alu_id"];
                        mtu_ids_boletim = (String.IsNullOrEmpty(mtu_ids_boletim) ? "" : mtu_ids_boletim + ",") + row["mtu_id"];
                    }
                    if (Convert.ToInt32(_rdbRelatorios.SelectedValue).Equals((int)ReportNameDocumentos.FichaIndividualAluno)
                        || Convert.ToInt32(_rdbRelatorios.SelectedValue).Equals((int)ReportNameDocumentos.FichaIndividualAvaliacaoPeriodica)
                        || Convert.ToInt32(_rdbRelatorios.SelectedValue).Equals((int)ReportNameDocumentos.DocAluCartaoIdentificacao)
                        || Convert.ToInt32(_rdbRelatorios.SelectedValue).Equals((int)ReportNameDocumentos.DocAluCarteirinhaEstudante)
                        || Convert.ToInt32(_rdbRelatorios.SelectedValue).Equals((int)ReportNameDocumentos.DocAluDeclaracaoComparecimento)
                        || Convert.ToInt32(_rdbRelatorios.SelectedValue).Equals((int)ReportNameDocumentos.DocAluFichaMatricula))
                    {
                        alu_ids_boletim = (String.IsNullOrEmpty(alu_ids_boletim) ? "" : alu_ids_boletim + ",") + row["alu_id"];
                        mtu_ids_boletim = (String.IsNullOrEmpty(mtu_ids_boletim) ? "" : mtu_ids_boletim + ",") + row["mtu_id"];
                    }
                }
            }
            else // se só a pagina atual tem selecionados corre as linhas do grid pegando os alu_ids atuais
            {
                foreach (GridViewRow row in _grvDocumentoAluno.Rows)
                {
                    CheckBox chkSelecionar = (CheckBox)row.FindControl("_chkSelecionar");

                    if (chkSelecionar.Checked)
                    {
                        alu_ids_boletim_nao_ordenado = (String.IsNullOrEmpty(alu_ids_boletim_nao_ordenado) ? "" : alu_ids_boletim_nao_ordenado + ",") + _grvDocumentoAluno.DataKeys[row.RowIndex].Values["alu_id"];

                        if (!_VS_AlunosSelecionados.ContainsKey(Convert.ToInt64(_grvDocumentoAluno.DataKeys[row.RowIndex].Values["alu_id"])))
                        {
                            _VS_AlunosSelecionados.Add(Convert.ToInt64(_grvDocumentoAluno.DataKeys[row.RowIndex].Values["alu_id"]), true);
                            esc_ids = !String.IsNullOrEmpty(esc_ids) ? String.Concat(esc_ids + ',', Convert.ToString(_grvDocumentoAluno.DataKeys[row.RowIndex].Values["esc_id"])) : Convert.ToString(_grvDocumentoAluno.DataKeys[row.RowIndex].Values["esc_id"]);
                            mtu_ids = (String.IsNullOrEmpty(mtu_ids) ? "" : mtu_ids + ",") + _grvDocumentoAluno.DataKeys[row.RowIndex].Values["mtu_id"];
                        }

                        if (Convert.ToInt32(_rdbRelatorios.SelectedValue).Equals((int)ReportNameDocumentos.BoletimEscolar)
                            && ChkEmitirAnterior.Checked)
                        {
                            alu_ids_boletim = (String.IsNullOrEmpty(alu_ids_boletim) ? "" : alu_ids_boletim + ",") + _grvDocumentoAluno.DataKeys[row.RowIndex].Values["alu_id"];
                            mtu_ids_boletim = (String.IsNullOrEmpty(mtu_ids_boletim) ? "" : mtu_ids_boletim + ",") + _grvDocumentoAluno.DataKeys[row.RowIndex].Values["mtu_id"];
                        }
                        if (Convert.ToInt32(_rdbRelatorios.SelectedValue).Equals((int)ReportNameDocumentos.FichaIndividualAluno)
                            || Convert.ToInt32(_rdbRelatorios.SelectedValue).Equals((int)ReportNameDocumentos.FichaIndividualAvaliacaoPeriodica)
                            || Convert.ToInt32(_rdbRelatorios.SelectedValue).Equals((int)ReportNameDocumentos.DocAluCartaoIdentificacao)
                            || Convert.ToInt32(_rdbRelatorios.SelectedValue).Equals((int)ReportNameDocumentos.DocAluCarteirinhaEstudante)
                            || Convert.ToInt32(_rdbRelatorios.SelectedValue).Equals((int)ReportNameDocumentos.DocAluDeclaracaoComparecimento)
                            || Convert.ToInt32(_rdbRelatorios.SelectedValue).Equals((int)ReportNameDocumentos.DocAluFichaMatricula))
                        {
                            alu_ids_boletim = (String.IsNullOrEmpty(alu_ids_boletim) ? "" : alu_ids_boletim + ",") + _grvDocumentoAluno.DataKeys[row.RowIndex].Values["alu_id"];
                            mtu_ids_boletim = (String.IsNullOrEmpty(mtu_ids_boletim) ? "" : mtu_ids_boletim + ",") + _grvDocumentoAluno.DataKeys[row.RowIndex].Values["mtu_id"];
                        }
                    }
                }
            }

            if (_VS_AlunosSelecionados.Count > 0)
            {
                string alu_ids = String.Empty;

                foreach (KeyValuePair<long, bool> kvp in _VS_AlunosSelecionados)
                {
                    alu_ids = !String.IsNullOrEmpty(alu_ids) ? String.Concat(alu_ids + ',', kvp.Key) : kvp.Key.ToString();
                }

                String sDeclaracaoHMTL = string.Empty;
                String sReportDev = string.Empty;

                if (!String.IsNullOrEmpty(_rdbRelatorios.SelectedValue))
                {
                    int selectedValue = Convert.ToInt32(_rdbRelatorios.SelectedValue);
                    if (selectedValue.Equals((int)ReportNameDocumentos.BoletimEscolar))
                    {
                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        report = ((int)ReportNameDocumentos.BoletimEscolar).ToString();
                        parametros = "alu_id=" + (ChkEmitirAnterior.Checked ? alu_ids_boletim : alu_ids_boletim_nao_ordenado) +
                                     "&mtu_id=" + mtu_ids_boletim +
                                     "&tur_id=" + UCComboTurma1.Valor[0] +
                                     "&cal_id=" + UCComboCalendario1.Valor +
                                     "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                     "&situacao=" + ChkEmitirAnterior.Checked +
                                     "&esc_exibeBoletimUnicoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_BOLETIM_UNICO_ESCOLA, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                     "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString(), ApplicationWEB.LogoRelatorioSSRS) +
                                     "&atg_tipo=" + Convert.ToInt32(ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.CabecalhoRelatorio).ToString();
                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.DocAluDeclaracaoComparecimento))
                    {
                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        report = ((int)ReportNameDocumentos.DocAluDeclaracaoComparecimento).ToString();
                        parametros = "alu_id=" + alu_ids_boletim +
                                     "&mtu_id=" + mtu_ids_boletim +
                                     "&esc_id=" + UCComboUAEscola.Esc_ID +
                                     "&uni_id=" + UCComboUAEscola.Uni_ID;
                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.DeclaracaoMatricula))
                    {
                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        report = ((int)ReportNameDocumentos.DeclaracaoMatricula).ToString();
                        parametros = "alu_ids=" + alu_ids +
                                     "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE) +
                                     "&email=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_EMAIL) +
                                     "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                     "&tur_id=" + UCComboTurma1.Valor[0] +
                                     "&esc_id=" + UCComboUAEscola.Esc_ID +
                                     "&uni_id=" + UCComboUAEscola.Uni_ID +
                                     "&situacao=false";
                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.DeclaracaoMatriculaExAluno))
                    {
                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        report = ((int)ReportNameDocumentos.DeclaracaoMatriculaExAluno).ToString();
                        parametros = "alu_id=" + alu_ids +
                                     "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE) +
                                     "&email=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_EMAIL) +
                                     "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                     "&tur_id=" + UCComboTurma1.Valor[0] +
                                     "&esc_id=" + UCComboUAEscola.Esc_ID +
                                     "&uni_id=" + UCComboUAEscola.Uni_ID +
                                     "&situacao=false";
                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.DeclaracaoPedidoTransferencia))
                    {
                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        report = ((int)ReportNameDocumentos.DeclaracaoPedidoTransferencia).ToString();
                        parametros = "alu_ids=" + alu_ids +
                                     "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE) +
                                     "&email=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_EMAIL) +
                                     "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                     "&tur_id=" + UCComboTurma1.Valor[0] +
                                     "&esc_id=" + UCComboUAEscola.Esc_ID +
                                     "&uni_id=" + UCComboUAEscola.Uni_ID +
                                     "&situacao=false";
                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.DeclaracaoConclusaoCurso))
                    {
                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        report = ((int)ReportNameDocumentos.DeclaracaoConclusaoCurso).ToString();
                        parametros = "alu_id=" + alu_ids +
                                     "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE) +
                                     "&email=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_EMAIL) +
                                     "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                     "&tur_id=" + UCComboTurma1.Valor[0] +
                                     "&esc_id=" + UCComboUAEscola.Esc_ID +
                                     "&uni_id=" + UCComboUAEscola.Uni_ID +
                                     "&situacao=false";
                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.DeclaracaoExAlunoUnidadeEscolar))
                    {
                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        report = ((int)ReportNameDocumentos.DeclaracaoExAlunoUnidadeEscolar).ToString();
                        parametros = "alu_id=" + alu_ids +
                                     "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE) +
                                     "&email=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_EMAIL) +
                                     "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                     "&tur_id=" + UCComboTurma1.Valor[0] +
                                     "&esc_id=" + UCComboUAEscola.Esc_ID +
                                     "&uni_id=" + UCComboUAEscola.Uni_ID +
                                     "&situacao=false";
                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.DeclaracaoMatriculaPeriodo))
                    {
                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        report = ((int)ReportNameDocumentos.DeclaracaoMatriculaPeriodo).ToString();
                        parametros = "alu_id=" + alu_ids +
                                     "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE) +
                                     "&email=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_EMAIL) +
                                     "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                     "&tur_id=" + UCComboTurma1.Valor[0] +
                                     "&esc_id=" + UCComboUAEscola.Esc_ID +
                                     "&uni_id=" + UCComboUAEscola.Uni_ID +
                                     "&situacao=false";
                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.FichaIndividualAluno))
                    {
                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        bool filtraPeriodo;
                        Boolean.TryParse(CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.FILTRA_POR_PERIODO, __SessionWEB.__UsuarioWEB.Usuario.ent_id), out filtraPeriodo);
                        report = ((int)ReportNameDocumentos.FichaIndividualAluno).ToString();

                        parametros = "alu_id=" + alu_ids_boletim +
                                     "&mtu_id=" + mtu_ids_boletim +
                                     "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                     "&situacao=" + ChkEmitirAnterior.Checked +
                                     "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE) +
                                     "&email=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_EMAIL) +
                                     "&cal_id=" + UCComboCalendario1.Valor +
                                     "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString(), ApplicationWEB.LogoRelatorioSSRS) +
                                     "&atg_tipo=" + ((int)ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.CabecalhoRelatorio).ToString();
                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.FichaCadastralAluno))
                    {
                        // ID do tipo de documento NIS.
                        Guid tdo_idNis = ACA_ParametroAcademicoBO.ParametroValorGuidPorEntidade(eChaveAcademico.TIPO_DOCUMENTACAO_NIS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        report = ((int)ReportNameDocumentos.FichaCadastralAluno).ToString();
                        parametros = "alu_id=" + alu_ids +
                                     "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                     "&tur_id=" + UCComboTurma1.Valor[0] +
                                     "&doc_nis_id=" + tdo_idNis +
                                     "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE) +
                                     "&email=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_EMAIL) +
                                     "&emitirDocAnoAnt=" + ChkEmitirAnterior.Checked +
                                     "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString(), ApplicationWEB.LogoRelatorioSSRS) +
                                     "&atg_tipo=" + Convert.ToInt32(ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.CabecalhoRelatorio).ToString();

                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.AutorizacaoPasseio))
                    {
                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        report = ((int)ReportNameDocumentos.AutorizacaoPasseio).ToString();
                        parametros = "alu_id=" + alu_ids +
                                     "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE) +
                                     "&email=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_EMAIL) +
                                     "&situacao=false";
                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.ControleRecebimentoAPM))
                    {
                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        report = ((int)ReportNameDocumentos.ControleRecebimentoAPM).ToString();
                        parametros = "alu_ids=" + alu_ids +
                                     "&tur_id=" + UCComboTurma1.Valor[0] +
                                     "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE) +
                                     "&email=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_EMAIL) +
                                     "&cal_id=" + UCComboCalendario1.Valor +
                                     "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                     "&situacao=false";
                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.TermoCompromisso))
                    {
                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        report = ((int)ReportNameDocumentos.TermoCompromisso).ToString();
                        parametros = "alu_id=" + alu_ids +
                                     "&cur_id=" + UCComboCurriculoPeriodo1.Valor[0] +
                                     "&crr_id=" + UCComboCurriculoPeriodo1.Valor[1] +
                                     "&crp_id=" + UCComboCurriculoPeriodo1.Valor[2] +
                                     "&tdo_id=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_RG) +
                                     "&esc_id=" + UCComboUAEscola.Esc_ID +
                                     "&uni_id=" + UCComboUAEscola.Uni_ID +
                                     "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE) +
                                     "&email=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_EMAIL) +
                                     "&situacao=false" +
                                     "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString() +
                                     "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString(), ApplicationWEB.LogoRelatorioSSRS) +
                                     "&atg_tipo=" + Convert.ToInt32(ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.CabecalhoRelatorio).ToString();
                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.ComprovanteEfetivacao))
                    {
                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        report = ((int)ReportNameDocumentos.ComprovanteEfetivacao).ToString();
                        parametros = String.Empty;
                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.HistoricoEscolar))
                    {
                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        sReportDev = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.REPORT_DEVEXPRESS, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);

                        report = ((int)ReportNameDocumentos.HistoricoEscolar).ToString();

                        #region Gera_Validacao_documento

                        bool bValidaDocto;
                        Boolean.TryParse(CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.VALIDAR_DOCUMENTO_EMITIDO, __SessionWEB.__UsuarioWEB.Usuario.ent_id, Convert.ToInt32(report)), out bValidaDocto);
                        dem_ids = "-1";

                        #endregion Gera_Validacao_documento



                        parametros = "alu_id=" + alu_ids +
                                     "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                     "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE) +
                                     "&email=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_EMAIL) +
                                     "&emitirDocAnoAnt=" + ChkEmitirAnterior.Checked +
                                     "&dem_id=" + dem_ids +
                                     "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString(), ApplicationWEB.LogoRelatorioSSRS) +
                                     "&atg_tipo=" + Convert.ToInt32(ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.CabecalhoRelatorio).ToString();


                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.HistoricoEscolarPedagogico))
                    {
                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        sReportDev = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.REPORT_DEVEXPRESS, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);

                        report = ((int)ReportNameDocumentos.HistoricoEscolarPedagogico).ToString();

                        #region Gera_Validacao_documento

                        bool bValidaDocto;
                        Boolean.TryParse(CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.VALIDAR_DOCUMENTO_EMITIDO, __SessionWEB.__UsuarioWEB.Usuario.ent_id, Convert.ToInt32(report)), out bValidaDocto);
                        dem_ids = "-1";

                        #endregion Gera_Validacao_documento

                        if (string.IsNullOrEmpty(sReportDev) || !Convert.ToBoolean(sReportDev))
                        {
                            parametros = "alu_id=" + alu_ids +
                                         "&mtu_id=" + mtu_ids +
                                         "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                         "&emitirDocAnoAnt=" + ChkEmitirAnterior.Checked +
                                         "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocHistoricoEscolarPedagogico.Municipio").ToString() +
                                         "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocHistoricoEscolarPedagogico.Secretaria").ToString();
                        }
                        else
                        {
                            DevReport = new MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo.HistoricoEscolar
                                (alu_ids,
                                 mtu_ids,
                                __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                                GetGlobalResourceObject("Reporting", "Reporting.DocHistoricoEscolarPedagogico.Municipio").ToString(),
                                GetGlobalResourceObject("Reporting", "Reporting.DocHistoricoEscolarPedagogico.Secretaria").ToString());
                        }
                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.DeclaracaoEscolaridade))
                    {
                        // ID do tipo de documento NIS.
                        Guid tdo_idNis = ACA_ParametroAcademicoBO.ParametroValorGuidPorEntidade(eChaveAcademico.TIPO_DOCUMENTACAO_NIS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        report = ((int)ReportNameDocumentos.DeclaracaoEscolaridade).ToString();
                        parametros = "alu_ids=" + alu_ids +
                                     "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                     "&tipo_grupamento=" + _rblGrupamento.SelectedValue +
                                     "&destinatario=" + _txtDestinatario.Text +
                                     "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE) +
                                     "&emitirDocAnoAnt=" + ChkEmitirAnterior.Checked +
                                     "&doc_nis_id=" + tdo_idNis +
                                     "&pso_id=" + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.PROGRAMA_SOCIAL_BOLSA_FAMILIA, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                     "&exibirFrequencia=" + rdbExibirFrequencia.SelectedValue;
                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.CertificadoConclusaoCurso))
                    {
                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        report = ((int)ReportNameDocumentos.CertificadoConclusaoCurso).ToString();
                        parametros = "alu_id=" + alu_ids +
                                     "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                     "&cal_id=" + UCComboCalendario1.Valor +
                                     "&tur_id=" + UCComboTurma1.Valor[0] +
                                     "&emitirDocAnoAnt=true";

                        #region Gera_Validacao_documento

                        bool bValidaDocto;
                        Boolean.TryParse(CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.VALIDAR_DOCUMENTO_EMITIDO, __SessionWEB.__UsuarioWEB.Usuario.ent_id, Convert.ToInt32(report)), out bValidaDocto);
                        parametros += "&dem_id=-1";

                        #endregion Gera_Validacao_documento
                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.ComprovanteMatricula))
                    {
                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        report = ((int)ReportNameDocumentos.ComprovanteMatricula).ToString();
                        parametros = "alu_ids=" + alu_ids +
                                     "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE) +
                                     "&email=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_EMAIL) +
                                     "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                     "&emitirDocAnoAnt=" + ChkEmitirAnterior.Checked;
                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.DeclaracaoSolicitacaoTransferencia))
                    {
                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        report = ((int)ReportNameDocumentos.DeclaracaoSolicitacaoTransferencia).ToString();
                        parametros = "alu_ids=" + alu_ids +
                                     "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE) +
                                     "&email=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_EMAIL) +
                                     "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                     "&tur_id=" + UCComboTurma1.Valor[0] +
                                     "&esc_id=" + UCComboUAEscola.Esc_ID +
                                     "&uni_id=" + UCComboUAEscola.Uni_ID +
                                     "&situacao=false";
                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.EncaminhamentoAlunoRemanejado))
                    {
                        bGerarRelatorio = false;

                        if (_VS_AlunosSelecionados.Count == 1)
                        {
                            foreach (GridViewRow row in _grvDocumentoAluno.Rows)
                            {
                                CheckBox chkSelecionar = (CheckBox)row.FindControl("_chkSelecionar");

                                if (chkSelecionar.Checked)
                                {
                                    lblDadosAlunoRemanej.Text = "<b>Aluno: </b> " + _grvDocumentoAluno.DataKeys[row.RowIndex].Values["pes_nome"] + "<br><br>";
                                    lblDadosAlunoRemanej.Text += "<b> Escola de origem:</b> " + _grvDocumentoAluno.DataKeys[row.RowIndex].Values["tur_escolaUnidade"] + "<br><br>";
                                    lblDadosAlunoRemanej.Text += "<b> Escola de destino: </b>" + _grvDocumentoAluno.DataKeys[row.RowIndex].Values["EscolaUniDestino"] + "<br><br>";
                                    lblDadosAlunoRemanej.Text += "<b>" + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " de destino </b>" + _grvDocumentoAluno.DataKeys[row.RowIndex].Values["GrupamentoDestino"] + "<br><br>";
                                }
                            }
                        }

                        DateTime dt;

                        // Configura os dados do aluno na tela, apenas quando selecionar apenas 1 aluno.
                        // Só gera o relatório no botão da popup divEncaminhamentoRemanejado.
                        if (!string.IsNullOrEmpty(txtDataEncaminhamento.Text) &&
                            !string.IsNullOrEmpty(txtHoraInicialEnc.Text) &&
                            !string.IsNullOrEmpty(txtHoraFinalEnc.Text) &&
                            DateTime.TryParse(txtDataEncaminhamento.Text, out dt) &&
                            DateTime.TryParse(txtHoraFinalEnc.Text, out dt) &&
                            DateTime.TryParse(txtHoraInicialEnc.Text, out dt) &&
                            Convert.ToDateTime(txtHoraFinalEnc.Text) > Convert.ToDateTime(txtHoraInicialEnc.Text))
                        {
                            sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                            report = ((int)ReportNameDocumentos.EncaminhamentoAlunoRemanejado).ToString();
                            parametros = "alu_ids=" + alu_ids +
                                         "&Data=" + txtDataEncaminhamento.Text +
                                         "&horaInicio=" + txtHoraInicialEnc.Text +
                                         "&horaFim=" + txtHoraFinalEnc.Text +
                                         "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                         "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE);
                            lblDadosAlunoRemanej.Text = string.Empty;
                            bGerarRelatorio = true;
                        }
                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.FichaIndividualAvaliacaoPeriodica))
                    {
                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        bool filtraPeriodo;
                        Boolean.TryParse(CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.FILTRA_POR_PERIODO, __SessionWEB.__UsuarioWEB.Usuario.ent_id), out filtraPeriodo);
                        report = ((int)ReportNameDocumentos.FichaIndividualAvaliacaoPeriodica).ToString();

                        parametros = "alu_id=" + alu_ids_boletim +
                                     "&mtu_id=" + mtu_ids_boletim +
                                     "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                     "&situacao=false" +
                                     "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE) +
                                     "&email=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_EMAIL) +
                                     "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString(), ApplicationWEB.LogoRelatorioSSRS) +
                                     "&atg_tipo=" + Convert.ToInt32(ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.CabecalhoRelatorio).ToString();

                        if (filtraPeriodo)
                        {
                            string[] periodo = _ddlPeriodo.SelectedValue.Split(';');
                            parametros += "&cal_id=" + periodo[0] +
                                          "&cap_id=" + periodo[1];
                        }
                        else
                            parametros += "&cal_id=" + UCComboCalendario1.Valor;
                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.DocAluCartaoIdentificacao))
                    {
                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        report = ((int)ReportNameDocumentos.DocAluCartaoIdentificacao).ToString();
                        parametros = "alu_id=" + alu_ids_boletim +
                                     "&mtu_id=" + mtu_ids_boletim;
                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.DocAluCarteirinhaEstudante))
                    {
                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        report = ((int)ReportNameDocumentos.DocAluCarteirinhaEstudante).ToString();
                        parametros = "alu_id=" + alu_ids_boletim +
                                     "&mtu_id=" + mtu_ids_boletim;
                    }
                    else if (selectedValue.Equals((int)ReportNameDocumentos.DocAluFichaMatricula))
                    {
                        sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id, selectedValue);
                        report = ((int)ReportNameDocumentos.DocAluFichaMatricula).ToString();
                        parametros = "alu_id=" + alu_ids_boletim +
                                     "&mtu_id=" + mtu_ids_boletim +
                                     "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                     "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString(), ApplicationWEB.LogoRelatorioSSRS) +
                                     "&atg_tipo=" + Convert.ToInt32(ACA_AvisoTextoGeralBO.eTiposAvisosTextosGerais.CabecalhoRelatorio).ToString() +
                                     "&matriculaEstadual=" + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    }
                    else
                    {
                        throw new ValidationException("Relatório não implementado.");
                    }

                    SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);

                    if (bGerarRelatorio)
                    {
                        if (string.IsNullOrEmpty(sDeclaracaoHMTL) || !Convert.ToBoolean(sDeclaracaoHMTL))
                        {
                            if (string.IsNullOrEmpty(sReportDev) || !Convert.ToBoolean(sReportDev))
                            {
                                CFG_RelatorioBO.CallReport("Documentos", report, parametros, HttpContext.Current);
                            }
                            else
                            {
                                GestaoEscolarUtilBO.SendParametersToReport(DevReport);
                                Response.Redirect("~/Documentos/RelatorioDev.aspx", false);
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            }
                        }
                        else
                        {
                            switch (Convert.ToInt16(report))
                            {
                                case (int)ReportNameDocumentos.DeclaracaoMatricula:
                                    parametros += "&Matricula_Estadual=" + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                                    parametros += "&Nome_Relatorio=" + _rdbRelatorios.SelectedItem.Text;
                                    Response.Redirect("~/Documentos/Declaracoes/DeclaracoesAluno.aspx", false);
                                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                                    break;

                                case (int)ReportNameDocumentos.DeclaracaoConclusaoCurso:
                                    parametros += "&Matricula_Estadual=" + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                                    parametros += "&Nome_Relatorio=" + _rdbRelatorios.SelectedItem.Text;
                                    Response.Redirect("~/Documentos/Declaracoes/DeclaracoesAluno.aspx", false);
                                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                                    break;

                                case (int)ReportNameDocumentos.DeclaracaoExAlunoUnidadeEscolar:
                                    parametros += "&Matricula_Estadual=" + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                                    parametros += "&Nome_Relatorio=" + _rdbRelatorios.SelectedItem.Text;
                                    Response.Redirect("~/Documentos/Declaracoes/DeclaracoesAluno.aspx", false);
                                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                                    break;

                                case (int)ReportNameDocumentos.DeclaracaoMatriculaExAluno:
                                    parametros += "&Matricula_Estadual=" + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                                    parametros += "&Nome_Relatorio=" + _rdbRelatorios.SelectedItem.Text;
                                    Response.Redirect("~/Documentos/Declaracoes/DeclaracoesAluno.aspx", false);
                                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                                    break;

                                case (int)ReportNameDocumentos.DeclaracaoPedidoTransferencia:
                                    parametros += "&Matricula_Estadual=" + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                                    parametros += "&Nome_Relatorio=" + _rdbRelatorios.SelectedItem.Text;
                                    Response.Redirect("~/Documentos/Declaracoes/DeclaracoesAluno.aspx", false);
                                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                                    break;

                                case (int)ReportNameDocumentos.DeclaracaoMatriculaPeriodo:
                                    parametros += "&Matricula_Estadual=" + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                                    parametros += "&Nome_Relatorio=" + _rdbRelatorios.SelectedItem.Text;
                                    Response.Redirect("~/Documentos/Declaracoes/DeclaracoesAluno.aspx", false);
                                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                                    break;

                                case (int)ReportNameDocumentos.DeclaracaoSolicitacaoTransferencia:
                                    parametros += "&Matricula_Estadual=" + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                                    parametros += "&Nome_Relatorio=" + _rdbRelatorios.SelectedItem.Text;
                                    Response.Redirect("~/Documentos/Declaracoes/DeclaracoesAluno.aspx", false);
                                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                                    break;

                                case (int)ReportNameDocumentos.BoletimEscolar:

                                    parametros += "&Matricula_Estadual=" + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                                    parametros += "&Nome_Relatorio=" + _rdbRelatorios.SelectedItem.Text;

                                    _VS_SalvaParametros = parametros;
                                    _VS_SalvaReport = report;

                                    grvPeriodoDeAvaliacao.DataSource = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(UCComboCalendario1.Valor, ApplicationWEB.AppMinutosCacheLongo);
                                    grvPeriodoDeAvaliacao.DataBind();

                                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "AlertaMat", "$(document).ready(function(){ $('#divPeriodoDeAvaliacao').dialog('open'); });", true);
                                    break;

                                case (int)ReportNameDocumentos.DocAluDeclaracaoComparecimento:

                                    _VS_SalvaParametros = parametros;
                                    _VS_SalvaReport = report;

                                    UCComboTipoResponsavelAluno1.CarregarTipoResponsavelAluno(false, false);

                                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "AlertaMat", "$(document).ready(function(){ $('#divDeclaracaoComparecimento').dialog('open'); });", true);
                                    break;

                                case (int)ReportNameDocumentos.DeclaracaoEscolaridade:

                                    parametros = "alu_ids=" + alu_ids +
                                                 "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE) +
                                                 "&email=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_EMAIL) +
                                                 "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                                 "&tur_id=" + UCComboTurma1.Valor[0] +
                                                 "&esc_id=" + UCComboUAEscola.Esc_ID +
                                                 "&uni_id=" + UCComboUAEscola.Uni_ID +
                                                 "&situacao=" + ChkEmitirAnterior.Checked +
                                                 "&Matricula_Estadual=" + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                                                 "&Nome_Relatorio=" + _rdbRelatorios.SelectedItem.Text;

                                    Response.Redirect("~/Documentos/Declaracoes/DeclaracoesAluno.aspx", false);
                                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                                    break;

                            }
                            CFG_RelatorioBO.SendParametersToReport(report, parametros);
                        }
                    }
                }
                else
                {
                    throw new ValidationException("Selecione uma opção de documento.");
                }
            }
            else
            {
                throw new ValidationException("Selecione pelo menos um aluno para gerar documento.");
            }
        }
        catch (ValidationException ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception err)
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
            ApplicationWEB._GravaErro(err);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o documento do aluno.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void _btnGerarRelatorio1_Click(object sender, EventArgs e)
    {
        _rfvPeriodo.Validate();
        if (_rfvPeriodo.IsValid)
            _btnGerarRelatorio_Click(sender, e);

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "AlertaMat", "$(document).ready(function(){ $('#divPeriodoDeAvaliacao').dialog('open'); });", true);
    }

    protected void _btnGerarRelatorio2_Click(object sender, EventArgs e)
    {
        _rfvGrupamento.Validate();
        if (_rfvGrupamento.IsValid)
            _btnGerarRelatorio_Click(sender, e);

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "AlertaMat", "$(document).ready(function(){ $('#divPeriodoDeAvaliacao').dialog('open'); });", true);
    }

    protected void btnGerarDeclaracao_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                if (Convert.ToDateTime(txtDataDeclaracao.Text + " " + txtHoraInicioDeclaracao.Text) >=
                    Convert.ToDateTime(txtDataDeclaracao.Text + " " + txtHoraFimDeclaracao.Text))
                    throw new ValidationException("O horário final da declaração deve ser maior que o horário inicial.");

                string parametros = _VS_SalvaParametros;

                parametros += "&tra_id=" + UCComboTipoResponsavelAluno1.Valor.ToString();
                parametros += "&data=" + txtDataDeclaracao.Text;
                parametros += "&horaInicio=" + txtHoraInicioDeclaracao.Text;
                parametros += "&horaFim=" + txtHoraFimDeclaracao.Text;
                parametros += "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE);
                parametros += "&email=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_EMAIL);
                parametros += "&Nome_Relatorio=" + _rdbRelatorios.SelectedItem.Text;

                CFG_RelatorioBO.SendParametersToReport(_VS_SalvaReport, parametros);

                Response.Redirect("~/Documentos/Declaracoes/DeclaracoesAluno.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "AlertaMat", "$(document).ready(function(){ $('#divDeclaracaoComparecimento').dialog('close'); });", true);
            }
        }
        catch (ValidationException ex)
        {
            _lblMessageDeclaracao.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception err)
        {
            ApplicationWEB._GravaErro(err);
            _lblMessageDeclaracao.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o documento do aluno.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void btnGerarEncaminhamentoRemanej_Click(object sender, EventArgs e)
    {
        //rfv.Validate();
        if (Page.IsValid)
        {
            _btnGerarRelatorio_Click(sender, e);
        }
    }

    protected void _rblGrupamento_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_rblGrupamento.SelectedValue == "1")
        {
            lblExibirFrequencia.Visible = true;
            rdbExibirFrequencia.Visible = true;
        }
        else
        {
            lblExibirFrequencia.Visible = false;
            rdbExibirFrequencia.Visible = false;
        }
    }

    protected void _btnPesquisar_Click(object sender, EventArgs e)
    {
        try
        {
            // Validação de preenchimento para o HistoricoEscolarSMESP
            if (Convert.ToInt32(_rdbRelatorios.SelectedValue) == (int)ReportNameDocumentos.HistoricoEscolarPedagogico)
            {
                // Validação de preenchimento para o HistoricoEscolarSMESP
                if ((string.IsNullOrEmpty(UCCamposBuscaAluno1.NomeAluno) && string.IsNullOrEmpty(UCCamposBuscaAluno1.MatriculaAluno)) && chkBuscaAvancada.Checked)
                    throw new ValidationException("Nome ou " + GestaoEscolarUtilBO.nomePadraoMatriculaEstadual(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " do aluno deve ser preenchido.");
            }

            if (UCCamposBuscaAluno1.IsValid)
                Pesquisar();
            else
                throw new ValidationException("Data de nascimento do aluno não está no formato dd/mm/aaaa ou é inexistente.");
        }
        catch (ValidationException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alunos.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void _ddlPeriodo_DataBound(object sender, EventArgs e)
    {
        _ddlPeriodo.Items.Insert(0, new ListItem("-- Selecione um período --", "-1;-1"));
    }

    protected void _ddlQtPaginado_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!_grvDocumentoAluno.Rows.Count.Equals(0))
        {
            // atribui nova quantidade itens por página para o grid
            _grvDocumentoAluno.PageSize = Convert.ToInt32(_ddlQtPaginado.SelectedValue);
            _grvDocumentoAluno.PageIndex = 0;

            // Seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(_grvDocumentoAluno);

            DataTable dtAlunos = SelecionaDataSource(0);

            if (dtAlunos != new DataTable())
            {
                _grvDocumentoAluno.VirtualItemCount = ACA_AlunoBO.GetTotalRecords();
                _grvDocumentoAluno.PageIndex = 0;
                _grvDocumentoAluno.PageSize = Convert.ToInt32(_ddlQtPaginado.SelectedValue);
                _grvDocumentoAluno.DataSource = dtAlunos;
                _grvDocumentoAluno.DataBind();
            }
        }
    }

    protected void _grvDocumentoAluno_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView grid = ((GridView)(sender));

        DataTable dtAlunos = SelecionaDataSource(e.NewPageIndex);

        if (dtAlunos != new DataTable())
        {
            grid.VirtualItemCount = ACA_AlunoBO.GetTotalRecords();
            grid.PageIndex = e.NewPageIndex;
            grid.PageSize = Convert.ToInt16(_ddlQtPaginado.SelectedValue);
            grid.DataSource = dtAlunos;
            grid.DataBind();
        }
    }

    protected void _grvDocumentoAluno_DataBound(object sender, EventArgs e)
    {
        string sDeclaracaoHMTL = string.Empty;
        if (Convert.ToInt32(_rdbRelatorios.SelectedValue) == (int)ReportNameDocumentos.BoletimEscolar)
        {
            sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                                 (int)ReportNameDocumentos.BoletimEscolar);
        }

        if ((Convert.ToInt32(_rdbRelatorios.SelectedValue) != (int)ReportNameDocumentos.BoletimEscolar)
            ||
            (string.IsNullOrEmpty(sDeclaracaoHMTL) || (!Convert.ToBoolean(sDeclaracaoHMTL)))
            )
        {
            _chkTodos.Checked = false;
        }

        // Seta os datasources para as colunas do HistoricoEscolarSMESP
        if (Convert.ToInt32(_rdbRelatorios.SelectedValue) == (int)ReportNameDocumentos.HistoricoEscolarPedagogico)
        {
            BoundField numeroChamada = _grvDocumentoAluno.Columns[columnNumeroChamada] as BoundField;
            numeroChamada.DataField = "numChamada";
            BoundField dataNascimento = _grvDocumentoAluno.Columns[columnDataNascimento] as BoundField;
            dataNascimento.DataField = "dataNascimento";
        }

        // antes do if acima, sempre movia false para o _chkTodos.Checked conforme o codigo comentado abaixo. 
        //_chkTodos.Checked = false;

        UCTotalRegistros1.Total = ACA_AlunoBO.GetTotalRecords();

        // Seta propriedades necessárias para ordenação nas colunas.
        ConfiguraColunasOrdenacao(_grvDocumentoAluno, VS_Ordenacao, VS_SortDirection);
    }

    protected void _grvDocumentoAluno_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            _chkTodos.Attributes.Remove("todososcursospeja");
            _chkTodos.Attributes.Add("todososcursospeja", (ACA_AlunoBO.numeroCursosPeja == ACA_AlunoBO.GetTotalRecords() ? "1" : "0"));
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox _chkSelecionar = (CheckBox)e.Row.FindControl("_chkSelecionar");

            if (_chkSelecionar != null)
            {
                _chkSelecionar.Attributes.Add("index", e.Row.RowIndex.ToString());

                bool selecionaTodos = false;
                if ((Convert.ToInt32(_rdbRelatorios.SelectedValue) == (int)ReportNameDocumentos.BoletimEscolar) &&   // foi adicionado essa condição para trazer todos selecionados 
                      ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_BOLETINS_ALUNOS_NA_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    selecionaTodos = true;
                }

                //if (_VS_AlunosSelecionados.ContainsKey(Convert.ToInt64(_chkSelecionar.Attributes["alu_id"]))) 

                if ((_VS_AlunosSelecionados.ContainsKey(Convert.ToInt64(_chkSelecionar.Attributes["alu_id"]))) || selecionaTodos)
                {
                    _chkSelecionar.Checked = true;
                    e.Row.Style.Add("background", "#F8F7CB");
                }
                else
                {
                    _chkSelecionar.Checked = false;
                    e.Row.Style.Remove("background");
                }
            }
        }
    }

    protected void _grvDocumentoAluno_Sorting(object sender, GridViewSortEventArgs e)
    {
        GridView grid = ((GridView)(sender));

        if (!string.IsNullOrEmpty(e.SortExpression))
        {
            Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

            SortDirection sortDirection = VS_SortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;

            if (filtros.ContainsKey("VS_Ordenacao"))
            {
                filtros["VS_Ordenacao"] = e.SortExpression;
            }
            else
            {
                filtros.Add("VS_Ordenacao", e.SortExpression);
            }

            if (filtros.ContainsKey("VS_SortDirection"))
            {
                filtros["VS_SortDirection"] = sortDirection.ToString();
            }
            else
            {
                filtros.Add("VS_SortDirection", sortDirection.ToString());
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.DocumentosAluno
                ,
                Filtros = filtros
            };
        }

        DataTable dtAlunos = SelecionaDataSource(grid.PageIndex);

        if (dtAlunos != new DataTable())
        {
            grid.DataSource = dtAlunos;
            grid.DataBind();
        }
    }

    protected void _rdbRelatorios_SelectedIndexChanged(object sender, EventArgs e)
    {
        LimparBuscaRealizada();

        SelecionaRelatorio();
    }

    protected void chkDoctoAntigo_CheckedChanged(object sender, EventArgs e)
    {
        HabilitarFiltrosPadrao(false);

        //fdsResultados.Style.Add("display", "none");
        fdsResultados.Visible = false;
    }

    protected void odsDocumentos_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            ApplicationWEB._GravaErro(e.Exception);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar listar o(s) documento(s) aluno(s).", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void odsPeriodoDeAvaliacao_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (!IsPostBack)
        {
            // Cancela o select se for a primeira entrada na tela.
            e.Cancel = true;
        }
    }

    protected void grvPeriodoAvaliacao_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lbkPeriodoAvaliacao = (LinkButton)e.Row.FindControl("lbkPeriodoAvaliacao");
            if (lbkPeriodoAvaliacao != null)
            {
                lbkPeriodoAvaliacao.CommandArgument = e.Row.RowIndex.ToString();
            }
        }
    }

    protected void grvPeriodoAvaliacao_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "lbkPeriodoAvaliacao_Select")
        {
            int index = Convert.ToInt32(e.CommandArgument.ToString());
            string parametros = _VS_SalvaParametros;

            parametros += "&PeriodoAvaliacao=" + int.Parse(grvPeriodoDeAvaliacao.DataKeys[index].Values["tpc_id"].ToString());
            CFG_RelatorioBO.SendParametersToReport(_VS_SalvaReport, parametros);

            Response.Redirect("~/Documentos/BoletimEscolar/BoletimEscolarDosAlunos.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

    }

    protected void chkBuscaAvancada_CheckedChanged(object sender, EventArgs e)
    {
        if (_rdbRelatorios.SelectedValue == ((int)ReportNameDocumentos.HistoricoEscolarPedagogico).ToString())
        {
            divBuscaAvancadaAluno.Visible = chkBuscaAvancada.Checked;
            UCComboCursoCurriculo1.Obrigatorio =
            UCComboCurriculoPeriodo1.Obrigatorio =
            UCComboCalendario1.Obrigatorio =
            UCComboTurma1.Obrigatorio = !chkBuscaAvancada.Checked;
            uppPesquisa.Update();
        }
    }

    protected void ChkEmitirAnterior_CheckedChanged(object sender, EventArgs e)
    {
        AnosAnteriores();
    }

    protected void btnVoltarPesquisa_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "AlertaMat", "$(document).ready(function(){ $('#divPeriodoDeAvaliacao').dialog('close'); });", true);
    }

    protected void btnVoltarPesquisaDec_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "AlertaMat", "$(document).ready(function(){ $('#divDeclaracaoComparecimento').dialog('close'); });", true);
    }

    #region Filtros

    protected void _ddlValidade_SelectedIndexChanged(object sender, EventArgs e)
    {
        CarregaDadosValidade();
    }

    protected void btnBuscaAluno_Click(object sender, EventArgs e)
    {
        try
        {
            UCAluno1.Limpar();
            UCAluno1.BuscaSomenteAtivos = true;

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "AbrirBuscaAluno", "$('#divBuscaAluno').dialog('open');", true);
        }
        catch (ArgumentException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alunos.", UtilBO.TipoMensagem.Erro);
        }
        finally
        {
            uppPesquisa.Update();
        }
    }

    protected void btnGerarRelatorioConviteReuniao_Click(object sender, EventArgs e)
    {
        try
        {
            if (_ValidarConviteReuniao())
            {
                string report = ((int)ReportNameDocumentos.ConviteReuniao).ToString();
                string parametros =
                    "Data=" + txtDataReuniao.Text +
                    "&horainicial=" + ddlHora.SelectedItem.Text +
                    "&minutosinicial=" + ddlMinutos.SelectedItem.Text +
                    "&horafinal=" + ddlHoraFinal.SelectedItem.Text +
                    "&minutosfinal=" + ddlMinutosFinal.SelectedItem.Text +
                    "&assunto=" + txtAssunto.Text +
                    "&uni_id=" + UCComboUAEscola.Uni_ID +
                    "&esc_id=" + UCComboUAEscola.Esc_ID +
                    "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE);

                //GerarRel(ReportNameDocumentos.ConviteReuniao);

                CFG_RelatorioBO.CallReport("Documentos", report, parametros, HttpContext.Current);
            }
        }
        catch (Exception err)
        {
            ApplicationWEB._GravaErro(err);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o documento do aluno.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void btnGerarRelatorioDeclaracaoSolicitacaoComparecimento_Click(object sender, EventArgs e)
    {
        try
        {
            if (_ValidarComparecimento())
            {
                string resp = txtNomeResp.Text;
                string report = ((int)ReportNameDocumentos.DeclaracaoSolicitacaoComparecimento).ToString();
                string parametros = "alu_id=" + VS_alu_id +
                                    "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                    "&uad_idSuperior=" + VS_uad_idSuperior +
                                    "&resp_aluno=" + resp +
                                    "&data=" + txtData.Text +
                                    "&hora=" + ddlHoraComparecimento.SelectedItem.Text +
                                    "&minuto=" + ddlMinutoComparecimento.SelectedItem.Text +
                                    "&telefone=" +
                                    SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE);

                CFG_RelatorioBO.CallReport("Documentos", report, parametros, HttpContext.Current);
            }
        }
        catch (Exception err)
        {
            ApplicationWEB._GravaErro(err);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o documento do aluno.", UtilBO.TipoMensagem.Erro);
        }
        finally
        {
            uppPesquisa.Update();
        }
    }

    protected void btnGerarRelatorioDeclaracaoSolicitacaoVaga_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                ESC_Escola entityEscola = new ESC_Escola { esc_id = UCComboUAEscola.Esc_ID };
                ESC_EscolaBO.GetEntity(entityEscola);
                string uad_id = entityEscola.uad_id.ToString();

                string aluNome = _txtNomeAluno.Text;

                string dataNasc = _txtDataNasc.Text;

                string curso = UCComboCursoCurriculo1.Texto;

                string curriculoPeriodo = UCComboCurriculoPeriodo1.Texto;

                string validade = RetornaValidade(Convert.ToInt32(_ddlValidade.SelectedValue));

                string escola_origem = !string.IsNullOrEmpty(_txtEscolaOrigem.Text) ? _txtEscolaOrigem.Text : "-";

                string report = ((int)ReportNameDocumentos.DeclaracaoSolicitacaoVaga).ToString();
                string parametros = "esc_id=" + UCComboUAEscola.Esc_ID +
                                     "&uni_id=" + UCComboUAEscola.Uni_ID +
                                     "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE) +
                                     "&email=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_EMAIL) +
                                     "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                                     "&uad_id=" + uad_id +
                                     "&aluno_nome=" + aluNome +
                                     "&data_nasc=" + dataNasc +
                                     "&curso=" + curso +
                                     "&curriculo_periodo=" + curriculoPeriodo +
                                     "&validade=" + validade +
                                     "&escola_origem=" + escola_origem;

                CFG_RelatorioBO.CallReport("Documentos", report, parametros, HttpContext.Current);
            }
        }
        catch (Exception err)
        {
            ApplicationWEB._GravaErro(err);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o documento de solicitação de vaga do aluno.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void btnGerarDeclaracaoTrabalho_Click(object sender, EventArgs e)
    {
        try
        {
            string report = ((int)ReportNameDocumentos.DeclaracaoTrabalho).ToString();
            string parametros = "Nome_Relatorio=" + _rdbRelatorios.SelectedItem.Text;
            Response.Redirect("~/Documentos/Declaracoes/DeclaracoesAluno.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            CFG_RelatorioBO.SendParametersToReport(report, parametros);
        }
        catch (Exception err)
        {
            ApplicationWEB._GravaErro(err);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a declaração de trabalho.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void btnGerarCertidaoEscolariedadeAnterior1972_Click(object sender, EventArgs e)
    {
        try
        {
            string report = ((int)ReportNameDocumentos.CertidaoEscolaridadeAnterior1972).ToString();
            string parametros = "uad_idSuperior=" + UCComboUAEscola.Uad_ID +
                                "&esc_id=" + UCComboUAEscola.Esc_ID +
                                "&uni_id=" + UCComboUAEscola.Uni_ID +
                                "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE) +
                                "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id;

            CFG_RelatorioBO.CallReport("Documentos", report, parametros, HttpContext.Current);
        }
        catch (Exception err)
        {
            ApplicationWEB._GravaErro(err);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a declaração de trabalho.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void UCAluno1_ReturnValues(IDictionary<string, object> parameters)
    {
        try
        {
            txtNomeAluno.Text = parameters["pes_nome"].ToString();
            txtNomeMae.Text = parameters["pes_nomeMae"].ToString();
            VS_alu_id = Convert.ToInt64(parameters["alu_id"]);
            VS_uad_idSuperior = (Guid)(parameters["uad_id"]);

            List<ACA_AlunoResponsavelBO.StructCadastro> ltResponsaveis = ACA_AlunoResponsavelBO.RetornaResponsaveisAluno(VS_alu_id, null);

            List<string> pai = (from ACA_AlunoResponsavelBO.StructCadastro resp in ltResponsaveis
                                where resp.entAlunoResp.tra_id == TipoResponsavelAlunoParametro.tra_idPai(__SessionWEB.__UsuarioWEB.Usuario.ent_id)
                                select resp.entPessoa.pes_nome).ToList();

            List<string> responsavel = (from ACA_AlunoResponsavelBO.StructCadastro resp in ltResponsaveis
                                        where resp.entAlunoResp.alr_principal
                                        select resp.entPessoa.pes_nome).ToList();

            txtNomePai.Text = pai.Count > 0 ? pai[0] : String.Empty;
            txtNomeResp.Text = responsavel.Count > 0 ? responsavel[0] : String.Empty;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados do aluno.", UtilBO.TipoMensagem.Erro);
        }
        finally
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharBuscaAluno", "$('#divBuscaAluno').dialog('close');", true);
            uppPesquisa.Update();
        }
    }

    #endregion Filtros

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
            UCComboCursoCurriculo1.Valor = new[] { -1, -1 };
            UCComboCursoCurriculo1.PermiteEditar = false;

            if (UCComboUAEscola.Esc_ID > 0 && UCComboUAEscola.Uni_ID > 0)
            {
                if (ChkEmitirAnterior.Checked)
                    UCComboCursoCurriculo1.CarregarPorEscola(UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID);
                else
                    UCComboCursoCurriculo1.CarregarVigentesPorEscola(UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID);

                UCComboCursoCurriculo1.SetarFoco();
                UCComboCursoCurriculo1.PermiteEditar = true;
            }

            UCComboCursoCurriculo1_IndexChanged();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
        }
    }

    private void UCComboCalendario1_IndexChanged()
    {
        try
        {
            //if (!_VS_PesquisaSalva)
            //{
            UCComboTurma1.Valor = new long[] { -1, -1, -1 };
            UCComboTurma1.PermiteEditar = false;

            if (UCComboCalendario1.Valor > 0)
            {
                if (Convert.ToInt32(_rdbRelatorios.SelectedValue).Equals((int)ReportNameDocumentos.BoletimEscolar))
                    UCComboTurma1.CarregarPorEscolaCurriculoCalendario_TurmasNormais(UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID, UCComboCurriculoPeriodo1.Valor[0], UCComboCurriculoPeriodo1.Valor[1], UCComboCurriculoPeriodo1.Valor[2], UCComboCalendario1.Valor);
                else if (Convert.ToInt32(_rdbRelatorios.SelectedValue).Equals((int)ReportNameDocumentos.HistoricoEscolarPedagogico))
                    UCComboTurma1.CarregarPorEscolaCalendarioEPeriodo(UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID, UCComboCalendario1.Valor, UCComboCurriculoPeriodo1.Valor[0], UCComboCurriculoPeriodo1.Valor[1], UCComboCurriculoPeriodo1.Valor[2], 1);
                else
                    UCComboTurma1.CarregarPorEscolaCurriculoCalendario(UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID, UCComboCurriculoPeriodo1.Valor[0], UCComboCurriculoPeriodo1.Valor[1], UCComboCurriculoPeriodo1.Valor[2], UCComboCalendario1.Valor);
                UCComboTurma1.PermiteEditar = true;
                UCComboTurma1.Focus();
            }
            //}
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a(s) turma(s) do período.", UtilBO.TipoMensagem.Erro);
        }
    }

    private void UCComboCurriculoPeriodo1__OnSelectedIndexChange()
    {
        try
        {
            //if (!_VS_PesquisaSalva)
            //{
            UCComboCalendario1.Valor = -1;

            if (TipoRelatorioSelecionado != ReportNameDocumentos.FichaIndividualAvaliacaoPeriodica)
            {
                UCComboCalendario1.PermiteEditar = false;
            }

            if (UCComboCurriculoPeriodo1.Valor[0] > 0)
            {
                UCComboCalendario1.CarregarPorCurso(UCComboCursoCurriculo1.Valor[0]);
                UCComboCalendario1.PermiteEditar = true;
                UCComboCalendario1.SetarFoco();
            }

            UCComboCalendario1_IndexChanged();
            //}
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o(s) calendários(s).", UtilBO.TipoMensagem.Erro);
        }
    }

    private void UCComboCursoCurriculo1_IndexChanged()
    {
        try
        {
            //if (!_VS_PesquisaSalva)
            //{
            UCComboCurriculoPeriodo1.Valor = new[] { -1, -1, -1 };
            UCComboCurriculoPeriodo1.PermiteEditar = false;

            if (UCComboCursoCurriculo1.Valor[0] > 0)
            {
                UCComboCurriculoPeriodo1.CarregarPorCursoCurriculo(UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1]);
                UCComboCurriculoPeriodo1.PermiteEditar = true;
                UCComboCurriculoPeriodo1.Focus();
            }

            UCComboCurriculoPeriodo1__OnSelectedIndexChange();
            //}
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion Delegates

    #region Métodos

    #region Filtros

    #region FiltrosDeclaracaoSolicitacaoVaga

    /// <summary>
    ///
    /// </summary>
    protected void FiltrosDeclaracaoSolicitacaoVaga()
    {
        HabilitarFiltrosPadrao(false);
        HabilitarFiltrosDiversos(false);
        UCComboUAEscola.Visible = true;
        UCComboCursoCurriculo1.Visible = true;
        UCComboCurriculoPeriodo1.Visible = true;
        divDeclaracaoSolicitacaoVaga.Visible = true;
        btnGerarRelatorioDeclaracaoSolicitacaoVaga.Visible = true;

        // Validação de campos obrigatórios
        UCComboUAEscola.ObrigatorioEscola = true;
        UCComboUAEscola.ObrigatorioUA = true;
        UCComboCursoCurriculo1.Obrigatorio = true;
        UCComboCurriculoPeriodo1.Obrigatorio = true;
    }

    /// <summary>
    ///
    /// </summary>
    private void CarregaDadosValidade()
    {
        string paramValue1 = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.SOLICITACAO_VAGA_VALIDADE_TIPO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        string paramValue2 = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.SOLICITACAO_VAGA_VALIDADE_VALOR, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

        if ((!string.IsNullOrEmpty(paramValue1)) && (_ddlValidade.SelectedValue.Equals("-1")))
        {
            _ddlValidade.SelectedValue = paramValue1;
        }
        if (!_ddlValidade.SelectedValue.Equals("-1"))
        {
            _pnlValidade.Visible = true;

            if (_ddlValidade.SelectedValue.Equals("1"))
            {
                _lblValor.Text = "Hora(s) *";
                _lblValor.AssociatedControlID = _txtHoras.ID;
                _txtHoras.Visible = true;
                _revHoras.Visible = true;
                if (!string.IsNullOrEmpty(paramValue1))
                {
                    if (string.IsNullOrEmpty(_txtHoras.Text))
                    {
                        if ((_ddlValidade.SelectedValue == paramValue1) && (!string.IsNullOrEmpty(paramValue2)))
                        {
                            if (ValidaValorSolicitacao(paramValue2, Convert.ToInt32(paramValue1)))
                            {
                                _txtHoras.Text = paramValue2;
                            }
                            else
                            {
                                _txtHoras.Text = string.Empty;
                            }
                        }
                        else
                        {
                            _txtHoras.Text = string.Empty;
                        }
                    }
                }

                _txtData.Visible = false;
                _cvValorData.Visible = false;

                _txtDias.Visible = false;

                _rfvValorValidade.Visible = true;
                _rfvValorValidade.ControlToValidate = _txtHoras.ID;
                _rfvValorValidade.ErrorMessage = "Hora(s) é obrigatório.";
            }

            if (_ddlValidade.SelectedValue.Equals("2"))
            {
                _lblValor.Text = "Dia(s) *";
                _lblValor.AssociatedControlID = _txtDias.ID;
                _txtDias.Visible = true;
                if (!string.IsNullOrEmpty(paramValue1))
                {
                    if (string.IsNullOrEmpty(_txtDias.Text))
                    {
                        if ((_ddlValidade.SelectedValue == paramValue1) && (!string.IsNullOrEmpty(paramValue2)))
                        {
                            if (ValidaValorSolicitacao(paramValue2, Convert.ToInt32(paramValue1)))
                            {
                                _txtDias.Text = paramValue2;
                            }
                            else
                            {
                                _txtDias.Text = string.Empty;
                            }
                        }
                        else
                        {
                            _txtDias.Text = string.Empty;
                        }
                    }
                }
                _txtData.Visible = false;
                _cvValorData.Visible = false;

                _txtHoras.Visible = false;
                _revHoras.Visible = false;

                _rfvValorValidade.ControlToValidate = _txtDias.ID;
                _rfvValorValidade.ErrorMessage = "Dia(s) é obrigatório.";
                _rfvValorValidade.Visible = true;
            }

            if (_ddlValidade.SelectedValue.Equals("3"))
            {
                _lblValor.Text = "Data *";
                _lblValor.AssociatedControlID = _txtData.ID;
                _txtData.Visible = true;
                if (!string.IsNullOrEmpty(paramValue1))
                {
                    if ((_ddlValidade.SelectedValue == paramValue1) && (!string.IsNullOrEmpty(paramValue2)))
                    {
                        if (ValidaValorSolicitacao(paramValue2, Convert.ToInt32(paramValue1)))
                        {
                            _txtData.Text = paramValue2;
                        }
                        else
                        {
                            _txtData.Text = string.Empty;
                        }
                    }
                    else
                    {
                        _txtData.Text = string.Empty;
                    }
                }

                _txtDias.Visible = false;
                _txtHoras.Visible = false;
                _revHoras.Visible = false;
                _rfvValorValidade.ControlToValidate = _txtData.ID;
                _rfvValorValidade.ErrorMessage = "Data é obrigatória.";
                _cvValorData.ValidationGroup = "solicitacaovaga";
                _cvValorData.Visible = true;
            }
        }
        else
        {
            _pnlValidade.Visible = false;
        }

        //Trava alteração de dropdown de validação através do parametro SOLICITACAO_VAGA_TRAVAR_VALIDADE
        if (!string.IsNullOrEmpty(ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.SOLICITACAO_VAGA_TRAVAR_VALIDADE, __SessionWEB.__UsuarioWEB.Usuario.ent_id)))
        {
            bool paramValueTrava = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.SOLICITACAO_VAGA_TRAVAR_VALIDADE, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            _ddlValidade.Enabled = !paramValueTrava;
            _txtData.Enabled = !paramValueTrava;
            _txtDias.Enabled = !paramValueTrava;
            _txtHoras.Enabled = !paramValueTrava;
        }
    }

    /// <summary>
    /// Função que retorna a validade da solicitaçao de vaga formatada de acordo com o tipo informado
    /// </summary>
    /// <param name="tipo"> 1 - Horas / 2 - Dias / 3 - Data</param>
    /// <returns>retorna uma string contendo a validade formatada</returns>
    private string RetornaValidade(int tipo)
    {
        string validade = string.Empty;

        TextBox _txtValor = new TextBox();

        foreach (Control item in _pnlValidade.Controls)
        {
            if ((item.GetType() == typeof(TextBox)) && (item.Visible))
                _txtValor = item as TextBox;
        }

        switch (tipo)
        {
            case 1:
                validade = String.Concat("por", " ", _txtValor.Text, " ", "horas");
                break;

            case 2:
                validade = String.Concat("por", " ", _txtValor.Text, " ", "dias");
                break;

            case 3:
                validade = String.Concat("até", " ", _txtValor.Text);
                break;

            default:
                break;
        }

        return validade;
    }

    /// <summary>
    /// Funçao que verifica se o valor recebido como valor de validade da solicitação de vaga está de
    /// acordo com o tipo informado
    /// </summary>
    /// <param name="validade">valor que será validado</param>
    ///<param name="tipo">1 - Horas / 2 - Dias / 3 - Data</param>
    ///<returns> retorna um booleano que indica a validaçao</returns>
    private bool ValidaValorSolicitacao(string validade, int tipo)
    {
        bool flag = false;

        if (tipo == 1 || tipo == 2)
        {
            Regex regex = new Regex(@"^[0-9]+$");
            if (regex.IsMatch(validade))
            {
                flag = true;
            }
        }
        else
        {
            Regex regex = new Regex(@"^([0-9]|[0,1,2][0-9]|3[0,1])/([\d]|1[0,1,2])/\d{4}$");
            if (regex.IsMatch(validade))
            {
                flag = true;
            }
        }
        return flag;
    }

    #endregion FiltrosDeclaracaoSolicitacaoVaga

    #region FiltrosDeclaracaoSolicitacaoComparecimento

    protected void FiltrosDeclaracaoSolicitacaoComparecimento()
    {
        HabilitarFiltrosPadrao(false);
        HabilitarFiltrosDiversos(false);
        divDeclaracaoSolicitacaoComparecimento.Visible = true;
        btnGerarRelatorioDeclaracaoSolicitacaoComparecimento.Visible = true;

        _carregaComboHora(ddlHoraComparecimento);
        _carregaComboMinuto(ddlMinutoComparecimento);
    }

    private bool _ValidarComparecimento()
    {
        if (String.IsNullOrEmpty(txtNomeAluno.Text))
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Aluno é obrigatório.", UtilBO.TipoMensagem.Erro);
            return false;
        }

        if (String.IsNullOrEmpty(txtNomeResp.Text))
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Nome do responsável é obrigatório.", UtilBO.TipoMensagem.Erro);
            return false;
        }

        if (ddlHoraComparecimento.SelectedIndex == 0)
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Hora é obrigatório.", UtilBO.TipoMensagem.Erro);
            return false;
        }

        if (ddlMinutoComparecimento.SelectedIndex == 0)
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Minuto é obrigatório.", UtilBO.TipoMensagem.Erro);
            return false;
        }

        if (Convert.ToDateTime(txtData.Text + " " + ddlHoraComparecimento.SelectedValue + ":" + ddlMinutoComparecimento.SelectedValue + ":00") <
            DateTime.Now)
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Data e hora tem que ser maior ou igual a atual.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        return true;
    }

    #endregion FiltrosDeclaracaoSolicitacaoComparecimento

    #region FiltrosConviteReuniao

    protected void FiltrosConviteReuniao()
    {
        HabilitarFiltrosPadrao(false);
        HabilitarFiltrosDiversos(false);
        UCComboUAEscola.Visible = true;
        divConviteReuniao.Visible = true;
        btnGerarRelatorioConviteReuniao.Visible = true;

        // Validação de campos obrigatórios
        UCComboUAEscola.ObrigatorioUA = true;
        UCComboUAEscola.ObrigatorioEscola = true;

        _carregaComboHora(ddlHora);
        _carregaComboMinuto(ddlMinutos);
        _carregaComboHora(ddlHoraFinal);
        _carregaComboMinuto(ddlMinutosFinal);
    }

    /// <summary>
    /// Verifca se os campos da tela estão preenchidos
    /// </summary>
    /// <returns></returns>
    private bool _ValidarConviteReuniao()
    {
        if (ddlHora.SelectedIndex == 0)
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Hora de início é obrigatório.", UtilBO.TipoMensagem.Erro);
            return false;
        }

        if (ddlMinutos.SelectedIndex == 0)
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Minutos de início é obrigatório.", UtilBO.TipoMensagem.Erro);
            return false;
        }

        if (ddlHoraFinal.SelectedIndex == 0)
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Hora de fim é obrigatório.", UtilBO.TipoMensagem.Erro);
            return false;
        }

        if (ddlMinutosFinal.SelectedIndex == 0)
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Minutos de fim é obrigatório.", UtilBO.TipoMensagem.Erro);
            return false;
        }

        if (Convert.ToDateTime(txtDataReuniao.Text + " " + ddlHora.SelectedValue + ":" + ddlMinutos.SelectedValue + ":00") <
            DateTime.Now)
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Data e hora tem que ser maior ou igual a atual.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        if (Convert.ToDateTime(txtDataReuniao.Text + " " + ddlHora.SelectedValue + ":" + ddlMinutos.SelectedValue + ":00") >
           Convert.ToDateTime(txtDataReuniao.Text + " " + ddlHoraFinal.SelectedValue + ":" + ddlMinutosFinal.SelectedValue + ":00"))
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Horário de término tem que ser maior que o horário de início.",
                                                     UtilBO.TipoMensagem.Alerta);
            return false;
        }

        return true;
    }

    #endregion FiltrosConviteReuniao

    #region FiltrosCertidaoEscolaridadeAnterior1972

    /// <summary>
    /// Configura a tela de campos de filtro para a geração do relatório.
    /// </summary>
    protected void FiltrosCertidaoEscolaridadeAnterior1972()
    {
        HabilitarFiltrosPadrao(false);
        HabilitarFiltrosDiversos(false);
        UCComboUAEscola.Visible = true;
        btnGerarCertidaoEscolariedadeAnterior1972.Visible = true;

        UCComboUAEscola.ObrigatorioUA = true;
        UCComboUAEscola.ObrigatorioEscola = true;
    }

    #endregion

    protected DataTable SelecionaDataSource(int Pagina, bool SelecionaTodos = false)
    {
        if (_rdbRelatorios.SelectedValue != "")
        {
            int selectedValue = Convert.ToInt32(_rdbRelatorios.SelectedValue);

            int qtdeLinhasPorPagina = SelecionaTodos ? 0 : Convert.ToInt32(_ddlQtPaginado.SelectedValue);

            bool documentoOficial = selectedValue.Equals((int)ReportNameDocumentos.HistoricoEscolar);

            if ((selectedValue.Equals((int)ReportNameDocumentos.BoletimEscolar) ||
                 selectedValue.Equals((int)ReportNameDocumentos.FichaIndividualAluno) ||
                 selectedValue.Equals((int)ReportNameDocumentos.FichaIndividualAvaliacaoPeriodica)) && ChkEmitirAnterior.Checked)
            {
                return ACA_AlunoBO.BuscaAlunos_BoletimEscolar
                (
                    UCComboCalendario1.Valor,
                    UCComboUAEscola.Esc_ID,
                    UCComboUAEscola.Uni_ID,
                    UCComboCursoCurriculo1.Valor[0],
                    UCComboCursoCurriculo1.Valor[1],
                    UCComboCurriculoPeriodo1.Valor[2],
                    UCComboTurma1.Valor[0],
                    Convert.ToByte(UCCamposBuscaAluno1.TipoBuscaNomeAluno),
                    UCCamposBuscaAluno1.NomeAluno,
                    Convert.ToDateTime(string.IsNullOrEmpty(UCCamposBuscaAluno1.DataNascAluno) ? new DateTime().ToString() : UCCamposBuscaAluno1.DataNascAluno),
                    UCCamposBuscaAluno1.NomeMaeAluno,
                    UCCamposBuscaAluno1.MatriculaAluno,
                    UCCamposBuscaAluno1.MatriculaEstadualAluno,
                    __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                    UCComboUAEscola.Uad_ID,
                    (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
                    __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                    __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                    ChkEmitirAnterior.Checked,
                    qtdeLinhasPorPagina,
                    Pagina,
                    (int)VS_SortDirection,
                    VS_Ordenacao
                );
            }
            else if (selectedValue.Equals((int)ReportNameDocumentos.HistoricoEscolar) && ChkEmitirAnterior.Checked &&
                     __SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Administracao)
            {
                return ACA_AlunoBO.BuscaAlunos_HistoricoEscolar
                    (
                        UCComboCalendario1.Valor,
                        UCComboUAEscola.Esc_ID,
                        UCComboUAEscola.Uni_ID,
                        UCComboCursoCurriculo1.Valor[0],
                        UCComboCursoCurriculo1.Valor[1],
                        UCComboCurriculoPeriodo1.Valor[2],
                        UCComboTurma1.Valor[0],
                        Convert.ToByte(UCCamposBuscaAluno1.TipoBuscaNomeAluno),
                        UCCamposBuscaAluno1.NomeAluno,
                        Convert.ToDateTime(string.IsNullOrEmpty(UCCamposBuscaAluno1.DataNascAluno) ? new DateTime().ToString() : UCCamposBuscaAluno1.DataNascAluno),
                        UCCamposBuscaAluno1.NomeMaeAluno,
                        UCCamposBuscaAluno1.MatriculaAluno,
                        UCCamposBuscaAluno1.MatriculaEstadualAluno,
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                        UCComboUAEscola.Uad_ID,
                        (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
                        __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                        __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                        ChkEmitirAnterior.Checked,
                        qtdeLinhasPorPagina,
                        Pagina,
                        (int)VS_SortDirection,
                        VS_Ordenacao
                    );
            }
            else if (selectedValue.Equals((int)ReportNameDocumentos.CertificadoConclusaoCurso))
            {
                return ACA_AlunoBO.BuscaAlunos_CertificadoConclusaoCurso
                    (
                        UCComboCalendario1.Valor,
                        UCComboUAEscola.Esc_ID,
                        UCComboUAEscola.Uni_ID,
                        UCComboCursoCurriculo1.Valor[0],
                        UCComboCursoCurriculo1.Valor[1],
                        UCComboCurriculoPeriodo1.Valor[2],
                        UCComboTurma1.Valor[0],
                        Convert.ToByte(UCCamposBuscaAluno1.TipoBuscaNomeAluno),
                        UCCamposBuscaAluno1.NomeAluno,
                        Convert.ToDateTime(string.IsNullOrEmpty(UCCamposBuscaAluno1.DataNascAluno) ? new DateTime().ToString() : UCCamposBuscaAluno1.DataNascAluno),
                        UCCamposBuscaAluno1.NomeMaeAluno,
                        UCCamposBuscaAluno1.MatriculaAluno,
                        UCCamposBuscaAluno1.MatriculaEstadualAluno,
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                        UCComboUAEscola.Uad_ID,
                        (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
                        __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                        __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                        true,
                        qtdeLinhasPorPagina,
                        Pagina,
                        (int)VS_SortDirection,
                        VS_Ordenacao
                    );
            }
            //Usa a mesma busca do boletim escolar para selecionar os alunos para a ficha individual
            //else if (selectedValue.Equals((int)ReportNameDocumentos.FichaIndividualAluno))
            //{
            //    return ACA_AlunoBO.BuscaAlunos_FichaIndividual
            //        (
            //            UCComboCalendario1.Valor,
            //            UCComboUAEscola.Esc_ID,
            //            UCComboUAEscola.Uni_ID,
            //            UCComboCursoCurriculo1.Valor[0],
            //            UCComboCursoCurriculo1.Valor[1],
            //            UCComboCurriculoPeriodo1.Valor[2],
            //            UCComboTurma1.Valor[0],
            //            Convert.ToByte(UCCamposBuscaAluno1.TipoBuscaNomeAluno),
            //            UCCamposBuscaAluno1.NomeAluno,
            //            Convert.ToDateTime(string.IsNullOrEmpty(UCCamposBuscaAluno1.DataNascAluno) ? new DateTime().ToString() : UCCamposBuscaAluno1.DataNascAluno),
            //            UCCamposBuscaAluno1.NomeMaeAluno,
            //            UCCamposBuscaAluno1.MatriculaAluno,
            //            UCCamposBuscaAluno1.MatriculaEstadualAluno,
            //            __SessionWEB.__UsuarioWEB.Usuario.ent_id,
            //            UCComboUAEscola.Uad_ID,
            //            (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
            //            __SessionWEB.__UsuarioWEB.Usuario.usu_id,
            //            __SessionWEB.__UsuarioWEB.Grupo.gru_id,
            //            Convert.ToInt32(_rdbRelatorios.SelectedValue) ==
            //                                       (int)ReportNameDocumentos.CertificadoConclusaoCurso ||
            //                                       Convert.ToInt32(_rdbRelatorios.SelectedValue) ==
            //                                       (int)ReportNameDocumentos.HistoricoEscolarPedagogico
            //                                           ? true
            //                                           : ChkEmitirAnterior.Checked,
            //            qtdeLinhasPorPagina,
            //            Pagina,
            //            (int)VS_SortDirection,
            //            VS_Ordenacao
            //        );
            //}
            else if (selectedValue.Equals((int)ReportNameDocumentos.HistoricoEscolarPedagogico))
            {
                return ACA_AlunoBO.BuscaAlunos_HistoricoEscolarPedagogico
                    (
                        UCComboCalendario1.Valor,
                        UCComboUAEscola.Esc_ID,
                        UCComboUAEscola.Uni_ID,
                        UCComboCursoCurriculo1.Valor[0],
                        UCComboCursoCurriculo1.Valor[1],
                        UCComboCurriculoPeriodo1.Valor[2],
                        UCComboTurma1.Valor[0],
                        Convert.ToByte(UCCamposBuscaAluno1.TipoBuscaNomeAluno),
                        UCCamposBuscaAluno1.NomeAluno,
                        UCCamposBuscaAluno1.MatriculaAluno,
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                        UCComboUAEscola.Uad_ID,
                        (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
                        __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                        __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                        true,
                        qtdeLinhasPorPagina,
                        Pagina,
                        (int)VS_SortDirection,
                        VS_Ordenacao,
                        true
                    );
            }
            else
            {
                return ACA_AlunoBO.BuscaAlunos_Documentos
                    (
                        UCComboCalendario1.Valor,
                        UCComboUAEscola.Esc_ID,
                        UCComboUAEscola.Uni_ID,
                        UCComboCursoCurriculo1.Valor[0],
                        UCComboCursoCurriculo1.Valor[1],
                        UCComboCurriculoPeriodo1.Valor[2],
                        UCComboTurma1.Valor[0],
                        Convert.ToByte(UCCamposBuscaAluno1.TipoBuscaNomeAluno),
                        UCCamposBuscaAluno1.NomeAluno,
                        Convert.ToDateTime(string.IsNullOrEmpty(UCCamposBuscaAluno1.DataNascAluno) ? new DateTime().ToString() : UCCamposBuscaAluno1.DataNascAluno),
                        UCCamposBuscaAluno1.NomeMaeAluno,
                        UCCamposBuscaAluno1.MatriculaAluno,
                        UCCamposBuscaAluno1.MatriculaEstadualAluno,
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                        UCComboUAEscola.Uad_ID,
                        (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
                        __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                        __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                        Convert.ToInt32(_rdbRelatorios.SelectedValue) ==
                                                   (int)ReportNameDocumentos.CertificadoConclusaoCurso ||
                                                   Convert.ToInt32(_rdbRelatorios.SelectedValue) ==
                                                   (int)ReportNameDocumentos.HistoricoEscolarPedagogico
                                                       ? true
                                                       : ChkEmitirAnterior.Checked,
                        qtdeLinhasPorPagina,
                        Pagina,
                        (int)VS_SortDirection,
                        VS_Ordenacao,
                        documentoOficial
                    );
            }
        }

        return new DataTable();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="habilita"></param>
    protected void HabilitarFiltrosDiversos(bool habilita)
    {
        divDeclaracaoSolicitacaoVaga.Visible =
        divDeclaracaoSolicitacaoComparecimento.Visible =
        divConviteReuniao.Visible =
        btnGerarRelatorioDeclaracaoSolicitacaoVaga.Visible =
        btnGerarRelatorioDeclaracaoSolicitacaoComparecimento.Visible =
        btnGerarRelatorioConviteReuniao.Visible = habilita;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="habilita"></param>
    protected void HabilitarFiltrosPadrao(bool habilita)
    {
        UCComboUAEscola.Visible =
        UCComboCursoCurriculo1.Visible =
        UCComboCurriculoPeriodo1.Visible =
        UCComboCalendario1.Visible =
        UCComboTurma1.Visible =
        UCCamposBuscaAluno1.Visible =
        _btnPesquisar.Visible =
        _btnGerarRelatorio.Visible = habilita;

        ChkEmitirAnterior.Visible =
        chkBuscaAvancada.Visible = false;
        HabilitarValidacao(false);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="habilita"></param>
    protected void HabilitarValidacao(bool habilita)
    {
        UCComboUAEscola.ObrigatorioUA =
        UCComboUAEscola.ObrigatorioEscola =
        UCComboCursoCurriculo1.Obrigatorio =
        UCComboCurriculoPeriodo1.Obrigatorio =
        UCComboCalendario1.Obrigatorio =
        UCComboTurma1.Obrigatorio = habilita;
    }

    /// <summary>
    /// Carrega o combo de hora
    /// </summary>
    private void _carregaComboHora(DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.Items.Insert(0, new ListItem("--", "-1", true));
        for (int i = 0; i <= 23; i++)
        {
            string hora = i < 10 ? string.Concat("0", i.ToString()) : i.ToString();
            ddl.Items.Insert(i + 1, new ListItem(hora, i.ToString(), true));
        }
    }

    /// <summary>
    /// Carrega o combo de minutos de 5 em 5 minutos
    /// </summary>
    private void _carregaComboMinuto(DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.Items.Insert(0, new ListItem("--", "-1", true));

        int x = 0;
        int index = 1;
        while (x <= 55)
        {
            string minuto = x < 10 ? string.Concat("0", x.ToString()) : x.ToString();
            ddl.Items.Insert(index, new ListItem(minuto, x.ToString(), true));
            index++;
            x = x + 5;
        }
    }

    private void LimparCampos()
    {
        UCCamposBuscaAluno1.LimpaCampos();

        _txtNomeAluno.Text = "";
        _txtDataNasc.Text = "";
        _txtEscolaOrigem.Text = "";
        txtNomeAluno.Text = "";
        txtNomePai.Text = "";
        txtNomeMae.Text = "";
        txtNomeResp.Text = "";
        txtData.Text = "";
        txtDataReuniao.Text = "";
        txtAssunto.Text = "";
        ChkEmitirAnterior.Checked = false;
        ChkEmitirAnterior.Text = "Emitir documentos de anos anteriores"; // recarrego pq tem opção que troca o texto.
    }


    #endregion Filtros

    [WebMethod]
    public static bool ExtensaoDosFiltros(Guid ent_id, int rlt_id)
    {
        bool ret = false;
        if (rlt_id == (int)ReportNameDocumentos.FichaIndividualAvaliacaoPeriodica)
            Boolean.TryParse(CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.FILTRA_POR_PERIODO, ent_id), out ret);
        return ret;
    }

    /// <summary>
    /// Inicializa os filtros da pagina.
    /// </summary>
    protected void Inicializar()
    {
        UCComboUAEscola.Inicializar();

        if (UCComboUAEscola.VisibleUA)
            UCComboUAEscola_IndexChangedUA();

        //if (UCComboUAEscola.Esc_ID <= 0)
        //{
        //    UCComboCursoCurriculo1.CancelSelect = true;
        //    UCComboCursoCurriculo1.PermiteEditar = false;
        //    UCComboCurriculoPeriodo1.CancelSelect = true;
        //    UCComboCurriculoPeriodo1.PermiteEditar = false;
        //    UCComboCalendario1.CancelSelect = false;
        //    UCComboCalendario1.PermiteEditar = false;
        //    UCComboTurma1.CancelSelect = true;
        //    UCComboTurma1.PermiteEditar = false;
        //}
    }

    /// <summary>
    /// Pesquisa os alunos de acordo com os filtros de busca definidos.
    /// </summary>
    protected void Pesquisar()
    {

        SalvaBusca();

        ACA_AlunoBO.numeroCursosPeja = 0;

        // quantidade de itens por página
        string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
        _ddlQtPaginado.SelectedValue = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao.ToString() : qtItensPagina;

        // ******************************

        DataTable dtAlunos = SelecionaDataSource(0);

        if (dtAlunos != new DataTable())
        {
            _grvDocumentoAluno.VirtualItemCount = ACA_AlunoBO.GetTotalRecords();
            _grvDocumentoAluno.PageIndex = 0;
            _grvDocumentoAluno.PageSize = Convert.ToInt32(_ddlQtPaginado.SelectedValue);
            _grvDocumentoAluno.DataSource = dtAlunos;

            // Seta os datasources para as colunas do HistoricoEscolarSMESP
            if (Convert.ToInt32(_rdbRelatorios.SelectedValue) == (int)ReportNameDocumentos.HistoricoEscolarPedagogico)
            {
                BoundField numeroChamada = _grvDocumentoAluno.Columns[columnNumeroChamada] as BoundField;
                numeroChamada.DataField = "numChamada";
                BoundField dataNascimento = _grvDocumentoAluno.Columns[columnDataNascimento] as BoundField;
                dataNascimento.DataField = "dataNascimento";
            }

            _grvDocumentoAluno.DataBind();
        }

        fdsResultados.Visible = true;

        if (Convert.ToInt32(_rdbRelatorios.SelectedValue) == (int)ReportNameDocumentos.FichaIndividualAvaliacaoPeriodica)
        {
            _ddlPeriodo.Visible = true;
            _ddlPeriodo.Items.Clear();
            _ddlPeriodo.DataBind();
        }

        _chkTodos.Visible = !_grvDocumentoAluno.Rows.Count.Equals(0);

        if ((_chkTodos.Visible == true) &&   // esse teste é utilizado para exibir o flag _chkTodos.Checked já selecionado, desde que o parametros possua valor True
             (Convert.ToInt32(_rdbRelatorios.SelectedValue) == (int)ReportNameDocumentos.BoletimEscolar) &&
             ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_BOLETINS_ALUNOS_NA_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
           )
        {
            //_VS_SelecionarTodos = true;
            _chkTodos.Checked = true;
        }
        divQtdPaginacao.Visible = _grvDocumentoAluno.Rows.Count > 0;

        _updResultado.Update();
    }

    /// <summary>
    /// Controla a visibilidade das colunas o grid.
    /// </summary>
    private void ControlaVisibleColunasGrid()
    {
        if (Convert.ToInt32(_rdbRelatorios.SelectedValue) == (int)ReportNameDocumentos.HistoricoEscolarPedagogico)
        {
            _grvDocumentoAluno.Columns[columnMatriculaEstadual].Visible = false;
            _grvDocumentoAluno.Columns[columnMatricula].Visible = false;
            _grvDocumentoAluno.Columns[columnNumeroChamada].Visible = true;
            _grvDocumentoAluno.Columns[columnNome].Visible = true;
            _grvDocumentoAluno.Columns[columnEscola].Visible = false;
            _grvDocumentoAluno.Columns[columnDataNascimento].Visible = true;
            _grvDocumentoAluno.Columns[columnTurma].Visible = false;
            _grvDocumentoAluno.Columns[columnCurso].Visible = false;
            _grvDocumentoAluno.Columns[columnCalendario].Visible = false;
            _grvDocumentoAluno.Columns[columnSituacao].Visible = false;

        }
        else
        {
            _grvDocumentoAluno.Columns[columnMatriculaEstadual].Visible = true;
            _grvDocumentoAluno.Columns[columnMatricula].Visible = true;
            _grvDocumentoAluno.Columns[columnNumeroChamada].Visible = false;
            _grvDocumentoAluno.Columns[columnNome].Visible = true;
            _grvDocumentoAluno.Columns[columnEscola].Visible = true;
            _grvDocumentoAluno.Columns[columnDataNascimento].Visible = false;
            _grvDocumentoAluno.Columns[columnTurma].Visible = true;
            _grvDocumentoAluno.Columns[columnCurso].Visible = true;
            _grvDocumentoAluno.Columns[columnCalendario].Visible = true;
            _grvDocumentoAluno.Columns[columnSituacao].Visible = true;

            // Carrega o nome referente ao parametro de matricula estadual.
            string nomeMatriculaEstadual = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            bool mostraMatriculaEstadual = !string.IsNullOrEmpty(nomeMatriculaEstadual);

            _grvDocumentoAluno.Columns[columnMatricula].Visible = !mostraMatriculaEstadual;
            _grvDocumentoAluno.Columns[columnMatriculaEstadual].Visible = mostraMatriculaEstadual;
            _grvDocumentoAluno.Columns[columnMatriculaEstadual].HeaderText = nomeMatriculaEstadual;
        }

        _updResultado.Update();
    }

    /// <summary>
    /// Salva os dados da busca realizada para ser carregada posteriormente.
    /// </summary>
    protected void SalvaBusca()
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
        filtros.Add("ttn_id", UCComboTurma1.Valor[2].ToString());
        filtros.Add("crp_idTurma", UCComboTurma1.Valor[1].ToString());
        filtros.Add("tipoBusca", UCCamposBuscaAluno1.TipoBuscaNomeAluno);

        if (divBuscaAvancadaAluno.Visible)
        {
            filtros.Add("pes_nome", UCCamposBuscaAluno1.NomeAluno);
            filtros.Add("pes_dataNascimento", UCCamposBuscaAluno1.DataNascAluno);
            filtros.Add("pes_nomeMae", UCCamposBuscaAluno1.NomeMaeAluno);
            filtros.Add("alc_matriculaEstadual", UCCamposBuscaAluno1.MatriculaEstadualAluno);
            filtros.Add("alc_matricula", UCCamposBuscaAluno1.MatriculaAluno);
        }

        if (divDeclaracaoSolicitacaoVaga.Visible)
        {
            filtros.Add("_txtNomeAluno", _txtNomeAluno.Text);
            filtros.Add("_txtDataNasc", _txtDataNasc.Text);
            filtros.Add("_txtEscolaOrigem", _txtEscolaOrigem.Text);
            filtros.Add("_ddlValidade", _ddlValidade.SelectedValue);
            filtros.Add("_txtHoras", _txtHoras.Text);
            filtros.Add("_txtDias", _txtDias.Text);
            filtros.Add("_txtData", _txtData.Text);
        }

        if (divDeclaracaoSolicitacaoComparecimento.Visible)
        {
            filtros.Add("txtNomeAluno", txtNomeAluno.Text);
            filtros.Add("txtNomePai", txtNomePai.Text);
            filtros.Add("txtNomeMae", txtNomeMae.Text);
            filtros.Add("txtNomeResp", txtNomeResp.Text);
            filtros.Add("txtData", txtData.Text);
            filtros.Add("ddlHoraComparecimento", ddlHoraComparecimento.SelectedValue);
            filtros.Add("ddlMinutoComparecimento", ddlMinutoComparecimento.SelectedValue);
        }

        if (divConviteReuniao.Visible)
        {
            filtros.Add("txtDataReuniao", txtDataReuniao.Text);
            filtros.Add("txtAssunto", txtAssunto.Text);
            filtros.Add("ddlHora", ddlHora.SelectedValue);
            filtros.Add("ddlMinutos", ddlMinutos.SelectedValue);
            filtros.Add("ddlHoraFinal", ddlHoraFinal.SelectedValue);
            filtros.Add("ddlMinutosFinal", ddlMinutosFinal.SelectedValue);
        }

        filtros.Add("relSelecionado", _rdbRelatorios.SelectedValue);
        filtros.Add("emitirDocAnoAnt", ChkEmitirAnterior.Checked ? "True" : "False");

        __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.DocumentosAluno, Filtros = filtros };
    }

    /// <summary>
    /// Verifica se há busca salva e carrega os combos da tela.
    /// </summary>
    protected void CarregaBusca()
    {
        if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.DocumentosAluno)
        {
            string valor, valor2, valor3;

            _rdbRelatorios.DataBind();
            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("relSelecionado", out valor);
            _rdbRelatorios.SelectedValue = valor;
            SelecionaRelatorio();

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("emitirDocAnoAnt", out valor);
            ChkEmitirAnterior.Checked = valor == "True";
            AnosAnteriores();

            long doc_id = -1;

            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
            {
                doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                UCComboUAEscola.InicializarVisaoIndividual(doc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                string esc_id;
                string uni_id;

                if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                    (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
                {
                    UCComboUAEscola.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                    UCComboUAEscola_IndexChangedUnidadeEscola();
                }
            }
            else
            {
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor);
                if (!string.IsNullOrEmpty(valor))
                {
                    UCComboUAEscola.Uad_ID = new Guid(valor);
                    UCComboUAEscola.CarregaEscolaPorUASuperiorSelecionada();

                    if (UCComboUAEscola.Uad_ID != Guid.Empty)
                    {
                        UCComboUAEscola.FocoEscolas = true;
                        UCComboUAEscola.PermiteAlterarCombos = true;
                    }
                    string esc_id;
                    string uni_id;

                    if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                        (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
                    {
                        UCComboUAEscola.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                        UCComboUAEscola_IndexChangedUnidadeEscola();
                    }
                }
            }

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out valor2);
            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out valor);
            UCComboCursoCurriculo1.Valor = new[] { Convert.ToInt32(valor2), Convert.ToInt32(valor) };
            UCComboCursoCurriculo1_IndexChanged();

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_id", out valor);
            //if (doc_id <= 0)
            UCComboCurriculoPeriodo1.Valor = new[] { UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1], Convert.ToInt32(valor) };
            //else
            //    UCBuscaDocenteTurma._VS_doc_id = doc_id;
            UCComboCurriculoPeriodo1__OnSelectedIndexChange();

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor);
            UCComboCalendario1.Valor = Convert.ToInt32(valor);
            UCComboCalendario1_IndexChanged();

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_id", out valor);
            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ttn_id", out valor2);
            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_idTurma", out valor3);
            UCComboTurma1.Valor = new[] { Convert.ToInt64(valor), Convert.ToInt64(valor3), Convert.ToInt64(valor2) };

            if (divBuscaAvancadaAluno.Visible)
            {
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tipoBusca", out valor);
                UCCamposBuscaAluno1.TipoBuscaNomeAluno = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("pes_nome", out valor);
                UCCamposBuscaAluno1.NomeAluno = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("pes_dataNascimento", out valor);
                UCCamposBuscaAluno1.DataNascAluno = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("pes_nomeMae", out valor);
                UCCamposBuscaAluno1.NomeMaeAluno = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("alc_matricula", out valor);
                UCCamposBuscaAluno1.MatriculaAluno = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("alc_matriculaEstadual", out valor);
                UCCamposBuscaAluno1.MatriculaEstadualAluno = valor;
            }

            if (divDeclaracaoSolicitacaoVaga.Visible)
            {
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("_txtNomeAluno", out valor);
                _txtNomeAluno.Text = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("_txtDataNasc", out valor);
                _txtDataNasc.Text = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("_txtEscolaOrigem", out valor);
                _txtEscolaOrigem.Text = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("_ddlValidade", out valor);
                _ddlValidade.SelectedValue = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("_txtHoras", out valor);
                _txtHoras.Text = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("_txtDias", out valor);
                _txtDias.Text = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("_txtData", out valor);
                _txtData.Text = valor;
            }

            if (divDeclaracaoSolicitacaoComparecimento.Visible)
            {
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("txtNomeAluno", out valor);
                txtNomeAluno.Text = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("txtNomePai", out valor);
                txtNomePai.Text = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("txtNomeMae", out valor);
                txtNomeMae.Text = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("txtNomeResp", out valor);
                txtNomeResp.Text = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("txtData", out valor);
                txtData.Text = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ddlHoraComparecimento", out valor);
                ddlHoraComparecimento.SelectedValue = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ddlMinutoComparecimento", out valor);
                ddlMinutoComparecimento.SelectedValue = valor;
            }

            if (divConviteReuniao.Visible)
            {
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("txtDataReuniao", out valor);
                txtDataReuniao.Text = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("txtAssunto", out valor);
                txtAssunto.Text = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ddlHora", out valor);
                ddlHora.SelectedValue = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ddlMinutos", out valor);
                ddlMinutos.SelectedValue = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ddlHoraFinal", out valor);
                ddlHoraFinal.SelectedValue = valor;
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ddlMinutosFinal", out valor);
                ddlMinutosFinal.SelectedValue = valor;
            }

            if (_btnPesquisar.Visible)
            {
                try
                {
                    // Validação de preenchimento para o HistoricoEscolarSMESP
                    if (Convert.ToInt32(_rdbRelatorios.SelectedValue) == (int)ReportNameDocumentos.HistoricoEscolarPedagogico)
                    {
                        // Validação de preenchimento para o HistoricoEscolarSMESP
                        if ((string.IsNullOrEmpty(UCCamposBuscaAluno1.NomeAluno) && string.IsNullOrEmpty(UCCamposBuscaAluno1.MatriculaAluno)) && chkBuscaAvancada.Checked)
                            throw new ValidationException("Nome ou " + GestaoEscolarUtilBO.nomePadraoMatriculaEstadual(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " do aluno deve ser preenchido.");
                    }

                    if (UCCamposBuscaAluno1.IsValid)
                        Pesquisar();
                    else
                        throw new ValidationException("Data de nascimento do aluno não está no formato dd/mm/aaaa ou é inexistente.");
                }
                catch (ValidationException ex)
                {
                    _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alunos.", UtilBO.TipoMensagem.Erro);
                }
            }


        }
    }

    /// <summary>
    /// Controla a exibicao dos filtros baseado no relatorio selecionado
    /// </summary>
    private void SelecionaRelatorio()
    {
        lblAvisoMensagem.Visible = false;
        msgCertConcCurso.Visible = false;
        btnGerarDeclaracaoTrabalho.Visible = false;

        //fdsResultados.Style.Add("display", "none");
        fdsResultados.Visible = false;
        LimparCampos();

        AdicionaAsteriscoObrigatorio(_lblDestinatario);
        _rfvGrupamento.Visible = true;

        UCCamposBuscaAluno1.VisibleDataNascimento = UCCamposBuscaAluno1.VisibleMatriculaEstadual = UCCamposBuscaAluno1.VisibleNomeMae = true;
        chkBuscaAvancada.Checked = false;

        switch (Convert.ToInt32(_rdbRelatorios.SelectedValue))
        {
            case (int)ReportNameDocumentos.DeclaracaoSolicitacaoVaga:
                FiltrosDeclaracaoSolicitacaoVaga();
                UCCamposObrigatorios.Visible = true;
                break;

            case (int)ReportNameDocumentos.DeclaracaoSolicitacaoComparecimento:
                FiltrosDeclaracaoSolicitacaoComparecimento();
                UCCamposObrigatorios.Visible = true;
                break;

            case (int)ReportNameDocumentos.ConviteReuniao:
                FiltrosConviteReuniao();
                UCCamposObrigatorios.Visible = true;
                break;

            case (int)ReportNameDocumentos.HistoricoEscolar:
            case (int)ReportNameDocumentos.FichaCadastralAluno:
            case (int)ReportNameDocumentos.ComprovanteMatricula:
            case (int)ReportNameDocumentos.BoletimEscolar:
            case (int)ReportNameDocumentos.EncaminhamentoAlunoRemanejado:
                HabilitarFiltrosPadrao(true);
                HabilitarFiltrosDiversos(false);
                ChkEmitirAnterior.Visible = true;

                if (Convert.ToInt32(_rdbRelatorios.SelectedValue) == (int)ReportNameDocumentos.BoletimEscolar)
                {
                    string sDeclaracaoHMTL = CFG_ParametroDocumentoAlunoBO.ParametroValor(ChaveParametroDocumentoAluno.DECLARACAO_HTML, __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                        (int)ReportNameDocumentos.BoletimEscolar);

                    if ((!string.IsNullOrEmpty(sDeclaracaoHMTL)) && (Convert.ToBoolean(sDeclaracaoHMTL)))
                    {
                        UCCamposObrigatorios.Visible =
                        UCComboUAEscola.ObrigatorioEscola =
                        UCComboUAEscola.ObrigatorioUA =
                        UCComboCursoCurriculo1.Obrigatorio =
                        UCComboCurriculoPeriodo1.Obrigatorio =
                        UCComboCalendario1.Obrigatorio = true;
                    }
                }
                else
                {
                    UCCamposObrigatorios.Visible = false;
                }
                break;
            case (int)ReportNameDocumentos.DocAluDeclaracaoComparecimento:
                {
                    HabilitarFiltrosPadrao(true);
                    HabilitarFiltrosDiversos(false);
                    ChkEmitirAnterior.Visible = false;
                    UCCamposObrigatorios.Visible =
                        UCComboUAEscola.ObrigatorioEscola =
                        UCComboUAEscola.ObrigatorioUA =
                        UCComboCursoCurriculo1.Obrigatorio =
                        UCComboCurriculoPeriodo1.Obrigatorio =
                        UCComboCalendario1.Obrigatorio = true;
                    break;
                }
            case (int)ReportNameDocumentos.DocAluCartaoIdentificacao:
            case (int)ReportNameDocumentos.DocAluCarteirinhaEstudante:
                {
                    HabilitarFiltrosPadrao(true);
                    HabilitarFiltrosDiversos(false);
                    ChkEmitirAnterior.Visible = false;
                    UCCamposObrigatorios.Visible = false;
                    break;
                }
            case (int)ReportNameDocumentos.DocAluFichaMatricula:
                {
                    HabilitarFiltrosPadrao(true);
                    HabilitarFiltrosDiversos(false);
                    ChkEmitirAnterior.Visible = false;
                    UCCamposObrigatorios.Visible =
                        UCComboUAEscola.ObrigatorioEscola =
                        UCComboUAEscola.ObrigatorioUA = true;
                    break;
                }
            case (int)ReportNameDocumentos.HistoricoEscolarPedagogico:
                HabilitarFiltrosPadrao(true);
                HabilitarFiltrosDiversos(false);
                ChkEmitirAnterior.Visible = false;
                UCCamposObrigatorios.Visible =
                UCComboUAEscola.ObrigatorioEscola =
                UCComboUAEscola.ObrigatorioUA =
                UCComboCursoCurriculo1.Obrigatorio =
                UCComboCurriculoPeriodo1.Obrigatorio =
                UCComboCalendario1.Obrigatorio =
                UCComboTurma1.Obrigatorio =
                chkBuscaAvancada.Visible =
                UCCamposBuscaAluno1.Visible = true;
                divBuscaAvancadaAluno.Visible = false;
                UCCamposBuscaAluno1.VisibleDataNascimento = UCCamposBuscaAluno1.VisibleMatriculaEstadual = UCCamposBuscaAluno1.VisibleNomeMae = false;
                break;

            case (int)ReportNameDocumentos.CertificadoConclusaoCurso:
                HabilitarFiltrosPadrao(true);
                HabilitarFiltrosDiversos(false);
                ChkEmitirAnterior.Visible = false;
                msgCertConcCurso.Visible = true;
                UCCamposObrigatorios.Visible = false;
                break;

            case (int)ReportNameDocumentos.DeclaracaoEscolaridade:
                HabilitarFiltrosPadrao(true);
                HabilitarFiltrosDiversos(false);
                RemoveAsteriscoObrigatorio(_lblDestinatario);
                _rfvGrupamento.Visible = false;
                ChkEmitirAnterior.Visible = true;
                UCCamposObrigatorios.Visible = false;
                break;

            case (int)ReportNameDocumentos.DeclaracaoSolicitacaoTransferencia:
                HabilitarFiltrosPadrao(true);
                HabilitarFiltrosDiversos(false);
                UCCamposObrigatorios.Visible = false;
                break;

            case (int)ReportNameDocumentos.DeclaracaoTrabalho:
                HabilitarFiltrosPadrao(false);
                HabilitarFiltrosDiversos(false);
                btnGerarDeclaracaoTrabalho.Visible = true;
                UCCamposObrigatorios.Visible = false;
                break;

            case (int)ReportNameDocumentos.CertidaoEscolaridadeAnterior1972:
                FiltrosCertidaoEscolaridadeAnterior1972();
                UCCamposObrigatorios.Visible = true;
                break;

            case (int)ReportNameDocumentos.FichaIndividualAvaliacaoPeriodica:
                HabilitarFiltrosPadrao(true);
                HabilitarFiltrosDiversos(false);
                ChkEmitirAnterior.Visible = false;
                UCCamposObrigatorios.Visible = true;
                UCComboCalendario1.Obrigatorio = true;
                UCComboCalendario1.Carregar();
                UCComboCalendario1.PermiteEditar = true;
                break;

            case (int)ReportNameDocumentos.FichaIndividualAluno:
                HabilitarFiltrosPadrao(true);
                HabilitarFiltrosDiversos(false);
                UCCamposObrigatorios.Visible = false;
                ChkEmitirAnterior.Visible = true;
                ChkEmitirAnterior.Text = "Exibir dados dos anos anteriores";
                break;

            default:
                HabilitarFiltrosPadrao(true);
                HabilitarFiltrosDiversos(false);
                UCCamposObrigatorios.Visible = false;
                break;
        }

        Inicializar();
        ControlaVisibleColunasGrid();
    }

    /// <summary>
    /// Carrega informações de anos anteriores
    /// </summary>
    private void AnosAnteriores()
    {
        if (ChkEmitirAnterior.Checked)
        {
            if (UCComboUAEscola.Esc_ID > 0 && UCComboUAEscola.Uni_ID > 0)
            {
                UCComboCursoCurriculo1.CarregarPorEscola(UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID);

                UCComboCursoCurriculo1.SetarFoco();
                UCComboCursoCurriculo1.PermiteEditar = true;
            }
        }
        else
        {
            if (UCComboUAEscola.Esc_ID > 0 && UCComboUAEscola.Uni_ID > 0)
            {
                UCComboCursoCurriculo1.CarregarVigentesPorEscola(UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID);

                UCComboCursoCurriculo1.SetarFoco();
                UCComboCursoCurriculo1.PermiteEditar = true;
            }
        }

        UCComboCursoCurriculo1_IndexChanged();
    }

    /// <summary>
    /// Limpa a busca salva na session.
    /// </summary>
    private void LimparBuscaRealizada()
    {
        __SessionWEB.BuscaRealizada = new BuscaGestao();
        UCComboUAEscola.Uad_ID = new Guid();
        UCComboUAEscola.CarregaEscolaPorUASuperiorSelecionada();

        UCComboUAEscola.SelectedValueEscolas = new[] { -1, -1 };
        UCComboUAEscola_IndexChangedUA();
    }

    #endregion Métodos
}
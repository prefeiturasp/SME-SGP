using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using ReportNameDocumentos = MSTech.GestaoEscolar.BLL.ReportNameDocumentos;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;

public partial class Documentos_Efetivacao_Busca : MotherPageLogado
{
    #region Constantes

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

    #region Propriedades

    private Boolean _VS_PesquisaSalva
    {
        get
        {
            return (Boolean)ViewState["_VS_PesquisaSalva"];
        }
        set
        {
            ViewState["_VS_PesquisaSalva"] = value;
        }
    }

    private SortedDictionary<long, bool> _VS_AlunosSelecionados
    {
        get
        {
            if (ViewState["_VS_AlunosSelecionados"] == null)
                ViewState["_VS_AlunosSelecionados"] = new SortedDictionary<long, bool>();
            return (SortedDictionary<long, bool>)ViewState["_VS_AlunosSelecionados"];
        }
    }

    #endregion Propriedades

    #region Page Life Cycle

    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        _VS_PesquisaSalva = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            _VS_PesquisaSalva = false;

            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
                _lblMessage.Text = message;

            try
            {
                if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                {
                    fdsBusca.Visible = false;
                    _lblMessage.Text = UtilBO.GetErroMessage("Você não possui permissão para acessar a página solicitada.", UtilBO.TipoMensagem.Alerta);
                }

                UCComboCalendario1.Obrigatorio = true;
                UCFiltroEscolas1.UnidadeAdministrativaCampoObrigatorio = true;
                UCFiltroEscolas1.EscolaCampoObrigatorio = true;
                UCComboCursoCurriculo1.Obrigatorio = true;
                UCComboCurriculoPeriodo1.Obrigatorio = true;
                UCComboTurma1.Obrigatorio = true;

                UCComboCalendario1.CarregarCalendarioAnual();
                UCComboCalendario1.PermiteEditar = false;
                UCFiltroEscolas1._LoadInicial();

                UCComboCursoCurriculo1.CancelSelect = true;
                UCComboCursoCurriculo1.PermiteEditar = false;

                UCComboCurriculoPeriodo1.CancelSelect = true;
                UCComboCurriculoPeriodo1.PermiteEditar = false;

                UCComboTurma1.CancelSelect = true;
                UCComboTurma1.PermiteEditar = false;

                UCComboTurma1.CancelSelect = true;
                UCComboTurmaDisciplina1.CarregarTurmaDisciplina(-1);
                UCComboTurmaDisciplina1.PermiteEditar = false;
                UCComboTurmaDisciplina1.Obrigatorio = true;

                VerificaBusca();

                //UCComboPeriodoCalendario1.CancelSelect = true;
                //UCComboPeriodoCalendario1.CarregarTipoPeriodoCalendario_FAV(-1);
                //UCComboPeriodoCalendario1.PermiteEditar = false;
                //UCComboPeriodoCalendario1.ValidationValueToCompare = "-1,-1";
                //UCComboPeriodoCalendario1.Obrigatorio = true;
                //UCComboPeriodoCalendario1.Titulo = "Período do calendário";
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }

            _divPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnGerarRelatorio.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
        }

        UCFiltroEscolas1._Selecionar += UCFiltroEscolas1__Selecionar;
        UCFiltroEscolas1._SelecionarEscola += UCFiltroEscolas1__SelecionarEscola;
        UCComboCursoCurriculo1.IndexChanged += UCComboCursoCurriculo1_IndexChanged;
        UCComboCurriculoPeriodo1._OnSelectedIndexChange += UCComboCurriculoPeriodo1__OnSelectedIndexChange;
        UCComboCalendario1.IndexChanged += UCComboCalendario1_IndexChanged;
        UCComboTurma1._SelecionaTurma += UCComboTurma1__SelectedIndexChanged;
    }

    #endregion Page Life Cycle

    #region Métodos

    private void SalvaFiltros()
    {
        Dictionary<string, string> filtros = new Dictionary<string, string>();
        string[] idsEscola = UCFiltroEscolas1._ComboUnidadeEscola.SelectedValue.Split(';');

        filtros.Add("uad_idSuperior", (UCFiltroEscolas1._ComboUnidadeAdministrativa.SelectedValue == Guid.Empty.ToString()
                                      ? "" : UCFiltroEscolas1._ComboUnidadeAdministrativa.SelectedValue));
        filtros.Add("uni_id", idsEscola[1]);
        filtros.Add("esc_id", idsEscola[0]);
        filtros.Add("cur_id", UCComboCursoCurriculo1.Valor[0].ToString());
        filtros.Add("crr_id", UCComboCursoCurriculo1.Valor[1].ToString());
        filtros.Add("crp_id", UCComboCurriculoPeriodo1.Valor[2].ToString());
        filtros.Add("tud_id", UCComboTurmaDisciplina1.Valor.ToString());
        filtros.Add("cal_id", UCComboCalendario1.Valor.ToString());
        filtros.Add("tur_id", UCComboTurma1.Valor[0].ToString());
        filtros.Add("ttn_id", UCComboTurma1.Valor[2].ToString());
        __SessionWEB.BuscaRealizada = new BuscaGestao
        {
            PaginaBusca = PaginaGestao.ComprovanteEfetivacao,
            Filtros = filtros
        };
    }

    /// <summary>
    /// Verifica se exsite filtros de buscas de documentos salvos na sessão
    /// Se existir , recupera e executa a busca
    /// </summary>
    protected void VerificaBusca()
    {
        string valor, valor1, valor2;

        if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.ComprovanteEfetivacao)
        {
            _VS_PesquisaSalva = true;
            UCFiltroEscolas1._ComboUnidadeAdministrativa.Enabled = true;

            // Combo de unidade administrativa
            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor);
            if (valor != "-1" && !String.IsNullOrEmpty(valor))
            {
                UCFiltroEscolas1._ComboUnidadeAdministrativa.SelectedValue = valor;

                UCFiltroEscolas1._UnidadeEscola_LoadBy_uad_idSuperior(new Guid(valor));
                UCFiltroEscolas1._ComboUnidadeEscola.Enabled = true;
            }

            // combo de escola
            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out valor);
            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out valor1);
            if (valor != "-1" && valor1 != "-1" && !String.IsNullOrEmpty(valor) && !String.IsNullOrEmpty(valor1))
            {
                UCFiltroEscolas1._ComboUnidadeEscola.SelectedValue = valor + ";" + valor1;

                UCComboCursoCurriculo1.CancelSelect = false;
                UCComboCursoCurriculo1.CarregarCursoCurriculoPorEscola(UCFiltroEscolas1._UCComboUnidadeEscola_Esc_ID, UCFiltroEscolas1._UCComboUnidadeEscola_Uni_ID, 0);
                UCComboCursoCurriculo1.PermiteEditar = true;
            }

            // Curso curriculo (ou Etapa de ensino)
            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out valor);
            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out valor1);
            if (valor != "-1" && valor1 != "-1" && !String.IsNullOrEmpty(valor) && !String.IsNullOrEmpty(valor1))
            {
                UCComboCursoCurriculo1.Valor = new[] { Convert.ToInt32(valor), Convert.ToInt32(valor1) };

                UCComboCurriculoPeriodo1.CancelSelect = false;
                UCComboCurriculoPeriodo1._MostrarMessageSelecione = true;
                UCComboCurriculoPeriodo1._Load(UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1]);
                UCComboCurriculoPeriodo1._Combo.Enabled = true;
            }

            // Periodo de ensino
            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_id", out valor2);
            if (valor != "-1" && valor1 != "-1" && valor2 != "-1" && !String.IsNullOrEmpty(valor) && !String.IsNullOrEmpty(valor1) && !String.IsNullOrEmpty(valor2))
            {
                UCComboCurriculoPeriodo1.Valor = new[] { Convert.ToInt32(valor), Convert.ToInt32(valor1), Convert.ToInt32(valor2) };

                UCComboCalendario1.CarregarCalendarioAnualPorCurso(UCComboCursoCurriculo1.Valor[0]);
                UCComboCalendario1.PermiteEditar = true;
            }

            // Calendário
            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor);
            if (valor != "-1" && !String.IsNullOrEmpty(valor))
            {
                UCComboCalendario1.Valor = Convert.ToInt32(valor);

                UCComboTurma1.CancelSelect = false;
                UCComboTurma1._MostrarMessageSelecione = true;
                UCComboTurma1.CarregarPorEscolaCurriculoCalendario(UCFiltroEscolas1._UCComboUnidadeEscola_Esc_ID, UCFiltroEscolas1._UCComboUnidadeEscola_Uni_ID, UCComboCurriculoPeriodo1.Valor[0], UCComboCurriculoPeriodo1.Valor[1], UCComboCurriculoPeriodo1.Valor[2], UCComboCalendario1.Valor);
                UCComboTurma1.PermiteEditar = true;
            }

            // Turma  tur_crp_ttn_id
            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_id", out valor);
            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_id", out valor1);
            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ttn_id", out valor2);
            if (valor != "-1" && valor1 != "-1" && valor2 != "-1" && !String.IsNullOrEmpty(valor) && !String.IsNullOrEmpty(valor1) && !String.IsNullOrEmpty(valor2))
            {
                UCComboTurma1.Valor = new[] { Convert.ToInt64(valor), Convert.ToInt64(valor1), Convert.ToInt64(valor2) };
                UCComboTurmaDisciplina1.CarregarTurmaDisciplina(UCComboTurma1.Valor[0]);
                UCComboTurmaDisciplina1.PermiteEditar = true;
                UCComboTurmaDisciplina1.SetarFoco();
            }

            // TurmaDisciplina
            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tud_id", out valor);
            if (valor != "-1" && !String.IsNullOrEmpty(valor))
            {
                UCComboTurmaDisciplina1.Valor = Convert.ToInt64(valor);
            }
        }
    }

    #endregion Métodos

    #region Eventos

    protected void odsDocumentoAluno_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (e.ExecutingSelectCount)
            e.InputParameters.Clear();
    }

    protected void _grvDocumentoAluno_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[cellCurso].Text = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            string _paramValor = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            if (!String.IsNullOrEmpty(_paramValor))
                e.Row.Cells[columnMatriculaEstadual].Text = _paramValor;
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox _chkSelecionar = (CheckBox)e.Row.FindControl("_chkSelecionar");
            if (_chkSelecionar != null)
            {
                _chkSelecionar.Attributes.Add("index", e.Row.RowIndex.ToString());
                if (_VS_AlunosSelecionados.ContainsKey(Convert.ToInt64(_chkSelecionar.Attributes["alu_id"])))
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

    protected void _btnGerarRelatorio_Click(object sender, EventArgs e)
    {
        try
        {
            SalvaFiltros();

            string parametros = String.Empty;
            string[] idsEscola = UCFiltroEscolas1._ComboUnidadeEscola.SelectedValue.Split(';');

            parametros = "tur_id=" + UCComboTurma1.Valor[0] +
                         "&esc_id=" + idsEscola[0] +
                         "&uni_id=" + idsEscola[1] +
                         "&telefone=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_TELEFONE) +
                         "&email=" + SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_MEIOCONTATO_EMAIL) +
                         "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                         "&tud_id=" + UCComboTurmaDisciplina1.Valor +
                         "&cal_id=" + UCComboCalendario1.Valor +
                         "&adm=" + (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao).ToString() +
                         "&gru_id=" + __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString() +
                         "&usu_id=" + __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString();

            string report = ((int)ReportNameDocumentos.ComprovanteEfetivacao).ToString();
            
            CFG_RelatorioBO.CallReport("Documentos", report, parametros, HttpContext.Current);
        }
        catch (Exception err)
        {
            ApplicationWEB._GravaErro(err);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o documento comprovante de efetivação.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void odsDocumentoAluno_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            ApplicationWEB._GravaErro(e.Exception);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alunos.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion Eventos

    #region Delegates

    private void UCFiltroEscolas1__Selecionar()
    {
        try
        {
            if (!_VS_PesquisaSalva)
            {
                UCFiltroEscolas1._ComboUnidadeEscola.SelectedValue = "-1;-1";
                UCFiltroEscolas1._ComboUnidadeEscola.Enabled = false;
                UCComboCursoCurriculo1.Valor = new[] { -1, -1 };
                UCComboCursoCurriculo1.PermiteEditar = false;
                UCComboCurriculoPeriodo1._Combo.SelectedValue = "-1;-1;-1";
                UCComboCurriculoPeriodo1._Combo.Enabled = false;
                UCComboCalendario1.Valor = -1;
                UCComboCalendario1.PermiteEditar = false;
                UCComboTurma1._Combo.SelectedValue = "-1;-1;-1";
                UCComboTurma1._Combo.Enabled = false;
                UCComboTurmaDisciplina1.Valor = -1;
                UCComboTurmaDisciplina1.PermiteEditar = false;

                if (UCFiltroEscolas1._VS_FiltroEscola &&
                    UCFiltroEscolas1._ComboUnidadeAdministrativa.SelectedValue != Guid.Empty.ToString())
                {
                    UCFiltroEscolas1._UnidadeEscola_LoadBy_uad_idSuperior(
                        new Guid(UCFiltroEscolas1._ComboUnidadeAdministrativa.SelectedValue));
                    UCFiltroEscolas1._ComboUnidadeEscola.Enabled = true;
                    UCFiltroEscolas1._ComboUnidadeEscola.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a(s) unidade(s) escolar(es).", UtilBO.TipoMensagem.Erro);
        }
    }

    private void UCFiltroEscolas1__SelecionarEscola()
    {
        try
        {
            if (!_VS_PesquisaSalva)
            {
                UCComboCursoCurriculo1.Valor = new[] { -1, -1 };
                UCComboCursoCurriculo1.PermiteEditar = false;
                UCComboCurriculoPeriodo1._Combo.SelectedValue = "-1;-1;-1";
                UCComboCurriculoPeriodo1._Combo.Enabled = false;
                UCComboCalendario1.Valor = -1;
                UCComboCalendario1.PermiteEditar = false;
                UCComboTurma1._Combo.SelectedValue = "-1;-1;-1";
                UCComboTurma1._Combo.Enabled = false;

                UCComboTurmaDisciplina1.Valor = -1;
                UCComboTurmaDisciplina1.PermiteEditar = false;
                //UCComboPeriodoCalendario1.Valor_Tpc_id__Cap_id = "-1;-1";
                //UCComboPeriodoCalendario1.PermiteEditar = false;

                if (UCFiltroEscolas1._UCComboUnidadeEscola_Esc_ID > 0)
                {
                    UCComboCursoCurriculo1.CancelSelect = false;
                    UCComboCursoCurriculo1.CarregarCursoCurriculoPorEscola(
                        UCFiltroEscolas1._UCComboUnidadeEscola_Esc_ID, UCFiltroEscolas1._UCComboUnidadeEscola_Uni_ID, 0);
                    UCComboCursoCurriculo1.PermiteEditar = true;
                    UCComboCursoCurriculo1.SetarFoco();
                }
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o(s) curso(s) da unidade escolar.", UtilBO.TipoMensagem.Erro);
        }
    }

    private void UCComboCursoCurriculo1_IndexChanged()
    {
        try
        {
            if (!_VS_PesquisaSalva)
            {
                UCComboCurriculoPeriodo1._Combo.SelectedValue = "-1;-1;-1";
                UCComboCurriculoPeriodo1._Combo.Enabled = false;
                UCComboCalendario1.Valor = -1;
                UCComboCalendario1.PermiteEditar = false;
                UCComboTurma1._Combo.SelectedValue = "-1;-1;-1";
                UCComboTurma1._Combo.Enabled = false;

                UCComboTurmaDisciplina1.Valor = -1;
                UCComboTurmaDisciplina1.PermiteEditar = false;

                //UCComboPeriodoCalendario1.Valor_Tpc_id__Cap_id = "-1;-1";
                //UCComboPeriodoCalendario1.PermiteEditar = false;

                if (UCComboCursoCurriculo1.Valor[0] > 0)
                {
                    UCComboCurriculoPeriodo1._Combo.Items.Clear();
                    UCComboCurriculoPeriodo1._MostrarMessageSelecione = true;
                    UCComboCurriculoPeriodo1._Load(UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1]);
                    UCComboCurriculoPeriodo1._Combo.Enabled = true;
                    UCComboCurriculoPeriodo1._Combo.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o(s) período(s) do curso.", UtilBO.TipoMensagem.Erro);
        }
    }

    private void UCComboCurriculoPeriodo1__OnSelectedIndexChange()
    {
        try
        {
            if (!_VS_PesquisaSalva)
            {
                UCComboCalendario1.Valor = -1;
                UCComboCalendario1.PermiteEditar = false;
                UCComboTurma1._Combo.SelectedValue = "-1;-1;-1";
                UCComboTurma1._Combo.Enabled = false;

                UCComboTurmaDisciplina1.Valor = -1;
                UCComboTurmaDisciplina1.PermiteEditar = false;

                //UCComboPeriodoCalendario1.Valor_Tpc_id__Cap_id = "-1;-1";
                //UCComboPeriodoCalendario1.PermiteEditar = false;

                if (UCComboCurriculoPeriodo1.Valor[0] > 0)
                {
                    UCComboCalendario1.CarregarCalendarioAnualPorCurso(UCComboCursoCurriculo1.Valor[0]);
                    UCComboCalendario1.PermiteEditar = true;
                    UCComboCalendario1.SetarFoco();
                    //   UCComboCalendario1.Valor = -1;
                }
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o(s) calendários(s).", UtilBO.TipoMensagem.Erro);
        }
    }

    private void UCComboTurma1__SelectedIndexChanged()
    {
        try
        {
            if (!_VS_PesquisaSalva)
            {
                //UCComboTurmaDisciplina1.Valor = -1;
                UCComboTurmaDisciplina1.PermiteEditar = false;

                if (UCComboTurma1.Valor[0] > 0)
                {
                    //UCComboPeriodoCalendario1.CarregarTipoPeriodoCalendario_FAV(UCComboTurma1.Valor[0]);
                    //UCComboPeriodoCalendario1.PermiteEditar = true;
                    // UCComboPeriodoCalendario1.SetarFoco();

                    UCComboTurmaDisciplina1.CarregarTurmaDisciplina(UCComboTurma1.Valor[0]);
                    UCComboTurmaDisciplina1.PermiteEditar = true;
                    UCComboTurmaDisciplina1.SetarFoco();
                }
                else
                {
                    UCComboTurmaDisciplina1.CarregarTurmaDisciplina(-1);
                }
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o(s) calendários(s).", UtilBO.TipoMensagem.Erro);
        }
    }

    private void UCComboCalendario1_IndexChanged()
    {
        try
        {
            if (!_VS_PesquisaSalva)
            {
                UCComboTurma1._Combo.SelectedValue = "-1;-1;-1";
                UCComboTurma1.PermiteEditar = false;

                UCComboTurmaDisciplina1.Valor = -1;
                UCComboTurmaDisciplina1.PermiteEditar = false;

                if (UCComboCalendario1.Valor > 0)
                {
                    UCComboTurma1._Combo.Items.Clear();
                    UCComboTurma1._MostrarMessageSelecione = true;
                    UCComboTurma1.CarregarPorEscolaCurriculoCalendario(UCFiltroEscolas1._UCComboUnidadeEscola_Esc_ID,
                                                                       UCFiltroEscolas1._UCComboUnidadeEscola_Uni_ID,
                                                                       UCComboCurriculoPeriodo1.Valor[0],
                                                                       UCComboCurriculoPeriodo1.Valor[1],
                                                                       UCComboCurriculoPeriodo1.Valor[2],
                                                                       UCComboCalendario1.Valor);

                    UCComboTurma1.PermiteEditar = true;
                    UCComboTurma1._Combo.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a(s) turma(s) do período.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion Delegates
}
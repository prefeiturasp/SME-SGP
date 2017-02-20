using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using MSTech.Validation.Exceptions;

public partial class Academico_Turma_Busca : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Propriedade que retorna o valor de tur_id.
    /// </summary>
    public long Edit_tur_id
    {
        get
        {
            return Convert.ToInt64(grvTurma.DataKeys[grvTurma.SelectedIndex]["tur_id"]);
        }
    }

    /// <summary>
    /// Propriedade que retorna o valor de cur_id.
    /// </summary>
    public int Edit_cur_id
    {
        get
        {
            return Convert.ToInt32(grvTurma.DataKeys[grvTurma.SelectedIndex]["cur_id"]);
        }
    }

    /// <summary>
    /// Propriedade que retorna o valor de crr_id.
    /// </summary>
    public int Edit_crr_id
    {
        get
        {
            return Convert.ToInt32(grvTurma.DataKeys[grvTurma.SelectedIndex]["crr_id"]);
        }
    }

    /// <summary>
    /// Propriedade que retorna o valor de crp_id.
    /// </summary>
    public int Edit_crp_id
    {
        get
        {
            return Convert.ToInt32(grvTurma.DataKeys[grvTurma.SelectedIndex]["crp_id"]);
        }
    }

    /// <summary>
    /// Propriedade que retorna a situação.
    /// </summary>
    public string Edit_tur_situacao
    {
        get
        {
            return Convert.ToString(grvTurma.DataKeys[grvTurma.SelectedIndex]["tur_situacao"]);
        }
    }

    /// <summary>
    /// Propriedade que retorno o turno da turma
    /// </summary>
    public int Edit_trn_id
    {
        get
        {
            return Convert.ToInt32(grvTurma.DataKeys[grvTurma.SelectedIndex]["trn_id"]);
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada.
    /// </summary>
    private string VS_Ordenacao
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Turma)
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                string valor;
                if (filtros.TryGetValue("VS_Ordenacao", out valor))
                {
                    return valor;
                }
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada.
    /// </summary>
    private SortDirection VS_SortDirection
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Turma)
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

    #endregion Propriedades

    #region Constantes

    private const int INDEX_COLUNA_CURSO = 3;

    #endregion Constantes

    #region Eventos Page Life Cycle

    protected void Page_init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            grvTurma.Columns[INDEX_COLUNA_CURSO].HeaderText = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
        }

        UCComboQtdePaginacao1.GridViewRelacionado = grvTurma;

        if (!IsPostBack)
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
                lblMessage.Text = message;

            grvTurma.PageSize = ApplicationWEB._Paginacao;

            try
            {
                Inicializar();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }

            Page.Form.DefaultButton = btnPesquisar.UniqueID;
            Page.Form.DefaultFocus = ucComboUAEscola.VisibleUA ? ucComboUAEscola.ComboUA_ClientID : ucComboUAEscola.ComboEscola_ClientID;
        }

        ucComboUAEscola.IndexChangedUA += UCFiltroEscolas1__Selecionar;
        ucComboUAEscola.IndexChangedUnidadeEscola += UCFiltroEscolas1__SelecionarEscola;
        ucComboCursoCurriculo.IndexChanged += _UCComboCursoCurriculo_IndexChanged;        
    }

    #endregion Eventos Page Life Cycle

    #region Delegates

    protected void UCComboQtdePaginacao_IndexChanged()
    {
        try
        {
            Pesquisar(grvTurma.PageIndex, false);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as turmas.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Evento change do combo de UA Superior.
    /// </summary>
    private void UCFiltroEscolas1__Selecionar()
    {
        try
        {
            ucComboDocente.Doc_id = -1;
            ucComboDocente.PermiteEditar = false;

            ucComboCurriculoPeriodo.Valor = new[] { -1, -1, -1 };
            ucComboCurriculoPeriodo.PermiteEditar = false;

            ucComboUAEscola.CarregaEscolaPorUASuperiorSelecionada();

            if (ucComboUAEscola.Uad_ID != Guid.Empty)
            {
                ucComboUAEscola.FocoEscolas = true;
                ucComboUAEscola.PermiteAlterarCombos = true;
            }
            else
            {
                // Limpa o combo de cursos - carrega todos.ss
                ucComboCursoCurriculo.Valor = new[] { -1, -1, -1 };
                ucComboCursoCurriculo.CarregarCursoCurriculo();
            }

            ucComboUAEscola.SelectedValueEscolas = new[] { -1, -1 };
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Evento change do combo de Escola.
    /// </summary>
    private void UCFiltroEscolas1__SelecionarEscola()
    {
        try
        {
            ucComboDocente.Doc_id = -1;
            ucComboDocente.PermiteEditar = false;

            ucComboCurriculoPeriodo.Valor = new[] { -1, -1, -1 };
            ucComboCurriculoPeriodo.PermiteEditar = false;

            if (ucComboUAEscola.Esc_ID > 0)
            {
                ucComboCursoCurriculo.CarregarCursoCurriculoPorEscola(ucComboUAEscola.Esc_ID, ucComboUAEscola.Uni_ID, 0);
                ucComboCursoCurriculo.SetarFoco();

                // Carrega os docentes de acordo com a escola selecionada.
                ucComboDocente._CancelaSelect = false;
                ucComboDocente._Load_By_esc_uni_id(ucComboUAEscola.Esc_ID + ";" + ucComboUAEscola.Uni_ID, 1);
                ucComboDocente.PermiteEditar = true;
            }
            else
            {
                ucComboCursoCurriculo.CarregarCursoCurriculo();
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o(s) curso(s) da unidade escolar.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Evento change do combo de curso.
    /// </summary>
    private void _UCComboCursoCurriculo_IndexChanged()
    {
        try
        {
            if (ucComboCursoCurriculo.Cur_ID > 0)
            {
                // Carrega períodos.
                ucComboCurriculoPeriodo.CancelSelect = false;
                ucComboCurriculoPeriodo._MostrarMessageSelecione = true;
                ucComboCurriculoPeriodo._Load(ucComboCursoCurriculo.Valor[0], ucComboCursoCurriculo.Valor[1]);
                ucComboCurriculoPeriodo.PermiteEditar = true;
                ucComboCurriculoPeriodo.FocaCombo();

                ucComboCalendario.CarregarCalendarioAnualPorCurso(ucComboCursoCurriculo.Cur_ID);
            }
            else
            {
                ucComboCurriculoPeriodo.Valor = new[] { -1, -1, -1 };
                ucComboCurriculoPeriodo.PermiteEditar = false;

                ucComboCalendario.CarregarCalendarioAnual();
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o(s) período(s) do curso.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion Delegates

    #region Métodos

    /// <summary>
    /// Realiza a consulta com os filtros informados, e salva a busca realizada na sessão.
    /// </summary>
    public void Pesquisar(int pageIndex, bool alteraSessaoBusca)
    {
        try
        {
            grvTurma.DataSource = TUR_TurmaBO.GetSelectBy_Pesquisa_Tipo
                (
                    __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                    __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                    ucComboUAEscola.Esc_ID,
                    ucComboUAEscola.Uni_ID,
                    ucComboCalendario.Valor,
                    ucComboCursoCurriculo.Valor[0],
                    ucComboCursoCurriculo.Valor[1],
                    ucComboCurriculoPeriodo.Valor[2],
                    ucComboTurno.Valor,
                    ucComboDocente.Doc_id,
                    txtCodigoTurma.Text,
                    __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                    ucComboUAEscola.Uad_ID,
                    TUR_TurmaTipo.Normal,
                    UCComboQtdePaginacao1.Valor,
                    pageIndex,
                    (int)VS_SortDirection,
                    VS_Ordenacao
                );

            grvTurma.PageIndex = pageIndex;
            grvTurma.PageSize = UCComboQtdePaginacao1.Valor;
            grvTurma.VirtualItemCount = TUR_TurmaBO.GetTotalRecords();

            // Atualiza o grid
            grvTurma.DataBind();

            fdsResultado.Visible = true;

            #region Salvar busca realizada com os parâmetros do ODS.

            if (alteraSessaoBusca)
            {
                Dictionary<string, string> filtros = new Dictionary<string, string>();

                filtros.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
                filtros.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
                filtros.Add("esc_id", ucComboUAEscola.Esc_ID.ToString());
                filtros.Add("uni_id", ucComboUAEscola.Uni_ID.ToString());
                filtros.Add("cal_id", ucComboCalendario.Valor.ToString());
                filtros.Add("cur_id", ucComboCursoCurriculo.Valor[0].ToString());
                filtros.Add("crr_id", ucComboCursoCurriculo.Valor[1].ToString());
                filtros.Add("crp_id", ucComboCurriculoPeriodo.Valor[2].ToString());
                filtros.Add("trn_id", ucComboTurno.Valor.ToString());
                filtros.Add("doc_id", ucComboDocente.Doc_id.ToString());
                filtros.Add("tur_codigo", txtCodigoTurma.Text);
                filtros.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
                filtros.Add("uad_idSuperior", ucComboUAEscola.Uad_ID.ToString());

                __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.Turma, Filtros = filtros };
            }

            #endregion Salvar busca realizada com os parâmetros do ODS.
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as turmas.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Verifica se tem busca salva na sessão, e se tiver, recupera e realiza a consulta,
    /// colocando os filtros nos campos da tela.
    /// </summary>
    private void VerificaBusca()
    {
        if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Turma)
        {
            string valor;
            string valor2;
            string valor3;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor);

            if (!string.IsNullOrEmpty(valor))
            {
                ucComboUAEscola.Uad_ID = new Guid(valor);
                SelecionarEscola(ucComboUAEscola.FiltroEscola);
            }

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out valor);
            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out valor2);
            ucComboCursoCurriculo.Valor = new[] { Convert.ToInt32(valor), Convert.ToInt32(valor2) };
            _UCComboCursoCurriculo_IndexChanged();

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_id", out valor3);
            if (Convert.ToInt32(valor3) > 0)
                ucComboCurriculoPeriodo._Combo.SelectedValue = valor + ";" + valor2 + ";" + valor3;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor);
            ucComboCalendario.Valor = Convert.ToInt32(valor);

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("trn_id", out valor);
            ucComboTurno.Valor = Convert.ToInt32(valor);

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("doc_id", out valor);
            if (Convert.ToInt64(valor) > 0)
                ucComboDocente.Doc_id = Convert.ToInt64(valor);

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_codigo", out valor);
            txtCodigoTurma.Text = valor;
            txtCodigoTurma.Focus();

            Pesquisar(0, true);
        }
        else
        {
            UCFiltroEscolas1__SelecionarEscola();

            ucComboCalendario.CarregarCalendarioAnual();
        }
    }

    /// <summary>
    /// Inicializa os combos.
    /// </summary>
    public void Inicializar()
    {
        try
        {
            ucComboUAEscola.FocusUA();
            ucComboUAEscola.Inicializar();
            ucComboTurno.CarregarTurno();

            this.VerificaBusca();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Seleciona a escola no combo de acordo com o parâmetro salvo na sessão de busca
    /// realizada.
    /// </summary>
    /// <param name="filtroEscolas"></param>
    private void SelecionarEscola(bool filtroEscolas)
    {
        if (filtroEscolas)
        {
            UCFiltroEscolas1__Selecionar();
        }

        string esc_id;
        string uni_id;

        if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
            (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
        {
            ucComboUAEscola.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
            UCFiltroEscolas1__SelecionarEscola();
        }
    }

    #endregion Métodos

    #region Eventos

    protected void grvTurma_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Pesquisar(e.NewPageIndex, false);
    }

    protected void grvTurma_Sorting(object sender, GridViewSortEventArgs e)
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
                PaginaBusca = PaginaGestao.Turma
                ,
                Filtros = filtros
            };
        }

        Pesquisar(grid.PageIndex, false);
    }

    protected void grvTurma_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = TUR_TurmaBO.GetTotalRecords();

        // Seta propriedades necessárias para ordenação nas colunas.
        ConfiguraColunasOrdenacao(grvTurma, VS_Ordenacao, VS_SortDirection);
    }
    
    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        Pesquisar(0, true);
    }

    protected void btnLimparPesquisa_Click(object sender, EventArgs e)
    {
        // Inicializa variável de sessão.
        __SessionWEB.BuscaRealizada = new BuscaGestao();
        Response.Redirect("Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    #endregion Eventos
}
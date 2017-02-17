using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using System.Linq;

public partial class Classe_ReunioesResponsaveisFrequencia_Busca : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Propriedade na qual cria variavel em ViewState armazenando valor de tur_id
    /// </summary>
    private int _VS_tur_id
    {
        get
        {
            if (ViewState["_VS_tur_id"] != null)
                return Convert.ToInt32(ViewState["_VS_tur_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_tur_id"] = value;
        }
    }

    /// <summary>
    /// Propriedade na qual cria variavel em ViewState armazenando valor de doc_id
    /// </summary>
    private long _VS_doc_id
    {
        get
        {
            if (ViewState["_VS_doc_id"] != null)
                return Convert.ToInt64(ViewState["_VS_doc_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_doc_id"] = value;
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada
    /// </summary>
    private string VS_Ordenacao
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Reunioes_Responsaveis_Frequencia)
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
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Reunioes_Responsaveis_Frequencia)
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
    /// Parâmetro acadêmico que indica se o cadastro de reunião de responáveis é por período do calendário.
    /// </summary>
    private bool cadastroReunioesPorPeriodo
    {
        get
        {
            return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CADASTRAR_REUNIOES_POR_PERIODO_CALENDARIO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }
    }

    /// <summary>
    /// ViewState que armazena o valor do id do período do calendário.
    /// </summary>
    private int VS_cap_id
    {
        get
        {
            return Convert.ToInt32(ViewState["VS_cap_id"] ?? "-1");
        }

        set
        {
            ViewState["VS_cap_id"] = value;
        }
    }

    private string _nomeModulo;

    /// <summary>
    /// Propriedade com o nome do modulo.
    /// </summary>
    private string NomeModulo
    {
        get
        {
            try
            {
                if (string.IsNullOrEmpty(_nomeModulo))
                {
                    SYS_Modulo entModulo;
                    if (Modulo.IsNew)
                    {
                        entModulo = new SYS_Modulo
                        {
                            mod_id = __SessionWEB.__UsuarioWEB.GrupoPermissao.mod_id,
                            sis_id = __SessionWEB.__UsuarioWEB.GrupoPermissao.sis_id
                        };
                        entModulo = GestaoEscolarUtilBO.GetEntityModuloCache(entModulo);
                    }
                    else
                    {
                        entModulo = Modulo;
                    }

                    _nomeModulo = string.IsNullOrEmpty(entModulo.mod_nome) ? "Frequência em reunião de responsáveis " : entModulo.mod_nome;
                }

                return _nomeModulo;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);

                return "Frequência em reunião de responsáveis";
            }
        }
    }

    #endregion Propriedades

    #region Constantes

    private int COLUNA_CURSO = 3;

    #endregion Constantes

    #region Delegates

    protected void UCComboQtdePaginacao_IndexChanged()
    {
        // atribui nova quantidade itens por página para o grid
        _dgvTurma.PageSize = UCComboQtdePaginacao1.Valor;
        _dgvTurma.PageIndex = 0;
        // atualiza o grid
        _dgvTurma.DataBind();
    }

    private void UCFiltroEscolas1__Selecionar()
    {
        try
        {
            UCFiltroEscolas1._ComboUnidadeEscola.SelectedValue = "-1;-1";
            UCFiltroEscolas1._ComboUnidadeEscola.Enabled = false;
            _UCComboCursoCurriculo.Valor = new[] { -1, -1 };
            _UCComboCalendario.Valor = -1;

            _UCComboCursoCurriculo.CarregarCursoCurriculo();
            _UCComboCalendario.CarregarCalendarioAnual();

            if (UCFiltroEscolas1._VS_FiltroEscola && UCFiltroEscolas1._ComboUnidadeAdministrativa.SelectedValue != Guid.Empty.ToString())
            {
                UCFiltroEscolas1._UnidadeEscola_LoadBy_uad_idSuperior(new Guid(UCFiltroEscolas1._ComboUnidadeAdministrativa.SelectedValue));
                UCFiltroEscolas1._ComboUnidadeEscola.Enabled = true;
                UCFiltroEscolas1._ComboUnidadeEscola.Focus();
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    private void UCFiltroEscolas1__SelecionarEscola()
    {
        try
        {
            _UCComboCursoCurriculo.Valor = new[] { -1, -1 };
            _UCComboCalendario.Valor = -1;

            _UCComboCursoCurriculo.CarregarCursoCurriculo();
            _UCComboCalendario.CarregarCalendarioAnual();

            if (UCFiltroEscolas1._UCComboUnidadeEscola_Esc_ID > 0)
            {
                _UCComboCursoCurriculo.CarregarCursoCurriculoPorEscola(UCFiltroEscolas1._UCComboUnidadeEscola_Esc_ID, UCFiltroEscolas1._UCComboUnidadeEscola_Uni_ID, 0);
                _UCComboCursoCurriculo.SetarFoco();
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o(s) curso(s) da unidade escolar.", UtilBO.TipoMensagem.Erro);
        }
    }

    private void _UCComboCursoCurriculo_IndexChanged()
    {
        try
        {
            if (_UCComboCursoCurriculo.Valor[0] > 0)
            {
                _UCComboCalendario.CarregarCalendarioAnualPorCurso(_UCComboCursoCurriculo.Valor[0]);
                _UCComboCalendario.SetarFoco();
            }
            else
            {
                _UCComboCalendario.CarregarCalendarioAnual();
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o(s) período(s) do curso.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion Delegates

    #region Métodos

    /// <summary>
    /// O método realiza a busca de turmas para lançamento de frequência em reunião de pais e responsáveis.
    /// </summary>
    private void _Pesquisar()
    {
        try
        {
            fdsResultado.Visible = true;

            Dictionary<string, string> filtros = new Dictionary<string, string>();

            _dgvTurma.PageIndex = 0;
            _odsTurma.SelectParameters.Clear();

            _odsTurma.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
            _odsTurma.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
            _odsTurma.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
            _odsTurma.SelectParameters.Add("uad_idSuperior", UCFiltroEscolas1._ComboUnidadeAdministrativa.SelectedValue);
            _odsTurma.SelectParameters.Add("esc_id", UCFiltroEscolas1._ComboUnidadeEscola.SelectedValue.Split(';')[0]);
            _odsTurma.SelectParameters.Add("uni_id", UCFiltroEscolas1._ComboUnidadeEscola.SelectedValue.Split(';')[1]);
            _odsTurma.SelectParameters.Add("cur_id", _UCComboCursoCurriculo.Valor[0].ToString());
            _odsTurma.SelectParameters.Add("crr_id", _UCComboCursoCurriculo.Valor[1].ToString());
            _odsTurma.SelectParameters.Add("cal_id", _UCComboCalendario.Valor.ToString());
            _odsTurma.SelectParameters.Add("trn_id", _UCComboTurno.Valor.ToString());
            _odsTurma.SelectParameters.Add("tur_codigo", _txtCodigoTurma.Text);
            _odsTurma.SelectParameters.Add("doc_id", _VS_doc_id.ToString());
            _odsTurma.SelectParameters.Add("adm", (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao).ToString());

            // atualiza o grid
            _dgvTurma.Sort(VS_Ordenacao, VS_SortDirection);

            #region Salvar busca realizada com os parâmetros do ODS.

            foreach (Parameter param in _odsTurma.SelectParameters)
            {
                filtros.Add(param.Name, param.DefaultValue);
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.Reunioes_Responsaveis_Frequencia
                ,
                Filtros = filtros
            };

            #endregion Salvar busca realizada com os parâmetros do ODS.

            _dgvTurma.DataBind();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as turmas.", UtilBO.TipoMensagem.Erro);
            _updBuscaFrequencia.Update();
        }
    }

    /// <summary>
    /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
    /// </summary>
    private void VerificaBusca()
    {
        if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Reunioes_Responsaveis_Frequencia)
        {
            string valor;
            string valor2;

            if (UCFiltroEscolas1._VS_FiltroEscola)
            {
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor);

                if (!string.IsNullOrEmpty(valor))
                {
                    UCFiltroEscolas1._ComboUnidadeAdministrativa.SelectedValue = valor;
                }

                if (valor != Guid.Empty.ToString())
                {
                    SelecionarEscola(UCFiltroEscolas1._VS_FiltroEscola);
                }
            }
            else
            {
                SelecionarEscola(UCFiltroEscolas1._VS_FiltroEscola);
            }

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out valor);
            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out valor2);
            _UCComboCursoCurriculo.Valor = new[] { Convert.ToInt32(valor), Convert.ToInt32(valor2) };
            _UCComboCursoCurriculo_IndexChanged();

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor);
            _UCComboCalendario.Valor = Convert.ToInt32(valor);

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("trn_id", out valor);
            _UCComboTurno.Valor = Convert.ToInt32(valor);

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("doc_id", out valor);
            _VS_doc_id.ToString();

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_codigo", out valor);
            _txtCodigoTurma.Text = valor;

            _Pesquisar();
        }
        else
        {
            fdsResultado.Visible = false;
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
            // Não selecionar valores do combo automaticamente.
            UCFiltroEscolas1.SelecionaCombosAutomatico = false;

            UCFiltroEscolas1._ComboUnidadeEscola.SelectedValue = esc_id + ";" + uni_id;

            UCFiltroEscolas1__SelecionarEscola();
        }
    }

    /// <summary>
    /// O método redireciona para a tela de lançamento de frequência em reuniões de pais e responsáveis por calendário.
    /// </summary>
    private void _ChamarCadastro()
    {
        try
        {
            Session["tur_idFrequencia"] = _VS_tur_id.ToString();
            Response.Redirect("~/Classe/ReunioesResponsaveisFrequencia/Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar entrar na página de cadastro.", UtilBO.TipoMensagem.Erro);
            _updBuscaFrequencia.Update();
        }
    }

    /// <summary>
    /// O método redireciona para a tela de lançamento de frequência em reuniões de pais e responsáveis por período do calendário.
    /// </summary>
    private void RedirecionaCadastroPorPeriodo()
    {
        try
        {
            Session["tur_idFrequencia"] = _VS_tur_id.ToString();
            Session["cap_idFrequencia"] = VS_cap_id.ToString();
            Response.Redirect("~/Classe/ReunioesResponsaveisFrequencia/Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar entrar na página de cadastro.", UtilBO.TipoMensagem.Erro);
            _updBuscaFrequencia.Update();
        }
    }

    #endregion Métodos

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference("~/Includes/jsBuscaPlanejamento.js"));
        }

        UCComboQtdePaginacao1.GridViewRelacionado = _dgvTurma;

        ScriptManager.RegisterStartupScript(_updBuscaFrequencia, typeof(UpdatePanel), fdsPesquisa.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsPesquisa.ClientID)), true);

        if (!IsPostBack)
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
                _lblMessage.Text = message;

            try
            {
                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                // mostra essa quantidade no combobox
                UCComboQtdePaginacao1.Valor = itensPagina;
                // atribui essa quantidade para os grid
                _dgvTurma.PageSize = itensPagina;

                _UCComboCalendario.CarregarCalendarioAnual();
                _UCComboCursoCurriculo.CarregarCursoCurriculo();
                _UCComboTurno.CarregarTurno();

                UCFiltroEscolas1._cvUnidadeAdministrativa.Enabled = false;
                UCFiltroEscolas1._cvUnidadeEscola.Enabled = false;
                UCFiltroEscolas1._LoadInicial();

                Page.Form.DefaultButton = _btnPesquisar.UniqueID;
                _dgvTurma.Columns[COLUNA_CURSO].HeaderText = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id);

                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                {
                    fdsPesquisa.Visible = false;

                    if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                    {
                        fdsResultado.Visible = true;
                        ldgResultado.Visible = false;
                        ldgResultadoListagem.Visible = true;

                        _VS_doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                        _Pesquisar();
                    }
                    else
                    {
                        fdsResultado.Visible = false;
                        _lblMessage.Text = UtilBO.GetErroMessage("Essa tela é exclusiva para docentes.", UtilBO.TipoMensagem.Alerta);
                    }
                }
                else
                {
                    fdsPesquisa.Visible = true;
                    fdsResultado.Visible = false;
                    ldgResultado.Visible = true;
                    ldgResultadoListagem.Visible = false;

                    Page.Form.DefaultFocus = UCFiltroEscolas1._VS_FiltroEscola ? UCFiltroEscolas1._ComboUnidadeAdministrativa.ClientID : UCFiltroEscolas1._ComboUnidadeEscola.ClientID;

                    VerificaBusca();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                _updBuscaFrequencia.Update();
            }

            _divPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnPesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnLimparPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
        }

        UCFiltroEscolas1._Selecionar += UCFiltroEscolas1__Selecionar;
        UCFiltroEscolas1._SelecionarEscola += UCFiltroEscolas1__SelecionarEscola;
        _UCComboCursoCurriculo.IndexChanged += _UCComboCursoCurriculo_IndexChanged;
    }

    protected void _btnPesquisar_Click(object sender, EventArgs e)
    {
        _Pesquisar();
        fdsResultado.Visible = true;
        _updBuscaFrequencia.Update();
    }

    protected void _btnLimparPesquisa_Click(object sender, EventArgs e)
    {
        // Limpa busca da sessão.
        __SessionWEB.BuscaRealizada = new BuscaGestao();
        Response.Redirect("Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void _dgvTurma_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = TUR_TurmaBO.GetTotalRecords();
        ConfiguraColunasOrdenacao(_dgvTurma);

        if ((!string.IsNullOrEmpty(_dgvTurma.SortExpression)) &&
        (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Reunioes_Responsaveis_Frequencia))
        {
            Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

            if (filtros.ContainsKey("VS_Ordenacao"))
            {
                filtros["VS_Ordenacao"] = _dgvTurma.SortExpression;
            }
            else
            {
                filtros.Add("VS_Ordenacao", _dgvTurma.SortExpression);
            }

            if (filtros.ContainsKey("VS_SortDirection"))
            {
                filtros["VS_SortDirection"] = _dgvTurma.SortDirection.ToString();
            }
            else
            {
                filtros.Add("VS_SortDirection", _dgvTurma.SortDirection.ToString());
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.Reunioes_Responsaveis_Frequencia
                ,
                Filtros = filtros
            };
        }
    }

    protected void _dgvTurma_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    //seta o valor da palavra curso a ser utilizada no grid
        //    e.Row.Cells[COLUNA_CURSO].Text = GestaoEscolarUtilBO.nomePadraoCurso();
        //}
        //else
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label _lblAlterar = (Label)e.Row.FindControl("_lblAlterar");
            if (_lblAlterar != null)
            {
                _lblAlterar.Visible = !__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            }

            LinkButton _btnAlterar = (LinkButton)e.Row.FindControl("_btnAlterar");
            if (_btnAlterar != null)
            {
                _btnAlterar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                _btnAlterar.CommandArgument = e.Row.RowIndex.ToString();
            }
        }
    }

    protected void _dgvTurma_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Selecionar")
        {
            int index = int.Parse(e.CommandArgument.ToString());
            _VS_tur_id = Convert.ToInt32(_dgvTurma.DataKeys[index].Values["tur_id"]);

            if (string.IsNullOrEmpty(_dgvTurma.DataKeys[index].Values["fav_id"].ToString()))
                _lblMessage.Text = UtilBO.GetErroMessage("É necessário selecionar uma turma que possua um formato de avaliação.", UtilBO.TipoMensagem.Alerta);

            if (cadastroReunioesPorPeriodo)
            {
                int cal_id = Convert.ToInt32(_dgvTurma.DataKeys[index].Values["cal_id"]);

                DataTable dtAvaliacoes;

                dtAvaliacoes = ACA_TipoPeriodoCalendarioBO.SelecionaTodosPor_EventoEfetivacao(cal_id, -1, _VS_tur_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                if (dtAvaliacoes.Rows.Count == 0)
                    _lblMessage.Text = UtilBO.GetErroMessage("Turma fora do período de " + NomeModulo + ".", UtilBO.TipoMensagem.Alerta);
                else if (dtAvaliacoes.Rows.Count == 1)
                {
                    VS_cap_id = Convert.ToInt32(dtAvaliacoes.Rows[0]["cap_id"]);

                    RedirecionaCadastroPorPeriodo();
                }
                else if (dtAvaliacoes.Rows.Count > 1)
                {
                    DataView dv = dtAvaliacoes.DefaultView;

                    dv.Sort = "tpc_ordem Asc";

                    dtAvaliacoes = dv.ToTable();

                    // Carregar Avaliações.
                    gvAvaliacoes.DataSource = dtAvaliacoes;
                    gvAvaliacoes.DataBind();

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "abreAvaliacoes", "$(document).ready(function(){$('#divAvaliacoes').dialog('open'); });", true);
                }
            }
            else
            {
                _ChamarCadastro();
            }
        }
    }

    protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("SelecionarAvaliacao"))
            {
                VS_cap_id = Convert.ToInt32(e.CommandArgument);

                RedirecionaCadastroPorPeriodo();
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion Eventos
}
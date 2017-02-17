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

public partial class Classe_EfetivacaoGestor_Busca : MotherPageLogado
{
    #region Delegates

    protected void UCComboQtdePaginacao_IndexChanged()
    {
        try
        {
            VS_cancelSelect = false;
            _odsEscola.SelectMethod = SelectMethod;

            // atribui nova quantidade itens por página para o grid
            _dgvEscola.PageSize = UCComboQtdePaginacao1.Valor;
            _dgvEscola.PageIndex = 0;
            // atualiza o grid
            _dgvEscola.DataBind();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as escolas.", UtilBO.TipoMensagem.Erro);
        }
    }

    private void UCBuscaLancamentoClasse1_OnPesquisar()
    {
        PesquisarEscolas();
    }

    #endregion Delegates

    #region Propriedades

    /// <summary>
    /// Retorna o método da busca, de acordo com a visão do usuário
    /// </summary>
    private string SelectMethod
    {
        get
        {
            return __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual ? "GetSelectBy_Docente_Efetivacao_TodosTipos" : "GetSelectBy_Pesquisa_TodosTipos";
        }
    }

    /// <summary>
    /// Propriedade na qual cria variavel em ViewState armazenando valor de esc_id
    /// </summary>
    public int _VS_esc_id
    {
        get
        {
            if (ViewState["_VS_esc_id"] != null)
                return Convert.ToInt32(ViewState["_VS_esc_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_esc_id"] = value;
        }
    }

    /// <summary>
    /// Propriedade na qual cria variavel em ViewState armazenando valor de cal_id
    /// </summary>
    public int _VS_cal_id
    {
        get
        {
            if (ViewState["_VS_cal_id"] != null)
                return Convert.ToInt32(ViewState["_VS_cal_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_cal_id"] = value;
        }
    }

    /// <summary>
    /// Propriedade na qual cria variavel em ViewState armazenando valor de uni_id
    /// </summary>
    public int _VS_uni_id
    {
        get
        {
            if (ViewState["_VS_uni_id"] != null)
                return Convert.ToInt32(ViewState["_VS_uni_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_uni_id"] = value;
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

                    _nomeModulo = string.IsNullOrEmpty(entModulo.mod_nome) ? "Efetivação de notas " : entModulo.mod_nome;
                }

                return _nomeModulo;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);

                return "Efetivação de notas";
            }
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada
    /// </summary>
    private string VS_Ordenacao
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Efetivacao)
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
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Efetivacao)
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
    /// Cancela o consulta do ObjectDataSource ao carregar a página pela primeira vez.
    /// </summary>
    private bool VS_cancelSelect
    {
        get
        {
            return Convert.ToBoolean(ViewState["VS_cancelSelect"] ?? false);
        }

        set
        {
            ViewState["VS_cancelSelect"] = value;
        }
    }

    #endregion Propriedades

    #region Métodos

    /// <summary>
    /// Atualiza o grid de escolas com os filtros informados.
    /// </summary>
    private void PesquisarEscolas()
    {
        try
        {
            VS_cancelSelect = false;

            Dictionary<string, string> filtros = new Dictionary<string, string>();

            _dgvEscola.PageIndex = 0;
            _odsEscola.SelectParameters.Clear();
            _odsEscola.SelectMethod = SelectMethod;

            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Individual)
            {
                _odsEscola.SelectParameters.Add("uad_idSuperior", UCBuscaLancamentoClasse1.UadIdSuperior.ToString());
                _odsEscola.SelectParameters.Add("esc_id", UCBuscaLancamentoClasse1.EscId.ToString());
                _odsEscola.SelectParameters.Add("uni_id", UCBuscaLancamentoClasse1.UniId.ToString());
                _odsEscola.SelectParameters.Add("cal_id", UCBuscaLancamentoClasse1.CalId.ToString());
                _odsEscola.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
                _odsEscola.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
                _odsEscola.SelectParameters.Add("adm", (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao).ToString());
            }

            _odsEscola.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
            _odsEscola.SelectParameters.Add("doc_id", _VS_doc_id.ToString());

            // atualiza o grid
            _dgvEscola.Sort(VS_Ordenacao, VS_SortDirection);

            #region Salvar busca realizada com os parâmetros do ODS.

            foreach (Parameter param in _odsEscola.SelectParameters)
            {
                filtros.Add(param.Name, param.DefaultValue);
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.Efetivacao
                ,
                Filtros = filtros
            };

            #endregion Salvar busca realizada com os parâmetros do ODS.

            _dgvEscola.DataBind();

            pnlResultado.Visible = true;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as escolas.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Seta as variáveis necessárias e redireciona pro cadastro.
    /// </summary>
    private void RedirecionaCadastro(int esc_id, int uni_id, int cal_id)
    {
        Session["esc_idEfetivacao"] = esc_id;
        Session["uni_idEfetivacao"] = uni_id;
        Session["cal_idEfetivacao"] = cal_id;
        Session["URL_Retorno_Efetivacao"] = Convert.ToByte(URL_Retorno_Efetivacao.EfetivacaoBusca);

        Response.Redirect("~/Classe/EfetivacaoGestor/Cadastro.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    #endregion Métodos

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        VS_cancelSelect = true;

        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference("~/Includes/jsBuscaEfetivacao.js"));
        }

        UCBuscaLancamentoClasse1.OnPesquisar += UCBuscaLancamentoClasse1_OnPesquisar;

        UCComboQtdePaginacao1.GridViewRelacionado = _dgvEscola;

        if (!IsPostBack)
        {
            UCBuscaLancamentoClasse1.PaginaBusca = PaginaGestao.Efetivacao;
            UCBuscaLancamentoClasse1.GroupingText = "Consulta de " + NomeModulo;

            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
            {
                _lblMessage.Text = message;
            }

            try
            {
                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                // mostra essa quantidade no combobox
                UCComboQtdePaginacao1.Valor = itensPagina;
                // atribui essa quantidade para o grid
                _dgvEscola.PageSize = itensPagina;

                pnlResultado.Visible = false;
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                {
                    UCBuscaLancamentoClasse1.Visible = false;

                    // Busca o doc_id do usuário logado.
                    if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                    {
                        pnlResultado.Visible = true;
                        _VS_doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                        PesquisarEscolas();
                    }
                    else
                    {
                        pnlResultado.Visible = false;
                        _lblMessage.Text = UtilBO.GetErroMessage("Essa tela é exclusiva para docentes.", UtilBO.TipoMensagem.Alerta);
                    }
                }
                else
                {
                    UCBuscaLancamentoClasse1.Inicializar();
                    UCBuscaLancamentoClasse1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }

            Page.Form.DefaultButton = UCBuscaLancamentoClasse1.Pesquisar_UniqueID;
        }
    }

    protected void _dgvEscola_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        VS_cancelSelect = false;
        _odsEscola.SelectMethod = SelectMethod;
    }

    protected void _dgvEscola_Sorting(object sender, GridViewSortEventArgs e)
    {
        VS_cancelSelect = false;
        _odsEscola.SelectMethod = SelectMethod;
    }

    protected void _dgvEscola_DataBound(object sender, EventArgs e)
    {
        UCComboQtdePaginacao1.Visible = !_dgvEscola.Rows.Count.Equals(0);

        UCTotalRegistros1.Total = ESC_EscolaBO.GetTotalRecords();
        ConfiguraColunasOrdenacao(_dgvEscola);

        if ((!string.IsNullOrEmpty(_dgvEscola.SortExpression)) &&
         (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Efetivacao))
        {
            Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

            if (filtros.ContainsKey("VS_Ordenacao"))
            {
                filtros["VS_Ordenacao"] = _dgvEscola.SortExpression;
            }
            else
            {
                filtros.Add("VS_Ordenacao", _dgvEscola.SortExpression);
            }

            if (filtros.ContainsKey("VS_SortDirection"))
            {
                filtros["VS_SortDirection"] = _dgvEscola.SortDirection.ToString();
            }
            else
            {
                filtros.Add("VS_SortDirection", _dgvEscola.SortDirection.ToString());
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.Efetivacao
                ,
                Filtros = filtros
            };
        }

        if (_dgvEscola.Rows.Count.Equals(1))
        {
            RedirecionaCadastro(Convert.ToInt32(_dgvEscola.DataKeys[0].Values["esc_id"])
                                    , Convert.ToInt32(_dgvEscola.DataKeys[0].Values["uni_id"])
                                    , Convert.ToInt32(_dgvEscola.DataKeys[0].Values["cal_id"]));
        }
    }

    protected void _dgvEscola_RowDataBound(object sender, GridViewRowEventArgs e)
    {
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
                _btnAlterar.Attributes.Add("onClick", "TopoDaPagina()");
                if (!Convert.ToString(_btnAlterar.CssClass).Contains("subir"))
                {
                    _btnAlterar.CssClass += " subir";
                }
            }
        }
    }

    protected void _dgvEscola_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Selecionar")
        {
            try
            {
                VS_cancelSelect = false;
                GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                int index = row.RowIndex;
                RedirecionaCadastro(Convert.ToInt32(_dgvEscola.DataKeys[index].Values["esc_id"])
                                    , Convert.ToInt32(_dgvEscola.DataKeys[index].Values["uni_id"])
                                    , Convert.ToInt32(_dgvEscola.DataKeys[index].Values["cal_id"]));
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar selecionar escola.", UtilBO.TipoMensagem.Erro);
            }
        }
    }

    protected void _odsEscola_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.Cancel = VS_cancelSelect;
        if (!e.Cancel)
        {
            _odsEscola.SelectMethod = SelectMethod;
        }
    }

    #endregion Eventos
}
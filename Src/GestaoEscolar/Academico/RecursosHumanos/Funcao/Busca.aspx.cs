using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using MSTech.Validation.Exceptions;
using System.Collections.Generic;
using System.Web;

public partial class Academico_RecursosHumanos_Funcao_Busca : MotherPageLogado
{
    #region Propriedades

    public int EditItem
    {
        get
        {
            return Convert.ToInt32(_dgvFuncao.DataKeys[_dgvFuncao.EditIndex].Value);
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada
    /// </summary>
    private string VS_Ordenacao
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Funcoes)
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
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Funcoes)
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

    #endregion

    #region Delegates

    protected void UCComboQtdePaginacao_IndexChanged()
    {
        // atribui nova quantidade itens por página para o grid
        _dgvFuncao.PageSize = UCComboQtdePaginacao1.Valor;
        _dgvFuncao.PageIndex = 0;
        _dgvFuncao.Sort("", SortDirection.Ascending);
        // atualiza o grid
        _dgvFuncao.DataBind();
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Realiza a consulta com os filtros informados, e salva a busca realizada na sessão.
    /// </summary>
    private void _Pesquisar()
    {
        try
        {
            Dictionary<string, string> filtros = new Dictionary<string, string>();
            _dgvFuncao.PageIndex = 0;
            _odsFuncao.SelectParameters.Clear();
            _odsFuncao.SelectParameters.Add("fun_nome", _txtFuncao.Text);
            _odsFuncao.SelectParameters.Add("fun_codigo", _txtCodigo.Text);
            _odsFuncao.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
            _odsFuncao.DataBind();

            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            _dgvFuncao.Sort(VS_Ordenacao, VS_SortDirection);

            #region Salvar busca realizada com os parâmetros do ODS.

            foreach (Parameter param in _odsFuncao.SelectParameters)
            {
                filtros.Add(param.Name, param.DefaultValue);
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.Funcoes
                ,
                Filtros = filtros
            };

            #endregion

            // mostra essa quantidade no combobox
            UCComboQtdePaginacao1.Valor = itensPagina;
            // atribui essa quantidade para o grid
            _dgvFuncao.PageSize = itensPagina;

            // atualiza o grid
            _dgvFuncao.DataBind();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as funções.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
    /// </summary>
    private void VerificaBusca()
    {
        if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Funcoes)
        {
            // Recuperar busca realizada e pesquisar automaticamente
            string valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("fun_codigo", out valor);
            _txtCodigo.Text = valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("fun_nome", out valor);
            _txtFuncao.Text = valor;

            _Pesquisar();
        }
        else
        {
            fdsResultados.Visible = false;
        }
    }

    #endregion

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
        }

        if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), fdsFuncao.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsFuncao.ClientID)), true);
        }

        UCComboQtdePaginacao1.GridViewRelacionado = _dgvFuncao;

        if (!IsPostBack)
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
                _lblMessage.Text = message;

            _dgvFuncao.PageSize = ApplicationWEB._Paginacao;

            VerificaBusca();

            Page.Form.DefaultFocus = _txtFuncao.ClientID;
            Page.Form.DefaultButton = _btnPesquisar.UniqueID;

            _divPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnPesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnLimparPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
        }
    }

    protected void _dgvFuncao_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = RHU_FuncaoBO.GetTotalRecords();
        // Seta propriedades necessárias para ordenação nas colunas.
        ConfiguraColunasOrdenacao(_dgvFuncao);

        if ((!string.IsNullOrEmpty(_dgvFuncao.SortExpression)) &&
            (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Funcoes))
        {
            Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

            if (filtros.ContainsKey("VS_Ordenacao"))
            {
                filtros["VS_Ordenacao"] = _dgvFuncao.SortExpression;
            }
            else
            {
                filtros.Add("VS_Ordenacao", _dgvFuncao.SortExpression);
            }

            if (filtros.ContainsKey("VS_SortDirection"))
            {
                filtros["VS_SortDirection"] = _dgvFuncao.SortDirection.ToString();
            }
            else
            {
                filtros.Add("VS_SortDirection", _dgvFuncao.SortDirection.ToString());
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.Funcoes
                ,
                Filtros = filtros
            };
        }
    }

    protected void _dgvFuncao_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label _lblAlterar = (Label)e.Row.FindControl("_lblAlterar");
            if (_lblAlterar != null)
            {
                _lblAlterar.Visible = !__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
            }

            LinkButton _btnAlterar = (LinkButton)e.Row.FindControl("_btnAlterar");
            if (_btnAlterar != null)
            {
                _btnAlterar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
            }

            ImageButton _btnExcluir = (ImageButton)e.Row.FindControl("_btnExcluir");
            if (_btnExcluir != null)
            {
                _btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                _btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
            }
        }
    }

    protected void _dgvFuncao_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Deletar")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());
                int fun_id = Convert.ToInt32(_dgvFuncao.DataKeys[index].Value);

                RHU_Funcao entity = new RHU_Funcao { fun_id = fun_id };
                RHU_FuncaoBO.GetEntity(entity);

                if (RHU_FuncaoBO.Delete(entity))
                {
                    _dgvFuncao.PageIndex = 0;
                    _dgvFuncao.Sort("", SortDirection.Ascending);
                    _dgvFuncao.DataBind();
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "fun_id: " + fun_id);
                    _lblMessage.Text = UtilBO.GetErroMessage("Função excluída com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir a função.", UtilBO.TipoMensagem.Erro);
            }
        }
    }

    protected void _btnLimparPesquisa_Click(object sender, EventArgs e)
    {
        // Limpa busca da sessão.
        __SessionWEB.BuscaRealizada = new BuscaGestao();
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/RecursosHumanos/Funcao/Busca.aspx", false);
    }

    protected void _btnNovo_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/RecursosHumanos/Funcao/Cadastro.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void _btnPesquisar_Click(object sender, EventArgs e)
    {
        fdsResultados.Visible = true;
        _Pesquisar();
    }

    #endregion
}

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using System.Collections.Generic;
using System.Web;

public partial class Academico_FormatoAvaliacao_Busca : MotherPageLogado
{
    #region Propriedades

    public int EditItem
    {
        get
        {
            return Convert.ToInt32(_dgvFormatoAvaliacao.DataKeys[_dgvFormatoAvaliacao.EditIndex].Value);
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada
    /// </summary>
    private string VS_Ordenacao
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.FormatoAvaliacao)
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
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.FormatoAvaliacao)
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
        _dgvFormatoAvaliacao.PageSize = UCComboQtdePaginacao1.Valor;
        _dgvFormatoAvaliacao.PageIndex = 0;
        // atualiza o grid
        _dgvFormatoAvaliacao.DataBind();
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Realiza a consulta com os filtros informados, e salva a busca realizada na sessão.
    /// </summary>
    public void _Pesquisar()
    {
        try
        {
            Dictionary<string, string> filtros = new Dictionary<string, string>();
            _dgvFormatoAvaliacao.Sort("", SortDirection.Ascending);
            _dgvFormatoAvaliacao.PageIndex = 0;
            odsFormatoAvaliacao.SelectParameters.Clear();
            odsFormatoAvaliacao.SelectParameters.Add("esc_uni_id", "-1;-1");
            odsFormatoAvaliacao.SelectParameters.Add("fav_nome", _txtNome.Text);
            odsFormatoAvaliacao.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
            odsFormatoAvaliacao.DataBind();

            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            _dgvFormatoAvaliacao.Sort(VS_Ordenacao, VS_SortDirection);

            #region Salvar busca realizada com os parâmetros do ODS.

            foreach (Parameter param in odsFormatoAvaliacao.SelectParameters)
            {
                filtros.Add(param.Name, param.DefaultValue);
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.FormatoAvaliacao
                ,
                Filtros = filtros
            };

            #endregion

            // mostra essa quantidade no combobox
            UCComboQtdePaginacao1.Valor = itensPagina;
            // atribui essa quantidade para o grid
            _dgvFormatoAvaliacao.PageSize = itensPagina;
            // atualiza o grid
            _dgvFormatoAvaliacao.DataBind();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os formatos de avaliação.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
    /// </summary>
    private void VerificaBusca()
    {
        if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.FormatoAvaliacao)
        {
            // Recuperar busca realizada e pesquisar automaticamente
            string valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("fav_nome", out valor);
            _txtNome.Text = valor;

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

        // Page.ClientScript.RegisterStartupScript(GetType(), fdsFormatoAvaliacao.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsFormatoAvaliacao.ClientID)), true);

        UCComboQtdePaginacao1.GridViewRelacionado = _dgvFormatoAvaliacao;

        if (!IsPostBack)
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
                _lblMessage.Text = message;

            _dgvFormatoAvaliacao.PageSize = ApplicationWEB._Paginacao;

            try
            {
                VerificaBusca();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }

            Page.Form.DefaultButton = _btnPesquisar.UniqueID;
            Page.Form.DefaultFocus = _txtNome.ClientID;

            _divPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnPesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnLimparPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
        }
    }

    protected void _dgvFormatoAvaliacao_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = ACA_FormatoAvaliacaoBO.GetTotalRecords();
        // Seta propriedades necessárias para ordenação nas colunas.
        ConfiguraColunasOrdenacao(_dgvFormatoAvaliacao);

        if ((!string.IsNullOrEmpty(_dgvFormatoAvaliacao.SortExpression)) &&
           (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.FormatoAvaliacao))
        {
            Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

            if (filtros.ContainsKey("VS_Ordenacao"))
            {
                filtros["VS_Ordenacao"] = _dgvFormatoAvaliacao.SortExpression;
            }
            else
            {
                filtros.Add("VS_Ordenacao", _dgvFormatoAvaliacao.SortExpression);
            }

            if (filtros.ContainsKey("VS_SortDirection"))
            {
                filtros["VS_SortDirection"] = _dgvFormatoAvaliacao.SortDirection.ToString();
            }
            else
            {
                filtros.Add("VS_SortDirection", _dgvFormatoAvaliacao.SortDirection.ToString());
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.FormatoAvaliacao
                ,
                Filtros = filtros
            };
        }
    }

    protected void _dgvFormatoAvaliacao_RowDataBound(object sender, GridViewRowEventArgs e)
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
        }
    }

    protected void _btnLimparPesquisa_Click(object sender, EventArgs e)
    {
        // Limpa busca da sessão.
        __SessionWEB.BuscaRealizada = new BuscaGestao();
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/FormatoAvaliacao/Busca.aspx", false);
    }

    protected void _btnPesquisar_Click(object sender, EventArgs e)
    {
        fdsResultados.Visible = true;
        _Pesquisar();
    }

    #endregion
}

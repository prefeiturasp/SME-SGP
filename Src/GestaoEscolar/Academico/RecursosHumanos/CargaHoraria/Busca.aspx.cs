using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using MSTech.Validation.Exceptions;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class Academico_RecursosHumanos_CargaHoraria_Busca : MotherPageLogado
{
    #region Propriedades

    public int EditItem
    {
        get
        {
            return Convert.ToInt32(_dgvCargaHoraria.DataKeys[_dgvCargaHoraria.EditIndex].Value);
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada
    /// </summary>
    private string VS_Ordenacao
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.CargaHoraria)
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
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.CargaHoraria)
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
        _dgvCargaHoraria.PageSize = UCComboQtdePaginacao1.Valor;
        _dgvCargaHoraria.PageIndex = 0;
        _dgvCargaHoraria.Sort("", SortDirection.Ascending);
        // atualiza o grid
        _dgvCargaHoraria.DataBind();
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
            _dgvCargaHoraria.PageIndex = 0;
            _odsCargaHoraria.SelectParameters.Clear();
            _odsCargaHoraria.SelectParameters.Add("chr_descricao", _txtDescricaoCargaHoraria.Text);
            _odsCargaHoraria.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            _dgvCargaHoraria.Sort(VS_Ordenacao, VS_SortDirection);

            #region Salvar busca realizada com os parâmetros do ODS.

            foreach (Parameter param in _odsCargaHoraria.SelectParameters)
            {
                filtros.Add(param.Name, param.DefaultValue);
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.CargaHoraria
                ,
                Filtros = filtros
            };

            #endregion

            // mostra essa quantidade no combobox
            UCComboQtdePaginacao1.Valor = itensPagina;
            // atribui essa quantidade para o grid
            _dgvCargaHoraria.PageSize = itensPagina;

            // atualiza o grid
            _dgvCargaHoraria.DataBind();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as cargas horárias.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
    /// </summary>
    private void VerificaBusca()
    {
        if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.CargaHoraria)
        {
            // Recuperar busca realizada e pesquisar automaticamente
            string valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("chr_descricao", out valor);
            _txtDescricaoCargaHoraria.Text = valor;

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

        UCComboQtdePaginacao1.GridViewRelacionado = _dgvCargaHoraria;

        if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), fdsCargaHoraria.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsCargaHoraria.ClientID)), true);
        }

        if (!IsPostBack)
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
                _lblMessage.Text = message;

            _dgvCargaHoraria.PageSize = ApplicationWEB._Paginacao;

            VerificaBusca();

            Page.Form.DefaultFocus = _txtDescricaoCargaHoraria.ClientID;
            Page.Form.DefaultButton = _btnPesquisar.UniqueID;

            _divPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnPesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnLimparPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
        }
    }

    protected void _dgvCargaHoraria_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = RHU_CargaHorariaBO.GetTotalRecords();
        // Seta propriedades necessárias para ordenação nas colunas.
        ConfiguraColunasOrdenacao(_dgvCargaHoraria);

        if ((!string.IsNullOrEmpty(_dgvCargaHoraria.SortExpression)) &&
            (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.CargaHoraria))
        {
            Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

            if (filtros.ContainsKey("VS_Ordenacao"))
            {
                filtros["VS_Ordenacao"] = _dgvCargaHoraria.SortExpression;
            }
            else
            {
                filtros.Add("VS_Ordenacao", _dgvCargaHoraria.SortExpression);
            }

            if (filtros.ContainsKey("VS_SortDirection"))
            {
                filtros["VS_SortDirection"] = _dgvCargaHoraria.SortDirection.ToString();
            }
            else
            {
                filtros.Add("VS_SortDirection", _dgvCargaHoraria.SortDirection.ToString());
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.CargaHoraria
                ,
                Filtros = filtros
            };
        }
    }

    protected void _btnLimparPesquisa_Click(object sender, EventArgs e)
    {
        // Limpa busca da sessão.
        __SessionWEB.BuscaRealizada = new BuscaGestao();
        Response.Redirect("Busca.aspx", false);
    }
    
    protected void _btnPesquisar_Click(object sender, EventArgs e)
    {
        fdsResultados.Visible = true;
        _Pesquisar();
    }

    #endregion
}
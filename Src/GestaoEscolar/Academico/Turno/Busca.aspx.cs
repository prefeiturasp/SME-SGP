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

public partial class Academico_Turno_Busca : MotherPageLogado
{
    #region Propriedades

    public int EditItem
    {
        get
        {
            return Convert.ToInt32(_dgvTurno.DataKeys[_dgvTurno.EditIndex].Value);
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada
    /// </summary>
    private string VS_Ordenacao
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Turno)
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
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Turno)
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
        _dgvTurno.PageSize = UCComboQtdePaginacao1.Valor;
        _dgvTurno.PageIndex = 0;
        _dgvTurno.Sort("", SortDirection.Ascending);
        // atualiza o grid
        _dgvTurno.DataBind();
    }

    #endregion

    #region Métodos

    private void _Pesquisar()
    {
        try
        {
            Dictionary<string, string> filtros = new Dictionary<string, string>();
            _dgvTurno.PageIndex = 0;
            _odsTurno.SelectParameters.Clear();
            _odsTurno.SelectParameters.Add("ttn_id", _UCComboTipoTurno.Valor.ToString());
            _odsTurno.SelectParameters.Add("trn_descricao", _txtDescricao.Text);
            _odsTurno.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
            _odsTurno.DataBind();

            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            _dgvTurno.Sort(VS_Ordenacao, VS_SortDirection);

            #region Salvar busca realizada com os parâmetros do ODS.

            foreach (Parameter param in _odsTurno.SelectParameters)
            {
                filtros.Add(param.Name, param.DefaultValue);
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.Turno
                ,
                Filtros = filtros
            };

            #endregion

            // mostra essa quantidade no combobox
            UCComboQtdePaginacao1.Valor = itensPagina;
            // atribui essa quantidade para o grid
            _dgvTurno.PageSize = itensPagina;
            // atualiza o grid
            _dgvTurno.DataBind();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar pesquisar os turnos.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
    /// </summary>
    private void VerificaBusca()
    {
        if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Turno)
        {
            // Recuperar busca realizada e pesquisar automaticamente
            string valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ttn_id", out valor);
            _UCComboTipoTurno.Valor = Convert.ToInt32(valor);

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("trn_descricao", out valor);
            _txtDescricao.Text = valor;

            _Pesquisar();
        }
        else
        {
            fdsResultado.Visible = false;
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

        UCComboQtdePaginacao1.GridViewRelacionado = _dgvTurno;

        if (!IsPostBack)
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
                _lblMessage.Text = message;

            _dgvTurno.PageSize = ApplicationWEB._Paginacao;

            _UCComboTipoTurno.CarregarTipoTurno();

            Page.Form.DefaultButton = _btnPesquisar.UniqueID;
            Page.Form.DefaultFocus = _UCComboTipoTurno.Combo_ClientID;

            VerificaBusca();

            if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), fdsTurno.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsTurno.ClientID)), true);
            }
        }
    }

    protected void _dgvTurno_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = ACA_TurnoBO.GetTotalRecords();
        // Seta propriedades necessárias para ordenação nas colunas.
        ConfiguraColunasOrdenacao(_dgvTurno);

        if ((!string.IsNullOrEmpty(_dgvTurno.SortExpression)) &&
           (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Turno))
        {
            Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

            if (filtros.ContainsKey("VS_Ordenacao"))
            {
                filtros["VS_Ordenacao"] = _dgvTurno.SortExpression;
            }
            else
            {
                filtros.Add("VS_Ordenacao", _dgvTurno.SortExpression);
            }

            if (filtros.ContainsKey("VS_SortDirection"))
            {
                filtros["VS_SortDirection"] = _dgvTurno.SortDirection.ToString();
            }
            else
            {
                filtros.Add("VS_SortDirection", _dgvTurno.SortDirection.ToString());
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.Turno
                ,
                Filtros = filtros
            };
        }

    }

    protected void _btnLimparPesquisa_Click(object sender, EventArgs e)
    {
        // Limpa busca da sessão.
        __SessionWEB.BuscaRealizada = new BuscaGestao();
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/Turno/Busca.aspx", false);
    }
    
    protected void _btnPesquisar_Click(object sender, EventArgs e)
    {
        fdsResultado.Visible = true;
        _Pesquisar();
    }

    #endregion
}

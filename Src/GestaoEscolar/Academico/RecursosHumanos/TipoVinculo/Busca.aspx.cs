using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.Entities;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.Validation.Exceptions;
using System.Web;

public partial class Academico_RecursosHumanos_TipoVinculo_Busca : MotherPageLogado
{       
    #region Propriedades

    public int EditItem
    {
        get
        {
            return Convert.ToInt32(_dgvTipoVinculo.DataKeys[_dgvTipoVinculo.EditIndex].Value);
        }
    }

    #endregion

    #region Delegates

    protected void UCComboQtdePaginacao_IndexChanged()
    {
        // atribui nova quantidade itens por página para o grid
        _dgvTipoVinculo.PageSize = UCComboQtdePaginacao1.Valor;
        _dgvTipoVinculo.PageIndex = 0;
        // atualiza o grid
        _dgvTipoVinculo.Sort("", SortDirection.Ascending);
        _dgvTipoVinculo.DataBind();
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
            _dgvTipoVinculo.Sort("", SortDirection.Ascending);
            _dgvTipoVinculo.PageIndex = 0;
            _odsTipoVinculo.SelectParameters.Clear();            
            _odsTipoVinculo.SelectParameters.Add("tvi_nome", _txtNome.Text);
            _odsTipoVinculo.SelectParameters.Add("tvi_descricao", _txtDescricao.Text);             
            _odsTipoVinculo.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());                                    
            _odsTipoVinculo.DataBind();

            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            // mostra essa quantidade no combobox
            UCComboQtdePaginacao1.Valor = itensPagina;
            // atribui essa quantidade para o grid
            _dgvTipoVinculo.PageSize = itensPagina;
            // atualiza o grid
            _dgvTipoVinculo.DataBind();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os tipos de vínculo.", UtilBO.TipoMensagem.Erro);
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
            Page.ClientScript.RegisterStartupScript(GetType(), fdsTipoVinculo.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsTipoVinculo.ClientID)), true);
        }

        UCComboQtdePaginacao1.GridViewRelacionado = _dgvTipoVinculo;

        if (!IsPostBack)
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
                _lblMessage.Text = message;

            _dgvTipoVinculo.PageSize = ApplicationWEB._Paginacao;

            fdsResultados.Visible = false;

            Page.Form.DefaultFocus = _txtNome.ClientID;
            Page.Form.DefaultButton = _btnPesquisar.UniqueID;
        }
    }

    protected void _odsTipoVinculo_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (e.ExecutingSelectCount)
            e.InputParameters.Clear();
    }

    protected void _dgvTipoVinculo_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = RHU_TipoVinculoBO.GetTotalRecords();
        // Seta propriedades necessárias para ordenação nas colunas.
        ConfiguraColunasOrdenacao(_dgvTipoVinculo);
    }
    
    protected void _btnLimparPesquisa_Click(object sender, EventArgs e)
    {
        __SessionWEB.BuscaRealizada = new BuscaGestao();
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/RecursosHumanos/TipoVinculo/Busca.aspx", false);      
    }
    
    protected void _btnPesquisar_Click(object sender, EventArgs e)
    {
        _Pesquisar();
        fdsResultados.Visible = true;
    }

    #endregion  
}


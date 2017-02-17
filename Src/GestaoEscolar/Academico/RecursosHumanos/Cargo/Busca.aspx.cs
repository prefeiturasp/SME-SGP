using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.Validation.Exceptions;
using System.Collections.Generic;
using System.Web;

public partial class Academico_RecursosHumanos_Cargo_Busca : MotherPageLogado
{
    #region Propriedades

    public int EditItem_crg_id
    {
        get
        {
            return Convert.ToInt32(_dgvCargo.DataKeys[_dgvCargo.EditIndex].Values[0]);
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada
    /// </summary>
    private string VS_Ordenacao
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Cargos)
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
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Cargos)
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
        _dgvCargo.PageSize = UCComboQtdePaginacao1.Valor;
        _dgvCargo.PageIndex = 0;
        _dgvCargo.Sort("", SortDirection.Ascending);
        // atualiza o grid
        _dgvCargo.DataBind();
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Pesquisa os cargos para serem exibidos no gride
    /// </summary>
    private void _Pesquisar()
    {
        try
        {
            Dictionary<string, string> filtros = new Dictionary<string, string>();
            _dgvCargo.PageIndex = 0;
            _odsCargo.SelectParameters.Clear();
            _odsCargo.SelectParameters.Add("tvi_id", _UCComboTipoVinculo.Valor.ToString());
            _odsCargo.SelectParameters.Add("crg_nome", _txtNome.Text);
            _odsCargo.SelectParameters.Add("crg_codigo", _txtCodigo.Text);
            _odsCargo.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
            _odsCargo.SelectParameters.Add("crg_cargoDocente", "2");
            _odsCargo.DataBind();

            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            _dgvCargo.Sort(VS_Ordenacao, VS_SortDirection);

            #region Salvar busca realizada com os parâmetros do ODS.

            foreach (Parameter param in _odsCargo.SelectParameters)
            {
                filtros.Add(param.Name, param.DefaultValue);
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.Cargos
                ,
                Filtros = filtros
            };

            #endregion

            // mostra essa quantidade no combobox
            UCComboQtdePaginacao1.Valor = itensPagina;
            // atribui essa quantidade para o grid
            _dgvCargo.PageSize = itensPagina;

            // atualiza o grid
            _dgvCargo.DataBind();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os cargos.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
    /// </summary>
    private void VerificaBusca()
    {
        if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Cargos)
        {
            // Recuperar busca realizada e pesquisar automaticamente
            string valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crg_codigo", out valor);
            _txtCodigo.Text = valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crg_nome", out valor);
            _txtNome.Text = valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tvi_id", out valor);
            _UCComboTipoVinculo.Valor = Convert.ToInt32(valor);

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

        UCComboQtdePaginacao1.GridViewRelacionado = _dgvCargo;

        if (!IsPostBack)
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
                _lblMessage.Text = message;

            _dgvCargo.PageSize = ApplicationWEB._Paginacao;

            try
            {
                _UCComboTipoVinculo.CarregarTipoVinculo();
                VerificaBusca();

                if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), fdsCargo.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsCargo.ClientID)), true);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }

            Page.Form.DefaultFocus = _UCComboTipoVinculo.Combo_ClientID;
            Page.Form.DefaultButton = _btnPesquisar.UniqueID;

            _divPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnPesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnLimparPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
        }
    }

    protected void _dgvCargo_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = RHU_CargoBO.GetTotalRecords();
        // Seta propriedades necessárias para ordenação nas colunas.
        ConfiguraColunasOrdenacao(_dgvCargo);

        if ((!string.IsNullOrEmpty(_dgvCargo.SortExpression)) &&
           (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Cargos))
        {
            Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

            if (filtros.ContainsKey("VS_Ordenacao"))
            {
                filtros["VS_Ordenacao"] = _dgvCargo.SortExpression;
            }
            else
            {
                filtros.Add("VS_Ordenacao", _dgvCargo.SortExpression);
            }

            if (filtros.ContainsKey("VS_SortDirection"))
            {
                filtros["VS_SortDirection"] = _dgvCargo.SortDirection.ToString();
            }
            else
            {
                filtros.Add("VS_SortDirection", _dgvCargo.SortDirection.ToString());
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.Cargos
                ,
                Filtros = filtros
            };
        }
    }

    protected void _dgvCargo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton _btnExcluir = (ImageButton)e.Row.FindControl("_btnExcluir");
            if (_btnExcluir != null)
            {
                _btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                _btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
            }
        }
    }

    protected void _dgvCargo_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Deletar")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());
                int crg_id = Convert.ToInt32(_dgvCargo.DataKeys[index].Value);

                RHU_Cargo entity = new RHU_Cargo { crg_id = crg_id };
                RHU_CargoBO.GetEntity(entity);

                if (RHU_CargoBO.Delete(entity))
                {
                    _dgvCargo.PageIndex = 0;
                    _dgvCargo.Sort("", SortDirection.Ascending);
                    _dgvCargo.DataBind();
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "crg_id: " + crg_id);
                    _lblMessage.Text = UtilBO.GetErroMessage("Cargo excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o cargo.", UtilBO.TipoMensagem.Erro);
            }
        }
    }

    protected void _btnLimparPesquisa_Click(object sender, EventArgs e)
    {
        // Limpa busca da sessão.
        __SessionWEB.BuscaRealizada = new BuscaGestao();
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/RecursosHumanos/Cargo/Busca.aspx", false);
    }

    protected void _btnNovo_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/RecursosHumanos/Cargo/Cadastro.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void _btnPesquisar_Click(object sender, EventArgs e)
    {
        fdsResultados.Visible = true;
        _Pesquisar();
    }

    #endregion
}

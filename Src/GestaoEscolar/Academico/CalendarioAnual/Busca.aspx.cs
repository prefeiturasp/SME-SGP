using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using System.Collections.Generic;
using System.Web;

public partial class Academico_Calendario_Anual_Busca : MotherPageLogado
{
    #region Constantes
    
    /// <summary>Posição da coluna Excluir no grid view do Calendario Anual.</summary>
    private const int PosicaoLimites = 5;

    /// <summary>Posição da coluna Excluir no grid view do Calendario Anual.</summary>
    private const int PosicaoExcluir = 6;

    #endregion

    #region Propriedades

    /// <summary>
    /// Retorna o cal_id do registro que esta sendo editado.
    /// </summary>
    public int EditItem
    {
        get
        {
            return Convert.ToInt32(_dgvCalendarioAnual.DataKeys[_dgvCalendarioAnual.EditIndex].Value);
        }
    }

    /// <summary>
    /// Retorna o cal_id do registro que esta selecionado.
    /// </summary>
    public int SelectedItem
    {
        get
        {
            return Convert.ToInt32(_dgvCalendarioAnual.DataKeys[_dgvCalendarioAnual.SelectedIndex].Value);
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada
    /// </summary>
    private string VS_Ordenacao
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.CalendarioAnual)
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
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.CalendarioAnual)
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
        _dgvCalendarioAnual.PageSize = UCComboQtdePaginacao1.Valor;
        _dgvCalendarioAnual.PageIndex = 0;
        // atualiza o grid
        _dgvCalendarioAnual.DataBind();
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

            _dgvCalendarioAnual.PageIndex = 0;
            _odsCalendarioAnual.SelectParameters.Clear();
            _odsCalendarioAnual.SelectParameters.Add("cal_ano", _txtAno.Text);
            _odsCalendarioAnual.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa ? __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString() : Guid.Empty.ToString());
            _odsCalendarioAnual.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa ? __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString() : Guid.Empty.ToString());
            _odsCalendarioAnual.SelectParameters.Add("doc_id", __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual && __SessionWEB.__UsuarioWEB.Docente != null ? __SessionWEB.__UsuarioWEB.Docente.doc_id.ToString() : "0");
            _odsCalendarioAnual.SelectParameters.Add("cal_descricao", _txtDescricao.Text);
            _odsCalendarioAnual.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

            // quantidade de itens por página            
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            _dgvCalendarioAnual.Sort(VS_Ordenacao, VS_SortDirection);

            #region Salvar busca realizada com os parâmetros do ODS.

            //Salvar UA Superior.            
            //  if (UCFiltroEscolas1._VS_FiltroEscola == true)
            //  filtros.Add("ua_superior", UCFiltroEscolas1._ComboUnidadeAdministrativa.SelectedValue);

            foreach (Parameter param in _odsCalendarioAnual.SelectParameters)
            {
                filtros.Add(param.Name, param.DefaultValue);
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.CalendarioAnual
                ,
                Filtros = filtros
            };

            #endregion

            // mostra essa quantidade no combobox
            UCComboQtdePaginacao1.Valor = itensPagina;
            // atribui essa quantidade para o grid
            _dgvCalendarioAnual.PageSize = itensPagina;
            // atualiza o grid
            _dgvCalendarioAnual.DataBind();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os calendários escolares.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
    /// </summary>
    private void VerificaBusca()
    {
        if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.CalendarioAnual)
        {
            // Recuperar busca realizada e pesquisar automaticamente
            string valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_ano", out valor);
            _txtAno.Text = valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_descricao", out valor);
            _txtDescricao.Text = valor;

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
            sm.Scripts.Add(new ScriptReference("~/Includes/jsTabs.js"));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
        }

        UCComboQtdePaginacao1.GridViewRelacionado = _dgvCalendarioAnual;

        if (!IsPostBack)
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
                _lblMessage.Text = message;

            _dgvCalendarioAnual.PageIndex = 0;
            _dgvCalendarioAnual.PageSize = ApplicationWEB._Paginacao;

            try
            {
                VerificaBusca();

                if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), fdsCalendario.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsCalendario.ClientID)), true);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }

            Page.Form.DefaultButton = _btnPesquisar.UniqueID;
            Page.Form.DefaultFocus = _txtAno.ClientID;

            _divPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnPesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnLimparPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;

            // Muda a visão da tela dependendo das permissões do grupo do usuário
            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Administracao)
            {
                _txtAno.Text = DateTime.Now.Year.ToString();
                _txtDescricao.Visible = false;
                _lblDescricao.Visible = false;
                //_btnNovo.Visible = false;
                _btnLimparPesquisa.Visible = false;
                _dgvCalendarioAnual.Columns[PosicaoLimites].Visible = false;
                _dgvCalendarioAnual.Columns[PosicaoExcluir].Visible = false;

                _Pesquisar();
                fdsResultados.Visible = true;
            }
        }

    }

    protected void _odsCalendarioAnual_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {

        if (e.ExecutingSelectCount)
            e.InputParameters.Clear();
    }
    
    protected void _dgvCalendarioAnual_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = ACA_CalendarioAnualBO.GetTotalRecords();

        ConfiguraColunasOrdenacao(_dgvCalendarioAnual);

        if ((!string.IsNullOrEmpty(_dgvCalendarioAnual.SortExpression)) &&
        (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.CalendarioAnual))
        {
            Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

            if (filtros.ContainsKey("VS_Ordenacao"))
            {
                filtros["VS_Ordenacao"] = _dgvCalendarioAnual.SortExpression;
            }
            else
            {
                filtros.Add("VS_Ordenacao", _dgvCalendarioAnual.SortExpression);
            }

            if (filtros.ContainsKey("VS_SortDirection"))
            {
                filtros["VS_SortDirection"] = _dgvCalendarioAnual.SortDirection.ToString();
            }
            else
            {
                filtros.Add("VS_SortDirection", _dgvCalendarioAnual.SortDirection.ToString());
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.CalendarioAnual
                ,
                Filtros = filtros
            };
        }
    }

    protected void _dgvCalendarioAnual_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            bool PodeAlterar = (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar) && !(__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Administracao);
            Label _lblAlterar = (Label)e.Row.FindControl("_lblAlterar");
            if (_lblAlterar != null)
            {
                _lblAlterar.Visible = !PodeAlterar;
            }

            LinkButton _btnAlterar = e.Row.FindControl("_btnAlterar") as LinkButton;
            if (_btnAlterar != null)
            {
                _btnAlterar.Visible = PodeAlterar;
            }

            ImageButton _btnExcluir = e.Row.FindControl("_btnExcluir") as ImageButton;
            if (_btnExcluir != null)
            {
                _btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                _btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
            }

            ImageButton _btnVisualizar = e.Row.FindControl("_btnVisualizar") as ImageButton;
            if (_btnVisualizar != null)
            {
                _btnVisualizar.CommandArgument = e.Row.RowIndex.ToString();
            }

            ImageButton _btnCadastrarLimites = e.Row.FindControl("_btnCadastrarLimites") as ImageButton;
            if (_btnCadastrarLimites != null)
            {
                _btnCadastrarLimites.CommandArgument = e.Row.RowIndex.ToString();
            }
        }
    }

    protected void _dgvCalendarioAnual_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Deletar")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());
                int cal_id = Convert.ToInt32(_dgvCalendarioAnual.DataKeys[index].Value);

                ACA_CalendarioAnual entity = new ACA_CalendarioAnual { cal_id = cal_id };
                ACA_CalendarioAnualBO.GetEntity(entity);

                if (ACA_CalendarioAnualBO.Delete(entity))
                {
                    _dgvCalendarioAnual.PageIndex = 0;
                    _dgvCalendarioAnual.DataBind();
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "cal_id: " + cal_id);
                    _lblMessage.Text = UtilBO.GetErroMessage("Calendário escolar excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o calendário escolar.", UtilBO.TipoMensagem.Erro);
            }
        }
    }

    protected void _btnNovo_Click(object sender, EventArgs e)
    {
        Response.Redirect("Cadastro.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void _btnPesquisar_Click(object sender, EventArgs e)
    {
        _Pesquisar();
        fdsResultados.Visible = true;
    }

    protected void _btnLimparPesquisa_Click(object sender, EventArgs e)
    {
        // Limpa busca da sessão.
        __SessionWEB.BuscaRealizada = new BuscaGestao();
        Response.Redirect("Busca.aspx", false);
    }

    #endregion
}

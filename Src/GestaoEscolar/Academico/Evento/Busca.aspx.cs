using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.Validation.Exceptions;
using System.Collections.Generic;
using System.Web;

public partial class Academico_Eventos_Busca : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Retorna o evt_id do registro que esta sendo editado.
    /// </summary>
    public long EditItem
    {
        get
        {
            return Convert.ToInt64(_dgvEventos.DataKeys[_dgvEventos.EditIndex].Value);
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada
    /// </summary>
    private string VS_Ordenacao
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Evento)
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                string valor;
                if (filtros.TryGetValue("VS_Ordenacao", out valor))
                {
                    return valor;
                }
            }

            return String.Empty;
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada
    /// </summary>
    private SortDirection VS_SortDirection
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Evento)
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
        _dgvEventos.PageSize = UCComboQtdePaginacao1.Valor;
        _dgvEventos.PageIndex = 0;
        // atualiza o grid
        _dgvEventos.DataBind();
    }


    /// <summary>
    /// Evento change do combo de UA Superior.
    /// </summary>
    private void UCFiltroEscolas1__Selecionar()
    {
        try
        {
            ucComboUAEscola.FiltroEscolasControladas = null;
            ucComboUAEscola.CarregaEscolaPorUASuperiorSelecionada();

            if (ucComboUAEscola.Uad_ID != Guid.Empty)
            {
                ucComboUAEscola.FocoEscolas = true;
                ucComboUAEscola.PermiteAlterarCombos = true;
            }

            ucComboUAEscola.SelectedValueEscolas = new[] { -1, -1 };
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
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

            _dgvEventos.PageIndex = 0;
            _odsEvento.SelectParameters.Clear();
            _odsEvento.SelectParameters.Add("uad_idSuperior", ucComboUAEscola.Uad_ID.ToString());
            _odsEvento.SelectParameters.Add("evt_nome", _txtNome.Text);
            _odsEvento.SelectParameters.Add("cal_id", _UCComboCalendario.Valor.ToString());
            _odsEvento.SelectParameters.Add("tev_id", _UCComboTipoEvento.Valor.ToString());
            _odsEvento.SelectParameters.Add("esc_uni_id", ucComboUAEscola.DdlEscola.SelectedValue);
            _odsEvento.SelectParameters.Add("evt_id", "0");
            _odsEvento.SelectParameters.Add("evt_situacao", "0");
            _odsEvento.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
            _odsEvento.SelectParameters.Add("evt_padrao", chkPadrao.Checked ? "1" : "0");
            _odsEvento.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
            _odsEvento.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
            _odsEvento.SelectParameters.Add("adm", (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao).ToString());
            _dgvEventos.DataBind();

            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            _dgvEventos.Sort(VS_Ordenacao, VS_SortDirection);

            #region Salvar busca realizada com os parâmetros do ODS.

            //Salvar UA Superior.            
            if (ucComboUAEscola.FiltroEscola)
                filtros.Add("ua_superior", ucComboUAEscola.Uad_ID.ToString());

            foreach (Parameter param in _odsEvento.SelectParameters)
            {
                filtros.Add(param.Name, param.DefaultValue);
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.Evento
                ,
                Filtros = filtros
            };

            #endregion

            // mostra essa quantidade no combobox
            UCComboQtdePaginacao1.Valor = itensPagina;
            // atribui essa quantidade para o grid
            _dgvEventos.PageSize = itensPagina;
            // atualiza o grid
            _dgvEventos.DataBind();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar pesquisar os eventos.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
    /// </summary>
    private void VerificaBusca()
    {
        if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Evento)
        {
            // Recuperar busca realizada e pesquisar automaticamente
            string valor;
            int valorInt;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("evt_nome", out valor);
            _txtNome.Text = valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tev_id", out valor);
            if (Int32.TryParse(valor, out valorInt))
                _UCComboTipoEvento.Valor = valorInt;

            if (ucComboUAEscola.FiltroEscola)
            {
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ua_superior", out valor);

                if (!string.IsNullOrEmpty(valor))
                    ucComboUAEscola.DdlUA.SelectedValue = valor;

                if (valor != Guid.Empty.ToString())
                    SelecionarEscola(ucComboUAEscola.FiltroEscola);
            }
            else
                SelecionarEscola(ucComboUAEscola.FiltroEscola);

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("evt_padrao", out valor);
            if (valor.Equals("1"))
            {
                chkPadrao.Checked = true;
                chkPadrao_CheckedChanged(chkPadrao, new EventArgs());
            }

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor);
            if (Int32.TryParse(valor, out valorInt))
                _UCComboCalendario.Valor = valorInt;

            _Pesquisar();
        }
        else
        {
            fdsResultados.Visible = false;
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
            UCFiltroEscolas1__Selecionar();

        string esc_uni_id;

        if (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_uni_id", out esc_uni_id))
        {
            ucComboUAEscola.DdlEscola.SelectedValue = esc_uni_id;
        }
    }

    #endregion

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
        {
            __SessionWEB.PostMessages = "Usuário não possui permissão para acessar essa página.";
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Index.aspx", false);
        }

        UCComboQtdePaginacao1.GridViewRelacionado = _dgvEventos;

        try
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            }

            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    _lblMessage.Text = message;

                _dgvEventos.PageIndex = 0;
                _dgvEventos.PageSize = ApplicationWEB._Paginacao;

                try
                {
                    ucComboUAEscola.FocusUA();
                    ucComboUAEscola.Inicializar();

                    _UCComboTipoEvento.CarregarTipoEvento(0);

                    _UCComboCalendario.CarregarCalendarioAnual();

                    VerificaBusca();
                    if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), fdsEventosCalendario.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsEventosCalendario.ClientID)), true);
                    }

                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }

                Page.Form.DefaultButton = _btnPesquisar.UniqueID;
                Page.Form.DefaultFocus = ucComboUAEscola.VisibleUA ? ucComboUAEscola.ComboUA_ClientID : ucComboUAEscola.ComboEscola_ClientID;

                _divPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                _btnPesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                _btnLimparPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                _btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            }

            ucComboUAEscola.IndexChangedUA += UCFiltroEscolas1__Selecionar;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void _dgvEventos_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = ACA_EventoBO.GetTotalRecords();
        // Seta propriedades necessárias para ordenação nas colunas.
        ConfiguraColunasOrdenacao(_dgvEventos);

        if ((!string.IsNullOrEmpty(_dgvEventos.SortExpression)) &&
           (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Evento))
        {
            Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

            if (filtros.ContainsKey("VS_Ordenacao"))
            {
                filtros["VS_Ordenacao"] = _dgvEventos.SortExpression;
            }
            else
            {
                filtros.Add("VS_Ordenacao", _dgvEventos.SortExpression);
            }

            if (filtros.ContainsKey("VS_SortDirection"))
            {
                filtros["VS_SortDirection"] = _dgvEventos.SortDirection.ToString();
            }
            else
            {
                filtros.Add("VS_SortDirection", _dgvEventos.SortDirection.ToString());
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.Evento
                ,
                Filtros = filtros
            };
        }
    }

    protected void _dgvEventos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            Label _lblAlterar = (Label)e.Row.FindControl("_lblAlterar");
            if (_lblAlterar != null)
            {
                _lblAlterar.Visible = !__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                // se for GESTÃO ou UA
                // e Evento for PADRÃO  -- Não pode alterar
                if ((__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao)
                    && _dgvEventos.DataKeys[e.Row.RowIndex][2].ToString().ToUpper() == "SIM")
                {
                    _lblAlterar.Visible = true;
                }

                // se for GESTÃO ou UA e 
                // NAO tiver permissao para editar
                if ((__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao)
                    && _dgvEventos.DataKeys[e.Row.RowIndex][3].ToString() == "0")
                {
                    _lblAlterar.Visible = true;
                }
            }

            LinkButton _btnAlterar = (LinkButton)e.Row.FindControl("_btnAlterar");
            if (_btnAlterar != null)
            {
                _btnAlterar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

                // se for GESTÃO ou UA
                // e Evento for PADRÃO  -- Não pode alterar
                if ((__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao)
                    && _dgvEventos.DataKeys[e.Row.RowIndex][2].ToString().ToUpper() == "SIM")
                {
                    _btnAlterar.Visible = false;
                }

                // se for GESTÃO ou UA e 
                // NAO tiver permissao para editar
                if ((__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao)
                    && _dgvEventos.DataKeys[e.Row.RowIndex][3].ToString() == "0")
                {
                    _btnAlterar.Visible = false;
                }
            }

            ImageButton _btnExcluir = (ImageButton)e.Row.FindControl("_btnExcluir");
            if (_btnExcluir != null)
            {
                _btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                _btnExcluir.CommandArgument = e.Row.RowIndex.ToString();

                // se data inicial menor que data de hoje não pode excluir
                string evtSemAtividadeDiscente = DataBinder.Eval(e.Row.DataItem, "evt_semAtividadeDiscente").ToString().ToUpper();
                bool param = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_CADASTRO_EVENTO_RETROATIVO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                bool param_discente = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_CADASTRO_EVENTO_RETROATIVO_SEM_ATIVIDADE_DISCENTE, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                if ((DateTime)_dgvEventos.DataKeys[e.Row.RowIndex][1] <= DateTime.Today && evtSemAtividadeDiscente.Equals("SIM") && param && !param_discente)
                    _btnExcluir.Visible = false;

                // se for GESTÃO ou UA
                // e Evento for PADRÃO  -- Não pode excluir
                if ((__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao)
                    && _dgvEventos.DataKeys[e.Row.RowIndex][2].ToString().ToUpper() == "SIM")
                {
                    _btnExcluir.Visible = false;
                }

                // se for GESTÃO ou UA e 
                // NAO tiver permissao para editar
                if ((__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao)
                    && _dgvEventos.DataKeys[e.Row.RowIndex][3].ToString() == "0")
                {
                    _btnExcluir.Visible = false;
                }

            }
        }
    }

    protected void _dgvEventos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Deletar")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());
                int evt_id = Convert.ToInt32(_dgvEventos.DataKeys[index].Value);

                ACA_Evento entity = new ACA_Evento { evt_id = evt_id };
                ACA_EventoBO.GetEntity(entity);

                if (ACA_EventoBO.Delete(entity))
                {
                    _dgvEventos.PageIndex = 0;
                    _dgvEventos.DataBind();
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "evt_id: " + evt_id);
                    _lblMessage.Text = UtilBO.GetErroMessage("Evento excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao excluir o evento.", UtilBO.TipoMensagem.Erro);
            }
        }
    }

    protected void _btnLimparPesquisa_Click(object sender, EventArgs e)
    {
        // Limpa busca da sessão.
        __SessionWEB.BuscaRealizada = new BuscaGestao();
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/Evento/Busca.aspx", false);
    }

    protected void _btnNovo_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/Evento/Cadastro.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void _btnPesquisar_Click(object sender, EventArgs e)
    {
        _Pesquisar();
        fdsResultados.Visible = true;
    }

    protected void chkPadrao_CheckedChanged(object sender, EventArgs e)
    {
        if (chkPadrao.Checked)
        {
            ucComboUAEscola.DdlUA.SelectedIndex = -1;
            ucComboUAEscola.DdlUA.Enabled = false;
            ucComboUAEscola.ObrigatorioUA = false;
            ucComboUAEscola.ObrigatorioEscola = false;
            ucComboUAEscola.DdlEscola.SelectedIndex = -1;
            ucComboUAEscola.DdlEscola.Enabled = false;
        }
        else
        {
            ucComboUAEscola.DdlUA.Enabled = true;

            if ((__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa)
                || !ucComboUAEscola.DdlUA.Visible)
                ucComboUAEscola.DdlEscola.Enabled = true;
        }
    }

    #endregion
}

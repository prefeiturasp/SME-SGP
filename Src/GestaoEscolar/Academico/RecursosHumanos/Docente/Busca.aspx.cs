using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

public partial class Academico_RecursosHumanos_Docente_Busca : MotherPageLogado
{
    #region Constantes

    private const int indiceColunaExcluir = 6;

    #endregion Constantes

    #region Propriedades

    public int EditItem
    {
        get
        {
            return Convert.ToInt32(_grvDocente.DataKeys[_grvDocente.EditIndex].Values[0]);
        }
    }

    public int EditItemDocId
    {
        get
        {
            return Convert.ToInt32(_grvDocente.DataKeys[_grvDocente.EditIndex].Values[1]);
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada
    /// </summary>
    private string VS_Ordenacao
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Docentes)
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
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Docentes)
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

    #endregion Propriedades

    #region Delegates

    protected void UCComboQtdePaginacao_IndexChanged()
    {
        // atribui nova quantidade itens por página para o grid
        _grvDocente.PageSize = UCComboQtdePaginacao1.Valor;
        _grvDocente.PageIndex = 0;
        // atualiza o grid
        _grvDocente.DataBind();
    }

    #endregion Delegates

    #region Métodos

    /// <summary>
    /// Realiza a consulta com os filtros informados, e salva a busca realizada na sessão.
    /// </summary>
    private void _Pesquisar()
    {
        try
        {
            ESC_Escola entEscola = new ESC_Escola
                                       {
                                           esc_id = uccFiltroEscolas.Esc_ID
                                       };
            if (!chkTodosDocentes.Checked && uccFiltroEscolas.Esc_ID > -1)
                ESC_EscolaBO.GetEntity(entEscola);

            Dictionary<string, string> filtros = new Dictionary<string, string>();

            bool todosDocentes = ((__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao)
                            || chkTodosDocentes.Checked);

            _grvDocente.PageIndex = 0;
            odsDocente.SelectParameters.Clear();
            odsDocente.SelectParameters.Add("pes_nome", _txtNome.Text);
            odsDocente.SelectParameters.Add("coc_matricula", _txtMatricula.Text);
            odsDocente.SelectParameters.Add("tipo_cpf", _txtCPF.Text);
            odsDocente.SelectParameters.Add("tipo_rg", _txtRG.Text);
            odsDocente.SelectParameters.Add("crg_id", UCComboCargo1.Valor.ToString());
            odsDocente.SelectParameters.Add("fun_id", UCComboFuncao1.Valor.ToString());

            // Se estiver checado o chekbox, não passa a escola do combo (está invisível na tela).
            odsDocente.SelectParameters.Add("uad_idSuperior", chkTodosDocentes.Checked ? Guid.Empty.ToString() : uccFiltroEscolas.Uad_ID.ToString());
            odsDocente.SelectParameters.Add("uad_id", chkTodosDocentes.Checked ? Guid.Empty.ToString() : entEscola.uad_id.ToString());

            odsDocente.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
            odsDocente.SelectParameters.Add("todosDocentes", todosDocentes.ToString());
            odsDocente.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
            odsDocente.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
            odsDocente.DataBind();

            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            _grvDocente.Sort(VS_Ordenacao, VS_SortDirection);

            #region Salvar busca realizada com os parâmetros do ODS.

            foreach (Parameter param in odsDocente.SelectParameters)
            {
                filtros.Add(param.Name, param.DefaultValue);
            }

            // Salva dados do combo de escola e UA Superior.
            filtros["uad_idSuperior"] = uccFiltroEscolas.Uad_ID.ToString();
            filtros.Add("esc_id", uccFiltroEscolas.Esc_ID.ToString());
            filtros.Add("uni_id", uccFiltroEscolas.Uni_ID.ToString());

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.Docentes
                ,
                Filtros = filtros
            };

            #endregion Salvar busca realizada com os parâmetros do ODS.

            // mostra essa quantidade no combobox
            UCComboQtdePaginacao1.Valor = itensPagina;
            // atribui essa quantidade para o grid
            _grvDocente.PageSize = itensPagina;
            // atualiza o grid
            _grvDocente.DataBind();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os docentes.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
    /// </summary>
    private void VerificaBusca()
    {
        if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Docentes)
        {
            // Recuperar busca realizada e pesquisar automaticamente
            string valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("pes_nome", out valor);
            _txtNome.Text = valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tipo_cpf", out valor);
            _txtCPF.Text = valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tipo_rg", out valor);
            _txtRG.Text = valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("todosDocentes", out valor);
            chkTodosDocentes.Checked =
                (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Administracao) &&
                Convert.ToBoolean(valor);

            if (uccFiltroEscolas.VisibleUA)
            {
                if (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor))
                {
                    if (valor != (new Guid()).ToString())
                    {
                        uccFiltroEscolas.Uad_ID = new Guid(valor);
                        uccFiltroEscolas.EnableEscolas = uccFiltroEscolas.Uad_ID != Guid.Empty;
                    }
                }
            }

            string esc_id, uni_id;
            if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
            {
                uccFiltroEscolas.SelectedValueEscolas = new[]
                                                            {
                                                                Convert.ToInt32(esc_id)
                                                                , Convert.ToInt32(uni_id)
                                                            };
            }

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crg_id", out valor);
            UCComboCargo1.Valor = string.IsNullOrEmpty(valor) ? -1 : Convert.ToInt32(valor);

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("fun_id", out valor);
            UCComboFuncao1.Valor = string.IsNullOrEmpty(valor) ? -1 : Convert.ToInt32(valor);

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("coc_matricula", out valor);
            _txtMatricula.Text = valor;

            _Pesquisar();
        }
        else
        {
            fdsResultados.Visible = false;
        }
    }

    #endregion Métodos

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.UiAriaTabs));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsTabs.js"));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroColaborador.js"));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsBuscaDocentes.js"));
        }

        // Page.ClientScript.RegisterStartupScript(GetType(), fdsConsulta.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsConsulta.ClientID)), true);

        UCComboQtdePaginacao1.GridViewRelacionado = _grvDocente;

        if (!IsPostBack)
        {
            try
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    _lblMessage.Text = message;

                _grvDocente.PageSize = ApplicationWEB._Paginacao;

                SYS_TipoDocumentacao tdo = new SYS_TipoDocumentacao();

                string tdo_id = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF);
                if (!string.IsNullOrEmpty(tdo_id))
                {
                    tdo.tdo_id = new Guid(tdo_id);
                    SYS_TipoDocumentacaoBO.GetEntity(tdo);
                    _lblCPF.Text = tdo.tdo_sigla;
                    _grvDocente.Columns[1].HeaderText = tdo.tdo_sigla;
                }
                else
                {
                    _lblCPF.Text = string.Empty;
                    _lblCPF.Visible = false;
                    _txtCPF.Visible = false;
                    _grvDocente.Columns[1].HeaderText = string.Empty;
                    _grvDocente.Columns[1].SortExpression = string.Empty;
                    _grvDocente.Columns[1].HeaderStyle.CssClass = "hide";
                    _grvDocente.Columns[1].ItemStyle.CssClass = "hide";
                }

                tdo_id = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_RG);
                if (!string.IsNullOrEmpty(tdo_id))
                {
                    tdo.tdo_id = new Guid(tdo_id);
                    SYS_TipoDocumentacaoBO.GetEntity(tdo);
                    _lblRG.Text = tdo.tdo_sigla;
                    _grvDocente.Columns[2].HeaderText = tdo.tdo_sigla;
                }
                else
                {
                    _lblRG.Text = string.Empty;
                    _lblRG.Visible = false;
                    _txtRG.Visible = false;
                    _grvDocente.Columns[2].HeaderText = string.Empty;
                    _grvDocente.Columns[2].SortExpression = string.Empty;
                    _grvDocente.Columns[2].HeaderStyle.CssClass = "hide";
                    _grvDocente.Columns[2].ItemStyle.CssClass = "hide";
                }

                bool controleIntegracao = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_COLABORADOR_APENAS_INTEGRACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                _btnNovo.Visible = !controleIntegracao && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;

                UCComboCargo1.CarregarCargoDocente();
                UCComboFuncao1.CarregarFuncao();

                Page.Form.DefaultButton = _btnPesquisar.UniqueID;
                Page.Form.DefaultFocus = _txtNome.ClientID;

                chkTodosDocentes.Visible = __SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Administracao;

                uccFiltroEscolas.Inicializar();

                VerificaBusca();

                if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), fdsConsulta.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsConsulta.ClientID)), true);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }

            _divPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnPesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            _btnLimparPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;

            _grvDocente.Columns[indiceColunaExcluir].Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
        }
    }

    protected void _grvDocente_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = ACA_DocenteBO.GetTotalRecords();

        // seta propriedades necessárias para ordenação nas colunas.
        ConfiguraColunasOrdenacao(_grvDocente);

        if ((!string.IsNullOrEmpty(_grvDocente.SortExpression)) &&
           (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Docentes))
        {
            Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

            if (filtros.ContainsKey("VS_Ordenacao"))
            {
                filtros["VS_Ordenacao"] = _grvDocente.SortExpression;
            }
            else
            {
                filtros.Add("VS_Ordenacao", _grvDocente.SortExpression);
            }

            if (filtros.ContainsKey("VS_SortDirection"))
            {
                filtros["VS_SortDirection"] = _grvDocente.SortDirection.ToString();
            }
            else
            {
                filtros.Add("VS_SortDirection", _grvDocente.SortDirection.ToString());
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.Docentes
                ,
                Filtros = filtros
            };
        }
    }

    protected void _grvDocente_RowDataBound(object sender, GridViewRowEventArgs e)
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
            }

            bool PermissaoExcluir = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "PermissaoExcluir"));

            bool col_controladoIntegracao = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "col_controladoIntegracao"));
            if (col_controladoIntegracao == false)
            {
                ImageButton _btnExcluir = (ImageButton)e.Row.FindControl("_btnExcluir");
                if (_btnExcluir != null)
                {
                    _btnExcluir.Visible = __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao
                                              ? true
                                              : PermissaoExcluir;

                    _btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                }
            }
            else
            {
                ImageButton _btnExcluir = (ImageButton)e.Row.FindControl("_btnExcluir");
                _btnExcluir.Visible = false;
            }
        }
    }

    protected void _grvDocente_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Deletar")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());
                long doc_id = Convert.ToInt64(_grvDocente.DataKeys[index].Values["doc_id"]);

                ACA_Docente entity = new ACA_Docente { doc_id = doc_id };
                ACA_DocenteBO.GetEntity(entity);

                if (ACA_DocenteBO.Delete(entity))
                {
                    _grvDocente.PageIndex = 0;
                    _grvDocente.DataBind();
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "doc_id: " + doc_id);
                    _lblMessage.Text = UtilBO.GetErroMessage("Docente excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o docente.", UtilBO.TipoMensagem.Erro);
            }
        }
    }

    protected void _btnLimparPesquisa_Click(object sender, EventArgs e)
    {
        // Limpa busca da sessão.
        __SessionWEB.BuscaRealizada = new BuscaGestao();
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/RecursosHumanos/Docente/Busca.aspx", false);
    }

    protected void _btnNovo_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/RecursosHumanos/Docente/Cadastro.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void _btnPesquisar_Click(object sender, EventArgs e)
    {
        _Pesquisar();
        fdsResultados.Visible = true;
    }

    #endregion Eventos
}
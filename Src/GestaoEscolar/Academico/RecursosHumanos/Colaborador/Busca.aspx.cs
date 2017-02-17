using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

public partial class Academico_RecursosHumanos_Colaborador_Busca : MotherPageLogado
{
    #region Constantes

    private const int indiceColunaCriarDocente = 6;
    private const int indiceColunaExcluir = 7;

    #endregion Constantes

    #region Propriedades

    public int EditItem
    {
        get
        {
            return Convert.ToInt32(_grvColaborador.DataKeys[_grvColaborador.EditIndex].Value);
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada
    /// </summary>
    private string VS_Ordenacao
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Colaboradores)
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                string valor;
                if (filtros.TryGetValue("VS_Ordenacao", out valor))
                {
                    return valor;
                }
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada
    /// </summary>
    private SortDirection VS_SortDirection
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Colaboradores)
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
        _grvColaborador.PageSize = UCComboQtdePaginacao1.Valor;
        _grvColaborador.PageIndex = 0;

        // atualiza o grid
        _grvColaborador.DataBind();
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
            Dictionary<string, string> filtros = new Dictionary<string, string>();

            ESC_Escola entEscola = new ESC_Escola
            {
                esc_id = ucComboUAEscola.Esc_ID
            };
            if (ucComboUAEscola.Esc_ID > -1)
                ESC_EscolaBO.GetEntity(entEscola);

            _grvColaborador.PageIndex = 0;
            odsColaborador.SelectParameters.Clear();
            odsColaborador.SelectParameters.Add("pes_nome", _txtNome.Text);
            odsColaborador.SelectParameters.Add("coc_matricula", _txtMatricula.Text);
            odsColaborador.SelectParameters.Add("tipo_cpf", _txtCPF.Text);
            odsColaborador.SelectParameters.Add("tipo_rg", _txtRG.Text);
            odsColaborador.SelectParameters.Add("crg_id", UCComboCargo1.Valor.ToString());
            odsColaborador.SelectParameters.Add("fun_id", UCComboFuncao1.Valor.ToString());

            odsColaborador.SelectParameters.Add("uad_idSuperior", chkTodosColaboradores.Checked ? Guid.Empty.ToString() : ucComboUAEscola.Uad_ID.ToString());
            odsColaborador.SelectParameters.Add("uad_id", chkTodosColaboradores.Checked ? Guid.Empty.ToString() : entEscola.uad_id.ToString());

            //odsColaborador.SelectParameters.Add("uad_id", _txtUad_id.Value);
            odsColaborador.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao)
                odsColaborador.SelectParameters.Add("todosColaboradores", true.ToString());
            else
                odsColaborador.SelectParameters.Add("todosColaboradores", chkTodosColaboradores.Checked.ToString());

            odsColaborador.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
            odsColaborador.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
            odsColaborador.DataBind();

            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            _grvColaborador.Sort(VS_Ordenacao, VS_SortDirection);

            #region Salvar busca realizada com os parâmetros do ODS.

            foreach (Parameter param in odsColaborador.SelectParameters)
            {
                filtros.Add(param.Name, param.DefaultValue);
            }


                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.Colaboradores
                    ,
                    Filtros = filtros
                };

            #endregion Salvar busca realizada com os parâmetros do ODS.

            // mostra essa quantidade no combobox
            UCComboQtdePaginacao1.Valor = itensPagina;

            // atribui essa quantidade para o grid
            _grvColaborador.PageSize = itensPagina;

            // atualiza o grid
            _grvColaborador.DataBind();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os colaboradores.", UtilBO.TipoMensagem.Erro);
        }
        finally
        {
            _updBuscaColaborador.Update();
        }
    }

    /// <summary>
    /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
    /// </summary>
    private void VerificaBusca()
    {
        if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Colaboradores)
        {
            // Recuperar busca realizada e pesquisar automaticamente
            string valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("pes_nome", out valor);
            _txtNome.Text = valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("coc_matricula", out valor);
            _txtMatricula.Text = valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tipo_cpf", out valor);
            _txtCPF.Text = valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tipo_rg", out valor);
            _txtRG.Text = valor;

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("todosColaboradores", out valor);
            chkTodosColaboradores.Checked = Convert.ToBoolean(valor);

            if (ucComboUAEscola.VisibleUA)
            {
                if (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor))
                {
                    if (valor != (new Guid()).ToString())
                    {
                        ucComboUAEscola.Uad_ID = new Guid(valor);
                        ucComboUAEscola.EnableEscolas = ucComboUAEscola.Uad_ID != Guid.Empty;
                    }
                }
            }

            string esc_id, uni_id;
            if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
            {
                ucComboUAEscola.SelectedValueEscolas = new[]
                                                            {
                                                                Convert.ToInt32(esc_id)
                                                                , Convert.ToInt32(uni_id)
                                                            };
            }

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crg_id", out valor);
            UCComboCargo1.Valor = string.IsNullOrEmpty(valor) ? -1 : Convert.ToInt32(valor);

            __SessionWEB.BuscaRealizada.Filtros.TryGetValue("fun_id", out valor);
            UCComboFuncao1.Valor = string.IsNullOrEmpty(valor) ? -1 : Convert.ToInt32(valor);

            _Pesquisar();
        }
        else
        {
            fdsResultado.Visible = false;
        }
    }

    #endregion Métodos

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.SetFocus(_txtNome);

        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.UiAriaTabs));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsTabs.js"));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroColaborador.js"));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsBuscaColaboradores.js"));
        }

        UCComboQtdePaginacao1.GridViewRelacionado = _grvColaborador;

        if (!IsPostBack)
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
                _lblMessage.Text = message;

            _grvColaborador.PageSize = ApplicationWEB._Paginacao;

            try
            {
                SYS_TipoDocumentacao tdo = new SYS_TipoDocumentacao();
                string tdo_id = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF);
                if (!string.IsNullOrEmpty(tdo_id))
                {
                    tdo.tdo_id = new Guid(tdo_id);
                    SYS_TipoDocumentacaoBO.GetEntity(tdo);
                    _lblCPF.Text = tdo.tdo_sigla;
                    _grvColaborador.Columns[1].HeaderText = tdo.tdo_sigla;
                }
                else
                {
                    _lblCPF.Text = string.Empty;
                    _lblCPF.Visible = false;
                    _txtCPF.Visible = false;
                    _grvColaborador.Columns[1].HeaderText = string.Empty;
                    _grvColaborador.Columns[1].SortExpression = string.Empty;
                    _grvColaborador.Columns[1].HeaderStyle.CssClass = "hide";
                    _grvColaborador.Columns[1].ItemStyle.CssClass = "hide";
                }

                tdo_id = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_RG);
                if (!string.IsNullOrEmpty(tdo_id))
                {
                    tdo.tdo_id = new Guid(tdo_id);
                    SYS_TipoDocumentacaoBO.GetEntity(tdo);
                    _lblRG.Text = tdo.tdo_sigla;
                    _grvColaborador.Columns[2].HeaderText = tdo.tdo_sigla;
                }
                else
                {
                    _lblRG.Text = string.Empty;
                    _lblRG.Visible = false;
                    _txtRG.Visible = false;
                    _grvColaborador.Columns[2].HeaderText = string.Empty;
                    _grvColaborador.Columns[2].SortExpression = "";
                    _grvColaborador.Columns[2].HeaderStyle.CssClass = "hide";
                    _grvColaborador.Columns[2].ItemStyle.CssClass = "hide";
                }

                bool controleIntegracao = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_COLABORADOR_APENAS_INTEGRACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                _btnNovo.Visible = !controleIntegracao && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;

                ucComboUAEscola.Inicializar();
                UCComboCargo1.CarregarCargo();
                UCComboFuncao1.CarregarFuncao();

                chkTodosColaboradores.Visible = __SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Administracao;

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

            _grvColaborador.Columns[indiceColunaExcluir].Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
            _grvColaborador.Columns[indiceColunaCriarDocente].Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_CRIACAO_DOCENTE_POR_COLABORADOR, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }
    }

    protected void _grvColaborador_DataBound(object sender, EventArgs e)
    {
        UCTotalRegistros1.Total = RHU_ColaboradorBO.GetTotalRecords();

        // seta propriedades necessárias para ordenação nas colunas.
        ConfiguraColunasOrdenacao(_grvColaborador);

        if ((!string.IsNullOrEmpty(_grvColaborador.SortExpression)) &&
           (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Colaboradores))
        {
            Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

            if (filtros.ContainsKey("VS_Ordenacao"))
            {
                filtros["VS_Ordenacao"] = _grvColaborador.SortExpression;
            }
            else
            {
                filtros.Add("VS_Ordenacao", _grvColaborador.SortExpression);
            }

            if (filtros.ContainsKey("VS_SortDirection"))
            {
                filtros["VS_SortDirection"] = _grvColaborador.SortDirection.ToString();
            }
            else
            {
                filtros.Add("VS_SortDirection", _grvColaborador.SortDirection.ToString());
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.Colaboradores
                ,
                Filtros = filtros
            };
        }
    }

    protected void _grvColaborador_RowDataBound(object sender, GridViewRowEventArgs e)
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

            // Exibe o botão para criação do docente a partir do colaborador
            int doc_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "doc_id"));
            if (doc_id == 0)
            {
                ImageButton btnCriaDocente = (ImageButton)e.Row.FindControl("btnCriaDocente");
                if (btnCriaDocente != null)
                {
                    btnCriaDocente.CommandArgument = e.Row.RowIndex.ToString();
                    btnCriaDocente.Visible = true;
                }
            }

        }
    }

    protected void _grvColaborador_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Deletar")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());
                long col_id = Convert.ToInt32(_grvColaborador.DataKeys[index].Value);

                RHU_Colaborador entity = new RHU_Colaborador { col_id = col_id };
                RHU_ColaboradorBO.GetEntity(entity);

                if (RHU_ColaboradorBO.Delete(entity, null, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    _grvColaborador.PageIndex = 0;
                    _grvColaborador.DataBind();
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "col_id: " + col_id);
                    _lblMessage.Text = UtilBO.GetErroMessage("Colaborador excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o colaborador.", UtilBO.TipoMensagem.Erro);
            }
        }

        if (e.CommandName == "CriaDocente")
        {
            try
            {
                int index = int.Parse(e.CommandArgument.ToString());
                long col_id = Convert.ToInt32(_grvColaborador.DataKeys[index].Value);

                Session["col_id"] = col_id;
                RedirecionarPagina("~/Academico/RecursosHumanos/Docente/Cadastro.aspx");
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }            
        }
    }

    protected void _btnLimparPesquisa_Click(object sender, EventArgs e)
    {
        // Limpa busca da sessão.
        __SessionWEB.BuscaRealizada = new BuscaGestao();
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/RecursosHumanos/Colaborador/Busca.aspx", false);
    }

    protected void _btnNovo_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/RecursosHumanos/Colaborador/Cadastro.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void _btnPesquisar_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(_txtCPF.Text))
        {
            if (!UtilBO._ValidaCPF(_txtCPF.Text))
                _lblMessage.Text = UtilBO.GetErroMessage("CPF é inválido.", UtilBO.TipoMensagem.Alerta);
            else
            {
                _Pesquisar();
                fdsResultado.Visible = true;
            }
        }
        else
        {
            _Pesquisar();
            fdsResultado.Visible = true;
        }
    }

    #endregion Eventos
}
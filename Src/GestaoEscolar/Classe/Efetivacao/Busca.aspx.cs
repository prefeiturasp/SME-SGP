using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

public partial class Classe_Efetivacao_Busca : MotherPageLogado
{
    #region Delegates

    protected void UCComboQtdePaginacao_IndexChanged()
    {
        try
        {
            VS_cancelSelect = false;
            _odsTurma.SelectMethod = __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual ? "GetSelectBy_Docente_Efetivacao_TodosTipos" : "GetSelectBy_Pesquisa_TodosTipos";

            // atribui nova quantidade itens por página para o grid
            _dgvTurma.PageSize = UCComboQtdePaginacao1.Valor;
            _dgvTurma.PageIndex = 0;
            // atualiza o grid
            _dgvTurma.DataBind();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as turmas.", UtilBO.TipoMensagem.Erro);
        }
    }

    private void UCBuscaLancamentoClasse1_OnPesquisar()
    {
        PesquisarTurmas();
    }

    #endregion Delegates

    #region Propriedades

    /// <summary>
    /// Propriedade na qual cria variavel em ViewState armazenando valor de tur_id
    /// </summary>
    public int _VS_tur_id
    {
        get
        {
            if (ViewState["_VS_tur_id"] != null)
                return Convert.ToInt32(ViewState["_VS_tur_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_tur_id"] = value;
        }
    }

    /// <summary>
    /// Propriedade na qual cria variavel em ViewState armazenando valor de fav_id
    /// </summary>
    public int _VS_fav_id
    {
        get
        {
            if (ViewState["_VS_fav_id"] != null)
                return Convert.ToInt32(ViewState["_VS_fav_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_fav_id"] = value;
        }
    }

    /// <summary>
    /// Propriedade na qual cria variavel em ViewState armazenando valor de doc_id
    /// </summary>
    private long _VS_doc_id
    {
        get
        {
            if (ViewState["_VS_doc_id"] != null)
                return Convert.ToInt64(ViewState["_VS_doc_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_doc_id"] = value;
        }
    }

    /// <summary>
    /// ID da avaliação selecionada.
    /// </summary>
    public int _VS_ava_id
    {
        get
        {
            if (ViewState["_VS_ava_id"] != null)
                return Convert.ToInt32(ViewState["_VS_ava_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_ava_id"] = value;
        }
    }

    private string _nomeModulo;

    /// <summary>
    /// Propriedade com o nome do modulo.
    /// </summary>
    private string NomeModulo
    {
        get
        {
            try
            {
                if (string.IsNullOrEmpty(_nomeModulo))
                {
                    SYS_Modulo entModulo;
                    if (Modulo.IsNew)
                    {
                        entModulo = new SYS_Modulo
                        {
                            mod_id = __SessionWEB.__UsuarioWEB.GrupoPermissao.mod_id,
                            sis_id = __SessionWEB.__UsuarioWEB.GrupoPermissao.sis_id
                        };
                        entModulo = GestaoEscolarUtilBO.GetEntityModuloCache(entModulo);
                    }
                    else
                    {
                        entModulo = Modulo;
                    }

                    _nomeModulo = string.IsNullOrEmpty(entModulo.mod_nome) ? "Efetivação de notas " : entModulo.mod_nome;
                }

                return _nomeModulo;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);

                return "Efetivação de notas";
            }
        }
    }

    /// <summary>
    /// Guarda o sortExpression da coluna ordenada
    /// </summary>
    private string VS_Ordenacao
    {
        get
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Efetivacao)
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
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Efetivacao)
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

    /// <summary>
    /// Cancela o consulta do ObjectDataSource ao carregar a página pela primeira vez.
    /// </summary>
    private bool VS_cancelSelect
    {
        get
        {
            return Convert.ToBoolean(ViewState["VS_cancelSelect"] ?? false);
        }

        set
        {
            ViewState["VS_cancelSelect"] = value;
        }
    }

    #endregion Propriedades

    #region Constantes

    private const int COLUNA_CURSO = 3;

    #endregion Constantes

    #region Métodos

    /// <summary>
    /// Atualiza o grid de turmas com os filtros informados.
    /// </summary>
    private void PesquisarTurmas()
    {
        try
        {
            VS_cancelSelect = false;

            Dictionary<string, string> filtros = new Dictionary<string, string>();

            _dgvTurma.PageIndex = 0;
            _odsTurma.SelectParameters.Clear();

            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
            {
                _odsTurma.SelectMethod = "GetSelectBy_Docente_Efetivacao_TodosTipos";
            }
            else
            {
                _odsTurma.SelectMethod = "GetSelectBy_Pesquisa_TodosTipos";
                _odsTurma.SelectParameters.Add("uad_idSuperior", UCBuscaLancamentoClasse1.UadIdSuperior.ToString());
                _odsTurma.SelectParameters.Add("esc_id", UCBuscaLancamentoClasse1.EscId.ToString());
                _odsTurma.SelectParameters.Add("uni_id", UCBuscaLancamentoClasse1.UniId.ToString());
                _odsTurma.SelectParameters.Add("cur_id", UCBuscaLancamentoClasse1.CurId.ToString());
                _odsTurma.SelectParameters.Add("crr_id", UCBuscaLancamentoClasse1.CrrId.ToString());
                _odsTurma.SelectParameters.Add("cal_id", UCBuscaLancamentoClasse1.CalId.ToString());
                _odsTurma.SelectParameters.Add("trn_id", UCBuscaLancamentoClasse1.TrnId.ToString());
                _odsTurma.SelectParameters.Add("tur_codigo", UCBuscaLancamentoClasse1.TurCodigo);
                _odsTurma.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
                _odsTurma.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
                _odsTurma.SelectParameters.Add("adm", (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao).ToString());
            }

            _odsTurma.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
            _odsTurma.SelectParameters.Add("doc_id", _VS_doc_id.ToString());

            // atualiza o grid
            _dgvTurma.Sort(VS_Ordenacao, VS_SortDirection);

            #region Salvar busca realizada com os parâmetros do ODS.

            foreach (Parameter param in _odsTurma.SelectParameters)
            {
                filtros.Add(param.Name, param.DefaultValue);
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.Efetivacao
                ,
                Filtros = filtros
            };

            #endregion Salvar busca realizada com os parâmetros do ODS.

            _dgvTurma.DataBind();

            pnlResultado.Visible = true;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as turmas.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Seta as variáveis necessárias e redireciona pro cadastro.
    /// </summary>
    private void RedirecionaCadastro()
    {
        Session["tur_idEfetivacao"] = _VS_tur_id;
        Session["tud_idEfetivacao"] = -1;
        Session["fav_idEfetivacao"] = _VS_fav_id;
        Session["ava_idEfetivacao"] = _VS_ava_id;
        Session["URL_Retorno_Efetivacao"] = Convert.ToByte(URL_Retorno_Efetivacao.EfetivacaoBusca);

        Response.Redirect("~/Classe/Efetivacao/Cadastro.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    #endregion Métodos

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        VS_cancelSelect = true;

        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference("~/Includes/jsBuscaEfetivacao.js"));
        }

        UCBuscaLancamentoClasse1.OnPesquisar += UCBuscaLancamentoClasse1_OnPesquisar;

        UCComboQtdePaginacao1.GridViewRelacionado = _dgvTurma;

        if (!IsPostBack)
        {
            UCBuscaLancamentoClasse1.PaginaBusca = PaginaGestao.Efetivacao;
            UCBuscaLancamentoClasse1.GroupingText = "Consulta de " + NomeModulo;

            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
            {
                _lblMessage.Text = message;
            }

            try
            {
                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                // mostra essa quantidade no combobox
                UCComboQtdePaginacao1.Valor = itensPagina;
                // atribui essa quantidade para o grid
                _dgvTurma.PageSize = itensPagina;

                pnlResultado.Visible = false;
                _dgvTurma.Columns[COLUNA_CURSO].HeaderText = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id);

                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                {
                    UCBuscaLancamentoClasse1.Visible = false;

                    // Busca o doc_id do usuário logado.
                    if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                    {
                        pnlResultado.Visible = true;
                        gvAvaliacoes.Columns[1].Visible = false;
                        _VS_doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                        PesquisarTurmas();
                    }
                    else
                    {
                        pnlResultado.Visible = false;
                        _lblMessage.Text = UtilBO.GetErroMessage("Essa tela é exclusiva para docentes.", UtilBO.TipoMensagem.Alerta);
                    }
                }
                else
                {
                    UCBuscaLancamentoClasse1.Inicializar();
                    UCBuscaLancamentoClasse1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }

            Page.Form.DefaultButton = UCBuscaLancamentoClasse1.Pesquisar_UniqueID;
        }
    }

    protected void _dgvTurma_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        VS_cancelSelect = false;
        _odsTurma.SelectMethod = __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual
                                     ? "GetSelectBy_Docente_Efetivacao_TodosTipos"
                                     : "GetSelectBy_Pesquisa_TodosTipos";
    }

    protected void _dgvTurma_Sorting(object sender, GridViewSortEventArgs e)
    {
        VS_cancelSelect = false;
        _odsTurma.SelectMethod = __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual
                                     ? "GetSelectBy_Docente_Efetivacao_TodosTipos"
                                     : "GetSelectBy_Pesquisa_TodosTipos";
    }

    protected void _dgvTurma_DataBound(object sender, EventArgs e)
    {
        UCComboQtdePaginacao1.Visible = !_dgvTurma.Rows.Count.Equals(0);

        UCTotalRegistros1.Total = TUR_TurmaBO.GetTotalRecords();
        ConfiguraColunasOrdenacao(_dgvTurma);

        if ((!string.IsNullOrEmpty(_dgvTurma.SortExpression)) &&
         (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.Efetivacao))
        {
            Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

            if (filtros.ContainsKey("VS_Ordenacao"))
            {
                filtros["VS_Ordenacao"] = _dgvTurma.SortExpression;
            }
            else
            {
                filtros.Add("VS_Ordenacao", _dgvTurma.SortExpression);
            }

            if (filtros.ContainsKey("VS_SortDirection"))
            {
                filtros["VS_SortDirection"] = _dgvTurma.SortDirection.ToString();
            }
            else
            {
                filtros.Add("VS_SortDirection", _dgvTurma.SortDirection.ToString());
            }

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.Efetivacao
                ,
                Filtros = filtros
            };
        }
    }

    protected void _dgvTurma_RowDataBound(object sender, GridViewRowEventArgs e)
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
                _btnAlterar.Attributes.Add("onClick", "TopoDaPagina()");
                if (!Convert.ToString(_btnAlterar.CssClass).Contains("subir"))
                {
                    _btnAlterar.CssClass += " subir";
                }
            }
        }
    }

    protected void _dgvTurma_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Selecionar")
        {
            try
            {
                VS_cancelSelect = false;
                GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;

                int index = row.RowIndex;
                _VS_tur_id = Convert.ToInt32(_dgvTurma.DataKeys[index].Values["tur_id"]);

                // Utilizado para verificar se é turma eletiva do aluno
                TUR_Turma tur = new TUR_Turma { tur_id = _VS_tur_id };
                TUR_TurmaBO.GetEntity(tur);

                // verifica se existe evento de efetivacao ligado ao calendário da turma
                int cal_id = Convert.ToInt32(_dgvTurma.DataKeys[index].Values["cal_id"]);

                // Busca o evento ligado ao calendário, que seja do tipo definido
                // no parâmetro como de efetivação.
                List<ACA_Evento> listEvento = ACA_EventoBO.GetEntity_Efetivacao_List(cal_id, _VS_tur_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo, true, __SessionWEB.__UsuarioWEB.Docente.doc_id);

                if (string.IsNullOrEmpty(_dgvTurma.DataKeys[index].Values["fav_id"].ToString()))
                {
                    _lblMessage.Text = UtilBO.GetErroMessage("É necessário selecionar uma turma que possua um formato de avaliação.", UtilBO.TipoMensagem.Alerta);
                }
                else
                {
                    _VS_fav_id = Convert.ToInt32(_dgvTurma.DataKeys[index].Values["fav_id"]);

                    int valor = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    int valorRecuperacao = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_RECUPERACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    int valorFinal = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    int valorRecuperacaoFinal = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_RECUPERACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    // verifica se existe evento do tipo Efetivação Nota
                    string listaTpcIdPeriodicaPeriodicaFinal = string.Empty;
                    IEnumerable<ACA_Evento> dadoNota =
                        (from ACA_Evento item in listEvento
                         where item.tev_id == valor
                         select item);
                    // se existir, pega os tpc_id's
                    List<ACA_Evento> lt = dadoNota.ToList();
                    if (lt.Count > 0)
                    {
                        var x = from ACA_Evento evt in listEvento
                                where evt.tev_id == valor
                                select evt.tpc_id;

                        foreach (int tpc_id in x.ToList())
                        {
                            if (string.IsNullOrEmpty(listaTpcIdPeriodicaPeriodicaFinal))
                                listaTpcIdPeriodicaPeriodicaFinal += Convert.ToString(tpc_id);
                            else
                                listaTpcIdPeriodicaPeriodicaFinal += "," + Convert.ToString(tpc_id);
                        }
                    }

                    // verifica se existe evento do tipo efetivação recuperacao
                    string listaTpcIdRecuperacao = string.Empty;
                    IEnumerable<ACA_Evento> dadoRecuperacao =
                        (from ACA_Evento item in listEvento
                         where
                             item.tev_id == valorRecuperacao
                         select item);
                    List<ACA_Evento> ltRe = dadoRecuperacao.ToList();
                    // se existir, pega os tpc_id's
                    if (ltRe.Count > 0)
                    {
                        var x = from ACA_Evento evt in listEvento
                                where
                                    evt.tev_id == valorRecuperacao
                                select evt.tpc_id;

                        foreach (int tpc_id in x.ToList())
                        {
                            if (string.IsNullOrEmpty(listaTpcIdRecuperacao))
                                listaTpcIdRecuperacao += Convert.ToString(tpc_id);
                            else
                                listaTpcIdRecuperacao += "," + Convert.ToString(tpc_id);
                        }
                    }

                    // verifica se existe evento do tipo efetivação final
                    bool existeFinal = false;
                    IEnumerable<ACA_Evento> dadoFinal =
                        (from ACA_Evento item in listEvento
                         where
                             item.tev_id == valorFinal
                         select item);
                    List<ACA_Evento> ltFinal = dadoFinal.ToList();
                    // se existir, marca para trazer as avaliações do tipo final
                    if (ltFinal.Count > 0)
                    {
                        existeFinal = true;
                    }

                    // verifica se existe evento do tipo recuperação final
                    bool existeRecuperacaoFinal = false;
                    IEnumerable<ACA_Evento> dadoRecuperacaoFinal =
                        (from ACA_Evento item in listEvento
                         where
                             item.tev_id == valorRecuperacaoFinal
                         select item);
                    List<ACA_Evento> ltRecuperacaoFinal = dadoRecuperacaoFinal.ToList();
                    // se existir, marca para trazer as avaliações do tipo recuperação final
                    if (ltRecuperacaoFinal.Count > 0)
                    {
                        existeRecuperacaoFinal = true;
                    }

                    DataTable dtAvaliacoes;

                    // Se for turma eletiva do aluno, carrega apenas os períodos do calendário em que
                    // a turma é oferecida
                    if ((TUR_TurmaTipo)tur.tur_tipo == TUR_TurmaTipo.EletivaAluno)
                    {
                        List<CadastroTurmaDisciplina> listaDisciplinas = TUR_TurmaDisciplinaBO.GetSelectCadastradosBy_Turma(_VS_tur_id);
                        dtAvaliacoes = ACA_AvaliacaoBO.ConsultaPor_Periodo_Efetivacao_TurmaDisciplinaCalendario(_VS_tur_id, listaDisciplinas[0].entTurmaDisciplina.tud_id, _VS_fav_id, listaTpcIdPeriodicaPeriodicaFinal, listaTpcIdRecuperacao, existeFinal, false, true);
                    }
                    else
                    {
                        dtAvaliacoes = ACA_AvaliacaoBO.ConsultaPor_Periodo_Efetivacao(_VS_tur_id, _VS_fav_id, 0, listaTpcIdPeriodicaPeriodicaFinal, listaTpcIdRecuperacao, existeFinal, existeRecuperacaoFinal, false, true,-1,ApplicationWEB.AppMinutosCacheLongo);
                    }

                    var avaliacoes = (from DataRow dr in dtAvaliacoes.Rows
                                      let fechado = Convert.ToBoolean(dr["ava_tpc_fechado"])
                                      let cap_dataInicio = Convert.ToDateTime(string.IsNullOrEmpty(dr["cap_dataInicio"].ToString()) ? new DateTime().ToString() : dr["cap_dataInicio"])
                                      where !(fechado && cap_dataInicio > DateTime.Now)
                                      select dr);

                    dtAvaliacoes = avaliacoes.Any() ? avaliacoes.CopyToDataTable() : new DataTable();

                    if (dtAvaliacoes.Rows.Count == 0)
                        _lblMessage.Text = UtilBO.GetErroMessage("Turma fora do período de " + NomeModulo + ".", UtilBO.TipoMensagem.Alerta);
                    else if (dtAvaliacoes.Rows.Count == 1)
                    {
                        ACA_FormatoAvaliacao ent = new ACA_FormatoAvaliacao { fav_id = _VS_fav_id };
                        ACA_FormatoAvaliacaoBO.GetEntity(ent);

                        _VS_ava_id = Convert.ToInt32(dtAvaliacoes.Rows[0]["ava_id"]);

                        RedirecionaCadastro();
                    }
                    else if (dtAvaliacoes.Rows.Count > 1)
                    {
                        // Carregar Avaliações.
                        gvAvaliacoes.DataSource = dtAvaliacoes;
                        gvAvaliacoes.DataBind();

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "abreAvaliacoes", "$(document).ready(function(){$('#divAvaliacoes').dialog('open'); });", true);
                    }
                }
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as avaliações.", UtilBO.TipoMensagem.Erro);
            }
        }
    }

    protected void _grvPeriodos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton _btnSelecionar = (LinkButton)e.Row.FindControl("_btnSelecionar");
            if (_btnSelecionar != null)
            {
                _btnSelecionar.CommandArgument = e.Row.RowIndex.ToString();
            }
        }
    }

    protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("SelecionarAvaliacao"))
            {
                _VS_ava_id = Convert.ToInt32(e.CommandArgument);

                RedirecionaCadastro();
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void _odsTurma_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.Cancel = VS_cancelSelect;
        if (!e.Cancel)
        {
            _odsTurma.SelectMethod = __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual
                                     ? "GetSelectBy_Docente_Efetivacao_TodosTipos"
                                     : "GetSelectBy_Pesquisa_TodosTipos";
        }
    }

    #endregion Eventos
}
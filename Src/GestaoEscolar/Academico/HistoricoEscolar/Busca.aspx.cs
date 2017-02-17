using DevExpress.XtraReports.UI;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Academico.HistoricoEscolar
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Alunos selecionados no grid para impressao de seus documentos
        /// </summary>
        private SortedDictionary<long, bool> _VS_AlunosSelecionados
        {
            get
            {
                if (ViewState["_VS_AlunosSelecionados"] == null)
                    ViewState["_VS_AlunosSelecionados"] = new SortedDictionary<long, bool>();
                return (SortedDictionary<long, bool>)ViewState["_VS_AlunosSelecionados"];
            }
        }

        private Boolean _VS_PesquisaSalva
        {
            get
            {
                return (Boolean)ViewState["_VS_PesquisaSalva"];
            }
            set
            {
                ViewState["_VS_PesquisaSalva"] = value;
            }
        }

        /// <summary>
        /// Guarda o ID do aluno
        /// </summary>
        private long VS_alu_id
        {
            get { return Convert.ToInt64(ViewState["VS_alu_id"]); }
            set { ViewState["VS_alu_id"] = value; }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private string VS_Ordenacao
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.HistoricoEscolarPedagogico)
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
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.HistoricoEscolarPedagogico)
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
        /// Guarda o ID da unidade administrativa superior
        /// </summary>
        private Guid VS_uad_idSuperior
        {
            get { return new Guid(ViewState["VS_uad_idSuperior"].ToString()); }
            set { ViewState["VS_uad_idSuperior"] = value; }
        }

        public long EditItem
        {
            get
            {
                return Convert.ToInt64(_grvDocumentoAluno.DataKeys[_grvDocumentoAluno.EditIndex].Values["alu_id"]);
            }
        }

        /// <summary>
        /// Guarda o modulo para qual a pagina será direcionada após a busca
        /// </summary>
        private int VS_mod
        {
            get
            {
                if (ViewState["VS_mod"] != null)
                    return Convert.ToInt32(ViewState["VS_mod"]);

                return -1;
            }
            set
            {
                ViewState["VS_mod"] = value;
            }
        }

        #endregion Propriedades

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            string mod = Request.QueryString["mod"];
            if (mod != null)
                VS_mod = Convert.ToInt32(mod);

            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.Json));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
            }

            UCComboCursoCurriculo1.IndexChanged += UCComboCursoCurriculo1_IndexChanged;
            UCComboCurriculoPeriodo1.IndexChanged += UCComboCurriculoPeriodo1__OnSelectedIndexChange;
            UCComboCalendario1.IndexChanged += UCComboCalendario1_IndexChanged;

            if (!IsPostBack)
            {
                _VS_PesquisaSalva = false;
                //_VS_SelecionarTodos = false;

                try
                {
                    string message = __SessionWEB.PostMessages;
                    if (!String.IsNullOrEmpty(message))
                    {
                        _lblMessage.Text = message;
                        _lblMessage.Focus();
                        __SessionWEB.PostMessages = String.Empty;
                    }

                    if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                    {
                        uppPesquisa.Visible = false;
                        _lblMessage.Text = UtilBO.GetErroMessage("Você não possui permissão para acessar a página solicitada.", UtilBO.TipoMensagem.Alerta);
                        _lblMessage.Focus();
                    }

                    Page.Form.DefaultFocus = UCComboUAEscola.ComboEscola_ClientID;
                    Page.Form.DefaultButton = _btnPesquisar.UniqueID;

                    // quantidade de itens por página
                    string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                    _ddlQtPaginado.SelectedValue = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao.ToString() : qtItensPagina;

                    // ******************************

                    string nomeMatriculaEstadual = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    bool mostraMatriculaEstadual = !string.IsNullOrEmpty(nomeMatriculaEstadual);

                    UCCamposBuscaAluno1.MostrarMatriculaEstadual = mostraMatriculaEstadual;
                    UCCamposBuscaAluno1.TituloMatriculaEstadual = nomeMatriculaEstadual;

                    fdsResultados.Visible = false;

                    Inicializar();
                    VerificaBusca();

                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                    _lblMessage.Focus();
                }

            }
        }

        #endregion Page Life Cycle

        #region Eventos

        protected void _btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validação de preenchimento para o HistoricoEscolarSMESP
                if ((string.IsNullOrEmpty(UCCamposBuscaAluno1.NomeAluno) && string.IsNullOrEmpty(UCCamposBuscaAluno1.MatriculaAluno)) && chkBuscaAvancada.Checked)
                    throw new ValidationException("Nome ou " + GetGlobalResourceObject("Mensagens", "MSG_NUMEROMATRICULA").ToString().ToLower() + " do aluno deve ser preenchido.");

                Pesquisar(0, true);
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                _lblMessage.Focus();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alunos.", UtilBO.TipoMensagem.Erro);
                _lblMessage.Focus();
            }
        }

        protected void _ddlQtPaginado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_grvDocumentoAluno.Rows.Count.Equals(0))
            {
                // Seta propriedades necessárias para ordenação nas colunas.
                ConfiguraColunasOrdenacao(_grvDocumentoAluno, VS_Ordenacao, VS_SortDirection);

                Pesquisar(0, false);
            }
        }

        protected void grvDocumentoAluno_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Pesquisar(e.NewPageIndex, false);
        }

        protected void _grvDocumentoAluno_DataBound(object sender, EventArgs e)
        {
            string sDeclaracaoHMTL = string.Empty;

            UCTotalRegistros1.Total = ACA_AlunoBO.GetTotalRecords();

            // Seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(_grvDocumentoAluno, VS_Ordenacao, VS_SortDirection);
                }

        protected void _grvDocumentoAluno_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnEditar = (ImageButton)e.Row.FindControl("btnEditar");
                if (btnEditar != null)
                {
                    btnEditar.CommandArgument = e.Row.RowIndex.ToString();
                }
                ImageButton btnRelatorio = (ImageButton)e.Row.FindControl("btnRelatorio");
                if (btnRelatorio != null)
                {
                    btnRelatorio.CommandArgument = e.Row.RowIndex.ToString();
                }
            }
        }

        protected void odsDocumentoAluno_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                if (e.Exception.InnerException is ValidationException)
                {
                    _lblMessage.Text = UtilBO.GetErroMessage(e.Exception.InnerException.Message, UtilBO.TipoMensagem.Alerta);
                    _lblMessage.Focus();
                }
                else
                {
                    ApplicationWEB._GravaErro(e.Exception);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alunos.", UtilBO.TipoMensagem.Erro);
                    _lblMessage.Focus();
                }

                e.ExceptionHandled = true;
            }
        }

        protected void _grvDocumentoAluno_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    Session["alu_id"] = _grvDocumentoAluno.DataKeys[index].Values["alu_id"].ToString();
                    Session["mtu_id"] = _grvDocumentoAluno.DataKeys[index].Values["mtu_id"].ToString();

                    switch (VS_mod)
                    {
                        case 1:
                            RedirecionarPagina("DadosAluno.aspx");
                            break;
                        case 2:
                            RedirecionarPagina("EnsinoFundamental.aspx");
                            break;
                        case 3:
                            RedirecionarPagina("Transferencia.aspx");
                            break;
                        case 4:
                            RedirecionarPagina("InformacoesComplementares.aspx");
                            break;
                        default:
                            RedirecionarPagina("DadosAluno.aspx");
                            break;
                    }

                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar editar o histórico.", UtilBO.TipoMensagem.Erro);
                    _lblMessage.Focus();
                }
            }
            else if (e.CommandName == "HistoricoEscolar")
            {
                int index = int.Parse(e.CommandArgument.ToString());

                string alu_id = _grvDocumentoAluno.DataKeys[index].Values["alu_id"].ToString();
                string mtu_id = _grvDocumentoAluno.DataKeys[index].Values["mtu_id"].ToString();

                XtraReport DevReport = new MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo.HistoricoEscolar
                        (alu_id,
                         mtu_id,
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                        GetGlobalResourceObject("Reporting", "Reporting.DocHistoricoEscolarPedagogico.Municipio").ToString(),
                        GetGlobalResourceObject("Reporting", "Reporting.DocHistoricoEscolarPedagogico.Secretaria").ToString());

                SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);

                GestaoEscolarUtilBO.SendParametersToReport(DevReport);
                Response.Redirect("~/Documentos/RelatorioDev.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        protected void chkBuscaAvancada_CheckedChanged(object sender, EventArgs e)
        {
            divBuscaAvancadaAluno.Visible = chkBuscaAvancada.Checked;

            setaObrigatorioUC(!chkBuscaAvancada.Checked);

            UCCamposBuscaAluno1.NomeAluno = "";
            UCCamposBuscaAluno1.MatriculaAluno = "";

            uppPesquisa.Update();
        }

        protected void _grvDocumentoAluno_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridView grid = ((GridView)(sender));

            if (!string.IsNullOrEmpty(e.SortExpression))
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

                SortDirection sortDirection = VS_SortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;

                if (filtros.ContainsKey("VS_Ordenacao"))
                {
                    filtros["VS_Ordenacao"] = e.SortExpression;
                }
                else
                {
                    filtros.Add("VS_Ordenacao", e.SortExpression);
                }

                if (filtros.ContainsKey("VS_SortDirection"))
                {
                    filtros["VS_SortDirection"] = sortDirection.ToString();
                }
                else
                {
                    filtros.Add("VS_SortDirection", sortDirection.ToString());
                }

                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.HistoricoEscolarPedagogico
                    ,
                    Filtros = filtros
                };
            }

            Pesquisar(grid.PageIndex, false);
        }

        #endregion Eventos

        #region Delegates

        protected void UCComboUAEscola_IndexChangedUA()
        {
            if (UCComboUAEscola.Esc_ID < 0)
                UCComboUAEscola.SelectedValueEscolas = new[] { -1, -1 };

            UCComboUAEscola_IndexChangedUnidadeEscola();
        }

        protected void UCComboUAEscola_IndexChangedUnidadeEscola()
        {
            try
            {
                UCComboCursoCurriculo1.Valor = new[] { -1, -1 };
                UCComboCursoCurriculo1.PermiteEditar = false;

                if (UCComboUAEscola.Esc_ID > 0 && UCComboUAEscola.Uni_ID > 0)
                {
                    UCComboCursoCurriculo1.CarregarPorEscola(UCComboUAEscola.Esc_ID,
                                                                           UCComboUAEscola.Uni_ID);

                    UCComboCursoCurriculo1.SetarFoco();
                    UCComboCursoCurriculo1.PermiteEditar = true;
                }

                UCComboCursoCurriculo1_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                _lblMessage.Focus();
            }
        }

        private void UCComboCalendario1_IndexChanged()
        {
            try
            {
                if (!_VS_PesquisaSalva)
                {
                    UCComboTurma1.Valor = new long[] { -1, -1, -1 };
                    UCComboTurma1.PermiteEditar = false;

                    if (UCComboCalendario1.Valor > 0)
                    {
                        UCComboTurma1.CarregarPorEscolaCalendarioEPeriodo(UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID, UCComboCalendario1.Valor, UCComboCurriculoPeriodo1.Valor[0], UCComboCurriculoPeriodo1.Valor[1], UCComboCurriculoPeriodo1.Valor[2], (byte)TUR_TurmaSituacao.Ativo);
                        UCComboTurma1.PermiteEditar = true;
                        UCComboTurma1.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a(s) turma(s) do período.", UtilBO.TipoMensagem.Erro);
                _lblMessage.Focus();
            }
        }

        private void UCComboCurriculoPeriodo1__OnSelectedIndexChange()
        {
            try
            {
                if (!_VS_PesquisaSalva)
                {
                    UCComboCalendario1.Valor = -1;
                    UCComboCalendario1.PermiteEditar = false;

                    if (UCComboCurriculoPeriodo1.Valor[0] > 0)
                    {
                        UCComboCalendario1.CarregarPorCurso(UCComboCurriculoPeriodo1.Valor[0]);
                        UCComboCalendario1.PermiteEditar = true;
                        UCComboCalendario1.SetarFoco();
                    }

                    UCComboCalendario1_IndexChanged();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o(s) calendários(s).", UtilBO.TipoMensagem.Erro);
                _lblMessage.Focus();
            }
        }

        private void UCComboCursoCurriculo1_IndexChanged()
        {
            try
            {
                if (!_VS_PesquisaSalva)
                {
                    UCComboCurriculoPeriodo1.Valor = new[] { -1, -1, -1 };
                    UCComboCurriculoPeriodo1.PermiteEditar = false;
                    //UCComboCalendario1.Valor = -1;

                    if (UCComboCursoCurriculo1.Valor[0] > 0)
                    {
                        UCComboCurriculoPeriodo1.CarregarPorCursoCurriculo(UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1]);
                        UCComboCurriculoPeriodo1.PermiteEditar = true;
                        UCComboCurriculoPeriodo1.Focus();

                        //UCComboCalendario1.CarregarPorCurso(UCComboCursoCurriculo1.Valor[0]);
                        //UCComboCalendario1.PermiteEditar = true;
                    }

                    UCComboCurriculoPeriodo1__OnSelectedIndexChange();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                _lblMessage.Focus();
            }
        }

        #endregion Delegates

        #region Métodos

        protected void Inicializar()
        {
            if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
            {
                UCComboUAEscola.InicializarVisaoIndividual(__SessionWEB.__UsuarioWEB.Docente.doc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, 2);
            }
            else
            {
                UCComboUAEscola.Inicializar();    
            }

            if (UCComboUAEscola.Esc_ID != -1)
                UCComboUAEscola_IndexChangedUA();
        }

        /// <summary>
        /// Pesquisa os alunos de acordo com os filtros de busca definidos.
        /// </summary>
        protected void Pesquisar(int pageIndex, bool alteraSessaoBusca)
        {
            ACA_AlunoBO.numeroCursosPeja = 0;

            // ******************************

            fdsResultados.Visible = true;

            _grvDocumentoAluno.DataSource = 
                ACA_AlunoBO.BuscaAlunos_HistoricoEscolarPedagogico
                    (
                        UCComboCalendario1.Valor,
                        UCComboUAEscola.Esc_ID,
                        UCComboUAEscola.Uni_ID,
                        UCComboCursoCurriculo1.Valor[0],
                        UCComboCursoCurriculo1.Valor[1],
                        UCComboCurriculoPeriodo1.Valor[2],
                        UCComboTurma1.Valor[0],
                        Convert.ToByte(UCCamposBuscaAluno1.TipoBuscaNomeAluno),
                        UCCamposBuscaAluno1.NomeAluno,
                        UCCamposBuscaAluno1.MatriculaAluno,
                        __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                        UCComboUAEscola.Uad_ID,
                        (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
                        __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                        __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                        true,
                        Convert.ToInt32(_ddlQtPaginado.SelectedValue),
                        pageIndex,
                        (int)VS_SortDirection,
                        VS_Ordenacao,
                        true
                    );

            // atribui essa quantidade para o grid
            _grvDocumentoAluno.PageSize = Convert.ToInt32(_ddlQtPaginado.SelectedValue);
            _grvDocumentoAluno.PageIndex = pageIndex;
            _grvDocumentoAluno.VirtualItemCount = ACA_AlunoBO.GetTotalRecords();

            _grvDocumentoAluno.DataBind();

            divQtdPaginacao.Visible = _grvDocumentoAluno.Rows.Count > 0;

            #region Salvar busca realizada com os parâmetros do ODS.

            if (alteraSessaoBusca)
        {
                Dictionary<string, string> filtros = new Dictionary<string, string>();

                filtros.Add("uad_idSuperior", UCComboUAEscola.Uad_ID.ToString());
                filtros.Add("esc_id", UCComboUAEscola.Esc_ID.ToString());
                filtros.Add("uni_id", UCComboUAEscola.Uni_ID.ToString());
                filtros.Add("cur_id", UCComboCursoCurriculo1.Valor[0].ToString());
                filtros.Add("crr_id", UCComboCursoCurriculo1.Valor[1].ToString());
                filtros.Add("crp_id", UCComboCurriculoPeriodo1.Valor[2].ToString());
                filtros.Add("cal_id", UCComboCalendario1.Valor.ToString());
                filtros.Add("tur_id", UCComboTurma1.Valor[0].ToString());
                filtros.Add("tipoBusca", UCCamposBuscaAluno1.TipoBuscaNomeAluno);
                filtros.Add("pes_nome", UCCamposBuscaAluno1.NomeAluno);
                filtros.Add("alc_matricula", UCCamposBuscaAluno1.MatriculaAluno);
                filtros.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
                filtros.Add("adm", (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao).ToString());
                filtros.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
                filtros.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
                filtros.Add("emitirDocAnoAnt", "true");

            filtros.Add("crp_idTur", UCComboTurma1.Valor[1].ToString());
            filtros.Add("ttn_id", UCComboTurma1.Valor[2].ToString());
            filtros.Add("buscaAvancada", chkBuscaAvancada.Checked.ToString());

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.HistoricoEscolarPedagogico
                ,
                Filtros = filtros
            };
            }

            #endregion Salvar busca realizada com os parâmetros do ODS.
        }

        private void VerificaBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.HistoricoEscolarPedagogico)
            {
                // Recuperar busca realizada e pesquisar automaticamente
                string valor, valor1, valor2;

                // UA Escola
                if (UCComboUAEscola.FiltroEscola)
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor);

                    if (!string.IsNullOrEmpty(valor))
                    {
                        UCComboUAEscola.Uad_ID = new Guid(valor);
                    }

                    UCComboUAEscola.EnableEscolas = (UCComboUAEscola.Uad_ID != Guid.Empty);

                    if (UCComboUAEscola.Uad_ID != Guid.Empty)
                    {
                        UCComboUAEscola.CarregaEscolaPorUASuperiorSelecionada();
                        SelecionarEscola();
                    }
                }
                else
                {
                    SelecionarEscola();
                }
                UCComboUAEscola_IndexChangedUnidadeEscola();

                // Curso
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out valor1);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_id", out valor2);
                UCComboCursoCurriculo1.Valor = new[] { Convert.ToInt32(valor), Convert.ToInt32(valor1) };
                UCComboCursoCurriculo1_IndexChanged();
                UCComboCurriculoPeriodo1.Valor = new[] { Convert.ToInt32(valor), Convert.ToInt32(valor1), Convert.ToInt32(valor2) };
                UCComboCurriculoPeriodo1__OnSelectedIndexChange();

                // Calendario
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor);
                UCComboCalendario1.Valor = Convert.ToInt32(valor);
                UCComboCalendario1_IndexChanged();

                // Turma
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_id", out valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_idTur", out valor1);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ttn_id", out valor2);
                UCComboTurma1.Valor = new long[] { Convert.ToInt64(valor), Convert.ToInt64(valor1), Convert.ToInt64(valor2) };

                // Dados Aluno
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("pes_nome", out valor);
                UCCamposBuscaAluno1.NomeAluno = valor;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("alc_matricula", out valor);
                UCCamposBuscaAluno1.MatriculaAluno = valor;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tipoBusca", out valor);
                UCCamposBuscaAluno1.TipoBuscaNomeAluno = valor;

                // Busca avancada
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("buscaAvancada", out valor);
                chkBuscaAvancada.Checked = Boolean.Parse(valor);

                Pesquisar(0, true);
            }
        }

        /// <summary>
        /// Seleciona a escola no combo de acordo com o parâmetro salvo na sessão de busca
        /// realizada.
        /// </summary>
        private void SelecionarEscola()
        {
            string esc_id;
            string uni_id;

            if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
            {
                UCComboUAEscola.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
            }
        }

        private void setaObrigatorioUC(bool obrigatorio)
        {
            UCComboCursoCurriculo1.Obrigatorio =
            UCComboCurriculoPeriodo1.Obrigatorio =
            UCComboCalendario1.Obrigatorio =
            UCComboTurma1.Obrigatorio = obrigatorio;
        }

        #endregion Métodos
    }
}
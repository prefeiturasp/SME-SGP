using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Relatorios.RelatorioGeralAtendimento
{
    public partial class Busca : MotherPageLogado
    {
        private string VS_Ordenacao
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.RelatorioGeralAtendimento)
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

        private SortedDictionary<long, bool> _VS_AlunosSelecionados
        {
            get
            {
                if (ViewState["_VS_AlunosSelecionados"] == null)
                    ViewState["_VS_AlunosSelecionados"] = new SortedDictionary<long, bool>();
                return (SortedDictionary<long, bool>)ViewState["_VS_AlunosSelecionados"];
            }
        }
        
        private SortDirection VS_SortDirection
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.RelatorioGeralAtendimento)
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

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.Json));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsBuscaDocumentosAluno.js"));
            }

            _UCComboCursoCurriculo.IndexChanged += _UCComboCursoCurriculo_IndexChanged;
            _UCComboCurriculoPeriodo.IndexChanged += _UCComboCurriculoPeriodo__OnSelectedIndexChange;

            if (!IsPostBack)
            {
                try
                {
                    string message = __SessionWEB.PostMessages;
                    if (!String.IsNullOrEmpty(message))
                    {
                        _lblMessage.Text = message;
                        __SessionWEB.PostMessages = String.Empty;
                    }

                    if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                    {
                        _updPesquisa.Visible = false;
                        _lblMessage.Text = UtilBO.GetErroMessage("Você não possui permissão para acessar a página solicitada.", UtilBO.TipoMensagem.Alerta);
                    }

                    HabilitarFiltrosPadrao(false);

                    Inicializar();

                    Page.Form.DefaultFocus = _UCComboTipoRelatorioAtendimento.ClientID;
                    Page.Form.DefaultButton = _btnPesquisar.UniqueID;

                    // quantidade de itens por página
                    string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                    _ddlQtPaginado.SelectedValue = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao.ToString() : qtItensPagina;

                    _fdsResultado.Visible = false;

                    CarregaBusca();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void HabilitarFiltrosPadrao(bool habilita)
        {
            _UCComboUAEscola.Visible =
            _UCComboCursoCurriculo.Visible =
            _UCComboCurriculoPeriodo.Visible =
            _UCComboCalendario.Visible =
            _btnPesquisar.Visible =
            _btnGerarRelatorio.Visible = habilita;

            HabilitarValidacao(false);
        }

        protected void HabilitarValidacao(bool habilita)
        {
            _UCComboUAEscola.ObrigatorioUA =
            _UCComboUAEscola.ObrigatorioEscola =
            _UCComboCursoCurriculo.Obrigatorio =
            _UCComboCurriculoPeriodo.Obrigatorio =
            _UCComboCalendario.Obrigatorio = habilita;
        }

        protected void Inicializar()
        {
            _UCComboUAEscola.Inicializar();

            if (_UCComboUAEscola.VisibleUA)
                _UCComboUAEscola_IndexChangedUA();

        }

        protected void CarregaBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.RelatorioGeralAtendimento)
            {
                string valor, valor2, valor3;

                long doc_id = -1;
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                {
                    doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                    _UCComboUAEscola.InicializarVisaoIndividual(doc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    string esc_id;
                    string uni_id;

                    if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                        (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
                    {
                        _UCComboUAEscola.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                        _UCComboUAEscola_IndexChangedUnidadeEscola();
                    }
                }
                else
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor);
                    if (!string.IsNullOrEmpty(valor))
                    {
                        _UCComboUAEscola.Uad_ID = new Guid(valor);
                        _UCComboUAEscola.CarregaEscolaPorUASuperiorSelecionada();

                        if (_UCComboUAEscola.Uad_ID != Guid.Empty)
                        {
                            _UCComboUAEscola.FocoEscolas = true;
                            _UCComboUAEscola.PermiteAlterarCombos = true;
                        }
                        string esc_id;
                        string uni_id;

                        if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                            (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
                        {
                            _UCComboUAEscola.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                            _UCComboUAEscola_IndexChangedUnidadeEscola();
                        }
                    }
                }

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out valor2);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out valor);
                _UCComboCursoCurriculo.Valor = new[] { Convert.ToInt32(valor2), Convert.ToInt32(valor) };
                _UCComboCursoCurriculo_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_id", out valor);

                _UCComboCurriculoPeriodo.Valor = new[] { _UCComboCursoCurriculo.Valor[0], _UCComboCursoCurriculo.Valor[1], Convert.ToInt32(valor) };

                _UCComboCurriculoPeriodo__OnSelectedIndexChange();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor);
                _UCComboCalendario.Valor = Convert.ToInt32(valor);

                if (_btnPesquisar.Visible)
                {
                    try
                    {
                        if (Page.IsValid)
                            Pesquisar();

                    }
                    catch (ValidationException ex)
                    {
                        _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                    }
                    catch (Exception ex)
                    {
                        ApplicationWEB._GravaErro(ex);
                        _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alunos.", UtilBO.TipoMensagem.Erro);
                    }
                }
            }
        }

        private void InicializarTela()
        {
            //todo ana carregar combos
            //UCComboUAEscola1.CarregaUnidadesEscolaresPorUASuperior();
            //UCCRelatorioAtendimento1.car

            _updPesquisa.Update();
        }

        protected void SalvaBusca()
        {
            Dictionary<string, string> filtros = new Dictionary<string, string>();
            //filtros.Add("cal_ano", UCComboAnoLetivo1.ano.ToString());
            //filtros.Add("tne_id", UCComboTipoNivelEnsino1.Valor.ToString());
            //filtros.Add("tme_id", UCComboTipoModalidadeEnsino1.Valor.ToString());

            __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.RelatorioGeralAtendimento, Filtros = filtros };

        }

        private void GerarRelatorio()
        {
            try
            {
                //string report, parametros;

                //report = ((int)MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica.RelatorioGeralAtendimento).ToString();
                //parametros = "cal_ano=" + UCComboAnoLetivo1.ano.ToString() +
                //             "&tne_id=" + UCComboTipoNivelEnsino1.Valor.ToString() +
                //             "&tne_nome=" + UCComboTipoNivelEnsino1.Texto +
                //             "&tme_id=" + UCComboTipoModalidadeEnsino1.Valor.ToString() +
                //             "&tme_nome=" + UCComboTipoModalidadeEnsino1.Texto +
                //             "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                //             "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio") +
                //             "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria") +
                //             "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                //                     , ApplicationWEB.LogoRelatorioSSRS);

                //CFG_RelatorioBO.CallReport("Relatorios", report, parametros, HttpContext.Current);
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar gerar o relatório.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void Pesquisar()
        {

            SalvaBusca();

            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            _ddlQtPaginado.SelectedValue = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao.ToString() : qtItensPagina;

            // ******************************

            DataTable dtAlunos = SelecionaDataSource(0);

            if (dtAlunos != new DataTable())
            {
                _grvAlunos.VirtualItemCount = ACA_AlunoBO.GetTotalRecords();
                _grvAlunos.PageIndex = 0;
                _grvAlunos.PageSize = Convert.ToInt32(_ddlQtPaginado.SelectedValue);
                _grvAlunos.DataSource = dtAlunos;

                _grvAlunos.DataBind();
            }

            _fdsResultado.Visible = true;

            _chkTodos.Visible = !_grvAlunos.Rows.Count.Equals(0);

            if ((_chkTodos.Visible == true))  // esse teste é utilizado para exibir o flag _chkTodos.Checked já selecionado, desde que o parametros possua valor True
            {
                _chkTodos.Checked = true;
            }

            divQtdPaginacao.Visible = _grvAlunos.Rows.Count > 0;

            _updResultado.Update();
        }

        protected DataTable SelecionaDataSource(int Pagina, bool SelecionaTodos = false)
        {

            int selectedValue = Convert.ToInt32(MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica.RelatorioGeralAtendimento);

            int qtdeLinhasPorPagina = SelecionaTodos ? 0 : Convert.ToInt32(_ddlQtPaginado.SelectedValue);

            //return ACA_AlunoBO.BuscaAlunos_RelatorioGeralAtendimento
            //    (
            //            _UCComboTipoRelatorioAtendimento.Valor,
            //            _UCCRelatorioAtendimento.Valor,
            //            _UCComboCalendario.Valor,
            //            _UCComboUAEscola.Esc_ID,
            //            _UCComboUAEscola.Uni_ID,
            //            _UCComboCursoCurriculo.Valor[0],
            //            _UCComboCursoCurriculo.Valor[1],
            //            _UCComboCurriculoPeriodo.Valor[2],
            //            __SessionWEB.__UsuarioWEB.Usuario.ent_id,
            //            _UCComboUAEscola.Uad_ID,
            //            (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
            //            __SessionWEB.__UsuarioWEB.Usuario.usu_id,
            //            __SessionWEB.__UsuarioWEB.Grupo.gru_id,
            //            qtdeLinhasPorPagina,
            //            Pagina,
            //            (int)VS_SortDirection,
            //            VS_Ordenacao
            //        );

            return null;

        }

        protected void _UCComboUAEscola_IndexChangedUA()
        {
            if (_UCComboUAEscola.Uad_ID == Guid.Empty)
                _UCComboUAEscola.SelectedValueEscolas = new[] { -1, -1 };

            _UCComboUAEscola_IndexChangedUnidadeEscola();
        }

        protected void _UCComboUAEscola_IndexChangedUnidadeEscola()
        {
            try
            {
                _UCComboCursoCurriculo.Valor = new[] { -1, -1 };
                _UCComboCursoCurriculo.PermiteEditar = false;

                if (_UCComboUAEscola.Esc_ID > 0 && _UCComboUAEscola.Uni_ID > 0)
                {
                    _UCComboCursoCurriculo.CarregarVigentesPorEscola(_UCComboUAEscola.Esc_ID, _UCComboUAEscola.Uni_ID);

                    _UCComboCursoCurriculo.SetarFoco();
                    _UCComboCursoCurriculo.PermiteEditar = true;
                }

                _UCComboCursoCurriculo_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void _UCComboCurriculoPeriodo__OnSelectedIndexChange()
        {
            try
            {
                _UCComboCalendario.Valor = -1;

                if (_UCComboCurriculoPeriodo.Valor[0] > 0)
                {
                    _UCComboCalendario.CarregarPorCurso(_UCComboCursoCurriculo.Valor[0]);
                    _UCComboCalendario.PermiteEditar = true;
                    _UCComboCalendario.SetarFoco();
                }

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o(s) calendários(s).", UtilBO.TipoMensagem.Erro);
            }
        }

        private void _UCComboCursoCurriculo_IndexChanged()
        {
            try
            {
                _UCComboCurriculoPeriodo.Valor = new[] { -1, -1, -1 };
                _UCComboCurriculoPeriodo.PermiteEditar = false;

                if (_UCComboCursoCurriculo.Valor[0] > 0)
                {
                    _UCComboCurriculoPeriodo.CarregarPorCursoCurriculo(_UCComboCursoCurriculo.Valor[0], _UCComboCursoCurriculo.Valor[1]);
                    _UCComboCurriculoPeriodo.PermiteEditar = true;
                    _UCComboCurriculoPeriodo.Focus();
                }

                _UCComboCurriculoPeriodo__OnSelectedIndexChange();

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void _ddlQtPaginado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_grvAlunos.Rows.Count.Equals(0))
            {
                // atribui nova quantidade itens por página para o grid
                _grvAlunos.PageSize = Convert.ToInt32(_ddlQtPaginado.SelectedValue);
                _grvAlunos.PageIndex = 0;

                // Seta propriedades necessárias para ordenação nas colunas.
                ConfiguraColunasOrdenacao(_grvAlunos);

                DataTable dtAlunos = SelecionaDataSource(0);

                if (dtAlunos != new DataTable())
                {
                    _grvAlunos.VirtualItemCount = ACA_AlunoBO.GetTotalRecords();
                    _grvAlunos.PageIndex = 0;
                    _grvAlunos.PageSize = Convert.ToInt32(_ddlQtPaginado.SelectedValue);
                    _grvAlunos.DataSource = dtAlunos;
                    _grvAlunos.DataBind();
                }
            }
        }

        protected void _btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {                
                if (Page.IsValid)
                    Pesquisar();
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alunos.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void _btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Relatorios/RelatorioGeralAtendimento/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void _grvAlunos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox _chkSelecionar = (CheckBox)e.Row.FindControl("_chkSelecionar");

                if (_chkSelecionar != null)
                {
                    _chkSelecionar.Attributes.Add("index", e.Row.RowIndex.ToString());

                    if ((_VS_AlunosSelecionados.ContainsKey(Convert.ToInt64(_chkSelecionar.Attributes["alu_id"]))))
                    {
                        _chkSelecionar.Checked = true;
                        e.Row.Style.Add("background", "#F8F7CB");
                    }
                    else
                    {
                        _chkSelecionar.Checked = false;
                        e.Row.Style.Remove("background");
                    }
                }
            }
        }

        protected void _grvAlunos_DataBound(object sender, EventArgs e)
        {
            string sDeclaracaoHMTL = string.Empty;
            
            _chkTodos.Checked = false;

            _UCTotalRegistros.Total = ACA_AlunoBO.GetTotalRecords();

            // Seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(_grvAlunos, VS_Ordenacao, VS_SortDirection);
        }

        protected void _grvAlunos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void _grvAlunos_Sorting(object sender, GridViewSortEventArgs e)
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
                    PaginaBusca = PaginaGestao.DocumentosAluno
                    ,
                    Filtros = filtros
                };
            }

            DataTable dtAlunos = SelecionaDataSource(grid.PageIndex);

            if (dtAlunos != new DataTable())
            {
                grid.DataSource = dtAlunos;
                grid.DataBind();
            }
        }

        protected void _btnGerarRelatorio_Click(object sender, EventArgs e)
        {

        }

        protected void _btnGerarRelatorioCima_Click(object sender, EventArgs e)
        {

        }
    }
}
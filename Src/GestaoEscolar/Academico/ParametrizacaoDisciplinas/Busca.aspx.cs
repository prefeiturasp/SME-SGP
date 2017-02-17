using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.Academico.ParametrizacaoDisciplinas
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada.
        /// </summary>
        private string VS_Ordenacao
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.ParametrizacaoDisciplinas)
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
        /// Guarda o sortExpression da coluna ordenada.
        /// </summary>
        private SortDirection VS_SortDirection
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.ParametrizacaoDisciplinas)
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

        #region Constantes

        private const int INDEX_COLUNA_CURSO = 3;

        #endregion Constantes

        #region Eventos Page Life Cycle

        protected void Page_init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                grvParametrosDisciplinas.Columns[INDEX_COLUNA_CURSO].HeaderText = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference("~/Includes/jsBuscaParametrizacaoDisciplinas.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroParametrizacaoDisciplinas.js"));
            }

            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMessage.Text = message;

                grvParametrosDisciplinas.PageSize = ApplicationWEB._Paginacao;

                try
                {
                    Inicializar();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }

                Page.Form.DefaultButton = btnPesquisar.UniqueID;
                Page.Form.DefaultFocus = ucComboUAEscola.VisibleUA ? ucComboUAEscola.ComboUA_ClientID : ucComboUAEscola.ComboEscola_ClientID;

                divPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnPesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnLimparPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            }

            ucComboUAEscola.IndexChangedUA += ucComboUAEscola_IndexChangedUA;
            ucComboUAEscola.IndexChangedUnidadeEscola += ucComboUAEscola_IndexChangedUnidadeEscola;
            ucComboCursoCurriculo.IndexChanged += ucComboCursoCurriculo_IndexChanged;
            ucComboCurriculoPeriodo._OnSelectedIndexChange += ucComboCurriculoPeriodo__OnSelectedIndexChange;
        }

        #endregion Eventos Page Life Cycle

        #region Delegates

        /// <summary>
        /// Atualiza o grid de acordo com a paginação
        /// </summary>
        protected void UCComboQtdePaginacao_IndexChanged()
        {
            try
            {
                // Atribui nova quantidade de itens por página para o grid.
                grvParametrosDisciplinas.PageSize = UCComboQtdePaginacao1.Valor;
                grvParametrosDisciplinas.PageIndex = 0;
                // Atualiza o grid
                grvParametrosDisciplinas.DataBind();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os parâmetros de configuração de disciplinas.",
                    UtilBO.TipoMensagem.Erro);
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
                ucComboUAEscola_IndexChangedUA();

            string esc_uni_id;

            if (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_uni_id", out esc_uni_id))
            {
                ucComboUAEscola.DdlEscola.SelectedValue = esc_uni_id;
                ucComboUAEscola_IndexChangedUnidadeEscola();
            }
        }

        /// <summary>
        /// Evento change do combo de UA Superior.
        /// </summary>
        private void ucComboUAEscola_IndexChangedUA()
        {
            try
            {
                ucComboCurriculoPeriodo.Valor = new[] { -1, -1, -1 };
                ucComboCurriculoPeriodo.PermiteEditar = false;                

                ucComboUAEscola.CarregaEscolaPorUASuperiorSelecionada();

                if (ucComboUAEscola.Uad_ID != Guid.Empty)
                {
                    ucComboUAEscola.FocoEscolas = true;
                    ucComboUAEscola.PermiteAlterarCombos = true;
                }
                else
                {
                    // Limpa o combo de cursos - carrega todos.ss
                    ucComboCursoCurriculo.Valor = new[] { -1, -1, -1 };
                    ucComboCursoCurriculo.CarregarCursoCurriculo();
                }

                ucComboUAEscola.SelectedValueEscolas = new[] { -1, -1 };
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo de Escola.
        /// </summary>
        private void ucComboUAEscola_IndexChangedUnidadeEscola()
        {
            try
            {
                ucComboCurriculoPeriodo.Valor = new[] { -1, -1, -1 };
                ucComboCurriculoPeriodo.PermiteEditar = false;                

                if (ucComboUAEscola.Esc_ID > 0)
                {
                    ucComboCursoCurriculo.CarregarCursoCurriculoPorEscola(ucComboUAEscola.Esc_ID, ucComboUAEscola.Uni_ID, 0);
                    ucComboCursoCurriculo.PermiteEditar = true;
                    ucComboCursoCurriculo.SetarFoco();
                }
                else
                {
                    ucComboCursoCurriculo.CarregarCursoCurriculo();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o(s) curso(s) da unidade escolar.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo de curso.
        /// </summary>
        private void ucComboCursoCurriculo_IndexChanged()
        {
            try
            {
                if (ucComboCursoCurriculo.Cur_ID > 0)
                {
                    // Carrega períodos.
                    ucComboCurriculoPeriodo.CancelSelect = false;
                    ucComboCurriculoPeriodo._MostrarMessageSelecione = true;
                    ucComboCurriculoPeriodo._Load(ucComboCursoCurriculo.Valor[0], ucComboCursoCurriculo.Valor[1]);
                    ucComboCurriculoPeriodo.PermiteEditar = true;
                    ucComboCurriculoPeriodo.FocaCombo();

                    ucComboCalendario.CarregarCalendarioAnualPorCurso(ucComboCursoCurriculo.Cur_ID);
                }
                else
                {
                    ucComboCurriculoPeriodo.Valor = new[] { -1, -1, -1 };
                    ucComboCurriculoPeriodo.PermiteEditar = false;

                    ucComboCalendario.CarregarCalendarioAnual();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o(s) período(s) do curso.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo de Curriculo Periodo.
        /// </summary>
        private void ucComboCurriculoPeriodo__OnSelectedIndexChange()
        {
            try
            {
                if (ucComboCurriculoPeriodo.Valor[0] > 0)
                {
                    UCComboTipoDisciplina1.CarregarTipoDisciplinaPorCursoCurriculoPeriodo(
                        ucComboCurriculoPeriodo.Valor[0], ucComboCurriculoPeriodo.Valor[1], ucComboCurriculoPeriodo.Valor[2]);
                    UCComboTipoDisciplina1.PermiteEditar = true;
                }
                else 
                {
                    UCComboTipoDisciplina1.CarregarTipoDisciplina();
                }

                UCComboTipoDisciplina1.Valor = -1;                 
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Delegates

        #region Metodos

        /// <summary>
        /// Realiza a consulta com os filtros informados, e salva a busca realizada na sessão.
        /// </summary>
        public void Pesquisar()
        {
            try
            {
                odsParametrosDisciplinas.SelectParameters.Clear();
                odsParametrosDisciplinas.SelectParameters.Add("esc_id", ucComboUAEscola.Esc_ID.ToString());
                odsParametrosDisciplinas.SelectParameters.Add("cur_id", ucComboCursoCurriculo.Valor[0].ToString());
                odsParametrosDisciplinas.SelectParameters.Add("crp_id", ucComboCurriculoPeriodo.Valor[2].ToString());
                odsParametrosDisciplinas.SelectParameters.Add("crr_id", ucComboCursoCurriculo.Valor[1].ToString());
                odsParametrosDisciplinas.SelectParameters.Add("cal_id", ucComboCalendario.Valor.ToString());
                odsParametrosDisciplinas.SelectParameters.Add("tds_id", UCComboTipoDisciplina1.Valor.ToString());
                odsParametrosDisciplinas.SelectParameters.Add("tur_codigo", txtCodigoTurma.Text);
                odsParametrosDisciplinas.SelectParameters.Add("paginado", "true");

                grvParametrosDisciplinas.PageIndex = 0;
                grvParametrosDisciplinas.PageSize = UCComboQtdePaginacao1.Valor;

                fdsResultado.Visible = true;

                // Limpar a ordenação realizada.
                grvParametrosDisciplinas.Sort(VS_Ordenacao, VS_SortDirection);

                #region Salvar busca realizada com os parâmetros do ODS.

                Dictionary<string, string> filtros = odsParametrosDisciplinas.SelectParameters.Cast<Parameter>().ToDictionary(param => param.Name, param => param.DefaultValue);

                // Salvar UA Superior.            
                if (ucComboUAEscola.FiltroEscola)
                {
                    filtros.Add("ua_superior", ucComboUAEscola.Uad_ID.ToString());
                    filtros.Add("esc_uni_id", ucComboUAEscola.DdlEscola.SelectedValue);
                }

                __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.ParametrizacaoDisciplinas, Filtros = filtros };

                #endregion Salvar busca realizada com os parâmetros do ODS.

                // Atualiza o grid
                grvParametrosDisciplinas.DataBind();

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os parâmetros de configuração de disciplinas.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica se tem busca salva na sessão, e se tiver, recupera e realiza a consulta,
        /// colocando os filtros nos campos da tela.
        /// </summary>
        private void VerificaBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.ParametrizacaoDisciplinas)
            {
                string valor1;
                string valor2;
                string valor3;

                #region Carregar Combos
                UCComboTipoDisciplina1.CarregarNivelEnsinoTipoDisciplina();
                ucComboCursoCurriculo.CarregarCursoCurriculo();
                #endregion

                if (ucComboUAEscola.FiltroEscola)
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ua_superior", out valor1);

                    if (!string.IsNullOrEmpty(valor1))
                    {
                        ucComboUAEscola.DdlUA.SelectedValue = valor1;
                    }

                    if (valor1 != Guid.Empty.ToString())
                    {
                        SelecionarEscola(ucComboUAEscola.FiltroEscola);
                    }
                }
                else
                {
                    SelecionarEscola(ucComboUAEscola.FiltroEscola);
                }

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out valor1);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out valor2);
                ucComboCursoCurriculo.Valor = new Int32[] { Convert.ToInt32(valor1), Convert.ToInt32(valor2) };
                ucComboCursoCurriculo_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_id", out valor3);
                if (Convert.ToInt32(valor3) > 0)
                    ucComboCurriculoPeriodo._Combo.SelectedValue = valor1 + ";" + valor2 + ";" + valor3;
                ucComboCurriculoPeriodo__OnSelectedIndexChange();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor1);
                ucComboCalendario.Valor = Convert.ToInt32(valor1);

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tds_id", out valor2);
                UCComboTipoDisciplina1.Valor = Convert.ToInt32(valor2);

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_codigo", out valor3);
                txtCodigoTurma.Text = valor3;
                txtCodigoTurma.Focus();

                Pesquisar();
            }
            else
            {
                ucComboUAEscola_IndexChangedUnidadeEscola();
                ucComboCalendario.CarregarCalendarioAnual();
            }
        }

        /// <summary>
        /// Verifica se usuário tem permissão de acesso à página.
        /// </summary>
        private void VerificaPermissaoUsuario()
        {
            if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Você não possui permissão para acessar a página solicitada.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Index.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        /// <summary>
        /// Inicializa os combos.
        /// </summary>
        public void Inicializar()
        {
            try
            {
                VerificaPermissaoUsuario();

                ucComboUAEscola.FocusUA();
                ucComboUAEscola.Inicializar();
                UCComboTipoDisciplina1.CarregarTipoDisciplina();
                UCComboTipoDisciplina1.Valor = -1;  

                this.VerificaBusca();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Salvar os parâmetros de configuração de disciplinas.
        /// </summary>
        public void Salvar()
        {
            try
            {
                if (grvParametrosDisciplinas.Rows.Count > 0)
                {
                    bool chkNaoLancarFrequencia = ((CheckBox)grvParametrosDisciplinas.HeaderRow.FindControl("chkNaoLancarFrequencia")).Checked;
                    bool chkNaoLancarNota = ((CheckBox)grvParametrosDisciplinas.HeaderRow.FindControl("chkNaoLancarNota")).Checked;
                    bool chkNaoExibirFrequencia = ((CheckBox)grvParametrosDisciplinas.HeaderRow.FindControl("chkNaoExibirFrequencia")).Checked;
                    bool chkNaoExibirNota = ((CheckBox)grvParametrosDisciplinas.HeaderRow.FindControl("chkNaoExibirNota")).Checked;
                    bool chkNaoExibirBoletim = ((CheckBox)grvParametrosDisciplinas.HeaderRow.FindControl("chkNaoExibirBoletim")).Checked;
                    bool chkNaoLancarPlanejamento = ((CheckBox)grvParametrosDisciplinas.HeaderRow.FindControl("chkNaoLancarPlanejamento")).Checked;
                    bool chkPermitirLancarAbonoFalta = ((CheckBox)grvParametrosDisciplinas.HeaderRow.FindControl("chkPermitirLancarAbonoFalta")).Checked;

                    List<TUR_TurmaDisciplina> lista =
                         (from GridViewRow row in grvParametrosDisciplinas.Rows
                          select new TUR_TurmaDisciplina
                          {
                              tud_id = Convert.ToInt32(grvParametrosDisciplinas.DataKeys[row.RowIndex]["tud_id"])
                              ,
                              tud_naoLancarFrequencia = (chkNaoLancarFrequencia ? chkNaoLancarFrequencia : ((CheckBox)row.FindControl("chkItemNaoLancarFrequencia")).Checked)
                              ,
                              tud_naoLancarNota = (chkNaoLancarNota ? chkNaoLancarNota : ((CheckBox)row.FindControl("chkItemNaoLancarNota")).Checked)
                              ,
                              tud_naoExibirFrequencia = (chkNaoExibirFrequencia ? chkNaoExibirFrequencia : ((CheckBox)row.FindControl("chkItemNaoExibirFrequencia")).Checked)
                              ,
                              tud_naoExibirNota = (chkNaoExibirNota ? chkNaoExibirNota : ((CheckBox)row.FindControl("chkItemNaoExibirNota")).Checked)
                              ,
                              tud_naoExibirBoletim = (chkNaoExibirBoletim ? chkNaoExibirBoletim : ((CheckBox)row.FindControl("chkItemNaoExibirBoletim")).Checked)
                              ,
                              tud_naoLancarPlanejamento = (chkNaoLancarPlanejamento ? chkNaoLancarPlanejamento : ((CheckBox)row.FindControl("chkItemNaoLancarPlanejamento")).Checked)
                              ,
                              tud_permitirLancarAbonoFalta = (chkPermitirLancarAbonoFalta ? chkPermitirLancarAbonoFalta : ((CheckBox)row.FindControl("chkItemPermitirLancarAbonoFalta")).Checked)
                          }
                        ).ToList();

                    if (lista.Count > 0)
                    {
                        if (TUR_TurmaDisciplinaBO.SalvarConfiguracaoParametrosDisciplinas(lista))
                        {
                            ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update,
                                                            String.Format("esc_id: {0} / uni_id: {1} / cur_id: {2} / crr_id: {3} / crp_id: {4} / cal_id: {5} / tds_id: {6}."
                                                            , ucComboUAEscola.Esc_ID
                                                            , ucComboUAEscola.Uni_ID
                                                            , ucComboCurriculoPeriodo.Valor[0]
                                                            , ucComboCurriculoPeriodo.Valor[1]
                                                            , ucComboCurriculoPeriodo.Valor[2]
                                                            , ucComboCalendario.Valor
                                                            , UCComboTipoDisciplina1.Valor));

                            lblMessage.Text = UtilBO.GetErroMessage("Parâmetro(s) de configuração de disciplina(s) salvo(s) com sucesso.", UtilBO.TipoMensagem.Sucesso);
                        }
                        grvParametrosDisciplinas.DataBind();
                    }
                }
                else
                {
                    lblMessage.Text = UtilBO.GetErroMessage("Não há nenhum parâmetro de configuração de disciplinas carregado.", UtilBO.TipoMensagem.Alerta);
                }
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o(s) parâmetro(s) de configuração de disciplina(s).",
                    UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Metodos

        #region Eventos

        protected void grvParametrosDisciplinas_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = TUR_TurmaDisciplinaBO.GetTotalRecords();

            // Seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(grvParametrosDisciplinas);

            if ((!string.IsNullOrEmpty(grvParametrosDisciplinas.SortExpression)) && (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.ParametrizacaoDisciplinas))
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

                if (filtros.ContainsKey("VS_Ordenacao"))
                {
                    filtros["VS_Ordenacao"] = grvParametrosDisciplinas.SortExpression;
                }
                else
                {
                    filtros.Add("VS_Ordenacao", grvParametrosDisciplinas.SortExpression);
                }

                if (filtros.ContainsKey("VS_SortDirection"))
                {
                    filtros["VS_SortDirection"] = grvParametrosDisciplinas.SortDirection.ToString();
                }
                else
                {
                    filtros.Add("VS_SortDirection", grvParametrosDisciplinas.SortDirection.ToString());
                }

                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.ParametrizacaoDisciplinas
                    ,
                    Filtros = filtros
                };
            }
        }

        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            // Inicializa variável de sessão.
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            Response.Redirect("Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar();
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                Salvar();
        }

        protected void odsParametrosDisciplinas_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (e.ExecutingSelectCount)
                e.InputParameters.Clear();
        }

        #endregion Eventos

    }
}
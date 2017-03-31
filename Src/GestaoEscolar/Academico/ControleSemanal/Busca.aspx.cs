using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.BLL.Caching;
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;
using ReportNameGestaoAcademicaDocumentosDocente = MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademicaDocumentosDocente;

namespace GestaoEscolar.Academico.ControleSemanal
{
    public partial class Busca : MotherPageLogadoCompressedViewState
    {
        #region Estrutura

        #endregion Estrutura

        #region Propriedades

        private int VS_rptTurmasIndice
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_rptTurmasIndice"] ?? -1);
            }

            set
            {
                ViewState["VS_rptTurmasIndice"] = value;
            }
        }


        private GridView Edit_grvTurma
        {
            get
            {
                if (VS_visaoDocente)
                    return (GridView)(rptTurmas.Items[VS_rptTurmasIndice].FindControl("grvTurma"));
                else
                    return grvTurmas;
            }
        }

        public int Edit_esc_id
        {
            get
            {
                return Convert.ToInt32(Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["esc_id"] ?? -1);
            }
        }

        public int Edit_uni_id
        {
            get
            {
                return Convert.ToInt32(Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["uni_id"] ?? -1);
            }
        }

        public long Edit_tur_id
        {
            get
            {
                return Convert.ToInt64(Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["tur_id"] ?? -1);
            }
        }

        public string Edit_escola
        {
            get
            {
                return Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["tur_escolaUnidade"].ToString();
            }
        }

        public string Edit_tur_codigo
        {
            get
            {
                return Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["tur_codigo"].ToString();
            }
        }

        public string Edit_tud_nome
        {
            get
            {
                return Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["tud_nome"].ToString();
            }
        }

        public int Edit_cal_id
        {
            get
            {
                return Convert.ToInt32(Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["cal_id"] ?? -1);
            }
        }

        public long Edit_tud_id
        {
            get
            {
                return Convert.ToInt64(Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["tud_id"] ?? -1);
            }
        }

        public byte Edit_tdt_posicao
        {
            get
            {
                return Convert.ToByte(Edit_grvTurma.DataKeys[Edit_grvTurma.EditIndex].Values["tdt_posicao"] ?? 0);
            }
        }
        
        private bool VS_titular
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_titular"] ?? false);
            }

            set
            {
                ViewState["VS_titular"] = value;
            }
        }

        /// <summary>
        /// Retorna se o usuário logado é docente.
        /// </summary>
        private bool VS_visaoDocente
        {
            get
            {
                long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                int visao = __SessionWEB.__UsuarioWEB.Grupo.vis_id;
                return (visao == SysVisaoID.Individual && doc_id > 0);
            }
        }

        public string PaginaRetorno
        {
            get
            {
                return "~/Academico/ControleTurma/Busca.aspx";
            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada.
        /// </summary>
        private string VS_Ordenacao
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.PlanejamentoSemanal)
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
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.PlanejamentoSemanal)
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
        /// Guarda a tela que precisa redirecionar.
        /// </summary>
        private string VS_TelaRedirecionar
        {
            get
            {
                if (ViewState["VS_TelaRedirecionar"] == null)
                    return "";

                return ViewState["VS_TelaRedirecionar"].ToString();
            }
            set
            {
                ViewState["VS_TelaRedirecionar"] = value;
            }
        }

        /// <summary>
        /// Parâmetro que indica se o fechamento será pré carregado no cache.
        /// </summary>
        private bool PreCarregarFechamentoCache
        {
            get
            {
                return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PRE_CARREGAR_CACHE_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        #endregion Propriedades

        #region Delegates

        /// <summary>
        /// Evento change do combo de UA Superior.
        /// </summary>
        private void UCComboUAEscola1_IndexChangedUA()
        {
            try
            {
                UCComboUAEscola1.FiltroEscolasControladas = true;
                UCComboUAEscola1.CarregaEscolaPorUASuperiorSelecionada();

                if (UCComboUAEscola1.Uad_ID != Guid.Empty)
                {
                    UCComboUAEscola1.FocoEscolas = true;
                    UCComboUAEscola1.PermiteAlterarCombos = true;
                }

                UCComboUAEscola1.SelectedValueEscolas = new[] { -1, -1 };
                UCComboUAEscola1_IndexChangedUnidadeEscola();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo de Escola.
        /// </summary>
        private void UCComboUAEscola1_IndexChangedUnidadeEscola()
        {
            try
            {
                UCCCurriculoPeriodo1.PermiteEditar = false;
                UCCCurriculoPeriodo1.Valor = new[] { -1, -1, -1 };
                UCComboCursoCurriculo1.PermiteEditar = false;
                UCComboCursoCurriculo1.Valor = new[] { -1, -1 };
                UCCCalendario1.PermiteEditar = false;
                UCCCalendario1.Valor = -1;

                if (UCComboUAEscola1.Esc_ID > 0)
                {
                    UCCCalendario1.CarregarCalendarioAnual();
                    UCCCalendario1.PermiteEditar = true;
                    UCCCalendario1.SetarFoco();
                }

                UCCCalendario1_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo calendario
        /// </summary>
        private void UCCCalendario1_IndexChanged()
        {
            UCCCurriculoPeriodo1.PermiteEditar = false;
            UCCCurriculoPeriodo1.Valor = new[] { -1, -1, -1 };
            UCComboCursoCurriculo1.PermiteEditar = false;
            UCComboCursoCurriculo1.Valor = new[] { -1, -1 };

            if (UCComboUAEscola1.Esc_ID > 0 && UCCCalendario1.Valor > 0)
            {
                UCComboCursoCurriculo1.PermiteEditar = true;
                // Carregar todos os cursos, não só ativos, para exibir turmas encerradas na busca.
                UCComboCursoCurriculo1.CarregarPorEscolaCalendarioSituacaoCurso(UCComboUAEscola1.Esc_ID, UCComboUAEscola1.Uni_ID, UCCCalendario1.Valor, 0);
                UCComboCursoCurriculo1.SetarFoco();
            }

            UCComboCursoCurriculo1_IndexChanged();
        }

        /// <summary>
        /// Evento change do combo de curso curriculo
        /// </summary>
        private void UCComboCursoCurriculo1_IndexChanged()
        {
            try
            {
                if (UCComboCursoCurriculo1.Valor[0] > 0)
                {
                    // carrego o ciclo
                    UCComboTipoCiclo.CarregarCicloPorCursoCurriculo(UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1]);
                    if (UCComboTipoCiclo.ddlCombo.Items.Count > 0)
                    {
                        UCComboTipoCiclo.Visible = true;
                        UCComboTipoCiclo.Enabled = true;
                        UCComboTipoCiclo.ddlCombo.Focus();

                        UCComboTipoCiclo_IndexChanged();
                    }
                    else
                    {
                        UCCCurriculoPeriodo1.CarregarPorCursoCurriculo(UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1]);
                        UCCCurriculoPeriodo1.PermiteEditar = true;
                        UCCCurriculoPeriodo1.SetarFoco();
                    }
                }
                else
                {
                    UCComboTipoCiclo.Visible = false;
                    UCCCurriculoPeriodo1.Valor = new[] { -1, -1, -1 };
                    UCCCurriculoPeriodo1.PermiteEditar = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo de tipo de ciclo
        /// </summary>
        private void UCComboTipoCiclo_IndexChanged()
        {
            try
            {
                if (UCComboTipoCiclo.Valor > 0)
                {
                    UCCCurriculoPeriodo1.CarregarPorCursoCurriculoTipoCiclo(UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1], UCComboTipoCiclo.Valor);
                    UCCCurriculoPeriodo1.PermiteEditar = true;
                    UCCCurriculoPeriodo1.SetarFoco();
                }
                else
                {
                    UCCCurriculoPeriodo1.Valor = new[] { -1, -1, -1 };
                    UCCCurriculoPeriodo1.PermiteEditar = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
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
                UCComboUAEscola1_IndexChangedUA();

            string esc_id, uni_id;

            if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
            {
                UCComboUAEscola1.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                UCComboUAEscola1_IndexChangedUnidadeEscola();
            }
        }

        /// <summary>
        /// Atualiza o grid de acordo com a quantidade de paginação
        /// </summary>
        protected void UCComboQtdePaginacao_IndexChanged()
        {
            try
            {
                // Atribui nova quantidade de itens por página para o grid.
                grvTurmas.PageSize = UCComboQtdePaginacao.Valor;
                grvTurmas.PageIndex = 0;
                // Atualiza o grid
                grvTurmas.DataBind();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Delegates

        #region Page life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            }

            if (!IsPostBack)
            {
                try
                {
                    string message = __SessionWEB.PostMessages;

                    if (!String.IsNullOrEmpty(message))
                        lblMensagem.Text = message;

                    Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
                    long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                    int visao = __SessionWEB.__UsuarioWEB.Grupo.vis_id;

                    if (VS_visaoDocente)
                    {
                        List<Struct_MinhasTurmas> dados = TUR_TurmaBO.SelecionaPorDocenteControleTurma(ent_id, doc_id, ApplicationWEB.AppMinutosCacheCurto);

                        // Guarda em uma variável as escolas que possuem alguma turma ativa
                        var dadosEscolasAtivas = dados.Where(p => p.Turmas.Any(t => t.tur_situacao == (byte)TUR_TurmaSituacao.Ativo)).ToList();

                        if (dadosEscolasAtivas.Count == 0)
                        {  // se o docente não possuir nenhuma turma - exibir a mensagem informativa
                            lblMensagem.Text = UtilBO.GetErroMessage((String)GetGlobalResourceObject("Mensagens", "MSG_ATRIBUICAODOCENTES"), UtilBO.TipoMensagem.Informacao);
                            lblMensagem1.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.Busca.DocenteSemTurma").ToString(),
                                                                      UtilBO.TipoMensagem.Alerta);
                        }

                        //VS_Dados = dados;
                        rptTurmas.DataSource = dadosEscolasAtivas;
                        rptTurmas.DataBind();

                        VS_titular = dados.Exists(p => p.Turmas.Any(t => t.tdc_id == (int)EnumTipoDocente.Titular));

                        pnlTurmas.Visible = false;
                        divResultadoDocente.Visible = true;
                    }
                    else
                    {
                        VS_titular = false;
                        divFiltros.Visible = true;
                        grvTurmas.PageSize = ApplicationWEB._Paginacao;

                        #region Inicializar

                        UCComboUAEscola1.FocusUA();
                        UCComboUAEscola1.Inicializar();

                        UCComboTipoCiclo.Carregar();
                        UCComboTipoCiclo.SelectedValue = "-1";

                        this.VerificarBusca();

                        #endregion Inicializar

                        Page.Form.DefaultButton = btnPesquisar.UniqueID;
                        Page.Form.DefaultFocus = UCComboUAEscola1.VisibleUA ? UCComboUAEscola1.ComboUA_ClientID : UCComboUAEscola1.ComboEscola_ClientID;
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }
            }

            #region Associando Delegates

            UCComboUAEscola1.IndexChangedUA += UCComboUAEscola1_IndexChangedUA;
            UCComboUAEscola1.IndexChangedUnidadeEscola += UCComboUAEscola1_IndexChangedUnidadeEscola;
            UCCCalendario1.IndexChanged += UCCCalendario1_IndexChanged;
            UCComboCursoCurriculo1.IndexChanged += UCComboCursoCurriculo1_IndexChanged;
            UCComboTipoCiclo.IndexChanged += UCComboTipoCiclo_IndexChanged;

            #endregion Associando Delegates
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

                string script = "permiteAlterarResultado=" + (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, ent_id) ? "1" : "0") + ";" +
                    "exibirNotaFinal=" + (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_NOTAFINAL_LANCAMENTO_AVALIACOES, ent_id) ? "1" : "0") + ";" +
                    "ExibeCompensacao=" + (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_COMPENSACAO_AUSENCIA_CADASTRADA, ent_id) ? "1" : "0") + ";" +
                    "MinutosCacheFechamento=" + ApplicationWEB.AppMinutosCacheFechamento + ";";

                if (sm.IsInAsyncPostBack)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Parametros", script, true);
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Parametros", script, true);
                }
            }
        }

        #endregion Page life Cycle

        #region Métodos
        
        /// <summary>
        /// Realiza a pesquisa mediante aos filtros informados.
        /// </summary>
        private void Pesquisar()
        {
            try
            {
                Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

                odsplanejamentoSemanal.SelectParameters.Clear();
                odsplanejamentoSemanal.SelectParameters.Add("esc_id", UCComboUAEscola1.Esc_ID.ToString());
                odsplanejamentoSemanal.SelectParameters.Add("uni_id", UCComboUAEscola1.Uni_ID.ToString());
                odsplanejamentoSemanal.SelectParameters.Add("cal_id", UCCCalendario1.Valor.ToString());
                odsplanejamentoSemanal.SelectParameters.Add("cur_id", UCComboCursoCurriculo1.Valor[0].ToString());
                odsplanejamentoSemanal.SelectParameters.Add("crr_id", UCComboCursoCurriculo1.Valor[1].ToString());
                odsplanejamentoSemanal.SelectParameters.Add("crp_id", UCCCurriculoPeriodo1.Valor[2].ToString());
                odsplanejamentoSemanal.SelectParameters.Add("ent_id", ent_id.ToString());
                odsplanejamentoSemanal.SelectParameters.Add("tur_codigo", txtCodigoTurma.Text);
                odsplanejamentoSemanal.SelectParameters.Add("tud_tipo", Convert.ToByte(ACA_CurriculoDisciplinaTipo.Regencia).ToString());
                odsplanejamentoSemanal.SelectParameters.Add("appMinutosCacheCurto", ApplicationWEB.AppMinutosCacheCurto.ToString());
                odsplanejamentoSemanal.SelectParameters.Add("tci_id", UCComboTipoCiclo.Tci_id.ToString());

                grvTurmas.PageIndex = 0;
                grvTurmas.PageSize = UCComboQtdePaginacao.Valor;
                divResultadoVisaoSuperior.Visible = true;

                // Limpar a ordenação realizada.
                grvTurmas.Sort(VS_Ordenacao, VS_SortDirection);

                #region Salvar busca realizada com os parâmetros do ODS.

                Dictionary<string, string> filtros = odsplanejamentoSemanal.SelectParameters.Cast<Parameter>().ToDictionary(param => param.Name, param => param.DefaultValue);

                if (UCComboUAEscola1.FiltroEscola)
                    filtros.Add("ua_superior", UCComboUAEscola1.Uad_ID.ToString());

                __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.PlanejamentoSemanal, Filtros = filtros };

                #endregion Salvar busca realizada com os parâmetros do ODS.

                // Atualiza o grid
                grvTurmas.DataBind();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica se tem busca salva na sessão, e se tiver, recupera e realiza a consulta,
        /// colocando os filtros nos campos da tela.
        /// </summary>
        private void VerificarBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.PlanejamentoSemanal)
            {
                string valor1;
                string valor2;
                string valor3;
                string esc_id;
                string uni_id;

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ua_superior", out valor1);

                if (!string.IsNullOrEmpty(valor1))
                {
                    UCComboUAEscola1.Uad_ID = new Guid(valor1);
                    SelecionarEscola(UCComboUAEscola1.FiltroEscola);
                    UCComboUAEscola1_IndexChangedUnidadeEscola();
                }
                else if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
                {
                    UCComboUAEscola1.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                    UCComboUAEscola1_IndexChangedUnidadeEscola();
                }

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor1);
                UCCCalendario1.Valor = Convert.ToInt32(valor1);
                UCCCalendario1_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out valor1);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out valor2);
                UCComboCursoCurriculo1.Valor = new Int32[] { Convert.ToInt32(valor1), Convert.ToInt32(valor2) };
                UCComboCursoCurriculo1_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tci_id", out valor1);
                UCComboTipoCiclo.Tci_id = Convert.ToInt32(valor1);
                UCComboTipoCiclo_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_id", out valor3);
                if (Convert.ToInt32(valor3) > 0)
                    UCCCurriculoPeriodo1.Valor = new Int32[] { Convert.ToInt32(valor1), Convert.ToInt32(valor2), Convert.ToInt32(valor3) };

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_codigo", out valor1);
                txtCodigoTurma.Text = valor1;
                txtCodigoTurma.Focus();

                Pesquisar();
            }
            else
            {
                UCComboUAEscola1_IndexChangedUnidadeEscola();
            }
        }

        /// <summary>
        /// Colocar todas as propriedades da turma na sessão.
        /// </summary>
        private void CarregaSessionPaginaRetorno()
        {
            Session.Remove("tud_id");
            Session.Remove("tdt_posicao");
            Session.Remove("PaginaRetorno");
            Session.Remove("TudIdCompartilhada");

            Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
            byte opcaoAba = Convert.ToByte(eOpcaoAbaMinhasTurmas.DiarioClasse);

            List<Struct_CalendarioPeriodos> listaCalendarioPeriodo = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(Edit_cal_id, ApplicationWEB.AppMinutosCacheLongo);
            Struct_CalendarioPeriodos periodo = listaCalendarioPeriodo.Where(x => (x.cap_dataInicio.Date <= DateTime.Now.Date && x.cap_dataFim.Date >= DateTime.Now.Date)).FirstOrDefault();

            Dictionary<string, string> listaDados = new Dictionary<string, string>();
            listaDados.Add("Edit_tdt_posicao", Edit_tdt_posicao.ToString());
            listaDados.Add("Edit_esc_id", Edit_esc_id.ToString());
            listaDados.Add("Edit_escola", Edit_escola.ToString());
            listaDados.Add("Edit_tur_id", Edit_tur_id.ToString());
            listaDados.Add("Edit_tur_codigo", Edit_tur_codigo.ToString());
            listaDados.Add("Edit_tud_id", Edit_tud_id.ToString());
            listaDados.Add("Edit_tud_nome", Edit_tud_nome.ToString());
            listaDados.Add("Edit_cal_id", Edit_cal_id.ToString());
            listaDados.Add("PaginaRetorno", "~/Academico/ControleSemanal/Busca.aspx");

            Session["DadosPaginaRetorno"] = listaDados;
            Session["VS_DadosTurmas"] = TUR_TurmaBO.SelecionaPorGestorMinhaEscola(ent_id, UCComboUAEscola1.Esc_ID, ApplicationWEB.AppMinutosCacheCurto);
            Session["Historico"] = false;
        }

        /// <summary>
        /// Colocar a turma/disciplina selecionada no historico na sessão.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tdt_posicao"></param>
        private void CarregaSessionHistorico(string tud_id, string tdt_posicao)
        {
            Session.Remove("DadosPaginaRetorno");
            Session.Remove("VS_DadosTurmas");
            Session.Remove("TudIdCompartilhada");

            // Cria variáveis na sessão
            Session["tud_id"] = tud_id;
            Session["tdt_posicao"] = tdt_posicao;
            Session["PaginaRetorno"] = "~/Academico/ControleTurma/Busca.aspx";
            Session["Historico"] = true;
        }

        /// <summary>
        /// Redireciona para a página informada dentro da pasta Controle de Turmas.
        /// </summary>
        /// <param name="pagina">Página</param>
        private void RedirecionaTela(string pagina)
        {
            Response.Redirect("~/Academico/ControleSemanal/" + pagina + ".aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Redireciona para uma das telas do Minhas Turmas, de acordo com o evento do grid.
        /// </summary>
        /// <param name="nomeTela"></param>
        /// <param name="nomePagina"></param>
        /// <param name="grid"></param>
        /// <param name="indice"></param>
        /// <param name="validarDisciplinaCompartilhada"></param>
        private void RedirecionaTela(string nomeTela, string nomePagina, GridView grid, string indice, bool validarDisciplinaCompartilhada)
        {
            try
            {
                int index = 0;
                if (int.TryParse(indice, out index))
                {
                    index = index % grid.PageSize;
                    grid.EditIndex = index;
                    if (grid != null)
                    {
                        if (VS_visaoDocente)
                        {
                            RepeaterItem itemTurma = (RepeaterItem)grid.NamingContainer;
                            VS_rptTurmasIndice = itemTurma.ItemIndex;
                        }

                        Edit_grvTurma.EditIndex = index;

                        CarregaSessionPaginaRetorno();

                        RedirecionaTela(nomePagina);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(string.Format("Erro ao tentar carregar {0}.", nomeTela), UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Métodos

        #region Eventos

        protected void grvTurmas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && PreCarregarFechamentoCache)
            {
                long tur_id = Convert.ToInt64(DataBinder.Eval(e.Row.DataItem, "tur_id"));
                int fav_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "fav_id"));
                int esc_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "esc_id"));
                int cal_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "cal_id"));
                byte tur_tipo = Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "tur_tipo"));
                long tud_id = Convert.ToInt64(DataBinder.Eval(e.Row.DataItem, "tud_id"));
                byte tud_tipo = Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "tud_tipo"));
                bool tud_disciplinaEspecial = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "tud_disciplinaEspecial"));
                byte tdt_posicao = Convert.ToByte(DataBinder.Eval(e.Row.DataItem, "tdt_posicao"));

                ACA_FormatoAvaliacao entityFormatoAvaliacao = new ACA_FormatoAvaliacao { fav_id = fav_id };
                ACA_FormatoAvaliacaoBO.GetEntity(entityFormatoAvaliacao);

                ACA_EscalaAvaliacao entityEscalaAvaliacao = new ACA_EscalaAvaliacao { esa_id = entityFormatoAvaliacao.esa_idPorDisciplina };
                ACA_EscalaAvaliacaoBO.GetEntity(entityEscalaAvaliacao);

                ACA_EscalaAvaliacao entityEscalaAvaliacaoDocente = new ACA_EscalaAvaliacao { esa_id = entityFormatoAvaliacao.esa_idDocente };
                ACA_EscalaAvaliacaoBO.GetEntity(entityEscalaAvaliacaoDocente);

                double notaMinimaAprovacao = 0;
                int ordemParecerMinimo = 0;

                // Valor do conceito global ou por disciplina.
                string valorMinimo = tud_id > 0 ?
                    entityFormatoAvaliacao.valorMinimoAprovacaoPorDisciplina :
                    entityFormatoAvaliacao.valorMinimoAprovacaoConceitoGlobal;

                if (entityEscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica)
                {
                    notaMinimaAprovacao = Convert.ToDouble(valorMinimo.Replace(',', '.'));
                }
                else if (entityEscalaAvaliacao.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres)
                {
                    ordemParecerMinimo = ACA_EscalaAvaliacaoParecerBO.RetornaOrdem_Parecer(entityEscalaAvaliacao.esa_id, valorMinimo, ApplicationWEB.AppMinutosCacheLongo);
                }

                bool incluirFinal = entityFormatoAvaliacao.fav_avaliacaoFinalAnalitica;

                List<Struct_CalendarioPeriodos> tabelaPeriodos = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(cal_id, ApplicationWEB.AppMinutosCacheLongo);

                List<ESC_EscolaCalendarioPeriodo> lstEscCalPeriodo = ESC_EscolaCalendarioPeriodoBO.SelectEscolasCalendarioCache(cal_id, ApplicationWEB.AppMinutosCacheCurto);

                List<Struct_CalendarioPeriodos> listaCalendarioPeriodo = tabelaPeriodos.Where(calP => (lstEscCalPeriodo.Where(escP => (escP.esc_id == esc_id && escP.tpc_id == calP.tpc_id)).Count() == 0)).ToList();

                int tpc_idUltimoPerido = tabelaPeriodos.Count > 0 ? tabelaPeriodos.Last().tpc_id : -1;
                int tpc_ordemUltimoPeriodo = tabelaPeriodos.Count > 0 ? tabelaPeriodos.Last().tpc_ordem : 0;

                //Busca o bimestre corrente
                Struct_CalendarioPeriodos periodoCorrente = listaCalendarioPeriodo.Where(x => (x.cap_dataInicio.Date <= DateTime.Now.Date && x.cap_dataFim.Date >= DateTime.Now.Date)).FirstOrDefault();
                int tpc_id = periodoCorrente.tpc_id;
                int tpc_ordem = periodoCorrente.tpc_ordem;
            }
        }

        /// <summary>
        /// Evento generico utilizando no grvTurma (docente) e grvTurmas (admin)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMinhasTurmas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            switch (e.CommandName)
            {
                case "PlanejamentoSemanal":
                {
                    RedirecionaTela("Planejamento semanal", "Cadastro", grid, e.CommandArgument.ToString(), true);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        protected void grvTurmas_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            
            if (!VS_visaoDocente)
            {
                UCTotalRegistros.Total = TUR_TurmaBO.GetTotalRecords();

                // Seta propriedades necessárias para ordenação nas colunas.
                ConfiguraColunasOrdenacao(grvTurmas);

                if ((!string.IsNullOrEmpty(grvTurmas.SortExpression)) && (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.PlanejamentoSemanal))
                {
                    Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

                    if (filtros.ContainsKey("VS_Ordenacao"))
                    {
                        filtros["VS_Ordenacao"] = grvTurmas.SortExpression;
                    }
                    else
                    {
                        filtros.Add("VS_Ordenacao", grvTurmas.SortExpression);
                    }

                    if (filtros.ContainsKey("VS_SortDirection"))
                    {
                        filtros["VS_SortDirection"] = grvTurmas.SortDirection.ToString();
                    }
                    else
                    {
                        filtros.Add("VS_SortDirection", grvTurmas.SortDirection.ToString());
                    }

                    __SessionWEB.BuscaRealizada = new BuscaGestao
                    {
                        PaginaBusca = PaginaGestao.PlanejamentoSemanal
                        ,
                        Filtros = filtros
                    };
                }
            }
            
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar();
        }

        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            // Limpa busca da sessão.
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            Response.Redirect("Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void rptTurmas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
                long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                int indice = e.Item.ItemIndex;
                int esc_id = 0;
                int cal_id = 0;

                // Id Escola
                HiddenField hdnId = e.Item.FindControl("hdnEscola") as HiddenField;
                if (hdnId != null && !string.IsNullOrEmpty(hdnId.Value))
                {
                    esc_id = Convert.ToInt32(hdnId.Value);
                }

                // Calendario
                HiddenField hdnCalendario = e.Item.FindControl("hdnCalendario") as HiddenField;
                if (hdnCalendario != null && !string.IsNullOrEmpty(hdnCalendario.Value))
                {
                    cal_id = Convert.ToInt32(hdnCalendario.Value);
                }

                int uni_id = 0;
                HiddenField hdnUnidadeEscola = e.Item.FindControl("hdnUnidadeEscola") as HiddenField;
                if (hdnUnidadeEscola != null && !string.IsNullOrEmpty(hdnUnidadeEscola.Value))
                {
                    uni_id = Convert.ToInt32(hdnUnidadeEscola.Value);
                }

                List<Struct_MinhasTurmas> dados = TUR_TurmaBO.SelecionaPorDocenteControleTurma(ent_id, doc_id, ApplicationWEB.AppMinutosCacheCurto);
                List<Struct_MinhasTurmas.Struct_Turmas> dadosTurmasAtivas = TUR_TurmaBO.SelecionaTurmasAtivasDocente(dados, esc_id, cal_id, true, false);

                GridView grdVw = e.Item.FindControl("grvTurma") as GridView;
                grdVw.DataSource = dadosTurmasAtivas;
                grdVw.DataBind();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvTurma_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridView grid = ((GridView)(sender));
                if (grid != null)
                {
                    Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
                    long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;

                    RepeaterItem itemTurma = (RepeaterItem)grid.NamingContainer;
                    int indice = itemTurma.ItemIndex;
                    int esc_id = 0;
                    int cal_id = 0;

                    // Id Escola
                    HiddenField hdnId = itemTurma.FindControl("hdnEscola") as HiddenField;
                    if (hdnId != null && !string.IsNullOrEmpty(hdnId.Value))
                    {
                        esc_id = Convert.ToInt32(hdnId.Value);
                    }

                    // Calendario
                    HiddenField hdnCalendario = itemTurma.FindControl("hdnCalendario") as HiddenField;
                    if (hdnCalendario != null && !string.IsNullOrEmpty(hdnCalendario.Value))
                    {
                        cal_id = Convert.ToInt32(hdnCalendario.Value);
                    }

                    grid.PageIndex = e.NewPageIndex;

                    List<Struct_MinhasTurmas> dados = TUR_TurmaBO.SelecionaPorDocenteControleTurma(ent_id, doc_id, ApplicationWEB.AppMinutosCacheCurto);
                    List<Struct_MinhasTurmas.Struct_Turmas> dadosTurmasAtivas = TUR_TurmaBO.SelecionaTurmasAtivasDocente(dados, esc_id, cal_id, true, false);

                    grid.DataSource = dadosTurmasAtivas;
                    grid.DataBind();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Eventos
    }
}
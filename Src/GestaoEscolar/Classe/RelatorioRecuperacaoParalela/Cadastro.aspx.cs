using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Classe.RelatorioRecuperacaoParalela
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Constantes

        private const int colunaDetalhar = 1;
        private const int colunaAlterar = 2;
        private const int colunaExcluir = 3;

        #endregion Constantes

        #region Propriedades

        private long VS_alu_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_alu_id"] ?? -1);
            }
            set
            {
                ViewState["VS_alu_id"] = value;
            }
        }

        private bool VS_disciplinaRP
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_disciplinaRP"] ?? false);
            }
            set
            {
                ViewState["VS_disciplinaRP"] = value;
            }
        }

        private sPermissoesRP VS_permissoesRP
        {
            get
            {
                if (ViewState["VS_permissoesRP"] == null)
                {
                    ViewState["VS_permissoesRP"] = new sPermissoesRP();
                }
                return (sPermissoesRP)ViewState["VS_permissoesRP"];
            }
            set
            {
                ViewState["VS_permissoesRP"] = value;
            }
        }

        /// <summary>
        /// Propriedade que seta a url de retorno da página.
        /// </summary>
        private string VS_PaginaRetorno
        {
            get
            {
                if (ViewState["VS_PaginaRetorno"] != null)
                    return ViewState["VS_PaginaRetorno"].ToString();

                return "";
            }
            set
            {
                ViewState["VS_PaginaRetorno"] = value;
            }
        }

        /// <summary>
        /// Propriedade que guarda dados necessários para a página de retorno.
        /// </summary>
        private object VS_DadosPaginaRetorno
        {
            get
            {
                return ViewState["VS_DadosPaginaRetorno"];
            }
            set
            {
                ViewState["VS_DadosPaginaRetorno"] = value;
            }
        }

        /// <summary>
        /// Propriedade que guarda dados necessários para a página de retorno Minhas turmas.
        /// </summary>
        private object VS_DadosPaginaRetorno_MinhasTurmas
        {
            get
            {
                return ViewState["VS_DadosPaginaRetorno_MinhasTurmas"];
            }
            set
            {
                ViewState["VS_DadosPaginaRetorno_MinhasTurmas"] = value;
            }
        }

        private byte VS_periodicidadePreenchimento
        {
            get
            {
                return Convert.ToByte(ViewState["VS_periodicidadePreenchimento"] ?? 0);
            }
            set
            {
                ViewState["VS_periodicidadePreenchimento"] = value;
            }
        }

        #endregion Propriedades

        #region Structs

        /// <summary>
        /// Estrutura que armazena as disciplinas para carregar o combo.
        /// </summary>
        [Serializable]
        private struct sDisciplinas
        {
            public string tud_nome { get; set; }
            public string tur_tud_id { get; set; }
        }

        /// <summary>
        /// Estrutura que armazena as permissões do usuário no relatório selecionado.
        /// </summary>
        [Serializable]
        private struct sPermissoesRP
        {
            public bool permissaoConsulta { get; set; }
            public bool permissaoEdicao { get; set; }
            public bool permissaoExclusao { get; set; }
        }

        #endregion Structs

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                RegistrarParametrosMensagemSair(true, (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.ExitPageConfirm));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsSetExitPageConfirmer.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroRelatorioRP.js"));
            }

            if (!IsPostBack)
            {
                if (Session["PaginaRetorno_RelatorioRP"] != null)
                {
                    try
                    {
                        VS_PaginaRetorno = Session["PaginaRetorno_RelatorioRP"].ToString();
                        Session.Remove("PaginaRetorno_RelatorioRP");
                        VS_DadosPaginaRetorno = Session["DadosPaginaRetorno"];
                        Session.Remove("DadosPaginaRetorno");
                        VS_DadosPaginaRetorno_MinhasTurmas = Session["VS_DadosTurmas"];
                        Session.Remove("VS_DadosTurmas");

                        Dictionary<string, string> listaDados = (Dictionary<string, string>)VS_DadosPaginaRetorno;
                        bool eletiva = Convert.ToByte(listaDados["Edit_tur_tipo"]) == (byte)TUR_TurmaTipo.EletivaAluno;
                        long tur_id = Convert.ToInt64(listaDados["Edit_tur_id"]);
                        long tud_id = Convert.ToInt64(listaDados["Tud_idRetorno_ControleTurma"]);
                        VS_disciplinaRP = eletiva;

                        VS_alu_id = Convert.ToInt64(Session["alu_id_RelatorioRP"]);
                        Session.Remove("alu_id_RelatorioRP");

                        // Carrega o combo de períodos do calendário
                        UCCPeriodoCalendario.CarregarPorTurma(tur_id);
                        UCCPeriodoCalendario.PermiteEditar = false;
                        UCCPeriodoCalendario.Tpc_ID = Convert.ToInt32(listaDados["Edit_tpc_id"]);

                        UCCRelatorioAtendimento.PermiteEditar = false;
                        fdsLancamento.Visible = false;
                        btnNovo.Visible = false;

                        // Carregar o combo de disciplinas
                        List<sDisciplinas> lstDisciplinas = new List<sDisciplinas>();
                        if (eletiva)
                        {
                            TUR_Turma turma = TUR_TurmaBO.GetEntity(new TUR_Turma { tur_id = tur_id });
                            TUR_TurmaDisciplina turmaDisciplina = TUR_TurmaDisciplinaBO.GetEntity(new TUR_TurmaDisciplina { tud_id = tud_id });

                            sDisciplinas disciplina = new sDisciplinas { tud_nome = string.Format("{0} - {1}", turma.tur_codigo, turmaDisciplina.tud_nome), tur_tud_id = string.Format("{0};{1}", tur_id, tud_id) };
                            lstDisciplinas.Add(disciplina);
                        }
                        else
                        {
                            int strTdsId = Convert.ToInt32(Session["tds_id_RelatorioRP"]);
                            Session.Remove("tds_id_RelatorioRP");

                            List<Struct_PreenchimentoAluno> lstAlunosRelatorioRP = CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.SelecionaAlunoPreenchimentoPorPeriodoDisciplina(UCCPeriodoCalendario.Tpc_ID, tur_id, tud_id, ApplicationWEB.AppMinutosCacheMedio);
                            lstAlunosRelatorioRP.FindAll(p => p.alu_id == VS_alu_id && (strTdsId <= 0 || p.tds_idRelacionada == strTdsId)).ForEach(
                                p => {
                                    sDisciplinas disciplina = new sDisciplinas { tud_nome = string.Format("{0} - {1}", p.tur_codigo, p.tud_nome), tur_tud_id = string.Format("{0};{1}", p.tur_id, p.tud_id) };
                                    lstDisciplinas.Add(disciplina);
                                });
                        }
                        ddlDisciplina.DataSource = lstDisciplinas;
                        ddlDisciplina.Items.Insert(0, new ListItem(string.Format("-- Selecione um(a) {0} --", GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA").ToString()), "-1;-1", true));
                        ddlDisciplina.DataBind();

                        if (ddlDisciplina.Items.Count == 2)
                        {
                            // Seleciona o único item
                            ddlDisciplina.SelectedIndex = 1;
                            ddlDisciplina_SelectedIndexChanged(null, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        ApplicationWEB._GravaErro(ex);
                        lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar relatórios.", UtilBO.TipoMensagem.Erro);
                    }
                }
            }

            UCCRelatorioAtendimento.IndexChanged += UCCRelatorioAtendimento_IndexChanged;
            UCCPeriodoCalendario.IndexChanged += UCCPeriodoCalendario_IndexChanged;

            if (!Convert.ToString(btnLimparBusca.CssClass).Contains("btnMensagemUnload"))
            {
                btnLimparBusca.CssClass += " btnMensagemUnload";
            }

            if (!Convert.ToString(btnVoltar.CssClass).Contains("btnMensagemUnload"))
            {
                btnVoltar.CssClass += " btnMensagemUnload";
            }

            if (!Convert.ToString(btnVoltarBaixo.CssClass).Contains("btnMensagemUnload"))
            {
                btnVoltarBaixo.CssClass += " btnMensagemUnload";
            }

            if (!Convert.ToString(btnCancelar.CssClass).Contains("btnMensagemUnload"))
            {
                btnCancelar.CssClass += " btnMensagemUnload";
            }

            if (!Convert.ToString(btnCancelarBaixo.CssClass).Contains("btnMensagemUnload"))
            {
                btnCancelarBaixo.CssClass += " btnMensagemUnload";
            }
        }

        private void UCCPeriodoCalendario_IndexChanged()
        {
            if (UCCPeriodoCalendario.Tpc_ID > 0)
            {
                UCCPeriodoCalendario.PermiteEditar = false;
                SetarTelaPermissao();

                string[] ids = ddlDisciplina.SelectedValue.Split(';');
                grvLancamentos.DataSource = CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.SelecionaPorAlunoTurmaDisciplinaRelatorioPeriodo(VS_alu_id, Convert.ToInt64(ids[1]), !VS_disciplinaRP, UCCRelatorioAtendimento.Valor, UCCPeriodoCalendario.Tpc_ID);
                grvLancamentos.DataBind();
            }
            else
            {
                fdsLancamento.Visible = false;
                btnNovo.Visible = false;
            }
        }

        protected void ddlDisciplina_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDisciplina.SelectedIndex > 0)
            {
                try
                {
                    // Desabilita o combo
                    ddlDisciplina.Enabled = false;

                    // Habilita o combo de relatórios
                    UCCRelatorioAtendimento.PermiteEditar = true;

                    string[] ids = ddlDisciplina.SelectedValue.Split(';');

                    // Carrega o combo de relatórios
                    UCCRelatorioAtendimento.CarregarRelatoriosRPDisciplina(VS_alu_id, Convert.ToInt64(ids[1]), !VS_disciplinaRP, !btnLimparBusca.Visible && UCCPeriodoCalendario.Tpc_ID <= 0 ? (byte)CLS_RelatorioAtendimentoPeriodicidade.Encerramento : (byte)0);
                    if (UCCRelatorioAtendimento.QuantidadeItensCombo == 2)
                    {
                        // Seleciona o único item
                        UCCRelatorioAtendimento.SelectedIndex = 1;
                        UCCRelatorioAtendimento_IndexChanged();
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar relatórios.", UtilBO.TipoMensagem.Erro);
                }
            }
            else
            {
                fdsLancamento.Visible = false;
                btnNovo.Visible = false;
            }
        }

        private void UCCRelatorioAtendimento_IndexChanged()
        {
            if (UCCRelatorioAtendimento.Valor > 0)
            {
                try
                {
                    // Desabilita o combo
                    UCCRelatorioAtendimento.PermiteEditar = false;

                    CLS_RelatorioAtendimento relatorio = CLS_RelatorioAtendimentoBO.GetEntity(new CLS_RelatorioAtendimento { rea_id = UCCRelatorioAtendimento.Valor });
                    
                    // Seleciona as permissões do usuário no relatório
                    MSTech.GestaoEscolar.BLL.RelatorioAtendimento relatorioAtendimento = CLS_RelatorioAtendimentoBO.SelecionaRelatorio(relatorio.rea_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id, ApplicationWEB.AppMinutosCacheLongo);
                    sPermissoesRP permissoesRP = new sPermissoesRP();
                    permissoesRP.permissaoEdicao = relatorioAtendimento.lstCargoPermissao.Any(p => p.rac_permissaoEdicao)
                                                    || relatorioAtendimento.lstGrupoPermissao.Any(p => p.rag_permissaoEdicao);
                    permissoesRP.permissaoConsulta = relatorioAtendimento.lstCargoPermissao.Any(p => p.rac_permissaoConsulta)
                                                    || relatorioAtendimento.lstGrupoPermissao.Any(p => p.rag_permissaoConsulta)
                                                    || permissoesRP.permissaoEdicao;
                    permissoesRP.permissaoExclusao = relatorioAtendimento.lstCargoPermissao.Any(p => p.rac_permissaoExclusao)
                                                    || relatorioAtendimento.lstGrupoPermissao.Any(p => p.rag_permissaoExclusao);
                    VS_permissoesRP = permissoesRP;

                    VS_periodicidadePreenchimento = relatorio.rea_periodicidadePreenchimento;
                    SetarTelaPermissao();

                    string[] ids = ddlDisciplina.SelectedValue.Split(';');
                    long tur_id = Convert.ToInt64(ids[0]);
                    long tud_id = Convert.ToInt64(ids[1]);

                    if (VS_periodicidadePreenchimento == (byte)CLS_RelatorioAtendimentoPeriodicidade.Periodico)
                    {
                        UCCPeriodoCalendario.Visible = true;
                        if (UCCPeriodoCalendario.Tpc_ID > 0)
                        {
                            grvLancamentos.DataSource = CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.SelecionaPorAlunoTurmaDisciplinaRelatorioPeriodo(VS_alu_id, tud_id, !VS_disciplinaRP, relatorio.rea_id, UCCPeriodoCalendario.Tpc_ID);
                            grvLancamentos.DataBind();
                        }
                        else
                        {
                            UCCPeriodoCalendario.PermiteEditar = true;
                        }
                    }
                    else
                    {
                        UCCPeriodoCalendario.Visible = false;
                        UCLancamentoRelatorioAtendimento.Carregar(VS_alu_id, tur_id, tud_id, -1, relatorio.rea_id);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar anotações.", UtilBO.TipoMensagem.Erro);
                }
            }
            else
            {
                fdsLancamento.Visible = false;
                btnNovo.Visible = false;
            }
        }

        protected void grvLancamentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExcluir != null)
                {
                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                }

                ImageButton btnAlterar = (ImageButton)e.Row.FindControl("btnAlterar");
                if (btnAlterar != null)
                {
                    btnAlterar.CommandArgument = e.Row.RowIndex.ToString();
                }

                ImageButton btnDetalhar = (ImageButton)e.Row.FindControl("btnDetalhar");
                if (btnDetalhar != null)
                {
                    btnDetalhar.CommandArgument = e.Row.RowIndex.ToString();
                }
            }
        }

        protected void grvLancamentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index, reap_id;
            if (e.CommandName == "Alterar" || e.CommandName == "Detalhar")
            {
                try
                {
                    index = int.Parse(e.CommandArgument.ToString());
                    reap_id = Convert.ToInt32(grvLancamentos.DataKeys[index].Values["reap_id"]);

                    // Esconde o grid de lançamentos
                    grvLancamentos.Visible = false;

                    string[] ids = ddlDisciplina.SelectedValue.Split(';');
                    long tur_id = Convert.ToInt64(ids[0]);
                    long tud_id = Convert.ToInt64(ids[1]);

                    // Carrega o lançamento cadastrado
                    UCLancamentoRelatorioAtendimento.Carregar(VS_alu_id, tur_id, tud_id, UCCPeriodoCalendario.Tpc_ID, UCCRelatorioAtendimento.Valor, false, reap_id);
                    pnlLancamento.Visible = true;
                    btnNovo.Visible = false;
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar lançamento de anotação.", UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "Deletar")
            {
                try
                {
                    index = int.Parse(e.CommandArgument.ToString());
                    reap_id = Convert.ToInt32(grvLancamentos.DataKeys[index].Values["reap_id"]);

                    string[] ids = ddlDisciplina.SelectedValue.Split(';');
                    if (CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.Delete(new CLS_RelatorioPreenchimentoAlunoTurmaDisciplina { reap_id = reap_id, tur_id = Convert.ToInt64(ids[0]), alu_id = VS_alu_id, tpc_id = UCCPeriodoCalendario.Tpc_ID }, UCCRelatorioAtendimento.Valor))
                    {
                        // Recarrega o grid de lançamentos
                        grvLancamentos.DataSource = CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.SelecionaPorAlunoTurmaDisciplinaRelatorioPeriodo(VS_alu_id, Convert.ToInt64(ids[1]), !VS_disciplinaRP, UCCRelatorioAtendimento.Valor, UCCPeriodoCalendario.Tpc_ID);
                        grvLancamentos.DataBind();

                        string msg = GetGlobalResourceObject("Classe", "RelatorioRecuperacaoParalela.Cadastro.MensagemSucessoExcluir").ToString();
                        lblMensagem.Text = UtilBO.GetErroMessage(msg, UtilBO.TipoMensagem.Sucesso);
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, msg + " | reap_id: " + reap_id);
                    }
                    else
                    {
                        lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar excluir lançamento de anotação.", UtilBO.TipoMensagem.Erro);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar excluir lançamento de anotação.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            try
            { 
                // Esconde o grid de lançamentos
                grvLancamentos.Visible = false;

                string[] ids = ddlDisciplina.SelectedValue.Split(';');
                long tur_id = Convert.ToInt64(ids[0]);
                long tud_id = Convert.ToInt64(ids[1]);

                // Carrega um novo lançamento
                UCLancamentoRelatorioAtendimento.Carregar(VS_alu_id, tur_id, tud_id, UCCPeriodoCalendario.Tpc_ID, UCCRelatorioAtendimento.Valor, false, 0);
                pnlLancamento.Visible = true;
                btnNovo.Visible = false;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar anotação.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnLimparBusca_Click(object sender, EventArgs e)
        {
            ddlDisciplina.SelectedIndex = 0;
            ddlDisciplina.Enabled = true;
            UCCRelatorioAtendimento.SelectedIndex = 0;
            UCCRelatorioAtendimento.PermiteEditar = false;
            UCCPeriodoCalendario.SelectedIndex = 0;
            UCCPeriodoCalendario.PermiteEditar = false;
            UCCPeriodoCalendario.Visible = false;
            fdsLancamento.Visible = false;
            btnNovo.Visible = false;
            if (ddlDisciplina.Items.Count == 2)
            {
                // Seleciona o único item
                ddlDisciplina.SelectedIndex = 1;
                ddlDisciplina_SelectedIndexChanged(null, null);
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            pnlLancamento.Visible = false;
            grvLancamentos.Visible = true;
            btnNovo.Visible = VS_disciplinaRP
                                && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar
                                && VS_permissoesRP.permissaoEdicao
                                && VS_periodicidadePreenchimento == (byte)CLS_RelatorioAtendimentoPeriodicidade.Periodico;
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            VerificaPaginaRedirecionar();
        }

        #endregion Eventos

        #region Métodos

        private void SetarTelaPermissao()
        {
            if (VS_periodicidadePreenchimento == (byte)CLS_RelatorioAtendimentoPeriodicidade.Encerramento
                || (VS_periodicidadePreenchimento == (byte)CLS_RelatorioAtendimentoPeriodicidade.Periodico && UCCPeriodoCalendario.Tpc_ID > 0))
            {
                // Exibe o botão para incluir novo apenas se for do tipo periódica
                btnNovo.Visible = VS_disciplinaRP
                                    && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar
                                    && VS_permissoesRP.permissaoEdicao
                                    && VS_periodicidadePreenchimento == (byte)CLS_RelatorioAtendimentoPeriodicidade.Periodico;

                // Exibe o botão salvar apenas se o usuário tem permissão
                UCLancamentoRelatorioAtendimento.VS_PermiteEditar = 
                    btnSalvar.Visible = btnSalvarBaixo.Visible = VS_disciplinaRP
                                                                    && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar
                                                                    && VS_permissoesRP.permissaoEdicao;

                // Carrega lançamentos
                fdsLancamento.Visible = true;

                btnLimparBusca.Visible = true;
            }

            if (VS_periodicidadePreenchimento == (byte)CLS_RelatorioAtendimentoPeriodicidade.Periodico)
            {
                if (UCCPeriodoCalendario.Tpc_ID > 0)
                {
                    pnlLancamento.Visible = false;
                    grvLancamentos.Visible = true;
                    btnCancelar.Visible = btnCancelarBaixo.Visible = true;

                    grvLancamentos.Columns[colunaDetalhar].Visible = (
                                                               // Se não for disciplina de recuperação paralela
                                                               // ou se o usuário não tem permissão de edição.
                                                               !VS_disciplinaRP
                                                               || !__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar
                                                               || !VS_permissoesRP.permissaoEdicao
                                                           )
                                                           && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                    grvLancamentos.Columns[colunaAlterar].Visible = VS_disciplinaRP
                                                                    && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar
                                                                    && VS_permissoesRP.permissaoEdicao;
                    grvLancamentos.Columns[colunaExcluir].Visible = VS_disciplinaRP
                                                                    && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir
                                                                    && VS_permissoesRP.permissaoExclusao;
                }
            }
            else
            {
                pnlLancamento.Visible = true;
                grvLancamentos.Visible = false;
                btnCancelar.Visible = btnCancelarBaixo.Visible = false;
            }
        }

        /// <summary>
        /// Verifica qual página deve voltar e redireciona.
        /// </summary>
        private void VerificaPaginaRedirecionar()
        {
            string url = "";

            if (!string.IsNullOrEmpty(VS_PaginaRetorno))
            {
                Session["DadosPaginaRetorno"] = VS_DadosPaginaRetorno;
                Session["VS_DadosTurmas"] = VS_DadosPaginaRetorno_MinhasTurmas;
                url = VS_PaginaRetorno;
            }

            if (!string.IsNullOrEmpty(url))
            {
                RedirecionarPagina(url);
            }
        }

        /// <summary>
        /// Salva o relatório preenchido.
        /// </summary>
        private void Salvar()
        {
            try
            {
                RelatorioPreenchimentoAluno rel = UCLancamentoRelatorioAtendimento.RetornaQuestionarioPreenchimento(false);
                List<CLS_AlunoDeficienciaDetalhe> lstAlunoDeficienciaDetalhe = UCLancamentoRelatorioAtendimento.RetornaListaDeficienciaDetalhe();
                if (CLS_RelatorioPreenchimentoBO.Salvar(rel, lstAlunoDeficienciaDetalhe, UCLancamentoRelatorioAtendimento.PermiteAlterarRacaCor, UCLancamentoRelatorioAtendimento.RacaCor))
                {
                    string msg = GetGlobalResourceObject("Classe", "RelatorioRecuperacaoParalela.Cadastro.MensagemSucessoSalvar").ToString();
                    lblMensagem.Text = UtilBO.GetErroMessage(msg, UtilBO.TipoMensagem.Sucesso);
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, msg + " | reap_id: " + rel.entityRelatorioPreenchimento.reap_id);

                    if (VS_periodicidadePreenchimento == (byte)CLS_RelatorioAtendimentoPeriodicidade.Periodico)
                    {
                        pnlLancamento.Visible = false;
                        grvLancamentos.Visible = true;
                        btnNovo.Visible = VS_disciplinaRP
                                && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar
                                && VS_permissoesRP.permissaoEdicao
                                && VS_periodicidadePreenchimento == (byte)CLS_RelatorioAtendimentoPeriodicidade.Periodico;

                        string[] ids = ddlDisciplina.SelectedValue.Split(';');

                        // Recarrega o grid de lançamentos
                        grvLancamentos.DataSource = CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.SelecionaPorAlunoTurmaDisciplinaRelatorioPeriodo(VS_alu_id, Convert.ToInt64(ids[1]), !VS_disciplinaRP, UCCRelatorioAtendimento.Valor, UCCPeriodoCalendario.Tpc_ID);
                        grvLancamentos.DataBind();
                    }
                }
            }
            catch (ValidationException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                updMensagem.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar salvar lançamento de anotação.", UtilBO.TipoMensagem.Erro);
                ApplicationWEB._GravaErro(ex);
                updMensagem.Update();
            }
        }

        #endregion Métodos
    }
}
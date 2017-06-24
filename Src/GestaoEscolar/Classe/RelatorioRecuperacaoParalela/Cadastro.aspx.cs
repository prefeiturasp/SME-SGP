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

        private const int colunaExcluir = 2;

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

        private long VS_tur_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_tur_id"] ?? -1);
            }
            set
            {
                ViewState["VS_tur_id"] = value;
            }
        }

        private long VS_tud_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_tud_id"] ?? -1);
            }
            set
            {
                ViewState["VS_tud_id"] = value;
            }
        }

        private bool VS_permiteEditar
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_permiteEditar"] ?? false);
            }
            set
            {
                ViewState["VS_permiteEditar"] = value;
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
            public string tds_nome { get; set; }
            public int tds_id { get; set; }
        }

        #endregion Structs

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
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
                        VS_tur_id = Convert.ToInt64(listaDados["Edit_tur_id"]);
                        VS_tud_id = Convert.ToInt64(listaDados["Tud_idRetorno_ControleTurma"]);
                        VS_permiteEditar = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && eletiva;

                        VS_alu_id = Convert.ToInt64(Session["alu_id_RelatorioRP"]);
                        Session.Remove("alu_id_RelatorioRP");

                        // Carrega o combo de períodos do calendário
                        UCCPeriodoCalendario.CarregarPorTurma(VS_tur_id);
                        UCCPeriodoCalendario.PermiteEditar = false;
                        UCCPeriodoCalendario.Tpc_ID = Convert.ToInt32(listaDados["Edit_tpc_id"]);

                        UCCRelatorioAtendimento.PermiteEditar = false;
                        updLancamento.Visible = false;

                        // Carregar o combo de disciplinas
                        string strTdsId = Session["tds_id_RelatorioRP"].ToString();
                        List<sDisciplinas> lstDisciplinas = new List<sDisciplinas>();
                        if (eletiva)
                        {
                            TUR_Turma turma = TUR_TurmaBO.GetEntity(new TUR_Turma { tur_id = VS_tur_id });
                            TUR_TurmaDisciplina turmaDisciplina = TUR_TurmaDisciplinaBO.GetEntity(new TUR_TurmaDisciplina { tud_id = VS_tud_id });

                            sDisciplinas disciplina = new sDisciplinas { tds_nome = string.Format("{0} - {1}", turma.tur_codigo, turmaDisciplina.tud_nome), tds_id = -1 };
                            lstDisciplinas.Add(disciplina);
                        }
                        else
                        {
                            List<string> lstStrTdsId = strTdsId.Split(',').ToList();
                            lstStrTdsId.ForEach(p => {
                                ACA_TipoDisciplina tipoDisciplina = ACA_TipoDisciplinaBO.GetEntity(new ACA_TipoDisciplina { tds_id = Convert.ToInt32(p) });
                                sDisciplinas disciplina = new sDisciplinas { tds_nome = tipoDisciplina.tds_nome, tds_id = tipoDisciplina.tds_id };
                                lstDisciplinas.Add(disciplina);
                            });
                        }
                        ddlDisciplina.DataSource = lstDisciplinas;
                        ddlDisciplina.Items.Insert(0, new ListItem(string.Format("-- Selecione um(a) {0} --", GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA").ToString()), "-1", true));
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

                    // Carrega o combo de relatórios
                    UCCRelatorioAtendimento.CarregarRelatoriosRPDisciplina(VS_alu_id, VS_tud_id, Convert.ToInt32(ddlDisciplina.SelectedValue));
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
        }

        private void UCCRelatorioAtendimento_IndexChanged()
        {
            if (UCCRelatorioAtendimento.Valor > 0)
            {
                try
                {
                    // Desabilita o combo
                    UCCRelatorioAtendimento.PermiteEditar = false;

                    // Exibe o botão para incluir novo apenas se for do tipo periódica
                    CLS_RelatorioAtendimento relatorio = CLS_RelatorioAtendimentoBO.GetEntity(new CLS_RelatorioAtendimento { rea_id = UCCRelatorioAtendimento.Valor });
                    divBotoes.Visible = VS_permiteEditar && relatorio.rea_periodicidadePreenchimento == (byte)CLS_RelatorioAtendimentoPeriodicidade.Periodico;

                    // Exibe o botão salvar apenas se o usuário tem permissão
                    btnSalvar.Visible = btnSalvarBaixo.Visible = VS_permiteEditar;

                    // Carrega lançamentos
                    VS_periodicidadePreenchimento = relatorio.rea_periodicidadePreenchimento;
                    if (VS_periodicidadePreenchimento == (byte)CLS_RelatorioAtendimentoPeriodicidade.Periodico)
                    {
                        UCCPeriodoCalendario.Visible = true;
                        pnlLancamento.Visible = false;
                        grvLancamentos.Visible = true;

                        grvLancamentos.Columns[colunaExcluir].Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir && VS_permiteEditar;
                        grvLancamentos.DataSource = CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.SelecionaPorAlunoTurmaDisciplinaRelatorioPeriodo(VS_alu_id, VS_tud_id, Convert.ToInt32(ddlDisciplina.SelectedValue), relatorio.rea_id, UCCPeriodoCalendario.Tpc_ID);
                        grvLancamentos.DataBind();
                    }
                    else
                    {
                        UCCPeriodoCalendario.Visible = false;
                        pnlLancamento.Visible = true;
                        grvLancamentos.Visible = false;
                        UCLancamentoRelatorioAtendimento.Carregar(VS_alu_id, VS_tur_id, VS_tud_id, -1, relatorio.rea_id);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar anotações.", UtilBO.TipoMensagem.Erro);
                }
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
            }
        }

        protected void grvLancamentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = int.Parse(e.CommandArgument.ToString());
            int reap_id = Convert.ToInt32(grvLancamentos.DataKeys[index].Values["reap_id"]);

            if (e.CommandName == "Alterar")
            {
                // Esconde o grid de lançamentos
                grvLancamentos.Visible = false;

                // Carrega o lançamento cadastrado
                UCLancamentoRelatorioAtendimento.Carregar(VS_alu_id, VS_tur_id, VS_tud_id, UCCPeriodoCalendario.Tpc_ID, UCCRelatorioAtendimento.Valor, false, reap_id);
                pnlLancamento.Visible = true;
            }
            else if (e.CommandName == "Deletar")
            {

            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            try
            { 
                // Esconde o grid de lançamentos
                grvLancamentos.Visible = false;

                // Carrega um novo lançamento
                pnlLancamento.Visible = true;
                UCLancamentoRelatorioAtendimento.Carregar(VS_alu_id, VS_tur_id, VS_tud_id, UCCPeriodoCalendario.Tpc_ID, UCCRelatorioAtendimento.Valor);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar anotação.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            VerificaPaginaRedirecionar();
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            VerificaPaginaRedirecionar();
        }

        #endregion Eventos

        #region Métodos

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

                        // Recarrega o grid de lançamentos
                        grvLancamentos.DataSource = CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.SelecionaPorAlunoTurmaDisciplinaRelatorioPeriodo(VS_alu_id, VS_tud_id, Convert.ToInt32(ddlDisciplina.SelectedValue), UCCRelatorioAtendimento.Valor, UCCPeriodoCalendario.Tpc_ID);
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
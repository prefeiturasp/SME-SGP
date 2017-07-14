using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Classe.RelatorioNaapa
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

        private sPermissoesNAAPA VS_permissoesNAAPA
        {
            get
            {
                if (ViewState["VS_permissoesNAAPA"] == null)
                {
                    ViewState["VS_permissoesNAAPA"] = new sPermissoesNAAPA();
                }
                return (sPermissoesNAAPA)ViewState["VS_permissoesNAAPA"];
            }
            set
            {
                ViewState["VS_permissoesNAAPA"] = value;
            }
        }

        #endregion Propriedades

        #region Structs

        /// <summary>
        /// Estrutura que armazena as permissões do usuário no relatório selecionado.
        /// </summary>
        [Serializable]
        private struct sPermissoesNAAPA
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
                sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroRelatorioNaapa.js"));
            }

            if (!IsPostBack)
            {
                try
                {
                    bool redirecionaPaginaBusca = true;
                    int idRelatorio = -1;

                    // Vem da tela de busca
                    if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                    {
                        VS_alu_id = PreviousPage.EditItemAluId;
                        VS_tur_id = PreviousPage.EditItemTurId;
                        redirecionaPaginaBusca = false;
                    }
                    // Vem da volta do imprimir ações realizadas
                    else if (Session["DadosPaginaRetorno"] != null)
                    {
                        string dadosRetorno = Session["DadosPaginaRetorno"].ToString();
                        Session.Remove("DadosPaginaRetorno");
                        Session.Remove("VS_DadosTurmas");

                        string[] vetDadosRetorno = dadosRetorno.Split(';');
                        if (vetDadosRetorno.Length == 3)
                        {
                            VS_alu_id = Convert.ToInt64(vetDadosRetorno[0]);
                            VS_tur_id = Convert.ToInt64(vetDadosRetorno[1]);
                            idRelatorio = Convert.ToInt32(vetDadosRetorno[2]);
                            redirecionaPaginaBusca = false;
                        }
                    }
                    if (redirecionaPaginaBusca)
                    {
                        RedirecionarPagina("~/Classe/RelatorioNaapa/Busca.aspx");
                    }

                    UCCRelatorioAtendimento.PermiteEditar = true;
                    fdsLancamento.Visible = false;
                    btnNovo.Visible = false;

                    // Carrega o combo de relatórios
                    UCCRelatorioAtendimento.CarregarPorPermissaoUsuarioTipo(CLS_RelatorioAtendimentoTipo.NAAPA);
                    if (UCCRelatorioAtendimento.QuantidadeItensCombo == 2)
                    {
                        // Seleciona o único item
                        UCCRelatorioAtendimento.SelectedIndex = 1;
                        UCCRelatorioAtendimento_IndexChanged();
                    }
                    else if (idRelatorio > 0)
                    {
                        UCCRelatorioAtendimento.Valor = idRelatorio;
                        UCCRelatorioAtendimento_IndexChanged();
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar relatórios.", UtilBO.TipoMensagem.Erro);
                }
            }

            UCCRelatorioAtendimento.IndexChanged += UCCRelatorioAtendimento_IndexChanged;

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

        private void UCCRelatorioAtendimento_IndexChanged()
        {
            if (UCCRelatorioAtendimento.Valor > 0)
            {
                try
                {
                    // Desabilita o combo
                    UCCRelatorioAtendimento.PermiteEditar = false;
                    litLancamento.Text = UCCRelatorioAtendimento.Texto;

                    // Seleciona as permissões do usuário no relatório
                    MSTech.GestaoEscolar.BLL.RelatorioAtendimento relatorioAtendimento = CLS_RelatorioAtendimentoBO.SelecionaRelatorio(UCCRelatorioAtendimento.Valor, __SessionWEB.__UsuarioWEB.Usuario.usu_id, ApplicationWEB.AppMinutosCacheLongo);
                    sPermissoesNAAPA permissoesNaapa = new sPermissoesNAAPA();
                    permissoesNaapa.permissaoEdicao = relatorioAtendimento.lstCargoPermissao.Any(p => p.rac_permissaoEdicao)
                                                    || relatorioAtendimento.lstGrupoPermissao.Any(p => p.rag_permissaoEdicao);
                    permissoesNaapa.permissaoConsulta = relatorioAtendimento.lstCargoPermissao.Any(p => p.rac_permissaoConsulta)
                                                    || relatorioAtendimento.lstGrupoPermissao.Any(p => p.rag_permissaoConsulta)
                                                    || permissoesNaapa.permissaoEdicao;
                    permissoesNaapa.permissaoExclusao = relatorioAtendimento.lstCargoPermissao.Any(p => p.rac_permissaoExclusao)
                                                    || relatorioAtendimento.lstGrupoPermissao.Any(p => p.rag_permissaoExclusao);
                    VS_permissoesNAAPA = permissoesNaapa;

                    SetarTelaPermissao();

                    grvLancamentos.DataSource = CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.SelecionaPorAlunoTurmaRelatorio(VS_alu_id, VS_tur_id, UCCRelatorioAtendimento.Valor);
                    grvLancamentos.DataBind();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar lançamentos.", UtilBO.TipoMensagem.Erro);
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

                    // Carrega o lançamento cadastrado
                    UCLancamentoRelatorioAtendimento.Carregar(VS_alu_id, VS_tur_id, -1, -1, UCCRelatorioAtendimento.Valor, false, reap_id);
                    pnlLancamento.Visible = true;
                    btnNovo.Visible = false;
                    lblMensagem.Text = string.Empty;
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar lançamento do relatório.", UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "Deletar")
            {
                try
                {
                    index = int.Parse(e.CommandArgument.ToString());
                    reap_id = Convert.ToInt32(grvLancamentos.DataKeys[index].Values["reap_id"]);

                    if (CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.Delete(new CLS_RelatorioPreenchimentoAlunoTurmaDisciplina { reap_id = reap_id }))
                    {
                        // Recarrega o grid de lançamentos
                        grvLancamentos.DataSource = CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.SelecionaPorAlunoTurmaRelatorio(VS_alu_id, VS_tur_id, UCCRelatorioAtendimento.Valor);
                        grvLancamentos.DataBind();

                        string msg = GetGlobalResourceObject("Classe", "RelatorioNaapa.Cadastro.MensagemSucessoExcluir").ToString();
                        lblMensagem.Text = UtilBO.GetErroMessage(msg, UtilBO.TipoMensagem.Sucesso);
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, msg + " | reap_id: " + reap_id);
                    }
                    else
                    {
                        lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar excluir lançamento do relatório.", UtilBO.TipoMensagem.Erro);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar excluir lançamento do relatório.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            try
            {
                // Esconde o grid de lançamentos
                grvLancamentos.Visible = false;

                // Carrega um novo lançamento
                UCLancamentoRelatorioAtendimento.Carregar(VS_alu_id, VS_tur_id, -1, -1, UCCRelatorioAtendimento.Valor, false, 0);
                pnlLancamento.Visible = true;
                btnNovo.Visible = false;
                lblMensagem.Text = string.Empty;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar relatório.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnLimparBusca_Click(object sender, EventArgs e)
        {
            UCCRelatorioAtendimento.SelectedIndex = 0;
            UCCRelatorioAtendimento.PermiteEditar = true;
            fdsLancamento.Visible = false;
            btnNovo.Visible = false;
            lblMensagem.Text = string.Empty;
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            pnlLancamento.Visible = false;
            grvLancamentos.Visible = true;
            btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir
                                && VS_permissoesNAAPA.permissaoEdicao;
            lblMensagem.Text = string.Empty;
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            VerificaPaginaRedirecionar();
        }
           
        #endregion Eventos

        #region Métodos

        private void SetarTelaPermissao()
        {
            // Exibe o botão para incluir novo apenas se o usuário tem permissão
            btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir
                                && VS_permissoesNAAPA.permissaoEdicao;

            // Exibe o botão salvar apenas se o usuário tem permissão
            UCLancamentoRelatorioAtendimento.VS_PermiteEditar =
                btnSalvar.Visible = btnSalvarBaixo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar
                                                                && VS_permissoesNAAPA.permissaoEdicao;

            // Carrega lançamentos
            fdsLancamento.Visible = true;
            btnLimparBusca.Visible = true;
            pnlLancamento.Visible = false;
            grvLancamentos.Visible = true;
            btnCancelar.Visible = btnCancelarBaixo.Visible = true;

            grvLancamentos.Columns[colunaDetalhar].Visible = (
                                                        !__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar
                                                        || !VS_permissoesNAAPA.permissaoEdicao
                                                    )
                                                    && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
            grvLancamentos.Columns[colunaAlterar].Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar
                                                            && VS_permissoesNAAPA.permissaoEdicao;
            grvLancamentos.Columns[colunaExcluir].Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir
                                                            && VS_permissoesNAAPA.permissaoExclusao;
        }

        /// <summary>
        /// Verifica qual página deve voltar e redireciona.
        /// </summary>
        private void VerificaPaginaRedirecionar()
        {
            RedirecionarPagina("~/Classe/RelatorioNaapa/Busca.aspx");
        }

        /// <summary>
        /// Salva o relatório preenchido.
        /// </summary>
        private void Salvar()
        {
            try
            {
                Page.Validate("geral");
                if (Page.IsValid)
                {
                    RelatorioPreenchimentoAluno rel = UCLancamentoRelatorioAtendimento.RetornaQuestionarioPreenchimento(false);
                    List<CLS_AlunoDeficienciaDetalhe> lstAlunoDeficienciaDetalhe = UCLancamentoRelatorioAtendimento.RetornaListaDeficienciaDetalhe();
                    List<CLS_RelatorioPreenchimentoAcoesRealizadas> lstAcoesRealizadas = UCLancamentoRelatorioAtendimento.RetornaListaAcoesRealizadas();

                    if (CLS_RelatorioPreenchimentoBO.Salvar(rel, lstAlunoDeficienciaDetalhe, UCLancamentoRelatorioAtendimento.PermiteAlterarRacaCor, UCLancamentoRelatorioAtendimento.RacaCor, lstAcoesRealizadas))
                    {
                        string msg = GetGlobalResourceObject("Classe", "RelatorioNaapa.Cadastro.MensagemSucessoSalvar").ToString();
                        lblMensagem.Text = UtilBO.GetErroMessage(msg, UtilBO.TipoMensagem.Sucesso);
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, msg + " | reap_id: " + rel.entityRelatorioPreenchimento.reap_id);

                        pnlLancamento.Visible = false;
                        grvLancamentos.Visible = true;
                        btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir
                                    && VS_permissoesNAAPA.permissaoEdicao;

                        // Recarrega o grid de lançamentos
                        grvLancamentos.DataSource = CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.SelecionaPorAlunoTurmaRelatorio(VS_alu_id, VS_tur_id, UCCRelatorioAtendimento.Valor);
                        grvLancamentos.DataBind();
                    }
                }
            }
            catch (ValidationException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar salvar lançamento do relatório.", UtilBO.TipoMensagem.Erro);
                ApplicationWEB._GravaErro(ex);
            }
        }

        #endregion Métodos
    }
}
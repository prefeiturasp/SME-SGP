using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Academico.RecursosHumanos.AtribuicaoEsporadica
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Retorna os dados do colaborador selecionado pela pesquisa.
        /// </summary>
        private ColaboradoresAtribuicao ColaboradorSelecionado
        {
            get
            {
                if (ViewState["ColaboradorSelecionado"] == null)
                    return new ColaboradoresAtribuicao();

                return (ColaboradoresAtribuicao)ViewState["ColaboradorSelecionado"];
            }
            set
            {
                ViewState["ColaboradorSelecionado"] = value;
            }
        }

        /// <summary>
        /// Indica se a tela está em modo de alteração.
        /// </summary>
        private bool Alteracao
        {
            get
            {
                if (ViewState["Alteracao"] == null)
                    return false;

                return (bool)ViewState["Alteracao"];
            }
            set
            {
                ViewState["Alteracao"] = value;
            }
        }

        /// <summary>
        /// Lista de escolas com tipo de classificação que possuem o cargo do colaborador selecionado.
        /// </summary>
        private List<sComboUAEscola> lstEscolasCargo
        {
            get
            {
                if (ViewState["lstEscolasCargo"] == null)
                {
                    ViewState["lstEscolasCargo"] = ColaboradorSelecionado.crg_id > 0 ?
                        ESC_UnidadeEscolaBO.SelecionaPorCargoTipoClassificacaoVigente(ColaboradorSelecionado.crg_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id, __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao, false) :
                        new List<sComboUAEscola>();
                }

                return (List<sComboUAEscola>)ViewState["lstEscolasCargo"];
            }

            set
            {
                ViewState["lstEscolasCargo"] = value;
            }
        }

        #endregion

        #region Delegates

        private void UCFiltroEscolas__Selecionar()
        {
            try
            {
                if (UCFiltroEscolas._VS_FiltroEscola)
                {
                    if (ColaboradorSelecionado.crg_id > 0 && !Alteracao)
                    {
                        UCFiltroEscolas._UnidadeEscola_LoadCombo(lstEscolasCargo.Where(p => p.uad_idSuperior == (UCFiltroEscolas._UCComboUnidadeAdministrativa_Uad_ID)).GroupBy(p => p.esc_uni_id).Select(p => p.First()).OrderBy(p => p.esc_uni_nome));
                    }
                    else
                    {
                        UCFiltroEscolas._UnidadeEscola_LoadBy_uad_idSuperior(UCFiltroEscolas._UCComboUnidadeAdministrativa_Uad_ID, false);
                    }
                }

                UCFiltroEscolas._ComboUnidadeEscola.Enabled = UCFiltroEscolas._ComboUnidadeAdministrativa.SelectedValue != Guid.Empty.ToString() && !Alteracao;

                if (UCFiltroEscolas._ComboUnidadeEscola.Enabled)
                    UCFiltroEscolas._ComboUnidadeEscola.Focus();

                upnEscola.Update();
            }                

            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Delegates

        #region Métodos

        /// <summary>
        /// Salva a atribuição esporádica para o docente selecionado.
        /// </summary>
        private void Salvar()
        {
            try
            {
                bool ret =
                    Alteracao ?
                    RHU_ColaboradorBO.SalvarAtribuicaoEsporadicaAlteracao
                    (
                        ColaboradorSelecionado
                        , Convert.ToDateTime(txtDataInicio.Text)
                        , Convert.ToDateTime(txtDataFim.Text)
                        , __SessionWEB.__UsuarioWEB.Usuario.ent_id
                    )
                    :
                    RHU_ColaboradorBO.SalvarAtribuicaoEsporadica
                    (
                        ColaboradorSelecionado
                        , __SessionWEB.__UsuarioWEB.Usuario.ent_id
                        , UCFiltroEscolas._UCComboUnidadeEscola_Esc_ID
                        , txtRF.Text
                        , Convert.ToDateTime(txtDataInicio.Text)
                        , Convert.ToDateTime(txtDataFim.Text)
                    );

                if (ret)
                {
                    ApplicationWEB._GravaLogSistema(Alteracao ? LOG_SistemaTipo.Update : LOG_SistemaTipo.Insert
                        , string.Format("Atribuição esporádica: col_id: {0}, crg_id: {1}, coc_id: {2}, doc_id: {3}"
                        , ColaboradorSelecionado.col_id, ColaboradorSelecionado.crg_id, ColaboradorSelecionado.coc_id
                        , ColaboradorSelecionado.doc_id));
                    RedirecionaBuscaMensagem("Atribuição salva com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    lblMensagem.Text = UtilBO.GetErroMessage("Não foi possível salvar a atribuição esporádica.", UtilBO.TipoMensagem.Erro);
                }
            }
            catch (ThreadAbortException)
            { }
            catch(ValidationException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a atribuição esporádica.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Redireciona pra busca com mensagem.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="tipo"></param>
        private void RedirecionaBuscaMensagem(string msg, UtilBO.TipoMensagem tipo)
        {
            try
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage(msg, tipo);
                Response.Redirect("~/Academico/RecursosHumanos/AtribuicaoEsporadica/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (ThreadAbortException)
            { }
        }

        /// <summary>
        /// Seta o botão de pesquisa como hide e mostra o de desfazer.
        /// </summary>
        private void SetaBotoesPesquisaRF()
        {
            btnRefazerPesquisa.Visible = true;
            btnPesquisar.Visible = false;
            txtRF.Enabled = false;
        }

        /// <summary>
        /// Carrega dados na tela para alteração (somente da vigência).
        /// </summary>
        /// <param name="atribuicao"></param>
        private void CarregarAlteracao(ColaboradoresAtribuicao atribuicao)
        {
            Alteracao = true;
            ColaboradorSelecionado = atribuicao;

            RHU_ColaboradorCargo entColaboradorCargo = RHU_ColaboradorCargoBO.GetEntity
                (new RHU_ColaboradorCargo
                {
                    col_id = atribuicao.col_id
                ,
                    crg_id = atribuicao.crg_id
                ,
                    coc_id = atribuicao.coc_id
                });

            ESC_Escola entEscola = ESC_EscolaBO.GetEntity(new ESC_Escola { esc_id = atribuicao.esc_id });
            SYS_UnidadeAdministrativa entUadSup;
            if (entEscola.uad_idSuperiorGestao != Guid.Empty)
            {
                entUadSup = SYS_UnidadeAdministrativaBO.GetEntity(
                    new SYS_UnidadeAdministrativa { ent_id = entEscola.ent_id, uad_id = entEscola.uad_idSuperiorGestao });
            }
            else
            {
                SYS_UnidadeAdministrativa entUad;
                entUad = SYS_UnidadeAdministrativaBO.GetEntity(
                    new SYS_UnidadeAdministrativa { ent_id = entEscola.ent_id, uad_id = entEscola.uad_id });

                entUadSup = SYS_UnidadeAdministrativaBO.GetEntity(
                    new SYS_UnidadeAdministrativa { ent_id = entEscola.ent_id, uad_id = entUad.uad_idSuperior });
            }

            lblMensagemInformacao.Text = string.Empty;

            // Seta valores.
            txtRF.Text = entColaboradorCargo.coc_matricula;
            txtNome.Text = atribuicao.pes_nome;
            UCFiltroEscolas._ComboUnidadeAdministrativa.SelectedValue = entUadSup.uad_id.ToString();
            UCFiltroEscolas__Selecionar();
            UCFiltroEscolas._ComboUnidadeEscola.SelectedValue = entEscola.esc_id + ";1";
            txtDataInicio.Text = entColaboradorCargo.coc_vigenciaInicio.ToString("dd/MM/yyyy");
            txtDataFim.Text = entColaboradorCargo.coc_vigenciaFim.ToString("dd/MM/yyyy");

            // Desabilita campos.
            SetaBotoesPesquisaRF();
            btnRefazerPesquisa.Visible = false;
            UCFiltroEscolas._ComboUnidadeAdministrativa.Enabled = false;
            UCFiltroEscolas._ComboUnidadeEscola.Enabled = false;

            txtDataInicio.Focus();

            upnDocente.Update();
            upnEscola.Update();
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager sm = ScriptManager.GetCurrent(this);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                }
                cvDataFimMaiorAtual.ValueToCompare = DateTime.Now.ToString("d");

                lblMensagemInformacao.Text = UtilBO.GetErroMessage
                    (GetGlobalResourceObject("Academico", "AtribuicaoEsporadica.Cadastro.lblMensagemInformacao.Text").ToString()
                        , UtilBO.TipoMensagem.Informacao);

                UCFiltroEscolas._Selecionar += UCFiltroEscolas__Selecionar;

                if (!IsPostBack)
                {
                    cvDataFim.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de fim");
                    cvDataInicio.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de início");

                    UCFiltroEscolas._LoadInicial(false);

                    if (PreviousPage != null && PreviousPage is Busca)
                    {
                        if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar)
                        {
                            RedirecionaBuscaMensagem("Usuário não autorizado.", UtilBO.TipoMensagem.Alerta);
                        }

                        // Carregar dados da atribuição para alterar.
                        CarregarAlteracao(PreviousPage.Atribuicao);
                    }
                    else
                    {
                        if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir)
                        {
                            RedirecionaBuscaMensagem("Usuário não autorizado.", UtilBO.TipoMensagem.Alerta);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }

        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Salvar();
            }
        }

        protected void btnPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                List<ColaboradoresAtribuicao> lista = RHU_ColaboradorBO.PesquisaPorFiltros_AtribuicaoEsporadica
                    (txtRF.Text, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                if (lista.Count > 0)
                {
                    if (UCFiltroEscolas._UCComboUnidadeEscola_Esc_ID > 0)
                    {
                        if (ESC_EscolaClassificacaoBO.VerificaExisteCargoClassificacao(UCFiltroEscolas._UCComboUnidadeEscola_Esc_ID, lista[0].crg_id))
                        {
                            ColaboradorSelecionado = lista[0];
                            txtNome.Text = ColaboradorSelecionado.pes_nome;
                            SetaBotoesPesquisaRF();

                            Guid uad_idSuperior = UCFiltroEscolas._UCComboUnidadeAdministrativa_Uad_ID;
                            int esc_id = UCFiltroEscolas._UCComboUnidadeEscola_Esc_ID;
                            int uni_id = UCFiltroEscolas._UCComboUnidadeEscola_Uni_ID;

                            lstEscolasCargo = ESC_UnidadeEscolaBO.SelecionaPorCargoTipoClassificacaoVigente(ColaboradorSelecionado.crg_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id, __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao, false);

                            if (UCFiltroEscolas._VS_FiltroEscola)
                            {
                                UCFiltroEscolas._UnidadeAdministrativa_LoadCombo(lstEscolasCargo.GroupBy(p => p.uad_idSuperior).Select(p => new sComboUAEscola { uad_id = p.Key, uad_nome = p.First().uad_nome }).OrderBy(p => p.uad_nome));
                                UCFiltroEscolas._ComboUnidadeAdministrativa.SelectedValue = uad_idSuperior.ToString();
                                UCFiltroEscolas__Selecionar();
                                UCFiltroEscolas._ComboUnidadeEscola.SelectedValue = string.Format("{0};{1}", esc_id, uni_id);
                            }
                            else
                            {
                                UCFiltroEscolas._UnidadeEscola_LoadCombo(lstEscolasCargo.GroupBy(p => p.esc_uni_id).Select(p => p.First()).OrderBy(p => p.esc_uni_nome));
                                UCFiltroEscolas._ComboUnidadeEscola.SelectedValue = string.Format("{0};{1}", esc_id, uni_id);
                            }
                        }
                        else
                        {
                            txtRF.Focus();
                            ColaboradorSelecionado = new ColaboradoresAtribuicao();
                            lblMensagemDocenteNaoEncontrado.Text = UtilBO.GetErroMessage(
                                GetGlobalResourceObject("Academico", "AtribuicaoEsporadica.Cadastro.lblMensagemDocenteNaoEncontradoCargoNaoPermitido.Text").ToString()
                                , UtilBO.TipoMensagem.Alerta);

                            lstEscolasCargo = new List<sComboUAEscola>();
                        }
                    }
                    else
                    {
                        ColaboradorSelecionado = lista[0];
                        txtNome.Text = ColaboradorSelecionado.pes_nome;
                        SetaBotoesPesquisaRF();

                        lstEscolasCargo = ESC_UnidadeEscolaBO.SelecionaPorCargoTipoClassificacaoVigente(ColaboradorSelecionado.crg_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id, __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao, false);

                        if (UCFiltroEscolas._VS_FiltroEscola)
                        {
                            UCFiltroEscolas._UnidadeAdministrativa_LoadCombo(lstEscolasCargo.GroupBy(p => p.uad_idSuperior).Select(p => new sComboUAEscola { uad_id = p.Key, uad_nome = p.First().uad_nome }).OrderBy(p => p.uad_nome));
                            UCFiltroEscolas._ComboUnidadeEscola.SelectedValue = "-1;-1";
                            UCFiltroEscolas._ComboUnidadeEscola.Enabled = false;
                        }
                        else
                        {
                            UCFiltroEscolas._UnidadeEscola_LoadCombo(lstEscolasCargo.GroupBy(p => p.esc_uni_id).Select(p => p.First()).OrderBy(p => p.esc_uni_nome));
                        }
                    }
                }
                else
                {
                    txtRF.Focus();
                    ColaboradorSelecionado = new ColaboradoresAtribuicao();
                    lblMensagemDocenteNaoEncontrado.Text = UtilBO.GetErroMessage(
                        GetGlobalResourceObject("Academico", "AtribuicaoEsporadica.Cadastro.lblMensagemDocenteNaoEncontrado.Text").ToString()
                        , UtilBO.TipoMensagem.Alerta);

                    lstEscolasCargo = new List<sComboUAEscola>();

                    UCFiltroEscolas._LoadInicial(false);
                }

                upnEscola.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os docentes.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Academico/RecursosHumanos/AtribuicaoEsporadica/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch
            {

            }
        }
                
        protected void btnRefazerPesquisa_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ColaboradorSelecionado = new ColaboradoresAtribuicao();
                lstEscolasCargo = new List<sComboUAEscola>();

                btnRefazerPesquisa.Visible = false;
                btnPesquisar.Visible = true;
                txtRF.Enabled = true;
                txtNome.Text = string.Empty;
                txtRF.Focus();
                upnDocente.Update();
                UCFiltroEscolas._LoadInicial(false);
                upnEscola.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion
    }
}
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

namespace GestaoEscolar.WebControls.LancamentoAvaliacao
{
    public partial class UCCadastroAvaliacao : MotherUserControl
    {
        #region Propriedades
        
        /// <summary>
        /// Armazena o ID da turma disciplina
        /// </summary>
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

        /// <summary>
        /// Armazena o ID da avaliacao
        /// </summary>
        private int VS_tnt_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_tnt_id"] ?? -1);
            }
            set
            {
                ViewState["VS_tnt_id"] = value;
            }
        }

        /// <summary>
        /// Retorna o tud_id selecionado no combo ddlComponenteAtAvaliativa.
        /// Valores do combo:
        /// [0] - Tur_id
        /// [1] - Tud_id
        /// [2] - Permissão
        /// [3] - Tud_tipo
        /// </summary>
        private long ddlComponenteAtAvaliativa_Tud_Id_Selecionado
        {
            get
            {
                if (!string.IsNullOrEmpty(ddlComponenteAtAvaliativa.SelectedValue))
                {
                    string[] valores = ddlComponenteAtAvaliativa.SelectedValue.Split(';');

                    if (valores.Length > 1)
                    {
                        return Convert.ToInt64(valores[1]);
                    }
                }

                return -1;
            }
        }

        #endregion Propriedades

        #region Delegates

        public delegate void commandRecarregarHabilidadesRelacionadas();

        public event commandRecarregarHabilidadesRelacionadas RecarregarHabilidadesRelacionadas;

        public delegate void commandSalvarAvaliacao();

        public event commandSalvarAvaliacao SalvarAvaliacao;

        public delegate void commandCancelarAvaliacao();

        public event commandCancelarAvaliacao CancelarAvaliacao;

        #endregion Delegates

        #region Métodos

        /// <summary>
        /// Limpa os campos.
        /// </summary>
        private void LimparCampos()
        {
            txtData.Text = string.Empty;
            if (ddlComponenteAtAvaliativa.Items.Count > 0)
                ddlComponenteAtAvaliativa.SelectedIndex = 0;
            txtNomeAtividade.Text = string.Empty;
            txtConteudoAtividade.Text = string.Empty;
            chkAtividadeExclusiva.Checked = false;
        }

        /// <summary>
        /// Habilita ou desabilita os campos de acordo com a permissao.
        /// </summary>
        private void HabilitarCampos(bool permiteEditar)
        {
            txtData.Enabled = permiteEditar;
            ddlComponenteAtAvaliativa.Enabled = permiteEditar;
            UCComboTipoAtividadeAvaliativa.PermiteEditar = permiteEditar;
            txtNomeAtividade.Enabled = permiteEditar;
            txtConteudoAtividade.Enabled = permiteEditar;
            chkAtividadeExclusiva.Enabled = permiteEditar;
            btnSalvarAtividade.Visible = permiteEditar;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MensagemSairCadastroAvaliacao", "var exibeMensagemSair=" + permiteEditar.ToString().ToLower() + ";", true);
        }

        /// <summary>
        /// Carrega componente(s) da regencia da turma selecionada.
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        public void CarregaComponenteRegencia(long tur_id, List<sComboTurmaDisciplina> lstDisciplinaComponenteDocente)
        {
            ddlComponenteAtAvaliativa.DataSource = lstDisciplinaComponenteDocente;
            ddlComponenteAtAvaliativa.DataBind();
        }

        /// <summary>
        /// Carrega uma nova avaliação ou uma avaliação para edição.
        /// </summary>
        /// <param name="tur_id"></param>
        /// <param name="tud_id"></param>
        /// <param name="tnt_id"></param>
        /// <param name="tud_tipo"></param>
        /// <param name="permiteEditar"></param>
        /// <param name="tpc_id"></param>
        /// <param name="cal_id"></param>
        /// <param name="tdt_posicao"></param>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <param name="crp_id"></param>
        public void CarregarAvaliacao(long tur_id
                                        , long tud_id
                                        , int tnt_id
                                        , byte tur_tipo
                                        , byte tud_tipo
                                        , bool permiteEditar
                                        , int tpc_id
                                        , int cal_id
                                        , byte tdt_posicao
                                        , int cur_id
                                        , int crr_id
                                        , int crp_id)
        {
            try
            {
                VS_tud_id = tud_id;
                VS_tnt_id = tnt_id;

                LimparCampos();
                HabilitarCampos(permiteEditar);
                lblComponenteAtAvaliativa.Visible = ddlComponenteAtAvaliativa.Visible = tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.Regencia || tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.ComponenteRegencia;
                chkAtividadeExclusiva.Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_ATIVIDADES_AVALIATIVAS_EXCLUSIVAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                fdsHabilidadesRelacionadas.Visible = tur_tipo != (byte)TUR_TurmaTipo.EletivaAluno && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.RELACIONAR_HABILIDADES_AVALIACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                CarregarHabilidadesAvaliacao(tur_id, tpc_id, cal_id, tdt_posicao, cur_id, crr_id, crp_id);
                UCComboTipoAtividadeAvaliativa.MostrarMessageOutros = false;

                if (tnt_id > 0)
                {
                    CLS_TurmaNota entity = new CLS_TurmaNota
                    {
                        tud_id = Convert.ToInt64(VS_tud_id),
                        tnt_id = Convert.ToInt32(VS_tnt_id)
                    };
                    CLS_TurmaNotaBO.GetEntity(entity);

                    UCComboTipoAtividadeAvaliativa.CarregaTipoAtividadeAvaliativaAtivosMaisInativo(true, entity.tav_id, VS_tud_id);
                    UCComboTipoAtividadeAvaliativa.PermiteEditar = false;
                    
                    if (tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                    {
		                if (entity.tud_id > 0)
		                {
                            IEnumerable<string> x = from ListItem lItem in ddlComponenteAtAvaliativa.Items
                                                    where lItem.Value.Split(';')[0].Equals(tur_id.ToString())
                                                    && lItem.Value.Split(';')[1].Equals(entity.tud_id.ToString())
				                select lItem.Value;
                            if (x.Count() > 0)
                                ddlComponenteAtAvaliativa.SelectedValue = x.First();
		                }
                        ddlComponenteAtAvaliativa.Enabled = false;
                    }

                    txtData.Text = entity.tnt_data == new DateTime() ? string.Empty : entity.tnt_data.ToString();
                    UCComboTipoAtividadeAvaliativa.Valor = entity.tav_id;
                    txtNomeAtividade.Text = entity.tnt_nome;
                    txtConteudoAtividade.Text = entity.tnt_descricao;
                    chkAtividadeExclusiva.Checked = entity.tnt_exclusiva;
                }
                else
                {
                    UCComboTipoAtividadeAvaliativa.CarregarTipoAtividadeAvaliativa(true, VS_tud_id);
                    UCComboTipoAtividadeAvaliativa.Valor = -1;
                }
                updAtividade.Update();

                if (txtData.Visible)
                {
                    txtData.Focus();
                }
                else if (ddlComponenteAtAvaliativa.Visible)
                {
                    ddlComponenteAtAvaliativa.Focus();
                }
                else
                {
                    UCComboTipoAtividadeAvaliativa.Focus();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAtividade.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("UserControl", "LancamentoAvaliacao.UCCadastroAvaliacao.lblMessageAtividade.ErroCarregar").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega as habilidades do planejamento de aulas
        /// </summary>                                        
        public void CarregarHabilidadesAvaliacao(long tur_id
                                                , int tpc_id
                                                , int cal_id
                                                , byte tdt_posicao
                                                , int cur_id
                                                , int crr_id
                                                , int crp_id)
        {
            if (fdsHabilidadesRelacionadas.Visible)
            {
                long tud_id = ddlComponenteAtAvaliativa.Visible ? ddlComponenteAtAvaliativa_Tud_Id_Selecionado : VS_tud_id;
                UCHabilidades.CarregarHabilidades(
                    cur_id,
                    crr_id,
                    crp_id,
                    tur_id,
                    tud_id,
                    cal_id,
                    tdt_posicao,
                    VS_tnt_id,
                    tpc_id
                );
            }
        }

        /// <summary>
        /// Salva a avaliação.
        /// </summary>     
        public bool SalvarNovaAtividade(TUR_Turma turma
                                        , int tpc_id
                                        , byte tdt_posicao
                                        , DateTime tur_dataEncerramento
                                        , DateTime cal_dataInicio
                                        , DateTime cal_dataFim
                                        , DateTime cap_dataInicio
                                        , DateTime cap_dataFim
                                        , bool fav_permiteRecuperacaoForaPeriodo
                                        , byte origemLogNota = 0
                                        , bool sucessoSalvarNotas = true)
        {
            try
            {
                if (!sucessoSalvarNotas)
                {
                    throw new ValidationException(GetGlobalResourceObject("UserControl", "LancamentoAvaliacao.UCCadastroAvaliacao.lblMessageAtividade.ErroSalvarNotasAlunos").ToString());
                }

                DateTime dataAtividade = new DateTime();
                if (!String.IsNullOrEmpty(txtData.Text))
                {
                    dataAtividade = Convert.ToDateTime(txtData.Text);
                    if (tur_dataEncerramento != new DateTime() && dataAtividade > tur_dataEncerramento)
                    {
                        throw new ValidationException(GetGlobalResourceObject("UserControl", "LancamentoAvaliacao.UCCadastroAvaliacao.lblMessageAtividade.ErroSalvarData").ToString());
                    }
                }

                CLS_TurmaNota entity;
                if (VS_tnt_id > 0)
                {    
                    entity = new CLS_TurmaNota
                    {
                        tnt_id = VS_tnt_id,
                        tud_id = ddlComponenteAtAvaliativa.Visible ? ddlComponenteAtAvaliativa_Tud_Id_Selecionado : VS_tud_id
                    };
                    CLS_TurmaNotaBO.GetEntity(entity);                  
                }
                else
                {
                    entity = new CLS_TurmaNota();
                    entity.tud_id = ddlComponenteAtAvaliativa.Visible ? ddlComponenteAtAvaliativa_Tud_Id_Selecionado : VS_tud_id;
                    entity.tur_id = turma.tur_id;
                    entity.usu_id = __SessionWEB.__UsuarioWEB.Usuario.usu_id;
                    entity.usu_idDocenteAlteracao = __SessionWEB.__UsuarioWEB.Usuario.usu_id;
                }

                entity.tpc_id = tpc_id;
                entity.tau_id = -1;
                entity.tnt_nome = txtNomeAtividade.Text;
                entity.tnt_descricao = txtConteudoAtividade.Text;
                entity.tnt_situacao = 1;
                entity.tav_id = UCComboTipoAtividadeAvaliativa.Valor;
                entity.tnt_data = dataAtividade;
                entity.tdt_posicao = tdt_posicao;
                entity.tnt_exclusiva = chkAtividadeExclusiva.Visible ? chkAtividadeExclusiva.Checked : false;

                if (CLS_TurmaNotaBO.Save(
                    entity
                    , turma
                    , cal_dataInicio
                    , cal_dataFim
                    , cap_dataInicio
                    , cap_dataFim
                    , __SessionWEB.__UsuarioWEB.Usuario.ent_id
                    , UCHabilidades.RetornaListaHabilidades()
                    , fav_permiteRecuperacaoForaPeriodo
                    , null
                    , null
                    , false
                    , __SessionWEB.__UsuarioWEB.Usuario.usu_id
                    , origemLogNota
                    , (byte)LOG_TurmaNota_Alteracao_Tipo.AlteracaoAtividade
                ))
                {
                    ApplicationWEB._GravaLogSistema(VS_tnt_id > 0 ? LOG_SistemaTipo.Update : LOG_SistemaTipo.Insert
                                                                            , "Atividade"
                                                                            + " | tpc_id: " + entity.tpc_id
                                                                            + " | tud_id: " + entity.tud_id
                                                                            + " | tnt_id: " + entity.tnt_id);
                    return true;
                }
            }
            catch (ValidationException ex)
            {
                lblMessageAtividade.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ArgumentException ex)
            {
                lblMessageAtividade.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAtividade.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("UserControl", "LancamentoAvaliacao.UCCadastroAvaliacao.lblMessageAtividade.ErroSalvar").ToString(), UtilBO.TipoMensagem.Erro);
            }
            return false;
        }
        #endregion Métodos

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                cvData.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data");
        }

        /// <summary>
        /// Tratamento para atualizar as habilidades para o componente da regencia.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlComponenteAtAvaliativa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (RecarregarHabilidadesRelacionadas != null)
                {
                    RecarregarHabilidadesRelacionadas();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAtividade.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("UserControl", "LancamentoAvaliacao.UCCadastroAvaliacao.lblMessageAtividade.ErroCarregar").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnSalvarAtividade_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    if (SalvarAvaliacao != null)
                    {
                        SalvarAvaliacao();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageAtividade.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("UserControl", "LancamentoAvaliacao.UCCadastroAvaliacao.lblMessageAtividade.ErroSalvar").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnCancelarAtividade_Click(object sender, EventArgs e)
        {
            if (CancelarAvaliacao != null)
            {
                CancelarAvaliacao();
            }
        }

        #endregion Eventos

    }
}
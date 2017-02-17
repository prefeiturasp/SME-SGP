using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Academico_Eventos_Cadastro : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Armazena o evt_id para edição
    /// </summary>
    private long _VS_evt_id
    {
        get
        {
            if (ViewState["_VS_evt_id"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_evt_id"]);
            }
            return -1;
        }
        set
        {
            ViewState["_VS_evt_id"] = value;
        }
    }

    /// <summary>
    /// ViewState com datatable de contatos
    /// Retorno e atribui valores para o DataTable de contatos
    /// </summary>
    public DataTable _VS_Calendario
    {
        get
        {
            if (ViewState["_VS_Calendario"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("cal_id");
                dt.Columns.Add("cal_id_Novo");
                dt.Columns.Add("cal_descricao");
                dt.Columns.Add("cal_ano");
                dt.Columns.Add("cal_periodo");
                ViewState["_VS_Calendario"] = dt;
            }
            return (DataTable)ViewState["_VS_Calendario"];
        }
        set
        {
            ViewState["_VS_Calendario"] = value;
        }
    }

    /// <summary>
    /// Armazena os calendarios excluidos do evento.
    /// </summary>
    public DataTable _VS_CalendarioExcluido
    {
        get
        {
            if (ViewState["_VS_CalendarioExcluido"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("cal_id");

                ViewState["_VS_CalendarioExcluido"] = dt;
            }
            return (DataTable)ViewState["_VS_CalendarioExcluido"];
        }
        set
        {
            ViewState["_VS_CalendarioExcluido"] = value;
        }
    }

    /// <summary>
    /// Armazena o sequencial para inclusão no DataTable
    /// </summary>
    public int _VS_seq
    {
        get
        {
            if (ViewState["_VS_seq"] != null)
                return Convert.ToInt32(ViewState["_VS_seq"]);
            return -1;
        }
        set
        {
            ViewState["_VS_seq"] = value;
        }
    }

    /// <summary>
    /// Indica se existe calendário para ser incluido no momento de salvar
    /// </summary>
    public bool _VS_IsNew
    {
        get
        {
            return Convert.ToBoolean(ViewState["_VS_IsNew"]);
        }
        set
        {
            ViewState["_VS_IsNew"] = value;
        }
    }

    /// <summary>
    /// Tipo de evento selecionado na tela.
    /// </summary>
    private ACA_TipoEvento _VS_TipoEvento
    {
        get { return ViewState["_VS_TipoEvento"] as ACA_TipoEvento; }
        set { ViewState["_VS_TipoEvento"] = value; }
    }

    /// <summary>
    /// Limites de período de eventos cadastrados para o tipo e calendários.
    /// </summary>
    private IEnumerable<ACA_EventoLimite> _VS_Limites
    {
        get { return ViewState["_VS_Limites"] as IEnumerable<ACA_EventoLimite>; }
        set { ViewState["_VS_Limites"] = value; }
    }

    /// <summary>
    /// Armazena o id do evento de efetivação de notas.
    /// </summary>
    public int VS_Evt_EfetivacaoNotas
    {
        get
        {
            if (ViewState["VS_Evt_EfetivacaoNotas"] != null)
                return Convert.ToInt32(ViewState["VS_Evt_EfetivacaoNotas"]);
            return -1; 
        }
        set
        {
            ViewState["VS_Evt_EfetivacaoNotas"] = value;
        }
    }

    /// <summary>
    /// Armazena o id do evento de efetivação da recuperação.
    /// </summary>
    public int VS_Evt_EfetivacaoRecuperacao
    {
        get
        {
            if (ViewState["VS_Evt_EfetivacaoRecuperacao"] != null)
                return Convert.ToInt32(ViewState["VS_Evt_EfetivacaoRecuperacao"]);
            return -1;
        }
        set
        {
            ViewState["VS_Evt_EfetivacaoRecuperacao"] = value;
        }
    }

    /// <summary>
    /// Armazena o id do evento de efetivação final.
    /// </summary>
    public int VS_Evt_EfetivacaoFinal
    {
        get
        {
            if (ViewState["VS_Evt_EfetivacaoFinal"] != null)
                return Convert.ToInt32(ViewState["VS_Evt_EfetivacaoFinal"]);
            return -1;
        }
        set
        {
            ViewState["VS_Evt_EfetivacaoFinal"] = value;
        }
    }

    /// <summary>
    /// Armazena o id do evento de efetivação da Recuperação Final.
    /// </summary>
    public int VS_Evt_EfetivacaoRecFinal
    {
        get
        {
            if (ViewState["VS_Evt_EfetivacaoRecFinal"] != null)
                return Convert.ToInt32(ViewState["VS_Evt_EfetivacaoRecFinal"]);
            return -1;
        }
        set
        {
            ViewState["VS_Evt_EfetivacaoRecFinal"] = value;
        }
    }
    
    #endregion Propriedades

    #region Delegates

    /// <summary>
    /// _s the uc filtro escolas__ selecionar.
    /// </summary>
    private void _UCFiltroEscolas__Selecionar()
    {
        try
        {
            if (_UCFiltroEscolas._VS_FiltroEscola)
                _UCFiltroEscolas._UnidadeEscola_LoadBy_uad_idSuperior(new Guid(_UCFiltroEscolas._ComboUnidadeAdministrativa.SelectedValue), true);

            _UCFiltroEscolas._ComboUnidadeEscola.Enabled = _UCFiltroEscolas._ComboUnidadeAdministrativa.SelectedValue != Guid.Empty.ToString();

            if (_UCFiltroEscolas._ComboUnidadeEscola.Enabled)
                _UCFiltroEscolas._ComboUnidadeEscola.Focus();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion Delegates

    #region Metodos

    /// <summary>
    /// Verifica se tem um calendário selecionado na tela.
    /// </summary>
    /// <returns></returns>
    private bool verificarCalendarioSelecionado(out List<int> calendariosSelecionados)
    {
        calendariosSelecionados = new List<int>();
        foreach (RepeaterItem item in rptCampos.Items)
        {
            CheckBox ckbCampo = (CheckBox)item.FindControl("ckbCampo");
            if (ckbCampo != null && ckbCampo.Checked)
            {
                HiddenField hdnId = (HiddenField)item.FindControl("hdnId");
                calendariosSelecionados.Add(Convert.ToInt32(hdnId.Value));
            }
        }

        return calendariosSelecionados.Count > 0;
    }

    /// <summary>
    /// Carrega os dados do evento na tela.
    /// </summary>
    /// <param name="evt_id">ID do evento</param>
    private void _LoadFromEntity(long evt_id)
    {
        try
        {
            ACA_Evento evento = new ACA_Evento { evt_id = evt_id };
            ACA_EventoBO.GetEntity(evento);

            ESC_Escola entEscola = new ESC_Escola
                                       {
                                           esc_id = evento.esc_id
                                       };
            ESC_EscolaBO.GetEntity(entEscola);
            SYS_UnidadeAdministrativa entUA = new SYS_UnidadeAdministrativa
                                                  {
                                                      ent_id = entEscola.ent_id,
                                                      uad_id = entEscola.uad_id
                                                  };
            SYS_UnidadeAdministrativaBO.GetEntity(entUA);

            if (evento.ent_id != __SessionWEB.__UsuarioWEB.Usuario.ent_id)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("O evento não pertence à entidade na qual você está logado.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Academico/Evento/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            _UCFiltroEscolas.SelecionaCombosAutomatico = false;
            //_UCFiltroEscolas._UnidadeEscola_Load(0);
            //_UCFiltroEscolas._ComboUnidadeEscola.DataBind();
            if (entEscola.esc_id > 0)
                _UCFiltroEscolas._ComboUnidadeEscola.SelectedValue = entEscola.esc_id + ";" + "1";


            Guid tua_id = ACA_ParametroAcademicoBO.VerificaFiltroEscolaPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            SYS_TipoUnidadeAdministrativa TipoUnidadeAdm = new SYS_TipoUnidadeAdministrativa { tua_id = tua_id };
            SYS_TipoUnidadeAdministrativaBO.GetEntity(TipoUnidadeAdm);

            _UCFiltroEscolas._LabelUnidadeAdministrativa.Text = string.IsNullOrEmpty(TipoUnidadeAdm.tua_nome) ?
                    "Unidade Administrativa" : TipoUnidadeAdm.tua_nome;
            _UCFiltroEscolas._UnidadeAdministrativa_LoadBy_tua_id_situacao(tua_id, Guid.Empty, 0);

            chkPadrao.Checked = evento.evt_padrao;
            if (chkPadrao.Checked)
            {
                _UCFiltroEscolas.EscolaCampoObrigatorio = false;
                _UCFiltroEscolas.UnidadeAdministrativaCampoObrigatorio = false;
                _UCFiltroEscolas._ComboUnidadeEscola.SelectedIndex = -1;
                _UCFiltroEscolas._ComboUnidadeAdministrativa.SelectedIndex = -1;
            }
            else
            {
                _UCFiltroEscolas.EscolaCampoObrigatorio = true;
                _UCFiltroEscolas.UnidadeAdministrativaCampoObrigatorio = true;
                if (_UCFiltroEscolas._ComboUnidadeAdministrativa.Visible)
                {
                    Guid uad_idSuperior = entEscola.uad_idSuperiorGestao.Equals(Guid.Empty) ? entUA.uad_idSuperior : entEscola.uad_idSuperiorGestao;

                    _UCFiltroEscolas._UnidadeEscola_LoadBy_uad_idSuperior(new Guid(_UCFiltroEscolas._ComboUnidadeAdministrativa.SelectedValue), true);
                    _UCFiltroEscolas._ComboUnidadeAdministrativa.SelectedValue = uad_idSuperior.ToString();
                }
                _UCFiltroEscolas._ComboUnidadeEscola.SelectedValue = Convert.ToString(evento.esc_id + ";" + evento.uni_id).Equals("0;0") ? "-1;-1" : Convert.ToString(evento.esc_id + ";" + evento.uni_id);
            }

            _VS_evt_id = evento.evt_id;
            _UCComboTipoEvento.CarregarTipoEvento(0);
            _UCComboTipoEvento.Valor = evento.tev_id;
            _UCComboTipoEvento.PermiteEditar = false;

            ACA_TipoEvento tipoEvento = new ACA_TipoEvento { tev_id = evento.tev_id };
            ACA_TipoEventoBO.GetEntity(tipoEvento);

            if (tipoEvento.tev_periodoCalendario)
            {
                MostraTipoPeriodoCalendario(true);
                UCCTipoPeriodoCalendario1.Valor = evento.tpc_id;
            }
            else
            {
                MostraTipoPeriodoCalendario(false);
            }

            // carrega as listas para selecao de calendários
            CarregaCalendarios(evento.tpc_id);
            //**********************************

            _txtNome.Text = evento.evt_nome;
            _txtDescricao.Text = evento.evt_descricao;
            _txtInicioEvento.Text = evento.evt_dataInicio.ToString("dd/MM/yyyy");
            _txtFimEvento.Text = evento.evt_dataFim.ToString("dd/MM/yyyy");
            //_ckbAtividadeDiscente.Checked = evento.evt_semAtividadeDiscente.Equals(true);

            rblAtividadeDiscente.SelectedValue = evento.evt_semAtividadeDiscente.Equals(true) ? "True" : "False";

            /// dados que não podem ser alterados:
            chkPadrao.Enabled = false;
            _UCFiltroEscolas._ComboUnidadeEscola.Enabled = false;
            _UCFiltroEscolas._cvUnidadeEscola.Enabled = false;
            _UCFiltroEscolas._ComboUnidadeAdministrativa.Enabled = false;
            _UCFiltroEscolas._cvUnidadeAdministrativa.Enabled = false;
            _UCComboTipoEvento.PermiteEditar = false;
            UCCTipoPeriodoCalendario1.PermiteEditar = false;
            //_ckbAtividadeDiscente.Enabled = false;
            rblAtividadeDiscente.Enabled = false;

            DateTime dtIni = evento.evt_dataInicio;

            bool param = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_CADASTRO_EVENTO_RETROATIVO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            bool param_discente = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_CADASTRO_EVENTO_RETROATIVO_SEM_ATIVIDADE_DISCENTE, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            if (((dtIni <= DateTime.Today) && evento.evt_semAtividadeDiscente && param && !param_discente)
                ||
                ((dtIni <= DateTime.Today) && !evento.evt_semAtividadeDiscente))
            {
                _txtInicioEvento.Enabled = false;
               
                foreach (RepeaterItem item in rptCampos.Items)
                {
                    CheckBox ckbCampo = (CheckBox)item.FindControl("ckbCampo");
                    if (ckbCampo != null)
                    {
                        ckbCampo.Enabled = false;
                    }
                }
            }

            DateTime dtFim = Convert.ToDateTime(_txtFimEvento.Text);
            if (dtFim <= DateTime.Today)
            {
                _txtFimEvento.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o evento.", UtilBO.TipoMensagem.Erro);
        }

    }

    /// <summary>
    /// Método que valida os campos do filto escola
    /// </summary>
    /// <param name="validar">variável que determina a validação</param>
    private void _ValidaCamposFiltroEscola(bool validar)
    {
        _UCFiltroEscolas.EscolaCampoObrigatorio = validar;
        _UCFiltroEscolas.UnidadeAdministrativaCampoObrigatorio = validar;
        _UCFiltroEscolas._LoadInicial(true, true);
        _UCFiltroEscolas._ComboUnidadeEscola.SelectedValue = "-1;-1";
        _UCFiltroEscolas._ComboUnidadeAdministrativa.Enabled = validar;
        _UCFiltroEscolas._cvUnidadeAdministrativa.Enabled = validar;
        if ((__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa) || (!validar))
        {
            _UCFiltroEscolas._ComboUnidadeEscola.Enabled = validar;
            _UCFiltroEscolas._cvUnidadeAdministrativa.Enabled = validar;
        }
    }

    /// <summary>
    /// Salva os dados do evento.
    /// </summary>
    /// <param name="bValidaDataInicial"></param>
    public void _Salvar(bool bValidaDataInicial)
    {
        try
        {
            ACA_Evento Evento = new ACA_Evento
            {
                evt_id = _VS_evt_id
                ,
                ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id
                ,
                tev_id = _UCComboTipoEvento.Valor
                ,
                esc_id = _UCFiltroEscolas._UCComboUnidadeEscola_Esc_ID
                ,
                uni_id = _UCFiltroEscolas._UCComboUnidadeEscola_Uni_ID
                ,
                evt_nome = _txtNome.Text
                ,
                evt_descricao = _txtDescricao.Text
                ,
                evt_dataInicio = Convert.ToDateTime(_txtInicioEvento.Text)
                ,
                evt_dataFim = Convert.ToDateTime(_txtFimEvento.Text)
                ,
                evt_semAtividadeDiscente = Convert.ToBoolean(rblAtividadeDiscente.SelectedValue)
                    //_ckbAtividadeDiscente.Checked
                ,
                evt_padrao = chkPadrao.Checked
                ,
                tpc_id = UCCTipoPeriodoCalendario1.Valor
                ,
                evt_situacao = 1
                ,
                IsNew = (_VS_evt_id > 0) ? false : true
                ,
                evt_limitarDocente = false
            };

            List<int> calendarios;
            if (!verificarCalendarioSelecionado(out calendarios))
            {
                throw new MSTech.Validation.Exceptions.ValidationException("Selecione pelo menos um calendário para o evento.");
            }
            
            // carrega datatable com os calendários selecionados
            CriarListaCalendarioEvento();

            if (ACA_EventoBO.Salvar
            (
                Evento
                , _VS_Calendario
                , _VS_CalendarioExcluido
                , bValidaDataInicial
                , __SessionWEB.__UsuarioWEB.Usuario.ent_id
                , __SessionWEB.__UsuarioWEB.Grupo.vis_id
            ))
            {
                if (_VS_evt_id > 0)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "evt_id: " + Evento.evt_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Evento alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "evt_id: " + Evento.evt_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Evento incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }

                Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/Evento/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o evento.", UtilBO.TipoMensagem.Erro);
            }
        }
        catch (DataAnteriorException)
        {
            btnConfirmarEvento.Text = _VS_evt_id > 0 ?
                "Confirmar alteração do evento" : "Confirmar inclusão do evento";
            btnCancelarEvento.Text = _VS_evt_id > 0 ?
                "Cancelar alteração do evento" : "Cancelar inclusão do evento";

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "ValidaDataInicial", "$(document).ready(function(){ $('#divDataAnterior').dialog('open'); });", true);
        }
        catch (MSTech.Validation.Exceptions.ValidationException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (DuplicateNameException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
        }
        catch (ArgumentException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o evento.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Mostra ou esconde o combo de tipo periodo calendario
    /// </summary>
    private void MostraTipoPeriodoCalendario(bool bMostra)
    {
        UCCTipoPeriodoCalendario1.ExibeCombo = bMostra;
        UCCTipoPeriodoCalendario1.PermiteEditar = bMostra;
        UCCTipoPeriodoCalendario1.Obrigatorio = bMostra;
        UCCTipoPeriodoCalendario1.Valor = -1;

        if (bMostra)
            UCCTipoPeriodoCalendario1.ValidationGroup = "evento";
    }

    /// <summary>
    /// Cria lista de entidades de CalendarioCurso de acordo com os calendários selecionados
    /// </summary>
    private void CriarListaCalendarioEvento()
    {
        _VS_Calendario = null;
        DataTable dtTemp = _VS_Calendario;

        foreach (RepeaterItem item in rptCampos.Items)
        {
            CheckBox ckbCampo = (CheckBox)item.FindControl("ckbCampo");

            if (ckbCampo != null && ckbCampo.Checked)
            {
                HiddenField hdnId = (HiddenField)item.FindControl("hdnId");
                DataRow dr = dtTemp.NewRow();

                ACA_CalendarioAnual calendario = new ACA_CalendarioAnual { cal_id = Convert.ToInt32(hdnId.Value) };
                ACA_CalendarioAnualBO.GetEntity(calendario);

                dr["cal_id"] = calendario.cal_id;
                dr["cal_id_novo"] = calendario.cal_id;
                dr["cal_descricao"] = calendario.cal_descricao;
                dr["cal_ano"] = calendario.cal_ano;
                if (calendario.cal_dataFim.Equals(null))
                    dr["cap_periodo"] = calendario.cal_dataInicio.ToString("dd/MM/yyyy");
                else
                    dr["cal_periodo"] = calendario.cal_dataInicio.ToString("dd/MM/yyyy") + " - " + calendario.cal_dataFim.ToString("dd/MM/yyyy");
                dtTemp.Rows.Add(dr);
            }
        }
        _VS_Calendario = dtTemp;
    }

    /// <summary>
    /// MEtodo que carrega os calendário nos lists box conforme o  tipo de periodo selecionado no combo (tpc_id)
    /// Carrega calendários disponíveis para serem selecionados e quando é alteracao carrega os ja selecionados no list de associados
    /// </summary>
    /// <param name="tpc_id"></param>
    private void CarregaCalendarios(int tpc_id)
    {
        // carrega as listas para selecao de calendários
        DataTable dtCampos = ACA_CalendarioEventoBO.Select_naoAssociados(__SessionWEB.__UsuarioWEB.Usuario.ent_id, _VS_evt_id, tpc_id);
        DataTable dtAssociados = ACA_CalendarioEventoBO.Select_Associados(__SessionWEB.__UsuarioWEB.Usuario.ent_id, _VS_evt_id);
        dtCampos.Merge(dtAssociados);

        rptCampos.DataSource = dtCampos.AsEnumerable().OrderBy(r => r["cal_descricao"])
                               .Select(p => new { cal_id = p["cal_id"], cal_descricao = p["cal_descricao"] });
        rptCampos.DataBind();

        foreach (RepeaterItem item in rptCampos.Items)
        {
            CheckBox ckbCampo = (CheckBox)item.FindControl("ckbCampo");
            HiddenField hdnId = (HiddenField)item.FindControl("hdnId");

            if (ckbCampo != null && hdnId != null)
            {
                ckbCampo.Checked = dtAssociados.AsEnumerable().Any(r => Convert.ToInt32(r["cal_id"]) == Convert.ToInt32(hdnId.Value));
            }
        }
    }

    /// <summary>
    /// Recarrega os limites de acordo com os filtros da tela.
    /// </summary>
    private void CarregarLimites()
    {
        if ((ACA_TipoEventoBO.eLiberacao)_VS_TipoEvento.tev_liberacao == ACA_TipoEventoBO.eLiberacao.Desnecessaria)
            _VS_Limites = null;
        else
            _VS_Limites = ACA_EventoLimiteBO.GetSelectByTipoEvento(_VS_TipoEvento.tev_id);
    }

    #endregion Metodos

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {

        if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
        {
            __SessionWEB.PostMessages = "Usuário não possui permissão para acessar essa página.";
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Index.aspx", false);
        }

        try
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference("~/Includes/JS-ModuloAcademico.js"));
            }

            if (!IsPostBack)
            {
                cvDataInicio.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de início do evento");
                cvDataFim.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de fim do evento");

                //Carregar a tela validando os campos necessários do filtro escola.
                _ValidaCamposFiltroEscola(true);

                UCCTipoPeriodoCalendario1.CarregarTipoPeriodoCalendario(false);

                MostraTipoPeriodoCalendario(false);
                chkPadrao.Checked = false;

                // busco eventos parametrizados para as efetivações    
                VS_Evt_EfetivacaoNotas = Convert.ToInt32(ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id));
                VS_Evt_EfetivacaoRecuperacao = Convert.ToInt32(ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_RECUPERACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id));
                VS_Evt_EfetivacaoFinal = Convert.ToInt32(ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id));
                VS_Evt_EfetivacaoRecFinal = Convert.ToInt32(ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_RECUPERACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id));

                if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                {
                    _LoadFromEntity(PreviousPage.EditItem);
                    Page.Form.DefaultFocus = _txtNome.ClientID;
                }
                else
                {
                    // carrega as listas para selecao de calendários
                    CarregaCalendarios(0);

                    _UCComboTipoEvento.CarregarTipoEvento(1);
                    _UCComboTipoEvento.Obrigatorio = true;
                    _UCComboTipoEvento.ValidationGroup = "evento";

                    if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_SELECIONADO_COM_ATIVIDADE_DISCENTE, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                    {   // somente deve marcar a opção se "Com atividade discente" se o parametro estiver com Sim, caso contrario, o radio
                        // fica em branco
                        rblAtividadeDiscente.SelectedValue = "False";
                    }

                    Page.Form.DefaultFocus = _UCFiltroEscolas._VS_FiltroEscola ? _UCFiltroEscolas._ComboUnidadeAdministrativa.ClientID : _UCFiltroEscolas._ComboUnidadeEscola.ClientID;

                    _btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
                }

                Page.Form.DefaultButton = _btnSalvar.UniqueID;
                chkPadrao.Visible = __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao;
            }

            // Inicializacao dos DELEGATES
            _UCFiltroEscolas._Selecionar += _UCFiltroEscolas__Selecionar;
            _UCComboTipoEvento.IndexChanged += _UCComboTipoEvento_IndexChanged;
            UCCTipoPeriodoCalendario1.IndexChanged += UCCTipoPeriodoCalendario_IndexChanged;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Metodo executado por delegate que 
    /// </summary>
    public void _UCComboTipoEvento_IndexChanged()
    {
        try
        {
            if (_UCComboTipoEvento.SelectedIndex == 0)
                MostraTipoPeriodoCalendario(false);
            else
            {
                string parametroAtivDiversificada = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_ATIVIDADE_DIVERSIFICADA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                ACA_TipoEvento tipoEvento = new ACA_TipoEvento { tev_id = _UCComboTipoEvento.Valor };
                ACA_TipoEventoBO.GetEntity(tipoEvento);

                if (tipoEvento.tev_periodoCalendario)
                {
                    MostraTipoPeriodoCalendario(true);
                }
                else
                {
                    MostraTipoPeriodoCalendario(false);
                    CarregaCalendarios(0);
                }

                if (parametroAtivDiversificada.Equals(_UCComboTipoEvento.Valor.ToString()))
                {
                    rblAtividadeDiscente.SelectedValue = "False";
                    rblAtividadeDiscente.Enabled = false;
                }
                else
                {
                    rblAtividadeDiscente.Enabled = true;
                }

                _VS_TipoEvento = tipoEvento;
                CarregarLimites();

                List<int> calendarios;

                if (verificarCalendarioSelecionado(out calendarios))
                {
                    string msg;
                    if (!ACA_EventoBO.ValidarLimite(false, _VS_TipoEvento, _VS_Limites, chkPadrao.Checked
                        , _UCFiltroEscolas._UCComboUnidadeEscola_Esc_ID, _UCFiltroEscolas._UCComboUnidadeEscola_Uni_ID
                        , UCCTipoPeriodoCalendario1.Valor
                        , calendarios
                        , _txtInicioEvento.Text
                        , _txtFimEvento.Text
                        , out msg
                        ))
                    {
                        _lblMessage.Text = UtilBO.GetErroMessage(msg, UtilBO.TipoMensagem.Alerta);
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    public void UCCTipoPeriodoCalendario_IndexChanged()
    {
        try
        {
            int tpc_id = UCCTipoPeriodoCalendario1.Valor;

            // carrega as listas para selecao de calendários
            CarregaCalendarios(tpc_id);

            //**********************************
            List<int> calendarios;

            if (verificarCalendarioSelecionado(out calendarios))
            {
                string msg;
                if (!ACA_EventoBO.ValidarLimite(false, _VS_TipoEvento, _VS_Limites, chkPadrao.Checked
                    , _UCFiltroEscolas._UCComboUnidadeEscola_Esc_ID, _UCFiltroEscolas._UCComboUnidadeEscola_Uni_ID
                    , UCCTipoPeriodoCalendario1.Valor
                    , calendarios
                    , _txtInicioEvento.Text
                    , _txtFimEvento.Text
                    , out msg
                    ))
                {
                    _lblMessage.Text = UtilBO.GetErroMessage(msg, UtilBO.TipoMensagem.Alerta);
                }
            }

        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// De acordo com checkbox é validado o campo filtro escola
    /// </summary>
    protected void chkPadrao_CheckedChanged(object sender, EventArgs e)
    {
        _ValidaCamposFiltroEscola(!chkPadrao.Checked);
        List<int> calendarios;

        if (verificarCalendarioSelecionado(out calendarios))
        {
            string msg;
            if (!ACA_EventoBO.ValidarLimite(false, _VS_TipoEvento, _VS_Limites, chkPadrao.Checked
                , _UCFiltroEscolas._UCComboUnidadeEscola_Esc_ID, _UCFiltroEscolas._UCComboUnidadeEscola_Uni_ID
                , UCCTipoPeriodoCalendario1.Valor
                , calendarios
                , _txtInicioEvento.Text
                , _txtFimEvento.Text
                , out msg
                ))
            {
                _lblMessage.Text = UtilBO.GetErroMessage(msg, UtilBO.TipoMensagem.Alerta);
            }
        }
    }

    protected void _UCFiltroEscolas__SelecionarEscola()
    {
        List<int> calendarios;
        if (verificarCalendarioSelecionado(out calendarios))
        {
            string msg;
            if (!ACA_EventoBO.ValidarLimite(false, _VS_TipoEvento, _VS_Limites, chkPadrao.Checked
                , _UCFiltroEscolas._UCComboUnidadeEscola_Esc_ID, _UCFiltroEscolas._UCComboUnidadeEscola_Uni_ID
                , UCCTipoPeriodoCalendario1.Valor
                , calendarios
                , _txtInicioEvento.Text
                , _txtFimEvento.Text
                , out msg
                ))
            {
                _lblMessage.Text = UtilBO.GetErroMessage(msg, UtilBO.TipoMensagem.Alerta);
            }
        }
    }

    protected void _btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/Evento/Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void _btnSalvar_Click(object sender, EventArgs e)
    {
        _lblMessage.Text = string.Empty;
        try
        {
            if ((cvDataInicio.IsValid) && (cvDataFim.IsValid))
            {
                List<int> calendarios;

                if (!verificarCalendarioSelecionado(out calendarios))
                {
                    throw new MSTech.Validation.Exceptions.ValidationException("Selecione pelo menos um calendário para o evento.");
                }
                //Verifica se o evento foi marcado como 'sem atividade discente'
                if (Convert.ToBoolean(rblAtividadeDiscente.SelectedValue))
                {
                    // carrega datatable com os calendários selecionados
                    CriarListaCalendarioEvento();

                    string lista_cal = string.Empty;
                    for (int i = 0; i < _VS_Calendario.Rows.Count; i++)
                        lista_cal = _VS_Calendario.Rows[i]["cal_id"].ToString() + ",";
                    if (lista_cal.Length > 0)
                        lista_cal = lista_cal.Remove(lista_cal.Length - 1);

                    //Verifica se existem aulas cadastradas no período informado para o evento
                    if (ACA_EventoBO.VerificaAulaPorCalendarioEscolaData(lista_cal, _UCFiltroEscolas._UCComboUnidadeEscola_Esc_ID, Convert.ToDateTime(_txtInicioEvento.Text), Convert.ToDateTime(_txtFimEvento.Text)))
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "ValidaAulaEventoSemAtividade", "$(document).ready(function(){ $('#divAulaEventoSemAtividade').dialog('open'); });", true);
                    else
                    {
                        // Bug #9852
                        _Salvar(_txtInicioEvento.Enabled);
                    }
                }
                else
                {
                    // Bug #9852
                    _Salvar(_txtInicioEvento.Enabled);
                }
            }
        }
        catch (MSTech.Validation.Exceptions.ValidationException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o evento.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void btnConfirmarEvento_Click(object sender, EventArgs e)
    {
        _Salvar(false);
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "ValidaDataInicial", "$(document).ready(function(){ $('#divDataAnterior').dialog('close'); });", true);
    }

    protected void cvAlcance_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = (chkPadrao.Checked || _UCFiltroEscolas._UCComboUnidadeEscola_Esc_ID > 0);
    }

    protected void btnConfirmarEventoSemAtividade_Click(object sender, EventArgs e)
    {
        _Salvar(_txtInicioEvento.Enabled);
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "ValidaAulaEventoSemAtividade", "$(document).ready(function(){ $('#divAulaEventoSemAtividade').dialog('close'); });", true);
    }
    
    #endregion Eventos
}

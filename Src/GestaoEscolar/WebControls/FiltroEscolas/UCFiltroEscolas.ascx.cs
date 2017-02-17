using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.Entities;
using MSTech.CoreSSO.BLL;

public partial class WebControls_FiltroEscolas_UCFiltroEscolas : MotherUserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //FIX - BUG(7622) 
        //Retirado if porque não setava o delegate
        if (_Selecionar != null)
            UCComboUnidadeAdministrativa1._IndexChanged += UCComboUnidadeAdministrativa1__IndexChanged;

        if (_SelecionarEscola != null)
            UCComboUnidadeEscola1._OnSelectedIndexChange += UCComboUnidadeEscola1__OnSelectedIndexChange;
    }

    #region Delegate

    public delegate void onSelecionar();
    public event onSelecionar _Selecionar;

    public void _SelecionarUnidadeAdministrativa()
    {
        if (_Selecionar != null)
            _Selecionar();
    }

    public delegate void onSelecionaEscola();
    public event onSelecionaEscola _SelecionarEscola;

    public void _SelecionarUnidadeEscola()
    {
        if (_SelecionarEscola != null)
            _SelecionarEscola();
    }

    #endregion

    #region Propriedades

    /// <summary>
    /// Propriedade que retorna se os está configurado para filtrar por UA.
    /// </summary>
    public bool _VS_FiltroEscola
    {
        get
        {
            if (ViewState["_VS_FiltroEscola"] != null)
            {
                if (ViewState["_VS_FiltroEscola"].Equals(true))
                    return true;

                return false;
            }

            return false;
        }
        set
        {
            ViewState["_VS_FiltroEscola"] = value;
        }
    }
    /// <summary>
    /// Propriedade que seta o Width do combo de Escolas.   
    /// </summary>
    public Int32 WidthComboEscolas
    {
        set
        {
            UCComboUnidadeEscola1.WidthCombo = value;
        }
    }
    /// <summary>
    /// Propriedade que seta o Width do combo de UA.   
    /// </summary>
    public Int32 WidthComboUA
    {
        set
        {
            UCComboUnidadeAdministrativa1.WidthCombo = value;
        }
    }
    /// <summary>
    /// ViewState com o valor de tua_id
    /// Apresenta valor apenas no caso de _VS_FiltroEscola = true
    /// </summary>
    public Guid _VS_tua_id
    {
        get
        {
            if (ViewState["_VS_tua_id"] != null)
            {
                return new Guid(ViewState["_VS_tua_id"].ToString());
            }
            return Guid.Empty;
        }
        set
        {
            ViewState["_VS_tua_id"] = value;
        }
    }

    public Label _LabelUnidadeAdministrativa
    {
        get
        {
            return UCComboUnidadeAdministrativa1._Label;
        }
    }

    public string _MensagemCVUnidadeAdministrativa
    {
        get
        {
            return cvUnidadeAdministrativa.ErrorMessage;
        }
        set
        {
            cvUnidadeAdministrativa.ErrorMessage = value;
        }
    }

    public string _MensagemCVEscola
    {
        get
        {
            return cvEscola.ErrorMessage;
        }
        set
        {
            cvEscola.ErrorMessage = value;
        }
    }

    public DropDownList _ComboUnidadeAdministrativa
    {
        get
        {
            return UCComboUnidadeAdministrativa1._Combo;
        }
    }

    public CompareValidator _cvUnidadeAdministrativa
    {
        get
        {
            return cvUnidadeAdministrativa;
        }
    }

    public bool CancelarConsultaEscola
    {
        set
        {
            UCComboUnidadeEscola1.CancelarConsulta = value;
        }
    }

    public Label _LabelUnidadeEscola
    {
        get
        {
            return UCComboUnidadeEscola1._Label;
        }
    }

    public DropDownList _ComboUnidadeEscola
    {
        get
        {
            return UCComboUnidadeEscola1._Combo;
        }
    }

    public CompareValidator _cvUnidadeEscola
    {
        get
        {
            return cvEscola;
        }
    }

    /// <summary>
    /// ID da unidade administrativa selecionada no combo.
    /// </summary>
    public Guid _UCComboUnidadeAdministrativa_Uad_ID
    {
        get
        {
            return (UCComboUnidadeAdministrativa1._Combo.SelectedIndex > 0) ?
                new Guid(UCComboUnidadeAdministrativa1._Combo.SelectedValue) : new Guid();
        }
    }

    /// <summary>
    /// ID da escola selecionada no combo.
    /// </summary>
    public Int32 _UCComboUnidadeEscola_Esc_ID
    {
        get
        {
            return UCComboUnidadeEscola1.Esc_ID;
        }
    }

    /// <summary>
    /// ID da Unidade de escola selecionada no combo.
    /// </summary>
    public Int32 _UCComboUnidadeEscola_Uni_ID
    {
        get
        {
            return UCComboUnidadeEscola1.Uni_ID;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool SelecionaCombosAutomatico
    {
        set
        {
            UCComboUnidadeEscola1.SelecionaAutomatico = value;
            UCComboUnidadeAdministrativa1.SelecionaAutomatico = value;
        }
    }

    /// <summary>
    /// Propriedade que retorna se o campo UA é obrigatório na respectiva tela.
    /// </summary>
    public bool UnidadeAdministrativaCampoObrigatorio
    {
        get
        {
            if (ViewState["CampoObrigatorio"] != null)
            {
                if (ViewState["CampoObrigatorio"].Equals(true))
                    return true;

                return false;
            }

            return false;
        }
        set
        {
            ViewState["CampoObrigatorio"] = value;
        }
    }

    /// <summary>
    /// Propriedade que retorna se o campo Escola é obrigatório na respectiva tela.
    /// </summary>
    public bool EscolaCampoObrigatorio
    {
        get
        {
            if (ViewState["EscolaCampoObrigatorio"] != null)
            {
                if (ViewState["EscolaCampoObrigatorio"].Equals(true))
                    return true;

                return false;
            }

            return false;
        }
        set
        {
            ViewState["EscolaCampoObrigatorio"] = value;
            CamposObrigatorios();
        }
    }

    /// <summary>
    /// Propriedade para controlar o viewstate dos controles:
    /// UCComboUnidadeEscola1
    /// </summary>
    public bool GuardaViewStateEscola
    {
        get
        {
            return UCComboUnidadeEscola1.GuardaViewState;
        }
        set
        {
            UCComboUnidadeEscola1.GuardaViewState = value;
        }
    }

    /// <summary>
    /// Propriedade para controlar o viewstate dos controles:
    /// UCComboUnidadeAdministrativa1
    /// </summary>
    public bool GuardaViewStateUnidadeAdministrativa
    {
        get
        {
            return UCComboUnidadeAdministrativa1.GuardaViewState;
        }
        set
        {
            UCComboUnidadeAdministrativa1.GuardaViewState = value;
        }
    }

    /// <summary>
    /// Propriedade para controlar o viewstate dos controles:
    /// UCComboUnidadeAdministrativa1, UCComboUnidadeEscola1
    /// </summary>
    public bool GuardaViewState
    {
        get
        {
            return UCComboUnidadeAdministrativa1.GuardaViewState && UCComboUnidadeEscola1.GuardaViewState;
        }
        set
        {
            UCComboUnidadeAdministrativa1.GuardaViewState = UCComboUnidadeEscola1.GuardaViewState = value;
        }
    }

    #endregion

    #region Métodos Load dos combos

    public void _UnidadeAdministrativa_LoadBy_tua_id_cfg_id(Guid tua_id, int cfg_id)
    {
        UCComboUnidadeAdministrativa1._LoadBy_tua_id_cfg_id(tua_id, cfg_id);
    }

    public void _UnidadeAdministrativa_LoadBy_uad_id(Guid uad_id)
    {
        UCComboUnidadeAdministrativa1.LoadByFiltroUASuperiorEscola(uad_id);
    }

    public void _UnidadeAdministrativa_LoadBy_tua_id_situacao(Guid tua_id, Guid uad_id, byte uad_situacao)
    {
        UCComboUnidadeAdministrativa1._Load(tua_id, uad_id);
    }

    public void _UnidadeAdministrativa_LoadCombo(object dataSource)
    {
        UCComboUnidadeAdministrativa1.CarregarCombo(dataSource);
    }

    public void _UnidadeEscola_Load(byte uni_situacao)
    {
        UCComboUnidadeEscola1._Load(uni_situacao);
    }

    public void _UnidadeEscola_LoadEscolasControladas(bool esc_controleSistema)
    {
        UCComboUnidadeEscola1._LoadEscolasControladas(esc_controleSistema);
    }

    public void _UnidadeEscola_LoadBy_uad_idSuperior(Guid uad_idSuperior, bool buscarTerceirizadas = true)
    {
        UCComboUnidadeEscola1._LoadBy_uad_idSuperior(uad_idSuperior, buscarTerceirizadas);
    }

    public void _UnidadeEscola_LoadBy_uad_idSuperior(Guid uad_idSuperior, bool esc_controleSistema, bool buscarTerceirizadas = true)
    {
        UCComboUnidadeEscola1._LoadBy_uad_idSuperior(uad_idSuperior, esc_controleSistema, buscarTerceirizadas);
    }

    public void _UnidadeEscola_LoadBy_uad_idSuperiorSituacao(Guid uad_idSuperior, byte uni_situacao, bool buscarTerceirizadas = true)
    {
        UCComboUnidadeEscola1._LoadBy_uad_idSuperiorSituacao(uad_idSuperior, uni_situacao, buscarTerceirizadas);
    }

    public void _UnidadeEscola_LoadByAll_uad_idSuperior(Guid uad_idSuperior)
    {
        UCComboUnidadeEscola1._LoadByAll_uad_idSuperior(uad_idSuperior);
    }

    public void _UnidadeEscola_LoadBy_uad_idSuperior_cfg_id(Guid uad_idSuperior, int cfg_id)
    {
        UCComboUnidadeEscola1._LoadBy_uad_idSuperior_cfg_id(uad_idSuperior, cfg_id);
    }

    public void _UnidadeEscola_LoadBy_cur_id_crr_id_crp_id(int cur_id, int crr_id, int crp_id)
    {
        UCComboUnidadeEscola1._LoadBy_cur_id_crr_id_crp_id(cur_id, crr_id, crp_id);
    }

    public void _UnidadeEscola_LoadBy_uad_idSuperior_cur_id_crr_id_crp_id(Guid uad_idSuperior, int cur_id, int crr_id, int crp_id)
    {
        UCComboUnidadeEscola1._LoadBy_uad_idSuperior_cur_id_crr_id_crp_id(uad_idSuperior, cur_id, crr_id, crp_id);
    }


    public void _UnidadeEscola_LoadBy_PoloPlanejamento_uad_idSuperior(int ppl_id, Guid uad_idSuperior)
    {
        UCComboUnidadeEscola1.CarregarPorPoloPlanejamentoUnidadeAdministrativaSuperior(ppl_id, uad_idSuperior);
    }

    public void _UnidadeEscola_LoadCombo(object dataSource)
    {
        UCComboUnidadeEscola1.CarregarCombo(dataSource);
    }

    #endregion    

    #region Métodos

    private void UCComboUnidadeAdministrativa1__IndexChanged()
    {
        _SelecionarUnidadeAdministrativa();
    }

    private void UCComboUnidadeEscola1__OnSelectedIndexChange()
    {
        _SelecionarUnidadeEscola();
    }

    private void CamposObrigatorios()
    {
        cvUnidadeAdministrativa.Enabled = UnidadeAdministrativaCampoObrigatorio;
        cvEscola.Enabled = EscolaCampoObrigatorio;
        cvEscola.Visible = EscolaCampoObrigatorio;

        RemoveAsteriscoObrigatorio(UCComboUnidadeAdministrativa1._Label);
        cvUnidadeAdministrativa.ErrorMessage = UCComboUnidadeAdministrativa1._Label.Text + " é obrigatório.";

        if (UnidadeAdministrativaCampoObrigatorio)
        {
            AdicionaAsteriscoObrigatorio(UCComboUnidadeAdministrativa1._Label);
        }
        else
        {
            RemoveAsteriscoObrigatorio(UCComboUnidadeAdministrativa1._Label);
        }

        if (EscolaCampoObrigatorio)
        {
            AdicionaAsteriscoObrigatorio(UCComboUnidadeEscola1._Label);
        }
        else
        {
            RemoveAsteriscoObrigatorio(UCComboUnidadeEscola1._Label);
        }
    }

    /// <summary>
    /// Verifica os parâmetros acadêmicos cadastrados, mostrando e carregando os combos 
    /// conforme a configuração.
    /// Se parâmetro FILTRAR_ESCOLA_UA_SUPERIOR = "Sim", mostra combo de Unidade Administrativa,
    /// e carrega no combo pelo tipo de UA que estiver setada no parâmetro 
    /// TIPO_UNIDADE_ADMINISTRATIVA_FILTRO_ESCOLA.
    /// </summary>
    public void _LoadInicial(bool buscarTerceirizadas = true, bool esc_controleSistema = false)
    {
        _LoadInicialSituacao(0, buscarTerceirizadas, esc_controleSistema);
    }

    /// <summary>
    /// Verifica os parâmetros acadêmicos cadastrados, mostrando e carregando os combos 
    /// conforme a configuração.
    /// Se parâmetro FILTRAR_ESCOLA_UA_SUPERIOR = "Sim", mostra combo de Unidade Administrativa,
    /// e carrega no combo pelo tipo de UA que estiver setada no parâmetro 
    /// TIPO_UNIDADE_ADMINISTRATIVA_FILTRO_ESCOLA.
    /// Se informar a situação, traz apenas as escola da situação informada
    /// </summary>
    /// <param name="uni_situacao"></param>
    public void _LoadInicialSituacao(byte uni_situacao, bool buscarTerceirizadas = true, bool esc_controleSistema = false)
    {
        try
        {
            UCComboUnidadeAdministrativa1._Combo.Visible = false;
            UCComboUnidadeAdministrativa1._Label.Visible = false;

            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa || !ACA_ParametroAcademicoBO.VerificaFiltroUniAdmSuperiorPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                UCComboUnidadeAdministrativa1.CancelaConsulta = true;
                UCComboUnidadeAdministrativa1.SelecionaAutomatico = false;

                UCComboUnidadeEscola1._Load(uni_situacao, buscarTerceirizadas, esc_controleSistema);
                UCComboUnidadeEscola1._Combo.Enabled = true;

                _VS_FiltroEscola = false;
            }
            else
            {
                Guid tua_id = ACA_ParametroAcademicoBO.VerificaFiltroEscolaPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                SYS_TipoUnidadeAdministrativa TipoUnidadeAdm = new SYS_TipoUnidadeAdministrativa { tua_id = tua_id };
                SYS_TipoUnidadeAdministrativaBO.GetEntity(TipoUnidadeAdm);
                _VS_tua_id = tua_id;

                UCComboUnidadeAdministrativa1._Label.Text = string.IsNullOrEmpty(TipoUnidadeAdm.tua_nome) ? "Unidade Administrativa" : TipoUnidadeAdm.tua_nome;
                UCComboUnidadeAdministrativa1._Load(tua_id, Guid.Empty);
                UCComboUnidadeAdministrativa1._Combo.Visible = true;
                UCComboUnidadeAdministrativa1._Label.Visible = true;

                UCComboUnidadeEscola1._Combo.Items.Clear();
                UCComboUnidadeEscola1.CancelarConsulta = true;
                UCComboUnidadeEscola1._MostrarMessageSelecione = true;
                UCComboUnidadeEscola1._Combo.Enabled = false;
                UCComboUnidadeEscola1._Label.Text = "Escola ";

                _VS_FiltroEscola = true;

                CamposObrigatorios();
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
        }
    }

    /// <summary>
    /// Verifica os parâmetros acadêmicos cadastrados, mostrando e carregando os combos 
    /// conforme a configuração.
    /// Se parâmetro FILTRAR_ESCOLA_UA_SUPERIOR = "Sim", mostra combo de Unidade Administrativa,
    /// e carrega no combo pelo tipo de UA que estiver setada no parâmetro 
    /// TIPO_UNIDADE_ADMINISTRATIVA_FILTRO_ESCOLA.
    /// </summary>
    public void _LoadInicial_SelecionaTodos()
    {
        try
        {
            UCComboUnidadeAdministrativa1._Combo.Visible = false;
            UCComboUnidadeAdministrativa1._Label.Visible = false;

            if (!ACA_ParametroAcademicoBO.VerificaFiltroUniAdmSuperiorPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                UCComboUnidadeAdministrativa1.CancelaConsulta = true;
                UCComboUnidadeAdministrativa1.SelecionaAutomatico = false;

                UCComboUnidadeEscola1._Load(0);
                UCComboUnidadeEscola1._Combo.Enabled = true;

                _VS_FiltroEscola = false;
            }
            else
            {
                Guid tua_id = ACA_ParametroAcademicoBO.VerificaFiltroEscolaPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                SYS_TipoUnidadeAdministrativa TipoUnidadeAdm = new SYS_TipoUnidadeAdministrativa { tua_id = tua_id };
                SYS_TipoUnidadeAdministrativaBO.GetEntity(TipoUnidadeAdm);
                _VS_tua_id = tua_id;

                UCComboUnidadeAdministrativa1._Label.Text = string.IsNullOrEmpty(TipoUnidadeAdm.tua_nome) ?
                    "Unidade Administrativa" : TipoUnidadeAdm.tua_nome;
                UCComboUnidadeAdministrativa1._Load_All(tua_id);
                UCComboUnidadeAdministrativa1._Combo.Visible = true;
                UCComboUnidadeAdministrativa1._Label.Visible = true;

                UCComboUnidadeEscola1._Load(0);
                UCComboUnidadeEscola1._Combo.Items.Clear();
                UCComboUnidadeEscola1.CancelarConsulta = true;
                UCComboUnidadeEscola1._MostrarMessageSelecione = true;
                UCComboUnidadeEscola1._Combo.Enabled = false;
                UCComboUnidadeEscola1._Label.Text = "Escola ";

                _VS_FiltroEscola = true;

                CamposObrigatorios();
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
        }
    }

    /// <summary>
    /// Verifica os parâmetros acadêmicos cadastrados, mostrando e carregando os combos 
    /// conforme a configuração. Não mostra o combo de escola, só o de UA.
    /// Se parâmetro FILTRAR_ESCOLA_UA_SUPERIOR = "Sim", mostra combo de Unidade Administrativa,
    /// e carrega no combo pelo tipo de UA que estiver setada no parâmetro 
    /// TIPO_UNIDADE_ADMINISTRATIVA_FILTRO_ESCOLA.
    /// </summary>
    public void _LoadInicialFiltroUA()
    {
        try
        {
            UCComboUnidadeAdministrativa1._Combo.Visible = false;
            UCComboUnidadeAdministrativa1._Label.Visible = false;

            UCComboUnidadeEscola1.Visible = false;
            UCComboUnidadeEscola1.CancelarConsulta = true;
            UCComboUnidadeEscola1.SelecionaAutomatico = false;

            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa ||
                !ACA_ParametroAcademicoBO.VerificaFiltroUniAdmSuperiorPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                _VS_FiltroEscola = false;

                // Não mostra o combo de UA superior - não deixar consultar.
                UCComboUnidadeAdministrativa1.CancelaConsulta = true;
                UCComboUnidadeAdministrativa1.SelecionaAutomatico = false;
            }
            else
            {
                Guid tua_id = ACA_ParametroAcademicoBO.VerificaFiltroEscolaPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                SYS_TipoUnidadeAdministrativa TipoUnidadeAdm = new SYS_TipoUnidadeAdministrativa { tua_id = tua_id };
                SYS_TipoUnidadeAdministrativaBO.GetEntity(TipoUnidadeAdm);
                _VS_tua_id = tua_id;

                UCComboUnidadeAdministrativa1._Label.Text = string.IsNullOrEmpty(TipoUnidadeAdm.tua_nome) ? "Unidade Administrativa" : TipoUnidadeAdm.tua_nome;
                UCComboUnidadeAdministrativa1._Load(tua_id, Guid.Empty);

                UCComboUnidadeAdministrativa1._Combo.Visible = true;
                UCComboUnidadeAdministrativa1._Label.Visible = true;

                _VS_FiltroEscola = true;
            }
            CamposObrigatorios();

        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
        }
    }

    #endregion
}
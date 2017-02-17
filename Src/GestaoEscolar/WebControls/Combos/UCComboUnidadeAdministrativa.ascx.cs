using System;
using System.Data;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboUnidadeAdministrativa : MotherUserControl
{
    #region Delegates

    public delegate void onIndexChanged();

    public event onIndexChanged _IndexChanged;

    #endregion Delegates

    #region Propriedades

    /// <summary>
    /// Atribui valores para o combo
    /// </summary>
    public DropDownList _Combo
    {
        get
        {
            return _ddlUA;
        }
    }

    /// <summary>
    /// Propriedade que seta o Width do combo.
    /// </summary>
    public Int32 WidthCombo
    {
        set
        {
            _ddlUA.Width = value;
        }
    }

    /// <summary>
    /// Atribui valores para o label
    /// </summary>
    public Label _Label
    {
        get
        {
            return LabelUA;
        }
    }

    /// <summary>
    /// Retorna a descrição do label removendo o *
    /// </summary>
    public string descricaoLabel
    {
        get
        {
            string descricaoLabel = LabelUA.Text.Replace("*", "").Trim();
            return descricaoLabel;
        }
    }

    /// <summary>
    /// Adciona e remove a mensagem "Selecione uma Unidade Adm." do dropdownlist.
    /// Por padrão é false e a mensagem "Selecione uma Unidade Adm." não é exibida.
    /// </summary>
    public bool _MostrarMessageSelecione
    {
        set
        {
            if (value)
            {
                string descricaoUa = LabelUA.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, "");
                _ddlUA.Items.Insert(0, new ListItem("-- Selecione um(a) " + descricaoUa + " --", Guid.Empty.ToString(), true));
            }
            _ddlUA.AppendDataBoundItems = value;
        }
    }

    public bool CancelaConsulta;

    public bool SelecionaAutomatico = true;

    /// <summary>
    /// Propriedade para controlar o viewstate dos controles:
    /// odsUA
    /// </summary>
    public bool GuardaViewState
    {
        get
        {
            return odsUA.EnableViewState;
        }
        set
        {
            odsUA.EnableViewState = value;
        }
    }

    #endregion Propriedades

    #region Métodos

    #region OK

    /// <summary>
    /// Carrega as Unidades Administrativas pelo tipo de UA.
    /// </summary>
    /// <param name="tua_id"></param>
    /// <param name="uad_id">ID da Unidade Administrativa que não será exibida no combo</param>
    public void _Load(Guid tua_id, Guid uad_id)
    {
        _ddlUA.Items.Clear();
        _ddlUA.DataSourceID = odsUA.ID;

        odsUA.SelectMethod = "GetSelectBy_Pesquisa_PermissaoUsuario";
        odsUA.SelectParameters.Clear();
        odsUA.SelectParameters.Add("tua_id", tua_id.ToString());
        odsUA.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsUA.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
        odsUA.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
        odsUA.SelectParameters.Add("uad_situacao", "0");
        odsUA.SelectParameters.Add("uad_id", uad_id.ToString());
        odsUA.DataBind();

        _ddlUA.DataBind();

        _MostrarMessageSelecione = true;
    }

    /// <summary>
    /// Carrega as Unidades Administrativas.
    /// Pela permissão do usuário na unidade administrativa e na unidade administrativa superior
    /// </summary>
    /// <param name="tua_id">tipo de unidade administrativa</param>
    /// <param name="ent_id">Entidade do usuário logado</param>
    /// <param name="gru_id">Id do grupo do usuário.</param>
    /// <param name="usu_id">Id do usuário.</param>
    public void _Load_UnidadeSuperior(Guid tua_id, Guid uad_id)
    {
        _ddlUA.Items.Clear();
        _ddlUA.DataSourceID = odsUA.ID;

        odsUA.SelectMethod = "GetSelectBy_Pesquisa_PermissaoUsuarioUASuperior";
        odsUA.SelectParameters.Clear();
        odsUA.SelectParameters.Add("tua_id", tua_id.ToString());
        odsUA.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsUA.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
        odsUA.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
        odsUA.DataBind();

        _ddlUA.DataBind();

        _MostrarMessageSelecione = true;
    }

    /// <summary>
    /// Carrega as Unidades Administrativas pelo tipo de UA.
    /// </summary>
    /// <param name="tua_id"></param>
    public void _Load_All(Guid tua_id)
    {
        _ddlUA.Items.Clear();
        _ddlUA.DataSourceID = odsUA.ID;

        odsUA.SelectMethod = "GetSelectBy_PesquisaTodos";
        odsUA.SelectParameters.Clear();
        odsUA.SelectParameters.Add("tua_id", tua_id.ToString());
        odsUA.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsUA.DataBind();

        _ddlUA.DataBind();

        _MostrarMessageSelecione = true;
    }

    /// <summary>
    /// Verifica nos parâmetros acadêmicos se o parâmetro FILTRAR_ESCOLA_UA_SUPERIOR está
    /// setado como "Sim", busca as UAs do tipo que estiver setado no parâmetro
    /// TIPO_UNIDADE_ADMINISTRATIVA_FILTRO_ESCOLA, se não, esconde o combo.
    /// </summary>
    /// <returns>Flag que informa se está setado para filtrar por UA Superior nos parâmetros acadêmicos.</returns>
    /// <param name="uad_id">ID da unidade administrativa que não será exibida no combo.</param>
    public bool LoadByFiltroUASuperiorEscola(Guid uad_id)
    {
        Guid tua_id = ACA_ParametroAcademicoBO.VerificaFiltroEscolaPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
        SYS_TipoUnidadeAdministrativa TipoUnidadeAdm = new SYS_TipoUnidadeAdministrativa { tua_id = tua_id };
        SYS_TipoUnidadeAdministrativaBO.GetEntity(TipoUnidadeAdm);

        // Seta no texto do combo o tipo de UA.
        LabelUA.Text = TipoUnidadeAdm.tua_nome ?? "Unidade administrativa";

        // Carrega UA pelo tipo.
        _Load_UnidadeSuperior(tua_id, uad_id);

        return ACA_ParametroAcademicoBO.VerificaFiltroUniAdmSuperiorPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
    }

    /// <summary>
    /// Verifica nos parâmetros acadêmicos se o parâmetro FILTRAR_ESCOLA_UA_SUPERIOR está
    /// setado como "Sim", busca as UAs do tipo que estiver setado no parâmetro
    /// TIPO_UNIDADE_ADMINISTRATIVA_FILTRO_ESCOLA, se não, esconde o combo.
    /// </summary>
    /// <returns>Flag que informa se está setado para filtrar por UA Superior nos parâmetros acadêmicos.</returns>
    /// <param name="uad_id">ID da unidade administrativa que não será exibida no combo.</param>
    public bool FiltraUnidadeSuperiorPorEscola(Guid uad_id)
    {
        Guid tua_id = ACA_ParametroAcademicoBO.VerificaFiltroEscolaPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
        SYS_TipoUnidadeAdministrativa TipoUnidadeAdm = new SYS_TipoUnidadeAdministrativa { tua_id = tua_id };
        SYS_TipoUnidadeAdministrativaBO.GetEntity(TipoUnidadeAdm);

        // Seta no texto do combo o tipo de UA.
        LabelUA.Text = TipoUnidadeAdm.tua_nome ?? "Unidade administrativa";

        // Carrega UA pelo tipo.
        _Load_UnidadeSuperior(tua_id, uad_id);

        return ACA_ParametroAcademicoBO.VerificaFiltroUniAdmSuperiorPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
    }

    /// <summary>
    /// Carrega o combo
    /// </summary>
    /// <param name="dataSource">Dados a serem inseridos no combo</param>
    public void CarregarCombo(object datasource)
    {
        _ddlUA.Items.Clear();
        _ddlUA.DataSourceID = string.Empty;
        _ddlUA.DataSource = datasource;
        _MostrarMessageSelecione = true;
        _ddlUA.DataBind();
    }

    #endregion OK

    #region Arrumar

    public void _LoadBy_tua_id_cfg_id(Guid tua_id, int cfg_id)
    {
        _ddlUA.DataSourceID = odsUA.ID;
        _ddlUA.Items.Clear();
        odsUA.SelectParameters.Clear();
        odsUA.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.ESC_UnidadeEscola";
        odsUA.TypeName = "MSTech.GestaoEscolar.BLL.ESC_UnidadeEscolaBO";
        odsUA.SelectMethod = "GetSelectByTipoUAConfiguracao";
        odsUA.EnablePaging = false;
        odsUA.SelectCountMethod = string.Empty;
        odsUA.MaximumRowsParameterName = string.Empty;
        odsUA.SelectParameters.Add("tua_id", tua_id.ToString());
        odsUA.SelectParameters.Add("cfg_id", cfg_id.ToString());
        odsUA.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

        odsUA.DataBind();

        _MostrarMessageSelecione = true;
        _ddlUA.DataBind();
    }

    #endregion Arrumar

    #endregion Métodos

    #region Eventos

    protected void Page_PreRender(object sender, EventArgs e)
    {
        _ddlUA.AutoPostBack = _IndexChanged != null;

        // Se o combo foi preenchido automaticamente.
        if ((!Page.IsPostBack) &&
            (!String.IsNullOrEmpty(_ddlUA.SelectedValue)) &&
            (_ddlUA.SelectedValue != Guid.Empty.ToString()) &&
            (_IndexChanged != null) &&
            (SelecionaAutomatico))
            _IndexChanged();
    }

    protected void _ddlUA_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_IndexChanged != null)
            _IndexChanged();
    }

    protected void odsUA_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        odsUA.Selected += odsUA_Selected;
        e.Cancel = CancelaConsulta && _ddlUA.Items.Count > 0;

        //Seta a variável CancelaConsulta para true, dessa forma, ao passar pelo método novamente os dados do combo não são duplicados
        CancelaConsulta = true;
    }

    protected void odsUA_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception == null)
        {
            try
            {
                // Verifica se só tem 1 registro no retorno, se sim, seleciona ele no combo.
                if (e.ReturnValue is DataTable)
                {
                    DataTable dt = (DataTable)e.ReturnValue;

                    if ((!Page.IsPostBack) && (dt.Rows.Count == 1) && (SelecionaAutomatico))
                    {
                        _ddlUA.SelectedValue = dt.Rows[0][_ddlUA.DataValueField].ToString();
                    }
                }
            }
            catch
            {
                if (_ddlUA.Items.FindByValue(Guid.Empty.ToString()) != null)
                    _ddlUA.SelectedValue = Guid.Empty.ToString();
            }
        }
        else
        {
            // Grava o erro e mostra pro usuário.
            ApplicationWEB._GravaErro(e.Exception.InnerException);

            // lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);

            e.ExceptionHandled = true;

            lblMessage.Text = "Erro ao tentar carregar " + _Label.Text.Replace('*', ' ').ToLower() + ".";
            lblMessage.Visible = true;
        }
    }

    #endregion Eventos
}
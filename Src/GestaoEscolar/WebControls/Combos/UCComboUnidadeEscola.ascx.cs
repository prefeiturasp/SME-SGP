using System;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System.Data;

public partial class WebControls_Combos_UCComboUnidadeEscola : MotherUserControl
{
    #region DELEGATES

    public delegate void SelectedIndexChange();
    public event SelectedIndexChange _OnSelectedIndexChange;
    
    #endregion

    #region PROPRIEDADES

    /// <summary>
    /// Propriedade que seta o SelectedValue do Combo.
    /// Valor[0] = Esc_ID
    /// Valor[1] = Uni_id
    /// </summary>
    public Int32[] ValorCombo
    {
        set
        {
            _ddlUnidadeEscola.SelectedValue = value[0] + ";" + value[1];
        }
    }
    /// <summary>
    /// Propriedade que seta o Width do combo.   
    /// </summary>
    public Int32 WidthCombo
    {
        set
        {
            _ddlUnidadeEscola.Width = value;
        }
    }

    /// <summary>
    /// Retorna o Esc_ID selecionado no combo.
    /// </summary>
    public Int32 Esc_ID
    {
        get
        {
            string[] s = _ddlUnidadeEscola.SelectedValue.Split(';');

            if (s.Length == 2)
            {
                return Convert.ToInt32(s[0]);
            }
            else
            {
                return -1;
            }
        }
    }

    /// <summary>
    /// Retorna Uni_ID selecionado no combo.
    /// </summary>
    public Int32 Uni_ID
    {
        get
        {
            string[] s = _ddlUnidadeEscola.SelectedValue.Split(';');

            if (s.Length == 2)
            {
                return Convert.ToInt32(s[1]);
            }
            else
            {
                return -1;
            }
        }
    }

    public bool CancelarConsulta;

    public bool SelecionaAutomatico = true;

    /// <summary>
    /// Atribui valores para o combo
    /// </summary>
    public DropDownList _Combo
    {
        get
        {
            return _ddlUnidadeEscola;
        }
        set
        {
            _ddlUnidadeEscola = value;
        }
    }

    public Label _Label
    {
        get
        {
            return LabelEscola;
        }
        set
        {
            LabelEscola = value;
        }
    }

    /// <summary>
    /// Adciona e remove a mensagem "Selecione uma Escola / Unidade" do dropdownlist.  
    /// Por padrão é false e a mensagem "Selecione uma Escola / Unidade" não é exibida.
    /// </summary>
    public bool _MostrarMessageSelecione
    {
        set
        {
            if ((value) && (_ddlUnidadeEscola.Items.FindByValue("-1;-1") == null))
                _ddlUnidadeEscola.Items.Insert(0, new ListItem("-- Selecione uma escola --", "-1;-1", true));
            _ddlUnidadeEscola.AppendDataBoundItems = value;
        }
    }

    /// <summary>
    /// Propriedade para controlar o viewstate dos controles:
    /// odsUnidadeEscola
    /// </summary>
    public bool GuardaViewState
    {
        get
        {
            return odsUnidadeEscola.EnableViewState;
        }
        set
        {
            odsUnidadeEscola.EnableViewState = value;
        }
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Mostra as unidades de escola não excluídas logicamente no dropdownlist
    /// que pertence a entidade logada
    /// situacao = INT, exibe as unidades de acordo com a situação 1-Ativo, 2-Bloqueado, 4-Desativado, 5-Em ativação no dropdownlist.
    /// </summary>
    public void _Load(byte uni_situacao, bool buscarTerceirizadas = true, bool esc_controleSistema = false)
    {
        _ddlUnidadeEscola.Items.Clear();
        odsUnidadeEscola.SelectParameters.Clear();
        _ddlUnidadeEscola.DataSourceID = odsUnidadeEscola.ID;
        odsUnidadeEscola.SelectParameters.Add("esc_id", "0");
        odsUnidadeEscola.SelectParameters.Add("uni_id", "0");
        odsUnidadeEscola.SelectParameters.Add("uni_situacao", uni_situacao.ToString());
        odsUnidadeEscola.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("buscarTerceirizadas", buscarTerceirizadas.ToString());
        odsUnidadeEscola.SelectParameters.Add("esc_controleSistema", esc_controleSistema.ToString());
        _MostrarMessageSelecione = true;
        _ddlUnidadeEscola.DataBind();
        
    }

     /// <summary>
    /// Mostra as unidades de escola controladas não excluídas logicamente no dropdownlist
    /// que pertence a entidade logada  
    /// </summary>
    public void _LoadEscolasControladas(bool esc_controleSistema)
    {
        _ddlUnidadeEscola.Items.Clear();
        odsUnidadeEscola.SelectParameters.Clear();
        _ddlUnidadeEscola.DataSourceID = odsUnidadeEscola.ID;
        _ddlUnidadeEscola.DataTextField = "esc_uni_nome";
        odsUnidadeEscola.SelectMethod = "SelecionaEscolasControladas";
        odsUnidadeEscola.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("esc_controleSistema", esc_controleSistema.ToString());
        odsUnidadeEscola.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());
        _MostrarMessageSelecione = true;
        _ddlUnidadeEscola.DataBind();
    }

    /// <summary>
    /// Carrega as escolas pelo uad_idSuperior e pela permissão do usuário logado.
    /// </summary>
    /// <param name="uad_idSuperior"></param>
    public void _LoadBy_uad_idSuperior(Guid uad_idSuperior, bool buscarTerceirizadas = true)
    {
        CancelarConsulta = false;
        _ddlUnidadeEscola.Items.Clear();
        odsUnidadeEscola.SelectParameters.Clear();
        _ddlUnidadeEscola.DataSourceID = odsUnidadeEscola.ID;
        _ddlUnidadeEscola.DataTextField = "esc_uni_nome";
        odsUnidadeEscola.SelectMethod = "GetSelectByUASuperior";
        odsUnidadeEscola.SelectParameters.Add("uad_idSuperior", uad_idSuperior.ToString());
        odsUnidadeEscola.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());
        odsUnidadeEscola.SelectParameters.Add("buscarTerceirizadas", buscarTerceirizadas.ToString());

        _MostrarMessageSelecione = true;
        _ddlUnidadeEscola.DataBind();
    }

    /// <summary>
    /// Carrega as escolas controladas pelo sistema de acordo com o uad_idSuperior e pela permissão do usuário logado.
    /// </summary>
    /// <param name="uad_idSuperior"></param>
    /// <param name="esc_controleSistema">parametro que definira se as escolas controladas pelo sistema serão retornadas ou não</param>
    public void _LoadBy_uad_idSuperior(Guid uad_idSuperior, bool esc_controleSistema, bool buscarTerceirizadas = true)
    {
        CancelarConsulta = false;
        _ddlUnidadeEscola.Items.Clear();
        odsUnidadeEscola.SelectParameters.Clear();
        _ddlUnidadeEscola.DataSourceID = odsUnidadeEscola.ID;
        _ddlUnidadeEscola.DataTextField = "esc_uni_nome";
        odsUnidadeEscola.SelectMethod = "SelecionaEscolasControladasPorUASuperior";
        odsUnidadeEscola.SelectParameters.Add("uad_idSuperior", uad_idSuperior.ToString());
        odsUnidadeEscola.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("esc_controleSistema", esc_controleSistema.ToString());
        odsUnidadeEscola.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());
        odsUnidadeEscola.SelectParameters.Add("buscarTerceirizadas", buscarTerceirizadas.ToString());

        _MostrarMessageSelecione = true;
        _ddlUnidadeEscola.DataBind();
    }

    /// <summary>
    /// Carrega as escolas pelo uad_idSuperior, pela situacao e pela permissão do usuário logado.
    /// </summary>
    /// <param name="uad_idSuperior"></param>
    /// <param name="uni_situacao"></param>
    public void _LoadBy_uad_idSuperiorSituacao(Guid uad_idSuperior, byte uni_situacao, bool buscarTerceirizadas = true)
    {
        CancelarConsulta = false;
        _ddlUnidadeEscola.Items.Clear();
        odsUnidadeEscola.SelectParameters.Clear();
        _ddlUnidadeEscola.DataSourceID = odsUnidadeEscola.ID;
        _ddlUnidadeEscola.DataTextField = "esc_uni_nome";
        odsUnidadeEscola.SelectMethod = "GetSelectByUASuperiorSituacao";
        odsUnidadeEscola.SelectParameters.Add("uad_idSuperior", uad_idSuperior.ToString());
        odsUnidadeEscola.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("uni_situacao", uni_situacao.ToString());
        odsUnidadeEscola.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());
        odsUnidadeEscola.SelectParameters.Add("buscarTerceirizadas", buscarTerceirizadas.ToString());

        _MostrarMessageSelecione = true;
        _ddlUnidadeEscola.DataBind();
    }

    /// <summary>
    /// Carrega as escolas pelo uad_idSuperior e sem filtrar pela permissão do usuário.
    /// </summary>
    /// <param name="uad_idSuperior"></param>
    public void _LoadByAll_uad_idSuperior(Guid uad_idSuperior)
    {
        CancelarConsulta = false;
        _ddlUnidadeEscola.Items.Clear();
        odsUnidadeEscola.SelectParameters.Clear();
        _ddlUnidadeEscola.DataSourceID = odsUnidadeEscola.ID;
        _ddlUnidadeEscola.DataTextField = "esc_uni_nome";
        odsUnidadeEscola.SelectMethod = "GetSelectByUASuperior";
        odsUnidadeEscola.SelectParameters.Add("uad_idSuperior", uad_idSuperior.ToString());
        odsUnidadeEscola.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        _MostrarMessageSelecione = true;
        _ddlUnidadeEscola.DataBind();
    }

    /// <summary>
    /// Carrega as escolas por pólo de planejamento e por unidade administrativa superior.
    /// </summary>
    /// <param name="ppl_id">ID do pólo de planejamento.</param>
    /// <param name="uad_idSuperior">ID da unidade superior.</param>
    public void CarregarPorPoloPlanejamentoUnidadeAdministrativaSuperior(int ppl_id, Guid uad_idSuperior)
    {
        CancelarConsulta = false;
        _ddlUnidadeEscola.Items.Clear();
        odsUnidadeEscola.SelectParameters.Clear();
        _ddlUnidadeEscola.DataSourceID = odsUnidadeEscola.ID;
        _ddlUnidadeEscola.DataTextField = "esc_uni_nome";
        odsUnidadeEscola.SelectMethod = "SelecionarEscolasPorPoloUnidadeAdministrativaSuperior";
        odsUnidadeEscola.SelectParameters.Add("ppl_id", ppl_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("uad_idSuperior", uad_idSuperior.ToString());
        odsUnidadeEscola.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("adm", (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao).ToString());
        odsUnidadeEscola.SelectParameters.Add("ordenarEscolasPorCodigo", ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToString());

        _MostrarMessageSelecione = true;
        _ddlUnidadeEscola.DataBind();
    }

    /// <summary>
    /// Carrega as escolas por pólo de planejamento, exceto escola passada por parâmetro.
    /// </summary>
    /// <param name="ppl_id">ID do pólo de planejamento.</param>
    /// <param name="esc_id">ID da escola.</param>
    public void CarregarPorPoloPlanejamentoRemanejamento(int ppl_id, int esc_id)
    {
        CancelarConsulta = false;
        _ddlUnidadeEscola.Items.Clear();
        odsUnidadeEscola.SelectParameters.Clear();
        _ddlUnidadeEscola.DataSourceID = odsUnidadeEscola.ID;
        _ddlUnidadeEscola.DataTextField = "esc_uni_nome";
        odsUnidadeEscola.SelectMethod = "SelecionaEscolasPorPoloRemanejamento";
        odsUnidadeEscola.SelectParameters.Add("ppl_id", ppl_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("esc_id", esc_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("ordenarEscolasPorCodigo", ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToString());

        _MostrarMessageSelecione = true;
        _ddlUnidadeEscola.DataBind();
    }

    /// <summary>
    /// Carrega o combo
    /// </summary>
    /// <param name="dataSource">Dados a serem inseridos no combo</param>
    public void CarregarCombo(object datasource)
    {
        _ddlUnidadeEscola.Items.Clear();
        _ddlUnidadeEscola.DataSourceID = string.Empty;
        _ddlUnidadeEscola.DataSource = datasource;
        _MostrarMessageSelecione = true;
        _ddlUnidadeEscola.DataBind();
    }

    #region Verificar

    // UCFiltroEscolas
    public void _LoadBy_uad_idSuperior_cfg_id(Guid uad_idSuperior, int cfg_id)
    {
        _ddlUnidadeEscola.Items.Clear();
        odsUnidadeEscola.SelectParameters.Clear();
        _ddlUnidadeEscola.DataTextField = "esc_uni_nome";
        odsUnidadeEscola.SelectMethod = "GetSelectByUASuperiorConfiguracao";
        odsUnidadeEscola.EnablePaging = false;
        odsUnidadeEscola.SelectCountMethod = string.Empty;
        odsUnidadeEscola.MaximumRowsParameterName = string.Empty;
        odsUnidadeEscola.SelectParameters.Add("uad_idSuperior", uad_idSuperior.ToString());
        odsUnidadeEscola.SelectParameters.Add("cfg_id", cfg_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("uad_id", UAsVisaoGrupo());

        odsUnidadeEscola.DataBind();

        _MostrarMessageSelecione = true;
        _ddlUnidadeEscola.DataBind();
    }

    // UCFiltroEscolas
    public void _LoadBy_cur_id_crr_id_crp_id(int cur_id, int crr_id, int crp_id)
    {
        _ddlUnidadeEscola.Items.Clear();
        odsUnidadeEscola.SelectParameters.Clear();
        _ddlUnidadeEscola.DataTextField = "esc_uni_nome";
        odsUnidadeEscola.SelectMethod = "GetSelectByCursoCurriculoPeriodo";
        odsUnidadeEscola.EnablePaging = false;
        odsUnidadeEscola.SelectCountMethod = string.Empty;
        odsUnidadeEscola.MaximumRowsParameterName = string.Empty;
        odsUnidadeEscola.SelectParameters.Add("cur_id", cur_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("crr_id", crr_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("crp_id", crp_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("uad_id", UAsVisaoGrupo());

        odsUnidadeEscola.DataBind();

        _MostrarMessageSelecione = true;
        _ddlUnidadeEscola.DataBind();
    }

    // UCFiltroEscolas
    public void _LoadBy_uad_idSuperior_cur_id_crr_id_crp_id(Guid uad_idSuperior, int cur_id, int crr_id, int crp_id)
    {
        _ddlUnidadeEscola.Items.Clear();
        odsUnidadeEscola.SelectParameters.Clear();
        _ddlUnidadeEscola.DataTextField = "esc_uni_nome";
        odsUnidadeEscola.SelectMethod = "GetSelectByUASuperiorCursoCurriculoPeriodo";
        odsUnidadeEscola.EnablePaging = false;
        odsUnidadeEscola.SelectCountMethod = string.Empty;
        odsUnidadeEscola.MaximumRowsParameterName = string.Empty;
        odsUnidadeEscola.SelectParameters.Add("uad_idSuperior", uad_idSuperior.ToString());
        odsUnidadeEscola.SelectParameters.Add("cur_id", cur_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("crr_id", crr_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("crp_id", crp_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsUnidadeEscola.SelectParameters.Add("uad_id", UAsVisaoGrupo());

        odsUnidadeEscola.DataBind();

        _MostrarMessageSelecione = true;
        _ddlUnidadeEscola.DataBind();
    }

    #endregion

    #endregion

    #region EVENTOS

    protected void _ddlUnidadeEscola_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_OnSelectedIndexChange != null)
            _OnSelectedIndexChange();
    }

    protected void odsUnidadeEscola_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {        
        // Seta evento Selected.
        odsUnidadeEscola.Selected += odsUnidadeEscola_Selected;
        e.Cancel = CancelarConsulta;// && _ddlUnidadeEscola.Items.Count > 0;

        //Seta a variável CancelaConsulta para true, dessa forma, ao passar pelo método novamente os dados do combo não são duplicados
        CancelarConsulta = true;
    }

    protected void odsUnidadeEscola_Selected(object sender, ObjectDataSourceStatusEventArgs e)
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
                        _ddlUnidadeEscola.SelectedValue = dt.Rows[0][_ddlUnidadeEscola.DataValueField].ToString();
                    }
                }
            }
            catch
            {
                if (_ddlUnidadeEscola.Items.FindByValue("-1;-1") != null)
                    _ddlUnidadeEscola.SelectedValue = "-1;-1";
            }
        }
        else
        {
            // Grava o erro e mostra pro usuário.
            ApplicationWEB._GravaErro(e.Exception.InnerException);

            lblMessage.Text = "Erro ao tentar carregar " + _Label.Text.Replace('*', ' ').ToLower() +".";
            
            lblMessage.Visible = true;
            e.ExceptionHandled = true;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        _ddlUnidadeEscola.AutoPostBack = _OnSelectedIndexChange != null;

        // Se o combo foi preenchido automaticamente.
        if ((!Page.IsPostBack) && (Esc_ID > 0) && (_OnSelectedIndexChange != null) && (SelecionaAutomatico))
            _OnSelectedIndexChange();
    }

    #endregion
}

using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;
using System.Data;

public partial class WebControls_Combos_UCComboCurriculoPeriodo : MotherUserControl
{
    #region Delegates

    public delegate void SelectedIndexChange();
    public event SelectedIndexChange _OnSelectedIndexChange;

    public delegate void SelectedIndexChange_Sender(object sender, EventArgs e);
    public event SelectedIndexChange_Sender _OnSelectedIndexChange_Sender;

    #endregion

    #region Propriedades

    public string ComboID
    {
        get { return ddlCombo.ID; }
    }


    private bool cancelSelect;
    /// <summary>
    /// Cancela o consulta do ObjectDataSource ao carregar a página pela primeira vez.
    /// </summary>
    public bool CancelSelect
    {
        get { return cancelSelect; }
        set { cancelSelect = value; }
    }

    /// <summary>
    /// Retorna e seta o valor selecionado no combo.
    /// valor[0] = cur_id
    /// valor[1] = crr_id
    /// valor[2] = crp_id
    /// </summary>
    public Int32[] Valor
    {
        get
        {
            string[] s = ddlCombo.SelectedValue.Split(';');

            if (s.Length == 3)
                return new[] { Convert.ToInt32(s[0]), Convert.ToInt32(s[1]), Convert.ToInt32(s[2]) };

            return new[] { -1, -1, -1 };
        }
        set
        {
            string s;
            if (value.Length == 3)
                s = value[0] + ";" + value[1] + ";" + value[2];
            else
                s = "-1;-1;-1";

            if (ddlCombo.Items.FindByValue(s) != null)
                ddlCombo.SelectedValue = s;
        }
    }

    /// <summary>
    /// Propriedade que seta o Width do combo.   
    /// </summary>
    public int WidthCombo
    {
        set
        {
            ddlCombo.Width = value;
        }
    }

    /// <summary>
    /// Propriedade que seta o SelectedIndex do Combo.       
    /// </summary>
    public int SelectedIndex
    {
        set
        {
            ddlCombo.SelectedValue = ddlCombo.Items[value].Value;
        }
    }

    /// <summary>
    /// Propriedade que verifica quantos items existem no combo
    /// </summary>
    public int QuantidadeItensCombo
    {
        get
        {
            return ddlCombo.Items.Count;
        }
    }

    /// <summary>
    /// Propriedade que seta a label e a validação do combo
    /// </summary>
    public bool Obrigatorio
    {
        set
        {
            if (value)
            {
                AdicionaAsteriscoObrigatorio(lblTitulo);
            }
            else
            {
                RemoveAsteriscoObrigatorio(lblTitulo);

            }

            cpvCombo.Visible = value;
        }
    }

    /// <summary>
    /// Deixa o combo habilitado de acordo com o valor passado
    /// </summary>
    public bool PermiteEditar
    {
        get
        {
            return ddlCombo.Enabled;
        }
        set
        {
            ddlCombo.Enabled = value;
        }
    }

    /// <summary>
    /// Seta o validationGroup do combo.
    /// </summary>
    public string ValidationGroup
    {
        set
        {
            cpvCombo.ValidationGroup = value;
        }
    }

    /// <summary>
    /// Atribui valor para o skin do combo
    /// </summary>
    public string SkinIDCombo
    {
        set
        {
            ddlCombo.SkinID = value;
        }
    }

    /// <summary>
    /// Atribui valores para o combo
    /// </summary>
    public bool ExibeFormatoPeriodo
    {
        set
        {
            lblFormatoPeriodo.Visible = value;
        }
    }

    /// <summary>
    /// Exibe ou não o combo, o titulo e o validador de acordo com o valor passado
    /// </summary>
    public bool ExibeCombo
    {
        set
        {
            ddlCombo.Visible = value;
            lblTitulo.Visible = value;
            cpvCombo.Visible = value;
        }
    }

    /// <summary>
    /// Atribui valores para o combo
    /// </summary>
    public DropDownList _Combo
    {
        get
        {
            return ddlCombo;
        }
        set
        {
            ddlCombo = value;
        }
    }

    /// <summary>
    /// Atribui valores para o combo
    /// </summary>
    public Label _Label
    {
        get
        {
            return lblTitulo;
        }
        set
        {
            lblTitulo = value;
        }
    }

    /// <summary>
    /// Adciona e remove a mensagem "Selecione um Período" do dropdownlist.  
    /// Por padrão é false e a mensagem "Selecione um Período" não é exibida.
    /// </summary>
    public bool _MostrarMessageSelecione
    {
        set
        {
            if (value && __SessionWEB != null && __SessionWEB.__UsuarioWEB != null && __SessionWEB.__UsuarioWEB.Usuario != null)
            {
                if (ddlCombo.Items.IndexOf(ddlCombo.Items.FindByValue("-1;-1;-1")) == -1)
                    ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1;-1", true));
            }

            ddlCombo.AppendDataBoundItems = value;
        }
    }

    /// <summary>
    /// Retorna o texto selecionado no combo
    /// </summary>
    public string Texto
    {
        get
        {
            return ddlCombo.SelectedItem.ToString();
        }
    }

    /// <summary>
    /// Propriedade visible da label do nome do combo
    /// </summary>
    public bool LabelVisible
    {
        set
        {
            lblTitulo.Visible = value;
        }
    }

    /// <summary>
    /// Retorna o título do combo (nome do CurriculoPeriodo no sistema).
    /// </summary>
    public string Titulo
    {
        get
        {
            return lblTitulo.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, "");
        }
    }

    /// <summary>
    /// Propriedade para controlar o viewstate dos controles:
    /// odsDados
    /// </summary>
    public bool GuardaViewState
    {
        get
        {
            return odsDados.EnableViewState;
        }
        set
        {
            odsDados.EnableViewState = value;
        }
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Retorna os currículos que concluem o nível de ensino.
    /// </summary>
    /// <param name="crr_id">Id do curso</param>
    /// <param name="cur_id">Id do currículo do curso</param>    
    public void CarregarAnosFinais(int cur_id, int crr_id)
    {
        odsDados.SelectParameters.Clear();
        ddlCombo.Items.Clear();

        ddlCombo.DataSourceID = odsDados.ID;

        odsDados.SelectMethod = "CarregarAnosFinais";
        odsDados.SelectParameters.Add("cur_id", cur_id.ToString());
        odsDados.SelectParameters.Add("crr_id", crr_id.ToString());
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        ddlCombo.DataBind();

        if (ddlCombo.Items.IndexOf(ddlCombo.Items.FindByValue("-1;-1;-1")) == -1)
            ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1;-1", true));

        ddlCombo.SelectedValue = "-1;-1;-1";
    }

    /// <summary>
    /// Seta o foco no combo.
    /// </summary>
    public void FocaCombo()
    {
        ddlCombo.Focus();
    }

    /// <summary>
    /// Verifica se o combo possui somente um item e seleciona o primeiro.
    /// </summary>
    public bool SelecionaPrimeiroItem()
    {
        if ((QuantidadeItensCombo == 2) &&
            (Valor[0] == -1))
        {
            // Seleciona o primeiro item.
            ddlCombo.SelectedValue = ddlCombo.Items[1].Value;

            if (_OnSelectedIndexChange != null)
                _OnSelectedIndexChange();

            return true;
        }

        return false;
    }

    /// <summary>
    /// Mostra os Curriculo Período não excluídos logicamente no dropdownlist  
    /// </summary>
    /// <param name="cur_id">ID de Curso</param>
    /// <param name="crr_id">ID de curriculo</param>
    public void _Load(int cur_id, int crr_id)
    {
        CancelSelect = false;
        odsDados.SelectParameters.Clear();
        ddlCombo.DataSourceID = odsDados.ID;
        ddlCombo.Items.Clear();

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1;-1", true));
        ddlCombo.AppendDataBoundItems = true;

        odsDados.SelectParameters.Add("cur_id", cur_id.ToString());
        odsDados.SelectParameters.Add("crr_id", crr_id.ToString());
        odsDados.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());
        ddlCombo.DataBind();
    }

    public void CarregarPorCursoDisciplina(int cur_id, int crr_id, int dis_id)
    {
        odsDados.SelectParameters.Clear();
        ddlCombo.DataSourceID = "";
        ddlCombo.Items.Clear();

        ddlCombo.DataSource = ACA_CurriculoPeriodoBO.SelecionaPorCursoDisciplina(cur_id, crr_id, dis_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo);

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1;-1", true));
        ddlCombo.AppendDataBoundItems = true;

        ddlCombo.DataBind();
    }

    /// <summary>
    /// Carrega no combo os currículoPeriodo cadastrados para o curso e escola informados, e filtra também
    /// o currículoPeríodo que seja equivalente (mesmo crp_ordem) do crpEquivalente informado.
    /// </summary>
    /// <param name="cur_id">ID do curso</param>
    /// <param name="crr_id">ID do currículo</param>
    /// <param name="esc_id">ID da escola</param>
    /// <param name="uni_id">ID da unidade da escola</param>
    /// <param name="cur_idEquivalente">ID do curso equivalente</param>
    /// <param name="crr_idEquivalente">ID do currículo equivalente</param>
    /// <param name="crp_idEquivalente">ID do período equivalente</param>
    /// <returns></returns>
    public void Carregar_Por_CursoEscola_PeriodoEquivalente
        (int cur_id, int crr_id, int esc_id, int uni_id, int cur_idEquivalente, int crr_idEquivalente, int crp_idEquivalente)
    {
        odsDados.SelectParameters.Clear();
        ddlCombo.Items.Clear();

        ddlCombo.DataSourceID = "";

        ddlCombo.DataSource = ACA_CurriculoPeriodoBO.Seleciona_Por_CursoEscola_PeriodoEquivalente
            (cur_id, crr_id, esc_id, uni_id, cur_idEquivalente, crr_idEquivalente, crp_idEquivalente);
        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1;-1", true));
        ddlCombo.AppendDataBoundItems = true;

        ddlCombo.DataBind();
    }

    /// <summary>
    /// Mostra os Curriculo Período não excluídos logicamente no dropdownlist 
    /// que pertencem ao Curriculo Escola Periodo.
    /// </summary>
    /// <param name="cur_id">ID de Curso</param>
    /// <param name="crr_id">ID de curriculo</param>
    /// <param name="esc_id">ID de escola</param>
    /// <param name="uni_id">ID de unidade</param>
    public void _LoadBy_cur_id_crr_id_esc_id_uni_id(int cur_id, int crr_id, int esc_id, int uni_id)
    {
        odsDados.SelectParameters.Clear();
        ddlCombo.Items.Clear();

        ddlCombo.DataSourceID = odsDados.ID;

        odsDados.SelectParameters.Add("cur_id", cur_id.ToString());
        odsDados.SelectParameters.Add("crr_id", crr_id.ToString());
        odsDados.SelectParameters.Add("esc_id", esc_id.ToString());
        odsDados.SelectParameters.Add("uni_id", uni_id.ToString());
        odsDados.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());
        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1;-1", true));
        ddlCombo.AppendDataBoundItems = true;
        ddlCombo.DataBind();

        ddlCombo.SelectedValue = "-1;-1;-1";
    }

    /// <summary>
    /// Retorna os períodos por curso e disciplina da turma
    /// </summary>
    /// <param name="cur_id">Id da tabela ACA_Curso do bd</param>
    /// <param name="crr_id">Id da tabela ACA_Curriculo do bd</param>
    /// <param name="tud_id">ID da disciplina da turma</param>    
    public void CarregarPorCursoTurmaDisciplina(int cur_id, int crr_id, long tud_id)
    {
        odsDados.SelectParameters.Clear();
        ddlCombo.Items.Clear();

        ddlCombo.DataSourceID = odsDados.ID;

        odsDados.SelectMethod = "SelecionaPorCursoTurmaDisciplina";
        odsDados.SelectParameters.Add("cur_id", cur_id.ToString());
        odsDados.SelectParameters.Add("crr_id", crr_id.ToString());
        odsDados.SelectParameters.Add("tud_id", tud_id.ToString());
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        ddlCombo.DataBind();

        if (ddlCombo.Items.IndexOf(ddlCombo.Items.FindByValue("-1;-1;-1")) == -1)
            ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1;-1", true));

        ddlCombo.SelectedValue = "-1;-1;-1";
    }

    /// <summary>
    /// Retorna os períodos por curso e disciplina da turma
    /// </summary>
    /// <param name="dis_id">ID da disciplina</param>    
    /// <param name="cur_id">Id da tabela ACA_Curso do bd</param>
    /// <param name="crr_id">Id da tabela ACA_Curriculo do bd</param>
    public void CarregarPorDisciplina(int dis_id, int cur_id, int crr_id)
    {
        odsDados.SelectParameters.Clear();
        ddlCombo.Items.Clear();

        ddlCombo.DataSourceID = "";
        ddlCombo.AppendDataBoundItems = true;

        ddlCombo.DataSource = ACA_CurriculoPeriodoBO.Select_Por_Disciplina(dis_id, cur_id, crr_id);
        ddlCombo.DataBind();

        if (ddlCombo.Items.IndexOf(ddlCombo.Items.FindByValue("-1;-1;-1")) == -1)
            ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1;-1", true));

        ddlCombo.SelectedValue = "-1;-1;-1";
    }

    /// <summary>
    /// Retorna os períodos que possuem a mesma quantidade de níveis da orientação curricular
    /// </summary>
    /// <param name="qtdeNiveis">Quantidade de níveis da orientação curricular</param>
    /// <param name="cur_id">Id do curso</param>
    /// <param name="crr_id">Id do currículo</param>
    public void CarregarPorQtdeNivelOrientacaoCurricular(int qtdeNiveis, int cur_id, int crr_id, int cal_id, int tds_id, Guid ent_id)
    {
        odsDados.SelectParameters.Clear();
        ddlCombo.Items.Clear();

        ddlCombo.DataSourceID = "";
        ddlCombo.AppendDataBoundItems = true;

        ddlCombo.DataValueField = "cur_id_crr_id_crp_id";
        ddlCombo.DataTextField = "crp_descricao";

        DataTable dtCurriculoPeriodo = ACA_CurriculoPeriodoBO.SelecionaPorQtdeNiveisOrientacaoCurricular(cur_id, crr_id, cal_id, tds_id, ent_id, qtdeNiveis);
        int aux = 0;

        foreach (DataRow row in dtCurriculoPeriodo.Rows)
        {
            ddlCombo.Items.Insert(aux, new ListItem(row["crp_descricao"].ToString(), row["cur_id_crr_id_crp_id"].ToString(), true));
            aux++;
        }

        if (ddlCombo.Items.IndexOf(ddlCombo.Items.FindByValue("-1;-1;-1")) == -1)
            ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1;-1", true));

        ddlCombo.SelectedValue = "-1;-1;-1";
    }

    /// <summary>
    /// Seleciona por escola, curso, e ordem do período (apenas períodos com ordem igual ou superior)
    /// </summary>
    /// <param name="esc_id">ID da escola.</param>
    /// <param name="uni_id">ID da unidade de escola.</param>
    /// <param name="cur_id">ID do curso.</param>
    /// <param name="crr_id">ID do currículo do curso.</param>
    /// <param name="crp_ordem">Ordem de validação do período.</param>
    public void CarregarPorEscolaCursoPeriodoOrdem(int esc_id, int uni_id, int cur_id, int crr_id, int crp_ordem)
    {
        odsDados.SelectParameters.Clear();
        ddlCombo.Items.Clear();

        ddlCombo.DataSourceID = "";
        ddlCombo.AppendDataBoundItems = true;

        ddlCombo.DataSource = ACA_CurriculoPeriodoBO.SelecionaPorEscolaCursoPeriodoOrdem(esc_id, uni_id, cur_id, crr_id, crp_ordem, ApplicationWEB.AppMinutosCacheLongo);

        if (ddlCombo.Items.IndexOf(ddlCombo.Items.FindByValue("-1;-1;-1")) == -1)
            ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1;-1", true));

        ddlCombo.DataBind();

        ddlCombo.SelectedValue = "-1;-1;-1";
    }

    /// <summary>
    /// Carrega os curriculo período
    /// filtrados por curso e currículo e tipo ciclo
    /// </summary>
    /// <param name="cur_id">ID do curso</param>
    /// <param name="crr_id">ID do currículo</param>
    /// <param name="tci_id">ID do tipo do ciclo</param>
    public void CarregarPorCursoCurriculoTipoCiclo(int cur_id, int crr_id, int tci_id)
    {
        odsDados.SelectParameters.Clear();
        ddlCombo.Items.Clear();

        ddlCombo.DataSourceID = "";
        ddlCombo.AppendDataBoundItems = true;

        ddlCombo.DataSource = ACA_CurriculoPeriodoBO.Select_Por_TipoCiclo(cur_id, crr_id, tci_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo);

        if (ddlCombo.Items.IndexOf(ddlCombo.Items.FindByValue("-1;-1;-1")) == -1)
            ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1;-1", true));

        ddlCombo.DataBind();

        ddlCombo.SelectedValue = "-1;-1;-1";
    }

    #endregion

    #region Eventos

    #region Page Life Cycle

    protected void Page_Init(object sender, EventArgs e)
    {
        bool obrigatorio = lblTitulo.Text.EndsWith(ApplicationWEB.TextoAsteriscoObrigatorio) ||
                           lblTitulo.Text.EndsWith(" *");

        //Altera o Label para o nome padrão de período no sistema
        lblTitulo.Text = GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id);

        //Altera a mensagem de validação para o nome padrão de curso no sistema
        cpvCombo.ErrorMessage = GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " é obrigatório.";

        Obrigatorio = obrigatorio;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ddlCombo.AutoPostBack = (_OnSelectedIndexChange != null) || (_OnSelectedIndexChange_Sender != null);
    }

    #endregion

    protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_OnSelectedIndexChange != null)
            _OnSelectedIndexChange();

        if (_OnSelectedIndexChange_Sender != null)
            _OnSelectedIndexChange_Sender(sender, e);
    }

    protected void odsDados_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.Cancel = cancelSelect;
        if (e.Cancel)
            ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", "-1;-1;-1", true));
        else
        {
            e.InputParameters["ent_id"] = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
            odsDados.Selected += odsDados_Selected;
        }
    }

    protected void odsDados_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            // Grava o erro e mostra pro usuário.
            ApplicationWEB._GravaErro(e.Exception.InnerException);

            e.ExceptionHandled = true;

            lblMessage.Text = "Erro ao tentar carregar o(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + ".";
            lblMessage.Visible = true;
        }
    }

    #endregion
}

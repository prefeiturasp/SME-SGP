using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboTipoDisciplina : MotherUserControl
{
    #region DELEGATES

    public delegate void SelectedIndexChanged();
    public event SelectedIndexChanged IndexChanged;

    public delegate void SelectedIndexChange_Sender(object sender, EventArgs e);
    public event SelectedIndexChange_Sender IndexChanged_Sender;

    #endregion

    #region PROPRIEDADES

    /// <summary>
    /// Retorna e seta o valor selecionado no combo, tds_id
    /// </summary>
    public int Valor
    {
        get
        {
            return Convert.ToInt32(ddlCombo.SelectedValue);
        }
        set
        {
            if (ddlCombo.Items.FindByValue(value.ToString()) != null)
                ddlCombo.SelectedValue = value.ToString();
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
    /// Propriedade que seta o Width do combo.
    /// </summary>
    public Int32 WidthCombo
    {
        set
        {
            ddlCombo.Width = value;
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
    /// Deixa o combo habilitado de acordo com o valor passado
    /// </summary>
    public bool PermiteEditar
    {
        set
        {
            ddlCombo.Enabled = value;
        }
    }

    ///<summary>
    ///Seta a Label lblTitulo
    ///</summary>
    public string Titulo
    {
        get
        {
            return lblTitulo.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, "");
        }
        set
        {
            lblTitulo.Text = value;
            cpvCombo.ErrorMessage = value.Replace('*', ' ') + " é obrigatório.";
        }
    }

    /// <summary>
    /// Mostra ou não o label do combo
    /// </summary>
    public bool ExibeTitulo
    {
        set
        {
            lblTitulo.Visible = value;
        }
    }

    /// <summary>
    /// Seta um valor diferente do padrão para o SkinID do combo
    /// </summary>
    public string Combo_CssClass
    {
        set
        {
            ddlCombo.CssClass = value;
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

    #endregion

    #region METODOS

    /// <summary>
    /// Seta o foco no combo    
    /// </summary>
    public void SetarFoco()
    {
        ddlCombo.Focus();
    }

    /// <summary>
    /// Mostra os dados não excluídos logicamente no dropdownlist    
    /// </summary>
    public void CarregarTipoDisciplina()
    {
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectParameters.Add("AppMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        odsDados.SelectMethod = "SelecionaTipoDisciplina";
        ddlCombo.DataTextField = "tds_nome";

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " --", "-1", true));
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Mostra os dados não excluídos logicamente no dropdownlist exibindo o nível de ensino
    /// e o tipo de disciplina no combo   
    /// </summary>
    public void CarregarNivelEnsinoTipoDisciplina()
    {
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectParameters.Add("AppMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());            

        ddlCombo.DataTextField = "tne_tds_nome";
        odsDados.SelectMethod = "SelecionaTipoDisciplina";

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " --", "-1", true));
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Mostra os dados não excluídos logicamente no dropdownlist exibindo o nível de ensino
    /// e o tipo de disciplina no combo   
    /// </summary>
    public void CarregarNivelEnsinoTipoDisciplinaObjetosAprendizagem(int cal_ano)
    {
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();
        odsDados.SelectParameters.Add("cal_ano", cal_ano.ToString());
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectParameters.Add("AppMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        ddlCombo.DataTextField = "tne_tds_nome";
        odsDados.SelectMethod = "SelecionaTipoDisciplinaObjetosAprendizagem";

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " --", "-1", true));
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Mostra os dados não excluídos logicamente no dropdownlist por nível de ensino   
    /// </summary>
    public void CarregarTipoDisciplinaPorNivelEnsino(int tne_id)
    {
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();

        ddlCombo.DataTextField = "tds_nome";
        odsDados.SelectMethod = "SelecionaTipoDisciplinaPorNivelEnsino";
        odsDados.SelectParameters.Add("tne_id", tne_id.ToString());
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " --", "-1", true));
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Mostra os tipos de disciplinas que pertencem ao curso, currículo e período
    /// </summary>
    public void CarregarTipoDisciplinaPorCursoCurriculoPeriodo(int cur_id, int crr_id, int crp_id)
    {
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();

        ddlCombo.DataTextField = "tds_nome";
        odsDados.SelectMethod = "SelecionaTipoDisciplinaPorCursoCurriculoPeriodo";
        odsDados.SelectParameters.Add("cur_id", cur_id.ToString());
        odsDados.SelectParameters.Add("crr_id", crr_id.ToString());
        odsDados.SelectParameters.Add("crp_id", crp_id.ToString());
        odsDados.SelectParameters.Add("AppMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " --", "-1", true));
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Mostra os tipos de disciplinas que pertencem ao curso, currículo e período
    /// </summary>
    public void CarregarTipoDisciplinaPorCursoCurriculoPeriodoEscola(int cur_id, int crr_id, int crp_id, int esc_id, int uni_id, int cal_id, int cap_id)
    {
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();

        ddlCombo.DataTextField = "tds_nome";
        odsDados.SelectMethod = "SelecionaTipoDisciplinaPorCursoCurriculoPeriodoEscola";
        odsDados.SelectParameters.Add("cur_id", cur_id.ToString());
        odsDados.SelectParameters.Add("crr_id", crr_id.ToString());
        odsDados.SelectParameters.Add("crp_id", crp_id.ToString());
        odsDados.SelectParameters.Add("esc_id", esc_id.ToString());
        odsDados.SelectParameters.Add("uni_id", uni_id.ToString());
        odsDados.SelectParameters.Add("cal_id", cal_id.ToString());
        odsDados.SelectParameters.Add("cap_id", cap_id.ToString());
        odsDados.SelectParameters.Add("AppMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " --", "-1", true));
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Carrega todos os tipos de disciplinas de acordo com os filtros informados
    /// exceto as disciplinas do tipo Eletiva do aluno e as disciplinas do tipo informado
    /// </summary>    
    /// <param name="cur_id"></param>
    /// <param name="crr_id"></param>
    /// <param name="crp_id"></param>
    /// <param name="crd_tipo">Tipo de disciplina que NÃO será carregado</param>
    /// <returns>DataTable com os dados</returns>    
    public void CarregarTipoDisciplinaPorCursoCurriculoPeriodoTipoDisciplina(int cur_id, int crr_id, int crp_id, byte crd_tipo)
    {
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();

        ddlCombo.DataTextField = "tds_nome";
        odsDados.SelectMethod = "SelecionaTipoDisciplinaPorCursoCurriculoPeriodoTipoDisciplina";
        odsDados.SelectParameters.Add("cur_id", cur_id.ToString());
        odsDados.SelectParameters.Add("crr_id", crr_id.ToString());
        odsDados.SelectParameters.Add("crp_id", crp_id.ToString());
        odsDados.SelectParameters.Add("crd_tipo", crd_tipo.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " --", "-1", true));
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Carrega todos os tipos de disciplinas de acordo com os filtros informados
    /// </summary>
    /// <param name="cur_id"></param>
    /// <param name="tci_id"></param>
    public void CarregarTipoDisciplinaPorCursoTipoCiclo(int cur_id, int tci_id, int esc_id)
    {
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();

        ddlCombo.DataTextField = "tds_nome";
        odsDados.SelectMethod = "SelecionaTipoDisciplinaPorCursoTipoCiclo";
        odsDados.SelectParameters.Add("cur_id", cur_id.ToString());
        odsDados.SelectParameters.Add("tci_id", tci_id.ToString());
        odsDados.SelectParameters.Add("esc_id", esc_id.ToString());
        odsDados.SelectParameters.Add("AppMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " --", "-1", true));
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Adiciona e remove a mensagem "Selecione um ..." do dropdownlist.  
    /// Por padrão é false e a mensagem não é exibida.
    /// </summary>
    public bool _MostrarMessageSelecione
    {
        set
        {
            if (value)
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione um tipo de " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " --", "-1", true));
            ddlCombo.AppendDataBoundItems = value;
        }
    }

    #endregion

    #region EVENTOS

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ddlCombo.AutoPostBack = (IndexChanged != null) || (IndexChanged_Sender != null);
    }

    protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (IndexChanged != null)
            IndexChanged();

        if (IndexChanged_Sender != null)
            IndexChanged_Sender(sender, e);
    }

    protected void odsDados_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            // Grava o erro e mostra pro usuário.
            ApplicationWEB._GravaErro(e.Exception.InnerException);

            e.ExceptionHandled = true;

            lblMessage.Text = "Erro ao tentar carregar " + lblTitulo.Text.Replace('*', ' ').ToLower() + ".";
            lblMessage.Visible = true;
        }
    }

    #endregion
}

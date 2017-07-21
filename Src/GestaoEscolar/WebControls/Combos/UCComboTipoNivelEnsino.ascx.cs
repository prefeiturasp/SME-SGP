using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboNivelEnsino : MotherUserControl
{
    #region DELEGATES

    public delegate void SelectedIndexChanged();
    public event SelectedIndexChanged IndexChanged;

    #endregion

    #region PROPRIEDADES

    /// <summary>
    /// Retorna e seta o valor selecionado no combo
    /// </summary>
    public int Valor
    {
        get
        {
            return Convert.ToInt32(ddlCombo.SelectedValue);
        }
        set
        {
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
    /// ClientID do combo
    /// </summary>
    public string Combo_ClientID
    {
        get
        {
            return ddlCombo.ClientID;
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

    /// <summary>
    /// Seta um titulo diferente do padrão para o combo
    /// </summary>
    public string Titulo
    {
        set
        {
            lblTitulo.Text = value;
            cpvCombo.ErrorMessage = value.Replace('*', ' ') + " é obrigatório.";
        }
        get
        {
            return lblTitulo.Text;  
        }

    }

    public bool MostrarMessageSelecione
    {
        set
        {
            if (value && __SessionWEB != null && __SessionWEB.__UsuarioWEB != null && __SessionWEB.__UsuarioWEB.Usuario != null)
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione um nível de ensino --", "-1", true));
            ddlCombo.AppendDataBoundItems = value;
        }
    }

    public bool MostrarMessageTodos
    {
        set
        {
            if (value && __SessionWEB != null && __SessionWEB.__UsuarioWEB != null && __SessionWEB.__UsuarioWEB.Usuario != null)
                ddlCombo.Items.Insert(0, new ListItem("Todos", "-1", true));
            ddlCombo.AppendDataBoundItems = value;
        }
    }

    /// <summary>
    /// Indica se deve trazer o primeiro item selecinado caso seja o único
    /// (Sem contar a MensagemSelecione)
    /// </summary>
    public bool TrazerComboCarregado
    {
        get
        {
            if (ViewState["TrazerComboCarregado"] != null)
                return Convert.ToBoolean(ViewState["TrazerComboCarregado"]);
            return false;
        }
        set
        {
            ViewState["TrazerComboCarregado"] = value;
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

    #endregion

    #region METODOS

    /// <summary>
    /// Traz o primeiro item selecinado caso seja o único
    /// </summary>
    private void SelecionaPrimeiroItem()
    {
        if (TrazerComboCarregado && ddlCombo.Items.Count == 2 && ddlCombo.Items[0].Value == "-1")
        {
            // Seleciona o primeiro item.
            ddlCombo.SelectedValue = ddlCombo.Items[1].Value;

            if (IndexChanged != null)
                IndexChanged();
        }
    }

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
    public void CarregarTipoNivelEnsino()
    {
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();
        odsDados.SelectMethod = "SelecionaTipoNivelEnsino";

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um nível de ensino --", "-1", true));
        ddlCombo.DataBind();
        SelecionaPrimeiroItem();
    }

    /// <summary>
    /// Mostra os dados não excluídos logicamente no dropdownlist    
    /// </summary>
    public void CarregarTipoNivelEnsinoSemInfantil()
    {
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectMethod = "SelecionaTipoNivelEnsinoSemInfantil";

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um nível de ensino --", "-1", true));
        ddlCombo.DataBind();
        SelecionaPrimeiroItem();
    }

    /// <summary>
    /// Mostra os dados não excluídos logicamente no dropdownlist    
    /// </summary>
    public void CarregarTipoNivelEnsinoEscola(int esc_id, int uni_id)
    {
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();
        odsDados.SelectParameters.Add("esc_id", esc_id.ToString());
        odsDados.SelectParameters.Add("uni_id", uni_id.ToString());
        odsDados.SelectMethod = "SelectByPesquisaTipoNivelEnsinoEscola";
        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um nível de ensino --", "-1", true));
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Mostra os dados não excluídos logicamente no dropdownlist    
    /// de acordo com as atribuições do docente.
    /// </summary>
    public void CarregarTipoNivelEnsinoDocenteEventoAno(long doc_id, string eventosAbertos, int cal_ano)
    {
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();
        odsDados.SelectParameters.Add("doc_id", doc_id.ToString());
        odsDados.SelectParameters.Add("cal_ano", cal_ano.ToString());
        odsDados.SelectParameters.Add("eventosAbertos", eventosAbertos);
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectMethod = "SelecionaTipoNivelEnsinoDocenteEventoAno";

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um nível de ensino --", "-1", true));
        ddlCombo.DataBind();
        SelecionaPrimeiroItem();
    }

    /// <summary>
    /// Mostra os dados não excluídos logicamente no dropdownlist    
    /// de acordo com as atribuições do docente.
    /// </summary>
    public void CarregarTipoNivelEnsinoDocenteEventoSemInfantilAno(long doc_id, string eventosAbertos, int cal_ano)
    {
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();
        odsDados.SelectParameters.Add("doc_id", doc_id.ToString());
        odsDados.SelectParameters.Add("cal_ano", cal_ano.ToString());
        odsDados.SelectParameters.Add("eventosAbertos", eventosAbertos);
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectMethod = "SelecionaTipoNivelEnsinoDocenteEventoSemInfantilAno";

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um nível de ensino --", "-1", true));
        ddlCombo.DataBind();
        SelecionaPrimeiroItem();
    }

    /// <summary>
    /// Mostra os dados não excluídos logicamente no dropdownlist    
    /// de acordo com as atribuições do docente.
    /// </summary>
    public void CarregarTipoNivelEnsinoDocenteEventoSemInfantil(long doc_id, string eventosAbertos)
    {
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();
        odsDados.SelectParameters.Add("doc_id", doc_id.ToString());
        odsDados.SelectParameters.Add("eventosAbertos", eventosAbertos);
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectMethod = "SelecionaTipoNivelEnsinoDocenteEventoSemInfantil";

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um nível de ensino --", "-1", true));
        ddlCombo.DataBind();
        SelecionaPrimeiroItem();
    }

    /// <summary>
    /// Adciona e remove a mensagem "Selecione um Cruso" do dropdownlist.
    /// Por padrão é false e a mensagem "Selecione um Curso" não é exibida.
    /// </summary>

    #endregion

    #region EVENTOS

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ddlCombo.AutoPostBack = IndexChanged != null;
    }

    protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (IndexChanged != null)
            IndexChanged();
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

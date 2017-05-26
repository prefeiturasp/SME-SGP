using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;

public partial class WebControls_Combos_UCComboModalidadeEnsino : MotherUserControl
{

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
    }

    #endregion

    #region Delegates

    public delegate void SelectedIndexChanged();
    public event SelectedIndexChanged IndexChanged;

    public delegate void SelectedIndexChange_Sender(object sender, EventArgs e);
    public event SelectedIndexChange_Sender IndexChanged_Sender;
    #endregion

    #region Eventos Page Life Cycle

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ddlCombo.AutoPostBack = IndexChanged != null;
    }

    #endregion

    #region METODOS
    protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (IndexChanged != null)
            IndexChanged();

        if (IndexChanged_Sender != null)
            IndexChanged_Sender(sender, e);
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
    public void CarregarTipoModalidadeEnsino()
    {
        ddlCombo.Items.Clear();
        ddlCombo.DataSource = ACA_TipoModalidadeEnsinoBO.SelecionaTipoModalidadeEnsinoFilhos();
      
        ddlCombo.Items.Insert(0, new ListItem("-- Selecione uma modalidade de ensino --", "-1", true));
        ddlCombo.DataBind();

        //if (ddlCombo.Items.Count == 2)
        //{
        //    ddlCombo.SelectedIndex = 1;
        //}
    }

    /// <summary>
    /// Retorna todos os tipos de modalidade de ensino não excluídos logicamente
    /// Vinculados a escola informada.
    /// <param name="esc_id">Id da escola</param>
    /// <param name="uni_id">Id da unidade escolar</param>
    /// <param name="ent_id">Id da entidade</param>
    /// <param name="uad_idSuperior">Id da entidade superior</param>
    /// </summary>    
    public void CarregarTipoModalidadeEnsino_Por_Escola(int esc_id, int uni_id, Guid ent_id, Guid uad_idSuperior)
    {
        ddlCombo.Items.Clear();
        ddlCombo.DataSource = ACA_TipoModalidadeEnsinoBO.SelecionaTipoModalidadeEnsino_Por_Escola(esc_id, uni_id, ent_id, uad_idSuperior);
        ddlCombo.Items.Insert(0, new ListItem("-- Selecione uma modalidade de ensino --", "-1", true));
        ddlCombo.DataBind();

    }

    /// <summary>
    /// Adciona e remove a mensagem "Selecione um Cruso" do dropdownlist.
    /// Por padrão é false e a mensagem "Selecione um Curso" não é exibida.
    /// </summary>
    public bool MostrarMessageSelecione
    {
        set
        {
            if (value && __SessionWEB != null && __SessionWEB.__UsuarioWEB != null && __SessionWEB.__UsuarioWEB.Usuario != null)
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione uma modalidade de ensino --", "-1", true));
            ddlCombo.AppendDataBoundItems = value;
        }
    }
    #endregion

    #region EVENTOS

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


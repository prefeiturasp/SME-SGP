using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboTipoPeriodoCalendario : MotherUserControl
{
    #region Delegates

    public delegate void SelectedIndexChanged();
    public event SelectedIndexChanged IndexChanged;

    #endregion

    #region Propriedades

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
    /// Seta um valor diferente do padrão para o SkinID do combo
    /// </summary>
    public string Combo_CssClass
    {
        set
        {
            ddlCombo.CssClass = value;
        }
    }

    #endregion

    #region Métodos

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
    public void CarregarTipoPeriodoCalendario(bool removerRecesso = true)
    {
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();
        odsDados.SelectMethod = "SelecionaTipoPeriodoCalendario";
        odsDados.SelectParameters.Add("AppMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());
        odsDados.SelectParameters.Add("removerRecesso", removerRecesso.ToString());
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + lblTitulo.Text.Replace('*',' ').ToLower() + " --", "-1", true));
        ddlCombo.DataBind();
    }

    #endregion

    #region Eventos

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

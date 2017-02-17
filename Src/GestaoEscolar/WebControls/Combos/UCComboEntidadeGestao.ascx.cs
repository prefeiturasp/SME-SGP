using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;

public partial class WebControls_Combos_UCComboEntidadeGestao : MotherUserControl
{
    #region Delegates

    public delegate void SelectedIndexChanged();
    public event SelectedIndexChanged IndexChanged;

    #endregion

    #region Propriedades

    ///// <summary>
    ///// Retorna e seta o valor selecionado no combo.
    ///// Referente ao campo ent_id.
    ///// </summary>
    //public int Valor     
    //{
    //    get
    //    {
    //        if (string.IsNullOrEmpty(ddlCombo.SelectedValue))
    //            return -1;

    //        return Convert.ToInt32(ddlCombo.SelectedValue);
    //    }
    //    set
    //    {
    //        ddlCombo.SelectedValue = value.ToString();
    //    }
    //}

    /// <summary>
    /// Retorna e seta o valor selecionado no combo.
    /// Referente ao campo ent_id.
    /// </summary>
    public Guid Valor
    {
        get
        {
            //if (new Guid(ddlCombo.SelectedValue) == new Guid())
            //    return new Guid();

            return  new Guid(ddlCombo.SelectedValue);
        }
        set
        {
            ddlCombo.SelectedValue = value.ToString();
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
    /// Texto do título ao combo.
    /// </summary>
    public string Texto
    {
        set
        {
            lblTitulo.Text = value;
            cpvCombo.ErrorMessage = value + " é obrigatório.";
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
    /// Adciona e remove a mensagem "Selecione um Formato" do dropdownlist.  
    /// Por padrão é false e a mensagem "Selecione um Formato" não é exibida.
    /// </summary>
    public bool _MostrarMessageSelecione
    {
        set
        {
            if (value)
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione uma entidade --", Guid.Empty.ToString(), true));
            ddlCombo.AppendDataBoundItems = value;
        }
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Seta o foco no combo.
    /// </summary>
    public new void Focus()
    {
        ddlCombo.Focus();
    }

    /// <summary>
    /// Mostra as entidades não excluídas logicamente no dropdownlist    
    /// </summary>
    public void Carregar()
    {
        try
        {
            ddlCombo.Items.Clear();

            ddlCombo.DataSource = SYS_EntidadeBO.GetSelect(Guid.Empty, Guid.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, false, 0, 0);

            ddlCombo.Items.Insert(0, new ListItem("-- Selecione uma entidade --", Guid.Empty.ToString(), true));
            ddlCombo.AppendDataBoundItems = true;
            ddlCombo.DataBind();
        }
        catch (Exception e)
        {
            // Grava o erro e mostra pro usuário.
            ApplicationWEB._GravaErro(e.InnerException);

            lblMessage.Text = "Erro ao tentar carregar " + lblTitulo.Text.Replace('*', ' ').ToLower() + ".";
            lblMessage.Visible = true;
        }
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

    #endregion
}
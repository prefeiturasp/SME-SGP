using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboTipoMovimentacao : MotherUserControl
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
    /// Atribui valores para o label
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
    /// Atribui estados para o combo
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
    public void CarregarTipoMovimentacao
    (
        byte tmv_motivo
    )
    {
        odsDados.SelectParameters.Clear();
        ddlCombo.Items.Clear();

        odsDados.SelectParameters.Add("tmv_motivo", tmv_motivo.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um tipo de movimentação --", "-1", true));
        ddlCombo.DataBind();
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

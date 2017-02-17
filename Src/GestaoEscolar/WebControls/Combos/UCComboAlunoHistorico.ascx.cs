using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;

public partial class WebControls_Combos_UCComboAlunoHistorico : MotherUserControl
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
            return ddlCombo.SelectedItem.Text;
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
    /// Carrega todas as observações do dropDownList.
    /// </summary>
    public void CarregarObservacaoPadrao()
    {
        odsDados.SelectParameters.Clear();
        odsDados.SelectMethod = "SelecionaTodos";

        lblTitulo.Text = "Observação padrão";
        ddlCombo.Items.Clear();
        ddlCombo.Items.Insert(0, new ListItem("-- Selecione uma observação padrão --", "-1", true));

        ddlCombo.DataBind();
    }

    /// <summary>
    /// Carrega as observações no dropDownList. Filtrados por tipo.
    /// </summary>
    /// <param name="hop_tipo"></param>
    public void CarregarObservacaoPorTipo(ACA_HistoricoObservacaoPadraoTipo hop_tipo)
    {
        odsDados.SelectParameters.Clear();
        odsDados.SelectMethod = "SelecionaPorTipo";
        odsDados.SelectParameters.Add("hop_tipo", Convert.ToByte(hop_tipo).ToString());

        ddlCombo.Items.Clear();
        switch (hop_tipo)
        {
            case ACA_HistoricoObservacaoPadraoTipo.Observacao:
                lblTitulo.Text = "Observação padrão";
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione uma observação --", "-1", true));
                break;
            case ACA_HistoricoObservacaoPadraoTipo.CriterioAvaliacao:
                lblTitulo.Text = "Critérios de avaliação";
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione um critério de avaliação --", "-1", true));
                break;
        }
        ddlCombo.DataBind();
    }

    #endregion

    #region EVENTOS

    protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (IndexChanged != null)
            IndexChanged();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ddlCombo.AutoPostBack = IndexChanged != null;
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

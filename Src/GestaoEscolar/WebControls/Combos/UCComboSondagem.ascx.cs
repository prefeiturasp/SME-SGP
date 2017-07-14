using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class WebControls_Combos_UCComboSondagem : MotherUserControl
{
    public delegate void SelectedIndexChanged();
    public event SelectedIndexChanged IndexChanged;

    /// <summary>
    /// Retorna o snd_id selecionado no combo.
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
    /// Propriedade que seta a label e a validação do combo
    /// </summary>
    public bool Obrigatorio
    {
        set
        {
            if (value)
            {
                AdicionaAsteriscoObrigatorio(lblSondagem);
            }
            else
            {
                RemoveAsteriscoObrigatorio(lblSondagem);

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
    public bool Enable
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
        set
        {
            lblSondagem.Text = value;
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
            lblSondagem.Visible = value;
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
    /// Seta o foco no combo    
    /// </summary>
    public void SetarFoco()
    {
        ddlCombo.Focus();
    }

    /// <summary>
    /// Carrega todos as Sondagens ativas
    /// </summary>
    public void Carregar()
    {
        try
        {
            List<ACA_Sondagem> lstSondagem = ACA_SondagemBO.SelecionaSondagemAtiva();
            ddlCombo.DataSource = lstSondagem;

            ddlCombo.Items.Insert(0, new ListItem("-- Selecione a sondagem --", "-1", true));

            ddlCombo.DataBind();
            if (ddlCombo.Items.Count == 1)
            {
                ddlCombo.SelectedIndex = 0;
            }
        }
        catch
        {
            throw;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ddlCombo.AutoPostBack = IndexChanged != null;
    }

    protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (IndexChanged != null)
            IndexChanged();
    }

    protected void odsSondagem_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            // Grava o erro e mostra pro usuário.
            ApplicationWEB._GravaErro(e.Exception.InnerException);

            e.ExceptionHandled = true;

            lblMessage.Text = "Erro ao tentar carregar " + lblSondagem.Text.Replace('*', ' ').ToLower() + ".";
            lblMessage.Visible = true;
        }
    }
}

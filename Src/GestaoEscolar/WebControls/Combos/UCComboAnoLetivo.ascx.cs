using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;
using System.Data;

public partial class WebControls_Combos_UCComboAnoLetivo : MotherUserControl
{
    #region DELEGATES

    public delegate void SelectedIndexChanged();
    public event SelectedIndexChanged IndexChanged;

    #endregion

    #region PROPRIEDADADES

    /// <summary>
    /// Adciona e remove a mensagem "Selecione um ano" do dropdownlist.  
    /// Por padrão é false e a mensagem "Selecione um ano" não é exibida.
    /// </summary>
    public bool _MostrarMessageSelecione
    {
        set
        {
            if (value)
                this.ddlCombo.Items.Insert(0, new ListItem("-- Selecione um ano --", "-1", true));
            this.ddlCombo.AppendDataBoundItems = value;
        }
    }

    /// <summary>
    /// Retorna o ano letivo selecionado no combo.
    /// </summary>
    public int ano
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
                AdicionaAsteriscoObrigatorio(lblAnoLetivo);
            }
            else
            {
                RemoveAsteriscoObrigatorio(lblAnoLetivo);

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
            lblAnoLetivo.Text = value;
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
            lblAnoLetivo.Visible = value;
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

    #region METODOS

    /// <summary>
    /// Seta o foco no combo    
    /// </summary>
    public void SetarFoco()
    {
        ddlCombo.Focus();
    }

    /// <summary>
    /// Mostra os anos de momentos cadastrados dentro da entidade.
    /// </summary>
    /// <param name="ent_id">Entidade - obrigatório</param>
    /// <param name="mom_ano">Ano - opcional</param>
    /// <returns></returns>
    public void Carregar()
    {
        try
        {
            ddlCombo.DataSource = ACA_CalendarioAnualBO.SelecionarAnosLetivos();
            _MostrarMessageSelecione = true;
            ddlCombo.DataBind();
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Mostra os anos de momentos cadastrados dentro da entidade.
    /// </summary>
    /// <param name="ent_id">Entidade - obrigatório</param>
    /// <param name="mom_ano">Ano - opcional</param>
    /// <returns></returns>
    public void CarregarAnoAtual()
    {
        try
        {
            DataTable dt = ACA_CalendarioAnualBO.SelecionarAnosLetivos();
            ddlCombo.DataSource = (from DataRow ano in dt.Rows
                                   orderby Convert.ToInt32(ano["cal_ano"]) descending
                                   select new { cal_ano = ano["cal_ano"].ToString() }).ToList();
            ddlCombo.DataBind();
            if (ddlCombo.Items.Count > 0)
            {
                if (ddlCombo.Items.Contains(new ListItem(DateTime.Now.Year.ToString(), DateTime.Now.Year.ToString())))
                    ddlCombo.SelectedValue = DateTime.Now.Year.ToString();
                else
                    ddlCombo.SelectedIndex = 0;
            }
        }
        catch
        {
            throw;
        }
    }

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

    protected void odsAnoLetivo_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            // Grava o erro e mostra pro usuário.
            ApplicationWEB._GravaErro(e.Exception.InnerException);

            e.ExceptionHandled = true;

            lblMessage.Text = "Erro ao tentar carregar " + lblAnoLetivo.Text.Replace('*', ' ').ToLower() + ".";
            lblMessage.Visible = true;
        }
    }

    #endregion
}

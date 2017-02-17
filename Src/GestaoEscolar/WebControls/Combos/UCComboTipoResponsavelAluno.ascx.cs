using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboTipoResponsavelAluno : MotherUserControl
{
    #region DELEGATES

    public delegate void SelectedIndexChanged();

    public event SelectedIndexChanged IndexChanged;

    #endregion DELEGATES

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
    /// Atribui valores para o combo
    /// </summary>
    public DropDownList Combo
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
    /// Retorna o Label do UserControl
    /// </summary>
    public Label Label
    {
        get { return lblTitulo; }
        set { lblTitulo = value; }
    }

    /// <summary>
    /// Mostra o label de titulo de acordo com o definido(o label é mostrado por padrão)
    /// </summary>
    public bool MostraLabel
    {
        set
        {
            lblTitulo.Visible = value;
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
    /// Seta um CssClas diferente do padrão para o combo
    /// </summary>
    public string CssClass
    {
        set
        {
            ddlCombo.CssClass = value;
        }
    }

    /// <summary>
    /// Adciona e remove a mensagem "Selecione um contato" do dropdownlist.
    /// Por padrão é false e a mensagem "Selecione um tipo de contato" não é exibida.
    /// </summary>
    public bool MostrarMessageSelecione
    {
        set
        {
            string titulo = string.Empty;

            if ((value) && (ddlCombo.Items.FindByValue("-1") == null))
                if (lblTitulo.Text.EndsWith(ApplicationWEB.TextoAsteriscoObrigatorio))
                    titulo = lblTitulo.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, "");
                else if (lblTitulo.Text.EndsWith("*"))
                    titulo = lblTitulo.Text.Replace("*", "");
                else
                    titulo = lblTitulo.Text;
            ddlCombo.Items.Insert(0, new ListItem("-- Selecione um " + titulo.ToLower() + " --", "-1", true));
            ddlCombo.AppendDataBoundItems = value;
        }
    }

    #endregion PROPRIEDADES

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
    public void CarregarTipoResponsavelAluno()
    {
        ddlCombo.Items.Clear();
        ddlCombo.DataSource = ACA_TipoResponsavelAlunoBO.SelecionaTipoResponsavelAluno();
        MostrarMessageSelecione = true;
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Mostra os dados não excluídos logicamente no dropdownlist
    /// </summary>
    public void CarregarTipoResponsavelAluno(bool trazerNaoExiste, bool trazerOProprio)
    {
        ddlCombo.Items.Clear();
        ddlCombo.DataSource = ACA_TipoResponsavelAlunoBO.SelecionaTipoResponsavelAluno(trazerNaoExiste, trazerOProprio);
        MostrarMessageSelecione = true;
        ddlCombo.DataBind();
    }

    #endregion METODOS

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

    #endregion EVENTOS
}
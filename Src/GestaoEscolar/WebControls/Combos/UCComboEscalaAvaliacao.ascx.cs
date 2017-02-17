using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboEscalaAvaliacao : MotherUserControl
{
    #region Delegates

    public delegate void SelectedIndexChanged();
    public event SelectedIndexChanged IndexChanged;

    #endregion

    #region Propriedades

    /// <summary>
    /// Retorna e seta o valor selecionado no combo.
    /// valor[0] = esa_id
    /// valor[1] = esa_tipo
    /// </summary>
    public Int32[] Valor
    {
        get
        {
            string[] s = ddlCombo.SelectedValue.Split(';');

            if (s.Length == 2)
                return new[] { Convert.ToInt32(s[0]), Convert.ToInt32(s[1]) };

            return new[] { -1, -1 };
        }
        set
        {
            string s;
            if (value.Length == 2)
                s = value[0] + ";" + value[1];
            else
                s = "-1;-1";

            ddlCombo.SelectedValue = s;
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
    /// Texto do título ao combo.
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
    /// propriedade que seta se o titulo será visível.
    /// </summary>
    public bool mostraTitulo
    {
        set
        {
            lblTitulo.Visible = value;
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
    /// Adciona e remove a mensagem "Selecione uma turma SAAI – Sala de apoio e acompanhamento a inclusão" do dropdownlist.  
    /// Por padrão é false e a mensagem "Selecione uma turma SAAI – Sala de apoio e acompanhamento a inclusão" não é exibida.
    /// </summary>
    public bool MostrarMessageSelecione
    {
        set
        {
            if (value)
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione uma escala --", "-1;-1", true));
            ddlCombo.AppendDataBoundItems = value;
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
    /// Carrega as escalas de avaliação
    /// </summary>
    public void CarregarEscalaAvaliacao()
    {
        ddlCombo.Items.Clear();
        ddlCombo.DataSource = ACA_EscalaAvaliacaoBO.SelecionaEscalaAvaliacaoPorTipo(true, true, true, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        ddlCombo.Items.Insert(0, new ListItem("-- Selecione uma escala --", "-1;-1", true));
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Carrega as escalas de avaliação de acordo com o tipo da escala
    /// </summary>
    /// <param name="numerico">Indica se vai trazer as escalas do tipo numerico</param>
    /// <param name="parecer">Indica se vai trazer as escalas do tipo parecer</param>
    /// <param name="relatorio">Indica se vai trazer as escalas do tipo relatorio</param>
    public void CarregarEscalaAvaliacaoPorTipo(bool numerico, bool parecer, bool relatorio)
    {
        ddlCombo.Items.Clear();
        ddlCombo.DataSource = ACA_EscalaAvaliacaoBO.SelecionaEscalaAvaliacaoPorTipo(numerico, parecer, relatorio, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        ddlCombo.Items.Insert(0, new ListItem("-- Selecione uma escala --", "-1;-1", true));
        ddlCombo.DataBind();
    }

    protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (IndexChanged != null)
            IndexChanged();
    }

    #endregion

    #region Eventos

    protected void Page_PreRender(object sender, EventArgs e)
    {
        // Se for pra mostrar dados adicionais dá postback também.        
        ddlCombo.AutoPostBack = IndexChanged != null;
    }

    #endregion    
}

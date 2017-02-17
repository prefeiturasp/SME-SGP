using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;

public partial class WebControls_Combos_UCComboCargo : MotherUserControl
{
    #region Delegates

    public delegate void SelectedIndexChanged();
    public event SelectedIndexChanged IndexChanged;

    #endregion

    #region Constantes

    private const string valorSelecione = "-1";

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
    /// Deixa o combo habilitado de acordo com o valor passado
    /// </summary>
    public bool PermiteEditar
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
    /// Coloca na primeira linha a mensagem de selecione um item.
    /// </summary>
    public bool MostrarMensagemSelecione
    {
        get
        {
            if (ViewState["MostrarMensagemSelecione"] != null)
                return Convert.ToBoolean(ViewState["MostrarMensagemSelecione"]);
            return true;
        }
        set
        {
            ViewState["MostrarMensagemSelecione"] = value;
        }
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Carrega a mensagem de selecione de acordo com o parâmetro
    /// </summary>
    private void CarregarMensagemSelecione()
    {
        if (MostrarMensagemSelecione && (ddlCombo.Items.FindByValue(valorSelecione) == null))
            ddlCombo.Items.Insert(0, new ListItem("-- Selecione um cargo --", valorSelecione, true));

        ddlCombo.AppendDataBoundItems = MostrarMensagemSelecione;
    }

    /// <summary>
    /// Carrega o combo
    /// </summary>
    /// <param name="dataSource">Dados a serem inseridos no combo</param>
    public void CarregarCombo(object dataSource)
    {
        try
        {
            ddlCombo.Items.Clear();
            ddlCombo.DataSource = dataSource;

            CarregarMensagemSelecione();
            ddlCombo.DataBind();
        }
        catch (Exception)
        {
            lblMessage.Text = "Erro ao tentar carregar " + lblTitulo.Text.Replace('*', ' ').ToLower() + ".";
            lblMessage.Visible = true;
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
    /// Carrega os cargos não excluídos logicamente 
    /// </summary>
    public void CarregarCargo()
    {
        CarregarCombo(RHU_CargoBO.SelecionaNaoExcluidos(__SessionWEB.__UsuarioWEB.Usuario.ent_id));
    }

    /// <summary>
    /// Carrega os cargos não excluídos logicamente com crg_cargoDocente = false
    /// </summary>
    public void CarregarCargoNormal()
    {
        CarregarCombo(RHU_CargoBO.SelecionaNaoExcluidosPorCargoDocente(false, __SessionWEB.__UsuarioWEB.Usuario.ent_id));
    }

    /// <summary>
    /// Carrega os cargos não excluídos logicamente com crg_cargoDocente = true
    /// </summary>
    public void CarregarCargoDocente()
    {
        CarregarCombo(RHU_CargoBO.SelecionaNaoExcluidosPorCargoDocente(true, __SessionWEB.__UsuarioWEB.Usuario.ent_id));
    }

    /// <summary>
    /// Carrega os cargos não excluídos logicamente com crg_cargoDocente = true
    /// </summary>
    public void CarregarCargoVerificandoControleIntegracao(bool controleIntegracao)
    {
        CarregarCombo(RHU_CargoBO.SelecionaVerificandoControleIntegracao(controleIntegracao, __SessionWEB.__UsuarioWEB.Usuario.ent_id));
    }

    /// <summary>
    /// Carrega os cargos não excluídos logicamente com crg_cargoDocente = true
    /// </summary>
    public void CarregarCargoDocenteVerificandoControleIntegracao(bool controleIntegracao)
    {
        CarregarCombo(RHU_CargoBO.SelecionaVerificandoControleIntegracaoPorCargoDocente(controleIntegracao, true, __SessionWEB.__UsuarioWEB.Usuario.ent_id));
    }

    /// <summary>
    /// Carrega os cargos não excluídos logicamente filtrando por tvi_id e ent_id
    /// </summary>
    public void CarregarCargoByTipoVinculo(int tvi_id)
    {
        CarregarCombo(RHU_CargoBO.SelecionaCargoByTipoVinculo(tvi_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id));
    }

    #endregion

    #region Eventos

    #region Page Life Cycle

    protected void Page_Init(object sender, EventArgs e)
    {
        bool obrigatorio = lblTitulo.Text.EndsWith(ApplicationWEB.TextoAsteriscoObrigatorio) ||
                            lblTitulo.Text.EndsWith(" *");

        cpvCombo.ValueToCompare = valorSelecione;

        Obrigatorio = obrigatorio;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ddlCombo.AutoPostBack = (IndexChanged != null);
        CarregarMensagemSelecione();
    }

    #endregion

    protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (IndexChanged != null)
            IndexChanged();
    }

    #endregion
}

using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboTurmaDisciplina : MotherUserControl
{
    #region DELEGATES

    public delegate void SelectedIndexChanged();
    public event SelectedIndexChanged IndexChanged;

    #endregion

    #region PROPRIEDADES

    /// <summary>
    /// Retorna e seta o valor selecionado no combo
    /// </summary>
    public Int64 Valor
    {
        get
        {
            return Convert.ToInt64(ddlCombo.SelectedValue);
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

    public bool VS_MostraFilhosRegencia
    {
        get
        {
            if (ViewState["VS_MostraFilhosRegencia"] != null)
            {
                return Convert.ToBoolean(ViewState["VS_MostraFilhosRegencia"]);
            }

            return true;
        }

        set
        {
            ViewState["VS_MostraFilhosRegencia"] = value;
        }
    }

    public bool VS_MostraRegencia
    {
        get
        {
            if (ViewState["VS_MostraRegencia"] != null)
            {
                return Convert.ToBoolean(ViewState["VS_MostraRegencia"]);
            }

            return true;
        }

        set
        {
            ViewState["VS_MostraRegencia"] = value;
        }
    }

    /// <summary>
    /// Coloca na primeira linha a mensagem de selecione um item.
    /// </summary>
    public bool MostrarMensagemSelecione
    {
        set
        {
            if (value)
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GetGlobalResourceObject("Mensagens","MSG_DISCIPLINA") + " --", "-1", true));
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
    public void CarregarTurmaDisciplina(long tur_id)
    {
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();
        odsDados.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.TUR_TurmaDisciplina";
        odsDados.TypeName = "MSTech.GestaoEscolar.BLL.TUR_TurmaDisciplinaBO";
        odsDados.SelectMethod = "GetSelectBy_tur_id";
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectParameters.Add("tur_id", tur_id.ToString());
        odsDados.SelectParameters.Add("mostraFilhosRegencia", VS_MostraFilhosRegencia.ToString());
        odsDados.SelectParameters.Add("mostraRegencia", VS_MostraRegencia.ToString());
        odsDados.SelectParameters.Add("AppMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());
        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GetGlobalResourceObject("Mensagens","MSG_DISCIPLINA") + " --", "-1", true));
        ddlCombo.DataBind();

        int qtdItens = ddlCombo.Items.Count;
        if (qtdItens == 2)
            ddlCombo.SelectedValue = ddlCombo.Items[1].Value;
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

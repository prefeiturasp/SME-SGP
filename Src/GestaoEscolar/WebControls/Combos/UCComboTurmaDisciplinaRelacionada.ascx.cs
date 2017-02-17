using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboTurmaDisciplinaRelacionada : MotherUserControl
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
    /// Coloca na primeira linha a mensagem de selecione um item.
    /// </summary>
    public bool MostrarMensagemSelecione
    {
        set
        {
            if (value)
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione um(a) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " --", "-1", true));
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
    /// Carrega o combo com as disciplinas compartilhadas.
    /// </summary>
    /// <param name="tud_id">ID da disciplina de docencia compartilhada</param>
    public void CarregarDisciplinasCompartilhadas(long tud_id, long doc_id)
    {
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();
        odsDados.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.TUR_TurmaDisciplina";
        odsDados.TypeName = "MSTech.GestaoEscolar.BLL.TUR_TurmaDisciplinaBO";
        odsDados.SelectMethod = "SelectRelacionadaVigenteBy_DisciplinaCompartilhada";
        odsDados.SelectParameters.Add("tud_id", tud_id.ToString());
        odsDados.SelectParameters.Add("AppMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());
        odsDados.SelectParameters.Add("retornarComponentesRegencia", "false");
        odsDados.SelectParameters.Add("doc_id", doc_id.ToString());
        ddlCombo.DataBind();
        ddlCombo.SelectedIndex = 0;
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

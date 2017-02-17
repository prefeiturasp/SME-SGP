using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;

public partial class WebControls_Combos_UCComboTipoTurno : MotherUserControl
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
    /// Propriedade que seta o SelectedIndex do Combo.       
    /// </summary>
    public int SelectedIndex
    {
        set
        {
            ddlCombo.SelectedIndex = value;
        }
    }

    /// <summary>
    /// Propriedade que verifica quantos items existem no combo
    /// </summary>
    public int QuantidadeItensCombo
    {
        get
        {
            return ddlCombo.Items.Count;
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
    /// Propriedade que seta o Width do combo.   
    /// </summary>
    public Int32 WidthCombo
    {
        set
        {
            ddlCombo.Width = value;
        }
    }

    /// <summary>
    /// Propriedade visible da label do nome do combo
    /// </summary>
    public bool LabelVisible
    {
        set
        {
            lblTitulo.Visible = value;
        }
    }

    /// <summary>
    /// Retorna o texto selecionado no combo
    /// </summary>
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
    /// Adciona e remove a mensagem "Selecione um responsável" do dropdownlist.  
    /// Por padrão é false e a mensagem "Selecione um tipo de turno" não é exibida.
    /// </summary>
    public bool MostrarMessageSelecione
    {
        set
        {
            if ((value) && (ddlCombo.Items.FindByValue("-1") == null))
                ddlCombo.Items.Insert(0, new ListItem("-- Selecione um tipo de turno --", "-1", true));
            ddlCombo.AppendDataBoundItems = value;
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
    public void CarregarTipoTurno()
    {
        ddlCombo.Items.Clear();
        //odsDados.SelectParameters.Clear();
        ddlCombo.DataSource = ACA_TipoTurnoBO.SelecionaTipoTurno();
        MostrarMessageSelecione = true;
        //odsDados.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.ACA_TipoTurno";
        //odsDados.TypeName = "MSTech.GestaoEscolar.BLL.ACA_TipoTurnoBO";
        //odsDados.SelectMethod = "SelecionaTipoTurno";        
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Mostra os dados não excluídos logicamente no dropdownlist, filtrados por escola e curriculo
    /// </summary>
    public void CarregarTipoTurnoPorEscolaCurriculo(
            int cur_id
            , int crr_id
            , int esc_id
            , int uni_id
        )
    {
        ddlCombo.Items.Clear();
        //odsDados.SelectParameters.Clear();

        //odsDados.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.ACA_CurriculoTurno";
        //odsDados.TypeName = "MSTech.GestaoEscolar.BLL.ACA_CurriculoTurnoBO";
        //odsDados.SelectMethod = "SelectPorEscolas";

        ddlCombo.DataSource = ACA_CurriculoTurnoBO.SelectPorEscolas(cur_id, crr_id, esc_id, uni_id, false, 0, 0);

        //odsDados.SelectParameters.Add("cur_id", cur_id.ToString());
        //odsDados.SelectParameters.Add("crr_id", crr_id.ToString());
        //odsDados.SelectParameters.Add("esc_id", esc_id.ToString());
        //odsDados.SelectParameters.Add("uni_id", uni_id.ToString());
        //odsDados.SelectParameters.Add("paginado", "false");
        //odsDados.SelectParameters.Add("pageSize", "0");
        //odsDados.SelectParameters.Add("currentPage", "0");

        MostrarMessageSelecione = true;
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Mostra os dados não excluídos logicamente no dropdownlist    
    /// por escola
    /// </summary>
    [Obsolete("Não utilizar mais.", false)]
    public void CarregarTipoTurnoPorEscola
    (
        int cur_id
        , int esc_id
        , int uni_id
        , byte ttn_situacao
    )
    {
        //ddlCombo.Items.Clear();
        //odsDados.SelectParameters.Clear();

        //odsDados.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.ACA_CurriculoEscola";
        //odsDados.TypeName = "MSTech.GestaoEscolar.BLL.ACA_CurriculoEscolaBO";
        //odsDados.SelectMethod = "SelecionaTipoTurnoPorEscola";

        //odsDados.SelectParameters.Add("cur_id", cur_id.ToString());
        //odsDados.SelectParameters.Add("esc_id", esc_id.ToString());
        //odsDados.SelectParameters.Add("uni_Id", uni_id.ToString());
        //odsDados.SelectParameters.Add("ttn_situacao", ttn_situacao.ToString());

        //MostrarMessageSelecione = true;
        //ddlCombo.DataBind();
    }

    #endregion

    #region EVENTOS

    //protected void odsDados_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    //{
    //    if (e.Exception != null)
    //    {
    //        // Grava o erro e mostra pro usuário.
    //        ApplicationWEB._GravaErro(e.Exception.InnerException);

    //        e.ExceptionHandled = true;

    //        lblMessage.Text = "Erro ao tentar carregar " + lblTitulo.Text.Replace('*', ' ').ToLower() + ".";
    //        lblMessage.Visible = true;
    //    }
    //}

    #endregion
}

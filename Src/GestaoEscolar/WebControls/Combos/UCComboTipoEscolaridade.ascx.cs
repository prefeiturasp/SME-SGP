using System;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboTipoEscolaridade : MotherUserControl
{
    #region PROPRIEDADADES

    /// <summary>
    /// Atribui valores para o combo
    /// </summary>
    public DropDownList _Combo
    {
        get
        {
            return _ddlTipoEscolaridade;
        }
        set
        {
            _ddlTipoEscolaridade = value;
        }
    }

    /// <summary>
    /// Atribui valores para o label
    /// </summary>
    public Label _Label
    {
        get
        {
            return LabelTipoEscolaridade;
        }
        set
        {
            LabelTipoEscolaridade = value;
        }
    }

    /// <summary>
    /// Adciona e remove a mensagem "Selecione uma Escolaridade" do dropdownlist.  
    /// Por padrão é false e a mensagem "Selecione uma Escolaridade" não é exibida.
    /// </summary>
    public bool _MostrarMessageSelecione
    {
        set
        {
            if (value)
                this._ddlTipoEscolaridade.Items.Insert(0, new ListItem("-- Selecione uma escolaridade --", Guid.Empty.ToString(), true));
            this._ddlTipoEscolaridade.AppendDataBoundItems = value;
        }
    }

    /// <summary>
    /// Retorna o valor selecionado no combo.
    /// </summary>
    public Guid Valor
    {
        get
        {
            if (String.IsNullOrEmpty(_ddlTipoEscolaridade.SelectedValue))
                return Guid.Empty;

            return new Guid(_ddlTipoEscolaridade.SelectedValue);
        }
        set
        {
            _ddlTipoEscolaridade.SelectedValue = value.ToString();
        }
    }


    #endregion

    #region METODOS

    /// <summary>
    /// Mostra os tipos de escolaridade não excluídos logicamente no dropdownlist    
    /// </summary>
    public void _Load(byte tes_situacao)
    {
        try
        {
            this.odsTipoEscolaridade.SelectParameters.Clear();
            this.odsTipoEscolaridade.SelectParameters.Add("tes_id", Guid.Empty.ToString());
            this.odsTipoEscolaridade.SelectParameters.Add("tes_nome", string.Empty);
            this.odsTipoEscolaridade.SelectParameters.Add("tes_situacao", tes_situacao.ToString());
            this.odsTipoEscolaridade.SelectParameters.Add("paginado", "false");
            odsTipoEscolaridade.SelectParameters.Add("currentPage", "1");
            odsTipoEscolaridade.SelectParameters.Add("pageSize", "1");
            
            this.odsTipoEscolaridade.DataBind();
            _ddlTipoEscolaridade.DataBind();
        }
        catch
        {
            throw;
        }
    }

    #endregion

    #region EVENTOS
    protected void odsTipoEscolaridade_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            // Grava o erro e mostra pro usuário.
            ApplicationWEB._GravaErro(e.Exception.InnerException);

            //lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);

            e.ExceptionHandled = true;

            lblMessage.Text = "Erro ao tentar carregar " + _Label.Text.Replace('*', ' ').ToLower() + "."; 
            lblMessage.Visible = true;
        }
    }
    #endregion
}

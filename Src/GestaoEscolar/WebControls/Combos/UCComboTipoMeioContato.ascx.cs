using System;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboTipoMeioContato : MotherUserControl
{
    #region DELEGATES

    public delegate void onIndexChanged();
    public event onIndexChanged _IndexChanged;

    #endregion

    #region PROPRIEDADES

    /// <summary>
    /// Atribui valores para o combo
    /// </summary>
    public DropDownList _Combo
    {
        get
        {
            return _ddlTipoMeioContato;
        }
        set
        {
            _ddlTipoMeioContato = value;
        }
    }

    /// <summary>
    /// Atribui valores para o label
    /// </summary>
    public Label _Label
    {
        get
        {
            return LabelTipoMeioContato;
        }
        set
        {
            LabelTipoMeioContato = value;
        }
    }

    /// <summary>
    /// Adciona e remove a mensagem "Selecione um Tipo de Contato" do dropdownlist.  
    /// Por padrão é false e a mensagem "Selecione um Tipo de Contato" não é exibida.
    /// </summary>
    public bool _MostrarMessageSelecione
    {
        set
        {
            if (value)
                this._ddlTipoMeioContato.Items.Insert(0, new ListItem("-- Selecione um tipo de contato --", Guid.Empty.ToString(), true));
            this._ddlTipoMeioContato.AppendDataBoundItems = value;
        }
    }

    #endregion

    #region METODOS

    /// <summary>
    /// Mostra os tipos de meio de contatos não excluídos logicamente no dropdownlist    
    /// </summary>
    public void _Load(byte tmc_situacao)
    {
        this.odsTipoMeioContato.SelectParameters.Clear();
        this.odsTipoMeioContato.SelectParameters.Add("tmc_id", Guid.Empty.ToString());
        this.odsTipoMeioContato.SelectParameters.Add("tmc_nome", string.Empty);
        this.odsTipoMeioContato.SelectParameters.Add("tmc_situacao", tmc_situacao.ToString());
        this.odsTipoMeioContato.SelectParameters.Add("paginado", "false");
        this.odsTipoMeioContato.SelectParameters.Add("pageSize", "1");
        this.odsTipoMeioContato.SelectParameters.Add("currentPage", "1");

        this.odsTipoMeioContato.DataBind();
        _ddlTipoMeioContato.DataBind();
    }

    #endregion

    #region EVENTOS

    protected void _ddlTipoMeioContato_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_IndexChanged != null)
        {
            _IndexChanged();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        this._ddlTipoMeioContato.AutoPostBack = this._IndexChanged != null;
    }

    
    protected void odsTipoMeioContato_Selected(object sender, ObjectDataSourceStatusEventArgs e)
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

using System;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboTipoDocumento : MotherUserControl
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
            return _ddlTipoDocumentacao;
        }
        set
        {
            _ddlTipoDocumentacao = value;
        }
    }

    /// <summary>
    /// Atribui valores para o label
    /// </summary>
    public Label _Label
    {
        get
        {
            return LabelTipoDocumentacao;
        }
        set
        {
            LabelTipoDocumentacao = value;
        }
    }

    /// <summary>
    /// Adciona e remove a mensagem "Selecione um Tipo de Documento" do dropdownlist.  
    /// Por padrão é false e a mensagem "Selecione um Tipo de Documento" não é exibida.
    /// </summary>
    public bool _MostrarMessageSelecione
    {
        set
        {
            if (value)
                this._ddlTipoDocumentacao.Items.Insert(0, new ListItem("-- Selecione um tipo de documento --", Guid.Empty.ToString(), true));
            this._ddlTipoDocumentacao.AppendDataBoundItems = value;
        }
    }

    #endregion

    #region METODOS

    /// <summary>
    /// Mostra os tipos de documentação não excluídos logicamente no dropdownlist    
    /// </summary>
    public void _Load(Guid tdo_id, byte tdo_situacao)
    {
        try
        {
            this.odsTipoDocumentacao.SelectParameters.Clear();
            this.odsTipoDocumentacao.SelectParameters.Add("tdo_id", tdo_id.ToString());
            this.odsTipoDocumentacao.SelectParameters.Add("tdo_nome", string.Empty);
            this.odsTipoDocumentacao.SelectParameters.Add("tdo_sigla", string.Empty);
            this.odsTipoDocumentacao.SelectParameters.Add("tdo_situacao", tdo_situacao.ToString());
            this.odsTipoDocumentacao.SelectParameters.Add("paginado", "false");
            odsTipoDocumentacao.SelectParameters.Add("currentPage", "1");
            odsTipoDocumentacao.SelectParameters.Add("pageSize", "1");
            
            this.odsTipoDocumentacao.DataBind();
            _ddlTipoDocumentacao.DataBind();
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
        _ddlTipoDocumentacao.AutoPostBack = (_IndexChanged != null);
    }

    protected void _ddlTipoDocumentacao_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_IndexChanged != null)
        {
            _IndexChanged();
        }
    }    

    protected void odsTipoDocumentacao_Selected(object sender, ObjectDataSourceStatusEventArgs e)
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
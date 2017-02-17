using System;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboTipoDeficiencia : MotherUserControl
{
    #region DELEGATES

    public delegate void onIndexChanged();

    public event onIndexChanged _IndexChanged;

    #endregion DELEGATES

    #region PROPRIEDADES

    /// <summary>
    /// Atribui valores para o combo
    /// </summary>
    public DropDownList _Combo
    {
        get
        {
            return _ddlTipoDeficiencia;
        }
        set
        {
            _ddlTipoDeficiencia = value;
        }
    }

    /// <summary>
    /// Atribui valores para o label
    /// </summary>
    public Label _Label
    {
        get
        {
            return LabelTipoDeficiencia;
        }
        set
        {
            LabelTipoDeficiencia = value;
        }
    }

    /// <summary>
    /// Adciona e remove a mensagem "Selecione um Tipo de Deficiência" do dropdownlist.
    /// Por padrão é false e mensagem "Selecione um Tipo de Deficiência" não é exibida.
    /// </summary>
    public bool _MostrarMessageSelecione
    {
        set
        {
            if (value)
                this._ddlTipoDeficiencia.Items.Insert(0, new ListItem("-- Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoTipoDeficiencia(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " --", Guid.Empty.ToString(), true));
            this._ddlTipoDeficiencia.AppendDataBoundItems = value;
        }
    }

    /// <summary>
    /// Adciona e remove a mensagem "Todas" do dropdownlist.
    /// Por padrão é false e mensagem "Todas" não é exibida.
    /// </summary>
    public bool _MostrarMessageTodas
    {
        set
        {
            if (value)
                this._ddlTipoDeficiencia.Items.Insert(0, new ListItem("Todas", Guid.Empty.ToString(), true));
            this._ddlTipoDeficiencia.AppendDataBoundItems = value;
        }
    }

    /// <summary>
    /// Deixa o combo habilitado de acordo com o valor passado
    /// </summary>
    public bool PermiteEditar
    {
        get
        {
            return _ddlTipoDeficiencia.Enabled;
        }
        set
        {
            _ddlTipoDeficiencia.Enabled = value;
        }
    }

    /// <summary>
    /// Retorna e seta o valor selecionado no combo
    /// </summary>
    public Guid Valor
    {
        get
        {
            return string.IsNullOrEmpty(_ddlTipoDeficiencia.SelectedValue) ? Guid.Empty : new Guid(_ddlTipoDeficiencia.SelectedValue);
        }
        set
        {
            _ddlTipoDeficiencia.SelectedValue = value.ToString();
        }
    }

    #endregion PROPRIEDADES

    #region METODOS

    /// <summary>
    /// Mostra os tipos de deficiências não excluídos logicamente no dropdownlist
    /// </summary>
    public void _Load(Guid tde_id, byte tde_situacao)
    {
        try
        {
            this.odsTipoDeficiencia.SelectParameters.Clear();
            this.odsTipoDeficiencia.SelectParameters.Add("tde_id", tde_id.ToString());
            this.odsTipoDeficiencia.SelectParameters.Add("tde_nome", "");
            this.odsTipoDeficiencia.SelectParameters.Add("tde_situacao", tde_situacao.ToString());
            this.odsTipoDeficiencia.SelectParameters.Add("paginado", "false");
            odsTipoDeficiencia.SelectParameters.Add("currentPage", "1");
            odsTipoDeficiencia.SelectParameters.Add("pageSize", "1");

            _ddlTipoDeficiencia.DataBind();
        }
        catch
        {
            throw;
        }
    }

    #endregion METODOS

    #region EVENTOS

    protected void Page_PreRender(object sender, EventArgs e)
    {
        this._ddlTipoDeficiencia.AutoPostBack = this._IndexChanged != null;
    }

    protected void _ddlTipoDeficiencia_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_IndexChanged != null)
        {
            _IndexChanged();
        }
    }

    protected void odsTipoDeficiencia_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            // Grava o erro e mostra pro usuário.
            ApplicationWEB._GravaErro(e.Exception.InnerException);

            // lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);

            e.ExceptionHandled = true;

            lblMessage.Text = "Erro ao tentar carregar " + _Label.Text.Replace('*', ' ').ToLower() + ".";
            lblMessage.Visible = true;
        }
    }

    #endregion EVENTOS
}
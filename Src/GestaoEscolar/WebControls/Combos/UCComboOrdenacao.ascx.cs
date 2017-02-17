using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboOrdenacao : MotherUserControl
{
    #region Delegates

    public delegate void SelectedIndexChange();

    public event SelectedIndexChange _OnSelectedIndexChange;

    #endregion

    #region Propriedades

    public DropDownList _Combo
    {
        get
        {
            return _ddlOrdenacao;
        }
        set
        {
            _ddlOrdenacao = value;
        }
    }
    
    /// <summary>
    /// Seta o evento onChange de Javascript do combo.
    /// </summary>
    public string ComboClientID
    {
        get
        {
            return this._ddlOrdenacao.ClientID;
        }
    }

    #endregion

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string pacOrdenacaoAluno = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.ORDENACAO_COMBO_ALUNO
                , __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            if (!string.IsNullOrEmpty(pacOrdenacaoAluno))
                _ddlOrdenacao.SelectedValue = pacOrdenacaoAluno;
        }

        _ddlOrdenacao.AutoPostBack = _OnSelectedIndexChange != null;
    }

    protected void _ddlOrdenacao_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        if (this._OnSelectedIndexChange != null)
            this._OnSelectedIndexChange();
    }

    #endregion

    #region Métodos

    public void CarregaComboParametro()
    {
        string pacOrdenacaoAluno = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.ORDENACAO_COMBO_ALUNO
            , __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        if (!string.IsNullOrEmpty(pacOrdenacaoAluno))
            _ddlOrdenacao.SelectedValue = pacOrdenacaoAluno;
    }

    #endregion
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboConfiguracao : MotherUserControl
{
    #region DELEGATES

    public delegate void SelectedIndexChange();
    public event SelectedIndexChange _OnSelectedIndexChange;

    #endregion

    #region PROPRIEDADADES

    /// <summary>
    /// Atribui valores para o combo
    /// </summary>
    public DropDownList _Combo
    {
        get
        {
            return _ddlConfiguracao;
        }
        set
        {
            _ddlConfiguracao = value;
        }
    }

    /// <summary>
    /// Atribui valores para o label
    /// </summary>
    public Label _Label
    {
        get
        {
            return LabelConfiguracao;
        }
        set
        {
            LabelConfiguracao = value;
        }
    }

    /// <summary>
    /// Adciona e remove a mensagem "Selecione uma Função" do dropdownlist.  
    /// Por padrão é false e a mensagem "Selecione uma Função" não é exibida.
    /// </summary>
    public bool _MostrarMessageSelecione
    {
        set
        {
            if (value)
                this._ddlConfiguracao.Items.Insert(0, new ListItem("-- Selecione um processo de matrícula --", "-1", true));
            this._ddlConfiguracao.AppendDataBoundItems = value;
        }
    }

    #endregion

    #region METODOS

    /// <summary>
    /// Mostra as configurações não excluídas logicamente no dropdownlist.
    /// Filtra as configurações que possuem evt_id (evento) vigente referente a data atual.
    /// </summary>
    public void _LoadBy_Situacao_evt_id_Vigente(byte cfg_situacao)
    {
        try
        {
            this.odsConfiguracao.SelectMethod = "GetSelectBy_Situacao_evt_id_Vigente";
            this.odsConfiguracao.SelectParameters.Clear();
            this.odsConfiguracao.SelectParameters.Add("cfg_situacao", cfg_situacao.ToString());
            this.odsConfiguracao.SelectParameters.Add("paginado", "false");
            odsConfiguracao.SelectParameters.Add("currentPage", "1");
            odsConfiguracao.SelectParameters.Add("pageSize", "1");
            this.odsConfiguracao.DataBind();
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Mostra as configurações não excluídas logicamente no dropdownlist.
    /// Sem filtro de evt_id vigente referente a data atual.
    /// </summary>
    public void _LoadBy_Situacao(byte cfg_situacao)
    {
        try
        {
            this.odsConfiguracao.SelectMethod = "GetSelectBy_Situacao";
            this.odsConfiguracao.SelectParameters.Clear();
            this.odsConfiguracao.SelectParameters.Add("cfg_situacao", cfg_situacao.ToString());
            this.odsConfiguracao.SelectParameters.Add("paginado", "false");
            odsConfiguracao.SelectParameters.Add("currentPage", "1");
            odsConfiguracao.SelectParameters.Add("pageSize", "1");
            this.odsConfiguracao.DataBind();
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Mostra as configurações não excluídas logicamente no dropdownlist.
    /// Filtrado por configurações com  processo fechamento vigente e evt_id vigente referente a data atual.
    /// </summary>
    public void _LoadBy_ProcessoFechamento(byte cfg_situacao)
    {
        try
        {
            this.odsConfiguracao.SelectMethod = "GetSelectBy_ProcessoFechamento";
            this.odsConfiguracao.SelectParameters.Clear();
            this.odsConfiguracao.SelectParameters.Add("cfg_situacao", cfg_situacao.ToString());
            this.odsConfiguracao.SelectParameters.Add("paginado", "false");
            odsConfiguracao.SelectParameters.Add("currentPage", "1");
            odsConfiguracao.SelectParameters.Add("pageSize", "1");
            this.odsConfiguracao.DataBind();
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
        this._ddlConfiguracao.AutoPostBack = this._OnSelectedIndexChange != null;
    }

    /// <summary>
    /// Na mudança de um item no combo
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void _ddlConfiguracao_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this._OnSelectedIndexChange != null)
            this._OnSelectedIndexChange();
    }

    protected void odsConfiguracao_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["ent_id"] = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
        odsConfiguracao.Selected += odsConfiguracao_Selected;

    }

    
    protected void odsConfiguracao_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            // Grava o erro e mostra pro usuário.
            ApplicationWEB._GravaErro(e.Exception.InnerException);

            ///lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar configuração.", UtilBO.TipoMensagem.Erro);

            e.ExceptionHandled = true;
            lblMessage.Text = "Erro ao tentar carregar " + _Label.Text.Replace('*', ' ').ToLower() + "."; 
            lblMessage.Visible = true;
        }
    }
    
    #endregion
}

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboEscalaAvaliacaoParecer : MotherUserControl
{

    #region PROPRIEDADES

    /// <summary>
    /// Atribui valores para o combo
    /// </summary>
    public DropDownList _Combo
    {
        get
        {
            return _ddlEscalaParecer;
        }
        set
        {
            _ddlEscalaParecer = value;
        }
    }

    /// <summary>
    /// Atribui valores para o combo
    /// </summary>
    public CompareValidator _CPVParecer
    {
        get
        {
            return _cpvParecer;
        }
        set
        {
            _cpvParecer = value;
        }
    }
    
    public int _VS_isa_id
    {
        get
        {
            if (ViewState["_VS_isa_id"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_isa_id"]);
            }
            return -1;
        }
        set { ViewState["_VS_isa_id"] = value; }
    }

    /// <summary>
    /// Atribui valores para o label
    /// </summary>
    public Label _Label
    {
        get
        {
            return _lblEscalaAvaliacao;
        }
        set
        {
            _lblEscalaAvaliacao = value;
        }
    }

    public bool _MostrarMessageSelecione
    {
        set
        {
            if (value)
                this._ddlEscalaParecer.Items.Insert(0, new ListItem("-- Selecione uma opção --", "-1", true));
            this._ddlEscalaParecer.AppendDataBoundItems = value;
        }
    }

    #endregion

    #region EVENTOS

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void odsEscalaParecer_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["esa_id"] = _VS_isa_id;
    }

    protected void odsEscalaParecer_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            // Grava o erro e mostra pro usuário.
            ApplicationWEB._GravaErro(e.Exception.InnerException);

            // lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar parecer.", UtilBO.TipoMensagem.Erro);
            e.ExceptionHandled = true;
            lblMessage.Text = "Erro ao tentar carregar " + _Label.Text.Replace('*', ' ').ToLower() + ".";
            lblMessage.Visible = true;
        }
    }

    #endregion

    #region MÉTODOS

    public void AdicionaSemConceito(string conceito)
    {
        _ddlEscalaParecer.Items.Add(new ListItem(conceito, "0"));
    }

    #endregion
}

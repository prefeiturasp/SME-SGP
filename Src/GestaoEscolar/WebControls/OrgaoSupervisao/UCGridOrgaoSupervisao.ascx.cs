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
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;

public partial class WebControls_OrgaoSupervisao_UCGridOrgaoSupervisao : MotherUserControl
{
    #region DELEGATES

    public delegate void onExclui(int id);
    public event onExclui _Excluir;

    public void _ExcluirOrgaoSupervisao(int id)
    {
        if (_Excluir != null)
            _Excluir(id);
    }

    public delegate void onAlterar(int id);
    public event onAlterar _Alterar;

    public void _AlterarOrgaoSupervisao(int id)
    {
        if (_Alterar != null)
            _Alterar(id);
    }

    public delegate void onNovo();
    public event onNovo _Novo;

    public void _NovoOrgaoSupervisao()
    {
        if (_Novo != null)
            _Novo();
    }

    #endregion

    #region PROPRIEDADES

    /// <summary>
    /// Retorna e atribui valores para o grid.
    /// </summary>
    public GridView _grvOrgaoSupervisao
    {
        get
        {
            return this.grvOrgaoSupervisao;
        }
        set
        {
            this.grvOrgaoSupervisao = value;
        }
    }

    /// <summary>
    /// Retorna e atribui valores para o update panel.
    /// </summary>
    public UpdatePanel _updGridOrgaoSupervisao
    {
        get
        {
            return updGridOrgaoSupervisao;
        }
        set
        {
            updGridOrgaoSupervisao = value;
        }
    }

    /// <summary>
    /// Seta a propriedade Visible do botão de Novo.
    /// </summary>
    public bool VisibleNovo
    {
        set
        {
            _btnNovo.Visible = value;
        }
    }

    /// <summary>
    /// Seta a propriedade Visible do botão de Excluir.
    /// </summary>
    public bool VisibleExcluir
    {
        set
        {
            _grvOrgaoSupervisao.Columns[4].Visible = value;
        }
    }

    #endregion

    #region METODOS

    public void _CarregarOrgaoSupervisao(DataTable _OrgaoSupervisao)
    {
        this.grvOrgaoSupervisao.DataSource = _OrgaoSupervisao;
        this.grvOrgaoSupervisao.DataBind();
    }

    #endregion

    #region EVENTOS

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this.Page);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
        }
    }

    protected void grvOrgaoSupervisao_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            int index = int.Parse(e.CommandArgument.ToString());

            int id = Convert.ToInt32(grvOrgaoSupervisao.Rows[index].Cells[1].Text);

            _ExcluirOrgaoSupervisao(id);
        }
        if (e.CommandName == "Alterar")
        {
            int index = int.Parse(e.CommandArgument.ToString());

            int id = Convert.ToInt32(grvOrgaoSupervisao.Rows[index].Cells[1].Text);            

            _AlterarOrgaoSupervisao(id);
        }
    }

    protected void grvOrgaoSupervisao_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton btnDelete = new ImageButton();
            btnDelete = (ImageButton)e.Row.FindControl("_btnExcluir");
            if (btnDelete != null)
            {
                btnDelete.CommandArgument = e.Row.RowIndex.ToString();
            }

            LinkButton btnAlterar = new LinkButton();
            btnAlterar = (LinkButton)e.Row.FindControl("_btnAlterar");
            if (btnAlterar != null)
            {
                btnAlterar.CommandArgument = e.Row.RowIndex.ToString();
            }
        }
    }

    protected void _btnNovo_Click(object sender, EventArgs e)
    {
        _NovoOrgaoSupervisao();
    }

    #endregion
}

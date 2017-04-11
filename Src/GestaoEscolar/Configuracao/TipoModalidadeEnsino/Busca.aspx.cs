using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.Validation.Exceptions;
using System.Web;
using System.Collections.Generic;
using System.Linq;

public partial class Configuracao_TipoModalidadeEnsino_Busca : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Id da modalidade de ensino superior
    /// </summary>
    private int VS_tme_idSuperior
    {
        get
        {
            if (ViewState["VS_tme_idSuperior"] == null)
            {
                ViewState["VS_tme_idSuperior"] = -1;
            }

            return Convert.ToInt32(ViewState["VS_tme_idSuperior"]);
        }

        set
        {
            ViewState["VS_tme_idSuperior"] = value;
        }
    }

    #endregion Propriedades

    #region Eventos Page Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
        }

        UCComboQtdePaginacao1.GridViewRelacionado = _dgvTipoModalidadeEnsino;

        if (!IsPostBack)
        {
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
                _lblMessage.Text = message;

            btnVoltar.Visible = false;
            if (Session["lstTmeIds"] != null)
            {
                Stack<int> lstTmeIds = (Stack<int>)Session["lstTmeIds"];
                btnVoltar.Visible = lstTmeIds.Any();
                VS_tme_idSuperior = lstTmeIds.Any() ? lstTmeIds.Peek() : -1;
            }

            // quantidade de itens por página
            string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
            int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

            // mostra essa quantidade no combobox
            UCComboQtdePaginacao1.Valor = itensPagina;
            // atribui essa quantidade para o grid
            _dgvTipoModalidadeEnsino.PageSize = itensPagina;

            _odsTipoModalidadeEnsino.SelectParameters.Clear();
            _odsTipoModalidadeEnsino.SelectParameters.Add("tme_idSuperior", VS_tme_idSuperior.ToString());

            // atualiza o grid
            _dgvTipoModalidadeEnsino.DataBind();
        }
    }

    #endregion

    #region Delegates

    protected void UCComboQtdePaginacao_IndexChanged()
    {
        // atribui nova quantidade itens por página para o grid
        _dgvTipoModalidadeEnsino.PageSize = UCComboQtdePaginacao1.Valor;
        _dgvTipoModalidadeEnsino.PageIndex = 0;

        _odsTipoModalidadeEnsino.SelectParameters.Clear();
        _odsTipoModalidadeEnsino.SelectParameters.Add("tme_idSuperior", VS_tme_idSuperior.ToString());

        // atualiza o grid
        _dgvTipoModalidadeEnsino.DataBind();
    }

    #endregion
    
    #region Eventos

    protected void _odsTipoModalidadeEnsino_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (e.ExecutingSelectCount)
            e.InputParameters.Clear();
    }

    #endregion

    protected void btnDetalhar_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btnDetalhar = sender as ImageButton;
        GridViewRow row = btnDetalhar.NamingContainer as GridViewRow;

        Stack<int> lstTmeIds = new Stack<int>();

        if (Session["lstTmeIds"] != null)
        {
            lstTmeIds = (Stack<int>)Session["lstTmeIds"];
        }

        lstTmeIds.Push(Convert.ToInt32(_dgvTipoModalidadeEnsino.DataKeys[row.RowIndex].Value));
        Session["lstTmeIds"] = lstTmeIds;
        RedirecionarPagina("Busca.aspx");
    }

    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        if (Session["lstTmeIds"] != null)
        {
            Stack<int> lstTmeIds = new Stack<int>();
            lstTmeIds = (Stack<int>)Session["lstTmeIds"];
            if (lstTmeIds.Any())
            {
                lstTmeIds.Pop();
                Session["lstTmeIds"] = lstTmeIds;
            }
        }
        RedirecionarPagina("Busca.aspx");
    }
}

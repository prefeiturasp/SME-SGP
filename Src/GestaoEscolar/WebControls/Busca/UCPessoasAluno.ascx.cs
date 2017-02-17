using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

public partial class WebControls_Busca_UCPessoasAluno : MotherUserControl
{
    protected IDictionary<string, object> returns = new Dictionary<string, object>();

    #region Propriedades

    /// <summary>
    /// Valores retornados pela busca.
    /// </summary>
    public IDictionary<string, object> Returns
    {
        get { return returns; }
    }

    public int Paginacao { get; set; }

    /// <summary>
    /// Nome do container onde foi colocado a busca.
    /// </summary>
    public string ContainerName { get; set; }

    /// <summary>
    /// Check se o é uma post back assincrono ou não (Ajax ou não).
    /// </summary>
    protected bool IsAsyncPostBack
    {
        get
        {
            bool retorno = false;
            var sm = ScriptManager.GetCurrent(Page);
            if (sm != null)
                retorno = sm.IsInAsyncPostBack;
            return retorno;
        }
    }

    public DelReturnValues ReturnValues { get; set; }

    #endregion

    #region Eventos Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            fdsResultados.Visible = false;

        pnlPesquisar.DefaultButton = _btnPesquisar.ID;
        _txtNome.Focus();
    }

    #endregion

    #region Delegates

    public delegate void DelReturnValues(IDictionary<string, object> parameters);

    #endregion

    #region Métodos

    public void _Limpar()
    {
        _txtNome.Text = string.Empty;
        _txtCPF.Text = string.Empty;
        _txtRG.Text = string.Empty;
        fdsResultados.Visible = false;

        Page.Form.DefaultFocus = _txtNome.ClientID;
        Page.Form.DefaultButton = _btnPesquisar.UniqueID;
    }

    private void Pesquisar()
    {
        try
        {
            odsPessoas.SelectParameters.Clear();

            bool consultaAlunos = chkBuscaAlunos.Checked;
            odsPessoas.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
            odsPessoas.SelectParameters.Add("nome", _txtNome.Text);
            odsPessoas.SelectParameters.Add("cpf", _txtCPF.Text);
            odsPessoas.SelectParameters.Add("rg", _txtRG.Text);
            odsPessoas.SelectParameters.Add("nis", _txtNIS.Text);
            odsPessoas.SelectParameters.Add("consultaAlunos", consultaAlunos.ToString());

            //odsPessoas.DataBind();

            _dgvPessoas.DataBind();

        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar pesquisar pessoa.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Fecha o popup da consulta.
    /// </summary>
    public void Close()
    {
        if (this.IsAsyncPostBack)
            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "CloseDialog", String.Format("$(\"#{0}\").dialog(\"close\");", this.ContainerName), true);
        else
        {
            if (!Page.ClientScript.IsClientScriptBlockRegistered("CloseDialog"))
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "CloseDialog", String.Format("$(\"#{0}\").dialog(\"close\");", this.ContainerName), true);
        }
    }

    protected void SetReturns(string key, object value)
    {
        this.Returns.Add(key, value);
    }

    private bool ValidaCampos()
    {
        bool camposPreenchidos = false;
        camposPreenchidos = !String.IsNullOrEmpty(_txtNome.Text) || !String.IsNullOrEmpty(_txtCPF.Text) || !String.IsNullOrEmpty(_txtRG.Text) || !String.IsNullOrEmpty(_txtNIS.Text);

        if (!camposPreenchidos)
            _lblMessage.Text = UtilBO.GetErroMessage("É necessário selecionar/preencher completamente pelo menos um filtro para realizar a pesquisa.", UtilBO.TipoMensagem.Alerta);

        return camposPreenchidos;
    }

    #endregion

    #region Eventos

    protected void _btnPesquisar_Click(object sender, EventArgs e)
    {
        if (ValidaCampos())
        {
            Pesquisar();
            fdsResultados.Visible = true;
        }
    }

    protected void odsPessoas_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (e.ExecutingSelectCount)
            e.InputParameters.Clear();
    }

    protected void _dgvPessoas_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            SetReturns(_dgvPessoas.DataKeyNames[0], _dgvPessoas.DataKeys[e.NewSelectedIndex].Values["pes_id"]);
            SetReturns(_dgvPessoas.DataKeyNames[1], _dgvPessoas.DataKeys[e.NewSelectedIndex].Values["pes_nome"]);
            if (ReturnValues != null)
                ReturnValues(Returns);
            else
                throw new NotImplementedException();
            if (!String.IsNullOrEmpty(ContainerName))
                    Close();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar selecionar pessoa.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion

    #region Inicialize

    public void Inicialize()
    {
        this.Inicialize(0, String.Empty);
    }

    public void Inicialize(int paginacao)
    {
        this.Inicialize(paginacao, String.Empty);
    }

    public void Inicialize(int paginacao, string containerName)
    {
        this.Paginacao = paginacao;
        this.ContainerName = containerName;
    }

    #endregion
}

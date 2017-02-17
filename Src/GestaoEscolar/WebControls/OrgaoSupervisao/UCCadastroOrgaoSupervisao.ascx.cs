using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.UserControlLibrary.Combos;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.Validation.Exceptions;

public partial class WebControls_OrgaoSupervisao_UCCadastroOrgaoSupervisao : MotherUserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                UCComboEntidade1._Label.Text += " *";
                string tua_id = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_UNIDADE_ADMINISTRATIVA_ESCOLA
                    , __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                if (!String.IsNullOrEmpty(tua_id))
                    _VS_tua_idEscola = new Guid(tua_id);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        UCComboEntidade1.OnSelectedIndexChange = UCComboEntidade1__IndexChanged;
    }

    #region DELEGATES

    public delegate void onInclui(DataTable dt);
    public event onInclui _Incluir;

    public void _IncluirOrgaoSupervisao(DataTable dt)
    {
        if (_Incluir != null)
            _Incluir(_VS_OrgaoSupervisao);
    }

    public delegate void onSeleciona();
    public event onSeleciona _Selecionar;

    public void _SelecionarUA()
    {
        if (_Selecionar != null)
            _Selecionar();
    }

    #endregion

    #region PROPRIEDADES

    /// <summary>
    /// Armazena o campo _VS_tua_idEscola
    /// </summary>
    public Guid _VS_tua_idEscola
    {
        get
        {
            if (ViewState["_VS_tua_idEscola"] != null)
                return new Guid(ViewState["_VS_tua_idEscola"].ToString());
            return Guid.Empty;
        }
        set
        {
            ViewState["_VS_tua_idEscola"] = value;
        }
    }

    /// <summary>
    /// ViewState com datatable de Orgãos de Supervisão
    /// Retorno e atribui valores para o DataTable de Orgãos de Supervisão
    /// </summary>
    public DataTable _VS_OrgaoSupervisao
    {
        get
        {
            if (ViewState["_VS_OrgaoSupervisao"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("esc_id");
                dt.Columns.Add("eos_id");
                dt.Columns.Add("ent_id");
                dt.Columns.Add("uad_id");
                dt.Columns.Add("eos_nome");
                dt.Columns.Add("uad_nome");
                dt.Columns.Add("orgao");
                dt.Columns.Add("data");

                ViewState["_VS_OrgaoSupervisao"] = dt;
            }
            return (DataTable)ViewState["_VS_OrgaoSupervisao"];
        }
        set
        {
            ViewState["_VS_OrgaoSupervisao"] = value;
        }
    }

    /// <summary>    
    /// Retorno e atribui valores para o TextBox Descrição
    /// </summary>
    public TextBox _txtDescricao
    {
        get
        {
            return txtDescricao;
        }
        set
        {
            txtDescricao = value;
        }
    }

    ///<summary>
    ///Atribui valores para o combo Entidade
    ///</summary>
    public DropDownList _ComboEntidade
    {
        get
        {
            return UCComboEntidade1._Combo;
        }
        set
        {
            UCComboEntidade1._Combo = value;
        }
    }

    /// <summary>    
    /// Retorno e atribui valores para o TextBox UA
    /// </summary>
    public TextBox _txtUA
    {
        get
        {
            return txtUA;
        }
        set
        {
            txtUA = value;
        }
    }

    /// <summary>    
    /// Retorno e atribui valores para o Button Incluir
    /// </summary>
    public Button _Botao
    {
        get
        {
            return _btnIncluir;
        }
        set
        {
            _btnIncluir = value;
        }
    }

    /// <summary>
    /// Retorna e atribui valores para o Button Pesquisar UA
    /// </summary>
    public ImageButton _BotaoPesquisaUA
    {
        get
        {
            return _btnPesquisarUA;
        }
        set
        {
            _btnPesquisarUA = value;
        }
    }

    /// <summary>
    /// Retorna e atribui valores para o update panel.
    /// </summary>
    public UpdatePanel _updCadastroOrgaoSupervisao
    {
        get
        {
            return updCadastroOrgaoSupervisao;
        }
        set
        {
            updCadastroOrgaoSupervisao = value;
        }
    }

    /// <summary>
    /// Armazena o campo esc_id
    /// </summary>
    public int _VS_esc_id
    {
        get
        {
            if (ViewState["_VS_esc_id"] != null)
                return Convert.ToInt32(ViewState["_VS_esc_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_esc_id"] = value;
        }
    }

    /// <summary>
    /// Armazena o id da UA
    /// </summary>
    public Guid _VS_uad_id
    {
        get
        {
            if (ViewState["_VS_uad_id"] != null)
                return new Guid(ViewState["_VS_uad_id"].ToString());
            else
                return Guid.Empty;
        }
        set
        {
            ViewState["_VS_uad_id"] = value;
        }
    }

    /// <summary>
    /// Indica se é uma alteração ou inclusão de contato
    /// </summary>
    public bool _VS_IsNew
    {
        get
        {
            return Convert.ToBoolean(ViewState["_VS_IsNew"]);
        }
        set
        {
            ViewState["_VS_IsNew"] = value;
        }
    }

    /// <summary>
    /// Armazena o campo eos_id
    /// </summary>
    public int _VS_id
    {
        get
        {
            if (ViewState["_VS_id"] != null)
                return Convert.ToInt32(ViewState["_VS_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_id"] = value;
        }
    }

    /// <summary>
    /// Armazena o sequencial para inclusão no DataTable
    /// </summary>
    public int _VS_seq
    {
        get
        {
            if (ViewState["_VS_seq"] != null)
                return Convert.ToInt32(ViewState["_VS_seq"]);
            return -1;
        }
        set
        {
            ViewState["_VS_seq"] = value;
        }
    }

    #endregion

    #region METODOS

    protected void UCComboEntidade1__IndexChanged(object sender, EventArgs e)
    {
        if (UCComboEntidade1._Combo.SelectedValue == Guid.Empty.ToString())
            _btnPesquisarUA.Enabled = false;
        else
            _btnPesquisarUA.Enabled = true;

        _btnPesquisarUA.Focus();
        txtUA.Text = string.Empty;
        _VS_uad_id = Guid.Empty;
    }

    public void _CarregarComboEntidade(bool MostraSelecione)
    {
        UCComboEntidade1.Inicialize(true
                    , "Entidade"
                    , String.Format("Entidade é obrigatório.")
                    , "*"
                    , String.Empty
                    , true
                    , new UCComboSelectItemMessage("-- Selecione uma entidade --", "-1")
                    , true
                    , new UCComboItemNotFoundMessage("-- Selecione uma entidade --", "-1"));
        UCComboEntidade1._EnableValidator = true;
        UCComboEntidade1._ValidationGroup = "OrgaoSupervisao";
        UCComboEntidade1._Load(Guid.Empty, 0);
    }

    public void _LimparCampos()
    {
        _VS_IsNew = true;
        _btnIncluir.Text = "Incluir";
        txtDescricao.Text = string.Empty;
        txtUA.Text = string.Empty;
        _VS_uad_id = Guid.Empty;
        UCComboEntidade1._Combo.SelectedValue = Guid.Empty.ToString();
        _btnPesquisarUA.Enabled = false;
        _lblMessage.Visible = false;
    }

    private bool _Validar()
    {
        if (String.IsNullOrEmpty(txtDescricao.Text.Trim()))
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Descrição é obrigatório.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        if ((UCComboEntidade1._Combo.SelectedValue == "-1") || (UCComboEntidade1._Combo.SelectedValue == null))
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Entidade é obrigatório.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        _lblMessage.Visible = false;
        return true;
    }

    private void _IncluirOrgaoSupervisao()
    {

        DataRow dr = _VS_OrgaoSupervisao.NewRow();

        if (_VS_seq == -1)
            _VS_seq = 1;
        else
            _VS_seq = _VS_seq + 1;

        dr["esc_id"] = _VS_esc_id;
        dr["eos_id"] = _VS_seq;
        dr["ent_id"] = UCComboEntidade1._Combo.SelectedValue;
        dr["uad_id"] = _VS_uad_id;
        dr["uad_nome"] = txtUA.Text;
        dr["eos_nome"] = txtDescricao.Text;
        dr["orgao"] = String.IsNullOrEmpty(txtUA.Text) ? UCComboEntidade1._Combo.SelectedItem.ToString() : UCComboEntidade1._Combo.SelectedItem + " / " + _txtUA.Text;
        dr["data"] = DateTime.Now;

        if (ValidaOrgaoSupervisaoExistente(dr, -1))
        {
            _VS_OrgaoSupervisao.Rows.Add(dr);
            _IncluirOrgaoSupervisao(_VS_OrgaoSupervisao);

        }


    }

    private bool ValidaOrgaoSupervisaoExistente(DataRow dr, int id)
    {
        foreach (DataRow drViewState in _VS_OrgaoSupervisao.Rows)
        {
            if (drViewState["eos_id"].ToString() != Convert.ToString(id))
            {
                if (drViewState[0].ToString() == dr[0].ToString() &&
                   drViewState[2].ToString() == dr[2].ToString() &&
                   drViewState[3].ToString() == dr[3].ToString() &&
                   drViewState[4].ToString() == dr[4].ToString() &&
                   drViewState[5].ToString() == dr[5].ToString() &&
                   drViewState[6].ToString() == dr[6].ToString())
                {
                    throw new ValidationException("Orgão de supervisão já cadastrado.");

                }
            }
        }
        return true;
    }

    private void _AlterarOrgaoSupervisao(int id)
    {

        int i;
        for (i = 0; i < _VS_OrgaoSupervisao.Rows.Count; i++)
        {
            if (_VS_OrgaoSupervisao.Rows[i].RowState != DataRowState.Deleted)
            {
                if (_VS_OrgaoSupervisao.Rows[i]["eos_id"].ToString() == Convert.ToString(id))
                {
                    _VS_OrgaoSupervisao.Rows[i]["ent_id"] = UCComboEntidade1._Combo.SelectedValue;
                    _VS_OrgaoSupervisao.Rows[i]["uad_id"] = _VS_uad_id;
                    _VS_OrgaoSupervisao.Rows[i]["eos_nome"] = txtDescricao.Text;
                    _VS_OrgaoSupervisao.Rows[i]["uad_nome"] = txtUA.Text;
                    _VS_OrgaoSupervisao.Rows[i]["orgao"] = String.IsNullOrEmpty(txtUA.Text) ? UCComboEntidade1._Combo.SelectedItem.ToString() : UCComboEntidade1._Combo.SelectedItem + " / " + _txtUA.Text;
                    if (ValidaOrgaoSupervisaoExistente(_VS_OrgaoSupervisao.Rows[i], id))
                    {
                        _Incluir(_VS_OrgaoSupervisao);
                    }

                }
            }
        }

    }

    #endregion

    #region EVENTOS

    protected void _btnIncluir_Click(object sender, EventArgs e)
    {
        if (_Validar())
        {
            try
            {
                if (_VS_IsNew)
                    _IncluirOrgaoSupervisao();
                else
                    _AlterarOrgaoSupervisao(_VS_id);

                _LimparCampos();
            }
            catch (Exception ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                _lblMessage.Visible = true;
                updCadastroOrgaoSupervisao.Update();
            }
        }
        else
        {
            _lblMessage.Visible = true;
            updCadastroOrgaoSupervisao.Update();
        }
    }

    protected void _btnPesquisarUA_Click(object sender, ImageClickEventArgs e)
    {
        _SelecionarUA();
    }

    #endregion
}

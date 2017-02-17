using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Funcao_UCCadastroFuncao : MotherUserControl
{
    #region DELEGATES

    public delegate void onInclui(DataTable dt);
    public event onInclui _Incluir;

    public void _IncluirFuncao(DataTable dt)
    {
        if (_Incluir != null)
        {
            _Incluir(_VS_funcoes);
        }
    }

    public delegate void onSeleciona();
    public event onSeleciona _Selecionar;

    public void _SelecionarUA()
    {
        if (_Selecionar != null)
        {
            _Selecionar();
        }
    }

    #endregion

    #region PROPRIEDADES

    /// <summary>
    /// ViewState com datatable de funções
    /// Retorno e atribui valores para o DataTable de funções
    /// </summary>
    public DataTable _VS_funcoes
    {
        get
        {
            if (ViewState["_VS_funcoes"] == null)
            {
                DataTable dt = new DataTable();                
                dt.Columns.Add("crg_id");
                dt.Columns.Add("seqcrg_id");
                dt.Columns.Add("fun_id");
                dt.Columns.Add("seqfun_id");
                dt.Columns.Add("uad_id");
                dt.Columns.Add("situacao_id");
                dt.Columns.Add("vigenciaini");
                dt.Columns.Add("vigenciafim");
                dt.Columns.Add("coc_matricula");
                dt.Columns.Add("cof_responsavelUA");
                dt.Columns.Add("cargofuncao");
                dt.Columns.Add("uad_nome");
                dt.Columns.Add("vigencia");
                dt.Columns.Add("situacao");
                dt.Columns.Add("observacao");
                dt.Columns.Add("chr_id");
                dt.Columns.Add("coc_vinculoSede");
                dt.Columns.Add("coc_vinculoExtra");
                dt.Columns.Add("coc_dataInicioMatricula");
                dt.Columns.Add("coc_readaptado");
                dt.Columns.Add("coc_complementacaoCargaHoraria");
                dt.Columns.Add("controladoIntegracao");

                ViewState["_VS_funcoes"] = dt;
            }

            return (DataTable)ViewState["_VS_funcoes"];
        }

        set
        {
            ViewState["_VS_funcoes"] = value;
        }
    }

    /// <summary>
    /// Atribui valores para o combo Função
    /// </summary>
    public int ComboFuncaoValor
    {
        get
        {
            return UCComboFuncao1.Valor;
        }

        set
        {
            UCComboFuncao1.Valor = value;
        }
    }

    /// <summary>
    /// Atribui valores para o combo Função
    /// </summary>
    public bool ComboFuncaoPermiteEditar
    {
        set
        {
            UCComboFuncao1.PermiteEditar = value;
        }
    }

    /// <summary>    
    /// Retorno e atribui valores para o TextBox Matricula
    /// </summary>
    public TextBox _txtMatricula
    {
        get
        {
            return txtMatricula;
        }

        set
        {
            txtMatricula = value;
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
    /// Retorno e atribui valores para o CheckBox Responsável UA
    /// </summary>
    public CheckBox _chkResponsavelUA
    {
        get
        {
            return chkResponsavelUA;
        }

        set
        {
            chkResponsavelUA = value;
        }
    }

    /// <summary>    
    /// Retorno e atribui valores para o TextBox Vigencia Inicial
    /// </summary>
    public TextBox _txtVigenciaIni
    {
        get
        {
            return txtVigenciaIni;
        }

        set
        {
            txtVigenciaIni = value;
        }
    }

    /// <summary>    
    /// Retorno e atribui valores para o TextBox Vigencia Final
    /// </summary>
    public TextBox _txtVigenciaFim
    {
        get
        {
            return txtVigenciaFim;
        }

        set
        {
            txtVigenciaFim = value;
        }
    }

    /// <summary>
    /// Retorna e seta o valor selecionado no combo de situação
    /// </summary>
    public int ValorSituacao
    {
        get
        {
            return Convert.ToInt32(ddlFuncaoSituacao.SelectedValue);
        }

        set
        {
            ddlFuncaoSituacao.SelectedValue = value.ToString();
        }
    }

    /// <summary>    
    /// Retorno e atribui valores para o TextBox Observacao
    /// </summary>
    public TextBox _txtObservacao
    {
        get
        {
            return txtObservacao;
        }

        set
        {
            txtObservacao = value;
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
    /// Retorno e atribui valores para o Button Cancelar
    /// </summary>
    public Button _BotaoCancelar
    {
        get
        {
            return _btnCancelar;
        }

        set
        {
            _btnCancelar = value;
        }
    }

    /// <summary>    
    /// Retorno e atribui valores para o Button Pesquisar UA
    /// </summary>
    public ImageButton _btnPesquisarUA
    {
        get
        {
            return btnPesquisarUA;
        }

        set
        {
            btnPesquisarUA = value;
        }
    }

    /// <summary>
    /// Retorna e atribui valores para o update panel.
    /// </summary>
    public UpdatePanel _updCadastroFuncao
    {
        get
        {
            return updCadastroFuncoes;
        }

        set
        {
            updCadastroFuncoes = value;
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
            {
                return new Guid(ViewState["_VS_uad_id"].ToString());
            }
            
            return Guid.Empty;
        }

        set
        {
            ViewState["_VS_uad_id"] = value;
        }
    }

    /// <summary>
    /// Indica se é uma alteração ou inclusão de função
    /// </summary>
    public bool _VS_IsNew
    {
        get
        {
            return Convert.ToBoolean(ViewState["_VS_IsNew"]);
        }

        set
        {
            if (!value)
            {
                divCadastro.Visible = true;
            }

            ViewState["_VS_IsNew"] = value;
        }
    }

    /// <summary>
    /// Armazena o id do grid
    /// </summary>
    public int _VS_id
    {
        get
        {
            if (ViewState["_VS_id"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_id"]);
            }

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
            {
                return Convert.ToInt32(ViewState["_VS_seq"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_seq"] = value;
        }
    }

    /// <summary>
    /// Armazena a flag controladoIntegracao para alteração no DataTable.
    /// </summary>
    public bool VS_cof_controladoIntegracao
    {
        get
        {
            if (ViewState["VS_cof_controladoIntegracao"] != null)
            {
                return Convert.ToBoolean(ViewState["VS_cof_controladoIntegracao"]);
            }

            return false;
        }

        set
        {
            ViewState["VS_cof_controladoIntegracao"] = value;
        }
    }

    /// <summary>
    /// Armazena a data de admissão do colaborador
    /// </summary>
    public DateTime _VS_dataAdmissao
    {
        get
        {
            if (ViewState["_VS_dataAdmissao"] != null)
            {
                return Convert.ToDateTime(ViewState["_VS_dataAdmissao"]);
            }

            return new DateTime();
        }

        set
        {
            ViewState["_VS_dataAdmissao"] = value;
        }
    }

    /// <summary>
    /// Armazena a data de demissão do colaborador
    /// </summary>
    public DateTime _VS_dataDemissao
    {
        get
        {
            if (ViewState["_VS_dataDemissao"] != null)
            {
                return Convert.ToDateTime(ViewState["_VS_dataDemissao"]);
            }

            return new DateTime();
        }

        set
        {
            ViewState["_VS_dataDemissao"] = value;
        }
    }

    /// <summary>
    /// Adiciona a classe css ao botao Incluir
    /// </summary>
    public string AdicionaClasseCss
    {
        set
        {
            if (!Convert.ToString(_btnIncluir.CssClass).Contains(value))
            {
                _btnIncluir.CssClass += " " + value;
            }
        }
    }

    /// <summary>
    /// Guarda o ID do colaborador.
    /// </summary>
    public long VS_col_id
    {
        get
        {
            if (ViewState["VS_col_id"] != null)
            {
                return Convert.ToInt64(ViewState["VS_col_id"]);
            }

            return 0;
        }

        set
        {
            ViewState["VS_col_id"] = value;
        }
    }

    #endregion

    #region METODOS

    /// <summary>
    /// Carrega o combo de funções.
    /// </summary>
    public void ComboFuncaoCarregar()
    {
        UCComboFuncao1.CarregarFuncao();
    }

    /// <summary>
    /// Seta o foco no combo de funções.
    /// </summary>
    public void ComboFuncaoSetarFoco()
    {
        UCComboFuncao1.SetarFoco();
    }

    /// <summary>
    /// Reinicializa os campos de cadastro da função.
    /// </summary>
    public void _LimparCampos()
    {
        divCadastro.Visible = true;
        _VS_IsNew = true;
        _VS_id = 0;
        _VS_uad_id = Guid.Empty;
        HabilitaControles(Controls, true);
        _btnIncluir.Text = "Incluir";
        _btnIncluir.Visible = true;
        _btnCancelar.Text = "Cancelar";
        _lblMessage.Visible = false;

        UCComboFuncao1.Valor = -1;        
        txtUA.Text = string.Empty;
        btnPesquisarUA.Enabled = true;
        chkResponsavelUA.Checked = false;
        txtMatricula.Text = string.Empty;
        txtVigenciaIni.Text = string.Empty;
        txtVigenciaFim.Text = string.Empty;
        txtObservacao.Text = string.Empty;
        ddlFuncaoSituacao.SelectedValue = "-1";
        rfvMatricula.Visible = false;
        LabelMatricula.Text = "Matrícula";

        UCComboFuncao1.PermiteEditar = true;
    }

    /// <summary>
    /// Valida os dados da nova função ou da função sendo editada.
    /// </summary>
    /// <returns></returns>
    private bool _Validar()
    {
        if (UCComboFuncao1.Valor <= 0)
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Função é obrigatório.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        if (String.IsNullOrEmpty(txtUA.Text.Trim()))
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Unidade adminsitrativa é obrigatório.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        if (String.IsNullOrEmpty(txtVigenciaIni.Text.Trim()))
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Vigência inicial é obrigatório.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        if (RHU_ColaboradorBO.VerificaMatriculaExistente(VS_col_id, txtMatricula.Text, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Outro docente/colaborador já possui um vínculo com esta matrícula.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        if (ExisteFuncaoComMesmasInformacoes())
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Já existe um vínculo com as mesmas informações.", UtilBO.TipoMensagem.Alerta);
            return false;            
        }

        cvVigenciaIni.Validate();
        if (!cvVigenciaIni.IsValid)
        {
            return false;
        }        

        if (!String.IsNullOrEmpty(txtVigenciaFim.Text.Trim()))
        {
            cvVigenciaFim.Validate();
            if (!cvVigenciaFim.IsValid)
            {
                return false;
            }

            if (Convert.ToDateTime(txtVigenciaIni.Text) > Convert.ToDateTime(txtVigenciaFim.Text))
            {
                _lblMessage.Text = UtilBO.GetErroMessage("Vigência inicial não pode ser maior que a vigência final.", UtilBO.TipoMensagem.Alerta);
                return false;
            }
        }

        if (_VS_dataAdmissao != new DateTime())
        {
            if (Convert.ToDateTime(txtVigenciaIni.Text) < _VS_dataAdmissao)
            {
                _lblMessage.Text = UtilBO.GetErroMessage("Vigência inicial não pode ser menor que a data de admissão.", UtilBO.TipoMensagem.Alerta);
                return false;
            }

            if (!string.IsNullOrEmpty(txtVigenciaFim.Text))
            {
                if (Convert.ToDateTime(txtVigenciaFim.Text) < _VS_dataAdmissao)
                {
                    _lblMessage.Text = UtilBO.GetErroMessage("Vigência final não pode ser menor que a data de admissão.", UtilBO.TipoMensagem.Alerta);
                    return false;
                }
            }
        }

        if (_VS_dataDemissao != new DateTime())
        {
            if (Convert.ToDateTime(txtVigenciaIni.Text) > _VS_dataDemissao)
            {
                _lblMessage.Text = UtilBO.GetErroMessage("Vigência inicial não pode ser maior que a data de demissão.", UtilBO.TipoMensagem.Alerta);
                return false;
            }

            if (!string.IsNullOrEmpty(txtVigenciaFim.Text))
            {
                if (Convert.ToDateTime(txtVigenciaFim.Text) > _VS_dataDemissao)
                {
                    _lblMessage.Text = UtilBO.GetErroMessage("Vigência final não pode ser maior que a data de demissão.", UtilBO.TipoMensagem.Alerta);
                    return false;
                }
            }
        }

        if (ddlFuncaoSituacao.SelectedValue == "-1")
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Situação é obrigatório.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        _lblMessage.Visible = false;
        return true;
    }

    /// <summary>
    /// Verifica se existe funções cadastrados com as mesmas informações
    /// </summary>
    /// <returns></returns>
    private bool ExisteFuncaoComMesmasInformacoes()
    {
        for (int i = 0; i < _VS_funcoes.Rows.Count; i++)
        {
            if (_VS_funcoes.Rows[i].RowState != DataRowState.Deleted)
            {
                bool cargoDiferente = _VS_funcoes.Rows[i]["seqfun_id"].ToString() != Convert.ToString(_VS_seq);

                if (((!_VS_IsNew && cargoDiferente) || (_VS_IsNew)) &&
                    _VS_funcoes.Rows[i]["fun_id"].ToString() == Convert.ToString(UCComboFuncao1.Valor) &&
                    _VS_funcoes.Rows[i]["coc_matricula"].ToString() == Convert.ToString(txtMatricula.Text) &&
                    _VS_funcoes.Rows[i]["uad_id"].ToString() == Convert.ToString(_VS_uad_id) &&
                    Convert.ToByte(_VS_funcoes.Rows[i]["situacao_id"]) != (byte)RHU_ColaboradorFuncaoSituacao.Desativado)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Adiciona nova função
    /// </summary>
    private void _IncluirFuncao()
    {
        DataRow dr = _VS_funcoes.NewRow();

        if (_VS_seq == -1)
        {
            _VS_seq = 1;
        }
        else
        {
            _VS_seq = _VS_seq + 1;
        }
        
        dr["seqfun_id"] = _VS_seq;
        dr["fun_id"] = UCComboFuncao1.Valor;
        dr["uad_id"] = _VS_uad_id;
        dr["vigenciaini"] = txtVigenciaIni.Text;        
        dr["vigenciafim"] = txtVigenciaFim.Text;
        dr["coc_matricula"] = txtMatricula.Text;
        dr["cargofuncao"] = UCComboFuncao1.Texto;
        dr["uad_nome"] = txtUA.Text;        
        dr["cof_responsavelUA"] = chkResponsavelUA.Checked;
        dr["controladoIntegracao"] = false;

        if (String.IsNullOrEmpty(txtVigenciaFim.Text.Trim()))
        {
            dr["vigencia"] = txtVigenciaIni.Text + " - *";
        }
        else
        {
            if (txtVigenciaIni.Text == txtVigenciaFim.Text)
            {
                dr["vigencia"] = txtVigenciaIni.Text;
            }
            else
            {
                dr["vigencia"] = txtVigenciaIni.Text + " - " + txtVigenciaFim.Text;
            }

            if (Convert.ToDateTime(txtVigenciaFim.Text) < DateTime.Now.Date)
            {
                ddlFuncaoSituacao.SelectedValue = ((byte)RHU_ColaboradorFuncaoSituacao.Desativado).ToString();
            }
        }

        dr["situacao_id"] = ddlFuncaoSituacao.SelectedValue;
        dr["situacao"] = ddlFuncaoSituacao.SelectedItem;
        dr["observacao"] = txtObservacao.Text;

        _VS_funcoes.Rows.Add(dr);
        _IncluirFuncao(_VS_funcoes);
    }

    /// <summary>
    /// Altera dados da função.
    /// </summary>
    private void _AlterarFuncao(int fun_id, int seqfun_id)
    {        
        for (int i = 0; i < _VS_funcoes.Rows.Count; i++)
        {
            if (_VS_funcoes.Rows[i].RowState != DataRowState.Deleted)
            {
                if (_VS_funcoes.Rows[i]["seqfun_id"].ToString() == Convert.ToString(seqfun_id) && _VS_funcoes.Rows[i]["fun_id"].ToString() == Convert.ToString(fun_id))
                {                    
                    _VS_funcoes.Rows[i]["uad_id"] = _VS_uad_id;
                    _VS_funcoes.Rows[i]["vigenciaini"] = txtVigenciaIni.Text;
                    _VS_funcoes.Rows[i]["vigenciafim"] = txtVigenciaFim.Text;
                    _VS_funcoes.Rows[i]["cargofuncao"] = UCComboFuncao1.Texto;
                    _VS_funcoes.Rows[i]["coc_matricula"] = txtMatricula.Text;                 
                    _VS_funcoes.Rows[i]["uad_nome"] = txtUA.Text;
                    _VS_funcoes.Rows[i]["cof_responsavelUA"] = chkResponsavelUA.Checked;

                    if (string.IsNullOrEmpty(txtVigenciaFim.Text.Trim()))
                    {
                        _VS_funcoes.Rows[i]["vigencia"] = txtVigenciaIni.Text + " - *";
                    }
                    else
                    {
                        if (txtVigenciaIni.Text == txtVigenciaFim.Text)
                        {
                            _VS_funcoes.Rows[i]["vigencia"] = txtVigenciaIni.Text;
                        }
                        else
                        {
                            _VS_funcoes.Rows[i]["vigencia"] = txtVigenciaIni.Text + " - " + txtVigenciaFim.Text;
                        }

                        if (Convert.ToDateTime(txtVigenciaFim.Text) < DateTime.Now.Date)
                        {
                            ddlFuncaoSituacao.SelectedValue = ((byte)RHU_ColaboradorFuncaoSituacao.Desativado).ToString();
                        }
                    }

                    _VS_funcoes.Rows[i]["situacao_id"] = ddlFuncaoSituacao.SelectedValue;
                    _VS_funcoes.Rows[i]["situacao"] = ddlFuncaoSituacao.SelectedItem;
                    _VS_funcoes.Rows[i]["observacao"] = txtObservacao.Text;                    

                    _Incluir(_VS_funcoes);
                }
            }
        }
    }

    /// <summary>
    /// Configura a tela conforme parâmetros acadêmicos referentes ao controle de integração.
    /// </summary>
    public void ConfiguraCadastro()
    {
        if (!ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_COLABORADOR_VINCULO_INTEGRADO_VIRTUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id) ||
            (ddlFuncaoSituacao.SelectedValue.Equals("6")))
        {
            HabilitaControles(Controls, false);
            UCComboFuncao1.PermiteEditar = false;
        }
        else if (!VS_cof_controladoIntegracao && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_COLABORADOR_VINCULO_INTEGRADO_VIRTUAL_MATRICULA_OBRIGATORIA, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
        {
            rfvMatricula.Visible = true;
            LabelMatricula.Text = "Matrícula *";

            if (_VS_IsNew)
                ddlFuncaoSituacao.SelectedValue = "1";

            ddlFuncaoSituacao.Enabled = false;         
        }
    }

    /// <summary>
    /// Seta a propriedade Enabled passada para todos os WebControl do ControlCollection
    /// passado.
    /// </summary>
    /// <param name="controls"></param>
    /// <param name="enabled"></param>
    protected void HabilitaControles(ControlCollection controls, bool enabled)
    {
        foreach (Control c in controls)
        {
            if (c.Controls.Count > 0)
                HabilitaControles(c.Controls, enabled);

            WebControl wb = c as WebControl;

            if (wb != null)
                wb.Enabled = enabled;
        }

        txtUA.Enabled = false;
    }

    #endregion

    #region EVENTOS

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(Page);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
        }

        if (!IsPostBack)
        {
            LabelMatricula.Text = "Matrícula";
            rfvMatricula.Visible = false;
            cvVigenciaIni.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de vigência inicial");
            cvVigenciaFim.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de vigência final");

            UCComboFuncao1.Obrigatorio = true;
            UCComboFuncao1.ValidationGroup = "Funcao";
            UCComboFuncao1.CarregarFuncao();
        }
    }

    protected void _btnIncluir_Click(object sender, EventArgs e)
    {
        if (_Validar())
        {
            if (_VS_IsNew)
            {
                _IncluirFuncao();
            }
            else
            {
                _AlterarFuncao(_VS_id, _VS_seq);
            }

            divCadastro.Visible = false;
        }
        else
        {
            _lblMessage.Visible = true;
            updCadastroFuncoes.Update();
        }
    }

    protected void btnPesquisarUA_Click(object sender, ImageClickEventArgs e)
    {
        _SelecionarUA();
    }

    #endregion
}

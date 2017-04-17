using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.CoreSSO.WebServices.Consumer;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

public partial class Academico_RecursosHumanos_Docente_Cadastro : MotherPageLogado
{
    #region Constantes
    
    private const int indiceGridCargoFuncaoExcluir = 5;

    #endregion

    #region Propriedades

    private long _VS_col_id
    {
        get
        {
            if (ViewState["_VS_col_id"] != null)
                return Convert.ToInt64(ViewState["_VS_col_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_col_id"] = value;
        }
    }

    private long _VS_doc_id
    {
        get
        {
            if (ViewState["_VS_doc_id"] != null)
                return Convert.ToInt64(ViewState["_VS_doc_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_doc_id"] = value;
        }
    }

    /// <summary>
    /// Retorna e atribui o ViewState com o tipo de busca de ua
    /// 1 - cargo
    /// 2 - fução
    /// </summary>
    public byte _VS_tipoBuscaUA
    {
        get
        {
            if (ViewState["_VS_tipoBuscaUA"] != null)
                return Convert.ToByte((ViewState["_VS_tipoBuscaUA"].ToString()));
            return 0;
        }
        set
        {
            ViewState["_VS_tipoBuscaUA"] = value;
        }
    }

    private long _VS_arq_idAntigo
    {
        get
        {
            if (ViewState["_VS_arq_idAntigo"] != null)
                return Convert.ToInt64(ViewState["_VS_arq_idAntigo"]);

            return -1;
        }
        set
        {
            ViewState["_VS_arq_idAntigo"] = value;
        }
    }

    private Guid _VS_pai_idAntigo
    {
        get
        {
            if (ViewState["_VS_pai_idAntigo"] != null)
                return new Guid(ViewState["_VS_pai_idAntigo"].ToString());
            return Guid.Empty;
        }
        set
        {
            ViewState["_VS_pai_idAntigo"] = value;
        }
    }

    private Guid _VS_cid_idAntigo
    {
        get
        {
            if (ViewState["_VS_cid_idAntigo"] != null)
                return new Guid(ViewState["_VS_cid_idAntigo"].ToString());
            return Guid.Empty;
        }
        set
        {
            ViewState["_VS_cid_idAntigo"] = value;
        }
    }

    private Guid _VS_tes_idAntigo
    {
        get
        {
            if (ViewState["_VS_tes_idAntigo"] != null)
                return new Guid(ViewState["_VS_tes_idAntigo"].ToString());
            return Guid.Empty;
        }
        set
        {
            ViewState["_VS_tes_idAntigo"] = value;
        }
    }

    private Guid _VS_tde_idAntigo
    {
        get
        {
            if (ViewState["_VS_tde_idAntigo"] != null)
                return new Guid(ViewState["_VS_tde_idAntigo"].ToString());
            return Guid.Empty;
        }
        set
        {
            ViewState["_VS_tde_idAntigo"] = value;
        }
    }

    private Guid _VS_usu_id
    {
        get
        {
            if (ViewState["_VS_usu_id"] != null)
                return new Guid((ViewState["_VS_usu_id"].ToString()));
            return Guid.Empty;
        }
        set
        {
            ViewState["_VS_usu_id"] = value;
        }
    }
    
    /// <summary>
    /// Armazena o índice no combo da opção de outros domínios.
    /// </summary>
    private int _VS_OutrosDominios
    {
        get
        {
            if (ViewState["_VS_OutrosDominios"] != null)
                return (int)ViewState["_VS_OutrosDominios"];
            return 0;
        }
        set
        {
            ViewState["_VS_OutrosDominios"] = value;
        }
    }

    private bool _VS_col_controladoIntegracao
    {
        get
        {
            if (ViewState["_VS_col_controladoIntegracao"] != null)
                return Convert.ToBoolean(ViewState["_VS_col_controladoIntegracao"]);
            return false;
        }
        set
        {
            ViewState["_VS_col_controladoIntegracao"] = value;
        }
    }

    /// <summary>
    /// Armazena as UA que o usuário tem permissão se for visão ua ou gestão
    /// </summary>
    private List<Guid> VS_lista
    {
        get
        {
            if (ViewState["VS_lista"] != null)
                return (List<Guid>)ViewState["VS_lista"];
            return new List<Guid>();
        }
        set
        {
            ViewState["VS_lista"] = value;
        }
    }

    #endregion

    #region Eventos Life Cycle

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Configura tamanho senha
            revSenhaTamanho.ValidationExpression = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TAMANHO_SENHA_USUARIO);
            revSenhaTamanho.ErrorMessage = String.Format(revSenhaTamanho.ErrorMessage, UtilBO.GetMessageTamanhoByRegex(revSenhaTamanho.ValidationExpression));
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.UiAriaTabs));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsTabs.js"));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.ExitPageConfirm));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsSetExitPageConfirmer.js"));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroPessoa.js"));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroColaborador.js"));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroDocente.js"));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsUCCadastroEndereco.js"));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroCertidaoCivil.js"));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsConfirmaFormacaoDocente.js"));
            sm.Services.Add(new ServiceReference("~/WSServicos.asmx"));
        }

        UCPessoas1.Paginacao = ApplicationWEB._Paginacao;
        UCPessoas1.ContainerName = "divBuscaPessoa";
        UCPessoas1.ReturnValues += UCPessoas1BuscaPessoa;

        // Eventos para o cadastro de cidades.
        UCCadastroPessoa1._AbreJanelaCadastroCidade += _AbreJanelaCadastroCidade;
        UCCertidaoCivil1._AbreJanelaCadastroCidade += _AbreJanelaCadastroCidade;
        UCGridDocumento1._TextChanged += UCGridDocumento1_TextChanged;

        UCUA1.Paginacao = ApplicationWEB._Paginacao;
        UCUA1.ContainerName = "divBuscaUA";
        UCUA1.ReturnValues += UCUA1BuscaUA;
        UCUA1.AddParameters("ent_idVisible", "false");
        UCUA1.AddParameters("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        UCUA1.AddParameters("vis_id", __SessionWEB.__UsuarioWEB.Grupo.vis_id.ToString());
        UCUA1.AddParameters("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
        UCUA1.AddParameters("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
        
        UCCadastroCargo1.AdicionaClasseCss = "subir";
        UCCadastroFuncao1.AdicionaClasseCss = "subir";

        // Cadastro de cidades - usado no user control de pessoa e certidão civil.
        UCCadastroCidade1.Inicialize(ApplicationWEB._Paginacao, "divCadastroCidade");

        if (!IsPostBack)
        {
            cvDataAdmissao.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de admissão");
            cvDataDemissao.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de demissão");
            UCCadastroPessoa1._lblMae.Text = "Nome da Mãe";
            UCCadastroPessoa1._lblPai.Text = "Nome do Pai";
            _CarregarComboDominios();

            Page.Form.Enctype = "multipart/form-data";

            try
            {
                // Carrega as UA que o usuário tem permissão se for visão ua ou gestão
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa ||
                    __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao)
                    VS_lista = ESC_EscolaBO.GetSelect_Uad_Ids_By_PermissaoUsuario(__SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id);

                //Atribui na Label e ErrorMessage do UCCadastroPessoa, referente ao docente.
                UCCadastroPessoa1._labelNome.Text = "Nome do docente *";
                UCCadastroPessoa1.rfvNome.ErrorMessage = "Nome do docente é obrigatório.";

                UCEnderecos1.Inicializar(false, false, "Pessoa", true, true);

                if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                {
                    _VS_col_id = PreviousPage.EditItem;
                    _VS_doc_id = PreviousPage.EditItemDocId;
                    _LoadFromEntity();
                }
                else if (Session["col_id"] != null)
                {
                    _VS_col_id = Int64.Parse(Session["col_id"].ToString());
                    _VS_doc_id = -1;
                    _LoadFromEntity();
                }
                else if (Session["pes_id"] != null)
                {
                    CarregaPorPessoa(new Guid(Session["pes_id"].ToString()));
                }
                else
                {
                    UCGridDocumento1._CarregarDocumento(Guid.Empty);

                    UCCertidaoCivil1.Inicializa("Pessoa");
                    
                    _grvCargosFuncoes.DataSource = new DataTable();
                    _grvCargosFuncoes.DataBind();

                    string parametro = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.PAIS_PADRAO_BRASIL);
                    if (!String.IsNullOrEmpty(parametro))
                        UCCadastroPessoa1.ComboNacionalidade1_Valor = new Guid(parametro.ToLower());

                    //Verifica se existe integraçao externa
                    _ckbUsuarioLive.Visible = VerificaIntegracaoExterna();
                }

                UCCadastroCargo1.ComboCargoCarregar(true);
                UCCadastroCargo1._VS_IsNew = true;

                UCCadastroFuncao1.ComboFuncaoCarregar();
                UCCadastroFuncao1._VS_IsNew = true;

                RHU_Colaborador entityColaborador = new RHU_Colaborador
                {
                    col_id = _VS_col_id
                };
                RHU_ColaboradorBO.GetEntity(entityColaborador);
                UCContato1.CarregarContatosDoBanco(entityColaborador.pes_id);

                UCComboColaboradorSituacao1.Obrigatorio = true;
                UCComboColaboradorSituacao1.ValidationGroup = "Pessoa";

                UCCadastroPessoa1._VS_tipoPessoa = 2;

                if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PREENCHER_LOGIN_SENHA_AUTOMATICAMENTE_COLABORADORES_DOCENTES, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    _txtLogin.Enabled = false;
                    _txtSenha.Enabled = false;
                    _txtConfirmacao.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                divDocente.Visible = false;
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }

            Page.Form.DefaultButton = _btnSalvar.UniqueID;

            bool controlarVinculoApenasIntegracao = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_COLABORADOR_APENAS_INTEGRACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            bool vinculoIntegradoVirtual = !controlarVinculoApenasIntegracao && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_COLABORADOR_VINCULO_INTEGRADO_VIRTUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            bool podeEditarVinculos = ((__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && _VS_col_id > 0) ||
                                          (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && _VS_col_id <= 0)) &&
                                         vinculoIntegradoVirtual;

            _btnNovaFuncao.Visible = _btnNovoCargo.Visible = podeEditarVinculos;
            _grvCargosFuncoes.Columns[indiceGridCargoFuncaoExcluir].Visible = podeEditarVinculos;

            bool podeEditar = ((__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && _VS_col_id > 0) ||
                               (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && _VS_col_id <= 0)) &&
                               !_VS_col_controladoIntegracao;
            
            VerificaObrigatoriedadeEmail();
        }

        bool podeEditarDocente = ((__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && _VS_col_id > 0) ||
                                      (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && _VS_col_id <= 0)) &&
                                     !_VS_col_controladoIntegracao;

        bool controlarVinculoApenasIntegracaoDocente = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_COLABORADOR_APENAS_INTEGRACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        bool vinculoIntegradoVirtualDocente = !controlarVinculoApenasIntegracaoDocente && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_COLABORADOR_VINCULO_INTEGRADO_VIRTUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

        bool podeEditarVinculosDocente = ((__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && _VS_col_id > 0) ||
                                      (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && _VS_col_id <= 0)) &&
                                     vinculoIntegradoVirtualDocente;
        
        if (!podeEditarDocente)
        {
            HabilitaControles(fdsDadosPessoais.Controls, false);
            UCEnderecos1.DesabilitarCamposEnderecos();
            HabilitaControles(fdsContato.Controls, false);
            HabilitaControles(fdsDocumentos.Controls, false);
            HabilitaControles(fdsCertidoes.Controls, false);
            HabilitaControles(fdsCriarUsuario.Controls, false);
            HabilitaControles(fdsUsuario.Controls, false);
        }

        if (!podeEditarDocente && !podeEditarVinculosDocente)
        {
            _btnCancelar.Visible = true;
        }

        RegistrarParametrosMensagemSair(_btnSalvar.Visible, (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0));

        #region INICIALIZACAO DOS DELEGATES

        UCCadastroPessoa1._Selecionar += UCCadastroPessoa1__Seleciona;

        UCCadastroCargo1._Incluir += UCCadastroCargo1__Incluir;
        UCCadastroCargo1._Selecionar += UCCadastroCargo1__Selecionar;
        UCCadastroFuncao1._Incluir += UCCadastroFuncao1__Incluir;
        UCCadastroFuncao1._Selecionar += UCCadastroFuncao1__Selecionar;

        #endregion
    }

    #endregion

    #region Métodos

    private void VerificaObrigatoriedadeEmail()
    {
        if (!SYS_ParametroBO.Parametro_ValidarObrigatoriedadeEmailUsuario() || _ckbUsuarioLive.Checked)
        {
            _rfvEmail.Enabled = false;
            RemoveAsteriscoObrigatorio(LabelEmail);
        }
        else
        {
            _rfvEmail.Enabled = true;
            AdicionaAsteriscoObrigatorio(LabelEmail);
        }
    }

    /// <summary>
    /// Carrega os dados do docente nos controles caso seja alteração.
    /// </summary>
    private void _LoadFromEntity()
    {
        try
        {
            DataTable dtDocente = ACA_DocenteBO.SelecionaPorColaboradorDocente(_VS_col_id, _VS_doc_id);

            if (dtDocente.Rows.Count > 0)
            {
                #region Carrega dados do docente

                DataRow row = dtDocente.Rows[0];

                string doc_codigoInep = row["doc_codigoInep"].ToString();
                string pes_nome = row["pes_nome"].ToString();
                string pes_nome_abreviado = row["pes_nome_abreviado"].ToString();
                string cid_nome = row["cid_nome"].ToString();

                bool col_controladoIntegracao = string.IsNullOrEmpty(row["col_controladoIntegracao"].ToString()) ? false : Convert.ToBoolean(row["col_controladoIntegracao"]);
                bool pes_naturalizado = string.IsNullOrEmpty(row["pes_naturalizado"].ToString()) ? false : Convert.ToBoolean(row["pes_naturalizado"]);

                DateTime col_dataAdmissao = string.IsNullOrEmpty(row["col_dataAdmissao"].ToString()) ? new DateTime() : Convert.ToDateTime(row["col_dataAdmissao"]);
                DateTime col_dataDemissao = string.IsNullOrEmpty(row["col_dataDemissao"].ToString()) ? new DateTime() : Convert.ToDateTime(row["col_dataDemissao"]);
                DateTime pes_dataNascimento = string.IsNullOrEmpty(row["pes_dataNascimento"].ToString()) ? new DateTime() : Convert.ToDateTime(row["pes_dataNascimento"]);

                byte col_situacao = string.IsNullOrEmpty(row["col_situacao"].ToString()) ? (byte)0 : (byte)row["col_situacao"];
                byte pes_estadoCivil = string.IsNullOrEmpty(row["pes_estadoCivil"].ToString()) ? (byte)0 : (byte)row["pes_estadoCivil"];
                byte pes_sexo = string.IsNullOrEmpty(row["pes_sexo"].ToString()) ? (byte)0 : (byte)row["pes_sexo"];
                byte pes_racaCor = string.IsNullOrEmpty(row["pes_racaCor"].ToString()) ? (byte)0 : (byte)row["pes_racaCor"];

                Guid pes_id = string.IsNullOrEmpty(row["pes_id"].ToString()) ? Guid.Empty : new Guid(row["pes_id"].ToString());
                Guid ent_id = string.IsNullOrEmpty(row["ent_id"].ToString()) ? Guid.Empty : new Guid(row["ent_id"].ToString());
                Guid cid_idNaturalidade = string.IsNullOrEmpty(row["cid_id"].ToString()) ? Guid.Empty : new Guid(row["cid_id"].ToString());
                Guid pai_idNacionalidade = string.IsNullOrEmpty(row["pai_idNacionalidade"].ToString()) ? Guid.Empty : new Guid(row["pai_idNacionalidade"].ToString());
                Guid pes_idFiliacaoPai = string.IsNullOrEmpty(row["pes_idFiliacaoPai"].ToString()) ? Guid.Empty : new Guid(row["pes_idFiliacaoPai"].ToString());
                Guid pes_idFiliacaoMae = string.IsNullOrEmpty(row["pes_idFiliacaoMae"].ToString()) ? Guid.Empty : new Guid(row["pes_idFiliacaoMae"].ToString());
                Guid tes_id = string.IsNullOrEmpty(row["tes_id"].ToString()) ? Guid.Empty : new Guid(row["tes_id"].ToString());
                Guid tde_id = string.IsNullOrEmpty(row["tde_id"].ToString()) ? Guid.Empty : new Guid(row["tde_id"].ToString());

                #endregion

                _txtCodigoInep.Text = doc_codigoInep;

                if (ent_id != __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                {
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("O docente não pertence à entidade na qual você está logado.", UtilBO.TipoMensagem.Alerta);
                    Response.Redirect("~/Academico/RecursosHumanos/Docente/Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

                _VS_col_controladoIntegracao = col_controladoIntegracao;

                //Exibe dados do colaborador
                _txtDataAdmissao.Text = col_dataAdmissao.ToString("dd/MM/yyyy");
                _txtDataDemissao.Text = (col_dataDemissao != new DateTime()) ? col_dataDemissao.ToString("dd/MM/yyyy") : string.Empty;
                UCComboColaboradorSituacao1.Valor = col_situacao > 0 ? col_situacao : -1;

                UCCadastroPessoa1._VS_pes_id = pes_id;
                UCCadastroPessoa1._txtNome.Text = pes_nome;
                UCCadastroPessoa1._txtNomeAbreviado.Text = pes_nome_abreviado;

                long arq_id;
                UCCadastroPessoa1.ConfiguraDadosFoto(PaginaGestao.Docentes, out arq_id);
                _VS_arq_idAntigo = arq_id;

                //Exibe cidade naturalidade da pessoa
                if (!string.IsNullOrEmpty(cid_nome))
                {
                    UCCadastroPessoa1._VS_cid_id = cid_idNaturalidade;
                    UCCadastroPessoa1._txtNaturalidade.Text = cid_nome;
                }

                //Exibe dados gerais da pessoa
                UCCadastroPessoa1._txtDataNasc.Text = (pes_dataNascimento != new DateTime()) ? pes_dataNascimento.ToString("dd/MM/yyyy") : string.Empty;
                UCCadastroPessoa1._ComboEstadoCivil.SelectedValue = (pes_estadoCivil > 0 ? pes_estadoCivil.ToString() : "-1");
                UCCadastroPessoa1._ComboSexo.SelectedValue = pes_sexo.ToString();

                UCCadastroPessoa1.ComboNacionalidade1_Valor = pai_idNacionalidade;
                UCCadastroPessoa1._chkNaturalizado.Checked = pes_naturalizado;
                UCCadastroPessoa1._ComboRacaCor.SelectedValue = (pes_racaCor > 0 ? pes_racaCor.ToString() : "-1");
                UCCadastroPessoa1._VS_pes_idFiliacaoPai = pes_idFiliacaoPai;
                UCCadastroPessoa1._VS_pes_idFiliacaoMae = pes_idFiliacaoMae;
                UCCadastroPessoa1._ComboEscolaridade.SelectedValue = (tes_id != Guid.Empty ? tes_id.ToString() : Guid.Empty.ToString());

                //Carregar tipo de deficiência cadastrada para a pessoa
                if (tde_id != new Guid())
                    UCCadastroPessoa1.ComboTipoDeficiencia1_Valor = tde_id;

                //Armazena os os id's antigos em ViewState
                _VS_pai_idAntigo = pai_idNacionalidade;
                _VS_cid_idAntigo = cid_idNaturalidade;
                _VS_tes_idAntigo = tes_id;
                _VS_tde_idAntigo = (tde_id != new Guid()) ? tde_id : Guid.Empty;

                #region Carrega dados do pai do docente

                if (pes_idFiliacaoPai != Guid.Empty)
                {
                    PES_Pessoa pesFiliacaoPai = new PES_Pessoa { pes_id = pes_idFiliacaoPai };
                    PES_PessoaBO.GetEntity(pesFiliacaoPai);
                    UCCadastroPessoa1._txtPai.Text = pesFiliacaoPai.pes_nome;
                    UCCadastroPessoa1.visibleLimparPai = true;
                    PES_PessoaDocumento cpf = new PES_PessoaDocumento
                    {
                        pes_id = new Guid(pesFiliacaoPai.pes_id.ToString())
                        ,
                        tdo_id = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF))
                        ,
                        psd_situacao = 1
                    };
                    PES_PessoaDocumentoBO.GetEntity(cpf);
                    PES_PessoaDocumento rg = new PES_PessoaDocumento
                    {
                        pes_id = new Guid(pesFiliacaoPai.pes_id.ToString())
                        ,
                        tdo_id = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_RG))
                        ,
                        psd_situacao = 1
                    };
                    PES_PessoaDocumentoBO.GetEntity(rg);
                    UCCadastroPessoa1.txtCPFPaiValor = cpf.psd_numero;
                    UCCadastroPessoa1.txtRGPaiValor = rg.psd_numero;
                }

                #endregion

                #region Carrega dados da mae do docente

                if (pes_idFiliacaoMae != Guid.Empty)
                {
                    PES_Pessoa pesFiliacaoMae = new PES_Pessoa { pes_id = pes_idFiliacaoMae };
                    PES_PessoaBO.GetEntity(pesFiliacaoMae);
                    UCCadastroPessoa1._txtMae.Text = pesFiliacaoMae.pes_nome;
                    UCCadastroPessoa1.visibleLimparMae = true;
                    PES_PessoaDocumento cpf = new PES_PessoaDocumento
                    {
                        pes_id = new Guid(pesFiliacaoMae.pes_id.ToString())
                        ,
                        tdo_id = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF))
                        ,
                        psd_situacao = 1
                    };
                    PES_PessoaDocumentoBO.GetEntity(cpf);
                    PES_PessoaDocumento rg = new PES_PessoaDocumento
                    {
                        pes_id = new Guid(pesFiliacaoMae.pes_id.ToString())
                        ,
                        tdo_id = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_RG))
                        ,
                        psd_situacao = 1
                    };
                    PES_PessoaDocumentoBO.GetEntity(rg);
                    UCCadastroPessoa1.txtCPFMaeValor = cpf.psd_numero;
                    UCCadastroPessoa1.txtRGMaeValor = rg.psd_numero;
                }

                #endregion

                //Carrega dados dos endereços da pessoa
                DataTable dtEndereco = PES_PessoaEnderecoBO.GetSelect(pes_id, false, 1, 1);
                UCEnderecos1.CarregarEnderecosBanco(dtEndereco);

                //Carrega dados dos documentos da pessoa
                UCGridDocumento1._CarregarDocumento(pes_id);

                // Carrega dados da certidões da pessoa.
                UCCertidaoCivil1.Inicializa("Pessoa");
                UCCertidaoCivil1.CarregaCertidaoCivil(pes_id);
                
                //Carrega dados dos cargos / funções do colaborador
                DataTable dtCargoFuncao = RHU_ColaboradorCargoBO.GetSelect(_VS_col_id, false, 1, 1, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                if (dtCargoFuncao.Rows.Count == 0)
                    dtCargoFuncao = null;

                UCCadastroCargo1.VS_IsDocente = true;
                UCCadastroCargo1._VS_dataAdmissao = col_dataAdmissao;
                UCCadastroFuncao1._VS_dataAdmissao = col_dataAdmissao;

                UCCadastroCargo1._VS_dataDemissao = col_dataDemissao;
                UCCadastroFuncao1._VS_dataDemissao = col_dataDemissao;

                UCCadastroCargo1.CarregarCargosDisciplinas(_VS_col_id);

                UCCadastroCargo1._VS_Coc_ID = RHU_ColaboradorCargoBO.VerificaUltimoCargoCadastrado(_VS_col_id, 0) - 1;
                UCCadastroFuncao1._VS_seq = RHU_ColaboradorFuncaoBO.VerificaUltimaFuncaoCadastrada(_VS_col_id, 0) - 1;

                UCCadastroCargo1.VS_col_id = UCCadastroFuncao1.VS_col_id = _VS_col_id;

                UCCadastroCargo1._VS_cargos = dtCargoFuncao;
                UCCadastroFuncao1._VS_funcoes = dtCargoFuncao;
                _CarregarCargoFuncao(UCCadastroCargo1._VS_cargos);

                _VS_usu_id = SYS_UsuarioBO.GetSelectBy_pes_id(UCCadastroPessoa1._VS_pes_id);

                //Verifica se existe integraçao externa
                _ckbUsuarioLive.Visible = VerificaIntegracaoExterna();

                #region Carrega dados do usuário do docente (se existir)

                if (_VS_usu_id != Guid.Empty)
                {
                    _chbCriarUsuario.Checked = true;
                    divUsuarios.Visible = true;

                    SYS_Usuario usuario = new SYS_Usuario { usu_id = _VS_usu_id };
                    SYS_UsuarioBO.GetEntity(usuario);

                    _txtLogin.Text = usuario.usu_login;
                    _txtEmail.Text = usuario.usu_email;

                    _chkSenhaAutomatica.Visible = true;
                    _chkSenhaAutomatica.Checked = false;
                    _rfvSenha.Visible = false;
                    _rfvConfirmarSenha.Visible = false;

                    if (!string.IsNullOrEmpty(usuario.usu_dominio))
                    {
                        _ckbUsuarioAD.Checked = true;

                        bool encontrou = false;
                        foreach (ListItem item in _ddlDominios.Items)
                        {
                            if (item.Value == usuario.usu_dominio)
                            {
                                item.Selected = true;
                                encontrou = true;
                            }
                        }
                        //Caso não encontre o domínio na lista de disponíveis...
                        if (!encontrou)
                        {
                            //Seta a opção outros.
                            _ddlDominios.SelectedIndex = _VS_OutrosDominios;
                            _TrataOutrosDominios();
                            _txtDominio.Text = usuario.usu_dominio;
                        }
                    }

                    _TrataUsuarioAD();

                    if (usuario.usu_situacao == 5)
                        _chkExpiraSenha.Checked = true;
                    else if (usuario.usu_situacao == 2)
                        _chkBloqueado.Checked = true;

                    _rfvSenha.Visible = false;
                    _rfvConfirmarSenha.Visible = false;

                    if (VerificaIntegracaoExterna())
                    {
                        ManageUserLive live = new ManageUserLive();
                        if (live.IsContaEmail(usuario.usu_email))
                        {
                            _ckbUsuarioLive.Checked = true;
                        }
                        _AtualizarUsuarioLive();
                    }
                    else
                    {
                        _ckbUsuarioLive.Visible = false;
                    }
                }
                else
                {
                    _chkSenhaAutomatica.Visible = true;
                    _chkSenhaAutomatica.Checked = false;
                    _chbCriarUsuario.Checked = false;
                    divUsuarios.Visible = false;
                    _txtLogin.Enabled = true;
                }

                #endregion
            }
            else
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Academico/RecursosHumanos/Docente/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o docente.", UtilBO.TipoMensagem.Erro);
        }
    }

    private void CarregaPorPessoa(Guid pes_id)
    {
        try
        {
            PES_Pessoa pessoa = new PES_Pessoa { pes_id = pes_id };
            PES_PessoaBO.GetEntity(pessoa);

            if (pessoa.pes_id != new Guid())
            {
                UCCadastroPessoa1._VS_pes_id = pessoa.pes_id;
                UCCadastroPessoa1._txtNome.Text = pessoa.pes_nome;
                UCCadastroPessoa1._txtNomeAbreviado.Text = pessoa.pes_nome_abreviado;

                //Exibe cidade naturalidade da pessoa
                if (pessoa.cid_idNaturalidade != new Guid())
                {
                    UCCadastroPessoa1._VS_cid_id = pessoa.cid_idNaturalidade;
                    UCCadastroPessoa1._txtNaturalidade.Text = END_CidadeBO.GetEntity(new END_Cidade { cid_id = pessoa.cid_idNaturalidade }).cid_nome;
                }

                //Exibe dados gerais da pessoa
                UCCadastroPessoa1._txtDataNasc.Text = (pessoa.pes_dataNascimento != new DateTime()) ? pessoa.pes_dataNascimento.ToString("dd/MM/yyyy") : string.Empty;
                UCCadastroPessoa1._ComboEstadoCivil.SelectedValue = (pessoa.pes_estadoCivil > 0 ? pessoa.pes_estadoCivil.ToString() : "-1");
                UCCadastroPessoa1._ComboSexo.SelectedValue = pessoa.pes_sexo.ToString();

                UCCadastroPessoa1.ComboNacionalidade1_Valor = pessoa.pai_idNacionalidade;
                UCCadastroPessoa1._chkNaturalizado.Checked = pessoa.pes_naturalizado;
                UCCadastroPessoa1._ComboRacaCor.SelectedValue = (pessoa.pes_racaCor > 0 ? pessoa.pes_racaCor.ToString() : "-1");
                UCCadastroPessoa1._VS_pes_idFiliacaoPai = pessoa.pes_idFiliacaoPai;
                UCCadastroPessoa1._VS_pes_idFiliacaoMae = pessoa.pes_idFiliacaoMae;
                UCCadastroPessoa1._ComboEscolaridade.SelectedValue = (pessoa.tes_id != Guid.Empty ? pessoa.tes_id.ToString() : Guid.Empty.ToString());

                //Carregar tipo de deficiência cadastrada para a pessoa
                List<PES_PessoaDeficiencia> pesDef = PES_PessoaDeficienciaBO.SelecionaPorPessoa(pessoa.pes_id);
                if (pesDef.Count > 0)
                {
                    UCCadastroPessoa1.ComboTipoDeficiencia1_Valor = pesDef[0].tde_id;
                    _VS_tde_idAntigo = (pesDef[0].tde_id != new Guid()) ? pesDef[0].tde_id : Guid.Empty;
                }

                //Armazena os os id's antigos em ViewState
                _VS_pai_idAntigo = pessoa.pai_idNacionalidade;
                _VS_cid_idAntigo = pessoa.cid_idNaturalidade;
                _VS_tes_idAntigo = pessoa.tes_id;


                #region Carrega dados do pai do docente

                if (pessoa.pes_idFiliacaoPai != Guid.Empty)
                {
                    PES_Pessoa pesFiliacaoPai = new PES_Pessoa { pes_id = pessoa.pes_idFiliacaoPai };
                    PES_PessoaBO.GetEntity(pesFiliacaoPai);
                    UCCadastroPessoa1._txtPai.Text = pesFiliacaoPai.pes_nome;
                    UCCadastroPessoa1.visibleLimparPai = true;
                    PES_PessoaDocumento cpf = new PES_PessoaDocumento
                    {
                        pes_id = new Guid(pesFiliacaoPai.pes_id.ToString())
                        ,
                        tdo_id = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF))
                        ,
                        psd_situacao = 1
                    };
                    PES_PessoaDocumentoBO.GetEntity(cpf);
                    PES_PessoaDocumento rg = new PES_PessoaDocumento
                    {
                        pes_id = new Guid(pesFiliacaoPai.pes_id.ToString())
                        ,
                        tdo_id = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_RG))
                        ,
                        psd_situacao = 1
                    };
                    PES_PessoaDocumentoBO.GetEntity(rg);
                    UCCadastroPessoa1.txtCPFPaiValor = cpf.psd_numero;
                    UCCadastroPessoa1.txtRGPaiValor = rg.psd_numero;
                }

                #endregion

                #region Carrega dados da mae do docente

                if (pessoa.pes_idFiliacaoMae != Guid.Empty)
                {
                    PES_Pessoa pesFiliacaoMae = new PES_Pessoa { pes_id = pessoa.pes_idFiliacaoMae };
                    PES_PessoaBO.GetEntity(pesFiliacaoMae);
                    UCCadastroPessoa1._txtMae.Text = pesFiliacaoMae.pes_nome;
                    UCCadastroPessoa1.visibleLimparMae = true;
                    PES_PessoaDocumento cpf = new PES_PessoaDocumento
                    {
                        pes_id = pesFiliacaoMae.pes_id
                        ,
                        tdo_id = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF))
                        ,
                        psd_situacao = 1
                    };
                    PES_PessoaDocumentoBO.GetEntity(cpf);
                    PES_PessoaDocumento rg = new PES_PessoaDocumento
                    {
                        pes_id = pesFiliacaoMae.pes_id
                        ,
                        tdo_id = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_RG))
                        ,
                        psd_situacao = 1
                    };
                    PES_PessoaDocumentoBO.GetEntity(rg);
                    UCCadastroPessoa1.txtCPFMaeValor = cpf.psd_numero;
                    UCCadastroPessoa1.txtRGMaeValor = rg.psd_numero;
                }

                #endregion

                //Carrega dados dos endereços da pessoa
                DataTable dtEndereco = PES_PessoaEnderecoBO.GetSelect(pes_id, false, 1, 1);
                UCEnderecos1.CarregarEnderecosBanco(dtEndereco);

                //Carrega dados dos documentos da pessoa
                UCGridDocumento1._CarregarDocumento(pes_id);

                // Carrega dados da certidões da pessoa.
                UCCertidaoCivil1.Inicializa("Pessoa");
                UCCertidaoCivil1.CarregaCertidaoCivil(pes_id);

                _VS_usu_id = SYS_UsuarioBO.GetSelectBy_pes_id(UCCadastroPessoa1._VS_pes_id);

                //Verifica se existe integraçao externa
                _ckbUsuarioLive.Visible = VerificaIntegracaoExterna();

                #region Carrega dados do usuário do docente (se existir)

                if (_VS_usu_id != Guid.Empty)
                {
                    _chbCriarUsuario.Checked = true;
                    divUsuarios.Visible = true;

                    SYS_Usuario usuario = new SYS_Usuario { usu_id = _VS_usu_id };
                    SYS_UsuarioBO.GetEntity(usuario);

                    _txtLogin.Text = usuario.usu_login;
                    _txtEmail.Text = usuario.usu_email;

                    _chkSenhaAutomatica.Visible = true;
                    _chkSenhaAutomatica.Checked = false;
                    _rfvSenha.Visible = false;
                    _rfvConfirmarSenha.Visible = false;

                    if (!string.IsNullOrEmpty(usuario.usu_dominio))
                    {
                        _ckbUsuarioAD.Checked = true;

                        bool encontrou = false;
                        foreach (ListItem item in _ddlDominios.Items)
                        {
                            if (item.Value == usuario.usu_dominio)
                            {
                                item.Selected = true;
                                encontrou = true;
                            }
                        }
                        //Caso não encontre o domínio na lista de disponíveis...
                        if (!encontrou)
                        {
                            //Seta a opção outros.
                            _ddlDominios.SelectedIndex = _VS_OutrosDominios;
                            _TrataOutrosDominios();
                            _txtDominio.Text = usuario.usu_dominio;
                        }
                    }

                    _TrataUsuarioAD();

                    if (usuario.usu_situacao == 5)
                        _chkExpiraSenha.Checked = true;
                    else if (usuario.usu_situacao == 2)
                        _chkBloqueado.Checked = true;

                    _rfvSenha.Visible = false;
                    _rfvConfirmarSenha.Visible = false;

                    if (VerificaIntegracaoExterna())
                    {
                        ManageUserLive live = new ManageUserLive();
                        if (live.IsContaEmail(usuario.usu_email))
                        {
                            _ckbUsuarioLive.Checked = true;
                        }
                        _AtualizarUsuarioLive();
                    }
                    else
                    {
                        _ckbUsuarioLive.Visible = false;
                    }
                }
                else
                {
                    _chkSenhaAutomatica.Visible = true;
                    _chkSenhaAutomatica.Checked = false;
                    _chbCriarUsuario.Checked = false;
                    divUsuarios.Visible = false;
                    _txtLogin.Enabled = true;
                }

                #endregion
            }
            else
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Academico/RecursosHumanos/Pessoas/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o docente.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Insere e altera um docente
    /// </summary>
    private void _Salvar()
    {
        try
        {
            ACA_Docente entityDocente = new ACA_Docente
            {
                doc_id = _VS_doc_id
                                                ,
                doc_codigoInep = _txtCodigoInep.Text
                                                ,
                doc_situacao = Convert.ToByte(1)
                                                ,
                //IsNew = (_VS_col_id > 0) ? false : true
                IsNew = (_VS_doc_id > 0) ? false : true
            };

            PES_Pessoa entityPessoa = new PES_Pessoa
            {
                pes_id = UCCadastroPessoa1._VS_pes_id
                                              ,
                pes_nome = UCCadastroPessoa1._txtNome.Text
                                              ,
                pes_nome_abreviado = UCCadastroPessoa1._txtNomeAbreviado.Text
                                              ,
                pai_idNacionalidade = UCCadastroPessoa1.ComboNacionalidade1_Valor
                                              ,
                pes_naturalizado = UCCadastroPessoa1._chkNaturalizado.Checked
                                              ,
                cid_idNaturalidade = UCCadastroPessoa1._VS_cid_id
                                              ,
                pes_dataNascimento = (String.IsNullOrEmpty(UCCadastroPessoa1._txtDataNasc.Text) ? new DateTime() : Convert.ToDateTime(UCCadastroPessoa1._txtDataNasc.Text))
                                              ,
                pes_estadoCivil = Convert.ToByte(UCCadastroPessoa1._ComboEstadoCivil.SelectedValue == "-1" ? "0" : UCCadastroPessoa1._ComboEstadoCivil.SelectedValue)
                                              ,
                pes_racaCor = Convert.ToByte(UCCadastroPessoa1._ComboRacaCor.SelectedValue == "-1" ? "0" : UCCadastroPessoa1._ComboRacaCor.SelectedValue)
                                              ,
                pes_sexo = Convert.ToByte(UCCadastroPessoa1._ComboSexo.SelectedValue == "-1" ? "0" : UCCadastroPessoa1._ComboSexo.SelectedValue)
                                              ,
                pes_idFiliacaoPai = UCCadastroPessoa1._VS_pes_idFiliacaoPai
                                              ,
                pes_idFiliacaoMae = UCCadastroPessoa1._VS_pes_idFiliacaoMae
                                              ,
                tes_id = new Guid(UCCadastroPessoa1._ComboEscolaridade.SelectedValue)
                                              ,
                pes_situacao = 1
                                              ,
                IsNew = (UCCadastroPessoa1._VS_pes_id != Guid.Empty) ? false : true
            };

            CFG_Arquivo entArquivo = null;

            if (!string.IsNullOrEmpty(UCCadastroPessoa1._iptFoto.PostedFile.FileName))
            {
                string tam = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TAMANHO_MAX_FOTO_PESSOA);

                if (!string.IsNullOrEmpty(tam))
                {
                    if (UCCadastroPessoa1._iptFoto.PostedFile.ContentLength > Convert.ToInt32(tam) * 1000)
                    {
                        txtSelectedTab.Value = "0";
                        throw new ArgumentException("Foto é maior que o tamanho máximo permitido.");
                    }

                    if (UCCadastroPessoa1._iptFoto.PostedFile.FileName.Substring(UCCadastroPessoa1._iptFoto.PostedFile.FileName.Length - 3, 3).ToUpper() != "JPG")
                    {
                        txtSelectedTab.Value = "0";
                        throw new ArgumentException("Foto tem que estar no formato \".jpg\".");
                    }
                }

                entArquivo = CFG_ArquivoBO.CriarEntidadeArquivo(UCCadastroPessoa1._iptFoto.PostedFile);

                if (_VS_arq_idAntigo > 0)
                {
                    // Se já existia foto e vai ser alterada, muda só o conteúdo.
                    entArquivo.arq_id = _VS_arq_idAntigo;
                    entArquivo.IsNew = false;
                }
            }

            if (_VS_arq_idAntigo > 0)
            {
                entityPessoa.arq_idFoto = _VS_arq_idAntigo;
            }

            PES_PessoaDeficiencia entityPessoaDeficiencia = new PES_PessoaDeficiencia
            {
                pes_id = UCCadastroPessoa1._VS_pes_id
                                                                    ,
                tde_id = UCCadastroPessoa1.ComboTipoDeficiencia1_Valor
                                                                    ,
                IsNew = true
            };

            RHU_Colaborador entityColaborador = new RHU_Colaborador
            {
                col_id = _VS_col_id
                                                        ,
                ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id
                                                        ,
                col_dataAdmissao = (String.IsNullOrEmpty(_txtDataAdmissao.Text) ? new DateTime() : Convert.ToDateTime(_txtDataAdmissao.Text))
                                                        ,
                col_dataDemissao = (String.IsNullOrEmpty(_txtDataDemissao.Text) ? new DateTime() : Convert.ToDateTime(_txtDataDemissao.Text))
                                                        ,
                col_situacao = Convert.ToByte(UCComboColaboradorSituacao1.Valor > 0 ? UCComboColaboradorSituacao1.Valor : 0)
                                                        ,
                col_controladoIntegracao = _VS_col_controladoIntegracao
                                                        ,
                IsNew = (_VS_col_id > 0) ? false : true
            };

            SYS_Usuario entityUsuario = new SYS_Usuario { usu_id = _VS_usu_id };
            SYS_UsuarioBO.GetEntity(entityUsuario);
            if (_chbCriarUsuario.Checked)
            {
                entityUsuario.usu_login = _txtLogin.Text;
                entityUsuario.usu_email = _txtEmail.Text;
                entityUsuario.usu_senha = string.IsNullOrEmpty(_txtSenha.Text) ? _txtSenha.Attributes["value"] : _txtSenha.Text;
                entityUsuario.pes_id = UCCadastroPessoa1._VS_pes_id;
                entityUsuario.ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
                entityUsuario.usu_situacao = 1;
                entityUsuario.usu_dataCriacao = DateTime.Now;
                entityUsuario.usu_dataAlteracao = DateTime.Now;
                entityUsuario.usu_dominio = (_ddlDominios.SelectedIndex == _VS_OutrosDominios) ? _txtDominio.Text : _ddlDominios.SelectedValue;

                entityUsuario.IsNew = (_VS_usu_id != Guid.Empty) ? false : true;

                if (_chkExpiraSenha.Checked)
                    entityUsuario.usu_situacao = 5;
                if (_chkBloqueado.Checked)
                    entityUsuario.usu_situacao = 2;
            }

            // Configura entidade PES_CertidaoCivil
            PES_CertidaoCivil entityCertidaoCivil;
            UCCertidaoCivil1.RetornaCertidaoCivil(out entityCertidaoCivil);

            if (ACA_DocenteBO.Save(entityPessoa
                                   , entityPessoaDeficiencia
                                   , UCEnderecos1._VS_enderecos
                                   , UCContato1._carregaDataTableComContatos
                                   , UCGridDocumento1.RetornaDocumentoSave()
                                   , entityCertidaoCivil
                                   , _VS_pai_idAntigo
                                   , _VS_cid_idAntigo
                                   , UCCadastroPessoa1.InformacoesPai
                                   , UCCadastroPessoa1.InformacoesMae
                                   , _VS_tes_idAntigo
                                   , _VS_tde_idAntigo
                                   , entityColaborador
                                   , UCCadastroCargo1._VS_cargos
                                   , UCCadastroCargo1._VS_cargosDisciplinas
                                   , _chbCriarUsuario.Checked
                                   , _ckbUsuarioLive.Checked
                                   , entityUsuario
                                   , _chkSenhaAutomatica.Checked
                                   , __SessionWEB.TituloGeral
                                   , ApplicationWEB._EmailHost
                                   , ApplicationWEB._EmailSuporte
                                   , __SessionWEB.__UsuarioWEB.Usuario.ent_id
                                   , entityDocente
                                   , ApplicationWEB.TipoImagensPermitidas
                                   , ApplicationWEB.TamanhoMaximoArquivo
                                   , entArquivo
                                   , UCCadastroPessoa1._chbExcluirImagem.Checked))
            {
                if (_VS_col_id <= 0)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "doc_id: " + entityDocente.doc_id + (entityUsuario.usu_id != Guid.Empty ? "; usu_id: " + entityUsuario.usu_id : ""));
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Docente incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "doc_id: " + entityDocente.doc_id + (entityUsuario.usu_id != Guid.Empty ? "; usu_id: " + entityUsuario.usu_id : ""));
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Docente alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }

                //Grava log de inclusão/alteração/exclusão de usuário
                if (_VS_usu_id != Guid.Empty)
                {
                    if (!_chbCriarUsuario.Checked)
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "usu_id: " + _VS_usu_id + "; doc_id: " + entityDocente.doc_id);
                    else
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "usu_id: " + entityUsuario.usu_id + "; doc_id: " + entityDocente.doc_id);
                }
                else if (entityUsuario.usu_id != Guid.Empty)
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "usu_id: " + entityUsuario.usu_id + "; doc_id: " + entityDocente.doc_id);

                if (Session["col_id"] != null)
                {
                    Session.Remove("col_id");
                    RedirecionarPagina(__SessionWEB._AreaAtual._Diretorio + "/Academico/RecursosHumanos/Colaborador/Busca.aspx");
                }
                else if (Session["pes_id"] != null)
                {
                    Session.Remove("pes_id");
                    RedirecionarPagina(__SessionWEB._AreaAtual._Diretorio + "/Academico/RecursosHumanos/Pessoas/Busca.aspx");
                }
                else
                {
                    Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/RecursosHumanos/Docente/Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            else
            {
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o docente.", UtilBO.TipoMensagem.Erro);
            }
        }
        catch (WebControls_Endereco_UCEnderecos.EnderecoException ex)
        {
            txtSelectedTab.Value = "1";
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (ValidationException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (ArgumentException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (DuplicateNameException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o docente.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Retorna o código da UA caso o parametro MostraCodigoEscola seja True
    /// </summary>
    /// <param name="uad_id">ID da UA</param>
    /// <returns></returns>
    public string CarregaCodigoEscola(Guid uad_id)
    {
        bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

        if (MostraCodigoEscola)
        {
            SYS_UnidadeAdministrativa uad = new SYS_UnidadeAdministrativa { uad_id = uad_id, ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id };
            SYS_UnidadeAdministrativaBO.GetEntity(uad);

            return (string.IsNullOrEmpty(uad.uad_codigo) ? "" : uad.uad_codigo + " - ");
        }

        return "";
    }

    void _AtualizarUsuarioLive()
    {
        if (_ckbUsuarioLive.Checked)
        {
            LabelLogin.Text = "Login";
            /****** ATENÇÃO - comentário temporário ******/
            //Label2.Text = "E-mail (seuEmail@rioeduca.net - caso não preenchido ou não existir, será criado uma nova conta live)";
            /*********************************************/
            _lblSenha.Text = "Senha";
            _lblConfirmacao.Text = "Confirmar senha";
        }
        else
        {
            LabelLogin.Text = "Login *";
            _lblSenha.Text = "Senha *";
            _lblConfirmacao.Text = "Confirmar senha *";
        }

        VerificaObrigatoriedadeEmail();

        _ckbUsuarioAD.Visible = !_ckbUsuarioLive.Checked;
        LabelLogin.Visible = !_ckbUsuarioLive.Checked;
        _txtLogin.Visible = !_ckbUsuarioLive.Checked;
        _rfvLogin.Enabled = !_ckbUsuarioLive.Checked;
        _chkSenhaAutomatica.Visible = !_ckbUsuarioLive.Checked;

        _lblSenha.Visible = (!_ckbUsuarioLive.Checked) || (_VS_usu_id != Guid.Empty);
        _txtSenha.Visible = !_ckbUsuarioLive.Checked || (_VS_usu_id != Guid.Empty);
        _rfvSenha.Enabled = !_ckbUsuarioLive.Checked || (_VS_usu_id != Guid.Empty);
        _lblConfirmacao.Visible = !_ckbUsuarioLive.Checked || (_VS_usu_id != Guid.Empty);
        _txtConfirmacao.Visible = !_ckbUsuarioLive.Checked || (_VS_usu_id != Guid.Empty);
        _rfvConfirmarSenha.Enabled = !_ckbUsuarioLive.Checked || (_VS_usu_id != Guid.Empty);
        _chkExpiraSenha.Visible = !_ckbUsuarioLive.Checked || (_VS_usu_id != Guid.Empty);
        _chkBloqueado.Visible = !_ckbUsuarioLive.Checked || (_VS_usu_id != Guid.Empty);

        revSenhaTamanho.Enabled = (_ckbUsuarioLive.Checked) && (_VS_usu_id != Guid.Empty);
        revSenha.Enabled = (_ckbUsuarioLive.Checked) && (_VS_usu_id != Guid.Empty);
    }
    
    #region Cargos / Funções

    public void _CarregarCargoFuncao(DataTable _CargoFuncao)
    {
        _grvCargosFuncoes.DataSource = _CargoFuncao;
        _grvCargosFuncoes.DataBind();
        _updGridCargosFuncoes.Update();
    }

    private void _AlterarCargo(int crg_id, int seqcrg_id)
    {
        try
        {
            UCCadastroCargo1._VS_IsNew = false;
            UCCadastroCargo1._VS_id = crg_id;
            UCCadastroCargo1._VS_Coc_ID = seqcrg_id;
            UCCadastroCargo1.ComboCargoPermiteEditar = false;
            UCCadastroCargo1.VS_coc_controladoIntegracao = false;

            UCCadastroCargo1._VS_dataAdmissao = (!string.IsNullOrEmpty(_txtDataAdmissao.Text) ? Convert.ToDateTime(_txtDataAdmissao.Text) : new DateTime());

            UCCadastroCargo1.CarregarDisciplinas(crg_id, seqcrg_id, _VS_col_id);

            for (int i = 0; i < UCCadastroCargo1._VS_cargos.Rows.Count; i++)
            {
                if (UCCadastroCargo1._VS_cargos.Rows[i].RowState != DataRowState.Deleted)
                {
                    if (UCCadastroCargo1._VS_cargos.Rows[i]["crg_id"].ToString() == Convert.ToString(crg_id) && (UCCadastroCargo1._VS_cargos.Rows[i]["seqcrg_id"].ToString() == Convert.ToString(seqcrg_id)))
                    {
                        UCCadastroCargo1._VS_uad_id = new Guid(UCCadastroCargo1._VS_cargos.Rows[i]["uad_id"].ToString());
                        UCCadastroCargo1.ComboCargoValor = string.IsNullOrEmpty(UCCadastroCargo1._VS_cargos.Rows[i]["crg_id"].ToString()) ? -1 : Convert.ToInt32(UCCadastroCargo1._VS_cargos.Rows[i]["crg_id"].ToString());
                        UCCadastroCargo1._txtMatricula.Text = UCCadastroCargo1._VS_cargos.Rows[i]["coc_matricula"].ToString();
                        UCCadastroCargo1._txtVigenciaIni.Text = UCCadastroCargo1._VS_cargos.Rows[i]["vigenciaini"].ToString();
                        UCCadastroCargo1._txtVigenciaFim.Text = UCCadastroCargo1._VS_cargos.Rows[i]["vigenciafim"].ToString();
                        UCCadastroCargo1._txtUA.Text = UCCadastroCargo1._VS_cargos.Rows[i]["uad_nome"].ToString();
                        UCCadastroCargo1.ValorSituacao = Convert.ToInt32(UCCadastroCargo1._VS_cargos.Rows[i]["situacao_id"].ToString());
                        UCCadastroCargo1._txtObservacao.Text = UCCadastroCargo1._VS_cargos.Rows[i]["observacao"].ToString();
                        UCCadastroCargo1._ckbVinculoSede.Checked = UCCadastroCargo1._VS_cargos.Rows[i]["coc_vinculoSede"].ToString() == "True" ? true : false;
                        UCCadastroCargo1._ckbVinculoExtra.Checked = UCCadastroCargo1._VS_cargos.Rows[i]["coc_vinculoExtra"].ToString() == "True" ? true : false;
                        UCCadastroCargo1._ckbColaboradorReadaptado.Checked = UCCadastroCargo1._VS_cargos.Rows[i]["coc_readaptado"].ToString() == "True" ? true : false;
                        UCCadastroCargo1._ckbComplementacaoCargaHoraria.Checked = UCCadastroCargo1._VS_cargos.Rows[i]["coc_complementacaoCargaHoraria"].ToString() == "True" ? true : false;
                        UCCadastroCargo1.txtDataInicioMatricula = UCCadastroCargo1._VS_cargos.Rows[i]["coc_dataInicioMatricula"].ToString();
                        UCCadastroCargo1.VS_coc_controladoIntegracao = Convert.ToBoolean(UCCadastroCargo1._VS_cargos.Rows[i]["controladoIntegracao"]);

                        RHU_Cargo cargo = new RHU_Cargo
                        {
                            crg_id = crg_id
                        };
                        RHU_CargoBO.GetEntity(cargo);

                        UCCadastroCargo1.ComboCargaHorariaCarregar(cargo);
                        UCCadastroCargo1.ValorCargaHoraria =
                            string.IsNullOrEmpty(UCCadastroCargo1._VS_cargos.Rows[i]["chr_id"].ToString())
                            ? -1 :
                            Convert.ToInt32(UCCadastroCargo1._VS_cargos.Rows[i]["chr_id"].ToString());

                        break;
                    }
                }
            }

            bool controlarVinculoApenasIntegracao = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_COLABORADOR_APENAS_INTEGRACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            bool vinculoIntegradoVirtual = !controlarVinculoApenasIntegracao && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_COLABORADOR_VINCULO_INTEGRADO_VIRTUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            bool podeEditarVinculo = vinculoIntegradoVirtual && !UCCadastroCargo1.VS_coc_controladoIntegracao;

            bool podeEditar = (((__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && _VS_col_id > 0) ||
                               (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && _VS_col_id <= 0))) &&
                               podeEditarVinculo;

            if (!podeEditar)
            {
                HabilitaControles(UCCadastroCargo1.Controls, false);
                UCCadastroCargo1._BotaoCancelar.Enabled = true;

                UCCadastroCargo1._Botao.Visible = false;
                UCCadastroCargo1._BotaoCancelar.Text = "Voltar";
                UCCadastroCargo1.Page.Form.DefaultButton = UCCadastroCargo1._BotaoCancelar.UniqueID;
            }
            else
            {
                UCCadastroCargo1._Botao.Text = "Alterar";
            }

            UCCadastroCargo1.ConfiguraCadastro();
            UCCadastroCargo1.ComboCargoSetarFoco();
            UCCadastroCargo1._updCadastroCargo.Update();

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "AlterarCargo", "$('#divCargos').dialog('open');", true);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar cargo.", UtilBO.TipoMensagem.Erro);
        }
    }

    private void _ExcluirCargo(int crg_id, int seqcrg_id)
    {
        for (int i = 0; i < UCCadastroCargo1._VS_cargos.Rows.Count; i++)
        {
            if (UCCadastroCargo1._VS_cargos.Rows[i].RowState != DataRowState.Deleted)
            {
                if (UCCadastroCargo1._VS_cargos.Rows[i]["crg_id"].ToString() == Convert.ToString(crg_id) && (UCCadastroCargo1._VS_cargos.Rows[i]["seqcrg_id"].ToString() == Convert.ToString(seqcrg_id)))
                {
                    UCCadastroCargo1._VS_cargos.Rows[i].Delete();
                    break;
                }
            }
        }

        _grvCargosFuncoes.DataSource = UCCadastroCargo1._VS_cargos;
        UCCadastroFuncao1._VS_funcoes = UCCadastroCargo1._VS_cargos;
        _grvCargosFuncoes.DataBind();
        _updGridCargosFuncoes.Update();
    }

    private void _AlterarFuncao(int fun_id, int seqfun_id)
    {
        UCCadastroFuncao1._VS_IsNew = false;
        UCCadastroFuncao1._VS_id = fun_id;
        UCCadastroFuncao1._VS_seq = seqfun_id;
        UCCadastroFuncao1.ComboFuncaoPermiteEditar = false;
        UCCadastroFuncao1.VS_cof_controladoIntegracao = false;

        for (int i = 0; i < UCCadastroFuncao1._VS_funcoes.Rows.Count; i++)
        {
            if (UCCadastroFuncao1._VS_funcoes.Rows[i].RowState != DataRowState.Deleted)
            {
                if (UCCadastroFuncao1._VS_funcoes.Rows[i]["fun_id"].ToString() == Convert.ToString(fun_id) && (UCCadastroFuncao1._VS_funcoes.Rows[i]["seqfun_id"].ToString() == Convert.ToString(seqfun_id)))
                {
                    UCCadastroFuncao1._VS_uad_id = new Guid(UCCadastroFuncao1._VS_funcoes.Rows[i]["uad_id"].ToString());
                    UCCadastroFuncao1.ComboFuncaoValor = string.IsNullOrEmpty(UCCadastroFuncao1._VS_funcoes.Rows[i]["fun_id"].ToString()) ? -1 : Convert.ToInt32(UCCadastroFuncao1._VS_funcoes.Rows[i]["fun_id"].ToString());
                    UCCadastroFuncao1._txtMatricula.Text = UCCadastroFuncao1._VS_funcoes.Rows[i]["coc_matricula"].ToString();
                    UCCadastroFuncao1._txtVigenciaIni.Text = UCCadastroFuncao1._VS_funcoes.Rows[i]["vigenciaini"].ToString();
                    UCCadastroFuncao1._txtVigenciaFim.Text = UCCadastroFuncao1._VS_funcoes.Rows[i]["vigenciafim"].ToString();
                    UCCadastroFuncao1._txtUA.Text = UCCadastroFuncao1._VS_funcoes.Rows[i]["uad_nome"].ToString();
                    UCCadastroFuncao1._chkResponsavelUA.Checked = UCCadastroFuncao1._VS_funcoes.Rows[i]["cof_responsavelUA"].ToString() == "True" ? true : false;
                    UCCadastroFuncao1.ValorSituacao = Convert.ToInt32(UCCadastroFuncao1._VS_funcoes.Rows[i]["situacao_id"].ToString());
                    UCCadastroFuncao1._txtObservacao.Text = UCCadastroFuncao1._VS_funcoes.Rows[i]["observacao"].ToString();
                    UCCadastroFuncao1.VS_cof_controladoIntegracao = Convert.ToBoolean(UCCadastroFuncao1._VS_funcoes.Rows[i]["controladoIntegracao"]);
                    break;
                }
            }
        }

        bool controlarVinculoApenasIntegracao = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_COLABORADOR_APENAS_INTEGRACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        bool vinculoIntegradoVirtual = !controlarVinculoApenasIntegracao && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_COLABORADOR_VINCULO_INTEGRADO_VIRTUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

        bool podeEditarVinculo = vinculoIntegradoVirtual && !UCCadastroFuncao1.VS_cof_controladoIntegracao;

        bool podeEditar = (((__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && _VS_col_id > 0) ||
                           (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && _VS_col_id <= 0))) &&
                           podeEditarVinculo;

        if (!podeEditar)
        {
            HabilitaControles(UCCadastroFuncao1.Controls, false);
            UCCadastroFuncao1._BotaoCancelar.Enabled = true;

            UCCadastroFuncao1._Botao.Visible = false;
            UCCadastroFuncao1._BotaoCancelar.Text = "Voltar";
            UCCadastroFuncao1.Page.Form.DefaultButton = UCCadastroCargo1._BotaoCancelar.UniqueID;
        }
        else
        {
            UCCadastroFuncao1._Botao.Text = "Alterar";
        }

        UCCadastroFuncao1.ConfiguraCadastro();
        UCCadastroFuncao1.ComboFuncaoSetarFoco();
        UCCadastroFuncao1._updCadastroFuncao.Update();

        ScriptManager.RegisterClientScriptBlock(this, GetType(), "AlterarFuncao", "$('#divFuncoes').dialog('open');", true);
    }

    private void _ExcluirFuncao(int fun_id, int seqfun_id)
    {
        for (int i = 0; i < UCCadastroFuncao1._VS_funcoes.Rows.Count; i++)
        {
            if (UCCadastroFuncao1._VS_funcoes.Rows[i].RowState != DataRowState.Deleted)
            {
                if (UCCadastroFuncao1._VS_funcoes.Rows[i]["fun_id"].ToString() == Convert.ToString(fun_id) && (UCCadastroFuncao1._VS_funcoes.Rows[i]["seqfun_id"].ToString() == Convert.ToString(seqfun_id)))
                {
                    UCCadastroFuncao1._VS_funcoes.Rows[i].Delete();
                    break;
                }
            }
        }

        _grvCargosFuncoes.DataSource = UCCadastroFuncao1._VS_funcoes;
        UCCadastroCargo1._VS_cargos = UCCadastroFuncao1._VS_funcoes;
        _grvCargosFuncoes.DataBind();
        _updGridCargosFuncoes.Update();
    }

    #endregion

    #region Usuário

    private void _ddlDominios_AddItems(string text, string value)
    {
        ListItem item = new ListItem { Text = text, Value = value };
        _ddlDominios.Items.Add(item);
    }

    private void _CarregarComboDominios()
    {
        //Carregar domínios.
        IList<string> lsDominios;
        try
        {
            lsDominios = MSTech.LDAP.LdapUtil.GetDomainNames();
        }
        catch
        {
            lsDominios = new List<string>();
        }
        _ddlDominios.Items.Clear();
        _ddlDominios_AddItems("-- Selecione um domínio --", string.Empty);
        int cont = 0; //Índice do selecione.
        foreach (string dominio in lsDominios)
        {
            string strDominio = dominio.Split('.')[0];
            _ddlDominios_AddItems(strDominio, strDominio);
            cont++;
        }
        _VS_OutrosDominios = ++cont;
        _ddlDominios_AddItems("Outros domínios...", "Outros domínios...");
    }

    private void _TrataOutrosDominios()
    {
        divOutrosDominios.Visible = _ddlDominios.SelectedIndex == _VS_OutrosDominios;
        _ddlDominios.Focus();
    }

    private void _TrataUsuarioAD()
    {
        if (_ckbUsuarioAD.Checked)
        {
            divDominios.Visible = true;
            _TrataOutrosDominios();
            divOpcoesSenha.Visible = false;
            _rfvConfirmarSenha.Enabled = false;
            _rfvSenha.Enabled = false;
            _txtSenha.Text = string.Empty;
            _txtConfirmacao.Text = string.Empty;
            _chkSenhaAutomatica.Checked = false;
            _chkSenhaAutomatica_CheckedChanged(null, null);
            _ckbUsuarioLive.Checked = false;
            VerificaObrigatoriedadeEmail();
            _ckbUsuarioLive.Visible = false;
        }
        else
        {
            divDominios.Visible = false;
            divOpcoesSenha.Visible = true;
            _ddlDominios.SelectedValue = string.Empty;
            _txtDominio.Text = string.Empty;
            _rfvConfirmarSenha.Enabled = true;
            _rfvSenha.Enabled = true;
            _txtSenha.Text = string.Empty;
            _txtConfirmacao.Text = string.Empty;
            _chkSenhaAutomatica.Checked = false;
            _chkSenhaAutomatica_CheckedChanged(null, null);
            _ckbUsuarioLive.Visible = VerificaIntegracaoExterna();
        }
    }

    private bool VerificaIntegracaoExterna()
    {
        ManageUserLive live = new ManageUserLive();
        return live.ExistsIntegracaoExterna();
    }

    #endregion

    #region Delegates

    private void UCPessoas1BuscaPessoa(IDictionary<string, object> parameters)
    {
        if (UCCadastroPessoa1._VS_tipoBuscaPessoa == 1)
        {
            UCCadastroPessoa1._VS_pes_idFiliacaoPai = new Guid(parameters["pes_id"].ToString());
            UCCadastroPessoa1._txtPai.Text = parameters["pes_nome"].ToString();
            PES_PessoaDocumento cpf = new PES_PessoaDocumento
            {
                pes_id = new Guid(parameters["pes_id"].ToString())
                ,
                tdo_id = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF))
            };
            PES_PessoaDocumentoBO.GetEntity(cpf);
            PES_PessoaDocumento rg = new PES_PessoaDocumento
            {
                pes_id = new Guid(parameters["pes_id"].ToString())
                ,
                tdo_id = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_RG))
            };
            PES_PessoaDocumentoBO.GetEntity(rg);
            UCCadastroPessoa1.txtCPFPaiValor = cpf.psd_numero;
            UCCadastroPessoa1.txtRGPaiValor = rg.psd_numero;
            UCCadastroPessoa1._updCadastroPessoas.Update();
            UCCadastroPessoa1.visibleLimparPai = true;
        }
        else if (UCCadastroPessoa1._VS_tipoBuscaPessoa == 2)
        {
            UCCadastroPessoa1._VS_pes_idFiliacaoMae = new Guid(parameters["pes_id"].ToString());
            UCCadastroPessoa1._txtMae.Text = parameters["pes_nome"].ToString();
            PES_PessoaDocumento cpf = new PES_PessoaDocumento
            {
                pes_id = new Guid(parameters["pes_id"].ToString())
                ,
                tdo_id = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF))
            };
            PES_PessoaDocumentoBO.GetEntity(cpf);
            PES_PessoaDocumento rg = new PES_PessoaDocumento
            {
                pes_id = new Guid(parameters["pes_id"].ToString())
                ,
                tdo_id = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_RG))
            };

            PES_PessoaDocumentoBO.GetEntity(rg);
            UCCadastroPessoa1.txtCPFMaeValor = cpf.psd_numero;
            UCCadastroPessoa1.txtRGMaeValor = rg.psd_numero;
            UCCadastroPessoa1._updCadastroPessoas.Update();
            UCCadastroPessoa1.visibleLimparMae = true;
        }
    }

    private void UCUA1BuscaUA(IDictionary<string, object> parameters)
    {
        if (_VS_tipoBuscaUA > 0)
        {
            string codigo = CarregaCodigoEscola(new Guid(parameters["uad_id"].ToString()));

            if (_VS_tipoBuscaUA == 1)
            {
                UCCadastroCargo1._VS_uad_id = new Guid(parameters["uad_id"].ToString());
                UCCadastroCargo1._txtUA.Text = codigo + parameters["uad_nome"];
                UCCadastroCargo1._updCadastroCargo.Update();
            }
            else if (_VS_tipoBuscaUA == 2)
            {
                UCCadastroFuncao1._VS_uad_id = new Guid(parameters["uad_id"].ToString());
                UCCadastroFuncao1._txtUA.Text = codigo + parameters["uad_nome"];
                UCCadastroFuncao1._updCadastroFuncao.Update();
            }
        }
    }

    private void UCCadastroPessoa1__Seleciona()
    {
        UCPessoas1._Limpar();
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "BuscaPessoa", "$('#divBuscaPessoa').dialog('open');", true);
        _updBuscaPessoa.Update();
    }

    /// <summary>
    /// Seta campos do cadastro de cidade.
    /// </summary>
    private void _AbreJanelaCadastroCidade()
    {
        if (!UCCadastroCidade1.Visible)
            UCCadastroCidade1.CarregarCombos();

        UCCadastroCidade1.Visible = true;
        UCCadastroCidade1.SetaFoco();

        _updCidades.Update();

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroMatriculaNovo", "$('#divCadastroCidade').dialog('open');", true);
    }

    private void UCGridDocumento1_TextChanged()
    {
        if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PREENCHER_LOGIN_SENHA_AUTOMATICAMENTE_COLABORADORES_DOCENTES, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
        {
            if (!string.IsNullOrEmpty(UCGridDocumento1.VS_CPF) && UCGridDocumento1.VS_CPF.Length >= 11)
            {
                _txtLogin.Text = UCGridDocumento1.VS_CPF;
                _txtLogin.Enabled = false;
                _txtSenha.Text = UCGridDocumento1.VS_CPF.Substring(UCGridDocumento1.VS_CPF.Length - 4, 4);
                _txtConfirmacao.Text = UCGridDocumento1.VS_CPF.Substring(UCGridDocumento1.VS_CPF.Length - 4, 4);
                _txtSenha.Attributes.Add("value", UCGridDocumento1.VS_CPF.Substring(UCGridDocumento1.VS_CPF.Length - 4, 4));
                _txtConfirmacao.Attributes.Add("value", UCGridDocumento1.VS_CPF.Substring(UCGridDocumento1.VS_CPF.Length - 4, 4));
                _txtSenha.Enabled = false;
                _txtConfirmacao.Enabled = false;

                _updUsuarios.Update();
            }
        }
    }

    #region CARGOS / FUNCOES

    private void UCCadastroCargo1__Selecionar()
    {
        _VS_tipoBuscaUA = 1;
        UCUA1._Limpar();
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "BuscaUACargo", "$('#divBuscaUA').dialog('open');", true);
        _updBuscaUA.Update();
    }

    private void UCCadastroFuncao1__Selecionar()
    {
        _VS_tipoBuscaUA = 2;
        UCUA1._Limpar();
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "BuscaUAFuncao", "$('#divBuscaUA').dialog('open');", true);
        _updBuscaUA.Update();
    }

    private void UCCadastroCargo1__Incluir(DataTable dt)
    {
        UCCadastroFuncao1._VS_funcoes = dt;

        _grvCargosFuncoes.DataSource = dt;
        _grvCargosFuncoes.DataBind();
        _updGridCargosFuncoes.Update();

        UCCadastroCargo1._Botao.Text = "Incluir";

        ScriptManager.RegisterClientScriptBlock(this, GetType(), "CadastroCargo", "$('#divCargos').dialog('close');", true);
    }

    private void UCCadastroFuncao1__Incluir(DataTable dt)
    {
        UCCadastroCargo1._VS_cargos = dt;

        _grvCargosFuncoes.DataSource = dt;
        _grvCargosFuncoes.DataBind();
        _updGridCargosFuncoes.Update();

        UCCadastroFuncao1._Botao.Text = "Incluir";

        ScriptManager.RegisterClientScriptBlock(this, GetType(), "CadastroFuncao", "$('#divFuncoes').dialog('close');", true);
    }

    #endregion

    #endregion

    private void ValidarCampos()
    {
        if (string.IsNullOrEmpty(_txtSenha.Text) && string.IsNullOrEmpty(_txtSenha.Attributes["value"]))
        {
            _rfvSenha.Enabled = true;
        }
        else
        {
            _rfvSenha.Enabled = false;
        }

        if (string.IsNullOrEmpty(_txtConfirmacao.Text) && string.IsNullOrEmpty(_txtConfirmacao.Attributes["value"]))
        {
            _rfvConfirmarSenha.Enabled = true;
        }
        else
        {
            _rfvConfirmarSenha.Enabled = false;
        }

        ManageUserLive live = new ManageUserLive();
        if ((live.IsContaEmail(_txtEmail.Text)) && (!_ckbUsuarioLive.Checked))
        {
            txtSelectedTab.Value = "5";
            throw new ValidationException("Integrar usuário live é obrigatório, o email " + _txtEmail.Text + " contém o domínio para integração com live.");
        }

        if ((_ckbUsuarioAD.Checked) && ((_ddlDominios.SelectedIndex == 0) || (_ddlDominios.SelectedIndex == _VS_OutrosDominios && string.IsNullOrEmpty(_txtDominio.Text))))
        {
            txtSelectedTab.Value = "5";
            throw new ValidationException("Domínio é obrigatório.");     
        }

        string msgErro;
        if (String.IsNullOrEmpty(_lblMessage.Text) && !UCContato1.SalvaConteudo(out msgErro))
        {
            UCContato1._MensagemErro_Mostrar = false;
            txtSelectedTab.Value = "1";
            throw new ValidationException(msgErro);
        }

        if (String.IsNullOrEmpty(_lblMessage.Text) && !UCGridDocumento1.ValidaConteudoGrid(out msgErro))
        {
            UCGridDocumento1._MensagemErro.Visible = false;
            txtSelectedTab.Value = "2";
            throw new ValidationException(msgErro);
        }

        if (!((from DataRow dr in UCGridDocumento1.RetornaDocumentoSave().Rows
               where dr.RowState != DataRowState.Deleted &&
                     dr["tdo_id"].ToString().Equals(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF))
               select dr).Any()) && ((_VS_col_id <= 0) || (!_VS_col_controladoIntegracao)))
        {
            UCGridDocumento1._MensagemErro.Visible = false;
            txtSelectedTab.Value = "2";
            throw new ValidationException("Documentação do tipo CPF é obrigatório.");
        }

        if (_ckbUsuarioLive.Checked)
        {
            /****** ATENÇÃO - validação temporária ******/
            if (!live.IsContaEmail(_txtEmail.Text))
                throw new ValidationException("E-mail inválido para integrar usuário live.");
            if (!live.VerificarContaEmailExistente(new UserLive { email = _txtEmail.Text }))
                throw new ValidationException("E-mail não encontrado no live para realizar integração.");
            /*******************************************/
        }
    }

    #endregion

    #region Eventos
    
    #region Cargos / Funções

    protected void _btnNovoCargo_Click(object sender, EventArgs e)
    {
        UCCadastroCargo1._LimparCampos();
        UCCadastroCargo1.VS_coc_controladoIntegracao = false;
        UCCadastroCargo1.ConfiguraCadastro();
        UCCadastroCargo1._updCadastroCargo.Update();
        UCCadastroCargo1.ComboCargoSetarFoco();
        UCCadastroCargo1.ComboCargoCarregarVerificandoControleIntegracaoDocente(false);
        UCCadastroCargo1.VS_IsDocente = true;

        UCCadastroCargo1._VS_dataAdmissao = (!string.IsNullOrEmpty(_txtDataAdmissao.Text) ? Convert.ToDateTime(_txtDataAdmissao.Text) : new DateTime());
        UCCadastroCargo1._VS_dataDemissao = (!string.IsNullOrEmpty(_txtDataDemissao.Text) ? Convert.ToDateTime(_txtDataDemissao.Text) : new DateTime());

        ScriptManager.RegisterClientScriptBlock(this, GetType(), "CadastroCargoNovo", "$('#divCargos').dialog('open');", true);
    }

    protected void _btnNovaFuncao_Click(object sender, EventArgs e)
    {
        UCCadastroFuncao1._LimparCampos();
        UCCadastroFuncao1.ConfiguraCadastro();
        UCCadastroFuncao1._updCadastroFuncao.Update();
        UCCadastroFuncao1.ComboFuncaoSetarFoco();

        UCCadastroFuncao1._VS_dataAdmissao = (!string.IsNullOrEmpty(_txtDataAdmissao.Text) ? Convert.ToDateTime(_txtDataAdmissao.Text) : new DateTime());
        UCCadastroFuncao1._VS_dataDemissao = (!string.IsNullOrEmpty(_txtDataDemissao.Text) ? Convert.ToDateTime(_txtDataDemissao.Text) : new DateTime());

        ScriptManager.RegisterClientScriptBlock(this, GetType(), "CadastroFuncaoNovo", "$('#divFuncoes').dialog('open');", true);
    }

    protected void _grvCargosFuncoes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer && e.Row.RowType != DataControlRowType.Pager)
        {
            string sUad_id = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "uad_id"));
            Guid uad_id = string.IsNullOrEmpty(sUad_id) ? Guid.Empty : new Guid(sUad_id);

            bool controlarVinculoApenasIntegracao = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_COLABORADOR_APENAS_INTEGRACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            bool vinculoIntegradoVirtual = !controlarVinculoApenasIntegracao && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_COLABORADOR_VINCULO_INTEGRADO_VIRTUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            bool podeEditarVinculo = vinculoIntegradoVirtual && (!Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "controladoIntegracao")));

            ImageButton _btnExcluir = (ImageButton)e.Row.FindControl("_btnExcluir");
            if (_btnExcluir != null)
            {
                _btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                _btnExcluir.Visible = __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao || VS_lista.Exists(p => p.Equals(uad_id)) && podeEditarVinculo;

                bool podeEditar = ((__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && _VS_col_id > 0) ||
                                   (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && _VS_col_id <= 0)) &&
                                  podeEditarVinculo;

                if (!podeEditar)
                {
                    _btnExcluir.Visible = false;
                }
            }

            LinkButton _btnAlterar = (LinkButton)e.Row.FindControl("_btnAlterar");
            if (_btnAlterar != null)
            {
                _btnAlterar.CommandArgument = e.Row.RowIndex.ToString();
                _btnAlterar.Visible = __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao || VS_lista.Exists(p => p.Equals(uad_id));
            }

            Label lblAlterar = (Label)e.Row.FindControl("_lblSelecionar");
            if (lblAlterar != null)
            {
                lblAlterar.Visible = !(__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao || VS_lista.Exists(p => p.Equals(uad_id)));
            }

            Label lblVinculoExtra = (Label)e.Row.FindControl("lblVinculoExtra");
            if (lblVinculoExtra != null)
            {
                bool coc_vinculoExtra;
                if (Boolean.TryParse(DataBinder.Eval(e.Row.DataItem, "coc_vinculoExtra").ToString(), out coc_vinculoExtra))
                {
                    lblVinculoExtra.Text = coc_vinculoExtra ? "Sim" : "Não";
                }
                else
                {
                    lblVinculoExtra.Text = string.Empty;
                }
            }
        }
    }

    protected void _grvCargosFuncoes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            int index = int.Parse(e.CommandArgument.ToString());

            int fun_id;
            if (!(int.TryParse(Convert.ToString(_grvCargosFuncoes.DataKeys[index].Values[2]), out fun_id)))
            {
                int crg_id = Convert.ToInt32(_grvCargosFuncoes.DataKeys[index].Values[0]);
                int seqcrg_id = Convert.ToInt32(_grvCargosFuncoes.DataKeys[index].Values[1]);
                _ExcluirCargo(crg_id, seqcrg_id);
            }
            else
            {
                fun_id = Convert.ToInt32(_grvCargosFuncoes.DataKeys[index].Values[2]);
                int seqfun_id = Convert.ToInt32(_grvCargosFuncoes.DataKeys[index].Values[3]);
                _ExcluirFuncao(fun_id, seqfun_id);
            }
        }

        if (e.CommandName == "Alterar")
        {
            int index = int.Parse(e.CommandArgument.ToString());

            int fun_id;
            if (!(int.TryParse(Convert.ToString(_grvCargosFuncoes.DataKeys[index].Values[2]), out fun_id)))
            {
                UCCadastroCargo1._LimparCampos();
                UCCadastroCargo1._btnPesquisarUA.Enabled = false;

                int crg_id = Convert.ToInt32(_grvCargosFuncoes.DataKeys[index].Values[0]);
                int seqcrg_id = Convert.ToInt32(_grvCargosFuncoes.DataKeys[index].Values[1]);
                _AlterarCargo(crg_id, seqcrg_id);
            }
            else
            {
                UCCadastroFuncao1._LimparCampos();
                UCCadastroFuncao1._btnPesquisarUA.Enabled = false;
                int seqfun_id = Convert.ToInt32(_grvCargosFuncoes.DataKeys[index].Values[3]);
                _AlterarFuncao(fun_id, seqfun_id);
            }
        }
    }

    #endregion

    #region Usuário

    protected void _chbCriarUsuario_CheckedChanged(object sender, EventArgs e)
    {
        if (_chbCriarUsuario.Checked)
        {
            divUsuarios.Visible = true;

            if (_VS_usu_id == Guid.Empty)
            {
                if (UCGridDocumento1._VS_documentos.Rows.Count > 0)
                {
                    Guid tdo_idCPF = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF));

                    DataTable tblCPF = UCGridDocumento1._VS_documentos.AsEnumerable()
                                            .Where(row => row.Field<Guid>("tdo_id") == tdo_idCPF)
                                            .CopyToDataTable();

                    string CPF = tblCPF.Rows[0]["numero"].ToString();

                    _txtLogin.Text = CPF;
                    _txtSenha.Text = CPF.Substring(CPF.Length - 4, 4);
                    _txtConfirmacao.Text = CPF.Substring(CPF.Length - 4, 4);
                    _txtSenha.Attributes.Add("value", CPF.Substring(CPF.Length - 4, 4));
                    _txtConfirmacao.Attributes.Add("value", CPF.Substring(CPF.Length - 4, 4));
                    _txtLogin.Enabled = false;
                    _txtSenha.Enabled = false;
                    _txtConfirmacao.Enabled = false;
                }
            }

            _txtLogin.Focus();
        }
        else
        {
            divUsuarios.Visible = false;
        }
    }

    protected void _chkSenhaAutomatica_CheckedChanged(object sender, EventArgs e)
    {
        if (_chkSenhaAutomatica.Checked)
        {
            _lblSenha.Visible = false;
            _lblConfirmacao.Visible = false;
            _txtSenha.Visible = false;
            _txtConfirmacao.Visible = false;

            _rfvSenha.Visible = false;
            _rfvConfirmarSenha.Visible = false;
            _cpvConfirmarSenha.Visible = false;

            _chkExpiraSenha.Enabled = false;
            _chkExpiraSenha.Visible = false;
            _chkExpiraSenha.Checked = true;

            _chkBloqueado.Checked = false;
            _chkBloqueado.Enabled = false;
            _chkBloqueado.Visible = false;
        }
        else
        {
            _lblSenha.Visible = true;
            _lblConfirmacao.Visible = true;
            _txtSenha.Visible = true;
            _txtConfirmacao.Visible = true;

            _rfvSenha.Visible = true;
            _rfvConfirmarSenha.Visible = true;
            _cpvConfirmarSenha.Visible = true;

            _chkExpiraSenha.Enabled = true;
            _chkExpiraSenha.Checked = false;
            _chkExpiraSenha.Visible = true;

            _chkBloqueado.Checked = false;
            _chkBloqueado.Enabled = true;
            _chkBloqueado.Visible = true;
        }

        _updUsuarios.Update();
    }

    protected void _ckbUsuarioAD_CheckedChanged(object sender, EventArgs e)
    {
        _TrataUsuarioAD();
    }

    protected void _ddlDominios_SelectedIndexChanged(object sender, EventArgs e)
    {
        _TrataOutrosDominios();
    }

    #endregion

    protected void _btnCancelar_Click(object sender, EventArgs e)
    {
        if (Session["col_id"] != null)
        {
            Session.Remove("col_id");
            RedirecionarPagina(__SessionWEB._AreaAtual._Diretorio + "Academico/RecursosHumanos/Colaborador/Busca.aspx");
        }
        else if (Session["pes_id"] != null)
        {
            Session.Remove("pes_id");
            RedirecionarPagina(__SessionWEB._AreaAtual._Diretorio + "Academico/RecursosHumanos/Pessoas/Busca.aspx");
        }
        else
        {
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/RecursosHumanos/Docente/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }

    protected void _btnSalvar_Click(object sender, EventArgs e)
    {
        try
        {
            ValidarCampos();

            if (Page.IsValid)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "ConfirmacaoFormacao", "$(document).ready(function(){ $('#divMensagemConfirmacaoFormacao').dialog('open'); });", true);
            }
        }
        catch (ValidationException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (ArgumentException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (DuplicateNameException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o docente.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void _ckbUsuarioLive_ChangeChecked(object sender, EventArgs e)
    {
        _AtualizarUsuarioLive();
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {        
        _Salvar();
    }

    #endregion
}
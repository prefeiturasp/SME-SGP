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

public partial class Academico_RecursosHumanos_Colaborador_Cadastro : MotherPageLogado
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
            {
                return Convert.ToInt64(ViewState["_VS_col_id"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_col_id"] = value;
        }
    }

    private int _VS_coc_id
    {
        get
        {
            if (ViewState["_VS_coc_id"] != null)
            {
                return (int)ViewState["_VS_coc_id"];
            }

            return -1;
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
            {
                return Convert.ToByte(ViewState["_VS_tipoBuscaUA"].ToString());
            }

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
            {
                return Convert.ToInt64(ViewState["_VS_arq_idAntigo"]);
            }

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
            {
                return new Guid(ViewState["_VS_pai_idAntigo"].ToString());
            }

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
            {
                return new Guid(ViewState["_VS_cid_idAntigo"].ToString());
            }

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
            {
                return new Guid(ViewState["_VS_tes_idAntigo"].ToString());
            }

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
            {
                return new Guid(ViewState["_VS_tde_idAntigo"].ToString());
            }

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
            {
                return new Guid(ViewState["_VS_usu_id"].ToString());
            }

            return Guid.Empty;
        }

        set
        {
            ViewState["_VS_usu_id"] = value;
        }
    }

    // Armazena o índice no combo da opção de outros domínios.
    private int _VS_OutrosDominios
    {
        get
        {
            if (ViewState["_VS_OutrosDominios"] != null)
            {
                return (int)ViewState["_VS_OutrosDominios"];
            }

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
            {
                return Convert.ToBoolean(ViewState["_VS_col_controladoIntegracao"]);
            }

            return false;
        }

        set
        {
            ViewState["_VS_col_controladoIntegracao"] = value;
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
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.ExitPageConfirm));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsSetExitPageConfirmer.js"));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroPessoa.js"));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroColaborador.js"));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsUCCadastroEndereco.js"));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroCertidaoCivil.js"));
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

        // Cadastro de cidades
        UCCadastroCidade1.Inicialize(ApplicationWEB._Paginacao, "divCadastroCidade");

        if (!IsPostBack)
        {
            try
            {
                cvDataAdmissao.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de admissão");
                cvDataDemissao.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de demissão");

                UCCadastroPessoa1._labelNome.Text = "Nome do colaborador *";
                UCCadastroPessoa1.rfvNome.ErrorMessage = "Nome do colaborador é obrigatório.";
                UCCadastroPessoa1._lblPai.Text = "Nome do Pai";
                UCCadastroPessoa1._lblMae.Text = "Nome da Mãe";
                _CarregarComboDominios();

                UCCadastroCargo1.ComboCargoCarregar(null);
                UCCadastroCargo1._VS_IsNew = true;

                UCEnderecos1.Inicializar(false, false, "Pessoa", true, true);

                if ((PreviousPage != null) && PreviousPage.IsCrossPagePostBack)
                {
                    _VS_col_id = PreviousPage.EditItem;
                    _LoadFromEntity();
                }
                else if (Session["pes_id"] != null)
                {
                    CarregaPorPessoa(new Guid(Session["pes_id"].ToString()));
                }
                else
                {
                    string pais_padrao = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.PAIS_PADRAO_BRASIL);

                    if (!string.IsNullOrEmpty(pais_padrao))
                    {
                        UCCadastroPessoa1.ComboNacionalidade1_Valor = new Guid(pais_padrao.ToLower());
                    }

                    UCGridDocumento1._CarregarDocumento(Guid.Empty);

                    UCCertidaoCivil1.Inicializa("Pessoa");

                    _grvCargosFuncoes.DataSource = new DataTable();
                    _grvCargosFuncoes.DataBind();

                    _ckbUserLive.Visible = VerificaIntegracaoExterna();
                }

                RHU_Colaborador entityColaborador = new RHU_Colaborador
                {
                    col_id = _VS_col_id
                };
                RHU_ColaboradorBO.GetEntity(entityColaborador);
                UCContato1.CarregarContatosDoBanco(entityColaborador.pes_id);

                UCCadastroFuncao1.ComboFuncaoCarregar();
                UCCadastroFuncao1._VS_IsNew = true;

                UCComboColaboradorSituacao1.Obrigatorio = true;
                UCComboColaboradorSituacao1.ValidationGroup = "Pessoa";

                UCCadastroPessoa1._VS_tipoPessoa = 3;

                if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PREENCHER_LOGIN_SENHA_AUTOMATICAMENTE_COLABORADORES_DOCENTES, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    _txtLogin.Enabled = false;
                    _txtSenha.Enabled = false;
                    _txtConfirmacao.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                divColaborador.Visible = false;
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }

            Page.Form.DefaultFocus = UCCadastroPessoa1._txtNome.ClientID;
            Page.Form.DefaultButton = _btnSalvar.UniqueID;

            bool controlarVinculoApenasIntegracao = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_COLABORADOR_APENAS_INTEGRACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            bool vinculoIntegradoVirtual = !controlarVinculoApenasIntegracao && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_COLABORADOR_VINCULO_INTEGRADO_VIRTUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            bool podeEditarVinculos = ((__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && _VS_col_id > 0) ||
                                          (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && _VS_col_id <= 0)) &&
                                         vinculoIntegradoVirtual;

            _btnNovaFuncao.Visible = _btnNovoCargo.Visible = podeEditarVinculos;
            _grvCargosFuncoes.Columns[indiceGridCargoFuncaoExcluir].Visible = podeEditarVinculos;

            VerificaObrigatoriedadeEmail(_ckbUserLive);
        }

        bool podeEditarColaborador = ((__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && _VS_col_id > 0) ||
                                      (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && _VS_col_id <= 0)) &&
                                     !_VS_col_controladoIntegracao;

        bool controlarVinculoApenasIntegracaoColaborador = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_COLABORADOR_APENAS_INTEGRACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        bool vinculoIntegradoVirtualColaborador = !controlarVinculoApenasIntegracaoColaborador && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_COLABORADOR_VINCULO_INTEGRADO_VIRTUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

        bool podeEditarVinculosColaborador = ((__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && _VS_col_id > 0) ||
                                      (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && _VS_col_id <= 0)) &&
                                     vinculoIntegradoVirtualColaborador;

        if (!podeEditarColaborador)
        {
            HabilitaControles(fdsDadosPessoais.Controls, false);
            UCEnderecos1.DesabilitarCamposEnderecos();
            HabilitaControles(fdsContato.Controls, false);
            HabilitaControles(fdsDocumento.Controls, false);
            HabilitaControles(fdsCertidoes.Controls, false);
            HabilitaControles(fdsCriarUsuario.Controls, false);
            HabilitaControles(fdsUsuario.Controls, false);
        }

        if (!podeEditarColaborador && !podeEditarVinculosColaborador)
        {
            _btnCancelar.Text = "Voltar";
            _btnCancelar.Visible = true;
            _btnSalvar.Visible = false;
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

    private void VerificaObrigatoriedadeEmail(CheckBox ckbUserLive)
    {
        if (!SYS_ParametroBO.Parametro_ValidarObrigatoriedadeEmailUsuario() || ckbUserLive.Checked)
        {
            _rfvEmail.Enabled = false;
            RemoveAsteriscoObrigatorio(_lblEmail);
        }
        else
        {
            _rfvEmail.Enabled = true;
            AdicionaAsteriscoObrigatorio(_lblEmail);
        }
    }

    /// <summary>
    /// Carrega os dados do colaborador nos controles caso seja alteração.
    /// </summary>
    private void _LoadFromEntity()
    {
        try
        {
            // Carrega entidade do colaborador
            RHU_Colaborador col = new RHU_Colaborador
            {
                col_id = _VS_col_id
            };
            RHU_ColaboradorBO.GetEntity(col);

            RHU_ColaboradorCargo coc = new RHU_ColaboradorCargo
            {
                coc_id = _VS_coc_id
            };
            RHU_ColaboradorCargoBO.GetEntity(coc);

            if (col.ent_id != __SessionWEB.__UsuarioWEB.Usuario.ent_id)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("O colaborador não pertence à entidade na qual você está logado.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Academico/RecursosHumanos/Colaborador/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            _VS_col_controladoIntegracao = col.col_controladoIntegracao;

            // Exibe dados do colaborador
            _txtDataAdmissao.Text = col.col_dataAdmissao.ToString("dd/MM/yyyy");
            _txtDataDemissao.Text = (col.col_dataDemissao != new DateTime()) ? col.col_dataDemissao.ToString("dd/MM/yyyy") : string.Empty;
            UCComboColaboradorSituacao1.Valor = col.col_situacao > 0 ? col.col_situacao : -1;

            // Carrega entidade de pessoa
            PES_Pessoa pes = new PES_Pessoa
            {
                pes_id = col.pes_id
            };
            PES_PessoaBO.GetEntity(pes);

            UCCadastroPessoa1._VS_pes_id = col.pes_id;
            UCCadastroPessoa1._txtNome.Text = pes.pes_nome;
            UCCadastroPessoa1._txtNomeAbreviado.Text = !string.IsNullOrEmpty(pes.pes_nome_abreviado) ? pes.pes_nome_abreviado : string.Empty;

            long arq_id;
            UCCadastroPessoa1.ConfiguraDadosFoto(PaginaGestao.Colaboradores, out arq_id);
            _VS_arq_idAntigo = arq_id;

            // Exibe cidade naturalidade da pessoa
            if (pes.cid_idNaturalidade != Guid.Empty)
            {
                END_Cidade cid = new END_Cidade
                {
                    cid_id = pes.cid_idNaturalidade
                };
                END_CidadeBO.GetEntity(cid);

                UCCadastroPessoa1._VS_cid_id = pes.cid_idNaturalidade;
                UCCadastroPessoa1._txtNaturalidade.Text = cid.cid_nome;
            }

            // Exibe dados gerais da pessoa
            UCCadastroPessoa1._txtDataNasc.Text = (pes.pes_dataNascimento != new DateTime()) ? pes.pes_dataNascimento.ToString("dd/MM/yyyy") : string.Empty;
            UCCadastroPessoa1._ComboEstadoCivil.SelectedValue = pes.pes_estadoCivil > 0 ? pes.pes_estadoCivil.ToString() : "-1";
            UCCadastroPessoa1._ComboSexo.SelectedValue = pes.pes_sexo.ToString();

            UCCadastroPessoa1.ComboNacionalidade1_Valor = pes.pai_idNacionalidade;
            UCCadastroPessoa1._chkNaturalizado.Checked = pes.pes_naturalizado;
            UCCadastroPessoa1._ComboRacaCor.SelectedValue = pes.pes_racaCor > 0 ? pes.pes_racaCor.ToString() : "-1";
            UCCadastroPessoa1._VS_pes_idFiliacaoPai = pes.pes_idFiliacaoPai;
            UCCadastroPessoa1._VS_pes_idFiliacaoMae = pes.pes_idFiliacaoMae;
            UCCadastroPessoa1._ComboEscolaridade.SelectedValue = pes.tes_id != Guid.Empty ? pes.tes_id.ToString() : Guid.Empty.ToString();

            // Carregar tipo de deficiência cadastrada para a pessoa
            DataTable dtPessoaDeficiencia = PES_PessoaDeficienciaBO.GetSelect(pes.pes_id, false, 1, 1);
            if (dtPessoaDeficiencia.Rows.Count > 0)
            {
                UCCadastroPessoa1.ComboTipoDeficiencia1_Valor = new Guid(dtPessoaDeficiencia.Rows[0]["tde_id"].ToString());
            }

            // Armazena os id's antigos em ViewState
            _VS_pai_idAntigo = pes.pai_idNacionalidade;
            _VS_cid_idAntigo = pes.cid_idNaturalidade;
            _VS_tes_idAntigo = pes.tes_id;
            _VS_tde_idAntigo = dtPessoaDeficiencia.Rows.Count > 0 ? new Guid(dtPessoaDeficiencia.Rows[0]["tde_id"].ToString()) : Guid.Empty;

            // Exibe dados do pai da pessoa
            if (pes.pes_idFiliacaoPai != Guid.Empty)
            {
                PES_Pessoa pesFiliacaoPai = new PES_Pessoa { pes_id = pes.pes_idFiliacaoPai };
                PES_PessoaBO.GetEntity(pesFiliacaoPai);
                UCCadastroPessoa1._txtPai.Text = pesFiliacaoPai.pes_nome;
                UCCadastroPessoa1.visibleLimparPai = true;
                PES_PessoaDocumento cpf = new PES_PessoaDocumento
                {
                    pes_id = new Guid(pesFiliacaoPai.pes_id.ToString()),
                    tdo_id = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF))
                };
                PES_PessoaDocumentoBO.GetEntity(cpf);
                PES_PessoaDocumento rg = new PES_PessoaDocumento
                {
                    pes_id = new Guid(pesFiliacaoPai.pes_id.ToString()),
                    tdo_id = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_RG))
                };
                PES_PessoaDocumentoBO.GetEntity(rg);
                if (cpf.psd_situacao == 1)
                {
                    UCCadastroPessoa1.txtCPFPaiValor = cpf.psd_numero;
                }

                if (rg.psd_situacao == 1)
                {
                    UCCadastroPessoa1.txtRGPaiValor = rg.psd_numero;
                }
            }

            // Exibe dados da mae da pessoa
            if (pes.pes_idFiliacaoMae != Guid.Empty)
            {
                PES_Pessoa pesFiliacaoMae = new PES_Pessoa { pes_id = pes.pes_idFiliacaoMae };
                PES_PessoaBO.GetEntity(pesFiliacaoMae);
                UCCadastroPessoa1._txtMae.Text = pesFiliacaoMae.pes_nome;
                UCCadastroPessoa1.visibleLimparMae = true;
                PES_PessoaDocumento cpf = new PES_PessoaDocumento
                {
                    pes_id = new Guid(pesFiliacaoMae.pes_id.ToString()),
                    tdo_id = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF))
                };
                PES_PessoaDocumentoBO.GetEntity(cpf);
                PES_PessoaDocumento rg = new PES_PessoaDocumento
                {
                    pes_id = new Guid(pesFiliacaoMae.pes_id.ToString()),
                    tdo_id = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_RG))
                };

                PES_PessoaDocumentoBO.GetEntity(rg);
                if (cpf.psd_situacao == 1)
                {
                    UCCadastroPessoa1.txtCPFMaeValor = cpf.psd_numero;
                }

                if (rg.psd_situacao == 1)
                {
                    UCCadastroPessoa1.txtRGMaeValor = rg.psd_numero;
                }
            }

            if (pes.pes_id != Guid.Empty)
            {
                // Carrega dados dos endereços da pessoa
                DataTable dtEndereco = PES_PessoaEnderecoBO.GetSelect(pes.pes_id, false, 1, 1);
                UCEnderecos1.CarregarEnderecosBanco(dtEndereco);
            }

            // Carrega dados dos documentos da pessoa
            UCGridDocumento1._CarregarDocumento(pes.pes_id);

            // Carrega dados da certidões da pessoa.
            UCCertidaoCivil1.Inicializa("Pessoa");
            UCCertidaoCivil1.CarregaCertidaoCivil(pes.pes_id);

            // Carrega dados dos cargos / funções do colaborador
            DataTable dtCargoFuncao = RHU_ColaboradorCargoBO.GetSelect(col.col_id, false, 1, 1, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            if (dtCargoFuncao.Rows.Count == 0)
            {
                dtCargoFuncao = null;
            }

            UCCadastroCargo1._VS_dataAdmissao = col.col_dataAdmissao;
            UCCadastroFuncao1._VS_dataAdmissao = col.col_dataAdmissao;

            UCCadastroCargo1.CarregarCargosDisciplinas(col.col_id);

            UCCadastroCargo1._VS_Coc_ID = RHU_ColaboradorCargoBO.VerificaUltimoCargoCadastrado(col.col_id, 0) - 1;
            UCCadastroFuncao1._VS_seq = RHU_ColaboradorFuncaoBO.VerificaUltimaFuncaoCadastrada(col.col_id, 0) - 1;

            UCCadastroCargo1.VS_col_id = UCCadastroFuncao1.VS_col_id = _VS_col_id;

            UCCadastroCargo1._VS_cargos = dtCargoFuncao;
            UCCadastroFuncao1._VS_funcoes = dtCargoFuncao;
            _CarregarCargoFuncao(UCCadastroCargo1._VS_cargos);

            _VS_usu_id = SYS_UsuarioBO.GetSelectBy_pes_id(UCCadastroPessoa1._VS_pes_id);

            // Verifica se existe integraçao externa
            _ckbUserLive.Visible = VerificaIntegracaoExterna();

            // Carrega dados do usuário da pessoa (se existir)
            if (_VS_usu_id != Guid.Empty)
            {
                _chbCriarUsuario.Checked = true;
                divUsuarios.Visible = true;

                SYS_Usuario usuario = new SYS_Usuario { usu_id = _VS_usu_id };
                SYS_UsuarioBO.GetEntity(usuario);

                _txtLogin.Text = usuario.usu_login;
                _txtEmail.Text = usuario.usu_email;

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

                    // Caso não encontre o domínio na lista de disponíveis...
                    if (!encontrou)
                    {
                        // Seta a opção outros.
                        _ddlDominios.SelectedIndex = _VS_OutrosDominios;
                        _TrataOutrosDominios();
                        _txtDominio.Text = usuario.usu_dominio;
                    }
                }

                _TrataUsuarioAD();

                if (usuario.usu_situacao == 5)
                {
                    _chkExpiraSenha.Checked = true;
                }
                else if (usuario.usu_situacao == 2)
                {
                    _chkBloqueado.Checked = true;
                }

                _chkSenhaAutomatica.Visible = true;
                _chkSenhaAutomatica.Checked = false;
                _rfvSenha.Visible = false;
                _rfvConfirmarSenha.Visible = false;

                ManageUserLive live = new ManageUserLive();
                if (live.ExistsIntegracaoExterna())
                {
                    if (live.IsContaEmail(_txtEmail.Text))
                    {
                        _chkSenhaAutomatica.Visible = false;
                        _chbCriarUsuario.Checked = true;
                        _ckbUserLive.Checked = true;
                        _ckbUserLive_CheckedChanged(_ckbUserLive, new EventArgs());
                    }
                    else
                    {
                        _chbCriarUsuario.Checked = true;
                        _ckbUserLive.Checked = false;
                        _ckbUserLive_CheckedChanged(_ckbUserLive, new EventArgs());
                    }
                }
                else
                {
                    _ckbUserLive.Visible = false;
                }
            }
            else
            {
                _chkSenhaAutomatica.Visible = true;
                _chkSenhaAutomatica.Checked = false;
                _chbCriarUsuario.Checked = false;
                divUsuarios.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o colaborador.", UtilBO.TipoMensagem.Erro);
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
    /// Insere e altera um colaborador
    /// </summary>
    private void _Salvar()
    {
        try
        {
            string msgErro;

            if (!UCContato1.SalvaConteudo(out msgErro))
            {
                UCContato1._MensagemErro_Mostrar = false;
                _lblMessage.Text = UtilBO.GetErroMessage(msgErro, UtilBO.TipoMensagem.Alerta);
                txtSelectedTab.Value = "1";
                return;
            }

            if (!UCGridDocumento1.ValidaConteudoGrid(out msgErro))
            {
                UCGridDocumento1._MensagemErro.Visible = false;
                _lblMessage.Text = UtilBO.GetErroMessage(msgErro, UtilBO.TipoMensagem.Alerta);
                txtSelectedTab.Value = "2";
                return;
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

            bool permitido = false;

            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao)
            {
                permitido = true;
            }
            else if ((__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao) || (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa))
            {
                for (int i = 0; i < UCCadastroCargo1._VS_cargos.Rows.Count; i++)
                {
                    if (UCCadastroCargo1._VS_cargos.Rows[i].RowState != DataRowState.Deleted)
                    {
                        permitido = true;
                        break;
                    }
                }
            }

            if (!permitido)
            {
                throw new ArgumentException("Não é permitido cadastrar colaborador sem vínculo de trabalho.");
            }

            if (_ckbUsuarioAD.Checked && _chbCriarUsuario.Checked)
            {
                if (String.IsNullOrEmpty(_ddlDominios.SelectedValue))
                {
                    throw new ArgumentException("Domínio é obrigatório.");
                }

                if (_ddlDominios.SelectedValue == "Outros domínios..." && string.IsNullOrEmpty(_txtDominio.Text))
                {
                    throw new ArgumentException("Outro domínio é obrigatório.");
                }
            }

            PES_Pessoa entityPessoa = new PES_Pessoa
            {
                pes_id = UCCadastroPessoa1._VS_pes_id,
                pes_nome = UCCadastroPessoa1._txtNome.Text,
                pes_nome_abreviado = UCCadastroPessoa1._txtNomeAbreviado.Text,
                pai_idNacionalidade = UCCadastroPessoa1.ComboNacionalidade1_Valor,
                pes_naturalizado = UCCadastroPessoa1._chkNaturalizado.Checked,
                cid_idNaturalidade = UCCadastroPessoa1._VS_cid_id,
                pes_dataNascimento = string.IsNullOrEmpty(UCCadastroPessoa1._txtDataNasc.Text) ? new DateTime() : Convert.ToDateTime(UCCadastroPessoa1._txtDataNasc.Text),
                pes_racaCor = Convert.ToByte(UCCadastroPessoa1._ComboRacaCor.SelectedValue == "-1" ? "0" : UCCadastroPessoa1._ComboRacaCor.SelectedValue),
                pes_sexo = Convert.ToByte(UCCadastroPessoa1._ComboSexo.SelectedValue == "-1" ? "0" : UCCadastroPessoa1._ComboSexo.SelectedValue),
                pes_idFiliacaoPai = UCCadastroPessoa1._VS_pes_idFiliacaoPai,
                pes_idFiliacaoMae = UCCadastroPessoa1._VS_pes_idFiliacaoMae,
                tes_id = new Guid(UCCadastroPessoa1._ComboEscolaridade.SelectedValue),
                pes_estadoCivil = Convert.ToByte(UCCadastroPessoa1._ComboEstadoCivil.SelectedValue == "-1" ? "0" : UCCadastroPessoa1._ComboEstadoCivil.SelectedValue),
                pes_situacao = 1,
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
                        throw new ArgumentException("Foto é maior que o tamanho máximo permitido.");
                    }

                    if (UCCadastroPessoa1._iptFoto.PostedFile.FileName.Substring(UCCadastroPessoa1._iptFoto.PostedFile.FileName.Length - 3, 3).ToUpper() != "JPG")
                    {
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
                pes_id = UCCadastroPessoa1._VS_pes_id,
                tde_id = UCCadastroPessoa1.ComboTipoDeficiencia1_Valor,
                IsNew = true
            };

            RHU_Colaborador entityColaborador = new RHU_Colaborador
            {
                col_id = _VS_col_id,
                ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                col_dataAdmissao = string.IsNullOrEmpty(_txtDataAdmissao.Text) ? new DateTime() : Convert.ToDateTime(_txtDataAdmissao.Text),
                col_dataDemissao = string.IsNullOrEmpty(_txtDataDemissao.Text) ? new DateTime() : Convert.ToDateTime(_txtDataDemissao.Text),
                col_situacao = Convert.ToByte(UCComboColaboradorSituacao1.Valor > 0 ? UCComboColaboradorSituacao1.Valor : 0),
                col_controladoIntegracao = _VS_col_controladoIntegracao,
                IsNew = (_VS_col_id > 0) ? false : true
            };

            SYS_Usuario entityUsuario = new SYS_Usuario { usu_id = _VS_usu_id };
            SYS_UsuarioBO.GetEntity(entityUsuario);
            if (_chbCriarUsuario.Checked)
            {
                entityUsuario.ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
                entityUsuario.usu_login = _txtLogin.Text;
                entityUsuario.usu_email = _txtEmail.Text;
                entityUsuario.usu_senha = string.IsNullOrEmpty(_txtSenha.Text) ? _txtSenha.Attributes["value"] : _txtSenha.Text;
                entityUsuario.pes_id = UCCadastroPessoa1._VS_pes_id;
                entityUsuario.usu_situacao = 1;
                entityUsuario.usu_dominio = (_ddlDominios.SelectedIndex == _VS_OutrosDominios) ? _txtDominio.Text : _ddlDominios.SelectedValue;
                entityUsuario.IsNew = (_VS_usu_id != Guid.Empty) ? false : true;

                if (_chkExpiraSenha.Checked)
                {
                    entityUsuario.usu_situacao = 5;
                }

                if (_chkBloqueado.Checked)
                {
                    entityUsuario.usu_situacao = 2;
                }

                ManageUserLive live = new ManageUserLive();
                if (_ckbUserLive.Checked)
                {
                    /****** ATENÇÃO - validação temporária ******/
                    if (!live.IsContaEmail(_txtEmail.Text))
                    {
                        throw new ValidationException("E-mail inválido para integrar usuário live.");
                    }

                    if (!live.VerificarContaEmailExistente(new UserLive { email = _txtEmail.Text }))
                    {
                        throw new ValidationException("E-mail não encontrado no live para realizar integração.");
                    }
                    /*******************************************/
                }
                else
                {
                    if (live.IsContaEmail(_txtEmail.Text))
                    {
                        throw new ValidationException("Integrar usuário live é obrigatório, o email " + _txtEmail.Text + " contém o domínio para integração com live.");
                    }
                }
            }

            string padraoUsuarioColaborador = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.PAR_GRUPO_PERFIL_COLAB, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            // Configura entidade PES_CertidaoCivil
            PES_CertidaoCivil entityCertidaoCivil;
            UCCertidaoCivil1.RetornaCertidaoCivil(out entityCertidaoCivil);

            // Salva os dados do colaborador.
            if (RHU_ColaboradorBO.Save(entityPessoa,
                entityPessoaDeficiencia,
                UCEnderecos1._VS_enderecos,
                UCContato1._carregaDataTableComContatos,
                UCGridDocumento1.RetornaDocumentoSave(),
                entityCertidaoCivil,
                _VS_pai_idAntigo,
                _VS_cid_idAntigo,
                UCCadastroPessoa1.InformacoesPai,
                UCCadastroPessoa1.InformacoesMae,
                _VS_tes_idAntigo,
                _VS_tde_idAntigo,
                entityColaborador,
                UCCadastroCargo1._VS_cargos,
                UCCadastroCargo1._VS_cargosDisciplinas,
                _chbCriarUsuario.Checked,
                _ckbUserLive.Checked,
                entityUsuario,
                null,
                padraoUsuarioColaborador,
                _chkSenhaAutomatica.Checked,
                __SessionWEB.TituloGeral,
                ApplicationWEB._EmailHost,
                ApplicationWEB._EmailSuporte,
                __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                null,
                null,
                ApplicationWEB.TipoImagensPermitidas,
                ApplicationWEB.TamanhoMaximoArquivo,
                entArquivo,
                UCCadastroPessoa1._chbExcluirImagem.Checked))
            {
                if (_VS_col_id <= 0)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "col_id: " + entityColaborador.col_id + (entityUsuario.usu_id != Guid.Empty ? "; usu_id: " + entityUsuario.usu_id : string.Empty));
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Colaborador incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "col_id: " + entityColaborador.col_id + (entityUsuario.usu_id != Guid.Empty ? "; usu_id: " + entityUsuario.usu_id : string.Empty));
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Colaborador alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }

                // Grava log de inclusão/alteração/exclusão de usuário
                if (_VS_usu_id != Guid.Empty)
                {
                    if (!_chbCriarUsuario.Checked)
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "usu_id: " + _VS_usu_id + "; col_id: " + entityColaborador.col_id);
                    }
                    else
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "usu_id: " + entityUsuario.usu_id + "; col_id: " + entityColaborador.col_id);
                    }
                }
                else if (entityUsuario.usu_id != Guid.Empty)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "usu_id: " + entityUsuario.usu_id + "; col_id: " + entityColaborador.col_id);
                }

                if (Session["pes_id"] != null)
                {
                    Session.Remove("pes_id");
                    RedirecionarPagina(__SessionWEB._AreaAtual._Diretorio + "/Academico/RecursosHumanos/Pessoas/Busca.aspx");
                }
                else
                {
                    Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/RecursosHumanos/Colaborador/Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            else
            {
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o colaborador.", UtilBO.TipoMensagem.Erro);
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
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o colaborador.", UtilBO.TipoMensagem.Erro);
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

    #region Cargos / Funções

    public void _CarregarCargoFuncao(DataTable _CargoFuncao)
    {
        _grvCargosFuncoes.DataSource = _CargoFuncao;
        _grvCargosFuncoes.DataBind();
        _updGridCargosFuncoes.Update();
    }

    private void _AlterarCargo(int crg_id, int seqcrg_id)
    {
        UCCadastroCargo1._VS_IsNew = false;
        UCCadastroCargo1._VS_id = crg_id;
        UCCadastroCargo1._VS_Coc_ID = seqcrg_id;
        UCCadastroCargo1.ComboCargoPermiteEditar = false;
        UCCadastroCargo1._btnPesquisarUA.Enabled = false;

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
                    UCCadastroCargo1.ValorCargaHoraria = string.IsNullOrEmpty(UCCadastroCargo1._VS_cargos.Rows[i]["chr_id"].ToString()) ? -1 : Convert.ToInt32(UCCadastroCargo1._VS_cargos.Rows[i]["chr_id"].ToString());

                    break;
                }
            }
        }

        bool controlarVinculoApenasIntegracao = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_COLABORADOR_APENAS_INTEGRACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        bool vinculoIntegradoVirtual = !controlarVinculoApenasIntegracao && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_COLABORADOR_VINCULO_INTEGRADO_VIRTUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

        bool podeEditarVinculo = vinculoIntegradoVirtual && !UCCadastroCargo1.VS_coc_controladoIntegracao;

        bool podeEditar = ((__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && _VS_col_id > 0) ||
                           (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && _VS_col_id <= 0)) &&
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
        UCCadastroFuncao1._btnPesquisarUA.Enabled = false;

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

        bool podeEditar = ((__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && _VS_col_id > 0) ||
                           (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && _VS_col_id <= 0)) &&
                           podeEditarVinculo;

        if (!podeEditar)
        {
            HabilitaControles(UCCadastroFuncao1.Controls, false);
            UCCadastroFuncao1._BotaoCancelar.Enabled = true;
            UCCadastroCargo1._BotaoCancelar.Enabled = true;

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

    private static string _TrataStrDominio(string dominio)
    {
        return dominio.Split('.')[0];
    }

    private void _CarregarComboDominios()
    {
        // Carregar domínios.
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
        int cont = 0; // Índice do selecione.
        foreach (string dominio in lsDominios)
        {
            string strDominio = _TrataStrDominio(dominio);
            _ddlDominios_AddItems(strDominio, strDominio);
            cont++;
        }

        _VS_OutrosDominios = ++cont;
        _ddlDominios_AddItems("Outros domínios...", "Outros domínios...");
    }

    private void _ddlDominios_AddItems(string text, string value)
    {
        ListItem item = new ListItem { Text = text, Value = value };
        _ddlDominios.Items.Add(item);
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
            _chkSenhaAutomatica.Visible = false;
            _chkSenhaAutomatica.Checked = false;
            _chkSenhaAutomatica_CheckedChanged(null, null);
            _ckbUserLive.Checked = false;
            VerificaObrigatoriedadeEmail(_ckbUserLive);
            _ckbUserLive.Visible = false;
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
            _chkSenhaAutomatica.Visible = true;
            _chkSenhaAutomatica.Checked = false;
            _chkSenhaAutomatica_CheckedChanged(null, null);
            _ckbUserLive.Visible = VerificaIntegracaoExterna();
        }
    }

    private bool VerificaIntegracaoExterna()
    {
        ManageUserLive live = new ManageUserLive();
        return live.ExistsIntegracaoExterna();
    }

    private void _TrataOutrosDominios()
    {
        divOutrosDominios.Visible = _ddlDominios.SelectedIndex == _VS_OutrosDominios;

        _ddlDominios.Focus();
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
                pes_id = new Guid(parameters["pes_id"].ToString()),
                tdo_id = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF))
            };
            PES_PessoaDocumentoBO.GetEntity(cpf);
            PES_PessoaDocumento rg = new PES_PessoaDocumento
            {
                pes_id = new Guid(parameters["pes_id"].ToString()),
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
                pes_id = new Guid(parameters["pes_id"].ToString()),
                tdo_id = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF))
            };
            PES_PessoaDocumentoBO.GetEntity(cpf);
            PES_PessoaDocumento rg = new PES_PessoaDocumento
            {
                pes_id = new Guid(parameters["pes_id"].ToString()),
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
        {
            UCCadastroCidade1.CarregarCombos();
        }

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

    private void UCCadastroCargo1__Incluir(DataTable dt)
    {
        UCCadastroFuncao1._VS_funcoes = dt;

        _grvCargosFuncoes.DataSource = dt;
        _grvCargosFuncoes.DataBind();
        _updGridCargosFuncoes.Update();

        UCCadastroCargo1._Botao.Text = "Incluir";

        ScriptManager.RegisterClientScriptBlock(this, GetType(), "CadastroCargo", "$('#divCargos').dialog('close');", true);
    }

    private void UCCadastroFuncao1__Selecionar()
    {
        _VS_tipoBuscaUA = 2;
        UCUA1._Limpar();
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "BuscaUAFuncao", "$('#divBuscaUA').dialog('open');", true);
        _updBuscaUA.Update();
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
        UCCadastroCargo1.ComboCargoCarregarVerificandoControleIntegracao(false);

        ScriptManager.RegisterClientScriptBlock(this, GetType(), "CadastroCargoNovo", "$('#divCargos').dialog('open');", true);
    }

    protected void _btnNovaFuncao_Click(object sender, EventArgs e)
    {
        UCCadastroFuncao1._LimparCampos();
        UCCadastroFuncao1.ConfiguraCadastro();
        UCCadastroFuncao1._updCadastroFuncao.Update();
        UCCadastroFuncao1.ComboFuncaoSetarFoco();

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

            // Carrega as UA que o usuário tem permissão se for visão ua ou gestão
            List<Guid> lista = new List<Guid>();
            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa ||
                __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao)
            {
                lista = ESC_EscolaBO.GetSelect_Uad_Ids_By_PermissaoUsuario(__SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id);
            }

            ImageButton btnDelete = (ImageButton)e.Row.FindControl("_btnExcluir");
            if (btnDelete != null)
            {
                btnDelete.CommandArgument = e.Row.RowIndex.ToString();
                btnDelete.Visible = __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao || lista.Exists(p => p.Equals(uad_id)) && podeEditarVinculo;

                bool podeEditar = ((__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && _VS_col_id > 0) ||
                                   (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && _VS_col_id <= 0)) &&
                                  podeEditarVinculo;

                if (!podeEditar)
                {
                    btnDelete.Visible = false;
                }
            }

            LinkButton btnAlterar = (LinkButton)e.Row.FindControl("_btnSelecionar");
            if (btnAlterar != null)
            {
                btnAlterar.CommandArgument = e.Row.RowIndex.ToString();
                btnAlterar.Visible = __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao || lista.Exists(p => p.Equals(uad_id));
            }

            Label lblAlterar = (Label)e.Row.FindControl("_lblSelecionar");
            if (lblAlterar != null)
            {
                lblAlterar.Visible = !(__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao || lista.Exists(p => p.Equals(uad_id)));
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

            if (!string.IsNullOrEmpty(_grvCargosFuncoes.DataKeys[index].Values[0].ToString().Trim()))
            {
                int crg_id = Convert.ToInt32(_grvCargosFuncoes.DataKeys[index].Values[0]);
                int seqcrg_id = Convert.ToInt32(_grvCargosFuncoes.DataKeys[index].Values[1]);
                _ExcluirCargo(crg_id, seqcrg_id);
            }
            else
            {
                int fun_id = Convert.ToInt32(_grvCargosFuncoes.DataKeys[index].Values[2]);
                int seqfun_id = Convert.ToInt32(_grvCargosFuncoes.DataKeys[index].Values[3]);
                _ExcluirFuncao(fun_id, seqfun_id);
            }
        }

        if (e.CommandName == "Alterar")
        {
            int index = int.Parse(e.CommandArgument.ToString());

            if (!string.IsNullOrEmpty(_grvCargosFuncoes.DataKeys[index].Values[0].ToString().Trim()))
            {
                UCCadastroCargo1._LimparCampos();
                int crg_id = Convert.ToInt32(_grvCargosFuncoes.DataKeys[index].Values[0]);
                int seqcrg_id = Convert.ToInt32(_grvCargosFuncoes.DataKeys[index].Values[1]);
                _AlterarCargo(crg_id, seqcrg_id);
            }
            else
            {
                UCCadastroFuncao1._LimparCampos();
                int fun_id = Convert.ToInt32(_grvCargosFuncoes.DataKeys[index].Values[2]);
                int seqfun_id = Convert.ToInt32(_grvCargosFuncoes.DataKeys[index].Values[3]);
                _AlterarFuncao(fun_id, seqfun_id);
            }
        }
    }

    #endregion

    #region Usuário

    protected void _ckbUserLive_CheckedChanged(object sender, EventArgs e)
    {
        if (((CheckBox)sender).Checked)
        {
            Label1.Text = "Login";
            /****** ATENÇÃO - comentário temporário ******/
            //Label2.Text = "E-mail (seuEmail@rioeduca.net - caso não preenchido ou não existir, será criado uma nova conta live)";
            /*********************************************/
            _lblSenha.Text = "Senha";
            _lblConfirmacao.Text = "Confirmar senha";
            _rfvLogin.Enabled = false;
            Label1.Visible = false;
            _txtLogin.Visible = false;
            _rfvSenha.Enabled = false;
            _lblSenha.Visible = _VS_usu_id != Guid.Empty;
            _txtSenha.Visible = _VS_usu_id != Guid.Empty;
            _rfvConfirmarSenha.Enabled = false;
            _lblConfirmacao.Visible = _VS_usu_id != Guid.Empty;
            _txtConfirmacao.Visible = _VS_usu_id != Guid.Empty;
            _ckbUsuarioAD.Visible = false;
            _chkSenhaAutomatica.Visible = false;
            _chkExpiraSenha.Visible = _VS_usu_id != Guid.Empty;
            _chkBloqueado.Visible = _VS_usu_id != Guid.Empty;
            revSenha.Enabled = true;
            revSenhaTamanho.Enabled = true;
        }
        else
        {
            Label1.Text = "Login *";
            _lblSenha.Text = "Senha *";
            _lblConfirmacao.Text = "Confirmar senha *";
            _rfvLogin.Enabled = true;
            Label1.Visible = true;
            _txtLogin.Visible = true;
            _rfvSenha.Enabled = true;
            _lblSenha.Visible = true;
            _txtSenha.Visible = true;
            _rfvConfirmarSenha.Enabled = true;
            _lblConfirmacao.Visible = true;
            _txtConfirmacao.Visible = true;
            _ckbUsuarioAD.Visible = true;
            _chkSenhaAutomatica.Visible = true;
            _chkExpiraSenha.Visible = true;
            _chkBloqueado.Visible = true;
            revSenha.Enabled = false;
            revSenhaTamanho.Enabled = false;
        }
        VerificaObrigatoriedadeEmail(((CheckBox)sender));
    }

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

            _chkExpiraSenha.Checked = true;
            _chkExpiraSenha.Visible = false;
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

            _chkExpiraSenha.Checked = false;
            _chkExpiraSenha.Visible = true;
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
        if (Session["pes_id"] != null)
        {
            Session.Remove("pes_id");
            RedirecionarPagina(__SessionWEB._AreaAtual._Diretorio + "Academico/RecursosHumanos/Pessoas/Busca.aspx");
        }
        else
        {
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/RecursosHumanos/Colaborador/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }

    protected void _btnSalvar_Click(object sender, EventArgs e)
    {
        ValidarCampos();

        if (Page.IsValid)
        {
            _Salvar();
        }
    }

    #endregion
}

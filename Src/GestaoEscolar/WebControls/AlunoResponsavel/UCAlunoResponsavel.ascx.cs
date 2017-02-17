using System;
using System.Collections.Generic;
using System.Web.UI;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

public partial class WebControls_AlunoResponsavel_UCAlunoResponsavel : MotherUserControl
{
    #region Enumerador

    /// <summary>
    /// Enum do tipo de responsável padrão da tela.
    /// </summary>
    public enum TipoResponsavel : byte
    {
        Mae = 1
        ,
        Pai = 2
        ,
        Outro = 3
    }

    #endregion

    #region Delegates

    /// <summary>
    /// Seta na página que
    /// utiliza o userControl que foi acionado o botão de 
    /// pesquisar pessoa.
    /// </summary>
    /// <param name="sender"></param>
    public delegate void SetaBuscaPessoa(WebControls_AlunoResponsavel_UCAlunoResponsavel sender);

    public SetaBuscaPessoa _SetaBuscaPessoa;

    #endregion

    #region Propriedades

    /// <summary>
    /// Guarda em viewstate o tipo de responsável do UserControl.
    /// </summary>
    public TipoResponsavel VS_TipoResponsavel
    {
        get
        {
            if (ViewState["VS_TipoResponsavel"] == null)
                return 0;

            return (TipoResponsavel)ViewState["VS_TipoResponsavel"];
        }
        set
        {
            ViewState["VS_TipoResponsavel"] = value;
        }
    }

    /// <summary>
    /// Retorna o texto do label de nome, dependendo do tipo de responsável.
    /// </summary>
    private string NomeLabel
    {
        get
        {
            switch (VS_TipoResponsavel)
            {
                case TipoResponsavel.Mae:
                    {
                        rfvNome.ErrorMessage = "Nome da mãe é obrigatório.";
                        return "Nome da mãe";
                    }
                case TipoResponsavel.Pai:
                    {
                        rfvNome.ErrorMessage = "Nome do pai é obrigatório.";
                        return "Nome do pai";
                    }
                default:
                    {
                        rfvNome.ErrorMessage = "Nome do responsável é obrigatório.";
                        return "Nome do responsável";
                    }
            }
        }
    }

    /// <summary>
    /// Seta se os campos referentes à pessoa do responsável são obrigatórios.
    /// </summary>
    public bool Obrigatorio
    {
        set
        {
            rfvNome.Visible = value;
            lblNome.Text = NomeLabel;
            if (value)
            {
                AdicionaAsteriscoObrigatorio(lblNome);
            }
            else
            {
                RemoveAsteriscoObrigatorio(lblNome);
            }

        }

        get
        {
            return rfvNome.Visible;
        }
    }

    /// <summary>
    /// Seta o validationGroup dos validators do usercontrol.
    /// </summary>
    public string ValidationGroup
    {
        set
        {
            rfvNome.ValidationGroup = value;
            cvDataNascimento.ValidationGroup = value;
        }
    }

    public WebControls_Busca_UCPessoasAluno UCBuscaPessoasAluno { get; set; }

    /// <summary>
    /// Guarda em ViewState o id do AlunoResponsavel.
    /// </summary>
    public Int32 VS_Alr_ID
    {
        private get
        {
            if (ViewState["VS_Alr_ID"] == null)
                return -1;

            return Convert.ToInt32(ViewState["VS_Alr_ID"]);
        }
        set
        {
            ViewState["VS_Alr_ID"] = value;
        }
    }

    /// <summary>
    /// Guarda em ViewState o id da pessoa selecionada na busca.
    /// </summary>
    public Guid VS_Pes_ID
    {
        private get
        {
            if (ViewState["VS_Pes_ID"] == null)
                return Guid.Empty;

            return new Guid(ViewState["VS_Pes_ID"].ToString());
        }
        set
        {
            ViewState["VS_Pes_ID"] = value;
        }
    }

    /// <summary>
    /// Retorna o texto do campo Nome.
    /// </summary>
    public string NomePessoa
    {
        get
        {
            return txtNome.Text;
        }
    }

    public bool Falecido
    {
        get
        {
            return chkSituacaoFalecido.Checked;
        }
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Retorna o documento do responsável
    /// </summary>
    /// <param name="pes_id">id da pessoa</param>
    /// <param name="docPadrao">tipo de documento (RG ou CPF)</param>
    /// <returns>número do documento</returns>
    private string RetornaDocumentoResponsavel(Guid pes_id, string docPadrao)
    {
        PES_PessoaDocumento entityPessoaDocumento = new PES_PessoaDocumento
        {
            pes_id = pes_id
            ,
            tdo_id = new Guid(docPadrao)
        };
        PES_PessoaDocumentoBO.GetEntity(entityPessoaDocumento);

        return entityPessoaDocumento.psd_numero;
    }

    /// <summary>
    /// Método chamado quando o usuário seleciona uma pessoa na tela de busca.
    /// </summary>
    /// <param name="parameters"></param>
    void UCPessoas1BuscaPessoa(IDictionary<string, object> parameters)
    {
        //Faz uma busca do id do tipo de documento (tdo_id).
        string docPadraoCPF = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF);
        string docPadraoRG = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_RG);

        //Variáveis que recebe os valores cadastrados caso já exista.
        byte situacao_responsavel;
        string alr_profissao;

        // ID do tipo de documento NIS.
        Guid tdo_idNis = ACA_ParametroAcademicoBO.ParametroValorGuidPorEntidade(eChaveAcademico.TIPO_DOCUMENTACAO_NIS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

        try
        {
            if (!String.IsNullOrEmpty(parameters["pes_id"].ToString()))
                VS_Pes_ID = new Guid(parameters["pes_id"].ToString());

            PES_Pessoa entity = new PES_Pessoa();
            entity.pes_id = VS_Pes_ID;
            PES_PessoaBO.GetEntity(entity);

            txtNome.Text = entity.pes_nome;
            txtDataNasc.Text = entity.pes_dataNascimento != new DateTime() ?
                entity.pes_dataNascimento.ToString("dd/MM/yyyy") :
                "";
            UCComboEstadoCivil1._Combo.SelectedValue = entity.pes_estadoCivil.ToString();
            UCComboSexo1._Combo.SelectedValue = (entity.pes_sexo == 0) ? "-1" : entity.pes_sexo.ToString();
            if (entity.tes_id != Guid.Empty)
                UCComboTipoEscolaridade1._Combo.SelectedValue = entity.tes_id.ToString();
            else
                UCComboTipoEscolaridade1._Combo.SelectedValue = UCComboTipoEscolaridade1._Combo.Items[0].Value;

            //Faz a verificação se existe as informações cadastradas referente a profissão e situação do responsável.
            ACA_AlunoResponsavelBO.RetornaAlunoResponsavel_Situacao_Profissao(entity.pes_id, out situacao_responsavel, out alr_profissao);
            chkSituacaoFalecido.Checked = situacao_responsavel == Convert.ToByte(ACA_AlunoResponsavelSituacao.Falecido);
            txtProfissao.Text = alr_profissao;

            txtCPFResponsavel.Text = RetornaDocumentoResponsavel(entity.pes_id, docPadraoCPF);
            txtRGResponsavel.Text = RetornaDocumentoResponsavel(entity.pes_id, docPadraoRG);
            txtNis.Text = RetornaDocumentoResponsavel(entity.pes_id, tdo_idNis.ToString());

            if (UCBuscaPessoasAluno != null)
            {
                // Limpar campos da busca.
                UCBuscaPessoasAluno._Limpar();
            }

            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "BuscaPessoa", "$('#divBuscaResponsavel').dialog('close');", true);
        }
        catch (Exception ex)
        {
            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a pessoa.", UtilBO.TipoMensagem.Erro);
            ApplicationWEB._GravaErro(ex);
        }
    }

    /// <summary>
    /// Caso algum dos campos esteja preenchido valida se o nome do 
    /// responsável foi informado ou se os campos não consta na certidão
    /// ou omitido na forma da lei estão checkados.
    /// </summary>
    private bool ValidaCampoDocumento()
    {
        if ((!string.IsNullOrEmpty(txtCPFResponsavel.Text) || !string.IsNullOrEmpty(txtRGResponsavel.Text) ||
             !string.IsNullOrEmpty(txtNis.Text) || !string.IsNullOrEmpty(txtDataNasc.Text) ||
             chkSituacaoFalecido.Checked || chkMoraComAluno.Checked ||
             (UCComboTipoEscolaridade1._Combo.SelectedValue != Guid.Empty.ToString()) ||
              ((UCComboSexo1._Combo.SelectedValue != "-1") && VS_TipoResponsavel != TipoResponsavel.Mae &&
                VS_TipoResponsavel != TipoResponsavel.Pai) || (UCComboEstadoCivil1._Combo.SelectedValue != "-1")) && Obrigatorio)
        {
            if (string.IsNullOrEmpty(txtNome.Text) && (!chkOmitidoFormaLei.Checked) && (!chkNaoConstaCertidaoNasc.Checked))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Retorna a mensagem referente a validação
    /// </summary>    
    /// <returns></returns>
    private string MsgValidacaoDocumento()
    {
        switch (VS_TipoResponsavel)
        {
            case TipoResponsavel.Mae:
                {
                    return "Nome da mãe é obrigatório.";
                }
            case TipoResponsavel.Pai:
                {
                    return "Nome do pai é obrigatório.";
                }
            default:
                {
                    return "Nome do responsável é obrigatório.";
                }
        }
    }

    /// <summary>
    /// Retorna a estrutura utilizada para cadastrar, com os dados do USercontrol.
    /// </summary>
    /// <param name="tra_idPrincipal">id do tipo de respnsável selecionado na tela.</param>
    /// <param name="item">Estrutura de cadastro.</param>
    /// <returns>Se tiver o responsável cadastrado</returns>
    public bool RetornaStructCadastro(Int32 tra_idPrincipal, out ACA_AlunoResponsavelBO.StructCadastro item)
    {
        //Faz a validação dos documentos do responsável.
        if (ValidaCampoDocumento())
            throw new ValidationException(MsgValidacaoDocumento());

        DateTime dataNascimento = new DateTime();
        if (!String.IsNullOrEmpty(txtDataNasc.Text.Trim()))
        {
            dataNascimento = Convert.ToDateTime(txtDataNasc.Text.Trim());
        }

        // Dados da pessoa.
        PES_Pessoa entPessoa = new PES_Pessoa
        {
            pes_id = VS_Pes_ID
        };

        if (VS_Pes_ID != Guid.Empty)
            PES_PessoaBO.GetEntity(entPessoa);

        entPessoa.pes_nome = txtNome.Text;
        entPessoa.pes_dataNascimento = dataNascimento;
        entPessoa.pes_estadoCivil = Convert.ToByte(
            (UCComboEstadoCivil1._Combo.SelectedValue.Equals("-1") ?
            "0" :
            UCComboEstadoCivil1._Combo.SelectedValue));
        if (VS_TipoResponsavel == TipoResponsavel.Pai)
        {
            entPessoa.pes_sexo = 1;
        }
        else
        {
            if (VS_TipoResponsavel == TipoResponsavel.Mae)
            {
                entPessoa.pes_sexo = 2;
            }
            else
            {
                entPessoa.pes_sexo = Convert.ToByte((UCComboSexo1._Combo.SelectedValue.Equals("-1") ? "0" : UCComboSexo1._Combo.SelectedValue));
            }
        }
        entPessoa.tes_id = UCComboTipoEscolaridade1.Valor;
        entPessoa.pes_situacao = 1;

        Int32 tra_id = RetornaTraID(tra_idPrincipal);

        ACA_AlunoResponsavel entRespnsavel = new ACA_AlunoResponsavel
        {
            alr_id = VS_Alr_ID
            ,
            alr_apenasFiliacao = chkApenasFiliacao.Checked
            ,
            alr_moraComAluno = chkMoraComAluno.Checked
            ,
            alr_constaCertidaoNascimento = !chkNaoConstaCertidaoNasc.Checked
            ,
            alr_omitidoFormaLei = chkOmitidoFormaLei.Checked
            ,
            tra_id = tra_id
            ,
            alr_principal = (tra_id == tra_idPrincipal)
            ,
            alr_situacao = (byte)(chkSituacaoFalecido.Checked ?
                                  ACA_AlunoResponsavelSituacao.Falecido :
                                  ACA_AlunoResponsavelSituacao.Ativo)
            ,
            alr_profissao = txtProfissao.Text
            ,
            IsNew = VS_Alr_ID <= 0
        };

        List<PES_PessoaDocumento> listDocResp;
        listDocResp = InserirDocumentoResponsavel(entPessoa.pes_id, txtCPFResponsavel.Text, txtRGResponsavel.Text, txtNis.Text.Trim());

        item = new ACA_AlunoResponsavelBO.StructCadastro();
        item.entPessoa = entPessoa;
        item.entAlunoResp = entRespnsavel;
        item.listPessoaDoc = listDocResp;


        bool naoConstaCertidao = false;
        if (VS_TipoResponsavel == TipoResponsavel.Mae || VS_TipoResponsavel == TipoResponsavel.Pai)
        {
            naoConstaCertidao = chkNaoConstaCertidaoNasc.Checked;
        }

        // Se o nome estiver preenchido, ou pelo menos um dos campos não consta na certidão 
        //ou omitido na forma da lei estiverem preenchidos, ou se for responsável o próprio, retorna true.
        return ((!String.IsNullOrEmpty(entPessoa.pes_nome) || naoConstaCertidao || chkOmitidoFormaLei.Checked)
               || (tra_id == TipoResponsavelAlunoParametro.tra_idProprio(__SessionWEB.__UsuarioWEB.Usuario.ent_id)));
    }

    /// <summary>
    /// Inserindo em uma lista os documentos referente
    /// ao responsável.
    /// </summary>
    /// <param name="pes_id">id da pessoa</param>
    /// <param name="documentoCPF">o número do CPF</param>
    /// <param name="documentoRG">o número do RG</param>
    /// <param name="documentoNIS">o número do NIS</param>
    /// <returns>uma lista dos documentos referente a pessoa</returns>
    private List<PES_PessoaDocumento> InserirDocumentoResponsavel(Guid pes_id, string documentoCPF, string documentoRG, string documentoNIS)
    {
        List<PES_PessoaDocumento> lista = new List<PES_PessoaDocumento>();

        // Caso for declarado o CPF, será inserindo uma lista. 
        if (!string.IsNullOrEmpty(documentoCPF))
        {
            //Faz uma busca do id do tipo de documento (tdo_id).
            string docPadraoCPF = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF);

            PES_PessoaDocumento entityPessoaDocumento = new PES_PessoaDocumento
            {
                pes_id = pes_id
                ,
                tdo_id = new Guid(docPadraoCPF)
                ,
                psd_numero = documentoCPF
            };

            lista.Add(entityPessoaDocumento);
        }

        // Caso for declarado o RG, será inserindo em uma lista.
        if (!string.IsNullOrEmpty(documentoRG))
        {
            //Faz uma busca do id do tipo de documento (tdo_id).
            string docPadraoRG = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_RG);

            PES_PessoaDocumento entityPessoaDocumento = new PES_PessoaDocumento
            {
                pes_id = pes_id
                ,
                tdo_id = new Guid(docPadraoRG)
                ,
                psd_numero = documentoRG
            };

            lista.Add(entityPessoaDocumento);
        }

        // Caso for declarado o NIS, será inserindo em uma lista.
        if (!string.IsNullOrEmpty(documentoNIS))
        {
            // ID do tipo de documento NIS.
            Guid tdo_idNis = ACA_ParametroAcademicoBO.ParametroValorGuidPorEntidade(eChaveAcademico.TIPO_DOCUMENTACAO_NIS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            PES_PessoaDocumento entityPessoaDocumento = new PES_PessoaDocumento
            {
                pes_id = pes_id
                ,
                tdo_id = tdo_idNis
                ,
                psd_numero = documentoNIS
            };

            lista.Add(entityPessoaDocumento);
        }

        return lista;
    }

    /// <summary>
    /// Retorna o campo tra_id de acordo com o tipo de responsável do UserControl. Se 
    /// for pai ou mãe retorna os ids que estiverem no parâmetro, se não, retorna o ID
    /// passado (porque será "outro").
    /// </summary>
    /// <param name="tra_idPrincipal">ID selecionado no combo da tela.</param>
    /// <returns>tra_id</returns>
    private int RetornaTraID(int tra_idPrincipal)
    {
        switch (VS_TipoResponsavel)
        {
            case TipoResponsavel.Mae:
                {
                    return TipoResponsavelAlunoParametro.tra_idMae(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                }
            case TipoResponsavel.Pai:
                {
                    return TipoResponsavelAlunoParametro.tra_idPai(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                }


            default:
                {
                    return tra_idPrincipal;
                }
        }
    }

    /// <summary>
    /// Limpa os campos do UserControl
    /// </summary>
    public void LimparCampos()
    {
        VS_Alr_ID = -1;
        VS_Pes_ID = Guid.Empty;

        txtNome.Text = "";
        txtDataNasc.Text = "";
        UCComboEstadoCivil1._Combo.SelectedValue = "-1";
        UCComboSexo1._Combo.SelectedValue = "-1";
        UCComboTipoEscolaridade1._Combo.SelectedValue = Guid.Empty.ToString();

        txtNis.Text = "";
        txtProfissao.Text = "";
    }

    /// <summary>
    /// Insere as informações do responsável na tela.
    /// </summary>
    /// <param name="entCadastro">Struct de cadastro com as informações</param>
    public void CarregarResponsavel(ACA_AlunoResponsavelBO.StructCadastro entCadastro)
    {
        VS_Alr_ID = entCadastro.entAlunoResp.alr_id;
        VS_Pes_ID = entCadastro.entPessoa.pes_id;

        txtNome.Text = entCadastro.entPessoa.pes_nome;
        if (entCadastro.entPessoa.pes_dataNascimento != new DateTime())
            txtDataNasc.Text = entCadastro.entPessoa.pes_dataNascimento.ToString("dd/MM/yyyy");

        if (entCadastro.entPessoa.pes_estadoCivil > 0)
            UCComboEstadoCivil1._Combo.SelectedValue = entCadastro.entPessoa.pes_estadoCivil.ToString();

        if (entCadastro.entPessoa.pes_sexo > 0)
            UCComboSexo1._Combo.SelectedValue = entCadastro.entPessoa.pes_sexo.ToString();

        if (entCadastro.entPessoa.tes_id != Guid.Empty)
            UCComboTipoEscolaridade1._Combo.SelectedValue = entCadastro.entPessoa.tes_id.ToString();

        txtProfissao.Text = entCadastro.entAlunoResp.alr_profissao;

        //Carrega os documentos do responsável (RG e CPF)
        if (entCadastro.listPessoaDoc.Count > 0)
        {
            //Faz uma busca do id do tipo de documento (tdo_id).
            string docPadraoCPF = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF);
            string docPadraoRG = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_RG);

            // ID do tipo de documento NIS.
            Guid tdo_idNis = ACA_ParametroAcademicoBO.ParametroValorGuidPorEntidade(eChaveAcademico.TIPO_DOCUMENTACAO_NIS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            foreach (PES_PessoaDocumento item in entCadastro.listPessoaDoc)
            {
                if (item.tdo_id == new Guid(docPadraoCPF))
                {
                    txtCPFResponsavel.Text = item.psd_numero;
                }
                else
                    if (item.tdo_id == new Guid(docPadraoRG))
                    {
                        txtRGResponsavel.Text = item.psd_numero;
                    }
                    else if (item.tdo_id == tdo_idNis)
                    {
                        txtNis.Text = item.psd_numero;
                    }
            }
        }

        chkApenasFiliacao.Checked = entCadastro.entAlunoResp.alr_apenasFiliacao;
        chkNaoConstaCertidaoNasc.Checked = !entCadastro.entAlunoResp.alr_constaCertidaoNascimento;
        chkOmitidoFormaLei.Checked = entCadastro.entAlunoResp.alr_omitidoFormaLei;
        chkMoraComAluno.Checked = entCadastro.entAlunoResp.alr_moraComAluno;
        chkSituacaoFalecido.Checked = entCadastro.entAlunoResp.alr_situacao == (byte)ACA_AlunoResponsavelSituacao.Falecido;
    }

    /// <summary>
    /// Carrega os dados iniciais necessários no user control.
    /// </summary>
    public void InicializarUserControl()
    {
        try
        {
            UCComboTipoEscolaridade1._MostrarMessageSelecione = true;
            UCComboTipoEscolaridade1._Load(1);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(Page);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            //sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroAluno.js"));
        }

        if (UCBuscaPessoasAluno != null)
        {
            // Valores padrão da busca de pessoas.
            UCBuscaPessoasAluno.Paginacao = ApplicationWEB._Paginacao;
            UCBuscaPessoasAluno.ContainerName = "divBuscaPessoa";
            UCBuscaPessoasAluno.ReturnValues += UCPessoas1BuscaPessoa;
        }

        tdNis.Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_NIS_FICHA_INSCRICAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

        if (!IsPostBack)
        {
            UCComboSexo1.Visible = !(VS_TipoResponsavel == TipoResponsavel.Pai || VS_TipoResponsavel == TipoResponsavel.Mae);

            chkSituacaoFalecido.Visible = VS_TipoResponsavel != TipoResponsavel.Outro;

            chkApenasFiliacao.Visible = false;
            chkNaoConstaCertidaoNasc.Visible = VS_TipoResponsavel != TipoResponsavel.Outro;
            chkOmitidoFormaLei.Visible = VS_TipoResponsavel != TipoResponsavel.Outro;

            cvDataNascimento.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de nascimento do responsável");

            if (VS_TipoResponsavel != TipoResponsavel.Outro)
            {
                chkSituacaoFalecido.Attributes.Add("onClick", "escondeComponente(" + chkSituacaoFalecido.ClientID + "," + chkMoraComAluno.ClientID + ")");
                chkMoraComAluno.Attributes.Add("onClick", "escondeComponente(" + chkMoraComAluno.ClientID + "," + chkSituacaoFalecido.ClientID + ")");
            }
        }
    }

    protected void _btnPesquisarPessoa_Click(object sender, ImageClickEventArgs e)
    {
        if (_SetaBuscaPessoa != null)
            _SetaBuscaPessoa(this);
    }

    #endregion

}

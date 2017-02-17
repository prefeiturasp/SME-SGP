using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System.Web.UI.HtmlControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.Validation.Exceptions;
using System.Collections.Generic;
using System.Web;
using System.IO;
using MSTech.GestaoEscolar.Entities;

public partial class WebControls_Pessoa_UCCadastroPessoa : MotherUserControl
{
    #region Delegates

    public delegate void AbreJanelaCadastroCidade();
    public event AbreJanelaCadastroCidade _AbreJanelaCadastroCidade;

    public delegate void onSeleciona();
    public event onSeleciona _Selecionar;

    private void UCComboTipoDeficiencia1_IndexChanged()
    {
        if (_VS_alu_id > 0)
        {
            if (ComboTipoDeficiencia1.Combo_SelectedIndex == 0)
            {
                trDeficientes.Visible = false;
                RetiraSelecao();
            }
            else
            {
                trDeficientes.Visible = true;
            }

            _updCadastroPessoa.Update();
        }
    }

    #endregion

    #region Propriedades

    /// <summary>
    /// 0 - Não houve exclusão da foto
    /// 1 - Foto excluida com sucesso
    /// 2 - Erro ao tentar excluir foto
    /// </summary>
    public byte _VS_excluir_foto
    {
        get
        {
            if (ViewState["_VS_excluir_foto"] != null)
                return (Byte)ViewState["_VS_excluir_foto"];
            return 0;
        }
        set
        {
            ViewState["_VS_excluir_foto"] = value;
        }
    }

    /// <summary>
    /// Retorna o tipo de pessoa
    /// 1 - Aluno
    /// 2 - Docente
    /// 3 - Colaborador
    /// </summary>
    public byte _VS_tipoPessoa
    {
        get
        {
            if (ViewState["_VS_tipoPessoa"] != null)
                return (Byte)ViewState["_VS_tipoPessoa"];
            return 0;
        }
        set
        {
            ViewState["_VS_tipoPessoa"] = value;
        }
    }

    /// <summary>
    /// Retorna e atribui o ViewState com o id do aluno
    /// </summary>
    public long _VS_alu_id
    {
        get
        {
            if (ViewState["_VS_alu_id"] != null)
                return (Int64)ViewState["_VS_alu_id"];
            return -1;
        }
        set
        {
            ViewState["_VS_alu_id"] = value;
        }
    }

    /// <summary>
    /// Configura o label do nome.
    /// </summary>
    public string LabelNome
    {
        set
        {
            lblNome.Text = value;
            _rfvNome.ErrorMessage = value.Replace('*', ' ') + " é obrigatório.";
        }
    }

    /// <summary>
    /// Retorna a StructColaboradorFiliacao da mãe
    /// </summary>
    public StructColaboradorFiliacao InformacoesMae
    {
        get
        {
            return ArrumaStructMae();
        }
    }

    /// <summary>
    /// Retorna a StructColaboradorFiliacao do pai
    /// </summary>
    public StructColaboradorFiliacao InformacoesPai
    {
        get
        {
            return ArrumaStructPai();
        }
    }

    /// <summary>
    /// Retorna e atribui o ViewState com o id da naturalidade
    /// </summary>
    public Guid _VS_cid_id
    {
        get
        {
            if (!string.IsNullOrEmpty(_txtCid_id.Value))
                return new Guid(_txtCid_id.Value);
            if (ViewState["_VS_cid_id"] != null)
                return new Guid(ViewState["_VS_cid_id"].ToString());
            return Guid.Empty;
        }
        set
        {
            ViewState["_VS_cid_id"] = value;
        }
    }

    /// <summary>
    /// Retorna e atribui o ViewState com o id do pai
    /// </summary>
    public Guid _VS_pes_idFiliacaoPai
    {
        get
        {
            PES_PessoaDocumento pesDoc = PES_PessoaDocumentoBO.GetEntityBy_Documento(txtCPFPai.Text, new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF)));

            if ((!string.IsNullOrEmpty(txtCPFPai.Text.Trim())) && (pesDoc.pes_id != Guid.Empty))
            {
                return pesDoc.pes_id;
            }

            if (!string.IsNullOrEmpty(_txtPes_idFiliacaoPai.Value))
                return new Guid(_txtPes_idFiliacaoPai.Value);

            if (ViewState["_VS_pes_idFiliacaoPai"] == null)
                return Guid.Empty;

            return new Guid(ViewState["_VS_pes_idFiliacaoPai"].ToString());
        }
        set
        {
            ViewState["_VS_pes_idFiliacaoPai"] = value;
        }
    }

    /// <summary>
    /// Retorna e atribui o ViewState com o id da mae
    /// </summary>
    public Guid _VS_pes_idFiliacaoMae
    {
        get
        {
            PES_PessoaDocumento pesDoc = PES_PessoaDocumentoBO.GetEntityBy_Documento(txtCPFMae.Text, new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF)));

            if ((!string.IsNullOrEmpty(txtCPFMae.Text.Trim())) && (pesDoc.pes_id != Guid.Empty))
            {
                return pesDoc.pes_id;
            }

            if (!string.IsNullOrEmpty(_txtPes_idFiliacaoMae.Value))
                return new Guid(_txtPes_idFiliacaoMae.Value);

            if (ViewState["_VS_pes_idFiliacaoMae"] == null)
                return Guid.Empty;

            return new Guid(ViewState["_VS_pes_idFiliacaoMae"].ToString());
        }
        set
        {
            ViewState["_VS_pes_idFiliacaoMae"] = value;
        }
    }

    /// <summary>
    /// Retorna e atribui o ViewState com o id da pessoa
    /// </summary>
    public Guid _VS_pes_id
    {
        get
        {
            if (ViewState["_VS_pes_id"] != null)
                return new Guid(ViewState["_VS_pes_id"].ToString());
            return Guid.Empty;
        }
        set
        {
            ViewState["_VS_pes_id"] = value;
        }
    }

    /// <summary>
    /// Retorna e atribui o ViewState com o tipo de busca de pessoa
    /// 1 - pai
    /// 2 - mãe
    /// </summary>
    public byte _VS_tipoBuscaPessoa
    {
        get
        {
            if (ViewState["_VS_tipoBuscaPessoa"] != null)
                return Convert.ToByte(ViewState["_VS_tipoBuscaPessoa"].ToString());
            return 0;
        }
        set
        {
            ViewState["_VS_tipoBuscaPessoa"] = value;
        }
    }

    /// <summary>
    /// Retorno e atribui valores para o textbox Nome
    /// </summary>
    public TextBox _txtNome
    {
        get
        {
            return txtNome;
        }
        set
        {
            txtNome = value;
        }
    }

    /// <summary>
    /// Retorno e atribui valores para o textbox Nome social
    /// </summary>
    public TextBox _txtNomeSocial
    {
        get
        {
            return txtNomeSocial;
        }
        set
        {
            txtNomeSocial = value;
        }
    }

    public bool MostraNomeSocial
    {
        set
        {
            trNomeSocial.Visible = value;
        }
    }

    public Label _labelNome
    {
        get
        {
            return lblNome;
        }
        set
        {
            lblNome = value;
        }
    }

    public RequiredFieldValidator rfvNome
    {
        get
        {
            return _rfvNome;
        }
        set
        {
            _rfvNome = value;
        }
    }

    /// <summary>
    /// Retorno e atribui valores para o textbox Nome Abreviado
    /// </summary>
    public TextBox _txtNomeAbreviado
    {
        get
        {
            return txtNomeAbreviado;
        }
        set
        {
            txtNomeAbreviado = value;
        }
    }

    public Label _lblDataNasc
    {
        get
        {
            return LabelDataNasc;
        }
        set
        {
            LabelDataNasc = value;
        }
    }

    public Label _lblPai
    {
        get
        {
            return LabelPai;
        }
        set
        {
            LabelPai = value;
        }
    }

    public Label _lblMae
    {
        get
        {
            return LabelMae;
        }
        set
        {
            LabelMae = value;
        }
    }

    /// <summary>
    /// Atribui valores para o combo Nacionalidade
    /// </summary>
    public Guid ComboNacionalidade1_Valor
    {
        get
        {
            return ComboNacionalidade1.Valor;
        }
        set
        {
            ComboNacionalidade1.Valor = value;
        }
    }

    /// <summary>
    /// Retorno e atribui valores para o checkbox Naturalizado
    /// </summary>
    public CheckBox _chkNaturalizado
    {
        get
        {
            return chkNaturalizado;
        }
        set
        {
            chkNaturalizado = value;
        }
    }

    /// <summary>
    /// Retorno e atribui valores para o textbox Naturalidade
    /// </summary>
    public TextBox _txtNaturalidade
    {
        get
        {
            return txtNaturalidade;
        }
        set
        {
            txtNaturalidade = value;
        }
    }

    /// <summary>
    /// Retorno e atribui valores para o textbox Data Nascimento
    /// </summary>
    public TextBox _txtDataNasc
    {
        get
        {
            return txtDataNasc;
        }
        set
        {
            txtDataNasc = value;
        }
    }

    /// <summary>
    /// Retorno e atribui valores para o textbox Pai
    /// </summary>
    public TextBox _txtPai
    {
        get
        {
            return txtPai;
        }
        set
        {
            txtPai = value;
        }
    }

    /// <summary>
    /// Retorno e atribui valores para o textbox CPF do Pai
    /// </summary>
    public string txtCPFPaiValor
    {
        set
        {
            txtCPFPai.Text = value;
        }
    }

    /// <summary>
    /// Retorno e atribui valores para o textbox RG do Pai
    /// </summary>
    public string txtRGPaiValor
    {
        set
        {
            txtRGPai.Text = value;
        }
    }

    /// <summary>
    /// Retorno e atribui valores para o textbox CPF da Mae
    /// </summary>
    public string txtCPFMaeValor
    {
        set
        {
            txtCPFMae.Text = value;
        }
    }

    /// <summary>
    /// Retorno e atribui valores para o textbox RG da Mae
    /// </summary>
    public string txtRGMaeValor
    {
        set
        {
            txtRGMae.Text = value;
        }
    }

    /// <summary>
    /// Retorno e atribui valores para o textbox Mae
    /// </summary>
    public TextBox _txtMae
    {
        get
        {
            return txtMae;
        }
        set
        {
            txtMae = value;
        }
    }

    /// <summary>
    /// Atribui valores para o combo Estado Civil
    /// </summary>
    public DropDownList _ComboEstadoCivil
    {
        get
        {
            return UCComboEstadoCivil1._Combo;
        }
        set
        {
            UCComboEstadoCivil1._Combo = value;
        }
    }

    /// <summary>
    /// Atribui valores para o combo Raça Cor
    /// </summary>
    public DropDownList _ComboRacaCor
    {
        get
        {
            return UCComboRacaCor1._Combo;
        }
        set
        {
            UCComboRacaCor1._Combo = value;
        }
    }

    /// <summary>
    /// Atribui valores para o combo Sexo
    /// </summary>
    public DropDownList _ComboSexo
    {
        get
        {
            return UCComboSexo1._Combo;
        }
        set
        {
            UCComboSexo1._Combo = value;
        }
    }

    /// <summary>
    /// Atribui valores para o combo Tipo de deficiência
    /// </summary>
    public DropDownList _ComboTipoDeficiencia
    {
        get
        {
            return ComboTipoDeficiencia1._Combo;
        }
        set
        {
            ComboTipoDeficiencia1._Combo = value;
        }
    }

    /// <summary>
    /// Atribui valores para o Label Escolaridade
    /// </summary>
    public Label _LabelEscolaridade
    {
        get
        {
            return UCComboTipoEscolaridade1._Label;
        }
        set
        {
            UCComboTipoEscolaridade1._Label = value;
        }
    }

    /// <summary>
    /// Atribui valores para o combo Escolaridade
    /// </summary>
    public DropDownList _ComboEscolaridade
    {
        get
        {
            return UCComboTipoEscolaridade1._Combo;
        }
        set
        {
            UCComboTipoEscolaridade1._Combo = value;
        }
    }

    /// <summary>
    /// Atribui valores para o Label Tipo de Deficiencia
    /// </summary>
    public string ComboTipoDeficiencia1_Titulo
    {
        set
        {
            ComboTipoDeficiencia1.Titulo = value;
        }
    }

    /// <summary>
    /// Atribui valores para o combo Tipo de Deficiencia
    /// </summary>
    public Guid ComboTipoDeficiencia1_Valor
    {
        get
        {
            return ComboTipoDeficiencia1.Valor;
        }
        set
        {
            ComboTipoDeficiencia1.Valor = value;
        }
    }

    /// <summary>
    /// Retorno e atribui valores para o UpdatePanel
    /// </summary>
    public UpdatePanel _updCadastroPessoas
    {
        get
        {
            return _updCadastroPessoa;
        }
        set
        {
            _updCadastroPessoa = value;
        }
    }

    public Label _lblFoto
    {
        get
        {
            return LabelFoto;
        }
        set
        {
            LabelFoto = value;
        }
    }

    public HtmlInputFile _iptFoto
    {
        get
        {
            return iptFoto;
        }
        set
        {
            iptFoto = value;
        }
    }

    public CheckBox _chbExcluirImagem
    {
        get
        {
            return chbExcluirImagem;
        }
        set
        {
            chbExcluirImagem = value;
        }
    }

    public Button _btnCapturaFoto
    {
        get
        {
            return btnCapturaFoto;
        }
        set
        {
            btnCapturaFoto = value;
        }
    }

    public bool visibleFoto
    {
        set
        {
            tdFoto.Visible = value;
            tdTipoEscolaridade.ColSpan = value ? 2 : 3;
        }
    }

    /// <summary>
    /// Seta o atributo Visible na div Pai.
    /// </summary>
    public bool visiblePai
    {
        set
        {
            divPai.Visible = value;
        }
    }

    /// <summary>
    /// Seta o atributo Visible na div Mãe.
    /// </summary>
    public bool visibleMae
    {
        set
        {
            divMae.Visible = value;
        }
    }

    /// <summary>
    /// Seta o atributo Visible para ops campos de escolaridade.
    /// </summary>
    public bool visibleEscolaridade
    {
        set
        {
            _LabelEscolaridade.Visible = value;
            _ComboEscolaridade.Visible = value;
        }
    }

    /// <summary>
    /// Seta o atributo Visible no botão de limpar mãe.
    /// </summary>
    public bool visibleLimparMae
    {
        set
        {
            _btnLimparMae.Visible = value;
        }
    }

    /// <summary>
    /// Seta o atributo Visible no botão de limpar pai.
    /// </summary>
    public bool visibleLimparPai
    {
        set
        {
            _btnLimparPai.Visible = value;
        }
    }

    /// <summary>
    /// Seta a verificaçao de validaçao da data de nascimento.
    /// </summary>
    public bool validaDataNascimento
    {
        set
        {
            if (value && !LabelDataNasc.Text.EndsWith("*"))
                LabelDataNasc.Text += " *";
            _rfvDataNasc.Visible = value;
        }
    }

    /// <summary>
    /// Seta o ValidationGroup dos validators.
    /// </summary>
    public string ValidationGroup
    {
        set
        {
            _rfvNome.ValidationGroup = value;
            _rfvDataNasc.ValidationGroup = value;
            cvDataNascimento.ValidationGroup = value;
            UCComboSexo1.ValidationGroup = value;
        }
    }

    /// <summary>
    /// Propriedade que seta a label e a validação do combo
    /// </summary>
    public bool _ComboSexoObrigatorio
    {
        set
        {
            UCComboSexo1.Obrigatorio = value;
        }
    }

    public bool visibleAlunoDeficiente
    {
        set
        {
            trDeficientes.Visible = value;
        }
    }

    #endregion

    #region Eventos Page Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(Page);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroAlunoMatricula.js"));
        }

        string script = String.Format("SetConfirmDialogButton('{0}','{1}');",
                        String.Concat("#", btnExcluir.ClientID), "Confirma a exclusão da foto?<br/> Atenção, essa operação não poderá ser desfeita e as alterações realizadas serão perdidas.");

        Page.ClientScript.RegisterStartupScript(GetType(), btnExcluir.ClientID, script, true);

        if (!IsPostBack)
        {
            try
            {
                string pessoa = string.Empty;

                if (_VS_tipoPessoa == 1)
                    pessoa = " do aluno";
                if (_VS_tipoPessoa == 2)
                    pessoa = " do docente";
                if (_VS_tipoPessoa == 3)
                    pessoa = " do colaborador";

                cvDataNascimento.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de nascimento" + pessoa);

                ComboNacionalidade1.Carregar();

                ComboTipoDeficiencia1.Carregar();
                ComboTipoDeficiencia1.Titulo = GestaoEscolarUtilBO.nomePadraoTipoDeficiencia(__SessionWEB.__UsuarioWEB.Usuario.ent_id);

                UCComboTipoEscolaridade1._MostrarMessageSelecione = true;
                UCComboTipoEscolaridade1._Load(0);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
            }
        }

        if (_VS_alu_id > 0)
            ComboTipoDeficiencia1.OnSeletedIndexChanged += UCComboTipoDeficiencia1_IndexChanged;
    }

    #endregion

    #region Eventos

    protected void _btnPai_Click(object sender, ImageClickEventArgs e)
    {
        _VS_tipoBuscaPessoa = 1;
        _SelecionarPessoa();
    }

    protected void _btnMae_Click(object sender, ImageClickEventArgs e)
    {
        _VS_tipoBuscaPessoa = 2;
        _SelecionarPessoa();
    }

    protected void btnCadastraCidade_Click(object sender, ImageClickEventArgs e)
    {
        if (_AbreJanelaCadastroCidade != null)
            _AbreJanelaCadastroCidade();
    }

    protected void _btnLimparPai_Click(object sender, ImageClickEventArgs e)
    {
        _VS_pes_idFiliacaoPai = Guid.Empty;
        _txtPes_idFiliacaoPai.Value = string.Empty;
        _txtPai.Text = string.Empty;
        txtRGPai.Text = string.Empty;
        txtCPFPai.Text = string.Empty;
        _btnLimparPai.Visible = false;
        rfvPai.Visible = false;
        if (LabelPai.Text.EndsWith("*"))
        {
            LabelPai.Text = LabelPai.Text.Substring(0, LabelPai.Text.Length - 2);
        }
    }

    protected void _btnLimparMae_Click(object sender, ImageClickEventArgs e)
    {
        _VS_pes_idFiliacaoMae = Guid.Empty;
        _txtPes_idFiliacaoMae.Value = string.Empty;
        _txtMae.Text = string.Empty;
        txtRGMae.Text = string.Empty;
        txtCPFMae.Text = string.Empty;
        _btnLimparMae.Visible = false;
        rfvMae.Visible = false;
        if (LabelMae.Text.EndsWith("*"))
        {
            LabelMae.Text = LabelMae.Text.Substring(0, LabelMae.Text.Length - 2);
        }
    }

    protected void btnExcluir_Click(object sender, EventArgs e)
    {
        try
        {
            CFG_Arquivo entArquivo = PES_PessoaBO.RetornaFotoPor_Pessoa(_VS_pes_id);
            if (entArquivo.IsNew && _iptFoto.PostedFile != null)
                entArquivo = CFG_ArquivoBO.CriarEntidadeArquivo(_iptFoto.PostedFile);

            _VS_excluir_foto = (byte)(ACA_AlunoBO.ExcluirFotoAluno(_VS_pes_id, entArquivo.arq_id) ? 1 : 2);

            string script = String.Format("RemoveConfirmDialogButton('{0}');", String.Concat("#", btnCapturaFoto.ClientID));
            btnCapturaFoto.OnClientClick = script;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMensagem.Text = UtilBO.GetErroMessage("Erro ao tentar excluir foto.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void btnCapturaFoto_Click(object sender, EventArgs e)
    {
        Session.Add("alu_id", _VS_alu_id);
        Response.Redirect("~/Academico/Aluno/CapturaFoto/Default.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Configura os dados da foto.
    /// </summary>
    /// <param name="pagina">Página que chamou o método.</param>
    /// <param name="arq_id">Id do arquivo da foto.</param>
    public void ConfiguraDadosFoto(PaginaGestao pagina, out long arq_id)
    {
        try
        {
            if (_VS_pes_id != new Guid())
            {
                CFG_Arquivo entArquivo = PES_PessoaBO.RetornaFotoPor_Pessoa(_VS_pes_id);
                if (!entArquivo.IsNew)
                {
                    if (pagina == PaginaGestao.Alunos)
                    {
                        imgFoto.ImageUrl = "~/Academico/Aluno/CapturaFoto/Imagem.ashx?idfoto=" + entArquivo.arq_id;

                        string script = String.Format("SetConfirmDialogButton('{0}','{1}');", String.Concat("#", btnCapturaFoto.ClientID), String.Format("Deseja substituir a foto atual por uma nova foto?"));
                        Page.ClientScript.RegisterStartupScript(GetType(), btnCapturaFoto.ClientID, script, true);
                    }
                    else
                        imgFoto.ImageUrl = __SessionWEB._AreaAtual._Diretorio + "Academico/RecursosHumanos/Colaborador/Imagem.ashx?id=" + _VS_pes_id;

                    SetarDataImagem(entArquivo);
                }

                // Configura imagem da pessoa, caso existir
                if (entArquivo.arq_data != null && entArquivo.arq_data.Length > 1)
                {
                    if (pagina == PaginaGestao.Alunos)
                        imgFoto.ImageUrl = "~/Academico/RecursosHumanos/Colaborador/Imagem.ashx?id=" + _VS_pes_id;

                    System.Drawing.Image img;
                    using (MemoryStream ms = new MemoryStream(entArquivo.arq_data, 0, entArquivo.arq_data.Length))
                    {
                        ms.Write(entArquivo.arq_data, 0, entArquivo.arq_data.Length);
                        img = System.Drawing.Image.FromStream(ms, true);
                    }

                    const int larguraMaxima = 200;
                    const int alturaMaxima = 200;
                    int alt;
                    int lar;

                    decimal proporcaoOriginal = (decimal)((img.Height * 100) / img.Width) / 100;

                    if (proporcaoOriginal > 1)
                    {
                        proporcaoOriginal = (decimal)((img.Width * 100) / img.Height) / 100;
                        alt = alturaMaxima;
                        lar = Convert.ToInt32(alturaMaxima * proporcaoOriginal);
                    }
                    else
                    {
                        lar = larguraMaxima;
                        alt = Convert.ToInt32(larguraMaxima * proporcaoOriginal);
                    }

                    imgFoto.Height = alt;
                    imgFoto.Width = lar;
                    imgFoto.Visible = true;
                    btnExcluir.Visible = true;
                    chbExcluirImagem.Visible = true;
                }
                else
                {
                    imgFoto.Visible = false;
                    btnExcluir.Visible = false;
                    lblDataFoto.Visible = false;
                    chbExcluirImagem.Visible = false;
                }

                switch (pagina)
                {
                    case PaginaGestao.Colaboradores:
                    case PaginaGestao.Docentes:
                        btnCapturaFoto.Visible = false;
                        btnExcluir.Visible = false;
                        break;

                    case PaginaGestao.Alunos:
                        btnCapturaFoto.Visible = true;
                        chbExcluirImagem.Visible = false;
                        break;
                }

                arq_id = entArquivo.arq_id;
            }
            else
                arq_id = -1;
        }
        catch
        {
            arq_id = -1;
            btnCapturaFoto.Visible = true;
            lblDataFoto.Visible = false;
            lblMensagemErroFoto.Text = UtilBO.GetErroMessage("Não foi possível carregar a foto.", UtilBO.TipoMensagem.Alerta);
        }
    }

    /// <summary>
    /// Seta o texto da data de última alteração da imagem.
    /// </summary>
    /// <param name="foto">Imagem salva para a pessoa</param>
    public void SetarDataImagem(CFG_Arquivo foto)
    {
        lblDataFoto.Visible = true;
        lblDataFoto.Text = "<br />Última alteração da foto: " + foto.arq_dataAlteracao.ToString("dd/MM/yyyy");
    }

    public void _SelecionarPessoa()
    {
        if (_Selecionar != null)
            _Selecionar();
    }

    /// <summary>
    /// Arruma a StructColaboradorFiliacao do pai, que vai retornar a estrutura com os dados necessários para salvar a pessoa no banco.
    /// </summary>
    public StructColaboradorFiliacao ArrumaStructPai()
    {
        if (ValidaCampoDocumento(true))
            throw new ValidationException("Nome do pai é obrigatório.");

        StructColaboradorFiliacao colaborador = new StructColaboradorFiliacao();
        
        // se a textbox estiver vazia, nao há dados do pai
        if (!string.IsNullOrEmpty(txtPai.Text))
        {
            PES_Pessoa entPessoa = new PES_Pessoa();
            List<PES_PessoaDocumento> lstDocumento = new List<PES_PessoaDocumento>();
            PES_PessoaDocumento cpf = new PES_PessoaDocumento
                                          {
                                              tdo_id = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF))
                                          };
            PES_PessoaDocumento rg = new PES_PessoaDocumento
                                         {
                                             tdo_id = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_RG))
                                         };
            if (_VS_pes_idFiliacaoPai != Guid.Empty)
            {
                entPessoa.pes_id = _VS_pes_idFiliacaoPai;
                PES_PessoaBO.GetEntity(entPessoa);
            }
            else
            {
                entPessoa.IsNew = true;
                cpf.IsNew = true;
                rg.IsNew = true;
            }
            entPessoa.pes_nome = txtPai.Text;
            entPessoa.pes_situacao = 1;
            // Adiciona dados do CPF
            if (!string.IsNullOrEmpty(txtCPFPai.Text))
            {
                if (!UtilBO._ValidaCPF(txtCPFPai.Text))
                    throw new ValidationException("CPF do pai é inválido.");

                cpf.pes_id = _VS_pes_idFiliacaoPai;
                PES_PessoaDocumentoBO.GetEntity(cpf);
                cpf.psd_numero = txtCPFPai.Text;
                cpf.psd_situacao = 1;
                lstDocumento.Add(cpf);
            }

            // adiciona dados do Rg
            if (!string.IsNullOrEmpty(txtRGPai.Text))
            {
                rg.pes_id = _VS_pes_idFiliacaoPai;
                PES_PessoaDocumentoBO.GetEntity(rg);
                rg.psd_numero = txtRGPai.Text;
                rg.psd_situacao = 1;
                lstDocumento.Add(rg);
            }

            colaborador.listaDocumentos = lstDocumento;
            colaborador.entPessoa = entPessoa;
        }

        return colaborador;
    }

    /// <summary>
    /// Arruma a StructColaboradorFiliacao da mae, que vai retornar a estrutura com os dados necessários para salvar a pessoa no banco.
    /// </summary>
    public StructColaboradorFiliacao ArrumaStructMae()
    {
        if (ValidaCampoDocumento(false))
            throw new ValidationException("Nome da mãe é obrigatório.");

        StructColaboradorFiliacao colaborador = new StructColaboradorFiliacao();

        // se a textbox estiver vazia, nao há dados da mãe
        if (!string.IsNullOrEmpty(txtMae.Text))
        {
            PES_Pessoa entPessoa = new PES_Pessoa();
            List<PES_PessoaDocumento> lstDocumento = new List<PES_PessoaDocumento>();
            PES_PessoaDocumento cpf = new PES_PessoaDocumento
                                          {
                                              tdo_id = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF))
                                          };
            PES_PessoaDocumento rg = new PES_PessoaDocumento
                                         {
                                             tdo_id = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_RG))
                                         };
            if (_VS_pes_idFiliacaoMae != Guid.Empty)
            {
                entPessoa.pes_id = _VS_pes_idFiliacaoMae;
                PES_PessoaBO.GetEntity(entPessoa);
            }
            else
            {
                entPessoa.IsNew = true;
                cpf.IsNew = true;
                rg.IsNew = true;
            }

            entPessoa.pes_nome = txtMae.Text;
            entPessoa.pes_situacao = 1;

            // Adiciona dados do CPF
            if (!string.IsNullOrEmpty(txtCPFMae.Text))
            {
                if (!UtilBO._ValidaCPF(txtCPFMae.Text))
                    throw new ValidationException("CPF da mãe é inválido.");

                cpf.pes_id = _VS_pes_idFiliacaoMae;
                PES_PessoaDocumentoBO.GetEntity(cpf);
                cpf.psd_numero = txtCPFMae.Text;
                cpf.psd_situacao = 1;
                lstDocumento.Add(cpf);
            }

            // Adiciona dados do Rg
            if (!string.IsNullOrEmpty(txtRGMae.Text))
            {
                rg.pes_id = _VS_pes_idFiliacaoMae;
                PES_PessoaDocumentoBO.GetEntity(rg);
                rg.psd_numero = txtRGMae.Text;
                rg.psd_situacao = 1;
                lstDocumento.Add(rg);
            }

            colaborador.listaDocumentos = lstDocumento;
            colaborador.entPessoa = entPessoa;
        }

        return colaborador;
    }

    /// <summary>
    /// Caso o campos de CPF ou RG dos responsáveis estejam preenchidos
    /// será obrigatório digitar o nome.
    /// </summary>
    private bool ValidaCampoDocumento(bool pai)
    {
        // Valida os campos referente ao pai        
        if (pai)
        {
            return (
               (!string.IsNullOrEmpty(txtCPFPai.Text) || !string.IsNullOrEmpty(txtRGPai.Text))
               && string.IsNullOrEmpty(txtPai.Text)
               );
        }

        // Valida os campos referente a mae
        return (
                   (!string.IsNullOrEmpty(txtCPFMae.Text) || !string.IsNullOrEmpty(txtRGMae.Text))
                   && string.IsNullOrEmpty(txtMae.Text)
               );
    }

    public void InicializarAlunoDeficiente()
    {
        UCAlunoDeficiente1.Inicializar();
    }

    public void CarregaAlunoDeficiente(long alu_id)
    {
        trDeficientes.Visible = true;
        UCAlunoDeficiente1.CarregarAluno(alu_id);
    }

    /// <summary>
    /// Retorna todos id dos itens selecionado em chlEquipamentos
    /// </summary>
    /// <returns></returns>
    public List<int> RetornaEquipamentosSelcionados()
    {
        return UCAlunoDeficiente1.RetornaEquipamentosSelcionados();
    }
    
    public void RetiraSelecao()
    {
        UCAlunoDeficiente1.RetiraSelecao();
    }

    /// <summary>
    /// Método para habilitar somente o gridview quando todo o resto está bloqueado, uso no cadastro de aluno
    /// </summary>
    public void HabilitaCamposFoto()
    {
        HabilitaControles(this.Controls, false);
        HabilitaControles(upnFoto.Controls, true);
        //HabilitaControles(pnlHistoricoMovimentacoes.Controls, true);
    }

    /// <summary>
    /// Seta a propriedade Enabled passada para todos os WebControl do ControlCollection
    /// passado.
    /// </summary>
    /// <param name="controls"></param>
    /// <param name="enabled"></param>
    private void HabilitaControles(ControlCollection controls, bool enabled)
    {
        foreach (Control c in controls)
        {
            if (c.Controls.Count > 0)
                HabilitaControles(c.Controls, enabled);

            WebControl wb = c as WebControl;

            if (wb != null)
                wb.Enabled = enabled;
        }
    }

    #endregion
}

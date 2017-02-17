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
using MSTech.Security.Cryptography;
using MSTech.Validation.Exceptions;
using ReportNameDocumentos = MSTech.GestaoEscolar.BLL.ReportNameDocumentos;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.GestaoEscolar.Web.WebProject.Util;

public partial class Academico_Aluno_Cadastro : MotherPageLogado
{
    #region Variáveis - método assíncrono

    // Variáveis usadas para carregar a aba de movimentacao assincronamente.
    private delegate void InvokerCarregaAbaMovimentacao(ACA_Aluno entityAluno);

    private InvokerCarregaAbaMovimentacao methodMov;
    private IAsyncResult resMov;

    #endregion Variáveis - método assíncrono

    #region Constantes

    private const string INDEX_ABA_ALUNO = "0";
    private const string INDEX_ABA_ENDERECOCONTATO = "1";
    private const string INDEX_ABA_DOCUMENTACAO = "2";
    private const string INDEX_ABA_MOVIMENTACAO = "3";
    private const string INDEX_ABA_FICHAMEDICA = "4";

    #endregion Constantes

    #region Propriedades

    /// <summary>
    /// Retorna se vai exibir o campo de gemeos
    /// </summary>
    private bool ExibirCampoAlunoGemeo
    {
        get { return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_CAMPO_ALUNO_GEMEO, __SessionWEB.__UsuarioWEB.Usuario.ent_id); }
    }

    /// <summary>
    /// ID do aluno.
    /// </summary>
    private long VS_alu_id
    {
        get
        {
            if (ViewState["VS_alu_id"] != null)
            {
                return Convert.ToInt64(ViewState["VS_alu_id"]);
            }

            return -1;
        }

        set
        {
            ViewState["VS_alu_id"] = value;
        }
    }

    /// <summary>
    /// ID do aluno.
    /// </summary>
    private bool VS_permissao
    {
        get
        {
            if (ViewState["VS_permissao"] != null)
            {
                return Convert.ToBoolean(ViewState["VS_permissao"]);
            }

            return false;
        }

        set
        {
            ViewState["VS_permissao"] = value;
        }
    }

    /// <summary>
    /// ID do usuário do aluno.
    /// </summary>
    private Guid VS_usu_id
    {
        get
        {
            if (ViewState["VS_usu_id"] != null)
            {
                return new Guid(ViewState["VS_usu_id"].ToString());
            }

            return Guid.Empty;
        }

        set
        {
            ViewState["VS_usu_id"] = value;
        }
    }

    /// <summary>
    /// Usuario atual é do AD
    /// </summary>
    public bool VS_podeAlterarFotoAluno
    {
        get
        {
            return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_ALTERAR_FOTO_ALUNO_CONSULTA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }
    }

    private DataTable dtUltMatricula;

    /// <summary>
    /// Datatable pra receber os dados da ultima matricula.
    /// </summary>
    private DataTable AlunoUltimaMatricula
    {
        get
        {
            return dtUltMatricula ?? (dtUltMatricula = VS_alu_id > 0
                ? ACA_AlunoCurriculoBO.SelecionaDadosUltimaMatricula(VS_alu_id)
                : new DataTable());
        }
    }

    /// <summary>
    /// Retorna o tipo de movimento da última movimentação do aluno.
    /// </summary>
    private byte VS_UltimoTipoMovimentacao
    {
        get
        {
            if (ViewState["VS_UltimoTipoMovimentacao"] == null)
            {
                ViewState["VS_UltimoTipoMovimentacao"] = MTR_TipoMovimentacaoBO.TipoMovimentoUltimaMovimentacaoAluno(VS_alu_id);
            }

            return Convert.ToByte((ViewState["VS_UltimoTipoMovimentacao"]));
        }
    }

    /// <summary>
    /// Guarda em ViewState o ID do userControl de responsável que chamou o evento
    /// de abrir a busca de pessoas e setar o retorno.
    /// </summary>
    private string VS_BuscaPessoa
    {
        get
        {
            if (ViewState["VS_BuscaPessoa"] != null)
            {
                return Convert.ToString(ViewState["VS_BuscaPessoa"]);
            }

            return string.Empty;
        }

        set
        {
            ViewState["VS_BuscaPessoa"] = value;
        }
    }

    /// <summary>
    /// Data de alteração utilizada para validação de duplicidade
    /// </summary>
    private DateTime VS_dataAlteracaoAluno
    {
        get
        {
            return Convert.ToDateTime(ViewState["VS_dataAlteracaoAluno"]);
        }

        set
        {
            ViewState["VS_dataAlteracaoAluno"] = value;
        }
    }

    /// <summary>
    /// Situação do aluno
    /// </summary>
    private byte VS_alu_situacao
    {
        get
        {
            return Convert.ToByte(ViewState["VS_alu_situacao"]);
        }

        set
        {
            ViewState["VS_alu_situacao"] = value;
        }
    }

    /// <summary>
    /// Retorna o valor do parâmetro "Permanecer na tela após gravações"
    /// </summary>
    private bool ParametroPermanecerTela
    {
        get
        {
            return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }
    }

    /// <summary>
    /// Indica qual o método que chamou a confirmação padrão
    /// 1-Qtde. alunos na turma
    /// 2-Alunos excedentes
    /// 3-Movimentação retroativa
    /// </summary>
    public byte VS_ConfirmacaoPadrao
    {
        get
        {
            if (ViewState["VS_ConfirmacaoPadrao"] != null)
            {
                return Convert.ToByte(ViewState["VS_ConfirmacaoPadrao"]);
            }

            return 0;
        }

        set
        {
            ViewState["VS_ConfirmacaoPadrao"] = value;
        }
    }

    /// <summary>
    /// ID da escola e da unidade de escola: "esc_id; uni_id".
    /// </summary>
    private string VS_esc_id_uni_id
    {
        get
        {
            if (ViewState["VS_esc_id_uni_id"] != null)
            {
                return ViewState["VS_esc_id_uni_id"].ToString();
            }

            return "-1;-1";
        }

        set
        {
            ViewState["VS_esc_id_uni_id"] = value;
        }
    }

    /// <summary>
    /// Parâmetro acadêmico que indica se a verificação de duplicidade fonética é verificada.
    /// </summary>
    private bool VerificarAlunoIntegridadeFonetica
    {
        get
        {
            return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.VERIFICAR_ALUNO_INTEGRIDADE_FONETICA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }
    }

    /// <summary>
    /// Propriedade que seta a url de retorno da página.
    /// </summary>
    private string VS_PaginaRetorno
    {
        get
        {
            if (ViewState["VS_PaginaRetorno"] != null)
                return ViewState["VS_PaginaRetorno"].ToString();

            return "";
        }
        set
        {
            ViewState["VS_PaginaRetorno"] = value;
        }
    }

    /// <summary>
    /// Propriedade que guarda dados necessários para a página de retorno.
    /// </summary>
    private object VS_DadosPaginaRetorno
    {
        get
        {
            return ViewState["VS_DadosPaginaRetorno"];
        }
        set
        {
            ViewState["VS_DadosPaginaRetorno"] = value;
        }
    }

    /// <summary>
    /// Propriedade que guarda dados necessários para a página de retorno Minhas turmas.
    /// </summary>
    private object VS_DadosPaginaRetorno_MinhasTurmas
    {
        get
        {
            return ViewState["VS_DadosPaginaRetorno_MinhasTurmas"];
        }
        set
        {
            ViewState["VS_DadosPaginaRetorno_MinhasTurmas"] = value;
        }
    }

    /// <summary>
    /// ViewState que armazena a lista de anexos do aluno.
    /// </summary>
    private List<ACA_AlunoAnexo> VS_listaAnexos
    {
        get
        {
            return (List<ACA_AlunoAnexo>)(ViewState["VS_listaAnexos"] ?? new List<ACA_AlunoAnexo>());
        }

        set
        {
            ViewState["VS_listaAnexos"] = value;
        }
    }

    #endregion Propriedades

    #region Eventos Page Life Cycle

    protected void Page_Init(object sender, EventArgs e)
    {
        // Setar user control de cidades para o user control de movimentação usá-lo.
        UCMovimentacao1.UCCidades1 = UCCadastroCidade1;
        UCMovimentacao1.UpnCadastroCidades = updCadastroCidade;

        UCCadastroPessoa1._ComboTipoDeficiencia.Width = 380;

        trPossuiGemeo.Visible = ExibirCampoAlunoGemeo;

        if (!IsPostBack)
        {
            cvDataCadastroFisico.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de cadastro físico");
            Page.Form.Enctype = "multipart/form-data";

            try
            {
                // Configuração utilizando o parâmetro "Permanecer na tela após gravações"
                if (ParametroPermanecerTela && (!Convert.ToString(btnCancelar.CssClass).Contains("btnMensagemUnload")))
                {
                    btnCancelar.CssClass += " btnMensagemUnload";
                }

                if (ParametroPermanecerTela && (!Convert.ToString(btnCancelarCima.CssClass).Contains("btnMensagemUnload")))
                {
                    btnCancelarCima.CssClass += " btnMensagemUnload";
                }

                bool documentoResponsavel = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.DOCUMENTO_OBRIGATORIO_RESPONSAVEL_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                MsgResponsavel.Visible = documentoResponsavel;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os parâmetros do sistema.", UtilBO.TipoMensagem.Erro);
            }
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
            sm.Scripts.Add(new ScriptReference(ArquivoJS.ExitPageConfirm));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsSetExitPageConfirmer.js"));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroPessoa.js"));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroAluno.js"));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsUCCadastroEndereco.js"));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroCertidaoCivil.js"));
            sm.Services.Add(new ServiceReference("~/WSServicos.asmx"));
        }
        UCCertidaoCivil1._AbreJanelaCadastroCidade += AbreJanelaCadastroCidade;

        bool permiteEditarFichaMedica = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_EDITAR_FICHA_MEDICA_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

        // Comentado pois a foto so podera ser alterada pela tela de captura de foto quando o usuario nao tiver permissao
        // bool podeAlterarFotoAluno = ACA_ParametroAcademicoBO.ParametroValorBooleano(eChaveAcademico.PERMITIR_ALTERAR_FOTO_ALUNO_CONSULTA);

        if (!IsPostBack)
        {
            // Verifica menssagem de session
            string message = __SessionWEB.PostMessages;
            if (!String.IsNullOrEmpty(message))
            {
                lblMessage.Text = message;
            }

            try
            {
                // Armazena ID do usuário para alteração, caso necessário
                if ((PreviousPage != null) && PreviousPage.IsCrossPagePostBack)
                {
                    VS_alu_id = PreviousPage.EditItem;
                    VS_permissao = PreviousPage.EditItem_Permissao;
                }
                else if (Session["aluno"] != null)
                {
                    long alu_id;
                    long.TryParse(Session["aluno"].ToString(), out alu_id);
                    VS_alu_id = alu_id;

                    Session.Remove("aluno");

                    if (Session["permissao"] != null)
                    {
                        bool permissao;
                        Boolean.TryParse(Session["permissao"].ToString(), out permissao);
                        VS_permissao = permissao;

                        Session.Remove("permissao");
                    }

                    if (Session["PaginaRetorno_AlteracaoAluno"] != null)
                    {
                        VS_PaginaRetorno = Session["PaginaRetorno_AlteracaoAluno"].ToString();
                        Session.Remove("PaginaRetorno_AlteracaoAluno");

                        VS_DadosPaginaRetorno = Session["DadosPaginaRetorno"];
                        Session.Remove("DadosPaginaRetorno");

                        VS_DadosPaginaRetorno_MinhasTurmas = Session["VS_DadosTurmas"];
                        Session.Remove("VS_DadosTurmas");
                    }
                }

                // Inicializa os componentes da tela
                InicializaTela();

                // Carrega os dados do aluno na tela
                CarregaAluno();

                UCCadastroPessoa1._VS_tipoPessoa = 1;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados do aluno.", UtilBO.TipoMensagem.Erro);
            }

            bool podeEditar = (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && VS_alu_id > 0);


            if (!podeEditar || !VS_permissao)
            {
                btnCancelar.Text = btnCancelarCima.Text = permiteEditarFichaMedica ? "Cancelar" : "Voltar";

                btnSalvarCima.Visible = btnSalvar.Visible = permiteEditarFichaMedica;

                HabilitaControles(fdsFichaMedica.Controls, permiteEditarFichaMedica);
            }

            divBtnVacinacao.Visible = !String.IsNullOrEmpty(ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.NOME_WS_RETORNA_IMAGEM_CARTEIRA_VACINACAO,
                                                                               __SessionWEB.__UsuarioWEB.Usuario.ent_id));

            Page.Form.DefaultButton = btnSalvar.UniqueID;
            Page.Form.DefaultFocus = UCCadastroPessoa1._txtNome.ClientID;

        }

        // Atualiza os componentes da tela
        AtualizaTela();

        bool podeEditarAluno = (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && VS_alu_id > 0);

        RegistrarParametrosMensagemSair(btnSalvar.Visible, (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0));


        HabilitaControles(fdsDadosPessoais.Controls, false);
        HabilitaControles(fdsEndereco.Controls, false);
        HabilitaControles(fdsContato.Controls, false);
        HabilitaControles(fdsDocumento.Controls, false);
        HabilitaControles(fdsCertidoes.Controls, false);
        HabilitaControles(fdsCertidoes.Controls, false);
        HabilitaControles(fdsMovimentacao.Controls, false);
        HabilitaControles(fdsAnexo.Controls, false);


        if (!podeEditarAluno || !VS_permissao)
        {
            HabilitaControles(fdsFichaMedica.Controls, permiteEditarFichaMedica);
            HabilitaControles(fdsCasoEmergencia.Controls, permiteEditarFichaMedica);
            // Comentado pois a foto so podera ser alterada pela tela de captura de foto quando o usuario nao tiver permissao
            //if (VS_podeAlterarFotoAluno)
            //{
            //UCCadastroPessoa1.HabilitaCamposFoto();
            //}
        }
    }

    #endregion Eventos Page Life Cycle

    #region Delegates

    protected void SetaBuscaPessoa(WebControls_AlunoResponsavel_UCAlunoResponsavel sender)
    {
        VS_BuscaPessoa = sender.ID;
    }

    protected void UCBuscaPessoasAluno1_ReturnValues(IDictionary<string, object> parameters)
    {
        updResponsavel.Update();
    }

    protected void AbreJanelaCadastroCidade()
    {
        if (!UCCadastroCidade1.Visible)
        {
            UCCadastroCidade1.CarregarCombos();
        }

        UCCadastroCidade1.Visible = true;
        UCCadastroCidade1.SetaFoco();

        updCadastroCidade.Update();
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroCidadeNovo", "$('#divCadastroCidade').dialog('open');", true);
    }

    protected void UCComboTipoResponsavel_IndexChanged()
    {
        ConfiguraTipoResponsavel(true);
    }

    #endregion Delegates

    #region Eventos
    protected void btnExibirVacinacao_Click(object sender, EventArgs e)
    {
        object ws = null;
        object[] args = new object[7];

        try
        {
            string link = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.NOME_WS_RETORNA_IMAGEM_CARTEIRA_VACINACAO,
                                                                               __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            if (string.IsNullOrEmpty(link))
                throw new ValidationException("O parâmetro para o serviço da carteirinha de vacinação não está preenchido.");

            args[0] = ApplicationWEB.WsCarteiraVacinacaoMunicipio;
            args[1] = ApplicationWEB.WsCarteiraVacinacaoUserName;
            args[2] = WsProxy.GerarHashMd5(ApplicationWEB.WsCarteiraVacinacaoPassword);
            args[3] = UCCadastroPessoa1._txtNome.Text;  //"DAVI LUIS OECKSLER"
            args[4] = string.IsNullOrEmpty(UCCadastroPessoa1._txtDataNasc.Text) ? new DateTime() : Convert.ToDateTime(UCCadastroPessoa1._txtDataNasc.Text); //07/09/2014
            args[5] = UCAlunoResponsavelMae.NomePessoa; //"JULIANE GESSER LUCINDA OECKSLER"
            args[6] = string.Empty;

            ws = WsProxy.CallWebService(link, "WSIntegracaoEducacao", "OBTERCARTEIRAVACINACAO", args);
        }
        catch (ValidationException ex)
        {
            lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados do aluno.", UtilBO.TipoMensagem.Erro);
        }

        if (ws != null && ws.ToString() == "")
        {
            try
            {
                byte[] resultado = Convert.FromBase64String(args[6].ToString());
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment;filename=carteiravacinacao.pdf");
                Response.BinaryWrite(resultado);
                Response.Flush();
                Response.Close();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Aluno.Cadastro.fdsFichaMedica.lblMessage.ErroCarregarCarteiraVacinacao").ToString(), UtilBO.TipoMensagem.Erro);
            }
            Response.End();
        }
        else
        {
            lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Aluno.Cadastro.fdsFichaMedica.lblMessage.ErroCarregarCarteiraVacinacao").ToString(), UtilBO.TipoMensagem.Erro);
        }

        //imgCartVacinacao.ImageUrl = imagem;
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "ExibirCarteiraVacinacao", "$('#divCartVacinacao').dialog('open'); CarregarCarteiraVacinacao();", true);
    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        // Comentado pois a foto so podera ser alterada pela tela de captura de foto quando o usuario nao tiver permissao

        //bool podeEditar = (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && VS_alu_id > 0) ||
        //                       (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && VS_alu_id <= 0);

        if (Page.IsValid /**|| (VS_podeAlterarFotoAluno && (!podeEditar || !VS_permissao ))**/)
        {
            SalvaAluno();
        }
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(VS_PaginaRetorno))
        {
            Session["DadosPaginaRetorno"] = VS_DadosPaginaRetorno;
            Session["VS_DadosTurmas"] = VS_DadosPaginaRetorno_MinhasTurmas;
            RedirecionarPagina(VS_PaginaRetorno);
        }
        else
        {
            RedirecionarPagina("~/Academico/Aluno/Busca.aspx");
        }
    }

    protected void btnFecharNumeroMat_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "AlertaMat", "$(document).ready(function(){ $('.divNumeroMat').dialog('close'); });", true);

        if (ParametroPermanecerTela)
        {
            Session["aluno"] = VS_alu_id;
            Session["permissao"] = VS_permissao;
            Response.Redirect("Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        else
        {
            Response.Redirect("Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }

    #region Anexos do aluno

    protected void grvAnexos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int index = e.Row.RowIndex;

            long arq_id = Convert.ToInt64(DataBinder.Eval(e.Row.DataItem, "arq_id") ?? "-1");

            HyperLink hplDownloadAnexo = (HyperLink)e.Row.FindControl("hplDownloadAnexo");

            if (hplDownloadAnexo != null)
            {
                hplDownloadAnexo.Visible = arq_id > 0 && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                hplDownloadAnexo.NavigateUrl = String.Format("~/FileHandler.ashx?file={0}", arq_id);
            }

            ImageButton btnEditarAnexo = (ImageButton)e.Row.FindControl("btnEditarAnexo");

            if (btnEditarAnexo != null)
            {
                btnEditarAnexo.CommandArgument = index.ToString();
                btnEditarAnexo.Visible = arq_id > 0 && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
            }

            ImageButton btnAdicionarAnexo = (ImageButton)e.Row.FindControl("btnAdicionarAnexo");

            if (btnAdicionarAnexo != null)
            {
                btnAdicionarAnexo.CommandArgument = index.ToString();
                btnAdicionarAnexo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && index == VS_listaAnexos.Count - 1;
            }

            ImageButton btnExcluirAnexo = (ImageButton)e.Row.FindControl("btnExcluirAnexo");

            if (btnExcluirAnexo != null)
            {
                btnExcluirAnexo.CommandArgument = index.ToString();
                btnExcluirAnexo.Visible = arq_id > 0 && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
            }
        }
    }

    protected void grvAnexos_DataBinding(object sender, EventArgs e)
    {
        try
        {
            GridView grv = (GridView)sender;

            if (grv.DataSource == null)
                grv.DataSource = VS_listaAnexos;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os anexos do aluno.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void grvAnexos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Add")
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "AdicionaAnexo", "$('#divUploadAnexo').dialog('open');", true);
        }
    }

    protected void btnSalvarAnexo_Click(object sender, EventArgs e)
    {
        try
        {
            SYS_Arquivo entityArquivo = SYS_ArquivoBO.CriarAnexo(fupAnexo.PostedFile);
            entityArquivo.arq_situacao = (byte)SYS_ArquivoSituacao.Temporario;
            if (SYS_ArquivoBO.Save(entityArquivo))
            {
                if (VS_listaAnexos.Any(p => p.arq_id <= 0))
                    VS_listaAnexos.RemoveAll(p => p.arq_id <= 0);

                VS_listaAnexos.Add(new ACA_AlunoAnexo
                {
                    alu_id = VS_alu_id
                    ,
                    aan_descricao = txtDescricaoAnexo.Text
                    ,
                    arq_id = entityArquivo.arq_id
                    ,
                    arq_nome = entityArquivo.arq_nome
                    ,
                    arq_situacao = (byte)entityArquivo.arq_situacao
                    ,
                    aan_situacao = (byte)ACA_AlunoAnexoSituacao.Ativo
                });

                grvAnexos.DataBind();
            }

        }
        catch (ValidationException ex)
        {
            lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar anexo.", UtilBO.TipoMensagem.Erro);
        }
        finally
        {
            txtDescricaoAnexo.Text = string.Empty;
        }
    }

    protected void grvAnexos_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView grv = ((GridView)sender);

        try
        {
            long arq_id = Convert.ToInt64(grv.DataKeys[e.RowIndex].Values["arq_id"]);

            ACA_AlunoAnexo entity = VS_listaAnexos.Any(p => p.arq_id == arq_id) ?
                VS_listaAnexos.Find(p => p.arq_id == arq_id) :
                new ACA_AlunoAnexo();

            if (entity.arq_id > 0)
            {
                VS_listaAnexos.Remove(entity);
            }

            if (VS_listaAnexos.Count <= 0)
                VS_listaAnexos.Add(new ACA_AlunoAnexo());

            grv.DataBind();
        }
        catch (ValidationException ex)
        {
            lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir anexo.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void grvAnexos_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView grv = ((GridView)sender);
        grv.EditIndex = e.NewEditIndex;
        grv.DataBind();

        HyperLink hplDownloadAnexo = (HyperLink)grv.Rows[e.NewEditIndex].FindControl("hplDownloadAnexo");
        if (hplDownloadAnexo != null)
            hplDownloadAnexo.Visible = false;

        ImageButton btnEditarAnexo = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("btnEditarAnexo");
        if (btnEditarAnexo != null)
            btnEditarAnexo.Visible = false;

        ImageButton btnAdicionarAnexo = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("btnAdicionarAnexo");
        if (btnAdicionarAnexo != null)
            btnAdicionarAnexo.Visible = false;

        ImageButton btnExcluirAnexo = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("btnExcluirAnexo");
        if (btnExcluirAnexo != null)
            btnExcluirAnexo.Visible = false;

        ImageButton btnSalvarAnexo = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("btnSalvarAnexo");
        if (btnSalvarAnexo != null)
            btnSalvarAnexo.Visible = true;

        ImageButton btnCancelarAnexo = (ImageButton)grv.Rows[e.NewEditIndex].FindControl("btnCancelarAnexo");
        if (btnCancelarAnexo != null)
            btnCancelarAnexo.Visible = true;

        grv.Rows[e.NewEditIndex].Focus();
    }

    protected void grvAnexos_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView grv = ((GridView)sender);

        try
        {
            long arq_id = Convert.ToInt64(grv.DataKeys[e.RowIndex].Values["arq_id"]);

            if (VS_listaAnexos.Any(p => p.arq_id == arq_id))
            {
                TextBox txtDescricaoAnexoEdicao = (TextBox)grv.Rows[e.RowIndex].FindControl("txtDescricaoAnexoEdicao");
                if (txtDescricaoAnexoEdicao != null)
                {
                    VS_listaAnexos.Where(p => p.arq_id == arq_id).ToList().ForEach(p => p.aan_descricao = txtDescricaoAnexoEdicao.Text);
                }
            }

            grv.EditIndex = -1;
            grv.DataBind();

            HyperLink hplDownloadAnexo = (HyperLink)grv.Rows[e.RowIndex].FindControl("hplDownloadAnexo");
            if (hplDownloadAnexo != null)
                hplDownloadAnexo.Visible = true;

            ImageButton btnEditarAnexo = (ImageButton)grv.Rows[e.RowIndex].FindControl("btnEditarAnexo");
            if (btnEditarAnexo != null)
                btnEditarAnexo.Visible = true;

            ImageButton btnAdicionarAnexo = (ImageButton)grv.Rows[e.RowIndex].FindControl("btnAdicionarAnexo");
            if (btnAdicionarAnexo != null)
                btnAdicionarAnexo.Visible = true;

            ImageButton btnExcluirAnexo = (ImageButton)grv.Rows[e.RowIndex].FindControl("btnExcluirAnexo");
            if (btnExcluirAnexo != null)
                btnExcluirAnexo.Visible = true;

            ImageButton btnSalvarAnexo = (ImageButton)grv.Rows[e.RowIndex].FindControl("btnSalvarAnexo");
            if (btnSalvarAnexo != null)
                btnSalvarAnexo.Visible = false;

            ImageButton btnCancelarAnexo = (ImageButton)grv.Rows[e.RowIndex].FindControl("btnCancelarAnexo");
            if (btnCancelarAnexo != null)
                btnCancelarAnexo.Visible = false;
        }
        catch (ValidationException ex)
        {
            lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar alterar anexo.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void grvAnexos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView grv = ((GridView)sender);
            grv.EditIndex = -1;
            grv.DataBind();

            HyperLink hplDownloadAnexo = (HyperLink)grv.Rows[e.RowIndex].FindControl("hplDownloadAnexo");
            if (hplDownloadAnexo != null)
                hplDownloadAnexo.Visible = true;

            ImageButton btnEditarAnexo = (ImageButton)grv.Rows[e.RowIndex].FindControl("btnEditarAnexo");
            if (btnEditarAnexo != null)
                btnEditarAnexo.Visible = true;

            ImageButton btnAdicionarAnexo = (ImageButton)grv.Rows[e.RowIndex].FindControl("btnAdicionarAnexo");
            if (btnAdicionarAnexo != null)
                btnAdicionarAnexo.Visible = true;

            ImageButton btnExcluirAnexo = (ImageButton)grv.Rows[e.RowIndex].FindControl("btnExcluirAnexo");
            if (btnExcluirAnexo != null)
                btnExcluirAnexo.Visible = true;

            ImageButton btnSalvarAnexo = (ImageButton)grv.Rows[e.RowIndex].FindControl("btnSalvarAnexo");
            if (btnSalvarAnexo != null)
                btnSalvarAnexo.Visible = false;

            ImageButton btnCancelarAnexo = (ImageButton)grv.Rows[e.RowIndex].FindControl("btnCancelarAnexo");
            if (btnCancelarAnexo != null)
                btnCancelarAnexo.Visible = false;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao cancelar operação.", UtilBO.TipoMensagem.Erro);
            updMessage.Update();
        }
    }

    #endregion

    #endregion Eventos

    #region Métodos

    /// <summary>
    /// Atualiza as informações da tela.
    /// </summary>
    private void AtualizaTela()
    {
        // Configura a busca de pessoas somente no user control de responsável que chamou
        if (!String.IsNullOrEmpty(VS_BuscaPessoa))
        {
            ((WebControls_AlunoResponsavel_UCAlunoResponsavel)
                updResponsavel.FindControl(VS_BuscaPessoa)).UCBuscaPessoasAluno = UCBuscaPessoasAluno1;
        }

        UCAlunoResponsavelMae._SetaBuscaPessoa += SetaBuscaPessoa;
        UCAlunoResponsavelPai._SetaBuscaPessoa += SetaBuscaPessoa;
        UCAlunoResponsavelOutro._SetaBuscaPessoa += SetaBuscaPessoa;
        UCBuscaPessoasAluno1.ReturnValues += UCBuscaPessoasAluno1_ReturnValues;

        // Cadastro de cidades
        UCCadastroCidade1.Inicialize(ApplicationWEB._Paginacao, "divCadastroCidade");
    }

    /// <summary>
    /// Inicializa os componentes.
    /// </summary>
    private void InicializaTela()
    {
        //Nome Social
        UCCadastroPessoa1.MostraNomeSocial = true;

        // Endereço
        UCEnderecos1.Inicializar(false, false, "Aluno", true, true, true);

        // Religião
        UCComboReligiao.CarregarReligiao();

        UCCadastroPessoa1.InicializarAlunoDeficiente();

        // Tipos de responsável
        UCComboTipoResponsavelAluno1.CarregarTipoResponsavelAluno();

        // Responsável - Mãe / Pai / Outro
        UCAlunoResponsavelMae.InicializarUserControl();
        UCAlunoResponsavelPai.InicializarUserControl();
        UCAlunoResponsavelOutro.InicializarUserControl();

        // Certidão cívil
        UCCertidaoCivil1.Inicializa("Aluno");
    }

    /// <summary>
    /// Carrega os dados do aluno.
    /// </summary>
    private void CarregaAluno()
    {
        UCMovimentacao1.MovimentacaoObrigatoria = VS_alu_id <= 0;

        if (VS_alu_id > 0)
        {
            ACA_Aluno entityAluno = new ACA_Aluno();
            PES_Pessoa entityPessoa = new PES_Pessoa();

            // Carrega entidade ACA_Aluno
            entityAluno.alu_id = VS_alu_id;
            ACA_AlunoBO.GetEntity(entityAluno);

            if (entityAluno.alu_situacao != (byte)ACA_AlunoSituacao.Excluido)
            {
                VS_dataAlteracaoAluno = entityAluno.alu_dataAlteracao;
                VS_alu_situacao = entityAluno.alu_situacao;

                // Estes métodos devem ser chamados primeiro, pois chamam o carregar assincronamente.
                CarregaMovimentacaoAsync(entityAluno);

                // Carrega entidade PES_Pessoa
                entityPessoa.pes_id = entityAluno.pes_id;
                PES_PessoaBO.GetEntity(entityPessoa);
                CarregaAbaDadosPessoais(entityAluno, entityPessoa);
                CarregaAbaEnderecoContato(entityPessoa);
                chkPossuiEmail.Checked = !entityAluno.alu_possuiEmail;
                CarregaAbaDocumentos(entityPessoa, entityAluno);
                CarregaAbaFichaMedica();

                // Verifica se possui endereço principal
                UCEnderecos1.VerificaEnderecoPrincipal();

                if (methodMov != null && resMov != null)
                {
                    methodMov.EndInvoke(resMov);
                }


                UCInfoComplementarAluno1.InformacaoComplementarAluno(VS_alu_id, AlunoUltimaMatricula);
                UpdFichaMedica.Update();

                chkPossuiInformacaoSigilosa.Checked = entityAluno.alu_possuiInformacaoSigilosa;
            }
            else
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Esta ação não pode ser realizada, pois o aluno foi excluído", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Academico/Aluno/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        else
        {
            __SessionWEB.PostMessages = UtilBO.GetErroMessage("Usuário não informado ou não encontrado.", UtilBO.TipoMensagem.Alerta);

            UCMovimentacao1.Visible = false;
            Response.Redirect("~/Academico/Aluno/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }

    /// <summary>
    /// Carrega os dados da aba Dados pessoais.
    /// </summary>
    /// <param name="entityAluno"></param>
    /// <param name="entityPessoa"></param>
    private void CarregaAbaDadosPessoais(ACA_Aluno entityAluno, PES_Pessoa entityPessoa)
    {
        // Configura dados pessoais do aluno
        UCCadastroPessoa1._VS_pes_id = entityPessoa.pes_id;
        UCCadastroPessoa1._VS_alu_id = entityAluno.alu_id;
        UCCadastroPessoa1._VS_pes_idFiliacaoPai = entityPessoa.pes_idFiliacaoPai;
        UCCadastroPessoa1._VS_pes_idFiliacaoMae = entityPessoa.pes_idFiliacaoMae;
        UCCadastroPessoa1._txtNome.Text = entityPessoa.pes_nome;
        UCCadastroPessoa1._txtNomeSocial.Text = entityPessoa.pes_nomeSocial;
        UCCadastroPessoa1._txtNomeAbreviado.Text = !string.IsNullOrEmpty(entityPessoa.pes_nome_abreviado) ? entityPessoa.pes_nome_abreviado : string.Empty;
        UCCadastroPessoa1._txtDataNasc.Text = (entityPessoa.pes_dataNascimento != new DateTime()) ? entityPessoa.pes_dataNascimento.ToString("dd/MM/yyyy") : string.Empty;
        UCCadastroPessoa1._ComboEstadoCivil.SelectedValue = entityPessoa.pes_estadoCivil > 0 ? entityPessoa.pes_estadoCivil.ToString() : "-1";
        UCCadastroPessoa1._ComboSexo.SelectedValue = entityPessoa.pes_sexo.ToString();
        UCCadastroPessoa1.ComboNacionalidade1_Valor = entityPessoa.pai_idNacionalidade;
        UCCadastroPessoa1._ComboRacaCor.SelectedValue = entityPessoa.pes_racaCor > 0 ? entityPessoa.pes_racaCor.ToString() : "-1";
        UCCadastroPessoa1._ComboEscolaridade.SelectedValue = entityPessoa.tes_id != Guid.Empty ? entityPessoa.tes_id.ToString() : Guid.Empty.ToString();
        UCCadastroPessoa1._chkNaturalizado.Checked = entityPessoa.pes_naturalizado;

        // Configura naturalidade da pessoa
        if (entityPessoa.cid_idNaturalidade != Guid.Empty)
        {
            END_Cidade entityCidade = new END_Cidade { cid_id = entityPessoa.cid_idNaturalidade };
            END_CidadeBO.GetEntity(entityCidade);

            UCCadastroPessoa1._VS_cid_id = entityPessoa.cid_idNaturalidade;
            UCCadastroPessoa1._txtNaturalidade.Text = entityCidade.cid_nome;
        }

        UCCadastroPessoa1.visibleFoto = false;

        // Configura dados do aluno
        txtObservacao.Text = entityAluno.alu_observacao;
        txtCodigoExterno.Text = entityAluno.alu_codigoExterno;
        chbAulaReligiao.Checked = entityAluno.alu_aulaReligiao;
        UCComboMeioTransporte.Value = entityAluno.alu_meioTransporte > 0 ? entityAluno.alu_meioTransporte : -1;
        ddlTempoDeslocamento.SelectedValue = entityAluno.alu_tempoDeslocamento > 0 ? entityAluno.alu_tempoDeslocamento.ToString() : "-1";
        chbRegressaSozinho.Checked = entityAluno.alu_regressaSozinho;
        chbDadosIncompletos.Checked = entityAluno.alu_dadosIncompletos;
        chbHistoricoEscolarIncompleto.Checked = entityAluno.alu_historicoEscolaIncompleto;
        UCComboReligiao.Valor = entityAluno.rlg_id > 0 ? entityAluno.rlg_id : -1;
        txtDataCadastroFisico.Text = entityAluno.alu_dataCadastroFisico != new DateTime() ? entityAluno.alu_dataCadastroFisico.ToString("dd/MM/yyyy") : string.Empty;
        txtResponsavelInfo.Text = entityAluno.alu_responsavelInfo;
        txtResponsavelInfoDoc.Text = entityAluno.alu_responsavelInfoDoc;
        txtResponsavelInfoOrgaoEmissor.Text = entityAluno.alu_responsavelInfoOrgaoEmissao;
        chbPossuiGemeo.Checked = entityAluno.alu_gemeo;

        // Carregar e configura tipo de deficiência cadastrada para a pessoa
        DataTable dtPessoaDeficiencia = PES_PessoaDeficienciaBO.GetSelect(entityPessoa.pes_id, false, 1, 1);
        if (dtPessoaDeficiencia.Rows.Count > 0)
        {
            UCCadastroPessoa1.ComboTipoDeficiencia1_Valor = new Guid(dtPessoaDeficiencia.Rows[0]["tde_id"].ToString());
            UCCadastroPessoa1.CarregaAlunoDeficiente(entityAluno.alu_id);
        }

        // Carrega responsáveis pelo aluno
        List<ACA_AlunoResponsavelBO.StructCadastro> ltResponsaveis = ACA_AlunoResponsavelBO.RetornaResponsaveisAluno(entityAluno.alu_id, null);
        foreach (ACA_AlunoResponsavelBO.StructCadastro item in ltResponsaveis)
        {
            if (item.entAlunoResp != null)
            {
                if (item.entAlunoResp.tra_id == TipoResponsavelAlunoParametro.tra_idMae(__SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    UCAlunoResponsavelMae.CarregarResponsavel(item);
                }
                else if (item.entAlunoResp.tra_id == TipoResponsavelAlunoParametro.tra_idPai(__SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    UCAlunoResponsavelPai.CarregarResponsavel(item);
                }
                else
                {
                    UCAlunoResponsavelOutro.CarregarResponsavel(item);
                }
            }
        }

        // Configura responsável principal
        ACA_AlunoResponsavelBO.StructCadastro principal = ltResponsaveis.Find(p => p.entAlunoResp.alr_principal);
        if (principal.entAlunoResp != null)
        {
            UCComboTipoResponsavelAluno1.Valor = principal.entAlunoResp.tra_id;
        }

        ConfiguraTipoResponsavel(false);
    }

    /// <summary>
    /// Carrega o dados da aba Endereço / Contato.
    /// </summary>
    /// <param name="entityPessoa"></param>
    /// /// <param name="carregaEmail"></param>
    private void CarregaAbaEnderecoContato(PES_Pessoa entityPessoa)
    {
        // Carrega dados dos endereços da pessoa.
        DataTable dtEndereco = PES_PessoaEnderecoBO.GetSelect(entityPessoa.pes_id, false, 1, 1);
        UCEnderecos1.CarregarEnderecosBanco(dtEndereco);

        // Carrega dados dos contatos da pessoa.
        UCContato1.CarregarContatosDoBanco(entityPessoa.pes_id);
    }

    /// <summary>
    /// Carrega os dados da aba Documentação.
    /// </summary>
    /// <param name="entityPessoa">Entidade PES_Pessoa.</param>
    private void CarregaAbaDocumentos(PES_Pessoa entityPessoa, ACA_Aluno entityAluno)
    {
        // Carrega dados dos documentos da pessoa.
        UCGridDocumento1._CarregarDocumento(entityPessoa.pes_id);

        // Carrega dados da certidões da pessoa.
        UCCertidaoCivil1.CarregaCertidaoCivil(entityPessoa.pes_id);

        VS_listaAnexos = ACA_AlunoAnexoBO.SelecionaAtivosPorALuno(entityAluno.alu_id);

        if (!VS_listaAnexos.Any())
        {
            VS_listaAnexos.Add(new ACA_AlunoAnexo());
        }

        grvAnexos.DataBind();
    }

    /// <summary>
    /// Carrega os dados da aba movimentação.
    /// </summary>
    /// <param name="entityAluno">Entidade ACA_Aluno.</param>
    private void CarregaAbaMovimentacao(ACA_Aluno entityAluno)
    {
        UCMovimentacao1.CarregarMatriculaAtual(VS_alu_id, entityAluno.alu_situacao);
    }

    /// <summary>
    /// Carrega os dados da aba Ficha Médica.
    /// </summary>
    private void CarregaAbaFichaMedica()
    {
        ACA_AlunoFichaMedica entityAlunoFichaMedica = new ACA_AlunoFichaMedica { alu_id = VS_alu_id };
        ACA_AlunoFichaMedicaBO.GetEntity(entityAlunoFichaMedica);

        txtTipoSanguineo.Text = entityAlunoFichaMedica.afm_tipoSanguineo;
        txtFatorRH.Text = entityAlunoFichaMedica.afm_fatorRH;
        txtDoencasConhecidas.Text = entityAlunoFichaMedica.afm_doencasConhecidas;
        txtAlergias.Text = entityAlunoFichaMedica.afm_alergias;
        txtMedicacoesPodeUtilizar.Text = entityAlunoFichaMedica.afm_medicacoesPodeUtilizar;
        txtMedicacoesUsoContinuo.Text = entityAlunoFichaMedica.afm_medicacoesUsoContinuo;
        txtConvenioMedico.Text = entityAlunoFichaMedica.afm_convenioMedico;
        txtHospitalRemocao.Text = entityAlunoFichaMedica.afm_hospitalRemocao;
        txtOutrasRecomendacoes.Text = entityAlunoFichaMedica.afm_outrasRecomendacoes;

        // Carrega dados dos contatos do aluno.
        DataTable dtContatoFichaMedica = ACA_FichaMedicaContatoBO.GetSelect_By_Aluno(entityAlunoFichaMedica.alu_id, false, 1, 1);
        UCGridContatoNomeTelefone1._VS_seq = dtContatoFichaMedica.Rows.Count > 0 ? dtContatoFichaMedica.Rows.Count : -1;
        if (dtContatoFichaMedica.Rows.Count == 0)
        {
            dtContatoFichaMedica = null;
        }

        UCGridContatoNomeTelefone1._VS_contatos = dtContatoFichaMedica;
        UCGridContatoNomeTelefone1._CarregarContato();
    }

    /// <summary>
    /// Carrega os dados da aba Movimentacao assincronamente.
    /// </summary>
    private void CarregaMovimentacaoAsync(ACA_Aluno entityAluno)
    {
        methodMov = CarregaAbaMovimentacao;
        resMov = methodMov.BeginInvoke(entityAluno, null, null);
    }

    /// <summary>
    /// Trata as execeções do tipo ValidationException e as que herdam dela.
    /// Verifica qual o tipo da execeção, e aponta para a aba correta na tela.
    /// </summary>
    /// <param name="ex">Execeção a ser verificada</param>
    private void TrataValidationException(ValidationException ex)
    {
        string aba = txtSelectedTab.Value;
        if (ex is ACA_Aluno_ValidationException)
        {
            aba = INDEX_ABA_ALUNO;
        }
        else if ((ex is ACA_AlunoCurriculo_ValidationException) ||
            (ex is MTR_Movimentacao_ValidationException))
        {
            aba = INDEX_ABA_MOVIMENTACAO;
        }
        else if (ex is WebControls_Endereco_UCEnderecos.EnderecoException)
        {
            aba = INDEX_ABA_ENDERECOCONTATO;
        }
        else if ((ex is EditarAluno_ValidationException) ||
            (ex is RealizarMovimentacao_ValidationException))
        {
            aba = INDEX_ABA_ALUNO;

            __SessionWEB.PostMessages = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);

            Response.Redirect("~/Academico/Aluno/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        //else
        //{
        //    aba = INDEX_ABA_ALUNO;
        //}

        string msgErro = UtilBO.GetErroMessage(ex.Message +
            ((UCCadastroPessoa1._iptFoto.PostedFile != null) ?
            "<br />" + GestaoEscolarUtilBO.VerificarFoto(UCCadastroPessoa1._iptFoto.PostedFile.FileName) : string.Empty)
            , UtilBO.TipoMensagem.Alerta);
        lblMessage.Text = msgErro;

        // UCEnderecos1.AtualizaEnderecos();
        txtSelectedTab.Value = aba;
    }

    /// <summary>
    /// Verifica e configura a ação de acordo com o parâmetro PermanecerTela.
    /// </summary>
    private void VerificarPermanecerTela()
    {
        if (ParametroPermanecerTela)
        {
            Session["aluno"] = VS_alu_id;
            Session["permissao"] = VS_permissao;
            Response.Redirect("~/Academico/Aluno/Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        else
        {
            Response.Redirect("~/Academico/Aluno/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }

    #region Dados pessoais

    /// <summary>
    /// Configura user controls de responsáveis de acordo com o tipo.
    /// </summary>
    /// <param name="limparOutro">Flag que indica se é para limpar os campos do responsável
    /// Outro</param>
    private void ConfiguraTipoResponsavel(bool limparOutro)
    {
        int tra_id = UCComboTipoResponsavelAluno1.Valor;

        if (tra_id > 0)
        {
            bool mae = tra_id == TipoResponsavelAlunoParametro.tra_idMae(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            bool pai = tra_id == TipoResponsavelAlunoParametro.tra_idPai(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            bool proprio = tra_id == TipoResponsavelAlunoParametro.tra_idProprio(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            bool outro = (!mae) && (!pai) && (!proprio);

            // Mostra o cadastro de "outro" responsável.
            UCAlunoResponsavelOutro.Visible = outro;
            if (limparOutro)
            {
                UCAlunoResponsavelOutro.LimparCampos();
            }

            // Configura obrigatoriedade no tipo de responsável.
            UCAlunoResponsavelMae.Obrigatorio = mae;
            UCAlunoResponsavelPai.Obrigatorio = pai;
            UCAlunoResponsavelOutro.Obrigatorio = outro;
        }
        else
        {
            UCAlunoResponsavelOutro.Visible = false;
            UCAlunoResponsavelMae.Obrigatorio = false;
            UCAlunoResponsavelPai.Obrigatorio = false;
            UCAlunoResponsavelOutro.Obrigatorio = false;
        }

        UCComboTipoResponsavelAluno1.SetarFoco();
    }

    /// <summary>
    /// Retorna a list com os responsáveis cadastrados para o aluno.
    /// </summary>
    /// <returns></returns>
    private List<ACA_AlunoResponsavelBO.StructCadastro> RetornaResponsaveis()
    {
        List<ACA_AlunoResponsavelBO.StructCadastro> lista = new List<ACA_AlunoResponsavelBO.StructCadastro>();
        ACA_AlunoResponsavelBO.StructCadastro responsavel;

        if (UCAlunoResponsavelMae.RetornaStructCadastro(UCComboTipoResponsavelAluno1.Valor, out responsavel))
        {
            lista.Add(responsavel);
        }

        if (UCAlunoResponsavelPai.RetornaStructCadastro(UCComboTipoResponsavelAluno1.Valor, out responsavel))
        {
            lista.Add(responsavel);
        }

        if (UCAlunoResponsavelOutro.RetornaStructCadastro(UCComboTipoResponsavelAluno1.Valor, out responsavel))
        {
            lista.Add(responsavel);
        }

        return lista;
    }

    #endregion Dados pessoais


    /// <summary>
    /// Salva os dados do aluno.
    /// </summary>
    /// <param name="verificarAlunoExistente">Indica se será verificado duplicidade de aluno</param>
    /// <param name="verificarAlunoIntegridadeFonetica">Indica se será verificado duplicidade fonética de aluno</param>
    /// <param name="verificarQuantidadeAlunosTurma">Indica se vai verificar a quantidade de alunos na turma ou não</param>
    /// <param name="verificarParametroNis"></param>
    /// <param name="verificarDataAcaoRetroativa">"Indica se vai enviar mensagem para o usuário confirmando a movimentação retroativa"</param>
    /// <param name="verificarConfirmacacao">Indica se mostra mensagem de confirmação referente a movimentação</param>
    private void SalvaAluno()
    { 
        try
        {
            ACA_Aluno entityAluno = new ACA_Aluno { alu_id = VS_alu_id };
            ACA_AlunoBO.GetEntity(entityAluno);
            

            // Configura entidade ACA_Aluno
            if (entityAluno.alu_situacao != (byte)ACA_AlunoSituacao.Excluido)
            {
                // Configura ACA_AlunoFichaMedica
                ACA_AlunoFichaMedica entityAlunoFichaMedica = new ACA_AlunoFichaMedica
                {
                    alu_id = entityAluno.alu_id,
                    afm_tipoSanguineo = txtTipoSanguineo.Text,
                    afm_fatorRH = txtFatorRH.Text,
                    afm_doencasConhecidas = txtDoencasConhecidas.Text,
                    afm_alergias = txtAlergias.Text,
                    afm_medicacoesPodeUtilizar = txtMedicacoesPodeUtilizar.Text,
                    afm_medicacoesUsoContinuo = txtMedicacoesUsoContinuo.Text,
                    afm_convenioMedico = txtConvenioMedico.Text,
                    afm_hospitalRemocao = txtHospitalRemocao.Text,
                    afm_outrasRecomendacoes = txtOutrasRecomendacoes.Text,
                    IsNew = !ACA_AlunoFichaMedicaBO.VerificaFichaMedicaExistente(entityAluno.alu_id)
                };

                string msg = "";
                UCGridContatoNomeTelefone1.SalvaConteudoGrid(out msg);

                if (ACA_AlunoBO.Salvar(
                        entityAluno,
                        entityAlunoFichaMedica,
                        UCGridContatoNomeTelefone1._VS_contatos
                        ))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "alu_id: " + entityAluno.alu_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Aluno alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    VS_alu_id = entityAluno.alu_id;

                    VerificarPermanecerTela();
                }
            }
            else
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Esta ação não pode ser realizada, pois o aluno foi excluído", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Academico/Aluno/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        catch (ValidationException ex)
        {
            TrataValidationException(ex);
        }
        catch (ArgumentException ex)
        {
            lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (DuplicateNameException ex)
        {
            lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (ParametroNisException ex)
        {
            lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {

            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o aluno.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion Métodos
}
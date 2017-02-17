using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using System.IO;
using MSTech.CoreSSO.Entities;
using MSTech.CoreSSO.BLL;
using System.Threading;
using System.Collections.Generic;
using System.Web;
using MSTech.CoreSSO.WebServices.Consumer;
using MSTech.Validation.Exceptions;

public partial class Academico_Aluno_CadastroAntigo : MotherPageLogado
{
    #region Constantes

    private const string indiceAbaAluno = "0";
    private const string indiceAbaEnderecoContato = "1";
    private const string indiceAbaDocumentacao = "2";
    private const string indiceAbaMovimentacao = "3";
    private const string indiceAbaHistorico = "4";
    private const string indiceAbaFichaMedica = "5";
    private const string indiceAbaUsuarios = "6";

    private const int indiceColunaEscolaAnterior = 3;
    private const int indiceColunaEscolaAtual = 4;

    #endregion

    #region Eventos Page Life Cycle

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cvMatriculaDataPrimeira.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data da primeira matrícula");
            cvMatriculaDataSaida.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de saída");
            cvMatriculaDataColacao.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de publicação no Diário Oficial");
            CustomValidator1.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data da primeira matrícula");
            CustomValidator2.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de saída");
            CustomValidator3.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de publicação no Diário Oficial");
            _revAnoLetivo.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoAno("Ano");
            
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
            sm.Scripts.Add(new ScriptReference(ArquivoJS.Tabs));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.ExitPageConfirm)); ;
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

            if (Convert.ToBoolean(ACA_ParametroAcademicoBO.ParametroValor(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA)))
            {
                if (!Convert.ToString(_btnCancelar.CssClass).Contains("btnMensagemUnload"))
                {
                    _btnCancelar.CssClass += " btnMensagemUnload";
                }

                if (!Convert.ToString(_btnNovo.CssClass).Contains("btnMensagemUnload"))
                {
                    _btnNovo.CssClass += " btnMensagemUnload";
                }
            }
        }

        SetaDelegates();

        UCCadastroPessoa1._labelNome.Text = "Nome do aluno *";
        UCCadastroPessoa1.rfvNome.ErrorMessage = "Nome do aluno é obrigatório.";

        if (!Convert.ToString(_btnIncluirHistorico.CssClass).Contains("subir"))
        {
            _btnIncluirHistorico.CssClass += " subir";
        }

        if (!IsPostBack)
        {
            _btnNovo.Visible = false;
            string _paramValor = ACA_ParametroAcademicoBO.ParametroValor(eChaveAcademico.MATRICULA_ESTADUAL);
            _lblMatrEst.Text = _paramValor;
            _txtMatriculaEstadual.Visible = _lblMatrEst.Visible = !string.IsNullOrEmpty(_paramValor);

            Page.Form.Enctype = "multipart/form-data";

            UCGridDocumento1._isAluno = true;
            _grvEscolaOrigem.PageSize = ApplicationWEB._Paginacao;
            fdsResultadosEscolaOrigem.Visible = false;

            try
            {
                if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                {
                    UCFiltroEscolas1.SelecionaCombosAutomatico = false;
                    _VS_alu_id = PreviousPage.EditItem;
                }
                else if (Session["aluno"] != null)
                {
                    Int64 alu_id;
                    Int64.TryParse(Session["aluno"].ToString(), out alu_id);
                    Session.Remove("aluno");
                    UCFiltroEscolas1.SelecionaCombosAutomatico = false;
                    _VS_alu_id = alu_id;
                }
                else
                {
                    if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir 
                        && !__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar)
                    {
                        Response.Redirect("Busca.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }

                    CarregarCidadeUsuarioLogado();
                    _btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
                }

                // Seta propriedades padrão dos user controls.
                InicializaUserControls();

                // Carrega dados na tela.
                LoadDadosTela();

                if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar)
                {
                    HabilitaControles(fdsDadosPessoais.Controls, false);
                    HabilitaControles(fdsEndereco.Controls, false);
                    HabilitaControles(fdsContato.Controls, false);
                    HabilitaControles(fdsDocumento.Controls, false);
                    HabilitaControles(fdsCertidoes.Controls, false);
                    HabilitaControles(fdsMovimentacao.Controls, false);
                    HabilitaControles(fdsHistorico.Controls, false);
                    HabilitaControles(fdsFichaMedica.Controls, false);
                    HabilitaControles(fdsCasoEmergencia.Controls, false);
                    HabilitaControles(fdsCriarUsuario.Controls, false);
                    HabilitaControles(fdsUsuario.Controls, false);

                    _btnSalvar.Visible = false;
                    _btnCancelar.Text = "Voltar";
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }

            Page.Form.DefaultFocus = UCCadastroPessoa1._txtNome.ClientID;
            Page.Form.DefaultButton = _btnSalvar.UniqueID;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (ckbAltCurso.Checked || ckbAltTurma.Checked || ckbRecAluno.Checked || ckbTranfDentroRede.Checked || ckbTranfForaRede.Checked)
        {
            _btnSalvar.OnClientClick = String.Format("SetConfirmDialogButton('{0}','{1}');", String.Concat("#", _btnSalvar.ClientID), String.Format("Confirma a  movimentação do aluno?"));
        }
        else
        {
            _btnSalvar.OnClientClick = "";
        }


        //// Seta confirmação no botão de salvar.
        //if (ckbAltCurso.Checked || ckbAltTurma.Checked || ckbRecAluno.Checked || ckbTranfDentroRede.Checked || ckbTranfForaRede.Checked)
        //{
        //    string script = String.Format("SetConfirmDialogButton('{0}','{1}');", String.Concat("#", _btnSalvar.ClientID), String.Format("Confirma a  movimentação do aluno?"));
        //    //Page.ClientScript.RegisterStartupScript(GetType(), _btnSalvar.ClientID, script, true);

        //    //if (!Page.ClientScript.IsClientScriptBlockRegistered("ConfirmMovimentacao"))
        //    //    Page.ClientScript.RegisterClientScriptBlock(GetType(), "ConfirmMovimentacao", script, true);

        //    if (!Page.ClientScript.IsStartupScriptRegistered(_btnSalvar.ClientID))
        //        Page.ClientScript.RegisterStartupScript(GetType(), _btnSalvar.ClientID, script, true);
        //}
        //else
        //    Page.ClientScript.RegisterStartupScript(GetType(), _btnSalvar.ClientID, "", true);
    }

    #endregion

    #region Propriedades

    /// <summary>
    /// Propriedade que retorna se os está configurado para filtrar por UA.
    /// </summary>
    private bool _VS_FiltroEscola
    {
        get
        {
            if (ViewState["_VS_FiltroEscola"] != null)
            {
                if (ViewState["_VS_FiltroEscola"].Equals(true))
                    return true;

                return false;
            }

            return false;
        }
        set
        {
            ViewState["_VS_FiltroEscola"] = value;
        }
    }

    /// <summary>
    /// Guarda o ID do aluno
    /// </summary>
    private long _VS_alu_id
    {
        get { return Convert.ToInt64(ViewState["_VS_alu_id"]); }
        set { ViewState["_VS_alu_id"] = value; }
    }

    /// <summary>
    /// Guarda o ID do curriculo do aluno
    /// </summary>
    private int _VS_alc_id
    {
        get { return Convert.ToInt32(ViewState["_VS_alc_id"]); }
        set { ViewState["_VS_alc_id"] = value; }
    }

    /// <summary>
    /// Guarda o ID da unidade administrativa superior
    /// </summary>
    private Guid _VS_uad_idSuperior
    {
        get
        {
            if (ViewState["_VS_uad_idSuperior"] != null)
                return new Guid(ViewState["_VS_uad_idSuperior"].ToString());

            return Guid.Empty;
        }
        set { ViewState["_VS_uad_idSuperior"] = value; }
    }

    /// <summary>
    /// Guarda o ID do tipo de unidade administrativa superior para filtrar escolas
    /// </summary>
    private Guid _VS_tua_id
    {
        get { return new Guid(ViewState["_VS_tua_id"].ToString()); }
        set { ViewState["_VS_tua_id"] = value; }
    }

    /// <summary>
    /// Guarda o ID da escola
    /// </summary>
    private int _VS_esc_id
    {
        get { return Convert.ToInt32(ViewState["_VS_esc_id"]); }
        set { ViewState["_VS_esc_id"] = value; }
    }

    /// <summary>
    /// Guarda o ID da unidade da escola
    /// </summary>
    private int _VS_uni_id
    {
        get { return Convert.ToInt32(ViewState["_VS_uni_id"]); }
        set { ViewState["_VS_uni_id"] = value; }
    }

    /// <summary>
    /// Guarda o ID do curso
    /// </summary>
    private int _VS_cur_id
    {
        get { return Convert.ToInt32(ViewState["_VS_cur_id"]); }
        set { ViewState["_VS_cur_id"] = value; }
    }

    /// <summary>
    /// Guarda o ID do curriculo
    /// </summary>
    private int _VS_crr_id
    {
        get { return Convert.ToInt32(ViewState["_VS_crr_id"]); }
        set { ViewState["_VS_crr_id"] = value; }
    }

    /// <summary>
    /// Guarda o ID do período
    /// </summary>
    private int _VS_crp_id
    {
        get { return Convert.ToInt32(ViewState["_VS_crp_id"]); }
        set { ViewState["_VS_crp_id"] = value; }
    }

    /// <summary>
    /// Guarda o ID da turma
    /// </summary>
    private long _VS_tur_id
    {
        get { return Convert.ToInt64(ViewState["_VS_tur_id"]); }
        set { ViewState["_VS_tur_id"] = value; }
    }

    /// <summary>
    /// Guarda o ID do tipo de turno
    /// </summary>
    private int _VS_ttn_id
    {
        get { return Convert.ToInt32(ViewState["_VS_ttn_id"]); }
        set { ViewState["_VS_ttn_id"] = value; }
    }

    /// <summary>
    /// Guarda o ID da matricula
    /// </summary>
    private int _VS_mtu_id
    {
        get { return Convert.ToInt32(ViewState["_VS_mtu_id"]); }
        set { ViewState["_VS_mtu_id"] = value; }
    }

    /// <summary>
    /// Guarda o ID da escola
    /// </summary>
    private int _VS_esc_id_Anterior
    {
        get { return Convert.ToInt32(ViewState["_VS_esc_id_Anterior"]); }
        set { ViewState["_VS_esc_id_Anterior"] = value; }
    }

    /// <summary>
    /// Guarda o ID da unidade da escola
    /// </summary>
    private int _VS_uni_id_Anterior
    {
        get { return Convert.ToInt32(ViewState["_VS_uni_id_Anterior"]); }
        set { ViewState["_VS_uni_id_Anterior"] = value; }
    }

    /// <summary>
    /// Guarda o ID do curso
    /// </summary>
    private int _VS_cur_id_Anterior
    {
        get { return Convert.ToInt32(ViewState["_VS_cur_id_Anterior"]); }
        set { ViewState["_VS_cur_id_Anterior"] = value; }
    }

    /// <summary>
    /// Guarda o ID do curriculo
    /// </summary>
    private int _VS_crr_id_Anterior
    {
        get { return Convert.ToInt32(ViewState["_VS_crr_id_Anterior"]); }
        set { ViewState["_VS_crr_id_Anterior"] = value; }
    }

    /// <summary>
    /// Guarda o ID do período
    /// </summary>
    private int _VS_crp_id_Anterior
    {
        get { return Convert.ToInt32(ViewState["_VS_crp_id_Anterior"]); }
        set { ViewState["_VS_crp_id_Anterior"] = value; }
    }

    /// <summary>
    /// Guarda o ID da turma
    /// </summary>
    private long _VS_tur_id_Anterior
    {
        get { return Convert.ToInt64(ViewState["_VS_tur_id_Anterior"]); }
        set { ViewState["_VS_tur_id_Anterior"] = value; }
    }

    /// <summary>
    /// Guarda o ID do tipo de turno
    /// </summary>
    private int _VS_ttn_id_Anterior
    {
        get { return Convert.ToInt32(ViewState["_VS_ttn_id_Anterior"]); }
        set { ViewState["_VS_ttn_id_Anterior"] = value; }
    }

    /// <summary>
    /// Guarda o ID da matricula
    /// </summary>
    private int _VS_mtu_id_Anterior
    {
        get { return Convert.ToInt32(ViewState["_VS_mtu_id_Anterior"]); }
        set { ViewState["_VS_mtu_id_Anterior"] = value; }
    }

    private Guid _VS_pes_id
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
    /// Ent_Id do usuário logado, recupera da sessão.
    /// </summary>
    private Guid Ent_id
    {
        get
        {
            return __SessionWEB.__UsuarioWEB.Usuario.ent_id;
        }
    }

    private byte[] _VS_pes_foto
    {
        get
        {
            if (ViewState["_VS_pes_foto"] != null)
                return (byte[])ViewState["_VS_pes_foto"];
            return null;
        }
        set
        {
            ViewState["_VS_pes_foto"] = value;
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

    private Guid _VS_pes_idPaiAntigo
    {
        get
        {
            if (ViewState["_VS_pes_idPaiAntigo"] != null)
                return new Guid(ViewState["_VS_pes_idPaiAntigo"].ToString());
            return Guid.Empty;
        }
        set
        {
            ViewState["_VS_pes_idPaiAntigo"] = value;
        }
    }

    private Guid _VS_pes_idMaeAntigo
    {
        get
        {
            if (ViewState["_VS_pes_idMaeAntigo"] != null)
                return new Guid(ViewState["_VS_pes_idMaeAntigo"].ToString());
            return Guid.Empty;
        }
        set
        {
            ViewState["_VS_pes_idMaeAntigo"] = value;
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
                return new Guid(ViewState["_VS_usu_id"].ToString());
            return Guid.Empty;
        }
        set
        {
            ViewState["_VS_usu_id"] = value;
        }
    }

    /// <summary>
    /// Entidade AlunoCurriculo
    /// </summary>
    private ACA_AlunoCurriculo entityAlunoCurriculoAntigo
    {
        get;
        set;
    }

    /// <summary>
    /// Entidade AlunoCurriculo
    /// </summary>
    private ACA_AlunoCurriculo entityAlunoCurriculoNovo
    {
        get;
        set;
    }

    /// <summary>
    /// Entidade AlunoCurriculo
    /// </summary>
    private MTR_MatriculaTurma entityMatriculaTurmaAntigo
    {
        get;
        set;
    }

    /// <summary>
    /// Entidade AlunoCurriculo
    /// </summary>
    private MTR_MatriculaTurma entityMatriculaTurmaNovo
    {
        get;
        set;
    }

    /// <summary>
    /// Entidade AlunoCurriculo
    /// </summary>
    private MTR_Movimentacao entityMovimentacao
    {
        get;
        set;
    }

    /// <summary>
    /// Armazena o sequencial para inclusão no DataTable Matriculas
    /// </summary>
    public int _VS_seqMatricula
    {
        get
        {
            if (ViewState["_VS_seqMatricula"] != null)
                return Convert.ToInt32(ViewState["_VS_seqMatricula"]);
            return -1;
        }
        set
        {
            ViewState["_VS_seqMatricula"] = value;
        }
    }

    /// <summary>
    /// Indica se é uma alteração ou inclusão de matricula
    /// </summary>
    public bool _VS_IsNewMatricula
    {
        get
        {
            return Convert.ToBoolean(ViewState["_VS_IsNewMatricula"]);
        }
        set
        {
            ViewState["_VS_IsNewMatricula"] = value;
        }
    }

    /// <summary>
    /// Armazena o id do grid de matrícula
    /// </summary>
    public int _VS_idMatricula
    {
        get
        {
            if (ViewState["_VS_idMatricula"] != null)
                return Convert.ToInt32(ViewState["_VS_idMatricula"]);
            return -1;
        }
        set
        {
            ViewState["_VS_idMatricula"] = value;
        }
    }

    /// <summary>
    /// ViewState com datatable de Histórico
    /// Retorno e atribui valores para o DataTable de Histórico
    /// </summary>
    public DataTable _VS_historico
    {
        get
        {
            if (ViewState["_VS_historico"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("alh_id");
                dt.Columns.Add("tne_id");
                dt.Columns.Add("tme_id");
                dt.Columns.Add("eco_id");
                dt.Columns.Add("mtu_id");
                dt.Columns.Add("alh_serie");
                dt.Columns.Add("alh_anoLetivo");
                dt.Columns.Add("alh_resultado");
                dt.Columns.Add("alh_avaliacao");
                dt.Columns.Add("alh_frequencia");
                dt.Columns.Add("tne_nome");
                dt.Columns.Add("tme_nome");
                dt.Columns.Add("eco_nome");
                dt.Columns.Add("alh_resultadoDescricao");
                dt.Columns.Add("tre_id");
                dt.Columns.Add("eco_codigoInep");
                dt.Columns.Add("end_id");
                dt.Columns.Add("eco_numero");
                dt.Columns.Add("eco_complemento");
                dt.Columns.Add("end_cep");
                dt.Columns.Add("end_logradouro");
                dt.Columns.Add("end_zona");
                dt.Columns.Add("end_distrito");
                dt.Columns.Add("end_bairro");
                dt.Columns.Add("cid_id");
                dt.Columns.Add("cid_nome");

                ViewState["_VS_historico"] = dt;
            }
            return (DataTable)ViewState["_VS_historico"];
        }
        set
        {
            ViewState["_VS_historico"] = value;
        }
    }

    /// <summary>
    /// Armazena o sequencial para inclusão no DataTable Historico
    /// </summary>
    public int _VS_seqHistorico
    {
        get
        {
            if (ViewState["_VS_seqHistorico"] != null)
                return Convert.ToInt32(ViewState["_VS_seqHistorico"]);
            return 0;
        }
        set
        {
            ViewState["_VS_seqHistorico"] = value;
        }
    }

    /// <summary>
    /// Indica se é uma alteração ou inclusão de Historico
    /// </summary>
    public bool _VS_IsNewHistorico
    {
        get
        {
            return Convert.ToBoolean(ViewState["_VS_IsNewHistorico"]);
        }
        set
        {
            ViewState["_VS_IsNewHistorico"] = value;
        }
    }

    /// <summary>
    /// Armazena o id do grid de historico
    /// </summary>
    public int _VS_idHistorico
    {
        get
        {
            if (ViewState["_VS_idHistorico"] != null)
                return Convert.ToInt32(ViewState["_VS_idHistorico"]);
            return -1;
        }
        set
        {
            ViewState["_VS_idHistorico"] = value;
        }
    }

    /// <summary>
    /// ViewState com datatable de Histórico Obsercação
    /// Retorno e atribui valores para o DataTable de Histórico Observação
    /// </summary>
    public DataTable _VS_historicoObservacao
    {
        get
        {
            if (ViewState["_VS_historicoObservacao"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("alu_id");
                dt.Columns.Add("alh_id");
                dt.Columns.Add("aho_id");
                dt.Columns.Add("aho_descricao");
                dt.Columns.Add("hop_id");
                dt.Columns.Add("aho_situacao");

                ViewState["_VS_historicoObservacao"] = dt;
            }
            return (DataTable)ViewState["_VS_historicoObservacao"];
        }
        set
        {
            ViewState["_VS_historicoObservacao"] = value;
        }
    }

    /// <summary>
    /// Armazena o sequencial para inclusão no DataTable Historico Observação
    /// </summary>
    public int _VS_seqHistoricoObservacao
    {
        get
        {
            if (ViewState["_VS_seqHistoricoObservacao"] != null)
                return Convert.ToInt32(ViewState["_VS_seqHistoricoObservacao"]);
            return 0;
        }
        set
        {
            ViewState["_VS_seqHistoricoObservacao"] = value;
        }
    }

    /// <summary>
    /// Indica se é uma alteração ou inclusão de Historico Observacao
    /// </summary>
    public bool _VS_IsNewHistoricoObservacao
    {
        get
        {
            return Convert.ToBoolean(ViewState["_VS_IsNewHistoricoObservacao"]);
        }
        set
        {
            ViewState["_VS_IsNewHistoricoObservacao"] = value;
        }
    }

    /// <summary>
    /// Armazena o id do grid de historico observação
    /// </summary>
    public int _VS_idHistoricoObservacao
    {
        get
        {
            if (ViewState["_VS_idHistoricoObservacao"] != null)
                return Convert.ToInt32(ViewState["_VS_idHistoricoObservacao"]);
            return -1;
        }
        set
        {
            ViewState["_VS_idHistoricoObservacao"] = value;
        }
    }

    /// <summary>
    /// ViewState com datatable de Disciplinas do Histórico
    /// Retorno e atribui valores para o DataTable de Disciplinas do Histórico
    /// </summary>
    public DataTable _VS_disciplina
    {
        get
        {
            if (ViewState["_VS_disciplina"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("alh_id");
                dt.Columns.Add("ahd_id");
                dt.Columns.Add("tds_id");
                dt.Columns.Add("ahd_disciplina");
                dt.Columns.Add("ahd_resultado");
                dt.Columns.Add("ahd_avaliacao");
                dt.Columns.Add("ahd_frequencia");
                dt.Columns.Add("tds_nome");
                dt.Columns.Add("ahd_resultadoDescricao");

                ViewState["_VS_disciplina"] = dt;
            }
            return (DataTable)ViewState["_VS_disciplina"];
        }
        set
        {
            ViewState["_VS_disciplina"] = value;
        }
    }

    /// <summary>
    /// Armazena o sequencial para inclusão no DataTable Disciplinas do Histórico
    /// </summary>
    public int _VS_seqDisciplina
    {
        get
        {
            if (ViewState["_VS_seqDisciplina"] != null)
                return Convert.ToInt32(ViewState["_VS_seqDisciplina"]);
            return -1;
        }
        set
        {
            ViewState["_VS_seqDisciplina"] = value;
        }
    }

    /// <summary>
    /// Indica se é uma alteração ou inclusão de Disciplinas do Histórico
    /// </summary>
    public bool _VS_IsNewDisciplina
    {
        get
        {
            return Convert.ToBoolean(ViewState["_VS_IsNewDisciplina"]);
        }
        set
        {
            ViewState["_VS_IsNewDisciplina"] = value;
        }
    }

    /// <summary>
    /// Armazena o id do grid de Disciplinas do Histórico
    /// </summary>
    public int _VS_idDisciplina
    {
        get
        {
            if (ViewState["_VS_idDisciplina"] != null)
                return Convert.ToInt32(ViewState["_VS_idDisciplina"]);
            return -1;
        }
        set
        {
            ViewState["_VS_idDisciplina"] = value;
        }
    }

    private long _VS_eco_id
    {
        get
        {
            if (ViewState["_VS_eco_id"] != null)
                return Convert.ToInt64(ViewState["_VS_eco_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_eco_id"] = value;
        }
    }

    /// <summary>
    /// Indica se é uma alteração ou inclusão de endereço
    /// </summary>
    public bool _VS_IsNew_end_id
    {
        get
        {
            return Convert.ToBoolean(ViewState["_VS_IsNew_end_id"]);
        }
        set
        {
            ViewState["_VS_IsNew_end_id"] = value;
        }
    }

    /// <summary>
    /// Propriedade usada para consultar as unidades administrativas que o usuário
    /// tem permissão.
    /// </summary>
    private List<Guid> uad_ids;
    private List<Guid> Uad_Ids_PermissaoUsuario
    {
        get
        {
            if (uad_ids == null)
                uad_ids = UAsVisaoGrupoList();

            return uad_ids;
        }
    }

    /// <summary>
    /// Guarda em ViewState o ID do userControl de responsável que chamou o evento
    /// de abrir a busca de pessoas, para setar o retorno.
    /// </summary>
    private string VS_TipoResponsavelBuscaPessoa
    {
        get
        {
            if (ViewState["VS_TipoResponsavelBuscaPessoa"] != null)
                return Convert.ToString(ViewState["VS_TipoResponsavelBuscaPessoa"]);

            return "";
        }
        set
        {
            ViewState["VS_TipoResponsavelBuscaPessoa"] = value;
        }
    }

    private bool CancelaSelect = true;

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

    #endregion

    #region Métodos

    /// <summary>
    /// Seta a cidade pelo endereço da entidade do usuário logado.
    /// </summary>
    private void CarregarCidadeUsuarioLogado()
    {
        //Recebe o parametro que verifica se a cidade deve ser automaticamente preenchida
        string preencheCidade = ACA_ParametroAcademicoBO.ParametroValor(eChaveAcademico.PAR_PREENCHER_CIDADE);

        if (!string.IsNullOrEmpty(preencheCidade) && Convert.ToBoolean(preencheCidade))
        {

            // Setar a cidade pelo endereço da Entidade do usuário logado.
            Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

            Guid ene_id = SYS_EntidadeEnderecoBO.Select_ene_idBy_ent_id(ent_id);

            SYS_EntidadeEndereco entEndereco = new SYS_EntidadeEndereco
            {
                ent_id = ent_id
                ,
                ene_id = ene_id
            };
            SYS_EntidadeEnderecoBO.GetEntity(entEndereco);

            // Recuperando entidade Endereço do usuário logado.
            END_Endereco endereco = new END_Endereco
            {
                end_id = entEndereco.end_id
            };
            END_EnderecoBO.GetEntity(endereco);

            // Recuperando a cidade.
            END_Cidade cidade = new END_Cidade
            {
                cid_id = endereco.cid_id
            };
            END_CidadeBO.GetEntity(cidade);
        }
    }

    /// <summary>
    /// Carrega escolas próximas ao usuário de acordo com o CEP 
    /// </summary>
    private void CarregaEscolasProximas()
    {
        try
        {
            _lstEscProx.Items.Clear();

            // percorre a lista de escolas do combo
            foreach (ListItem esc in UCFiltroEscolas1._ComboUnidadeEscola.Items)
            {
                if (!(esc.Value == "-1;-1"))
                {
                    ESC_UnidadeEscola escola = new ESC_UnidadeEscola
                    {
                        esc_id = Convert.ToInt32(esc.Value.Split(';')[0]),
                        uni_id = Convert.ToInt32(esc.Value.Split(';')[1])
                    };

                    // carrega escola
                    ESC_UnidadeEscolaBO.GetEntity(escola);

                    if (escola.uni_cepsProximos != null)
                    {
                        // vetor com os ceps das proximidades da escola
                        string[] cepProxEscSpplited = escola.uni_cepsProximos.Split(',');

                        // percorre os endereços cadastrados para o aluno
                        foreach (DataRow enderecoaluno in UCEnderecos1._VS_enderecos.Rows)
                        {
                            if (enderecoaluno.RowState != DataRowState.Deleted)
                            {
                                // percorre os ceps próximos da escola
                                foreach (string cepProxEsc in cepProxEscSpplited)
                                {
                                    // ignora strings vazias
                                    if (!string.IsNullOrEmpty(cepProxEsc))
                                    {
                                        // verifica se o ceps cadastrados para o aluno está nas proximidades de alguma escola
                                        if (cepProxEsc.Trim() == Convert.ToString(enderecoaluno["end_cep"]))
                                        {
                                            if (_lstEscProx.Items.FindByValue(esc.Value) == null)
                                            {
                                                // adiciona nome da escola no listbox
                                                _lstEscProx.Items.Add(esc);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            divEscolasProximas.Visible = _lstEscProx.Items.Count > 0;

        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Insere e altera um aluno
    /// </summary>
    private void _Salvar(bool PermaneceTela, bool verificarAlunoExistente, bool verificarIntegridadePeloSom)
    {
        DataTable dtDuplicidadeFonetica = new DataTable();

        try
        {
            string msgErro;
            if (!UCContato1.SalvaConteudo(out msgErro))
            {
                UCContato1._MensagemErro_Mostrar = false;
                _lblMessage.Text = UtilBO.GetErroMessage(msgErro, UtilBO.TipoMensagem.Alerta); 
                txtSelectedTab.Value = indiceAbaEnderecoContato;
                return;
            }

            if (_lblMessage.Text == string.Empty && !UCGridCertidaoCivil1.AtualizaViewState(out msgErro))
            {
                UCContato1._MensagemErro_Mostrar = false;
                _lblMessage.Text = UtilBO.GetErroMessage(msgErro, UtilBO.TipoMensagem.Alerta); 
                txtSelectedTab.Value = indiceAbaDocumentacao;
                return;
            }

            if (_lblMessage.Text == string.Empty && !UCGridDocumento1.ValidaConteudoGrid(out msgErro))
            {
                UCGridDocumento1._MensagemErro.Visible = false;
                _lblMessage.Text = UtilBO.GetErroMessage(msgErro, UtilBO.TipoMensagem.Alerta); 
                txtSelectedTab.Value = indiceAbaDocumentacao;
                return;
            }

            if (_lblMessage.Text == string.Empty && !UCGridContatoNomeTelefone1.SalvaConteudoGrid(out msgErro))
            {
                UCGridContatoNomeTelefone1._MensagemErro.Visible = false;
                _lblMessage.Text = UtilBO.GetErroMessage(msgErro, UtilBO.TipoMensagem.Alerta); 
                txtSelectedTab.Value = indiceAbaFichaMedica;
                return;
            }

            PES_Pessoa entityPessoa = new PES_Pessoa
            {
                pes_id = UCCadastroPessoa1._VS_pes_id
                ,
                pes_nome = UCCadastroPessoa1._txtNome.Text
                ,
                pes_nome_abreviado = UCCadastroPessoa1._txtNomeAbreviado.Text
                ,
                pai_idNacionalidade = new Guid(UCCadastroPessoa1._ComboNacionalidade.SelectedValue)
                ,
                pes_naturalizado = UCCadastroPessoa1._chkNaturalizado.Checked
                ,
                cid_idNaturalidade = UCCadastroPessoa1._VS_cid_id
                ,
                pes_dataNascimento = (UCCadastroPessoa1._txtDataNasc.Text == string.Empty ? new DateTime() : Convert.ToDateTime(UCCadastroPessoa1._txtDataNasc.Text))
                ,
                pes_racaCor = UCCadastroPessoa1._ComboRacaCor.SelectedValue == "-1" ? Convert.ToByte(null) : Convert.ToByte(UCCadastroPessoa1._ComboRacaCor.SelectedValue)
                ,
                pes_sexo = UCCadastroPessoa1._ComboSexo.SelectedValue == "-1" ? Convert.ToByte(null) : Convert.ToByte(UCCadastroPessoa1._ComboSexo.SelectedValue)
                ,
                pes_idFiliacaoPai = UCCadastroPessoa1._VS_pes_idFiliacaoPai
                ,
                pes_idFiliacaoMae = UCCadastroPessoa1._VS_pes_idFiliacaoMae
                ,
                tes_id = new Guid(UCCadastroPessoa1._ComboEscolaridade.SelectedValue)
                ,
                pes_estadoCivil = UCCadastroPessoa1._ComboEstadoCivil.SelectedValue == "-1" ? Convert.ToByte(null) : Convert.ToByte(UCCadastroPessoa1._ComboEstadoCivil.SelectedValue)
                ,
                pes_situacao = 1
                ,
                pes_dataCriacao = DateTime.Now
                ,
                pes_dataAlteracao = DateTime.Now
                ,
                IsNew = (UCCadastroPessoa1._VS_pes_id != Guid.Empty) ? false : true
            };

            if (UCCadastroPessoa1._iptFoto.PostedFile != null && !string.IsNullOrEmpty(UCCadastroPessoa1._iptFoto.PostedFile.FileName))
            {
                string tam = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TAMANHO_MAX_FOTO_PESSOA);

                if (!string.IsNullOrEmpty(tam))
                {
                    if (UCCadastroPessoa1._iptFoto.PostedFile.ContentLength > Convert.ToInt32(tam) * 1000)
                    {
                        txtSelectedTab.Value = indiceAbaAluno;
                        throw new ArgumentException("Foto é maior que o tamanho máximo permitido.");
                    }

                    if (UCCadastroPessoa1._iptFoto.PostedFile.FileName.Substring(UCCadastroPessoa1._iptFoto.PostedFile.FileName.Length - 3, 3).ToUpper() != "JPG")
                    {
                        txtSelectedTab.Value = indiceAbaAluno;
                        throw new ArgumentException("Foto tem que estar no formato \".jpg\".");
                    }
                }

                byte[] Imagem = new byte[1];
                Imagem[0] = Convert.ToByte(0);

                int Tamanho = Convert.ToInt32(UCCadastroPessoa1._iptFoto.PostedFile.InputStream.Length);
                Imagem = new byte[Tamanho];

                UCCadastroPessoa1._iptFoto.PostedFile.InputStream.Read(Imagem, 0, Tamanho);
                entityPessoa.pes_foto = !string.IsNullOrEmpty(UCCadastroPessoa1._iptFoto.PostedFile.FileName) ? Imagem : null;
            }
            else
            {
                entityPessoa.pes_foto = UCCadastroPessoa1._chbExcluirImagem.Checked ? null : _VS_pes_foto;
            }

            PES_PessoaDeficiencia entityPessoaDeficiencia = new PES_PessoaDeficiencia
            {
                pes_id = UCCadastroPessoa1._VS_pes_id
                ,
                tde_id = new Guid(UCCadastroPessoa1._ComboTipoDeficiencia.SelectedValue)
                ,
                IsNew = true
            };

            ACA_Aluno entityAluno = new ACA_Aluno
            {
                alu_id = _VS_alu_id
                ,
                pes_id = _VS_pes_id
                ,
                ent_id = Ent_id
                ,
                rlg_id = UCComboReligiao.Valor
                ,
                alu_meioTransporte = _ddlMeioTransporte.SelectedValue == "-1" ? Convert.ToByte(0) : Convert.ToByte(_ddlMeioTransporte.SelectedValue)
                ,
                alu_tempoDeslocamento = _ddlTempoDeslocamento.SelectedValue == "-1" ? Convert.ToByte(0) : Convert.ToByte(_ddlTempoDeslocamento.SelectedValue)
                ,
                alu_regressaSozinho = chkRegressaSozinho.Checked
                ,
                alu_observacao = _txtObservacao.Text
                ,
                alu_dadosIncompletos = chkDadosIncompletos.Checked
                ,
                alu_historicoEscolaIncompleto = chkHistoricoEscolarIncompleto.Checked
                ,
                alu_situacao = Convert.ToInt32(UCComboAlunoSituacao1._Combo.SelectedValue) < 0 ? Convert.ToByte(null) : Convert.ToByte(UCComboAlunoSituacao1._Combo.SelectedValue)
                ,
                alu_dataAlteracao = VS_dataAlteracaoAluno
                ,
                IsNew = !(_VS_alu_id > 0)
            };

            ACA_AlunoAtendimentoEspecial entityAtendimentoEspecial = new ACA_AlunoAtendimentoEspecial
            {
                alu_id = _VS_alu_id
                ,
                tae_id = UCComboTipoAtendimentoEspecial1.Valor
                ,
                IsNew = true
            };

            #region AlunoFichaMédica

            ACA_AlunoFichaMedica entityAlunoFichaMedica = new ACA_AlunoFichaMedica
            {
                alu_id = _VS_alu_id
                ,
                afm_tipoSanguineo = _txtTipoSanguineo.Text
                ,
                afm_fatorRH = _txtFatorRH.Text
                ,
                afm_doencasConhecidas = _txtDoencaConhecidas.Text
                ,
                afm_alergias = _txtAlergias.Text
                ,
                afm_medicacoesPodeUtilizar = _txtMedicacoesPodeUtilizar.Text
                ,
                afm_medicacoesUsoContinuo = _txtMedicacoesUsoContinuo.Text
                ,
                afm_convenioMedico = _txtConvenioMedico.Text
                ,
                afm_hospitalRemocao = _txtHospitalRemocao.Text
                ,
                afm_outrasRecomendacoes = _txtOutrasRecomendacoes.Text
                ,
                IsNew = (_VS_alu_id > 0) ? !ACA_AlunoFichaMedicaBO.VerificaFichaMedicaExistente(_VS_alu_id) : true
            };

            #endregion

            // Validação para salvar usuário
            SYS_Usuario entityUsuario = new SYS_Usuario { usu_id = _VS_usu_id };

            bool salvarUsuario;
            bool verificaEmailUsuario;

            if (_chbCriarUsuario.Checked || _chbIntegrarUsuarioLive.Checked)
            {
                entityUsuario.usu_login = _txtLogin.Text;
                entityUsuario.usu_email = _txtEmail.Text;
                entityUsuario.usu_senha = _txtSenha.Text;
                entityUsuario.pes_id = UCCadastroPessoa1._VS_pes_id;
                entityUsuario.ent_id = Ent_id;
                entityUsuario.usu_situacao = 1;
                entityUsuario.usu_dataCriacao = DateTime.Now;
                entityUsuario.usu_dataAlteracao = DateTime.Now;
                entityUsuario.IsNew = (_VS_usu_id == Guid.Empty);

                entityUsuario.usu_dominio = string.Empty;

                if (_chkExpiraSenha.Checked)
                    entityUsuario.usu_situacao = 5;
                if (_chkBloqueado.Checked)
                    entityUsuario.usu_situacao = 2;

                ManageUserLive verificar = new ManageUserLive();

                verificaEmailUsuario = false;

                if (!_chbIntegrarUsuarioLive.Checked)
                {
                    if (verificar.IsContaEmail(_txtEmail.Text))
                    {
                        txtSelectedTab.Value = indiceAbaUsuarios;
                        throw new ArgumentException("Integrar usuário live é obrigatório, o email " + _txtEmail.Text + " contém o domínio para integração com live.");
                    }
                }
                else
                {
                    verificaEmailUsuario = true;
                }

                salvarUsuario = true;

            }
            else
            {
                verificaEmailUsuario = false;
                salvarUsuario = false;
            }

            List<int> liProgramaSocial = new List<int>();

            if (fsProgramaSocial.Visible)
            {
                foreach (ListItem li in cblProgramaSocial.Items)
                {
                    if (li.Selected)
                        liProgramaSocial.Add(Convert.ToInt32(li.Value));
                }
            }

            CarregarMovimentacao();

            if (ACA_AlunoBO.Save(entityPessoa
                                 , entityAtendimentoEspecial
                                 , entityPessoaDeficiencia
                                 , UCEnderecos1._VS_enderecos
                                 , UCContato1._carregaDataTableComContatos
                                 , UCGridDocumento1.RetornaDocumentoSave()
                                 , UCGridCertidaoCivil1._VS_certidoes
                                 , _VS_pai_idAntigo
                                 , _VS_cid_idAntigo
                                 , _VS_pes_idPaiAntigo
                                 , _VS_pes_idMaeAntigo
                                 , _VS_tes_idAntigo
                                 , _VS_tde_idAntigo
                                 , entityAluno
                                 , RetornaResponsaveisCadastrados()
                                 , ucComboTipoResponsavel.Valor
                                 , entityAlunoCurriculoAntigo
                                 , entityAlunoCurriculoNovo
                                 , entityMatriculaTurmaAntigo
                                 , entityMatriculaTurmaNovo
                                 , entityMovimentacao
                                 , ckbTranfForaRede.Checked
                                 , _VS_historico
                                 , _VS_historicoObservacao
                                 , _VS_disciplina
                                 , liProgramaSocial
                                 , salvarUsuario
                                 , verificaEmailUsuario
                                 , entityUsuario
                                 , _chkSenhaAutomatica.Checked
                                 , ApplicationWEB._TituloDasPaginas
                                 , ApplicationWEB._EmailHost
                                 , ApplicationWEB._EmailSuporte
                                 , Ent_id
                                 , entityAlunoFichaMedica
                                 , UCGridContatoNomeTelefone1._VS_contatos
                                 , verificarAlunoExistente
                                 , verificarIntegridadePeloSom
                                 , out dtDuplicidadeFonetica
                                 , __SessionWEB.__UsuarioWEB.Usuario.usu_id
                                 , __SessionWEB.__UsuarioWEB.Grupo.gru_id
                                 ))
            {

                if (_VS_alu_id <= 0)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "alu_id: " + entityAluno.alu_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage(String.Format("Aluno incluído com sucesso."), UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "alu_id: " + entityAluno.alu_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage(String.Format("Aluno alterado com sucesso."), UtilBO.TipoMensagem.Sucesso);
                }

                if (PermaneceTela) //se clicar botão salvar
                {
                    _VS_alu_id = entityAluno.alu_id;
                    LoadDadosTela();
                    _lblMessage.Text = __SessionWEB.PostMessages;
                    _btnNovo.Visible = true;
                }
                else //se clicar botão salvar e voltar
                {
                    Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/Aluno/Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            else
            {
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o aluno.", UtilBO.TipoMensagem.Erro);
            }
        }
        catch (ExcessoAlunosTurmaException ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (WebControls_Endereco_UCEnderecos.EnderecoException ex)
        {
            txtSelectedTab.Value = indiceAbaEnderecoContato;
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (ValidationException ex)
        {
            TrataValidationException(ex);
        }
        catch (ArgumentException ex)
        {
            UCEnderecos1.AtualizaEnderecos();

            if (UCCadastroPessoa1._iptFoto.PostedFile != null)
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message + "<br />" + GestaoEscolarUtilBO.VerificarFoto(UCCadastroPessoa1._iptFoto.PostedFile.FileName), UtilBO.TipoMensagem.Alerta);
        }
        catch (DuplicateNameException ex)
        {
            UCEnderecos1.AtualizaEnderecos();

            if (UCCadastroPessoa1._iptFoto.PostedFile != null)
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message + "<br />" + GestaoEscolarUtilBO.VerificarFoto(UCCadastroPessoa1._iptFoto.PostedFile.FileName), UtilBO.TipoMensagem.Alerta);
        }
        catch (AlunoExistenteException)
        {
            UCEnderecos1.AtualizaEnderecos();

            lblAlunoExistente.Text = "Nome: <b>" + UCCadastroPessoa1._txtNome.Text + "</b><BR />" +
                                     "Data de nascimento: <b>" + UCCadastroPessoa1._txtDataNasc.Text + "</b><BR />";

            if (_VS_alu_id <= 0)
            {
                btnConfirmarAlunoExistente.Text = "Confirmar inclusão de novo aluno";
                btnCancelarAlunoExistente.Text = "Cancelar inclusão de novo aluno";
            }
            else
            {
                btnConfirmarAlunoExistente.Text = "Confirmar alteração do aluno";
                btnCancelarAlunoExistente.Text = "Cancelar alteração do aluno";
            }

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "AlunoExistente", "$(document).ready(function(){ $('#divAlunoExistente').dialog('open'); });", true);
        }
        catch (DuplicidadeBuscaFoneticaException)
        {
            UCEnderecos1.AtualizaEnderecos();

            lblDuplicidadeFonetica.Text = "Nome: <b>" + UCCadastroPessoa1._txtNome.Text + "</b><BR />" +
                                          "Data de nascimento: <b>" + UCCadastroPessoa1._txtDataNasc.Text + "</b><BR />" +
                                          "Nome da mãe: <b>" + ucResponsavelMae.NomePessoa + "</b><BR /><BR />";

            grvDuplicidadeFonetica.DataSource = dtDuplicidadeFonetica;
            grvDuplicidadeFonetica.DataBind();

            if (_VS_alu_id <= 0)
            {
                btnConfirmarAluno.Text = "Confirmar inclusão de novo aluno";
                btnCancelarAluno.Text = "Cancelar inclusão de novo aluno";
            }
            else
            {
                btnConfirmarAluno.Text = "Confirmar alteração do aluno";
                btnCancelarAluno.Text = "Cancelar alteração do aluno";
            }

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "ExibirDuplicidadeFonetica", "$(document).ready(function(){ $('#divDuplicidadeFonetica').dialog('open'); });", true);
        }
        catch (EditarAluno_ValidationException ex)
        {
            __SessionWEB.PostMessages = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);

            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/Aluno/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            UCEnderecos1.AtualizaEnderecos();
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o aluno.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Trata as excessões do tipo ValidationException e as que herdam dela.
    /// Verifica qual o tipo da excessão, e aponta para a aba correta na tela.
    /// </summary>
    /// <param name="ex">Excessão disparada no método Save</param>
    private void TrataValidationException(ValidationException ex)
    {
        string aba;

        if (ex is ACA_Aluno_ValidationException)
        {
            aba = indiceAbaAluno;
        }
        else if ((ex is ACA_AlunoCurriculo_ValidationException) ||
            (ex is MTR_Movimentacao_ValidationException))
        {
            aba = indiceAbaMovimentacao;
        }
        else if ((ex is ACA_AlunoHistorico_ValidationException) ||
            (ex is ACA_AlunoHistoricoObservacao_ValidationException))
        {
            aba = indiceAbaHistorico;
        }
        else if (ex is WebControls_Endereco_UCEnderecos.EnderecoException)
        {
            aba = indiceAbaEnderecoContato;
        }
        else
        {
            aba = indiceAbaAluno;
        }

        string msgErro = UtilBO.GetErroMessage(ex.Message +
            ((UCCadastroPessoa1._iptFoto.PostedFile != null) ?
            "<br />" + GestaoEscolarUtilBO.VerificarFoto(UCCadastroPessoa1._iptFoto.PostedFile.FileName) :
            "")
            , UtilBO.TipoMensagem.Alerta);

        UCEnderecos1.AtualizaEnderecos();
        txtSelectedTab.Value = aba;

        // Seta mensagem de erro.
        _lblMessage.Text = msgErro;
    }

    /// <summary>
    /// Retorna a list com os responsáveis cadastrados para o aluno.
    /// </summary>
    /// <returns></returns>
    private List<ACA_AlunoResponsavelBO.StructCadastro> RetornaResponsaveisCadastrados()
    {
        List<ACA_AlunoResponsavelBO.StructCadastro> lista = new List<ACA_AlunoResponsavelBO.StructCadastro>();

        ACA_AlunoResponsavelBO.StructCadastro item;

        if (ucResponsavelMae.RetornaStructCadastro(ucComboTipoResponsavel.Valor, out item))
            lista.Add(item);

        if (ucResponsavelPai.RetornaStructCadastro(ucComboTipoResponsavel.Valor, out item))
            lista.Add(item);

        if (ucResponsavelOutro.RetornaStructCadastro(ucComboTipoResponsavel.Valor, out item))
            lista.Add(item);

        return lista;
    }

    #region SubCadastros

    #region Usuário

    /// <summary>
    /// Método criado para habilitar campos obrigatório
    /// </summary>
    private void _ValidarCampoCadastroUsuario()
    {
        ManageUserLive verificar = new ManageUserLive();

        // tratamento para os campos de usuario live
        if (_chbIntegrarUsuarioLive.Checked)
        {
            UCCamposObrigatorios4.Visible = false;
            _lblEmail.Text = "E-mail ";
            _lblMsgUsuarioLive.Visible = true;
            _rfvEmail.Enabled = false;
            _chkSenhaAutomatica.Visible = false;
            _rfvLogin.Enabled = false;
            _txtLogin.Visible = false;
            _rfvSenha.Enabled = false;
            _txtSenha.Visible = false;
            revSenha.Enabled = false;
            revSenhaTamanho.Enabled = false;
            _rfvConfirmarSenha.Enabled = false;
            _txtConfirmacao.Visible = false;
            Label1.Visible = false;
            _lblSenha.Visible = false;
            _lblConfirmacao.Visible = false;
            _chkExpiraSenha.Visible = false;
            _chkBloqueado.Visible = false;

            //quando for carregado o usuário, verifica se o dominio é do usuario live
            //caso seja, configura tais campos.
            if (verificar.IsContaEmail(_txtEmail.Text))
            {
                _txtEmail.Enabled = (_VS_usu_id != Guid.Empty ? false : true);
                _txtSenha.Visible = (_VS_usu_id != Guid.Empty ? true : false);
                _lblSenha.Visible = (_VS_usu_id != Guid.Empty ? true : false);
                _lblSenha.Text = "Senha ";

                revSenha.Enabled = (_VS_usu_id != Guid.Empty ? true : false);
                revSenhaTamanho.Enabled = (_VS_usu_id != Guid.Empty ? true : false);
                _lblConfirmacao.Text = "Confirmar senha ";
                _lblConfirmacao.Visible = (_VS_usu_id != Guid.Empty ? true : false);
                _txtConfirmacao.Visible = (_VS_usu_id != Guid.Empty ? true : false);
                _chkExpiraSenha.Visible = (_VS_usu_id != Guid.Empty ? true : false);
                _chkBloqueado.Visible = (_VS_usu_id != Guid.Empty ? true : false);
            }
        }
        else
        {
            //configurando campos quando não for usuário do tipo live.
            UCCamposObrigatorios4.Visible = true;
            _lblEmail.Text = "E-mail *";
            _txtEmail.Enabled = true;
            _lblMsgUsuarioLive.Visible = false;
            _rfvEmail.Enabled = true;
            _chkSenhaAutomatica.Visible = true;
            _rfvLogin.Enabled = true;
            _txtLogin.Visible = true;
            _rfvSenha.Enabled = true;
            _txtSenha.Visible = true;
            revSenha.Enabled = false;
            revSenhaTamanho.Enabled = false;
            _rfvConfirmarSenha.Enabled = true;
            _txtConfirmacao.Visible = true;
            Label1.Text = "Login *";
            Label1.Visible = true;
            _lblSenha.Text = (_VS_usu_id != Guid.Empty ? "Senha" : "Senha *");
            _lblSenha.Visible = true;
            _lblConfirmacao.Text = (_VS_usu_id != Guid.Empty ? "Confirmar senha" : "Confirmar senha *");
            _lblConfirmacao.Visible = true;
            _chkExpiraSenha.Visible = true;
            _chkBloqueado.Visible = true;
        }
    }

    #endregion

    #region Movimentação

    /// <summary>
    /// Retorna o código da situação do AlunoCurriculo
    /// </summary>
    /// <returns></returns>
    private byte RetornaSituacaoAlunoCurriculo()
    {
        switch (UCComboAlunoSituacao1._Combo.SelectedValue)
        {
            case "1":
                return Convert.ToByte(ACA_AlunoCurriculoSituacao.Ativo);
            case "4":
                return Convert.ToByte(ACA_AlunoCurriculoSituacao.Inativo);
            case "7":
                return Convert.ToByte(ACA_AlunoCurriculoSituacao.EmMatricula);
            case "8":
                return Convert.ToByte(ACA_AlunoCurriculoSituacao.Ativo);
            default:
                return 0;
        }
    }

    /// <summary>
    /// Retorna o código da situação da MatriculaTurma
    /// </summary>
    /// <returns></returns>
    private byte RetornaSituacaoMatriculaTurma()
    {
        switch (UCComboAlunoSituacao1._Combo.SelectedValue)
        {
            case "1":
                return Convert.ToByte(MTR_MatriculaTurmaSituacao.Ativo);
            case "4":
                return Convert.ToByte(MTR_MatriculaTurmaSituacao.Inativo);
            case "7":
                return Convert.ToByte(MTR_MatriculaTurmaSituacao.EmMatricula);
            case "8":
                return Convert.ToByte(MTR_MatriculaTurmaSituacao.Ativo);
            default:
                return 0;
        }
    }

    /// <summary>
    /// Valida a movimentação.     
    /// </summary>
    private void ValidarMovimentacao()
    {
        try
        {
            if (divVelhaMovimentacao.Visible)
            {
                if ((ckbAltCurso.Checked
                    || ckbAltTurma.Checked
                    || ckbRecAluno.Checked
                    || ckbTranfDentroRede.Checked
                    || ckbTranfForaRede.Checked)
                    && (UCComboTipoMovimentacaoEntrada1.Valor <= 0 && UCComboTipoMovimentacaoSaida.Valor <= 0))
                {
                    throw new MTR_Movimentacao_ValidationException("Selecione pelo menos um tipo de movimentação de entrada ou saída.");
                }

                if (ckbAltTurma.Checked)
                {
                    if (UCComboTurma2._Combo.SelectedValue == "-1;-1;-1")
                    {
                        throw new MTR_Movimentacao_ValidationException("Turma é obrigatório para incluir / alterar aluno na turma.");
                    }

                    if (UCComboTurma2._Combo.SelectedValue.Split(';')[0] == _VS_tur_id_Anterior.ToString())
                    {
                        throw new MTR_Movimentacao_ValidationException("Selecione uma turma diferente da atual para incluir / alterar aluno na turma.");
                    }
                }
                else
                    if (ckbRecAluno.Checked)
                    {
                        if (UCComboCurriculoPeriodo2._Combo.SelectedValue == "-1;-1;-1")
                        {
                            throw new MTR_Movimentacao_ValidationException(GestaoEscolarUtilBO.nomePadraoPeriodo() + " é obrigatório para reclassificar aluno.");
                        }

                        if (UCComboCurriculoPeriodo2._Combo.SelectedValue == _VS_cur_id_Anterior + ";" + _VS_crr_id_Anterior + ";" + _VS_crp_id_Anterior)
                        {
                            throw new MTR_Movimentacao_ValidationException("Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoPeriodo().ToLower() + " diferente do atual para reclassificar aluno.");
                        }
                    }
                    else
                        if (ckbAltCurso.Checked)
                        {
                            if (UCComboCursoCurriculo2.Valor[0] <= 0)
                            {
                                throw new MTR_Movimentacao_ValidationException(GestaoEscolarUtilBO.nomePadraoCurso() + " é obrigatório para alterar o(a) " + GestaoEscolarUtilBO.nomePadraoCurso().ToLower() + ".");
                            }

                            if (UCComboCursoCurriculo2.Valor[0] == _VS_cur_id_Anterior && UCComboCursoCurriculo2.Valor[1] == _VS_crr_id_Anterior)
                            {
                                throw new MTR_Movimentacao_ValidationException("Selecione um(a) " + GestaoEscolarUtilBO.nomePadraoCurso().ToLower() + " diferente do atual para alterar o(a) " + GestaoEscolarUtilBO.nomePadraoCurso().ToLower() + ".");
                            }

                            if (UCComboCurriculoPeriodo2._Combo.SelectedValue == "-1;-1;-1")
                            {
                                throw new MTR_Movimentacao_ValidationException(GestaoEscolarUtilBO.nomePadraoPeriodo() + " é obrigatório para alterar o(a) " + GestaoEscolarUtilBO.nomePadraoCurso().ToLower() + ".");
                            }
                        }
                        else
                            if (ckbTranfDentroRede.Checked)
                            {
                                if (ddlUnidadeEscola.SelectedValue == "-1;-1")
                                {
                                    throw new MTR_Movimentacao_ValidationException("Escola é obrigatório para transferir o aluno dentro da rede.");
                                }

                                if (ddlUnidadeEscola.SelectedValue == _VS_esc_id_Anterior + ";" + _VS_uni_id_Anterior)
                                {
                                    throw new MTR_Movimentacao_ValidationException("Selecione uma escola diferente da atual para transferir o aluno dentro da rede.");
                                }

                                if (UCComboCursoCurriculo2.Valor[0] <= 0)
                                {
                                    throw new MTR_Movimentacao_ValidationException(GestaoEscolarUtilBO.nomePadraoCurso() + " é obrigatório para transferir o aluno dentro da rede.");
                                }

                                if (UCComboCurriculoPeriodo2._Combo.SelectedValue == "-1;-1;-1")
                                {
                                    throw new MTR_Movimentacao_ValidationException(GestaoEscolarUtilBO.nomePadraoPeriodo() + " é obrigatório para transferir o aluno dentro da rede.");
                                }
                            }
            }
            else
            {
                if (divNovaMovimentacao.Visible && UCComboTipoMovimentacaoEntrada.Valor <= 0)
                {
                    throw new ValidationException("Selecione pelo menos um tipo de movimentação de entrada.");
                }
            }
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Carrega os dados da aba de movimentação
    /// </summary>
    private void CarregaAbaMovimentacao()
    {
        //Desmarca os checkbox e combos
        ckbAltCurso.Checked = false;
        ckbAltTurma.Checked = false;
        ckbRecAluno.Checked = false;
        ckbTranfDentroRede.Checked = false;
        ckbTranfForaRede.Checked = false;
        checkBox_CheckedChanged(ckbAltTurma, new EventArgs());

        PesquisarMovimentacoes();

        if (_VS_alu_id > 0)
        {

            if (UCComboAlunoSituacao1._Combo.SelectedValue == "1" || UCComboAlunoSituacao1._Combo.SelectedValue == "7"
                || UCComboAlunoSituacao1._Combo.SelectedValue == "8")
            {
                DataTable dtCurriculo = ACA_AlunoCurriculoBO.GetSelectBy_alu_id(_VS_alu_id);

                if (dtCurriculo.Rows.Count > 0)
                {
                    divNovaMovimentacao.Visible = false;
                    divVelhaMovimentacao.Visible = true;

                    _VS_alc_id = Convert.ToInt32(dtCurriculo.Rows[0]["alc_id"].ToString());
                    _VS_esc_id = _VS_esc_id_Anterior = Convert.ToInt32(dtCurriculo.Rows[0]["esc_id"].ToString());
                    _VS_uni_id = _VS_uni_id_Anterior = Convert.ToInt32(dtCurriculo.Rows[0]["uni_id"].ToString());
                    _VS_cur_id = _VS_cur_id_Anterior = Convert.ToInt32(dtCurriculo.Rows[0]["cur_id"].ToString());
                    _VS_crr_id = _VS_crr_id_Anterior = Convert.ToInt32(dtCurriculo.Rows[0]["crr_id"].ToString());
                    _VS_crp_id = _VS_crp_id_Anterior = Convert.ToInt32(dtCurriculo.Rows[0]["crp_id"].ToString());
                    _VS_tur_id = _VS_tur_id_Anterior = Convert.ToInt64(string.IsNullOrEmpty(dtCurriculo.Rows[0]["tur_id"].ToString()) ? "-1" : dtCurriculo.Rows[0]["tur_id"].ToString());
                    _VS_mtu_id = _VS_mtu_id_Anterior = Convert.ToInt32(string.IsNullOrEmpty(dtCurriculo.Rows[0]["mtu_id"].ToString()) ? "-1" : dtCurriculo.Rows[0]["mtu_id"].ToString());
                    _VS_ttn_id = _VS_ttn_id_Anterior = Convert.ToInt32(string.IsNullOrEmpty(dtCurriculo.Rows[0]["ttn_id"].ToString()) ? "-1" : dtCurriculo.Rows[0]["ttn_id"].ToString());

                    ESC_Escola escola = new ESC_Escola { esc_id = Convert.ToInt32(dtCurriculo.Rows[0]["esc_id"].ToString()) };
                    ESC_EscolaBO.GetEntity(escola);

                    if (Convert.ToBoolean(ACA_ParametroAcademicoBO.ParametroValor(eChaveAcademico.FILTRAR_ESCOLA_UA_SUPERIOR)))
                    {
                        SYS_UnidadeAdministrativa ua = new SYS_UnidadeAdministrativa { uad_id = escola.uad_id, ent_id = escola.ent_id };
                        SYS_UnidadeAdministrativaBO.GetEntity(ua);

                        SYS_UnidadeAdministrativa uaSuperior = new SYS_UnidadeAdministrativa { uad_id = ua.uad_idSuperior, ent_id = ua.ent_id };
                        SYS_UnidadeAdministrativaBO.GetEntity(uaSuperior);

                        if (!string.IsNullOrEmpty(uaSuperior.uad_nome))
                            lblDadosAluno.Text = UCFiltroEscolas1._LabelUnidadeAdministrativa.Text.Replace("*", string.Empty)
                                + ": <b>" + uaSuperior.uad_nome + "</b><BR />";

                        _VS_uad_idSuperior = uaSuperior.uad_id;
                    }
                    else
                    {
                        _VS_uad_idSuperior = Guid.Empty;
                    }

                    if (UCComboAlunoSituacao1._Combo.SelectedValue == "7" || UCComboAlunoSituacao1._Combo.SelectedValue == "8")
                    {
                        UCComboTurma1.Obrigatorio = false;
                        UCComboTurma1.Visible = false;
                        UCComboTurma1.Valor = new int[] { -1, -1, -1 };
                        ckbAltTurma.Enabled = false;
                    }

                    lblDadosAluno.Text += "Escola: <b>" + dtCurriculo.Rows[0]["unidadeescola"] + "</b><BR />";
                    lblDadosAluno.Text += GestaoEscolarUtilBO.nomePadraoCurso() + ": <b>" + dtCurriculo.Rows[0]["cur_nome"] + "</b><BR />";
                    lblDadosAluno.Text += GestaoEscolarUtilBO.nomePadraoPeriodo() + ": <b>" + dtCurriculo.Rows[0]["crp_descricao"] + "</b><BR />";

                    if (!string.IsNullOrEmpty(dtCurriculo.Rows[0]["tur_codigo"].ToString()))
                        lblDadosAluno.Text += "Turma: <b>" + dtCurriculo.Rows[0]["tur_codigo"] + "</b><BR />";

                    _txtMatriculaNumero2.Text = dtCurriculo.Rows[0]["alc_matricula"].ToString();
                    _txtCensoID2.Text = dtCurriculo.Rows[0]["alc_codigoInep"].ToString();
                    if (!string.IsNullOrEmpty(ACA_ParametroAcademicoBO.ParametroValor(eChaveAcademico.MATRICULA_ESTADUAL)))
                    {
                        _lblMatriculaEstadual2.Visible = true;
                        _txtMatriculaEstadual2.Visible = true;
                        _txtMatriculaEstadual2.Text = dtCurriculo.Rows[0]["alc_matriculaEstadual"].ToString();
                    }
                    else
                    {
                        _lblMatriculaEstadual2.Visible = false;
                        _txtMatriculaEstadual2.Visible = false;
                    }

                    _txtMatriculaDataPrimeira2.Text = dtCurriculo.Rows[0]["alc_dataPrimeiraMatricula"].ToString();
                    _txtMatriculaDataSaida2.Text = dtCurriculo.Rows[0]["alc_dataSaida"].ToString();
                    _txtMatriculaDataColacao2.Text = dtCurriculo.Rows[0]["alc_dataColacao"].ToString();

                    grvHistoricoMovimentacao.Columns[indiceColunaEscolaAnterior].HeaderText = "Escola / " + GestaoEscolarUtilBO.nomePadraoCurso()
                        + " / " + GestaoEscolarUtilBO.nomePadraoPeriodo() + " anterior";
                    grvHistoricoMovimentacao.Columns[indiceColunaEscolaAtual].HeaderText = "Escola / " + GestaoEscolarUtilBO.nomePadraoCurso()
                        + " / " + GestaoEscolarUtilBO.nomePadraoPeriodo() + " atual";
                }
                else
                {
                    divNovaMovimentacao.Visible = true;
                    divVelhaMovimentacao.Visible = false;

                    // Limpar todos os ViewStates - o aluno não possui nenhuma matrícula.
                    _VS_alc_id = -1;
                    _VS_esc_id = _VS_esc_id_Anterior = -1;
                    _VS_uni_id = _VS_uni_id_Anterior = -1;
                    _VS_cur_id = _VS_cur_id_Anterior = -1;
                    _VS_crr_id = _VS_crr_id_Anterior = -1;
                    _VS_crp_id = _VS_crp_id_Anterior = -1;
                    _VS_tur_id = _VS_tur_id_Anterior = -1;
                    _VS_mtu_id = _VS_mtu_id_Anterior = -1;
                    _VS_ttn_id = _VS_ttn_id_Anterior = -1;
                    _VS_uad_idSuperior = Guid.Empty;
                }
            }
            else
            {
                // Limpar todos os ViewStates - o aluno está na situação Inativo.
                _VS_alc_id = -1;
                _VS_esc_id = _VS_esc_id_Anterior = -1;
                _VS_uni_id = _VS_uni_id_Anterior = -1;
                _VS_cur_id = _VS_cur_id_Anterior = -1;
                _VS_crr_id = _VS_crr_id_Anterior = -1;
                _VS_crp_id = _VS_crp_id_Anterior = -1;
                _VS_tur_id = _VS_tur_id_Anterior = -1;
                _VS_mtu_id = _VS_mtu_id_Anterior = -1;
                _VS_ttn_id = _VS_ttn_id_Anterior = -1;
                _VS_uad_idSuperior = Guid.Empty;

                divNovaMovimentacao.Visible = false;
                divVelhaMovimentacao.Visible = false;
            }
        }
    }

    /// <summary>
    /// Atualiza os combos de unidade administrativa superior e de unidade da escola da seguinte forma:
    /// 1) Se o usuário for da visão administrador sempre carrega todas as UA's superior e todas as escolas
    /// 2) Se o usuário for da visão UA ou Gestão e selecionar um aluno matriculado em alguma escola que ele tem permissão,
    ///    carrega todas as UA's superior e todas as escolas
    /// 3) Se o usuário for da visão UA ou Gestão e selecionar um aluno NÃO matriculado em alguma escola que ele tem permissão,
    ///    carrega a UA superior e escola do aluno selecionado + as UA's superior e escola que ele tem permissão 
    /// </summary>
    private void LoadEscolaDados(Guid uad_idSuperior)
    {
        bool CarregarTodasEscolas = false;

        if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa)
        {
            DataTable dtEscola = ESC_UnidadeEscolaBO.GetSelect(0, 0, 0, __SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id);

            for (int i = 0; i < dtEscola.Rows.Count; i++)
            {
                if (dtEscola.Rows[i]["esc_id"].ToString() == _VS_esc_id.ToString() && dtEscola.Rows[i]["uni_id"].ToString() == _VS_uni_id.ToString())
                {
                    CarregarTodasEscolas = true;
                    break;
                }
            }
        }
        else
        {
            CarregarTodasEscolas = true;
        }

        if (_VS_FiltroEscola)
        {
            if (CarregarTodasEscolas)
            {
                lblInformacao.Visible = false;

                DataTable dtUA = SYS_UnidadeAdministrativaBO.GetSelectBy_Pesquisa_PermissaoTotal(_VS_tua_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                ddlUASuperior.Items.Clear();
                ddlUASuperior.Items.Insert(0, new ListItem("-- Selecione um(a) " + lblUASuperior.Text.Replace("*", "") + " --", Guid.Empty.ToString(), true));
                ddlUASuperior.DataSource = dtUA;
                ddlUASuperior.DataBind();
                ddlUASuperior.SelectedValue = uad_idSuperior.ToString();

                DataTable dt = ESC_UnidadeEscolaBO.GetSelectByUASuperiorPermissaoTotal(uad_idSuperior, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                ddlUnidadeEscola.Items.Clear();
                ddlUnidadeEscola.Items.Insert(0, new ListItem("-- Selecione uma escola --", "-1;-1", true));
                ddlUnidadeEscola.Enabled = false;

                ddlUnidadeEscola.DataSource = dt;
                ddlUnidadeEscola.DataBind();
            }
            else
            {
                lblInformacao.Text = UtilBO.GetErroMessage("Só é possível movimentar um aluno de outra escola para a(s) sua(s) própria(s) escola(s).", UtilBO.TipoMensagem.Informacao);
                lblInformacao.Visible = true;

                DataTable dtUA = SYS_UnidadeAdministrativaBO.GetSelectBy_Pesquisa_PermissaoUsuarioUASuperior(_VS_tua_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id);

                ddlUASuperior.Items.Clear();
                ddlUASuperior.Items.Insert(0, new ListItem("-- Selecione um(a) " + lblUASuperior.Text.Replace("*", "") + " --", Guid.Empty.ToString(), true));
                ddlUASuperior.DataSource = dtUA;
                ddlUASuperior.DataBind();

                if (ddlUASuperior.Items.FindByValue(uad_idSuperior.ToString()) != null)
                    ddlUASuperior.SelectedValue = uad_idSuperior.ToString();

                DataTable dt = ESC_UnidadeEscolaBO.GetSelectByUASuperior(uad_idSuperior, __SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id);

                ddlUnidadeEscola.Items.Clear();
                ddlUnidadeEscola.Items.Insert(0, new ListItem("-- Selecione uma escola --", "-1;-1", true));
                ddlUnidadeEscola.Enabled = false;
                ddlUnidadeEscola.DataSource = dt;
                ddlUnidadeEscola.DataBind();
            }
        }
        else
        {
            if (CarregarTodasEscolas)
            {
                lblInformacao.Visible = false;

                DataTable dt = ESC_UnidadeEscolaBO.GetSelectPermissaoTotal(__SessionWEB.__UsuarioWEB.Usuario.ent_id);

                ddlUnidadeEscola.Items.Clear();
                ddlUnidadeEscola.Items.Insert(0, new ListItem("-- Selecione uma escola --", "-1;-1", true));
                ddlUnidadeEscola.Enabled = true;
                ddlUnidadeEscola.DataSource = dt;
                ddlUnidadeEscola.DataBind();
            }
            else
            {
                lblInformacao.Text = UtilBO.GetErroMessage("Só é possível movimentar um aluno de outra escola para a(s) sua(s) própria(s) escola(s).", UtilBO.TipoMensagem.Informacao);
                lblInformacao.Visible = true;

                DataTable dt = ESC_UnidadeEscolaBO.GetSelect(0, 0, 0, __SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id);

                ddlUnidadeEscola.Items.Clear();
                ddlUnidadeEscola.Items.Insert(0, new ListItem("-- Selecione uma escola --", "-1;-1", true));
                ddlUnidadeEscola.Enabled = true;
                ddlUnidadeEscola.DataSource = dt;
                ddlUnidadeEscola.DataBind();
            }
        }
    }

    /// <summary>
    /// Verifica os parâmetros acadêmicos cadastrados, mostrando e carregando os combos 
    /// conforme a configuração. Não mostra o combo de escola, só o de UA.
    /// Se parâmetro FILTRAR_ESCOLA_UA_SUPERIOR = "Sim", mostra combo de Unidade Administrativa,
    /// e carrega no combo pelo tipo de UA que estiver setada no parâmetro 
    /// TIPO_UNIDADE_ADMINISTRATIVA_FILTRO_ESCOLA.
    /// Desconsiderando as permissões do usuário
    /// </summary>
    private void LoadInicialEscolaDados()
    {
        ddlUASuperior.Enabled = false;
        ddlUnidadeEscola.Enabled = false;

        ddlUASuperior.Visible = false;
        lblUASuperior.Visible = false;

        ddlUnidadeEscola.Visible = false;
        lblUnidadeEscola.Visible = false;

        if (!ACA_ParametroAcademicoBO.VerificaFiltroUniAdmSuperior())
        {
            DataTable dt = ESC_UnidadeEscolaBO.GetSelectPermissaoTotal(__SessionWEB.__UsuarioWEB.Usuario.ent_id);

            ddlUnidadeEscola.DataTextField = "uni_escolaNome";
            ddlUnidadeEscola.Items.Clear();
            ddlUnidadeEscola.Items.Insert(0, new ListItem("-- Selecione uma escola --", "-1;-1", true));
            ddlUnidadeEscola.DataSource = dt;
            ddlUnidadeEscola.DataBind();

            _VS_FiltroEscola = false;
        }
        else
        {
            _VS_tua_id = ACA_ParametroAcademicoBO.VerificaFiltroEscola("TIPO_UNIDADE_ADMINISTRATIVA_FILTRO_ESCOLA", false, 0, 10);
            SYS_TipoUnidadeAdministrativa TipoUnidadeAdm = new SYS_TipoUnidadeAdministrativa { tua_id = _VS_tua_id };
            SYS_TipoUnidadeAdministrativaBO.GetEntity(TipoUnidadeAdm);

            lblUASuperior.Text = string.IsNullOrEmpty(TipoUnidadeAdm.tua_nome) ? "Unidade administrativa superior" : TipoUnidadeAdm.tua_nome;
            lblUASuperior.Text += " *";

            DataTable dt = SYS_UnidadeAdministrativaBO.GetSelectBy_Pesquisa_PermissaoTotal(_VS_tua_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            ddlUASuperior.Items.Clear();
            ddlUASuperior.Items.Insert(0, new ListItem("-- Selecione um(a) " + lblUASuperior.Text.Replace("*", "") + " --", Guid.Empty.ToString(), true));
            ddlUASuperior.DataSource = dt;
            ddlUASuperior.DataBind();

            ddlUnidadeEscola.DataTextField = "esc_uni_nome";
            ddlUnidadeEscola.Items.Clear();
            ddlUnidadeEscola.Items.Insert(0, new ListItem("-- Selecione uma escola --", "-1;-1", true));
            ddlUnidadeEscola.Enabled = false;

            _VS_FiltroEscola = true;
        }
    }

    /// <summary>
    /// Salva a movimentação no banco de dados   
    /// </summary>
    private void CarregarMovimentacao()
    {
        ValidarMovimentacao();

        //Dados da movimentação
        entityMovimentacao = new MTR_Movimentacao
        {
            alu_id = _VS_alu_id
            ,
            usu_id = __SessionWEB.__UsuarioWEB.Usuario.usu_id
            ,
            alc_idAnterior = _VS_alc_id
            ,
            alc_idAtual = ckbTranfForaRede.Checked || UCComboAlunoSituacao1._Combo.SelectedValue == "4" ? -1 : _VS_alc_id
            ,
            mtu_idAnterior = _VS_mtu_id
            ,
            mov_dataRealizacao = DateTime.Now.Date
            ,
            mov_situacao = 1
            ,
            IsNew = true
        };

        if (divVelhaMovimentacao.Visible)
        {
            entityMovimentacao.tmv_idEntrada = UCComboTipoMovimentacaoEntrada1.Valor;
            entityMovimentacao.tmv_idSaida = UCComboTipoMovimentacaoSaida.Valor;

            #region AlunoCurriculo

            // Dados da matrícula anterior                
            entityAlunoCurriculoAntigo = new ACA_AlunoCurriculo();
            entityAlunoCurriculoAntigo.alu_id = _VS_alu_id;
            entityAlunoCurriculoAntigo.alc_id = _VS_alc_id;
            ACA_AlunoCurriculoBO.GetEntity(entityAlunoCurriculoAntigo);

            // caso houver alteração apenas nesses dados
            entityAlunoCurriculoAntigo.alc_matricula = _txtMatriculaNumero2.Text;
            entityAlunoCurriculoAntigo.alc_codigoInep = _txtCensoID2.Text;
            entityAlunoCurriculoAntigo.alc_dataPrimeiraMatricula = string.IsNullOrEmpty(_txtMatriculaDataPrimeira2.Text) ? new DateTime() : Convert.ToDateTime(_txtMatriculaDataPrimeira2.Text);
            entityAlunoCurriculoAntigo.alc_dataSaida = string.IsNullOrEmpty(_txtMatriculaDataSaida2.Text) ? new DateTime() : Convert.ToDateTime(_txtMatriculaDataSaida2.Text);
            entityAlunoCurriculoAntigo.alc_dataColacao = string.IsNullOrEmpty(_txtMatriculaDataColacao2.Text) ? new DateTime() : Convert.ToDateTime(_txtMatriculaDataColacao2.Text);
            entityAlunoCurriculoAntigo.alc_matriculaEstadual = _txtMatriculaEstadual2.Text;
            entityAlunoCurriculoAntigo.alc_situacao = RetornaSituacaoAlunoCurriculo();

            if (ckbAltCurso.Checked || ckbAltTurma.Checked || ckbRecAluno.Checked || ckbTranfDentroRede.Checked)
            {
                // caso houver algum tipo de movimentação inativa o AlunoCurriculo anterior
                entityAlunoCurriculoAntigo.alc_situacao = Convert.ToByte(ACA_AlunoCurriculoSituacao.Inativo);

                // carrega os dados da nova matricula
                entityAlunoCurriculoNovo = new ACA_AlunoCurriculo();
                entityAlunoCurriculoNovo.alu_id = _VS_alu_id;
                entityAlunoCurriculoNovo.alc_id = _VS_alc_id;
                entityAlunoCurriculoNovo.esc_id = _VS_esc_id;
                entityAlunoCurriculoNovo.uni_id = _VS_uni_id;
                entityAlunoCurriculoNovo.cur_id = _VS_cur_id;
                entityAlunoCurriculoNovo.crr_id = _VS_crr_id;
                entityAlunoCurriculoNovo.crp_id = _VS_crp_id;
                entityAlunoCurriculoNovo.alc_matricula = _txtMatriculaNumero2.Text;
                entityAlunoCurriculoNovo.alc_codigoInep = _txtCensoID2.Text;
                entityAlunoCurriculoNovo.alc_situacao = RetornaSituacaoAlunoCurriculo();
                entityAlunoCurriculoNovo.alc_dataPrimeiraMatricula = string.IsNullOrEmpty(_txtMatriculaDataPrimeira2.Text) ? new DateTime() : Convert.ToDateTime(_txtMatriculaDataPrimeira2.Text);
                entityAlunoCurriculoNovo.alc_dataSaida = string.IsNullOrEmpty(_txtMatriculaDataSaida2.Text) ? new DateTime() : Convert.ToDateTime(_txtMatriculaDataSaida2.Text);
                entityAlunoCurriculoNovo.alc_dataColacao = string.IsNullOrEmpty(_txtMatriculaDataColacao2.Text) ? new DateTime() : Convert.ToDateTime(_txtMatriculaDataColacao2.Text);
                entityAlunoCurriculoNovo.alc_matriculaEstadual = _txtMatriculaEstadual2.Text;
                entityAlunoCurriculoNovo.IsNew = true;
            }
            else
                if (ckbTranfForaRede.Checked)
                {
                    entityAlunoCurriculoNovo = null;
                    entityAlunoCurriculoAntigo.alc_situacao = Convert.ToByte(ACA_AlunoCurriculoSituacao.Inativo);
                }
                else
                {
                    entityAlunoCurriculoNovo = null;
                    entityMovimentacao = null;
                }

            #endregion

            #region MatriculaTurma
            //Dados da turma antiga
            if ((_VS_tur_id_Anterior > 0 && _VS_tur_id_Anterior != _VS_tur_id)
                || UCComboAlunoSituacao1._Combo.SelectedValue == "4" || ckbTranfForaRede.Checked)
            {
                // inativa matriculaTurma anterior
                if (_VS_mtu_id_Anterior > 0)
                {
                    entityMatriculaTurmaAntigo = new MTR_MatriculaTurma();
                    entityMatriculaTurmaAntigo.alu_id = _VS_alu_id;
                    entityMatriculaTurmaAntigo.mtu_id = _VS_mtu_id_Anterior;
                    MTR_MatriculaTurmaBO.GetEntity(entityMatriculaTurmaAntigo);
                    entityMatriculaTurmaAntigo.mtu_situacao = Convert.ToByte(MTR_MatriculaTurmaSituacao.Inativo);
                }
            }
            else
            {
                entityMatriculaTurmaAntigo = null;
            }

            // Dados da nova turma
            if (_VS_tur_id != _VS_tur_id_Anterior
                && (ckbAltCurso.Checked
                || ckbAltTurma.Checked
                || ckbRecAluno.Checked
                || ckbTranfDentroRede.Checked))
            {
                entityMatriculaTurmaNovo = new MTR_MatriculaTurma();
                entityMatriculaTurmaNovo.alu_id = _VS_alu_id;
                entityMatriculaTurmaNovo.alc_id = _VS_alc_id;
                entityMatriculaTurmaNovo.tur_id = _VS_tur_id;
                entityMatriculaTurmaNovo.cur_id = _VS_cur_id;
                entityMatriculaTurmaNovo.crr_id = _VS_crr_id;
                entityMatriculaTurmaNovo.crp_id = _VS_crp_id;
                entityMatriculaTurmaNovo.mtu_dataMatricula = DateTime.Now.Date;
                entityMatriculaTurmaNovo.mtu_numeroChamada = MTR_MatriculaTurmaBO.VerificaUltimoNumeroChamada(_VS_tur_id, _VS_cur_id, _VS_crr_id, _VS_crp_id);
                entityMatriculaTurmaNovo.mtu_situacao = RetornaSituacaoMatriculaTurma();
                entityMatriculaTurmaNovo.IsNew = true;
            }
            else
                if (ckbTranfForaRede.Checked)
                {
                    entityMatriculaTurmaNovo = null;
                }
                else
                {
                    entityMatriculaTurmaNovo = null;
                    entityMovimentacao = null;
                }

            #endregion
        }
        else
            if (divNovaMovimentacao.Visible)
            {
                entityMovimentacao.tmv_idEntrada = UCComboTipoMovimentacaoEntrada.Valor;

                entityAlunoCurriculoAntigo = null;
                entityMatriculaTurmaAntigo = null;

                #region AlunoCurriculo

                // dados da matricula atual
                entityAlunoCurriculoNovo = new ACA_AlunoCurriculo();
                entityAlunoCurriculoNovo.alu_id = _VS_alu_id;
                entityAlunoCurriculoNovo.alc_id = _VS_alc_id;
                entityAlunoCurriculoNovo.esc_id = _VS_esc_id;
                entityAlunoCurriculoNovo.uni_id = _VS_uni_id;
                entityAlunoCurriculoNovo.cur_id = _VS_cur_id;
                entityAlunoCurriculoNovo.crr_id = _VS_crr_id;
                entityAlunoCurriculoNovo.crp_id = _VS_crp_id;
                entityAlunoCurriculoNovo.alc_matricula = _txtMatriculaNumero.Text;
                entityAlunoCurriculoNovo.alc_codigoInep = _txtCensoID.Text;
                entityAlunoCurriculoNovo.alc_situacao = RetornaSituacaoAlunoCurriculo();
                entityAlunoCurriculoNovo.alc_dataPrimeiraMatricula = string.IsNullOrEmpty(_txtMatriculaDataPrimeira.Text) ? new DateTime() : Convert.ToDateTime(_txtMatriculaDataPrimeira.Text);
                entityAlunoCurriculoNovo.alc_dataSaida = string.IsNullOrEmpty(_txtMatriculaDataSaida.Text) ? new DateTime() : Convert.ToDateTime(_txtMatriculaDataSaida.Text);
                entityAlunoCurriculoNovo.alc_dataColacao = string.IsNullOrEmpty(_txtMatriculaDataColacao.Text) ? new DateTime() : Convert.ToDateTime(_txtMatriculaDataColacao.Text);
                entityAlunoCurriculoNovo.alc_matriculaEstadual = _txtMatriculaEstadual.Text;
                entityAlunoCurriculoNovo.IsNew = true;

                #endregion

                #region MatriculaTurma

                if (UCComboAlunoSituacao1._Combo.SelectedValue == "1")
                {
                    // Dados da nova turma
                    entityMatriculaTurmaNovo = new MTR_MatriculaTurma();
                    entityMatriculaTurmaNovo.alu_id = _VS_alu_id;
                    entityMatriculaTurmaNovo.alc_id = _VS_alc_id;
                    entityMatriculaTurmaNovo.tur_id = _VS_tur_id;
                    entityMatriculaTurmaNovo.cur_id = _VS_cur_id;
                    entityMatriculaTurmaNovo.crr_id = _VS_crr_id;
                    entityMatriculaTurmaNovo.crp_id = _VS_crp_id;
                    entityMatriculaTurmaNovo.mtu_dataMatricula = DateTime.Now.Date;
                    entityMatriculaTurmaNovo.mtu_numeroChamada = MTR_MatriculaTurmaBO.VerificaUltimoNumeroChamada(_VS_tur_id, _VS_cur_id, _VS_crr_id, _VS_crp_id);
                    entityMatriculaTurmaNovo.mtu_situacao = RetornaSituacaoMatriculaTurma();
                    entityMatriculaTurmaNovo.IsNew = true;
                }
                else
                {
                    entityMatriculaTurmaNovo = null;
                }

                #endregion
            }
            else
            {
                entityMovimentacao.tmv_idEntrada = 0;
                entityMovimentacao.tmv_idSaida = 0;

                entityAlunoCurriculoNovo = null;
                entityMatriculaTurmaNovo = null;

                // caso seja um aluno antigo e inativo
                if (_VS_alc_id > 0 && _VS_alu_id > 0)
                {
                    // Dados da matrícula anterior                
                    entityAlunoCurriculoAntigo = new ACA_AlunoCurriculo();
                    entityAlunoCurriculoAntigo.alu_id = _VS_alu_id;
                    entityAlunoCurriculoAntigo.alc_id = _VS_alc_id;
                    ACA_AlunoCurriculoBO.GetEntity(entityAlunoCurriculoAntigo);
                    entityAlunoCurriculoAntigo.alc_situacao = RetornaSituacaoAlunoCurriculo();

                    // Dados da turma anterior
                    if (_VS_mtu_id_Anterior > 0)
                    {
                        entityMatriculaTurmaAntigo = new MTR_MatriculaTurma();
                        entityMatriculaTurmaAntigo.alu_id = _VS_alu_id;
                        entityMatriculaTurmaAntigo.mtu_id = _VS_mtu_id_Anterior;
                        MTR_MatriculaTurmaBO.GetEntity(entityMatriculaTurmaAntigo);
                        entityMatriculaTurmaAntigo.mtu_situacao = RetornaSituacaoMatriculaTurma();
                    }
                }
                else
                {
                    entityAlunoCurriculoAntigo = null;
                    entityMatriculaTurmaAntigo = null;
                    entityMovimentacao = null;
                }
            }
    }

    /// <summary>
    /// Pesquisao histórico de movimentações do aluno
    /// </summary>
    private void PesquisarMovimentacoes()
    {
        CancelaSelect = false;
        odsHistoricoMovimentacao.SelectParameters.Clear();
        odsHistoricoMovimentacao.SelectParameters.Add("alu_id", _VS_alu_id.ToString());
        odsHistoricoMovimentacao.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsHistoricoMovimentacao.SelectParameters.Add("paginado", "true");
        odsHistoricoMovimentacao.SelectParameters.Add("currentPage", "1");
        odsHistoricoMovimentacao.SelectParameters.Add("pageSize", "1");
        grvHistoricoMovimentacao.DataBind();
    }

    #endregion

    #region Histórico

    public void _LimparCamposHistorico()
    {
        _VS_IsNewHistorico = true;
        _btnIncluirHistorico.Text = "Incluir";

        UCComboTipoNivelEnsino1.Valor = -1;
        UCComboTipoNivelEnsino1.PermiteEditar = true;

        UCComboTipoModalidadeEnsino1.Valor = -1;
        _txtEscolaOrigem.Text = string.Empty;
        _txtSerie.Text = string.Empty;
        _txtAnoLetivo.Text = string.Empty;
        _ddlResultado.SelectedValue = "-1";
        _ddlTipoControle.SelectedValue = "-1";
        _ddlTipoControle.Enabled = true;
        _txtAvaliacao.Text = string.Empty;
        _txtFrequencia.Text = string.Empty;

        _LimparCamposCadastroEscolaOrigem();
        _CarregarHistoricoObservacao();

        divHistoricoNotaGlobal.Visible = false;
        fdsHistoricoDisciplinas.Visible = false;

        _txtBuscaEscolaOrigem.Text = string.Empty;

        _lblMessageHistorico.Visible = false;
    }

    private bool _ValidarHistorico()
    {
        if (UCComboTipoNivelEnsino1.Valor <= 0)
        {
            _lblMessageHistorico.Text = UtilBO.GetErroMessage("Nível de ensino é obrigatório.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        if (_txtSerie.Text.Trim() == string.Empty)
        {
            _lblMessageHistorico.Text = UtilBO.GetErroMessage("Período é obrigatório.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        if (_txtAnoLetivo.Text.Trim() == string.Empty)
        {
            _lblMessageHistorico.Text = UtilBO.GetErroMessage("Ano é obrigatório.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        if ((_ddlResultado.SelectedValue == "-1") || (_ddlResultado.SelectedValue == null))
        {
            _lblMessageHistorico.Text = UtilBO.GetErroMessage("Resultado é obrigatório.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        if ((_ddlTipoControle.SelectedValue == "-1") || (_ddlTipoControle.SelectedValue == null))
        {
            _lblMessageHistorico.Text = UtilBO.GetErroMessage("Tipo de controle é obrigatório.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        if (_ddlTipoControle.SelectedValue == "2")
        {
            if (_txtAvaliacao.Text.Trim() == string.Empty)
            {
                _lblMessageHistorico.Text = UtilBO.GetErroMessage("Avaliação é obrigatório.", UtilBO.TipoMensagem.Alerta);
                return false;
            }
        }

        _lblMessageHistorico.Visible = false;
        return true;
    }

    private void _CarregarHistorico()
    {
        _grvHistorico.DataSource = _VS_historico;
        _grvHistorico.DataBind();

        divDialogHistorico.Visible = false;
    }

    private void _IncluirHistorico()
    {
        bool existeEndereco = false;
        for (int i = 0; i < UCEnderecos2._VS_enderecos.Rows.Count; i++)
        {
            if (UCEnderecos2._VS_enderecos.Rows[i].RowState != DataRowState.Deleted)
            {
                existeEndereco = true;
                break;
            }
        }

        DataRow dr = _VS_historico.NewRow();

        dr["alh_id"] = _VS_seqHistorico;
        dr["tne_id"] = UCComboTipoNivelEnsino1.Valor;
        dr["tme_id"] = UCComboTipoModalidadeEnsino1.Valor;
        dr["eco_id"] = _VS_eco_id;
        dr["mtu_id"] = -1;
        dr["alh_serie"] = _txtSerie.Text;
        dr["alh_anoLetivo"] = _txtAnoLetivo.Text;
        dr["alh_resultado"] = _ddlResultado.SelectedValue == "-1" ? Convert.ToByte(0) : Convert.ToByte(_ddlResultado.SelectedValue);
        dr["alh_avaliacao"] = _txtAvaliacao.Text;
        dr["alh_frequencia"] = _txtFrequencia.Text;
        dr["tne_nome"] = UCComboTipoModalidadeEnsino1.Valor > 0 ? UCComboTipoNivelEnsino1.Texto + " - " + UCComboTipoModalidadeEnsino1.Texto : UCComboTipoNivelEnsino1.Texto;
        dr["tme_nome"] = UCComboTipoModalidadeEnsino1.Valor > 0 ? UCComboTipoModalidadeEnsino1.Texto : string.Empty;
        dr["eco_nome"] = _txtEscolaOrigem.Text;
        dr["alh_resultadoDescricao"] = _ddlResultado.SelectedItem.ToString();
        dr["tre_id"] = UCComboTipoRedeEnsino1.Valor;
        dr["eco_codigoInep"] = _txtCodigoInepEscolaOrigem.Text;
        dr["end_id"] = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["end_id"] : Guid.Empty;
        dr["eco_numero"] = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["numero"] : string.Empty;
        dr["eco_complemento"] = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["complemento"] : string.Empty;
        dr["end_cep"] = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["end_cep"] : string.Empty;
        dr["end_logradouro"] = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["end_logradouro"] : string.Empty;
        dr["end_zona"] = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["end_zona"] : 0;
        dr["end_distrito"] = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["end_distrito"] : string.Empty;
        dr["end_bairro"] = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["end_bairro"] : string.Empty;
        dr["cid_id"] = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["cid_id"] : Guid.Empty;
        dr["cid_nome"] = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["cid_nome"] : string.Empty;

        _VS_historico.Rows.Add(dr);

        _CarregarHistorico();
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroHistoricoLoadFechar", "$('#divHistorico').dialog('close');", true);
    }

    private void _AlterarHistoricoGrid()
    {
        divDialogHistorico.Visible = true;
        _VS_IsNewHistorico = false;
        _btnIncluirHistorico.Text = "Alterar";
        UCComboTipoNivelEnsino1.PermiteEditar = false;

        for (int i = 0; i < _VS_historico.Rows.Count; i++)
        {
            if (_VS_historico.Rows[i].RowState != DataRowState.Deleted)
            {
                if (_VS_historico.Rows[i]["alh_id"].ToString() == _VS_idHistorico.ToString())
                {
                    UCComboTipoNivelEnsino1.Valor = Convert.ToInt32(_VS_historico.Rows[i]["tne_id"].ToString());
                    UCComboTipoModalidadeEnsino1.Valor = Convert.ToInt32(_VS_historico.Rows[i]["tme_id"].ToString());
                    _txtEscolaOrigem.Text = _VS_historico.Rows[i]["eco_nome"].ToString();
                    _txtSerie.Text = _VS_historico.Rows[i]["alh_serie"].ToString();
                    _txtAnoLetivo.Text = _VS_historico.Rows[i]["alh_anoLetivo"].ToString();
                    _ddlResultado.SelectedValue = _VS_historico.Rows[i]["alh_resultado"].ToString();

                    if (string.IsNullOrEmpty(_VS_historico.Rows[i]["alh_avaliacao"].ToString()))
                    {
                        _ddlTipoControle.SelectedValue = "1";
                        divHistoricoNotaGlobal.Visible = false;
                        fdsHistoricoDisciplinas.Visible = true;

                        _txtAvaliacao.Text = string.Empty;
                        _txtFrequencia.Text = string.Empty;

                        _CarregarHistoricoDisciplina();
                    }
                    else
                    {
                        _ddlTipoControle.SelectedValue = "2";
                        divHistoricoNotaGlobal.Visible = true;
                        fdsHistoricoDisciplinas.Visible = false;

                        _txtAvaliacao.Text = _VS_historico.Rows[i]["alh_avaliacao"].ToString();
                        _txtFrequencia.Text = _VS_historico.Rows[i]["alh_frequencia"].ToString();

                        _CarregarHistoricoDisciplina();
                    }

                    _CarregarHistoricoObservacao();

                    UCComboTipoRedeEnsino1.Valor = Convert.ToInt32(_VS_historico.Rows[i]["tre_id"].ToString());
                    _txtNomeEscolaOrigem.Text = _VS_historico.Rows[i]["eco_nome"].ToString();
                    _txtCodigoInepEscolaOrigem.Text = _VS_historico.Rows[i]["eco_codigoInep"].ToString();

                    UCEnderecos2.CarregarEndereco(new Guid(_VS_historico.Rows[i]["end_id"].ToString()), _VS_historico.Rows[i]["eco_numero"].ToString(), _VS_historico.Rows[i]["eco_complemento"].ToString());

                    _VS_eco_id = Convert.ToInt32(_VS_historico.Rows[i]["eco_id"].ToString());


                    break;
                }
            }
        }

        UCComboTipoRedeEnsino1.SetarFoco();
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroHistoricoAlterar", "$('#divHistorico').dialog('open');", true);
    }

    private void _AlterarHistorico()
    {
        bool existeEndereco = false;
        for (int i = 0; i < UCEnderecos2._VS_enderecos.Rows.Count; i++)
        {
            if (UCEnderecos2._VS_enderecos.Rows[i].RowState != DataRowState.Deleted)
            {
                existeEndereco = true;
                break;
            }
        }

        for (int i = 0; i < _VS_historico.Rows.Count; i++)
        {
            if (_VS_historico.Rows[i].RowState != DataRowState.Deleted)
            {
                if (_VS_historico.Rows[i]["alh_id"].ToString() == _VS_idHistorico.ToString())
                {
                    _VS_historico.Rows[i]["tme_id"] = UCComboTipoModalidadeEnsino1.Valor;
                    _VS_historico.Rows[i]["eco_id"] = _VS_eco_id;
                    _VS_historico.Rows[i]["mtu_id"] = -1;
                    _VS_historico.Rows[i]["alh_serie"] = _txtSerie.Text;
                    _VS_historico.Rows[i]["alh_anoLetivo"] = _txtAnoLetivo.Text;
                    _VS_historico.Rows[i]["alh_resultado"] = _ddlResultado.SelectedValue == "-1" ? Convert.ToByte(0) : Convert.ToByte(_ddlResultado.SelectedValue);
                    _VS_historico.Rows[i]["alh_avaliacao"] = _txtAvaliacao.Text;
                    _VS_historico.Rows[i]["alh_frequencia"] = _txtFrequencia.Text;
                    _VS_historico.Rows[i]["tne_nome"] = UCComboTipoModalidadeEnsino1.Valor > 0 ? UCComboTipoNivelEnsino1.Texto + " - " + UCComboTipoModalidadeEnsino1.Texto : UCComboTipoNivelEnsino1.Texto;
                    _VS_historico.Rows[i]["tme_nome"] = UCComboTipoModalidadeEnsino1.Valor > 0 ? UCComboTipoModalidadeEnsino1.Texto : string.Empty;
                    _VS_historico.Rows[i]["eco_nome"] = _txtEscolaOrigem.Text;
                    _VS_historico.Rows[i]["alh_resultadoDescricao"] = _ddlResultado.SelectedItem.ToString();
                    _VS_historico.Rows[i]["tre_id"] = UCComboTipoRedeEnsino1.Valor;
                    _VS_historico.Rows[i]["eco_codigoInep"] = _txtCodigoInepEscolaOrigem.Text;
                    _VS_historico.Rows[i]["end_id"] = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["end_id"] : Guid.Empty;
                    _VS_historico.Rows[i]["eco_numero"] = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["numero"] : string.Empty;
                    _VS_historico.Rows[i]["eco_complemento"] = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["complemento"] : string.Empty;
                    _VS_historico.Rows[i]["end_cep"] = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["end_cep"] : string.Empty;
                    _VS_historico.Rows[i]["end_logradouro"] = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["end_logradouro"] : string.Empty;
                    _VS_historico.Rows[i]["end_zona"] = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["end_zona"] : 0;
                    _VS_historico.Rows[i]["end_distrito"] = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["end_distrito"] : string.Empty;
                    _VS_historico.Rows[i]["end_bairro"] = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["end_bairro"] : string.Empty;
                    _VS_historico.Rows[i]["cid_id"] = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["cid_id"] : Guid.Empty;
                    _VS_historico.Rows[i]["cid_nome"] = existeEndereco ? UCEnderecos2._VS_enderecos.Rows[0]["cid_nome"] : string.Empty;

                    _CarregarHistorico();
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroHistoricoLoadFechar", "$('#divHistorico').dialog('close');", true);

                    break;
                }
            }
        }
    }

    private void _ExcluirHistorico()
    {
        for (int i = 0; i < _VS_historico.Rows.Count; i++)
        {
            if (_VS_historico.Rows[i].RowState != DataRowState.Deleted)
            {
                if (_VS_historico.Rows[i]["alh_id"].ToString() == _VS_idHistorico.ToString())
                {
                    _VS_historico.Rows[i].Delete();
                    break;
                }
            }
        }

        for (int i = 0; i < _VS_disciplina.Rows.Count; i++)
        {
            if (_VS_disciplina.Rows[i].RowState != DataRowState.Deleted)
            {
                if (_VS_disciplina.Rows[i]["alh_id"].ToString() == _VS_idHistorico.ToString())
                {
                    _VS_disciplina.Rows[i].Delete();
                }
            }
        }

        _CarregarHistorico();
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroHistoricoLoadFechar", "$('#divHistorico').dialog('close');", true);
    }

    #endregion

    #region Escola Origem

    private void _LimparCamposCadastroEscolaOrigem()
    {
        UCComboTipoRedeEnsino1.Valor = -1;
        _txtNomeEscolaOrigem.Text = string.Empty;
        _txtCodigoInepEscolaOrigem.Text = string.Empty;

        _txtEscolaOrigem.Text = string.Empty;

    }

    private bool _ValidarCadastroEscolaOrigem()
    {
        if (_txtNomeEscolaOrigem.Text.Trim() == string.Empty)
        {
            _lblMessageEscolaOrigem.Text = UtilBO.GetErroMessage("Escola de origem é obrigatório.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        _lblMessageEscolaOrigem.Visible = false;
        return true;
    }

    private void _PesquisarEscolaOrigem()
    {
        try
        {
            _grvEscolaOrigem.PageIndex = 0;
            odsEscolaOrigem.SelectParameters.Clear();            
            odsEscolaOrigem.SelectParameters.Add("eco_id", "0");
            odsEscolaOrigem.SelectParameters.Add("tre_id", "0");
            odsEscolaOrigem.SelectParameters.Add("eco_nome", _txtBuscaEscolaOrigem.Text.Trim());
            odsEscolaOrigem.SelectParameters.Add("eco_situacao", "0");
            odsEscolaOrigem.SelectParameters.Add("paginado", "true");
            odsEscolaOrigem.DataBind();
            _grvEscolaOrigem.DataBind();

        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar as escolas de origem.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion

    #region Disciplinas

    /// <summary>
    /// Métodos criado para Inclusão de disciplina
    /// </summary>
    public void _LimparCamposDisciplina()
    {
        _VS_IsNewDisciplina = true;
        _btnIncluirDisciplina.Text = "Incluir";

        UCComboTipoDisciplina1.Valor = -1;
        _txtDisciplina.Text = string.Empty;
        _ddlResultadoDisciplina.SelectedValue = "-1";
        _txtAvaliacaoDisciplina.Text = string.Empty;
        _txtFrequenciaDisciplina.Text = string.Empty;

        _lblMessageDisciplina.Visible = false;
    }

    private bool _ValidarDisciplina()
    {
        if ((_ddlResultadoDisciplina.SelectedValue == "-1") || (_ddlResultadoDisciplina.SelectedValue == null))
        {
            _lblMessageDisciplina.Text = UtilBO.GetErroMessage("Resultado é obrigatório.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        if (_txtDisciplina.Text.Trim() == string.Empty)
        {
            _lblMessageDisciplina.Text = UtilBO.GetErroMessage("Disciplina é obrigatório.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        if (_ValidarDisciplinaExistenteHistorico())
        {
            _lblMessageDisciplina.Text = UtilBO.GetErroMessage("Já existe uma disciplina cadastrada com este nome.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        _lblMessageDisciplina.Visible = false;
        return true;
    }

    private void _CarregarHistoricoDisciplina()
    {
        if (_VS_IsNewHistorico)
        {
            DataView dv = _VS_disciplina.DefaultView;
            dv.RowFilter = "alh_id = " + _VS_seqHistorico;

            _grvHistoricoDisciplinas.DataSource = dv;
            _grvHistoricoDisciplinas.DataBind();
        }
        else
        {
            DataView dv = _VS_disciplina.DefaultView;
            dv.RowFilter = "alh_id = " + _VS_idHistorico;

            _grvHistoricoDisciplinas.DataSource = dv;
            _grvHistoricoDisciplinas.DataBind();
        }

        _ddlTipoControle.Enabled = _grvHistoricoDisciplinas.Rows.Count <= 0;

        divDialogDisciplina.Visible = false;
    }

    private bool _ValidarDisciplinaExistenteHistorico()
    {
        int alh_id = _VS_IsNewHistorico ? _VS_seqHistorico : _VS_idHistorico;

        //Valida as disciplinas inseridas no historico, evitando a repetiçao de disciplinas do mesmo tipo e nome

        if (_VS_disciplina.Rows.Count > 0)
        {
            if (_VS_IsNewDisciplina)
            {
                for (int i = 0; i < _VS_disciplina.Rows.Count; i++)
                {
                    if (_VS_disciplina.Rows[i].RowState != DataRowState.Deleted)
                    {
                        if (_VS_disciplina.Rows[i]["alh_id"].ToString() == alh_id.ToString() && Convert.ToInt32(_VS_disciplina.Rows[i]["tds_id"].ToString()) == UCComboTipoDisciplina1.Valor
                           && _VS_disciplina.Rows[i]["ahd_disciplina"].ToString() == _txtDisciplina.Text)
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < _VS_disciplina.Rows.Count; i++)
                {
                    if (_VS_disciplina.Rows[i].RowState != DataRowState.Deleted)
                    {
                        if (_VS_disciplina.Rows[i]["ahd_id"].ToString() != _VS_idDisciplina.ToString() && _VS_disciplina.Rows[i]["ahd_disciplina"].ToString().Equals(_txtDisciplina.Text))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        return false;
    }

    private void _IncluirDisciplina()
    {
        DataRow dr = _VS_disciplina.NewRow();

        if (_VS_seqDisciplina == -1)
            _VS_seqDisciplina = 1;
        else
            _VS_seqDisciplina = _VS_seqDisciplina + 1;

        dr["alh_id"] = _VS_IsNewHistorico ? _VS_seqHistorico : _VS_idHistorico;

        dr["ahd_id"] = _VS_seqDisciplina;
        dr["tds_id"] = UCComboTipoDisciplina1.Valor;
        dr["ahd_disciplina"] = _txtDisciplina.Text;
        dr["ahd_resultado"] = Convert.ToInt32(_ddlResultadoDisciplina.SelectedValue);
        dr["ahd_avaliacao"] = _txtAvaliacaoDisciplina.Text;
        dr["ahd_frequencia"] = _txtFrequenciaDisciplina.Text;
        dr["tds_nome"] = UCComboTipoDisciplina1.Valor > 0 ? UCComboTipoDisciplina1.Texto : string.Empty;
        dr["ahd_resultadoDescricao"] = _ddlResultadoDisciplina.SelectedItem.ToString();

        _VS_disciplina.Rows.Add(dr);

        _CarregarHistoricoDisciplina();
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroDisciplinaSalvarFechar", "$('#divDisciplina').dialog('close');", true);
    }

    private void _AlterarDisciplinaGrid()
    {
        _VS_IsNewDisciplina = false;
        _btnIncluirDisciplina.Text = "Alterar";

        for (int i = 0; i < _VS_disciplina.Rows.Count; i++)
        {
            if (_VS_disciplina.Rows[i].RowState != DataRowState.Deleted)
            {
                if (_VS_disciplina.Rows[i]["alh_id"].ToString() == _VS_idHistorico.ToString() && _VS_disciplina.Rows[i]["ahd_id"].ToString() == _VS_idDisciplina.ToString())
                {
                    UCComboTipoDisciplina1.Valor = Convert.ToInt32(_VS_disciplina.Rows[i]["tds_id"].ToString());
                    _txtDisciplina.Text = _VS_disciplina.Rows[i]["ahd_disciplina"].ToString();
                    _ddlResultadoDisciplina.SelectedValue = _VS_disciplina.Rows[i]["ahd_resultado"].ToString();
                    _txtAvaliacaoDisciplina.Text = _VS_disciplina.Rows[i]["ahd_avaliacao"].ToString();
                    _txtFrequenciaDisciplina.Text = _VS_disciplina.Rows[i]["ahd_frequencia"].ToString();

                    break;
                }
            }
        }

        UCComboTipoDisciplina1.SetarFoco();
        divDialogDisciplina.Visible = true;
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroDisciplinaAlterar", "$('#divDisciplina').dialog('open');", true);
    }

    private void _AlterarDisciplina()
    {
        for (int i = 0; i < _VS_disciplina.Rows.Count; i++)
        {
            if (_VS_disciplina.Rows[i].RowState != DataRowState.Deleted)
            {
                if (_VS_disciplina.Rows[i]["alh_id"].ToString() == _VS_idHistorico.ToString() && _VS_disciplina.Rows[i]["ahd_id"].ToString() == _VS_idDisciplina.ToString())
                {
                    _VS_disciplina.Rows[i]["tds_id"] = UCComboTipoDisciplina1.Valor;
                    _VS_disciplina.Rows[i]["ahd_disciplina"] = _txtDisciplina.Text;
                    _VS_disciplina.Rows[i]["ahd_resultado"] = Convert.ToInt32(_ddlResultadoDisciplina.SelectedValue);
                    _VS_disciplina.Rows[i]["ahd_avaliacao"] = _txtAvaliacaoDisciplina.Text;
                    _VS_disciplina.Rows[i]["ahd_frequencia"] = _txtFrequenciaDisciplina.Text;
                    _VS_disciplina.Rows[i]["tds_nome"] = UCComboTipoDisciplina1.Valor > 0 ? UCComboTipoDisciplina1.Texto : string.Empty;
                    _VS_disciplina.Rows[i]["ahd_resultadoDescricao"] = _ddlResultadoDisciplina.SelectedItem.ToString();

                    _CarregarHistoricoDisciplina();
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroDisciplinaSalvarFechar", "$('#divDisciplina').dialog('close');", true);

                    break;
                }
            }
        }
    }

    private void _ExcluirDisciplina()
    {
        for (int i = 0; i < _VS_disciplina.Rows.Count; i++)
        {
            if (_VS_disciplina.Rows[i].RowState != DataRowState.Deleted)
            {
                if (_VS_disciplina.Rows[i]["alh_id"].ToString() == _VS_idHistorico.ToString() && _VS_disciplina.Rows[i]["ahd_id"].ToString() == _VS_idDisciplina.ToString())
                {
                    _VS_disciplina.Rows[i].Delete();
                    break;
                }
            }
        }

        _CarregarHistoricoDisciplina();
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroDisciplinaSalvarFechar", "$('#divDisciplina').dialog('close');", true);
    }

    #endregion

    #region Observação do histórico

    /// <summary>
    /// Métodos criado para Histórico de observações
    /// </summary>
    public void _LimparCamposHistoricoObservacao()
    {
        _VS_IsNewHistoricoObservacao = true;
        _btnIncluirHistoricoObservacao.Text = "Incluir";

        UCComboAlunoHistorico.Valor = -1;
        _txtObservacao1.Text = string.Empty;
        _txtObservacao1.Enabled = true;

        _lblMessageHistoricoObservacao.Visible = false;
    }

    private bool _ValidarHistoricoObservacao()
    {

        if ((_txtObservacao1.Text.Trim() == string.Empty) && (UCComboAlunoHistorico.Valor <= 0))
        {
            _lblMessageHistoricoObservacao.Text = UtilBO.GetErroMessage("É necessário selecionar uma observação padrão ou informar uma observação.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        if (_ValidarHistoricoObservacaoExistenteHistorico())
        {
            _lblMessageHistoricoObservacao.Text = UtilBO.GetErroMessage("Essa observação já foi cadastrada.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        if (_txtObservacao1.Text.Length >= 1000)
        {
            _lblMessageHistoricoObservacao.Text = UtilBO.GetErroMessage("Observação do histórico pode conter até 1000 caracteres.", UtilBO.TipoMensagem.Alerta);
            return false;
        }

        _lblMessageDisciplina.Visible = false;
        return true;
    }

    private void _CarregarHistoricoObservacao()
    {
        if (_VS_IsNewHistorico)
        {
            DataView dv = _VS_historicoObservacao.DefaultView;
            dv.RowFilter = "alh_id = " + _VS_seqHistorico;

            _grvHistoricoObservacoes.DataSource = dv;
            _grvHistoricoObservacoes.DataBind();
        }
        else
        {
            DataView dv = _VS_historicoObservacao.DefaultView;
            dv.RowFilter = "alh_id = " + _VS_idHistorico;

            _grvHistoricoObservacoes.DataSource = dv;
            _grvHistoricoObservacoes.DataBind();
        }

        divDialogHistoricoObservacao.Visible = false;
    }

    private bool _ValidarHistoricoObservacaoExistenteHistorico()
    {
        int alh_id = _VS_IsNewHistorico ? _VS_seqHistorico : _VS_idHistorico;

        if (_VS_historicoObservacao.Rows.Count > 0)
        {
            for (int i = 0; i < _VS_historicoObservacao.Rows.Count; i++)
            {
                if (_VS_historicoObservacao.Rows[i].RowState != DataRowState.Deleted)
                {
                    if ((_VS_historicoObservacao.Rows[i]["alh_id"].ToString().Equals(alh_id.ToString()))
                        && (_VS_historicoObservacao.Rows[i]["hop_id"].ToString().Equals(UCComboAlunoHistorico.Valor.ToString()))
                        && (_VS_historicoObservacao.Rows[i]["aho_id"].ToString() == _VS_idHistoricoObservacao.ToString()))
                    {
                        return false;
                    }

                    if (_VS_historicoObservacao.Rows[i]["alh_id"].ToString().Equals(alh_id.ToString()) && _VS_historicoObservacao.Rows[i]["hop_id"].ToString().Equals(UCComboAlunoHistorico.Valor.ToString()))
                    {
                        return true;
                    }

                }
            }
            return false;
        }

        return false;
    }

    private void _IncluirHistóricoObservacao()
    {
        DataRow dr = _VS_historicoObservacao.NewRow();

        if (_VS_seqHistoricoObservacao == 0)
            _VS_seqHistoricoObservacao = 1;
        else
            _VS_seqHistoricoObservacao = _VS_seqHistoricoObservacao + 1;

        dr["alh_id"] = _VS_IsNewHistorico ? _VS_seqHistorico : _VS_idHistorico;

        dr["aho_id"] = _VS_seqHistoricoObservacao;

        dr["aho_descricao"] = UCComboAlunoHistorico.Valor == -1 ? _txtObservacao1.Text : UCComboAlunoHistorico.Texto;

        dr["hop_id"] = UCComboAlunoHistorico.Valor;

        _VS_historicoObservacao.Rows.Add(dr);

        _CarregarHistoricoObservacao();
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroHistoricoObservacaoLoadFechar", "$('#divObservacao').dialog('close');", true);
    }

    private void _AlterarHistoricoObservacaoGrid()
    {
        _VS_IsNewHistoricoObservacao = false;
        _btnIncluirHistoricoObservacao.Text = "Alterar";

        for (int i = 0; i < _VS_historicoObservacao.Rows.Count; i++)
        {
            if (_VS_historicoObservacao.Rows[i].RowState != DataRowState.Deleted)
            {
                if ((_VS_historicoObservacao.Rows[i]["alh_id"].ToString() == _VS_idHistorico.ToString()) && (_VS_historicoObservacao.Rows[i]["aho_id"].ToString() == _VS_idHistoricoObservacao.ToString()))
                {
                    if (_VS_historicoObservacao.Rows[i]["hop_id"].ToString() == "-1")
                    {
                        _txtObservacao1.Text = _VS_historicoObservacao.Rows[i]["aho_descricao"].ToString();
                        UCComboAlunoHistorico.Valor = -1;

                        _txtObservacao1.Enabled = true;
                    }
                    else
                    {
                        UCComboAlunoHistorico.Valor = Convert.ToInt32(_VS_historicoObservacao.Rows[i]["hop_id"].ToString());
                        _txtObservacao1.Text = string.Empty;

                        _txtObservacao1.Enabled = false;
                    }

                    break;
                }
            }
        }

        UCComboAlunoHistorico._Combo.Focus();
        divDialogHistoricoObservacao.Visible = true;
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroHistoricoObservacao", "$('#divObservacao').dialog('open');", true);
    }

    private void _AlterarHistoricoObservacao()
    {

        for (int j = 0; j < _VS_historicoObservacao.Rows.Count; j++)
        {
            if (_VS_historicoObservacao.Rows[j].RowState != DataRowState.Deleted)
            {
                if (_VS_historicoObservacao.Rows[j]["alh_id"].ToString() == _VS_idHistorico.ToString() && _VS_historicoObservacao.Rows[j]["aho_id"].ToString() == _VS_idHistoricoObservacao.ToString())
                {
                    _VS_historicoObservacao.Rows[j]["aho_descricao"] = UCComboAlunoHistorico.Valor == -1 ? _txtObservacao1.Text : UCComboAlunoHistorico.Texto;
                    _VS_historicoObservacao.Rows[j]["hop_id"] = UCComboAlunoHistorico.Valor;

                    _CarregarHistoricoObservacao();
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroHistoricoObservacaoSalvarFechar", "$('#divObservacao').dialog('close');", true);

                    break;
                }
            }
        }
    }

    private void _ExcluirHistoricoObservacao()
    {
        for (int i = 0; i < _VS_historicoObservacao.Rows.Count; i++)
        {
            if (_VS_historicoObservacao.Rows[i].RowState != DataRowState.Deleted)
            {
                if (_VS_historicoObservacao.Rows[i]["alh_id"].ToString() == _VS_idHistorico.ToString() && _VS_historicoObservacao.Rows[i]["aho_id"].ToString() == _VS_idHistoricoObservacao.ToString())
                {
                    _VS_historicoObservacao.Rows[i].Delete();
                    break;
                }
            }
        }

        _CarregarHistoricoObservacao();
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroHistoricoObservacaoSalvarFechar", "$('#divObservacao').dialog('close');", true);
    }

    #endregion

    #endregion

    #region Navegação da tela

    /// <summary>
    /// Carrega os dados na tela.
    /// </summary>
    private void LoadDadosTela()
    {
        ACA_Aluno alu = new ACA_Aluno();
        PES_Pessoa pes = new PES_Pessoa();

        if (_VS_alu_id > 0)
        {
            // Carregar dados do aluno.
            alu.alu_id = _VS_alu_id;
            ACA_AlunoBO.GetEntity(alu);

            VS_dataAlteracaoAluno = alu.alu_dataAlteracao;

            // Verifica se usuário logado pertence à mesma entidade do aluno, caso não seja, não
            // é permitido editar.
            if (alu.ent_id != Ent_id)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("O aluno não pertence à entidade na qual você está logado.", UtilBO.TipoMensagem.Alerta);

                Response.Redirect("~/Academico/Aluno/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            pes.pes_id = alu.pes_id;
            PES_PessoaBO.GetEntity(pes);
        }

        LoadDadosPessoais(alu, pes);
        LoadEnderecos(alu);
        LoadDocumentos(alu);
        CarregaAbaMovimentacao();
        LoadHistorico();
        LoadFichaMedica(alu);
        LoadUsuario();
        LoadProgramasSociais();
    }

    /// <summary>
    /// Carrega os programas sociais do aluno, para inclusão / alteração.
    /// </summary>
    private void LoadProgramasSociais()
    {
        cblProgramaSocial.DataBind();
        List<ACA_AlunoProgramaSocial> li = ACA_AlunoProgramaSocialBO.GetSelectBy_Aluno(_VS_alu_id);
        foreach (ListItem item in cblProgramaSocial.Items)
        {
            int pso_id = Convert.ToInt32(item.Value);
            item.Selected = li.Exists(p => p.pso_id == pso_id);
        }

        if (cblProgramaSocial.Items.Count == 0)
        {
            fsProgramaSocial.Visible = false;
        }
    }

    /// <summary>
    /// Carrega os dados pessoais do aluno, para inclusão / alteração.
    /// </summary>
    /// <returns></returns>
    private void LoadDadosPessoais(ACA_Aluno alu, PES_Pessoa pes)
    {
        UCCadastroPessoa1._LabelEscolaridade.Visible = false;
        UCCadastroPessoa1._ComboEscolaridade.Visible = false;

        try
        {
            UCCadastroPessoa1.visibleMae = false;
            UCCadastroPessoa1.visiblePai = false;

            if (_VS_alu_id > 0)
            {
                // Exibe dados do Aluno.
                UCComboAlunoSituacao1._Combo.SelectedValue = alu.alu_situacao.ToString();
                _txtObservacao.Text = alu.alu_observacao;
                UCComboReligiao.Valor = alu.rlg_id > 0 ? alu.rlg_id : -1;
                _ddlMeioTransporte.SelectedValue = alu.alu_meioTransporte > 0 ? alu.alu_meioTransporte.ToString() : "-1";
                _ddlTempoDeslocamento.SelectedValue = alu.alu_tempoDeslocamento > 0 ? alu.alu_tempoDeslocamento.ToString() : "-1";
                chkRegressaSozinho.Checked = alu.alu_regressaSozinho;
                chkDadosIncompletos.Checked = alu.alu_dadosIncompletos;
                chkHistoricoEscolarIncompleto.Checked = alu.alu_historicoEscolaIncompleto;

                //Exibe o tipo de atendimento especial
                DataTable atendimentoEspecial = ACA_AlunoAtendimentoEspecialBO.GetSelectBy_alu_id(_VS_alu_id);
                UCComboTipoAtendimentoEspecial1.Valor = atendimentoEspecial.Rows.Count > 0 ? Convert.ToInt32(atendimentoEspecial.Rows[0]["tae_id"].ToString()) : -1;
                _VS_pes_id = alu.pes_id;

                _VS_pes_foto = pes.pes_foto;
                UCCadastroPessoa1._imgFoto.ImageUrl = __SessionWEB._AreaAtual._Diretorio + "Academico/RecursosHumanos/Colaborador/Imagem.ashx?id=" + pes.pes_id;

                // Exibe imagem caso exista.
                if (pes.pes_foto != null && pes.pes_foto.Length > 1)
                {
                    System.Drawing.Image img;
                    using (MemoryStream ms = new MemoryStream(pes.pes_foto, 0, pes.pes_foto.Length))
                    {
                        ms.Write(pes.pes_foto, 0, pes.pes_foto.Length);
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


                    UCCadastroPessoa1._imgFoto.Height = alt;
                    UCCadastroPessoa1._imgFoto.Width = lar;
                    UCCadastroPessoa1._imgFoto.Visible = true;
                    UCCadastroPessoa1._chbExcluirImagem.Visible = true;
                    UCCadastroPessoa1._chbExcluirImagem.Checked = false;
                }
                else
                {
                    UCCadastroPessoa1._imgFoto.Visible = false;
                    UCCadastroPessoa1._chbExcluirImagem.Visible = false;
                }

                UCCadastroPessoa1._VS_pes_id = alu.pes_id;
                UCCadastroPessoa1._txtNome.Text = pes.pes_nome;
                UCCadastroPessoa1._txtNomeAbreviado.Text = (!string.IsNullOrEmpty(pes.pes_nome_abreviado) ? pes.pes_nome_abreviado : "");

                //Exibe cidade naturalidade da pessoa
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

                //Exibe dados gerais da pessoa
                UCCadastroPessoa1._txtDataNasc.Text = (pes.pes_dataNascimento != new DateTime()) ? pes.pes_dataNascimento.ToString("dd/MM/yyyy") : "";
                UCCadastroPessoa1._ComboEstadoCivil.SelectedValue = (pes.pes_estadoCivil > 0 ? pes.pes_estadoCivil.ToString() : "-1");
                UCCadastroPessoa1._ComboSexo.SelectedValue = pes.pes_sexo.ToString();

                UCCadastroPessoa1._ComboNacionalidade.SelectedValue = (pes.pai_idNacionalidade != Guid.Empty ? pes.pai_idNacionalidade.ToString() : Guid.Empty.ToString());
                UCCadastroPessoa1._chkNaturalizado.Checked = pes.pes_naturalizado;
                UCCadastroPessoa1._ComboRacaCor.SelectedValue = (pes.pes_racaCor > 0 ? pes.pes_racaCor.ToString() : "-1");
                UCCadastroPessoa1._VS_pes_idFiliacaoPai = pes.pes_idFiliacaoPai;
                UCCadastroPessoa1._VS_pes_idFiliacaoMae = pes.pes_idFiliacaoMae;
                UCCadastroPessoa1._ComboEscolaridade.SelectedValue = (pes.tes_id != Guid.Empty ? pes.tes_id.ToString() : Guid.Empty.ToString());

                //Carregar tipo de deficiência cadastrada para a pessoa
                DataTable dtPessoaDeficiencia = PES_PessoaDeficienciaBO.GetSelect(pes.pes_id, false, 1, 1);
                if (dtPessoaDeficiencia.Rows.Count > 0)
                    UCCadastroPessoa1._ComboTipoDeficiencia.SelectedValue = dtPessoaDeficiencia.Rows[0]["tde_id"].ToString();

                //Armazena os os id's antigos em ViewState
                _VS_pai_idAntigo = pes.pai_idNacionalidade;
                _VS_cid_idAntigo = pes.cid_idNaturalidade;
                _VS_pes_idPaiAntigo = pes.pes_idFiliacaoPai;
                _VS_pes_idMaeAntigo = pes.pes_idFiliacaoMae;
                _VS_tes_idAntigo = pes.tes_id;
                _VS_tde_idAntigo = dtPessoaDeficiencia.Rows.Count > 0 ? new Guid(dtPessoaDeficiencia.Rows[0]["tde_id"].ToString()) : Guid.Empty;

                //Exibe dados do pai da pessoa
                PES_Pessoa pesFiliacaoPai = new PES_Pessoa { pes_id = pes.pes_idFiliacaoPai };
                PES_PessoaBO.GetEntity(pesFiliacaoPai);
                UCCadastroPessoa1._txtPai.Text = pesFiliacaoPai.pes_nome;

                //Exibe dados da mae da pessoa
                PES_Pessoa pesFiliacaoMae = new PES_Pessoa { pes_id = pes.pes_idFiliacaoMae };
                PES_PessoaBO.GetEntity(pesFiliacaoMae);
                UCCadastroPessoa1._txtMae.Text = pesFiliacaoMae.pes_nome;

                // Carrega dados dos responsáveis pelo aluno.
                List<ACA_AlunoResponsavelBO.StructCadastro> lista = ACA_AlunoResponsavelBO.RetornaResponsaveisAluno(_VS_alu_id, null);

                foreach (ACA_AlunoResponsavelBO.StructCadastro item in lista)
                {
                    if (item.entAlunoResp != null)
                    {
                        if (item.entAlunoResp.tra_id == TipoResponsavelAlunoParametro.tra_idMae)
                        {
                            ucResponsavelMae.CarregarResponsavel(item);
                        }
                        else if (item.entAlunoResp.tra_id == TipoResponsavelAlunoParametro.tra_idPai)
                        {
                            ucResponsavelPai.CarregarResponsavel(item);
                        }
                        else
                        {
                            ucResponsavelOutro.CarregarResponsavel(item);
                        }
                    }
                }

                ACA_AlunoResponsavelBO.StructCadastro principal =
                    lista.Find(p => p.entAlunoResp.alr_principal);

                if (principal.entAlunoResp != null)
                {
                    ucComboTipoResponsavel.Valor = principal.entAlunoResp.tra_id;
                }

                MostraResponsavelTipo(false);
            }
            else
            {
                string parametro = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.PAIS_PADRAO_BRASIL);
                if (!String.IsNullOrEmpty(parametro))
                    UCCadastroPessoa1._ComboNacionalidade.SelectedValue = parametro.ToLower();

                if (__SessionWEB._cid_id != Guid.Empty)
                {
                    UCCadastroPessoa1._VS_cid_id = __SessionWEB._cid_id;
                    END_Cidade cid = new END_Cidade { cid_id = UCCadastroPessoa1._VS_cid_id };
                    END_CidadeBO.GetEntity(cid);
                    UCCadastroPessoa1._txtNaturalidade.Text = cid.cid_nome;
                }

                UCCadastroPessoa1._ComboEstadoCivil.SelectedValue = "1";
                UCComboAlunoSituacao1._Combo.SelectedValue = "0";
            }

            return;
        }
        catch (ThreadAbortException)
        {
            return;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o aluno.", UtilBO.TipoMensagem.Erro);
            return;
        }
    }

    /// <summary>
    /// Carrega os dados da aba de Endereços.
    /// </summary>
    /// <returns></returns>
    private void LoadEnderecos(ACA_Aluno alu)
    {
        try
        {
            if (_VS_alu_id > 0)
            {
                // Carrega dados dos endereços da pessoa.
                DataTable dtEndereco = PES_PessoaEnderecoBO.GetSelect(alu.pes_id, false, 1, 1);

                UCEnderecos1.CarregarEnderecosBanco(dtEndereco);

                // Carrega dados dos contatos da pessoa.
                DataTable dtContato = PES_PessoaContatoBO.GetSelect(alu.pes_id, false, 1, 1);

                if (dtContato.Rows.Count == 0)
                    dtContato = null;
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o aluno.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Carrega os dados da aba Documentação.
    /// </summary>
    /// <returns></returns>
    private void LoadDocumentos(ACA_Aluno alu)
    {
        try
        {
            if (_VS_alu_id > 0)
            {
                // Carrega dados dos documentos da pessoa.
                UCGridDocumento1._CarregarDocumento(alu.pes_id);

                // Carrega dados da certidões da pessoa.
                DataTable dtCertidao = PES_CertidaoCivilBO.GetSelect(alu.pes_id, false, 1, 1);

                if (dtCertidao.Rows.Count == 0)
                    dtCertidao = null;

                UCGridCertidaoCivil1._VS_certidoes = dtCertidao;
                UCGridCertidaoCivil1._CarregarCertidaoCivil();
            }
            else
            {
                UCGridDocumento1._CarregarDocumento(Guid.Empty);
                UCGridCertidaoCivil1._CarregarCertidaoCivil();
            }

            return;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o aluno.", UtilBO.TipoMensagem.Erro);
            return;
        }
    }

    /// <summary>
    /// Carrega os dados da aba Histórico.
    /// </summary>
    /// <returns></returns>
    private void LoadHistorico()
    {
        try
        {
            UCComboAlunoHistorico.Carregar();

            if (_VS_alu_id > 0)
            {
                // Carrega dados dos históricos do aluno.
                DataTable dtHistorico = ACA_AlunoHistoricoBO.GetSelectBy_alu_id(_VS_alu_id, false, 1, 1);

                if (dtHistorico.Rows.Count == 0)
                    dtHistorico = null;

                _VS_seqHistorico = ACA_AlunoHistoricoBO.VerificaUltimoHistoricoCadastrado(_VS_alu_id) - 1;
                _VS_historico = dtHistorico;

                _CarregarHistorico();

                // Carrega dados das observações de históricos do aluno.
                DataTable dtHistoricoObservacao = ACA_AlunoHistoricoObservacaoBO.GetSelectBy_HistoricoObservacao_alu_id(_VS_alu_id, false, 1, 1);

                if (dtHistoricoObservacao.Rows.Count == 0)
                    dtHistoricoObservacao = null;

                _VS_seqHistoricoObservacao = ACA_AlunoHistoricoObservacaoBO.VerificaUltimoHistoricoObservacaoCadastrada(_VS_alu_id) - 1;
                _VS_historicoObservacao = dtHistoricoObservacao;

                _CarregarHistoricoObservacao();


                // Carrega dados da disciplinas dos históricos do aluno.
                DataTable dtDisciplina = ACA_AlunoHistoricoDisciplinaBO.GetSelectBy_alu_id(_VS_alu_id, false, 1, 1);

                if (dtDisciplina.Rows.Count == 0)
                    dtDisciplina = null;

                _VS_seqDisciplina = ACA_AlunoHistoricoDisciplinaBO.VerificaUltimaDisciplinaCadastrada(_VS_alu_id) - 1;
                _VS_disciplina = dtDisciplina;

                _CarregarHistoricoDisciplina();
            }
            else
            {
                _grvHistorico.DataSource = new DataTable();
                _grvHistorico.DataBind();

                _grvHistoricoObservacoes.DataSource = new DataTable();
                _grvHistoricoObservacoes.DataBind();

                _grvHistoricoDisciplinas.DataSource = new DataTable();
                _grvHistoricoDisciplinas.DataBind();
            }

            return;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o aluno.", UtilBO.TipoMensagem.Erro);
            return;
        }
    }

    /// <summary>
    /// Carrega os dados da aba Ficha Médica.
    /// </summary>
    /// <returns></returns>
    private void LoadFichaMedica(ACA_Aluno alu)
    {
        try
        {
            if (_VS_alu_id > 0)
            {
                // Carrega Ficha Medica.
                ACA_AlunoFichaMedica EntityAlunoFichaMedica = new ACA_AlunoFichaMedica
                {
                    alu_id = _VS_alu_id
                };
                ACA_AlunoFichaMedicaBO.GetEntity(EntityAlunoFichaMedica);

                _txtTipoSanguineo.Text = EntityAlunoFichaMedica.afm_tipoSanguineo;
                _txtFatorRH.Text = EntityAlunoFichaMedica.afm_fatorRH;
                _txtDoencaConhecidas.Text = EntityAlunoFichaMedica.afm_doencasConhecidas;
                _txtAlergias.Text = EntityAlunoFichaMedica.afm_alergias;
                _txtMedicacoesPodeUtilizar.Text = EntityAlunoFichaMedica.afm_medicacoesPodeUtilizar;
                _txtMedicacoesUsoContinuo.Text = EntityAlunoFichaMedica.afm_medicacoesUsoContinuo;
                _txtConvenioMedico.Text = EntityAlunoFichaMedica.afm_convenioMedico;
                _txtHospitalRemocao.Text = EntityAlunoFichaMedica.afm_hospitalRemocao;
                _txtOutrasRecomendacoes.Text = EntityAlunoFichaMedica.afm_outrasRecomendacoes;

                // Carrega dados dos contatos da pessoa.
                DataTable dtContatoFichaMedica = ACA_FichaMedicaContatoBO.GetSelect_By_Aluno(alu.alu_id, false, 1, 1);

                UCGridContatoNomeTelefone1._VS_seq = dtContatoFichaMedica.Rows.Count > 0 ? dtContatoFichaMedica.Rows.Count : -1;

                if (dtContatoFichaMedica.Rows.Count == 0)
                    dtContatoFichaMedica = null;

                UCGridContatoNomeTelefone1._VS_contatos = dtContatoFichaMedica;
                UCGridContatoNomeTelefone1._CarregarContato();

            }
            else
            {
                UCGridContatoNomeTelefone1._CarregarContato();
            }

            return;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o aluno.", UtilBO.TipoMensagem.Erro);
            return;
        }
    }

    /// <summary>
    /// Carrega dados da aba de Usuários.
    /// </summary>
    /// <returns></returns>
    private void LoadUsuario()
    {
        try
        {

            ManageUserLive verificar = new ManageUserLive();
            if (verificar.ExistsIntegracaoExterna())
            {
                _lblMsgUsuarioLive.Text = string.Concat("(seuEmail@", verificar.IntegracaoExterna.ine_dominio, " - caso não preenchido ou não existir, será criado uma nova conta live)");

            }
            else
            {
                _chbIntegrarUsuarioLive.Visible = false;
            }
            if (_VS_alu_id > 0)
            {
                _VS_usu_id = SYS_UsuarioBO.GetSelectBy_pes_id(UCCadastroPessoa1._VS_pes_id);

                // Carrega dados do usuário da pessoa (se existir).
                if (_VS_usu_id != Guid.Empty)
                {
                    divUsuarios.Visible = true;
                    _chbCriarUsuario.Checked = true;

                    SYS_Usuario usuario = new SYS_Usuario { usu_id = _VS_usu_id };
                    SYS_UsuarioBO.GetEntity(usuario);
                    _txtLogin.Text = usuario.usu_login;
                    _txtEmail.Text = usuario.usu_email;

                    if (usuario.usu_situacao == 5)
                        _chkExpiraSenha.Checked = true;
                    else if (usuario.usu_situacao == 2)
                        _chkBloqueado.Checked = true;

                    _chkSenhaAutomatica.Visible = false;
                    _chkSenhaAutomatica.Checked = false;

                    _lblSenha.Text = "Senha ";
                    _txtLogin.Enabled = false;
                    _rfvSenha.Visible = false;
                    _lblConfirmacao.Text = "Confirmar senha ";
                    _rfvConfirmarSenha.Visible = false;


                    if (verificar.ExistsIntegracaoExterna())
                    {
                        // verificação quando for carregar o tipo de usuario
                        if (verificar.IsContaEmail(_txtEmail.Text))
                        {
                            _chbIntegrarUsuarioLive.Checked = true;
                            _ValidarCampoCadastroUsuario();
                        }
                        else
                        {
                            _chbIntegrarUsuarioLive.Checked = false;
                            _lblMsgUsuarioLive.Visible = false;
                        }
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
            }

            return;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o aluno.", UtilBO.TipoMensagem.Erro);
            return;
        }
    }

    /// <summary>
    /// Seta propriedades padrão dos UserControls da tela.
    /// </summary>
    private void InicializaUserControls()
    {
        _VS_idMatricula = -1;
        _VS_IsNewHistorico = true;
        _VS_IsNewDisciplina = true;

        // Dados pessoais
        UCCadastroPessoa1.validaDataNascimento = true;
        UCCadastroPessoa1._lblDataNasc.Text = "Data de nascimento *";

        // Cadastro de cidades - usado no user control de pessoa e certidão civil.
        UCCadastroCidade1.Inicialize(ApplicationWEB._Paginacao, "divCadastroCidade");

        // Tipos de responsáveis.        
        ucComboTipoResponsavel.CarregarTipoResponsavelAluno();
        ucComboTipoResponsavel.Obrigatorio = true;
        ucComboTipoResponsavel.ValidationGroup = "Pessoa";

        ucResponsavelMae.Obrigatorio = false;
        ucResponsavelPai.Obrigatorio = false;
        ucResponsavelMae.InicializarUserControl();
        ucResponsavelPai.InicializarUserControl();
        ucResponsavelOutro.InicializarUserControl();

        UCFiltroEscolas1._cvUnidadeAdministrativa.ValidationGroup = "Pessoa";
        UCFiltroEscolas1._cvUnidadeEscola.ValidationGroup = "Pessoa";

        UCComboCursoCurriculo1.ValidationGroup = "Pessoa";
        UCComboCursoCurriculo1.Obrigatorio = true;
        UCComboCursoCurriculo1.CarregarCursoCurriculo();
        UCComboCursoCurriculo1.PermiteEditar = false;

        UCComboCursoCurriculo2.ValidationGroup = "Pessoa";
        UCComboCursoCurriculo2.Obrigatorio = true;
        UCComboCursoCurriculo2.Visible = false;
        UCComboCursoCurriculo2.CarregarCursoCurriculo();

        UCComboCurriculoPeriodo1._cpvCombo.ValidationGroup = "Pessoa";
        UCComboCurriculoPeriodo1._MostrarMessageSelecione = true;
        UCComboCurriculoPeriodo1._Load(-1, -1);
        UCComboCurriculoPeriodo1._Combo.Enabled = false;
        UCComboCurriculoPeriodo1._cpvCombo.ErrorMessage = GestaoEscolarUtilBO.nomePadraoPeriodo() + " é obrigatório.";

        UCComboCurriculoPeriodo2._cpvCombo.ValidationGroup = "Pessoa";
        UCComboCurriculoPeriodo2._Load(-1, -1);
        UCComboCurriculoPeriodo2._MostrarMessageSelecione = true;
        UCComboCurriculoPeriodo2.Visible = false;
        UCComboCurriculoPeriodo2._cpvCombo.ErrorMessage = GestaoEscolarUtilBO.nomePadraoPeriodo() + " é obrigatório.";

        UCComboTurma1._Validator.ValidationGroup = "Pessoa";
        UCComboTurma1.PermiteEditar = false;
        UCComboTurma1._MostrarMessageSelecione = true;
        UCComboTurma1.Obrigatorio = true;

        UCComboTurma2._Validator.ValidationGroup = "Pessoa";
        UCComboTurma2._MostrarMessageSelecione = true;
        UCComboTurma2.PermiteEditar = false;
        UCComboTurma2.Visible = false;
        UCComboTurma2.Obrigatorio = true;

        UCComboTipoMovimentacaoEntrada.ValidationGroup = "Pessoa";
        UCComboTipoMovimentacaoEntrada.Titulo = "Tipo de movimentação de entrada *";
        UCComboTipoMovimentacaoEntrada.Obrigatorio = true;
        UCComboTipoMovimentacaoEntrada.CarregarTipoMovimentacao(Convert.ToByte(ACA_TipoMovimentacaoMotivo.Entrada));

        UCComboTipoMovimentacaoEntrada1.ValidationGroup = "Pessoa";
        UCComboTipoMovimentacaoEntrada1.Titulo = "Tipo de movimentação de entrada";
        UCComboTipoMovimentacaoEntrada1.CarregarTipoMovimentacao(Convert.ToByte(ACA_TipoMovimentacaoMotivo.Entrada));

        UCComboTipoMovimentacaoSaida.ValidationGroup = "Pessoa";
        UCComboTipoMovimentacaoSaida.Titulo = "Tipo de movimentação de saída";
        UCComboTipoMovimentacaoSaida.CarregarTipoMovimentacao(Convert.ToByte(ACA_TipoMovimentacaoMotivo.Saida));

        LoadInicialEscolaDados();

        // Contatos

        ACA_Aluno entityAluno = new ACA_Aluno
        {
            alu_id = _VS_alu_id
        };
        ACA_AlunoBO.GetEntity(entityAluno);
        UCContato1.CarregarContatosDoBanco(entityAluno.pes_id);

        // Histórico.
        UCComboTipoNivelEnsino1.Obrigatorio = true;
        UCComboTipoNivelEnsino1.ValidationGroup = "Historico";
        UCComboTipoNivelEnsino1.CarregarTipoNivelEnsino();

        UCComboTipoModalidadeEnsino1.CarregarTipoModalidadeEnsino();
        UCComboTipoDisciplina1.CarregarNivelEnsinoTipoDisciplina();
        UCComboTipoRedeEnsino1.CarregarTipoRedeEnsino();

        UCEnderecos1.Inicializar(false, false, "Pessoa", true);
        UCEnderecos2.Inicializar(false, true, "EscolaOrigem", false);

        UCFiltroEscolas1.UnidadeAdministrativaCampoObrigatorio = true;
        UCFiltroEscolas1.EscolaCampoObrigatorio = true;
        UCFiltroEscolas1._LoadInicial();

        UCComboCurriculoPeriodo1._Label.Text += " *";

        UCComboReligiao.CarregarReligiao();

        UCComboTipoAtendimentoEspecial1.CarregarTipoAtendimentoEspecial();
    }

    #endregion

    #region Delegates

    /// <summary>
    /// Seta métodos Delegates.
    /// </summary>
    private void SetaDelegates()
    {
        UCFiltroEscolas1._Selecionar += UCFiltroEscolas1__Selecionar;
        UCFiltroEscolas1._SelecionarEscola += UCFiltroEscolas1__SelecionarEscola;
        UCFiltroEscolas1._Selecionar += UCComboUnidadeAdministrativa__OnSelectedIndexChange;
        UCComboCursoCurriculo1.IndexChanged += UCComboCursoCurriculo1_IndexChanged;
        UCComboCurriculoPeriodo1._OnSelectedIndexChange += new WebControls_Combos_UCComboCurriculoPeriodo.SelectedIndexChange(UCComboCurriculoPeriodo1__OnSelectedIndexChange);
        UCComboTurma1._IndexChanged += new WebControls_Combos_UCComboTurma.onIndexChanged(UCComboTurma1__IndexChanged);
        UCComboTurma2._IndexChanged += new WebControls_Combos_UCComboTurma.onIndexChanged(UCComboTurma2__IndexChanged);

        UCComboCursoCurriculo2.IndexChanged += UCComboCursoCurriculo2_IndexChanged;
        UCComboCurriculoPeriodo2._OnSelectedIndexChange += new WebControls_Combos_UCComboCurriculoPeriodo.SelectedIndexChange(UCComboCurriculoPeriodo2__OnSelectedIndexChange);

        UCComboAlunoSituacao1.IndexChanged += UCComboAlunoSituacao1__IndexChanged;

        UCComboAlunoHistorico.IndexChanged += UCComboAlunoHistorico_IndexChanged;

        ucComboTipoResponsavel.IndexChanged += ucComboTipoResponsavel_IndexChanged;

        // Eventos para o cadastro de cidades.
        UCCadastroPessoa1._AbreJanelaCadastroCidade += _AbreJanelaCadastroCidade;
        UCGridCertidaoCivil1._AbreJanelaCadastroCidade += _AbreJanelaCadastroCidade;

        // Seta as buscas da pessoa dos usercontrols de responsáveis.
        ucResponsavelMae._SetaBuscaPessoa += SetaBuscaePessoa;
        ucResponsavelPai._SetaBuscaPessoa += SetaBuscaePessoa;
        ucResponsavelOutro._SetaBuscaPessoa += SetaBuscaePessoa;

        // Seta a busca de pessoas somente no user control de responsável que chamou.
        if (!String.IsNullOrEmpty(VS_TipoResponsavelBuscaPessoa))
        {
            ((WebControls_AlunoResponsavel_UCAlunoResponsavel)
                upnTipoResponsavel.FindControl(VS_TipoResponsavelBuscaPessoa)).UCBuscaPessoasAluno = UCBuscaPessoasAluno1;
        }
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

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroMatriculaNovo", "$('#divCadastroCidade').dialog('open');", true);
    }

    private void SetaBuscaePessoa(WebControls_AlunoResponsavel_UCAlunoResponsavel sender)
    {
        VS_TipoResponsavelBuscaPessoa = sender.ID;
    }

    void ucComboTipoResponsavel_IndexChanged()
    {
        MostraResponsavelTipo(true);
    }

    /// <summary>
    /// Mostra / esconde user controls de responsáveis de acordo com o tipo.
    /// </summary>
    /// <param name="limparOutro">Flag que indica se é para limpar os campos do responsável
    /// Outro</param>
    private void MostraResponsavelTipo(bool limparOutro)
    {
        if (ucComboTipoResponsavel.Valor == -1)
        {
            ucResponsavelOutro.Visible = false;
            ucResponsavelMae.Obrigatorio = false;
            ucResponsavelPai.Obrigatorio = false;
            ucResponsavelOutro.Obrigatorio = false;
        }
        else
        {
            // Verificar o tipo de responsável selecionado.
            int tra_id = ucComboTipoResponsavel.Valor;

            bool mae = (tra_id == TipoResponsavelAlunoParametro.tra_idMae);
            bool pai = (tra_id == TipoResponsavelAlunoParametro.tra_idPai);
            bool proprio = (tra_id == TipoResponsavelAlunoParametro.tra_idProprio);
            bool outro = ((!mae) && (!pai) && (!proprio));

            // Mostra o cadastro de "outro" responsável.
            ucResponsavelOutro.Visible = outro;

            // Seta obrigatoriedade no tipo de responsável.
            ucResponsavelMae.Obrigatorio = mae;
            ucResponsavelPai.Obrigatorio = pai;
            ucResponsavelOutro.Obrigatorio = outro;

            if (limparOutro)
                ucResponsavelOutro.LimparCampos();
        }

        ucComboTipoResponsavel.SetarFoco();
    }

    #region COMBOS

    private void UCFiltroEscolas1__Selecionar()
    {
        try
        {
            if (UCFiltroEscolas1._VS_FiltroEscola)
            {
                UCFiltroEscolas1.CancelarConsultaEscola = false;
                UCFiltroEscolas1._UnidadeEscola_LoadBy_uad_idSuperior(new Guid(UCFiltroEscolas1._ComboUnidadeAdministrativa.SelectedValue));
            }

            if (UCFiltroEscolas1._ComboUnidadeAdministrativa.SelectedValue == Guid.Empty.ToString())
            {
                UCFiltroEscolas1._ComboUnidadeEscola.Enabled = false;
            }
            else
            {
                UCFiltroEscolas1._ComboUnidadeEscola.Enabled = true;
                UCFiltroEscolas1._ComboUnidadeEscola.Focus();
            }

            _lstEscProx.Items.Clear();

            UCComboCursoCurriculo1.Obrigatorio = true;
            UCComboCursoCurriculo1.CarregarCursoCurriculo();
            UCComboCursoCurriculo1.PermiteEditar = false;

            UCComboCurriculoPeriodo1._Combo.Items.Clear();
            UCComboCurriculoPeriodo1._MostrarMessageSelecione = true;
            UCComboCurriculoPeriodo1._Load(-1, -1);
            UCComboCurriculoPeriodo1._Combo.Enabled = false;

        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    private void UCFiltroEscolas1__SelecionarEscola()
    {
        try
        {
            if (UCFiltroEscolas1._ComboUnidadeEscola.SelectedValue != "-1;-1")
            {
                UCComboCursoCurriculo1.Obrigatorio = true;
                UCComboCursoCurriculo1.CarregarCursoCurriculoPorEscola(UCFiltroEscolas1._UCComboUnidadeEscola_Esc_ID, UCFiltroEscolas1._UCComboUnidadeEscola_Uni_ID, 0);
                UCComboCursoCurriculo1.PermiteEditar = true;
                UCComboCursoCurriculo1.SetarFoco();

                _VS_esc_id = UCFiltroEscolas1._UCComboUnidadeEscola_Esc_ID;
                _VS_uni_id = UCFiltroEscolas1._UCComboUnidadeEscola_Uni_ID;
            }
            else
            {
                UCComboCursoCurriculo1.Obrigatorio = true;
                UCComboCursoCurriculo1.CarregarCursoCurriculo();
                UCComboCursoCurriculo1.PermiteEditar = false;

                _VS_esc_id = -1;
                _VS_uni_id = -1;
            }

            UCComboCurriculoPeriodo1._Combo.Items.Clear();
            UCComboCurriculoPeriodo1._MostrarMessageSelecione = true;
            UCComboCurriculoPeriodo1._Load(-1, -1);
            UCComboCurriculoPeriodo1._Combo.Enabled = false;

            UCComboTurma1.PermiteEditar = false;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    private void UCComboCursoCurriculo1_IndexChanged()
    {
        try
        {
            if (UCComboCursoCurriculo1.Valor[0] > 0)
            {
                UCComboCurriculoPeriodo1._Combo.Items.Clear();
                UCComboCurriculoPeriodo1._MostrarMessageSelecione = true;
                UCComboCurriculoPeriodo1._LoadBy_cur_id_crr_id_esc_id_uni_id
                    (
                        UCComboCursoCurriculo1.Valor[0]
                        , UCComboCursoCurriculo1.Valor[1]
                        , Convert.ToInt32(UCFiltroEscolas1._ComboUnidadeEscola.SelectedValue.Split(';')[0])
                        , Convert.ToInt32(UCFiltroEscolas1._ComboUnidadeEscola.SelectedValue.Split(';')[1])

                    );
                UCComboCurriculoPeriodo1._Combo.Enabled = true;
                UCComboCurriculoPeriodo1._Combo.Focus();

                _VS_cur_id = UCComboCursoCurriculo1.Valor[0];
                _VS_crr_id = UCComboCursoCurriculo1.Valor[1];
            }
            else
            {
                UCComboCurriculoPeriodo1._Combo.Items.Clear();
                UCComboCurriculoPeriodo1._MostrarMessageSelecione = true;
                UCComboCurriculoPeriodo1._Load(-1, -1);
                UCComboCurriculoPeriodo1._Combo.Enabled = false;

                _VS_cur_id = -1;
                _VS_crr_id = -1;
            }

            UCComboTurma1._MostrarMessageSelecione = true;
            UCComboTurma1.PermiteEditar = false;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    private void UCComboCurriculoPeriodo1__OnSelectedIndexChange()
    {
        try
        {
            if (UCComboCurriculoPeriodo1._Combo.SelectedValue != "-1-1-1")
            {
                UCComboTurma1._Combo.Items.Clear();
                UCComboTurma1._MostrarMessageSelecione = true;
                UCComboTurma1.PermiteEditar = true;
                UCComboTurma1._LoadBy_esc_uni_id_crp_id_tur_situacao(UCFiltroEscolas1._UCComboUnidadeEscola_Esc_ID,
                    UCFiltroEscolas1._UCComboUnidadeEscola_Uni_ID, UCComboCurriculoPeriodo1.Valor[0],
                    UCComboCurriculoPeriodo1.Valor[1], UCComboCurriculoPeriodo1.Valor[2], 1);

                _VS_crp_id = UCComboCurriculoPeriodo1.Valor[2];
            }
            else
            {
                UCComboTurma1._Combo.Items.Clear();
                UCComboTurma1._MostrarMessageSelecione = true;
                UCComboTurma1.PermiteEditar = false;
                UCComboTurma1._LoadBy_esc_uni_id_crp_id_tur_situacao(UCFiltroEscolas1._UCComboUnidadeEscola_Esc_ID,
                    UCFiltroEscolas1._UCComboUnidadeEscola_Uni_ID, UCComboCursoCurriculo1.Valor[0],
                    UCComboCursoCurriculo1.Valor[1], -1, 1);

                _VS_crp_id = -1;
            }

        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    private void UCComboTurma1__IndexChanged()
    {
        _VS_tur_id = UCComboTurma1.Valor[0];
        _VS_ttn_id = UCComboTurma1.Valor[2];
    }

    private void UCComboCursoCurriculo2_IndexChanged()
    {
        try
        {
            if (UCComboCursoCurriculo2.Valor[0] > 0)
            {
                UCComboCurriculoPeriodo2._Combo.Items.Clear();
                UCComboCurriculoPeriodo2._MostrarMessageSelecione = true;
                UCComboCurriculoPeriodo2._LoadBy_cur_id_crr_id_esc_id_uni_id
                    (
                        UCComboCursoCurriculo2.Valor[0]
                        , UCComboCursoCurriculo2.Valor[1]
                        , Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[0]),
                        Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[1])

                    );
                UCComboCurriculoPeriodo2._Combo.Enabled = true;
                UCComboCurriculoPeriodo2._Combo.Focus();

                _VS_cur_id = UCComboCursoCurriculo2.Valor[0];
                _VS_crr_id = UCComboCursoCurriculo2.Valor[1];
            }
            else
            {
                UCComboCurriculoPeriodo2._Combo.Items.Clear();
                UCComboCurriculoPeriodo2._MostrarMessageSelecione = true;
                UCComboCurriculoPeriodo2._Load(-1, -1);
                UCComboCurriculoPeriodo2._Combo.Enabled = false;

                _VS_cur_id = -1;
                _VS_crr_id = -1;
            }

            UCComboTurma2._MostrarMessageSelecione = true;
            UCComboTurma2.PermiteEditar = false;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    private void UCComboCurriculoPeriodo2__OnSelectedIndexChange()
    {
        try
        {
            if (UCComboCurriculoPeriodo2._Combo.SelectedValue != "-1-1-1")
            {
                UCComboTurma2._Combo.Items.Clear();
                UCComboTurma2._MostrarMessageSelecione = true;
                UCComboTurma2.PermiteEditar = true;

                DataTable dt = TUR_TurmaBO.GetSelectPermissaoTotal(Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[0])
                    , Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[1]), UCComboCurriculoPeriodo2.Valor[0]
                    , UCComboCurriculoPeriodo2.Valor[1], UCComboCurriculoPeriodo2.Valor[2]
                    , __SessionWEB.__UsuarioWEB.Usuario.ent_id, Convert.ToByte(TUR_TurmaSituacao.Ativo));
                UCComboTurma2._Combo.DataSourceID = "";
                UCComboTurma2._Combo.DataSource = dt;
                UCComboTurma2._Combo.DataBind();

                _VS_crp_id = UCComboCurriculoPeriodo2.Valor[2];
            }
            else
            {
                UCComboTurma2._Combo.Items.Clear();
                UCComboTurma2._MostrarMessageSelecione = true;
                UCComboTurma2.PermiteEditar = false;
                UCComboTurma2._LoadBy_esc_uni_id_crp_id_tur_situacao(Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[0]),
                Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[1]), UCComboCursoCurriculo2.Valor[0],
                    UCComboCursoCurriculo2.Valor[1], -1, 1);

                _VS_crp_id = -1;
            }

        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    private void UCComboTurma2__IndexChanged()
    {
        _VS_tur_id = UCComboTurma2.Valor[0];
        _VS_ttn_id = UCComboTurma2.Valor[2];
    }

    private void UCComboUnidadeAdministrativa__OnSelectedIndexChange()
    {
        //Chama o método que carrega as cidades próximas
        CarregaEscolasProximas();
    }

    private void UCComboAlunoHistorico_IndexChanged()
    {
        _txtObservacao1.Text = string.Empty;
        _txtObservacao1.Enabled = UCComboAlunoHistorico.Valor <= 0;
    }

    private void UCComboAlunoSituacao1__IndexChanged()
    {
        bool mostrarDiv = ACA_AlunoCurriculoBO.GetSelectBy_alu_id(_VS_alu_id).Rows.Count > 0;

        if (UCComboAlunoSituacao1._Combo.SelectedValue == "1")
        {
            divNovaMovimentacao.Visible = !mostrarDiv;
            divVelhaMovimentacao.Visible = mostrarDiv;

            UCFiltroEscolas1.UnidadeAdministrativaCampoObrigatorio = true;
            UCFiltroEscolas1.EscolaCampoObrigatorio = true;
            UCComboCursoCurriculo1.PermiteEditar = false;
            UCComboCursoCurriculo1.Valor = new int[] { -1, -1 };
            UCComboCursoCurriculo1.Obrigatorio = true;
            UCComboCurriculoPeriodo1.PermiteEditar = false;
            UCComboCurriculoPeriodo1.Valor = new int[] { -1, -1, -1 };
            UCComboCurriculoPeriodo1.Obrigatorio = true;
            UCComboTurma1.Valor = new int[] { -1, -1, -1 };
            UCComboTurma1.Obrigatorio = true;
            UCComboTurma1.Visible = true;
            UCComboTurma1.PermiteEditar = false;
            ckbAltTurma.Enabled = true;
            UCComboTipoMovimentacaoEntrada.Valor = -1;
            UCFiltroEscolas1._LoadInicial();
        }
        else
            if (UCComboAlunoSituacao1._Combo.SelectedValue == "7" || UCComboAlunoSituacao1._Combo.SelectedValue == "8")
            {
                divNovaMovimentacao.Visible = !mostrarDiv;
                divVelhaMovimentacao.Visible = mostrarDiv;

                UCFiltroEscolas1.UnidadeAdministrativaCampoObrigatorio = true;
                UCFiltroEscolas1.EscolaCampoObrigatorio = true;
                UCComboCursoCurriculo1.PermiteEditar = false;
                UCComboCursoCurriculo1.Valor = new int[] { -1, -1 };
                UCComboCursoCurriculo1.Obrigatorio = true;
                UCComboCurriculoPeriodo1.PermiteEditar = false;
                UCComboCurriculoPeriodo1.Valor = new int[] { -1, -1, -1 };
                UCComboCurriculoPeriodo1.Obrigatorio = true;
                UCComboTurma1.Valor = new int[] { -1, -1, -1 };
                UCComboTurma1.Obrigatorio = false;
                UCComboTurma1.Visible = false;
                UCComboTurma1.Valor = new int[] { -1, -1, -1 };
                ckbAltTurma.Enabled = ckbAltTurma.Checked = false;
                UCFiltroEscolas1._LoadInicial();
            }
            else
            {
                divNovaMovimentacao.Visible = false;
                divVelhaMovimentacao.Visible = false;
            }
    }

    #endregion

    #endregion

    #endregion

    #region Eventos

    #region SubCadastros

    #region Usuário

    protected void _chbCriarUsuario_CheckedChanged(object sender, EventArgs e)
    {
        if (_chbCriarUsuario.Checked)
        {
            _ValidarCampoCadastroUsuario();
            divUsuarios.Visible = true;
            _txtLogin.Focus();
        }
        else
        {
            divUsuarios.Visible = false;
        }
    }

    protected void _chbIntegrarUsuarioLive_CheckedChanged(object sender, EventArgs e)
    {
        _ValidarCampoCadastroUsuario();
        _txtLogin.Focus();
    }

    protected void _chkSenhaAutomatica_CheckedChanged(object sender, EventArgs e)
    {
        if (_chkSenhaAutomatica.Checked)
        {
            _lblSenha.Visible = false;
            _lblConfirmacao.Visible = false;
            _txtSenha.Visible = false;
            _txtConfirmacao.Visible = false;
            _chkExpiraSenha.Enabled = false;

            _rfvSenha.Visible = false;
            _rfvConfirmarSenha.Visible = false;
            _cpvConfirmarSenha.Visible = false;

            _chkExpiraSenha.Checked = true;
        }
        else
        {
            if (!((_VS_alu_id < 0) && (_chbIntegrarUsuarioLive.Checked)))
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
            }
        }
    }

    #endregion

    #region Histórico

    protected void _btnNovoHistorico_Click(object sender, EventArgs e)
    {
        _VS_seqHistorico = _VS_seqHistorico + 1;
        _LimparCamposHistorico();
        UCComboTipoNivelEnsino1.SetarFoco();

        divDialogHistorico.Visible = true;
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroHistoricoNovo", "$('#divHistorico').dialog('open');", true);
    }

    protected void _btnIncluirHistorico_Click(object sender, EventArgs e)
    {
        if (_ValidarHistorico())
        {
            if (_VS_IsNewHistorico)
                _IncluirHistorico();
            else
                _AlterarHistorico();

            _LimparCamposHistorico();
        }
        else
        {
            _lblMessageHistorico.Visible = true;
        }
    }

    protected void _grvHistorico_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer && e.Row.RowType != DataControlRowType.Pager)
        {
            LinkButton _btnAlterar = (LinkButton)e.Row.FindControl("_btnAlterar");
            if (_btnAlterar != null)
            {
                _btnAlterar.CommandArgument = e.Row.RowIndex.ToString();

                if (((DataRowView)e.Row.DataItem).Row["mtu_id"].ToString() != "-1")
                    _btnAlterar.Visible = false;
            }

            Label _lblAlterar = (Label)e.Row.FindControl("_lblAlterar");
            if (_lblAlterar != null)
            {
                if (((DataRowView)e.Row.DataItem).Row["mtu_id"].ToString() != "-1")
                    _lblAlterar.Visible = true;
            }

            ImageButton _btnExcluir = (ImageButton)e.Row.FindControl("_btnExcluir");
            if (_btnExcluir != null)
            {
                _btnExcluir.CommandArgument = e.Row.RowIndex.ToString();

                if (((DataRowView)e.Row.DataItem).Row["mtu_id"].ToString() != "-1")
                    _btnExcluir.Visible = false;
            }
        }
    }

    protected void _grvHistorico_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            int index = int.Parse(e.CommandArgument.ToString());
            _VS_idHistorico = Convert.ToInt32(_grvHistorico.DataKeys[index].Value);

            _ExcluirHistorico();
        }
        else if (e.CommandName == "Alterar")
        {
            int index = int.Parse(e.CommandArgument.ToString());
            _VS_idHistorico = Convert.ToInt32(_grvHistorico.DataKeys[index].Value);

            _LimparCamposHistorico();
            _AlterarHistoricoGrid();
        }
    }

    #endregion

    #region Disciplinas

    protected void _btnNovoDisciplina_Click(object sender, EventArgs e)
    {
        _LimparCamposDisciplina();
        UCComboTipoDisciplina1.SetarFoco();

        divDialogDisciplina.Visible = true;
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroDisciplinaNovo", "$('#divDisciplina').dialog('open');", true);
    }

    protected void _btnIncluirDisciplina_Click(object sender, EventArgs e)
    {
        if (_ValidarDisciplina())
        {
            if (_VS_IsNewDisciplina)
                _IncluirDisciplina();
            else
                _AlterarDisciplina();

            _LimparCamposDisciplina();
        }
        else
        {
            _lblMessageDisciplina.Visible = true;
        }
    }

    protected void _grvHistoricoDisciplinas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer && e.Row.RowType != DataControlRowType.Pager)
        {
            LinkButton _btnAlterar = (LinkButton)e.Row.FindControl("_btnAlterar");
            if (_btnAlterar != null)
            {
                _btnAlterar.CommandArgument = e.Row.RowIndex.ToString();
            }

            ImageButton _btnExcluir = (ImageButton)e.Row.FindControl("_btnExcluir");
            if (_btnExcluir != null)
            {
                _btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
            }
        }
    }

    protected void _grvHistoricoDisciplinas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            int index = int.Parse(e.CommandArgument.ToString());
            _VS_idHistorico = Convert.ToInt32(_grvHistoricoDisciplinas.DataKeys[index].Values[0]);
            _VS_idDisciplina = Convert.ToInt32(_grvHistoricoDisciplinas.DataKeys[index].Values[1]);

            _ExcluirDisciplina();
        }
        else if (e.CommandName == "Alterar")
        {
            int index = int.Parse(e.CommandArgument.ToString());
            _VS_idHistorico = Convert.ToInt32(_grvHistoricoDisciplinas.DataKeys[index].Values[0]);
            _VS_idDisciplina = Convert.ToInt32(_grvHistoricoDisciplinas.DataKeys[index].Values[1]);

            _AlterarDisciplinaGrid();
        }
    }

    #endregion

    #region Observação do histórico

    protected void _btnNovoHistoricoObservacao_Click(object sender, EventArgs e)
    {
        _LimparCamposHistoricoObservacao();
        UCComboAlunoHistorico._Combo.Focus();


        divDialogHistoricoObservacao.Visible = true;
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroHistoricoObservacaoNovo", "$('#divObservacao').dialog('open');", true);
    }

    protected void _btnIncluirHistoricoObservacao_Click(object sender, EventArgs e)
    {
        if (_ValidarHistoricoObservacao())
        {
            if (_VS_IsNewHistoricoObservacao)
                _IncluirHistóricoObservacao();
            else
                _AlterarHistoricoObservacao();

            _LimparCamposHistoricoObservacao();
        }
        else
        {
            _lblMessageHistoricoObservacao.Visible = true;
        }
    }

    protected void _grvHistoricoObservacoes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer && e.Row.RowType != DataControlRowType.Pager)
        {
            LinkButton _btnAlterar = (LinkButton)e.Row.FindControl("_btnAlterar");
            if (_btnAlterar != null)
            {
                _btnAlterar.CommandArgument = e.Row.RowIndex.ToString();
            }

            ImageButton _btnExcluir = (ImageButton)e.Row.FindControl("_btnExcluir");
            if (_btnExcluir != null)
            {
                _btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
            }
        }
    }

    protected void _grvHistoricoObservacoes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            int index = int.Parse(e.CommandArgument.ToString());
            _VS_idHistorico = Convert.ToInt32(_grvHistoricoObservacoes.DataKeys[index].Values[0]);
            _VS_idHistoricoObservacao = Convert.ToInt32(_grvHistoricoObservacoes.DataKeys[index].Values[1]);

            _ExcluirHistoricoObservacao();
        }
        else if (e.CommandName == "Alterar")
        {
            int index = int.Parse(e.CommandArgument.ToString());
            _VS_idHistorico = Convert.ToInt32(_grvHistoricoObservacoes.DataKeys[index].Values[0]);
            _VS_idHistoricoObservacao = Convert.ToInt32(_grvHistoricoObservacoes.DataKeys[index].Values[1]);

            _AlterarHistoricoObservacaoGrid();
        }
    }

    #endregion

    #region Escola Origem

    protected void _btnPesquisarEscolaOrigem_Click(object sender, ImageClickEventArgs e)
    {
        fdsResultadosEscolaOrigem.Visible = false;
        _txtBuscaEscolaOrigem.Text = string.Empty;

        divDialogBuscaEscolaOrigem.Visible = true;
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "BuscaEscolaOrigem", "$('#divBuscaEscolaOrigem').dialog('open');", true);
    }

    protected void _btnNovoEscolaOrigem_Click(object sender, EventArgs e)
    {
        try
        {
            _LimparCamposCadastroEscolaOrigem();
            UCComboTipoRedeEnsino1.SetarFoco();

            CarregarCidadeUsuarioLogado();
        }
        catch (Exception ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            ApplicationWEB._GravaErro(ex);
        }

        divDialogEscolaOrigem.Visible = true;
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroEscolaOrigemNovo", "$('#divEscolaOrigem').dialog('open');", true);
    }

    protected void _btnIncluirEscolaOrigem_Click(object sender, EventArgs e)
    {
        if (_ValidarCadastroEscolaOrigem())
        {
            _txtEscolaOrigem.Text = _txtNomeEscolaOrigem.Text;
            _VS_eco_id = -1;

            divDialogEscolaOrigem.Visible = false;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "CadastroEscolaOrigemFechar", "$('#divEscolaOrigem').dialog('close');", true);

            divDialogBuscaEscolaOrigem.Visible = false;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "BuscaEscolaOrigemFechar", "$('#divBuscaEscolaOrigem').dialog('close');", true);
        }
        else
        {
            _lblMessageEscolaOrigem.Visible = true;
        }
    }

    protected void _btnBuscaEscolaOrigem_Click(object sender, EventArgs e)
    {
        _PesquisarEscolaOrigem();
        fdsResultadosEscolaOrigem.Visible = true;
    }

    protected void odsEscolaOrigem_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (e.ExecutingSelectCount)
            e.InputParameters.Clear();
    }

    protected void _grvEscolaOrigem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton btnSelecionar = (LinkButton)e.Row.FindControl("_btnSelecionar");
            if (btnSelecionar != null)
            {
                btnSelecionar.CommandArgument = e.Row.RowIndex.ToString();
            }
        }
    }

    protected void _grvEscolaOrigem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Selecionar")
        {
            int index = int.Parse(e.CommandArgument.ToString());
            _VS_eco_id = Convert.ToInt64(_grvEscolaOrigem.DataKeys[index].Values[0]);
            _txtEscolaOrigem.Text = _grvEscolaOrigem.DataKeys[index].Values[1].ToString();

            divDialogBuscaEscolaOrigem.Visible = false;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "BuscaEscolaOrigemFechar3", "$('#divBuscaEscolaOrigem').dialog('close');", true);
        }
    }

    #endregion

    protected void odsPessoas_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (e.ExecutingSelectCount)
            e.InputParameters.Clear();
    }

    protected void _ddlTipoControle_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_ddlTipoControle.SelectedValue == "1")
        {
            divHistoricoNotaGlobal.Visible = false;
            fdsHistoricoDisciplinas.Visible = true;

            _CarregarHistoricoDisciplina();
        }
        else if (_ddlTipoControle.SelectedValue == "2")
        {
            divHistoricoNotaGlobal.Visible = true;
            fdsHistoricoDisciplinas.Visible = false;
        }
        else
        {
            divHistoricoNotaGlobal.Visible = false;
            fdsHistoricoDisciplinas.Visible = false;
        }

        _txtAvaliacao.Text = string.Empty;
        _txtFrequencia.Text = string.Empty;
    }

    #endregion

    /// <summary>
    /// Disparado ao selecionar um item no listBox de escolas próximas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void _lstEscProx_SelectedIndexChanged(object sender, EventArgs e)
    {
        UCFiltroEscolas1._ComboUnidadeEscola.SelectedValue = _lstEscProx.SelectedValue;
        UCFiltroEscolas1__SelecionarEscola();
    }

    protected void _btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/Aluno/Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void _btnSalvar_Click(object sender, EventArgs e)
    {
        string permaneceTela = ACA_ParametroAcademicoBO.ParametroValor(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA);
        if (string.IsNullOrEmpty(permaneceTela))
            permaneceTela = "False";

        if (Page.IsValid)
        {
            _Salvar(Convert.ToBoolean(permaneceTela), true, true);
        }
    }

    protected void btnCancelarAlunoExistente_Click(object sender, EventArgs e)
    {
        string permaneceTela = ACA_ParametroAcademicoBO.ParametroValor(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA);
        if (string.IsNullOrEmpty(permaneceTela))
            permaneceTela = "False";

        _Salvar(Convert.ToBoolean(permaneceTela), false, true);
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "AlunoExistente", "$(document).ready(function(){ $('#divAlunoExistente').dialog('close'); });", true);
    }

    protected void btnConfirmarAluno_Click(object sender, EventArgs e)
    {
        string permaneceTela = ACA_ParametroAcademicoBO.ParametroValor(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA);
        if (string.IsNullOrEmpty(permaneceTela))
            permaneceTela = "False";

        _Salvar(Convert.ToBoolean(permaneceTela), false, false);
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "DuplicidadeFonetica", "$(document).ready(function(){ $('#divDuplicidadeFonetica').dialog('close'); });", true);
    }

    protected void btnConfirmaMovimetacao_Click(object sender, EventArgs e)
    {
        string permaneceTela = ACA_ParametroAcademicoBO.ParametroValor(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA);
        if (string.IsNullOrEmpty(permaneceTela))
            permaneceTela = "False";

        _Salvar(Convert.ToBoolean(permaneceTela), false, true);
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "ConfirmarMovimentacao", "$(document).ready(function(){ $('#divConfirmMovimentacao').dialog('close'); });", true);
    }

    protected void _btnNovo_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/Aluno/Cadastro.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void checkBox_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;

        if (_VS_alu_id > 0)
        {
            LoadEscolaDados(_VS_uad_idSuperior);

            if (ddlUnidadeEscola.Items.FindByValue(_VS_esc_id_Anterior + ";" + _VS_uni_id_Anterior) != null)
                ddlUnidadeEscola.SelectedValue = _VS_esc_id_Anterior + ";" + _VS_uni_id_Anterior;

            UCComboCursoCurriculo2.Obrigatorio = ckbAltTurma.Checked;
            UCComboCursoCurriculo2.CarregarCursoCurriculoPorEscola(_VS_esc_id_Anterior, _VS_uni_id_Anterior, 0);

            UCComboCurriculoPeriodo2._Combo.Items.Clear();
            UCComboCurriculoPeriodo2._MostrarMessageSelecione = true;
            UCComboCurriculoPeriodo2._LoadBy_cur_id_crr_id_esc_id_uni_id(_VS_cur_id_Anterior, _VS_crr_id_Anterior, _VS_esc_id_Anterior, _VS_uni_id_Anterior);

            DataTable dt = TUR_TurmaBO.GetSelectPermissaoTotal(_VS_esc_id_Anterior, _VS_uni_id_Anterior, _VS_cur_id_Anterior, _VS_crr_id_Anterior, _VS_crp_id_Anterior, __SessionWEB.__UsuarioWEB.Usuario.ent_id, Convert.ToByte(TUR_TurmaSituacao.Ativo));
            UCComboTurma2._Combo.DataSourceID = "";
            UCComboTurma2._Combo.Items.Clear();
            UCComboTurma2._Combo.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1;-1;-1", true));
            UCComboTurma2._Combo.DataSource = dt;
            UCComboTurma2._Combo.DataBind();
        }

        ckbAltTurma.Checked = ckbAltTurma == cb ? ckbAltTurma.Checked : false;
        ckbRecAluno.Checked = ckbRecAluno == cb ? ckbRecAluno.Checked : false;
        ckbAltCurso.Checked = ckbAltCurso == cb ? ckbAltCurso.Checked : false;
        ckbTranfDentroRede.Checked = ckbTranfDentroRede == cb ? ckbTranfDentroRede.Checked : false;
        ckbTranfForaRede.Checked = ckbTranfForaRede == cb ? ckbTranfForaRede.Checked : false;

        if (ckbAltTurma.Checked)
        {
            lblUASuperior.Visible = false;
            ddlUASuperior.Visible = false;
            lblUnidadeEscola.Visible = false;
            ddlUnidadeEscola.Visible = false;

            UCComboCursoCurriculo2.Obrigatorio = false;
            UCComboCursoCurriculo2.Visible = false;

            UCComboCurriculoPeriodo2.Obrigatorio = false;
            UCComboCurriculoPeriodo2.Visible = false;

            UCComboTurma2.Obrigatorio = true;
            UCComboTurma2.PermiteEditar = true;
            UCComboTurma2.Visible = true;
            UCComboTurma2.Valor = new int[] { -1, -1, -1 };
        }
        else
            if (ckbRecAluno.Checked)
            {
                lblUASuperior.Visible = false;
                ddlUASuperior.Visible = false;
                lblUnidadeEscola.Visible = false;
                ddlUnidadeEscola.Visible = false;

                UCComboCursoCurriculo2.Obrigatorio = false;
                UCComboCursoCurriculo2.Visible = false;

                UCComboCurriculoPeriodo2.Obrigatorio = true;
                UCComboCurriculoPeriodo2.PermiteEditar = true;
                UCComboCurriculoPeriodo2.Visible = true;
                UCComboCurriculoPeriodo2.Valor = new int[] { -1, -1, -1 };

                UCComboTurma2.Obrigatorio = true;
                UCComboTurma2.PermiteEditar = false;
                UCComboTurma2.Visible = true;
                UCComboTurma2.Valor = new int[] { -1, -1, -1 };
            }
            else
                if (ckbAltCurso.Checked)
                {
                    lblUASuperior.Visible = false;
                    ddlUASuperior.Visible = false;
                    lblUnidadeEscola.Visible = false;
                    ddlUnidadeEscola.Visible = false;

                    UCComboCursoCurriculo2.Obrigatorio = true;
                    UCComboCursoCurriculo2.PermiteEditar = true;
                    UCComboCursoCurriculo2.Visible = true;
                    UCComboCursoCurriculo2.Valor = new[] { -1, -1 };

                    UCComboCurriculoPeriodo2.Obrigatorio = true;
                    UCComboCurriculoPeriodo2.PermiteEditar = false;
                    UCComboCurriculoPeriodo2.Visible = true;
                    UCComboCurriculoPeriodo2.Valor = new int[] { -1, -1, -1 };

                    UCComboTurma2.Obrigatorio = true;
                    UCComboTurma2.PermiteEditar = false;
                    UCComboTurma2.Visible = true;
                    UCComboTurma2.Valor = new int[] { -1, -1, -1 };
                }
                else
                    if (ckbTranfDentroRede.Checked)
                    {
                        lblUASuperior.Visible = true;
                        ddlUASuperior.Visible = true;
                        lblUnidadeEscola.Visible = true;
                        ddlUnidadeEscola.Visible = true;
                        ddlUnidadeEscola.SelectedValue = "-1;-1";

                        ddlUASuperior.Enabled = true;
                        ddlUASuperior.SelectedValue = Guid.Empty.ToString();

                        UCComboCursoCurriculo2.Obrigatorio = true;
                        UCComboCursoCurriculo2.PermiteEditar = false;
                        UCComboCursoCurriculo2.Visible = true;
                        UCComboCursoCurriculo2.Valor = new[] { -1, -1 };

                        UCComboCurriculoPeriodo2.Obrigatorio = true;
                        UCComboCurriculoPeriodo2.PermiteEditar = false;
                        UCComboCurriculoPeriodo2.Visible = true;
                        UCComboCurriculoPeriodo2.Valor = new int[] { -1, -1, -1 };

                        UCComboTurma2.Obrigatorio = true;
                        UCComboTurma2.PermiteEditar = false;
                        UCComboTurma2.Visible = true;
                        UCComboTurma2.Valor = new int[] { -1, -1, -1 };
                    }
                    else
                    {
                        lblUASuperior.Visible = false;
                        ddlUASuperior.Visible = false;
                        lblUnidadeEscola.Visible = false;
                        ddlUnidadeEscola.Visible = false;

                        UCComboCursoCurriculo2.Obrigatorio = false;
                        UCComboCursoCurriculo2.Visible = false;

                        UCComboCurriculoPeriodo2.Obrigatorio = false;
                        UCComboCurriculoPeriodo2.Visible = false;

                        UCComboTurma2.Obrigatorio = false;
                        UCComboTurma2.Visible = false;

                        UCComboTipoMovimentacaoEntrada1.Valor = -1;
                        UCComboTipoMovimentacaoSaida.Valor = -1;
                    }

    }

    protected void ddlUASuperior_SelectedIndexChanged(object sender, EventArgs e)
    {
        UCComboCursoCurriculo2.Valor = new[] { -1, -1 };
        UCComboCursoCurriculo2.PermiteEditar = false;
        UCComboCurriculoPeriodo2._Combo.SelectedValue = "-1;-1;-1";
        UCComboCurriculoPeriodo2._Combo.Enabled = false;
        UCComboTurma2._Combo.SelectedValue = "-1;-1;-1";
        UCComboTurma2._Combo.Enabled = false;

        try
        {
            if (_VS_FiltroEscola)
                LoadEscolaDados(new Guid(ddlUASuperior.SelectedValue));

            ddlUnidadeEscola.Enabled = ddlUASuperior.SelectedValue != Guid.Empty.ToString();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void ddlUnidadeEscola_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            if (ddlUnidadeEscola.SelectedValue != "-1;-1")
            {
                UCComboCursoCurriculo2.CarregarCursoCurriculoPorEscola(Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[0]), Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[1]), 0);
            }

            _VS_esc_id = Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[0]);
            _VS_uni_id = Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[1]);

            UCComboCursoCurriculo2.PermiteEditar = ddlUnidadeEscola.SelectedValue != "-1;-1";
            UCComboCurriculoPeriodo2.Valor = new int[] { -1, -1, -1 };
            UCComboCurriculoPeriodo2.PermiteEditar = false;

            UCComboTurma2._Combo.SelectedValue = "-1;-1;-1";
            UCComboTurma2._Combo.Enabled = false;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void odsHistoricoMovimentacao_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (e.ExecutingSelectCount)
            e.InputParameters.Clear();
        else
            e.Cancel = CancelaSelect;
    }

    protected void grvHistoricoMovimentacao_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        CancelaSelect = false;
    }

    #endregion
}

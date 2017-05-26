using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

public partial class Academico_Escola_Cadastro : MotherPageLogado
{
    #region Constantes

    private const string INDEX_ABA_DADOSBASICOS = "0";

    private const string INDEX_ABA_CURSO = "1";

    private const string INDEX_ABA_OBS = "2";

    private const string INDEX_ABA_IMPORTACAO = "3";

    #endregion

    #region Propriedades

    /// <summary>
    /// Id do curso.
    /// </summary>
    private int _VS_cur_id
    {
        get
        {
            if (ViewState["_VS_cur_id"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_cur_id"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_cur_id"] = value;
        }
    }

    /// <summary>
    /// Id do currículo.
    /// </summary>
    private int _VS_crr_id
    {
        get
        {
            if (ViewState["_VS_crr_id"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_crr_id"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_crr_id"] = value;
        }
    }

    /// <summary>
    /// Id da escola.
    /// </summary>
    private int _VS_esc_id
    {
        get
        {
            if (ViewState["_VS_esc_id"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_esc_id"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_esc_id"] = value;
        }
    }

    /// <summary>
    /// Id da unidade.
    /// </summary>
    private int _VS_uni_id
    {
        get
        {
            if (ViewState["_VS_uni_id"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_uni_id"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_uni_id"] = value;
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

    private string VS_BuscaColaborador
    {
        get
        {
            if (ViewState["VS_BuscaColaborador"] != null)
            {
                return ViewState["VS_BuscaColaborador"].ToString();
            }

            return string.Empty;
        }

        set
        {
            ViewState["VS_BuscaColaborador"] = value;
        }
    }

    /// <summary>
    /// Guarda o uad_id da escola, em ViewState.
    /// </summary>
    private Guid _VS_uad_id
    {
        get
        {
            if (ViewState["_VS_uad_id"] == null)
            {
                return new Guid();
            }

            return (Guid)ViewState["_VS_uad_id"];
        }

        set
        {
            ViewState["_VS_uad_id"] = value;
        }
    }

    /// <summary>
    /// Guarda o uad_idSuperiorGestao da escola, em ViewState.
    /// </summary>
    private Guid _VS_uad_idSuperiorGestao
    {
        get
        {
            if (ViewState["_VS_uad_idSuperiorGestao"] == null)
            {
                return new Guid();
            }

            return (Guid)ViewState["_VS_uad_idSuperiorGestao"];
        }

        set
        {
            ViewState["_VS_uad_idSuperiorGestao"] = value;
        }
    }

    private int Vs_index
    {
        get
        {
            if (ViewState["Vs_index"] != null)
            {
                return Convert.ToInt32(ViewState["Vs_index"]);
            }

            return -1;
        }

        set
        {
            ViewState["Vs_index"] = value;
        }
    }

    /// <summary>
    /// ViewState que armazena a data em que a tela foi acessada.
    /// </summary>
    private DateTime VS_dataAcesso
    {
        get
        {
            return Convert.ToDateTime(ViewState["VS_dataAcesso"] ?? new DateTime().ToString());
        }

        set
        {
            ViewState["VS_dataAcesso"] = value;
        }
    }

    /// <summary>
    /// Verdadeiro caso esteja alterando o período de importação da escola
    /// </summary>
    private bool _VS_esc_id_importacao
    {
        get
        {
            if (ViewState["_VS_esc_id_importacao"] != null)
            {
                return Convert.ToBoolean(ViewState["_VS_esc_id_importacao"]);
            }

            return false;
        }

        set
        {
            ViewState["_VS_esc_id_importacao"] = value;
        }
    }

    /// <summary>
    /// Indica se o usuário tem permissão total de alteração na tela
    /// </summary>
    private bool alteracaoTotalUsuarioAdmin
    {
        get
        {
            return __SessionWEB.__UsuarioWEB.Usuario.usu_login.ToUpper() == "ADMIN" && ApplicationWEB.LiberarAlteracaoTotalAdmin;
        }
    }

    #region Aba_Cursos_Curriculo

    public bool _VS_IsNewCurso
    {
        get
        {
            return Convert.ToBoolean(ViewState["_VS_IsNewCurso"]);
        }

        set
        {
            ViewState["_VS_IsNewCurso"] = value;
        }
    }

    private int _VS_ces_id
    {
        get
        {
            if (ViewState["_VS_ces_id"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_ces_id"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_ces_id"] = value;
        }
    }

    private int _VS_crt_id
    {
        get
        {
            if (ViewState["_VS_crt_id"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_crt_id"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_crt_id"] = value;
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

    private SortedDictionary<int, List<ACA_CursoBO.TmpTurnos>> _VS_tmpTurnos
    {
        get
        {
            if (ViewState["_VS_tmpTurnos"] == null)
            {
                SortedDictionary<int, List<ACA_CursoBO.TmpTurnos>> lt = new SortedDictionary<int, List<ACA_CursoBO.TmpTurnos>>();
                ViewState["_VS_tmpTurnos"] = lt;
            }

            return (SortedDictionary<int, List<ACA_CursoBO.TmpTurnos>>)ViewState["_VS_tmpTurnos"];
        }
    }

    /// <summary>
    /// Indica se é uma alteração ou inclusão de turnos
    /// </summary>
    public bool _VS_IsNewTurno
    {
        get
        {
            return Convert.ToBoolean(ViewState["_VS_IsNewTurno"]);
        }

        set
        {
            ViewState["_VS_IsNewTurno"] = value;
        }
    }

    /// <summary>
    /// ViewState com datatable de turnos
    /// Retorno e atribui valores para o DataTable de turnos
    /// </summary>
    public DataTable _VS_Turnos
    {
        get
        {
            if (ViewState["_VS_Turnos"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("cur_id");
                dt.Columns.Add("crr_id");
                dt.Columns.Add("crt_id");
                dt.Columns.Add("esc_id");
                dt.Columns.Add("uni_id");
                dt.Columns.Add("ces_id");
                dt.Columns.Add("ttn_id");
                dt.Columns.Add("ttn_nome");
                dt.Columns.Add("crt_vigenciaInicio");
                dt.Columns.Add("crt_vigenciaFim");
                dt.Columns.Add("crt_vigencia");
                dt.Columns.Add("crt_delete");
                dt.Columns.Add("isNew");

                ViewState["_VS_Turnos"] = dt;
            }

            return (DataTable)ViewState["_VS_Turnos"];
        }

        set
        {
            ViewState["_VS_Turnos"] = value;
        }
    }

    public DataTable _VS_Turnos_Iniciais
    {
        get
        {
            if (ViewState["_VS_Turnos_Iniciais"] == null)
            {
                DataTable dt = new DataTable();
                ViewState["_VS_Turnos_Iniciais"] = dt;
            }

            return (DataTable)ViewState["_VS_Turnos_Iniciais"];
        }

        set
        {
            ViewState["_VS_Turnos_Iniciais"] = value;
        }
    }

    private DataTable dtTipoTurno;

    /// <summary>
    /// ViewState contendo os tipos de turno
    /// </summary>
    private DataTable _VS_Tipo_Turno
    {
        get
        {
            if (dtTipoTurno == null)
            {
                dtTipoTurno = ACA_TipoTurnoBO.SelecionaTipoTurno();
            }

            return dtTipoTurno;
        }
    }

    public DataTable _VS_CursoPeriodo
    {
        get
        {
            if (ViewState["_VS_CursoPeriodo"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("crp_id");

                ViewState["_VS_CursoPeriodo"] = dt;
            }

            return (DataTable)ViewState["_VS_CursoPeriodo"];
        }

        set
        {
            ViewState["_VS_CursoPeriodo"] = value;
        }
    }

    /// <summary>
    /// ViewState com datatable de periodos
    /// Retorno e atribui valores para o DataTable de periodos
    /// </summary>
    public DataTable _VS_periodos
    {
        get
        {
            if (ViewState["_VS_periodos"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("data");
                dt.Columns.Add("crr_id");
                dt.Columns.Add("crp_id");
                dt.Columns.Add("crp_ordem");
                dt.Columns.Add("crp_descricao");
                dt.Columns.Add("crp_ciclo");
                dt.Columns.Add("crp_cicloDescricao");
                dt.Columns.Add("crp_idadeIdealAnoInicio");
                dt.Columns.Add("crp_idadeIdealMesInicio");
                dt.Columns.Add("crp_idadeIdealAnoFim");
                dt.Columns.Add("crp_idadeIdealMesFim");
                dt.Columns.Add("crp_situacao");
                dt.Columns.Add("situacao");

                ViewState["_VS_periodos"] = dt;
            }

            return (DataTable)ViewState["_VS_periodos"];
        }

        set
        {
            ViewState["_VS_periodos"] = value;
        }
    }

    public DataTable _VS_Curso
    {
        get
        {
            if (ViewState["_VS_Curso"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("cur_id");
                dt.Columns.Add("crr_id");
                dt.Columns.Add("esc_id");
                dt.Columns.Add("uni_id");
                dt.Columns.Add("ces_id");
                dt.Columns.Add("cur_nome");
                dt.Columns.Add("ces_vigenciaInicio");
                dt.Columns.Add("ces_vigenciaFim");
                dt.Columns.Add("ces_vigencia");
                dt.Columns.Add("vis_id");

                ViewState["_VS_Curso"] = dt;
            }

            return (DataTable)ViewState["_VS_Curso"];
        }

        set
        {
            ViewState["_VS_Curso"] = value;
        }
    }

    /// <summary>
    /// Indica se é uma alteração ou inclusão de periodo
    /// </summary>
    public bool _VS_IsNewPeriodo
    {
        get
        {
            return Convert.ToBoolean(ViewState["_VS_IsNewPeriodo"]);
        }

        set
        {
            ViewState["_VS_IsNewPeriodo"] = value;
        }
    }

    public DataTable _VS_CurriculoCursoPeriodo
    {
        get
        {
            if (ViewState["_VS_CurriculoCursoPeriodo"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("cur_id");
                dt.Columns.Add("crr_id");
                dt.Columns.Add("esc_id");
                dt.Columns.Add("uni_id");
                dt.Columns.Add("ces_id");
                dt.Columns.Add("crp_id");
                dt.Columns.Add("cep_situacao");

                ViewState["_VS_CurriculoCursoPeriodo"] = dt;
            }

            return (DataTable)ViewState["_VS_CurriculoCursoPeriodo"];
        }

        set
        {
            ViewState["_VS_CurriculoCursoPeriodo"] = value;
        }
    }

    /// <summary>
    /// ViewState que armazena quantidade de registros na turma curriculo ou na matricula.
    /// </summary>
    public DataTable VS_QuantidadeTurmaCurriculoMatricula
    {
        get
        {
            if (ViewState["VS_QuantidadeTurmaCurriculoMatricula"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("cur_id");
                dt.Columns.Add("crr_id");
                dt.Columns.Add("esc_id");
                dt.Columns.Add("uni_id");
                dt.Columns.Add("Quantidade");

                ViewState["VS_QuantidadeTurmaCurriculoMatricula"] = dt;
            }

            return (DataTable)ViewState["VS_QuantidadeTurmaCurriculoMatricula"];
        }

        set
        {
            ViewState["VS_QuantidadeTurmaCurriculoMatricula"] = value;
        }
    }

    #endregion

    #endregion

    #region Delegates

    /// <summary>
    /// Seta delegates dos UserControls da tela.
    /// </summary>
    private void SetaDelegates()
    {
        UCCCalendario1.IndexChanged += UCCCalendario_IndexChanged;
    }

    #region Calendario

    protected void UCCCalendario_IndexChanged()
    {
        try
        {
            if (UCCCalendario1.Valor > 0)
            {
                chkTiposPeriodos.DataSource = ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendarioPorTipoPeriodoCalendario(0, UCCCalendario1.Valor);
                chkTiposPeriodos.DataValueField = "tpc_id";
                chkTiposPeriodos.DataTextField = "tpc_nome";
                chkTiposPeriodos.DataBind();
            }
            else
            {
                chkTiposPeriodos.DataSource = null;
                chkTiposPeriodos.DataBind();

                chkTiposPeriodos.Items.Clear();
            }

            // hdnTab.Value = INDEX_ABA_IMPORTACAO;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados do calendário.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion Calendario

    #endregion Delegates

    #region Métodos
    
    /// <summary>
    /// Verifica se usuário tem acesso para incluir/alterar escola.
    /// Se o ator estiver autenticado usando um grupo com visão unidade administrativa não 
    /// deverá ser possível incluir ou excluir escolas e apenas alterar dados referentes aos 
    /// contatos e dependências da escola.
    /// </summary>
    /// <returns>Se usuário tem acesso</returns>
    private bool VerificaPermissaoUsuario()
    {
        bool visaoUA = __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa;
        bool ret = false;

        if (_VS_esc_id > 0)
        {
            if (visaoUA)
            {
                List<Guid> uad_ids = UAsVisaoGrupoList();

                ESC_Escola ent = new ESC_Escola
                {
                    esc_id = _VS_esc_id
                };
                ESC_EscolaBO.SelecionaEscola(ent);

                if (uad_ids.Exists(p => p == ent.uad_id))
                {
                    ret = true;
                }
                else
                {
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("O usuário não tem permissão para alterar os dados dessa escola.", UtilBO.TipoMensagem.Alerta);
                }
            }
            else
            {
                ret = true;
            }
        }
        else
        {
            __SessionWEB.PostMessages = UtilBO.GetErroMessage("O usuário não tem permissão para incluir escolas.", UtilBO.TipoMensagem.Alerta);
        }

        if (ret)
        {
            HabilitaControles(fsDadosBasicos.Controls, false);
            HabilitaControles(fsContatos.Controls, false);
            HabilitaControles(fsEndereco.Controls, false);
            HabilitaControles(fdsClassificacao.Controls, false);
            HabilitaControles(fsOrgaoSupervisao.Controls, false);

            _grvCurso.Enabled = true;
            _txtObs.Enabled = false;
            UCCCalendario1.PermiteEditar = true;
            chkTiposPeriodos.Enabled = false;
        }

        return ret;
    }

    /// <summary>
    /// Seta a cidade pelo endereço da entidade do usuário logado.
    /// </summary>
    private void CarregarCidadeUsuarioLogado()
    {
        if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PAR_PREENCHER_CIDADE, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
        {
            // Setar a cidade pelo endereço da Entidade do usuário logado.
            Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

            Guid ene_id = SYS_EntidadeEnderecoBO.Select_ene_idBy_ent_id(ent_id);

            SYS_EntidadeEndereco entEndereco = new SYS_EntidadeEndereco
            {
                ent_id = ent_id,
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
    /// Carrega o combo de Unidade Administrativa superior
    /// Se for alteração, não mostra a própria UA no combo.
    /// </summary>
    /// <param name="uad_id">Id da unidade administrativa.</param>
    private void CarregarComboUASuperior(Guid uad_id)
    {
        // Carrega combo de Unidade Administrativa Superior.
        if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa)
        {
            if (UCComboUnidadeAdministrativa1.FiltraUnidadeSuperiorPorEscola(uad_id) ||
               (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao))
            {
                // Se usuário for de um grupo com visão Gestão, ou estiver setado "Sim"
                // no parâmetro FILTRAR_ESCOLA_UA_SUPERIOR.
                cvUnidadeAdministrativaSuperior.Visible = true;
                cvUnidadeAdministrativaSuperior.ErrorMessage = UCComboUnidadeAdministrativa1._Label.Text + " é obrigatório.";
                cvUnidadeAdministrativaSuperior.ValueToCompare = Guid.Empty.ToString();

                UCComboUnidadeAdministrativa1._Label.Text += " *";
            }
            else
            {
                cvUnidadeAdministrativaSuperior.Visible = false;
            }

        }
        else
        {
            if (UCComboUnidadeAdministrativa1.LoadByFiltroUASuperiorEscola(uad_id) ||
                (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao))
            {
                // Se usuário for de um grupo com visão Gestão, ou estiver setado "Sim"
                // no parâmetro FILTRAR_ESCOLA_UA_SUPERIOR.
                cvUnidadeAdministrativaSuperior.Visible = true;
                cvUnidadeAdministrativaSuperior.ErrorMessage = UCComboUnidadeAdministrativa1._Label.Text + " é obrigatório.";
                cvUnidadeAdministrativaSuperior.ValueToCompare = Guid.Empty.ToString();

                UCComboUnidadeAdministrativa1._Label.Text += " *";
            }
            else
            {
                cvUnidadeAdministrativaSuperior.Visible = false;
            }
        }
    }

    /// <summary>
    /// Insere e altera uma escola
    /// </summary>
    public void _Salvar()
    {
        try
        {            
            ESC_Escola entityEscola = new ESC_Escola
            {
                esc_id = _VS_esc_id,
                esc_nome = _txtNomeEscola.Text.Trim(),
                cid_id = UCEnderecos1._Cid_ids.Count > 0 ? UCEnderecos1._Cid_ids[0] : Guid.Empty,
                esc_codigo = _txtCodigoEscola.Text,
                esc_codigoInep = _txtCodigoInep.Text,
                tre_id = UCComboTipoRedeEnsino1.Valor,
                ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id == Guid.Empty ? new Guid() : __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                uad_id = _VS_uad_id,
                uad_idSuperiorGestao = _VS_uad_idSuperiorGestao,
                esc_funcionamentoInicio = Convert.ToDateTime(_txtFunIni.Text),
                esc_funcionamentoFim = string.IsNullOrEmpty(_txtFunFim.Text) ? new DateTime() : Convert.ToDateTime(_txtFunFim.Text),
                esc_fundoVerso = txtFundoVerso.Text,
                esc_situacao = UCComboEscolaSituacao1._Combo.SelectedValue == "-1" ? Convert.ToByte(null) : Convert.ToByte(UCComboEscolaSituacao1._Combo.SelectedValue),
                esc_controleSistema = _ckbControleSistema.Checked,
                esc_autorizada = Convert.ToByte(ddlRegulamentadaAutorizada.SelectedValue),
                esc_atoCriacao = _txtAtoCriacao.Text,
                esc_codigoNumeroMatricula = string.IsNullOrEmpty(_txtCodigoNumeroChamada.Text) ? -1 : Convert.ToInt32(_txtCodigoNumeroChamada.Text),
                esc_dataPublicacaoDiarioOficial = string.IsNullOrEmpty(_txtDataDiarioOficial.Text) ? new DateTime() : Convert.ToDateTime(_txtDataDiarioOficial.Text),
                IsNew = (_VS_esc_id > 0) ? false : true
            };

            if (ESC_EscolaBO.Save(entityEscola))
            {
                if (_VS_esc_id <= 0)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "esc_id: " + Convert.ToString(entityEscola.esc_id));
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Escola incluída com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "esc_id: " + Convert.ToString(entityEscola.esc_id));
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Escola alterada com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }

                Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/Escola/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a escola.", UtilBO.TipoMensagem.Erro);
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
            _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta).Replace("unidade administrativa", "escola");
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a escola.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Carrega os dados da escola nos controles caso seja alteração.
    /// </summary>
    /// <param name="esc_id">Id da escola.</param>
    private void _LoadFromEntity(int esc_id)
    {
        try
        {
            // Desabilitar campos que não podem ser alterados.
            UCComboTipoRedeEnsino1.PermiteEditar = false;
            UCComboTipoUAEscola1.PermiteEditar = false;
            
            VS_dataAcesso = DateTime.Now;

            // Carrega Escolas
            ESC_Escola esc = new ESC_Escola { esc_id = esc_id };
            ESC_EscolaBO.SelecionaEscola(esc);

            _VS_uad_idSuperiorGestao = esc.uad_idSuperiorGestao;

            ESC_UnidadeEscola unidEsc = new ESC_UnidadeEscola { esc_id = esc_id, uni_id = 1 };
            ESC_UnidadeEscolaBO.GetEntity(unidEsc);

            if (esc.ent_id != __SessionWEB.__UsuarioWEB.Usuario.ent_id)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("A escola não pertence à entidade na qual você está logado.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Academico/Escola/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            _VS_uad_id = esc.uad_id;

            _VS_cid_idAntigo = esc.cid_id;

            SYS_UnidadeAdministrativa uad = new SYS_UnidadeAdministrativa
            {
                ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                uad_id = _VS_uad_id
            };
            SYS_UnidadeAdministrativaBO.GetEntity(uad);

            // Verifica se existem outros registros ligados à escola, se existir, não 
            // será possível alterar a data de início de funcionamento.
            // Retorna se existem outros registros ligados à escola.
            if ((esc.esc_funcionamentoInicio.Date <= DateTime.Now.Date) &&
                GestaoEscolarUtilBO.VerificarIntegridade
                (
                    "esc_id",
                    esc.esc_id.ToString(),
                    "ESC_EscolaDiretor,ESC_Escola,ESC_EscolaOrgaoSupervisao,ESC_UnidadeEscola," +
                    "ESC_UnidadeEscolaContato,ESC_UnidadeEscolaEnsino,ESC_UnidadeEscolaPredio,REL_AlunosSituacaoFechamento",
                    null))
            {
                _txtFunIni.Enabled = false;
            }

            UCComboTipoUAEscola1.Valor = uad.tua_id;

            // Carrega o combo de ua superior com todas as UA's exceto a UA da própria escola.
            CarregarComboUASuperior(_VS_uad_idSuperiorGestao.Equals(Guid.Empty) ? _VS_uad_id : _VS_uad_idSuperiorGestao);

            Guid uadSuperior = esc.uad_idSuperiorGestao.Equals(Guid.Empty) ? uad.uad_idSuperior : esc.uad_idSuperiorGestao;

            // Unidade administrativa superior.
            UCComboUnidadeAdministrativa1._Combo.SelectedValue = UCComboUnidadeAdministrativa1._Combo.Items.FindByValue(uadSuperior.ToString()) != null ? uadSuperior.ToString() : Guid.Empty.ToString();

            _txtNomeEscola.Text = esc.esc_nome;
            _txtCodigoEscola.Text = esc.esc_codigo;
            _txtCodigoInep.Text = esc.esc_codigoInep;
            _txtCepProx.Text = unidEsc.uni_cepsProximos;
            _txtObs.Text = unidEsc.uni_observacao;
            UCComboTipoRedeEnsino1.Valor = esc.tre_id == Convert.ToInt32(null) ? -1 : esc.tre_id;
            _txtFunIni.Text = esc.esc_funcionamentoInicio.ToString("dd/MM/yyyy");
            _txtFunFim.Text = esc.esc_funcionamentoFim == new DateTime() ? string.Empty : esc.esc_funcionamentoFim.ToString("dd/MM/yyyy");
            txtFundoVerso.Text = esc.esc_fundoVerso;
            UCComboEscolaSituacao1._Combo.SelectedValue = esc.esc_situacao == Convert.ToInt32(null) ? "-1" : esc.esc_situacao.ToString();
            _ckbControleSistema.Checked = esc.esc_controleSistema;
            _ckbControleSistema.Enabled = !esc.esc_controleSistema;
            ckbTerceirizada.Checked = esc.esc_terceirizada;
            ckbTerceirizada.Enabled = false;
            ddlRegulamentadaAutorizada.SelectedValue = esc.esc_autorizada.ToString();
            _txtAtoCriacao.Text = esc.esc_atoCriacao;
            _txtCodigoNumeroChamada.Text = esc.esc_codigoNumeroMatricula > 0 ? esc.esc_codigoNumeroMatricula.ToString() : string.Empty;
            _txtDataDiarioOficial.Text = esc.esc_dataPublicacaoDiarioOficial == new DateTime() ? string.Empty : esc.esc_dataPublicacaoDiarioOficial.ToString("dd/MM/yyyy");
            _txtCodIntegracao.Text = uad.uad_codigoIntegracao;

            _txtNomeEscola.Enabled = false;
            _txtCodigoEscola.Enabled = false;

            // Carrega Órgãos de Supervisão
            List<ESC_EscolaOrgaoSupervisao> list = ESC_EscolaOrgaoSupervisaoBO.GetSelectBy_OrgaoSupervisor(esc_id);
            grvOrgaoSupervisao.DataSource = list;
            grvOrgaoSupervisao.DataBind();

            // Carrega Contatos
            UCContato1._VS_seq = ESC_UnidadeEscolaContatoBO.VerificaUltimoContatoCadastrado(_VS_esc_id, _VS_uni_id) - 1;
            UCContato1.CarregarContatosDeEscolaDoBanco(_VS_esc_id, _VS_uni_id);
            UCContato1.BloquearEdicao();

            // Carrega dados dos endereços da pessoa.
            DataTable dtEndereco = ESC_PredioEnderecoBO.SelecionaEndereco(uad.ent_id, uad.uad_id);
            Guid end_idPrincipal = Guid.Empty;

            if (dtEndereco.Select().Any(p => Convert.ToBoolean(p["uae_enderecoPrincipal"] != DBNull.Value ? p["uae_enderecoPrincipal"].ToString() : "false")))
            {
                end_idPrincipal = dtEndereco.Rows.Count == 1 ? new Guid(dtEndereco.Select().First()["end_id"].ToString()) :
                                  new Guid(dtEndereco.Select().First(p => Convert.ToBoolean(p["uae_enderecoPrincipal"] != DBNull.Value ? p["uae_enderecoPrincipal"].ToString() : "false"))["end_id"].ToString());
            }

            dtEndereco.Columns["uae_id"].ColumnName = "id";
            dtEndereco.Columns["uae_numero"].ColumnName = "numero";
            dtEndereco.Columns["uae_complemento"].ColumnName = "complemento";
            dtEndereco.AcceptChanges();

            UCEnderecos1.CarregarEnderecosComPrincipalBanco(dtEndereco, end_idPrincipal);
            UCEnderecos1.DesabilitarCamposEnderecos();

            // Carrega cursos da escola            
            DataTable dtCurso = ACA_CurriculoEscolaBO.CarregaCursosReferenteEscola(esc_id, false, 1, 1);
            _VS_CurriculoCursoPeriodo = ACA_CurriculoEscolaPeriodoBO.CarregaCursosPeriodoReferenteEscola(esc_id, false, 1, 1);

            VS_QuantidadeTurmaCurriculoMatricula = ACA_CurriculoEscolaBO.ExisteCurriculo_TurmaCurriculoMatricula(_VS_esc_id, _VS_uni_id);

            if (_VS_CurriculoCursoPeriodo.Rows.Count == 0)
            {
                _VS_CurriculoCursoPeriodo = null;
            }

            // Carrega os turnos
            DataTable dtTurnosEscola = ACA_CurriculoTurnoBO.SelectPorEscolas(_VS_esc_id, _VS_uni_id);
            List<DataRow> ltTurnosCurso;
            for (int j = 0; j < dtCurso.Rows.Count; j++)
            {
                List<ACA_CursoBO.TmpTurnos> lt = new List<ACA_CursoBO.TmpTurnos>();

                int cur_id = Convert.ToInt32(dtCurso.Rows[j]["cur_id"]);
                int crr_id = Convert.ToInt32(dtCurso.Rows[j]["crr_id"]);

                ltTurnosCurso = (from DataRow dr in dtTurnosEscola.Rows
                                 where dr["esc_id"].ToString().Equals(_VS_esc_id.ToString()) &&
                                       dr["uni_id"].ToString().Equals(_VS_uni_id.ToString()) &&
                                       dr["cur_id"].ToString().Equals(cur_id.ToString()) &&
                                       dr["crr_id"].ToString().Equals(crr_id.ToString())
                                 select dr
                                ).ToList();

                // Armazena os turnos carregados inicialmente para o curso em um viewstate para futura verificaçao.
                _VS_Turnos_Iniciais = ltTurnosCurso.Count > 0 ? ltTurnosCurso.CopyToDataTable() : new DataTable();

                foreach (DataRow dr in ltTurnosCurso)
                {
                    int crt_id = Convert.ToInt32(dr["crt_id"]);
                    int ttn_id = Convert.ToInt32(dr["ttn_id"]);
                    string ttn_nome = dr["ttn_nome"].ToString();
                    string vigenciaInicio = Convert.ToDateTime(dr["crt_vigenciaInicio"].ToString()).Date.ToString();
                    string vigenciaFim = string.IsNullOrEmpty(dr["crt_vigenciaFim"].ToString()) ? new DateTime().ToString() : Convert.ToDateTime(dr["crt_vigenciaFim"].ToString()).Date.ToString();
                    ACA_CursoBO.AddTmpTurnos(Convert.ToInt32(dtCurso.Rows[j]["cur_id"]), Convert.ToInt32(dtCurso.Rows[j]["crr_id"]), Convert.ToInt32(dtCurso.Rows[j]["esc_id"]), Convert.ToInt32(dtCurso.Rows[j]["uni_id"]), Convert.ToInt32(dtCurso.Rows[j]["ces_id"]), crt_id, ttn_id, ttn_nome, vigenciaInicio, vigenciaFim, false, lt);
                }

                _VS_tmpTurnos.Add(cur_id, lt);
            }

            if (dtCurso.Rows.Count == 0)
            {
                dtCurso = null;
            }

            _VS_Curso = dtCurso;
            if (_VS_Curso != null)
            {
                _VS_Curso.DefaultView.Sort = "cur_nome ASC";
                _grvCurso.DataSource = _VS_Curso;
                _grvCurso.DataBind();
            }

            // Carrega o calendário e os períodos selecionados
            DataTable dtPeriodos = ESC_EscolaCalendarioPeriodoBO.SelectEscolaPeriodos(_VS_esc_id);

            if (dtPeriodos.Rows.Count > 0)
            {
                // Seleciona o combo de calendário
                UCCCalendario1.Valor = Convert.ToInt32(dtPeriodos.Rows[0]["cal_id"].ToString());
                UCCCalendario_IndexChanged();

                // Seleciona os períodos
                foreach (ListItem item in chkTiposPeriodos.Items)
                {
                    bool existePeriodo = dtPeriodos.Rows.Cast<DataRow>().Any(row => row["tpc_id"].ToString().Equals(item.Value));
                    if (existePeriodo)
                    {
                        item.Selected = true;
                    }
                }
            }
            

        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a escola.", UtilBO.TipoMensagem.Erro);
        }
    }

    #region Autorização / Regulamentação

    /// <summary>
    /// Carrega as opções de Autorização / regulamentação da escola
    /// </summary>
    private void CarregaComboAutorizacao()
    {
        ddlRegulamentadaAutorizada.Items.Clear();

        IEnumerable<KeyValuePair<int, string>> x =
            Enum.GetValues(typeof(ESC_EscolaBO.Autorizada)).Cast<ESC_EscolaBO.Autorizada>().Select(
                p => new KeyValuePair<int, string>((int)p, StringValueAttribute.GetStringValue(p)));

        var t = from KeyValuePair<int, string> pair in x
                orderby pair.Key
                select new { tipo = pair.Key, desc = pair.Value };

        ddlRegulamentadaAutorizada.Items.Insert(0, new ListItem("-- Selecione uma opção --", "0"));
        ddlRegulamentadaAutorizada.DataSource = t;
        ddlRegulamentadaAutorizada.DataBind();
    }

    #endregion

    #region Turno

    /// <summary>
    /// Método para carregar dados no grid
    /// </summary>
    public void CarregaTurnos()
    {
        _grvTurnos.DataSource = _VS_Turnos;
        _grvTurnos.DataBind();

        foreach (GridViewRow linha in _grvTurnos.Rows)
        {
            string crt_id = _grvTurnos.DataKeys[linha.RowIndex].Values["crt_id"].ToString();

            int index = RetornaIndice(crt_id);

            WebControls_Combos_UCComboTipoTurno UCComboTipoTurno1 = (WebControls_Combos_UCComboTipoTurno)linha.FindControl("UCComboTipoTurno1");

            // Evita a chamada ao metodo de carregar os tipos de turno por mais de uma vez utilizando o viewstate _VS_Tipo_Turno
            UCComboTipoTurno1.Combo.DataSource = _VS_Tipo_Turno;
            UCComboTipoTurno1.Combo.DataBind();
            UCComboTipoTurno1.Valor = string.IsNullOrEmpty(_VS_Turnos.Rows[index]["ttn_id"].ToString()) ? -1 : Convert.ToInt32(_VS_Turnos.Rows[index]["ttn_id"].ToString());

            TextBox txtVigenciaInicioTurno = (TextBox)linha.FindControl("_txtVigenciaIniTurno");
            DateTime dtVigenciaIni = string.IsNullOrEmpty(_VS_Turnos.Rows[index]["crt_vigenciaInicio"].ToString()) ? new DateTime() : Convert.ToDateTime(_VS_Turnos.Rows[index]["crt_vigenciaInicio"]);
            txtVigenciaInicioTurno.Text = dtVigenciaIni != new DateTime() ? dtVigenciaIni.ToShortDateString() : string.Empty;

            TextBox txtVigenciaFimTurno = (TextBox)linha.FindControl("_txtVigenciaFimTurno");
            DateTime dtVigenciaFim = string.IsNullOrEmpty(_VS_Turnos.Rows[index]["crt_vigenciaFim"].ToString()) ? new DateTime() : Convert.ToDateTime(_VS_Turnos.Rows[index]["crt_vigenciaFim"]);
            txtVigenciaFimTurno.Text = dtVigenciaFim != new DateTime() ? dtVigenciaFim.ToShortDateString() : string.Empty;
        }

        _updGridTurno.Update();
    }

    /// <summary>
    /// Retorna o indice na tabela do viewstate pelo id informado.
    /// </summary>
    /// <param name="crt_id"></param>
    /// <returns>Índice da tabela.</returns>
    private int RetornaIndice(string crt_id)
    {
        int indice = -1;

        var x = from DataRow dr in _VS_Turnos.Select().Where(p => p.RowState != DataRowState.Deleted)
                where
                    dr["crt_id"].ToString().Equals(crt_id, StringComparison.OrdinalIgnoreCase)
                select _VS_Turnos.Rows.IndexOf(dr);

        if (x.Count() > 0)
        {
            indice = x.First();
        }

        return indice;
    }

    #endregion

    #region Curso

    private void _LimpaCamposCursos()
    {
        _VS_IsNewCurso = true;

        _txtVigenciaIniCurso.Text = string.Empty;
        _txtVigenciaFimCurso.Text = string.Empty;

        _txtVigenciaIniCurso.Enabled = true;
        _txtVigenciaFimCurso.Enabled = true;

        _UCComboCursoCurriculo1.Valor = new[] { -1, -1 };
        _VS_periodos.Clear();
        pnlPeriodo.Visible = false;

        _UCComboCursoCurriculo1.PermiteEditar = true;
        _txtVigenciaIniCurso.Enabled = true;

        _VS_Turnos.Clear();
        _grvTurnos.DataSource = _VS_Turnos;
        _grvTurnos.DataBind();

        var x = from DataRow dr in _VS_Curso.Select().Where(p => p.RowState != DataRowState.Deleted)
                orderby Convert.ToInt32(dr["ces_id"].ToString()) descending
                select Convert.ToInt32(dr["ces_id"].ToString());

        _VS_ces_id = x.Count() > 0 ? x.First() + 1 : 1;
    }

    #endregion

    #region Período

    private void _AdicionaPeriodos()
    {
        try
        {
            chkPeriodos.Items.Clear();

            for (int i = 0; i < _VS_periodos.Rows.Count; i++)
            {
                if (_VS_periodos.Rows[i].RowState != DataRowState.Deleted)
                {
                    ListItem lt = new ListItem
                    {
                        Value = _VS_periodos.Rows[i]["crp_id"].ToString(),
                        Text = _VS_periodos.Rows[i]["crp_descricao"].ToString(),
                        Selected = false
                    };
                    chkPeriodos.Items.Add(lt);
                    pnlPeriodo.Visible = true;
                }
            }
        }
        catch
        {
            _lblMessageCurso.Text = UtilBO.GetErroMessage("Erro ao adicionar os periodos na escola.", UtilBO.TipoMensagem.Erro);
        }
    }

    private void _CarregaPeriodos()
    {
        DataTable dtPeriodos = ACA_CurriculoPeriodoBO.GetSelect(_VS_cur_id, _VS_crr_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, false, 1, 1);
        if (dtPeriodos.Rows.Count == 0)
        {
            dtPeriodos = null;
        }

        _VS_periodos = dtPeriodos;

        //no método _AdicionaPeriodos mostra os períodos se tiver algum cadastrado
        pnlPeriodo.Visible = false;
    }

    #endregion

    #region Tipo de Classificação
    /// <summary>
    /// Cria lista de entidades de EscolaClassificacao de acordo com os tipos de classificação selecionados
    /// </summary>
    private List<ESC_EscolaClassificacao> CriarListaTiposClassificacao()
    {
        List<ESC_EscolaClassificacao> lt = new List<ESC_EscolaClassificacao>();

        //for (int i = 0; i < lstTiposClassificacaoAssociados.Items.Count; i++)
        //{
        //    ESC_EscolaClassificacao entity = new ESC_EscolaClassificacao { tce_id = Convert.ToInt32(lstTiposClassificacaoAssociados.Items[i].Value) };
        //    lt.Add(entity);
        //}

        foreach (RepeaterItem item in rptCampos.Items)
        {
            CheckBox ckbCampo = (CheckBox)item.FindControl("ckbCampo");
            if (ckbCampo != null && ckbCampo.Checked)
            {
                HiddenField hdnId = (HiddenField)item.FindControl("hdnId");
                if (hdnId != null)
                {
                    ESC_EscolaClassificacao entity = new ESC_EscolaClassificacao { tce_id = Convert.ToInt32(hdnId.Value) };
                    lt.Add(entity);
                }
            }
        }
        return lt;
    }

    /// <summary>
    /// Configura validação de classificação para tipos adicionados. 
    /// </summary>
    private void ConfiguraValidacaoClassificacao()
    {
        bool campoSelecionado = false;
        foreach (RepeaterItem item in rptCampos.Items)
        {
            CheckBox ckbCampo = (CheckBox)item.FindControl("ckbCampo");
            if (ckbCampo != null && ckbCampo.Checked)
            {
                campoSelecionado = true;
                break;
            }
        }

        if (campoSelecionado)
        {
            AdicionaAsteriscoObrigatorio(lblDataInicio);
            rfvDataInicio.Enabled = ctvDataInicio.Enabled = cpvDataInicio.Enabled = ctvDataFim.Enabled = true;
        }
        else
        {
            RemoveAsteriscoObrigatorio(lblDataInicio);
            rfvDataInicio.Enabled = ctvDataInicio.Enabled = cpvDataInicio.Enabled = ctvDataFim.Enabled = false;
        }
    }

    /// <summary>
    /// Cria lista de entidades de ESC_EscolaCalendarioPeriodo de acordo com os períodos selecionados
    /// </summary>
    private List<ESC_EscolaCalendarioPeriodo> CriarListaPeriodos()
    {
        List<ESC_EscolaCalendarioPeriodo> lt = new List<ESC_EscolaCalendarioPeriodo>();

        if (divImportacao.Visible)
        {
            foreach (ListItem item in chkTiposPeriodos.Items)
            {
                if (item.Selected)
                {
                    ESC_EscolaCalendarioPeriodo entity = new ESC_EscolaCalendarioPeriodo
                    {
                        cal_id = UCCCalendario1.Valor,
                        tpc_id = Convert.ToInt32(item.Value)
                    };
                    lt.Add(entity);
                }
            }
        }

        return lt;
    }

    #endregion

    #region Orgão de supervisão

    /// <summary>
    /// Lê todos os dados existentes na Grid grvOrgaoSupervisao e atribui para um List.
    /// </summary>
    /// <returns>List de entidade ESC_EscolaOrgaoSupervisao.</returns>
    private List<ESC_EscolaOrgaoSupervisao> readGrid(bool salvar)
    {
        List<ESC_EscolaOrgaoSupervisao> list = new List<ESC_EscolaOrgaoSupervisao>();

        try
        {
            foreach (GridViewRow row in grvOrgaoSupervisao.Rows)
            {
                ESC_EscolaOrgaoSupervisao campos = new ESC_EscolaOrgaoSupervisao
                {
                    esc_id = _VS_esc_id,
                    eos_id = Convert.ToInt32(((Label)row.FindControl("lblEos_id")).Text),
                    eos_nome = ((TextBox)row.FindControl("txtDescricao")).Text
                };

                WebControls_Combos_UCComboEntidadeGestao v_UCComboEntidadeGestao = (
                    WebControls_Combos_UCComboEntidadeGestao)row.FindControl("UCComboEntidadeGestao1");
                campos.ent_id = v_UCComboEntidadeGestao.Valor;

                TextBox txt_uad_id = (TextBox)row.FindControl("txt_uad_id");
                campos.uad_id = string.IsNullOrEmpty(txt_uad_id.Text) ? Guid.Empty : new Guid(txt_uad_id.Text);
                campos.uad_nome = ((TextBox)row.FindControl("txtUa")).Text;
                campos.eos_situacao = 1;

                if ((!string.IsNullOrEmpty(campos.eos_nome) && campos.ent_id != Guid.Empty) || !salvar)
                {
                    list.Add(campos);
                }
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar ler os dados na tabela.", UtilBO.TipoMensagem.Erro);
        }

        return list;
    }

    #endregion
    
    /// <summary>
    /// Bloqueia todos os campos do cadastro de escola, exceto os dados da Importação de Fechamento
    /// </summary>
    private void BloquearCadastroEscola()
    {
        HabilitaControles(fsEndereco.Controls, false);
        HabilitaControles(fsContatos.Controls, false);
        HabilitaControles(fdsClassificacao.Controls, false);
        HabilitaControles(fsDadosBasicos.Controls, false);
        HabilitaControles(fsOrgaoSupervisao.Controls, false);
        HabilitaControles(_fdsTurnos.Controls, false);
        HabilitaControles(fdsCurso.Controls, false);
        HabilitaControles(divCurriculoCurso.Controls, false);

        _txtObs.Enabled = false;
    }

    #endregion

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.UiAriaTabs));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsTabs.js"));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroEscola.js"));
            sm.Services.Add(new ServiceReference("~/WSServicos.asmx"));
        }

        Guid ent_id = Guid.Empty;
        if (Vs_index >= 0)
        {
            WebControls_Combos_UCComboEntidadeGestao UCComboEntidadeGestao1 =
                (WebControls_Combos_UCComboEntidadeGestao)grvOrgaoSupervisao.Rows[Vs_index].FindControl("UCComboEntidadeGestao1");
            ent_id = UCComboEntidadeGestao1.Valor;
        }

        if (!ACA_ParametroAcademicoBO.VerificaFiltroUniAdmSuperiorPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id))
        {
            UCComboUnidadeAdministrativa1.Visible = false;
        }

        if (!ACA_ParametroAcademicoBO.VerificaFiltroUniAdmSuperiorPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id))
        {
            UCComboUnidadeAdministrativa1.Visible = false;
        }
        
        bool visaoUA = __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa;

        //bool permiteImportacao = ACA_ParametroAcademicoBO.ParametroValorBooleano(eChaveAcademico.PERMITIR_IMPORTACAO_DADOS_EFETIVACAO);
        //divImportacao.Visible = permiteImportacao;
        //if (!permiteImportacao)
        //{
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "TabCadastroEscolaImportacao",
        //                                                "$('#liImportacao').hide();", true);
        //}

        if (Session["EscolaImportacaoFechamento"] != null && !string.IsNullOrEmpty(Session["EscolaImportacaoFechamento"].ToString()))
        {
            _VS_esc_id = Convert.ToInt32(Session["EscolaImportacaoFechamento"]);
            Session["EscolaImportacaoFechamento"] = null;
            _VS_esc_id_importacao = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_IMPORTACAO_DADOS_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }

        if (!_VS_esc_id_importacao)
        {
            divImportacao.Visible = liImportacao.Visible = false;
        }

        if (!IsPostBack)
        {
            cvFunIni.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de início do funcionamento");
            cvFunFim.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de fim do funcionamento");
            cvVigenciaIniCurso.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Vigência inicial do curso");
            cvVigenciaFimCurso.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Vigência final do curso");
            cvDataDiarioOficial.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data de publicação no diário oficial");

            UCEnderecos1.Inicializar(false, false, string.Empty, true, true, true, true);

            UCComboEscolaSituacao1._Label.Text += " *";
            
            // Preenche o dropdownlist de tipos de acordo com os valores do enumerador "Autorizada".
            CarregaComboAutorizacao();

            // seta o valor padrao do nome de curso para o sistema
            aCursos.InnerText = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            divCurriculoCurso.Attributes["title"] = "Cadastro de " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower();
            _grvCurso.EmptyDataText = "Não existe " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " cadastrado(a).";

            _UCComboCursoCurriculo1.ValidationGroup = "Curso";
            _UCComboCursoCurriculo1.Obrigatorio = true;
            _UCComboCursoCurriculo1.CarregarCursoCurriculo();
            UCCCalendario1.Carregar();
            UCCCalendario1.SelectedIndex = 0;

            pnlPeriodo.GroupingText = GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id);

            divFundoVerso.Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_FUNDO_CARTEIRINHA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            try
            {
                _VS_uni_id = 1;

                //if (Session["EscolaImportacaoFechamento"] != null && !string.IsNullOrEmpty(Session["EscolaImportacaoFechamento"].ToString()))
                //{
                //    _VS_esc_id = Convert.ToInt32(Session["EscolaImportacaoFechamento"]);
                //    Session["EscolaImportacaoFechamento"] = null;
                //    //divImportacao.Visible = true;
                //    _VS_esc_id_importacao = true;                    
                //}
                if ((PreviousPage != null) && PreviousPage.IsCrossPagePostBack)
                {
                    _VS_esc_id = PreviousPage.EditItem_esc_id;
                }
                else
                {
                    _btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
                }
                
                // carrega os dados no checkboxlist
                DataTable dtCampos = ESC_EscolaClassificacaoBO.SelecionaTipoClassificacaoNaoAssociado(_VS_esc_id);
                DataTable dtAssociados = ESC_EscolaClassificacaoBO.SelecionaTipoClassificacaoAssociado(_VS_esc_id, -1);
                dtCampos.Merge(dtAssociados);
                rptCampos.DataSource = dtCampos.AsEnumerable().OrderBy(r => r["tce_nome"])
                                        .Select(p => new { tce_id = p["tce_id"], tce_nome = p["tce_nome"] });
                rptCampos.DataBind();

                if (dtAssociados.Rows.Count > 0)    
                {
                    foreach (RepeaterItem item in rptCampos.Items)
                    {
                        CheckBox ckbCampo = (CheckBox)item.FindControl("ckbCampo");
                        HiddenField hdnId = (HiddenField)item.FindControl("hdnId");

                        if (ckbCampo != null && hdnId != null)
                        {
                            ckbCampo.Checked = dtAssociados.AsEnumerable().Any(r => Convert.ToInt32(r["tce_id"]) == Convert.ToInt32(hdnId.Value));
                        }
                    }

                    txtDataInicioClass.Text = dtAssociados.Rows[0]["ecv_dataInicio"].ToString();
                    txtDataFimClass.Text = string.IsNullOrEmpty(dtAssociados.Rows[0]["ecv_dataFim"].ToString()) ? string.Empty : dtAssociados.Rows[0]["ecv_dataFim"].ToString();
                }

                ConfiguraValidacaoClassificacao();
                fdsClassificacao.Visible = true;

                UCComboTipoRedeEnsino1.Obrigatorio = true;
                UCComboTipoRedeEnsino1.ValidationGroup = "escola";
                UCComboTipoRedeEnsino1.CarregarTipoRedeEnsino();

                UCContato1._VS_GuidSeq = false;

                UCComboTipoUAEscola1.CarregarTipoUAEscola();
                UCComboTipoUAEscola1.Obrigatorio = true;
                UCComboTipoUAEscola1.ValidationGroup = "escola";

                if (VerificaPermissaoUsuario())
                {
                    if (_VS_esc_id > 0)
                    {
                        _LoadFromEntity(_VS_esc_id);
                    }
                    else
                    {
                        // Seta o valor padrão como o selecionado para o combo de rede de ensino
                        int redeEnsinoPadrao = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.PAR_REDE_ENSINO_PADRAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                        if (redeEnsinoPadrao > 0)
                        {
                            UCComboTipoRedeEnsino1.Valor = Convert.ToInt32(redeEnsinoPadrao);
                        }

                        // Seta a cidade pelo endereço do usuário logado.
                        CarregarCidadeUsuarioLogado();

                        // Carrega o combo de ua superior com todas as UA's
                        CarregarComboUASuperior(Guid.Empty);

                        UCContato1.CarregarContatosDeEscolaDoBanco(_VS_esc_id, _VS_uni_id);

                        _grvCurso.DataSource = new DataTable();
                        _grvCurso.DataBind();
                    }
                }
                else
                {
                    Response.Redirect("~/Academico/Escola/Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o cadastro.", UtilBO.TipoMensagem.Erro);
            }

            bool podeEditar = (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && _VS_esc_id > 0) ||
                               (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && _VS_esc_id <= 0);

            if (!podeEditar)
            {
                if (!visaoUA)
                {
                    _btnCancelar.Text = "Voltar";
                    _btnSalvar.Visible = podeEditar;
                }
            }

            Page.Form.DefaultFocus = _VS_esc_id > 0 ? _txtCodigoEscola.ClientID : UCComboTipoUAEscola1.Combo_ClientID;
            Page.Form.DefaultButton = _btnSalvar.UniqueID;
        }

        bool podeEditarEscola = (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && _VS_esc_id > 0) ||
                               (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && _VS_esc_id <= 0);

        if (!podeEditarEscola)
        {
            if (!visaoUA)
            {
                HabilitaControles(fsEndereco.Controls, false);
                HabilitaControles(fsContatos.Controls, false);
                HabilitaControles(fdsClassificacao.Controls, false);
                HabilitaControles(fsDadosBasicos.Controls, false);
                HabilitaControles(fsOrgaoSupervisao.Controls, false);
                HabilitaControles(_fdsTurnos.Controls, podeEditarEscola);
                HabilitaControles(fdsCurso.Controls, podeEditarEscola);
                HabilitaControles(divCurriculoCurso.Controls, podeEditarEscola);
                HabilitaControles(fdsImportacao.Controls, false);
            }
        }
        SetaDelegates();

        if (_VS_esc_id_importacao)
        {
            // Bloqueia todos os campos do cadastro da escola
            BloquearCadastroEscola();

            // Seleciona a aba Importação de Fechamento
            hdnTab.Value = INDEX_ABA_IMPORTACAO;

            divImportacao.Visible = liImportacao.Visible = true;

            _btnCancelar.Text = "Cancelar";
            _btnSalvar.Visible = true;
            HabilitaControles(fdsImportacao.Controls, true);
        }
        else
        {
            divImportacao.Visible = liImportacao.Visible = false;
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            UCEnderecos1.VS_LocalizacaoGeografica = true;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    #region Curso

    protected void _grvCurso_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[3].Text = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Repeater rpt = (Repeater)e.Row.FindControl("_rptTipoTurno");
            if (rpt != null)
            {
                int cur_id = Convert.ToInt32(e.Row.Cells[1].Text);
                if (_VS_tmpTurnos.ContainsKey(cur_id))
                {
                    rpt.DataSource = _VS_tmpTurnos[cur_id];
                    rpt.DataBind();
                }
            }

            LinkButton btnAlterar = (LinkButton)e.Row.FindControl("_btnAlterarCurso");
            if (btnAlterar != null)
            {
                btnAlterar.CommandArgument = e.Row.RowIndex.ToString();
                btnAlterar.Visible = true;
            }

            Label lblAlterar = (Label)e.Row.FindControl("_lblAlterarCurso");
            if (lblAlterar != null)
            {
                lblAlterar.Visible = false;
            }
        }
    }

    protected void _grvCurso_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Alterar")
        {
            int index = int.Parse(e.CommandArgument.ToString());

            _VS_Turnos.Clear();
            _VS_IsNewCurso = false;
            _VS_ces_id = Convert.ToInt32(_grvCurso.Rows[index].Cells[0].Text);
            _VS_cur_id = Convert.ToInt32(_grvCurso.Rows[index].Cells[1].Text);
            _VS_crr_id = Convert.ToInt32(_grvCurso.Rows[index].Cells[2].Text);

            _CarregaPeriodos();
            _AdicionaPeriodos();

            List<DataRow> ltCursos = (from DataRow dr in _VS_Curso.Select().Where(p => p.RowState != DataRowState.Deleted)
                                      where dr["cur_id"].ToString() == _VS_cur_id.ToString()
                                      select dr).ToList();

            if (ltCursos.Count > 0)
            {
                _UCComboCursoCurriculo1.Valor = new[] { Convert.ToInt32(ltCursos[0]["cur_id"]), Convert.ToInt32(ltCursos[0]["crr_id"]) };
                _UCComboCursoCurriculo1.PermiteEditar = false;
                _txtVigenciaIniCurso.Text = Convert.ToDateTime(ltCursos[0]["ces_vigenciaInicio"].ToString()).ToString("dd/MM/yyyy");

                if (_VS_esc_id > 0)
                {
                    _txtVigenciaIniCurso.Enabled = Convert.ToDateTime(_txtVigenciaIniCurso.Text) > DateTime.Now.Date;
                }
                else
                {
                    _txtVigenciaIniCurso.Enabled = true;
                }

                _txtVigenciaFimCurso.Text = string.IsNullOrEmpty(ltCursos[0]["ces_vigenciaFim"].ToString()) || ltCursos[0]["ces_vigenciaFim"].ToString() == new DateTime().ToString() ? string.Empty : Convert.ToDateTime(ltCursos[0]["ces_vigenciaFim"].ToString()).ToString("dd/MM/yyyy");

                foreach (ListItem item in chkPeriodos.Items)
                {
                    item.Selected = false;
                }

                List<DataRow> ltCurriculoCursoPeriodo = (from DataRow dr in _VS_CurriculoCursoPeriodo.Select().Where(p => p.RowState != DataRowState.Deleted)
                                                         where dr["cur_id"].ToString() == _VS_cur_id.ToString()
                                                         select dr).ToList();

                foreach (ListItem item in ltCurriculoCursoPeriodo.SelectMany(row => chkPeriodos.Items.Cast<ListItem>().Where(i => i.Value == row["crp_id"].ToString())))
                {
                    item.Selected = true;
                }

                if (_VS_tmpTurnos.ContainsKey(_VS_cur_id))
                {
                    for (int u = 0; u < _VS_tmpTurnos[_VS_cur_id].Count; u++)
                    {
                        DataRow dr = _VS_Turnos.NewRow();
                        dr["cur_id"] = _VS_cur_id;
                        dr["crr_id"] = _VS_crr_id;
                        dr["esc_id"] = _VS_esc_id;
                        dr["uni_id"] = _VS_uni_id;
                        dr["ces_id"] = _VS_ces_id;
                        dr["crt_id"] = _VS_tmpTurnos[_VS_cur_id][u].crt_id;
                        dr["ttn_id"] = _VS_tmpTurnos[_VS_cur_id][u].ttn_id;
                        dr["ttn_nome"] = _VS_tmpTurnos[_VS_cur_id][u].ttn_nome;
                        dr["crt_vigenciaInicio"] = _VS_tmpTurnos[_VS_cur_id][u].crt_vigenciaInicio;
                        dr["crt_vigenciaFim"] = _VS_tmpTurnos[_VS_cur_id][u].crt_vigenciaFim;

                        if (!string.IsNullOrEmpty(_VS_tmpTurnos[_VS_cur_id][u].crt_vigenciaFim) && _VS_tmpTurnos[_VS_cur_id][u].crt_vigenciaInicio == _VS_tmpTurnos[_VS_cur_id][u].crt_vigenciaFim)
                        {
                            if (Convert.ToDateTime(_VS_tmpTurnos[_VS_cur_id][u].crt_vigenciaFim) != new DateTime())
                            {
                                dr["crt_vigencia"] = _VS_tmpTurnos[_VS_cur_id][u].crt_vigenciaInicio.Split(' ')[0];
                            }
                        }
                        else if (!string.IsNullOrEmpty(_VS_tmpTurnos[_VS_cur_id][u].crt_vigenciaFim) && _VS_tmpTurnos[_VS_cur_id][u].crt_vigenciaInicio != _VS_tmpTurnos[_VS_cur_id][u].crt_vigenciaFim)
                        {
                            if (Convert.ToDateTime(_VS_tmpTurnos[_VS_cur_id][u].crt_vigenciaFim) != new DateTime())
                            {
                                dr["crt_vigencia"] = _VS_tmpTurnos[_VS_cur_id][u].crt_vigenciaInicio.Split(' ')[0] + " - " + _VS_tmpTurnos[_VS_cur_id][u].crt_vigenciaFim.Split(' ')[0];
                            }
                            else
                            {
                                dr["crt_vigencia"] = _VS_tmpTurnos[_VS_cur_id][u].crt_vigenciaInicio.Split(' ')[0] + " - *";
                            }
                        }
                        else
                        {
                            dr["crt_vigencia"] = _VS_tmpTurnos[_VS_cur_id][u].crt_vigenciaInicio.Split(' ')[0] + " - *";
                        }

                        dr["isNew"] = _VS_tmpTurnos[_VS_cur_id][u].isNew;
                        _VS_Turnos.Rows.Add(dr);
                    }
                    CarregaTurnos();
                }

                HabilitaControles(divCurriculoCurso.Controls, false);
                HabilitaControles(_fdsTurnos.Controls, false);
                _UCComboCursoCurriculo1.PermiteEditar = false;
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "CadastroCurriculoCurso", "$('.divCurriculoCurso').dialog('open');", true);
            }
        }
    }

    #endregion

    /// <summary>
    /// Método para validar os CEPs próximos à escola
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cv_CepProx_ServerValidate(object source, ServerValidateEventArgs args)
    {
        // expressão regular para testar os ceps
        Regex regex = new Regex(@"^\d{8}$");

        // vetor de strings contendo os ceps
        string[] splittedCeps = _txtCepProx.Text.Split(',');

        // número de ceps com erros
        int cepsErrados = 0;

        // percorre o vetor de ceps
        foreach (string cep in splittedCeps)
        {
            // verifica se o cep não está vazio
            if (!string.IsNullOrEmpty(cep))
            {
                // se não passar pela expressão regular incrementa o números de ceps errados
                if (!regex.IsMatch(cep.Trim()))
                {
                    cepsErrados++;
                }
            }
        }

        // se existirem ceps com erros invalida o controle
        if (cepsErrados > 0)
        {
            cv_CepProx.ErrorMessage = string.Format("Existe(m) {0} CEP(s) errado(s).", cepsErrados);
            args.IsValid = false;
        }
    }

    protected void _btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/Escola/Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void _btnSalvar_Click(object sender, EventArgs e)
    {
        // o sistema deve salvar APENAS estas informações da parametrização da importação, ou seja, não deve validar nenhuma informação do cadastro da escola.
        if (_VS_esc_id_importacao)
        {
            List<ESC_EscolaCalendarioPeriodo> listPeriodos = CriarListaPeriodos();

            if (ESC_EscolaCalendarioPeriodoBO.SaveList(_VS_esc_id, listPeriodos))
            {
                //    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "esc_id: " + Convert.ToString(entityEscola.esc_id));                

                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Dados da importação de fechamento da escola salvos com sucesso.", UtilBO.TipoMensagem.Sucesso);

                Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/Escola/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a escola.", UtilBO.TipoMensagem.Erro);
            }
        }
        else
        {
            _Salvar();
        }
    }

    #region Orgão de supervisão

    protected void grvOrgaoSupervisao_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            WebControls_Combos_UCComboEntidadeGestao UCComboEntidadeGestao1 =
                (WebControls_Combos_UCComboEntidadeGestao)e.Row.FindControl("UCComboEntidadeGestao1");

            if (UCComboEntidadeGestao1 != null)
            {
                UCComboEntidadeGestao1.Obrigatorio = true;
                UCComboEntidadeGestao1.ValidationGroup = "OrgaoSupervisao";
                UCComboEntidadeGestao1.ExibeTitulo = false;
                UCComboEntidadeGestao1.Carregar();

                Guid ent_id = new Guid(DataBinder.Eval(e.Row.DataItem, "ent_id").ToString());
                if (ent_id != new Guid())
                {
                    UCComboEntidadeGestao1.Valor = ent_id;
                }

                TextBox txtUa = (TextBox)e.Row.FindControl("txtUa");
                if (txtUa != null)
                {
                    txtUa.Enabled = false;
                }
            }

            TextBox txt = (TextBox)e.Row.FindControl("txtUa");
            if (txt != null && (DataBinder.Eval(e.Row.DataItem, "uad_nome") != null))
            {
                txt.Text = DataBinder.Eval(e.Row.DataItem, "uad_nome").ToString();
            }
            else if (txt != null)
            {
                txt.Text = string.Empty;
            }
        }
    }

    #endregion
    
    #endregion
}

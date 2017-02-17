using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.BLL.Caching;

namespace GestaoEscolar.Classe.CompensacaoAusencia
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Propriedade em ViewState que armazena valor de cpa_id (ID da compensacao)
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_cpa_id
        {
            get
            {
                if (ViewState["VS_cpa_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_cpa_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_cpa_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena o numero de aulas compensadas
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_QtAulasComp
        {
            get
            {
                if (ViewState["VS_QtAulasComp"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_QtAulasComp"]);
                }

                return 0;
            }

            set
            {
                ViewState["VS_QtAulasComp"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de doc_id
        /// </summary>
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
        /// Guarda a posição do docente.
        /// </summary>
        private byte VS_posicao
        {
            get
            {
                if (ViewState["VS_posicao"] == null)
                {
                    return 0;
                }

                return Convert.ToByte(ViewState["VS_posicao"]);
            }

            set
            {
                ViewState["VS_posicao"] = value;
            }
        }

        /// <summary>
        /// Flag que indica se a disciplina é oferecia para alunos de libras.
        /// </summary>
        private bool VS_DisciplinaEspecial
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_DisciplinaEspecial"] ?? false);
            }

            set
            {
                ViewState["VS_DisciplinaEspecial"] = value;
            }
        }

        /// <summary>
        /// Flag que indica se o fechamento da turma é automático.
        /// </summary>
        private bool VS_FechamentoAutomatico
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_FechamentoAutomatico"] ?? false);
            }

            set
            {
                ViewState["VS_FechamentoAutomatico"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o tipo de docente logado.
        /// </summary>
        private EnumTipoDocente VS_tipoDocente
        {
            get
            {
                return (EnumTipoDocente)(ViewState["VS_tipoDocente"] ?? 0);
            }

            set
            {
                ViewState["VS_tipoDocente"] = value;
            }
        }

        /// <summary>
        /// Viewstate que armazena o ID do protocolo do diário de classe que gerou a compensação de ausência.
        /// </summary>
        private Guid VS_pro_id
        {
            get
            {
                return new Guid((ViewState["VS_pro_id"] ?? Guid.Empty.ToString()).ToString());
            }

            set
            {
                ViewState["VS_pro_id"] = value;
            }
        }

        bool mostraSalvar = true;

        #endregion Propriedades

        #region Eventos Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            }

            // Seta o metodo criado no delegate ao evento do componente
            UCComboUAEscola.IndexChangedUnidadeEscola += UCComboUAEscola_IndexChangedUnidadeEscola;
            UCComboUAEscola.IndexChangedUA += UCComboUAEscola_IndexChangedUA;
            UCComboCalendario.IndexChanged += UCComboCalendario_IndexChanged;
            UCCCursoCurriculo.IndexChanged += UCCCursoCurriculo_IndexChanged;
            UCCTurmaDisciplina1.IndexChanged += UCComboTurmaDisciplina_IndexChanged;
            UCCPeriodoCalendario.IndexChanged += UCCPeriodoCalendario_IndexChanged;

            if (!IsPostBack)
            {
                try
                {
                    ddlTurma.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1", true));
                    //Esconde o botão salvar
                    btnSalvar.Visible = false;

                    if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                    {
                        // Busca o doc_id do usuário logado.
                        if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                        {
                            //Seta o docente
                            _VS_doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;

                            //Esconde os campos não visíveis para docentes
                            UCCCursoCurriculo.Visible = false;
                            ddlTurma.Enabled = false;

                            //Carrega as escolas no combo
                            UCComboUAEscola.InicializarVisaoIndividual(_VS_doc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                            if (UCComboUAEscola.QuantidadeItemsComboEscolas == 2)
                            {
                                ddlTurma.Enabled = true;
                                InicializaCamposCadastroVisaoIndividual(UCComboUAEscola.Esc_ID);
                            }
                            else
                                InicializaCamposCadastroVisaoIndividual(0);
                        }
                        else
                        {
                            divPesquisa.Visible = false;
                            lblMessage.Text = UtilBO.GetErroMessage("Essa tela é exclusiva para docentes.", UtilBO.TipoMensagem.Alerta);
                        }
                    }
                    else
                    {
                        //Inicializa os campos de cadastro
                        InicializaCamposCadastro();
                    }

                    bool docente = _VS_doc_id > 0;

                    if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                    {
                        long[] pp = PreviousPage.Edit_cpa_id;
                        Carregar(pp[0], pp[1]);
                        //Exibe o botão salvar nas alterações
                        btnSalvar.Visible = (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar ||
                                             __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir) && mostraSalvar;
                    }
                    else if (Session["PaginaRetorno_CompensacaoAusencia"] != null)
                    {
                        VS_PaginaRetorno = Session["PaginaRetorno_CompensacaoAusencia"].ToString();
                        Session.Remove("PaginaRetorno_CompensacaoAusencia");
                        VS_DadosPaginaRetorno = Session["DadosPaginaRetorno"];
                        Session.Remove("DadosPaginaRetorno");

                        VS_DadosPaginaRetorno_MinhasTurmas = Session["VS_DadosTurmas"];
                        Session.Remove("VS_DadosTurmas");

                        if (docente)
                        {
                            Dictionary<string, string> dadosPaginaRetorno = (Dictionary<string, string>)VS_DadosPaginaRetorno;

                            if (UCComboUAEscola.QuantidadeItemsComboEscolas != 2)
                            {
                                UCComboUAEscola.SelectedValueEscolas = new int[] { Convert.ToInt32(dadosPaginaRetorno["Edit_esc_id"]), Convert.ToInt32(dadosPaginaRetorno["Edit_uni_id"]) };
                                InicializaCamposCadastroVisaoIndividual(UCComboUAEscola.Esc_ID);
                                UCComboUAEscola_IndexChangedUnidadeEscola();
                            }

                            UCComboCalendario.Valor = Convert.ToInt32(dadosPaginaRetorno["Edit_cal_id"]);
                            UCComboCalendario_IndexChanged();
                            UCComboCalendario.PermiteEditar = false;

                            ddlTurma.SelectedValue = dadosPaginaRetorno["Edit_tur_id"];
                            ddlTurma_SelectedIndexChanged(null, null);
                            ddlTurma.Enabled = false;

                            if (UCComboUAEscola.Esc_ID == -1)
                            {
                                TUR_Turma tur = TUR_TurmaBO.GetEntity(new TUR_Turma { tur_id = Convert.ToInt64(dadosPaginaRetorno["Edit_tur_id"]) });
                                UCComboUAEscola.SelectedValueEscolas = new[] { tur.esc_id, tur.uni_id };
                                UCComboUAEscola.PermiteAlterarCombos = false;
                            }

                            UCCTurmaDisciplina1.Valor = Convert.ToInt64(dadosPaginaRetorno["Tud_idRetorno_ControleTurma"]);
                            UCComboTurmaDisciplina_IndexChanged();
                            UCCTurmaDisciplina1.PermiteEditar = false;

                            //Não tem períodos abertos para lançar compensação, retornar para tela anterior.
                            if (UCCPeriodoCalendario.QuantidadeItensCombo == 1)
                            {
                                __SessionWEB.PostMessages =
                                    UtilBO.GetErroMessage
                                    ("Não é possível criar compensação, pois o bimestre não está aberto para edição."
                                        , UtilBO.TipoMensagem.Alerta);
                                VerificaPaginaRedirecionar();

                            }
                        }
                        else
                        {
                            // Se veio da tela de Minhas turmas e não é docente, redireciona pra busca.
                            __SessionWEB.PostMessages = UtilBO.GetErroMessage("Operação exclusiva para docentes.", UtilBO.TipoMensagem.Alerta);
                            Response.Redirect("~/Classe/CompensacaoAusencia/Busca.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        }
                    }

                    if (docente)
                    {
                        Page.Form.DefaultFocus = UCCPeriodoCalendario.ClientID_Combo;
                    }
                    else
                    {
                        Page.Form.DefaultFocus = UCComboUAEscola.ComboUA_ClientID;
                    }

                    Page.Form.DefaultButton = btnSalvar.UniqueID;

                    //Nesse ponto, verifico a permissão apenas de alteração
                    if (VS_cpa_id > 0)
                        btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && mostraSalvar;
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        #endregion Eventos Page Life Cycle

        #region Métodos

        /// <summary>
        /// Método para carregar um registro de compensacao, a fim de atualizar suas informações.
        /// Recebe dados referente a compensacao para realizar busca.
        /// </summary>
        /// <param name="cpa_id">ID da compensacao</param>
        public void Carregar(long cpa, long tud)
        {
            try
            {
                int cpa_id = Convert.ToInt32(cpa.ToString());

                // Armazena valor ID do informativo a ser alterada.
                VS_cpa_id = cpa_id;

                // Busca do informativo baseado no ID do informativo.
                CLS_CompensacaoAusencia entCompensacao = new CLS_CompensacaoAusencia { cpa_id = cpa_id, tud_id = tud };
                CLS_CompensacaoAusenciaBO.GetEntity(entCompensacao);

                VS_pro_id = entCompensacao.pro_id;

                VS_QtAulasComp = entCompensacao.cpa_quantidadeAulasCompensadas;

                DataTable dt = CLS_CompensacaoAusenciaBO.RetornaIdsCadastro(entCompensacao.tud_id, entCompensacao.cpa_id);

                // Pega somente a primeira linha
                DataRow row = dt.Rows[0];

                Guid uad_id;
                int esc_id, uni_id, cur_id, crr_id, cap_id, tpc_id, cal_id;
                long tur_id, crp_id, ttn_id, tud_id;
                bool fav_fechamentoAutomatico;

                uad_id = string.IsNullOrEmpty(row[1].ToString()) ? new Guid() : new Guid(row[1].ToString());
                esc_id = Convert.ToInt32(row[2].ToString());
                uni_id = Convert.ToInt32(row[3].ToString());
                cur_id = Convert.ToInt32(row[4].ToString());
                crr_id = Convert.ToInt32(row[5].ToString());
                cap_id = Convert.ToInt32(row[10].ToString());
                tpc_id = Convert.ToInt32(row[11].ToString());
                cal_id = Convert.ToInt32(row[12].ToString());
                tur_id = Convert.ToInt64(row[7].ToString());
                crp_id = Convert.ToInt64(row[6].ToString());
                ttn_id = Convert.ToInt64(row[8].ToString());
                tud_id = Convert.ToInt64(row[9].ToString());
                fav_fechamentoAutomatico = Convert.ToBoolean(row["fav_fechamentoAutomatico"].ToString());

                VS_FechamentoAutomatico = fav_fechamentoAutomatico;

                if (_VS_doc_id <= 0)
                {
                    //CRE / Escola
                    UCComboUAEscola.Inicializar();
                    if (!uad_id.Equals(new Guid()))
                        UCComboUAEscola.Uad_ID = uad_id;
                    UCComboUAEscola.MostraApenasAtivas = true;
                    UCComboUAEscola.SelectedValueEscolas = new int[] { esc_id, uni_id };
                    UCComboUAEscola.PermiteAlterarCombos = false;

                    //Calendario
                    UCComboCalendario.CarregarCalendariosComBimestresAtivos(esc_id, true);
                    UCComboCalendario.Valor = cal_id;

                    //Etapa de ensino
                    UCCCursoCurriculo.CarregarPorEscolaSituacaoCurso(UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID, 0);
                    UCCCursoCurriculo.PermiteEditar = false;
                    UCCCursoCurriculo.Valor = new int[] { cur_id, crr_id };

                    ddlTurma.Items.Clear();
                    ddlTurma.DataTextField = "tur_codigo";

                    ddlTurma.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1", true));

                    ddlTurma.DataSource = TUR_TurmaBO.GetSelectBy_Escola_Periodo_Situacao(__SessionWEB.__UsuarioWEB.Usuario.usu_id,
                                                                                          __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                                                                                          (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
                                                                                          UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID,
                                                                                          UCComboCalendario.Valor, UCCCursoCurriculo.Valor[0],
                                                                                          UCCCursoCurriculo.Valor[1], -1,
                                                                                          __SessionWEB.__UsuarioWEB.Usuario.ent_id, 0, 0,
                                                                                          ApplicationWEB.AppMinutosCacheLongo)
                                          .GroupBy(p => new { tur_id = p.tur_id, tur_codigo = p.tur_codigo }).Select(p => p.Key).ToList(); ;
                    ddlTurma.DataBind();
                }
                else
                {
                    UCComboUAEscola.SelectedValueEscolas = new int[] { esc_id, uni_id };
                    UCComboUAEscola.PermiteAlterarCombos = false;

                    //Calendario
                    UCComboCalendario.CarregarCalendariosComBimestresAtivos(esc_id, true);
                    UCComboCalendario.Valor = cal_id;
                    UCComboCalendario.PermiteEditar = false;

                    //Carrega os campos
                    int posicaoDocenteCompatilhado = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.POSICAO_DOCENCIA_COMPARTILHADA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    ddlTurma.Items.Clear();
                    ddlTurma.DataTextField = "tur_esc_nome";

                    ddlTurma.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1", true));

                    ddlTurma.DataSource = TUR_TurmaBO.GetSelectBy_Docente_TodosTipos_Posicao(__SessionWEB.__UsuarioWEB.Usuario.ent_id, _VS_doc_id, posicaoDocenteCompatilhado, 0, UCComboCalendario.Valor, true, false, ApplicationWEB.AppMinutosCacheLongo)
                                          .GroupBy(p => new { tur_id = p.tur_id, tur_esc_nome = p.tur_esc_nome }).Select(p => p.Key).ToList();
                    ddlTurma.DataBind();
                }

                ddlTurma_SelectedIndexChanged(null, null);

                ddlTurma.Enabled = false;
                ddlTurma.SelectedValue = tur_id.ToString();

                //Disciplina
                if (_VS_doc_id <= 0)
                    UCCTurmaDisciplina1.CarregarTurmaDisciplina(tur_id);
                else
                    UCCTurmaDisciplina1.CarregarTurmaDisciplina(tur_id, _VS_doc_id);

                UCCTurmaDisciplina1.PermiteEditar = false;
                UCCTurmaDisciplina1.Valor = tud_id;

                //Periodo Calendario
                UCCPeriodoCalendario.CarregarPorPeriodoEventoEfetivacaoVigentes(cal_id, tud_id, tur_id);
                UCCPeriodoCalendario.PermiteEditar = false;
                UCCPeriodoCalendario.Valor = new int[2] { cap_id, tpc_id };
                UCCPeriodoCalendario_IndexChanged();

                // Só habilita os campos de quantidade de aulas compensadas e alunos selecionados, 
                // na edicao de uma compensacao do último bimestre "aberto" para edição.
                bool selecaoUltimoBimestre = UCCPeriodoCalendario.SelecaoUltimoBimestre();
                txtQtAulas.Enabled = selecaoUltimoBimestre;

                UCComboCalendario.PermiteEditar = false;

                if (UCCPeriodoCalendario.Tpc_ID > 0)
                {
                    // Atividades
                    txtAtividades.Text = entCompensacao.cpa_atividadesDesenvolvidas;

                    // Qt Aulas
                    txtQtAulas.Text = entCompensacao.cpa_quantidadeAulasCompensadas.ToString();

                    // Alunos compensados
                    List<CLS_CompensacaoAusenciaAluno> listaAlunos = CLS_CompensacaoAusenciaAlunoBO.SelectByCpa_id(entCompensacao.cpa_id, entCompensacao.tud_id);
                    foreach (RepeaterItem item in rptAlunos.Items)
                    {
                        CheckBox ckbAluno = (CheckBox)item.FindControl("ckbAluno");
                        HiddenField hdnId = (HiddenField)item.FindControl("hdnId");

                        if (ckbAluno != null && hdnId != null)
                        {
                            ckbAluno.Enabled = selecaoUltimoBimestre;
                            ckbAluno.Checked = listaAlunos.Any(p => string.Concat(p.tud_id, ";", p.alu_id, ";", p.mtu_id, ";", p.mtd_id) == hdnId.Value);
                        }
                    }
                }
                else
                {
                    // Voltar pra busca, pois não é possível editar uma compensação de um bimestre não aberto.
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Não é possível editar a compensação, pois o bimestre não está aberto para edição."
                        , UtilBO.TipoMensagem.Alerta);
                    Response.Redirect("~/Classe/CompensacaoAusencia/Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a compensação de ausência.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega os campos de busca com a configuração padrão para uma nova busca
        /// </summary>
        private void InicializaCamposCadastro()
        {
            //Carrega os campos
            UCComboUAEscola.Inicializar();

            //if (UCComboUAEscola.VisibleUA)
            UCComboUAEscola_IndexChangedUA();
        }

        /// <summary>
        /// Carrega os campos de busca com a configuração padrão para uma nova busca para visão individual
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        private void InicializaCamposCadastroVisaoIndividual(int esc_id)
        {
            //Carrega os campos

            int posicaoDocenteCompatilhado = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.POSICAO_DOCENCIA_COMPARTILHADA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            ddlTurma.Items.Clear();
            ddlTurma.DataTextField = "tur_esc_nome";

            ddlTurma.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1", true));

            ddlTurma.DataSource = TUR_TurmaBO.GetSelectBy_Docente_TodosTipos_Posicao(__SessionWEB.__UsuarioWEB.Usuario.ent_id, _VS_doc_id, posicaoDocenteCompatilhado, esc_id, UCComboCalendario.Valor, false, false, ApplicationWEB.AppMinutosCacheLongo)
                                  .GroupBy(p => new { tur_id = p.tur_id, tur_esc_nome = p.tur_esc_nome }).Select(p => p.Key).ToList();
            ddlTurma.DataBind();

            //if (UCComboUAEscola.VisibleUA)
            ddlTurma_SelectedIndexChanged(null, null);
        }

        /// <summary>
        /// Método para salvar um informativo.
        /// </summary>
        private void Salvar()
        {
            try
            {
                bool permiteEditar = true;

                if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                {
                    long tud_id = UCCTurmaDisciplina1.Valor;
                    byte tdt_posicao = TUR_TurmaDocenteBO.SelecionaPosicaoPorDocenteTurma(__SessionWEB.__UsuarioWEB.Docente.doc_id, tud_id, ApplicationWEB.AppMinutosCacheLongo);
                    permiteEditar = CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(tdt_posicao, (byte)EnumModuloPermissao.Compensacoes)
                                    .Any(p => p.pdc_permissaoEdicao);
                }

                if (permiteEditar)
                {
                    if (Convert.ToInt32(txtQtAulas.Text) == 0)
                        throw new ValidationException("Quantidade de aulas compensadas deve ser um número maior do que zero.");

                    CLS_CompensacaoAusencia entCompensacao = new CLS_CompensacaoAusencia();
                    entCompensacao.cpa_id = VS_cpa_id;
                    entCompensacao.tud_id = UCCTurmaDisciplina1.Valor;
                    entCompensacao.tpc_id = UCCPeriodoCalendario.Valor[0];
                    entCompensacao.cpa_atividadesDesenvolvidas = txtAtividades.Text;
                    entCompensacao.cpa_quantidadeAulasCompensadas = Convert.ToInt32(txtQtAulas.Text);
                    entCompensacao.pro_id = VS_pro_id;
                    entCompensacao.cpa_situacao = 1;
                    entCompensacao.IsNew = VS_cpa_id < 0;

                    List<CLS_CompensacaoAusenciaAluno> listCompensacaoAluno = new List<CLS_CompensacaoAusenciaAluno>();

                    foreach (RepeaterItem item in rptAlunos.Items)
                    {
                        CheckBox ckbAluno = (CheckBox)item.FindControl("ckbAluno");
                        if (ckbAluno != null && ckbAluno.Checked)
                        {
                            HiddenField hdnId = (HiddenField)item.FindControl("hdnId");
                            if (hdnId != null)
                            {
                                string[] valor = hdnId.Value.Split(';');
                                CLS_CompensacaoAusenciaAluno compAluno = new CLS_CompensacaoAusenciaAluno
                                {
                                    tud_id = Convert.ToInt64(valor[0]),
                                    cpa_id = Convert.ToInt32(entCompensacao.cpa_id),
                                    alu_id = Convert.ToInt64(valor[1]),
                                    mtu_id = Convert.ToInt32(valor[2]),
                                    mtd_id = Convert.ToInt32(valor[3])
                                };

                                listCompensacaoAluno.Add(compAluno);
                            }
                        }
                    }

                    if (listCompensacaoAluno.Count == 0)
                        throw new ValidationException("É necessário selecionar pelo menos um aluno para realizar a compensação.");

                    if (CLS_CompensacaoAusenciaBO.Save(entCompensacao, listCompensacaoAluno, VS_FechamentoAutomatico, UCComboCalendario.Valor))
                    {
                        ApplicationWEB._GravaLogSistema(VS_cpa_id > 0 ? LOG_SistemaTipo.Update : LOG_SistemaTipo.Insert, "cpa_id: " + entCompensacao.cpa_id + " tud_id: " + entCompensacao.tud_id);
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("Compensação de ausência " + (VS_cpa_id > 0 ? "alterada" : "incluída") + " com sucesso.", UtilBO.TipoMensagem.Sucesso);

                        VS_QtAulasComp = 0;

                        VerificaPaginaRedirecionar();
                    }
                }
                else
                {
                    string msg = String.Format("O docente não possui permissão para incluir compensações de ausência para o(a) {0} selecionado(a).", GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_MIN"));
                    lblMessage.Text = UtilBO.GetErroMessage(msg, UtilBO.TipoMensagem.Alerta);
                }
            }
            catch (ValidationException e)
            {
                lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ArgumentException e)
            {
                lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar compensação de ausência.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica qual página deve voltar e redireciona.
        /// </summary>
        private void VerificaPaginaRedirecionar()
        {
            string url = "~/Classe/CompensacaoAusencia/Busca.aspx";

            if (!string.IsNullOrEmpty(VS_PaginaRetorno))
            {
                Session["DadosPaginaRetorno"] = VS_DadosPaginaRetorno;
                Session["VS_DadosTurmas"] = VS_DadosPaginaRetorno_MinhasTurmas;
                url = VS_PaginaRetorno;
            }

            RedirecionarPagina(url);
        }

        #endregion Métodos

        #region Delegates

        /// <summary>
        /// Verifica alteracao do index do combo UA Escola e trata o combo Escola e Curso curriculo.
        /// </summary>
        protected void UCComboUAEscola_IndexChangedUA()
        {
            UCComboUAEscola.MostraApenasAtivas = true;

            UCComboUAEscola.SelectedValueEscolas = new[] { -1, -1 };

            UCComboUAEscola.EnableEscolas = (UCComboUAEscola.Uad_ID != Guid.Empty || !UCComboUAEscola.DdlUA.Visible);

            UCComboUAEscola_IndexChangedUnidadeEscola();
        }

        /// <summary>
        /// Verifica alteracao do index do combo Escola e trata o combo calendário
        /// </summary>
        protected void UCComboUAEscola_IndexChangedUnidadeEscola()
        {
            try
            {
                if (_VS_doc_id > 0)
                {
                    if (UCComboUAEscola.Esc_ID > 0)
                    {
                        ddlTurma.Enabled = true;

                        UCComboCalendario.CarregarCalendariosComBimestresAtivos(UCComboUAEscola.Esc_ID, true);

                        UCComboCalendario.SetarFoco();
                        UCComboCalendario.PermiteEditar = true;

                    }
                    else
                    {
                        ddlTurma.SelectedIndex = 0;
                        ddlTurma.Enabled = false;

                        UCCTurmaDisciplina1.Valor = -1;
                        UCCTurmaDisciplina1.PermiteEditar = false;

                        UCCPeriodoCalendario.Valor = new[] { -1, -1 };
                        UCCPeriodoCalendario.PermiteEditar = false;

                        UCComboCalendario.Valor = -1;
                        UCComboCalendario.PermiteEditar = false;
                    }
                }
                else
                {
                    UCComboCalendario.Valor = -1;
                    UCComboCalendario.PermiteEditar = false;

                    if (UCComboUAEscola.Esc_ID > 0 && UCComboUAEscola.Uni_ID > 0)
                    {
                        UCComboCalendario.CarregarCalendariosComBimestresAtivos(UCComboUAEscola.Esc_ID, true);

                        UCComboCalendario.SetarFoco();
                        UCComboCalendario.PermiteEditar = true;
                    }

                    UCComboCalendario_IndexChanged();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica alteracao do index do combo calendario e trata o combo Curso curriculo.
        /// </summary>
        protected void UCComboCalendario_IndexChanged()
        {
            try
            {
                if (_VS_doc_id > 0)
                {
                    if (UCComboCalendario.Valor > 0)
                    {
                        UCComboCalendario.PermiteEditar = true;
                        ddlTurma.Enabled = true;
                        InicializaCamposCadastroVisaoIndividual(UCComboUAEscola.Esc_ID);
                    }
                    else
                    {
                        ddlTurma.SelectedIndex = 0;
                        ddlTurma.Enabled = false;

                        UCCTurmaDisciplina1.Valor = -1;
                        UCCTurmaDisciplina1.PermiteEditar = false;

                        UCCPeriodoCalendario.Valor = new[] { -1, -1 };
                        UCCPeriodoCalendario.PermiteEditar = false;
                    }
                }
                else
                {

                    UCCCursoCurriculo.Valor = new[] { -1, -1 };
                    UCCCursoCurriculo.PermiteEditar = false;

                    if (UCComboCalendario.Valor > 0 && UCComboUAEscola.Esc_ID > 0 && UCComboUAEscola.Uni_ID > 0)
                    {
                        // Permite carregar cursos ativos ou encerrados (turmas histórico).
                        UCCCursoCurriculo.CarregarPorEscolaCalendarioSituacaoCurso(UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID, UCComboCalendario.Valor, 0);

                        UCCCursoCurriculo.SetarFoco();
                        UCCCursoCurriculo.PermiteEditar = true;
                    }

                    UCCCursoCurriculo_IndexChanged();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void UCCCursoCurriculo_IndexChanged()
        {
            try
            {
                ddlTurma.SelectedValue = "-1";
                ddlTurma.Enabled = false;

                if (UCCCursoCurriculo.Valor[0] > 0 && UCCCursoCurriculo.Valor[1] > 0 || _VS_doc_id > 0)
                {
                    ddlTurma.Items.Clear();
                    ddlTurma.DataTextField = "tur_codigo";

                    ddlTurma.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1", true));

                    ddlTurma.DataSource = TUR_TurmaBO.GetSelectBy_Escola_Periodo_Situacao(__SessionWEB.__UsuarioWEB.Usuario.usu_id,
                                                                                          __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                                                                                          (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao),
                                                                                          UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID,
                                                                                          UCComboCalendario.Valor, _VS_doc_id > 0 ? 0 : UCCCursoCurriculo.Valor[0],
                                                                                          _VS_doc_id > 0 ? 0 : UCCCursoCurriculo.Valor[1], -1,
                                                                                          __SessionWEB.__UsuarioWEB.Usuario.ent_id, 0, 0,
                                                                                          ApplicationWEB.AppMinutosCacheLongo)
                                          .GroupBy(p => new { tur_id = p.tur_id, tur_codigo = p.tur_codigo }).Select(p => p.Key).ToList(); ;
                    ddlTurma.DataBind();

                    ddlTurma.Focus();
                    ddlTurma.Enabled = true;
                }

                this.ddlTurma_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCComboTurmaDisciplina_IndexChanged()
        {
            try
            {
                UCCPeriodoCalendario.Valor = new[] { -1, -1 };
                UCCPeriodoCalendario.PermiteEditar = false;
                // utilizado para evitar chamar o evento de alteracao do calendario periodo duas vezes seguidas.
                bool selecionouComboPeriodos = false;

                if (UCCTurmaDisciplina1.Valor > -1)
                {
                    long tur_id = Convert.ToInt64(ddlTurma.SelectedValue);

                    TUR_Turma entTurma = new TUR_Turma { tur_id = tur_id };
                    TUR_TurmaBO.GetEntity(entTurma);

                    VS_FechamentoAutomatico = entTurma.fav_fechamentoAutomatico;

                    TUR_TurmaDisciplina entityTurmaDisciplina = new TUR_TurmaDisciplina { tud_id = UCCTurmaDisciplina1.Valor };
                    TUR_TurmaDisciplinaBO.GetEntity(entityTurmaDisciplina);

                    if (entityTurmaDisciplina.tud_naoLancarFrequencia)
                    {
                        lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " não pode lançar frequência na turma.", UtilBO.TipoMensagem.Alerta);
                        UCCTurmaDisciplina1.Valor = -1;
                    }
                    else
                    {
                        bool sucessoProcessarPendenciaFechamento = true;
                        if (VS_FechamentoAutomatico)
                        {
                            var pendencias = CLS_AlunoFechamentoPendenciaBO.SelecionarAguardandoProcessamento(tur_id, entityTurmaDisciplina.tud_id, entityTurmaDisciplina.tud_tipo, 0);

                            if ((pendencias != null) && (pendencias.Rows.Count > 0))
                            {
                                try
                                {
                                    // limpa cache desta turma
                                    string pattern;
                                    pattern = String.Format("{0}_{1}", ModelCache.FECHAMENTO_AUTO_BIMESTRE_PATTERN_KEY, entityTurmaDisciplina.tud_id);
                                    CacheManager.Factory.RemoveByPattern(pattern);
                                    pattern = String.Format("{0}_{1}", ModelCache.FECHAMENTO_AUTO_BIMESTRE_FILTRO_DEFICIENCIA_PATTERN_KEY, entityTurmaDisciplina.tud_id);
                                    CacheManager.Factory.RemoveByPattern(pattern);
                                    pattern = String.Format("{0}_{1}", ModelCache.FECHAMENTO_AUTO_BIMESTRE_COMPONENTES_REGENCIA_PATTERN_KEY, tur_id);
                                    CacheManager.Factory.RemoveByPattern(pattern);
                                    pattern = String.Format("{0}_{1}", ModelCache.FECHAMENTO_AUTO_FINAL_PATTERN_KEY, entityTurmaDisciplina.tud_id);
                                    CacheManager.Factory.RemoveByPattern(pattern);
                                    pattern = String.Format("{0}_{1}", ModelCache.FECHAMENTO_AUTO_FINAL_FILTRO_DEFICIENCIA_PATTERN_KEY, entityTurmaDisciplina.tud_id);
                                    CacheManager.Factory.RemoveByPattern(pattern);
                                    pattern = String.Format("{0}_{1}", ModelCache.FECHAMENTO_AUTO_FINAL_COMPONENTES_REGENCIA_PATTERN_KEY, tur_id);
                                    CacheManager.Factory.RemoveByPattern(pattern);

                                    pattern = String.Format(ModelCache.PENDENCIA_FECHAMENTO_ESCOLA_TURMA_DISCIPLINA_MODEL_KEY, entTurma.esc_id, entTurma.uni_id, entTurma.cal_id, entityTurmaDisciplina.tud_id);
                                    CacheManager.Factory.Remove(pattern);

                                    CLS_AlunoFechamentoPendenciaBO.Processar(entityTurmaDisciplina.tud_id, (byte)AvaliacaoTipo.Final, pendencias);
                                }
                                catch (Exception ex)
                                {
                                    sucessoProcessarPendenciaFechamento = false;
                                    ApplicationWEB._GravaErro(ex);
                                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Classe", "CompensacaoAusencia.Cadastro.MensagemErroProcessarPendenciaFechamento").ToString(), UtilBO.TipoMensagem.Erro);
                                }
                            }

                        }
                        if (sucessoProcessarPendenciaFechamento)
                        {
                            UCCPeriodoCalendario.CarregarPorPeriodoEventoEfetivacaoVigentes(entTurma.cal_id, UCCTurmaDisciplina1.Valor, entTurma.tur_id, true);
                            selecionouComboPeriodos = UCCPeriodoCalendario.Valor[0] != -1 && UCCPeriodoCalendario.Valor[1] != -1;

                            UCCPeriodoCalendario.SetarFoco();
                            UCCPeriodoCalendario.PermiteEditar = true;
                        }

                        VS_DisciplinaEspecial = entityTurmaDisciplina.tud_disciplinaEspecial;

                        VS_posicao = TUR_TurmaDocenteBO.SelecionaPosicaoPorDocenteTurma(_VS_doc_id, UCCTurmaDisciplina1.Valor, ApplicationWEB.AppMinutosCacheLongo);

                        VS_tipoDocente = ACA_TipoDocenteBO.SelecionaTipoDocentePorPosicao(VS_posicao, ApplicationWEB.AppMinutosCacheLongo);
                    }
                }

                if (!selecionouComboPeriodos)
                    UCCPeriodoCalendario_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCCPeriodoCalendario_IndexChanged()
        {
            rptAlunos.DataSource = null;
            bool possuiAlunos = false;

            if (UCCPeriodoCalendario.Valor[0] != -1 && UCCPeriodoCalendario.Valor[1] != -1)
            {
                long tur_id = Convert.ToInt64(ddlTurma.SelectedValue);

                TUR_Turma tur = new TUR_Turma();
                tur.tur_id = tur_id;
                TUR_TurmaBO.GetEntity(tur);

                TUR_TurmaDisciplina tud = new TUR_TurmaDisciplina();
                tud.tud_id = UCCTurmaDisciplina1.Valor;
                TUR_TurmaDisciplinaBO.GetEntity(tud);

                DateTime cap_dataInicio, cap_dataFim;

                ACA_CalendarioPeriodoBO.RetornaDatasPeriodoPor_FormatoAvaliacaoTurmaDisciplina(UCCPeriodoCalendario.Valor[0], string.Empty, UCCTurmaDisciplina1.Valor, tur.fav_id, out cap_dataInicio, out cap_dataFim);

                DataTable dt = _VS_doc_id > 0 && VS_DisciplinaEspecial ?
                    MTR_MatriculaTurmaDisciplinaBO.SelecionaAtivosCompensacaoAusenciaFiltroDeficiencia(tur_id, UCCTurmaDisciplina1.Valor, UCCPeriodoCalendario.Valor[0], 0, VS_tipoDocente, VS_cpa_id, false, true) :
                    MTR_MatriculaTurmaDisciplinaBO.SelecionaAtivosCompensacaoAusencia(UCCTurmaDisciplina1.Valor, UCCPeriodoCalendario.Valor[0], 0, VS_cpa_id, false, true);
                rptAlunos.DataSource = dt;

                possuiAlunos = dt.Rows.Count > 0;
                rptAlunos.DataBind();

                lblMessage.Text = "";

                if (tud.tud_tipo == (byte)TurmaDisciplinaTipo.Experiencia && 
                    !TUR_TurmaDisciplinaTerritorioBO.VerificaOferecimentoExperienciaBimestre(UCCTurmaDisciplina1.Valor, UCComboCalendario.Valor, UCCPeriodoCalendario.Valor[0]))
                {
                    mostraSalvar = false;
                    lblMessage.Text += UtilBO.GetErroMessage("A experiência não possui territórios vigentes no bimestre.", UtilBO.TipoMensagem.Alerta);
                }

                if (VS_cpa_id <= 0)
                {
                    if (possuiAlunos)
                        btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && mostraSalvar;
                    else
                    {
                        btnSalvar.Visible = false;
                        lblMessage.Text += UtilBO.GetErroMessage("Não existem alunos com ausências para serem compensadas.", UtilBO.TipoMensagem.Alerta);
                    }
                }
            }

            fdsAlunos.Visible = possuiAlunos;
        }

        #endregion Delegates

        #region Eventos

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                Salvar();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            VerificaPaginaRedirecionar();
        }

        /// <summary>
        /// Carrega as disciplinas da turma
        /// </summary>
        protected void ddlTurma_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long tur_id = Convert.ToInt64(ddlTurma.SelectedValue);

                if (_VS_doc_id <= 0)
                    UCCTurmaDisciplina1.CarregarTurmaDisciplina(tur_id);
                else
                    UCCTurmaDisciplina1.CarregarTurmaDisciplina(tur_id, _VS_doc_id);

                if (tur_id > 0)
                {
                    UCCTurmaDisciplina1.SetarFoco();
                    UCCTurmaDisciplina1.PermiteEditar = true;
                }

                UCComboTurmaDisciplina_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Eventos
    }
}
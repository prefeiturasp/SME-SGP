namespace GestaoEscolar.WebControls.PlanejamentoAnual
{
    using System;
    using System.Linq;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using MSTech.CoreSSO.BLL;
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.Web.WebProject;
    using MSTech.Validation.Exceptions;
    using System.Collections.Generic;

    public partial class UCPlanejamentoProjetos : MotherUserControl
    {
        #region Propriedades

        private List<sComboTurmaDisciplina> VS_TurmaDisciplinaDocente
        {
            get
            {
                if (ViewState["VS_TurmaDisciplinaDocente"] == null)
                    ViewState["VS_TurmaDisciplinaDocente"] = __SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ?
                            TUR_TurmaDisciplinaBO.SelecionaDisciplinaPorTurmaDocente_SemVigencia(0, __SessionWEB.__UsuarioWEB.Docente.doc_id, 0, 0, false, ApplicationWEB.AppMinutosCacheMedio)
                            : TUR_TurmaDisciplinaBO.SelecionaDisciplinaPorTurmaDocente_SemVigencia(VS_tur_id, 0, 0, 0, false, ApplicationWEB.AppMinutosCacheMedio);

                return (List<sComboTurmaDisciplina>)(ViewState["VS_TurmaDisciplinaDocente"]);
            }
            set
            {
                ViewState["VS_TurmaDisciplinaDocente"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o id da turma.
        /// </summary>
        private long VS_tur_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_tur_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_tur_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o id da escola.
        /// </summary>
        private int VS_esc_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_esc_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_esc_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o id da escola.
        /// </summary>
        private int VS_uni_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_uni_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_uni_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o id da turmadisciplina.
        /// </summary>
        private long VS_tud_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_tud_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_tud_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o id da turmadisciplina.
        /// </summary>
        private long VS_tud_idComponenteSelecionado
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_tud_idComponenteSelecionado"] ?? "-1");
            }

            set
            {
                ViewState["VS_tud_idComponenteSelecionado"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o id do tipo disciplina.
        /// </summary>
        private int VS_tds_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_tds_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_tds_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o tipo da turmadisciplina.
        /// </summary>
        private byte VS_tud_tipo
        {
            get
            {
                return Convert.ToByte(ViewState["VS_tud_tipo"] ?? "-1");
            }

            set
            {
                ViewState["VS_tud_tipo"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena a posicao.
        /// </summary>
        private byte VS_tdt_posicao
        {
            get
            {
                return Convert.ToByte(ViewState["VS_tdt_posicao"] ?? "-1");
            }

            set
            {
                ViewState["VS_tdt_posicao"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena a permissão de edição de objetos de aprendizagem.
        /// </summary>
        private bool VS_permiteEditarObjAprendizagem
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_permiteEditarObjAprendizagem"] ?? false);
            }

            set
            {
                ViewState["VS_permiteEditarObjAprendizagem"] = value;
            }
        }

        #region Plano Ciclo

        /// <summary>
        /// ViewState que armazena o id da turmadisciplinaplanejamento geral.
        /// </summary>
        private int VS_tdp_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_tdp_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_tdp_id"] = value;
            }
        }

        /// <summary>
        /// Armazena os IDs do tipo de ciclo em viewstate.
        /// </summary>
        private List<int> VS_tciIdsTurma
        {
            get
            {
                return ViewState["VS_tciIdsTurma"] == null ? new List<int>() : (List<int>)ViewState["VS_tciIdsTurma"];
            }

            set
            {
                ViewState["VS_tciIdsTurma"] = value;
            }
        }

        /// <summary>
        /// Id do plano de ciclo da turma.
        /// </summary>
        private int VS_plc_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_plc_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_plc_id"] = value;
            }
        }

        /// <summary>
        /// Mensagem de erro para ser pega pelo Delegates no caso de algum erro ou alerta ao salvar o ciclo
        /// </summary>
        public string sMensagemErroPlanoCiclo;

        #endregion Plano Ciclo

        #region Plano Anual

        /// <summary>
        /// Parâmetro de mensagem de ajuda no campo de diagnóstico inicial.
        /// </summary>
        private string DiagnosticoInicial
        {
            get
            {
                return CFG_ParametroMensagemBO.RetornaValor(CFG_ParametroMensagemChave.PLANEJAMENTO_DIAGNOSTICOINICIAL);
            }
        }

        /// <summary>
        /// Parâmetro de mensagem de ajuda no campo de proposta metodológica.
        /// </summary>
        private string Proposta
        {
            get
            {
                return CFG_ParametroMensagemBO.RetornaValor(CFG_ParametroMensagemChave.PLANEJAMENTO_PLANEJAMENTOANUAL);
            }
        }

        private int tpc_idSelecionado = 0;

        #endregion Plano Anual

        #region Plano Aluno


        /// <summary>
        /// ViewState que armazena o id do aluno que esta sendo alterado geral.
        /// </summary>
        private long VS_plano_alu_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_alu_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_alu_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o id do curso.
        /// </summary>
        private int VS_cal_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_cal_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_cal_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o id do curso.
        /// </summary>
        private int VS_cur_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_cur_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_cur_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o id do curriculo.
        /// </summary>
        private int VS_crr_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_crr_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_crr_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o id do periodo do curso.
        /// </summary>
        private int VS_crp_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_crp_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_crp_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o planejamento dos alunos na turmadisciplina com os relacionados
        /// </summary>
        public List<AlunoPlanejamento> VS_AlunoPlanejamento
        {
            get
            {
                return (List<AlunoPlanejamento>)ViewState["VS_AlunoPlanejamento"] ?? new List<AlunoPlanejamento>();
            }

            set
            {
                ViewState["VS_AlunoPlanejamento"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena os ids das turmas normais de matricula de alunos matriculados em turmas multisseriadas do docente.
        /// </summary>
        private string VS_TurmasNormais_Ids
        {
            get
            {
                return (ViewState["VS_TurmasNormais_Ids"] ?? "").ToString();
            }

            set
            {
                ViewState["VS_TurmasNormais_Ids"] = value;
            }
        }

        #endregion Plano Aluno

        #region Objetos de Aprendizagem

        public List<Struct_ObjetosAprendizagem> VS_lstObjetosAprendizagem
        {
            get
            {
                if (ViewState["VS_lstObjetosAprendizagem"] != null)
                {
                    return (List<Struct_ObjetosAprendizagem>)(ViewState["VS_lstObjetosAprendizagem"]);
                }
                else
                {
                    return new List<Struct_ObjetosAprendizagem>();
                }
            }

            set
            {
                ViewState["VS_lstObjetosAprendizagem"] = value;
            }
        }

        public bool abaObjAprendVisivel
        {
            get
            {
                if (ViewState["abaObjAprendVisivel"] != null)
                {
                    return Convert.ToBoolean(ViewState["abaObjAprendVisivel"]);
                }
                else
                {
                    return false;
                }
            }

            set
            {
                ViewState["abaObjAprendVisivel"] = value;
            }
        }

        public bool rptObjetosVisible
        {
            get
            {
                return rptEixo.Visible;
            }
        }

        #endregion Objetos de Aprendizagem

        /// <summary>
        /// Retorna se o usuário logado é docente.
        /// </summary>
        private bool VS_visaoDocente
        {
            get
            {
                long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                int visao = __SessionWEB.__UsuarioWEB.Grupo.vis_id;
                return (visao == SysVisaoID.Individual && doc_id > 0);
            }
        }

        private byte PosicaoDocente
        {
            get
            {
                return (VS_turmaDisciplinaCompartilhada != null || ACA_TipoDocenteBO.SelecionaPosicaoPorTipoDocenteCache(EnumTipoDocente.Compartilhado, ApplicationWEB.AppMinutosCacheLongo) == VS_tdt_posicao)
                       || ACA_TipoDocenteBO.SelecionaPosicaoPorTipoDocenteCache(EnumTipoDocente.SegundoTitular, ApplicationWEB.AppMinutosCacheLongo) == VS_tdt_posicao
                       ? ACA_TipoDocenteBO.SelecionaPosicaoPorTipoDocenteCache(EnumTipoDocente.Titular, ApplicationWEB.AppMinutosCacheLongo) : VS_tdt_posicao;
            }
        }

        /// <summary>
        /// Retorna a posição do tipo de docente
        /// </summary>
        private byte PosicaoTitular
        {
            get
            {
                return ACA_TipoDocenteBO.SelecionaPosicaoPorTipoDocenteCache(EnumTipoDocente.Titular, ApplicationWEB.AppMinutosCacheLongo);
            }
        }

        /// <summary>
        /// Carrega as configurações para a disciplina compartilhada, relacionada com a disciplina atual.
        /// </summary>
        public TUR_TurmaDisciplina VS_turmaDisciplinaCompartilhada
        {
            get
            {
                if (ViewState["VS_turmaDisciplinaCompartilhada"] != null)
                    return (TUR_TurmaDisciplina)ViewState["VS_turmaDisciplinaCompartilhada"];
                return null;
            }

            set
            {
                ViewState["VS_turmaDisciplinaCompartilhada"] = value;
            }
        }

        /// <summary>
        /// Seta a permissão de alteração na tela, habilitando botões de salvar e campos texto.
        /// </summary>
        public bool PermiteEdicao
        {
            get
            {
                if (ViewState["VS_PermiteEdicao"] != null)
                {
                    return Convert.ToBoolean(ViewState["VS_PermiteEdicao"]);
                }

                return false;
            }
            set
            {
                ViewState["VS_PermiteEdicao"] = value;
                if (!value)
                {
                    // Remove todos os botões do editor html.
                    txtPlanoCiclo.RemovePlugins = "toolbar";
                    txtPlanoAluno.RemovePlugins = "toolbar";
                }

                txtDiagnosticoInicial.Enabled = txtProposta.Enabled = value;
                txtPlanoAluno.Enabled = value;
            }
        }

        private int tne_id;

        #endregion

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            SetarJavaScript();

            #region Config CKEditor

            string script = string.Empty;

            txtPlanoCiclo.config.toolbar = new object[]
                                        {
                                            new object[]
                                                {
                                                    "Print"
                                                },
                                            new object[]
                                                {
                                                    "Cut", "Copy", "-", "Paste", "PasteText", "PasteFromWord", "-", "Undo", "Redo"
                                                },
                                            new object[]
                                                {
                                                    "Find", "Replace", "-", "SelectAll", "-", "SpellChecker", "Scayt"
                                                },
                                            new object[]
                                                {
                                                    "Bold", "Italic", "Underline", "Strike", "Subscript", "Superscript", "-", "RemoveFormat"
                                                },
                                            new object[]
                                                {
                                                    "NumberedList", "BulletedList", "-", "Outdent", "Indent", "-", "Blockquote", "CreateDiv",
                                                    "-", "JustifyLeft", "JustifyCenter", "JustifyRight", "JustifyBlock" ,"-", "BidiLtr", "BidiRtl"
                                                },
                                            new object[]
                                                {
                                                    "Link", "Unlink"
                                                },
                                            new object[]
                                                {
                                                    "Image", "Table", "HorizontalRule", "Smiley", "SpecialChar", "PageBreak"
                                                },
                                            new object[]
                                                {
                                                    "Styles", "Format", "Font", "FontSize"
                                                },
                                            new object[]
                                                {
                                                    "TextColor", "BGColor"
                                                },
                                            new object[]
                                                {
                                                    "Maximize", "ShowBlocks"
                                                }
                                        };

            txtPlanoAluno.config.toolbar = new object[]
                                        {
                                            new object[]
                                                {
                                                    "Print"
                                                },
                                            new object[]
                                                {
                                                    "Cut", "Copy", "-", "Paste", "PasteText", "PasteFromWord", "-", "Undo", "Redo"
                                                },
                                            new object[]
                                                {
                                                    "Find", "Replace", "-", "SelectAll", "-", "SpellChecker", "Scayt"
                                                },
                                            new object[]
                                                {
                                                    "Bold", "Italic", "Underline", "Strike", "Subscript", "Superscript", "-", "RemoveFormat"
                                                },
                                            new object[]
                                                {
                                                    "NumberedList", "BulletedList", "-", "Outdent", "Indent", "-", "Blockquote", "CreateDiv",
                                                    "-", "JustifyLeft", "JustifyCenter", "JustifyRight", "JustifyBlock" ,"-", "BidiLtr", "BidiRtl"
                                                },
                                            new object[]
                                                {
                                                    "Link", "Unlink"
                                                },
                                            new object[]
                                                {
                                                    "Image", "Table", "HorizontalRule", "Smiley", "SpecialChar", "PageBreak"
                                                },
                                            new object[]
                                                {
                                                    "Styles", "Format", "Font", "FontSize"
                                                },
                                            new object[]
                                                {
                                                    "TextColor", "BGColor"
                                                },
                                            new object[]
                                                {
                                                    "Maximize", "ShowBlocks"
                                                }
                                        };

            #endregion Config CKEditor

            uccTipoCiclo.IndexChanged += uccTipoCiclo_IndexChanged;
            UCCAlunos.IndexChanged += UCCAlunos_IndexChanged;
            //UCCTurmaAluno.IndexChanged += UCCTurmaAluno_IndexChanged;
        }

        #endregion Page Life Cycle

        #region Delegates

        protected void uccTipoCiclo_IndexChanged()
        {
            if (Convert.ToInt32(uccTipoCiclo.Valor) > 0)
                CarregarPlanoCiclo(Convert.ToInt32(uccTipoCiclo.Valor));
        }

        protected void UCCAlunos_IndexChanged()
        {
            SalvaPlanoAluno(VS_plano_alu_id);
            CarregarPlanoAluno(Convert.ToInt64(UCCAlunos.Valor.Split(';')[0]));
        }

        public delegate void ReplicarPlanejamento();
        public ReplicarPlanejamento ReplicaPlanoAnual;

        public delegate void ReplicarPlanejamentoCicloDelegate();
        public ReplicarPlanejamentoCicloDelegate SalvarPlanoCicloDelegate;

        #endregion Delegates

        #region Métodos

        #region Métodos iniciais

        public void LoadDdlComponenteRegencia(long tudIdPendencia)
        {
            //// Só carrega componentes da regência se o tipo da disciplina for de REGENCIA.
            if (VS_tud_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.Regencia))
            {
                lblComponenteAtAvaliativa.Visible = ddlComponenteAtAvaliativa.Visible = true;
                CarregaComponenteRegenciaDocente(ddlComponenteAtAvaliativa);
                if (tudIdPendencia > 0)
                {
                    foreach (ListItem item in ddlComponenteAtAvaliativa.Items)
                    {
                        if (item.Value.StartsWith(string.Format("{0};{1};", VS_tur_id, tudIdPendencia)))
                        {
                            ddlComponenteAtAvaliativa.SelectedValue = item.Value;
                            break;
                        }
                    }
                }
            }
            else
                lblComponenteAtAvaliativa.Visible = ddlComponenteAtAvaliativa.Visible = false;
        }

        /// <summary>
        /// Carrega disciplina(s componente(s) da regencia somente da turma selecionada.
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="ddlDisciplinaComponentes">Combo que será carregado</param>
        private void CarregaComponenteRegenciaDocente(DropDownList ddlDisciplinaComponentes)
        {
            List<sComboTurmaDisciplina> turmaDisciplinaComponenteRegencia = (from dr in VS_TurmaDisciplinaDocente
                                                                             where Convert.ToByte(dr.tur_tud_id.Split(';')[3]) == Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                                                             && (Convert.ToInt64(dr.tur_tud_id.Split(';')[0]) == VS_tur_id)
                                                                             orderby dr.tud_nome
                                                                             select new sComboTurmaDisciplina
                                                                             {
                                                                                 tur_tud_nome = dr.tur_tud_nome.ToString()
                                                                                 ,
                                                                                 tur_tud_id = dr.tur_tud_id.ToString()
                                                                                 ,
                                                                                 tud_nome = dr.tud_nome.ToString()
                                                                             }).ToList();

            if (ddlDisciplinaComponentes.Items.Count == 0)
            {
                ddlDisciplinaComponentes.DataSource = turmaDisciplinaComponenteRegencia;
                ddlDisciplinaComponentes.DataBind();
            }
        }

        /// <summary>
        /// Carrega dados do planejament da turma.
        /// </summary>
        public void CarregarTurma(long tur_id, int cal_id, int esc_id, int uni_id, int cur_id, int crr_id, int crp_id,
                                  long tud_id, int tds_id, byte tud_tipo, byte tdt_posicao, string tciIds, string tur_ids = null, 
                                  int tne_id = -1, int tme_id = -1, bool permiteEditarObjAprendizagem = false, long tudIdPendencia = -1)
        {
            VS_TurmasNormais_Ids = tur_ids;
            VS_tur_id = tur_id;
            VS_esc_id = esc_id;
            VS_uni_id = uni_id;
            VS_cal_id = cal_id;
            VS_esc_id = esc_id;
            VS_cur_id = cur_id;
            VS_crr_id = crr_id;
            VS_crp_id = crp_id;
            VS_tud_id = tud_id;
            VS_tud_tipo = tud_tipo;
            VS_tds_id = tds_id;
            VS_permiteEditarObjAprendizagem = permiteEditarObjAprendizagem;

            LoadDdlComponenteRegencia(tudIdPendencia);

            if (!String.IsNullOrEmpty(tciIds))
            {
                string[] vetTipoCiclo = tciIds.Split(',');
                var elements = from element in vetTipoCiclo
                               select Convert.ToInt32(element);
                VS_tciIdsTurma = elements.ToList();
            }
            else
            {
                VS_tciIdsTurma = new List<int>();
            }
            VS_tdt_posicao = tdt_posicao;

            bool ensinoInfantil = tne_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            this.tne_id = tne_id;
            abaPlanoCiclo.Visible = divTabsPlanoCiclo.Visible = !ensinoInfantil || ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_ABA_PLANEJAMENTO_PLANO_CICLO_ENSINO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            abaPlanoAnual.Visible = divTabsPlanoAnual.Visible = !ensinoInfantil || ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_ABA_PLANEJAMENTO_PLANO_ANUAL_ENSINO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            abaPlanoAluno.Visible = divTabsPlanoAluno.Visible = !ensinoInfantil || ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_ABA_PLANEJAMENTO_PLANO_ALUNO_ENSINO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            bool modalidadeEJA = tme_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_MODALIDADE_EJA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            bool modalidadeCIEJA = tme_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_MODALIDADE_CIEJA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            litPlanoCiclo.Text = modalidadeEJA || modalidadeCIEJA ? GetGlobalResourceObject("UserControl", "UCPlanejamentoProjetos.litPlanoCiclo.Text.Etapa").ToString() : litPlanoCiclo.Text;
            litPlanoAnual.Text = modalidadeEJA && !modalidadeCIEJA ? GetGlobalResourceObject("UserControl", "UCPlanejamentoProjetos.litPlanoAnual.Text.Semestral").ToString() : litPlanoAnual.Text;


            if (VS_tciIdsTurma.Count > 0 && abaPlanoCiclo.Visible)
            {
                CarregarPlanoCiclo(VS_tciIdsTurma[0]);
            }
            if (abaPlanoAnual.Visible)
            {
                CarregarPlanoAnual();
            }
            if (abaPlanoAluno.Visible)
            {
                CarregarPlanoAlunos();
            }

            //CarregarProjeto();

            CarregarDocumentos();

            CarregarObjetosAprendizagem();

            if (tudIdPendencia > 0)
            {
                int indiceAba = 4;
                if (!abaPlanoCiclo.Visible)
                {
                    indiceAba--;
                }
                if (!abaPlanoAnual.Visible)
                {
                    indiceAba--;
                }
                if (!abaPlanoAluno.Visible)
                {
                    indiceAba--;
                }
                selected_tab.Value = indiceAba.ToString();
            }
        }

        /// <summary>
        /// Carrega os objetos de aprendizagem
        /// </summary>
        public void CarregarObjetosAprendizagem()
        {
            ACA_CurriculoPeriodo crp = new ACA_CurriculoPeriodo { cur_id = VS_cur_id, crr_id = VS_crr_id, crp_id = VS_crp_id };
            ACA_CurriculoPeriodoBO.GetEntity(crp);

            ACA_TipoCiclo tci = new ACA_TipoCiclo { tci_id = crp.tci_id };
            ACA_TipoCicloBO.GetEntity(tci);

            ACA_TipoCurriculoPeriodo tcp = new ACA_TipoCurriculoPeriodo { tcp_id = crp.tcp_id };
            ACA_TipoCurriculoPeriodoBO.GetEntity(tcp);

            TUR_TurmaDisciplina entityTud = new TUR_TurmaDisciplina { tud_id = VS_tud_id };
            TUR_TurmaDisciplinaBO.GetEntity(entityTud);

            TUR_Turma entityTurma = new TUR_Turma { tur_id = VS_tur_id };
            TUR_TurmaBO.GetEntity(entityTurma);

            abaObjAprendVisivel = abaobjAprendizagem.Visible = divTabsObjetoAprendizagem.Visible = VS_permiteEditarObjAprendizagem &&
                                Convert.ToBoolean(tcp.tcp_objetoAprendizagem) && Convert.ToBoolean(tci.tci_objetoAprendizagem) &&
                                entityTurma.tur_tipo == (byte)TUR_TurmaTipo.Normal;

            if (abaobjAprendizagem.Visible)
            {
                lblAvisoObjetosAprendizagem.Text = (string)GetGlobalResourceObject("UserControl", "UCPlanejamentoProjetos.lblAvisoObjetosAprendizagem.Text");
                updAvisoObjetosAprendizagem.Update();
                btnAjudaObjetos.Visible = !string.IsNullOrEmpty(lblAvisoObjetosAprendizagem.Text);

                VS_lstObjetosAprendizagem = ACA_ObjetoAprendizagemBO.SelectListaBy_TurmaDisciplina(VS_tud_id, VS_cal_id);

                CarregaRepeaterObjetoAprendizagem();
            }
        }

        private void CarregaRepeaterObjetoAprendizagem()
        {
            List<Struct_ObjetosAprendizagem> list = VS_lstObjetosAprendizagem;
            if (VS_tud_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.Regencia) && ddlComponenteAtAvaliativa.Items.Count > 0)
            {
                list = VS_lstObjetosAprendizagem.Where(p => p.tud_id == Convert.ToInt64(ddlComponenteAtAvaliativa.SelectedValue.Split(';')[1])).ToList();
                VS_tud_idComponenteSelecionado = Convert.ToInt64(ddlComponenteAtAvaliativa.SelectedValue.Split(';')[1]);
            }

            if (!list.Any())
            {
                rptEixo.Visible = false;
                lblMensagemObjetos.Text = UtilBO.GetErroMessage("Não existem objetos de conhecimento cadastrados.", UtilBO.TipoMensagem.Alerta);
            }
            else
            {
                rptEixo.Visible = true;
                rptEixo.DataSource = (
                    from Struct_ObjetosAprendizagem dr in list.Where(p => p.oae_id > 0)
                    group dr by dr.oae_id into g
                    select new
                    {
                        oae_id = g.Key
                        ,
                        oae_descricao = g.First().oae_descricao
                        ,
                        oae_ordem = g.First().oae_ordem
                    }
                ).OrderBy(p => p.oae_ordem).ToList();
                rptEixo.DataBind();
            }
        }

        #endregion 
        
        /// <summary>
        /// Seta os eventos javascript da tela.
        /// </summary>
        private void SetarJavaScript()
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);
            if (sm != null)
            {
                //sm.Scripts.Add(new ScriptReference("~/Includes/jsUCPlanejamentoProjetos.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/ckeditor/ckeditor.js"));
            }

            #region Plano Anual

            btnTextoGrandeDiagnosticoInicial.OnClientClick = "abrirTextoGrande('" + lblDiagnosticoInicial.ClientID + "', '" + txtDiagnosticoInicial.ClientID + "', '" + btnTextoGrandeDiagnosticoInicial.ClientID + "', '" + btnVoltaEstadoAnteriorTextoDiagnosticoInicial.ClientID + "'); return false;";
            btnTextoGrandeProposta.OnClientClick = "abrirTextoGrande('" + lblProposta.ClientID + "', '" + txtProposta.ClientID + "', '" + btnTextoGrandeProposta.ClientID + "', '" + btnVoltaEstadoAnteriorTextoProposta.ClientID + "'); return false;";

            btnVoltaEstadoAnteriorTextoDiagnosticoInicial.OnClientClick = String.Format("abrirTextoPequeno('{0}', '{1}', '{2}', '{3}', '{4}'); return false;",
                                                                            lblDiagnosticoInicial.ClientID,
                                                                            txtDiagnosticoInicial.ClientID,
                                                                            btnTextoGrandeDiagnosticoInicial.ClientID,
                                                                            btnVoltaEstadoAnteriorTextoDiagnosticoInicial.ClientID,
                                                                            "true");

            btnVoltaEstadoAnteriorTextoProposta.OnClientClick = String.Format("abrirTextoPequeno('{0}', '{1}', '{2}', '{3}', '{4}'); return false;",
                                                                            lblProposta.ClientID,
                                                                            txtProposta.ClientID,
                                                                            btnTextoGrandeProposta.ClientID,
                                                                            btnVoltaEstadoAnteriorTextoProposta.ClientID,
                                                                            "true");

            if (!string.IsNullOrEmpty(DiagnosticoInicial))
            {
                // Altera a função para configurar a mensagem também no txt grande.
                btnTextoGrandeDiagnosticoInicial.OnClientClick =
                    String.Format("abrirTextoGrandeComMensagem('{0}','{1}','{2}','{3}','{4}'); return false;",
                        lblDiagnosticoInicial.ClientID,
                        txtDiagnosticoInicial.ClientID,
                        DiagnosticoInicial,
                        btnTextoGrandeDiagnosticoInicial.ClientID,
                        btnVoltaEstadoAnteriorTextoDiagnosticoInicial.ClientID);

                // Altera a função para configurar a mensagem também no txt pequeno.
                btnVoltaEstadoAnteriorTextoDiagnosticoInicial.OnClientClick =
                    String.Format("abrirTextoPequenoComMensagem('{0}','{1}','{2}','{3}','{4}', '{5}'); return false;",
                        lblDiagnosticoInicial.ClientID,
                        txtDiagnosticoInicial.ClientID,
                        DiagnosticoInicial,
                        btnTextoGrandeDiagnosticoInicial.ClientID,
                        btnVoltaEstadoAnteriorTextoDiagnosticoInicial.ClientID,
                        "true");

                lblDiagnosticoInicialInfo.Text = DiagnosticoInicial;
            }

            if (!string.IsNullOrEmpty(Proposta))
            {
                // Altera a função para configurar a mensagem também no txt grande.
                btnTextoGrandeProposta.OnClientClick =
                    String.Format("abrirTextoGrandeComMensagem('{0}','{1}','{2}','{3}','{4}'); return false;",
                        lblProposta.ClientID,
                        txtProposta.ClientID,
                        Proposta,
                        btnTextoGrandeProposta.ClientID,
                        btnVoltaEstadoAnteriorTextoProposta.ClientID);

                // Altera a função para configurar a mensagem também no txt pequeno.
                btnVoltaEstadoAnteriorTextoProposta.OnClientClick =
                    String.Format("abrirTextoPequenoComMensagem('{0}','{1}','{2}','{3}','{4}','{5}'); return false;",
                        lblProposta.ClientID,
                        txtProposta.ClientID,
                        Proposta,
                        btnTextoGrandeProposta.ClientID,
                        btnVoltaEstadoAnteriorTextoProposta.ClientID,
                        "true");

                lblPropostaInfo.Text = Proposta;
            }

            #endregion Plano Anual
        }

        #endregion Métodos iniciais

        #region Plano de ciclo

        /// <summary>
        /// Carrega o planejamento do ciclo/série
        /// </summary>
        /// <param name="tci_id"></param>
        private void CarregarPlanoCiclo(int tci_id)
        {
            try
            {
                txtPlanoCiclo.Enabled = btnCancelarCiclo.Visible = false;
                btnEditarPlanoCiclo.Visible = true;
                if (VS_visaoDocente)
                {
                    // Se for docente, só pode editar plano do ciclo da sua turma, se não for, pode alterar todos os ciclos.
                    divEditarPlanoCiclo.Visible = PermiteEdicao && (VS_tciIdsTurma.Contains(tci_id) && PosicaoDocente == PosicaoTitular);
                }
                else
                {
                    divEditarPlanoCiclo.Visible = PermiteEdicao;
                }

                if (uccTipoCiclo.QuantidadeItensCombo == 0)
                    uccTipoCiclo.CarregarCombo(ACA_TipoCicloBO.SelecionaCicloPorCursoCurriculo(VS_cur_id, VS_crr_id, ApplicationWEB.AppMinutosCacheLongo), "tci_nome", "tci_id");

                uccTipoCiclo.Valor = tci_id.ToString();
                lblCiclo.Text = uccTipoCiclo.TextoSelecionado;

                CLS_PlanejamentoCiclo entityCiclo = CLS_PlanejamentoCicloBO.SelecionaAtivoPorTurmaTipoCiclo(VS_tur_id, tci_id);

                VS_plc_id = entityCiclo.plc_id;

                txtPlanoCiclo.Text = string.IsNullOrEmpty(entityCiclo.plc_planoCiclo) ? string.Empty : entityCiclo.plc_planoCiclo;
                lblUsuarioCiclo.Text = string.IsNullOrEmpty(entityCiclo.nomeUsuario) ?
                                        string.Empty :
                                        String.Format("Alterado por {0} em {1} às {2}",
                                                      entityCiclo.nomeUsuario,
                                                      entityCiclo.plc_dataCriacao.ToString("dd/MM/yyyy"),
                                                      entityCiclo.plc_dataCriacao.ToString("HH:mm"));
                imgHistoricoAlteracaoCiclo.Visible = !string.IsNullOrEmpty(lblUsuarioCiclo.Text);

                grvHistoricoCiclo.DataSource = CLS_PlanejamentoCicloBO.SelecionaHistoricoAlteracoes(VS_tur_id, tci_id);
                grvHistoricoCiclo.DataBind();

                updHistoricoCiclo.Update();
            }
            catch (ValidationException ex)
            {
                lblMensagemCiclo.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagemCiclo.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o plano de ciclo.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Salva o plano de ciclo.
        /// </summary>
        /// <returns></returns>
        public bool SalvarPlanoCiclo()
        {
            try
            {
                sMensagemErroPlanoCiclo = string.Empty;

                if (!txtPlanoCiclo.Enabled)
                    return false;

                TUR_Turma entityTurma = new TUR_Turma
                {
                    tur_id = VS_tur_id
                };
                TUR_TurmaBO.GetEntity(entityTurma);

                ACA_CalendarioAnual entityCalendario = new ACA_CalendarioAnual
                {
                    cal_id = entityTurma.cal_id
                };
                ACA_CalendarioAnualBO.GetEntity(entityCalendario);

                CLS_PlanejamentoCiclo entityCiclo = new CLS_PlanejamentoCiclo
                {
                    esc_id = entityTurma.esc_id
                    ,
                    uni_id = entityTurma.uni_id
                    ,
                    tci_id = Convert.ToInt32(uccTipoCiclo.Valor)
                    ,
                    plc_anoLetivo = entityCalendario.cal_ano
                    ,
                    plc_id = VS_plc_id
                };
                CLS_PlanejamentoCicloBO.GetEntity(entityCiclo);

                if (entityCiclo.plc_planoCiclo == txtPlanoCiclo.Text)
                {
                    lblMensagemCiclo.Text = UtilBO.GetErroMessage("Não há alterações no plano de ciclo.", UtilBO.TipoMensagem.Informacao);
                    return false;
                }

                entityCiclo.plc_planoCiclo = txtPlanoCiclo.Text;
                entityCiclo.usu_id = __SessionWEB.__UsuarioWEB.Usuario.usu_id;
                entityCiclo.plc_situacao = (byte)PlanejamentoCicloSituacao.Ativo;
                //Alteração para salvar o objeto novo
                entityCiclo.IsNew = true;
                entityCiclo.plc_id = -1;
                entityCiclo.plc_dataAlteracao = entityCiclo.plc_dataCriacao = new DateTime();

                if (CLS_PlanejamentoCicloBO.SalvarPlanoCiclo(entityCiclo))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, String.Format("Plano de ciclo alterado | tci_id: {0}; esc_id: {1}; uni_id: {2}", entityCiclo.tci_id, entityCiclo.esc_id, entityCiclo.uni_id));
                    // Removida mensagem de sucesso, pois já exibe na tela (fora do usercontrol).
                    //lblMensagemCiclo.Text = UtilBO.GetErroMessage("Plano de ciclo alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    CarregarPlanoCiclo(Convert.ToInt32(uccTipoCiclo.Valor));

                    if (SalvarPlanoCicloDelegate != null)
                        SalvarPlanoCicloDelegate();

                    return true;
                }
            }
            catch (ValidationException ex)
            {
                sMensagemErroPlanoCiclo = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                sMensagemErroPlanoCiclo = UtilBO.GetErroMessage("Erro ao tentar salvar o plano de ciclo.", UtilBO.TipoMensagem.Erro);
            }


            if (SalvarPlanoCicloDelegate != null)
                SalvarPlanoCicloDelegate();

            return false;
        }

        #endregion Plano de ciclo

        #region Plano anual

        /// <summary>
        /// Carrega o planejamento anual. Diagnóstico, Proposta e Bimestres
        /// </summary>
        private void CarregarPlanoAnual()
        {
            try
            {
                DataTable dtPlanejamentoAnual = CLS_TurmaDisciplinaPlanejamentoBO.SelecionaPorTurmaDisciplinaPeriodoCalendarioNulo(VS_tud_id, PosicaoTitular);
                if (dtPlanejamentoAnual.Rows.Count > 0)
                {
                    txtDiagnosticoInicial.Text = dtPlanejamentoAnual.Rows[0]["tdp_diagnostico"].ToString();
                    txtProposta.Text = dtPlanejamentoAnual.Rows[0]["tdp_planejamento"].ToString();
                    VS_tdp_id = Convert.ToInt32(dtPlanejamentoAnual.Rows[0]["tdp_id"]);
                }

                DataTable dtPlanBimestre = CLS_TurmaDisciplinaPlanejamentoBO.SelecionaPorTurmaDisciplina(VS_tud_id);
                if (!dtPlanBimestre.AsEnumerable().Any(p => DateTime.Today >= Convert.ToDateTime(p.Field<object>("cap_dataInicio")) &&
                                                            DateTime.Today <= Convert.ToDateTime(p.Field<object>("cap_dataFim"))))
                {
                    if (dtPlanBimestre.AsEnumerable().Any(p => DateTime.Today < Convert.ToDateTime(p.Field<object>("cap_dataInicio"))))
                        tpc_idSelecionado = Convert.ToInt32(dtPlanBimestre.AsEnumerable()
                                                            .Where(p => DateTime.Today < Convert.ToDateTime(p.Field<object>("cap_dataInicio")))
                                                            .FirstOrDefault().Field<object>("tpc_id"));
                    else
                        tpc_idSelecionado = Convert.ToInt32(dtPlanBimestre.AsEnumerable()
                                                            .LastOrDefault().Field<object>("tpc_id"));
                }
                else
                    tpc_idSelecionado = Convert.ToInt32(dtPlanBimestre.AsEnumerable()
                                                        .Where(p => DateTime.Today >= Convert.ToDateTime(p.Field<object>("cap_dataInicio")) &&
                                                                    DateTime.Today <= Convert.ToDateTime(p.Field<object>("cap_dataFim")))
                                                        .FirstOrDefault().Field<object>("tpc_id"));

                rptPlanejamentoBimestre.DataSource = dtPlanBimestre;
                rptPlanejamentoBimestre.DataBind();

                if (VS_visaoDocente &&
                        // se nao for um docente da disciplina com docencia compartilhada, eu verifico pela posicao
                        (VS_turmaDisciplinaCompartilhada == null && PosicaoDocente != PosicaoTitular)
                        // se for um docente da disciplina com docencia compartilhada, eu verifico pela configuracao da disciplina se 
                        // nao permite lancar o planejamento em conjunto com o titular
                        || (VS_turmaDisciplinaCompartilhada != null && VS_turmaDisciplinaCompartilhada.tud_naoLancarPlanejamento))
                    txtDiagnosticoInicial.ReadOnly = txtProposta.ReadOnly = true;
            }
            catch (ValidationException ex)
            {
                lblMensagemAnual.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagemAnual.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o plano anual.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Seleciona o planejamento anual pela turma disciplina e posição titular. Salva plano anual
        /// </summary>
        /// <returns></returns>
        public bool ReplicarPlanoAnual()
        {
            try
            {
                List<CLS_TurmaDisciplinaPlanejamento> lstPlanejamento = new List<CLS_TurmaDisciplinaPlanejamento>();

                foreach (ListItem turma in chkTurmas.Items)
                {
                    if (!turma.Selected)
                        continue;

                    Int64 tud_id = Convert.ToInt64(turma.Value);
                    int tdp_id = -1;
                    DataTable dtPlanejamentoAnual = CLS_TurmaDisciplinaPlanejamentoBO.SelecionaPorTurmaDisciplinaPeriodoCalendarioNulo(VS_tud_id, PosicaoTitular);
                    if (dtPlanejamentoAnual.Rows.Count > 0)
                        VS_tdp_id = Convert.ToInt32(dtPlanejamentoAnual.Rows[0]["tdp_id"]);

                    CLS_TurmaDisciplinaPlanejamento planejamento = new CLS_TurmaDisciplinaPlanejamento
                    {
                        tud_id = tud_id,
                        tdp_id = tdp_id,
                        tpc_id = -1
                    };

                    CLS_TurmaDisciplinaPlanejamentoBO.GetEntity(planejamento);

                    planejamento.tdp_diagnostico = txtDiagnosticoInicial.Text;
                    planejamento.tdp_planejamento = txtProposta.Text;
                    if (planejamento.IsNew)
                    {
                        planejamento.tdt_posicao = PosicaoTitular;
                        planejamento.cur_id = VS_cur_id;
                        planejamento.crr_id = VS_crr_id;
                        planejamento.crp_id = VS_crp_id;
                        planejamento.tdp_situacao = 1;
                    }

                    lstPlanejamento.Add(planejamento);

                    DataTable dtPlanejamentoBimestre = CLS_TurmaDisciplinaPlanejamentoBO.SelecionaPorTurmaDisciplina(tud_id);

                    foreach (RepeaterItem item in rptPlanejamentoBimestre.Items)
                    {
                        HiddenField hdnTpcId = (HiddenField)item.FindControl("hdnTpcId");
                        TextBox txtPlanejamentoBimestre = (TextBox)item.FindControl("txtPlanejamentoBimestre");

                        if (!dtPlanejamentoBimestre.AsEnumerable()
                                                   .Any(p => Convert.ToInt32(p.Field<object>("tpc_id")) == Convert.ToInt32(hdnTpcId.Value)))
                            continue;

                        tdp_id = Convert.ToInt32(dtPlanejamentoBimestre.AsEnumerable()
                                                 .Where(p => Convert.ToInt32(p.Field<object>("tpc_id")) == Convert.ToInt32(hdnTpcId.Value))
                                                 .FirstOrDefault().Field<object>("tdp_id"));

                        planejamento = new CLS_TurmaDisciplinaPlanejamento
                        {
                            tud_id = tud_id,
                            tdp_id = tdp_id,
                            tpc_id = Convert.ToInt32(hdnTpcId.Value)
                        };

                        CLS_TurmaDisciplinaPlanejamentoBO.GetEntity(planejamento);

                        planejamento.tdp_planejamento = txtPlanejamentoBimestre.Text;
                        if (planejamento.IsNew)
                        {
                            planejamento.tdt_posicao = PosicaoTitular;
                            planejamento.cur_id = VS_cur_id;
                            planejamento.crr_id = VS_crr_id;
                            planejamento.crp_id = VS_crp_id;
                            planejamento.tdp_situacao = 1;
                        }

                        lstPlanejamento.Add(planejamento);
                    }
                }

                if (!lstPlanejamento.Any())
                    throw new ValidationException("Nenhuma turma selecionada para replicar os dados.");

                if (CLS_TurmaDisciplinaPlanejamentoBO.SalvaPlanejamentoTurmaDisciplina(lstPlanejamento))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, String.Format("Plano anual replicado | tud_id: {0};", VS_tud_id.ToString()));
                    // Removida mensagem de sucesso, pois já exibe na tela (fora do usercontrol).
                    //lblMensagemAnual.Text = UtilBO.GetErroMessage("Plano anual salvo e replicado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharPlanoAnual", "$('#divReplicarPlanejamentoAnual').dialog('close');", true);
                    return true;
                }
            }
            catch (ValidationException ex)
            {
                lblMessageReplicar.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageReplicar.Text = UtilBO.GetErroMessage("Erro ao tentar replicar o plano anual.", UtilBO.TipoMensagem.Erro);
            }

            return false;
        }

        /// <summary>
        /// Salva as alterações feitas do planejamento anual
        /// </summary>
        /// <returns></returns>
        public bool SalvarPlanoAnual()
        {
            try
            {
                if (VS_visaoDocente &&
                        // se nao for um docente da disciplina com docencia compartilhada, eu verifico pela posicao
                        (VS_turmaDisciplinaCompartilhada == null && PosicaoDocente != PosicaoTitular)
                        // se for um docente da disciplina com docencia compartilhada, eu verifico pela configuracao da disciplina se 
                        // nao permite lancar o planejamento em conjunto com o titular
                        || (VS_turmaDisciplinaCompartilhada != null && VS_turmaDisciplinaCompartilhada.tud_naoLancarPlanejamento))
                    return false;

                List<CLS_TurmaDisciplinaPlanejamento> lstPlanejamento = new List<CLS_TurmaDisciplinaPlanejamento>();

                CLS_TurmaDisciplinaPlanejamento planejamento = new CLS_TurmaDisciplinaPlanejamento
                {
                    tud_id = VS_tud_id,
                    tdp_id = VS_tdp_id,
                    tpc_id = -1
                };
                CLS_TurmaDisciplinaPlanejamentoBO.GetEntity(planejamento);

                planejamento.tdp_diagnostico = txtDiagnosticoInicial.Text;
                planejamento.tdp_planejamento = txtProposta.Text;
                planejamento.tdt_posicao = PosicaoTitular;
                planejamento.cur_id = VS_cur_id;
                planejamento.crr_id = VS_crr_id;
                planejamento.crp_id = VS_crp_id;
                planejamento.tdp_situacao = 1;

                lstPlanejamento.Add(planejamento);

                foreach (RepeaterItem item in rptPlanejamentoBimestre.Items)
                {
                    HiddenField hdnTdpId = (HiddenField)item.FindControl("hdnTdpId");
                    HiddenField hdnTpcId = (HiddenField)item.FindControl("hdnTpcId");
                    TextBox txtPlanejamentoBimestre = (TextBox)item.FindControl("txtPlanejamentoBimestre");

                    planejamento = new CLS_TurmaDisciplinaPlanejamento
                    {
                        tud_id = VS_tud_id,
                        tdp_id = string.IsNullOrEmpty(hdnTdpId.Value) ? -1 : Convert.ToInt32(hdnTdpId.Value),
                        tpc_id = Convert.ToInt32(hdnTpcId.Value)
                    };
                    CLS_TurmaDisciplinaPlanejamentoBO.GetEntity(planejamento);

                    planejamento.tdp_planejamento = txtPlanejamentoBimestre.Text;
                    planejamento.tdt_posicao = PosicaoTitular;
                    planejamento.cur_id = VS_cur_id;
                    planejamento.crr_id = VS_crr_id;
                    planejamento.crp_id = VS_crp_id;
                    planejamento.tdp_situacao = 1;

                    lstPlanejamento.Add(planejamento);
                }

                if (CLS_TurmaDisciplinaPlanejamentoBO.SalvaPlanejamentoTurmaDisciplina(lstPlanejamento))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, String.Format("Plano anual | tud_id: {0};", VS_tud_id.ToString()));
                    // Removida mensagem de sucesso, pois já exibe na tela (fora do usercontrol).
                    //lblMensagemAnual.Text = UtilBO.GetErroMessage("Plano anual salvo com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    if (VS_visaoDocente)
                    {
                        chkTurmas.Items.Clear();
                        chkTurmas.DataSource = CLS_TurmaDisciplinaPlanejamentoBO.SelecionaOutrasTurmasDocente(VS_tur_id, VS_cal_id, VS_cur_id, VS_crr_id, VS_crp_id, VS_tud_id, PosicaoTitular);
                        chkTurmas.DataBind();

                        if (chkTurmas.Items.Count > 0)
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "ReplicarPlanejamento", "$(document).ready(function() { $('#divReplicarPlanejamentoAnual').dialog('open'); });", true);
                    }
                    CarregarPlanoAnual();

                    return true;
                }
            }
            catch (ValidationException ex)
            {
                lblMensagemAnual.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagemAnual.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o plano anual.", UtilBO.TipoMensagem.Erro);
            }

            return false;
        }

        #endregion Plano anual

        #region Plano aluno

        /// <summary>
        /// Carrega os planejamentos dos alunos com as disciplinas relacionadas
        /// </summary>
        private void CarregarPlanoAlunos()
        {
            UCCAlunos.TituloCombo = "Aluno(a)";
            UCCAlunos.Visible_Label = false;
            UCCAlunos.CarregarCombo(CLS_AlunoPlanejamentoBO.SelecionaAlunosPorTud(tud_id: VS_tud_id, documentoOficial: false, tur_ids: VS_TurmasNormais_Ids), "nomeAluno", "alu_id");
            UCCAlunos.SelectedIndex = 0;

            VS_AlunoPlanejamento = CLS_AlunoPlanejamentoBO.SelecionaPlanejamentoAlunoRelacionados(VS_tud_id, null, VS_TurmasNormais_Ids);
        }

        /// <summary>
        /// Salva no VS_AlunoPlanejamento a alteração do aluno
        /// </summary>
        /// <param name="alu_id"></param>
        private void SalvaPlanoAluno(long alu_id)
        {
            try
            {
                if (alu_id > 0)
                {
                    VS_AlunoPlanejamento = (from AlunoPlanejamento alunoPlanejamento in VS_AlunoPlanejamento
                                            select new AlunoPlanejamento
                                            {
                                                alu_id = alunoPlanejamento.alu_id
                                                ,
                                                apl_id = alunoPlanejamento.apl_id
                                                ,
                                                apl_planejamento = alunoPlanejamento.alu_id == alu_id ? txtPlanoAluno.Text : alunoPlanejamento.apl_planejamento
                                                ,
                                                tud_id = alunoPlanejamento.tud_id
                                                ,
                                                alterado = alunoPlanejamento.alu_id == alu_id ?
                                                    (alunoPlanejamento.alterado ?
                                                        true :
                                                        !(txtPlanoAluno.Text == alunoPlanejamento.apl_planejamento)) :
                                                    alunoPlanejamento.alterado
                                                ,
                                                AlunoPlanejamentoRelacionada = (from AlunoPlanejamentoRelacionada alunoPlanejamentoRelacionada in alunoPlanejamento.AlunoPlanejamentoRelacionada
                                                                                select new AlunoPlanejamentoRelacionada
                                                                                {
                                                                                    alu_id = alunoPlanejamentoRelacionada.alu_id
                                                                                    ,
                                                                                    tud_id = alunoPlanejamentoRelacionada.tud_id
                                                                                    ,
                                                                                    apl_id = alunoPlanejamentoRelacionada.apl_id
                                                                                    ,
                                                                                    Relacionado = alunoPlanejamentoRelacionada.alu_id == alu_id ?
                                                                                        (chlTurmaDiscRelacionada.Items.FindByValue(alunoPlanejamentoRelacionada.tud_idRelacionado.ToString()).Selected ? true : false) :
                                                                                        alunoPlanejamentoRelacionada.Relacionado
                                                                                    ,
                                                                                    tud_idRelacionado = alunoPlanejamentoRelacionada.tud_idRelacionado
                                                                                    ,
                                                                                    dis_nome = alunoPlanejamentoRelacionada.dis_nome
                                                                                    ,
                                                                                    alterado = alunoPlanejamentoRelacionada.alu_id == alu_id ?
                                                                                        (alunoPlanejamentoRelacionada.alterado ?
                                                                                            true :
                                                                                            !(chlTurmaDiscRelacionada.Items.FindByValue(alunoPlanejamentoRelacionada.tud_idRelacionado.ToString()).Selected == alunoPlanejamentoRelacionada.Relacionado)) :
                                                                                         alunoPlanejamentoRelacionada.alterado

                                                                                }).ToList()
                                            }

                    ).ToList();

                }
            }
            catch (ValidationException ex)
            {
                lblMensagemAluno.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagemAluno.Text = UtilBO.GetErroMessage("Erro ao tentar gravar o plano do aluno.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega o planejamento com as disciplinas relacionadas do aluno
        /// </summary>
        /// <param name="alu_id"></param>
        private void CarregarPlanoAluno(long alu_id)
        {
            try
            {
                if (Convert.ToInt64(UCCAlunos.Valor.Split(';')[0]) <= 0)
                    UCCAlunos.Valor = alu_id.ToString();

                divAluno.Visible = divTurmaDisciplinaAluno.Visible = false;
                if (alu_id > 0)
                {
                    VS_plano_alu_id = alu_id;

                    AlunoPlanejamento alunoPlanejamento = VS_AlunoPlanejamento.Find(p => p.alu_id == alu_id);

                    //todo: Verificar se utiliza os metodos/STPs em mais lugares
                    //DataTable dtTurmaDisciplinaRelacionada = CLS_AlunoPlanejamentoRelacionadaBO.SelecionaPlanejamentoAlunoRelacionada(alu_id, VS_tud_id, alunoPlanejamento.apl_id);
                    //CLS_AlunoPlanejamento alunoPlanejamento = CLS_AlunoPlanejamentoBO.SelecionaPlanejamentoAluno(alu_id, VS_tud_id);

                    chlTurmaDiscRelacionada.DataSource = alunoPlanejamento.AlunoPlanejamentoRelacionada.OrderBy(p => p.dis_nome);
                    chlTurmaDiscRelacionada.DataBind();

                    chlTurmaDiscRelacionada.Visible = chlTurmaDiscRelacionada.Items.Count > 0;

                    foreach (ListItem item in chlTurmaDiscRelacionada.Items)
                        item.Selected = (alunoPlanejamento.AlunoPlanejamentoRelacionada.AsEnumerable().Where(i => Convert.ToInt64(i.tud_idRelacionado) == Convert.ToInt64(item.Value) &&
                                                                                                    i.Relacionado).Any());

                    txtPlanoAluno.Text = alunoPlanejamento.apl_planejamento;

                    divAluno.Visible = true;

                    if (VS_visaoDocente &&
                            // se nao for um docente da disciplina com docencia compartilhada, eu verifico pela posicao
                            (VS_turmaDisciplinaCompartilhada == null && PosicaoDocente != PosicaoTitular)
                            // se for um docente da disciplina com docencia compartilhada, eu verifico pela configuracao da disciplina se 
                            // nao permite lancar o planejamento em conjunto com o titular
                            || (VS_turmaDisciplinaCompartilhada != null && VS_turmaDisciplinaCompartilhada.tud_naoLancarPlanejamento))
                    {
                        txtPlanoAluno.ReadOnly = true;
                    }
                }
            }
            catch (ValidationException ex)
            {
                lblMensagemAluno.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagemAluno.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o plano do aluno.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Salva no banco, todas as alterações dos planejamentos e matérias relacionadas dos alunos
        /// </summary>
        /// <returns></returns>
        public bool SalvarPlanoAluno()
        {
            try
            {

                long alu_id = Convert.ToInt64(UCCAlunos.Valor.Split(';')[0]);
                SalvaPlanoAluno(alu_id);

                if (VS_visaoDocente &&
                        // se nao for um docente da disciplina com docencia compartilhada, eu verifico pela posicao
                        (VS_turmaDisciplinaCompartilhada == null && PosicaoDocente != PosicaoTitular)
                        // se for um docente da disciplina com docencia compartilhada, eu verifico pela configuracao da disciplina se 
                        // nao permite lancar o planejamento em conjunto com o titular
                        || (VS_turmaDisciplinaCompartilhada != null && VS_turmaDisciplinaCompartilhada.tud_naoLancarPlanejamento))
                    return false;

                if (CLS_AlunoPlanejamentoBO.Salvar(VS_AlunoPlanejamento))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, String.Format("Plano aluno | tud_id: {0}; alu_id: {1}; apl_id: (2);", VS_tud_id.ToString(), alu_id.ToString(), VS_AlunoPlanejamento.Find(p => p.alu_id == alu_id).apl_id));
                    // Removida mensagem de sucesso, pois já exibe na tela (fora do usercontrol).
                    //lblMensagemAluno.Text = UtilBO.GetErroMessage("Plano do aluno salvo com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    CarregarPlanoAlunos();
                    CarregarPlanoAluno(alu_id);
                    return true;
                }
            }
            catch (ValidationException ex)
            {
                lblMensagemAluno.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagemAluno.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o plano do aluno.", UtilBO.TipoMensagem.Erro);
            }

            return false;
        }

        #endregion Plano aluno

        #region Documentos

        /// <summary>
        /// Carrega os documentos ativos
        /// </summary>
        private void CarregarDocumentos()
        {
            DataTable dtArea = ACA_TipoAreaDocumentoBO.SelecionarAtivos();
            dtArea.Columns.Add("PPP");

            // Adiciona PPP
            DataRow drPPP = dtArea.NewRow();
            drPPP["tad_id"] = -1;
            drPPP["tad_nome"] = GetGlobalResourceObject("Mensagens", "MSG_PLANO_POLITICO_PEDAGOGICO").ToString();
            drPPP["PPP"] = true;
            drPPP["tad_cadastroEscolaBoolean"] = true;
            dtArea.Rows.Add(drPPP);

            rptAreas.DataSource = dtArea;
            rptAreas.DataBind();

            lblSemAreas.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCPlanejamentoProjetos.lblSemAreas.Text"),
                                                     UtilBO.TipoMensagem.Informacao);
            lblSemAreas.Visible = rptAreas.Items.Count <= 0;
        }

        #endregion Documentos

        #region Objeto de Aprendizagem

        /// <summary>
        /// Salva no banco, todas as alterações dos objetos de aprendizagem
        /// </summary>
        /// <returns></returns>
        public void SalvarObjetoAprendizagemTurmaDisciplina()
        {
            try
            {
                AtualizarListaObjetos();

                List<CLS_ObjetoAprendizagemTurmaDisciplina> listObjTudDis = VS_lstObjetosAprendizagem.Where(p => p.selecionado)
                                                                            .Select(p => new CLS_ObjetoAprendizagemTurmaDisciplina
                                                                            {
                                                                                tud_id = p.tud_id,
                                                                                tpc_id = p.tpc_id,
                                                                                oap_id = p.oap_id
                                                                            }).ToList();

                if (VS_tud_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.Regencia) &&
                    ddlComponenteAtAvaliativa.Items.Count > 0)
                {
                    List<long> lstTuds = (from dr in VS_TurmaDisciplinaDocente
                                          where Convert.ToByte(dr.tur_tud_id.Split(';')[3]) == Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                          && (Convert.ToInt64(dr.tur_tud_id.Split(';')[0]) == VS_tur_id)
                                          select Convert.ToInt64(dr.tur_tud_id.Split(';')[1])).ToList();

                    CLS_ObjetoAprendizagemTurmaDisciplinaBO.SalvarLista(listObjTudDis, lstTuds, VS_cal_id, null, VS_tud_id);
                }
                else
                    CLS_ObjetoAprendizagemTurmaDisciplinaBO.SalvarLista(listObjTudDis, new List<long> { VS_tud_id }, VS_cal_id);

                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, String.Format("Objeto Conhecimento Turma Disciplina | tud_id: {0}; ", VS_tud_id.ToString()));
            }
            catch (ValidationException ex)
            {
                lblMensagemAluno.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagemAluno.Text = UtilBO.GetErroMessage("Erro ao tentar salvar objetos de conhecimento.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Atualiza a lista VS_lstObjetosAprendizagem com os objetos selecionados na tela (ou dos componentes)
        /// </summary>
        private void AtualizarListaObjetos()
        {
            List<CLS_ObjetoAprendizagemTurmaDisciplina> listSelecionados = CriarListaObjetoAprendizagemTurmaDisciplina();

            List<Struct_ObjetosAprendizagem> VS_lstObjetosAprendizagemAux = new List<Struct_ObjetosAprendizagem>();
            VS_lstObjetosAprendizagemAux = VS_lstObjetosAprendizagem.Select(item => new Struct_ObjetosAprendizagem
            {
                tud_id = item.tud_id,
                oap_id = item.oap_id,
                oap_descricao = item.oap_descricao,
                oap_situacao = item.oap_situacao,
                tpc_id = item.tpc_id,
                tpc_nome = item.tpc_nome,
                tpc_ordem = item.tpc_ordem,
                selecionado = (VS_tud_tipo != Convert.ToByte(ACA_CurriculoDisciplinaTipo.Regencia) &&
                                item.tud_id == VS_tud_id &&
                                listSelecionados.Any(p => p.tud_id == item.tud_id && p.oap_id == item.oap_id && p.tpc_id == item.tpc_id)) ||
                                (VS_tud_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.Regencia) &&
                                ddlComponenteAtAvaliativa.Items.Count > 0 &&
                                item.tud_id == VS_tud_idComponenteSelecionado &&
                                listSelecionados.Any(p => p.tud_id == item.tud_id && p.oap_id == item.oap_id && p.tpc_id == item.tpc_id)) ||
                                (VS_tud_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.Regencia) &&
                                ddlComponenteAtAvaliativa.Items.Count > 0 &&
                                item.tud_id != VS_tud_idComponenteSelecionado &&
                                item.selecionado),
                oae_id = item.oae_id,
                oae_descricao = item.oae_descricao,
                oae_ordem = item.oae_ordem,
                oae_idSub = item.oae_idSub,
                oae_descricaoSub = item.oae_descricaoSub,
                oae_ordemSub = item.oae_ordemSub
            }).ToList();

            VS_lstObjetosAprendizagem = VS_lstObjetosAprendizagemAux;
        }

        private List<CLS_ObjetoAprendizagemTurmaDisciplina> CriarListaObjetoAprendizagemTurmaDisciplina()
        {
            List<CLS_ObjetoAprendizagemTurmaDisciplina> lstObjTudDis = new List<CLS_ObjetoAprendizagemTurmaDisciplina>();
            if (rptEixo.Visible)
            {
                foreach (RepeaterItem itemEixo in rptEixo.Items)
                {
                    if (itemEixo.ItemType == ListItemType.Item || itemEixo.ItemType == ListItemType.AlternatingItem)
                    {
                        Repeater rptSubEixo = (Repeater)itemEixo.FindControl("rptSubEixo");
                        if (rptSubEixo != null)
                        {
                            foreach (RepeaterItem itemSubEixo in rptSubEixo.Items)
                            {
                                if (itemSubEixo.ItemType == ListItemType.Item || itemSubEixo.ItemType == ListItemType.AlternatingItem)
                                {
                                    Repeater rptobjAprendizagemSub = (Repeater)itemSubEixo.FindControl("rptobjAprendizagem");
                                    if (rptobjAprendizagemSub != null)
                                    {
                                        CriarListaRepeater(ref lstObjTudDis, rptobjAprendizagemSub);
                                    }
                                }
                            }
                        }

                        Repeater rptobjAprendizagem = (Repeater)itemEixo.FindControl("rptobjAprendizagem");
                        if (rptobjAprendizagem != null)
                        {
                            CriarListaRepeater(ref lstObjTudDis, rptobjAprendizagem);
                        }
                    }
                }
            }
            return lstObjTudDis;
        }

        private void CriarListaRepeater(ref List<CLS_ObjetoAprendizagemTurmaDisciplina> lstObjTudDis, Repeater rptobjAprendizagem)
        {
            foreach (RepeaterItem itemObj in rptobjAprendizagem.Items)
            {
                Repeater rptchkBimestre = (Repeater)itemObj.FindControl("rptchkBimestre");
                if (rptchkBimestre != null)
                {
                    foreach (RepeaterItem chk in rptchkBimestre.Items)
                    {
                        CheckBox ckbCampo = (CheckBox)chk.FindControl("ckbCampo");

                        if (ckbCampo != null && ckbCampo.Checked)
                        {
                            HiddenField tpc_id = (HiddenField)chk.FindControl("tpc_id");
                            HiddenField oap_id = (HiddenField)chk.FindControl("oap_id");
                            if (tpc_id != null && oap_id != null)
                            {
                                if (VS_tud_tipo == Convert.ToByte(ACA_CurriculoDisciplinaTipo.Regencia) &&
                                    ddlComponenteAtAvaliativa.Items.Count > 0)
                                {
                                    lstObjTudDis.Add(new CLS_ObjetoAprendizagemTurmaDisciplina
                                    {
                                        tud_id = VS_tud_idComponenteSelecionado,
                                        tpc_id = Convert.ToInt32(tpc_id.Value),
                                        oap_id = Convert.ToInt32(oap_id.Value)
                                    });
                                }
                                else
                                {
                                    lstObjTudDis.Add(new CLS_ObjetoAprendizagemTurmaDisciplina
                                    {
                                        tud_id = VS_tud_id,
                                        tpc_id = Convert.ToInt32(tpc_id.Value),
                                        oap_id = Convert.ToInt32(oap_id.Value)
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion Objeto de Aprendizagem

        #region Eventos

        #region Plano do ciclo

        protected void btnEditarPlanoCiclo_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            txtPlanoCiclo.Enabled = true;
            btn.Visible = false;
            btnCancelarCiclo.Visible = true;
        }

        protected void btnSalvarPlanoCiclo_Click(object sender, EventArgs e)
        {
            SalvarPlanoCiclo();
        }

        protected void btnCancelarCiclo_Click(object sender, EventArgs e)
        {
            try
            {
                CLS_PlanejamentoCiclo entityCiclo = CLS_PlanejamentoCicloBO.SelecionaAtivoPorTurmaTipoCiclo(VS_tur_id, Convert.ToInt32(uccTipoCiclo.Valor));

                txtPlanoCiclo.Text = string.IsNullOrEmpty(entityCiclo.plc_planoCiclo) ? string.Empty : entityCiclo.plc_planoCiclo;

                Button btn = (Button)sender;
                txtPlanoCiclo.Enabled = false;
                btn.Visible = false;
                btnEditarPlanoCiclo.Visible = true;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagemCiclo.Text = UtilBO.GetErroMessage("Erro ao carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Plano do ciclo

        #region Plano anual

        protected void rptPlanejamentoBimestre_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblPlanejamentoBimestre = (Label)e.Item.FindControl("lblPlanejamentoBimestre");
                ImageButton btnTextoGrandePlanejamentoBimestre = (ImageButton)e.Item.FindControl("btnTextoGrandePlanejamentoBimestre");
                Label lblPlanejamentoBimestreInfo = (Label)e.Item.FindControl("lblPlanejamentoBimestreInfo");
                TextBox txtPlanejamentoBimestre = (TextBox)e.Item.FindControl("txtPlanejamentoBimestre");
                ImageButton btnVoltaEstadoAnteriorTextoPlanejamentoBimestre = (ImageButton)e.Item.FindControl("btnVoltaEstadoAnteriorTextoPlanejamentoBimestre");
                HiddenField hdnBimestreVisivel = (HiddenField)e.Item.FindControl("hdnBimestreVisivel");

                DateTime cap_dataInicio = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "cap_dataInicio"));
                DateTime cap_dataFim = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "cap_dataFim"));
                int tpc_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tpc_id"));

                if ((DateTime.Today <= cap_dataInicio || DateTime.Today >= cap_dataFim) && tpc_id != tpc_idSelecionado)
                {
                    txtPlanejamentoBimestre.Style.Add("display", "none");
                    hdnBimestreVisivel.Value = "false";
                }

                btnTextoGrandePlanejamentoBimestre.OnClientClick =
                   String.Format("abrirTextoGrandeComMensagem('{0}','{1}','{2}', '{3}', '{4}'); return false;",
                       lblPlanejamentoBimestre.ClientID,
                       txtPlanejamentoBimestre.ClientID,
                       DataBinder.Eval(e.Item.DataItem, "cap_descricao").ToString(),
                       btnTextoGrandePlanejamentoBimestre.ClientID,
                       btnVoltaEstadoAnteriorTextoPlanejamentoBimestre.ClientID);

                btnVoltaEstadoAnteriorTextoPlanejamentoBimestre.OnClientClick =
                   String.Format("abrirTextoPequenoComMensagem('{0}','{1}','{2}', '{3}', '{4}', '{5}'); return false;",
                       lblPlanejamentoBimestre.ClientID,
                       txtPlanejamentoBimestre.ClientID,
                       DataBinder.Eval(e.Item.DataItem, "cap_descricao").ToString(),
                       btnTextoGrandePlanejamentoBimestre.ClientID,
                       btnVoltaEstadoAnteriorTextoPlanejamentoBimestre.ClientID,
                       hdnBimestreVisivel.Value);

                btnTextoGrandePlanejamentoBimestre.OnClientClick = "abrirTextoGrande('" + lblPlanejamentoBimestre.ClientID + "', '" + txtPlanejamentoBimestre.ClientID + "', '" + btnTextoGrandePlanejamentoBimestre.ClientID + "', '" + btnVoltaEstadoAnteriorTextoPlanejamentoBimestre.ClientID + "'); return false;";

                btnVoltaEstadoAnteriorTextoPlanejamentoBimestre.OnClientClick = "abrirTextoPequeno('" + lblPlanejamentoBimestre.ClientID + "', '" + txtPlanejamentoBimestre.ClientID + "', '" + btnTextoGrandePlanejamentoBimestre.ClientID + "', '" + btnVoltaEstadoAnteriorTextoPlanejamentoBimestre.ClientID + "', '" + hdnBimestreVisivel.Value + "'); return false;";

                if (VS_visaoDocente &&
                        // se nao for um docente da disciplina com docencia compartilhada, eu verifico pela posicao
                        (VS_turmaDisciplinaCompartilhada == null && PosicaoDocente != PosicaoTitular)
                        // se for um docente da disciplina com docencia compartilhada, eu verifico pela configuracao da disciplina se 
                        // nao permite lancar o planejamento em conjunto com o titular
                        || (VS_turmaDisciplinaCompartilhada != null && VS_turmaDisciplinaCompartilhada.tud_naoLancarPlanejamento))
                    txtPlanejamentoBimestre.ReadOnly = true;
            }
        }

        protected void btnReplicar_Click(object sender, EventArgs e)
        {
            if (ReplicaPlanoAnual != null)
                ReplicaPlanoAnual();
        }

        #endregion Plano anual

        #region Plano aluno

        protected void grvTurmaDisciplinaAluno_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnVisualizar = (ImageButton)e.Row.FindControl("btnVisualizar");
                if (btnVisualizar != null)
                    btnVisualizar.CommandArgument = e.Row.RowIndex.ToString();

                Image imgLancado = (Image)e.Row.FindControl("imgLancado");
                if (imgLancado != null)
                    imgLancado.Visible = grvTurmaDisciplinaAluno.DataKeys[e.Row.RowIndex].Values["tur_id"].Equals("1");
            }
        }

        protected void grvTurmaDisciplinaAluno_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Visualizar")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                divPlanoAluno.Attributes.Add("title", grvTurmaDisciplinaAluno.DataKeys[index].Values["turma"].ToString());
                txtPlanoAluno.Text = grvTurmaDisciplinaAluno.DataKeys[index].Values["planejamento"].ToString();
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "PlanoAluno", "$('.divPlanoAluno').dialog('open');});", true);
            }
        }

        protected void btnFecharPlanoAluno_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharPlanoAluno", "$('.divPlanoAluno').dialog('close');", true);
        }

        #endregion Plano aluno

        #region Documentos

        protected void rptAreas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int tad_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tad_id"));
                int esc_id = !Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "tad_cadastroEscolaBoolean")) ? 0 : VS_esc_id;
                bool ppp = DataBinder.Eval(e.Item.DataItem, "PPP") != DBNull.Value && Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "PPP"));

                DataTable dtDocumentos = ACA_ArquivoAreaBO.GetSelectBy_Id_Dre_Escola(tad_id, esc_id, new Guid(), tne_id, ppp, ppp);

                if (dtDocumentos.Rows.Count > 0)
                {
                    Repeater rptDocumentos = (Repeater)e.Item.FindControl("rptDocumentos");
                    rptDocumentos.Visible = true;
                    rptDocumentos.DataSource = dtDocumentos;
                    rptDocumentos.DataBind();
                }
                else
                {
                    e.Item.Visible = false;
                    //Label lblSemDocumentos = (Label)e.Item.FindControl("lblSemDocumentos");
                    //lblSemDocumentos.Text = UtilBO.GetErroMessage((string)GetGlobalResourceObject("UserControl", "UCPlanejamentoProjetos.lblSemDocumentos.Text"),
                    //                                              UtilBO.TipoMensagem.Informacao);
                    //lblSemDocumentos.Visible = true;
                }
            }
        }

        protected void rptDocumentos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HyperLink hplDocumento = (HyperLink)e.Item.FindControl("hplDocumento");
                if (Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "aar_tipoDocumento")) == 1)
                {
                    string arq_id = DataBinder.Eval(e.Item.DataItem, "arq_id").ToString();
                    hplDocumento.NavigateUrl = String.Format("~/FileHandler.ashx?file={0}", arq_id);
                }
                else
                {
                    string httpLink = DataBinder.Eval(e.Item.DataItem, "aar_link").ToString();
                    if (!httpLink.Contains("://"))
                        httpLink = "http://" + httpLink;
                    hplDocumento.NavigateUrl = httpLink;
                    hplDocumento.Target = "_blank";
                }
            }
        }

        #endregion Documentos

        #region Objetos Aprendizagem

        protected void rptEixo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int oae_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "oae_id"));

                if (VS_lstObjetosAprendizagem.Any(p => p.oae_id == oae_id && p.oae_idSub > 0))
                {
                    Repeater rptSubEixo = (Repeater)e.Item.FindControl("rptSubEixo");
                    if (rptSubEixo != null)
                    {
                        rptSubEixo.DataSource = (
                            from Struct_ObjetosAprendizagem dr in VS_lstObjetosAprendizagem.Where(p => p.oae_id == oae_id && p.oae_idSub > 0)
                            group dr by dr.oae_idSub into g
                            select new
                            {
                                oae_id = g.Key
                                ,
                                oae_descricao = g.First().oae_descricaoSub
                                ,
                                oae_ordem = g.First().oae_ordemSub
                            }
                        ).OrderBy(p => p.oae_ordem).ToList();
                        rptSubEixo.DataBind();
                    }
                }

                if (VS_lstObjetosAprendizagem.Any(p => p.oae_id == oae_id && p.oae_idSub <= 0))
                {
                    Repeater rptobjAprendizagem = (Repeater)e.Item.FindControl("rptobjAprendizagem");
                    if (rptobjAprendizagem != null)
                    {
                        rptobjAprendizagem.DataSource = (
                            from Struct_ObjetosAprendizagem dr in VS_lstObjetosAprendizagem.Where(p => p.oae_id == oae_id && p.oae_idSub <= 0)
                            group dr by dr.oap_id into g
                            select new
                            {
                                oap_id = g.Key
                                ,
                                oap_descricao = g.First().oap_descricao
                            }
                        ).OrderBy(p => p.oap_descricao).ToList();
                        rptobjAprendizagem.DataBind();
                    }
                }
            }
        }

        protected void rptSubEixo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int oae_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "oae_id"));

                Repeater rptobjAprendizagem = (Repeater)e.Item.FindControl("rptobjAprendizagem");
                if (rptobjAprendizagem != null)
                {
                    rptobjAprendizagem.DataSource = (
                        from Struct_ObjetosAprendizagem dr in VS_lstObjetosAprendizagem.Where(p => p.oae_idSub == oae_id)
                        group dr by dr.oap_id into g
                        select new
                        {
                            oap_id = g.Key
                            ,
                            oap_descricao = g.First().oap_descricao
                        }
                    ).OrderBy(p => p.oap_descricao).ToList();
                    rptobjAprendizagem.DataBind();
                }
            }
        }

        protected void rptobjAprendizagem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                Repeater rptBimestre = (Repeater)e.Item.FindControl("rptBimestre");
                if (rptBimestre != null)
                {
                    rptBimestre.DataSource = VS_lstObjetosAprendizagem.OrderBy(r => r.tpc_ordem)
                                             .Select(p => new { tpc_id = p.tpc_id, tpc_nome = p.tpc_nome, tpc_ordem = p.tpc_ordem }).Distinct();
                    rptBimestre.DataBind();
                }
            }
            else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rptchkBimestre = (Repeater)e.Item.FindControl("rptchkBimestre");
                if (rptchkBimestre != null)
                {
                    var lst = VS_lstObjetosAprendizagem.Where(p => p.oap_id == Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "oap_id")))
                                                .OrderBy(r => r.tpc_ordem).Select(p => new { tpc_id = p.tpc_id, oap_id = p.oap_id, oap_situacao = p.oap_situacao, selecionado = p.selecionado });
                    rptchkBimestre.DataSource = lst;
                    rptchkBimestre.DataBind();
                }
            }
        }

        protected void rptchkBimestre_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox ckb = (CheckBox)e.Item.FindControl("ckbCampo");
                HiddenField oap_situacao = (HiddenField)e.Item.FindControl("oap_situacao");
                if (ckb != null)
                {
                    ckb.Enabled = VS_permiteEditarObjAprendizagem && PermiteEdicao;

                    if (oap_situacao.Value == "2")
                        ckb.Enabled = false;
                }
            }
        }

        protected void ddlComponenteAtAvaliativa_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarListaObjetos();

            CarregaRepeaterObjetoAprendizagem();
        }

        #endregion Objetos Aprendizagem

        #endregion Eventos
    }
}

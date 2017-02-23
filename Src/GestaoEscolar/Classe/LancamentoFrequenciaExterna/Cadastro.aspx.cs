namespace GestaoEscolar.Classe.LancamentoFrequenciaExterna
{
    using MSTech.CoreSSO.BLL;
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.Web.WebProject;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;


    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

         /// <summary>
        /// Viewstate que armazena o ID do aluno.
        /// </summary>
        public long VS_alu_id
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
        /// Viewstate que armazena o ID da matrícula turma do aluno.
        /// </summary>
        private int VS_mtu_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_mtu_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_mtu_id"] = value;
            }
        }

        private List<DadosAlunoEntradaRede> VS_ListaDadosPeriodo
        {
            get
            {
                if (ViewState["VS_ListaDadosPeriodo"] != null)
                {
                    return (List<DadosAlunoEntradaRede>)ViewState["VS_ListaDadosPeriodo"];
                }

                return (List<DadosAlunoEntradaRede>)(ViewState["VS_ListaDadosPeriodo"] = new List<DadosAlunoEntradaRede>());
            }

            set
            {
                ViewState["VS_ListaDadosPeriodo"] = value;
            }
        }

        #endregion Propriedades

        #region Variáveis

        /// <summary>
        /// Guarda a quantidade de itens do tipo filho
        /// </summary>
        private int QtComponenteRegencia;

        /// <summary>
        /// Guarda a quantidade de itens a serem exibidos no boletim
        /// </summary>
        private int QtComponentes;

        /// <summary>
        /// Variável de controle para colocar Rowspan apenas na primeira linha quando for disciplina de regência.
        /// </summary>
        private bool ControleMescla = false;

        #endregion Variáveis

        #region Page Life Cicle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroFrequenciaExterna.js"));
            }

            if (!IsPostBack)
            {
                if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                {
                    VS_alu_id = PreviousPage.EditAluId;
                    VS_mtu_id = PreviousPage.EditMtuId;

                    CarregarDadosAluno();
                }
            }
        }

        #endregion Page Life Cicle

        #region Métodos

        public void CarregarDadosAluno()
        {
            int tev_idFechamento = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            VS_ListaDadosPeriodo = MTR_MatriculaTurmaBO.SelecionarDadosAlunoEntradaRede(VS_alu_id, VS_mtu_id, tev_idFechamento, false);

            if (VS_ListaDadosPeriodo.Any())
            {
                DadosAlunoEntradaRede dados = VS_ListaDadosPeriodo.First();

                #region Cabeçalho - dados do aluno

                // Carrega os dados do aluno.
                lblNomeAluno.Text = dados.pes_nome;
                lblTurma.Text = dados.tur_codigo;
                if (dados.mtu_numeroChamada > 0)
                {
                    lblNumeroChamada.Text = Convert.ToString(dados.mtu_numeroChamada);
                }
                else
                {
                    lblNumeroChamada.Text = "-";
                }
                lblCodigoEol.Text = dados.alc_matricula;

                if (dados.arq_idFoto > 0)
                {
                    imgFotoAluno.ImageUrl = string.Format("~/WebControls/BoletimCompletoAluno/Imagem.ashx?idfoto={0}", dados.arq_idFoto);
                }
                else
                {
                    imgFotoAluno.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "imgsAreaAluno/fotoAluno.png";
                }

                #endregion

                CarregarLancamento();
            }
        }

        private void CarregarLancamento()
        {
            List<DadosLancamentoFreqExterna> lDadosAluno = CLS_AlunoFrequenciaExternaBO.SelecionaDadosAlunoLancamentoFrequenciaExterna(VS_alu_id, VS_mtu_id);

            if (lDadosAluno.Any())
            {
                int cur_id = lDadosAluno.First().cur_id;
                ACA_Curso cur = new ACA_Curso { cur_id = cur_id };
                ACA_CursoBO.GetEntity(cur);

                //Verifica se o nível de ensino do curso é do ensino infantil.
                bool ensinoInfantil = cur.tne_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                #region Períodos

                var periodos = from DadosAlunoEntradaRede item in VS_ListaDadosPeriodo
                               orderby item.tpc_ordem
                               group item by item.tpc_id
                                  into g
                               select
                                   new
                                   {
                                       tpc_id = g.Key
                                       ,
                                       tpc_nome = g.First().cap_descricao
                                       ,
                                       g.First().tpc_ordem
                                   };

                rptPeriodosNomes.DataSource = periodos;
                rptPeriodosColunasFixas.DataSource = periodos;
                rptPeriodosNomesEnriquecimento.DataSource = periodos;
                rptPeriodosNomesEI.DataSource = periodos;
                rptPeriodosColunasFixasEnriquecimento.DataSource = periodos;
                rptPeriodosColunasFixasEI.DataSource = periodos;
                rptPeriodosNomes.DataBind();
                rptPeriodosColunasFixas.DataBind();
                rptPeriodosNomesEnriquecimento.DataBind();
                rptPeriodosNomesEI.DataBind();
                rptPeriodosColunasFixasEnriquecimento.DataBind();
                rptPeriodosColunasFixasEI.DataBind();

                #endregion Períodos

                #region Disciplinas

                bool controleOrdemDisciplinas = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS,
                __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                var todasDisciplinas = (from DadosLancamentoFreqExterna item in lDadosAluno
                                        where item.tur_id > 0 && item.tud_tipo != (byte)TurmaDisciplinaTipo.Experiencia
                                        orderby item.tud_tipo, item.Disciplina
                                        group item by item.Disciplina into g
                                        select new
                                        {
                                            tud_id = g.First().tud_id
                                            ,
                                            Disciplina = g.Key
                                            ,
                                            tds_ordem = g.First().tds_ordem
                                            ,
                                            tud_tipo = g.First().tud_tipo
                                            ,
                                            regencia = g.First().tud_tipo == (byte)TurmaDisciplinaTipo.Regencia
                                                         || g.First().tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia
                                                         || (g.First().tud_tipo == (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia
                                                         && controleOrdemDisciplinas) ? 1 : 2
                                            ,
                                            enriquecimentoCurricular = g.First().EnriquecimentoCurricular
                                            ,
                                            frequencias = (from per in periodos.ToList()
                                                           orderby per.tpc_ordem
                                                           select new
                                                           {
                                                               per.tpc_id
                                                               ,
                                                               frequencia = (from DadosLancamentoFreqExterna bFrequencia in lDadosAluno
                                                                             where bFrequencia.Disciplina == g.Key
                                                                             && bFrequencia.tpc_id == per.tpc_id
                                                                             select new
                                                                             {
                                                                                 bFrequencia.numeroAulas
                                                                                 ,
                                                                                 bFrequencia.numeroFaltas
                                                                                 ,
                                                                                 numeroAulasPrevistas = bFrequencia.possuiLancamentoAulasPrevistas ?
                                                                                    bFrequencia.numeroAulasPrevistas.ToString() : "-"
                                                                                 ,
                                                                                 bFrequencia.possuiLancamentoAulasPrevistas
                                                                                 ,
                                                                                 tud_Tipo = g.First().tud_tipo
                                                                                 ,
                                                                                 bFrequencia.tud_id
                                                                                 ,
                                                                                 bFrequencia.mtu_id
                                                                                 ,
                                                                                 bFrequencia.mtd_id
                                                                                 ,
                                                                                 bFrequencia.tud_idRegencia
                                                                                 ,
                                                                                 bFrequencia.mtd_idRegencia
                                                                                 ,
                                                                                 afx_id = bFrequencia.ID
                                                                                 ,
                                                                                 enriquecimentoCurricular = bFrequencia.EnriquecimentoCurricular
                                                                             }).FirstOrDefault()
                                                           })
                                        });

                var experiencias = (from DadosLancamentoFreqExterna item in lDadosAluno
                                    where item.tur_id > 0 && item.tud_tipo == (byte)TurmaDisciplinaTipo.Experiencia
                                    orderby item.tud_tipo, item.Disciplina
                                    group item by item.Disciplina into g
                                    select new
                                    {
                                        tud_id = g.First().tud_id
                                        ,
                                        Disciplina = g.Key
                                        ,
                                        tds_ordem = g.First().tds_ordem
                                        ,
                                        tud_tipo = g.First().tud_tipo
                                        ,
                                        regencia = g.First().tud_tipo == (byte)TurmaDisciplinaTipo.Regencia
                                                     || g.First().tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia
                                                     || (g.First().tud_tipo == (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia
                                                     && controleOrdemDisciplinas) ? 1 : 2
                                        ,
                                        enriquecimentoCurricular = g.First().EnriquecimentoCurricular
                                        ,
                                        frequencias = (from per in periodos.ToList()
                                                       orderby per.tpc_ordem
                                                       select new
                                                       {
                                                           per.tpc_id
                                                           ,
                                                           frequencia = (from DadosLancamentoFreqExterna bFrequencia in lDadosAluno
                                                                         where bFrequencia.Disciplina == g.Key
                                                                         && bFrequencia.tpc_id == per.tpc_id
                                                                         select new
                                                                         {
                                                                             bFrequencia.numeroAulas
                                                                             ,
                                                                             bFrequencia.numeroFaltas
                                                                             ,
                                                                             numeroAulasPrevistas = bFrequencia.possuiLancamentoAulasPrevistas ?
                                                                                 bFrequencia.numeroAulasPrevistas.ToString() : "-"
                                                                             ,
                                                                             bFrequencia.possuiLancamentoAulasPrevistas
                                                                             ,
                                                                             tud_Tipo = g.First().tud_tipo
                                                                             ,
                                                                             bFrequencia.tud_id
                                                                             ,
                                                                             bFrequencia.mtu_id
                                                                             ,
                                                                             bFrequencia.mtd_id
                                                                             ,
                                                                             bFrequencia.tud_idRegencia
                                                                             ,
                                                                             bFrequencia.mtd_idRegencia
                                                                             ,
                                                                             afx_id = bFrequencia.ID
                                                                             ,
                                                                             enriquecimentoCurricular = bFrequencia.EnriquecimentoCurricular
                                                                         }).FirstOrDefault()
                                                       })
                                    });

                var disciplinas = (from item in todasDisciplinas
                                   where !item.enriquecimentoCurricular //Retira as que são de enriquecimento curricular
                                   select item
                              );

                // Realiza uma ordenação posterior em que so importa se a disciplina é ou nao regencia/componente da regencia para manter o agrupamento
                var dispOrdenadas = from item in disciplinas
                                    orderby item.regencia, controleOrdemDisciplinas ? item.tds_ordem.ToString() : item.Disciplina
                                    select item;

                // "Agrupa" a frequência das disciplinas componentes e complementares à regência. 
                QtComponenteRegencia = dispOrdenadas.Count(p => ((byte)TurmaDisciplinaTipo.ComponenteRegencia == p.tud_tipo || (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia == p.tud_tipo));

                // "Agrupa" a frequência das disciplinas
                QtComponentes = dispOrdenadas.Count();

                if (!ensinoInfantil)
                {
                    divDisciplinas.Visible = true;
                    rptDisciplinas.DataSource = dispOrdenadas;
                    rptDisciplinas.DataBind();
                }
                else
                {
                    divDisciplinas.Visible = false;
                }

                #endregion Disciplinas

                #region Disciplinas de enriquecimento curricular e experiências

                var disciplinasEnriquecimentoCurricular = (from item in todasDisciplinas
                                                           where item.enriquecimentoCurricular //Verifica se são de enriquecimento curricular
                                                           select item
                                  );

                if (disciplinasEnriquecimentoCurricular.Count() > 0 || experiencias.Count() > 0)
                {
                    divEnriquecimentoCurricular.Visible = true;

                    // Realiza uma ordenação posterior em que so importa se a disciplina é ou nao regencia/componente da regencia para manter o agrupamento
                    var dispOrdenadasEnriquecimento = from item in disciplinasEnriquecimentoCurricular
                                                      orderby item.regencia, controleOrdemDisciplinas ? item.tds_ordem.ToString() : item.Disciplina
                                                      select item;
                    

                    rptDisciplinasEnriquecimentoCurricular.DataSource = dispOrdenadasEnriquecimento;
                    rptDisciplinasEnriquecimentoCurricular.DataBind();

                    rptDisciplinasExperiencias.DataSource = experiencias.OrderBy(p => controleOrdemDisciplinas ? p.tds_ordem.ToString() : p.Disciplina);
                    rptDisciplinasExperiencias.DataBind();
                }
                else
                {
                    divEnriquecimentoCurricular.Visible = false;
                }

                #endregion Disciplinas de enriquecimento curricular

                #region Ensino Infantil

                if (ensinoInfantil)
                {
                    divEnsinoInfantil.Visible = true;

                    rptDisciplinasEnsinoInfantil.DataSource = dispOrdenadas;
                    rptDisciplinasEnsinoInfantil.DataBind();
                }
                else
                    divEnsinoInfantil.Visible = false;

                #endregion
            }
        }

        private void Salvar()
        {
            try
            {
                List<CLS_AlunoFrequenciaExterna> lstAlunoFrequenciaExterna =
                    (from RepeaterItem item in rptDisciplinas.Items
                     where item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item
                     let rptFrequenciaDisciplina = (Repeater)item.FindControl("rptFrequenciaDisciplina")
                     from RepeaterItem itemFreq in rptFrequenciaDisciplina.Items
                     where itemFreq.ItemType == ListItemType.AlternatingItem || itemFreq.ItemType == ListItemType.Item
                     let hdnMtuId = (HiddenField)itemFreq.FindControl("hdnMtuId")
                     let hdnMtdId = (HiddenField)itemFreq.FindControl("hdnMtdId")
                     let hdnMtdIdReg = (HiddenField)itemFreq.FindControl("hdnMtdIdReg")
                     let hdnTpc = (HiddenField)itemFreq.FindControl("hdnTpc")
                     let hdnAfx = (HiddenField)itemFreq.FindControl("hdnAfx")
                     let afx_id = string.IsNullOrEmpty(hdnAfx.Value) ? -1 : Convert.ToInt32(hdnAfx.Value)
                     let txtAulas = (TextBox)itemFreq.FindControl("txtAulas")
                     let txtFaltas = (TextBox)itemFreq.FindControl("txtFaltas")
                     let hdnTudTipo = (HiddenField)itemFreq.FindControl("hdnTudTipo")
                     let tud_tipo = Convert.ToByte(hdnTudTipo.Value)
                     where txtAulas.Visible  && txtFaltas.Visible
                     select new CLS_AlunoFrequenciaExterna
                     {
                         alu_id = VS_alu_id
                         ,
                         mtu_id = Convert.ToInt32(hdnMtuId.Value)
                         ,
                         mtd_id = tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia || tud_tipo == (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia ?
                                    Convert.ToInt32(hdnMtdIdReg.Value) : Convert.ToInt32(hdnMtdId.Value)
                         ,
                         tpc_id = Convert.ToInt32(hdnTpc.Value)
                         ,
                         afx_id = afx_id
                         ,
                         afx_qtdAulas = string.IsNullOrEmpty(txtAulas.Text) ? 0 : Convert.ToInt32(txtAulas.Text)
                         ,
                         afx_qtdFaltas = string.IsNullOrEmpty(txtFaltas.Text) ? 0 : Convert.ToInt32(txtFaltas.Text)
                         ,
                         IsNew = afx_id <= 0
                     }).ToList();

                lstAlunoFrequenciaExterna.AddRange
                    (from RepeaterItem item in rptDisciplinasEnriquecimentoCurricular.Items
                     where item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item
                     let rptFrequenciaDisciplina = (Repeater)item.FindControl("rptFrequenciaDisciplina")
                     from RepeaterItem itemFreq in rptFrequenciaDisciplina.Items
                     let hdnMtuId = (HiddenField)itemFreq.FindControl("hdnMtuId")
                     let hdnMtdId = (HiddenField)itemFreq.FindControl("hdnMtdId")
                     let hdnTpc = (HiddenField)itemFreq.FindControl("hdnTpc")
                     let hdnAfx = (HiddenField)itemFreq.FindControl("hdnAfx")
                     let afx_id = string.IsNullOrEmpty(hdnAfx.Value) ? -1 : Convert.ToInt32(hdnAfx.Value)
                     let txtAulas = (TextBox)itemFreq.FindControl("txtAulas")
                     let txtFaltas = (TextBox)itemFreq.FindControl("txtFaltas")
                     select new CLS_AlunoFrequenciaExterna
                     {
                         alu_id = VS_alu_id
                         ,
                         mtu_id = Convert.ToInt32(hdnMtuId.Value)
                         ,
                         mtd_id = Convert.ToInt32(hdnMtdId.Value)
                         ,
                         tpc_id = Convert.ToInt32(hdnTpc.Value)
                         ,
                         afx_id = afx_id
                         ,
                         afx_qtdAulas = string.IsNullOrEmpty(txtAulas.Text) ? 0 : Convert.ToInt32(txtAulas.Text)
                         ,
                         afx_qtdFaltas = string.IsNullOrEmpty(txtFaltas.Text) ? 0 : Convert.ToInt32(txtFaltas.Text)
                         ,
                         IsNew = afx_id <= 0
                     });

                lstAlunoFrequenciaExterna.AddRange
                    (from RepeaterItem item in rptDisciplinasExperiencias.Items
                     where item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item
                     let rptFrequenciaDisciplina = (Repeater)item.FindControl("rptFrequenciaDisciplina")
                     from RepeaterItem itemFreq in rptFrequenciaDisciplina.Items
                     let hdnMtuId = (HiddenField)itemFreq.FindControl("hdnMtuId")
                     let hdnMtdId = (HiddenField)itemFreq.FindControl("hdnMtdId")
                     let hdnTpc = (HiddenField)itemFreq.FindControl("hdnTpc")
                     let hdnAfx = (HiddenField)itemFreq.FindControl("hdnAfx")
                     let afx_id = string.IsNullOrEmpty(hdnAfx.Value) ? -1 : Convert.ToInt32(hdnAfx.Value)
                     let txtAulas = (TextBox)itemFreq.FindControl("txtAulas")
                     let txtFaltas = (TextBox)itemFreq.FindControl("txtFaltas")
                     select new CLS_AlunoFrequenciaExterna
                     {
                         alu_id = VS_alu_id
                         ,
                         mtu_id = Convert.ToInt32(hdnMtuId.Value)
                         ,
                         mtd_id = Convert.ToInt32(hdnMtdId.Value)
                         ,
                         tpc_id = Convert.ToInt32(hdnTpc.Value)
                         ,
                         afx_id = afx_id
                         ,
                         afx_qtdAulas = string.IsNullOrEmpty(txtAulas.Text) ? 0 : Convert.ToInt32(txtAulas.Text)
                         ,
                         afx_qtdFaltas = string.IsNullOrEmpty(txtFaltas.Text) ? 0 : Convert.ToInt32(txtFaltas.Text)
                         ,
                         IsNew = afx_id <= 0
                     });

                lstAlunoFrequenciaExterna.AddRange
                    (from RepeaterItem item in rptDisciplinasEnsinoInfantil.Items
                     where item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item
                     let rptFrequenciaDisciplina = (Repeater)item.FindControl("rptFrequenciaDisciplina")
                     from RepeaterItem itemFreq in rptFrequenciaDisciplina.Items
                     where itemFreq.ItemType == ListItemType.AlternatingItem || itemFreq.ItemType == ListItemType.Item
                     let hdnMtuId = (HiddenField)itemFreq.FindControl("hdnMtuId")
                     let hdnMtdId = (HiddenField)itemFreq.FindControl("hdnMtdId")
                     let hdnMtdIdReg = (HiddenField)itemFreq.FindControl("hdnMtdIdReg")
                     let hdnTpc = (HiddenField)itemFreq.FindControl("hdnTpc")
                     let hdnAfx = (HiddenField)itemFreq.FindControl("hdnAfx")
                     let afx_id = string.IsNullOrEmpty(hdnAfx.Value) ? -1 : Convert.ToInt32(hdnAfx.Value)
                     let txtAulas = (TextBox)itemFreq.FindControl("txtAulas")
                     let txtFaltas = (TextBox)itemFreq.FindControl("txtFaltas")
                     let hdnTudTipo = (HiddenField)itemFreq.FindControl("hdnTudTipo")
                     let tud_tipo = Convert.ToByte(hdnTudTipo.Value)
                     where txtAulas.Visible && txtFaltas.Visible
                     select new CLS_AlunoFrequenciaExterna
                     {
                         alu_id = VS_alu_id
                         ,
                         mtu_id = Convert.ToInt32(hdnMtuId.Value)
                         ,
                         mtd_id = tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia || tud_tipo == (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia ?
                                    Convert.ToInt32(hdnMtdIdReg.Value) : Convert.ToInt32(hdnMtdId.Value)
                         ,
                         tpc_id = Convert.ToInt32(hdnTpc.Value)
                         ,
                         afx_id = afx_id
                         ,
                         afx_qtdAulas = string.IsNullOrEmpty(txtAulas.Text) ? 0 : Convert.ToInt32(txtAulas.Text)
                         ,
                         afx_qtdFaltas = string.IsNullOrEmpty(txtFaltas.Text) ? 0 : Convert.ToInt32(txtFaltas.Text)
                         ,
                         IsNew = afx_id <= 0
                     });

                if (CLS_AlunoFrequenciaExternaBO.Salvar(lstAlunoFrequenciaExterna))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, string.Format("Lançamento de ausência em outras redes: alu_id: {0} | mtu_id: {1}", VS_alu_id, VS_mtu_id));
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Lançamento de ausência em outras redes realizado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    RedirecionarPagina("~/Classe/LancamentoFrequenciaExterna/Busca.aspx");
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao salvar.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Métodos

        protected void rptDisciplinas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                byte tud_tipo = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "tud_Tipo"));
                ControleMescla = (tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia || tud_tipo == (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia);
            }
        }

        protected void rptFrequenciaDisciplina_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                byte tud_tipo = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "frequencia.tud_Tipo"));

                if (tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia || tud_tipo == (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia)
                {
                    if (!ControleMescla)
                    {
                        var tdAulasPrevistas = (HtmlTableCell)e.Item.FindControl("tdAulasPrevistas");
                        var tdAulas = (HtmlTableCell)e.Item.FindControl("tdAulas");
                        var tdFaltas = (HtmlTableCell)e.Item.FindControl("tdFaltas");
                        tdAulasPrevistas.RowSpan = QtComponenteRegencia;
                        tdAulas.RowSpan = QtComponenteRegencia;
                        tdFaltas.RowSpan = QtComponenteRegencia;
                    }
                    else
                    {
                        var tdAulasPrevistas = (HtmlTableCell)e.Item.FindControl("tdAulasPrevistas");
                        var tdAulas = (HtmlTableCell)e.Item.FindControl("tdAulas");
                        var tdFaltas = (HtmlTableCell)e.Item.FindControl("tdFaltas");
                        tdAulasPrevistas.Visible = false;
                        tdAulas.Visible = false;
                        tdFaltas.Visible = false;
                    }
                }

                bool possuiLancamentoAulasPrevistas = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "frequencia.possuiLancamentoAulasPrevistas"));
                int qtdeAulasPrevistas = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "frequencia.numeroAulasPrevistas"));
                int qtdeFaltas = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "frequencia.numeroFaltas"));

                if (possuiLancamentoAulasPrevistas && qtdeFaltas > qtdeAulasPrevistas)
                {
                    var imgAvisoAulasPrevistas = (Image)e.Item.FindControl("imgAvisoAulasPrevistas");
                    imgAvisoAulasPrevistas.CssClass = "";
                }
            }
        }

        protected void rptDisciplinasEnriquecimentoCurricular_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                byte tud_tipo = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "tud_Tipo"));
                ControleMescla = (tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia || tud_tipo == (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia);
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            RedirecionarPagina("~/Classe/LancamentoFrequenciaExterna/Busca.aspx");
        }

        protected void rptDisciplinasEnsinoInfantil_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                byte tud_tipo = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "tud_Tipo"));
                ControleMescla = (tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia || tud_tipo == (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia);
            }
        }
    }
}
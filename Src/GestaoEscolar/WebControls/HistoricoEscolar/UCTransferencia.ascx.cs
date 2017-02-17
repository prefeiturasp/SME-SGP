namespace GestaoEscolar.WebControls.HistoricoEscolar
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using MSTech.CoreSSO.BLL;
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.Web.WebProject;
    using MSTech.Validation.Exceptions;

    public partial class UCTransferencia : MotherUserControl
    {
        #region Delegate

        public delegate void onClickVisualizar();
        public event onClickVisualizar clickVisualizar;

        public delegate void onClickVoltar();
        public event onClickVoltar clickVoltar;

        #endregion

        #region Variáveis

        protected bool mostraConceitoGlobal;

        protected string nomeNota;

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

        private bool mostraTotalLinha = false;

        #endregion Variáveis

        #region Propriedades

        /// <summary>
        /// ViewState que armazena o ID do aluno.
        /// </summary>
        public long VS_alu_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_alu_id"] ?? "0");
            }

            set
            {
                ViewState["VS_alu_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o ID da matrícula turma do aluno.
        /// </summary>
        public int VS_mtu_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_mtu_id"] ?? "0");
            }

            set
            {
                ViewState["VS_mtu_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o ID do formato de avaliação da turma do aluno.
        /// </summary>
        private int VS_fav_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_fav_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_fav_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazeno tipo de avaliação do formato.
        /// </summary>
        private byte VS_esa_tipo
        {
            get
            {
                return Convert.ToByte(ViewState["VS_esa_tipo"] ?? "0");
            }

            set
            {
                ViewState["VS_esa_tipo"] = value;
            }
        }

        private List<BoletimAluno> BoletimDados;

        /// <summary>
        /// Formatação para a % de frequencia
        /// </summary>
        public string VS_FormatacaoPorcentagemFrequencia
        {
            get
            {
                if (ViewState["VS_FormatacaoPorcentagemFrequencia"] != null)
                    return ViewState["VS_FormatacaoPorcentagemFrequencia"].ToString();
                return string.Empty;
            }
            set
            {
                ViewState["VS_FormatacaoPorcentagemFrequencia"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazeno ID da escala de avaliação do formato.
        /// </summary>
        public int VS_esa_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_esa_id"] ?? "-1");
            }

            set
            {
                ViewState["VS_esa_id"] = value;
            }
        }
        
        private List<ACA_EscalaAvaliacaoParecer> ltPareceres;

        /// <summary>
        /// DataTable de pareceres cadastrados na escala de avaliação.
        /// </summary>
        private List<ACA_EscalaAvaliacaoParecer> LtPareceres
        {
            get
            {
                return ltPareceres ??
                       (ltPareceres = ACA_EscalaAvaliacaoParecerBO.GetSelectBy_Escala(VS_esa_id));
            }
        }

        private ACA_FormatoAvaliacao formatoAvaliacao;

        /// <summary>
        /// Entidade do formato de avaliação.
        /// </summary>
        private ACA_FormatoAvaliacao EntFormatoAvaliacao
        {
            get
            {
                return formatoAvaliacao ??
                    (
                        formatoAvaliacao =
                            ACA_FormatoAvaliacaoBO.GetEntity(new ACA_FormatoAvaliacao { fav_id = VS_fav_id })
                    );
            }
        }

        private ACA_EscalaAvaliacaoNumerica escalaAvaliacaoNumerica;

        /// <summary>
        /// Entidade da escala de avaliação do tipo numérica.
        /// </summary>
        private ACA_EscalaAvaliacaoNumerica EntEscalaAvaliacaoNumerica
        {
            get
            {
                return escalaAvaliacaoNumerica ??
                    (
                        escalaAvaliacaoNumerica =
                            ACA_EscalaAvaliacaoNumericaBO.GetEntity(new ACA_EscalaAvaliacaoNumerica { esa_id = EntFormatoAvaliacao.esa_idPorDisciplina })
                    );
            }
        }

        public string message
        {
            set
            {
                lblMessage.Text = value;
            }
        }

        #endregion Propriedades

        #region Page Life Cycle

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);
            if (sm != null)
            {
                string arredondamento = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ARREDONDAMENTO_NOTA_AVALIACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id).ToString();

                string script = "var numeroCasasDecimais = " + RetornaNumeroCasasDecimais(Convert.ToDouble(EntEscalaAvaliacaoNumerica.ean_variacao.ToString()).ToString()) + ";" +
                    "var arredondamento = " + arredondamento.ToString().ToLower() + ";" +
                    "var variacaoEscala = '" + EntEscalaAvaliacaoNumerica.ean_variacao.ToString().Replace(',', '.') + "';";

                if (sm.IsInAsyncPostBack)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Arredondamento", script, true);
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Arredondamento", script, true);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager sm = ScriptManager.GetCurrent(this.Page);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsUCTransferencia.js"));
                }
            }
            catch (Exception err)
            {
                ApplicationWEB._GravaErro(err);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Aplica a variação em um valor decimal.
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        private string AplicarVariacao(decimal valor)
        {
            int numeroCasasDecimais = RetornaNumeroCasasDecimais(Convert.ToDouble(EntEscalaAvaliacaoNumerica.ean_variacao.ToString()).ToString());
            string formatacao = ("{0:0.").PadRight(numeroCasasDecimais + 5, '0') + "}";
            return String.Format(formatacao, valor);
        }

        /// <summary>
        /// Valida as notas inseridas.
        /// </summary>
        /// <param name="ltAvaliacao"></param>
        /// <returns></returns>
        private bool ValidarAvaliacao(List<CLS_AlunoAvaliacaoTurmaDisciplina> ltAvaliacao)
        {
            return ltAvaliacao.Where(p => !string.IsNullOrEmpty(p.atd_avaliacao))
                              .Select(p => Convert.ToDecimal(p.atd_avaliacao))
                              .ToList()
                              .TrueForAll(p => p >= EntEscalaAvaliacaoNumerica.ean_menorValor && p <= EntEscalaAvaliacaoNumerica.ean_maiorValor);
        }

        /// <summary>
        /// Retorna o número de casas decimais de acordo com a variação da escala de avaliação
        /// (só se for do tipo numérica.
        /// </summary>
        /// <returns></returns>
        private int RetornaNumeroCasasDecimais(string variacao)
        {
            int numeroCasasDecimais = 1;
            if (VS_esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica)
            {
                int notainteira;
                if (Int32.TryParse(variacao, out notainteira))
                {
                    numeroCasasDecimais = 0;
                }
                else if (variacao.IndexOf(",") >= 0)
                {
                    numeroCasasDecimais = variacao.Substring(variacao.IndexOf(","), variacao.Length - 1).Length - 1;
                }
            }

            return numeroCasasDecimais;
        }

        /// <summary>
        /// Retorna se é tipo da disciplina de componente de regência do enumerador.
        /// </summary>
        private bool RetornatipoComponenteRegencia(byte tud_tipo)
        {
            return ((byte)TurmaDisciplinaTipo.ComponenteRegencia == tud_tipo || (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia == tud_tipo);
        }

        /// <summary>
        /// Retorna vazio ou "-" em caso de nota vazia. Vazio no caso de edição.
        /// </summary>
        /// <param name="PermiteEditar"></param>
        /// <returns></returns>
        private string RetornaSemNota(bool PermiteEditar)
        {
            return PermiteEditar ? string.Empty : "-";
        }

        /// <summary>
        /// Carrega repeaters do boletim do aluno.
        /// </summary>
        /// <param name="listaNotasEFaltas">Lista com os dados do boletim.</param>
        private void CarregaBoletim()
        {
            BoletimDados = CLS_AlunoAvaliacaoTurmaDisciplinaBO.SelecionaDadosHistoricoTransferencia(VS_alu_id, VS_mtu_id, __SessionWEB.__UsuarioWEB.Docente.doc_id);

            VS_fav_id = BoletimDados.First().fav_id;
            VS_esa_tipo = (byte)BoletimDados.First().esa_tipo;
            VS_esa_id = BoletimDados.First().esa_id;

            hdnVariacaoFrequencia.Value = BoletimDados.FirstOrDefault().fav_variacao.ToString();

            mostraConceitoGlobal = BoletimDados.Count(p => p.tud_global && p.mtu_id > 0) > 0;

            // Seta nota ou conceito com base no tipo da escala de avaliacao
            nomeNota = BoletimDados.Any(p => p.esa_tipo == 1) ? "Nota" : "Conceito";

            decimal variacao = BoletimDados.FirstOrDefault().fav_variacao;

            VS_FormatacaoPorcentagemFrequencia =
                GestaoEscolarUtilBO.CriaFormatacaoDecimal(variacao > 0 ? GestaoEscolarUtilBO.RetornaNumeroCasasDecimais(variacao) : 2);

            #region Periodos / COCs / Bimestres

            var periodos = from BoletimAluno item in BoletimDados
                           where item.ava_id > 0
                           orderby item.tpc_ordem
                           group item by item.tpc_id
                               into g
                               select
                                   new
                                   {
                                       tpc_id = g.Key
                                       ,
                                       g.First().tpc_nome
                                       ,
                                       g.First().tpc_ordem
                                       ,
                                       g.First().fav_id
                                       ,
                                       g.First().ava_id
                                       ,
                                       g.First().ava_idRec
                                       ,
                                       g.First().ava_nomeRec
                                       ,
                                       MatriculaPeriodo =
                               g.First().mtu_id > 0
                                   ? "Responsável pelo lançamento no " + g.First().tpc_nome + ": Turma " +
                                     g.First().tur_codigo + " (" +
                                     (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                                          ? g.First().esc_codigo + " - " + g.First().esc_nome
                                          : g.First().esc_nome)
                                     + ")"
                                   : "Aluno não matriculado no " + g.First().tpc_nome
                                   };

            rptPeriodosNomes.DataSource = periodos;
            rptPeriodosColunasFixas.DataSource = periodos;
            rptPeriodosNomesEnriquecimento.DataSource = periodos;
            rptPeriodosColunasFixasEnriquecimento.DataSource = periodos;
            rptPeriodosNomesProjeto.DataSource = periodos;
            rptPeriodosColunasFixasProjeto.DataSource = periodos;
            rptPeriodosNomes.DataBind();
            rptPeriodosColunasFixas.DataBind();
            rptPeriodosNomesEnriquecimento.DataBind();
            rptPeriodosColunasFixasEnriquecimento.DataBind();
            rptPeriodosNomesProjeto.DataBind();
            rptPeriodosColunasFixasProjeto.DataBind();

            #endregion Periodos / COCs / Bimestres

            #region Disciplinas

            bool controleOrdemDisciplinas = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            var todasDisciplinas = (from BoletimAluno item in BoletimDados
                                    where item.tur_id > 0
                                    orderby item.tud_tipo, item.tud_global descending, item.Disciplina
                                    group item by item.Disciplina
                                        into g
                                        select
                                            new
                                            {
                                                Disciplina = g.First().nomeDisciplina
                                                ,
                                                tds_ordem = g.First().tds_ordem
                                                ,
                                                tud_Tipo = g.First().tud_tipo
                                                ,
                                                g.First().tud_global
                                                ,
                                                mostrarDisciplina = g.Count(p => p.MostrarLinhaDisciplina)
                                                ,
                                                regencia = g.First().tud_tipo == (byte)TurmaDisciplinaTipo.Regencia
                                                             || g.First().tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia
                                                             || (g.First().tud_tipo == (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia
                                                             && controleOrdemDisciplinas) ? 1 : 2
                                                ,
                                                enriquecimentoCurricular = g.First().EnriquecimentoCurricular
                                                ,
                                                recuperacao = g.First().Recuperacao
                                                ,
                                                projetoComplementar = g.First().ProjetoComplementar
                                                ,
                                                alu_id = g.First().alu_id
                                                ,
                                                mtu_id = g.First().mtu_id
                                                ,
                                                mtd_id = g.First().mtd_id
                                                ,
                                                tud_id = g.First().tud_id
                                                ,
                                                notas = (
                                                            from per in periodos.ToList()
                                                            orderby per.tpc_ordem
                                                            select new
                                                            {
                                                                per.tpc_id
                                                                ,
                                                                per.fav_id
                                                                ,
                                                                per.ava_id
                                                                ,
                                                                nota = (
                                                                           from BoletimAluno bNota in BoletimDados
                                                                           where
                                                                               bNota.Disciplina == g.Key
                                                                               && bNota.tpc_id == per.tpc_id
                                                                           select new
                                                                           {
                                                                               Nota = (
                                                                                   //
                                                                                                 bNota.dda_id > 0 ? "-"
                                                                                                 :
                                                                                   //
                                                                                                 !bNota.mostraNota || bNota.naoExibirNota
                                                                                                     ? RetornaSemNota(bNota.PermiteEditar)
                                                                                                     : (bNota.NotaNumerica
                                                                                                            ? bNota.avaliacao ??
                                                                                                              RetornaSemNota(bNota.PermiteEditar)
                                                                                                            : (bNota.
                                                                                                                   NotaAdicionalNumerica
                                                                                                                   ? bNota.
                                                                                                                         avaliacaoAdicional ??
                                                                                                                     RetornaSemNota(bNota.PermiteEditar)
                                                                                                                   : bNota.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres
                                                                                                                         ? bNota.avaliacao ?? RetornaSemNota(bNota.PermiteEditar)
                                                                                                                         : RetornaSemNota(bNota.PermiteEditar))
                                                                                                       )
                                                                                             ).Replace(".", ",")
                                                                                     ,
                                                                               Conceito =
                                                                                       (
                                                                                   //
                                                                                       bNota.dda_id > 0 ? "-"
                                                                                       :
                                                                                   //
                                                                                       bNota.mostraConceito
                                                                                            ? (bNota.NotaNumerica
                                                                                                   ? "-"
                                                                                                   : bNota.avaliacao)
                                                                                            : "-")
                                                                               ,
                                                                               bNota.tpc_id
                                                                               ,
                                                                               bNota.NotaRP
                                                                               ,
                                                                               Frequencia = bNota.mostraFrequencia && !string.IsNullOrEmpty(bNota.frequencia) ? 
                                                                                                bNota.frequencia : "-"
                                                                               ,
                                                                               tud_Tipo = g.First().tud_tipo
                                                                               ,
                                                                               PermiteEditar = bNota.PermiteEditar
                                                                               ,
                                                                               esa_tipo = bNota.esa_tipo
                                                                               ,
                                                                               bNota.NotaID
                                                                               ,
                                                                               eap_ordem = bNota.eap_ordem
                                                                               ,
                                                                               bNota.ProjetoId
                                                                               ,
                                                                               bNota.NotaProjetoId
                                                                               ,
                                                                               bNota.ResultadoProjeto
                                                                               ,
                                                                               numeroFaltas = bNota.numeroFaltas
                                                                               ,
                                                                               numeroAulas = bNota.numeroAulas
                                                                               ,
                                                                               ausenciasCompensadas = bNota.ausenciasCompensadas
                                                                               ,
                                                                               FrequenciaFinalAjustada = bNota.strFrequenciaFinalAjustada
                                                                               ,
                                                                               TudIdRegencia = bNota.TudIdRegencia
                                                                               ,
                                                                               MtdIdRegencia = bNota.MtdIdRegencia
                                                                               ,
                                                                               AtdIdRegencia = bNota.AtdIdRegencia
                                                                               ,
                                                                               PermiteEdicaoDocente = bNota.PermiteEdicaoDocente
                                                                           }).FirstOrDefault()
                                                            })
                                            }).ToList();

            var disciplinas = (from item in todasDisciplinas
                               where !item.enriquecimentoCurricular //Retira as que são de enriquecimento curricular
                               && !item.recuperacao //Retira as recuperacoes 
                               && !item.projetoComplementar // Retira os projetos complementares
                               select item
                               );

            // Realiza uma ordenação posterior em que so importa se a disciplina é ou nao regencia/componente da regencia para manter o agrupamento
            var dispOrdenadas = from item in disciplinas
                                orderby item.regencia, controleOrdemDisciplinas ? item.tds_ordem.ToString() : item.Disciplina
                                select item;

            // "Agrupa" a frequência das disciplinas componentes e complementares à regência.
            QtComponenteRegencia = dispOrdenadas.Count(p => (RetornatipoComponenteRegencia(p.tud_Tipo)) && p.mostrarDisciplina > 0);

            // "Agrupa" a frequência das disciplinas
            QtComponentes = dispOrdenadas.Count(p => (p.mostrarDisciplina > 0));

            rptDisciplinas.DataSource = dispOrdenadas.Where(p => p.mostrarDisciplina > 0);
            rptDisciplinas.DataBind();

            #endregion Disciplinas

            #region Disciplinas de enriquecimento curricular

            var disciplinasEnriquecimentoCurricular = (from item in todasDisciplinas
                                                       where item.enriquecimentoCurricular //Verifica se são de enriquecimento curricular
                                                       && !item.recuperacao //Retira as recuperacoes
                                                       && !item.projetoComplementar // Retira os projetos complementares
                                                       select item
                              );

            if (disciplinasEnriquecimentoCurricular.Count() > 0)
            {
                // Realiza uma ordenação posterior em que so importa se a disciplina é ou nao regencia/componente da regencia para manter o agrupamento
                var dispOrdenadasEnriquecimento = from item in disciplinasEnriquecimentoCurricular
                                                  orderby item.regencia, controleOrdemDisciplinas ? item.tds_ordem.ToString() : item.Disciplina
                                                  select item;

                rptDisciplinasEnriquecimentoCurricular.DataSource = dispOrdenadasEnriquecimento.Where(p => p.mostrarDisciplina > 0);
                rptDisciplinasEnriquecimentoCurricular.DataBind();
            }
            else
            {
                divEnriquecimentoCurricular.Visible = false;
            }

            #endregion Disciplinas de enriquecimento curricular

            #region Projeto complementar

            if (__SessionWEB.__UsuarioWEB.Docente.doc_id == 0)
            {

                var disciplinasProjeto = (from item in todasDisciplinas
                                              where item.projetoComplementar
                                              orderby item.Disciplina
                                              select item
                                  );

                if (disciplinasProjeto.Count() > 0)
                {
                    rptDisciplinasProjeto.DataSource = disciplinasProjeto.Where(p => p.mostrarDisciplina > 0);
                    rptDisciplinasProjeto.DataBind();
                }
                else
                {
                    divProjetos.Visible = false;
                }
            }
            else
            {
                divProjetos.Visible = false;
            }

            #endregion Projeto complementar
        }

        /// <summary>
        /// Carrega o boletim de acordo com o aluno e matricula na turma.
        /// </summary>
        /// <param name="dadosBoletimAluno">Dados do boletim do aluno.</param>
        public bool CarregarAluno()
        {
            try
            {
                if (MTR_MovimentacaoBO.VerificaAlunoPossuiMovimentacaoSaidaEscola(VS_alu_id, VS_mtu_id))
                {
                    CarregaBoletim();

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "Boletim",
                           "$('.tblBoletim tbody tr:even').addClass('linhaImpar');"
                           + "$('.tblBoletim tbody tr:odd').addClass('linhaPar');"
                           + "RemoveNosTextoVazioTabelasIE9();"
                           , true);

                    return true;
                }
                else
                {
                    lblMessagePanel.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("UserControl", "UCTransferencia.lblMessagePanel.Text").ToString(), UtilBO.TipoMensagem.Nenhuma);
                    divConteudoNotasBimestrais.Visible = btnSalvar.Visible = false;
                }

                return false;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                return true;
            }
        }

        /// <summary>
        /// Carrega os pareceres com os dados do DataTable, inserindo um atributo a mais em cada linha (ordem).
        /// </summary>
        /// <param name="ddlPareceres">Combo a ser carregado</param>
        private void CarregarPareceres(DropDownList ddl, byte eap_ordem, string avaliacao)
        {
            ListItem li = new ListItem("-- Selecione um conceito --", "-1;-1", true);
            ddl.Items.Add(li);

            foreach (ACA_EscalaAvaliacaoParecer eap in LtPareceres)
            {
                li = new ListItem(eap.descricao, eap.eap_valor + ";" + eap.eap_ordem.ToString());
                ddl.Items.Add(li);
            }

            string valor = String.Format("{0};{1}", avaliacao, eap_ordem);

            ddl.SelectedValue = ddl.Items.Cast<ListItem>().Any(p => p.Value == valor) ? valor : "-1;-1";
        }

        private List<CLS_AlunoProjeto> CriarListaProjeto()
        {
            NumberFormatInfo formatPorcentagemFrequencia = new NumberFormatInfo() { NumberDecimalSeparator = "." };
            return
                (
                    from RepeaterItem itemDisciplina in rptDisciplinasProjeto.Items
                    let alu_id = Convert.ToInt64(((HiddenField)itemDisciplina.FindControl("hdnAluId")).Value)
                    let rptNotasDisciplina = (Repeater)itemDisciplina.FindControl("rptNotasDisciplina")
                    from RepeaterItem itemNota in rptNotasDisciplina.Items
                    let tpc_id = Convert.ToInt32(((HiddenField)itemNota.FindControl("hdnTpcId")).Value)
                    let ProjetoId = Convert.ToInt32(((HiddenField)itemNota.FindControl("hdnProjetoId")).Value)
                    let hdnNotaProjetoId = (HiddenField)itemNota.FindControl("hdnNotaProjetoId")
                    let NotaProjetoId = Convert.ToInt32(string.IsNullOrEmpty(hdnNotaProjetoId.Value) ? "-1" : hdnNotaProjetoId.Value)
                    let txtFrequencia = (TextBox)itemNota.FindControl("txtFrequencia")
                    select new CLS_AlunoProjeto
                    {
                        alu_id = alu_id
                        ,
                        ahp_id = ProjetoId
                        ,
                        apj_id = NotaProjetoId > 0 ? NotaProjetoId : -1
                        ,
                        tpc_id = tpc_id
                        ,
                        apj_frequencia = decimal.Parse(string.IsNullOrEmpty(txtFrequencia.Text) ? "-1" : txtFrequencia.Text, formatPorcentagemFrequencia)
                        ,
						apj_resultado = string.IsNullOrEmpty(txtFrequencia.Text) ? (byte)0 : (decimal.Parse(string.IsNullOrEmpty(txtFrequencia.Text) ? "-1" : txtFrequencia.Text, formatPorcentagemFrequencia) > 0 ?
									    (byte)TipoResultado.Aprovado : (byte)TipoResultado.ReprovadoFrequencia)
						,
                        IsNew = NotaProjetoId <= 0
                    }
                ).ToList();
        }

        /// <summary>
        /// O método salva as notas que serão exibidas no histórico.
        /// </summary>
        private void Salvar()
        {
            try
            {
                List<CLS_AlunoAvaliacaoTurmaDisciplina> ltAlunoAvaliacaoTurmaDisciplina = CriarListaAvaliacao();

                List<CLS_AlunoProjeto> ltAlunoProjeto = CriarListaProjeto();

                if (ltAlunoProjeto.Any(p => p.apj_frequencia > 100))
                    throw new ValidationException("Frequência em projetos/atividades complementares deve ser menor que 100.");


                if (VS_esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica && !ValidarAvaliacao(ltAlunoAvaliacaoTurmaDisciplina))
                    throw new ValidationException(String.Format("Notas devem estar no intervalo entre {0} e {1}", AplicarVariacao(EntEscalaAvaliacaoNumerica.ean_menorValor),
                                                                                                                  AplicarVariacao(EntEscalaAvaliacaoNumerica.ean_maiorValor)));

                if (CLS_AlunoAvaliacaoTurmaDisciplinaBO.SalvarListaAvaliacaoTransferencia(ltAlunoAvaliacaoTurmaDisciplina, ltAlunoProjeto))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, String.Format("Histórico | Transferência | alu_id: {0}; mtu_id: {1}", VS_alu_id, VS_mtu_id));
                    CarregaBoletim();
                    lblMessage.Text = UtilBO.GetErroMessage(VS_esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica ? "Nota(s) salva(s) com sucesso." : "Conceito(s) salvo(s) com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage(VS_esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica ? "Erro ao tentar salvar a(s) nota(s)." : "Erro ao tentar salvar o(s) conceito(s).", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// O método returna uma lista de avaliações para o histórico.
        /// </summary>
        /// <returns></returns>
        private List<CLS_AlunoAvaliacaoTurmaDisciplina> CriarListaAvaliacao()
        {
            NumberFormatInfo formatPorcentagemFrequencia = new NumberFormatInfo() { NumberDecimalSeparator = "." };
            List<CLS_AlunoAvaliacaoTurmaDisciplina> retorno = new List<CLS_AlunoAvaliacaoTurmaDisciplina>();
            bool adicionouRegencia = false;

            foreach (RepeaterItem itemDisciplina in rptDisciplinas.Items)
            {
                long alu_id = Convert.ToInt64(((HiddenField)itemDisciplina.FindControl("hdnAluId")).Value);
                int mtu_id = Convert.ToInt32(((HiddenField)itemDisciplina.FindControl("hdnMtuId")).Value);
                int mtd_id = Convert.ToInt32(((HiddenField)itemDisciplina.FindControl("hdnMtdId")).Value);
                long tud_id = Convert.ToInt64(((HiddenField)itemDisciplina.FindControl("hdnTudId")).Value);
                Repeater rptNotasDisciplina = (Repeater)itemDisciplina.FindControl("rptNotasDisciplina");
                foreach (RepeaterItem itemNota in rptNotasDisciplina.Items)
                {
                    HiddenField hdnEsaTipo = (HiddenField)itemNota.FindControl("hdnEsaTipo");
                    HiddenField hdnPermiteEditar = (HiddenField)itemNota.FindControl("hdnPermiteEditar");
                    byte esa_tipo = Convert.ToByte(string.IsNullOrEmpty(hdnEsaTipo.Value) ? "0" : hdnEsaTipo.Value);
                    bool PermiteEditar = Convert.ToBoolean(string.IsNullOrEmpty(hdnPermiteEditar.Value) ? "false" : hdnPermiteEditar.Value);
                    TextBox txtNota = (TextBox)itemNota.FindControl("txtNota");
                    DropDownList ddlNota = (DropDownList)itemNota.FindControl("ddlNota");
                    if (esa_tipo > 0 && PermiteEditar && (txtNota.Enabled || ddlNota.Enabled))
                    {
                        int fav_id = Convert.ToInt32(((HiddenField)itemNota.FindControl("hdnFavId")).Value);
                        int ava_id = Convert.ToInt32(((HiddenField)itemNota.FindControl("hdnAvaId")).Value);
                        int NotaId = Convert.ToInt32(((HiddenField)itemNota.FindControl("hdnNotaId")).Value);
                        Literal litFrequencia = (Literal)itemNota.FindControl("litFrequencia");
                        HiddenField hdnNumeroFaltas = (HiddenField)itemNota.FindControl("hdnNumeroFaltas");
                        HiddenField hdnNumeroAulas = (HiddenField)itemNota.FindControl("hdnNumeroAulas");
                        HiddenField hdnAusenciasCompensadas = (HiddenField)itemNota.FindControl("hdnAusenciasCompensadas");
                        HiddenField hdnFrequenciaFinalAjustada = (HiddenField)itemNota.FindControl("hdnFrequenciaFinalAjustada");
                        HiddenField hdnTudIdRegencia = (HiddenField)itemNota.FindControl("hdnTudIdRegencia");
                        bool componenteRegencia = Convert.ToInt64(hdnTudIdRegencia.Value) > 0;
                        retorno.Add(new CLS_AlunoAvaliacaoTurmaDisciplina
                        {
                            alu_id = alu_id
                            ,
                            mtu_id = mtu_id
                            ,
                            mtd_id = mtd_id
                            ,
                            tud_id = tud_id
                            ,
                            fav_id = fav_id
                            ,
                            ava_id = ava_id
                            ,
                            atd_id = NotaId > 0 ? NotaId : -1
                            ,
                            atd_avaliacao = RetornaAvaliacao(esa_tipo, tud_id, txtNota, ddlNota)
                            ,
                            atd_frequencia = decimal.Parse(componenteRegencia || RetornaTextoVazio(litFrequencia.Text) ? "-1" : litFrequencia.Text, formatPorcentagemFrequencia)
                            ,
                            atd_numeroFaltas = componenteRegencia || RetornaTextoVazio(hdnNumeroFaltas.Value) ? -1 : Convert.ToInt32(hdnNumeroFaltas.Value)
                            ,
                            atd_numeroAulas = componenteRegencia || RetornaTextoVazio(hdnNumeroAulas.Value) ? -1 : Convert.ToInt32(hdnNumeroAulas.Value)
                            ,
                            atd_ausenciasCompensadas = componenteRegencia || RetornaTextoVazio(hdnAusenciasCompensadas.Value) ? -1 : Convert.ToInt32(hdnAusenciasCompensadas.Value)
                            ,
                            atd_frequenciaFinalAjustada = decimal.Parse(componenteRegencia || RetornaTextoVazio(hdnFrequenciaFinalAjustada.Value) ? "0" : hdnFrequenciaFinalAjustada.Value, formatPorcentagemFrequencia)
                            ,
                            IsNew = true
                        });

                        // Adiciona as informações da frequência como efetivação da disciplina de regência, caso seja componente da regência
                        // e ainda não exista o registro da efetivação.
                        NotaId = Convert.ToInt32(((HiddenField)itemNota.FindControl("hdnAtdIdRegencia")).Value);
                        if (componenteRegencia && !adicionouRegencia && NotaId <= 0)
                        {
                            retorno.Add(new CLS_AlunoAvaliacaoTurmaDisciplina
                            {
                                alu_id = alu_id
                                ,
                                mtu_id = mtu_id
                                ,
                                mtd_id = Convert.ToInt32(((HiddenField)itemNota.FindControl("hdnMtdIdRegencia")).Value)
                                ,
                                tud_id = Convert.ToInt64(hdnTudIdRegencia.Value)
                                ,
                                fav_id = fav_id
                                ,
                                ava_id = ava_id
                                ,
                                atd_id = NotaId > 0 ? NotaId : -1
                                ,
                                atd_avaliacao = string.Empty
                                ,
                                atd_frequencia = decimal.Parse(RetornaTextoVazio(litFrequencia.Text) ? "-1" : litFrequencia.Text, formatPorcentagemFrequencia)
                                ,
                                atd_numeroFaltas = RetornaTextoVazio(hdnNumeroFaltas.Value) ? -1 : Convert.ToInt32(hdnNumeroFaltas.Value)
                                ,
                                atd_numeroAulas = RetornaTextoVazio(hdnNumeroAulas.Value) ? -1 : Convert.ToInt32(hdnNumeroAulas.Value)
                                ,
                                atd_ausenciasCompensadas = RetornaTextoVazio(hdnAusenciasCompensadas.Value) ? -1 : Convert.ToInt32(hdnAusenciasCompensadas.Value)
                                ,
                                atd_frequenciaFinalAjustada = decimal.Parse(RetornaTextoVazio(hdnFrequenciaFinalAjustada.Value) ? "-1" : hdnFrequenciaFinalAjustada.Value, formatPorcentagemFrequencia)
                                ,
                                IsNew = true
                            });
                            adicionouRegencia = true;
                        }
                    }
                }
            }
            foreach (RepeaterItem itemDisciplina in rptDisciplinasEnriquecimentoCurricular.Items)
            {
                long alu_id = Convert.ToInt64(((HiddenField)itemDisciplina.FindControl("hdnAluId")).Value);
                int mtu_id = Convert.ToInt32(((HiddenField)itemDisciplina.FindControl("hdnMtuId")).Value);
                int mtd_id = Convert.ToInt32(((HiddenField)itemDisciplina.FindControl("hdnMtdId")).Value);
                long tud_id = Convert.ToInt64(((HiddenField)itemDisciplina.FindControl("hdnTudId")).Value);
                Repeater rptNotasDisciplina = (Repeater)itemDisciplina.FindControl("rptNotasDisciplina");
                foreach (RepeaterItem itemNota in rptNotasDisciplina.Items)
                {
                    HiddenField hdnPermiteEditar = (HiddenField)itemNota.FindControl("hdnPermiteEditar");
                    bool PermiteEditar = Convert.ToBoolean(string.IsNullOrEmpty(hdnPermiteEditar.Value) ? "false" : hdnPermiteEditar.Value);
                    int NotaId = Convert.ToInt32(((HiddenField)itemNota.FindControl("hdnNotaId")).Value);
                    if (PermiteEditar && NotaId <= 0)
                    {
                        int fav_id = Convert.ToInt32(((HiddenField)itemNota.FindControl("hdnFavId")).Value);
                        int ava_id = Convert.ToInt32(((HiddenField)itemNota.FindControl("hdnAvaId")).Value);
                        Literal litFrequencia = (Literal)itemNota.FindControl("litFrequencia");
                        HiddenField hdnNumeroFaltas = (HiddenField)itemNota.FindControl("hdnNumeroFaltas");
                        HiddenField hdnNumeroAulas = (HiddenField)itemNota.FindControl("hdnNumeroAulas");
                        HiddenField hdnAusenciasCompensadas = (HiddenField)itemNota.FindControl("hdnAusenciasCompensadas");
                        HiddenField hdnFrequenciaFinalAjustada = (HiddenField)itemNota.FindControl("hdnFrequenciaFinalAjustada");
                        retorno.Add(new CLS_AlunoAvaliacaoTurmaDisciplina
                        {
                            alu_id = alu_id
                            ,
                            mtu_id = mtu_id
                            ,
                            mtd_id = mtd_id
                            ,
                            tud_id = tud_id
                            ,
                            fav_id = fav_id
                            ,
                            ava_id = ava_id
                            ,
                            atd_id = NotaId > 0 ? NotaId : -1
                            ,
                            atd_avaliacao = string.Empty
                            ,
                            atd_frequencia = decimal.Parse(RetornaTextoVazio(litFrequencia.Text) ? "-1" : litFrequencia.Text, formatPorcentagemFrequencia)
                            ,
                            atd_numeroFaltas = RetornaTextoVazio(hdnNumeroFaltas.Value) ? -1 : Convert.ToInt32(hdnNumeroFaltas.Value)
                            ,
                            atd_numeroAulas = RetornaTextoVazio(hdnNumeroAulas.Value) ? -1 : Convert.ToInt32(hdnNumeroAulas.Value)
                            ,
                            atd_ausenciasCompensadas = RetornaTextoVazio(hdnAusenciasCompensadas.Value) ? -1 : Convert.ToInt32(hdnAusenciasCompensadas.Value)
                            ,
                            atd_frequenciaFinalAjustada = decimal.Parse(RetornaTextoVazio(hdnFrequenciaFinalAjustada.Value) ? "0" : hdnFrequenciaFinalAjustada.Value, formatPorcentagemFrequencia)
                            ,
                            IsNew = true
                        });
                    }
                }
            }
            return retorno;
        }

        /// <summary>
        /// Retorna se o texto passado por parametro representa uma informação vazia.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        private bool RetornaTextoVazio(string texto)
        {
            return string.IsNullOrEmpty(texto) || texto == "-";
        }

        /// <summary>
        /// Retorna a nota / parecer informado na linha do grid.
        /// </summary>
        private string RetornaAvaliacao(byte esa_tipo, long tud_id, TextBox txtNota, DropDownList ddlPareceres)
        {

            EscalaAvaliacaoTipo tipo = (EscalaAvaliacaoTipo)esa_tipo;

            if (txtNota != null && tipo == EscalaAvaliacaoTipo.Numerica)
            {
                if (txtNota.Visible)
                {
                    return txtNota.Text;
                }
            }

            if (ddlPareceres != null && tipo == EscalaAvaliacaoTipo.Pareceres)
            {
                if (ddlPareceres.Visible)
                {
                    if (ddlPareceres.SelectedValue.Split(';')[0] == "-1")
                    {
                        return string.Empty;
                    }

                    return ddlPareceres.SelectedValue.Split(';')[0];
                }
            }

            return string.Empty;
        }

        #endregion

        #region Eventos

        protected void rptDisciplinas_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //Mescla as linhas de falta, total de ausencias e total comp. para os componentes da regencia
                byte tud_tipo = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "tud_Tipo"));

                if (RetornatipoComponenteRegencia(tud_tipo))
                {
                    ControleMescla = true;
                }
            }
        }

        protected void rptNotasDisciplina_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (!mostraTotalLinha)
                    mostraTotalLinha = (DataBinder.Eval(e.Item.DataItem, "nota") != null
                                        && (DataBinder.Eval(e.Item.DataItem, "nota.Nota").ToString() != "-"
                                            || DataBinder.Eval(e.Item.DataItem, "nota.Frequencia").ToString() != "-"));

                Literal litNota = (Literal)e.Item.FindControl("litNota");
                TextBox txtNota = (TextBox)e.Item.FindControl("txtNota");
                DropDownList ddlNota = (DropDownList)e.Item.FindControl("ddlNota");

                if (litNota != null && txtNota != null && ddlNota != null)
                {
                    if (DataBinder.Eval(e.Item.DataItem, "nota") != null)
                    {
                        bool PermiteEditar = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "nota.PermiteEditar").ToString());
                        byte esa_tipo = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "nota.esa_tipo").ToString());
                        txtNota.Visible = PermiteEditar && esa_tipo == (byte)EscalaAvaliacaoTipo.Numerica;
                        ddlNota.Visible = PermiteEditar && (esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres);
                        litNota.Visible = !txtNota.Visible && !ddlNota.Visible;

                        if (esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres && PermiteEditar)
                        {
                            byte eap_ordem = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "nota.eap_ordem").ToString());
                            string nota = DataBinder.Eval(e.Item.DataItem, "nota.Nota").ToString();
                            CarregarPareceres(ddlNota, eap_ordem, nota);
                        }

                        if (PermiteEditar)
                        {
                            txtNota.Enabled = ddlNota.Enabled = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "nota.PermiteEdicaoDocente").ToString());
                        }
                    }
                    else
                    {
                        txtNota.Visible = ddlNota.Visible = false;
                        litNota.Visible = true;
                        litNota.Text = "-";
                    }
                }

                HtmlTableCell tdFrequencia = (HtmlTableCell)e.Item.FindControl("tdFrequencia");

                if (tdFrequencia != null)
                {
                    if (DataBinder.Eval(e.Item.DataItem, "nota") == null)
                    {
                        tdFrequencia.InnerText = "-";
                    }

                    byte tud_tipo = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "nota.tud_Tipo"));
                    if (RetornatipoComponenteRegencia(tud_tipo))
                    {
                        if (!ControleMescla)
                        {

                            tdFrequencia.RowSpan = QtComponenteRegencia;
                        }
                        else
                        {
                            tdFrequencia.Visible = false;
                        }
                    }
                }
            }
        }

        protected void rptDisciplinasEnriquecimentoCurricular_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //Mescla as linhas de falta, total de ausencias e total comp. para os componentes da regencia
                byte tud_tipo = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "tud_Tipo"));
                if (RetornatipoComponenteRegencia(tud_tipo))
                {
                    ControleMescla = true;
                }
            }
        }

        protected void rptNotasDisciplinaProjeto_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TextBox txtFrequencia = (TextBox)e.Item.FindControl("txtFrequencia");

                if (txtFrequencia != null)
                {
                    if (DataBinder.Eval(e.Item.DataItem, "nota") != null)
                    {
                        int NotaProjetoId = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "nota.NotaProjetoId").ToString());
                        string sFrequencia = DataBinder.Eval(e.Item.DataItem, "nota.Frequencia").ToString();
                        txtFrequencia.Text = NotaProjetoId > 0 ? sFrequencia : "";
                    }
                }
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (clickVoltar != null)
                clickVoltar();
        }

        protected void btnVisualizar_Click(object sender, EventArgs e)
        {
            if (clickVisualizar != null)
                clickVisualizar();
        }

        #endregion Eventos
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;

namespace AreaAluno.WebControls.AlunoBoletimEscolar
{
    public partial class UCAlunoBoletimEscolar : MotherUserControl
    {
        #region Propriedades

        private List<BoletimAluno> BoletimDados;

        private List<BoletimAlunoDocCompartilhada> DocenciasCompartilhadas = new List<BoletimAlunoDocCompartilhada>();

        private bool mostraTotalLinha;

        /// <summary>
        /// Serve para consulta se ja mesclou as linhas do boletim
        /// </summary>
        private bool VS_Controle_Mescla;

        /// <summary>
        /// Serve para consulta se ja mesclou as linhas do boletim
        /// </summary>
        private bool VS_Controle_Mescla_Disp;
      
        /// <summary>
        /// Serve para consulta se ja mesclou as linhas do boletim para a regencia
        /// </summary>
        private bool VS_Controle_Mescla_Regencia;

        /// <summary>
        /// Guarda a quantidade de itens do tipo filho
        /// </summary>
        private int VS_Qt_Comp_Reg;

        /// <summary>
        /// Guarda a quantidade de itens do tipo filho
        /// </summary>
        private int VS_Qt_Comp;

        /// <summary>
        /// Retorna o parametro academico para exibição da compensação de ausência
        /// </summary>
        private bool ExibeCompensacaoAusencia
        {
            get
            {
                return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_COMPENSACAO_AUSENCIA_CADASTRADA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }
       
        public int VS_tpc_id
        {
            get
            {
                if (ViewState["VS_tpc_id"] != null)
                    return Convert.ToInt32(ViewState["VS_tpc_id"]);
                return 0;
            }
            set
            {
                ViewState["VS_tpc_id"] = value;
            }
        }

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

        #endregion Propriedades

        #region Variáveis

        protected bool mostraConceitoGlobal;

        protected string nomeNota;

        /// <summary>
        /// Asterisco da docencia compartilhada
        /// </summary>
        private string asteriscoCompartilhada = "*";

        #endregion Variáveis

        #region Métodos

        /// <summary>
        /// Retorna se é tipo da disciplina de componente de regência do enumerador.
        /// </summary>
        private bool tipoComponenteRegencia(byte tud_tipo)
        {
            return ((byte)TurmaDisciplinaTipo.ComponenteRegencia == tud_tipo || (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia == tud_tipo);
        }

        /// <summary>
        /// Carrega repeaters do boletim do aluno.
        /// </summary>
        /// <param name="listaNotasEFaltas">Lista com os dados do boletim.</param>
        private void CarregaBoletim(ACA_AlunoBO.BoletimDadosAluno dadosBoletimAluno)
        {
            BoletimDados = dadosBoletimAluno.listaNotasEFaltas;

            int cur_id = dadosBoletimAluno.cur_id;
            int crr_id = dadosBoletimAluno.crr_id;
            int crp_id = dadosBoletimAluno.crp_id;

            List<ACA_CurriculoPeriodo> lstCurriculoPeriodo = ACA_CurriculoPeriodoBO.Seleciona_PeriodosRelacionados_Equivalentes(cur_id, crr_id, crp_id);

            mostraConceitoGlobal = BoletimDados.Count(p => p.tud_global && p.mtu_id > 0) > 0;

            // Seta nota ou conceito com base no tipo da escala de avaliacao
            nomeNota = BoletimDados.Any(p => p.esa_tipo == 1) ? "Nota" : "Conceito";

            lblNotasFaltas.Text = nomeNota + "s e Faltas";

            decimal variacao = BoletimDados.FirstOrDefault().fav_variacao;
            VS_FormatacaoPorcentagemFrequencia =
                GestaoEscolarUtilBO.CriaFormatacaoDecimal(variacao > 0 ? GestaoEscolarUtilBO.RetornaNumeroCasasDecimais(variacao) : 2);

            divBoletim.Visible = true;

            if (ExibeCompensacaoAusencia)
            {
                var thTotalComp = new HtmlTableCell();
                thTotalComp = (HtmlTableCell)divBoletim.FindControl("thTotalComp");
                thTotalComp.Visible = true;

                var thFreqFinal = new HtmlTableCell();
                thFreqFinal = (HtmlTableCell)divBoletim.FindControl("thFreqFinal");
                thFreqFinal.Visible = true;

                var thTotalCompEnriquecimento = new HtmlTableCell();
                thTotalCompEnriquecimento = (HtmlTableCell)divBoletim.FindControl("thTotalCompEnriquecimento");
                thTotalCompEnriquecimento.Visible = true;

                var thFreqFinalEnriquecimento = new HtmlTableCell();
                thFreqFinalEnriquecimento = (HtmlTableCell)divBoletim.FindControl("thFreqFinalEnriquecimento");
                thFreqFinalEnriquecimento.Visible = true;

                var thFreqFinalRecuperacao = new HtmlTableCell();
                thFreqFinalRecuperacao = (HtmlTableCell)divBoletim.FindControl("thFreqFinalRecuperacao");
                thFreqFinalRecuperacao.Visible = true;
            }

            #region Periodos / COCs / Bimestres

            var periodos = from BoletimAluno item in BoletimDados
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
            rptPeriodosNomesRecuperacao.DataSource = periodos;
            rptPeriodosColunasFixasRecuperacao.DataSource = periodos;
            rptPeriodosNomes.DataBind();
            rptPeriodosColunasFixas.DataBind();
            rptPeriodosNomesEnriquecimento.DataBind();
            rptPeriodosColunasFixasEnriquecimento.DataBind();
            rptPeriodosNomesRecuperacao.DataBind();
            rptPeriodosColunasFixasRecuperacao.DataBind();

            #endregion Periodos / COCs / Bimestres

            #region Disciplinas
            bool controleOrdemDisciplinas = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTROLAR_ORDEM_DISCIPLINAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            decimal FrequenciaFinalAjustadaRegencia = BoletimDados.LastOrDefault(p => ((p.tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia ||
                                                                               p.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia) &&
                                                                               (p.FrequenciaFinalAjustada > 0 && p.tpc_id == VS_tpc_id))).FrequenciaFinalAjustada;

            int tpc_ordem = BoletimDados.FirstOrDefault(p => p.tpc_id == VS_tpc_id).tpc_ordem;
            bool ultimoBimestre = BoletimDados.OrderByDescending(i => i.tpc_ordem).FirstOrDefault().tpc_id.Equals(VS_tpc_id);

            var todasDisciplinas = (from BoletimAluno item in BoletimDados
                                    where item.tur_id > 0
                                    orderby item.tud_tipo, item.tud_global descending, item.Disciplina
                                    group item by item.Disciplina
                                        into g
                                        select
                                            new
                                            {
                                                tud_id = g.First().tud_id
                                                ,
                                                Disciplina = g.First().nomeDisciplina
                                                ,
                                                tds_ordem = g.First().tds_ordem
                                                ,
                                                totalFaltas = g.First().tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada || g.Any(p => p.naoExibirFrequencia) ? "-" :
                                                              g.Any(p => p.NotaID > 0) ? (g.Sum(p => (p.mostraFrequencia && !p.naoExibirFrequencia && (p.NotaID > 0 || tipoComponenteRegencia(p.tud_tipo))
                                                                                          && p.tpc_ordem <= tpc_ordem) ? p.numeroFaltas : 0)).ToString() : "-"
                                                ,
                                                ausenciasCompensadas = g.First().tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada || g.Any(p => p.naoExibirFrequencia) ? "-" : 
                                                                       g.Any(p => p.NotaID > 0) ? (g.Sum(p => p.tpc_ordem <= tpc_ordem ? p.ausenciasCompensadas : 0)).ToString() : "-"
                                                ,
                                                FrequenciaFinalAjustada = g.Any(p => p.naoExibirFrequencia) ? "-" :
                                                                          g.Any(p => p.NotaID > 0 && p.tpc_id == VS_tpc_id) ? ((FrequenciaFinalAjustadaRegencia > 0) ? FrequenciaFinalAjustadaRegencia :
                                                                          g.LastOrDefault(p => p.FrequenciaFinalAjustada > 0 && p.tpc_id == VS_tpc_id).FrequenciaFinalAjustada).ToString(VS_FormatacaoPorcentagemFrequencia) : "-"
                                                ,
                                                tud_Tipo = g.First().tud_tipo
                                                ,
                                                g.First().tud_global
                                                ,
                                                mostrarDisciplina = g.Count(p => p.MostrarLinhaDisciplina)
                                                ,
                                                NotaTotal = g.First().NotaTotal
                                                ,
                                                MediaFinal = (g.Any(p => p.naoExibirNota) || !ultimoBimestre) ? "-" : (!string.IsNullOrEmpty(g.Last().NotaResultado) ? g.Last().NotaResultado : "-")
                                                ,
                                                regencia = g.First().tud_tipo == (byte)TurmaDisciplinaTipo.Regencia
                                                             || g.First().tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia
                                                             || (g.First().tud_tipo == (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia
                                                             && controleOrdemDisciplinas) ? 1 : 2
                                                ,
                                                enriquecimentoCurricular = g.First().EnriquecimentoCurricular
                                                ,
                                                parecerFinal = g.First().tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada ? "-" :
                                                               // Se for experiência, vai exibir o menor resultado do aluno (F, NF e nulo nessa ordem)
                                                               g.First().tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.Experiencia ? g.OrderBy(p => p.ParecerFinal ?? "z").FirstOrDefault().ParecerFinal :
                                                               // Caso contrário, mantem o parecer final atual
                                                               g.Last().ParecerFinal
                                                ,
                                                parecerConclusivo = g.Last().ParecerConclusivo
                                                ,
                                                recuperacao = g.First().Recuperacao
                                                ,
                                                disRelacionadas = g.First().disRelacionadas
                                                ,
                                                notas = (
                                                            from per in periodos.ToList()
                                                            orderby per.tpc_ordem
                                                            select new
                                                            {
                                                                per.tpc_id
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
                                                                                                 !bNota.mostraNota || bNota.naoExibirNota || (VS_tpc_id > 0 && bNota.tpc_id > VS_tpc_id)
                                                                                                     ? "-"
                                                                                                     : (bNota.NotaNumerica
                                                                                                            ? bNota.avaliacao ??
                                                                                                              "-"
                                                                                                            : (bNota.
                                                                                                                   NotaAdicionalNumerica
                                                                                                                   ? bNota.
                                                                                                                         avaliacaoAdicional ??
                                                                                                                     "-"
                                                                                                                   : bNota.esa_tipo == (byte)EscalaAvaliacaoTipo.Pareceres
                                                                                                                         ? bNota.avaliacao ?? "-"
                                                                                                                         : "-")
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
                                                                               numeroFaltas = bNota.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada ? "-" :

                                                                                            // Se for "Experiência", faz contagem específica
                                                                                            bNota.tud_tipo == (byte)ACA_CurriculoDisciplinaTipo.Experiencia ?

                                                                                            ((((bNota.cur_id == cur_id && bNota.crr_id == crr_id && bNota.crp_id == crp_id) || lstCurriculoPeriodo.Any(p => p.cur_id == bNota.cur_id && p.crr_id == bNota.crr_id && p.crp_id == bNota.crp_id))
                                                                                                && bNota.mostraFrequencia && !bNota.naoExibirFrequencia && bNota.tpc_id <= VS_tpc_id && (bNota.NotaID > 0 || bNota.numeroFaltas > 0))
                                                                                                    ? (g.Sum(p => (p.tpc_id == bNota.tpc_id) ? p.numeroFaltas : 0)).ToString() : "-")

                                                                                                    :

                                                                                            // Caso contrário, mantem a contagem atual                                                                                                
                                                                                            ((((bNota.cur_id == cur_id && bNota.crr_id == crr_id && bNota.crp_id == crp_id) || lstCurriculoPeriodo.Any(p => p.cur_id == bNota.cur_id && p.crr_id == bNota.crr_id && p.crp_id == bNota.crp_id))
                                                                                                && bNota.mostraFrequencia && !bNota.naoExibirFrequencia && bNota.tpc_id <= VS_tpc_id && (bNota.NotaID > 0 || bNota.numeroFaltas > 0)) ? bNota.numeroFaltas.ToString() : "-")
                                                                               ,
                                                                               tud_Tipo = g.First().tud_tipo
                                                                           }).FirstOrDefault()
                                                            })
                                            }).ToList();

            var disciplinas = (from item in todasDisciplinas
                               where !item.enriquecimentoCurricular //Retira as que são de enriquecimento curricular
                               && !item.recuperacao //Retira as recuperacoes
                               select item
                               );

            // Realiza uma ordenação posterior em que so importa se a disciplina é ou nao regencia/componente da regencia para manter o agrupamento
            var dispOrdenadas = from item in disciplinas
                                orderby item.regencia, controleOrdemDisciplinas ? item.tds_ordem.ToString() : item.Disciplina
                                select item;
            // "Agrupa" a frequência das disciplinas componentes e complementares à regência. 
            VS_Qt_Comp_Reg = dispOrdenadas.Count(p => (tipoComponenteRegencia(p.tud_Tipo)) && p.mostrarDisciplina > 0);

            // "Agrupa" a frequência das disciplinas
            VS_Qt_Comp = dispOrdenadas.Count(p => (p.mostrarDisciplina > 0));

            rptDisciplinas.DataSource = dispOrdenadas.Where(p => p.mostrarDisciplina > 0);
            rptDisciplinas.DataBind();

            #endregion Disciplinas

            #region Disciplinas de enriquecimento curricular

            var disciplinasEnriquecimentoCurricular = (from item in todasDisciplinas
                                                       where item.enriquecimentoCurricular //Verifica se são de enriquecimento curricular
                                                       && !item.recuperacao //Retira as recuperacoes
                                                       select item
                              );

            if (disciplinasEnriquecimentoCurricular.Count() > 0)
            {
                divEnriquecimentoCurricular.Visible = true;
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

            #region Território do saber


            if (!string.IsNullOrEmpty(dadosBoletimAluno.territorioSaber))
            {
                divTerritorioSaber.Visible = true;

                string[] linhasTS = dadosBoletimAluno.linhaTerritorioSaber.Split(';');
                string[] itensTS = dadosBoletimAluno.territorioSaber.Split(';');

                int qtdMaxColunas = 5;// itensTS.Max(i => i.Split(',').Length);//tamanho fixo de 5 colunas
                thTerritorioSaber.ColSpan = qtdMaxColunas;

                Dictionary<string, DataTable> dicLinhasOrdenadas = new Dictionary<string, DataTable>();
                for (int i = 0; i < linhasTS.Length; i++)
                {
                    List<string> itemAdd = itensTS[i].Split(',').ToList();
                    while (itemAdd.Count < qtdMaxColunas)
                        itemAdd.Add("-");
                    DataTable dtAdd = new DataTable();
                    dtAdd.Columns.Add("territorio");
                    foreach (string t in itemAdd)
                    {
                        DataRow rowAdd = dtAdd.NewRow();
                        rowAdd["territorio"] = t;
                        dtAdd.Rows.Add(rowAdd);
                    }
                    dicLinhasOrdenadas.Add(linhasTS[i], dtAdd);
                }

                rptLinhasTerritorio.DataSource = dicLinhasOrdenadas.Select(t => new { linha = t.Key, itemTerritorio = t.Value });
                rptLinhasTerritorio.DataBind();
            }
            else
            {
                divTerritorioSaber.Visible = false;
            }

            #endregion Território do saber

            #region Recuperacao

            var disciplinasRecuperacao = (from item in todasDisciplinas
                                          where item.recuperacao //Seleciona as recuperacoes
                                          select item
                              );

            if (disciplinasRecuperacao.Count() > 0)
            {
                divRecuperacao.Visible = true;
                var dispOrdenadasRecuperacao = from item in disciplinasRecuperacao
                                               orderby item.regencia, controleOrdemDisciplinas ? item.tds_ordem.ToString() : item.Disciplina
                                               select item;

                rptDisciplinasRecuperacao.DataSource = disciplinasRecuperacao.Where(p => p.mostrarDisciplina > 0);
                rptDisciplinasRecuperacao.DataBind();
            }
            else
            {
                divRecuperacao.Visible = false;
            }

            // Exibe a linha do parecer conclusivo caso seja o último bimestre selecionado, e o parecer tenha sido lançado.
            if (ultimoBimestre && !String.IsNullOrEmpty(todasDisciplinas
               .Where(p => p.tud_Tipo != (byte)ACA_CurriculoDisciplinaTipo.DisciplinaEletivaAluno)
               .Last().parecerConclusivo))
            {
                trParecerConclusivo.Visible = true;
                lblParecerConclusivoResultado.Visible = true;

                lblParecerConclusivoResultado.Text = todasDisciplinas
                   .Where(p => p.tud_Tipo != (byte)ACA_CurriculoDisciplinaTipo.DisciplinaEletivaAluno)
                   .Last().parecerConclusivo;
            }
            else
            {
                trParecerConclusivo.Visible = false;
            }

            #endregion Recuperacao

            #region Docencia compartilhada

            if (DocenciasCompartilhadas.Any())
            {
                divDocenciaCompartilhada.Visible = true;
                rptTudDocenciaCompartilhada.DataSource = DocenciasCompartilhadas;
                rptTudDocenciaCompartilhada.DataBind();
            }
            else
                divDocenciaCompartilhada.Visible = false;

            #endregion

            //#region Disciplinas do tipo Eletiva
            //CarregarDisciplinasEletivas(dadosBoletimAluno);
            //#endregion

            #region Linha de Faltas

            //var linhaFaltas = (from per in periodos.ToList()
            //                   orderby per.tpc_ordem
            //                   select new
            //                   {
            //                       tpc_id = per.tpc_id,
            //                       nota = (from f in BoletimDados
            //                               where f.tpc_id == per.tpc_id
            //                               group f by f.tpc_id into g
            //                               select
            //                                   g.Count(p => p.mostraFrequencia && (p.NotaID > 0 || p.tud_tipo == tipoComponenteRegencia)) > 0 ?
            //                                   (g.Sum(p => (p.mostraFrequencia && p.NotaID > 0
            //                                                && p.tud_tipo != tipoComponenteRegencia
            //                                                ) ? p.numeroFaltas : 0)
            //                                               + g.FirstOrDefault
            //                                               (p => p.tud_tipo == tipoComponenteRegencia
            //                                               ).numeroFaltas).ToString() : "-"
            //                       ).FirstOrDefault()
            //                   }
            //                ).ToList();

            //LINHA TOTALIZADORA
            //litLinhaFaltasTotal.Text = linhaFaltas.Count(p => string.IsNullOrEmpty(p.nota)) == linhaFaltas.Count() ?
            //                                "-"
            //                                : linhaFaltas.Where(p => p.nota != "-").Sum(p => Convert.ToInt32(p.nota)).ToString();

            //rptFaltasPeriodo.DataSource = linhaFaltas;
            //rptFaltasPeriodo.DataBind();

            #endregion Linha de Faltas
        }

        /// <summary>
        /// Carrega dados do cabeçalho do boletim.
        /// </summary>
        /// <param name="dadosBoletimAluno">Dados do boletim do aluno.</param>
        private void CarregaCabecalho(ACA_AlunoBO.BoletimDadosAluno dadosBoletimAluno)
        {
            litEscola.Text = dadosBoletimAluno.esc_nome;
            litNome.Text = dadosBoletimAluno.pes_nome;
            litNumero.Text = Convert.ToString(dadosBoletimAluno.mtu_numeroChamada);
            litTurma.Text = dadosBoletimAluno.tur_codigo;
            litAno.Text = Convert.ToString(dadosBoletimAluno.cal_ano);
            litCurso.Text = dadosBoletimAluno.cur_nome;
        }

        ///// <summary>
        ///// Carrega as disciplinas do tipo "Eletiva do aluno" que o aluno cursa
        ///// </summary>
        ///// <param name="dadosBoletimAluno">Dados do boletim do aluno.</param>
        //private void CarregarDisciplinasEletivas(ACA_AlunoBO.BoletimDadosAluno dadosBoletimAluno)
        //{
        //    lblRecuperacao.Visible = !string.IsNullOrEmpty(dadosBoletimAluno.recuperacaoParalela);
        //    lblRecuperacaoParalela.Text = dadosBoletimAluno.recuperacaoParalela;
        //}

        /// <summary>
        /// Carrega o boletim de acordo com o aluno e matricula na turma.
        /// </summary>
        /// <param name="dadosBoletimAluno">Dados do boletim do aluno.</param>
        public void ExibeBoletim(ACA_AlunoBO.BoletimDadosAluno dadosBoletimAluno)
        {
            try
            {
                divImprimirBoletim.Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_IMPRESSAO_BOLETIM_ONLINE, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                CarregaCabecalho(dadosBoletimAluno);
                CarregaBoletim(dadosBoletimAluno);

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "Boletim",
                       "$('.tblBoletim tbody tr:even').addClass('linhaImpar');"
                       + "$('.tblBoletim tbody tr:odd').addClass('linhaPar');"
                       + "RemoveNosTextoVazioTabelasIE9();"
                       , true);
            }
            catch (Exception)
            {
                //ApplicationWEB.GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o boletim.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Métodos

        #region Eventos

        protected void rptDisciplinas_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (ExibeCompensacaoAusencia)
                {
                    var tdTotAusenciasCompensadas = new HtmlTableCell();
                    tdTotAusenciasCompensadas = (HtmlTableCell)e.Item.FindControl("tdTotAusenciasCompensadas");
                    //tdTotAusenciasCompensadas.RowSpan = VS_Qt_Comp_Reg;
                    tdTotAusenciasCompensadas.Visible = true;

                    var tdTotFrequenciaAjustada = new HtmlTableCell();
                    tdTotFrequenciaAjustada = (HtmlTableCell)e.Item.FindControl("tdTotFrequenciaAjustada");
                    //tdTotFrequenciaAjustada.RowSpan = VS_Qt_Comp_Reg;
                    tdTotFrequenciaAjustada.Visible = true;
                }

                if (VS_Qt_Comp_Reg > 0)
                {
                    if (!VS_Controle_Mescla_Regencia && ExibeCompensacaoAusencia)
                    {
                        var tdTotFrequenciaAjustada = new HtmlTableCell();
                        tdTotFrequenciaAjustada = (HtmlTableCell)e.Item.FindControl("tdTotFrequenciaAjustada");
                        tdTotFrequenciaAjustada.RowSpan = VS_Qt_Comp;
                        tdTotFrequenciaAjustada.Visible = true;

                        VS_Controle_Mescla_Regencia = true;
                    }
                    else if (ExibeCompensacaoAusencia)
                    {
                        var tdTotFrequenciaAjustada = new HtmlTableCell();
                        tdTotFrequenciaAjustada = (HtmlTableCell)e.Item.FindControl("tdTotFrequenciaAjustada");
                        tdTotFrequenciaAjustada.Visible = false;
                    }
                }
                else if (ExibeCompensacaoAusencia)
                {
                    var tdTotFrequenciaAjustada = new HtmlTableCell();
                    tdTotFrequenciaAjustada = (HtmlTableCell)e.Item.FindControl("tdTotFrequenciaAjustada");
                    tdTotFrequenciaAjustada.Visible = true;
                }

                byte tipo = (byte)DataBinder.Eval(e.Item.DataItem, "tud_Tipo");
                if (tipoComponenteRegencia(tipo))
                {
                    if (!VS_Controle_Mescla_Disp)
                    {
                        var tdFaltas = new HtmlTableCell();
                        tdFaltas = (HtmlTableCell)e.Item.FindControl("tdTotFaltas");
                        tdFaltas.RowSpan = VS_Qt_Comp_Reg;
                        VS_Controle_Mescla_Disp = true;

                        if (ExibeCompensacaoAusencia)
                        {
                            var tdTotAusenciasCompensadas = new HtmlTableCell();
                            tdTotAusenciasCompensadas = (HtmlTableCell)e.Item.FindControl("tdTotAusenciasCompensadas");
                            tdTotAusenciasCompensadas.RowSpan = VS_Qt_Comp_Reg;
                            tdTotAusenciasCompensadas.Visible = true;
                        }
                    }
                    else
                    {
                        var tdFaltas = new HtmlTableCell();
                        tdFaltas = (HtmlTableCell)e.Item.FindControl("tdTotFaltas");
                        tdFaltas.Visible = false;

                        if (ExibeCompensacaoAusencia)
                        {
                            var tdTotAusenciasCompensadas = new HtmlTableCell();
                            tdTotAusenciasCompensadas = (HtmlTableCell)e.Item.FindControl("tdTotAusenciasCompensadas");
                            tdTotAusenciasCompensadas.Visible = false;
                        }
                    }
                    VS_Controle_Mescla = true;
                }
            }
        }

        protected void rptDisciplinasEnriquecimentoCurricular_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //Mescla as linhas de falta, total de ausencias e total comp. para os componentes da regencia
                byte tud_tipo = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "tud_Tipo"));
                if (tipoComponenteRegencia(tud_tipo))
                {
                    if (!VS_Controle_Mescla_Disp)
                    {
                        var tdFaltas = new HtmlTableCell();
                        tdFaltas = (HtmlTableCell)e.Item.FindControl("tdTotFaltasEnriquec");
                        tdFaltas.RowSpan = VS_Qt_Comp_Reg;
                        tdFaltas.Visible = true;

                        if (ExibeCompensacaoAusencia)
                        {
                            var tdTotAusenciasCompensadas = new HtmlTableCell();
                            tdTotAusenciasCompensadas = (HtmlTableCell)e.Item.FindControl("tdTotAusenciasCompensadasEnriquec");
                            tdTotAusenciasCompensadas.RowSpan = VS_Qt_Comp_Reg;
                            tdTotAusenciasCompensadas.Visible = true;
                        }

                        VS_Controle_Mescla_Disp = true;
                    }
                    else
                    {
                        var tdFaltas = new HtmlTableCell();
                        tdFaltas = (HtmlTableCell)e.Item.FindControl("tdTotFaltasEnriquec");
                        tdFaltas.Visible = false;

                        if (ExibeCompensacaoAusencia)
                        {
                            var tdTotAusenciasCompensadas = new HtmlTableCell();
                            tdTotAusenciasCompensadas = (HtmlTableCell)e.Item.FindControl("tdTotAusenciasCompensadasEnriquec");
                            tdTotAusenciasCompensadas.Visible = false;
                        }
                    }
                    VS_Controle_Mescla = true;
                }
                else if (ExibeCompensacaoAusencia)
                {
                    var tdTotAusenciasCompensadas = new HtmlTableCell();
                    tdTotAusenciasCompensadas = (HtmlTableCell)e.Item.FindControl("tdTotAusenciasCompensadasEnriquec");
                    tdTotAusenciasCompensadas.Visible = true;
                }
                if ((byte)TurmaDisciplinaTipo.DocenciaCompartilhada == tud_tipo && 
                    !string.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "disRelacionadas").ToString()))
                {
                    long tud_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "tud_id"));
                    if (!DocenciasCompartilhadas.Any(d => d.tud_id == tud_id))
                    {
                        Literal litDiscEnrCurricular = (Literal)e.Item.FindControl("litDiscEnrCurricular");

                        DocenciasCompartilhadas.Add(new BoletimAlunoDocCompartilhada
                        {
                            tud_id = tud_id,
                            Disciplina = asteriscoCompartilhada + GetGlobalResourceObject("WebControls", "BoletimCompletoAluno.UCDadosBoletimAluno.MensagemCompartilhada").ToString() +
                                         DataBinder.Eval(e.Item.DataItem, "disRelacionadas") + "."
                        });

                        litDiscEnrCurricular.Text += asteriscoCompartilhada;
                        asteriscoCompartilhada += "*";
                    }
                }
            }
        }

        protected void rptNotasDisciplina_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //var notas = DataBinder.Eval(e.Item.DataItem, "Conceito");
                if (mostraTotalLinha)
                    mostraTotalLinha = (DataBinder.Eval(e.Item.DataItem, "nota") != null
                                        && (DataBinder.Eval(e.Item.DataItem, "nota.Nota").ToString() != "-"
                                            || DataBinder.Eval(e.Item.DataItem, "nota.numeroFaltas").ToString() != "-"));
            }

            string tudTipo = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "nota.tud_Tipo"));
            byte tipo;
            byte.TryParse(tudTipo, out tipo);
            if (tipoComponenteRegencia(tipo))
            {
                if (!VS_Controle_Mescla)
                {
                    var tdFaltas = new HtmlTableCell();
                    tdFaltas = (HtmlTableCell)e.Item.FindControl("tdFaltas");
                    tdFaltas.RowSpan = VS_Qt_Comp_Reg;
                }
                else
                {
                    var tdFaltas = new HtmlTableCell();
                    tdFaltas = (HtmlTableCell)e.Item.FindControl("tdFaltas");
                    tdFaltas.Visible = false;
                }
            }
        }

        protected void rptTudDocenciaCompartilhada_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal litDocenciaCompartilhada = (Literal)e.Item.FindControl("litDocenciaCompartilhada");
                litDocenciaCompartilhada.Text = ((BoletimAlunoDocCompartilhada)e.Item.DataItem).Disciplina;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Se adicionar esses scripts, o botão Imprimir não funciona no Boletim de SP.
            //    ScriptManager sm = ScriptManager.GetCurrent(Page);
            //    if (sm != null)
            //    {
            //        sm.Scripts.Add(new ScriptReference("includes/jquery-1.5.1.min.js"));
            //        sm.Scripts.Add(new ScriptReference("includes/jsUtil.js"));
            //    }
            if (!IsPostBack)
            {
                VS_Controle_Mescla = false;
                VS_Controle_Mescla_Disp = false;
            }
        }

        #endregion Eventos
    }
}
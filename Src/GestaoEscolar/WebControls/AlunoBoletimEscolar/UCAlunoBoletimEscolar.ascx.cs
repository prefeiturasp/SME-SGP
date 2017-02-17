using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.AlunoBoletimEscolar
{
    public partial class UCAlunoBoletimEscolar : MotherUserControl
    {
        #region Propriedades

        /// <summary>
        /// Mostra ou esconde a tela de boletim.
        /// </summary>
        public bool ExibeBoletim
        {
            set
            {
                fdsBoletim.Visible = value;
            }
        }

        #endregion Propriedades

        #region Variáveis

        private List<BoletimAluno> BoletimDados;

        private List<BoletimAlunoAvaliacoesSecretaria> BoletimAvaliacoesDados;

        protected bool mostra5COC;

        #endregion Variáveis

        #region Métodos

        /// <summary>
        /// Retorna a classe de css para a linha da disciplina de acordo com o tud_global (quando é conceito global, a linha tem outro estilo).
        /// </summary>
        /// <param name="tud_global">Indica se é conceito global</param>
        /// <returns></returns>
        protected string classeLinhaDisciplina(bool tud_global)
        {
            return "trDisciplina" + (tud_global ? " global" : "");
        }

        /// <summary>
        /// Carrega o boletim do aluno, todas as notas do ano letivo do aluno (informado na tabela de curriculo).
        /// </summary>
        /// <param name="idAluno">Id do aluno</param>
        /// <param name="dtCurriculo">Tabela com o último currículo do aluno</param>
        public void CarregarBoletim(long idAluno, DataTable dtCurriculo)
        {
            fdsBoletim.Visible = dtCurriculo.Rows.Count > 0 && dtCurriculo.Rows[0]["mtu_id"] != DBNull.Value;
            fsSemBoletim.Visible = !fdsBoletim.Visible;
            lblSemBoletim.Text = UtilBO.GetErroMessage("O aluno não possui avaliação.", UtilBO.TipoMensagem.Informacao);

            if (dtCurriculo.Rows.Count > 0)
            {
                if (dtCurriculo.Rows[0]["mtu_id"] != DBNull.Value)
                {
                    CarregaCabecalho(dtCurriculo);

                    int mtu_idBuscar = Convert.ToInt32(dtCurriculo.Rows[0]["mtu_id"]);

                    CarregaBoletim(idAluno, mtu_idBuscar);
                    CarregaAvaliacoesSecretaria(idAluno, mtu_idBuscar);
                }
            }
        }

        /// <summary>
        /// Carrega repeaters da avaliação da secretaria.
        /// </summary>
        /// <param name="idAluno">ID do aluno</param>
        /// <param name="mtu_idBuscar">Id da matrícula do aluno</param>
        private void CarregaAvaliacoesSecretaria(long idAluno, int mtu_idBuscar)
        {
            BoletimAvaliacoesDados = CLS_AlunoAvaliacaoTurmaBO.RetornaBoletimAlunoAvaliacoesSecretaria(idAluno, mtu_idBuscar);

            divAvaliacoesSecretaria.Visible = BoletimAvaliacoesDados.Count > 0;

            if (BoletimAvaliacoesDados.Count > 0)
            {
                var periodos = from BoletimAlunoAvaliacoesSecretaria item in BoletimAvaliacoesDados
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
                                           };

                var avaliacoes = from BoletimAlunoAvaliacoesSecretaria item in BoletimAvaliacoesDados
                                 group item by item.chaveAvaliacao
                                     into g
                                     select new
                                                {
                                                    chaveAvaliacao = g.Key
                                                    ,
                                                    g.First().avs_nome
                                                    ,
                                                    notas = (
                                                        from per in periodos.ToList()
                                                        orderby per.tpc_ordem
                                                        select new
                                                        {
                                                            per.tpc_id
                                                            ,
                                                            per.tpc_ordem
                                                            ,
                                                            nota = (
                                                                        from BoletimAlunoAvaliacoesSecretaria bNota in BoletimAvaliacoesDados
                                                                        where bNota.chaveAvaliacao == g.Key
                                                                        && bNota.tpc_id == per.tpc_id
                                                                        select new
                                                                        {
                                                                            Nota =
                                                                            (bNota.Nota ?? "").Replace(".", ",")
                                                                        }

                                                                    ).FirstOrDefault()
                                                        }
                                                    )
                                                };

                rptPeridosSecretaria.DataSource = periodos;
                rptPeridosSecretaria.DataBind();

                rptAvaliacoesSecretaria.DataSource = avaliacoes;
                rptAvaliacoesSecretaria.DataBind();
            }
        }

        /// <summary>
        /// Carrega repeaters do boletim do aluno.
        /// </summary>
        /// <param name="idAluno">ID do aluno</param>
        /// <param name="mtu_idBuscar">Id da matrícula do aluno</param>
        private void CarregaBoletim(long idAluno, int mtu_idBuscar)
        {
            BoletimDados = CLS_AlunoAvaliacaoTurmaBO.RetornaBoletimAluno(idAluno, mtu_idBuscar);

            fdsBoletim.Visible = BoletimDados.Count > 0;

            if (fdsBoletim.Visible)
            {
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

                var disciplinas = from BoletimAluno item in BoletimDados
                                  orderby item.tud_global descending
                                  group item by item.Disciplina
                                      into g
                                      select
                                          new
                                              {
                                                  g.First().nomeDisciplina
                                                  ,
                                                  totalFaltas = g.First().dda_id > 0 ? "-" : g.Sum(p => p.numeroFaltas).ToString()
                                                  ,
                                                  g.First().tud_global
                                                  ,
                                                  mostrarDisciplina = g.Count(p => p.MostrarLinhaDisciplina)
                                                  ,
                                                  NotaRecEsp = g.First().dda_id > 0 ? "-" : g.First().NotaRecEsp
                                                  ,
                                                  notas = (
                                                              from per in periodos.ToList()
                                                              orderby per.tpc_ordem
                                                              select new
                                                                         {
                                                                             per.tpc_id
                                                                             ,
                                                                             nota = (
                                                                                        from BoletimAluno bNota in
                                                                                            BoletimDados
                                                                                        where
                                                                                            bNota.Disciplina == g.Key
                                                                                            && bNota.tpc_id == per.tpc_id
                                                                                        select new
                                                                                                   {
                                                                                                       Nota =
                                                                                            (
                                                                                                           //
                                                                                                bNota.dda_id > 0 ? "-"
                                                                                                :
                                                                                                           //
                                                                                                !bNota.mostraNota
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
                                                                                                       numeroFaltas =
                                                                                            (//
                                                                                              bNota.dda_id > 0 ? "-"
                                                                                              :
                                                                                                           //
                                                                                                (bNota.mostraFrequencia && bNota.NotaID > 0)
                                                                                                 ? bNota.numeroFaltas.
                                                                                                       ToString()
                                                                                                 : "-")
                                                                                                   }).FirstOrDefault()
                                                                         })
                                              };

                // Se o último período do boletim possui avaliação de recuperação relacionada, mostra a coluna de 5º COC.
                mostra5COC = (BoletimDados.Count() > 0 ? BoletimDados.Max(p => p.ava_idRecEsp) : 0) > 0;

                coluna5COC.Visible = coluna5COCNota.Visible = mostra5COC;
                // coluna5COCConceito.Visible = coluna5COCNota.Visible = coluna5COCFaltas.Visible = mostra5COC;

                rptPeriodosNomes.DataSource = periodos;
                rptPeriodosColunasFixas.DataSource = periodos;
                rptPeriodosNomes.DataBind();
                rptPeriodosColunasFixas.DataBind();

                rptDisciplinas.DataSource = disciplinas.Where(p => p.mostrarDisciplina > 0);
                rptDisciplinas.DataBind();

                // Buscar última frequência acumulada lançada no conceito global.
                if (BoletimDados.Where(p => p.tud_global && (p.NotaID > 0 || p.frequenciaAcumulada > 0)).Count() > 0)
                {

                    decimal freq = BoletimDados
                                        .Where(p => p.tud_global && (p.NotaID > 0 || p.frequenciaAcumulada > 0))
                                        .OrderBy(p => p.tpc_ordem)
                                        .LastOrDefault()
                                        .frequenciaAcumulada;

                    decimal variacao = BoletimDados.FirstOrDefault().fav_variacao;

                    litFrequenciaAcumulada.Text = freq.ToString(
                        GestaoEscolarUtilBO.CriaFormatacaoDecimal(
                            variacao > 0
                                ? GestaoEscolarUtilBO.RetornaNumeroCasasDecimais(variacao)
                                : 2
                        )
                    );
                }
                else
                {
                    litFreqFrase.Visible = false;
                    litFreqPorc.Visible = false;
                }
            }
        }

        /// <summary>
        /// Carrega dados do cabeçalho do boletim.
        /// </summary>
        /// <param name="dtCurriculo">Tabela utilizada para buscar os dados</param>
        private void CarregaCabecalho(DataTable dtCurriculo)
        {
            litEscola.Text =
                (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                     ? dtCurriculo.Rows[0]["esc_codigo"] + " - "
                     : "")
                + dtCurriculo.Rows[0]["esc_nome"];

            litNome.Text = dtCurriculo.Rows[0]["pes_nome"].ToString();
            litNumero.Text = dtCurriculo.Rows[0]["mtu_numeroChamada"].ToString();
            litCurso.Text = dtCurriculo.Rows[0]["cur_nome"] + " - " + dtCurriculo.Rows[0]["crp_descricao"];
            litTurma.Text = dtCurriculo.Rows[0]["tur_codigo"].ToString();
            litAno.Text = dtCurriculo.Rows[0]["cal_ano"].ToString();
        }

        #endregion Métodos
    }
}
/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using System.Collections.Generic;
    using MSTech.Data.Common;
    using System;
    using System.Linq;
using MSTech.Validation.Exceptions;
	
	/// <summary>
	/// Description: CLS_PlanejamentoOrientacaoCurricular Business Object. 
	/// </summary>
	public class CLS_PlanejamentoOrientacaoCurricularBO : BusinessBase<CLS_PlanejamentoOrientacaoCurricularDAO, CLS_PlanejamentoOrientacaoCurricular>
    {
        #region Métodos de consulta

        /// <summary>
        /// O método seleciona a hierarquia de orientações curriculares na tela de planejamento anual.
        /// </summary>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="crp_id">ID do péríodo.</param>
        /// <param name="crp_idAnterior">ID do período anterior.</param>
        /// <param name="tpc_id">ID do período do calendário.</param>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="cal_id">ID do calendário.</param>
        /// <param name="tdt_posicao">Posição do docente.</param>
        /// <param name="anoAnterior">Flag que indica se a busca será realizada para as orientações curriculares anteriores</param>
        /// <returns></returns>
        public static DataTable SelecionaPorTurmaPeriodoDisciplina
        (
            int cur_id,
            int crr_id,
            int crp_id,
            int crp_idAnterior,
            int tpc_id,
            long tur_id,
            long tud_id,
            int cal_id,
            byte tdt_posicao,
            bool anoAnterior,
            Guid ent_id
        )
        {
            return new CLS_PlanejamentoOrientacaoCurricularDAO().SelecionaPorTurmaPeriodoDisciplina
                (
                    cur_id,
                    crr_id,
                    crp_id,
                    crp_idAnterior,
                    tpc_id,
                    tur_id,
                    tud_id,
                    cal_id,
                    tdt_posicao,
                    anoAnterior,
                    ent_id
                );
        }

        /// <summary>
        /// O método seleciona a hierarquia de orientações curriculares de uma avaliação.
        /// </summary>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="crp_id">ID do péríodo.</param>
        /// <param name="crp_idAnterior">ID do período anterior.</param>
        /// <param name="tpc_id">ID do período do calendário.</param>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="cal_id">ID do calendário.</param>
        /// <param name="tdt_posicao">Posição do docente.</param>
        /// <param name="anoAnterior">Flag que indica se a busca será realizada para as orientações curriculares anteriores</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="tnt_id">ID da turma nota</param>
        /// <returns></returns>
        public static DataTable SelecionaPorTurmaPeriodoDisciplinaAvaliacao
        (
            int cur_id,
            int crr_id,
            int crp_id,
            int crp_idAnterior,
            int tpc_id,
            long tur_id,
            long tud_id,
            int cal_id,
            byte tdt_posicao,
            bool anoAnterior,
            Guid ent_id,
            int tnt_id
        )
        {
            return new CLS_PlanejamentoOrientacaoCurricularDAO().SelecionaPorTurmaPeriodoDisciplinaAvaliacao
                (
                    cur_id,
                    crr_id,
                    crp_id,
                    crp_idAnterior,
                    tpc_id,
                    tur_id,
                    tud_id,
                    cal_id,
                    tdt_posicao,
                    anoAnterior,
                    ent_id,
                    tnt_id
                );
        }

        /// <summary>
        /// Busca o planejamento orientação curricular atravéz da turma 
        /// </summary>
        /// <param name="tur_id"></param>
        /// <returns></returns>
        public static List<CLS_PlanejamentoOrientacaoCurricular> BuscaPlanejamentoOrientacaoCurricular
        (
           long tur_id
        )
        {
            return new CLS_PlanejamentoOrientacaoCurricularDAO().BuscaPlanejamentoOrientacaoCurricular
                (
                    tur_id
                );
        }

        /// <summary>
        /// Busca o planejamento orientação curricular atravéz da turma 
        /// </summary>
        /// <param name="tur_id"></param>
        /// <returns></returns>
        public static DataTable BuscaPlanejamentoOrientacaoCurricularDT
        (
           int tud_id, int tdt_posicao
        )
        {
            return new CLS_PlanejamentoOrientacaoCurricularDAO().BuscaPlanejamentoOrientacaoCurricularDT
                (
                    tud_id, tdt_posicao
                );
        }

        #endregion Métodos de consulta

        #region Métodos para salvar 

        /// <summary>
        /// O método salva os dados do planejamento anual.
        /// </summary>
        /// <param name="ltPlanejamento">Lista de dados do planejamento anual.</param>
        /// <param name="ltHabilidade">Lista de orientações curriculares da turma.</param>
        /// <param name="ltDiagnostico">Lista de orientações anteriores.</param>
        /// <param name="tur_ids">Lista de IDs das turmas para replicar o planejamento.</param>
        /// <param name="replicarPlanejamento">Flag que indica se o planejamento será replicado.</param>
        /// <param name="tur_id">Id da turma do planejamento.</param>
        /// <returns></returns>
        public static bool SalvaPlanejamentoTurmaDisciplina
        (
            List<CLS_TurmaDisciplinaPlanejamento> ltPlanejamento,
            List<CLS_PlanejamentoOrientacaoCurricular> ltHabilidade,
            List<CLS_PlanejamentoOrientacaoCurricularDiagnostico> ltDiagnostico,
            List<long> tur_ids,
            bool replicarPlanejamento,
            long tur_id
        )
        {
            TalkDBTransaction banco = new CLS_PlanejamentoOrientacaoCurricularDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);
            bool retorno = true;

            try
            {
                #region Gerar dados para replicar o planejamento anual

                // Verifica se o planejamento será replicado em outras turmas.
                if (replicarPlanejamento && tur_ids.Any())
                {
                    // Busca a lista de turmas e suas disciplinas
                    string turmas = string.Concat(tur_id, ",", string.Join(",", (from turma in tur_ids select Convert.ToString(turma)).ToArray()));
                    List<TurmaRelTurmaDisciplina> listaTurmaRelTurmaDisciplina = TUR_TurmaDisciplinaBO.SelecionaPorTurmas(turmas, banco);

                    // Lista das turmas em que o planejamento será replicado e suas disciplinas.
                    List<TurmaRelTurmaDisciplina> turmasReplicar = listaTurmaRelTurmaDisciplina.FindAll(p => p.tur_id != tur_id);

                    // Lista das disciplinas e posições de docente que serão salvas no planejamento anual.
                    var tudPosicao = ltPlanejamento.GroupBy(p => new { p.tud_id, p.tdt_posicao }).Select(p => new { p.Key.tud_id, p.Key.tdt_posicao }).Union(
                                     ltDiagnostico.GroupBy(p => new { p.tud_id, p.tdt_posicao }).Select(p => new { p.Key.tud_id, p.Key.tdt_posicao }).Union(
                                     ltHabilidade.GroupBy(p => new { p.tud_id, p.tdt_posicao }).Select(p => new { p.Key.tud_id, p.Key.tdt_posicao }))).Distinct();

                    List<TurmaRelTurmaDisciplina.DadosTurmaDisciplina> tipoDisciplinas = new List<TurmaRelTurmaDisciplina.DadosTurmaDisciplina>();
                    foreach (TurmaRelTurmaDisciplina turRelTud in listaTurmaRelTurmaDisciplina)
                    {
                        tipoDisciplinas.AddRange(turRelTud.ltTurmaDisciplina);
                    }

                    string tud_ids = string.Join(",", (from tud in tipoDisciplinas 
                                                       select Convert.ToString(tud.tud_id)).ToArray());

                    // Busca a lista de docentes dos tud_ids.
                    List<TUR_TurmaDocente> listaTurmaDocente = TUR_TurmaDocenteBO.SelecionaPosicaoPorTudIds(tud_ids);
                    List<TUR_TurmaDocente> listaTurmaDocenteAtivos = listaTurmaDocente.FindAll(p => p.tdt_situacao == (byte)TUR_TurmaDocenteSituacao.Ativo);

                    // Lista de relação disciplina, posição e docente.
                    var docentes = from tud in tudPosicao
                                   let docente = listaTurmaDocente.FindAll(p => p.tud_id == tud.tud_id && p.tdt_posicao == tud.tdt_posicao).OrderBy(q => q.tdt_situacao).ThenByDescending(r => r.tdt_vigenciaInicio).FirstOrDefault()
                                   where docente != null
                                   select new 
                                   {
                                       tud.tud_id
                                       ,
                                       tud.tdt_posicao
                                       ,
                                       docente.doc_id
                                   };

                    
                    // Replicar para cada turma
                    foreach (var turReplicar in turmasReplicar)
                    {
                        /*
                         * Para cada lista que armazena os dados do planejamento anual:
                         * - Buscar os dados referentes o planejamento;
                         * - Buscar turmas disciplinas da turma em que será realizada a replicação com o mesmo tipo de
                         * disciplina da turma disciplina do planejamento original
                         * - Buscar a posição do docente na disciplina de destino ao replicar
                         * - Replicar o planejamento com os dados obtidos
                         * */

                        ltPlanejamento.AddRange(
                            (from planejamento in ltPlanejamento
                             join tds in tipoDisciplinas on planejamento.tud_id equals tds.tud_id
                             join doc in docentes on new { tds.tud_id, planejamento.tdt_posicao } equals new { doc.tud_id, doc.tdt_posicao }
                             join tudReplicar in turReplicar.ltTurmaDisciplina on tds.tds_id equals tudReplicar.tds_id
                             select new CLS_TurmaDisciplinaPlanejamento
                             {
                                 tud_id = tudReplicar.tud_id
                                 ,
                                 tpc_id = planejamento.tpc_id
                                 ,
                                 tdp_diagnostico = planejamento.tdp_diagnostico
                                 ,
                                 tdp_planejamento = planejamento.tdp_planejamento
                                 ,
                                 tdp_avaliacaoTrabalho = planejamento.tdp_avaliacaoTrabalho
                                 ,
                                 tdp_intervencoesPedagogicas = planejamento.tdp_intervencoesPedagogicas
                                 ,
                                 tdp_registroIntervencoes = planejamento.tdp_registroIntervencoes
                                 ,
                                 tdp_recursos = planejamento.tdp_recursos
                                 ,
                                 cur_id = planejamento.cur_id
                                 ,
                                 crr_id = planejamento.crr_id
                                 ,
                                 crp_id = planejamento.crp_id
                                 ,
                                 tdt_posicao = listaTurmaDocenteAtivos.FindAll(p => p.tud_id == tudReplicar.tud_id && p.doc_id == doc.doc_id).FirstOrDefault().tdt_posicao
                                 ,
                                 tdp_situacao = 1
                             }).ToList());

                        ltDiagnostico.AddRange(
                            (from dignostico in ltDiagnostico
                             join tds in tipoDisciplinas on dignostico.tud_id equals tds.tud_id
                             join doc in docentes on new { tds.tud_id, dignostico.tdt_posicao } equals new { doc.tud_id, doc.tdt_posicao }
                             join tudReplicar in turReplicar.ltTurmaDisciplina on tds.tds_id equals tudReplicar.tds_id
                             select new CLS_PlanejamentoOrientacaoCurricularDiagnostico
                             {
                                 tud_id = tudReplicar.tud_id
                                 ,
                                 ocr_id = dignostico.ocr_id
                                 ,
                                 pod_alcancado = dignostico.pod_alcancado
                                 ,
                                 tdt_posicao = listaTurmaDocenteAtivos.FindAll(p => p.tud_id == tudReplicar.tud_id && p.doc_id == doc.doc_id).FirstOrDefault().tdt_posicao
                             }).ToList());

                        ltHabilidade.AddRange(
                            (from habilidade in ltHabilidade
                             join tds in tipoDisciplinas on habilidade.tud_id equals tds.tud_id
                             join doc in docentes on new { tds.tud_id, habilidade.tdt_posicao } equals new { doc.tud_id, doc.tdt_posicao }
                             join tudReplicar in turReplicar.ltTurmaDisciplina on tds.tds_id equals tudReplicar.tds_id
                             select new CLS_PlanejamentoOrientacaoCurricular
                             {
                                 tud_id = tudReplicar.tud_id
                                 ,
                                 ocr_id = habilidade.ocr_id
                                 ,
                                 tpc_id = habilidade.tpc_id
                                 ,
                                 poc_planejado = habilidade.poc_planejado
                                 ,
                                 poc_trabalhado = habilidade.poc_trabalhado
                                 ,
                                 poc_alcancado = habilidade.poc_alcancado
                                 ,
                                 tdt_posicao = listaTurmaDocenteAtivos.FindAll(p => p.tud_id == tudReplicar.tud_id && p.doc_id == doc.doc_id).FirstOrDefault().tdt_posicao
                             }).ToList());
                    }
                }

                #endregion Gerar dados para replicar o planejamento anual

                // Salva os dados.
                retorno = SalvaPlanejamentoTurmaDisciplina(ltPlanejamento, ltHabilidade, ltDiagnostico, banco);

                return retorno;
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                banco.Close();
            }
        }

        /// <summary>
        /// O método salva os dados do planejamento anual.
        /// </summary>
        /// <param name="ltPlanejamento">Lista de dados do planejamento anual.</param>
        /// <param name="ltHabilidade">Lista de orientações curriculares da turma.</param>
        /// <param name="ltDiagnostico">Lista de orientações anteriores.</param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static bool SalvaPlanejamentoTurmaDisciplina
        (
            List<CLS_TurmaDisciplinaPlanejamento> ltPlanejamento,
            List<CLS_PlanejamentoOrientacaoCurricular> ltHabilidade,
            List<CLS_PlanejamentoOrientacaoCurricularDiagnostico> ltDiagnostico,
            TalkDBTransaction banco,
            bool sincronizacaoDiarioClasse = false
        )
        {
            bool retorno = true;

            retorno &= sincronizacaoDiarioClasse ?
                ltPlanejamento.Aggregate(true, (salvou, entity) => salvou & CLS_TurmaDisciplinaPlanejamentoBO.SaveSincronizacaoDiarioClasse(entity, banco)) :
                ltPlanejamento.Aggregate(true, (salvou, entity) => salvou & CLS_TurmaDisciplinaPlanejamentoBO.Save(entity, banco));


            #region Verifica se algum plano de aula nao pode ser desplanejado

            //Seleciona as habilidades que seram salvas como nao planejadas
            List<CLS_PlanejamentoOrientacaoCurricular> ltHabilidadeNaoPlanejadas = ltHabilidade.Where(p => !p.poc_planejado).ToList();

            //Carrega as habilidades que estao ligadas a uma aula
            List<sOrientacoesCurricularesPorDisciplinaBimestreComAulasPlanejadas> listOrientacoesComAula
                = new List<sOrientacoesCurricularesPorDisciplinaBimestreComAulasPlanejadas>();
            
            ltHabilidadeNaoPlanejadas.Select(p => p.tud_id).Distinct().ToList().ForEach(tud_id => {
                listOrientacoesComAula.AddRange(CLS_TurmaAulaOrientacaoCurricularBO.AulasPlanejadasSelecionaPorDisciplina(tud_id));
            });
            
            //Verifica se tem alguma aula nao planejada para aquele bimestre que esteja ligada a uma aula.
            var lAux = (
                    from aula in listOrientacoesComAula
                    join habilidade in ltHabilidadeNaoPlanejadas
                        on new { aula.tud_id, aula.tpc_id, aula.ocr_id } equals new { habilidade.tud_id, habilidade.tpc_id, habilidade.ocr_id }
                    select aula
                ).Distinct().ToList();

            if (lAux.Any())
                throw new ValidationException("Não é possível desplanejar uma habilidade que já tenha sido planejada para uma aula.");

            #endregion

            // Salva os dados na tabela CLS_PlanejamentoOrientacaoCurricular.
            DataTable dtPlanejamentoOrientacaoCurricular = CLS_PlanejamentoOrientacaoCurricular.TipoTabela_PlanejamentoOrientacaoCurricular();
            if (ltHabilidade.Any())
            {
                List<DataRow> ltDrPlanejamentoOrientacaoCurricular = (from CLS_PlanejamentoOrientacaoCurricular planejamentoOrientacaoCurricular in ltHabilidade select PlanejamentoOrientacaoCurricularToDataRow(planejamentoOrientacaoCurricular, dtPlanejamentoOrientacaoCurricular.NewRow())).ToList();

                dtPlanejamentoOrientacaoCurricular = ltDrPlanejamentoOrientacaoCurricular.CopyToDataTable();
            
                retorno &= SalvarEmLote(dtPlanejamentoOrientacaoCurricular, banco);
            }

            // Salva os dados na tabela CLS_PlanejamentoOrientacaoCurricularDiagnostico.
            retorno &= CLS_PlanejamentoOrientacaoCurricularDiagnosticoBO.SalvarEmLote(ltDiagnostico, banco);

            return retorno;
        }

        /// <summary>
        /// Salva os dados do planejamento em lote.
        /// </summary>
        /// <param name="dtPlanejamentoOrientacaoCurricular">DataTable de dados planejamento.</param>
        /// <param name="banco">Transação.</param>
        /// <returns>True em caso de sucesso.</returns>
        public static bool SalvarEmLote(DataTable dtPlanejamentoOrientacaoCurricular, TalkDBTransaction banco = null)
        {
            return banco == null ?
                   new CLS_PlanejamentoOrientacaoCurricularDAO().SalvarEmLote(dtPlanejamentoOrientacaoCurricular) :
                   new CLS_PlanejamentoOrientacaoCurricularDAO { _Banco = banco }.SalvarEmLote(dtPlanejamentoOrientacaoCurricular);
        }

        /// <summary>
        /// O método converte ua registro da CLS_PlanejamentoOrientacaoCurricular em um DataRow.
        /// </summary>
        /// <param name="planejamentoOrientacaoCurricular">Registro da CLS_PlanejamentoOrientacaoCurricular.</param>
        /// <param name="dr">Layout do DataTable.</param>
        /// <returns>DataRow.</returns>
        private static DataRow PlanejamentoOrientacaoCurricularToDataRow(CLS_PlanejamentoOrientacaoCurricular planejamentoOrientacaoCurricular, DataRow dr)
        {
            dr["tud_id"] = planejamentoOrientacaoCurricular.tud_id;
            dr["ocr_id"] = planejamentoOrientacaoCurricular.ocr_id;
            dr["tpc_id"] = planejamentoOrientacaoCurricular.tpc_id;
            dr["poc_planejado"] = planejamentoOrientacaoCurricular.poc_planejado;
            dr["poc_trabalhado"] = planejamentoOrientacaoCurricular.poc_trabalhado;
            dr["poc_alcancado"] = planejamentoOrientacaoCurricular.poc_alcancado;
            dr["tdt_posicao"] = planejamentoOrientacaoCurricular.tdt_posicao;

            return dr;
        }

        #endregion
    }
}
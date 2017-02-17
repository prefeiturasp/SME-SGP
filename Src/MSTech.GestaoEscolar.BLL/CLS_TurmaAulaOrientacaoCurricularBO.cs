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
    using System.Linq;
    using System;
    using MSTech.Validation.Exceptions;

    #region Estruturas

    [Serializable]
    public struct sOrientacoesCurricularesPorDisciplinaBimestreComAulasPlanejadas
    {
        public long tud_id;
        public int tpc_id;
        public long ocr_id;
    }

    #endregion
	/// <summary>
	/// Description: CLS_TurmaAulaOrientacaoCurricular Business Object. 
	/// </summary>
    public class CLS_TurmaAulaOrientacaoCurricularBO : BusinessBase<CLS_TurmaAulaOrientacaoCurricularDAO, CLS_TurmaAulaOrientacaoCurricular>
    {



        /// <summary>
        /// Seleciona as orietancoes curriculares que estejam ligadas a uma aula pelo planejamento
        /// por disciplina
        /// </summary>
        /// <param name="tud_id">Id da disciplina</param>
        /// <returns>Orientacoes curriculares por disciplina e bimestre que tenham aulas planjedas</returns>
        public static List<sOrientacoesCurricularesPorDisciplinaBimestreComAulasPlanejadas> AulasPlanejadasSelecionaPorDisciplina(long tud_id)
        {
            CLS_TurmaAulaOrientacaoCurricularDAO dao = new CLS_TurmaAulaOrientacaoCurricularDAO();
            DataTable dt = dao.AulasPlanejadasSelecionaPorDisciplina(tud_id);
            List<sOrientacoesCurricularesPorDisciplinaBimestreComAulasPlanejadas> lt = new List<sOrientacoesCurricularesPorDisciplinaBimestreComAulasPlanejadas>();

            if(dt.Rows.Count > 0)
                lt = (
                        from DataRow dr in dt.Rows 
                        select new sOrientacoesCurricularesPorDisciplinaBimestreComAulasPlanejadas{
                            tud_id = Convert.ToInt64(dr["tud_id"].ToString()),
                            tpc_id = Convert.ToInt32(dr["tpc_id"].ToString()),
                            ocr_id = Convert.ToInt64(dr["ocr_id"].ToString()),
                        }
                    ).Distinct().ToList();

            return lt;
        }

        /// <summary>
        /// Seleciona a hierarquia de orientações curriculares de acordo com a turma aula.
        /// </summary>
        /// <param name="tud_id">Id da turma disciplina</param>
        /// <param name="tud_id">Id da turma REGENCIA</param>
        /// <param name="tau_id">Id da turma aula</param>
        /// <returns></returns>
        public static DataTable SelecionaPorTurmaPeriodoDisciplina(long tud_id, long tud_idRegencia, int tau_id)
        {
            return new CLS_TurmaAulaOrientacaoCurricularDAO().SelecionaPorTurmaAula(tud_id, tud_idRegencia, tau_id);
        }

        /// <summary>
        /// Retorna uma lista de orientacoes curriculares 
        /// </summary>
        /// <param name="tud_id">Id da turma disciplina</param>
        /// <param name="tud_id">Id da turma REGENCIA</param>
        /// <param name="tau_id">Id da turma aula</param>
        /// <returns></returns>
        public static List<CLS_TurmaAulaOrientacaoCurricular> GetSelectBy_TurmaAula(long tudId, long tudIdRegencia, int tauId)
        {
            return new CLS_TurmaAulaOrientacaoCurricularDAO().GetSelectBy_TurmaAula(tudId, tudIdRegencia, tauId);
        }

        public static bool Salvar(List<CLS_TurmaAulaOrientacaoCurricular> listOriCurTurAula, TalkDBTransaction _Banco)
        {
            long tudId = listOriCurTurAula.First().tud_id;
            long tudIdRegencia = listOriCurTurAula.First().tud_idRegencia;
            int tauId = listOriCurTurAula.First().tau_id;

            listOriCurTurAula.Remove(listOriCurTurAula.Find(p => p.ocr_id == 0));

            //Carrega Recursos gravados no banco
            List<CLS_TurmaAulaOrientacaoCurricular> listaBanco = CLS_TurmaAulaOrientacaoCurricularBO.GetSelectBy_TurmaAula(tudId, tudIdRegencia > 0 ? tudIdRegencia : tudId, tauId);

            //busca registros que devem ser excluidos
            IEnumerable<Int64> dadosTela =
            (from CLS_TurmaAulaOrientacaoCurricular item in listOriCurTurAula.AsEnumerable()
             orderby item.ocr_id descending
             select item.ocr_id).AsEnumerable();

            IEnumerable<Int64> dadosExcluir =
                (from CLS_TurmaAulaOrientacaoCurricular item in listaBanco.AsEnumerable()
                 orderby item.ocr_id descending
                 select item.ocr_id).Except(dadosTela);

            IList<Int64> dadosDif = dadosExcluir.ToList();

            foreach (Int64 ocrId in dadosDif)
            {
                CLS_TurmaAulaOrientacaoCurricularBO.Delete(
                    new CLS_TurmaAulaOrientacaoCurricular { tud_id = tudId, tau_id = tauId, ocr_id = ocrId });
            }

            //busca registro que devem ser alterados
            IEnumerable<Int64> dadosBanco =
                (from CLS_TurmaAulaOrientacaoCurricular item in listaBanco.AsEnumerable()
                 orderby item.ocr_id descending
                 select item.ocr_id).AsEnumerable();

            IEnumerable<Int64> dadosAlterar =
                (from CLS_TurmaAulaOrientacaoCurricular item in listOriCurTurAula.AsEnumerable()
                 orderby item.ocr_id descending
                 select item.ocr_id).Intersect(dadosBanco);

            IList<Int64> dadosAlte = dadosAlterar.ToList();

            //Altera recursos já gravados
            CLS_TurmaAulaOrientacaoCurricular entityAltera;
            foreach (Int64 ocrId in dadosAlte)
            {

                entityAltera = listOriCurTurAula.Find(p => p.ocr_id == ocrId);
                listOriCurTurAula.Remove(entityAltera);
                entityAltera.IsNew = false;
                CLS_TurmaAulaOrientacaoCurricularBO.Save(entityAltera, _Banco);
            }

            // Salva recursos utilizados na aula
            foreach (CLS_TurmaAulaOrientacaoCurricular entity in listOriCurTurAula)
            {
                if (entity.Validate())
                    CLS_TurmaAulaOrientacaoCurricularBO.Save(entity, _Banco);
                else
                    throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
            }

            return true;
        }

        /// <summary>
        /// Retorna um datarow com dados da orientacao curricular do plano de aula
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static DataRow TurmaAulaOrientacaoCurricularToDataRow(CLS_TurmaAulaOrientacaoCurricular entity, DataRow dr)
        {
            if (entity.idAula > 0)
                dr["idAula"] = entity.idAula;
            else
                dr["idAula"] = DBNull.Value;

            dr["tud_id"] = entity.tud_id;
            dr["tau_id"] = entity.tau_id;
            dr["ocr_id"] = entity.ocr_id;
            dr["tao_pranejado"] = entity.tao_pranejado;
            dr["tao_alcancado"] = entity.tao_alcancado;
            dr["tao_trabalhado"] = entity.tao_trabalhado;

            return dr;
        }

    }
}
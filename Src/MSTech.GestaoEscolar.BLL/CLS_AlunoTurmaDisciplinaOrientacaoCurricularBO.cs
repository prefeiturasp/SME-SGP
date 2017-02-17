/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using System;
    using MSTech.Data.Common;
    using MSTech.Validation.Exceptions;
    using System.Collections.Generic;
    using System.Linq;
	
	/// <summary>
	/// Description: CLS_AlunoTurmaDisciplinaOrientacaoCurricular Business Object. 
	/// </summary>
	public class CLS_AlunoTurmaDisciplinaOrientacaoCurricularBO : BusinessBase<CLS_AlunoTurmaDisciplinaOrientacaoCurricularDAO, CLS_AlunoTurmaDisciplinaOrientacaoCurricular>
    {
        #region Consultas

        /// <summary>
        /// Seleciona os alunos matriculados na turma para lançamento de alcance das habilidades na disciplina.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="ocr_id">ID da orientação curricular.</param>
        /// <param name="tpc_id">ID do tipo de período do calendário.</param>
        /// <param name="cal_id">ID do calendário.</param>
        /// <param name="tur_ids">ID das turmas normais de matricula dos alunos matriculados em turmas multisseriadas.</param>
        /// <returns></returns>
        public static DataTable SelecionaAlunosPorTurmaDisciplinaPeriodo
        (
            long tud_id,
            long ocr_id,
            int tpc_id,
            int cal_id,
            string tur_ids = null
        )
        {
            DataTable dtTurma = TUR_Turma.TipoTabela_Turma();

            if (!string.IsNullOrEmpty(tur_ids))
            {
                string[] ltTurIds = tur_ids.Split(';');

                ltTurIds.ToList().ForEach
                     (
                         tur_id =>
                         {
                             DataRow dr = dtTurma.NewRow();
                             dr["tur_id"] = tur_id;
                             dtTurma.Rows.Add(dr);
                         }
                     );
            }

            return new CLS_AlunoTurmaDisciplinaOrientacaoCurricularDAO().SelecionaAlunosPorTurmaDisciplinaPeriodo
                (
                    tud_id,
                    ocr_id,
                    tpc_id,
                    cal_id,
                    dtTurma
                );
        }

        /// <summary>
        /// Seleciona os alunos com lançamento de alcance por turma disciplina.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <returns></returns>
        public static List<CLS_AlunoTurmaDisciplinaOrientacaoCurricular> SelecionaAlunosPorTurmaDisciplina(long tud_id, TalkDBTransaction banco = null)
        {
            CLS_AlunoTurmaDisciplinaOrientacaoCurricularDAO dao = banco == null ?
                new CLS_AlunoTurmaDisciplinaOrientacaoCurricularDAO() :
                new CLS_AlunoTurmaDisciplinaOrientacaoCurricularDAO { _Banco = banco };

            return dao.SelecionaAlunosPorTurmaDisciplina(tud_id);
        }

        #region Diario de Classe

        /// <summary>
        /// Busca o planejamento da turma .
        /// </summary>
        /// <param name="tur_id"></param>
        /// <returns></returns>
        public static DataTable BuscaAlunoOrientacaoCurricular
        (
            string esc_id, Int64 tur_id, DateTime syncDate
        )
        {
            CLS_AlunoTurmaDisciplinaOrientacaoCurricularDAO dao = new CLS_AlunoTurmaDisciplinaOrientacaoCurricularDAO();
            return dao.BuscaAlunoOrientacaoCurricular(esc_id, tur_id, syncDate);
        }

        #endregion

        #endregion

        #region Validações

        /// <summary>
        /// O método verifica se já existe um registro salvo para o aluno para a mesma disciplina, orientação e período.
        /// </summary>
        /// <param name="entity">Entidade CLS_AlunoTurmaDisciplinaOrientacaoCurricular</param>
        /// <param name="banco">Banco em transação (opcional)</param>
        /// <returns>True se existir.</returns>
        public static bool VerificaExistentePorOrientacaoPeriodoDisciplina(CLS_AlunoTurmaDisciplinaOrientacaoCurricular entity, TalkDBTransaction banco = null)
        {
            CLS_AlunoTurmaDisciplinaOrientacaoCurricularDAO dao = banco == null ? new CLS_AlunoTurmaDisciplinaOrientacaoCurricularDAO() :
                                                                                  new CLS_AlunoTurmaDisciplinaOrientacaoCurricularDAO { _Banco = banco };

            return dao.VerificaExistentePorOrientacaoPeriodoDisciplina(entity.tud_id, entity.alu_id, entity.mtu_id, entity.mtd_id, entity.ocr_id, entity.aha_id, entity.tpc_id);
        }

        #endregion

        #region Saves

        /// <summary>
        /// O método salva uma lista de lançamento de alcance de habilidade.
        /// </summary>
        /// <param name="ltEntities">Lista de lançamento de alcance de habilidade</param>
        /// <returns></returns>
        public static bool Salvar(List<CLS_AlunoTurmaDisciplinaOrientacaoCurricular> ltEntities)
        {
            TalkDBTransaction banco = new CLS_AlunoTurmaDisciplinaOrientacaoCurricularDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                return ltEntities.Aggregate(true, (salvou, entity) => salvou & Save(entity, banco));
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                if (banco.ConnectionIsOpen)
                    banco.Close();
            }
        }

        /// <summary>
        /// O método salva uma lista de lançamento de alcance de habilidade.
        /// </summary>
        /// <param name="ltEntities">Lista de lançamento de alcance de habilidade</param>
        /// <returns></returns>
        public static bool Salvar(List<CLS_AlunoTurmaDisciplinaOrientacaoCurricular> ltEntities, TalkDBTransaction banco, bool sincronizacaoDiarioClasse = false)
        {
           return sincronizacaoDiarioClasse ?
               ltEntities.Aggregate(true, (salvou, entity) => salvou & SalvarSincronizacaoDiarioClasse(entity, banco)) :
               ltEntities.Aggregate(true, (salvou, entity) => salvou & Save(entity, banco));
        }

        /// <summary>
        /// O método inclui/altera uma registro na tabela CLS_AlunoTurmaDisciplinaOrientacaoCurricular.
        /// </summary>
        /// <param name="entity">Entidade CLS_AlunoTurmaDisciplinaOrientacaoCurricular</param>
        /// <param name="banco">Transação.</param>
        /// <returns></returns>
        public static new bool Save(CLS_AlunoTurmaDisciplinaOrientacaoCurricular entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                if (VerificaExistentePorOrientacaoPeriodoDisciplina(entity, banco))
                    throw new ValidationException("O lançamento de alcance já existe.");

                return (new CLS_AlunoTurmaDisciplinaOrientacaoCurricularDAO { _Banco = banco }).Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// O método inclui/altera uma registro na tabela CLS_AlunoTurmaDisciplinaOrientacaoCurricular.
        /// </summary>
        /// <param name="entity">Entidade CLS_AlunoTurmaDisciplinaOrientacaoCurricular</param>
        /// <param name="banco">Transação.</param>
        /// <returns></returns>
        public static bool SalvarSincronizacaoDiarioClasse(CLS_AlunoTurmaDisciplinaOrientacaoCurricular entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                if (VerificaExistentePorOrientacaoPeriodoDisciplina(entity, banco))
                    throw new ValidationException("O lançamento de alcance já existe.");

                return (new CLS_AlunoTurmaDisciplinaOrientacaoCurricularDAO { _Banco = banco }).SalvarSincronizacaoDiarioClasse(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        #endregion
    }
}
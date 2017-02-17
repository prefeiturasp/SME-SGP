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
    using System.Linq;
    using MSTech.Data.Common;

    #region Enums

    /// <summary>
    /// Situações do registro
    /// </summary>
    public enum TurmaNotaAlunoOrientacaoCurricularSituacao : short
    {
        Ativo = 1,
        Excluido = 3
    }

    #endregion Enums
	
	/// <summary>
	/// Description: CLS_TurmaNotaAlunoOrientacaoCurricular Business Object. 
	/// </summary>
	public class CLS_TurmaNotaAlunoOrientacaoCurricularBO : BusinessBase<CLS_TurmaNotaAlunoOrientacaoCurricularDAO, CLS_TurmaNotaAlunoOrientacaoCurricular>
	{
        #region Consulta

        /// <summary>
        /// Seleciona as Orientações curriculares ligadas a uma avaliação e aluno
        /// </summary>
        /// <param name="tud_id">ID da Turma Disciplina</param>
        /// <param name="tnt_id">ID da Turma Aula</param>
        /// <param name="alu_id">ID do Aluno</param>
        /// <param name="mtu_id">ID da Matricula do aluno na turma</param>
        /// <param name="mtd_id">ID da Matricula do aluno na disciplina</param>
        /// <returns>Orientações curriculares ligadas a uma avaliação e aluno</returns>
        public static List<CLS_TurmaNotaAlunoOrientacaoCurricular> SelecionaPorAvaliacaoAluno(long tud_id, int tnt_id, long alu_id, int mtu_id, int mtd_id)
        {
            CLS_TurmaNotaAlunoOrientacaoCurricularDAO dao = new CLS_TurmaNotaAlunoOrientacaoCurricularDAO();
            DataTable dt = dao.SelecionaPorAvaliacaoAluno(tud_id, tnt_id, alu_id, mtu_id, mtd_id);

            return dt.Rows.Count > 0
                ? (from DataRow dr in dt.Rows select dao.DataRowToEntity(dr, new CLS_TurmaNotaAlunoOrientacaoCurricular())).ToList()
                : new List<CLS_TurmaNotaAlunoOrientacaoCurricular>();
        }

        /// <summary>
        /// Valida se existe habilidade marcada para algum aluno na avaliação, que não esteja mais selecionada na avaliação.
        /// </summary>
        /// <param name="list">Lista de dados.</param>
        /// <param name="tntIdRelacionada">Id da avaliacao relacionada (de recuperacao).</param>
        /// <param name="banco">Transação.</param>
        /// <returns>True em caso de sucesso.</returns>
        public static bool ValidarHabilidadesAvaliacao(List<CLS_TurmaNotaOrientacaoCurricular> list, TalkDBTransaction banco = null, int tntIdRelacionada = -1)
        {
            if (list.Any())
            {
                List<CLS_TurmaNotaOrientacaoCurricular_SalvarEmLote> listDados =
                    list.Select(p => new CLS_TurmaNotaOrientacaoCurricular_SalvarEmLote
                    {
                        tud_id = p.tud_id,
                        tnt_id = tntIdRelacionada > 0 ? tntIdRelacionada : p.tnt_id,
                        ocr_id = p.ocr_id,
                        toc_id = p.toc_id,
                        toc_alcancada = p.toc_alcancado,
                        toc_situacao = p.toc_situacao
                    }).ToList();

                DataTable dtDados = GestaoEscolarUtilBO.EntityToDataTable<CLS_TurmaNotaOrientacaoCurricular_SalvarEmLote>(listDados);
                return banco == null ?
                       new CLS_TurmaNotaAlunoOrientacaoCurricularDAO().ValidarHabilidadesAvaliacao(dtDados) :
                       new CLS_TurmaNotaAlunoOrientacaoCurricularDAO { _Banco = banco }.ValidarHabilidadesAvaliacao(dtDados);
            }

            return true;
        }

        #endregion Consulta		
	
        #region Saves

        /// <summary>
        /// Salva os dados em lote
        /// </summary>
        /// <param name="list">Lista de dados.</param>
        /// <param name="banco">Transação.</param>
        /// <returns>True em caso de sucesso.</returns>
        public static bool SalvarEmLote(List<CLS_TurmaNotaAlunoOrientacaoCurricular> list, TalkDBTransaction banco = null)
        {
            if (list.Any())
            {
                List<CLS_TurmaNotaAlunoOrientacaoCurricular_SalvarEmLote> listDadosResultado = new List<CLS_TurmaNotaAlunoOrientacaoCurricular_SalvarEmLote>();
                List<CLS_TurmaNotaAlunoOrientacaoCurricular_SalvarEmLote> listDados =
                    list.Select(p => new CLS_TurmaNotaAlunoOrientacaoCurricular_SalvarEmLote
                    {
                        tud_id = p.tud_id,
                        tnt_id = p.tnt_id,
                        alu_id = p.alu_id,
                        mtu_id = p.mtu_id,
                        mtd_id = p.mtd_id,
                        ocr_id = p.ocr_id,
                        aoc_id = p.aoc_id,
                        aoc_alcancada = p.aoc_alcancado,
                        aoc_situacao = p.aoc_situacao
                    }).ToList();

                #region Atualiza os aoc_id

                listDadosResultado.AddRange(listDados.Where(p => p.aoc_id > 0));
                listDados.RemoveAll(p => p.aoc_id > 0);

                var lAux = listDados.Select(p => new { p.tud_id, p.tnt_id, p.alu_id, p.mtu_id, p.mtd_id, p.ocr_id }).Distinct().ToList();
                lAux.ForEach(aux =>
                {
                    List<CLS_TurmaNotaAlunoOrientacaoCurricular_SalvarEmLote> l = new List<CLS_TurmaNotaAlunoOrientacaoCurricular_SalvarEmLote>();
                    l.AddRange(listDados.Where(p => p.tud_id == aux.tud_id 
                                                && p.tnt_id == aux.tnt_id 
                                                && p.alu_id == aux.alu_id
                                                && p.mtu_id == aux.mtu_id
                                                && p.mtd_id == aux.mtd_id
                                                && p.ocr_id == aux.ocr_id));

                    int aoc_idMax = listDadosResultado.Any(p => p.tud_id == aux.tud_id 
                                                            && p.tnt_id == aux.tnt_id
                                                            && p.alu_id == aux.alu_id
                                                            && p.mtu_id == aux.mtu_id
                                                            && p.mtd_id == aux.mtd_id
                                                            && p.ocr_id == aux.ocr_id)
                        ? listDadosResultado.Where(p => p.tud_id == aux.tud_id 
                                                    && p.tnt_id == aux.tnt_id
                                                    && p.alu_id == aux.alu_id
                                                    && p.mtu_id == aux.mtu_id
                                                    && p.mtd_id == aux.mtd_id
                                                    && p.ocr_id == aux.ocr_id).Select(p => p.aoc_id).Max() + 1
                        : 1;

                    l.ForEach(p =>
                    {
                        p.aoc_id = aoc_idMax;
                        aoc_idMax++;
                    });

                    listDadosResultado.AddRange(l);
                });

                #endregion Atualiza os aoc_id

                DataTable dtDados = GestaoEscolarUtilBO.EntityToDataTable<CLS_TurmaNotaAlunoOrientacaoCurricular_SalvarEmLote>(listDadosResultado);

                return banco == null ?
                       new CLS_TurmaNotaAlunoOrientacaoCurricularDAO().SalvarEmLote(dtDados) :
                       new CLS_TurmaNotaAlunoOrientacaoCurricularDAO { _Banco = banco }.SalvarEmLote(dtDados);
            }

            return true;
        }

        #endregion Saves
	}
}
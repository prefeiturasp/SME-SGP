/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using MSTech.Business.Common;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL;
    using MSTech.GestaoEscolar.Entities;

    #region Enums

    /// <summary>
    /// Situações do registro
    /// </summary>
    public enum TurmaNotaOrientacaoCurricularSituacao : short
    {
        Ativo = 1,
        Excluido = 3
    }

    #endregion Enums

    /// <summary>
    /// Description: CLS_TurmaNotaOrientacaoCurricular Business Object.
    /// </summary>
    public class CLS_TurmaNotaOrientacaoCurricularBO : BusinessBase<CLS_TurmaNotaOrientacaoCurricularDAO, CLS_TurmaNotaOrientacaoCurricular>
    {
        #region Consulta

        /// <summary>
        /// Seleciona as Orientações curriculares ligadas a uma Avaliação
        /// </summary>
        /// <param name="tud_id">ID da Turma Disciplina</param>
        /// <param name="tnt_id">ID da Turma Aula</param>
        /// <returns>Orientações curriculares ligadas a uma avaliação</returns>
        public static List<CLS_TurmaNotaOrientacaoCurricular> SelecionaPorAvaliacao(long tud_id, int tnt_id)
        {
            CLS_TurmaNotaOrientacaoCurricularDAO dao = new CLS_TurmaNotaOrientacaoCurricularDAO();
            DataTable dt = dao.SelecionaPorAvaliacao(tud_id, tnt_id);

            return dt.Rows.Count > 0
                ? (from DataRow dr in dt.Rows select dao.DataRowToEntity(dr, new CLS_TurmaNotaOrientacaoCurricular())).ToList()
                : new List<CLS_TurmaNotaOrientacaoCurricular>();
        }

        #endregion Consulta

        #region Saves

        /// <summary>
        /// Salva os dados em lote
        /// </summary>
        /// <param name="list">Lista de dados.</param>
        /// <param name="banco">Transação.</param>
        /// <returns>True em caso de sucesso.</returns>
        public static bool SalvarEmLote(List<CLS_TurmaNotaOrientacaoCurricular> list, TalkDBTransaction banco = null)
        {
            if (list.Any())
            {
                List<CLS_TurmaNotaOrientacaoCurricular_SalvarEmLote> listDadosResultado = new List<CLS_TurmaNotaOrientacaoCurricular_SalvarEmLote>();
                List<CLS_TurmaNotaOrientacaoCurricular_SalvarEmLote> listDados =
                    list.Select(p => new CLS_TurmaNotaOrientacaoCurricular_SalvarEmLote
                    {
                        tud_id = p.tud_id,
                        tnt_id = p.tnt_id,
                        ocr_id = p.ocr_id,
                        toc_id = p.toc_id,
                        toc_alcancada = p.toc_alcancado,
                        toc_situacao = p.toc_situacao
                    }).ToList();

                #region Atualiza os toc_id

                listDadosResultado.AddRange(listDados.Where(p => p.toc_id > 0));
                listDados.RemoveAll(p => p.toc_id > 0);

                var lAux = listDados.Select(p => new { p.tud_id, p.tnt_id, p.ocr_id }).Distinct().ToList();
                lAux.ForEach(aux =>
                {
                    List<CLS_TurmaNotaOrientacaoCurricular_SalvarEmLote> l = new List<CLS_TurmaNotaOrientacaoCurricular_SalvarEmLote>();
                    l.AddRange(listDados.Where(p => p.tud_id == aux.tud_id && p.tnt_id == aux.tnt_id && p.ocr_id == aux.ocr_id));

                    int toc_idMax = listDadosResultado.Any(p => p.tud_id == aux.tud_id && p.tnt_id == aux.tnt_id && p.ocr_id == aux.ocr_id)
                        ? listDadosResultado.Where(p => p.tud_id == aux.tud_id && p.tnt_id == aux.tnt_id && p.ocr_id == aux.ocr_id).Select(p => p.toc_id).Max() + 1
                        : 1;

                    l.ForEach(p =>
                    {
                        p.toc_id = toc_idMax;
                        toc_idMax++;
                    });

                    listDadosResultado.AddRange(l);
                });

                #endregion Atualiza os toc_id

                DataTable dtDados = GestaoEscolarUtilBO.EntityToDataTable<CLS_TurmaNotaOrientacaoCurricular_SalvarEmLote>(listDadosResultado);

                return banco == null ?
                       new CLS_TurmaNotaOrientacaoCurricularDAO().SalvarEmLote(dtDados) :
                       new CLS_TurmaNotaOrientacaoCurricularDAO { _Banco = banco }.SalvarEmLote(dtDados);
            }

            return true;
        }

        #endregion Saves
    }
}
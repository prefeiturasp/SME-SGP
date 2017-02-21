/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// Description: CLS_AlunoFrequenciaExterna Business Object. 
    /// </summary>
    public class CLS_AlunoFrequenciaExternaBO : BusinessBase<CLS_AlunoFrequenciaExternaDAO, CLS_AlunoFrequenciaExterna>
	{
        /// <summary>
        /// Retorna os lançamentos de frequência externa para os alunos da tabela de mts, no tpc informado
        /// </summary>
        /// <param name="listaMtds"></param>
        /// <param name="tpc_id"></param>
        /// <returns></returns>
        public static List<CLS_AlunoFrequenciaExterna> SelecionaPor_MatriculasDisciplinaPeriodo(List<MTR_MatriculaTurmaDisciplina> listaMtds, int tpc_id)
        {
            //CLS_AlunoFrequenciaExternaDAO dao = new CLS_AlunoFrequenciaExternaDAO();
            DataTable dt = MTR_MatriculaTurmaDisciplina.TipoTabela_AlunoMatriculaTurmaDisciplina();
            foreach (MTR_MatriculaTurmaDisciplina item in listaMtds)
            {
                DataRow dr = GestaoEscolarUtilBO.EntityToDataRow(dt, item);
            }

            DataTable dtRet = new CLS_AlunoFrequenciaExternaDAO().SelecionaPor_MatriculasDisciplinaPeriodo(dt, tpc_id);
            
            return GestaoEscolarUtilBO.DataTableToListEntity<CLS_AlunoFrequenciaExterna>(dtRet);
        }
    }
}
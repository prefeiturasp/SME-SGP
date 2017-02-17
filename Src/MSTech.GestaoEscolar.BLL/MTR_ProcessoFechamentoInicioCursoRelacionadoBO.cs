/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System.Data;
using System.Linq;

namespace MSTech.GestaoEscolar.BLL
{
    using System;
    using System.Collections.Generic;
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;

    #region Estruturas

    /// <summary>
    /// Estrutura utilizada para guardar os cursos
    /// relacionados do processo
    /// </summary>
    public struct MTR_ProcessoFechamentoInicioCursosRelacionadosPorProcesso
    {
        /// <summary>
        /// Id do curso relacionado com o processo
        /// </summary>
        public int cur_id;

        /// <summary>
        /// Id do currículo do curso relacionado com o processo
        /// </summary>
        public int crr_id;

        /// <summary>
        /// Id do período do curso relacionado com o processo
        /// </summary>
        public int crp_id;

        /// <summary>
        /// Ordem do período do curso relacionado com o processo
        /// </summary>
        public int crp_ordem;
    }

    #endregion

    /// <summary>
	/// Description: MTR_ProcessoFechamentoInicioCursoRelacionado Business Object. 
	/// </summary>
	public class MTR_ProcessoFechamentoInicioCursoRelacionadoBO : BusinessBase<MTR_ProcessoFechamentoInicioCursoRelacionadoDAO, MTR_ProcessoFechamentoInicioCursoRelacionado>
    {
        #region Estrutura

        /// <summary>
        /// Estrutura para cursos equivalentes no processo usado para salvar CheckboxList.
        /// </summary>
        public struct ProcessoCursoRelacionado
        {
            public int cur_id;
            public int crr_id;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Seleciona os cursos equivalentes de um curso em um processo de fechamento/início.
        /// </summary>
        /// <param name="pfi_id">ID do processo de fechamento/início.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <returns></returns>
        public static List<String> SelecionaCursosRelacionados(int pfi_id, int cur_id, int crr_id)
		{
		    return new MTR_ProcessoFechamentoInicioCursoRelacionadoDAO().SelecionaCursosRelacionados(pfi_id, cur_id, crr_id);
		}
        
        /// <summary>
        /// Seleciona todos os cursos relacionados com o processo e curso informados
        /// </summary>
        /// <param name="pfi_id">Id do processo de fechamento/início do ano letivo</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do currículo do curso</param>        
        public static List<MTR_ProcessoFechamentoInicioCursosRelacionadosPorProcesso> SelecionaCursosRelacionadosPorProcesso(int pfi_id, int cur_id, int crr_id)
        {
            return new MTR_ProcessoFechamentoInicioCursoRelacionadoDAO().SelecionaCursosRelacionadosPorProcesso(pfi_id, cur_id, crr_id).Rows.Cast<DataRow>().Select(dr =>
                    new MTR_ProcessoFechamentoInicioCursosRelacionadosPorProcesso
                    {
                        cur_id = Convert.ToInt32(dr["cur_id"]),
                        crr_id = Convert.ToInt32(dr["crr_id"]),
                        crp_id = Convert.ToInt32(dr["crp_id"]),
                        crp_ordem = Convert.ToInt32(dr["crp_ordem"]),
                    }).ToList();
        }

        /// <summary>
        /// Seleciona todos os cursos relacionados com o processo, curso e ordem informados
        /// </summary>
        /// <param name="pfi_id">Id do processo de fechamento/início do ano letivo</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do currículo do curso</param>
        /// <param name="crp_ordem">Ordem do período do curso</param>
        public static List<MTR_ProcessoFechamentoInicioCursosRelacionadosPorProcesso> SelecionaCursosRelacionadosPorProcessoOrdem(int pfi_id, int cur_id, int crr_id, int crp_ordem)
        {
            return SelecionaCursosRelacionadosPorProcesso(pfi_id, cur_id, crr_id).FindAll(p=>p.crp_ordem == crp_ordem);
        }

        #endregion
    }
}
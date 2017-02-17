/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MSTech.Data.Common;
using MSTech.Validation.Exceptions;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
	/// MTR_ParametroFormacaoTurmaTurno Business Object 
	/// </summary>
	public class MTR_ParametroFormacaoTurmaTurnoBO : BusinessBase<MTR_ParametroFormacaoTurmaTurnoDAO,MTR_ParametroFormacaoTurmaTurno>
	{
        /// <summary>
        /// Seleciona todos os cursos e grupamentos de ensino de
        /// acordo com os registro da tabela MTR_ParametroFormacaoTurma.
        /// </summary>
        /// <param name="pfi_id">Id do processo de fechamento/início.</param>
        /// <param name="pft_id">Id do parâmetro período.</param>
        /// <param name="mostrarportipoturno"></param>
        /// <returns>Lista de estruturas de parâmetro de formação de turmas.</returns>
        public static List<MTR_ParametroFormacaoTurmaTurno> SelecionaPorProcessoParametro(int pfi_id, int pft_id, bool mostrarportipoturno)
        {
            List<MTR_ParametroFormacaoTurmaTurno> listaParametroPeriodoTurno = new List<MTR_ParametroFormacaoTurmaTurno>();

            MTR_ParametroFormacaoTurmaTurnoDAO dao = new MTR_ParametroFormacaoTurmaTurnoDAO();
            DataTable dt = dao.SelectBy_ProcessoParametro(pfi_id, pft_id, mostrarportipoturno);

            foreach (DataRow row in dt.Rows)
            {
                MTR_ParametroFormacaoTurmaTurno entity = new MTR_ParametroFormacaoTurmaTurno();
                entity = dao.DataRowToEntity(row, entity);

                listaParametroPeriodoTurno.Add(entity);
            }

            return listaParametroPeriodoTurno;
        }

        /// <summary>
        /// Seleciona todos os cursos e grupamentos de ensino de
        /// acordo com os registro da tabela MTR_ParametroFormacaoTurma.
        /// </summary>
        /// <param name="pfi_id">Id do processo de fechamento/início.</param>
        /// <param name="pft_id">Id do parâmetro período.</param>
        /// <param name="mostrarportipoturno"></param>
        /// <returns>DataTable com os cursos e grupamentos de ensino.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorProcessoParametroTable(int pfi_id, int pft_id, bool mostrarportipoturno)
        {
            MTR_ParametroFormacaoTurmaTurnoDAO dao = new MTR_ParametroFormacaoTurmaTurnoDAO();
            return dao.SelectBy_ProcessoParametro(pfi_id, pft_id, mostrarportipoturno);
        }		
	}
}
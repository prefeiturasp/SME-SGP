/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using MSTech.Data.Common;
using System.Collections.Generic;
using MSTech.Validation.Exceptions;
using System.Data;

namespace MSTech.GestaoEscolar.BLL
{

    /// <summary>
    /// MTR_ParametroFormacaoTurmaTipoDeficiencia Business Object 
    /// </summary>
    public class MTR_ParametroFormacaoTurmaTipoDeficienciaBO : BusinessBase<MTR_ParametroFormacaoTurmaTipoDeficienciaDAO, MTR_ParametroFormacaoTurmaTipoDeficiencia>
    {
        /// <summary>
        /// Retorna um lista contendo os parâmetros de tipo de deficiência filtrado por pfi_id e pft_id.
        /// </summary>
        /// <param name="pfi_id">Id do processo de fechamento/início.</param>
        /// <param name="pft_id">Id do processo de formação de turmas.</param>
        /// <returns>Lista contendo os parâmetro de tipo de deficiência.</returns>
        public static List<MTR_ParametroFormacaoTurmaTipoDeficiencia> SelecionaPorProcessoParametro(int pfi_id, int pft_id)
        {
            List<MTR_ParametroFormacaoTurmaTipoDeficiencia> listaParametroPeriodoTurno = new List<MTR_ParametroFormacaoTurmaTipoDeficiencia>();

            MTR_ParametroFormacaoTurmaTipoDeficienciaDAO dao = new MTR_ParametroFormacaoTurmaTipoDeficienciaDAO();
            DataTable dt = dao.SelectBy_ProcessoParametro(pfi_id, pft_id);

            foreach (DataRow row in dt.Rows)
            {
                MTR_ParametroFormacaoTurmaTipoDeficiencia entity = new MTR_ParametroFormacaoTurmaTipoDeficiencia();
                entity = dao.DataRowToEntity(row, entity);

                listaParametroPeriodoTurno.Add(entity);
            }

            return listaParametroPeriodoTurno;
        }

    }
}
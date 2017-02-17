/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using MSTech.Data.Common;
using System.Data;
using System.Collections.Generic;
using MSTech.Validation.Exceptions;
using System;

namespace MSTech.GestaoEscolar.BLL
{

    /// <summary>
    /// MTR_ParametroFormacaoTurmaCapacidadeDeficiente Business Object 
    /// </summary>
    public class MTR_ParametroFormacaoTurmaCapacidadeDeficienteBO : BusinessBase<MTR_ParametroFormacaoTurmaCapacidadeDeficienteDAO, MTR_ParametroFormacaoTurmaCapacidadeDeficiente>
    {
        /// <summary>
        /// Retorna uma lista contendo os parâmetros de capacidade por deficiente filtrado por pfi_id e pft_id.
        /// </summary>
        /// <param name="pfi_id">Id do processo de fechamento/início.</param>
        /// <param name="pft_id">Id do processo de formação de turmas.</param>
        /// <returns>Lista contendo os parâmetro de capacidade por deficiente.</returns>
        public static List<MTR_ParametroFormacaoTurmaCapacidadeDeficiente> SelecionaPorProcessoParametro(int pfi_id, int pft_id)
        {
            List<MTR_ParametroFormacaoTurmaCapacidadeDeficiente> listaParametroPeriodoTurno = new List<MTR_ParametroFormacaoTurmaCapacidadeDeficiente>();

            MTR_ParametroFormacaoTurmaCapacidadeDeficienteDAO dao = new MTR_ParametroFormacaoTurmaCapacidadeDeficienteDAO();
            DataTable dt = dao.SelectBy_ProcessoParametro(pfi_id, pft_id);

            foreach (DataRow row in dt.Rows)
            {
                MTR_ParametroFormacaoTurmaCapacidadeDeficiente entity = new MTR_ParametroFormacaoTurmaCapacidadeDeficiente();
                entity = dao.DataRowToEntity(row, entity);

                listaParametroPeriodoTurno.Add(entity);
            }

            return listaParametroPeriodoTurno;
        }
    }

}
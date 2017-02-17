/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using System.Collections.Generic;
using System.Linq;
using MSTech.Data.Common;
using MSTech.Validation.Exceptions;
using MSTech.CoreSSO.BLL;

namespace MSTech.GestaoEscolar.BLL
{	
	/// <summary>
	/// MTR_TipoMomentoAnoCurriculo Business Object 
	/// </summary>
	public class MTR_TipoMomentoAnoCurriculoBO : BusinessBase<MTR_TipoMomentoAnoCurriculoDAO,MTR_TipoMomentoAnoCurriculo>
	{
        /// <summary>
        /// Retorna uma lista de entidades MTR_TipoMomentoAnoCurriculo
        /// </summary>
        /// <param name="mom_ano">Ano - obrigatório</param>
        /// <param name="mom_id">ID - obrigatório</param>
        /// <returns></returns>
        public static List<MTR_TipoMomentoAnoCurriculo> GetSelectByAnoMomentoID(int mom_ano, int mom_id)
        {
            MTR_TipoMomentoAnoCurriculoDAO dao = new MTR_TipoMomentoAnoCurriculoDAO();
            List<MTR_TipoMomentoAnoCurriculo> listaRetorno = new List<MTR_TipoMomentoAnoCurriculo>();
            DataTable dt = dao.SelecionaByAnoMomento(mom_ano, mom_id);

            foreach (DataRow dr in dt.Rows)
            {
                listaRetorno.Add(dao.DataRowToEntity(dr, new MTR_TipoMomentoAnoCurriculo()));
            }

            return listaRetorno;
        }
	}
}
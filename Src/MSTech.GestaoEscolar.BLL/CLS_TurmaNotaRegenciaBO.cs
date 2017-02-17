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
	
	/// <summary>
	/// Description: CLS_TurmaNotaRegencia Business Object. 
	/// </summary>
	public class CLS_TurmaNotaRegenciaBO : BusinessBase<CLS_TurmaNotaRegenciaDAO, CLS_TurmaNotaRegencia>
	{
        /// <summary>
        /// O método converte um registro da CLS_TurmaNotaRegencia em um DataRow.
        /// </summary>
        /// <param name="turmaNotaAluno">Registro da CLS_TurmaNotaRegencia.</param>
        /// <param name="dr">Layout do DataTable.</param>
        /// <returns>DataRow.</returns>
        public static DataRow TurmaNotaRegenciaToDataRow(CLS_TurmaNotaRegencia turmaNotaRegencia, DataRow dr)
        {
            if (turmaNotaRegencia.idAtividade > 0)
                dr["idAtividade"] = turmaNotaRegencia.idAtividade;
            else
                dr["idAtividade"] = DBNull.Value;

            dr["tud_id"] = turmaNotaRegencia.tud_id;
            dr["tnt_id"] = turmaNotaRegencia.tnt_id;
            dr["tud_idAula"] = turmaNotaRegencia.tud_idAula;
            dr["tau_idAula"] = turmaNotaRegencia.tau_idAula;

            return dr;
        }
				
	}
}
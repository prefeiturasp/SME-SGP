/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using MSTech.Data.Common;

namespace MSTech.GestaoEscolar.BLL
{
	
	/// <summary>
	/// TUR_TurmaDisciplinaCalendario Business Object 
	/// </summary>
	public class TUR_TurmaDisciplinaCalendarioBO : BusinessBase<TUR_TurmaDisciplinaCalendarioDAO,TUR_TurmaDisciplinaCalendario>
	{
        /// <summary>
        /// Retorna todos os períodoCalendario cadastrados para o tud_id (TurmaDisciplina).
        /// </summary>
		public static List<TUR_TurmaDisciplinaCalendario> GetSelectBy_TurmaDisciplina(Int64 tud_id)
		{
		    TUR_TurmaDisciplinaCalendarioDAO dao = new TUR_TurmaDisciplinaCalendarioDAO();
		    return dao.SelectBy_TurmaDisciplina(tud_id);
		}

        /// <summary>
        /// Retorna todos os períodos do calendário cadastrados para o tud_id.
        /// </summary>
        /// <param name="tud_id">Id da turma disciplina.</param>
        /// <param name="banco">Transação com o banco.</param>
        public static List<TUR_TurmaDisciplinaCalendario> GetSelectBy_TurmaDisciplina
        (
            Int64 tud_id
            , TalkDBTransaction banco
        )
        {
            TUR_TurmaDisciplinaCalendarioDAO dao = new TUR_TurmaDisciplinaCalendarioDAO 
                { 
                    _Banco = banco 
                };

            return dao.SelectBy_TurmaDisciplina(tud_id);
        }
	}
}
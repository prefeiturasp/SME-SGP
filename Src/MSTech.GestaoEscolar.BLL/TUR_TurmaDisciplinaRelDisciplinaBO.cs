using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.ComponentModel;

namespace MSTech.GestaoEscolar.BLL
{
    public class TUR_TurmaDisciplinaRelDisciplinaBO : BusinessBase<TUR_TurmaDisciplinaRelDisciplinaDAO, TUR_TurmaDisciplinaRelDisciplina>    
    {

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Int32 GetSelectBy_tud_id
        (
           long tud_id
        )
        {
            totalRecords = 0;
            TUR_TurmaDisciplinaRelDisciplinaDAO dao = new TUR_TurmaDisciplinaRelDisciplinaDAO();
            try
            {
                return dao.SelectBy_tud_id(tud_id, out totalRecords);
            }
            catch
            {
                throw;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Int32 GetSelectTdsBy_tud_id
        (
           long tud_id
        )
        {
            totalRecords = 0;
            TUR_TurmaDisciplinaRelDisciplinaDAO dao = new TUR_TurmaDisciplinaRelDisciplinaDAO();
            try
            {
                return dao.SelectTdsBy_tud_id(tud_id, out totalRecords);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Seleciona os registros filtrados pelo tud_id
        /// </summary>
        /// <returns>Lista do tipo TUR_TurmaDisciplinaRelDisciplina</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<TUR_TurmaDisciplinaRelDisciplina> GetSelect
        (
           long tud_id
        )
        {
            totalRecords = 0;
            TUR_TurmaDisciplinaRelDisciplinaDAO dao = new TUR_TurmaDisciplinaRelDisciplinaDAO();            
            return dao.Select_TurmaDisciplinaRelDisciplina(tud_id, out totalRecords);            
        }
    }
}

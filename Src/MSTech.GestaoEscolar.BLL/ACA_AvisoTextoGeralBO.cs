/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System.Data;

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.ComponentModel;
    using System.Data;
    using System;
    using System.Web;
    using System.Linq;

	/// <summary>
	/// Description: ACA_AvisoTextoGeral Business Object. 
	/// </summary>
	public class ACA_AvisoTextoGeralBO : BusinessBase<ACA_AvisoTextoGeralDAO, ACA_AvisoTextoGeral>
    {
        #region Enumerador
        /// <summary>
        /// Enumerador com os tipos de avisos/textos gerais
        /// </summary>
        public enum eTiposAvisosTextosGerais
        {
            Cabecalho = 4
            ,
            Declaracao = 5
            ,
            CabecalhoRelatorio = 6
        }
        #endregion Enumerador		

        /// <summary>
        /// Retorna apenas o unico aviso tipo 4 = (Cabeçalho)
        /// </summary>
        /// <param name="atg_id"></param>
        /// <param name="alunos_ids"></param>
        /// <returns></returns>
        public static int SelecionaPorTipoAviso(int atg_tipo)
        {
            ACA_AvisoTextoGeralDAO dao = new ACA_AvisoTextoGeralDAO();

            DataTable dtAviso = dao.SelecionaPorTipoAviso(atg_tipo);

            int atg_id = 0;

            foreach (DataRow row in dtAviso.Rows)
            {
                foreach (DataColumn column in dtAviso.Columns)
                {
                    atg_id = Convert.ToInt32(row[column]);
                }
            }

            return atg_id;
        }
	}
}

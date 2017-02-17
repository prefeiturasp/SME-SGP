/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using System.ComponentModel;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{	
	/// <summary>
	/// ACA_Religiao Business Object 
	/// </summary>
	public class ACA_ReligiaoBO : BusinessBase<ACA_ReligiaoDAO,ACA_Religiao>
	{
        /// <summary>
        /// Seleciona os dados da religião de acordo com o nome da mesma.
        /// </summary>
        /// <param name="entity">Entidade ACA_Religiao</param>     
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaReligiao
        (
             ACA_Religiao entity
        )
        {
            ACA_ReligiaoDAO dao = new ACA_ReligiaoDAO();
            return dao.SelectBy_Religiao(entity.rlg_id, entity.rlg_nome);
        }
        
        /// <summary>
        /// Retorna todos os tipos de religião não excluídos logicamente
        /// Sem paginação
        /// </summary>        
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaReligiao()
        {
            ACA_ReligiaoDAO dao = new ACA_ReligiaoDAO();
            return dao.SelectBy_Pesquisa(false, 1, 1, out totalRecords);
        }
	}
}
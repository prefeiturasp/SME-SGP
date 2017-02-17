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
	/// MTR_MotivoTransferencia Business Object 
	/// </summary>
	public class MTR_MotivoTransferenciaBO : BusinessBase<MTR_MotivoTransferenciaDAO,MTR_MotivoTransferencia>
	{
        /// <summary>
        /// Retorna todos os motivos de transferência não excluídos logicamente
        /// Sem paginação
        /// </summary>        
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaMotivoTransferencia()
                    
        {
            MTR_MotivoTransferenciaDAO dao = new MTR_MotivoTransferenciaDAO();
            return dao.SelectBy_Pesquisa(false, 1, 1, out totalRecords);
        }
	}
}
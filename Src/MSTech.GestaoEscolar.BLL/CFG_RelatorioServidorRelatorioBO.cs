/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
using System;
    using MSTech.Data.Common;
    using System.Collections.Generic;
	
	/// <summary>
	/// Description: CFG_RelatorioServidorRelatorio Business Object. 
	/// </summary>
	public class CFG_RelatorioServidorRelatorioBO : BusinessBase<CFG_RelatorioServidorRelatorioDAO, CFG_RelatorioServidorRelatorio>
	{		
        /// <summary>
        /// Deleta todos os relatórios relacionados à um determinado servidor.
        /// </summary>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="srr_id">ID Servidor de relatórios</param>
        /// <returns>Booleano</returns>
        public static bool DeletarRelatoriosPorEntidadeServidor(CFG_ServidorRelatorio srr, TalkDBTransaction banco)
        {
            CFG_RelatorioServidorRelatorioDAO dao = new CFG_RelatorioServidorRelatorioDAO() { _Banco = banco };
            return dao.DeleteAll(srr);
        }		
	}
}
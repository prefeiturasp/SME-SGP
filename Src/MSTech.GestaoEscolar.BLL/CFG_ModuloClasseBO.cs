/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
using System.Collections.Generic;

    #region Enumeradores

    /// <summary>
    /// Enumerador da situação do tipo de docente.
    /// </summary>
    public enum EnumModuloClasseSituacao : byte
    {
        Ativo = 1
        ,
        Excluido = 3
    }

    #endregion

    /// <summary>
	/// Description: CFG_ModuloClasse Business Object. 
	/// </summary>
	public class CFG_ModuloClasseBO : BusinessBase<CFG_ModuloClasseDAO, CFG_ModuloClasse>
	{
        /// <summary>
        /// Seleciona todos os registros atvos da tabela
        /// </summary>
        /// <returns></returns>
		public static List<CFG_ModuloClasse> SelecionaAtivos(int sis_id)
        {
            return new CFG_ModuloClasseDAO().SelecionaAtivos(sis_id);
        }
	}
}
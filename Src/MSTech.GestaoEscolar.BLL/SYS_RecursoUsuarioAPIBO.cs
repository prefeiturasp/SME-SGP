/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using System.Linq;    /// <summary>
                          /// Description: SYS_RecursoUsuarioAPI Business Object. 
                          /// </summary>
    public class SYS_RecursoUsuarioAPIBO : BusinessBase<SYS_RecursoUsuarioAPIDAO, SYS_RecursoUsuarioAPI>
	{
        #region Métodos de consulta

        /// <summary>
        /// Seleciona usuários para autenticação em apis
        /// </summary>
        /// <param name="rap_id"></param>
        /// <returns></returns>
        public static List<SYS_UsuarioAPI> SelecionaUsuarioPorRecurso(eRecursoAPI rap_id)
        {
            return GestaoEscolarUtilBO.MapToEnumerable<SYS_UsuarioAPI>(new SYS_RecursoUsuarioAPIDAO().SelecionaUsuarioPorRecurso((int)rap_id)).ToList();
        }

        #endregion Métodos de consulta
    }
}
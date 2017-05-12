/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
	
	/// <summary>
	/// Description: SYS_UsuarioAPI Business Object. 
	/// </summary>
	public class SYS_UsuarioAPIBO : BusinessBase<SYS_UsuarioAPIDAO, SYS_UsuarioAPI>
	{
        #region Métodos de consulta

        /// <summary>
        /// Verifica se existe o usuário por login e senha.
        /// </summary>
        /// <param name="uap_usuario"></param>
        /// <param name="uap_senha"></param>
        /// <returns></returns>
        public static bool AutenticarUsuario(SYS_UsuarioAPI entity)
        {
            return new SYS_UsuarioAPIDAO().AutenticarUsuario(entity.uap_usuario, entity.uap_senha);
        }

        #endregion Métodos de consulta
    }
}
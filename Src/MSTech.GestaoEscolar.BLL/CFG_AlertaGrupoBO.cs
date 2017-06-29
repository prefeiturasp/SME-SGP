/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Data;

    /// <summary>
    /// Description: CFG_AlertaGrupo Business Object. 
    /// </summary>
    public class CFG_AlertaGrupoBO : BusinessBase<CFG_AlertaGrupoDAO, CFG_AlertaGrupo>
	{
        /// <summary>
        /// Carrega os grupos para o alerta
        /// </summary>
        /// <param name="cfa_id">ID do alerta</param>
        /// <returns></returns>
        public static DataTable SelecionarGruposPorAlerta(int cfa_id)
        {
            return new CFG_AlertaGrupoDAO().SelecionarGruposPorAlerta(cfa_id);
        }
    }
}
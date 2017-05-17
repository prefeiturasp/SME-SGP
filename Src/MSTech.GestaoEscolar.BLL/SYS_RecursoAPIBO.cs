/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
	
    public enum eRecursoAPI
    {
        ParecerConclusivoEOL = 1
    }

    public enum RecursoAPISituacao
    {
        Ativo = 1,
        Excluido = 3
    }

    /// <summary>
    /// Description: SYS_RecursoAPI Business Object. 
    /// </summary>
    public class SYS_RecursoAPIBO : BusinessBase<SYS_RecursoAPIDAO, SYS_RecursoAPI>
	{
				
	}
}
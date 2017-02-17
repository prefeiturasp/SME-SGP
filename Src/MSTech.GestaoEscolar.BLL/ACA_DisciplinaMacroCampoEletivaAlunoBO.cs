/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using System;
using System.Data;
using MSTech.Data.Common;

namespace MSTech.GestaoEscolar.BLL
{
       
    /// <summary>
	/// ACA_DisciplinaMacroCampoEletivaAluno Business Object 
	/// </summary>
	public class ACA_DisciplinaMacroCampoEletivaAlunoBO : BusinessBase<ACA_DisciplinaMacroCampoEletivaAlunoDAO,ACA_DisciplinaMacroCampoEletivaAluno>
    {
        public static DataTable SelecionaMacroCampoDisciplina(int dis_id, TalkDBTransaction banco)
        {
            ACA_DisciplinaMacroCampoEletivaAlunoDAO dao = 
                (banco == null 
                    ? new ACA_DisciplinaMacroCampoEletivaAlunoDAO () 
                    : new ACA_DisciplinaMacroCampoEletivaAlunoDAO {_Banco = banco}
                );
            DataTable dt = dao.SelecionaMacroCampoDisciplina(dis_id);
            return dt;
        }		
	}
}
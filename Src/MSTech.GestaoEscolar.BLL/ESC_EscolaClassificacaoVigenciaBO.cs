/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using MSTech.Data.Common;
    using System.Data;
    using System.Collections.Generic;
	
	/// <summary>
	/// Description: ESC_EscolaClassificacaoVigencia Business Object. 
	/// </summary>
    public class ESC_EscolaClassificacaoVigenciaBO : BusinessBase<ESC_EscolaClassificacaoVigenciaDAO, ESC_EscolaClassificacaoVigencia>
    {
        #region Métodos de consulta

        public static DataTable SelecionaAtivas(int esc_id, TalkDBTransaction banco)
        {
            ESC_EscolaClassificacaoVigenciaDAO dao = new ESC_EscolaClassificacaoVigenciaDAO();

            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            return dao.SelecionaAtivas(esc_id);
        }

        #endregion
    }
}
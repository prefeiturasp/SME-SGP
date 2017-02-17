using System;
using System.Data;
using System.ComponentModel;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    public class ESC_TipoRedeEnsinoBO : BusinessBase<ESC_TipoRedeEnsinoDAO, ESC_TipoRedeEnsino>
    {
        /// <summary>
        /// Retorna todos os tipos de rede de ensino não excluídos logicamente
        /// Sem paginação
        /// </summary>        
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoRedeEnsino()
        {
            ESC_TipoRedeEnsinoDAO dao = new ESC_TipoRedeEnsinoDAO();
            return dao.SelectBy_Pesquisa();
        }

    }
}
